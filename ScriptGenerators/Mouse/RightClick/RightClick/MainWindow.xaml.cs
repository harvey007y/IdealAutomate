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

namespace RightClick
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
            if (strWindowTitle.StartsWith("RightClick"))
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


        DisplayRightClick:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblRightClick";
            myControlEntity1.Text = "RightClick";
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
            myControlEntity1.Text = "myActions.RightClick([[myArray]]);";
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
            myControlEntity1.ID = "lblmyArray";
            myControlEntity1.Text = "myArray:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtmyArray";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRightClickmyArray");
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
            myControlEntity1.Text = "      ImageEntity myImage = new ImageEntity(); \r\n" +
" \r\n" +
"      if (boolRunningFromHome) { \r\n" +
"        myImage.ImageFile = myActions.ConvertWebImageToLocalFile(@\"[[homeimage]]\");  \r\n " +
"      } else { \r\n " +
"        myImage.ImageFile = myActions.ConvertWebImageToLocalFile(@\"[[workimage]]\"); \r\n " +
"      } \r\n" +
"      myImage.Sleep = 200; \r\n" +
"      myImage.Attempts = 5; \r\n" +
"      myImage.RelativeX = 10; \r\n" +
"      myImage.RelativeY = 10; \r\n" +
" \r\n" +
"      int[,] myArray = myActions.PutAll(myImage); \r\n" +
"      if (myArray.Length == 0) { \r\n" +
"        myActions.MessageBoxShow(\"I could not find image of SVN Update\"); \r\n" +
"      } \r\n" +
"      // We found output completed and now want to copy the results \r\n" +
"      // to notepad \r\n" +
" \r\n" +
"      // Highlight the output completed line \r\n" +
"      myActions.Sleep(1000); \r\n" +
"      myActions.RightClick(myArray); ";

            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity1, myListControlEntity1);

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);

            string strmyArray = myListControlEntity1.Find(x => x.ID == "txtmyArray").Text;

            myActions.SetValueByKey("ScriptGeneratorRightClickmyArray", strmyArray);

            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity1);

            if (strButtonPressed == "btnDDLRefresh")
            {
                goto DisplayRightClick;
            }

            if (strButtonPressed == "btnOkay")
            {

                string strmyArrayToUse = "";

                strmyArrayToUse = strmyArray.Trim();

                string strGeneratedLinex = "";

                strGeneratedLinex = "myActions.RightClick(" + strmyArrayToUse + ");";
                // ============

                myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);

                string strOutFile = strApplicationPath + "TemplateCode3GlobalsNew.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile))
                {
                    file.WriteLine("static int[,] " + strmyArrayToUse + " = new int[1,1];");
                }

                myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write4MainTemplateToExternalFile(strApplicationPath);

                string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
                WriteCodeBodyToExternalFile(strAppendCodeToExistingFile, strOutCodeBodyFile, strmyArrayToUse);

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
        private void WriteCodeBodyToExternalFile(string strAppendCodeToExistingFile, string strOutCodeBodyFile, string strmyArrayToUse)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.WriteLine("myActions.RightClick(" + strmyArrayToUse + ");");

                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.WriteLine("myActions.RightClick(" + strmyArrayToUse + ");");

                }
            }
        }

    }
}
