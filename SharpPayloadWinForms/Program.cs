﻿using System;
using System.Windows.Forms;

namespace SharpPayloadWinForms
{
    internal class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        ///     Unlike C++, C# does not have the ability to specify an EntryPoint. Therefore SharpDomain looks for a [STAThread]
        ///     Attribute and uses that as an EntryPoint
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}