#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#endregion

namespace System.Windows.Forms.Samples
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ExplorerView());
        }
    }
}