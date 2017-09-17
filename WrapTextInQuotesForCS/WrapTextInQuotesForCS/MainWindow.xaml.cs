using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace WrapTextInQuotesForCS {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
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
            Methods myActions = new Methods();


            InitializeComponent();
            this.Hide();


            myActions.Sleep(1000);
            string strExecutable = @"C:\Windows\system32\notepad.exe";
            string strContent = @"C:\Data\Text_In.txt";
            Process.Start(strExecutable, string.Concat("", strContent, ""));
            myActions.WindowShape("RedBox", "", "Step 1", " Put your sql in the notepad file that just opened and save it. \nHit Okay button to continue to next step ", 0, 0);
            string strInFile = @"C:\Data\Text_In.txt";
            // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
            string strOutFile = @"C:\Data\Text_Out_Wrapped.txt";
            List<string> listOfSolvedProblems = new List<string>();
            List<string> listofRecs = new List<string>();
            string[] lineszz = System.IO.File.ReadAllLines(strInFile);


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {
                int intLineCount = lineszz.Count();
                int intCtr = 0;
                char tab = '\u0009';
                foreach (string line in lineszz) {
                    intCtr++;
                    string linex = line.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r\n", "").Replace("\t", "");
                    linex = Regex.Replace(linex, @"[^\u0000-\u007F]+", string.Empty);
                    linex = linex.Replace("\" +", "\\r\\n\" +");
                    if (intCtr < intLineCount) {
                        file.WriteLine("\"" + linex + " \\r\\n\" +");
                    } else {
                        file.WriteLine("\"" + linex + " \\r\\n\";");
                    }
                }

            }
            strExecutable = @"C:\Windows\system32\notepad.exe";
            strContent = strOutFile;
            Process.Start(strExecutable, string.Concat("", strContent, ""));

            goto myExit;

            myExit:

            Application.Current.Shutdown();
        }
    }
}