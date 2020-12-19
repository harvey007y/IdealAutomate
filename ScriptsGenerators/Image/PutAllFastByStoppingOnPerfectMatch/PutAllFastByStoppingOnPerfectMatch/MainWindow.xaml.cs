using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections;

namespace PutAllFastByStoppingOnPerfectMatch
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
            if (strWindowTitle.StartsWith("PutAllFastByStoppingOnPerfectMatch"))
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
        DisplayPutAllFastByStoppingOnPerfectMatch:
            intRowCtr = 0;
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblPutAllFastByStoppingOnPerfectMatch";
            myControlEntity1.Text = "PutAllFastByStoppingOnPerfectMatch";
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
            myControlEntity1.Text = "      myImage = new ImageEntity();\r\n " +
" \r\n " +
"      if (boolRunningFromHome) { \r\n " +
"        myImage.ImageFile = \"Images\\\\\" + \"[[homeimage]]\";  \r\n " +
"      } else { \r\n " +
"        myImage.ImageFile = \"Images\\\\\" + \"[[workimage]]\"; \r\n " +
"      } \r\n " +
"      myImage.Sleep = [[Sleep]];  \r\n " +
"      myImage.Attempts = [[Attempts]];  \r\n " +
"      myImage.RelativeX = [[RelativeX]];  \r\n " +
"      myImage.RelativeY = [[RelativeY]]; \r\n " +
" \r\n " +
"      int[,] [[ResultMyArray]] = myActions.PutAllFastByStoppingOnPerfectMatch(myImage); \r\n" +
"      if (myArray.Length == 0) { \r\n" +
"        myActions.MessageBoxShow(\"I could not find image of SVN Update\"); \r\n" +
"      } \r\n" +
"      // We found output completed and now want to copy the results \r\n" +
"      // to notepad \r\n" +
" \r\n" +
"      // Highlight the output completed line \r\n" +
"      myActions.Sleep(1000); \r\n" +
"      myActions.LeftClick(myArray); ";
            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.Height = 225;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblHomeImage";
            myControlEntity1.Text = "HomeImage";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtHomeImage";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorHomeImage");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblWorkImage";
            myControlEntity1.Text = "WorkImage";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtWorkImage";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorWorkImage");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblSleep";
            myControlEntity1.Text = "Sleep";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtSleep";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSleep");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblAttempts";
            myControlEntity1.Text = "Attempts";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtAttempts";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorAttempts");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblRelativeX";
            myControlEntity1.Text = "RelativeX";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtRelativeX";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeX");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblRelativeY";
            myControlEntity1.Text = "RelativeY";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtRelativeY";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeY");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblEmptyRow7";
            myControlEntity1.Text = "";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblOccurrence";
            myControlEntity1.Text = "Occurrence";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtOccurrence";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorOccurrence");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblTolerance";
            myControlEntity1.Text = "Tolerance";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtTolerance";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTolerance");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblUseGrayScale";
            myControlEntity1.Text = "UseGrayScale";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("True", "True"));
            cbp.Add(new ComboBoxPair("False", "False"));
            myControlEntity1.ListOfKeyValuePairs = cbp;
            myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorUseGrayScale");
            myControlEntity1.ID = "cbxUseGrayScale";
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
            myControlEntity1.ID = "lblValue";
            myControlEntity1.Text = "ResultMyArray:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtResultMyArray";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutAllFastByStoppingOnPerfectMatchResultMyArray");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 700, intWindowTop, intWindowLeft);
            string strHomeImage = myListControlEntity1.Find(x => x.ID == "txtHomeImage").Text;
            string strWorkImage = myListControlEntity1.Find(x => x.ID == "txtWorkImage").Text;
            string strSleep = myListControlEntity1.Find(x => x.ID == "txtSleep").Text;
            string strAttempts = myListControlEntity1.Find(x => x.ID == "txtAttempts").Text;
            string strRelativeX = myListControlEntity1.Find(x => x.ID == "txtRelativeX").Text;
            string strRelativeY = myListControlEntity1.Find(x => x.ID == "txtRelativeY").Text;
            string strOccurrence = myListControlEntity1.Find(x => x.ID == "txtOccurrence").Text;
            string strTolerance = myListControlEntity1.Find(x => x.ID == "txtTolerance").Text;
            string strUseGrayScale = myListControlEntity1.Find(x => x.ID == "cbxUseGrayScale").SelectedValue;
            myActions.SetValueByKey("ScriptGeneratorHomeImage", strHomeImage);
            myActions.SetValueByKey("ScriptGeneratorWorkImage", strWorkImage);
            myActions.SetValueByKey("ScriptGeneratorSleep", strSleep);
            myActions.SetValueByKey("ScriptGeneratorAttempts", strAttempts);
            myActions.SetValueByKey("ScriptGeneratorRelativeX", strRelativeX);
            myActions.SetValueByKey("ScriptGeneratorRelativeY", strRelativeY);
            myActions.SetValueByKey("ScriptGeneratorOccurrence", strOccurrence);
            myActions.SetValueByKey("ScriptGeneratorTolerance", strTolerance);
            myActions.SetValueByKey("ScriptGeneratorUseGrayScale", strUseGrayScale);

            string strResultMyArray = myListControlEntity1.Find(x => x.ID == "txtResultMyArray").Text;

            myActions.SetValueByKey("ScriptGeneratorPutAllFastByStoppingOnPerfectMatchResultMyArray", strResultMyArray);
            //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

            string strResultMyArrayToUse = "";
            if (strButtonPressed == "btnOkay")
            {


                strResultMyArrayToUse = strResultMyArray.Trim();


            }
            string strInFile = strApplicationPath + "Templates\\TemplatePutAllFastByStoppingOnPerfectMatch.txt";
            // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

            List<string> listOfSolvedProblems = new List<string>();
            List<string> listofRecs = new List<string>();
            string[] lineszz = System.IO.File.ReadAllLines(strInFile);

            sb.Length = 0;

            int intLineCount = lineszz.Count();
            int intCtr = 0;
            for (int i = 0; i < intLineCount; i++)
            {
                string line = lineszz[i];
                line = line.Replace("&&HomeImage", strHomeImage.Trim());
                line = line.Replace("&&WorkImage", strWorkImage.Trim());
                line = line.Replace("&&ResultMyArray", strResultMyArrayToUse.Trim());
                if (strSleep != "")
                {
                    line = line.Replace("&&Sleep", strSleep);
                }
                if (strAttempts != "")
                {
                    line = line.Replace("&&Attempts", strAttempts);
                }
                if (strRelativeX != "")
                {
                    line = line.Replace("&&RelativeX", strRelativeX);
                }
                if (strRelativeY != "")
                {
                    line = line.Replace("&&RelativeY", strRelativeY);
                }
                if (strOccurrence != "")
                {
                    line = line.Replace("&&Occurrence", strOccurrence);
                }
                if (strTolerance != "")
                {
                    line = line.Replace("&&Tolerance", strTolerance);
                }
                if (strUseGrayScale != "False")
                {
                    line = line.Replace("&&UseGrayScale", strUseGrayScale);
                }

                if (!line.Contains("&&"))
                {
                    sb.AppendLine(line);
                }
            }
            if (strButtonPressed == "btnOkay")
            {


                myActions.PutEntityInClipboard(sb.ToString());
                myActions.MessageBoxShow(sb.ToString());
            }
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
    }
}
