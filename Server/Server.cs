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
    class Server
    {
        public static ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        public static Client client;
        private static Dictionary<string, Client> connectedClients;
        TcpListener server;
        public Server()
        {
            int port = 9999;
            string ipAddress = "127.0.0.1";
            connectedClients = new Dictionary<string, Client>();
            try
            {
                server = new TcpListener(IPAddress.Parse(ipAddress), port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            try
            {
                server.Start();
                Console.WriteLine($"Listening on {ipAddress} PORT: {port} for clients...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Thread respondThread = new Thread(() => Respond(connectedClients));
            respondThread.Start();
        }
        public async void Run()
        {
            while (true)
            {
                Console.WriteLine("Wating for a connection...");
                try
                {
                    TcpClient clientSocket = default(TcpClient);
                    clientSocket = await server.AcceptTcpClientAsync();
                    Console.WriteLine("Attempting connection with new client");
                    Thread clientThread = new Thread(() => AcceptClient(clientSocket));
                    clientThread.Start();
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException : {0}", e);
                }
            }
        }
		private async void AcceptClient(TcpClient newClient)
		{
            try
            {
                NetworkStream stream = newClient.GetStream();
                Client client = new Client(stream, newClient, connectedClients);
                await CheckUserName(client);
                Task b = new Task(() => client.Recieve());
                b.Start();
            }
            catch
            {
                Console.WriteLine("User disconnected.");
            }
        }
        private async Task CheckUserName(Client client)
        {
            client.GetUserName();
            if (!connectedClients.ContainsKey(client.UserId))
            {
                connectedClients.Add(client.UserId, client);
                Console.WriteLine($"{client.UserId} is online");
                ShowOnlineUsers(client);
                Message message = new Message(null, $"{client.UserId} is online");
                messageQueue.Enqueue(message);
            }
            else
            {
                client.Send("User name already in use. Try another.\n");
                await CheckUserName(client);
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
        public static void RemoveUser(string userID)
        {
            connectedClients.Remove(userID);
            Console.WriteLine($"{userID} disconnected.");
            Message message = new Message(null, $"{userID} has left the chat.");
            messageQueue.Enqueue(message);
        }
    }
}
