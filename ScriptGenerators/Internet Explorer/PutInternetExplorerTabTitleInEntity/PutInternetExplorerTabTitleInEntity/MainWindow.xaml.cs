using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;

namespace PutInternetExplorerTabTitleInEntity
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
            if (strWindowTitle.StartsWith("PutInternetExplorerTabTitleInEntity"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            int intWindowTop = 0;
            int intWindowLeft = 0;
            int intRowCtr = 0;
            ControlEntity myControlEntity1 = new ControlEntity();
            List<ControlEntity> myListControlEntity1 = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();

            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;

            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window

            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Script Generator";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            // get project folder
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            int intLastSlashIndex = directory.LastIndexOf("\\");
        DisplayPutInternetExplorerTabTitleInEntity:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp1 = new List<ComboBoxPair>();

            intRowCtr = 0;
            myListControlEntity1 = new List<ControlEntity>();
            myControlEntity = new ControlEntity();
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.Text = "Get Active Window Title";
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblSyntax";
            myControlEntity1.Text = "Syntax:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtSyntax2";
            myControlEntity1.Text = "string [[ResultValue]] = myActions.PutInternetExplorerTabTitleInEntity();";
            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblOutput";
            myControlEntity1.Text = "Output:";
            myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblResultValue";
            myControlEntity1.Text = "ResultValue:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtResultValue";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabTitleInEntityResultValue");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity1, myListControlEntity1);

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);

            string strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
            // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;

            myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabTitleInEntityResultValue", strResultValue);
            //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity1);
            if (strButtonPressed == "btnOkay")
            {

                string strResultValueToUse = "";

                strResultValueToUse = strResultValue.Trim();

                string strGeneratedLinex = "";               

                strGeneratedLinex = strResultValueToUse + " = myActions.PutInternetExplorerTabTitleInEntity();";

                // ============

                myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);

                string strOutFile = strApplicationPath + "TemplateCode3GlobalsNew.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile))
                {
                    file.WriteLine("static string " + strResultValueToUse + " = \"\";");
                }

                myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write4MainTemplateToExternalFile(strApplicationPath);

                string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
                WriteCodeBodyToExternalFile(strAppendCodeToExistingFile, strOutCodeBodyFile, strResultValueToUse);

                myActions.Write5FunctionsTemplateToExternalFile(strApplicationPath);

                myActions.WriteCodeEndToExternalFile();

                string strOutCodeBigFile = myActions.WriteCodeBigExternalFile(strOutCodeBodyFile);

                string strExecutable = @"C:\Windows\system32\notepad.exe";
                string strContent = strOutCodeBigFile;
                Process.Start(strExecutable, string.Concat("", strContent, ""));

                //============

                myActions.PutEntityInClipboard(strGeneratedLinex);

                myActions.MessageBoxShow(strGeneratedLinex + Environment.NewLine + Environment.NewLine + "The generated text has been put into your clipboard");
            }
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
        private void WriteCodeBodyToExternalFile(string strAppendCodeToExistingFile, string strOutCodeBodyFile, string strResultValueToUse)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.WriteLine(strResultValueToUse + " = myActions.PutInternetExplorerTabTitleInEntity();");

                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.WriteLine(strResultValueToUse + " = myActions.PutInternetExplorerTabTitleInEntity();");

                }
            }
        }

    }
}
