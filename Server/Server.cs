using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Server
{
    public class Server
    {
        public static ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        private static Dictionary<string, Client> connectedClients;
        private TcpListener server;
        private ILogger log;
        public Server(ILogger log)
        {
            int port = 9999;
            string ipAddress = "127.0.0.1";
            this.log = log;
            connectedClients = new Dictionary<string, Client>();
            try
            {
                server = new TcpListener(IPAddress.Parse(ipAddress), port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            StartServer(log, ipAddress, port);
            PromptToStopServer(log);
        }
        public void StartServer(ILogger log, string ipAddress, int port)
        {
            Thread respondThread = new Thread(() => Respond(connectedClients));
            respondThread.Start();
            server.Start();
            Message serverStartedMessage = new Message(log, null, $"Listening on {ipAddress} PORT: {port} for clients...");
            Task a = new Task(() => Run(log));
            a.Start();
        }
        public void PromptToStopServer(ILogger log)
        {
            Console.WriteLine("Press ESC key to close.");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Notifying active users server disconnected...");
                Message serverStoppingMessage = new Message(log, null, "Server Disconnected.");
                messageQueue.Enqueue(serverStoppingMessage);
                Thread.Sleep(2000);
                Console.WriteLine("Closing Server...");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }
        public async void Run(ILogger log)
        {
            while (true)
            {
                Console.WriteLine("Wating for a connection...");
                try
                {
                    TcpClient clientSocket = default(TcpClient);
                    clientSocket = await server.AcceptTcpClientAsync();
                    Console.WriteLine("Attempting connection with new client");
                    Thread clientThread = new Thread(() => AcceptClient(log, clientSocket));
                    clientThread.Start();
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException : {0}", e);
                }
            }
        }
		private async void AcceptClient(ILogger log, TcpClient newClient)
		{
            try
            {
                NetworkStream stream = newClient.GetStream();
                Client client = new Client(log, stream, newClient, connectedClients);
                await CheckUserName(log, client);
                Task b = new Task(() => client.Recieve(log));
                b.Start();
            }
            catch
            {
                Console.WriteLine("User disconnected.");
            }
        }
        private async Task CheckUserName(ILogger log, Client client)
        {
            client.GetUserName();
            if (!connectedClients.ContainsKey(client.UserId))
            {
                connectedClients.Add(client.UserId, client);
                ShowOnlineUsers(client);
                Message message = new Message(log, null, $"{client.UserId} is online");
                messageQueue.Enqueue(message);
            }
            else
            {
                client.Send("User name already in use. Try another.\n");
                await CheckUserName(log, client);
            }
            Console.WriteLine($"{connectedClients.Count()} users active.");
        }
        private void Respond(Dictionary<string, Client> ConnectedClients)
        {
            while (true)
            {
                Message message = default(Message);
                if (messageQueue.TryDequeue(out message))
                {
                    foreach(KeyValuePair<string, Client> entry in connectedClients)
                    {
                       if(entry.Key != message.UserId)
                        {
                            entry.Value.Send($"{message.UserId} >> {message.Body}");
                        }
                    }
                }
            }
        }     
        private void ShowOnlineUsers(Client client)
        {
            if(connectedClients.Count() > 1)
            {
                client.Send("Online Users:");
                foreach (KeyValuePair<string, Client> entry in connectedClients)
                {
                    if (entry.Key != client.UserId)
                    {
                        client.Send($" >{entry.Key}\n");
                    }
                }
            }
            else
            {
                client.Send("One is the loneliest number...");
            }
        }
        public static void RemoveUser(ILogger log, string userID)
        {
            connectedClients.Remove(userID);
            Message message = new Message(log, null, $"{userID} has left the chat.");
            messageQueue.Enqueue(message);
        }
    }
}
