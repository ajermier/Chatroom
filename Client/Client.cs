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
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
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
