﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanilaChat
{
    public static class Program
    {
        public static Form1 MainChatWindow { get; set; }
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainChatWindow = new Form1();

            Application.Run(MainChatWindow);
        }
    }
}
