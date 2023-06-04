#region Using directives

using IdealAutomate.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

#endregion

namespace System.Windows.Forms.Samples
{
    static class Program
    {
        public static string MyRoamingFolder;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            CurrentUser objProduct = new CurrentUser();
            objProduct.CurrentUserName = userName;
            objProduct.LaunchStartDateTime = DateTime.Now;
            string json = JsonConvert.SerializeObject(objProduct);
            var baseAddress = "http://idealautomate.com/webapidemo1/api/values/PostUser?CurrentUserName=" + objProduct.CurrentUserName + "&LaunchStartDateTime=" + objProduct.LaunchStartDateTime.ToString() + "";

           // var baseAddress = "https://localhost:44386/api/values/PostUser?CurrentUserName=" + objProduct.CurrentUserName + "&LaunchStartDateTime=" + objProduct.LaunchStartDateTime.ToString() + "";
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            string parsedContent = json;
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);
            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
            var response = http.GetResponse();
            var stream = response.GetResponseStream();
            Methods myActions = new Methods();
            MyRoamingFolder = myActions.GetValueByKeyGlobal("cbxTabsCollectionSelectedValue");
            if (MyRoamingFolder == "")
            {
                myActions.SetValueByKeyGlobal("cbxTabsCollectionSelectedValue", "IdealAutomateExplorer");
                MyRoamingFolder = "IdealAutomateExplorer";
            }
            if (!IsRunAsAdministrator()) {
                
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
                        string settingsDirectory = myActions.GetAppDirectoryForScript();
                        if (!Directory.Exists(settingsDirectory)) {
                            Directory.CreateDirectory(settingsDirectory);
                        }
                        string filePath = Path.Combine(settingsDirectory, "IdealAutomateLog.txt");
                        //System.Web.HttpContext.Current.Server.MapPath("~//Trace.html")
                        StreamWriter sw = null;

                        try {
                            if (File.Exists(filePath)) {
                                // TODO: Wade - uncomment the following - just commented it out for debugging
                                File.Delete(filePath);
                            }
                        } catch (Exception) {


                        }
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