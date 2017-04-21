using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChatApp
{
    public static class GuiUI
    {
        public static void DisplayMessage(string message)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).chatBox.Text = message.Trim('\0');
        }
        public static string GetInput()
        {
            return ((MainWindow)System.Windows.Application.Current.MainWindow).messageText.Text.ToString();
        }
    }
}
