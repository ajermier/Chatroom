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

        public Message(Logger log, Client Sender, string Body)
        {
            sender = Sender;
            this.Body = Body;
            UserId = sender?.UserId;
            log.Log(this.Body, UserId);
        }
    }
}
