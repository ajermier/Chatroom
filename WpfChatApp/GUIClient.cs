using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfChatApp
{
    class GUIClient
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public GUIClient(string IP, int port)
        {
            clientSocket = new TcpClient();
            ConnectToServer(IP, port);
        }
        public void ConnectToServer(string IP, int port)
        {
            GuiUI.DisplayMessage("Trying to establish connection with chat server...");
            while (clientSocket.Connected == false)
            {
                try
                {
                    clientSocket.Connect(IPAddress.Parse(IP), port);
                }
                catch
                {
                    GuiUI.DisplayMessage(".");
                }
            }
            GuiUI.DisplayMessage("Connected!");
            stream = clientSocket.GetStream();
            GuiUI.DisplayMessage("\n");
            SendRecieve();
        }
        public void Send()
        {
            while (true)
            {
                byte[] message = Encoding.ASCII.GetBytes(GuiUI.GetInput());
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
                GuiUI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
            }

        }
        public void SendRecieve()
        {
            Thread clientRecieve = new Thread(() => Recieve());
            clientRecieve.Start();
            Send();
        }
    }

}

