using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;



namespace Hardcodet.Wpf.Samples {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string lastMenuItem;
        public static Stopwatch myStopwatch = Stopwatch.StartNew();
        public MainWindow()
        {
            
            InitializeComponent();
            //this.Closed += (s, e) => Application.Current.Shutdown();
            //string directory = AppDomain.CurrentDomain.BaseDirectory;
            //DirectoryInfo dir = new DirectoryInfo(directory);
            //FileInfo[] files = dir.GetFiles("*.bmp");
            //foreach (FileInfo file in files)
            //{
            //    if (!IsFileLocked(file)) file.Delete();
            //}
            Switcher.pageSwitcher = this;
            string[] args = null;
            string strScheduledScriptName = "";
            //if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            //{
            //    var inputArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
            //    if (inputArgs != null && inputArgs.Length > 0)
            //    {
            //        args = inputArgs[0].Split(new char[] { ',' });
            //        //MessageBox.Show(args[0]);
            //        strScheduledScriptName = args[0];
            //    }
            //}
           // Switcher.Switch(new LocalScripts(strScheduledScriptName));
        }

        public static Boolean IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                //Don't change FileAccess to ReadWrite, 
                //because if a file is in readOnly, it fails.
                stream = file.Open
                (
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                );
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }

       
    }
}
