using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System;
using System.IO;
using System.Windows.Controls;
using System.Text;
using System.Linq;
using System.Windows.Media;
using Shell32;

namespace HotKeysMenu {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Dictionary<string, string> dict = new Dictionary<string, string>();
        bool _boolNamespace = false;
        bool _boolCommentsContinued = false;
        bool _boolFirstMethodFound = false;
        bool _boolFirstStatementInMethod = true;
        bool _boolLineProcessed;
        bool _boolMethod = false;
        bool _boolClass = false;
        bool _boolStruct = false;
        bool _boolMultipleStatementsPossible = false;
        bool _boolFoundOpenBracket = false;
        bool _boolTraceLoggingOn = true;
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
        string strStruct = "";

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
        int intMethodLineNum = 0;
        string strCurrentFile = "";
        int intBegin = 0;
        int intEnd = 0;
        int intInLine = 0;
        int intLit = 0;
        int intNestingLevel = 0;
        int intClassLevel = 0;
        int intStructLevel = 1;
        int intMethodLevel = 2;
        int intMethodCtr = 0;
        int[] strLocalVariableNestingArray;
        List<string> listComments = new List<string>();
        List<string> listMethodSourceCode = new List<string>();
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
        int intPageSize = 75;
        int intColumnWidth = 250;
        string strPreviousCategory = "";
        string strApplicationBinDebug = "";
        string myNewProjectSourcePath = "";
        public static string strPathToSearch = @"C:\SVNIA\trunk";

        public static string strSearchPattern = @"*.*";

    //    public static string strSearchExcludePattern = @"*.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy";
        public static string strSearchExcludePattern = @"";
        public static string strSearchText = @"notepad";

        public static string strLowerCaseSearchText = @"notepad";

        public static int intHits;

        public static bool _boolMatchCase = false;

        public ArrayList ArrayHotKeys;

        public static bool _boolUseRegularExpression = false;

        public static bool _boolStringFoundInFile;
        string strFindWhat = "";
        //===========
        string strFindWhatToUse = "";
        string strFileTypeToUse = "";
        string strExcludeToUse = "";
        string strFolderToUse = "";
        string strExecuteCategorize = "";
        string strHotkeysOnly = "";
        Methods myActions = new Methods();
        string strFolder = "";
        List<ComboBoxPair>  cbp = new List<ComboBoxPair>();
        List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();

    string strButtonPressed = "";
        private static string[] GetFiles(string sourceFolder, string filters, System.IO.SearchOption searchOption) {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
        }
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
            if (strWindowTitle.StartsWith("HotKeysMenu")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);

            SearchDialog:
            searchDialogRtn();
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string[] files = null;
            if (strHotkeysOnly == "All Executables") {
                try {
                    //List<FileInfo> myFileList = TraverseTree("*.exe", strFolder);
                    //Array.Resize(ref files, myFileList.Count);
                    //for (int i = 0; i < myFileList.Count; i++) {
                    //    var item = myFileList[i];

                    //    files[i] = item.FullName; 
                    //}
                    files = GetFiles(strFolder, "*.exe|*.lnk", SearchOption.AllDirectories);
                } catch (UnauthorizedAccessException e) {
                    myActions.MessageBoxShow(e.Message + " - line 162 in HotKeysMenu");
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
                if (files == null) {
                    myActions.MessageBoxShow("No executable files found");
                    goto SearchDialog;
                }
            } else {
                ArrayList arrayListScriptInfo = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
                Array.Resize(ref files, arrayListScriptInfo.Count);
                for (int i = 0; i < arrayListScriptInfo.Count; i++) {
                    var item = arrayListScriptInfo[i];
                    string[] myCols = item.ToString().Split('^');
                    // myCols[0] has scriptname after last hyphen
                    // myCols[1] has shortcut key
                    // myCols[5] has executable
                    files[i] = myCols[5];
                }

            }
            foreach (string myFile in files) {
                if (!myFile.ToUpper().Contains("VSHOST") && !myFile.ToUpper().Contains(@"\OBJ\")) {
                    AddToListCategoryMethods(myFile);
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
                string[] myArrayFields = item.Key.Replace("Category::", "").ToString().Split('^');
                {
                    intRow++;
                    if (intRow > 20) {
                        intRow = 1;
                        intCol++;
                    }
                    string strMethodName = myArrayFields[1];
                    string strCategory = myArrayFields[0];
                    string strExecutable = myArrayFields[2];

                    if (strCategory != strPreviousCategory) {
                        if (intRow > 18) {
                            intRow = 1;
                            intCol++;
                        }
                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Label;
                        myControlEntity.ID = "lbl" + strCategory.Replace(" ", "").Replace("<", "").Replace(">", "").Replace("#", "").Replace("+", "");
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
                    myControlEntity.ID = "myButton" + strMethodName.Replace(" ", "").Replace("<", "").Replace(">", "").Replace("=", "").Replace("!", "").Replace("#", "").Replace("+", "");
                    myControlEntity.Text = strMethodName;
                    myControlEntity.ToolTipx = strExecutable;

                    myControlEntity.RowNumber = intRow;
                    myControlEntity.ColumnNumber = intCol;
                    //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                }
            }



            intRow++;
            if (intRow > 15) {
                intRow = 1;
                intCol++;
            }
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "(" + intStartPageMethod.ToString() + " to " + intEndPageMethod.ToString() + ") of Total Executables: " + intTotalMethods.ToString();
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


            intRow++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSearch";
            myControlEntity.Text = "Search";
            myControlEntity.RowNumber = intRow;
            myControlEntity.ColumnNumber = intCol;
            //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
            //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



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
            if (strButtonPressed == "btnSearch") {
                //  myActions.MessageBoxShow(strButtonPressed);
                goto SearchDialog;
            }
            strMethod = strButtonPressed.Replace("myButton", "");
            string strExecutablex = "";
            if (strExecuteCategorize == "Execute") {
                foreach (var item in listCategoryMethod) {
                    string[] myArrayFields = item.Key.Replace("Category::", "").ToString().Split('^');

                    string strMethodName = myArrayFields[1].Replace(" ","");
                    string strCategory = myArrayFields[0];
                    if (strMethod == strMethodName) {
                        strExecutablex = myArrayFields[2];
                    }
                }
                if (strExecutablex != "") {
                    if (strExecutablex.EndsWith(".lnk")) {
                        
                             myActions.Run(GetShortcutTargetFile(strExecutablex), "");
                    } else {
                        myActions.Run(strExecutablex, "");
                    }
                    myActions.Sleep(1000);
          GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
          // Display main menu and go back to where you 
          // process input from main menu
          intWidth = (intCol + 1) * intColumnWidth;
          strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 650, intWidth, intWindowTop, intWindowLeft);
          goto DisplayWindowAgain;
        }
            } else {
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
                myControlEntity1.ID = "lbl" + strMethod.Replace("<", "").Replace(">", "").Replace("+", "");
                myControlEntity1.Text = strMethod;
                myControlEntity1.Width = 300;
                myControlEntity1.RowNumber = 0;
                myControlEntity1.ColumnNumber = 0;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                intRowCtr++;
                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.Label;
                myControlEntity1.ID = "lblCategory";
                myControlEntity1.Text = "Category (optional):";
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ColumnNumber = 0;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



                myControlEntity1.ControlEntitySetDefaults();
                myControlEntity1.ControlType = ControlType.ComboBox;
                if (strMethod.StartsWith("Window")) {
                    var debug = true;
                }
                myControlEntity1.SelectedValue = myActions.GetValueByKey("cbxCategory" + strMethod + "SelectedValue");
                string myFolder = myActions.GetValueByKey("cbxFolderSelectedValue");

                myControlEntity1.ID = "cbxCategory_" + GetSafeFilename(myFolder);
                myControlEntity1.RowNumber = intRowCtr;
                myControlEntity1.ToolTipx = "";
                //foreach (var item in alcbxFindWhat) {
                //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
                //}
                //myControlEntity1.ListOfKeyValuePairs = cbp;
                myControlEntity1.ComboBoxIsEditable = true;
                myControlEntity1.ColumnNumber = 1;

                myControlEntity1.ColumnSpan = 2;
                myListControlEntity1.Add(myControlEntity1.CreateControlEntity());


                // Get saved position of window from roaming..
                GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                // Display input dialog
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 1200, intWindowTop, intWindowLeft);
                // Get Values from input dialog and save to roaming






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
                    myFolder = myActions.GetValueByKey("cbxFolderSelectedValue");


                    string strCategory = myListControlEntity1.Find(x => x.ID == "cbxCategory_" + GetSafeFilename(myFolder)).SelectedValue;

                    myActions.SetValueByKey("cbxCategory" + strMethod.Replace("<", "").Replace(">", "").Replace("+", "") + "SelectedValue", strCategory);

                }
                GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                // Display main menu and go back to where you 
                // process input from main menu
                intWidth = (intCol + 1) * intColumnWidth;
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, intWidth, intWindowTop, intWindowLeft);
                goto DisplayWindowAgain;

            }
            
            myExit:
            myActions.ScriptStartedUpdateStats();
            Application.Current.Shutdown();
        }

        

        public string GetAppDirectoryForScript(string strScriptName) {
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + strScriptName;
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }

        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft) {
            strWindowLeft = myActions.GetValueByKey("WindowLeft");
            strWindowTop = myActions.GetValueByKey("WindowTop");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
        }

        private void AddToListCategoryMethods(string myFile) {
            Methods myActions = new Methods();
            
            int intLastSlash = myFile.LastIndexOf('\\');
            if (intLastSlash < 1) {
                myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                return;
            }
            string strFileNameOnly = myFile.Substring(intLastSlash + 1).Replace(".exe", "").Replace(".lnk", "");
            string strMyCategory = myActions.GetValueByKey("cbxCategory" + strFileNameOnly + "SelectedValue");
            if (strSearchText.Trim() == "") {
                if (!listCategoryMethod.ContainsKey(strMyCategory + "^" + strFileNameOnly + "^" + myFile)) {
                    listCategoryMethod.Add(strMyCategory + "^" + strFileNameOnly + "^" + myFile, strMyCategory + "^" + strFileNameOnly + "^" + myFile);
                }
            }
        }
        public string GetSafeFilename(string filename) {

            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

        }

        private void searchDialogRtn() {
            listCategoryMethod.Clear();
            intCol = 0;
            intRow = 0;
            intStartPageMethod = 0;
            intEndPageMethod = 0;
            intTotalMethods = 0;
            intPageSize = 75;
            intColumnWidth = 250;
            strPreviousCategory = "";
            strApplicationBinDebug = "";
            myNewProjectSourcePath = "";
            strPathToSearch = @"C:\SVNIA\trunk";
            strSearchPattern = @"*.*";
            strSearchExcludePattern = @"";
            strSearchText = @"notepad";
            strLowerCaseSearchText = @"notepad";
            intHits = 0;
            _boolMatchCase = false;
            _boolUseRegularExpression = false;
            _boolStringFoundInFile = false;
            strFindWhat = "";
            //
            string strApplicationBinDebug1 = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath1 = strApplicationBinDebug1.Replace("\\bin\\Debug", "");
            System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(myNewProjectSourcePath1, @"Text"));

            DisplaySelectFolderWindow:
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp2 = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp3 = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "Find Text In Files";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblHotkeysOnly";
            myControlEntity.Text = "HotkeysOnly";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("Hotkeys", "Hotkeys"));
            cbp.Add(new ComboBoxPair("All Executables", "All Executables"));
            myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.SelectedValue = myControlEntity.SelectedValue = myActions.GetValueByKey("cbxHotkeysOnlySelectedValue");
            if (myControlEntity.SelectedValue == null || myControlEntity.SelectedValue == "") {
                myControlEntity.SelectedValue = "--Select Item ---";
            }
            myControlEntity.ID = "cbxHotkeysOnly";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblExecuteCategorize";
            myControlEntity.Text = "ExecuteCategorize";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            cbp1.Clear();
            cbp1.Add(new ComboBoxPair("Execute", "Execute"));
            cbp1.Add(new ComboBoxPair("Categorize", "Categorize"));
            myControlEntity.ListOfKeyValuePairs = cbp1;
            myControlEntity.SelectedValue = myControlEntity.SelectedValue = myActions.GetValueByKey("cbxExecuteCategorizeSelectedValue");
            if (myControlEntity.SelectedValue == null || myControlEntity.SelectedValue == "") {
                myControlEntity.SelectedValue = "--Select Item ---";
            }
            myControlEntity.ID = "cbxExecuteCategorize";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblFindWhat";
            myControlEntity.Text = "FindWhat";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFindWhatSelectedValue");
            myControlEntity.ID = "cbxFindWhat";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxFindWhat) {
            //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
            //}
            //myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;

            myControlEntity.ColumnSpan = 2;
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

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.CheckBox;
            myControlEntity.ID = "chkMatchCase";
            myControlEntity.Text = "Match Case";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            string strMatchCase = myActions.GetValueByKey("chkMatchCase");

            if (strMatchCase.ToLower() == "true") {
                myControlEntity.Checked = true;
            } else {
                myControlEntity.Checked = false;
            }
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.CheckBox;
            myControlEntity.ID = "chkUseRegularExpression";
            myControlEntity.Text = "UseRegularExpression";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            string strUseRegularExpression = myActions.GetValueByKey("chkUseRegularExpression");
            if (strUseRegularExpression.ToLower() == "true") {
                myControlEntity.Checked = true;
            } else {
                myControlEntity.Checked = false;
            }
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            DisplaySelectFolderWindowAgain:
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            _boolMatchCase = myListControlEntity.Find(x => x.ID == "chkMatchCase").Checked;
            _boolUseRegularExpression = myListControlEntity.Find(x => x.ID == "chkUseRegularExpression").Checked;
            strHotkeysOnly = myListControlEntity.Find(x => x.ID == "cbxHotkeysOnly").SelectedValue;
            strExecuteCategorize = myListControlEntity.Find(x => x.ID == "cbxExecuteCategorize").SelectedValue;
            myActions.SetValueByKey("cbxHotkeysOnlySelectedValue", strHotkeysOnly);
            myActions.SetValueByKey("cbxExecuteCategorizeSelectedValue", strExecuteCategorize);
            strFindWhat = myListControlEntity.Find(x => x.ID == "cbxFindWhat").SelectedValue;
            //  string strFindWhatKey = myListControlEntity.Find(x => x.ID == "cbxFindWhat").SelectedKey;


            strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;
            myActions.SetValueByKey("chkMatchCase", _boolMatchCase.ToString());
            myActions.SetValueByKey("chkUseRegularExpression", _boolUseRegularExpression.ToString());
            myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);
            myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
            string settingsDirectory = "";
            if (strButtonPressed == "btnSelectFolder") {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");
                string str = "LastSearchFolder";


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
                    string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
                    string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
                    settingsDirectory = GetAppDirectoryForScript(myActions.ConvertFullFileNameToScriptPath(myNewProjectSourcePath));
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
                        if (strNewHostName != item._Key && item._Key != "--Select Item ---") {
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

            if (strButtonPressed == "btnOkay") {
                //if ((strFindWhat == "--Select Item ---" || strFindWhat == "")) {
                //    myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox; else press Cancel to Exit");
                //    goto DisplaySelectFolderWindow;
                //}

                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplaySelectFolderWindow;
                }



                strFindWhatToUse = strFindWhat;

                if (_boolUseRegularExpression) {
                    strFindWhatToUse = strFindWhatToUse.Replace(")", "\\)").Replace("(", "\\(");
                }





                strFolderToUse = strFolder;


            }


            strPathToSearch = strFolderToUse;

            strSearchPattern = strFileTypeToUse;

            strSearchExcludePattern = strExcludeToUse;

            strSearchText = strFindWhatToUse;

            strLowerCaseSearchText = strFindWhatToUse.ToLower();
            myActions.SetValueByKey("FindWhatToUse", strFindWhatToUse);
        }
        public static List<FileInfo> TraverseTree(string filterPattern, string root) {
            string[] arrayExclusionPatterns = strSearchExcludePattern.Split(';');
            for (int i = 0; i < arrayExclusionPatterns.Length; i++) {
                arrayExclusionPatterns[i] = arrayExclusionPatterns[i].ToLower().ToString().Replace("*", "");
            }
            List<FileInfo> myFileList = new List<FileInfo>();
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(20);

            if (!System.IO.Directory.Exists(root)) {
                MessageBox.Show(root + " - folder did not exist");
            }


            dirs.Push(root);

            while (dirs.Count > 0) {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have
                // discovery permission on a folder or file. It may or may not be acceptable 
                // to ignore the exception and continue enumerating the remaining files and 
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception 
                // will be raised. This will happen if currentDir has been deleted by
                // another application or thread after our call to Directory.Exists. The 
                // choice of which exceptions to catch depends entirely on the specific task 
                // you are intending to perform and also on how much you know with certainty 
                // about the systems on which this code will run.
                catch (UnauthorizedAccessException e) {
                    Console.WriteLine(e.Message);
                    continue;
                } catch (System.IO.DirectoryNotFoundException e) {
                    Console.WriteLine(e.Message);
                    continue;
                } catch (System.ArgumentException e) {
                    //      MessageBox.Show(e.Message + " CurrentDir = " + currentDir);
                    continue;
                }

                string[] files = null;
                try {
                    files = System.IO.Directory.GetFiles(currentDir, filterPattern);
                } catch (UnauthorizedAccessException e) {

                    Console.WriteLine(e.Message);
                    continue;
                } catch (System.IO.DirectoryNotFoundException e) {
                    Console.WriteLine(e.Message);
                    continue;
                } catch (System.IO.PathTooLongException e) {
                    Console.WriteLine(e.Message);
                    continue;
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files) {
                    try {
                        // Perform whatever action is required in your scenario.
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        bool boolFileHasGoodExtension = true;
                        //foreach (var item in arrayExclusionPatterns) {
                        //    if (fi.FullName.ToLower().Contains(item)) {
                        //        boolFileHasGoodExtension = false;
                        //    }
                        //}
                        if (boolFileHasGoodExtension) {
                            myFileList.Add(fi);
                        }
                        //    Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
                    } catch (System.IO.FileNotFoundException e) {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
            return myFileList;
        }
        public static string GetShortcutTargetFile(string shortcutFilename) {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new Shell32.Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null) {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

    }
}