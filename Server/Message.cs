using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class Message
    {
        public Client sender;
        public string Body;
        public string UserId;

        public Message(ILogger log, Client Sender, string Body)
        {
            sender = Sender;
            this.Body = Body;
            UserId = sender?.UserId;
            log.Log(this.Body, UserId);
        }
    }
}
