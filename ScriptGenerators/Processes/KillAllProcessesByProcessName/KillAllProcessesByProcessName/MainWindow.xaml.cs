using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;

namespace KillAllProcessesByProcessName {
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
      IdealAutomate.Core.Methods myActions = new Methods();
      myActions.ScriptStartedUpdateStats();

      InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("KillAllProcessesByProcessName")) {
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
        DisplayKillAllProcessesByProcessNameWindow:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblKillAllProcessesByProcessName";
            myControlEntity1.Text = "Kill All Processes By Process Name";
            myControlEntity1.Width = 300;
            myControlEntity1.RowNumber = 0;
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
            myControlEntity1.Text = "myActions.KillAllProcessesByProcessName([[Process Name]]);";
            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
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
            myControlEntity1.ID = "lblProcessName";
            myControlEntity1.Text = "Process Name:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtProcessName";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorKillAllProcessesByProcessNameProcessName");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity1, myListControlEntity1);

            string  strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
          
            string strProcessName = myListControlEntity1.Find(x => x.ID == "txtProcessName").Text;
             myActions.SetValueByKey("ScriptGeneratorKillAllProcessesByProcessNameProcessName", strProcessName);
            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity1);



            if (strButtonPressed == "btnOkay")
            {
 
                string strProcessNameToUse = "";
               
                    strProcessNameToUse = "\"" + strProcessName.Trim() + "\"";
               
                string strGeneratedLinex = "";

                strGeneratedLinex = "myActions.KillAllProcessesByProcessName(" + strProcessNameToUse + ");";

                // ============

                myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);

               

                myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write4MainTemplateToExternalFile(strApplicationPath);

                string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
                WriteCodeBodyToExternalFile(strAppendCodeToExistingFile, strOutCodeBodyFile, strProcessNameToUse);

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
        private void WriteCodeBodyToExternalFile(string strAppendCodeToExistingFile, string strOutCodeBodyFile, string strProcessNameToUse)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.WriteLine("myActions.KillAllProcessesByProcessName(" + strProcessNameToUse + ");");

                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.WriteLine("myActions.KillAllProcessesByProcessName(" + strProcessNameToUse + ");");

                }
            }
        }

    }
}
