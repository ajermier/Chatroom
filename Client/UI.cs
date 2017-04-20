using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class UI
    {
        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message.Trim('\0'));
        }
        public static string GetInput()
        {
            return Console.ReadLine();
        }
        public static void DisplayChatRoomTitle()
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine("-----------------------");
            Console.WriteLine("------THE CHATROOM-----");
            Console.WriteLine("-----------------------");
            Console.WriteLine("-----------------------");
        }
    }
}
