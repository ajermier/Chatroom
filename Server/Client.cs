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
    public class Client
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        public Client(ILogger log, NetworkStream Stream, TcpClient Client, Dictionary<string, Client> ConnectedClients)
        {
            stream = Stream;
            client = Client;
            UserId = "defaultUser";
        }
        public bool Connection { get { return client.Connected; } }
        public TcpClient TCPConn { get { return client; } set { TCPConn = client; } }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve(ILogger log)
        {
            while (true)
            {
            byte[] recievedMessage = new byte[256];
            try
                {
                    stream.Read(recievedMessage, 0, recievedMessage.Length);
                }
            catch
                {
                if(Connection == false)
                {
                    TCPConn.Close();
                    Server.RemoveUser(log, UserId);
                }   
                    break;
                }
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage).Trim('\0');
            Message message = new Message(log, this, recievedMessageString);
            Server.messageQueue.Enqueue(message);
            }
        }
        public void GetUserName()
        {
            Send("Enter User Name: ");
            byte[] recievedID = new byte[256];
            stream.Read(recievedID, 0, recievedID.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedID).Trim('\0');
            UserId = recievedMessageString;
        }
    }
}
