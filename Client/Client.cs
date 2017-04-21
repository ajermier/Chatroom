using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            ConnectToServer(IP, port);
        }
        public void ConnectToServer(string IP, int port)
        {
            Console.Write("Trying to establish connection with chat server...");
            while (clientSocket.Connected == false)
            {
                try
                {
                    clientSocket.Connect(IPAddress.Parse(IP), port);
                }
                catch
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine("Connected!");
            stream = clientSocket.GetStream();
            Console.WriteLine();
            SendRecieve();
        }
        public void Send()
        {
            while (true)
            {
            string messageString = UI.GetInput();
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            try
            {
                stream.Write(message, 0, message.Length);
            }
            catch
            {
                break;
            }
            }
        }
        public void Recieve()
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
                    break;
                }
                UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
            }

        }
        public void SendRecieve()
        {
            UI.DisplayChatRoomTitle();
            Thread clientRecieve = new Thread(() => Recieve());
            clientRecieve.Start();
            Send();
        }
    }
}
