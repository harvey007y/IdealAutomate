using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;
using System.Xml.Linq;
using AODL.Document.TextDocuments;
using AODL.Document.Content;
using DocumentFormat.OpenXml.Packaging;

namespace SetBreakpoints {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static string strPathToSearch = @"C:\SVNIA\trunk";

        public static string strSearchPattern = @"*.*";

        public static string strSearchExcludePattern = @"*.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy";

        public static string strSearchText = @"notepad";

        public static string strLowerCaseSearchText = @"notepad";

        public static int intHits;

        public static bool boolMatchCase = false;
        public static bool boolUseRegularExpression = false;

        public static bool boolStringFoundInFile;
        string strFindWhat = "";

        public static List<MatchInfo> matchInfoList;
        string _strFullFileName = "";
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
            if (strWindowTitle.StartsWith("SetBreakpoints")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            /*
             * 1x. Use FindTextinFiles dialog to ask what folder or file to search
             * 2x. Use the results to provide list of places to set breakpoints
             * 3. For each file, open in VS if new file and then use ctrl-g and line number
             *    to go to line and F9 to set breakpoint
             *    Problems:
             *    1. Do not display find results to user, but use the results to go to 
             *       the correct line in vs
             *    2. How do I know if the VS solution is open and how do I open it if not
             *       a. look in each parent folder of file for the first sln file you find
             *       b. if you do not find sln file, look for csproj file
             *       c. if you do not find either of those, write the line to
             *          an error file that you will display to user upon completion
             *       d. One you have found the sln or csproj file, see if
             *          any of the devenv processes running contain the solution
             *          or project name in the title. If they do, just activate
             *          that process, else start a process with the sln or csproj
             *       e. go to line in the file in and F9 to turn on debugging
             */
            FindTextInFoldersFiles();
            myActions.MessageBoxShow("Your Breakpoints have been set");
          
            goto myExit;
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Multiple Controls";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter Search Term";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "Hello World";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel2";
            myControlEntity.Text = "Select Website";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.ID = "myComboBox";
            myControlEntity.Text = "Hello World";
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            cbp.Add(new ComboBoxPair("google", "http://www.google.com"));
            cbp.Add(new ComboBoxPair("yahoo", "http://www.yahoo.com"));
            myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.SelectedValue = "http://www.yahoo.com";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.CheckBox;
            myControlEntity.ID = "myCheckBox";
            myControlEntity.Text = "Use new tab";
            myControlEntity.RowNumber = 2;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            string mySearchTerm = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string myWebSite = myListControlEntity.Find(x => x.ID == "myComboBox").SelectedValue;

            bool boolUseNewTab = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
            if (boolUseNewTab == true) {
                List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
                myWindowTitles.RemoveAll(item => item == "");
                if (myWindowTitles.Count > 0) {
                    myActions.ActivateWindowByTitle(myWindowTitles[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
                    myActions.TypeText("%(d)", 1500); // select address bar
                    myActions.TypeText("{ESC}", 1500);
                    myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab
                    myActions.TypeText("%(d)", 1500);
                    myActions.TypeText(myWebSite, 1500);
                    myActions.TypeText("{ENTER}", 1500);
                    myActions.TypeText("{ESC}", 1500);

                } else {
                    myActions.Run("iexplore", myWebSite);

                }
            } else {
                myActions.Run("iexplore", myWebSite);
            }

            myActions.Sleep(1000);
            if (myWebSite == "http://www.google.com") {
                myActions.TypeText("%(d)", 500);
                myActions.TypeText("{ESC}", 500);
                myActions.TypeText("{F6}", 500);
                myActions.TypeText("{TAB}", 500);
                myActions.TypeText("{TAB 2}", 500);
                myActions.TypeText("{ESC}", 500);
            }
            myActions.TypeText(mySearchTerm, 500);
            myActions.TypeText("{ENTER}", 500);


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
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
        private void FindTextInFoldersFiles() {
            Methods myActions = new Methods();
            myActions = new Methods();
            

            string strSavedDomainName = myActions.GetValueByKey("DomainName");
            if (strSavedDomainName == "") {
                strSavedDomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            //InitializeComponent();
            //this.Hide();







            DisplayFindTextInFilesWindow:
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
            myControlEntity.ID = "lblFileType";
            myControlEntity.Text = "FileType";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFileTypeSelectedValue");
            myControlEntity.ID = "cbxFileType";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "Here is an example: *.*";
            //foreach (var item in alcbxFileType) {
            //    cbp1.Add(new ComboBoxPair(item.ToString(), item.ToString()));
            //}
            //myControlEntity.ListOfKeyValuePairs = cbp1;
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblExclude";
            myControlEntity.Text = "Exclude";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxExcludeSelectedValue");
            myControlEntity.ID = "cbxExclude";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "Here is an example: *.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy";
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
            myControlEntity.Text = "Select Folder or File...";
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

            DisplayWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            boolMatchCase = myListControlEntity.Find(x => x.ID == "chkMatchCase").Checked;
            boolUseRegularExpression = myListControlEntity.Find(x => x.ID == "chkUseRegularExpression").Checked;

            strFindWhat = myListControlEntity.Find(x => x.ID == "cbxFindWhat").SelectedValue;
            //  string strFindWhatKey = myListControlEntity.Find(x => x.ID == "cbxFindWhat").SelectedKey;

            string strFileType = myListControlEntity.Find(x => x.ID == "cbxFileType").SelectedValue;
            //     string strFileTypeKey = myListControlEntity.Find(x => x.ID == "cbxFileType").SelectedKey;

            string strExclude = myListControlEntity.Find(x => x.ID == "cbxExclude").SelectedValue;
            //      string strExcludeKey = myListControlEntity.Find(x => x.ID == "cbxExclude").SelectedKey;

            string strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;
            myActions.SetValueByKey("chkMatchCase", boolMatchCase.ToString());
            myActions.SetValueByKey("chkUseRegularExpression", boolUseRegularExpression.ToString());
            myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);
            myActions.SetValueByKey("cbxFileTypeSelectedValue", strFileType);
            myActions.SetValueByKey("cbxExcludeSelectedValue", strExclude);
            myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
            string settingsDirectory = "";
            if (strButtonPressed == "btnSelectFolder") {
                FileFolderDialog dialog = new FileFolderDialog();
                dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");
                string str = "LastSearchFolder";


                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && (Directory.Exists(dialog.SelectedPath) || File.Exists(dialog.SelectedPath))) {
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
                    goto DisplayWindowAgain;
                }
            }
            string strFindWhatToUse = "";
            string strFileTypeToUse = "";
            string strExcludeToUse = "";
            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {
                if ((strFindWhat == "--Select Item ---" || strFindWhat == "")) {
                    myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }
                if ((strFileType == "--Select Item ---" || strFileType == "")) {
                    myActions.MessageBoxShow("Please enter File Type or select File Type from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }
                if ((strExclude == "--Select Item ---" || strExclude == "")) {
                    myActions.MessageBoxShow("Please enter Exclude or select Exclude from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }
                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }



                strFindWhatToUse = strFindWhat;

                if (boolUseRegularExpression) {
                    strFindWhatToUse = strFindWhatToUse.Replace(")", "\\)").Replace("(", "\\(");
                }


                strFileTypeToUse = strFileType;



                strExcludeToUse = strExclude;


                strFolderToUse = strFolder;


            }


            strPathToSearch = strFolderToUse;

            strSearchPattern = strFileTypeToUse;

            strSearchExcludePattern = strExcludeToUse;

            strSearchText = strFindWhatToUse;

            strLowerCaseSearchText = strFindWhatToUse.ToLower();
            myActions.SetValueByKey("FindWhatToUse", strFindWhatToUse);

            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            st.Start();
            intHits = 0;
            int intLineCtr;
            List<FileInfo> myFileList = new List<FileInfo>();
            if (File.Exists(strPathToSearch)) {
                System.IO.FileInfo fi = new System.IO.FileInfo(strPathToSearch);
                myFileList.Add(fi);
            } else {
                myFileList = TraverseTree(strSearchPattern, strPathToSearch);
            }
            int intFiles = 0;
            matchInfoList = new List<MatchInfo>();
            //         myFileList = myFileList.OrderBy(fi => fi.FullName).ToList();
            Parallel.ForEach(myFileList, myFileInfo => {
                intLineCtr = 0;
                boolStringFoundInFile = false;
                ReadFileToString(myFileInfo.FullName, intLineCtr, matchInfoList);
                if (boolStringFoundInFile) {
                    intFiles++;
                }
            });
            matchInfoList = matchInfoList.Where(mi => mi != null).OrderBy(mi => mi.FullName).ThenBy(mi => mi.LineNumber).ToList();
            List<string> lines = new List<string>();
            string prevFullName = "";
            bool boolSolutionFileFound = false;
            string strSolutionName = "";
          //  string strCurrLine = "";
 


            string strApplicationBinDebug1 = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath1 = strApplicationBinDebug1.Replace("\\bin\\Debug", "");

            settingsDirectory = GetAppDirectoryForScript(myActions.ConvertFullFileNameToScriptPath(myNewProjectSourcePath1));
            using (FileStream fs = new FileStream(settingsDirectory + @"\MatchInfo.txt", FileMode.Create)) {
                StreamWriter file = new System.IO.StreamWriter(fs, Encoding.Default);

                file.WriteLine(@"-- " + strSearchText + " in " + strPathToSearch + " from " + strSearchPattern + " excl  " + strSearchExcludePattern + " --");
                foreach (var item in matchInfoList) {
                    file.WriteLine("\"" + item.FullName + "\"(" + item.LineNumber + "," + item.LinePosition + "): " + item.LineText.Substring(0, item.LineText.Length > 5000 ? 5000 : item.LineText.Length));
                }
                int intUniqueFiles = matchInfoList.Select(x => x.FullName).Distinct().Count();
                st.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = st.Elapsed;
                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                file.WriteLine("RunTime " + elapsedTime);
                file.WriteLine(intHits.ToString() + " hits");
                // file.WriteLine(myFileList.Count().ToString() + " files");           
                file.WriteLine(intUniqueFiles.ToString() + " files with hits");
                file.Close();

                myActions.KillAllProcessesByProcessName("notepad++");
                // Get the elapsed time as a TimeSpan value.
                ts = st.Elapsed;
                // Format and display the TimeSpan value.
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                   ts.Hours, ts.Minutes, ts.Seconds,
                   ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                Console.WriteLine(intHits.ToString() + " hits");
                // Console.WriteLine(myFileList.Count().ToString() + " files");
                Console.WriteLine(intUniqueFiles.ToString() + " files with hits");
                Console.ReadLine();
                //  myActions.KillAllProcessesByProcessName("notepad++");
                string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                string strContent = settingsDirectory + @"\MatchInfo.txt";
             
                myActions.Run(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
                myActions.MessageBoxShow("RunTime: " + elapsedTime + "\n\r\n\rHits: " + intHits.ToString() + "\n\r\n\rFiles with hits: " + intUniqueFiles.ToString() + "\n\r\n\rPut Cursor on line and\n\r press Ctrl+Alt+N\n\rto view detail page. ");
                System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("MatchInfo.txt contains the lines where breakpoints will be set.\n\r\n\r If you want to make changes, edit matchinfo.txt and save.\n\r\n\rTo go ahead and set breakpoints, click yes, otherwise, click no to cancel");
                if (myResult == System.Windows.Forms.DialogResult.Yes) {
                    // TODO: 1. Read matchinfo.txt file
                    //       2. Loop thru each line except first and last
                    //       3. Set breakpoint for each line
                    using (StreamReader objSRFile = File.OpenText(strContent)) {
                        string strReadLine = "";
                        while ((strReadLine = objSRFile.ReadLine()) != null) {
                            if (strReadLine.StartsWith("--")) {
                                continue;
                            }
                            if (strReadLine.StartsWith("RunTime")) {
                                break;
                            }
                            string myOrigEditPlusLine = strReadLine;                            
                            List<string> myBeginDelim = new List<string>();
                            List<string> myEndDelim = new List<string>();
                            myBeginDelim.Add("\"");
                            myEndDelim.Add("\"");
                            FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

                            string myQuote = "\"";
                            delimParms.lines[0] = myOrigEditPlusLine;


                            myActions.FindDelimitedText(delimParms);
                            int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
                            if (intLastSlash < 1) {
                                myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                                break;
                            }
                            string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
                            string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
                            myBeginDelim.Clear();
                            myEndDelim.Clear();
                            myBeginDelim.Add("(");
                            myEndDelim.Add(",");
                            delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
                            delimParms.lines[0] = myOrigEditPlusLine;
                            myActions.FindDelimitedText(delimParms);
                            string strLineNumber = delimParms.strDelimitedTextFound;

                            //========
                            string strFullName = Path.Combine(strPathOnly, strFileNameOnly);
                            string strSolutionFullFileName = "";
                            if (strFullName != prevFullName) {
                                string currentTempName = strFullName;
                                while (currentTempName.IndexOf("\\") > -1) {
                                    currentTempName = currentTempName.Substring(0, currentTempName.LastIndexOf("\\"));
                                    FileInfo fi = new FileInfo(currentTempName);
                                    if (Directory.Exists(currentTempName)) {
                                        string[] files = null;
                                        try {
                                            files = System.IO.Directory.GetFiles(currentTempName, "*.sln");
                                            if (files.Length > 0) {
                                                strSolutionFullFileName = files[0];
                                                boolSolutionFileFound = true;
                                                strSolutionName = currentTempName.Substring(currentTempName.LastIndexOf("\\") + 1).Replace(".sln", "");
                                                List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
                                                myWindowTitles.RemoveAll(vsItem => vsItem == "");
                                                bool boolVSMatchingSolutionFound = false;
                                                foreach (var vsTitle in myWindowTitles) {
                                                    if (vsTitle.StartsWith(strSolutionName + " - ")) {
                                                        boolVSMatchingSolutionFound = true;
                                                        myActions.ActivateWindowByTitle(vsTitle, 3);
                                                        myActions.Sleep(1000);
                                                        myActions.TypeText("{ESCAPE}",500);
                                                        myBeginDelim = new List<string>();
                                                        myEndDelim = new List<string>();
                                                        myBeginDelim.Add("\"");
                                                        myEndDelim.Add("\"");
                                                        delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

                                                        myQuote = "\"";
                                                        delimParms.lines[0] = myOrigEditPlusLine;


                                                        myActions.FindDelimitedText(delimParms);
                                                        intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
                                                        if (intLastSlash < 1) {
                                                            myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                                                            break;
                                                        }
                                                        strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
                                                        strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
                                                        myBeginDelim.Clear();
                                                        myEndDelim.Clear();
                                                        myBeginDelim.Add("(");
                                                        myEndDelim.Add(",");
                                                        delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
                                                        delimParms.lines[0] = myOrigEditPlusLine;
                                                        myActions.FindDelimitedText(delimParms);
                                                        strLineNumber = delimParms.strDelimitedTextFound;
                                                        myActions.TypeText("{ESC}", 2000);
                                                        myActions.TypeText("%(f)", 1000);
                                                        myActions.TypeText("{DOWN}", 1000);
                                                        myActions.TypeText("{RIGHT}", 1000);
                                                        myActions.TypeText("f", 1000);
                                                        // myActions.TypeText("^(o)", 2000);
                                                        myActions.TypeText("%(d)", 1500);
                                                        myActions.TypeText(strPathOnly, 1500);
                                                        myActions.TypeText("{ENTER}", 500);
                                                        myActions.TypeText("%(n)", 500);
                                                        myActions.TypeText(strFileNameOnly, 1500);
                                                        myActions.TypeText("{ENTER}", 1000);
                                                        break;
                                                    }
                                                }
                                                if (boolVSMatchingSolutionFound == false) {
                                                    string strVSPath = myActions.GetValueByKeyGlobal("VS2013Path");
                                                    myActions.Run(strVSPath, strSolutionFullFileName);
                                                    myActions.Sleep(10000);
                                                    myActions.MessageBoxShow("When visual studio finishes loading, please click okay to continue");
                                                    myActions.TypeText("{ESCAPE}", 500);                                                  
                                                    boolSolutionFileFound = true;
                                                    strSolutionName = currentTempName.Substring(currentTempName.LastIndexOf("\\") + 1).Replace(".sln", "");
                                                    myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
                                                    myWindowTitles.RemoveAll(vsItem => vsItem == "");
                                                    boolVSMatchingSolutionFound = false;
                                                    foreach (var vsTitle in myWindowTitles) {
                                                        if (vsTitle.StartsWith(strSolutionName + " - ")) {
                                                            boolVSMatchingSolutionFound = true;
                                                            myActions.ActivateWindowByTitle(vsTitle, 3);
                                                            myActions.Sleep(1000);
                                                            myActions.TypeText("{ESCAPE}", 500);
                                                            myBeginDelim = new List<string>();
                                                             myEndDelim = new List<string>();
                                                            myBeginDelim.Add("\"");
                                                            myEndDelim.Add("\"");
                                                             delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

                                                             myQuote = "\"";
                                                            delimParms.lines[0] = myOrigEditPlusLine;


                                                            myActions.FindDelimitedText(delimParms);
                                                            intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
                                                            if (intLastSlash < 1) {
                                                                myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                                                                break;
                                                            }
                                                            strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
                                                            strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
                                                            myBeginDelim.Clear();
                                                            myEndDelim.Clear();
                                                            myBeginDelim.Add("(");
                                                            myEndDelim.Add(",");
                                                            delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
                                                            delimParms.lines[0] = myOrigEditPlusLine;
                                                            myActions.FindDelimitedText(delimParms);
                                                            strLineNumber = delimParms.strDelimitedTextFound;
                                                            myActions.TypeText("{ESC}", 2000);
                                                            myActions.TypeText("%(f)", 1000);
                                                            myActions.TypeText("{DOWN}", 1000);
                                                            myActions.TypeText("{RIGHT}", 1000);
                                                            myActions.TypeText("f", 1000);
                                                            // myActions.TypeText("^(o)", 2000);
                                                            myActions.TypeText("%(d)", 1500);
                                                            myActions.TypeText(strPathOnly, 1500);
                                                            myActions.TypeText("{ENTER}", 500);
                                                            myActions.TypeText("%(n)", 500);
                                                            myActions.TypeText(strFileNameOnly, 1500);
                                                            myActions.TypeText("{ENTER}", 1000);
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (boolVSMatchingSolutionFound == false) {
                                                    myActions.MessageBoxShow("Could not find visual studio for " + strSolutionName);
                                                }
                                                break;

                                            }
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
                                    }
                                }
                            }
                            myActions.TypeText("^(g)", 500);
                            myActions.TypeText(strLineNumber, 500);
                            myActions.TypeText("{ENTER}", 500);
                            myActions.TypeText("{F9}", 1000);
                            prevFullName = strFullName;
                        }
                        objSRFile.Close();
                    }
                }
            }
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
                        foreach (var item in arrayExclusionPatterns) {
                            if (fi.FullName.ToLower().Contains(item)) {
                                boolFileHasGoodExtension = false;
                            }
                        }
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
        public static void ReadFileToString(string fullFilePath, int intLineCtr, List<MatchInfo> matchInfoList) {
            while (true) {
                if (fullFilePath.EndsWith(".odt")
                    ) {
                    if (FindTextInWordPad(strSearchText, fullFilePath)) {
                        intHits++;
                        boolStringFoundInFile = true;
                        MatchInfo myMatchInfo = new MatchInfo();
                        myMatchInfo.FullName = fullFilePath;
                        myMatchInfo.LineNumber = 1;
                        myMatchInfo.LinePosition = 1;
                        myMatchInfo.LineText = strSearchText;
                        matchInfoList.Add(myMatchInfo);
                    }
                }
                if (fullFilePath.EndsWith(".doc") ||
        fullFilePath.EndsWith(".docx")

        ) {
                    if (FindTextInWord(strSearchText, fullFilePath)) {
                        intHits++;
                        boolStringFoundInFile = true;
                        MatchInfo myMatchInfo = new MatchInfo();
                        myMatchInfo.FullName = fullFilePath;
                        myMatchInfo.LineNumber = 1;
                        myMatchInfo.LinePosition = 1;
                        myMatchInfo.LineText = strSearchText;
                        matchInfoList.Add(myMatchInfo);
                    }
                }
                try {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.Open)) {
                        using (StreamReader sr = new StreamReader(fs, Encoding.Default)) {
                            string s;
                            string s_lower = "";
                            while ((s = sr.ReadLine()) != null) {
                                intLineCtr++;
                                if (boolUseRegularExpression) {
                                    if (boolMatchCase) {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(s, strSearchText, System.Text.RegularExpressions.RegexOptions.None)) {
                                            intHits++;
                                            boolStringFoundInFile = true;
                                            MatchInfo myMatchInfo = new MatchInfo();
                                            myMatchInfo.FullName = fullFilePath;
                                            myMatchInfo.LineNumber = intLineCtr;
                                            myMatchInfo.LinePosition = s.IndexOf(strSearchText) + 1;
                                            myMatchInfo.LineText = s;
                                            matchInfoList.Add(myMatchInfo);
                                        }
                                    } else {
                                        s_lower = s.ToLower();
                                        if (System.Text.RegularExpressions.Regex.IsMatch(s_lower, strLowerCaseSearchText, System.Text.RegularExpressions.RegexOptions.IgnoreCase)) {
                                            intHits++;
                                            boolStringFoundInFile = true;
                                            MatchInfo myMatchInfo = new MatchInfo();
                                            myMatchInfo.FullName = fullFilePath;
                                            myMatchInfo.LineNumber = intLineCtr;
                                            myMatchInfo.LinePosition = s_lower.IndexOf(strLowerCaseSearchText) + 1;
                                            myMatchInfo.LineText = s;
                                            matchInfoList.Add(myMatchInfo);
                                        }
                                    }
                                } else {
                                    if (boolMatchCase) {
                                        if (s.Contains(strSearchText)) {
                                            intHits++;
                                            boolStringFoundInFile = true;
                                            MatchInfo myMatchInfo = new MatchInfo();
                                            myMatchInfo.FullName = fullFilePath;
                                            myMatchInfo.LineNumber = intLineCtr;
                                            myMatchInfo.LinePosition = s.IndexOf(strSearchText) + 1;
                                            myMatchInfo.LineText = s;
                                            matchInfoList.Add(myMatchInfo);
                                        }
                                    } else {
                                        s_lower = s.ToLower();
                                        if (s_lower.Contains(strLowerCaseSearchText)) {

                                            intHits++;
                                            boolStringFoundInFile = true;
                                            MatchInfo myMatchInfo = new MatchInfo();
                                            myMatchInfo.FullName = fullFilePath;
                                            myMatchInfo.LineNumber = intLineCtr;
                                            myMatchInfo.LinePosition = s_lower.IndexOf(strLowerCaseSearchText) + 1;
                                            myMatchInfo.LineText = s;
                                            matchInfoList.Add(myMatchInfo);
                                        }
                                    }
                                }
                            }
                            return;
                        }

                    }
                } catch (FileNotFoundException ex) {
                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
                } catch (IOException ex) {
                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
                } catch (UnauthorizedAccessException ex) {
                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
                }
            }
        }
        protected static bool FindTextInWordPad(string text, string flname) {
            Methods myActions = new Methods();
            StringBuilder sb = new StringBuilder();
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            if (!File.Exists(strApplicationBinDebug + "\\aodlread\\settings.xml")) {
                if (!Directory.Exists(strApplicationBinDebug + "\\aodlread")) {
                    Directory.CreateDirectory(strApplicationBinDebug + "\\aodlread");
                }
                File.Copy(strApplicationBinDebug.Replace("\\bin\\Debug", "") + "\\aodlread\\settings.xml", strApplicationBinDebug + "\\aodlread\\settings.xml");
            }
            try {
                using (var doc = new TextDocument()) {
                    doc.Load(flname);

                    //The header and footer are in the DocumentStyles part. Grab the XML of this part
                    XElement stylesPart = XElement.Parse(doc.DocumentStyles.Styles.OuterXml);
                    //Take all headers and footers text, concatenated with return carriage
                    string stylesText = string.Join("\r\n", stylesPart.Descendants().Where(x => x.Name.LocalName == "header" || x.Name.LocalName == "footer").Select(y => y.Value));

                    //Main content
                    var mainPart = doc.Content.Cast<IContent>();
                    var mainText = String.Join("\r\n", mainPart.Select(x => x.Node.InnerText));

                    //Append both text variables
                    sb.Append(stylesText + "\r\n");

                    sb.Append(mainText);
                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    myActions.MessageBoxShow(ex.InnerException.ToString() + " - Line 5706 in ExplorerView");
                } else {
                    //  myActions.MessageBoxShow(ex.Message + " - Line 5708 in ExplorerView");
                }
            }
            if (sb.ToString().Contains(text)) {
                return true;
            } else {
                return false;
            }

        }

        protected static bool FindTextInWord(string text, string flname) {
            Methods myActions = new Methods();
            StringBuilder sb = new StringBuilder();
            string docText = null;
            try {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(flname, true)) {
                    docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream())) {
                        docText = sr.ReadToEnd();
                    }
                }

            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    myActions.MessageBoxShow(ex.InnerException.ToString() + " filename is:" + flname + " - Line 5733 in ExplorerView");
                } else {
                    myActions.MessageBoxShow(ex.Message + " filename is:" + flname + " - Line 5735 in ExplorerView");
                }
            }

            if (docText != null && docText.Contains(text)) {
                return true;
            } else {
                return false;
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
