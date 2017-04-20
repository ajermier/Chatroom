using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class Message
    {
        public Client sender;
        public string Body;
        public string UserId;

        public Message(Client Sender, string Body)
        {
            sender = Sender;
            this.Body = Body;
            UserId = sender?.UserId;
            Logger(this.Body);
        }
        public static void Logger(string item)
        {
            DateTime date = DateTime.Now;
            string itemDate = date.ToString();
            string LogFile = ".\\ChatLog.txt";
            try
            {
                File.AppendAllText(LogFile, $"{itemDate} - {item}" + Environment.NewLine);
            }
            catch
            {
                Console.WriteLine("Problem writing/accessing log file.");
            }
        }
    }
}
