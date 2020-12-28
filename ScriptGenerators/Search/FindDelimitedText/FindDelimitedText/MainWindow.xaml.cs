using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace FindDelimitedText {
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
      if (strWindowTitle.StartsWith("FindDelimitedText")) {
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
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp1 = new List<ComboBoxPair>();
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblFindDelimitedText";
            myControlEntity1.Text = "Find Delimited Text";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblInputs";
            myControlEntity1.Text = "Inputs:";
            myControlEntity1.ToolTipx = "";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lbllines";
            myControlEntity1.Text = "lines";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtlines";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorlines"); ;
            myControlEntity1.ToolTipx = "string[]";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblStartingColumn";
            myControlEntity1.Text = "StartingColumn";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtStartingColumn";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorStartingColumn"); ;
            myControlEntity1.ToolTipx = "int";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblListBeginDelim";
            myControlEntity1.Text = "ListBeginDelim";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtListBeginDelim";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorListBeginDelim"); ;
            myControlEntity1.ToolTipx = "List<string>";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblListEndDelim";
            myControlEntity1.Text = "ListEndDelim";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtListEndDelim";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorListEndDelim"); ;
            myControlEntity1.ToolTipx = "List<string>";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblEmptyRow9";
            myControlEntity1.Text = "";
            myControlEntity1.ToolTipx = "&&TOOLTIP";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblLineCtrInputandOut";
            myControlEntity1.Text = "LineCtr Input and Out:";
            myControlEntity1.ToolTipx = "";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblLineCtr";
            myControlEntity1.Text = "LineCtr";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtLineCtr";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorLineCtr"); ;
            myControlEntity1.ToolTipx = "int";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblEmptyRow12";
            myControlEntity1.Text = "";
            myControlEntity1.ToolTipx = "&&TOOLTIP";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblOutputparmsfollow";
            myControlEntity1.Text = "Output parms follow:";
            myControlEntity1.ToolTipx = "";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblstrDelimitedTextFound";
            myControlEntity1.Text = "strDelimitedTextFound";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtstrDelimitedTextFound";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorstrDelimitedTextFound"); ;
            myControlEntity1.ToolTipx = "string";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblintDelimFound";
            myControlEntity1.Text = "intDelimFound";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtintDelimFound";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorintDelimFound"); ;
            myControlEntity1.ToolTipx = "int";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblstrResultTypeFound";
            myControlEntity1.Text = "strResultTypeFound";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtstrResultTypeFound";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorstrResultTypeFound"); ;
            myControlEntity1.ToolTipx = "string";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblintEndDelimColPosFound";
            myControlEntity1.Text = "intEndDelimColPosFound";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtintEndDelimColPosFound";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorintEndDelimColPosFound"); ;
            myControlEntity1.ToolTipx = "int";
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
            myControlEntity1.Text = "     // Here is an example of looking for what is between two quotes  \r\n" +
"      // in a single line of text in order to find path and file name \r\n" +
"      List<string> myBeginDelim = new List<string>(); \r\n" +
"      List<string> myEndDelim = new List<string>(); \r\n" +
"      myBeginDelim.Add(\"\\\"\"); \r\n" +
"      myEndDelim.Add(\"\\\"\"); \r\n" +
"      FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim); \r\n" +
" \r\n" +
"      string myQuote = \"\\\"\"; \r\n" +
"      delimParms.lines[0] = myOrigEditPlusLine; \r\n" +
" \r\n" +
" \r\n" +
"      myActions.FindDelimitedText(delimParms); \r\n" +
"      int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\\\'); \r\n" +
"      if (intLastSlash < 1) { \r\n" +
"        myActions.MessageBoxShow(\"Could not find last slash in in EditPlusLine - aborting\"); \r\n" +
"        goto myExit;     \r\n" +
"      } \r\n" +
"      string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash); \r\n" +
"      string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1); \r\n" +
"      myBeginDelim.Clear(); \r\n" +
"      myEndDelim.Clear(); \r\n" +
" \r\n" +
"      // in this example, we are trying to find line number that is between open \r\n" +
"      // paren and comma \r\n" +
"      myBeginDelim.Add(\"(\"); \r\n" +
"      myEndDelim.Add(\",\"); \r\n" +
"      delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim); \r\n" +
"      delimParms.lines[0] = myOrigEditPlusLine; \r\n" +
"      myActions.FindDelimitedText(delimParms); \r\n" +
"      string strLineNumber = delimParms.strDelimitedTextFound; ";

            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity1, myListControlEntity1);

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 750, 800, intWindowTop, intWindowLeft);
            string strlines = myListControlEntity1.Find(x => x.ID == "txtlines").Text;
            myActions.SetValueByKey("ScriptGeneratorlines", strlines);
            string strStartingColumn = myListControlEntity1.Find(x => x.ID == "txtStartingColumn").Text;
            myActions.SetValueByKey("ScriptGeneratorStartingColumn", strStartingColumn);
            string strListBeginDelim = myListControlEntity1.Find(x => x.ID == "txtListBeginDelim").Text;
            myActions.SetValueByKey("ScriptGeneratorListBeginDelim", strListBeginDelim);
            string strListEndDelim = myListControlEntity1.Find(x => x.ID == "txtListEndDelim").Text;
            myActions.SetValueByKey("ScriptGeneratorListEndDelim", strListEndDelim);
            string strLineCtr = myListControlEntity1.Find(x => x.ID == "txtLineCtr").Text;
            myActions.SetValueByKey("ScriptGeneratorLineCtr", strLineCtr);
            string strstrDelimitedTextFound = myListControlEntity1.Find(x => x.ID == "txtstrDelimitedTextFound").Text;
            myActions.SetValueByKey("ScriptGeneratorstrDelimitedTextFound", strstrDelimitedTextFound);
            string strintDelimFound = myListControlEntity1.Find(x => x.ID == "txtintDelimFound").Text;
            myActions.SetValueByKey("ScriptGeneratorintDelimFound", strintDelimFound);
            string strstrResultTypeFound = myListControlEntity1.Find(x => x.ID == "txtstrResultTypeFound").Text;
            myActions.SetValueByKey("ScriptGeneratorstrResultTypeFound", strstrResultTypeFound);
            string strintEndDelimColPosFound = myListControlEntity1.Find(x => x.ID == "txtintEndDelimColPosFound").Text;
            myActions.SetValueByKey("ScriptGeneratorintEndDelimColPosFound", strintEndDelimColPosFound);
            string strInFile = strApplicationPath + "Templates\\TemplateFindDelimitedText.txt";
            // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity1);

            List<string> listOfSolvedProblems = new List<string>();
            List<string> listofRecs = new List<string>();
            string[] lineszz = System.IO.File.ReadAllLines(strInFile);

            sb.Length = 0;

            int intLineCount = lineszz.Count();
            int intCtr = 0;
            for (int i = 0; i < intLineCount; i++)
            {
                string line = lineszz[i];
                line = line.Replace("&&ListBeginDelim", strListBeginDelim.Trim());
                line = line.Replace("&&ListEndDelim", strListEndDelim.Trim());
                line = line.Replace("&&lines", strlines.Trim());
                if (strStartingColumn != "")
                {
                    line = line.Replace("&&intStartingColumn", strStartingColumn);
                }
                if (strLineCtr != "")
                {
                    line = line.Replace("&&intLineCtr", strLineCtr);
                }
                if (strstrDelimitedTextFound != "")
                {
                    line = line.Replace("&&strDelimitedTextFound", strstrDelimitedTextFound);
                }
                if (strintDelimFound != "")
                {
                    line = line.Replace("&&intDelimTextFound", strintDelimFound);
                }
                if (strstrResultTypeFound != "")
                {
                    line = line.Replace("&&strResultTypeFound", strstrResultTypeFound);
                }
                if (strintEndDelimColPosFound != "")
                {
                    line = line.Replace("&&intEndDelimColPosFound", strintEndDelimColPosFound);
                }


                if (!line.Contains("&&"))
                {
                    sb.AppendLine(line);
                }
            }
            if (strButtonPressed == "btnOkay")
            {

                // ============

                myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);

                string strOutFile = strApplicationPath + "TemplateCode3GlobalsNew.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile))
                {

                    file.WriteLine("static List<string> " + strListBeginDelim.Trim() + " = new List<string>();");
                    file.WriteLine("static List<string> " + strListEndDelim.Trim() + " = new List<string>();");
                   
                    file.WriteLine("static FindDelimitedTextParms delimParms;");
                    file.WriteLine("static string[] " + strlines.Trim() + " = new string[5000];");
                    file.WriteLine("static string " + strstrDelimitedTextFound + " = \"\";");
                    file.WriteLine("static int " + strStartingColumn + " = -1;");
                    file.WriteLine("static int " + strintDelimFound + " = -1;");
                    file.WriteLine("static string " + strstrResultTypeFound + " = \"\";");
                    file.WriteLine("static int " + strintEndDelimColPosFound + " = -1;");
                    file.WriteLine("static int " + strLineCtr + " = -1;");
                }

                myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

                myActions.Write4MainTemplateToExternalFile(strApplicationPath);

                string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
                WriteCodeBodyToExternalFile(strAppendCodeToExistingFile, strOutCodeBodyFile, sb.ToString());

                myActions.Write5FunctionsTemplateToExternalFile(strApplicationPath);

                myActions.WriteCodeEndToExternalFile();

                string strOutCodeBigFile = myActions.WriteCodeBigExternalFile(strOutCodeBodyFile);

                string strExecutable = @"C:\Windows\system32\notepad.exe";
                string strContent = strOutCodeBigFile;
                Process.Start(strExecutable, string.Concat("", strContent, ""));

                //============
                myActions.PutEntityInClipboard(sb.ToString());
                myActions.MessageBoxShow(sb.ToString());
            }
        myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
        private void WriteCodeBodyToExternalFile(string strAppendCodeToExistingFile, string strOutCodeBodyFile, string strCodeBody)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.Write(strCodeBody);

                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.Write(strCodeBody);

                }
            }
        }

    }
}
