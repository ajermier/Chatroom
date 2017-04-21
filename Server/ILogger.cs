using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Server
{
    interface ILogger
    {
        void Logger(string item, string UserId);
    }
}
