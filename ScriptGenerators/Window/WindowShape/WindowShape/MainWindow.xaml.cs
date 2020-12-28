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

namespace WindowShape
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
            if (strWindowTitle.StartsWith("WindowShape"))
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
        DisplayWindowShape:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            cbp1 = new List<ComboBoxPair>();
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblWindowShape";
            myControlEntity1.Text = "Activate Window By Title";
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
            myControlEntity1.Text = "myActions.WindowShape([[Shape]], [[Orientation]], [[Title]], [[Content]], [[Top]], [[Left]]);";
            myControlEntity1.ColumnSpan = 5;
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
            myControlEntity1.ID = "lblShape";
            myControlEntity1.Text = "Shape:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("Box", "Box"));
            cbp.Add(new ComboBoxPair("Arrow", "Arrow"));
            myControlEntity1.ListOfKeyValuePairs = cbp;
            myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorShape");
            if (myControlEntity1.SelectedValue == null)
            {
                myControlEntity1.SelectedValue = "--Select Item ---";
            }
            myControlEntity1.ID = "cbxShape";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblOrientation";
            myControlEntity1.Text = "Orientation:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.ComboBox;
            cbp1.Clear();
            cbp1.Add(new ComboBoxPair("Left", "Left"));
            cbp1.Add(new ComboBoxPair("Right", "Right"));
            cbp1.Add(new ComboBoxPair("Up", "Up"));
            cbp1.Add(new ComboBoxPair("Down", "Down"));

            myControlEntity1.ListOfKeyValuePairs = cbp1;
            myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorOrientation");
            if (myControlEntity1.SelectedValue == null)
            {
                myControlEntity1.SelectedValue = "--Select Item ---";
            }
            myControlEntity1.ID = "cbxOrientation";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblTitle";
            myControlEntity1.Text = "Title:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtTitle";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTitlex");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myControlEntity1.ColumnSpan = 5;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());









            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblContent";
            myControlEntity1.Text = "Content:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtContent";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorContentx");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.Height = 100;
            myControlEntity1.Multiline = true;
            myControlEntity1.ColumnNumber = 1;
            myControlEntity1.ColumnSpan = 5;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblTop";
            myControlEntity1.Text = "Top:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtTop";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTopx");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblLeft";
            myControlEntity1.Text = "Left:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtLeft";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorLeftx");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity1, myListControlEntity1);

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 500, 700, intWindowTop, intWindowLeft);

            string strTitlex = myListControlEntity1.Find(x => x.ID == "txtTitle").Text;
            string strContentx = myListControlEntity1.Find(x => x.ID == "txtContent").Text;
            string strTopx = myListControlEntity1.Find(x => x.ID == "txtTop").Text;
            string strLeftx = myListControlEntity1.Find(x => x.ID == "txtLeft").Text;
            string strShape = myListControlEntity1.Find(x => x.ID == "cbxShape").SelectedValue;
            string strOrientation = myListControlEntity1.Find(x => x.ID == "cbxOrientation").SelectedValue;
            myActions.SetValueByKey("ScriptGeneratorTitlex", strTitlex);
            myActions.SetValueByKey("ScriptGeneratorContentx", strContentx);
            myActions.SetValueByKey("ScriptGeneratorTopx", strTopx);
            myActions.SetValueByKey("ScriptGeneratorLeftx", strLeftx);
            myActions.SetValueByKey("ScriptGeneratorShape", strShape);
            myActions.SetValueByKey("ScriptGeneratorOrientation", strOrientation);

            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity1);

            if (strButtonPressed == "btnOkay")
            {

                string strGeneratedLinex = "";

                strGeneratedLinex = "myActions.WindowShape(\"" + strShape + "\", \"" + strOrientation + "\", \"" + strTitlex + "\", \"" + strContentx + "\"," + strTopx + ", " + strLeftx + ");";

                // ============

                myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);               

                myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write4MainTemplateToExternalFile(strApplicationPath);

                string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
                WriteCodeBodyToExternalFile(strAppendCodeToExistingFile, strOutCodeBodyFile, strGeneratedLinex);

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
        private void WriteCodeBodyToExternalFile(string strAppendCodeToExistingFile, string strOutCodeBodyFile, string strGeneratedLinex)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.WriteLine(strGeneratedLinex);

                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.WriteLine(strGeneratedLinex);

                }
            }
        }

    }
}
