﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            TxtLogger chatLogTxt = new TxtLogger();
            new Server(chatLogTxt);
        }
    }
}
