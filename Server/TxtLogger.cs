using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Server
{
    public class TxtLogger : ILogger
    {
        public void Log(string item, string UserId)
        {
            DateTime date = DateTime.Now;
            string itemDate = date.ToString();
            string LogFile = ".\\ChatLog.txt";
            try
            {
                File.AppendAllText(LogFile, $"{itemDate} {UserId} >> {item}" + Environment.NewLine);
                Console.WriteLine($"{UserId} >> {item}");
            }
            catch
            {
                Console.WriteLine("Problem writing/accessing log file.");
            }
        }
    }
}
