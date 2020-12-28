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
using System.IO;
using System.Text.RegularExpressions;
using Snipping_OCR;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace MultipleBalloonWindowControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        int intWindowHeight = 110;
        int _maxLineChars = 0;
        string strBalloonArrowDirection = "NONE";
        double windowWidth = 0;
        System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
        List<string> _myContentList = new List<string>();
        public MainWindow()
        {
            string[] args = Environment.GetCommandLineArgs();
            string strMinimized = "";
            if (args.Length > 1 && args[1] == "Minimized")
            {
                strMinimized = "Minimized";
            }
            string _PositionType = "Absolute";
            string _RelativeTop = "0";
            string _RelativeLeft = "0";
            string _RelativeFullFileName = "";
            string relativeCodeSnippet = "";

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
            if (strWindowTitle.StartsWith("MultipleWindowControls"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);

            int intWindowTop = 0;
            int intWindowLeft = 0;
            int intBalloonWindowTop = 0;
            int intBalloonWindowLeft = 0;
            string strWindowTop = "";
            string strWindowLeft = "";

            string strIterations = "";
            string strIterationsID = "";
            string strIteratorID = "";
            string strSuffix = "";
            string strStartingRow = "";
            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
            int intRowCtr = -1;



            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window
            StringBuilder sb2 = new StringBuilder(); // this is for retrieving stuff from window
            StringBuilder sb3 = new StringBuilder(); // this is for defining the template
            StringBuilder sb4 = new StringBuilder(); // this is for doing replacements to template
            StringBuilder sbFunctions = new StringBuilder(); // this is for one time functions we will need

            // if sb is empty, initialize it with fields we will need
            string strInFilex = strApplicationPath + "TemplateCode5Functions.txt";
            // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

            string[] lineszzz = System.IO.File.ReadAllLines(strInFilex);
            foreach (var line in lineszzz)
            {
                sbFunctions.AppendLine(line);
            }

        // add controls to window that will allow user to specify what controls should be in balloon
        AddControl:
            string strButtonPressed = "";
            intRowCtr++;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();



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
            myControlEntity.ID = "btnImage";
            myControlEntity.Text = "Image";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 9;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnEmptyRow";
            myControlEntity.Text = "Empty Row";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 10;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnButton";
            myControlEntity.Text = "Button";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 11;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnPositionPopup";
            myControlEntity.Text = "Position Popup";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 11;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblEmptyRow1";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 12;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnRemoveAllControls";
            myControlEntity.Text = "Reset Popup to Empty";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 13;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblEmptyRow1";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 14;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDisplay";
            myControlEntity.Text = "Display Prototype";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 15;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr = 15;

            string strAppendCodeToExistingFile = myActions.CheckboxForAppendCode(intRowCtr, myControlEntity, myListControlEntity);

            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 500, 500, intWindowTop, intWindowLeft);
            strSuffix = myListControlEntity.Find(x => x.ID == "txtSuffix").Text;
            strStartingRow = myListControlEntity.Find(x => x.ID == "txtStartingRow").Text;
            strAppendCodeToExistingFile = myActions.GetAndUpdateValueForCheckBoxAppendCode(myListControlEntity);

            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            intBalloonWindowTop = intWindowTop + 350;
            intBalloonWindowLeft = intWindowLeft + 200;
            int intStartingRow = -1;
            Int32.TryParse(strStartingRow, out intStartingRow);
            if (strStartingRow.Trim() != "" && intRowCtr == 0)
            {
                intRowCtr = intStartingRow;
            }


            #region AddControlsWindow
            // if remove all controls, set sb to length 0
            if (strButtonPressed == "btnRemoveAllControls")
            {
                sb.Length = 0;

                goto AddControl;
            }
            if (sb.Length == 0)
            {
                sb.AppendLine("myListControlEntity = new List<ControlEntity>();");
            }

            //string mySearchTerm = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            // label
            if (strButtonPressed == "btnChooseDefaultScript")
            {
                myActions.RunSync(myActions.GetValueByKey("SVNPath") + @"DDLMaint\DDLMaint\bin\debug\DDLMaint.exe", "");
                goto AddControl;
            }

            // if label button pressed, show screen to let them add a label
            if (strButtonPressed == "btnLabel")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();
                int intRowCtrx = 0;
                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Label Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.Multiline = true;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblToolTip";
                myControlEntity.Text = "ToolTip";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtToolTip";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorLabelToolTip"); ;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblFontSize";
                myControlEntity.Text = "FontSize";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtFontSize";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorLabelFontSize"); ;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumnSpan";
                myControlEntity.Text = "ColumnSpan:";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtColumnSpan";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorLabelColumnSpan"); ;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strHeight = "";
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateLabel.txt";
                string strToolTip = myListControlEntity.Find(x => x.ID == "txtToolTip").Text;
                string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                string strFontSize = myListControlEntity.Find(x => x.ID == "txtFontSize").Text;
                myActions.SetValueByKey("ScriptGeneratorLabelToolTip", strToolTip);
                if (strFontSize == "")
                {
                    strFontSize = "12";
                }
                myActions.SetValueByKey("ScriptGeneratorLabelFontSize", strFontSize);
                myActions.SetValueByKey("ScriptGeneratorLabelColumnSpan", strColumnSpan);
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
                _myContentList.Add(strText);
                FindWidthInCharsForContent(_myContentList);

                double dblContentHeight = CalculateStringHeight(strText, _maxLineChars);
                strHeight = dblContentHeight.ToString();
                intWindowHeight += (int)dblContentHeight;
                int windowWidthx = _maxLineChars * 8;
                if (windowWidthx > windowWidth)
                {
                    windowWidth = windowWidthx;
                }
                if (strWidth == "")
                {
                    strWidth = windowWidth.ToString();
                }
                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    if (strText.Trim() == "")
                    {
                        line = line.Replace("\"&&TEXT\"", "myActions.GetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\");");
                    }
                    else
                    {
                        if (line.Contains("&&TEXT"))
                        {
                            string strTextTrim = strText.Trim();
                            string strTextReplaced = strTextTrim.Replace(System.Environment.NewLine, "|");
                            string[] lineText = strTextReplaced.Split('|');
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
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    line = line.Replace("&&FONTSIZE", strFontSize.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    if (strWidth != "")
                    {
                        line = line.Replace("&&WIDTH", strWidth);
                    }
                    line = line.Replace("&&HEIGHT", strHeight);
                    if (!line.Contains("&&WIDTH") && line.Trim() != "myControlEntity.Text = \"\" +")
                    {
                        sb.AppendLine(line);
                    }
                }

                goto AddControl;
            }

            // EmptyRow
            if (strButtonPressed == "btnEmptyRow")
            {
                intWindowHeight += 50;
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
                string strHeight = "50";
                string strToolTip = "";
                string strColumnSpan = "";

                string strID = "EmptyRow" + intRowCtr;
                string strInFile = strApplicationPath + "TemplateLabel.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);

                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&FONTSIZE", "12");
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    if (strWidth != "")
                    {
                        line = line.Replace("&&WIDTH", strWidth);
                    }
                    line = line.Replace("&&HEIGHT", strHeight);

                    if (!line.Contains("&&WIDTH"))
                    {
                        sb.AppendLine(line);
                    }

                    //

                }

                goto AddControl;
            }

            // TextBox ----------------------------------------
            if (strButtonPressed == "btnTextBox")
            {
                intWindowHeight += 50;
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();
                int intRowCtrx = 0;
                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add TextBox Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.Multiline = true;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblHeight";
                myControlEntity.Text = "Height";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtHeight";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.CheckBox;
                myControlEntity.ID = "chkMultiline";
                myControlEntity.Text = "Multiline";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblToolTip";
                myControlEntity.Text = "ToolTip";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtToolTip";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorTextBoxToolTip"); ;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblFontSize";
                myControlEntity.Text = "FontSize";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtFontSize";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorTextBoxFontSize"); ;
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.Height = 100;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtrx++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumnSpan";
                myControlEntity.Text = "ColumnSpan:";
                myControlEntity.RowNumber = intRowCtrx;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtColumnSpan";
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorTextBoxColumnSpan"); ;
                myControlEntity.RowNumber = intRowCtrx;
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
                string strFontSize = myListControlEntity.Find(x => x.ID == "txtFontSize").Text;
                myActions.SetValueByKey("ScriptGeneratorTextBoxToolTip", strToolTip);
                if (strFontSize == "")
                {
                    strFontSize = "12";
                }
                myActions.SetValueByKey("ScriptGeneratorTextBoxFontSize", strFontSize);
                string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                myActions.SetValueByKey("ScriptGeneratorTextBoxColumnSpan", strColumnSpan);
                myActions.SetValueByKey("ScriptGeneratorTextBoxToolTip", strToolTip);
                _myContentList.Add(strText);
                FindWidthInCharsForContent(_myContentList);

                double dblContentHeight = CalculateStringHeight(strText, _maxLineChars);
                strHeight = dblContentHeight.ToString();
                intWindowHeight += (int)dblContentHeight;
                int windowWidthx = _maxLineChars * 8;
                if (windowWidthx > windowWidth)
                {
                    windowWidth = windowWidthx;
                }
                if (strWidth != "")
                {
                    strWidth = windowWidth.ToString();
                }
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);

                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SPACEDOUTID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    if (strText.Trim() == "")
                    {
                        line = line.Replace("\"&&TEXT\"", "myActions.GetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\");");
                    }
                    else
                    {
                        if (line.Contains("&&TEXT"))
                        {
                            string[] lineText = strText.Trim().Replace("\\r\\n", "|").Split('|');
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
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&FONTSIZE", strFontSize.Trim());
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

                    if (!line.Contains("&&WIDTH") && !line.Contains("&&HEIGHT") && !line.Contains("&&MULTILINE") && line.Trim() != "myControlEntity.Text = \"\" +")
                    {
                        sb.AppendLine(line);
                    }
                }
                sb2.AppendLine("string str" + strID.Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"txt" + strID.Replace(" ", "") + "\").Text;");
                if (strText.Trim() == "")
                {
                    sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ");");
                }
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strID.Replace(" ", "") + "\",str" + strID.Replace(" ", "") + ");");

                goto AddControl;
            }

            // ComboBox ----------------------------------------
            if (strButtonPressed == "btnComboBox")
            {
                intWindowHeight += 50;
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
                myControlEntity.ToolTipx = "If you do not enter keys here, you can use DDLMaint script to create a ddlname in the DDLNAMES table that is the same as the id for the combobox that you are creating. \r\n Then, you add items for the combobox to the DLLItems table using DDLMaint or SSMS ";
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
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorComboBoxToolTip"); ;
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
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorComboBoxDDLName"); ;
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
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorComboBoxColumnSpan"); ;
                myControlEntity.RowNumber = 11;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

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
                myActions.SetValueByKey("ScriptGeneratorComboBoxColumnSpan", strColumnSpan);
                myActions.SetValueByKey("ScriptGeneratorComboBoxToolTip", strToolTip);

                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";                
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);
                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));

                    line = line.Replace("&&DDLNAME", strDDLName.Trim().Replace(" ", ""));
                    line = line.Replace("&&SPACEDOUTID", strID.Trim());
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                    if (strSelectedValue.Trim() == "")
                    {
                        line = line.Replace("\"&&SELECTEDVALUE\"", "myActions.GetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\", \"IdealAutomateDB\");");
                    }
                    else
                    {
                        line = line.Replace("&&SELECTEDVALUE", strSelectedValue.Trim());
                    }

                    if (strKey1.Trim() != "")
                    {
                        line = line.Replace("&&KEY1", strKey1);
                    }
                    if (strValue1.Trim() != "")
                    {
                        line = line.Replace("&&VALUE1", strValue1);
                    }

                    if (strKey2.Trim() != "")
                    {
                        line = line.Replace("&&KEY2", strKey2);
                    }
                    if (strValue1.Trim() != "")
                    {
                        line = line.Replace("&&VALUE2", strValue2);
                    }

                    if (strKey3.Trim() != "")
                    {
                        line = line.Replace("&&KEY3", strKey3);
                    }
                    if (strValue3.Trim() != "")
                    {
                        line = line.Replace("&&VALUE3", strValue3);
                    }

                    if (strKey4.Trim() != "")
                    {
                        line = line.Replace("&&KEY4", strKey4);
                    }
                    if (strValue4.Trim() != "")
                    {
                        line = line.Replace("&&VALUE4", strValue4);
                    }

                    if (strKey5.Trim() != "")
                    {
                        line = line.Replace("&&KEY5", strKey5);
                    }
                    if (strValue5.Trim() != "")
                    {
                        line = line.Replace("&&VALUE5", strValue5);
                    }

                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    if (strWidth != "")
                    {
                        line = line.Replace("&&WIDTH", strWidth);
                    }

                    if (!line.Contains("&&WIDTH") && !line.Contains("&&SELECTEDVALUE") &&
                      !line.Contains("&&KEY1") && !line.Contains("&&VALUE1") &&
                      !line.Contains("&&KEY2") && !line.Contains("&&VALUE2") &&
                      !line.Contains("&&KEY3") && !line.Contains("&&VALUE3") &&
                      !line.Contains("&&KEY4") && !line.Contains("&&VALUE4") &&
                      !line.Contains("&&KEY5") && !line.Contains("&&VALUE5")
                      )
                    {
                        sb.AppendLine(line);
                    }
                }

                sb2.AppendLine("string str" + strID.Replace(" ", "") + " = myListControlEntity" + strSuffix + ".Find(x => x.ID == \"cbx" + strID.Replace(" ", "") + "\").SelectedValue;");
                if (strSelectedValue.Trim() == "")
                {
                    sb2.AppendLine("myActions.SetValueByKey(\"" + myActions.GetValueByKey("ScriptsDefaultKey") + strID.Trim().Replace(" ", "") + "\", str" + strID.Trim().Replace(" ", "") + ");");
                }
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strID.Replace(" ", "") + "\",str" + strID.Replace(" ", "") + ");");

                goto AddControl;
            }

            // Heading -------------------------------------------------
            if (strButtonPressed == "btnHeading")
            {
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
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
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

                goto AddControl;
            }

            ImageEntity myImage = new ImageEntity();
            // Image 
            if (strButtonPressed == "btnImage")
            {
                intRowCtr = 0;
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lblhead";
                myControlEntity.Text = "Image";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lbllabel";
                myControlEntity.Text = "Click okay and use crosshairs to snip image on screen";
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                ++intRowCtr;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "myLabel3";
                myControlEntity.Text = "You can exit the cross hairs screen by hitting escape if you decide not to proceed";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                if (strButtonPressed == "btnCancel")
                {
                    myActions.MessageBoxShow("Okay button not pressed - going to previous menu");
                    goto AddControl;
                }


                // my goal is to have them paste image in popup dialog
                // this image will be saved to a table with fullpathname as key
                // the putall method will start at top again to see if we can use the alternate image from the file
                // At the beginning of putall, we will check to see if there is an alternate image to use from the table

                string directory = AppDomain.CurrentDomain.BaseDirectory;
                SnippingTool.Snip();
                startPoint = SnippingTool._pointStart;
                if (SnippingTool.Image != null)
                {
                    double dblWinHeight = 300;
                    double dblWinWidth = 0;
                    Clipboard.SetImage(BitmapSourceFromImage(SnippingTool.Image));
                    myListControlEntity = new List<ControlEntity>();

                    myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Add Image";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    intRowCtr = 0;

                    intRowCtr++;

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Image;
                    myControlEntity.ID = "myImage";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 2;

                    myImage.ImageFile = "xyz";// strFullFileName;
                    SaveClipboardImageToFile(myImage.ImageFile);
                    byte[] mybytearray = File.ReadAllBytes(myImage.ImageFile);

                    int imageHeight = 0;
                    int imageWidth = 0;
                    using (System.Drawing.Bitmap bm = BytesToBitmap(mybytearray))
                    {
                        imageHeight = bm.Height;
                        imageWidth = bm.Width;
                        myControlEntity.Width = bm.Width;
                        myControlEntity.Height = bm.Height;
                        dblWinHeight += bm.Height;
                        dblWinWidth += bm.Width;
                        myControlEntity.Source = BitmapSourceFromImage(bm);
                    }

                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblFullFileName";
                    myControlEntity.Text = "FullFileName";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtFullFileName";
                    myControlEntity.Text = myActions.GetValueByKey("FullFileName");
                    myControlEntity.ToolTipx = @"Enter full file name to use to save the image. Example, C:\Data\Images\MyImage.png";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myControlEntity.ColumnSpan = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblID";
                    myControlEntity.Text = "ID";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtID";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblToolTip";
                    myControlEntity.Text = "ToolTip";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtToolTip";
                    myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorButtonToolTip"); ;
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Height = 100;
                    myControlEntity.Multiline = true;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblColumnSpan";
                    myControlEntity.Text = "ColumnSpan:";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtColumnSpan";
                    myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorButtonColumnSpan"); ;
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());




                    if (dblWinHeight > 750)
                    {
                        dblWinHeight = 750;
                    }
                    if (dblWinWidth > 1000)
                    {
                        dblWinWidth = 1000;
                    }
                    if (dblWinWidth < 500)
                    {
                        dblWinWidth = 500;
                    }
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, (int)dblWinHeight, (int)dblWinWidth, 0, 0);



                    string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;

                    string strToolTip = myListControlEntity.Find(x => x.ID == "txtToolTip").Text;
                    string strColumnSpan = myListControlEntity.Find(x => x.ID == "txtColumnSpan").Text;
                    myActions.SetValueByKey("ScriptGeneratorButtonColumnSpan", strColumnSpan);
                    myActions.SetValueByKey("ScriptGeneratorButtonToolTip", strToolTip);
                    if (strButtonPressed == "btnCancel")
                    {
                        myActions.MessageBoxShow("Okay button not pressed - going to previous menu");
                        goto AddControl;
                    }

                    string strFullFileName = myListControlEntity.Find(x => x.ID == "txtFullFileName").Text;
                    myActions.SetValueByKey("FullFileName", strFullFileName);
                    SaveClipboardImageToFile(strFullFileName);
                    string strInFile = strApplicationPath + "TemplateImage.txt";
                    List<string> listOfSolvedProblems = new List<string>();
                    List<string> listofRecs = new List<string>();
                    string[] lineszz = System.IO.File.ReadAllLines(strInFile);
                    intWindowHeight += imageHeight;
                    int windowWidthx = imageWidth;
                    if (windowWidthx > windowWidth)
                    {
                        windowWidth = windowWidthx;
                    }


                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                        string line = lineszz[i];
                        line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                        line = line.Replace("&&SUFFIX", strSuffix.Trim());
                        line = line.Replace("&&FULLFILENAME", strFullFileName.Trim());
                        int intColumnSpan = 1;
                        Int32.TryParse(strColumnSpan, out intColumnSpan);
                        line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                        line = line.Replace("&&TOOLTIP", strToolTip.Trim());
                        line = line.Replace("&&ROW", intRowCtr.ToString());
                        line = line.Replace("&&HEIGHT", imageHeight.ToString());
                        line = line.Replace("&&WIDTH", imageWidth.ToString());
                        sb.AppendLine(line);

                    }


                }



                goto AddControl;
            }
            // Iterator ----------------------------------------
            if (strButtonPressed == "btnIterator")
            {
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
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    if (line.Contains("&&STARTROW"))
                    {
                        line = line.Replace("&&STARTROW", intRowCtr.ToString());
                        intRowCtr++;
                    }
                    if (line.Contains("&&INCREMENTBYROW"))
                    {
                        line = line.Replace("&&INCREMENTBYROW", intRowCtr.ToString());
                        intRowCtr++;
                    }
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strIteratorID.Trim().Replace(" ", ""));
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
            if (strButtonPressed == "btnNumberOfIterations")
            {
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
                for (int i = 0; i < intLineCount; i++)
                {
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
            if (strButtonPressed == "btnTemplate")
            {
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
                for (int i = 0; i < intLineCount; i++)
                {
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
            if (strButtonPressed == "btnButton")
            {
                intWindowHeight += 50;
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
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorButtonToolTip"); ;
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
                myControlEntity.Text = myActions.GetValueByKey("ScriptGeneratorButtonColumnSpan"); ;
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
                myActions.SetValueByKey("ScriptGeneratorButtonColumnSpan", strColumnSpan);
                myActions.SetValueByKey("ScriptGeneratorButtonToolTip", strToolTip);
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                int intLineCount = lineszz.Count();
                int intCtr = 0;
                for (int i = 0; i < intLineCount; i++)
                {
                    string line = lineszz[i];
                    line = line.Replace("&&ID", strID.Trim().Replace(" ", ""));
                    line = line.Replace("&&SUFFIX", strSuffix.Trim());
                    line = line.Replace("&&TEXT", strText.Trim());
                    int intColumnSpan = 1;
                    Int32.TryParse(strColumnSpan, out intColumnSpan);
                    line = line.Replace("&&COLUMNSPAN", intColumnSpan.ToString());
                    line = line.Replace("&&TOOLTIP", strToolTip.Trim());
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

                goto AddControl;
            }


            // Add Position for Popup 
            if (strButtonPressed == "btnPositionPopup")
            {
                intRowCtr = 0;
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lblhead";
                myControlEntity.Text = "Position Popup";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lbllabel";
                myControlEntity.Text = "Absolute positioning allows you to use crosshairs to specify absolute position on screen. .";
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lbllabel2";
                myControlEntity.Text = "Relative position allows you to snip an image and position popup relative to image.";
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblPositionType";
                myControlEntity.Text = "Position Type";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("Absolute", "Absolute"));
                cbp.Add(new ComboBoxPair("Relative to Image", "Relative"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                myControlEntity.SelectedValue = "Absolute";
                myControlEntity.ID = "cbxPositionType";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "Absolute positioning allows you to use crosshairs to specify absolute position on screen. Relative position allows you to snip an image and position popup relative to image.";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblBalloonArrowDirection";
                myControlEntity.Text = "Balloon Arrow Direction";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1.Clear();
                cbp1.Add(new ComboBoxPair("NONE", "NONE"));
                cbp1.Add(new ComboBoxPair("DEFAULT", "DEFAULT"));
                cbp1.Add(new ComboBoxPair("TOP_LEFT", "TOP_LEFT"));
                cbp1.Add(new ComboBoxPair("TOP_RIGHT", "TOP_RIGHT"));
                cbp1.Add(new ComboBoxPair("BOTTOM_LEFT", "BOTTOM_LEFT"));
                cbp1.Add(new ComboBoxPair("BOTTOM_RIGHT", "BOTTOM_RIGHT"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                myControlEntity.SelectedValue = myActions.GetValueByKey("strBalloonArrowDirection");
                if (myControlEntity.SelectedValue == "")
                {
                    myControlEntity.SelectedValue = "DEFAULT";
                }
                myControlEntity.ID = "cbxBalloonArrowDirection";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "Specify location of arrow on balloon";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                ++intRowCtr;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "myLabel3";
                myControlEntity.Text = "You can exit the cross hairs screen by hitting escape if you decide not to proceed";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                if (strButtonPressed == "btnCancel")
                {
                    myActions.MessageBoxShow("Okay button not pressed - going to previous menu");
                    goto AddControl;
                }

                string strPositionType = myListControlEntity.Find(x => x.ID == "cbxPositionType").SelectedValue;
                strBalloonArrowDirection = myListControlEntity.Find(x => x.ID == "cbxBalloonArrowDirection").SelectedValue;
                myActions.SetValueByKey("strBalloonArrowDirection", strBalloonArrowDirection);
                if (strPositionType == "Absolute")
                {
                    SnippingTool.Snip();
                    startPoint = SnippingTool._pointStart;
                    _PositionType = "Absolute";
                    goto AddControl;
                }

                // my goal is to have them paste image in popup dialog
                // this image will be saved to a table with fullpathname as key
                // the putall method will start at top again to see if we can use the alternate image from the file
                // At the beginning of putall, we will check to see if there is an alternate image to use from the table

                string directory = AppDomain.CurrentDomain.BaseDirectory;
                SnippingTool.Snip();
                startPoint = SnippingTool._pointStart;
                if (SnippingTool.Image != null)
                {
                    double dblWinHeight = 300;
                    double dblWinWidth = 0;
                    Clipboard.SetImage(BitmapSourceFromImage(SnippingTool.Image));
                    myListControlEntity = new List<ControlEntity>();

                    myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Position Popup Relative to Image";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    intRowCtr = 0;

                    intRowCtr++;

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Image;
                    myControlEntity.ID = "myImage";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 2;
                    myImage.ImageFile = "xyz";// strFullFileName;
                    SaveClipboardImageToFile(myImage.ImageFile);
                    byte[] mybytearray = File.ReadAllBytes(myImage.ImageFile);
                    System.Drawing.Bitmap bm = BytesToBitmap(mybytearray);
                    myControlEntity.Width = bm.Width;
                    myControlEntity.Height = bm.Height;
                    dblWinHeight += bm.Height;
                    dblWinWidth += bm.Width;
                    myControlEntity.Source = BitmapSourceFromImage(bm);
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblFullFileName";
                    myControlEntity.Text = "FullFileName";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtFullFileName";
                    myControlEntity.Text = myActions.GetValueByKey("FullFileName");
                    myControlEntity.ToolTipx = @"Enter full file name to use to save the image. Example, C:\Data\Images\MyImage.png";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myControlEntity.ColumnSpan = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblTop";
                    myControlEntity.Text = "Top Offset";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtTop";
                    myControlEntity.Text = myActions.GetValueByKey("Top"); ;
                    myControlEntity.ToolTipx = "Top offset";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myControlEntity.ColumnSpan = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblLeft";
                    myControlEntity.Text = "Left Offset";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtLeft";
                    myControlEntity.Text = myActions.GetValueByKey("Left"); ;
                    myControlEntity.ToolTipx = "Left offset";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myControlEntity.ColumnSpan = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    if (dblWinHeight > 750)
                    {
                        dblWinHeight = 750;
                    }
                    if (dblWinWidth > 1000)
                    {
                        dblWinWidth = 1000;
                    }
                    if (dblWinWidth < 500)
                    {
                        dblWinWidth = 500;
                    }
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, (int)dblWinHeight, (int)dblWinWidth, 0, 0);
                    if (strButtonPressed == "btnCancel")
                    {
                        myActions.MessageBoxShow("Okay button not pressed - going to previous menu");
                        goto AddControl;
                    }

                    string strFullFileName = myListControlEntity.Find(x => x.ID == "txtFullFileName").Text;
                    myActions.SetValueByKey("FullFileName", strFullFileName);
                    string strTop = myListControlEntity.Find(x => x.ID == "txtTop").Text;
                    myActions.SetValueByKey("Top", strTop);
                    string strLeft = myListControlEntity.Find(x => x.ID == "txtLeft").Text;
                    myActions.SetValueByKey("Left", strLeft);
                    SaveClipboardImageToFile(strFullFileName);
                    _PositionType = "Relative";
                    _RelativeFullFileName = strFullFileName;
                    _RelativeLeft = strLeft;
                    _RelativeTop = strTop;
                }

                //    AbsolutePosition(ref myControlEntity, ref myListControlEntity);


                goto AddControl;
            }

            if (strButtonPressed == "btnDisplay")
            {

                if (intWindowHeight > 700)
                {
                    intWindowHeight = 700;
                }
                int startPointXTemp = startPoint.X;
                int startPointYTemp = startPoint.Y;
                double windowWidthTemp = windowWidth * 2;
                if (windowWidthTemp > 1000)
                { windowWidthTemp = 1000; }
                if (windowWidthTemp < 500)
                { windowWidthTemp = 500; }
                if (_PositionType == "Relative")
                {
                    startPointXTemp = 0;
                    startPointYTemp = 0;
                }
                else
                {
                    startPointXTemp = startPoint.X;
                    startPointYTemp = startPoint.Y;
                }
                // http://www.codeproject.com/Tips/715891/Compiling-Csharp-Code-at-Runtime
                string code = "";
                relativeCodeSnippet = "     myImage = new ImageEntity(); \r\n" +
    " \r\n" +
    "    \r\n" +
    "        myImage.ImageFile = @\"" + _RelativeFullFileName + "\"; \r\n" +
    "      " +
    "      myImage.Sleep = 1000;  \r\n" +
    "      myImage.Attempts = 1;  \r\n" +
    "      myImage.RelativeX = 0;   \r\n" +
    "      myImage.RelativeY = 0;   \r\n" +
    "  \r\n" +
    "      resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);  \r\n" +
    "            newTop = 0;  \r\n" +
    "            newLeft = 0;  \r\n" +
    "      if (resultArray.Length == 0) {  \r\n" +
    "     List<ControlEntity> myListControlEntityBackup = myListControlEntity; \r\n" +
                "     myListControlEntity = new List<ControlEntity>(); \r\n" +
" \r\n" +
"                 myControlEntity = new ControlEntity(); \r\n" +
"                myControlEntity.ControlEntitySetDefaults(); \r\n" +
"                myControlEntity.ControlType = ControlType.Heading; \r\n" +
"                myControlEntity.Text = \"Image Not Found\"; \r\n" +
"                myListControlEntity.Add(myControlEntity.CreateControlEntity()); \r\n" +
" \r\n" +
" \r\n" +
"                myControlEntity.ControlEntitySetDefaults(); \r\n" +
"                myControlEntity.ControlType = ControlType.Label; \r\n" +
"                myControlEntity.ID = \"myLabel\"; \r\n" +
"                myControlEntity.Text = \"I could not find image to position popup relative to \"; \r\n" +
"                myControlEntity.RowNumber = 0; \r\n" +
"                myControlEntity.ColumnNumber = 0; \r\n" +
"                myListControlEntity.Add(myControlEntity.CreateControlEntity()); \r\n" +
" \r\n" +
"                myControlEntity.ControlEntitySetDefaults(); \r\n" +
"                myControlEntity.ControlType = ControlType.Label; \r\n" +
"                myControlEntity.ID = \"myLabel\"; \r\n" +
"                myControlEntity.Text = \"Here is what that image looks like:\"; \r\n" +
"                myControlEntity.RowNumber = 2; \r\n" +
"                myControlEntity.ColumnNumber = 0; \r\n" +
"                myListControlEntity.Add(myControlEntity.CreateControlEntity()); \r\n" +
" \r\n" +
"                myControlEntity.ControlEntitySetDefaults(); \r\n" +
" \r\n" +
"                mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile); \r\n" +
"                bm = BytesToBitmap(mybytearray); \r\n" +
"                myControlEntity.Width = bm.Width; \r\n" +
"                myControlEntity.Height = bm.Height; \r\n" +
"                myControlEntity.Source = BitmapSourceFromImage(bm); \r\n" +
" \r\n" +
" \r\n" +
"                myControlEntity.ControlType = ControlType.Image; \r\n" +
"                myControlEntity.ID = \"myImage\"; \r\n" +
"                myControlEntity.RowNumber = 4; \r\n" +
"                myControlEntity.ColumnNumber = 0; \r\n" +
" \r\n" +
"                myListControlEntity.Add(myControlEntity.CreateControlEntity()); \r\n" +
" \r\n" +
"                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0); \r\n" +
"                myListControlEntity = myListControlEntityBackup; \r\n" +

                "      }  else  \r\n" +
    "       {  \r\n" +
    "            newLeft = resultArray[0,0];   \r\n" +
    "            newTop = resultArray[0, 1];   \r\n" +
    "            int intRelativeTop = 0;   \r\n" +
    "            int intRelativeLeft = 0;  \r\n" +
    "            Int32.TryParse(\"" + _RelativeTop + "\", out intRelativeTop);   \r\n" +
    "            Int32.TryParse(\"" + _RelativeLeft + "\", out intRelativeLeft);   \r\n" +
    "            newTop += intRelativeTop;   \r\n" +
    "            newLeft += intRelativeLeft;   \r\n" +
    "      }     \r\n";

                // relative means you need code to find image on screen, which is in relativeCodeSnippet
                StringBuilder sbCode = new StringBuilder();
                string strInFile = strApplicationPath + "TemplateCode1Usings.txt";
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);
                foreach (var line in lineszz)
                {
                    sbCode.AppendLine(line);
                }

                strInFile = strApplicationPath + "TemplateCode2NamespaceClass.txt";
                lineszz = System.IO.File.ReadAllLines(strInFile);
                foreach (var line in lineszz)
                {
                    sbCode.AppendLine(line);
                }

                strInFile = strApplicationPath + "TemplateCode3Globals.txt";
                lineszz = System.IO.File.ReadAllLines(strInFile);
                foreach (var line in lineszz)
                {
                    sbCode.AppendLine(line);
                }

                strInFile = strApplicationPath + "TemplateCode4Main.txt";
                lineszz = System.IO.File.ReadAllLines(strInFile);
                foreach (var line in lineszz)
                {
                    sbCode.AppendLine(line);
                }
                if (_PositionType == "Relative")
                {
                    code = sbCode.ToString() +
                    sb.ToString() +
                    " myControlEntity" + strSuffix + " = new ControlEntity();" +
                                  relativeCodeSnippet + " strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + " , " + intWindowHeight + ", " + windowWidthTemp + ", newTop,  newLeft, \"" + strBalloonArrowDirection + "\");" + sb2.ToString() +
                   "Console.WriteLine(\"Hello, world!\");"
                   + @"
            }"
                    + sbFunctions.ToString() + @"
        }
    }
";
                }
                else
                {
                    code = sbCode.ToString() +
 sb.ToString() +
              " strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + " , " + intWindowHeight + ", " + windowWidthTemp + "," + startPointYTemp + ", " + startPointXTemp + ", \"" + strBalloonArrowDirection + "\");" + sb2.ToString() +
 "Console.WriteLine(\"Hello, world!\");"
                   + @"
            }"
                    + sbFunctions.ToString() + @"
        }
    }
";
                }

                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();
                // Reference to System.Drawing library
                parameters.ReferencedAssemblies.Add("System.Drawing.dll");
                parameters.ReferencedAssemblies.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"IdealAutomate\IdealAutomateCore.dll"));
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
                if (results.Errors.HasErrors)
                {
                    StringBuilder sb5 = new StringBuilder();

                    foreach (CompilerError error in results.Errors)
                    {
                        sb5.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    }

                    myActions.MessageBoxShow(sb5.ToString());
                }
                Assembly assembly = results.CompiledAssembly;
                Type program = assembly.GetType("First.Program");
                MethodInfo main = program.GetMethod("Main");
                main.Invoke(null, null);
                if (_PositionType == "Relative")
                {
                    myActions.MessageBoxShow(sb.ToString() + relativeCodeSnippet + " strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + " , " + intWindowHeight + ", " + windowWidthTemp + ", newTop, newLeft, \"" + strBalloonArrowDirection + "\");" + sb2.ToString());
                }
                else
                {
                    myActions.MessageBoxShow(sb.ToString() + " strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + " , " + intWindowHeight + ", " + windowWidthTemp + "," + startPoint.Y + ", " + startPoint.X + ", \"" + strBalloonArrowDirection + "\"); " + sb2.ToString());
                }
                goto AddControl;
            }

            System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("Click Yes to Confirm or No to Continue");
            if (myResult == System.Windows.Forms.DialogResult.No)
            {
                goto AddControl;
            }
            if (strButtonPressed == "btnCancel")
            {
                goto myExit;
            }
            #endregion AddControlsWindow
            // Done --------------------
            if (intWindowHeight > 700)
            {
                intWindowHeight = 700;
            }

           

            // ============

            myActions.Write1UsingsTemplateToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

            myActions.Write2NameSpaceClassTemplateToExternalFile(strApplicationPath);

            myActions.Write3GlobalsToExternalFile(strApplicationPath, strAppendCodeToExistingFile);

            myActions.Write4MainTemplateToExternalFile(strApplicationPath);

            string strOutCodeBodyFile = @"C:\Data\CodeBody.txt";
            WriteCodeBodyToExternalFile(strMinimized, _PositionType, relativeCodeSnippet, strSuffix, sb, sb2, strAppendCodeToExistingFile, strOutCodeBodyFile);

            myActions.Write5FunctionsTemplateToExternalFile(strApplicationPath);

            myActions.WriteCodeEndToExternalFile();
         
            string strOutCodeBigFile = myActions.WriteCodeBigExternalFile(strOutCodeBodyFile);

            //============

            string strExecutable = @"C:\Windows\system32\notepad.exe";
            string strContent = strOutCodeBigFile;
            Process.Start(strExecutable, string.Concat("", strContent, ""));        


            goto myExit;
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }

        private void WriteCodeBodyToExternalFile(string strMinimized, string _PositionType, string relativeCodeSnippet, string strSuffix, StringBuilder sb, StringBuilder sb2, string strAppendCodeToExistingFile, string strOutCodeBodyFile)
        {
            string strOutCodeBodyFileBackup = @"C:\Data\CodeBodyBackup.txt";
            File.Copy(strOutCodeBodyFile, strOutCodeBodyFileBackup, true);
            if (strAppendCodeToExistingFile.ToLower() == "true")
            {
                using (System.IO.StreamWriter file = System.IO.File.AppendText(strOutCodeBodyFile))
                {
                    file.Write(sb.ToString());
                    if (_PositionType == "Relative")
                    {
                        file.Write(relativeCodeSnippet);
                    }

                    double windowWidthTemp = windowWidth * 2;
                    if (windowWidthTemp > 1000)
                    { windowWidthTemp = 1000; }
                    if (windowWidthTemp < 500)
                    { windowWidthTemp = 500; }
                    if (_PositionType == "Relative")
                    {
                        file.WriteLine("strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + ", " + intWindowHeight + ", " + windowWidthTemp + ", newTop, newLeft, \"" + strBalloonArrowDirection + "\");");
                    }
                    else
                    {
                        file.WriteLine("strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + ", " + intWindowHeight + ", " + windowWidthTemp + "," + startPoint.Y + ", " + startPoint.X + ", \"" + strBalloonArrowDirection + "\"); ");
                    }
                    file.WriteLine("if (strButtonPressed == \"btnCancel\") {");
                    file.WriteLine("  myActions.MessageBoxShow(\"Okay button not pressed - Script Cancelled\");");
                    file.WriteLine("  goto myExit;");
                    file.WriteLine("}");
                    file.Write(sb2.ToString());
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutCodeBodyFile))
                {
                    file.Write(sb.ToString());
                    if (_PositionType == "Relative")
                    {
                        file.Write(relativeCodeSnippet);
                    }

                    double windowWidthTemp = windowWidth * 2;
                    if (windowWidthTemp > 1000)
                    { windowWidthTemp = 1000; }
                    if (windowWidthTemp < 500)
                    { windowWidthTemp = 500; }
                    if (_PositionType == "Relative")
                    {
                        file.WriteLine("strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + ", " + intWindowHeight + ", " + windowWidthTemp + ", newTop, newLeft, \"" + strBalloonArrowDirection + "\");");
                    }
                    else
                    {
                        file.WriteLine("strButtonPressed = myActions.WindowBalloonMultipleControls" + strMinimized + "(ref myListControlEntity" + strSuffix + ", " + intWindowHeight + ", " + windowWidthTemp + "," + startPoint.Y + ", " + startPoint.X + ", \"" + strBalloonArrowDirection + "\"); ");
                    }
                    file.WriteLine("if (strButtonPressed == \"btnCancel\") {");
                    file.WriteLine("  myActions.MessageBoxShow(\"Okay button not pressed - Script Cancelled\");");
                    file.WriteLine("  goto myExit;");
                    file.WriteLine("}");

                    file.Write(sb2.ToString());
                }
            }
        }







        private void FindWidthInCharsForContent(List<string> myContentList)
        {
            foreach (var myContent in myContentList)
            {
                var textArr = myContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //     txtline.Text = textArr.Length.ToString();

                foreach (var item in textArr)
                {
                    if (item.Length > _maxLineChars)
                    {
                        _maxLineChars = item.Length;
                    }
                }
            }

            if (_maxLineChars > 80)
            {
                _maxLineChars = 80;
            }
        }
        private double CalculateStringHeight(string myContent, int controlWidthInChars)
        {
            double dblHeight = 0;
            int intCtr = 0;
            //  int intLineWidthInCharacters = 40;
            int intLineHeight = 25;
            int textLength = myContent.Length;
            int intTextBoxHeight = 0;
            if (textLength > 0)
            {
                //var lines = tb.Lines.Count();               
                var textArr = myContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //     txtline.Text = textArr.Length.ToString();
                int totalNumberOfLines = 0;
                int numberOfLines = 0;
                foreach (var item in textArr)
                {
                    numberOfLines = item.Length / controlWidthInChars;
                    if (numberOfLines < 1)
                    {
                        numberOfLines = 1;
                    }
                    totalNumberOfLines += numberOfLines;

                }

                intTextBoxHeight = totalNumberOfLines * intLineHeight;

            }
            if (intTextBoxHeight < 29)
            {
                intTextBoxHeight = 30;
            }
            if (intTextBoxHeight > 700 - intWindowHeight)
            {
                intTextBoxHeight = 700 - intWindowHeight;
            }
            dblHeight = intTextBoxHeight;
            return dblHeight;
        }
        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft)
        {
            strWindowLeft = myActions.GetValueByKeyGlobal("WindowLeft");
            strWindowTop = myActions.GetValueByKeyGlobal("WindowTop");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
        }

        private static BitmapSource BitmapSourceFromImage(System.Drawing.Image img)
        {
            MemoryStream memStream = new MemoryStream();

            // save the image to memStream as a png
            img.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);

            // gets a decoder from this stream
            System.Windows.Media.Imaging.PngBitmapDecoder decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(memStream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default);

            return decoder.Frames[0];
        }
        private static System.Drawing.Bitmap BytesToBitmap(byte[] byteArray)
        {


            using (MemoryStream ms = new MemoryStream(byteArray))
            {


                System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);


                return img;


            }
        }
        private static void SaveClipboardImageToFile(string filePath)
        {
            var image = Clipboard.GetImage();
            if (image == null)
            {
                MessageBoxResult result = MessageBox.Show("Clipboard Cannot Be Empty", "PopUp Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {

                }
            }
            else
            {
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(fileStream);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.InnerException == null
             ? ex.Message
             : ex.Message + " --> " + ex.InnerException.ToString());
                }
            }
        }
    }
}