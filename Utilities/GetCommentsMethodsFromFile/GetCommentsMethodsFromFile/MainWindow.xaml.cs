using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections;
using System.Windows.Controls;

namespace GetCommentsMethodsFromFile {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        bool boolNamespace = false;
        bool boolCommentsContinued = false;
        bool boolFirstMethodFound = false;
        bool boolFirstStatementInMethod = true;
        bool boolLineProcessed;
        bool boolMethod = false;
        bool boolClass = false;
        bool boolMultipleStatementsPossible = false;
        bool boolFoundOpenBracket = false;
        bool boolTraceLoggingOn = true;
        string strMethod = "";
        string strMethodOrig = "";
        string strParameters = "";
        string strCleaned = "";
        List<string> listReservedWords = new List<string>();
        List<string> listTypeWords = new List<string>();
        List<string> listOutput = new List<string>();
        List<string> listParameters = new List<string>();
        string myFile = "";
        string strClass = "";

        string strCleanedHold;
        string strInputLineCleaned;
        string strInputLineOrig;

        string strTraceOnFile;
        string[] strGlobalVariableArray;
        string[] strLocalVariableArray;
        string[] strLocalVariableTypeArray;
        System.IO.StreamWriter outputfile;
        int idxGlobalVariable = 0;
        int idxLocalVariable = 0;
        int idxLocalVariableNesting = 0;
        int intBegin = 0;
        int intEnd = 0;
        int intInLine = 0;
        int intLit = 0;
        int intNestingLevel = 0;
        int intClassLevel = 0;
        int intMethodLevel = 1;
        int intMethodCtr = 0;
        int[] strLocalVariableNestingArray;
        List<string> listComments = new List<string>();
        List<string> listMethod = new List<string>();
        SortedList<string, string> listCategoryMethod = new SortedList<string, string>();

        string strInitialDirectory = "";
        int intRowCtr = 0;
        List<ControlEntity> myListControlEntity = new List<ControlEntity>();
        ControlEntity myControlEntity = new ControlEntity();
        int intCol = 0;
        int intRow = 0;
        int intStartPageMethod = 0;
        int intEndPageMethod = 0;
        int intTotalMethods = 0;
        int intPageSize = 80;
        int intColumnWidth = 250;
        string strPreviousCategory = "";
        string strApplicationBinDebug = "";
        string myNewProjectSourcePath = "";
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
            if (strWindowTitle.StartsWith("GetCommentsMethodsFromFile")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            string strApplicationBinDebug1 = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath1 = strApplicationBinDebug1.Replace("\\bin\\Debug", "");
      


            System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(myNewProjectSourcePath1, @"Text"));

            foreach (FileInfo file in di.GetFiles()) {
                if (file.Name != "Reserved_Words_CS.txt" && file.Name != "Type_Words_CS.txt") {
                    file.Delete();
                }
            }
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            DisplaySelectFolderWindow:
            intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp2 = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp3 = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "Open Folder";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblFolder";
            myControlEntity.Text = "Folder";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFolderSelectedValue");
            myControlEntity.ID = "cbxFolder";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = @"Here is an example: C:\Users\harve\Documents\GitHub";
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSelectFolder";
            myControlEntity.Text = "Select Folder...";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 3;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            DisplaySelectFolderWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }


            string strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);

            if (strButtonPressed == "btnSelectFolder") {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");


                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && Directory.Exists(dialog.SelectedPath)) {
                    myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue = dialog.SelectedPath;
                    myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey = dialog.SelectedPath;
                    myListControlEntity.Find(x => x.ID == "cbxFolder").Text = dialog.SelectedPath;

                    myActions.SetValueByKey("LastSearchFolder", dialog.SelectedPath);
                    strFolder = dialog.SelectedPath;
                    myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
                    string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                    string fileName = "cbxFolder.txt";


                    string settingsDirectory = GetAppDirectoryForScript(myActions.ConvertFullFileNameToScriptPath(myNewProjectSourcePath));
                    string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
                    ArrayList alHosts = new ArrayList();
                    cbp = new List<ComboBoxPair>();
                    cbp.Clear();
                    cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
                    ComboBox myComboBox = new ComboBox();


                    if (!File.Exists(settingsPath)) {
                        using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                            objSWFile.Close();
                        }
                    }
                    using (StreamReader objSRFile = File.OpenText(settingsPath)) {
                        string strReadLine = "";
                        while ((strReadLine = objSRFile.ReadLine()) != null) {
                            string[] keyvalue = strReadLine.Split('^');
                            if (keyvalue[0] != "--Select Item ---") {
                                cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));
                            }
                        }
                        objSRFile.Close();
                    }
                    string strNewHostName = dialog.SelectedPath;
                    List<ComboBoxPair> alHostx = cbp;
                    List<ComboBoxPair> alHostsNew = new List<ComboBoxPair>();
                    ComboBoxPair myCbp = new ComboBoxPair(strNewHostName, strNewHostName);
                    bool boolNewItem = false;

                    alHostsNew.Add(myCbp);
                    if (alHostx.Count > 24) {
                        for (int i = alHostx.Count - 1; i > 0; i--) {
                            if (alHostx[i]._Key.Trim() != "--Select Item ---") {
                                alHostx.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    foreach (ComboBoxPair item in alHostx) {
                        if (strNewHostName.ToLower() != item._Key.ToLower() && item._Key != "--Select Item ---") {
                            boolNewItem = true;
                            alHostsNew.Add(item);
                        }
                    }

                    using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                        foreach (ComboBoxPair item in alHostsNew) {
                            if (item._Key != "") {
                                objSWFile.WriteLine(item._Key + '^' + item._Value);
                            }
                        }
                        objSWFile.Close();
                    }
                    goto DisplaySelectFolderWindowAgain;
                }
            }

            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {

                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplaySelectFolderWindow;
                }

                strFolderToUse = strFolder;
                myActions.SetValueByKey("InitialDirectory", strFolder);              
               
            }
                                  
            // Read in lines from ReservedWords txt file.
             strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
             myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            foreach (string line in File.ReadLines(Path.Combine(myNewProjectSourcePath, @"Text\Reserved_Words_CS.txt"))) {
                listReservedWords.Add(line.Trim());
            }

            // Read in lines from TypeWords_CS txt file.
            foreach (string line in File.ReadLines(Path.Combine(myNewProjectSourcePath, @"Text\Type_Words_CS.txt"))) {
                listTypeWords.Add(line.Trim());
            }

            // Read in lines from code file. We read until we get a clean line of code and
            // then process it.
            // We read each line and do the following in order to clean the input:
            /*
             * 1. Remove comments
             * 1.5 Replace verbatim strings with #Literalx
             * 2. Replace quoted strings with #Literalx, 
             *    where x is a number - these literals are stored in a dict
             * 3. If it is a statement or condition that continues 
             *    for more than one line, we put it on one line
             * 4. We put each bracket on a line by itself so 
             *    we can tell what level of nesting we are at
             * 5. We put each action statement on a line by itself
             * 
             * Once the input has been cleaned, here is what we do:
             * 1. If we are writing the cleaned statement to the file, 
             *    we need to replace the #Literalx with dict
             * 2. If the cleaned statement is not in an executable 
             *    part of the code (using, namespace, class),
             *    we do not write a tracelog statement. 
             * 3. If the cleaned statement is a method statement, 
             *    we write the tracelog after writing the method statement.
             * 4. If the cleaned statement is a conditional statement, 
             *    we write the tracelog before writing the conditional statement.
             * 5. If the cleaned statement is an action statement, 
             *    we write the tracelog after writing the action statement
             */
            string[] files = null;
            try {
                files = System.IO.Directory.GetFiles(strFolder, "*.cs",SearchOption.AllDirectories);
            } catch (UnauthorizedAccessException e) {

                Console.WriteLine(e.Message);                
            } catch (System.IO.DirectoryNotFoundException e) {
                Console.WriteLine(e.Message);                
            } catch (System.IO.PathTooLongException e) {
                Console.WriteLine(e.Message);                
            } catch (Exception e) {
                Console.WriteLine(e.Message);                
            }

            // Perform the required action on each file here.
            // Modify this block to perform your required task.
            foreach (string myFile in files) {
                  //  myFile = @"C:\Users\harve\Documents\GitHub\IdealAutomate\IdealAutomateCore\IdealAutomateCore\Methods.cs";
            System.IO.StreamReader file =
               new System.IO.StreamReader(myFile);
            while ((strInputLineOrig = file.ReadLine()) != null) {
                intInLine = intInLine + 1;
                intBegin = intInLine;
                strInputLineCleaned = "";
                strInputLineCleaned = RemoveLiteralsComments(strInputLineOrig);
                // we need to check for continued lines and put them all on the same cleaned line
                if (strInputLineCleaned.StartsWith("namespace ")) {
                    while (file.EndOfStream == false && strInputLineCleaned.IndexOf("{") == -1) {
                        ReadCleanConcatCodeRec(file);
                    }
                } else if (strInputLineCleaned.StartsWith("class ") || strInputLineCleaned.Contains(" class ")) {
                    while (file.EndOfStream == false && strInputLineCleaned.IndexOf("{") == -1) {
                        ReadCleanConcatCodeRec(file);
                    }
                } else if ((strInputLineCleaned.StartsWith("public ") ||
                          strInputLineCleaned.StartsWith("private ") ||
                      strInputLineCleaned.StartsWith("static ") ||
                      strInputLineCleaned.StartsWith("protected ") ||
                      strInputLineCleaned.StartsWith("internal ")) &&
                      strInputLineCleaned.IndexOf("(") > -1) {
                    while (file.EndOfStream == false && strInputLineCleaned.IndexOf("{") == -1) {
                        ReadCleanConcatCodeRec(file);
                    }

                } //switch, throw, try, catch, finally, do, for, foreach, while
                  else if ((strInputLineCleaned.StartsWith("if ") ||
                            strInputLineCleaned.StartsWith("else ") ||
                          strInputLineCleaned.StartsWith("switch ") ||
                          strInputLineCleaned.StartsWith("throw ") ||
                          strInputLineCleaned.StartsWith("try ") ||
                          strInputLineCleaned.StartsWith("catch ") ||
                          strInputLineCleaned.StartsWith("finally ") ||
                          strInputLineCleaned.StartsWith("do ") ||
                          strInputLineCleaned.StartsWith("for ") ||
                          strInputLineCleaned.StartsWith("foreach ") ||
                          strInputLineCleaned.StartsWith("foreach(") ||
                          strInputLineCleaned.StartsWith("while ") ||
                          strInputLineCleaned.Contains(" if ") ||
                          strInputLineCleaned.Contains(" else ") ||
                          strInputLineCleaned.Contains(" switch ") ||
                          strInputLineCleaned.Contains(" throw ") ||
                          strInputLineCleaned.Contains(" try ") ||
                          strInputLineCleaned.Contains(" catch ") ||
                          strInputLineCleaned.Contains(" finally ") ||
                          strInputLineCleaned.Contains(" do ") ||
                          strInputLineCleaned.Contains(" for ") ||
                          strInputLineCleaned.Contains(" foreach ") ||
                          strInputLineCleaned.Contains(" foreach(") ||
                          strInputLineCleaned.Contains(" while ") ||
                          strInputLineCleaned.Contains(";if ") ||
                          strInputLineCleaned.Contains(";else ") ||
                          strInputLineCleaned.Contains(";switch ") ||
                          strInputLineCleaned.Contains(";throw ") ||
                          strInputLineCleaned.Contains(";try ") ||
                          strInputLineCleaned.Contains(";catch ") ||
                          strInputLineCleaned.Contains(";finally ") ||
                          strInputLineCleaned.Contains(";do ") ||
                          strInputLineCleaned.Contains(";for ") ||
                          strInputLineCleaned.Contains(";foreach ") ||
                          strInputLineCleaned.Contains(";while ")) &&
                          strInputLineCleaned.IndexOf("(") > -1) {
                    //we need to continue until we find and equal number of opening and closing
                    // parentheses
                    int intParens = CountParens(strInputLineCleaned);

                    while (file.EndOfStream == false && intParens != 0) {
                        ReadCleanConcatCodeRec(file);
                        intParens = CountParens(strInputLineCleaned);
                    }

                } else if (strInputLineCleaned != "") {
                    Regex rgx = new Regex(@"#Comment(\d+)>>");
                    while (file.EndOfStream == false
                        && strInputLineCleaned.IndexOf(";") == -1
                         && strInputLineCleaned.IndexOf("{") == -1
                          && strInputLineCleaned.IndexOf("}") == -1
                           && strInputLineCleaned.IndexOf(":") == -1
                           && rgx.IsMatch(strInputLineCleaned) == false
                        ) {
                        ReadCleanConcatCodeRec(file);
                    }
                }
                intEnd = intInLine;

                // if input lines were continued, they are all on one line in strInputLineCleaned now
                //
                // now we need to see if cleaned contains any brackets and put each bracket
                // on a line by itself.

                int intOpenBracket = strInputLineCleaned.IndexOf('{');
                int intClosedBracket = strInputLineCleaned.IndexOf('}');
                int intSemicolon = strInputLineCleaned.IndexOf(';');
                int intMin = 999999;
                int intLastBracket = -1;
                if (intOpenBracket == -1) intOpenBracket = 999999;
                if (intClosedBracket == -1) intClosedBracket = 999999;
                if (intSemicolon == -1) intSemicolon = 999999;
                intMin = Math.Min(intOpenBracket, intMin);
                intMin = Math.Min(intClosedBracket, intMin);
                intMin = Math.Min(intSemicolon, intMin);
                if (intMin == 999999) {
                    intMin = -1;
                }

                while (intMin != -1) {
                    // We need to write what is before the bracket (if anything) to one line
                    if (intMin > 0) {
                        if (strInputLineCleaned.Contains("#Comment") == false) {
                            if (strInputLineCleaned.StartsWith("for ") == false && strInputLineCleaned.StartsWith("for(") == false) {
                                if (intMin == intSemicolon && intSemicolon != -1) {
                                    //we need to handle a semicolon by writing up to and including semicolon
                                    strCleaned = strInputLineCleaned.Substring(0, intMin + 1);
                                    ProcessCleaned();
                                } else {
                                    strCleaned = strInputLineCleaned.Substring(0, intMin);
                                    ProcessCleaned();
                                }
                            } else {
                                // we need to find where open and close parens are equal and write that substring
                                // TODO: this will not work when close paren is not on same line
                                int intCloseParen;
                                intCloseParen = strInputLineCleaned.LastIndexOf(")");
                                if (intCloseParen > -1) {
                                    intMin = intCloseParen + 1;
                                    //strCleaned = strInputLineCleaned.Substring(0, intMin);
                                    //ProcessCleaned();

                                }

                            }
                        } else {
                            // we need to find where open and close parens are equal and write that substring
                            // TODO: this will not work when close paren is not on same line
                            int intCloseParen;
                            intCloseParen = strInputLineCleaned.LastIndexOf(">>");
                            if (intCloseParen > -1) {
                                intMin = intCloseParen + 2;
                                //strCleaned = strInputLineCleaned.Substring(0, intMin);
                                //ProcessCleaned();                                
                            }
                        }
                    }
                    if (intMin < strInputLineCleaned.Length) {
                        intLastBracket = intMin;
                        // We need to write out the bracket to a new line
                        if (strInputLineCleaned.Substring(intMin, 1) != ";") {
                            strCleaned = strInputLineCleaned.Substring(intMin, 1);
                            ProcessCleaned();


                        }
                    }

                    // D.
                    // Increment the index.
                    if (intMin == strInputLineCleaned.Length || (intMin + 1 == strInputLineCleaned.Length && strInputLineCleaned.EndsWith(";"))) {
                        break;
                    } else {
                        strInputLineCleaned = strInputLineCleaned.Substring(intMin + 1);
                    }
                    intOpenBracket = strInputLineCleaned.IndexOf('{');
                    intClosedBracket = strInputLineCleaned.IndexOf('}');
                    intSemicolon = strInputLineCleaned.IndexOf(';');
                    intMin = 999999;

                    if (intOpenBracket == -1) intOpenBracket = 999999;
                    if (intClosedBracket == -1) intClosedBracket = 999999;
                    if (intSemicolon == -1) intSemicolon = 999999;
                    intMin = Math.Min(intOpenBracket, intMin);
                    intMin = Math.Min(intClosedBracket, intMin);
                    intMin = Math.Min(intSemicolon, intMin);
                    if (intMin == 999999) intMin = -1;
                }
                if (intLastBracket == -1) {
                    strCleaned = strInputLineCleaned;
                    ProcessCleaned();
                } else if (intLastBracket + 1 < strInputLineCleaned.Length) {
                    strCleaned = strInputLineCleaned.Substring(intLastBracket + 1);
                    ProcessCleaned();
                }


            }
            }
            




            

            // Loop thru array of categories and methods
            // When you encounter new category, write red label; else write grey button
            // The name for each button is myButton followed by method name
            // If there are more than 20 rows, start a new column
            // If there are more than 18 rows, and you encounter a new category, start
            // new column so you do not have category at the bottom without any methods
            // below it
            int intWindowTop = 0;
            int intWindowLeft = 0;
            string strWindowTop = "";
            string strWindowLeft = "";
            intRowCtr = 0;
  
            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window

            intEndPageMethod = listCategoryMethod.Count();
            intTotalMethods = listCategoryMethod.Count();
            BuildMenuDialogPage:
            myListControlEntity = new List<ControlEntity>();

            myControlEntity = new ControlEntity();
            intCol = 0;
            intRow = 0;
            intRowCtr = 0;
            if (intStartPageMethod < 0) {
                intStartPageMethod = 0;
            }
            intEndPageMethod = intStartPageMethod + intPageSize;
            if (intEndPageMethod > intTotalMethods) {
                intEndPageMethod = intTotalMethods;
            }
            for (int i = intStartPageMethod; i < intEndPageMethod; i++) {

                var item = listCategoryMethod.ToList()[i];
                string[] myArrayFields = item.Key.Replace("Category::","").ToString().Split('^');
                {
                    intRow++;
                    if (intRow > 20) {
                        intRow = 1;
                        intCol++;
                    }
                    string strMethodName = myArrayFields[1];
                    string strCategory = myArrayFields[0];

                    if (strCategory != strPreviousCategory) {
                        if (intRow > 18) {
                            intRow = 1;
                            intCol++;
                        }
                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Label;
                        myControlEntity.ID = "lbl" + strCategory.Replace(" ", "");
                        myControlEntity.Text = strCategory;
                        myControlEntity.RowNumber = intRow;
                        myControlEntity.ColumnNumber = intCol;
                        myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                        myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());
                        strPreviousCategory = strCategory;
                        intRow++;
                    }
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Button;
                    myControlEntity.ID = "myButton" + strMethodName.Replace(" ", "");
                    myControlEntity.Text = strMethodName;
                    myControlEntity.RowNumber = intRow;
                    myControlEntity.ColumnNumber = intCol;
                    //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                }
            }



            intRow++;
            if (intRow > 18) {
                intRow = 1;
                intCol++;
            }
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "(" + intStartPageMethod.ToString() + " to " + intEndPageMethod.ToString() + ") of Total Methods: " + intTotalMethods.ToString();
            myControlEntity.RowNumber = intRow;
            myControlEntity.FontFamilyx = new FontFamily("Segoe UI Bold"); 
            myControlEntity.ColumnNumber = intCol;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            if (intStartPageMethod > 0) {
                intRow++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnPrev";
                myControlEntity.Text = "Prev";
                myControlEntity.RowNumber = intRow;
                myControlEntity.ColumnNumber = intCol;
                //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
            }

            if (intEndPageMethod < intTotalMethods) {
                intRow++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnNext";
                myControlEntity.Text = "Next";
                myControlEntity.RowNumber = intRow;
                myControlEntity.ColumnNumber = intCol;
                //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
            }
            string strScripts = "";
            string strVariables = "";
            string strVariablesValue = "";
            string strScripts1 = "";
            string strVariables1 = "";
            string strScripts2 = "";
            string strVariables2 = "";
            string strVariables1Value = "";
            string strVariables2Value = "";
            int intWidth = (intCol + 1) * intColumnWidth;

            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, intWidth, intWindowTop, intWindowLeft);
            DisplayWindowAgain:

            if (strButtonPressed == "btnCancel") {
                // myActions.MessageBoxShow(strButtonPressed);
                goto myExit;
            }
            if (strButtonPressed == "btnOkay") {
                //  myActions.MessageBoxShow(strButtonPressed);
                goto myExit;
            }
            if (strButtonPressed == "btnPrev") {
                //  myActions.MessageBoxShow(strButtonPressed);
                intStartPageMethod = intStartPageMethod - intPageSize;
                goto BuildMenuDialogPage;
            }
            if (strButtonPressed == "btnNext") {
                //  myActions.MessageBoxShow(strButtonPressed);
                if (intStartPageMethod + intPageSize < intTotalMethods) {
                    intStartPageMethod = intStartPageMethod + intPageSize;
                }
                goto BuildMenuDialogPage;
            }
            strMethod = strButtonPressed.Replace("myButton", "");
            bool boolDescription = false;
            bool boolParameters = false;
            bool boolReturnType = false;
            bool boolMethod = false;
            bool boolOriginalMethod = false;
            string strReturnType = "";
            string strOriginalMethod = "";
            listComments.Clear();
            listParameters.Clear();
            sb.Length = 0;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(Path.Combine(myNewProjectSourcePath, @"Text\" + strMethod + ".txt"))) {
                while ((strInputLineOrig = sr.ReadLine()) != null) {
                    if (strInputLineOrig == "D e s c r i p t i o n:") {
                        boolDescription = true;
                        continue;
                    }
                    if (strInputLineOrig == "P a r a m e t e r s:") {
                        boolDescription = false;
                        boolParameters = true;
                        continue;
                    }
                    if (strInputLineOrig == "R e t u r n  T y p e:") {                        
                        boolParameters = false;
                        boolReturnType = true;
                        continue;
                    }
                    if (strInputLineOrig == "M e t h o d:") {
                        boolReturnType = false;
                        boolMethod = true;
                        continue;
                    }
                    if (strInputLineOrig == "O r i g i n a l  M e t h o d:") {
                        boolMethod = false;
                        boolOriginalMethod = true;
                        continue;
                    }
                    if (boolDescription) {
                        listComments.Add(strInputLineOrig);
                        continue;
                    }
                    if (boolParameters && strInputLineOrig.Trim() != "") {
                        listParameters.Add(strInputLineOrig);
                        continue;
                    }
                    if (boolReturnType && strInputLineOrig.Trim() != "") {
                        strReturnType = strInputLineOrig.Substring(0, strInputLineOrig.IndexOf(" "));
                        continue;
                    }
                    if (boolMethod && strInputLineOrig.Trim() != "") {
                        strMethod = strInputLineOrig;
                        continue;
                    }
                    if (boolOriginalMethod && strInputLineOrig.Trim() != "") {
                        strOriginalMethod = strInputLineOrig;
                        continue;
                    }

                }
            }
            if (strReturnType.ToUpper() != "VOID" && strReturnType.Trim() != "") {
                sb.Append(strReturnType + " " + "[[result]] = ");
            }
            sb.Append("myActions.");
            sb.Append(strMethod);
            sb.Append("(");
            foreach (var item in listParameters) {
                string[] arrayParameters = item.Split(' ');
                sb.Append("[[" + arrayParameters[1] + "]],");
            }
            sb.Append(");");
            string strSyntax = sb.ToString().Replace(",)", ")");
            StringBuilder sb1 = new StringBuilder();
            foreach (var item in listComments) {
                sb1.AppendLine(item);
            }
            ControlEntity myControlEntity1 = new ControlEntity();
            List<ControlEntity> myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            cbp1 = new List<ComboBoxPair>();
            DisplayIEGoToURLWindow:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();

            // Row 0 is heading that says:
            // IE Go To URL
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lbl" + strMethod;
            myControlEntity1.Text = strMethod;
            myControlEntity1.Width = 300;
            myControlEntity1.RowNumber = 0;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            // Row 1 has label Syntax and textbox that contains syntax
            // The syntax is hard-coded inline
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
            myControlEntity1.Text = strSyntax;            
            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            // Row 2 has label Parameters
            if (listParameters.Count > 0) {
                intRowCtr++;
                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.Label;
                myControlEntity1.ID = "lblParameters";
                myControlEntity1.Text = "Parameters:";
                myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 0;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
            }
            // Row 3 has label Website URL 
            // and textbox that contains Website URL
            // The value for Website URL comes from roaming folder for script
            foreach (var item in listParameters) {
                string[] arrayParameters = item.Split(' ');
                intRowCtr++;
                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.Label;
                myControlEntity1.ID = "lbl" + arrayParameters[1];
                myControlEntity1.Text = arrayParameters[0] + " [[" + arrayParameters[1] + "]]:";
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 0;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.TextBox;
                myControlEntity1.ID = "txt" + arrayParameters[1];
                myControlEntity1.Text = myActions.GetValueByKey("txt" + strMethod + arrayParameters[1]);
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 1;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
            }

            if (strReturnType.ToUpper() != "VOID" && strReturnType.Trim() != "") {
                intRowCtr++;
                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.Label;
                myControlEntity1.ID = "lblReturnType";
                myControlEntity1.Text = "Returned Result:";
                myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 0;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                intRowCtr++;
                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.Label;
                myControlEntity1.ID = "lblResult";
                myControlEntity1.Text = strReturnType + " [[Result]]:";
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 0;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.TextBox;
                myControlEntity1.ID = "txt" + strMethod + "Result";
                myControlEntity1.Text = myActions.GetValueByKey("txt" + strMethod + "Result");
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 1;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
            }

   
            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblDescription";
            myControlEntity1.Text = "Description:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtDescription";
            myControlEntity1.Height = 200;
            myControlEntity1.Text = sb1.ToString();
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myControlEntity1.ColumnSpan = 4;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            // Get saved position of window from roaming..
            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            // Display input dialog
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 1200, intWindowTop, intWindowLeft);
            // Get Values from input dialog and save to roaming
            
           
            
            foreach (var item in listParameters) {
                string[] arrayParameters = item.Split(' ');
                string myParameterValue = myListControlEntity1.Find(x => x.ID == "txt" + arrayParameters[1]).Text;
                myActions.SetValueByKey("txt" + strMethod + arrayParameters[1], myParameterValue);
            }
            if (strReturnType.ToUpper() != "VOID" && strReturnType.Trim() != "") {
                string myResultValue = myListControlEntity1.Find(x => x.ID == "txt" + strMethod + "Result").Text;
                myActions.SetValueByKey("txt" + strMethod + "Result", myResultValue);
            }


            // if okay button pressed, validate inputs; place inputs into syntax; put generated 
            // code into clipboard and display generated code
            if (strButtonPressed == "btnOkay") {
                //if (strWebsiteURLx == "" && strVariables == "--Select Item ---") {
                //    myActions.MessageBoxShow("Please enter Website URL or select script variable; else press Cancel to Exit");
                //    goto DisplayIEGoToURLWindow;
                //}
                //string strWebsiteURLToUse = "";
                //if (strWebsiteURLx.Trim() == "") {
                //    strWebsiteURLToUse = strVariables;
                //} else {
                //    strWebsiteURLToUse = "\"" + strWebsiteURLx.Trim() + "\"";
                //}
                string strGeneratedLinex = strSyntax;

                foreach (var item in listParameters) {
                    string[] arrayParameters = item.Split(' ');                    
                    string searchTerm = "[[" + arrayParameters[1] + "]]";
                    string replacementValue = myListControlEntity1.Find(x => x.ID == "txt" + arrayParameters[1]).Text;
                    strGeneratedLinex = strGeneratedLinex.Replace(searchTerm, replacementValue);
                }

                if (strReturnType.ToUpper() != "VOID" && strReturnType.Trim() != "") {
                    string searchTerm = "[[Result]]";
                    string replacementValue = myListControlEntity1.Find(x => x.ID == "txt" + strMethod + "Result").Text;
                    strGeneratedLinex = strGeneratedLinex.Replace(searchTerm, replacementValue);
                }

                //strGeneratedLinex = "myActions.IEGoToURL(myActions, " + strWebsiteURLToUse + ", " + strUseNewTab + ");";

                myActions.PutEntityInClipboard(strGeneratedLinex);
                myActions.MessageBoxShow(strGeneratedLinex + Environment.NewLine + Environment.NewLine + "The generated text has been put into your clipboard");
            }
            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            // Display main menu and go back to where you 
            // process input from main menu
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 1200, intWindowTop, intWindowLeft);
            goto DisplayWindowAgain;
            
            myExit:
            string strApplicationBinDebug2 = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath2 = strApplicationBinDebug2.Replace("\\bin\\Debug", "");



            System.IO.DirectoryInfo di2 = new DirectoryInfo(Path.Combine(myNewProjectSourcePath2, @"Text"));

            foreach (FileInfo file in di2.GetFiles()) {
                if (file.Name != "Reserved_Words_CS.txt" && file.Name != "Type_Words_CS.txt") {
                    file.Delete();
                }
            }
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft) {
            strWindowLeft = myActions.GetValueByKey("WindowLeft");
            strWindowTop = myActions.GetValueByKey("WindowTop");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
        }
        private void ProcessMethod() {
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            if (!strMethodOrig.ToUpper().StartsWith("PUBLIC")) {
                listComments.Clear();
                return;
            }
            string strCategory = "";
            //string strLine = "";
            //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(myNewProjectSourcePath, @"Text\output.txt"))) {
            listOutput.Add("D e s c r i p t i o n:");
            bool boolCategoryFound = false;
            foreach (var item in listComments) {
                listOutput.Add(item);
                if (item.Contains(@"Category::")) {
                    boolCategoryFound = true;
                    if (!listCategoryMethod.ContainsKey(item.Trim().Replace(">", "_") + "^" + strMethod)) {
                        listCategoryMethod.Add(item.Trim().Replace(">", "_") + "^" + strMethod, item + "^" + strMethod);
                    }                    
                }
            }
            if (boolCategoryFound == false) {
                if (!listCategoryMethod.ContainsKey("_UnCategorized" + "^" + strMethod)) {
                    listCategoryMethod.Add("_UnCategorized" + "^" + strMethod, "_UnCategorized" + "^" + strMethod);
                }
            }
            int i = 0;
            listOutput.Add(" ");
           listOutput.Add("P a r a m e t e r s:");
            foreach (var item in strLocalVariableArray) {

                listOutput.Add(strLocalVariableTypeArray[i] + " " + item);
                // sw.WriteLine(strLine);
            }

            string strReturnType = strMethodOrig.Substring(7);
            strReturnType = strReturnType.Substring(0, strReturnType.IndexOf(strMethod)).Trim();
            listOutput.Add(" ");
            listOutput.Add("R e t u r n  T y p e:");

          listOutput.Add(strReturnType + " " + "RETURNED VARIABLE TYPE");
            listOutput.Add(" ");
            listOutput.Add("M e t h o d:");
            listOutput.Add(strMethod);
            listOutput.Add(" ");
            listOutput.Add("O r i g i n a l  M e t h o d:");
            listOutput.Add(strMethodOrig);
            intMethodCtr++;
            listOutput.Add("================================" + intMethodCtr.ToString());
            listComments.Clear();
            //}
            string strLine = "";
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(myNewProjectSourcePath, @"Text\" + strMethod + ".txt"))) {
                foreach (var item in listOutput) {
                    strLine = item;
                    sw.WriteLine(strLine);
                }
            }
            listOutput.Clear();
        }

        private void CheckForClass() {
            if (strCleaned.ToUpper().Trim().StartsWith("CLASS ") ||
                strCleaned.ToUpper().Trim().Contains(" CLASS ")
                ) {
                // write inherits line
                boolClass = true;

                boolLineProcessed = true;

                strClass = strCleaned.Trim().Substring(strCleaned.ToUpper().IndexOf("CLASS ") + 6);
                int myIndex = 0;
                if (strClass.IndexOf(" ") > -1) { myIndex = strClass.IndexOf(" "); }
                if (myIndex > 0) {
                    strClass = strClass.Substring(0, strClass.IndexOf(" "));
                }

                myIndex = 0;
                if (strClass.IndexOf(":") > -1) { myIndex = strClass.IndexOf(":"); }
                if (myIndex > 0) {
                    strClass = strClass.Substring(0, strClass.IndexOf(":"));
                }
            }
        }
        private void CheckForMethodOrFunction() {


            if (strCleaned.ToUpper().IndexOf("(") > -1
                && strCleaned.ToUpper().EndsWith(";") == false
                && (strCleaned.ToUpper().Trim().StartsWith("VOID ")
                || strCleaned.ToUpper().Trim().StartsWith("INTERNAL ")
                || strCleaned.ToUpper().Trim().StartsWith("PRIVATE ")
                || strCleaned.ToUpper().Trim().StartsWith("PUBLIC ")
                || strCleaned.ToUpper().Trim().StartsWith("PROTECTED ")
                || strCleaned.ToUpper().Trim().StartsWith("STATIC "))) {
                //intNestingLevel = 0;
                strMethodOrig = strCleaned;


                boolMethod = true;
                // we are putting method name and parameters in strMethod
                strMethod = strCleaned.Substring(0, strCleaned.ToUpper().IndexOf("("));
                strMethod = strCleaned.Substring(strMethod.LastIndexOf(" ") + 1);


                string strParameters = "";


                strParameters = strMethod.Substring(strMethod.IndexOf("("), strMethod.LastIndexOf(")") - strMethod.IndexOf("(") + 1);

                strMethod = strMethod.Substring(0, strMethod.IndexOf("("));
                listMethod.Add(strMethod);
                Array.Resize(ref strLocalVariableArray, 0);
                Array.Resize(ref strLocalVariableTypeArray, 0);
                idxLocalVariable = 0;
                Array.Resize(ref strLocalVariableNestingArray, 0);
                idxLocalVariableNesting = 0;
                if (strParameters.Length > 0) {
                    ParseParameters(strParameters);
                }
                strCleanedHold = strCleaned;
                boolFirstStatementInMethod = true;
                boolLineProcessed = true;

            }

            if (strClass == "" && boolFirstMethodFound == false) {
                CheckForApplicationVariables(strCleaned);
            }

        }

        private void ParseParameters(string strParameters) {
            char[] delimiters = new char[] { ',' };

            string[] CurrentLinePhrases = strParameters.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string strPhrase in CurrentLinePhrases) {
                string strCurrentPhrase = strPhrase.Trim();
                string strCurrentPhraseType = strPhrase;

                if (strCurrentPhrase.StartsWith("ref ")) {
                    strCurrentPhrase = strCurrentPhrase.Substring(4);
                }

                // remove any property types that preceed the parameter
                if (strCurrentPhrase.IndexOf(" ") > -1) {
                    strCurrentPhraseType = strCurrentPhrase.Substring(0, strCurrentPhrase.IndexOf(" "));
                    strCurrentPhrase = strCurrentPhrase.Substring(strCurrentPhrase.IndexOf(" ") + 1);
                }

                char[] delimiters1 = new char[] { '(', ')', ' ', ',', '=', '"', '[', ']' };

                string[] CurrentLineWords = strCurrentPhrase.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);
                string strLocalVariableType = "";
                string strLocalVariable = "";

                if (CurrentLineWords.Length > 0) {
                    strLocalVariableType = strCurrentPhraseType.Replace("(", "");
                    strLocalVariable = CurrentLineWords[0];
                    AddLocalVariable(strLocalVariable, strLocalVariableType);
                }

                if (CurrentLineWords.Length == 2) {
                    strLocalVariableType = "int";
                    strLocalVariable = CurrentLineWords[1];
                    AddLocalVariable(strLocalVariable, strLocalVariableType);
                }
            }

        }

        private void AddLocalVariable(string strLocalVariable, string strLocalVariableType) {
            Array.Resize(ref strLocalVariableArray, idxLocalVariable + 1);
            Array.Resize(ref strLocalVariableTypeArray, idxLocalVariable + 1);
            strLocalVariableArray[idxLocalVariable] = strLocalVariable;
            strLocalVariableTypeArray[idxLocalVariable] = strLocalVariableType;
            idxLocalVariable = idxLocalVariable + 1;

            Array.Resize(ref strLocalVariableNestingArray, idxLocalVariableNesting + 1);
            strLocalVariableNestingArray[idxLocalVariableNesting] = intNestingLevel;
            idxLocalVariableNesting = idxLocalVariableNesting + 1;
        }

        private void CheckForApplicationVariables(string strInputLine) {
            if (strInputLine.IndexOf("=") > -1) {
                strInputLine = strInputLine.Substring(0, strInputLine.IndexOf("="));
            }
            // TODO: AS is not a C# keyword
            if (strCleaned.ToUpper().IndexOf(" AS ") > -1) {
                strInputLine = strInputLine.Substring(0, strCleaned.ToUpper().IndexOf(" AS "));
            }
            char[] delimiters = new char[] { '(', ')', ' ', ',', '=', '"' };

            string[] CurrentLineWords = strInputLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (CurrentLineWords.Length == 2) {
                Array.Resize(ref strGlobalVariableArray, idxGlobalVariable + 1);
                strGlobalVariableArray[idxGlobalVariable] = CurrentLineWords[1];
                idxGlobalVariable = idxGlobalVariable + 1;
            }

            if (CurrentLineWords.Length == 3) {
                string strFirst;
                strFirst = CurrentLineWords[0].ToUpper().Trim();
                if (strFirst == "PUBLIC" || strFirst == "PROTECTED" || strFirst == "PRIVATE" || strFirst == "INTERNAL" || strFirst == "PROPERTY") {
                    Array.Resize(ref strGlobalVariableArray, idxGlobalVariable + 1);
                    strGlobalVariableArray[idxGlobalVariable] = CurrentLineWords[2];
                    idxGlobalVariable = idxGlobalVariable + 1;
                }
            }

            if (CurrentLineWords.Length == 4) {
                string strFirst;
                strFirst = CurrentLineWords[0].ToUpper().Trim();
                string strSecond;
                strSecond = CurrentLineWords[1].ToUpper().Trim();
                if (strFirst == "PUBLIC"
                    || strFirst == "PROTECTED"
                    || strFirst == "PRIVATE"
                    || strFirst == "INTERNAL"
                    || strFirst == "PROPERTY") {
                    if (strSecond == "PROPERTY") {
                        Array.Resize(ref strGlobalVariableArray, idxGlobalVariable + 1);
                        strGlobalVariableArray[idxGlobalVariable] = CurrentLineWords[3];
                        idxGlobalVariable = idxGlobalVariable + 1;
                    }
                }
            }


        }
        private void ProcessCleaned() {
            if (intInLine == 37) {
                int intDebug;
            }
            strInputLineOrig = strCleaned;
            // TODO: Take strCleaned and replace #Literalx with quoted string from dict
            Regex rgx = new Regex(@"#(Literal|Comment)(\d+)>>");
            while (rgx.IsMatch(strInputLineOrig)) {
                ReplaceLiteralWithDictValue();
            }
            boolLineProcessed = false;

            // strCleaned only has markers for quoted strings and comments
            // strInputLineOrig has the quoted strings and comments in it

            // TODO: Namespace is going to throw off my absolute values for class and method

            if (strCleaned.Contains("new ") && strCleaned.Contains("char")) {

                boolTraceLoggingOn = false;
                return;
            }
            if (boolTraceLoggingOn == false && strCleaned.Contains("}") == false) {

                return;
            }
            if (boolTraceLoggingOn == false && strCleaned.Contains("}") == true) {
                //  outputfile.WriteLine("};");
                boolTraceLoggingOn = true;
                return;
            }
            int Count = CountBrackets(strCleaned);
            intNestingLevel = intNestingLevel + Count;

            if (boolNamespace == true && intNestingLevel == 0) {
                intClassLevel = 0;
                intMethodLevel = 1;
                boolNamespace = false;
            }

            if (strCleaned.StartsWith("namespace")) {
                intClassLevel = 1;
                intMethodLevel = 2;
                boolNamespace = true;
            }

            CheckForComments();
            if (boolLineProcessed) {
                return;
            }

            CheckForClass();
            if (boolLineProcessed) {
                listComments.Clear();
                return;
            }
            //CheckForDuplicateImports();
            //if (boolLineProcessed) return;

            //CheckForEndOfClass();
            //if (boolLineProcessed) return;

            CheckForMethodOrFunction();
            if (boolLineProcessed) {
                ProcessMethod();
                return;
            }

            listComments.Clear();
            if (boolMethod == true) {
                CheckForNewMethodBeforeEnd();
                if (boolLineProcessed) return;

                CheckForMethodEnd();
                if (boolLineProcessed) return;


                CheckForMethodBodyStatement();
                if (boolLineProcessed) return;
            }

            //This is for the inherits statement
            if (boolLineProcessed == false) {

                boolLineProcessed = true;
            }
        }
        private void ReadCleanConcatCodeRec(System.IO.StreamReader file) {
            intInLine = intInLine + 1;
            strInputLineOrig = file.ReadLine();
            strInputLineCleaned += " " + RemoveLiteralsComments(strInputLineOrig);
        }
        private string RemoveLiteralsComments(string strInputLine) {
            string strInputLineUnclean = strInputLine;


            // Three steps:
            // 1. Remove escaped characters because they lead to confusion
            // 2. Replace strings enclosed in single or double quotes with #Literal
            // 3. Remove comments

            // TODO: this will not handle verbatim strings on multiple lines
            // look for @" not preceded by \
            //    then look for first double quote not preceded by \ or double quote

            int intStartVerbatim;
            int intEndVerbatim = 0;
            bool boolEndVerbatim = false;
            bool boolContinueSearch = true;
            string strVerbatim;
            intStartVerbatim = strInputLineUnclean.IndexOf("@\"");
            if (intStartVerbatim > -1) // we found possible start
            {
                if (strInputLineUnclean.Substring(intStartVerbatim - 1, 1) != "//") //we have start
                {
                    intEndVerbatim = intStartVerbatim + 1;
                    while (boolEndVerbatim == false
                        && boolContinueSearch == true) {
                        if (intEndVerbatim + 1 > strInputLineUnclean.Length) {
                            boolContinueSearch = false;
                            break;
                        }
                        intEndVerbatim = strInputLineUnclean.IndexOf("\"", intEndVerbatim + 1);
                        if (intEndVerbatim == -1) {
                            boolContinueSearch = false;
                            break;
                        }

                        if (strInputLineUnclean.Substring(intEndVerbatim - 1, 1) != "\""
                           && strInputLineUnclean.Substring(intEndVerbatim - 1, 1) != "\\") {
                            boolEndVerbatim = true;
                            strVerbatim = strInputLineUnclean.Substring(intStartVerbatim, intEndVerbatim - intStartVerbatim + 1);
                            strInputLineUnclean = strInputLineUnclean.Replace(strVerbatim, "#Literal" + intLit.ToString() + ">>");
                            dict.Add("#Literal" + intLit.ToString() + ">>", "\"VerbatimLiteral\"");
                            intLit = intLit + 1;
                        }

                    }

                }
            }

            // Step 1 remove esacpe character and one character after it
            // from strInputLineUnclean unless one character after it is also an escape 
            // character - trying to replace \" and \' with nothing so they
            // will not interfere with my looking for literals
            Regex rgx;
            string pattern = Regex.Escape("\\\"");
            string replacement = "#Literal";
            rgx = new Regex(pattern);
            string result = strInputLineUnclean;
            foreach (Match mymatch in rgx.Matches(strInputLineUnclean)) {
                result = rgx.Replace(result, replacement + intLit.ToString() + ">>", 1);
                dict.Add(replacement + intLit.ToString() + ">>", mymatch.ToString());
                intLit = intLit + 1;
            }


            pattern = Regex.Escape("\\\'");
            strInputLineUnclean = result;
            rgx = new Regex(pattern);

            foreach (Match mymatch in rgx.Matches(strInputLineUnclean)) {
                result = rgx.Replace(result, replacement + intLit.ToString() + ">>", 1);
                dict.Add(replacement + intLit.ToString() + ">>", mymatch.ToString());
                intLit = intLit + 1;
            }

            // Now we do not need to worry about escape characters any more

            // Step 2. Replace strings enclosed in single or double quotes with #Literal 

            pattern = @"([\\""'])(?:\\\1|.)*?\1";
            strInputLineUnclean = result;
            rgx = new Regex(pattern);

            foreach (Match mymatch in rgx.Matches(strInputLineUnclean)) {
                result = rgx.Replace(result, replacement + intLit.ToString() + ">>", 1);
                dict.Add(replacement + intLit.ToString() + ">>", mymatch.ToString());
                intLit = intLit + 1;
            }

            strInputLineUnclean = result;

            // Step 3: Remove comments
            int intCommentPosition = strInputLineUnclean.IndexOf("//");

            // if comment is the first thing we find, append everything before comment to 
            // sb and clear out strInputLineUnclean because we are done
            if (intCommentPosition > -1) {
                string strComment = strInputLineUnclean.Substring(intCommentPosition);
                strInputLineUnclean = strInputLineUnclean.Substring(0, intCommentPosition);
                strInputLineUnclean += "#Comment" + intLit.ToString() + ">>";
                dict.Add("#Comment" + intLit.ToString() + ">>", strComment);
                intLit = intLit + 1;
            }

            // Step 4: Remove comments continued
            // Possibilities
            // 1. Only Starts on this line
            // 2. Starts on this line and ends on this line
            // 3. Only Ends on this line
            // 4. boolContinued = true and does not end
            // 5. boolContinued = true and does end
            string strInputLineUncleanHold = strInputLineUnclean;
            intCommentPosition = strInputLineUnclean.IndexOf("/*");
            int intEndComment = -1;
            // if comment is the first thing we find, append everything before comment to 
            // sb and clear out strInputLineUnclean because we are done
            if (intCommentPosition > -1)  // starts on this line
            {
                boolCommentsContinued = true;

                intEndComment = strInputLineUncleanHold.IndexOf("*/");
                if (intEndComment == -1) // Only starts on this line
                {
                    string strComment = strInputLineUnclean.Substring(intCommentPosition);
                    strInputLineUnclean = strInputLineUnclean.Substring(0, intCommentPosition);
                    strInputLineUnclean += "#Comment" + intLit.ToString() + ">>";
                    dict.Add("#Comment" + intLit.ToString() + ">>", strComment);
                    intLit = intLit + 1;
                } else // starts and ends on this line
                  {
                    boolCommentsContinued = false;
                    int intLength = (intEndComment - intCommentPosition) + 2;
                    string strComment = strInputLineUnclean.Substring(intCommentPosition, intLength);
                    strInputLineUnclean = strInputLineUnclean.Substring(0, intCommentPosition);
                    strInputLineUnclean += "#Comment" + intLit.ToString() + ">>";
                    dict.Add("#Comment" + intLit.ToString() + ">>", strComment);
                    intLit = intLit + 1;
                }
            } else // comment did not start on this line
              {
                intCommentPosition = strInputLineUncleanHold.IndexOf("*/");
                if (boolCommentsContinued == true && intCommentPosition > -1) // comment ends on this line
                {
                    boolCommentsContinued = false;
                    string strComment = strInputLineUnclean.Substring(0, intCommentPosition + 2);
                    strInputLineUnclean = strInputLineUnclean.Substring(intCommentPosition + 2);
                    strInputLineUnclean = "#Comment" + intLit.ToString() + ">>" + strInputLineUnclean;
                    dict.Add("#Comment" + intLit.ToString() + ">>", strComment);
                    intLit = intLit + 1;
                } else {
                    if (boolCommentsContinued == true) // simply a continued comment
                    {
                        string strComment = strInputLineUnclean;
                        strInputLineUnclean = "";
                        strInputLineUnclean += "#Comment" + intLit.ToString() + ">>";
                        dict.Add("#Comment" + intLit.ToString() + ">>", strComment);
                        intLit = intLit + 1;
                    }
                }
            }



            return strInputLineUnclean.Trim();
        }
        private int CountParens(string strInputLine) {
            // positive if more opening brackets than closing ones
            int intOpen = strInputLine.Count(c => c == '(');
            int intClose = strInputLine.Count(c => c == ')');
            int count = intOpen - intClose;
            return count;
        }
        void ReplaceLiteralWithDictValue() {
            MatchCollection matches = Regex.Matches(strInputLineOrig, @"#(Literal|Comment)(\d+)>>", RegexOptions.RightToLeft);

            foreach (Match match in matches) {
                strInputLineOrig = strInputLineOrig.Replace(match.ToString(), dict[match.ToString()]);

            }
        }
        private void CheckForComments() {
            if (strCleaned.Trim() == "") {
                // we are not throwing away comments
                //           WriteOrig(strInputLineOrig);

                boolLineProcessed = true;
            }
            if (strCleaned.Contains("#Comment")) {
                listComments.Add(strInputLineOrig.Replace("<para>", "").Replace("</para>", "").Replace("///", "").Replace("//", ""));
                boolLineProcessed = true;
            }
        }
        private int CountBrackets(string strInputLine) {
            // positive if more opening brackets than closing ones
            int intOpen = strInputLine.Count(c => c == '{');
            int intClose = strInputLine.Count(c => c == '}');
            int count = intOpen - intClose;
            return count;
        }
        private void CheckForMethodEnd() {
            if (intNestingLevel < intMethodLevel + 1
                && strCleaned.Contains("}")) {
                //set boolMethod to false and write line
                boolMethod = false;


                //  WriteInputToTracelog();

                //  WriteOrig(strInputLineOrig);
                boolLineProcessed = true;
            }
        }
        private void CheckForNewMethodBeforeEnd() {
            if (boolFirstMethodFound == false
                && (strCleaned.ToUpper().IndexOf("(") > -1
                && strCleaned.ToUpper().EndsWith(";") == false
                && (strCleaned.ToUpper().Trim().StartsWith("VOID ")
                || strCleaned.ToUpper().Trim().StartsWith("INTERNAL ")
                || strCleaned.ToUpper().Trim().StartsWith("PRIVATE ")
                || strCleaned.ToUpper().Trim().StartsWith("PUBLIC ")
                || strCleaned.ToUpper().Trim().StartsWith("PROTECTED ")
                || strCleaned.ToUpper().Trim().StartsWith("STATIC ")))) {
                //set boolMethod to true and write line
                boolMethod = true;

                // WriteOrig(strInputLineOrig);

                if (strCleaned.ToUpper().IndexOf("(") > -1
                    && strCleaned.ToUpper().EndsWith(";") == false
                    && (strCleaned.ToUpper().Trim().StartsWith("VOID ")
                    || strCleaned.ToUpper().Trim().StartsWith("INTERNAL ")
                    || strCleaned.ToUpper().Trim().StartsWith("PRIVATE ")
                    || strCleaned.ToUpper().Trim().StartsWith("PUBLIC ")
                    || strCleaned.ToUpper().Trim().StartsWith("PROTECTED ")
                    || strCleaned.ToUpper().Trim().StartsWith("STATIC "))) {
                    // we are putting method name and parameters in strMethod
                    strMethod = strCleaned.Substring(0, strCleaned.ToUpper().IndexOf("("));
                    strMethod = strCleaned.Substring(strMethod.LastIndexOf(" ") + 1);
                    // we are removing the parameters
                    strMethod = strMethod.Substring(0, strMethod.ToUpper().IndexOf("("));
                }

                boolLineProcessed = true;
            }
        }
        private void CheckForMethodBodyStatement() {
            if (strCleaned.Contains("{")) {
                boolFoundOpenBracket = true;
            }
            if (boolMultipleStatementsPossible
                && boolFoundOpenBracket == false
                && strCleaned.Contains(";")
                ) {
                //  outputfile.WriteLine("{");
                //WriteOrig(strInputLineOrig);
                //WriteInputToTracelog();
                //   outputfile.WriteLine("}");
                boolMultipleStatementsPossible = false;
                boolFoundOpenBracket = false;
                return;
            }
            if (strCleaned.StartsWith("{") == true && boolFirstStatementInMethod == true) {
                //WriteOrig(strInputLineOrig);
                strCleaned = strCleanedHold;
                if (boolFirstMethodFound == false) {
                    // write general declarations

                    //outputfile.WriteLine(@"#region TraceOn Begin 3");
                    //outputfile.WriteLine("int intLevel = -1;");

                    //outputfile.WriteLine("string[] aryLevels = new string[5001];");

                    //outputfile.WriteLine("string strBase;");
                    //outputfile.WriteLine(@"#endregion TraceOn End 3");

                    //write the input line


                    boolFirstMethodFound = true;
                }


                //WriteInputToTracelog();
                boolLineProcessed = true;
                return;
            }

            //CheckForLocalVariables(strCleaned);

            if (strCleaned.Contains("}")) {
                //RemoveLocalVariablesAtNestingLevel(intNestingLevel);
            }

            // we need to write tracelog before writing orig for many conditional type
            // statements and break statements
            if (strCleaned.Trim() != ""
                && (strCleaned.ToUpper().Trim().StartsWith("IF ")
                || strCleaned.ToUpper().Trim().StartsWith("IF(")
                || strCleaned.ToUpper().Trim().StartsWith("SWITCH ")
                || strCleaned.ToUpper().Trim().StartsWith("SWITCH(")
                || strCleaned.ToUpper().Trim().StartsWith("WHILE ")
                || strCleaned.ToUpper().Trim().StartsWith("WHILE(")
                || strCleaned.ToUpper().Trim().StartsWith("FOR ")
                || strCleaned.ToUpper().Trim().StartsWith("FOR(")
                || strCleaned.ToUpper().Trim().StartsWith("FOREACH ")
                || strCleaned.ToUpper().Trim().StartsWith("FOREACH(")
                || strCleaned.ToUpper().Trim().StartsWith("DO ")
                || strCleaned.ToUpper().Trim() == "DO"
                 || strCleaned.ToUpper().Trim().StartsWith("TRY ")
                  || strCleaned.ToUpper().Trim() == "TRY"
                || strCleaned.ToUpper().Trim().StartsWith("RETURN ")
                || strCleaned.ToUpper().Trim().StartsWith("RETURN;")
                || strCleaned.ToUpper().Trim().StartsWith("BREAK;")
                || strCleaned.ToUpper().Trim().Contains("}")
                )) {
                //WriteInputToTracelog();
                //WriteOrig(strInputLineOrig);
            } else {
                //WriteOrig(strInputLineOrig);
                if (strCleaned.Trim() != ""
                    && strCleaned.StartsWith("else ") == false
                    && strCleaned.Trim() != "finally"
                     && strCleaned.Trim().StartsWith("catch") == false
                    && strCleaned.StartsWith("else(") == false) {
                    //WriteInputToTracelog();
                }
            }



            boolLineProcessed = true;

            // boolMultipleStatementsPossible
            // boolFoundOpenBracket
            // If strCleaned.StartsWith, 
            // we need to wrap following statement in brackets if we do not encounter a 
            // { before we encounter a semicolon 
            if (strCleaned.ToUpper().Trim().StartsWith("IF ")
                || strCleaned.ToUpper().Trim().StartsWith("IF(")
                || strCleaned.ToUpper().Trim().StartsWith("FOR ")
                || strCleaned.ToUpper().Trim().StartsWith("FOR(")
                || strCleaned.ToUpper().Trim().StartsWith("FOREACH ")
                || strCleaned.ToUpper().Trim().StartsWith("FOREACH(")
                || strCleaned.ToUpper().Trim().StartsWith("DO ")
                || (strCleaned.ToUpper().Trim().StartsWith("WHILE ") && strCleaned.Contains(";") == false)
                || (strCleaned.ToUpper().Trim().StartsWith("WHILE(") && strCleaned.Contains(";") == false)
                ) {
                boolMultipleStatementsPossible = true;
                boolFoundOpenBracket = false;
            }

            if (strCleaned == "{" && boolMultipleStatementsPossible == true) {
                boolMultipleStatementsPossible = false;
                boolFoundOpenBracket = false;
            }

        }
        public string GetAppDirectoryForScript(string strScriptName) {
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + strScriptName;
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }
    }
}
