using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace MessageBoxShowWithYesNo
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
            if (strWindowTitle.StartsWith("MessageBoxShowWithYesNo"))
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
        DisplayMessageBoxShowWithYesNoWindow:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblMessageBoxShowWithYesNo";
            myControlEntity1.Text = "MessageBoxShowWithYesNo";
            myControlEntity1.Width = 300;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
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
            myControlEntity1.Text = "System.Windows.Forms.DialogResult [[ResultYesNo]] = myActions.MessageBoxShowWithYesNo([[Message]]);";
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
            myControlEntity1.ID = "lblResultYesNo";
            myControlEntity1.Text = "ResultYesNo:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtResultYesNo";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoResultYesNo");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());


            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblEmptyRow1";
            myControlEntity1.Text = "";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblInput";
            myControlEntity1.Text = "Input:";
            myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblMessage";
            myControlEntity1.Text = "Message:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtMessage";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoMessage");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblExample";
            myControlEntity1.Text = "Example:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtExample";
            myControlEntity1.Height = 250;
            myControlEntity1.Text = "        System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo(\"I could not find \" + myImage.ImageFile + \"Do you want me to try again ? \"); \r\n " +
"        if (myResult == System.Windows.Forms.DialogResult.Yes) { \r\n " +
"          goto TryAgain; \r\n " +
"        } else { \r\n " +
"          goto myExit; \r\n " +
"        } ";

            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity1, myListControlEntity1);

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);

            string strMessage = myListControlEntity1.Find(x => x.ID == "txtMessage").Text;
            string strResultYesNo = myListControlEntity1.Find(x => x.ID == "txtResultYesNo").Text;
            // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;

            myActions.SetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoMessage", strMessage);
            myActions.SetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoResultYesNo", strResultYesNo);
            //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity1);

            if (strButtonPressed == "btnOkay")
            {


                string strMessageToUse = "";

                strMessageToUse = "\"" + strMessage.Trim() + "\"";


                string strResultYesNoToUse = "";

                strResultYesNoToUse = strResultYesNo.Trim();

                string strGeneratedLinex = "";

                strGeneratedLinex = strResultYesNoToUse + " = myActions.MessageBoxShowWithYesNo(" + strMessageToUse + ");";

                // ============

                myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);

                string strOutFile = strApplicationPath + "TemplateCode3GlobalsNew.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile))
                {
                    file.WriteLine("static System.Windows.Forms.DialogResult " + strResultYesNoToUse + ";");
                }

                myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write4MainTemplateToExternalFile(strApplicationPath);

                string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
                WriteCodeBodyToExternalFile(strAppendCodeToExistingFile, strOutCodeBodyFile, strMessageToUse, strResultYesNoToUse);

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
        private void WriteCodeBodyToExternalFile(string strAppendCodeToExistingFile, string strOutCodeBodyFile, string strMessageToUse, string strResultYesNoToUse)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.WriteLine(strResultYesNoToUse + " = myActions.MessageBoxShowWithYesNo(" + strMessageToUse + ");");
                    file.WriteLine("        if (" + strResultYesNoToUse + " == System.Windows.Forms.DialogResult.Yes) { ");
                    file.WriteLine("        //  goto TryAgain;");
                    file.WriteLine("        } else {");
                    file.WriteLine("          goto myExit; ");
                    file.WriteLine("        } ");
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.WriteLine(strResultYesNoToUse + " = myActions.MessageBoxShowWithYesNo(" + strMessageToUse + ");");
                    file.WriteLine("        if (" + strResultYesNoToUse + " == System.Windows.Forms.DialogResult.Yes) { ");
                    file.WriteLine("        //  goto TryAgain;");
                    file.WriteLine("        } else {");
                    file.WriteLine("          goto myExit; ");
                    file.WriteLine("        } ");

                }
            }
        }

    }
}
