using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace SortDeleteDupsCaseInsensitive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            bool boolRunningFromHome = false;
            var window = new Window() //make sure the window is invisible
            {
                Width = 0,
                Height = 0,
                Left = -2000,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                ShowActivated = false,
            };
            window.Show();
            IdealAutomate.Core.Methods myActions = new Methods();
            myActions.ScriptStartedUpdateStats();

            InitializeComponent();
            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("SortDeleteDupsCaseInsensitive"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);

            myActions.Run(@"C:\Windows\system32\notepad.exe", @"C:\Data\listsorting.txt");

            myActions.MessageBoxShow("1. Paste the list that you want to sort and remove dups into notepad that opened. \r\n \r\n 2. SAVE IT \r\n \r\n 3. Hit Okay");

            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;

            string[] myArray = System.IO.File.ReadAllLines(@"C:\Data\listsorting.txt");

            List<string> myList = new List<string>();
            foreach (var item in myArray)
            {
                myList.Add(item);
            }
            var distinctsorted = myList.Distinct().OrderBy(q => q).ToArray();
            string strOutFile = @"C:\Data\listsorted.txt";
            System.IO.File.WriteAllLines(strOutFile, distinctsorted);
            //    myActions.RunSync(strApplicationPath + "sortdeletedupscaseinsensitive.bat", @"C:\Data\listsorting.txt");
            string strExecutable = @"C:\Windows\system32\notepad.exe";
            string strContent = strOutFile;
            Process.Start(strExecutable, string.Concat("", strContent, ""));
            goto myExit;

        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
    }
}