#region Using directives

using IdealAutomate.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
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
            if (!IsRunAsAdministrator()) {
                Methods myActions = new Methods();
                string launchMode = myActions.GetValueByKey("LaunchMode");
                if (launchMode == "Admin") {
                    var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                    // The following properties run the new process as administrator
                    processInfo.UseShellExecute = true;
                    processInfo.Verb = "runas";

                    // Start the new process
                    try {
                        Process.Start(processInfo);
                    } catch (Exception) {
                        // The user did not allow the application to run as administrator
                        MessageBox.Show("Sorry, this application must be run as Administrator.");
                    }
                } else {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    try { 
                    Application.Run(new ExplorerView());
                    } catch (Exception ex) {

                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                        MessageBox.Show(ex.InnerException.ToString());

                    }
                }

                // Shut down the current process
                
                
            } else {



                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try {
                    Application.Run(new ExplorerView());
                } catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                    MessageBox.Show(ex.InnerException.ToString());

                }
            }
        }
        private static bool IsRunAsAdministrator() {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}