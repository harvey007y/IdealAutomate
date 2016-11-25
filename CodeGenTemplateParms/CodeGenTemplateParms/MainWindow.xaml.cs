using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace CodeGenTemplateParms {
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

            InitializeComponent();
            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("CodeGenTemplateParms")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            string strIterations = "";
            string strIterationsID = "";
            string strIteratorID = "";
            string strSuffix = "";
            string strStartingRow = "";
            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
            int intRowCtr = -1;
            string strOutFile = @"C:\Data\BlogPost.txt";
            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window
            StringBuilder sb2 = new StringBuilder(); // this is for retrieving stuff from window
            StringBuilder sb3 = new StringBuilder(); // this is for defining the template
            StringBuilder sb4 = new StringBuilder(); // this is for doing replacements to template


            AddControl:
            string strButtonPressed = "";
            intRowCtr++;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblDefaultScript";
            myControlEntity.Text = "Default Script:";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblDefaultScriptValue";
            myControlEntity.Text = myActions.GetValueByKey("ScriptsDefaultKey", "IdealAutomateDB");
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnChooseDefaultScript";
            myControlEntity.Text = "Choose Default Script";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblEmptyRow5";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 2;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblSuffix";
            myControlEntity.Text = "Suffix for Control Entity";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 3;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtSuffix";
            myControlEntity.Text = strSuffix;
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 3;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblSuffix1";
            myControlEntity.Text = "(Optional)";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 3;
            myControlEntity.ColumnNumber = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblStartingRow";
            myControlEntity.Text = "Starting Row";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 4;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtStartingRow";
            myControlEntity.Text = strStartingRow;
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 4;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblStartingRow1";
            myControlEntity.Text = "(Optional)";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 4;
            myControlEntity.ColumnNumber = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnLabel";
            myControlEntity.Text = "Label";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 5;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnTextBox";
            myControlEntity.Text = "TextBox";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 6;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnComboBox";
            myControlEntity.Text = "ComboBox";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 7;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnHeading";
            myControlEntity.Text = "Heading";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 8;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnEmptyRow";
            myControlEntity.Text = "Empty Row";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 9;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnButton";
            myControlEntity.Text = "Button";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 10;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblEmptyRow0";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 11;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnIterator";
            myControlEntity.Text = "Iterator";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 12;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnNumberOfIterations";
            myControlEntity.Text = "Number of Iterations";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 13;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnTemplate";
            myControlEntity.Text = "Template";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 14;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblEmptyRow1";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 15;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDisplay";
            myControlEntity.Text = "Display Prototype";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 16;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
            strSuffix = myListControlEntity.Find(x => x.ID == "txtSuffix").Text;
            strStartingRow = myListControlEntity.Find(x => x.ID == "txtStartingRow").Text;
            int intStartingRow = -1;
            Int32.TryParse(strStartingRow, out intStartingRow);
            if (strStartingRow.Trim() != "" && intRowCtr == 0) {
                intRowCtr = intStartingRow;
            }
            if (sb.Length == 0) {
                sb.AppendLine("List<ControlEntity> myListControlEntity" + strSuffix + " = new List<ControlEntity>();");
                sb.AppendLine("List<ComboBoxPair> cbp" + strSuffix + " = new List<ComboBoxPair>();");
            }

            //string mySearchTerm = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            // label
            if (strButtonPressed == "btnChooseDefaultScript") {
                myActions.RunSync(@"C:\SVNIA\trunk\DDLMaint\DDLMaint\bin\debug\DDLMaint.exe", "");
                goto AddControl;
            }
            if (strButtonPressed == "btnLabel") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Label Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblToolTip";
                myControlEntity.Text = "ToolTip";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtToolTip";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorLabelToolTip", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 3;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumnSpan";
                myControlEntity.Text = "ColumnSpan:";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtColumnSpan";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorLabelColumnSpan", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateLabel.txt";
                string strToolTip = myListControlEntity.Find(x => x.ID == "txtToolTip").Text;
                string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                myActions.SetValueByKey("ScriptGeneratorLabelToolTip", strToolTip, "IdealAutomateDB");
                myActions.SetValueByKey("ScriptGeneratorLabelColumnSpan", strColumnSpan, "IdealAutomateDB");
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ",""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                    }

                    if (!line.Contains("&&WIDTH")) {
                        sb.AppendLine(line);
                    }
                }

                goto AddControl;
            }

            // EmptyRow
            if (strButtonPressed == "btnEmptyRow") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Empty Row";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = "";
                string strWidth = "";
                string strID = "EmptyRow" + intRowCtr;
                string strInFile = strApplicationPath + "TemplateLabel.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);

                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                    }

                    if (!line.Contains("&&WIDTH")) {
                        sb.AppendLine(line);
                    }
                }

                goto AddControl;
            }

            // TextBox ----------------------------------------
            if (strButtonPressed == "btnTextBox") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add TextBox Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblHeight";
                myControlEntity.Text = "Height";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtHeight";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.CheckBox;
                myControlEntity.ID = "chkMultiline";
                myControlEntity.Text = "Multiline";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblToolTip";
                myControlEntity.Text = "ToolTip";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtToolTip";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorTextBoxToolTip", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 5;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumnSpan";
                myControlEntity.Text = "ColumnSpan:";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtColumnSpan";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorTextBoxColumnSpan", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strHeight = myListControlEntity.Find(x => x.ID == "txtHeight").Text;
                bool boolMultiline = myListControlEntity.Find(x => x.ID == "chkMultiline").Checked;
                string strInFile = strApplicationPath + "TemplateTextBox.txt";
                string strToolTip = myListControlEntity.Find(x => x.ID == "txtToolTip").Text;
                string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                myActions.SetValueByKey("ScriptGeneratorTextBoxColumnSpan", strColumnSpan, "IdealAutomateDB");
                myActions.SetValueByKey("ScriptGeneratorTextBoxToolTip", strToolTip, "IdealAutomateDB");

                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);

                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SPACEDOUTID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    if (strText.Trim() == "") {
                        line = line.Replace("\"&&TEXT\"", "myActions.GetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey", "IdealAutomateDB") + strID.Trim().Replace(" ","") + "\", \"IdealAutomateDB\");");
                    } else {
                        line = line.Replace("&&TEXT", strText.Trim().Replace("\\r\\n", "\" + System.Environment.NewLine + \""));
                    }
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                    }
                    if (strHeight != "") {
                        line = line.Replace("&&HEIGHT", strHeight);
                    }
                    if (boolMultiline) {
                        line = line.Replace("&&MULTILINE", boolMultiline.ToString());
                    }

                    if (!line.Contains("&&WIDTH") && !line.Contains("&&HEIGHT") && !line.Contains("&&MULTILINE")) {
                        sb.AppendLine(line);
                    }
                }
                sb2.AppendLine("string str" + strID.Replace(" ","") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"txt" + strID.Replace(" ", "") + "\").Text;");
                if (strText.Trim() == "") {
                    sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey", "IdealAutomateDB") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ", \"IdealAutomateDB\");");
                }
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strID.Replace(" ", "") + "\",str" + strID.Replace(" ", "") + ");");

                goto AddControl;
            }

            // ComboBox ----------------------------------------
            if (strButtonPressed == "btnComboBox") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add ComboBox Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblSelectedValue";
                myControlEntity.Text = "Default Selected Value";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtSelectedValue";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblKeys";
                myControlEntity.Text = "Keys";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblValues";
                myControlEntity.Text = "Values";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey1";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue1";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey2";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue2";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey3";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue3";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey4";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 7;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue4";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 7;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey5";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 8;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue5";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 8;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblToolTip";
                myControlEntity.Text = "ToolTip";
                myControlEntity.RowNumber = 9;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtToolTip";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorComboBoxToolTip", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 9;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblDDLName";
                myControlEntity.Text = "DDLName";
                myControlEntity.RowNumber = 10;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtDDLName";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorComboBoxDDLName", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 10;                
                myControlEntity.ToolTipx = "Only needed when same combobox is used on same page more than once. \r\n The DDLName will be the same ID for both comboboxes, and the ID will be unique for each combobox. \r\n Example, DDLName = variables for two comboboxes, but ID will be variables1 and variables2";
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblDDLName2";
                myControlEntity.Text = "(Optional)";
                myControlEntity.RowNumber = 10;
                myControlEntity.ColumnNumber = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumnSpan";
                myControlEntity.Text = "ColumnSpan:";
                myControlEntity.RowNumber = 11;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtColumnSpan";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorComboBoxColumnSpan", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 11;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 500, 500, 0, 0);

                string strSelectedValue = myListControlEntity.Find(x => x.ID == "txtSelectedValue").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strKey1 = myListControlEntity.Find(x => x.ID == "txtKey1").Text;
                string strValue1 = myListControlEntity.Find(x => x.ID == "txtValue1").Text;

                string strKey2 = myListControlEntity.Find(x => x.ID == "txtKey2").Text;
                string strValue2 = myListControlEntity.Find(x => x.ID == "txtValue2").Text;

                string strKey3 = myListControlEntity.Find(x => x.ID == "txtKey3").Text;
                string strValue3 = myListControlEntity.Find(x => x.ID == "txtValue3").Text;

                string strKey4 = myListControlEntity.Find(x => x.ID == "txtKey4").Text;
                string strValue4 = myListControlEntity.Find(x => x.ID == "txtValue4").Text;

                string strKey5 = myListControlEntity.Find(x => x.ID == "txtKey5").Text;
                string strValue5 = myListControlEntity.Find(x => x.ID == "txtValue5").Text;

                string strInFile = strApplicationPath + "TemplateComboBox.txt";
                string strToolTip = myListControlEntity.Find(x => x.ID == "txtToolTip").Text;
                string strDDLName = myListControlEntity.Find(x => x.ID == "txtDDLName").Text;
                string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                myActions.SetValueByKey("ScriptGeneratorComboBoxColumnSpan", strColumnSpan, "IdealAutomateDB");
                myActions.SetValueByKey("ScriptGeneratorComboBoxToolTip", strToolTip, "IdealAutomateDB");

                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";                
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);
                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    
                    line = line.Replace("&&DDLNAME", strDDLName.Trim().Replace(" ", ""));
                    line = line.Replace("&&SPACEDOUTID", strID.Trim());
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    if (strSelectedValue.Trim() == "") {
                        line = line.Replace("\"&&SELECTEDVALUE\"", "myActions.GetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey", "IdealAutomateDB") + strID.Trim().Replace(" ", "") + "\", \"IdealAutomateDB\");");
                    } else {
                        line = line.Replace("&&SELECTEDVALUE", strSelectedValue.Trim());
                    }
                    
                    if (strKey1.Trim() != "") {
                        line = line.Replace("&&KEY1", strKey1);
                    }
                    if (strValue1.Trim() != "") {
                        line = line.Replace("&&VALUE1", strValue1);
                    }

                    if (strKey2.Trim() != "") {
                        line = line.Replace("&&KEY2", strKey2);
                    }
                    if (strValue1.Trim() != "") {
                        line = line.Replace("&&VALUE2", strValue2);
                    }

                    if (strKey3.Trim() != "") {
                        line = line.Replace("&&KEY3", strKey3);
                    }
                    if (strValue3.Trim() != "") {
                        line = line.Replace("&&VALUE3", strValue3);
                    }

                    if (strKey4.Trim() != "") {
                        line = line.Replace("&&KEY4", strKey4);
                    }
                    if (strValue4.Trim() != "") {
                        line = line.Replace("&&VALUE4", strValue4);
                    }

                    if (strKey5.Trim() != "") {
                        line = line.Replace("&&KEY5", strKey5);
                    }
                    if (strValue5.Trim() != "") {
                        line = line.Replace("&&VALUE5", strValue5);
                    }

                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                    }

                    if (!line.Contains("&&WIDTH") && !line.Contains("&&SELECTEDVALUE") &&
                      !line.Contains("&&KEY1") && !line.Contains("&&VALUE1") &&
                      !line.Contains("&&KEY2") && !line.Contains("&&VALUE2") &&
                      !line.Contains("&&KEY3") && !line.Contains("&&VALUE3") &&
                      !line.Contains("&&KEY4") && !line.Contains("&&VALUE4") &&
                      !line.Contains("&&KEY5") && !line.Contains("&&VALUE5")
                      ) {
                        sb.AppendLine(line);
                    }
                }

                sb2.AppendLine("string str" + strID.Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"cbx" + strID.Replace(" ", "") + "\").SelectedValue;");
                if (strSelectedValue.Trim() == "") {
                    sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey", "IdealAutomateDB") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ", \"IdealAutomateDB\");");
                }
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strID.Replace(" ", "") + "\",str" + strID.Replace(" ", "") + ");");

                goto AddControl;
            }

            // Heading -------------------------------------------------
            if (strButtonPressed == "btnHeading") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Heading Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateHeading.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);
                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                    }

                    if (!line.Contains("&&WIDTH")) {
                        sb.AppendLine(line);
                    }
                }

                goto AddControl;
            }

            // Iterator ----------------------------------------
            if (strButtonPressed == "btnIterator") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Iterator Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblStart";
                myControlEntity.Text = "Start";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtStart";
                myControlEntity.Text = "0";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblIncrementBy";
                myControlEntity.Text = "Increment By";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtIncrementBy";
                myControlEntity.Text = "1";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strStart = myListControlEntity.Find(x => x.ID == "txtStart").Text;
                string strIncrementBy = myListControlEntity.Find(x => x.ID == "txtIncrementBy").Text;
                strIteratorID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateIterator.txt";



                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    if (line.Contains("&&STARTROW")) {
                        line = line.Replace("&&STARTROW", intRowCtr.ToString());
                        intRowCtr++;
                    }
                    if (line.Contains("&&INCREMENTBYROW")) {
                        line = line.Replace("&&INCREMENTBYROW", intRowCtr.ToString());
                        intRowCtr++;
                    }
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strIteratorID.Trim().Replace(" ",""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&START", strStart.Trim());
                    line = line.Replace("&&INCREMENTBY", strIncrementBy.Trim());
                    line = line.Replace("&&TEXT", strStart.Trim());

                    sb.AppendLine(line);
                }

                sb2.AppendLine("string strStart" + strIteratorID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtStart" + strIteratorID.Trim() + "\").Text;");
                sb2.AppendLine("int intStart" + strIteratorID.Trim() + " = 0;");
                sb2.AppendLine("Int32.TryParse(strStart" + strIteratorID.Trim() + ",out intStart" + strIteratorID.Trim() + ");");
                sb2.AppendLine("string strIncrementBy" + strIteratorID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtIncrementBy" + strIteratorID.Trim() + "\").Text;");
                sb2.AppendLine("int intIncrementBy" + strIteratorID.Trim() + " = 0;");
                sb2.AppendLine("Int32.TryParse(strIncrementBy" + strIteratorID.Trim() + ",out intIncrementBy" + strIteratorID.Trim() + ");");
                sb2.AppendLine("string strIterator" + strIteratorID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtIterator" + strIteratorID.Trim() + "\").Text;");
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strIteratorID + "\",intIterator.ToString());");
                goto AddControl;
            }

            // Number of Iterations ----------------------------------------
            if (strButtonPressed == "btnNumberOfIterations") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Number of Iterations Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblIterations";
                myControlEntity.Text = "Number of Iterations";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtIterations";
                myControlEntity.Text = "1";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());





                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                strIterations = myListControlEntity.Find(x => x.ID == "txtIterations").Text;
                strIterationsID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateIterations.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strIterationsID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&ITERATIONS", strIterations.Trim());
                    sb.AppendLine(line);
                }

                sb2.AppendLine("string strIterations" + strIterationsID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtIterations" + strIterationsID.Trim() + "\").Text;");
                sb2.AppendLine("int intIterations" + strIterationsID.Trim() + " = 0;");
                sb2.AppendLine("Int32.TryParse(strIterations" + strIterationsID.Trim() + ",out intIterations" + strIterationsID.Trim() + ");");
                goto AddControl;
            }

            // Template ----------------------------------------
            if (strButtonPressed == "btnTemplate") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Template Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblTemplate";
                myControlEntity.Text = "Template";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtTemplate";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());





                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strTemplate = myListControlEntity.Find(x => x.ID == "txtTemplate").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateTemplate.txt";

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);


                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEMPLATE", strTemplate.Trim());
                    sb.AppendLine(line);
                }

                sb2.AppendLine("string strTemplate" + strID.Trim().Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"txtTemplate" + strID.Trim().Replace(" ", "") + "\").Text;");
                sb3.AppendLine("string txtTemplateOut =  strTemplate" + strID.Trim().Replace(" ", "") + ";");
                goto AddControl;
            }

            // Add button control
            if (strButtonPressed == "btnButton") {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Button Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblToolTip";
                myControlEntity.Text = "ToolTip";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtToolTip";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorButtonToolTip", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 3;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumnSpan";
                myControlEntity.Text = "ColumnSpan:";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtColumnSpan";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorButtonColumnSpan", "IdealAutomateDB"); ;
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateButton.txt";
                string strToolTip = myListControlEntity.Find(x => x.ID == "txtToolTip").Text;
                string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                myActions.SetValueByKey("ScriptGeneratorButtonColumnSpan", strColumnSpan, "IdealAutomateDB");
                myActions.SetValueByKey("ScriptGeneratorButtonToolTip", strToolTip, "IdealAutomateDB");
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++) {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                    }

                    if (!line.Contains("&&WIDTH")) {
                        sb.AppendLine(line);
                    }
                }

                goto AddControl;
            }

            if (strButtonPressed == "btnDisplay") {
                // http://www.codeproject.com/Tips/715891/Compiling-Csharp-Code-at-Runtime
                string code = @"
   using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System;
using System.Drawing;


using System.Reflection;

    namespace First
    {
        public class Program : Window 
        {
            public static void Main()
            {
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
            " +
                "IdealAutomate.Core.Methods myActions = new Methods();" +
                "string strButtonPressed = \"\";" +               
                " ControlEntity myControlEntity" + strSuffix + " = new ControlEntity();" +
                               sb.ToString() + " strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity"  + strSuffix + " , 600, 500, 0, 0);" + sb2.ToString() +

                "Console.WriteLine(\"Hello, world!\");"
                + @"
            }
        }
    }
";
                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();
                // Reference to System.Drawing library
                parameters.ReferencedAssemblies.Add("System.Drawing.dll");
                parameters.ReferencedAssemblies.Add(@"C:\SVNIA\trunk\IdealAutomateCore\IdealAutomateCore\bin\Debug\IdealAutomateCore.dll");
                parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\PresentationFramework.dll");
                parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\PresentationCore.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.Data.dll");
                parameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parameters.ReferencedAssemblies.Add("System.Xaml.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");
                parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\WindowsBase.dll");


                // True - memory generation, false - external file generation
                parameters.GenerateInMemory = true;
                // True - exe file generation, false - dll file generation
                parameters.GenerateExecutable = true;
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
                if (results.Errors.HasErrors) {
                    StringBuilder sb5 = new StringBuilder();

                    foreach (CompilerError error in results.Errors) {
                        sb5.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    }

                    myActions.MessageBoxShow(sb5.ToString());
                }
                Assembly assembly = results.CompiledAssembly;
                Type program = assembly.GetType("First.Program");
                MethodInfo main = program.GetMethod("Main");
                main.Invoke(null, null);
                myActions.MessageBoxShow(sb.ToString() + " strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);" + sb2.ToString());
                goto AddControl;
            }

            System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("Click Yes to Confirm or No to Continue");
            if (myResult == System.Windows.Forms.DialogResult.No) {
                goto AddControl;
            }
            if (strButtonPressed == "btnCancel") {
                goto myExit;
            }
            // Done --------------------

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {
                file.WriteLine(" ControlEntity myControlEntity" + strSuffix + " = new ControlEntity();");
                file.Write(sb.ToString());
                file.WriteLine("myActions.WindowMultipleControls(ref myListControlEntity" + strSuffix + ", 400, 500, 0, 0);");
                file.Write(sb2.ToString());

                // TODO: I have the template in txtTemplateOut and I need to
                // loop iteration times over the template starting at start iterator
                // incrementing iterator incrementby 
                file.WriteLine("int intIterator = intStart" + strIteratorID.Trim() + ";");
                file.WriteLine(" string strOutFile = @\"C:\\Data\\TemplateOut.txt\";");
                file.WriteLine("using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {");
                file.WriteLine(" for (int i = 0; i < intIterations" + strIterationsID.Trim() + "; i++)");
                file.WriteLine("{");
                file.Write(sb3.ToString());
                file.Write(sb4.ToString());
                file.WriteLine("intIterator += intIncrementBy" + strIteratorID.Trim() + ";");
                file.WriteLine("file.WriteLine(\"\");");
                file.WriteLine("file.Write(txtTemplateOut);");
                file.WriteLine("}");
                file.WriteLine("}");
                file.WriteLine("string strExecutable = @\"C:\\Windows\\system32\\notepad.exe\";");
                file.WriteLine("string strContent = strOutFile;");
                file.WriteLine("Process.Start(strExecutable, string.Concat(\"\", strContent, \"\"));");
            }
            string strExecutable = @"C:\Windows\system32\notepad.exe";
            string strContent = strOutFile;
            Process.Start(strExecutable, string.Concat("", strContent, ""));

            //bool boolUseNewTab = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
            //if (boolUseNewTab == true)
            //{
            //    List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
            //    myWindowTitles.RemoveAll(item => item == "");
            //    if (myWindowTitles.Count > 0)
            //    {
            //        myActions.ActivateWindowByTitle(myWindowTitles[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
            //        myActions.TypeText("%(d)", 1500); // select address bar
            //        myActions.TypeText("{ESC}", 1500);
            //        myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab
            //        myActions.TypeText("%(d)", 1500);
            //        myActions.TypeText(myWebSite, 1500);
            //        myActions.TypeText("{ENTER}", 1500);
            //        myActions.TypeText("{ESC}", 1500);

            //    }
            //    else {
            //        myActions.Run("iexplore", myWebSite);

            //    }
            //}
            //else {
            //    myActions.Run("iexplore", myWebSite);
            //}

            //myActions.Sleep(1000);
            //myActions.TypeText(mySearchTerm, 500);
            //myActions.TypeText("{ENTER}", 500);


            goto myExit;
            myActions.RunSync(@"C:\Windows\Explorer.EXE", @"C:\SVN");
            myActions.TypeText("%(e)", 500);
            myActions.TypeText("a", 500);
            myActions.TypeText("^({UP 10})", 500);
            myActions.TypeText("^(\" \")", 500);
            myActions.TypeText("+({F10})", 500);
            ImageEntity myImage = new ImageEntity();

            if (boolRunningFromHome) {
                myImage.ImageFile = "Images\\imgSVNUpdate_Home.PNG";
            } else {
                myImage.ImageFile = "Images\\imgSVNUpdate.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 5;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;

            int[,] myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0) {
                myActions.MessageBoxShow("I could not find image of SVN Update");
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.Sleep(1000);
            myActions.LeftClick(myArray);
            myImage = new ImageEntity();
            if (boolRunningFromHome) {
                myImage.ImageFile = "Images\\imgUpdateLogOK_Home.PNG";
            } else {
                myImage.ImageFile = "Images\\imgUpdateLogOK.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 200;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0) {
                myActions.MessageBoxShow("I could not find image of OK button for update log");
            }
            myActions.Sleep(1000);
            myActions.LeftClick(myArray);
            myActions.TypeText("%(f)", 200);
            myActions.TypeText("{UP}", 500);
            myActions.TypeText("{ENTER}", 500);
            myActions.Sleep(1000);
            myActions.RunSync(@"C:\Windows\Explorer.EXE", @"");
            myImage = new ImageEntity();
            if (boolRunningFromHome) {
                myImage.ImageFile = "Images\\imgPatch2015_08_Home.PNG";
            } else {
                myImage.ImageFile = "Images\\imgPatch2015_08.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 200;
            myImage.RelativeX = 30;
            myImage.RelativeY = 10;


            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0) {
                myActions.MessageBoxShow("I could not find image of " + myImage.ImageFile);
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.RightClick(myArray);

            myImage = new ImageEntity();

            if (boolRunningFromHome) {
                myImage.ImageFile = "Images\\imgSVNUpdate_Home.PNG";
            } else {
                myImage.ImageFile = "Images\\imgSVNUpdate.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 5;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;

            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0) {
                myActions.MessageBoxShow("I could not find image of SVN Update");
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.Sleep(1000);
            myActions.LeftClick(myArray);
            myImage = new ImageEntity();
            if (boolRunningFromHome) {
                myImage.ImageFile = "Images\\imgUpdateLogOK_Home.PNG";
            } else {
                myImage.ImageFile = "Images\\imgUpdateLogOK.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 200;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0) {
                myActions.MessageBoxShow("I could not find image of OK button for update log");
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.Sleep(1000);
            myActions.LeftClick(myArray);
            myActions.TypeText("%(f)", 200);
            myActions.TypeText("{UP}", 500);
            myActions.TypeText("{ENTER}", 500);
            myActions.Sleep(1000);
            myActions.Run(@"C:\SVNStats.bat", "");
            myActions.Run(@"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE", @"C:\SVNStats\SVNStats.xlsx");
            myExit:
            Application.Current.Shutdown();
        }
    }
}