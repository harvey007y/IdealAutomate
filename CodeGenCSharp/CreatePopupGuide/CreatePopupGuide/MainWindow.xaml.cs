using System.Windows;
//using IdealAutomate.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System;
using System.Windows.Controls;

namespace CreatePopupGuide
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
            CreatePopupGuide.Methods myActions = new Methods();
            myActions.ScriptStartedUpdateStats();

            InitializeComponent();
            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("CreatePopupGuide"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            int intWindowTop = 0;
            int intWindowLeft = 0;
            string strWindowTop = "";
            string strWindowLeft = "";
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lblGenerateCodeForPopupGuide";
            myControlEntity.Text = "Generate Code for Popup Guide";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblHeading";
            myControlEntity.Text = "Heading";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtHeading";
            myControlEntity.Text = myActions.GetValueByKey("Heading"); ;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblLabel";
            myControlEntity.Text = "Label";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtLabel";
            myControlEntity.Text = myActions.GetValueByKey("Label"); ;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Multiline = true;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblCopyableText";
            myControlEntity.Text = "CopyableText";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtCopyableText";
            myControlEntity.Text = myActions.GetValueByKey("CopyableText"); ;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Multiline = true;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblOrientation";
            myControlEntity.Text = "Orientation";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("None", "None"));
            cbp.Add(new ComboBoxPair("Up", "Up"));
            cbp.Add(new ComboBoxPair("Right", "Right"));
            cbp.Add(new ComboBoxPair("Down", "Down"));
            cbp.Add(new ComboBoxPair("Left", "Left"));
            myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.SelectedValue = "None";
            myControlEntity.ID = "cbxOrientation";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "Select none for no arrow or direction if you want an arrow";
            myControlEntity.DDLName = "";
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
       //     GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, intWindowTop, intWindowLeft);
            if (strButtonPressed == "btnCancel")
            {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }

            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);

            string strHeading = myListControlEntity.Find(x => x.ID == "txtHeading").Text;
            myActions.SetValueByKey("Heading", strHeading);
            string strLabel = myListControlEntity.Find(x => x.ID == "txtLabel").Text;
            myActions.SetValueByKey("Label", strLabel);
            string strCopyableText = myListControlEntity.Find(x => x.ID == "txtCopyableText").Text;
            myActions.SetValueByKey("CopyableText", strCopyableText);
            string strOrientation = myListControlEntity.Find(x => x.ID == "cbxOrientation").SelectedValue;
            string strID = "";
            //====================================
            string strStartingRow = "";
            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
            intRowCtr = -1;
            string strOutFile = @"C:\Data\BlogPost.txt";
            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window
            StringBuilder sb2 = new StringBuilder(); // this is for retrieving stuff from window
            StringBuilder sb3 = new StringBuilder(); // this is for defining the template
            StringBuilder sb4 = new StringBuilder(); // this is for doing replacements to template
            string strSuffix = "";
            string strText = "";
            string strWidth = "300";
            string strMinimized = "";
            if (sb.Length == 0)
            {
                //sb.AppendLine("int intRowCtr = 0;");
                //sb.AppendLine("ControlEntity myControlEntity = new ControlEntity();");
                sb.AppendLine("List<ControlEntity> myListControlEntity" + strSuffix + " = new List<ControlEntity>();");
                sb.AppendLine("List<ComboBoxPair> cbp" + strSuffix + " = new List<ComboBoxPair>();");
            }
            //====Heading======
            string strInFile = strApplicationPath + "TemplateHeading.txt";
            // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
            string[] lineszz = System.IO.File.ReadAllLines(strInFile);
            int intLineCount = lineszz.Count();
            int intCtr = 0;
            strID = "Heading";
            for (int i = 0; i < intLineCount; i++)
            {
                string line = lineszz[i];
                line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                line = line.Replace("&&SUFFIX", strSuffix.Trim());
                line = line.Replace("&&TEXT", strHeading.Trim());
                line = line.Replace("&&ROW", intRowCtr.ToString());

                if (strWidth != "")
                {
                    line = line.Replace("&&WIDTH", strWidth);
                }

                if (!line.Contains("&&WIDTH"))
                {
                    sb.AppendLine(line);
                }

            }


            sb2.AppendLine("string str" + strID.Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"txt" + strID.Replace(" ", "") + "\").Text;");
            if (strText.Trim() == "")
            {
                sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ");");
            }
            //====Label======
            strInFile = strApplicationPath + "TemplateLabel.txt";
            strID = "Label";
    


            List<string> listOfSolvedProblems = new List<string>();
            List<string> listofRecs = new List<string>();
            lineszz = System.IO.File.ReadAllLines(strInFile);



            intLineCount = lineszz.Count();
            intCtr = 0;
            for (int i = 0; i < intLineCount; i++)
            {
                string line = lineszz[i];
                line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                line = line.Replace("&&SUFFIX", strSuffix.Trim());
                line = line.Replace("&&TEXT", strLabel.Trim());
                line = line.Replace("&&TOOLTIP", ""); //  strToolTip.Trim());
                line = line.Replace("&&ROW", intRowCtr.ToString());
                int intColumnSpan = 2;
                //Int32.TryParse(strColumnSpan, out intColumnSpan);
                line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                if (strWidth != "")
                {
                    line = line.Replace("&&WIDTH", strWidth);
                }

                if (!line.Contains("&&WIDTH"))
                {
                    sb.AppendLine(line);
                }
            }
            sb2.AppendLine("string str" + strID.Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"txt" + strID.Replace(" ", "") + "\").Text;");
            if (strText.Trim() == "")
            {
                sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ");");
            }
            //====Copyable Textbox======
            strInFile = strApplicationPath + "TemplateTextBox.txt";
            strID = "CopyableText";
            lineszz = System.IO.File.ReadAllLines(strInFile);
            string strHeight = "250";
            int intTextBoxHeight = 250;
            bool boolMultiline = true;
            intLineCount = lineszz.Count();
            intCtr = 0;
            int intLineWidthInCharacters = 50;
            int intLineHeight = 30;
            int textLength = strCopyableText.Trim().Length;
            if (textLength > 0) {
                //var lines = tb.Lines.Count();               
                var textArr = strCopyableText.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //     txtline.Text = textArr.Length.ToString();
                int totalNumberOfLines = 0;
                int numberOfLines = 0;
                foreach (var item in textArr)
                {
                     numberOfLines = textLength / intLineWidthInCharacters;
                    if (numberOfLines < 1)
                    {
                        numberOfLines = 1;
                    }
                    totalNumberOfLines += numberOfLines;
                }
               
                intTextBoxHeight = totalNumberOfLines * intLineHeight;
                if (intTextBoxHeight > 29 && intTextBoxHeight < 600)
                {
                    strHeight = intTextBoxHeight.ToString();
                }
                    }
            for (int i = 0; i < intLineCount; i++)
            {
                string line = lineszz[i];
                line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                line = line.Replace("&&SPACEDOUTID", strID.Trim().Replace(" ", ""));
                line = line.Replace("&&SUFFIX", strSuffix.Trim());
                if (strCopyableText.Trim() == "")
                {
                    line = line.Replace("\"&&TEXT\"", strCopyableText.Trim());
                }
                else
                {
                    if (line.Contains("&&TEXT"))
                    {
                        string[] lineText = strCopyableText.Trim().Replace("\\r\\n", "|").Split('|');
                        line = line.Replace("\"&&TEXT\";", "\"\" +");
                        sb.AppendLine(line);
                        int intLineCountText = lineText.Count();
                        intCtr = 0;
                        char tab = '\u0009';
                        foreach (string linea in lineText)
                        {
                            intCtr++;
                            string linex = linea.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r\n", "").Replace("\t", "");
                            linex = Regex.Replace(linex, @"[^\u0000-\u007F]+", string.Empty);
                            linex = linex.Replace("\" +", "\\r\\n\" +");
                            if (intCtr < intLineCountText)
                            {
                                sb.AppendLine("\"" + linex + " \\r\\n\" +");
                            }
                            else
                            {
                                sb.AppendLine("\"" + linex + " \\r\\n\";");
                            }
                        }
                        //continue;
                    }
                }
                int intColumnSpan = 1;

                //	Int32.TryParse(strColumnSpan, out intColumnSpan);
                line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                line = line.Replace("&&TOOLTIP", "");
                line = line.Replace("&&ROW", intRowCtr.ToString());
                if (strWidth != "")
                {
                    line = line.Replace("&&WIDTH", strWidth);
                }
                if (strHeight != "")
                {
                    line = line.Replace("&&HEIGHT", strHeight);
                }
                if (boolMultiline)
                {
                    line = line.Replace("&&MULTILINE", boolMultiline.ToString().ToLower());
                }

                //if (!line.Contains("&&WIDTH") && !line.Contains("&&HEIGHT") && !line.Contains("&&MULTILINE"))
                //{
                    if (line.Trim() != "myControlEntity.Text = \"\" +")
                    {
                        sb.AppendLine(line);
                    }
                //}
            }
            sb2.AppendLine("string str" + strID.Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"txt" + strID.Replace(" ", "") + "\").Text;");
            if (strText.Trim() == "")
            {
                sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ");");
            }

            // Done --------------------
            int intWindowHeight = intTextBoxHeight + 250;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile))
            {
                file.WriteLine("int intRowCtr = 0;");
                file.WriteLine("ControlEntity myControlEntity" + strSuffix + " = new ControlEntity();");
                file.Write(sb.ToString());
                file.WriteLine("string strButtonPressed = myActions.WindowMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + ", " + intWindowHeight + ", 500, " + intWindowTop + ", " + intWindowLeft + ");"); //intWindowTop, intWindowLeft);
                file.WriteLine("if (strButtonPressed == \"btnCancel\") {");
                file.WriteLine("  myActions.MessageBoxShow(\"Okay button not pressed - Script Cancelled\");");
                file.WriteLine("  goto myExit;");
                file.WriteLine("}");
                file.WriteLine("");

                file.Write(sb2.ToString());


            }
            string strExecutable = @"C:\Windows\system32\notepad.exe";
            string strContent = strOutFile;
            Process.Start(strExecutable, string.Concat("", strContent, ""));
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft)
        {
            strWindowLeft = myActions.GetValueByKeyGlobal("WindowLeft");
            strWindowTop = myActions.GetValueByKeyGlobal("WindowTop");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
        }

     
    }
}
