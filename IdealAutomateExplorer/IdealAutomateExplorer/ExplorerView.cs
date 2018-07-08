#region Using directives

using System;

using System.Windows.Forms.Integration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using IdealAutomate.Core;
using System.Collections;
using WindowsInput;
using WindowsInput.Native;
using System.Windows;
using System.Linq;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;

using AODL;
using AODL.Document;
using AODL.Document.TextDocuments;
using System.Xml;
using System.Xml.Linq;
using AODL.Document.Content;
using DocumentFormat.OpenXml.Packaging;
using Hardcodet.Wpf.Samples.Pages;
using Hardcodet.Wpf.Samples;
using System.Management;
using DgvFilterPopup;






#endregion

namespace System.Windows.Forms.Samples {
    partial class ExplorerView : Form {
        private DirectoryView _dir;
        string strInitialDirectory = "";
        int _CurrentIndex = 0;
        static string _ExecutableFromClick = "";
        DataGridViewExt _CurrentDataGridView;
        DataGridViewExt dataGridView3;
        BindingSource _CurrentFileViewBindingSource = new BindingSource();
        SplitContainer _CurrentSplitContainer = new SplitContainer();
        bool boolStopEvent = false;
        bool _newTab = true;
        bool _ignoreSelectedIndexChanged = true;
        int _selectedTabIndex = 0;
        Rectangle _IconRectangle = new Rectangle();
        List<HotKeyRecord> listHotKeyRecords = new List<HotKeyRecord>();
        Dictionary<string, VirtualKeyCode> dictVirtualKeyCodes = new Dictionary<string, VirtualKeyCode>();
        List<BindingSource> listBindingSource = new List<BindingSource>();
        List<SplitContainer> listSplitContainer = new List<SplitContainer>();
        List<int> listLoadedIndexes = new List<int>();
        private Point _imageLocation = new Point(13, 5);
        private Point _imgHitArea = new Point(13, 2);
        const int LEADING_SPACE = 12;
        const int CLOSE_SPACE = 15;
        const int CLOSE_AREA = 15;

        private const int SC_MAXIMIZE = 61488;
        private const int WM_CLOSE = 16; //0x10
        private const int WM_SYSCOMMAND = 274;
        private bool _Panel2KeyPress = false;
        private bool _WordPadLoaded = false;
        private bool _WebBrowserLoaded = false;
        private bool _NotepadppLoaded = false;
        ArrayList _myArrayList;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public IntPtr myhWnd;
        static StringBuilder _searchErrors = new StringBuilder();


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hwnd, uint msg, long wParam, long lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int GetKeyNameText(int lParam, [Out] StringBuilder lpString,
    int nSize);

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState", SetLastError = true)]
        private static extern bool NativeGetKeyboardState([Out] byte[] keyStates);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();
        /*- Retrieves Id of the thread that created the specified window -*/
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessID);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowTextA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, string WinTitle, int MaxLength);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowTextLengthA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetWindowTextLengthx(int hwnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        public static extern int SetWindowLongA([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, int nIndex, int dwNewLong);


        private IntPtr _appHandle;
        Process _proc;
        public static string strPathToSearch = @"C:\SVNIA\trunk";

        public static string strSearchPattern = @"*.*";

        public static string strSearchExcludePattern = @"*.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy";

        public static string strSearchText = @"notepad";

        public static string strLowerCaseSearchText = @"notepad";

        public static int intHits;

        public static bool boolMatchCase = false;

        public ArrayList ArrayHotKeys;

        public static bool boolUseRegularExpression = false;

        public static bool boolStringFoundInFile;
        string strFindWhat = "";

        public static List<MatchInfo> matchInfoList;
        string _strFullFileName = "";
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        String Url = string.Empty;
        SplitContainer mySplitContainer = new SplitContainer();
        int _selectedRow = 0;
        private const int SWP_NOOWNERZORDER = 0x200;
        private const int SWP_NOREDRAW = 0x8;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int WS_EX_MDICHILD = 0x40;
        private const int SWP_FRAMECHANGED = 0x20;
        private const int SWP_NOACTIVATE = 0x10;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOSIZE = 0x1;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_CHILD = 0x40000000;
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar


        public ExplorerView() {
            InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerView));

            Methods myActions = new Methods();

            for (int i = 0; i < 20; i++) {
                BindingSource myNewBindingSource = new BindingSource();
                listBindingSource.Add(myNewBindingSource);
            }
            for (int i = 0; i < 20; i++) {
                SplitContainer myNewSplitContainer = new SplitContainer();
                listSplitContainer.Add(myNewSplitContainer);
            }
            // _CurrentDataGridView = new DataGridViewExt(myActions.GetValueByKey("InitialDirectory" + i.ToString()));
            _CurrentFileViewBindingSource = FileViewBindingSource;
            _CurrentSplitContainer = mySplitContainer;
            tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tabControl1.HotTrack = true;

            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.Padding = new System.Drawing.Point(20, 3);
            tabControl1.MouseClick += TabControl1_MouseClick;

        }



        private void TabControl1_MouseClick(object sender, MouseEventArgs e) {
            // _CurrentDataGridView.ClearSelection();
            //Looping through the controls.
            int removedIndex = -1;
            for (int i = 0; i < this.tabControl1.TabPages.Count - 1; i++) {
                Rectangle r = tabControl1.GetTabRect(i);
                //Getting the position of the "x" mark.
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                if (closeButton.Contains(e.Location)) {
                    if (tabControl1.TabPages.Count == 2) {
                        MessageBox.Show("You can not remove all tabs");
                        break;
                    }
                    if (MessageBox.Show("Would you like to Close this Tab?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        this.tabControl1.TabPages.RemoveAt(i);
                        var itemToRemove = listLoadedIndexes.SingleOrDefault(l => l == i);
                        if (itemToRemove != -1) {
                            listLoadedIndexes.Remove(itemToRemove);
                        }
                        for (int j = 0; j < listLoadedIndexes.Count; j++) {
                            if (listLoadedIndexes[j] > i) {
                                listLoadedIndexes[j] = listLoadedIndexes[j] - 1;
                            }
                        }

                        removedIndex = i;
                        break;
                    }
                }
            }
            Methods myActions = new Methods();
            int nextIndex = 0;
            if (removedIndex > -1) {
                for (int i = removedIndex; i < this.tabControl1.TabPages.Count - 1; i++) {
                    nextIndex = i + 1;
                    string nextInitialDirectory = myActions.GetValueByKey("InitialDirectory" + nextIndex.ToString());
                    myActions.SetValueByKey("InitialDirectory" + i.ToString(), nextInitialDirectory);
                    string nextInitialDirectorySelectedRow = myActions.GetValueByKey("InitialDirectory" + nextIndex.ToString() + "SelectedRow");
                    if (nextInitialDirectorySelectedRow != "") {
                        myActions.SetValueByKey("InitialDirectory" + i.ToString() + "SelectedRow", nextInitialDirectorySelectedRow);
                    }

                    listSplitContainer[i] = listSplitContainer[nextIndex];
                    listBindingSource[i] = listBindingSource[nextIndex];
                }


                myActions.SetValueByKey("NumOfTabs", this.tabControl1.TabPages.Count.ToString());
            }
            if (_ignoreSelectedIndexChanged == true) {
                _ignoreSelectedIndexChanged = false;
                //  return;
            }

            // Adding a new tab
            if (tabControl1.SelectedIndex == tabControl1.TabCount - 1) {
                strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                // Set Initial Directory to My Documents
                string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
                if (Directory.Exists(strSavedDirectory)) {
                    strInitialDirectory = strSavedDirectory;
                }
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



                DisplayWindowAgain:
                string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
                LineAfterDisplayWindow:
                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    this.Cursor = Cursors.Default;
                    return;
                }


                string strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
                //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;

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
                        string strApplicationBinDebug = Application.StartupPath;
                        string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

                        string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
                        goto DisplayWindowAgain;
                    }
                }

                string strFolderToUse = "";
                if (strButtonPressed == "btnOkay") {

                    if ((strFolder == "--Select Item ---" || strFolder == "")) {
                        myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                        goto DisplayFindTextInFilesWindow;
                    }

                    strFolderToUse = strFolder;
                    myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strFolder);
                    _CurrentIndex = tabControl1.SelectedIndex;

                }

                AddDataGridToTab(strInitialDirectory);

                myActions.SetValueByKey("NumOfTabs", (tabControl1.TabCount).ToString());
                strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                // Set Initial Directory to My Documents
                string strSavedDirectory1 = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
                if (Directory.Exists(strSavedDirectory1)) {
                    strInitialDirectory = strSavedDirectory1;
                }
                _dir = new DirectoryView(strInitialDirectory, _myArrayList);
                this._CurrentFileViewBindingSource.DataSource = _dir;
                tabControl1.TabPages[tabControl1.SelectedIndex].Text = _dir.FileView.Name;
                tabControl1.TabPages[tabControl1.SelectedIndex].ToolTipText = _dir.FileView.FullName;

            }
            this.Cursor = Cursors.WaitCursor;           
            myActions.SetValueByKey("CurrentIndex", tabControl1.SelectedIndex.ToString());
            _CurrentDataGridView = (DataGridViewExt)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0].Controls[0].Controls[0];
            _CurrentFileViewBindingSource = listBindingSource[tabControl1.SelectedIndex];
            _dir = (DirectoryView)this._CurrentFileViewBindingSource.DataSource;
            _CurrentSplitContainer = listSplitContainer[tabControl1.SelectedIndex];
            _CurrentSplitContainer = (SplitContainer)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            _CurrentSplitContainer.SplitterMoved -= _CurrentSplitContainer_SplitterMoved;
            _CurrentSplitContainer.MouseEnter -= _CurrentSplitContainer_MouseEnter;
            _CurrentSplitContainer.MouseLeave -= _CurrentSplitContainer_MouseLeave;

            _CurrentSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(_CurrentSplitContainer_SplitterMoved);
            _CurrentSplitContainer.MouseEnter += new System.EventHandler(_CurrentSplitContainer_MouseEnter);
            _CurrentSplitContainer.MouseLeave += new System.EventHandler(_CurrentSplitContainer_MouseLeave);
            if (listLoadedIndexes.Contains(tabControl1.SelectedIndex)) {
                string strCurrentPath = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
                _newTab = true;
                cbxCurrentPath.Text = strCurrentPath;
                cbxCurrentPath.SelectedValue = strCurrentPath;
                this.Cursor = Cursors.Default;
                return;
            } else {
                _newTab = true;
                RefreshDataGrid();
                listLoadedIndexes.Add(tabControl1.SelectedIndex);
            }
            _selectedRow = myActions.GetValueByKeyAsInt("InitialDirectory" + tabControl1.SelectedIndex.ToString() + "SelectedRow");
            if (_selectedRow > 0) {
                if (_CurrentDataGridView.Rows.Count > _selectedRow && _CurrentDataGridView.Rows[_selectedRow].Cells.Count > 1) {
                    //_CurrentDataGridView.Rows[selectedRow].Cells[1].Selected = true;
                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "True") {

                        DataGridViewCell c = _CurrentDataGridView.Rows[_selectedRow].Cells[1];

                        //    _CurrentDataGridView.FirstDisplayedScrollingRowIndex = _selectedRow;
                        //    _CurrentDataGridView.PerformLayout();
                        string fileName = _CurrentDataGridView.Rows[_selectedRow].Cells["FullName"].Value.ToString();
                        if (fileName.EndsWith(".url")

                         ) {
                            //Close the running process.
                            if (_appHandle != IntPtr.Zero) {
                                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                System.Threading.Thread.Sleep(1000);
                                _appHandle = IntPtr.Zero;
                            }
                            ////tries to start the process 
                            //try {                               
                            //    myActions.KillAllProcessesByProcessName("iexplore");
                            //    _proc = Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", GetInternetShortcut(fileName));
                            //} catch (Exception) {
                            //    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    return;
                            //}


                            //try {
                            //    System.Threading.Thread.Sleep(500);
                            //    while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            //        System.Threading.Thread.Sleep(10);
                            //        _proc.Refresh();
                            //    }

                            //    _proc.WaitForInputIdle();
                            //    _appHandle = _proc.MainWindowHandle;



                            //SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                            //SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                            //} catch (Exception ex) {

                            //    MessageBox.Show(ex.Message);
                            //}
                            //if (toolStripComboBox1.Text != "")
                            //    Url = toolStripComboBox1.Text;
                            _WordPadLoaded = false;
                            _NotepadppLoaded = false;
                            _WebBrowserLoaded = true;
                            Url = GetInternetShortcut(fileName);
                            InitializeComponentWebBrowser();
                            webBrowser1.ScriptErrorsSuppressed = true;
                            webBrowser1.Navigate(Url);

                            _CurrentSplitContainer.Panel2.Controls.Clear();
                            FlowLayoutPanel flp = new FlowLayoutPanel();
                            flp.Dock = DockStyle.Fill;

                            flp.Controls.Add(toolStrip1);
                            flp.Controls.Add(webBrowser1);
                            webBrowser1.Size = new System.Drawing.Size(_CurrentSplitContainer.Panel2.ClientSize.Width, _CurrentSplitContainer.Panel2.Height - 50);

                            flp.Controls.Add(statusStrip1);
                            _CurrentSplitContainer.Panel2.Controls.Add(flp);

                            webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(webpage_ProgressChanged);
                            webBrowser1.DocumentTitleChanged += new EventHandler(webpage_DocumentTitleChanged);
                            webBrowser1.StatusTextChanged += new EventHandler(webpage_StatusTextChanged);
                            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webpage_Navigated);
                            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted);
                        }
                        if (fileName.EndsWith(".rtf")
                            || fileName.EndsWith(".odt")
                            || fileName.EndsWith(".doc")
                            || fileName.EndsWith(".docx")
                            ) {
                            _NotepadppLoaded = false;
                            _WebBrowserLoaded = false;
                            _WordPadLoaded = true;
                            //Close the running process
                            _CurrentSplitContainer.Panel2.Controls.Clear();
                            if (_appHandle != IntPtr.Zero) {
                                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                System.Threading.Thread.Sleep(1000);
                                _appHandle = IntPtr.Zero;
                            }
                            //tries to start the process 
                            TryToCloseAllOpenFilesInTab();
                            try {
                                ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Windows NT\Accessories\wordpad.exe", "\"" + fileName + "\"");
                                psi.WindowStyle = ProcessWindowStyle.Minimized;
                                _proc = Process.Start(psi);
                            } catch (Exception) {
                                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Cursor = Cursors.Default;
                                return;
                            }


                            System.Threading.Thread.Sleep(500);
                            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                System.Threading.Thread.Sleep(10);
                                _proc.Refresh();
                            }

                            _proc.WaitForInputIdle();
                            _appHandle = _proc.MainWindowHandle;

                            SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                            AddAppHandleToOpenFiles(fileName, _appHandle);
                            // Remove border and whatnot
                            SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                            MoveWindow(_appHandle, 0, 0, _CurrentSplitContainer.Panel2.Width - 5, _CurrentSplitContainer.Panel2.Height, true);
                            //  SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                            //  SetTitle(_dir.FileView);
                        } else {
                            if (fileName.EndsWith(".txt")
                                || fileName.EndsWith(".bat")
                                || fileName.EndsWith(".cs")
                                || fileName.EndsWith(".xaml")
                                || fileName.EndsWith(".sln")
                                || fileName.EndsWith(".csproj")
                                || fileName.EndsWith(".resx")
                                 || fileName.EndsWith(".js")
                                  || fileName.EndsWith(".css")
                                  || fileName.EndsWith(".html")
                                  || fileName.EndsWith(".htm")
                                  || fileName.EndsWith(".xml")
                                  || fileName.EndsWith(".sql")
                                  || fileName.EndsWith(".asp")
                                  || fileName.EndsWith(".inc")
                                  || fileName.EndsWith(".dinc")
                                  || fileName.EndsWith(".aspx")
                                  || fileName.EndsWith(".csv")) {
                                _WordPadLoaded = false;
                                _NotepadppLoaded = true;
                                _WebBrowserLoaded = false;
                                //Close the running process
                                _CurrentSplitContainer.Panel2.Controls.Clear();
                                if (_appHandle != IntPtr.Zero) {
                                    PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                    System.Threading.Thread.Sleep(1000);
                                    _appHandle = IntPtr.Zero;
                                }
                                //tries to start the process 
                                try {
                                    myActions.KillAllProcessesByProcessName("notepad++");
                                    if (!File.Exists(@"C:\Program Files (x86)\Notepad++\notepad++.exe")) {
                                        myActions.MessageBoxShow(" You need to download notepad++ to use this feature.\n\r\n\rFile not found: " + @"C:\Program Files (x86)\Notepad++\notepad++.exe");
                                    } else {
                                        _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", fileName);
                                    }
                                } catch (Exception) {
                                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }


                                //    System.Threading.Thread.Sleep(500);
                                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                    System.Threading.Thread.Sleep(10);
                                    _proc.Refresh();
                                }

                                _proc.WaitForInputIdle();
                                _appHandle = _proc.MainWindowHandle;

                                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                                AddAppHandleToOpenFiles(fileName, _appHandle);
                                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                                SetTitle(_dir.FileView);
                            }
                        }



                    }
                }

            }
            _newTab = true;
            this.Cursor = Cursors.Default;
        }

        private void TabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
            e.DrawBackground();
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.LightGray), e.Bounds);
            if (e.Index == tabControl1.SelectedIndex) {
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            }
            e.DrawFocusRectangle();
            //This code will render a "x" mark at the end of the Tab caption.
            if (e.Index != tabControl1.TabCount - 1) {
                e.Graphics.DrawString("x", e.Font, new SolidBrush(Color.Black), e.Bounds.Right - CLOSE_AREA, e.Bounds.Top + 4);
            }

            e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, e.Font, new SolidBrush(Color.Black), e.Bounds.Left + LEADING_SPACE, e.Bounds.Top + 4);


        }

        #region Helper Methods
        private void SetTitle(FileView fv) {
            // Clicked on the Name property, update the title
            this.Text = fv.Name + " - Ideal Automate Explorer";
            this.Icon = fv.Icon;
            cbxCurrentPath.Text = fv.FullName;
            cbxCurrentPath.SelectedValue = fv.FullName;



            string[] strInitialDirectoryArray = new string[1];
            strInitialDirectoryArray[0] = fv.FullName;
            Methods myActions = new Methods();

            if (tabControl1.TabCount - 1 != tabControl1.SelectedIndex) {
                tabControl1.TabPages[tabControl1.SelectedIndex].Text = fv.Name;
                myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), fv.FullName);
            }

            //File.WriteAllLines(Path.Combine(Application.StartupPath, @"Text\InitialDirectory.txt"), strInitialDirectoryArray);
        }

        private void toolStripMenuItem4_CheckStateChanged(object sender, EventArgs e) {
            ToolStripMenuItem item = (sender as ToolStripMenuItem);

            foreach (ToolStripMenuItem child in viewSplitButton.DropDownItems) {
                if (child != item) {
                    child.Checked = false;
                } else {
                    item.Checked = true;
                }
            }
        }

        // Clear the one of many list
        private void ClearItems(ToolStripMenuItem selected) {
            // Clear items
            foreach (ToolStripMenuItem child in viewSplitButton.DropDownItems) {
                if (child != selected) {
                    child.Checked = false;
                }
            }
        }

        private bool DoActionRequired(object sender) {
            ToolStripMenuItem item = (sender as ToolStripMenuItem);
            bool doAction;

            // Clear other items
            ClearItems(item);

            // Check this one
            if (!item.Checked) {
                item.Checked = true;
                doAction = false;
            } else {
                // Item click and wasn't previously checked - Do action
                doAction = true;
            }

            return doAction;
        }
        #endregion

        #region Event Handlers        
        private void ExplorerView_Load(object sender, EventArgs e) {
            DeleteOpenFiles();


            this.Cursor = Cursors.WaitCursor;
            Methods myActions = new Methods();
            string launchMode = myActions.GetValueByKey("LaunchMode");
            if (launchMode == "Admin") {
                toolStripComboBox2.SelectedIndex = 0;
            } else {
                toolStripComboBox2.SelectedIndex = 1;
            }
            //_CurrentDataGridView.ClearSelection();

            _CurrentSplitContainer.Height = Screen.PrimaryScreen.WorkingArea.Size.Height - 155;

            int _CurrentSplitContainerWidth = myActions.GetValueByKeyAsInt("_CurrentSplitContainerWidth");

            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (_CurrentSplitContainerWidth > 0) {
                _CurrentSplitContainer.SplitterDistance = _CurrentSplitContainerWidth;
            } else {
                _CurrentSplitContainer.Width = Screen.PrimaryScreen.Bounds.Width;
            }

            if (detailsMenuItemChecked == "True") {
                _CurrentSplitContainer.Panel2Collapsed = false;
                this.detailsMenuItem.Checked = true;
                this.listMenuItem.Checked = false;
                //tries to start the process 
                try {
                    if (!File.Exists(@"C:\Program Files\Windows NT\Accessories\wordpad.exe")) {
                        myActions.MessageBoxShow(" File not found: " + @"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                    } else {
                        ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                        psi.WindowStyle = ProcessWindowStyle.Minimized;
                        _proc = Process.Start(psi);
                    }
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                //disables button and textbox
                //txtProcess.Enabled = false;
                //btnStart.Enabled = false;

                //host the started process in the panel 
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            } else {
                _CurrentSplitContainer.Panel2Collapsed = true;
                this.detailsMenuItem.Checked = false;
                this.listMenuItem.Checked = true;
            }



            int intTotalSavingsForAllScripts = 0;

            int numOfTabs = myActions.GetValueByKeyAsInt("NumOfTabs");
            // default to desktop if they have no tabs
            if (numOfTabs < 2) {
                numOfTabs = 2;
                myActions.SetValueByKey("NumOfTabs", numOfTabs.ToString());
                myActions.SetValueByKey("InitialDirectory0", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                myActions.SetValueByKey("InitialDirectory1", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            }

            // for each tab, add name and tooltip;

            // if an initial directory does not exist for a tab,
            // default to the documents folder for that tab
            for (int i = 0; i < numOfTabs - 1; i++) {
                strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                // Set Initial Directory to My Documents
                string strSavedDirectory1 = myActions.GetValueByKey("InitialDirectory" + i.ToString());
                if (Directory.Exists(strSavedDirectory1)) {
                    strInitialDirectory = strSavedDirectory1;
                }

                // do not fill the directory because first time through
                // we are just addding name and tooltip to each tab
                _dir = new DirectoryView(strInitialDirectory, false, _myArrayList);
                this._CurrentFileViewBindingSource.DataSource = _dir;

                tabControl1.TabPages[i].Text = _dir.FileView.Name;
                tabControl1.TabPages[i].ToolTipText = _dir.FileView.FullName;
                _CurrentIndex = i;
                AddDataGridToTab(strInitialDirectory);


            }

            // fill the directory for the selected tab with everything under 
            // that tabs initial directory

            // we are skipping the following load of the directory because
            // it seems to be redundant, but we may need to reinstate
            // if categories do not expand and collapse correctly
            goto testskip;
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents

            string strSavedDirectory2 = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
            if (Directory.Exists(strSavedDirectory2)) {
                strInitialDirectory = strSavedDirectory2;
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;
            tabControl1.TabPages[tabControl1.SelectedIndex].Text = _dir.FileView.Name;
            _CurrentIndex = tabControl1.SelectedIndex;
            // AddDataGridToTab();

            testskip:
            int currentIndex = myActions.GetValueByKeyAsInt("CurrentIndex");
            tabControl1.SelectedIndex = currentIndex;
            _CurrentDataGridView = (DataGridViewExt)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0].Controls[0].Controls[0];
            _CurrentFileViewBindingSource = listBindingSource[tabControl1.SelectedIndex];
            _CurrentSplitContainer = listSplitContainer[tabControl1.SelectedIndex];
            _CurrentSplitContainer = (SplitContainer)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];

            _CurrentSplitContainer.SplitterMoved -= _CurrentSplitContainer_SplitterMoved;

            _CurrentSplitContainer.MouseEnter -= _CurrentSplitContainer_MouseEnter;
            _CurrentSplitContainer.MouseLeave -= _CurrentSplitContainer_MouseLeave;

            _CurrentSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(_CurrentSplitContainer_SplitterMoved);
            _CurrentSplitContainer.MouseEnter += new System.EventHandler(_CurrentSplitContainer_MouseEnter);
            _CurrentSplitContainer.MouseLeave += new System.EventHandler(_CurrentSplitContainer_MouseLeave);
            //if (!listLoadedIndexes.Contains(tabControl1.SelectedIndex)) {
            //    RefreshDataGrid();
            //    listLoadedIndexes.Add(tabControl1.SelectedIndex);
            //}
            goto skipSecondLoad;
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            skipSecondLoad:
            string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            string settingsDirectory =
       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            string fileName = cbxCurrentPath.Name + ".txt";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            ArrayList alHosts = new ArrayList();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            cbp.Clear();
            //  cbxCurrentPath.Items.Clear();
            //  cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
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
                    if (keyvalue[0] != "--Select Item ---" && keyvalue[0] != "") {
                        cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));

                    }
                }
                objSRFile.Close();
            }

            foreach (var item in cbp) {
                cbxCurrentPath.Items.Add(item);
            }
            cbxCurrentPath.DisplayMember = "_Value";

            // Set Size column to right align
            DataGridViewColumn col = this._CurrentDataGridView.Columns["Size"];

            if (null != col) {
                DataGridViewCellStyle style = col.HeaderCell.Style;

                style.Padding = new Padding(style.Padding.Left, style.Padding.Top, 6, style.Padding.Bottom);
                style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Select first item.
            col = this._CurrentDataGridView.Columns["Name"];

            if (null != col) {
                this._CurrentDataGridView.Rows[0].Cells[col.Index].Selected = true;
            }
            AddGlobalHotKeys();

            myActions.SetValueByKey("ExpandCollapseAll", "");
            if (!listLoadedIndexes.Contains(tabControl1.SelectedIndex)) {
                RefreshDataGrid();
                listLoadedIndexes.Add(tabControl1.SelectedIndex);
            }
            //    myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), tabControl1.TabPages[tabControl1.SelectedIndex].Text);


            this.Cursor = Cursors.Default;

        }

        private void DeleteOpenFiles() {
            string fileName = "OpenFiles.txt";
            string strApplicationBinDebug = Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

            string settingsDirectory =
Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                try {
                    File.Delete(settingsPath);
                } catch (Exception ex) {
                    MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (this._CurrentDataGridView.Columns[e.ColumnIndex].Name == "SizeCol") {
                long size = (long)e.Value;

                if (size < 0) {
                    e.Value = "";
                } else {
                    size = ((size + 999) / 1000);
                    e.Value = size.ToString() + " " + "KB";
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e == null) {
                this.Cursor = Cursors.Default;
                return;
            }
            Icon icon = (e.Value as Icon);
            if (e.Value == null) {
                this.Cursor = Cursors.Default;
                return;
            }
            if (null != icon) {
                using (SolidBrush b = new SolidBrush(e.CellStyle.BackColor)) {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }
                _IconRectangle = e.CellBounds;
                // Draw right aligned icon (1 pixed padding)
                try {
                    int myX = e.CellBounds.Width - icon.Width - 1;
                    int myY = e.CellBounds.Y + 1;
                    if (myX < 1) {
                        myX = 15;
                    }
                    if (myY < 1) {
                        myY = 15;
                    }
                    e.Graphics.DrawIcon(icon, myX, myY);
                } catch (Exception ex) {

                    MessageBox.Show(ex.Message + " - Line 500 in ExplorerView");
                } finally {
                    e.Handled = true;
                }

            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            if (e == null) {
                return;
            }
            if (e.RowIndex > ((DataGridViewExt)sender).Rows.Count - 1) {
                return;
            }
            if (e.RowIndex < 0 || ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["NameCol"].Value == null) {
                return;
            }
            string fileName = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["FullName"].Value.ToString();
            string scriptName = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["NameCol"].Value.ToString();
            Methods myActions = new Methods();
            string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToPublicPath(fileName) + "\\" + scriptName.Replace(".txt", "").Replace(".rtf", ""));
            string strNestingLevel = "";
            if (((DataGridViewExt)sender).Rows.Count == 0) {
                return;
            }
            strNestingLevel = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["NestingLevel"].Value.ToString();
            int nestingLevel = 0;
            Int32.TryParse(strNestingLevel, out nestingLevel);
            int indent = nestingLevel * 20;
            if (categoryState == "Collapsed" || categoryState == "Expanded") {
                ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["NameCol"].Style.Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["NameCol"].Style.Padding = new Padding(indent, 0, 0, 0);
            } else {
                ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["NameCol"].Style.Padding = new Padding(indent, 0, 0, 0);
                DataGridViewCell iconCell = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["dataGridViewImageColumn1"];
            }


            if (_selectedRow > 0 && e.RowIndex == _selectedRow) {
                if (_selectedRow < _CurrentDataGridView.Rows.Count) {
                    if (_CurrentDataGridView.Rows[_selectedRow].Cells.Count > 1) {

                        _CurrentDataGridView.Rows[_selectedRow].Cells[1].Selected = true;
                        //  _CurrentDataGridView.FirstDisplayedScrollingRowIndex = _selectedRow;
                        //  _CurrentDataGridView.PerformLayout();
                    }
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            // Call Active on DirectoryView
            string fileName = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["FullName"].Value.ToString();
            Methods myActions = new Methods();
            // fileName = fileName;
            string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", fileName);
            if (categoryState == "Expanded") {
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", fileName);
                RefreshDataGrid();
                this.Cursor = Cursors.Default;
                return;
            }
            if (categoryState == "Collapsed") {
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Expanded", fileName);
                RefreshDataGrid();
                this.Cursor = Cursors.Default;
                return;
            }
            try {
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName); // GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                _dir.Activate(this._CurrentFileViewBindingSource[myIndex] as FileView);
                SetTitle(_dir.FileView);



                if (myFileView.IsDirectory) {
                    RefreshDataGrid();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message + " - Line 559 in ExplorerView");
            }
        }

        private int GetIndexForCurrentFileViewBindingSourceForFullName(string fileName) {
            int findIndex = -1;
            for (int i = 0; i < this._CurrentFileViewBindingSource.Count; i++) {
                FileView item = (FileView)this._CurrentFileViewBindingSource[i];
                if (item.FullName == fileName) {
                    findIndex = i;
                    break;
                }
            }
            return findIndex;
        }

        private void thumbnailsMenuItem_Click(object sender, EventArgs e) {
            if (DoActionRequired(sender)) {
                MessageBox.Show("Thumbnails View");
            }
        }

        private void tilesMenuItem_Click(object sender, EventArgs e) {
            if (DoActionRequired(sender)) {
                MessageBox.Show("Tiles View");
            }
        }

        private void iconsMenuItem_Click(object sender, EventArgs e) {
            if (DoActionRequired(sender)) {
                MessageBox.Show("Icons View");
            }
        }

        private void listMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            if (DoActionRequired(sender)) {
                _CurrentSplitContainer.Panel2Collapsed = true;
                myActions.SetValueByKey("DetailsMenuItemChecked", "False");
            }
        }

        private void detailsMenuItem_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            Methods myActions = new Methods();
            if (DoActionRequired(sender)) {
                _CurrentSplitContainer.Panel2Collapsed = false;
                myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                //tries to start the process 
                try {
                    if (!File.Exists(@"C:\Program Files\Windows NT\Accessories\wordpad.exe")) {
                        myActions.MessageBoxShow(" File not found: " + @"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                    } else {
                        ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                        psi.WindowStyle = ProcessWindowStyle.Minimized;
                        _proc = Process.Start(psi);
                    }
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                //disables button and textbox
                //txtProcess.Enabled = false;
                //btnStart.Enabled = false;

                //host the started process in the panel 
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                // Remove border and whatnot
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }
            this.Cursor = Cursors.Default;
        }

        void Renderer_RenderToolStripBorder(object sender, ToolStripRenderEventArgs e) {
            e.Graphics.DrawLine(SystemPens.ButtonShadow, 0, 1, toolBar.Width, 1);
            e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 2, toolBar.Width, 2);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }


        private void upSplitButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;
            _dir.Up();
            SetTitle(_dir.FileView);
            RefreshDataGrid();
            Cursor.Current = Cursors.Default;
        }

        private void backSplitButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;
            _dir.Up();
            SetTitle(_dir.FileView);
            RefreshDataGrid();
            Cursor.Current = Cursors.Default;
        }
        #endregion

        private void btnTraceOn_Click(object sender, EventArgs e) {

        }
        private void ev_Process_File(string myFile) {
            try {
                Process.Start(myFile);
            } catch (Exception ex) {
                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
        }

        private void ev_Build_File(string myFile) {
            Methods myActions = new Methods();
            try {
                string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
                myActions.RunSync(strApplicationPath + "BuildProjects.bat", myFile);
            } catch (Exception ex) {
                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
        }

        private void ev_Delete_File(string myFile) {
            try {
                File.Delete(myFile);
            } catch (Exception ex) {
                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
        }

        private void ev_Delete_Directory(string myFile) {
            try {
                Directory.Delete(myFile, true);
            } catch (Exception ex) {
                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
        }





        private void btnVisualStudio_Click(object sender, EventArgs e) {

        }



        private void projectToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New Project";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New Project Name";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewProjectDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string basePathForNewProject = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewProject = myFileView.FullName;
                    basePathName = myFileView.Name;
                }
            }
            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewProject) + "\\" + basePathName;
            string myNewProjectName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewProjectDir = Path.Combine(basePathForNewProject, myNewProjectName);
            string scriptPathNewProject = myActions.ConvertFullFileNameToPublicPath(strNewProjectDir) + "\\-" + myNewProjectName;
            // myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", scriptPathNewProject);
            if (Directory.Exists(strNewProjectDir)) {
                myActions.MessageBoxShow(strNewProjectDir + "already exists");
                goto ReDisplayNewProjectDialog;
            }
            try {
                // create the directories
                Directory.CreateDirectory(strNewProjectDir);
                string strNewProjectDirNewProjectDir = Path.Combine(strNewProjectDir, myNewProjectName);
                Directory.CreateDirectory(strNewProjectDirNewProjectDir);
                string binDir = Path.Combine(strNewProjectDirNewProjectDir, "bin");
                Directory.CreateDirectory(binDir);
                string strDebug = Path.Combine(binDir, "Debug");
                Directory.CreateDirectory(strDebug);
                string strImages = Path.Combine(strNewProjectDirNewProjectDir, "Images");
                Directory.CreateDirectory(strImages);
                string strObj = Path.Combine(strNewProjectDirNewProjectDir, "obj");
                Directory.CreateDirectory(strObj);
                string strX86 = Path.Combine(strObj, "x86");
                Directory.CreateDirectory(strX86);
                string strObjDebug = Path.Combine(strX86, "Debug");
                Directory.CreateDirectory(strObjDebug);
                string strObjDebugTempPE = Path.Combine(strObjDebug, "TempPE");
                Directory.CreateDirectory(strObjDebugTempPE);
                string strProperties = Path.Combine(strNewProjectDirNewProjectDir, "Properties");
                Directory.CreateDirectory(strProperties);
                string strApplicationBinDebug = Application.StartupPath;
                string myNewProjectSourcePath = strApplicationBinDebug.Replace("bin\\Debug", "MyNewProject");
                if (myNewProjectSourcePath.EndsWith("MyNewProject") == false &&
                    myNewProjectSourcePath.EndsWith("MyNewProject\\") == false) {
                    myNewProjectSourcePath = Path.Combine(myNewProjectSourcePath, "MyNewProject");
                }
                string strLine = "";

                string[] myLines0 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "MyNewProject.sln"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, myNewProjectName + ".sln"))) {
                    foreach (var item in myLines0) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }
                myNewProjectSourcePath = Path.Combine(myNewProjectSourcePath, "MyNewProject");
                strNewProjectDir = Path.Combine(strNewProjectDir, myNewProjectName);
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\112_UpArrowShort_Green.ico"), Path.Combine(strNewProjectDir, "Images\\112_UpArrowShort_Green.ico"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgPatch16.PNG"), Path.Combine(strNewProjectDir, "Images\\imgPatch16.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgPatch17.PNG"), Path.Combine(strNewProjectDir, "Images\\imgPatch17.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgPatch2015_08.PNG"), Path.Combine(strNewProjectDir, "Images\\imgPatch2015_08.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgPatch2015_08_Home.PNG"), Path.Combine(strNewProjectDir, "Images\\imgPatch2015_08_Home.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgSVNUpdate.PNG"), Path.Combine(strNewProjectDir, "Images\\imgSVNUpdate.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgSVNUpdate_Home.PNG"), Path.Combine(strNewProjectDir, "Images\\imgSVNUpdate_Home.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgUpdateLogOK.PNG"), Path.Combine(strNewProjectDir, "Images\\imgUpdateLogOK.PNG"));
                File.Copy(Path.Combine(myNewProjectSourcePath, "Images\\imgUpdateLogOK_Home.PNG"), Path.Combine(strNewProjectDir, "Images\\imgUpdateLogOK_Home.PNG"));


                string[] myLines = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "Properties\\AssemblyInfo.cs"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "Properties\\AssemblyInfo.cs"))) {
                    foreach (var item in myLines) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }


                string[] myLines1 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "Properties\\Resources.Designer.cs"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "Properties\\Resources.Designer.cs"))) {
                    foreach (var item in myLines1) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }
                File.Copy(Path.Combine(myNewProjectSourcePath, "Properties\\Resources.resx"), Path.Combine(strNewProjectDir, "Properties\\Resources.resx"));

                string[] myLines2 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "Properties\\Settings.Designer.cs"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "Properties\\Settings.Designer.cs"))) {
                    foreach (var item in myLines2) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }
                File.Copy(Path.Combine(myNewProjectSourcePath, "Properties\\Settings.settings"), Path.Combine(strNewProjectDir, "Properties\\Settings.settings"));

                string[] myLines3 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "App.xaml"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "App.xaml"))) {
                    foreach (var item in myLines3) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }

                string[] myLines4 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "App.xaml.cs"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "App.xaml.cs"))) {
                    foreach (var item in myLines4) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }

                File.Copy(Path.Combine(myNewProjectSourcePath, "ClassDiagram1.cd"), Path.Combine(strNewProjectDir, "ClassDiagram1.cd"));

                string[] myLines5 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "MainWindow.xaml"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "MainWindow.xaml"))) {
                    foreach (var item in myLines5) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }

                string[] myLines6 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "MainWindow.xaml.cs"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, "MainWindow.xaml.cs"))) {
                    foreach (var item in myLines6) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }

                string[] myLines7 = File.ReadAllLines(Path.Combine(myNewProjectSourcePath, "MyNewProject.csproj"));
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(strNewProjectDir, myNewProjectName + ".csproj"))) {
                    foreach (var item in myLines7) {
                        strLine = item.Replace("MyNewProject", myNewProjectName);
                        sw.WriteLine(strLine);
                    }
                }


            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            RefreshDataGrid();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.

                    ev_Delete_Directory(myFileView.FullName.ToString());
                    string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";

                    if (Directory.Exists(settingsDirectory)) {
                        Directory.Delete(settingsDirectory, true);
                    }

                } else {
                    ev_Delete_File(myFileView.FullName.ToString());
                }

            }
            RefreshDataGrid();
        }

        private void AddGlobalHotKeys() {
            Methods myActions = new Methods();
            _myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            ArrayList newArrayList = new ArrayList();
            string strHotKey = "";
            HotKeyRecord myHotKeyRecord = new HotKeyRecord();
            bool boolHotKeysGood = true;
            foreach (var item in _myArrayList) {
                string[] myScriptInfoFields = item.ToString().Split('^');
                string scriptName = myScriptInfoFields[0];

                strHotKey = myScriptInfoFields[1];
                if (strHotKey != "") {

                    string strHotKeyExecutable = myScriptInfoFields[5];
                    myHotKeyRecord = new HotKeyRecord();
                    myHotKeyRecord.HotKeys = strHotKey.Split('+');
                    myHotKeyRecord.Executable = strHotKeyExecutable;
                    myHotKeyRecord.ExecuteContent = "";
                    boolHotKeysGood = true;
                    foreach (string myHotKey in myHotKeyRecord.HotKeys) {
                        if (dictVirtualKeyCodes.ContainsKey(myHotKey)) {
                            MessageBox.Show("Invalid hotkey: " + myHotKey + " on script: " + scriptName);
                            boolHotKeysGood = false;
                        }
                    }
                    if (boolHotKeysGood) {
                        listHotKeyRecords.Add(myHotKeyRecord);
                    }
                }


            }
            myHotKeyRecord = new HotKeyRecord();
            strHotKey = "Ctrl+Alt+N";
            myHotKeyRecord.HotKeys = strHotKey.Split('+');
            myHotKeyRecord.Executable = @"OpenLineInNotepad";
            myHotKeyRecord.ExecuteContent = null;
            myHotKeyRecord.ScriptID = 0;
            boolHotKeysGood = true;
            foreach (string myHotKey in myHotKeyRecord.HotKeys) {
                if (dictVirtualKeyCodes.ContainsKey(myHotKey)) {
                    MessageBox.Show("Invalid hotkey: " + myHotKey);
                    boolHotKeysGood = false;
                }
            }
            if (boolHotKeysGood) {
                listHotKeyRecords.Add(myHotKeyRecord);
            }

            myHotKeyRecord = new HotKeyRecord();
            strHotKey = "Ctrl+Alt+E";
            myHotKeyRecord.HotKeys = strHotKey.Split('+');
            myHotKeyRecord.Executable = @"OpenLineInIAExplorer";
            myHotKeyRecord.ExecuteContent = null;
            myHotKeyRecord.ScriptID = 0;
            boolHotKeysGood = true;
            foreach (string myHotKey in myHotKeyRecord.HotKeys) {
                if (dictVirtualKeyCodes.ContainsKey(myHotKey)) {
                    MessageBox.Show("Invalid hotkey: " + myHotKey);
                    boolHotKeysGood = false;
                }
            }
            if (boolHotKeysGood) {
                listHotKeyRecords.Add(myHotKeyRecord);
            }
            dictVirtualKeyCodes.Add("Ctrl", VirtualKeyCode.CONTROL);
            dictVirtualKeyCodes.Add("Alt", VirtualKeyCode.MENU);
            dictVirtualKeyCodes.Add("Shift", VirtualKeyCode.SHIFT);
            dictVirtualKeyCodes.Add("Space", VirtualKeyCode.SPACE);
            dictVirtualKeyCodes.Add("Up", VirtualKeyCode.UP);
            dictVirtualKeyCodes.Add("Down", VirtualKeyCode.DOWN);
            dictVirtualKeyCodes.Add("Left", VirtualKeyCode.LEFT);
            dictVirtualKeyCodes.Add("Right", VirtualKeyCode.RIGHT);
            dictVirtualKeyCodes.Add("A", VirtualKeyCode.VK_A);
            dictVirtualKeyCodes.Add("B", VirtualKeyCode.VK_B);
            dictVirtualKeyCodes.Add("C", VirtualKeyCode.VK_C);
            dictVirtualKeyCodes.Add("D", VirtualKeyCode.VK_D);
            dictVirtualKeyCodes.Add("E", VirtualKeyCode.VK_E);
            dictVirtualKeyCodes.Add("F", VirtualKeyCode.VK_F);
            dictVirtualKeyCodes.Add("G", VirtualKeyCode.VK_G);
            dictVirtualKeyCodes.Add("H", VirtualKeyCode.VK_H);
            dictVirtualKeyCodes.Add("I", VirtualKeyCode.VK_I);
            dictVirtualKeyCodes.Add("J", VirtualKeyCode.VK_J);
            dictVirtualKeyCodes.Add("K", VirtualKeyCode.VK_K);
            dictVirtualKeyCodes.Add("L", VirtualKeyCode.VK_L);
            dictVirtualKeyCodes.Add("M", VirtualKeyCode.VK_M);
            dictVirtualKeyCodes.Add("N", VirtualKeyCode.VK_N);
            dictVirtualKeyCodes.Add("O", VirtualKeyCode.VK_O);
            dictVirtualKeyCodes.Add("P", VirtualKeyCode.VK_P);
            dictVirtualKeyCodes.Add("Q", VirtualKeyCode.VK_Q);
            dictVirtualKeyCodes.Add("R", VirtualKeyCode.VK_R);
            dictVirtualKeyCodes.Add("S", VirtualKeyCode.VK_S);
            dictVirtualKeyCodes.Add("T", VirtualKeyCode.VK_T);
            dictVirtualKeyCodes.Add("U", VirtualKeyCode.VK_U);
            dictVirtualKeyCodes.Add("V", VirtualKeyCode.VK_V);
            dictVirtualKeyCodes.Add("W", VirtualKeyCode.VK_W);
            dictVirtualKeyCodes.Add("X", VirtualKeyCode.VK_X);
            dictVirtualKeyCodes.Add("Y", VirtualKeyCode.VK_Y);
            dictVirtualKeyCodes.Add("Z", VirtualKeyCode.VK_Z);
            // Create a timer and set a two millisecond interval.
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Interval = 2;

            // Alternate method: create a Timer with an interval argument to the constructor. 
            //aTimer = new System.Timers.Timer(2000); 

            // Create a timer with a two millisecond interval.
            aTimer = new System.Timers.Timer(2);

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }
        private static bool GetKeyboardState(byte[] keyStates) {
            if (keyStates == null)
                throw new ArgumentNullException("keyState");
            if (keyStates.Length != 256)
                throw new ArgumentException("The buffer must be 256 bytes long.", "keyState");
            return NativeGetKeyboardState(keyStates);
        }

        private static byte[] GetKeyboardState() {
            byte[] keyStates = new byte[256];
            if (!GetKeyboardState(keyStates))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return keyStates;
        }

        private static bool AnyKeyPressed() {
            byte[] keyState = GetKeyboardState();
            // skip the mouse buttons
            return keyState.Skip(8).Any(state => (state & 0x80) != 0);
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e) {
            InputSimulator myInputSimulator = new InputSimulator();

            if (myInputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.ESCAPE)) {
                _CurrentDataGridView.ClearSelection();
                _selectedRow = 0;
                Methods myActions = new Methods();
                myActions.SetValueByKey("InitialDirectory" + _selectedTabIndex.ToString() + "SelectedRow", "0");
                TryToCloseAllOpenFilesInTab();
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }

            if (AnyKeyPressed()) {
                _Panel2KeyPress = true;
            }

            if (myInputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.CONTROL) || myInputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.MENU)) {
                foreach (HotKeyRecord myHotKeyRecord in listHotKeyRecords) {
                    bool boolAllHotKeysPressed = true;
                    foreach (string myHotKey in myHotKeyRecord.HotKeys) {
                        VirtualKeyCode myVirtualKeyCode;
                        dictVirtualKeyCodes.TryGetValue(myHotKey, out myVirtualKeyCode);
                        if (!myInputSimulator.InputDeviceState.IsKeyDown(myVirtualKeyCode)) {
                            boolAllHotKeysPressed = false;
                        }
                    }


                    if (boolAllHotKeysPressed && boolStopEvent == false) {
                        boolStopEvent = true;
                        //TODO: increment number times executed
                        if (myHotKeyRecord.Executable == "OpenLineInNotepad") {
                            OpenLineInNotepad();
                            break;
                        }

                        if (myHotKeyRecord.Executable == "OpenLineInIAExplorer") {
                            OpenLineInIAExplorer();
                            break;
                        }


                        //if (myHotKeyRecord.Executable.Contains("OptionSetupIA") || myHotKeyRecord.Executable.Contains("OpenNotepadLineInVS")) {
                        //    Methods myActions = new Methods();
                        //    myActions.RunProcessAsAdmin(myHotKeyRecord.Executable, myHotKeyRecord.ExecuteContent ?? "");
                        //    break;
                        //}

                        RunWaitTillStart(myHotKeyRecord.Executable, myHotKeyRecord.ExecuteContent ?? "");
                    }
                }
            }
        }
        private void OpenLineInNotepad() {
            Methods myActions = new Methods();
            myActions.TypeText("{RIGHT}", 500);
            myActions.TypeText("{HOME}", 500);
            myActions.TypeText("+({END})", 500);
            myActions.TypeText("^(c)", 500);
            myActions.Sleep(500);
            string strCurrentLine = "";
            RunAsSTAThread(
            () => {
                strCurrentLine = myActions.PutClipboardInEntity();
            });
            List<string> myBeginDelim = new List<string>();
            List<string> myEndDelim = new List<string>();
            myBeginDelim.Add("\"");
            myEndDelim.Add("\"");
            FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

            string myQuote = "\"";
            delimParms.lines[0] = strCurrentLine;


            myActions.FindDelimitedText(delimParms);
            int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
            if (intLastSlash < 1) {
                myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                return;
            }
            string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
            string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
            string strFullFileName = delimParms.strDelimitedTextFound;
            myBeginDelim.Clear();
            myEndDelim.Clear();
            myBeginDelim.Add("(");
            myEndDelim.Add(",");
            delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
            delimParms.lines[0] = strCurrentLine;
            myActions.FindDelimitedText(delimParms);
            string strLineNumber = delimParms.strDelimitedTextFound;
            string strExecutable = "";
            if (strFullFileName.EndsWith(".doc") || strFullFileName.EndsWith(".docx")) {
                strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
            } else {
                if (strFullFileName.EndsWith(".odt")) {
                    strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
                } else {
                    myActions.KillAllProcessesByProcessName("notepad++");
                    strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                }
            }
            string strContent = strFullFileName;
            //Close the running process
            //if (_appHandle != IntPtr.Zero) {
            //  PostMessage(_appHandle, WM_CLOSE, 0, 0);
            //  System.Threading.Thread.Sleep(1000);
            //  _appHandle = IntPtr.Zero;
            //}
            //tries to start the process 
            //try {
            //  _proc = Process.Start(strExecutable, "\"" + strContent + "\"");
            //} catch (Exception) {
            //  MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //  return;
            //}


            //System.Threading.Thread.Sleep(500);
            //while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
            //  System.Threading.Thread.Sleep(10);
            //  _proc.Refresh();
            //}

            //_proc.WaitForInputIdle();
            //_appHandle = _proc.MainWindowHandle;

            //SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
            //SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            _NotepadppLoaded = true;
            myActions.Run(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
            if (strFullFileName.EndsWith(".doc") || strFullFileName.EndsWith(".docx") || strFullFileName.EndsWith(".odt")) {
            } else {
                myActions.TypeText("^(g)", 2000);
                myActions.TypeText(strLineNumber, 500);
                myActions.TypeText("{ENTER}", 500);
            }
            myActions.TypeText("^(f)", 500);

            string strFindWhatToUse = strFindWhat;
            string blockText = strFindWhatToUse;
            strFindWhatToUse = "";
            char[] specialChars = { '{', '}', '(', ')', '+', '^' };

            foreach (char letter in blockText) {
                bool _specialCharFound = false;

                for (int i = 0; i < specialChars.Length; i++) {
                    if (letter == specialChars[i]) {
                        _specialCharFound = true;
                        break;
                    }
                }

                if (_specialCharFound)
                    strFindWhatToUse += "{" + letter.ToString() + "}";
                else
                    strFindWhatToUse += letter.ToString();
            }
            myActions.TypeText(strFindWhatToUse, 500);
            myActions.TypeText("{ENTER}", 500);
            myActions.TypeText("{ESC}", 500);
            boolStopEvent = false;
        }
        static void RunAsSTAThread(Action goForIt) {
            AutoResetEvent @event = new AutoResetEvent(false);
            Thread thread = new Thread(
                () => {
                    goForIt();
                    @event.Set();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            @event.WaitOne();
        }
        private void OpenLineInIAExplorer() {

            Methods myActions = new Methods();
            myActions.TypeText("{RIGHT}", 500);
            myActions.TypeText("{HOME}", 500);
            myActions.TypeText("+({END})", 500);
            myActions.TypeText("^(c)", 500);
            myActions.Sleep(500);
            string strCurrentLine = "";
            RunAsSTAThread(
            () => {
                strCurrentLine = myActions.PutClipboardInEntity();
            });
            List<string> myBeginDelim = new List<string>();
            List<string> myEndDelim = new List<string>();
            myBeginDelim.Add("\"");
            myEndDelim.Add("\"");
            FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

            string myQuote = "\"";
            delimParms.lines[0] = strCurrentLine;


            myActions.FindDelimitedText(delimParms);
            int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
            if (intLastSlash < 1) {
                myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                return;
            }
            string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
            string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
            string strFullFileName = delimParms.strDelimitedTextFound;



            myBeginDelim.Clear();
            myEndDelim.Clear();
            myBeginDelim.Add("(");
            myEndDelim.Add(",");
            delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
            delimParms.lines[0] = strCurrentLine;
            myActions.FindDelimitedText(delimParms);
            string strLineNumber = delimParms.strDelimitedTextFound;
            string strExecutable = "";

            if (strFullFileName.EndsWith(".doc") || strFullFileName.EndsWith(".docx")) {
                strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
            } else {
                if (strFullFileName.EndsWith(".odt")) {
                    strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
                } else {
                    myActions.KillAllProcessesByProcessName("notepad++");
                    strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                }
            }

            string strContent = strFullFileName;
            //Close the running process
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            //tries to start the process 
            TryToCloseAllOpenFilesInTab();
            try {
                if (!File.Exists(strExecutable)) {
                    myActions.MessageBoxShow(" File not found: " + strExecutable);
                } else {
                    ProcessStartInfo psi = new ProcessStartInfo(strExecutable, "\"" + strContent + "\"");
                    psi.WindowStyle = ProcessWindowStyle.Minimized;
                    _proc = Process.Start(psi);
                }

            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }


            System.Threading.Thread.Sleep(500);
            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                System.Threading.Thread.Sleep(10);
                _proc.Refresh();
            }

            _proc.WaitForInputIdle();
            _appHandle = _proc.MainWindowHandle;

            try {
                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
            } catch (Exception) {


            }
            SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            if (strFullFileName.EndsWith(".doc") || strFullFileName.EndsWith(".docx") || strFullFileName.EndsWith(".odt")) {
            } else {
                myActions.TypeText("^(g)", 2000);
                myActions.TypeText(strLineNumber, 500);
                myActions.TypeText("{ENTER}", 500);
            }
            myActions.TypeText("^(f)", 500);

            string strFindWhatToUse = strFindWhat;
            string blockText = strFindWhatToUse;
            strFindWhatToUse = "";
            char[] specialChars = { '{', '}', '(', ')', '+', '^' };

            foreach (char letter in blockText) {
                bool _specialCharFound = false;

                for (int i = 0; i < specialChars.Length; i++) {
                    if (letter == specialChars[i]) {
                        _specialCharFound = true;
                        break;
                    }
                }

                if (_specialCharFound)
                    strFindWhatToUse += "{" + letter.ToString() + "}";
                else
                    strFindWhatToUse += letter.ToString();
            }
            myActions.TypeText(strFindWhatToUse, 500);
            myActions.TypeText("{ENTER}", 500);
            myActions.TypeText("{ESC}", 500);
            boolStopEvent = false;
            ////Close the running process
            //if (_appHandle != IntPtr.Zero) {
            //    PostMessage(_appHandle, WM_CLOSE, 0, 0);
            //    System.Threading.Thread.Sleep(1000);
            //    _appHandle = IntPtr.Zero;
            //}
            //// Open an existing file, or create a new one.

            _strFullFileName = strFullFileName;

            // Determine the full path of the file just created.
            if (InvokeRequired) {
                Invoke(new MethodInvoker(updateGUI));
            } else {
                // Do Something
                updateGUI();
            }



        }

        private void updateGUI() {
            Methods myActions = new Methods();
            string strFullFileName = _strFullFileName;
            FileInfo fi = new FileInfo(strFullFileName);
            DirectoryInfo di = fi.Directory;
            _CurrentSplitContainer.Panel1.Focus();


            _ignoreSelectedIndexChanged = true;
            tabControl1.SelectedIndex = tabControl1.TabCount - 1;
            myActions.TypeText("{TAB}", 1500);
            myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), di.FullName);

            _CurrentIndex = tabControl1.SelectedIndex;




            AddDataGridToTab(strInitialDirectory);

            myActions.SetValueByKey("NumOfTabs", (tabControl1.TabCount).ToString());
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory1 = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
            if (Directory.Exists(strSavedDirectory1)) {
                strInitialDirectory = strSavedDirectory1;
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;
            tabControl1.TabPages[tabControl1.SelectedIndex].Text = _dir.FileView.Name;
            tabControl1.TabPages[tabControl1.SelectedIndex].ToolTipText = _dir.FileView.FullName;


            _CurrentDataGridView = (DataGridViewExt)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0].Controls[0];
            _CurrentFileViewBindingSource = listBindingSource[tabControl1.SelectedIndex];
            _CurrentSplitContainer = listSplitContainer[tabControl1.SelectedIndex];
            RefreshDataGrid();
        }

        public void RunWaitTillStart(string myEntityForExecutable, string myEntityForContent) {


            if (myEntityForExecutable == null) {
                string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBox.Show(message);
                return;
            }
            string strExecutable = myEntityForExecutable;

            string strContent = "";
            if (myEntityForContent != null) {
                strContent = myEntityForContent;
            }
            var p = new Process();
            p.StartInfo.FileName = strExecutable;
            if (strContent != "") {
                p.StartInfo.Arguments = string.Concat("\"", strContent, "\"");
            }
            bool started = true;
            try {
                p.Start();
            } catch (Exception) {
                MessageBox.Show("Could not start process; Executable = " + strExecutable);
                boolStopEvent = false;

            }

            int procId = 0;
            try {
                procId = p.Id;
                Console.WriteLine("ID: " + procId);
            } catch (InvalidOperationException) {
                started = false;
                boolStopEvent = false;
            } catch (Exception ex) {
                started = false;
                boolStopEvent = false;
            }
            while (started == true && GetProcByID(procId) != null) {
                System.Threading.Thread.Sleep(1000);
                boolStopEvent = false;
                started = false;
            }

        }

        public void Run(string myEntityForExecutable, string myEntityForContent) {


            if (myEntityForExecutable == null) {
                string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBox.Show(message);
                return;
            }
            string strExecutable = myEntityForExecutable;

            string strContent = "";
            if (myEntityForContent != null) {
                strContent = myEntityForContent;
            }
            Methods myActions = new Methods();
            if (strContent == "") {
                if (!File.Exists(strExecutable)) {
                    myActions.MessageBoxShow(" File not found: " + strExecutable);
                } else {
                    Process.Start(strExecutable);
                }
            } else {
                try {
                    if (!File.Exists(strExecutable)) {
                        myActions.MessageBoxShow(" File not found: " + strExecutable);
                    } else {
                        Process.Start(strExecutable, string.Concat("\"", strContent, "\""));
                    }
                } catch (Exception ex) {

                    MessageBox.Show(ex.ToString() + " - Line 1446 in ExplorerView");
                }
            }

        }
        private Process GetProcByID(int id) {
            Process[] processlist = Process.GetProcesses();
            return processlist.FirstOrDefault(pr => pr.Id == id);
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New Category";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New Category Name";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewCategoryDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string myNewCategoryName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewCategoryDir = Path.Combine(_dir.FileView.FullName, myNewCategoryName);
            if (Directory.Exists(strNewCategoryDir)) {
                myActions.MessageBoxShow(strNewCategoryDir + "already exists");
                goto ReDisplayNewCategoryDialog;
            }
            try {
                // create the directories
                Directory.CreateDirectory(strNewCategoryDir);
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", myActions.ConvertFullFileNameToPublicPath(strNewCategoryDir) + "\\" + myNewCategoryName);



            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            RefreshDataGrid();
        }
        public DataTable ConvertToDataTable<T>(IList<T> data) {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data) {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        private void RefreshDataGrid() {


            // refresh datagridview
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            Methods myActions = new Methods();
            int mySplitterDistance = myActions.GetValueByKeyAsInt("_CurrentSplitContainerWidth" + _selectedTabIndex.ToString());
            if (mySplitterDistance > 0) {
                _CurrentSplitContainer.SplitterDistance = mySplitterDistance;
            }
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
                myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strSavedDirectory);
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            _CurrentDataGridView.DataSource = null;
            List<FileView> myListFileView = new List<FileView>();
            foreach (FileView item in _CurrentFileViewBindingSource.List) {
                myListFileView.Add(item);

            }
            _CurrentDataGridView.DataSource = ConvertToDataTable<FileView>(myListFileView);

            new DgvFilterManager(_CurrentDataGridView);
            int sortedColumn = myActions.GetValueByKeyAsInt("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            string myDirection = myActions.GetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            if (sortedColumn > -1 && _CurrentDataGridView.ColumnCount >= sortedColumn) {
                if (myDirection == "Ascending") {
                    _CurrentDataGridView.Sort(_CurrentDataGridView.Columns[sortedColumn], ListSortDirection.Ascending);
                } else {
                    _CurrentDataGridView.Sort(_CurrentDataGridView.Columns[sortedColumn], ListSortDirection.Descending);
                }
                myActions.SetValueByKey("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), "-1");
                myActions.SetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), ListSortDirection.Ascending.ToString());
            }
            //   this._CurrentDataGridView.Sort(_CurrentDataGridView.Columns[1], ListSortDirection.Ascending);
            // Use of the DataGridViewColumnSelector
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(_CurrentDataGridView);
            cs.MaxHeight = 100;
            cs.Width = 110;
            _selectedRow = myActions.GetValueByKeyAsInt("InitialDirectory" + tabControl1.SelectedIndex.ToString() + "SelectedRow");
            if (_selectedRow > 0) {
                if (_CurrentDataGridView.Rows.Count > _selectedRow && _CurrentDataGridView.Rows[_selectedRow].Cells.Count > 1) {
                    //_CurrentDataGridView.Rows[selectedRow].Cells[1].Selected = true;
                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "True") {

                        DataGridViewCell c = _CurrentDataGridView.Rows[_selectedRow].Cells[1];
                        if (!c.Selected) {
                            _CurrentDataGridView.ClearSelection();
                            c.Selected = true;
                            //    _CurrentDataGridView.FirstDisplayedScrollingRowIndex = _selectedRow;
                            //    _CurrentDataGridView.PerformLayout();
                            string fileName = _CurrentDataGridView.Rows[_selectedRow].Cells["FullName"].Value.ToString();
                            if (fileName.EndsWith(".url")

                             ) {
                                //Close the running process.
                                if (_appHandle != IntPtr.Zero) {
                                    if (_newTab == false) {
                                        PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                    }
                                    _newTab = false;
                                    System.Threading.Thread.Sleep(1000);
                                    _appHandle = IntPtr.Zero;
                                }
                                ////tries to start the process 
                                //try {                               
                                //    myActions.KillAllProcessesByProcessName("iexplore");
                                //    _proc = Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", GetInternetShortcut(fileName));
                                //} catch (Exception) {
                                //    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    return;
                                //}


                                //try {
                                //    System.Threading.Thread.Sleep(500);
                                //    while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                //        System.Threading.Thread.Sleep(10);
                                //        _proc.Refresh();
                                //    }

                                //    _proc.WaitForInputIdle();
                                //    _appHandle = _proc.MainWindowHandle;



                                //SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                                //SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                                //} catch (Exception ex) {

                                //    MessageBox.Show(ex.Message);
                                //}
                                //if (toolStripComboBox1.Text != "")
                                //    Url = toolStripComboBox1.Text;
                                _WordPadLoaded = false;
                                _NotepadppLoaded = false;
                                _WebBrowserLoaded = true;
                                Url = GetInternetShortcut(fileName);
                                InitializeComponentWebBrowser();
                                webBrowser1.ScriptErrorsSuppressed = true;
                                webBrowser1.Navigate(Url);

                                _CurrentSplitContainer.Panel2.Controls.Clear();
                                FlowLayoutPanel flp = new FlowLayoutPanel();
                                flp.Dock = DockStyle.Fill;

                                flp.Controls.Add(toolStrip1);
                                flp.Controls.Add(webBrowser1);
                                webBrowser1.Size = new System.Drawing.Size(_CurrentSplitContainer.Panel2.ClientSize.Width, _CurrentSplitContainer.Panel2.Height - 50);

                                flp.Controls.Add(statusStrip1);
                                _CurrentSplitContainer.Panel2.Controls.Add(flp);

                                webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(webpage_ProgressChanged);
                                webBrowser1.DocumentTitleChanged += new EventHandler(webpage_DocumentTitleChanged);
                                webBrowser1.StatusTextChanged += new EventHandler(webpage_StatusTextChanged);
                                webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webpage_Navigated);
                                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted);
                            }
                            if (fileName.EndsWith(".rtf")
                                || fileName.EndsWith(".odt")
                                || fileName.EndsWith(".doc")
                                || fileName.EndsWith(".docx")
                                ) {
                                _NotepadppLoaded = false;
                                _WebBrowserLoaded = false;
                                _WordPadLoaded = true;
                                //Close the running process
                                _CurrentSplitContainer.Panel2.Controls.Clear();
                                if (_appHandle != IntPtr.Zero) {
                                    TryToCloseAllOpenFilesInTab();
                                    _newTab = false;
                                    System.Threading.Thread.Sleep(1000);
                                    _appHandle = IntPtr.Zero;
                                }
                                //tries to start the process 
                                try {
                                    if (!File.Exists(@"C:\Program Files\Windows NT\Accessories\wordpad.exe")) {
                                        myActions.MessageBoxShow(" File not found: " + @"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                                    } else {
                                        ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Windows NT\Accessories\wordpad.exe", "\"" + fileName + "\"");
                                        psi.WindowStyle = ProcessWindowStyle.Minimized;
                                        _proc = Process.Start(psi);
                                    }
                                } catch (Exception) {
                                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }


                                System.Threading.Thread.Sleep(500);
                                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                    System.Threading.Thread.Sleep(10);
                                    _proc.Refresh();
                                }

                                _proc.WaitForInputIdle();
                                _appHandle = _proc.MainWindowHandle;

                                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                                AddAppHandleToOpenFiles(fileName, _appHandle);
                                // Remove border and whatnot
                                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                                MoveWindow(_appHandle, 0, 0, _CurrentSplitContainer.Panel2.Width - 5, _CurrentSplitContainer.Panel2.Height, true);
                                //  SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                                //  SetTitle(_dir.FileView);
                            } else {
                                if (fileName.EndsWith(".txt")
                                    || fileName.EndsWith(".bat")
                                    || fileName.EndsWith(".cs")
                                    || fileName.EndsWith(".xaml")
                                    || fileName.EndsWith(".sln")
                                    || fileName.EndsWith(".csproj")
                                    || fileName.EndsWith(".resx")
                                     || fileName.EndsWith(".js")
                                      || fileName.EndsWith(".css")
                                      || fileName.EndsWith(".html")
                                      || fileName.EndsWith(".htm")
                                      || fileName.EndsWith(".xml")
                                      || fileName.EndsWith(".sql")
                                      || fileName.EndsWith(".asp")
                                      || fileName.EndsWith(".inc")
                                      || fileName.EndsWith(".dinc")
                                      || fileName.EndsWith(".aspx")
                                      || fileName.EndsWith(".csv")) {
                                    _WordPadLoaded = false;
                                    _NotepadppLoaded = true;
                                    _WebBrowserLoaded = false;
                                    //Close the running process
                                    _CurrentSplitContainer.Panel2.Controls.Clear();
                                    if (_appHandle != IntPtr.Zero) {
                                        if (_newTab == false) {
                                            PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                        }
                                        _newTab = false;
                                        System.Threading.Thread.Sleep(1000);
                                        _appHandle = IntPtr.Zero;
                                    }
                                    //tries to start the process 
                                    try {
                                        myActions.KillAllProcessesByProcessName("notepad++");
                                        if (!File.Exists(@"C:\Program Files (x86)\Notepad++\notepad++.exe")) {
                                            myActions.MessageBoxShow(" You need to download notepad++ to use this feature.\n\r\n\rFile not found: " + @"C:\Program Files (x86)\Notepad++\notepad++.exe");
                                        } else {
                                            _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", fileName);
                                        }
                                    } catch (Exception) {
                                        MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }


                                    //    System.Threading.Thread.Sleep(500);
                                    while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                        System.Threading.Thread.Sleep(10);
                                        _proc.Refresh();
                                    }

                                    _proc.WaitForInputIdle();
                                    _appHandle = _proc.MainWindowHandle;

                                    SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                                    AddAppHandleToOpenFiles(fileName, _appHandle);
                                    SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                                    SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                                    SetTitle(_dir.FileView);
                                }
                            }

                        }

                    }
                }

            }
        }


        private void subCategoryToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New SubCategory";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New SubCategory Name";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewSubCategoryDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string myNewSubCategoryName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewSubCategoryDir = "";

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory) {
                    strNewSubCategoryDir = Path.Combine(myFileView.FullName, myNewSubCategoryName);
                    if (Directory.Exists(strNewSubCategoryDir)) {
                        myActions.MessageBoxShow(strNewSubCategoryDir + "already exists");
                        goto ReDisplayNewSubCategoryDialog;
                    }
                    try {
                        // create the directories
                        Directory.CreateDirectory(strNewSubCategoryDir);
                        myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", myActions.ConvertFullFileNameToPublicPath(strNewSubCategoryDir) + "\\" + myNewSubCategoryName);
                    } catch (Exception ex) {
                        MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
                    }
                }
            }

            RefreshDataGrid();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            Methods myActions = new Methods();
            //    DataGridViewCell myCell = _CurrentDataGridView.SelectedCells[0];
            myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString() + "SelectedRow", e.RowIndex.ToString());
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                if (e.RowIndex > -1) {
                    // Call Active on DirectoryView
                    string fileName = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["FullName"].Value.ToString();


                    string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", fileName);
                    if (categoryState == "Expanded" && e.ColumnIndex == 0) {
                        myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", fileName);
                        RefreshDataGrid();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    if (categoryState == "Collapsed" && e.ColumnIndex == 0) {
                        myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Expanded", fileName);
                        RefreshDataGrid();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    try {
                        //int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                        //FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                        //if (myFileView.IsDirectory && e.ColumnIndex < 2) {
                        //    _dir.Activate(this._CurrentFileViewBindingSource[myIndex] as FileView);
                        //    SetTitle(_dir.FileView);
                        //    RefreshDataGrid();
                        //}
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message + " - Line 1625 in ExplorerView");
                    }
                }
            }
            this.Cursor = Cursors.Default;

        }

        private void openStripMenuItem3_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
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



            DisplayWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }


            string strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;

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
                    string strApplicationBinDebug = Application.StartupPath;
                    string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

                    string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
                    goto DisplayWindowAgain;
                }
            }

            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {

                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }

                strFolderToUse = strFolder;
                myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strFolder);

                RefreshDataGrid();
                this.Cursor = Cursors.Default;
                return;
            }

            if (strButtonPressed == "btnOkay") {
                strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 300, 1200, 100, 100);
                goto LineAfterDisplayWindow;
            }
        }
        public string GetAppDirectoryForScript(string strScriptName) {
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }


        public static void Copy(string sourceDirectory, string targetDirectory) {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);

        }

        public static void CopyFile(string sourceFile, string targetDirectory) {
            FileInfo fi = new FileInfo(sourceFile);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);
            string newFile = Path.Combine(target.FullName, fi.Name);
            if (File.Exists(newFile)) {
                // TODO: need to check if newFile + "1" exists, etc
                fi.CopyTo(newFile + "1", true);
            } else {
                fi.CopyTo(newFile, true);
            }

        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target) {
            if (!Directory.Exists(target.FullName)) {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles()) {
                //  Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            Methods myActions = new Methods();
            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);


                string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
                string fromRoamingDirectory = Path.Combine(settingsDirectory, diSourceSubDir.FullName);


                string toRoamingDirectory = Path.Combine(settingsDirectory, target.FullName);
                if (Directory.Exists(fromRoamingDirectory)) {
                    DirectoryInfo fromRoamingDirectoryDI = new DirectoryInfo(fromRoamingDirectory);
                    DirectoryInfo toRoamingDirectoryDI = Directory.CreateDirectory(toRoamingDirectory);
                    // Copy each file into the new directory.
                    foreach (FileInfo fi in fromRoamingDirectoryDI.GetFiles()) {
                        //  Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                        fi.CopyTo(Path.Combine(toRoamingDirectoryDI.FullName, fi.Name), true);
                    }
                }
                CopyAll(diSourceSubDir, nextTargetSubDir);


            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New Folder";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New Folder Name";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewFolderDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewFolder = myFileView.FullName;
                    basePathName = myFileView.Name;
                }
            }
            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewFolder) + "\\" + basePathName;
            string myNewFolderName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewFolderDir = Path.Combine(basePathForNewFolder, myNewFolderName);
            if (Directory.Exists(strNewFolderDir)) {
                myActions.MessageBoxShow(strNewFolderDir + "already exists");
                goto ReDisplayNewFolderDialog;
            }
            try {
                // create the directories
                string newFolderScriptPath = basePathForNewFolder + "\\" + myNewFolderName;
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                Directory.CreateDirectory(strNewFolderDir);
            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            RefreshDataGrid();
        }

        private void notepadToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    _NotepadppLoaded = true;
                    string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You cannot open folders with Notepad");
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            Methods myActions = new Methods();
            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (detailsMenuItemChecked == "True") {
                if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Left) {
                    DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                    if (!c.Selected) {
                        _CurrentDataGridView.ClearSelection();
                        _selectedRow = 0;
                        // Methods myActions = new Methods();
                        myActions.SetValueByKey("InitialDirectory" + _selectedTabIndex.ToString() + "SelectedRow", "0");
                        c.Selected = true;
                        myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString() + "SelectedRow", e.RowIndex.ToString());
                        string fileName = ((DataGridViewExt)sender).Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                        if (fileName.EndsWith(".url")

                         ) {
                            //Close the running process.
                            if (_appHandle != IntPtr.Zero) {
                                if (_newTab == false) {
                                    PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                }
                                _newTab = false;
                                System.Threading.Thread.Sleep(1000);
                                _appHandle = IntPtr.Zero;
                            }
                            ////tries to start the process 
                            //try {                               
                            //    myActions.KillAllProcessesByProcessName("iexplore");
                            //    _proc = Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", GetInternetShortcut(fileName));
                            //} catch (Exception) {
                            //    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    return;
                            //}


                            //try {
                            //    System.Threading.Thread.Sleep(500);
                            //    while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            //        System.Threading.Thread.Sleep(10);
                            //        _proc.Refresh();
                            //    }

                            //    _proc.WaitForInputIdle();
                            //    _appHandle = _proc.MainWindowHandle;



                            //SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                            //SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                            //} catch (Exception ex) {

                            //    MessageBox.Show(ex.Message);
                            //}
                            //if (toolStripComboBox1.Text != "")
                            //    Url = toolStripComboBox1.Text;
                            _WordPadLoaded = false;
                            _NotepadppLoaded = false;
                            _WebBrowserLoaded = true;
                            Url = GetInternetShortcut(fileName);
                            InitializeComponentWebBrowser();
                            webBrowser1.ScriptErrorsSuppressed = true;
                            webBrowser1.Navigate(Url);

                            _CurrentSplitContainer.Panel2.Controls.Clear();
                            FlowLayoutPanel flp = new FlowLayoutPanel();
                            flp.Dock = DockStyle.Fill;

                            flp.Controls.Add(toolStrip1);
                            flp.Controls.Add(webBrowser1);
                            webBrowser1.Size = new System.Drawing.Size(_CurrentSplitContainer.Panel2.ClientSize.Width, _CurrentSplitContainer.Panel2.Height - 50);

                            flp.Controls.Add(statusStrip1);
                            _CurrentSplitContainer.Panel2.Controls.Add(flp);

                            webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(webpage_ProgressChanged);
                            webBrowser1.DocumentTitleChanged += new EventHandler(webpage_DocumentTitleChanged);
                            webBrowser1.StatusTextChanged += new EventHandler(webpage_StatusTextChanged);
                            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webpage_Navigated);
                            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted);
                        }
                        if (fileName.EndsWith(".rtf")
                            || fileName.EndsWith(".odt")
                            || fileName.EndsWith(".doc")
                            || fileName.EndsWith(".docx")
                            ) {
                            _NotepadppLoaded = false;
                            _WebBrowserLoaded = false;
                            _WordPadLoaded = true;
                            //Close the running process
                            _CurrentSplitContainer.Panel2.Controls.Clear();
                            if (_appHandle != IntPtr.Zero) {
                                 System.Threading.Thread.Sleep(1000);
                                _appHandle = IntPtr.Zero;
                            }
                            _newTab = false;
                            //tries to start the process 
                             TryToCloseAllOpenFilesInTab();
                            try {
                                if (!File.Exists(@"C:\Program Files\Windows NT\Accessories\wordpad.exe")) {
                                    myActions.MessageBoxShow(" File not found: " + @"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                                } else {


                                    ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Windows NT\Accessories\wordpad.exe", "\"" + fileName + "\"");
                                    psi.WindowStyle = ProcessWindowStyle.Minimized;
                                    _proc = Process.Start(psi);
                                }
                            } catch (Exception ex) {
                                MessageBox.Show("Something went wrong trying to start your process: " + ex.Message, "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Cursor = Cursors.Default;
                                return;
                            }


                            System.Threading.Thread.Sleep(500);
                            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                System.Threading.Thread.Sleep(10);
                                _proc.Refresh();
                            }

                            _proc.WaitForInputIdle();
                            _appHandle = _proc.MainWindowHandle;

                            SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                            AddAppHandleToOpenFiles(fileName, _appHandle);
                            // Remove border and whatnot                         

                            SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                            MoveWindow(_appHandle, 0, 0, _CurrentSplitContainer.Panel2.Width - 5, _CurrentSplitContainer.Panel2.Height, true);
                            //  SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                            //  SetTitle(_dir.FileView);
                        } else {
                            if (fileName.EndsWith(".txt")
                                || fileName.EndsWith(".bat")
                                || fileName.EndsWith(".cs")
                                || fileName.EndsWith(".xaml")
                                || fileName.EndsWith(".sln")
                                || fileName.EndsWith(".csproj")
                                || fileName.EndsWith(".resx")
                                 || fileName.EndsWith(".js")
                                  || fileName.EndsWith(".css")
                                  || fileName.EndsWith(".html")
                                  || fileName.EndsWith(".htm")
                                  || fileName.EndsWith(".xml")
                                  || fileName.EndsWith(".sql")
                                  || fileName.EndsWith(".asp")
                                  || fileName.EndsWith(".inc")
                                  || fileName.EndsWith(".dinc")
                                  || fileName.EndsWith(".aspx")
                                  || fileName.EndsWith(".csv")) {
                                _WordPadLoaded = false;
                                _NotepadppLoaded = true;
                                _WebBrowserLoaded = false;
                                //Close the running process
                                _CurrentSplitContainer.Panel2.Controls.Clear();
                                if (_appHandle != IntPtr.Zero) {
                                    if (_newTab == false) {
                                        PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                    }
                                    _newTab = false;
                                    System.Threading.Thread.Sleep(1000);
                                    _appHandle = IntPtr.Zero;
                                }
                                //tries to start the process 
                                try {
                                    myActions.KillAllProcessesByProcessName("notepad++");
                                    if (!File.Exists(@"C:\Program Files (x86)\Notepad++\notepad++.exe")) {
                                        myActions.MessageBoxShow(" You need to download notepad++ to use this feature.\n\r\n\rFile not found: " + @"C:\Program Files (x86)\Notepad++\notepad++.exe");
                                    } else {
                                        _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", fileName);
                                    }
                                } catch (Exception) {
                                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }


                                //    System.Threading.Thread.Sleep(500);
                                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                    System.Threading.Thread.Sleep(10);
                                    _proc.Refresh();
                                }

                                _proc.WaitForInputIdle();
                                _appHandle = _proc.MainWindowHandle;

                                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                                AddAppHandleToOpenFiles(fileName, _appHandle);
                                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                                SetTitle(_dir.FileView);
                            }
                        }

                    }
                }
            }
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right) {
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected) {
                    _CurrentDataGridView.ClearSelection();
                    c.DataGridView.CurrentCell = c;
                    c.Selected = true;
                    _selectedRow = e.RowIndex;
                    myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString() + "SelectedRow", e.RowIndex.ToString());
                }
            }

        }

        private void TryToCloseAllOpenFilesInTab() {
            string fileName1 = "OpenFiles.txt";
            string strApplicationBinDebug = Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

            string settingsDirectory =
Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName1);
            ArrayList openedFiles = new ArrayList();
            List<OpenedFile> openedFile = new List<OpenedFile>();
            openedFile.Clear();

            if (!File.Exists(settingsPath)) {
                using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                    objSWFile.Close();
                }
            }
            using (StreamReader objSRFile = File.OpenText(settingsPath)) {
                string strReadLine = "";
                while ((strReadLine = objSRFile.ReadLine()) != null) {
                    string[] openedFileFieldsArray = strReadLine.Split('^');                   
                    openedFile.Add(new OpenedFile(openedFileFieldsArray[0], openedFileFieldsArray[1], openedFileFieldsArray[2]));
                }
                objSRFile.Close();
            }         


            IntPtr _appHandleToClose = IntPtr.Zero;

            for (int i = openedFile.Count - 1; i > -1; i--) {
                if (openedFile[i]._Tab.Trim() == _selectedTabIndex.ToString()) {
                    _appHandleToClose = new IntPtr(Convert.ToInt32(openedFile[i]._Process.Trim()));
                    if (_appHandleToClose != IntPtr.Zero) {
                        PostMessage(_appHandleToClose, WM_CLOSE, 0, 0);
                        System.Threading.Thread.Sleep(1000);
                        _appHandleToClose = IntPtr.Zero;
                    }
                    openedFile.RemoveAt(i);
                    break;
                }
            }

            using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                foreach (OpenedFile item in openedFile) {                    
                        objSWFile.WriteLine(item._Tab + '^' + item._FileName + '^' + item._Process);
                }               
                objSWFile.Close();
            }
        }

        private void AddAppHandleToOpenFiles(string fileName, IntPtr appHandle) {
            string fileName1 = "OpenFiles.txt";
            string strApplicationBinDebug = Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

            string settingsDirectory =
Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName1);
            ArrayList openedFiles = new ArrayList();
            List<OpenedFile> openedFile = new List<OpenedFile>();
            openedFile.Clear();

            if (!File.Exists(settingsPath)) {
                using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                    objSWFile.Close();
                }
            }
            using (StreamReader objSRFile = File.OpenText(settingsPath)) {
                string strReadLine = "";
                while ((strReadLine = objSRFile.ReadLine()) != null) {
                    string[] openedFileFieldsArray = strReadLine.Split('^');                    
                    openedFile.Add(new OpenedFile(openedFileFieldsArray[0], openedFileFieldsArray[1], openedFileFieldsArray[2]));
                }
                objSRFile.Close();
            }     

            using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                foreach (OpenedFile item in openedFile) {
                    if (item._FileName != "") {
                        objSWFile.WriteLine(item._Tab + '^' + item._FileName + '^' + item._Process);
                    }
                }
                objSWFile.WriteLine(tabControl1.SelectedIndex.ToString() + '^' + fileName + '^' + _appHandle.ToString());
                objSWFile.Close();
            }
        }

        private void webpage_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            if (webBrowser1.CanGoBack) toolStripButton1.Enabled = true;
            else toolStripButton1.Enabled = false;

            if (webBrowser1.CanGoForward) toolStripButton2.Enabled = true;
            else toolStripButton2.Enabled = false;
            toolStripStatusLabel1.Text = "Done";
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            myBrowser();
        }

        private void myBrowser() {
            if (toolStripComboBox1.Text != "")
                Url = toolStripComboBox1.Text;
            webBrowser1.Navigate(Url);
            webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(webpage_ProgressChanged);
            webBrowser1.DocumentTitleChanged += new EventHandler(webpage_DocumentTitleChanged);
            webBrowser1.StatusTextChanged += new EventHandler(webpage_StatusTextChanged);
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webpage_Navigated);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted);
        }
        private void webpage_DocumentTitleChanged(object sender, EventArgs e) {
            this.Text = webBrowser1.DocumentTitle.ToString();
        }
        private void webpage_StatusTextChanged(object sender, EventArgs e) {
            toolStripStatusLabel1.Text = webBrowser1.StatusText;
        }

        private void webpage_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e) {
            toolStripProgressBar1.Maximum = (int)e.MaximumProgress;
            toolStripProgressBar1.Value = ((int)e.CurrentProgress < 0 || (int)e.MaximumProgress < (int)e.CurrentProgress) ? (int)e.MaximumProgress : (int)e.CurrentProgress;
        }

        private void webpage_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
            toolStripComboBox1.Text = webBrowser1.Url.ToString();
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            webBrowser1.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            webBrowser1.GoForward();
            this.Cursor = Cursors.Default;
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            webBrowser1.GoBack();
            this.Cursor = Cursors.Default;
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            webBrowser1.GoHome();
            this.Cursor = Cursors.Default;
        }

        private void toolStripButton6_Click(object sender, EventArgs e) {
            webBrowser1.ShowPrintPreviewDialog();
        }

        public static string GetInternetShortcut(string filePath) {
            string url = "";

            using (TextReader reader = new StreamReader(filePath)) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("URL=")) {
                        string[] splitLine = line.Split('=');
                        if (splitLine.Length > 0) {
                            url = splitLine[1];
                            break;
                        }
                    }
                }
            }

            return url;
        }

        private void notepadToolStripMenuItem1_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    myActions.KillAllProcessesByProcessName("notepad++");
                    _NotepadppLoaded = true;
                    string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You can not open folders with Notepad++");
                }
            }
        }

        private void textDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            FileView myFileView;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                }
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();
                    int intRowCtr = 0;

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Create New TextDocument";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Enter New TextDocument Name";
                    myControlEntity.RowNumber = 0;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "myTextBox";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = 0;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblCustom";
                    myControlEntity.Text = "Custom";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());



                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.ComboBox;
                    myControlEntity.SelectedValue = "";
                    myControlEntity.ID = "cbxCustom";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ToolTipx = "";
                    //foreach (var item in alcbxCustom) {
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
                    myControlEntity.ID = "lbldescription";
                    myControlEntity.Text = "Description";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());








                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.Text = "";
                    myControlEntity.ID = "txtDescription";
                    myControlEntity.Multiline = true;
                    myControlEntity.Height = 200;
                    myControlEntity.TextWrap = true;
                    myControlEntity.Width = 650;
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ToolTipx = "";
                    //foreach (var item in alcbxdescription) {
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
                    myControlEntity.ID = "lblStatus";
                    myControlEntity.Text = "Status";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());





                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.ComboBox;
                    myControlEntity.SelectedValue = "";
                    myControlEntity.ID = "cbxStatus";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ToolTipx = "";
                    //foreach (var item in alcbxStatus) {
                    //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
                    //}
                    //myControlEntity.ListOfKeyValuePairs = cbp;
                    myControlEntity.ComboBoxIsEditable = true;
                    myControlEntity.ColumnNumber = 1;







                    myControlEntity.ColumnSpan = 2;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    ReDisplayNewTextDocumentDialog:
                    string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);

                    if (strButtonPressed == "btnCancel") {
                        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                        return;
                    }
                    //string basePathForNewTextDocument = _dir.FileView.FullName;
                    //string basePathName = _dir.FileView.Name;

                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;

                    string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
                    string strCustom = myListControlEntity.Find(x => x.ID == "cbxCustom").SelectedValue;
                    string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;
                    string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;


                    if (!myNewTextDocumentName.Contains(".")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".txt";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
                    myActions.SetValueByPublicKeyInCurrentFolder("cbxCustomSelectedValue", strCustom, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("custom", strCustom, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("description", strdescription, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("status", strStatus, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("cbxStatusSelectedValue", strStatus, strNewTextDocumentDir);


                    if (!File.Exists(strNewTextDocumentDir)) {
                        string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".txt", "");
                        //  myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);

                        // File.Create(strNewTextDocumentDir);

                    }
                    _NotepadppLoaded = true;
                    string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";

                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "False") {
                        myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                        _CurrentSplitContainer.Panel2Collapsed = false;
                        this.detailsMenuItem.Checked = true;
                        this.listMenuItem.Checked = false;
                        //tries to start the process 
                        try {
                            if (!File.Exists(strExecutable)) {
                                myActions.MessageBoxShow(" File not found: " + strExecutable);
                            } else {
                                _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                            }
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        //disables button and textbox
                        //txtProcess.Enabled = false;
                        //btnStart.Enabled = false;

                        //host the started process in the panel 
                        System.Threading.Thread.Sleep(500);
                        while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                        AddAppHandleToOpenFiles(fileName, _appHandle);
                        SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                        //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    } else {
                        //Close the running process
                        if (_appHandle != IntPtr.Zero) {
                            PostMessage(_appHandle, WM_CLOSE, 0, 0);
                            System.Threading.Thread.Sleep(1000);
                            _appHandle = IntPtr.Zero;
                        }
                        //tries to start the process 
                        try {
                            if (!File.Exists(strExecutable)) {
                                myActions.MessageBoxShow(" File not found: " + strExecutable);
                            } else {
                                _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                            }
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        //disables button and textbox
                        //txtProcess.Enabled = false;
                        //btnStart.Enabled = false;

                        //host the started process in the panel 
                        System.Threading.Thread.Sleep(500);
                        while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                        AddAppHandleToOpenFiles(fileName, _appHandle);
                        SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    }
                    _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);

                } else {
                    myActions.MessageBoxShow("You can not create a text file inside a file; you need to select folder first");
                }
            }
        }

        private void folderToolStripMenuItem1_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New Folder";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New Folder Name";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewFolderDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewFolder = myFileView.FullName;
                    basePathName = myFileView.Name;
                }
            }
            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewFolder) + "\\" + basePathName;
            string myNewFolderName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewFolderDir = Path.Combine(basePathForNewFolder, myNewFolderName);
            if (Directory.Exists(strNewFolderDir)) {
                myActions.MessageBoxShow(strNewFolderDir + "already exists");
                goto ReDisplayNewFolderDialog;
            }
            try {
                // create the directories
                string newFolderScriptPath = basePathForNewFolder + "\\" + myNewFolderName;
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                Directory.CreateDirectory(strNewFolderDir);
            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            RefreshDataGrid();
        }

        private void wordPadToolStripMenuItem1_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    string strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
                    _WordPadLoaded = true;
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You can not create a wordpad file inside a file; first select folder and then right click");
                }
            }
        }

        private void wordPadToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                    fileFullName = myFileView.FullName;
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Create New WordPad Document";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    int intRowCtr = 0;


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Enter New WordPad Document Name";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "myTextBox";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblCustom";
                    myControlEntity.Text = "Custom";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());



                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.ComboBox;
                    myControlEntity.SelectedValue = "";
                    myControlEntity.ID = "cbxCustom";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ToolTipx = "";
                    //foreach (var item in alcbxCustom) {
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
                    myControlEntity.ID = "lbldescription";
                    myControlEntity.Text = "Description";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());



                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.Text = "";
                    myControlEntity.ID = "txtDescription";
                    myControlEntity.Multiline = true;
                    myControlEntity.Height = 200;
                    myControlEntity.TextWrap = true;
                    myControlEntity.Width = 650;
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ToolTipx = "";
                    //foreach (var item in alcbxdescription) {
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
                    myControlEntity.ID = "lblStatus";
                    myControlEntity.Text = "Status";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());



                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.ComboBox;
                    myControlEntity.SelectedValue = "";
                    myControlEntity.ID = "cbxStatus";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ToolTipx = "";
                    //foreach (var item in alcbxStatus) {
                    //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
                    //}
                    //myControlEntity.ListOfKeyValuePairs = cbp;
                    myControlEntity.ComboBoxIsEditable = true;
                    myControlEntity.ColumnNumber = 1;

                    myControlEntity.ColumnSpan = 2;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    ReDisplayNewTextDocumentDialog:
                    string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);

                    if (strButtonPressed == "btnCancel") {
                        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                        return;
                    }
                    string strCustom = myListControlEntity.Find(x => x.ID == "cbxCustom").SelectedValue;
                    string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;

                    string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;

                    basePathForNewTextDocument = _dir.FileView.FullName;
                    basePathName = _dir.FileView.Name;

                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;

                    string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
                    if (!myNewTextDocumentName.Contains(".")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".rtf";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
                    myActions.SetValueByPublicKeyInCurrentFolder("cbxCustomSelectedValue", strCustom, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("custom", strCustom, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("description", strdescription, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("status", strStatus, strNewTextDocumentDir);
                    myActions.SetValueByPublicKeyInCurrentFolder("cbxStatusSelectedValue", strStatus, strNewTextDocumentDir);

                    if (!File.Exists(strNewTextDocumentDir)) {
                        string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".rtf", "");
                        //   myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                        using (StreamWriter sw = new StreamWriter(strNewTextDocumentDir)) {



                        }
                        //   File.Create(strNewTextDocumentDir);

                    }
                    _WordPadLoaded = true;
                    string strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "False") {
                        myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                        _CurrentSplitContainer.Panel2Collapsed = false;
                        this.detailsMenuItem.Checked = true;
                        this.listMenuItem.Checked = false;
                        //tries to start the process 
                        TryToCloseAllOpenFilesInTab();
                        try {
                            if (!File.Exists(strExecutable)) {
                                myActions.MessageBoxShow(" File not found: " + strExecutable);
                            } else {
                                _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                            }
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        //disables button and textbox
                        //txtProcess.Enabled = false;
                        //btnStart.Enabled = false;

                        //host the started process in the panel 
                        System.Threading.Thread.Sleep(500);
                        while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                        AddAppHandleToOpenFiles(fileName, _appHandle);
                        SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                        //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    } else {
                        //Close the running process
                        if (_appHandle != IntPtr.Zero) {
                            PostMessage(_appHandle, WM_CLOSE, 0, 0);
                            System.Threading.Thread.Sleep(1000);
                            _appHandle = IntPtr.Zero;
                        }
                        //tries to start the process 
                        try {
                            if (!File.Exists(strExecutable)) {
                                myActions.MessageBoxShow(" File not found: " + strExecutable);
                            } else {
                                _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                            }
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        //disables button and textbox
                        //txtProcess.Enabled = false;
                        //btnStart.Enabled = false;

                        //host the started process in the panel 
                        System.Threading.Thread.Sleep(500);
                        while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                        AddAppHandleToOpenFiles(fileName, _appHandle);
                        SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    }
                    _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);

                } else {
                    myActions.MessageBoxShow("You can not create a text document inside a file; first select folder and right click");
                }

            }
        }

        private void urlShortcutToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                    fileFullName = myFileView.FullName;
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Create New Url Shortcut";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    int intRowCtr = 0;

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Enter New Url Shortcut Name";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtShortcutName";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblShortcutUrl";
                    myControlEntity.Text = "Shortcut Url";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());



                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtShortcutUrl";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    ReDisplayNewTextDocumentDialog:

                    string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                    if (strButtonPressed == "btnCancel") {
                        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                        return;
                    }

                    string strShortcutName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
                    string strShortcutUrl = myListControlEntity.Find(x => x.ID == "txtShortcutUrl").Text;


                    string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
                    if (!myNewTextDocumentName.EndsWith(".url")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".url";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);

                    myActions.SetValueByKeyForNonCurrentScript("shortcutName", strShortcutName, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
                    myActions.SetValueByKeyForNonCurrentScript("shortcutUrl", strShortcutUrl, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
                    if (!File.Exists(strNewTextDocumentDir)) {
                        string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".rtf", "");
                        //   myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                        using (StreamWriter writer = new StreamWriter(strNewTextDocumentDir)) {

                            writer.WriteLine("[InternetShortcut]");

                            writer.WriteLine("URL=" + strShortcutUrl);

                            writer.Flush();

                        }
                    }
                    _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);

                } else {
                    myActions.MessageBoxShow("You can not create a text document inside a file; first select folder and right click");
                }

            }
        }

        private void DeleteStripMenuItem4_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.

                    ev_Delete_Directory(myFileView.FullName.ToString());
                    string scriptPath = myActions.ConvertFullFileNameToPublicPath(myFileView.FullName) + "\\" + myFileView.Name;
                    string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
                    settingsDirectory = Path.Combine(settingsDirectory, scriptPath);
                    if (Directory.Exists(settingsDirectory)) {
                        Directory.Delete(settingsDirectory, true);
                    }

                } else {
                    ev_Delete_File(myFileView.FullName.ToString());
                }

            }
            RefreshDataGrid();
        }

        private void subCategoryStripMenuItem5_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New SubCategory";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New SubCategory Name";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewSubCategoryDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string myNewSubCategoryName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewSubCategoryDir = "";

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory) {
                    strNewSubCategoryDir = Path.Combine(myFileView.FullName, myNewSubCategoryName);
                    if (Directory.Exists(strNewSubCategoryDir)) {
                        myActions.MessageBoxShow(strNewSubCategoryDir + "already exists");
                        goto ReDisplayNewSubCategoryDialog;
                    }
                    try {
                        // create the directories
                        Directory.CreateDirectory(strNewSubCategoryDir);
                        myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", myActions.ConvertFullFileNameToPublicPath(strNewSubCategoryDir) + "\\" + myNewSubCategoryName);
                    } catch (Exception ex) {
                        MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
                    }
                }
            }

            RefreshDataGrid();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            _selectedTabIndex = tabControl1.SelectedIndex;
        }

        private void AddDataGridToTab(string pstrInitialDirectory) {
            tabControl1.TabPages.Insert(_CurrentIndex + 1, "    +");
            DataGridViewExt myDataGridView = new DataGridViewExt(pstrInitialDirectory);
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();

            DataGridViewImageColumn dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();

            DataGridViewTextBoxColumn NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buildStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hotKeyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metadataStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notepadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordPadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowsExplorerStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.textDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.urlShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordPadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DataGridViewTextBoxColumn HotKeyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn DescriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TotalExecutionsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn SuccessfulExecutionsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn PercentCorrectCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn LastExecutedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn SizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn AvgExecutionTimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ManualExecutionTimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn CustomCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn StatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TotalSavingsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn DateModifiedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn NestingLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.backSplitButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.forwardSplitButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.upSplitButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            //this.viewSplitButton = new System.Windows.Forms.ToolStripSplitButton();

            this.listMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.categoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();

            this.copyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addHotKeyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHotKeyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectActualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectIdealToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();



            ((System.ComponentModel.ISupportInitialize)(myDataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();

            this.toolBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            //this.tabPage1.SuspendLayout();
            //this.tabPage2.SuspendLayout();

            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            myDataGridView.AllowUserToAddRows = false;
            myDataGridView.AllowUserToDeleteRows = false;
            myDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            myDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            myDataGridView.AutoGenerateColumns = false;
            myDataGridView.BackgroundColor = System.Drawing.Color.White;
            myDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            myDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            myDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            myDataGridView.ColumnHeadersHeight = 22;
            // #IMPORTANT: put new columns at end because
            // columns are referred to by index in code and
            // adding new columns in the middle will break code
            // Index  Column                      DataPropertyName
            // 0       dataGridViewImageColumn1,  Icon
            // 1       NameCol,                   Name
            // 2       HotKeyCol,                 HotKey
            // 3       TotalExecutionsCol,        TotalExecutions
            // 4       SuccessfulExecutionsCol,   SuccessfulExecutions
            // 5       PercentCorrectCol,         PercentCorrect
            // 6       LastExecutedCol,           LastExecuted
            // 7       SizeCol,                   Size 
            // 8       AvgExecutionTimeCol,       AvgExecutionTime
            // 9       ManualExecutionTimeCol,    ManualExecutionTime
            // 10      TotalSavingsCol,           TotalSavings
            // 11      Type,                      Type
            // 12      DateModifiedCol,           DateModified
            // 13      FullName,                  FullName             
            // 14      CustomCol,                 Custom
            // 15      StatusCol,                 Status
            // 16      DescriptionCol,            Description
            // 17      NestingLevel               NestingLevel
            myDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            dataGridViewImageColumn1,
            NameCol,
             HotKeyCol,
             TotalExecutionsCol,
             SuccessfulExecutionsCol,
             PercentCorrectCol,
             LastExecutedCol,
             SizeCol,
             AvgExecutionTimeCol,
             ManualExecutionTimeCol,
             TotalSavingsCol,
             Type,
             DateModifiedCol,


             FullName,
             CustomCol,
             StatusCol,
             DescriptionCol,
             NestingLevel
            });
            //  myDataGridView.DataSource = this.FileViewBindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            myDataGridView.DefaultCellStyle = dataGridViewCellStyle11;
            myDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            myDataGridView.Location = new System.Drawing.Point(3, 3);
            myDataGridView.Name = "myDataGridView";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            myDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            myDataGridView.RowHeadersVisible = false;
            myDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            myDataGridView.Size = new System.Drawing.Size(842, 262);
            myDataGridView.TabIndex = 0;
            myDataGridView.DataError +=
      new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            myDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            myDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            myDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            myDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            myDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            myDataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            myDataGridView.Sorted += new EventHandler(this.dataGridView1_Sorted);
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewImageColumn1.DataPropertyName = "Icon";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle3.NullValue")));
            dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewImageColumn1.HeaderText = "";
            dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            dataGridViewImageColumn1.ReadOnly = true;
            dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            dataGridViewImageColumn1.Width = 20;
            // 
            // NameCol
            // 
            NameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            NameCol.ContextMenuStrip = contextMenuStrip1;
            NameCol.DataPropertyName = "Name";
            // NameCol.FillWeight = 200F;
            NameCol.HeaderText = "Name";
            NameCol.Name = "NameCol";
            NameCol.Width = 300;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildStripMenuItem4,
            this.compareStripMenuItem,
            this.copyStripMenuItem,
          //  this.customStripMenuItem,
            this.toolStripMenuItem4,
       //     this.hotKeyStripMenuItem,
       //     this.manualTimeStripMenuItem,
            this.metadataStripMenuItem,
            this.newToolStripMenuItem1,
            this.openWithToolStripMenuItem,
            this.runToolStripMenuItem 
        //    this.statusStripMenuItem
            });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(132, 70);
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(131, 22);
            this.toolStripMenuItem4.Text = "Delete";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.DeleteStripMenuItem4_Click);

            // 
            this.buildStripMenuItem4.Name = "buildStripMenuItem4";
            this.buildStripMenuItem4.Size = new System.Drawing.Size(131, 22);
            this.buildStripMenuItem4.Text = "Build";
            this.buildStripMenuItem4.Click += new System.EventHandler(this.buildStripMenuItem4_Click_1);
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // copyStripMenuItem
            // 
            this.copyStripMenuItem.Name = "copyStripMenuItem";
            this.copyStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyStripMenuItem.Text = "Copy";
            this.copyStripMenuItem.Click += new System.EventHandler(this.copyStripMenuItem_Click);

            // 
            // 
            // descriptionStripMenuItem
            // 
            this.descriptionStripMenuItem.Name = "descriptionStripMenuItem";
            this.descriptionStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.descriptionStripMenuItem.Text = "Modify Metadata";
            this.descriptionStripMenuItem.Click += new System.EventHandler(this.descriptionStripMenuItem_Click);

            // 
            // hotKeysStripMenuItem
            // 
            this.hotKeyStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHotKeyStripMenuItem,
            this.removeHotKeyStripMenuItem});
            this.hotKeyStripMenuItem.Name = "hotKeyStripMenuItem";
            this.hotKeyStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hotKeyStripMenuItem.Text = "HotKeys";
            // 
            // metadataStripMenuItem
            // 
            this.metadataStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.descriptionStripMenuItem,
            this.hotKeyStripMenuItem
            });
            this.metadataStripMenuItem.Name = "metadataStripMenuItem";
            this.metadataStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.metadataStripMenuItem.Text = "Metadata";
            // 
            // addHotKeyToolStripMenuItem
            // 
            this.addHotKeyStripMenuItem.Name = "addHotKeyToolStripMenuItem";
            this.addHotKeyStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addHotKeyStripMenuItem.Text = "Add HotKey";
            this.addHotKeyStripMenuItem.Click += new System.EventHandler(this.addHotKeyToolStripMenuItem_Click_1);
            // 
            // removeHotKeyToolStripMenuItem
            // 
            this.removeHotKeyStripMenuItem.Name = "removeHotKeyToolStripMenuItem";
            this.removeHotKeyStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.removeHotKeyStripMenuItem.Text = "Remove HotKey";
            this.removeHotKeyStripMenuItem.Click += new System.EventHandler(this.removeHotKeyToolStripMenuItem_Click_1);
            // 
            // compareStripMenuItem
            // 
            this.compareStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectActualToolStripMenuItem,
            this.selectIdealToolStripMenuItem});
            this.compareStripMenuItem.Name = "compareStripMenuItem";
            this.compareStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.compareStripMenuItem.Text = "Compare";
            // 
            // selectActualToolStripMenuItem
            // 
            this.selectActualToolStripMenuItem.Name = "selectActualToolStripMenuItem";
            this.selectActualToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.selectActualToolStripMenuItem.Text = "Select Actual.txt";
            this.selectActualToolStripMenuItem.Click += new System.EventHandler(this.selectActualToolStripMenuItem_Click_1);
            // 
            // selectIdealToolStripMenuItem
            // 
            this.selectIdealToolStripMenuItem.Name = "selectIdealToolStripMenuItem";
            this.selectIdealToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.selectIdealToolStripMenuItem.Text = "Select Ideal.txt";
            this.selectIdealToolStripMenuItem.Click += new System.EventHandler(this.selectIdealToolStripMenuItem_Click_1);
            // 
            // openWithToolStripMenuItem
            // 
            this.openWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notepadToolStripMenuItem,
            this.notepadToolStripMenuItem1,
            this.visualStudioToolStripMenuItem,
            this.WindowsExplorerStripMenuItem2,
            this.wordPadToolStripMenuItem1});
            this.openWithToolStripMenuItem.Name = "openWithToolStripMenuItem";
            this.openWithToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.openWithToolStripMenuItem.Text = "Open With";
            // 
            // notepadToolStripMenuItem
            // 
            this.notepadToolStripMenuItem.Name = "notepadToolStripMenuItem";
            this.notepadToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.notepadToolStripMenuItem.Text = "Notepad";
            this.notepadToolStripMenuItem.Click += new System.EventHandler(this.notepadToolStripMenuItem_Click);
            // 
            // notepadToolStripMenuItem1
            // 
            this.notepadToolStripMenuItem1.Name = "notepadToolStripMenuItem1";
            this.notepadToolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            this.notepadToolStripMenuItem1.Text = "Notepad++";
            this.notepadToolStripMenuItem1.Click += new System.EventHandler(this.notepadToolStripMenuItem1_Click);
            // 
            // visualStudioToolStripMenuItem
            // 
            this.visualStudioToolStripMenuItem.Name = "visualStudioToolStripMenuItem";
            this.visualStudioToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.visualStudioToolStripMenuItem.Text = "Visual Studio";
            this.visualStudioToolStripMenuItem.Click += new System.EventHandler(this.visualStudioToolStripMenuItem_Click);

            // 
            // wordPadToolStripMenuItem1
            // 
            this.wordPadToolStripMenuItem1.Name = "wordPadToolStripMenuItem1";
            this.wordPadToolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            this.wordPadToolStripMenuItem1.Text = "WordPad";
            this.wordPadToolStripMenuItem1.Click += new System.EventHandler(this.wordPadToolStripMenuItem1_Click);
            // 
            // 
            // WindowsExplorerStripMenuItem2
            // 
            this.WindowsExplorerStripMenuItem2.Name = "WindowsExplorerStripMenuItem2";
            this.WindowsExplorerStripMenuItem2.Size = new System.Drawing.Size(136, 22);
            this.WindowsExplorerStripMenuItem2.Text = "Windows Explorer";
            this.WindowsExplorerStripMenuItem2.Click += new System.EventHandler(this.WindowsExplorerStripMenuItem2_Click);
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.folderToolStripMenuItem1,
            this.toolStripMenuItem5,
            this.textDocumentToolStripMenuItem,
            this.urlShortcutToolStripMenuItem,
            this.wordPadToolStripMenuItem,
            this.fileShortcutToolStripMenuItem});
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.newToolStripMenuItem1.Text = "New";
            // 
            // folderToolStripMenuItem1
            // 
            this.folderToolStripMenuItem1.Name = "folderToolStripMenuItem1";
            this.folderToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.folderToolStripMenuItem1.Text = "Folder";
            this.folderToolStripMenuItem1.Click += new System.EventHandler(this.folderToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem5.Text = "SubCategory";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.subCategoryStripMenuItem5_Click);
            // 
            // textDocumentToolStripMenuItem
            // 
            this.textDocumentToolStripMenuItem.Name = "textDocumentToolStripMenuItem";
            this.textDocumentToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.textDocumentToolStripMenuItem.Text = "Text Document";
            this.textDocumentToolStripMenuItem.Click += new System.EventHandler(this.textDocumentToolStripMenuItem_Click);
            // 
            // wordPadToolStripMenuItem
            // 
            this.wordPadToolStripMenuItem.Name = "wordPadToolStripMenuItem";
            this.wordPadToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.wordPadToolStripMenuItem.Text = "WordPad";
            this.wordPadToolStripMenuItem.Click += new System.EventHandler(this.wordPadToolStripMenuItem_Click);
            // 
            // urlShortcutToolStripMenuItem
            // 
            this.urlShortcutToolStripMenuItem.Name = "urlShortcutToolStripMenuItem";
            this.urlShortcutToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.urlShortcutToolStripMenuItem.Text = "Url Shortcut";
            this.urlShortcutToolStripMenuItem.Click += new System.EventHandler(this.urlShortcutToolStripMenuItem_Click);
            // 
            // fileShortcutToolStripMenuItem
            // 
            this.fileShortcutToolStripMenuItem.Name = "fileShortcutToolStripMenuItem";
            this.fileShortcutToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.fileShortcutToolStripMenuItem.Text = "File Shortcut";
            this.fileShortcutToolStripMenuItem.Click += new System.EventHandler(this.fileShortcutToolStripMenuItem_Click);
            // 
            // HotKeyCol
            // 
            HotKeyCol.DataPropertyName = "HotKey";
            HotKeyCol.HeaderText = "HotKey";
            HotKeyCol.Name = "HotKeyCol";
            HotKeyCol.ReadOnly = true;
            // 
            // DescriptionCol
            // 
            DescriptionCol.DataPropertyName = "Description";
            DescriptionCol.HeaderText = "Description";
            DescriptionCol.Name = "DescriptionCol";
            DescriptionCol.ReadOnly = true;
            // 
            // TotalExecutionsCol
            // 
            TotalExecutionsCol.DataPropertyName = "TotalExecutions";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            TotalExecutionsCol.DefaultCellStyle = dataGridViewCellStyle4;
            TotalExecutionsCol.HeaderText = "Total Executions";
            TotalExecutionsCol.Name = "TotalExecutionsCol";
            TotalExecutionsCol.ReadOnly = true;
            // 
            // SuccessfulExecutionsCol
            // 
            SuccessfulExecutionsCol.DataPropertyName = "SuccessfulExecutions";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            SuccessfulExecutionsCol.DefaultCellStyle = dataGridViewCellStyle5;
            SuccessfulExecutionsCol.HeaderText = "Successful Executions";
            SuccessfulExecutionsCol.Name = "SuccessfulExecutionsCol";
            SuccessfulExecutionsCol.ReadOnly = true;
            SuccessfulExecutionsCol.Width = 75;
            // 
            // PercentCorrectCol
            // 
            PercentCorrectCol.DataPropertyName = "PercentCorrect";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            PercentCorrectCol.DefaultCellStyle = dataGridViewCellStyle6;
            PercentCorrectCol.HeaderText = "Percent Correct";
            PercentCorrectCol.Name = "PercentCorrectCol";
            PercentCorrectCol.ReadOnly = true;
            // 
            // LastExecutedCol
            // 
            LastExecutedCol.DataPropertyName = "LastExecuted";
            LastExecutedCol.HeaderText = "Last Executed";
            LastExecutedCol.Name = "LastExecutedCol";
            LastExecutedCol.ReadOnly = true;
            LastExecutedCol.Width = 125;
            // 
            // SizeCol
            // 
            SizeCol.DataPropertyName = "Size";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            SizeCol.DefaultCellStyle = dataGridViewCellStyle7;
            SizeCol.HeaderText = "Size";
            SizeCol.Name = "SizeCol";
            SizeCol.ReadOnly = true;
            SizeCol.Width = 60;
            // 
            // AvgExecutionTimeCol
            // 
            AvgExecutionTimeCol.DataPropertyName = "AvgExecutionTime";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            AvgExecutionTimeCol.DefaultCellStyle = dataGridViewCellStyle8;
            AvgExecutionTimeCol.HeaderText = "Avg Time Secs";
            AvgExecutionTimeCol.Name = "AvgExecutionTimeCol";
            AvgExecutionTimeCol.ReadOnly = true;
            // 
            // ManualExecutionTimeCol
            // 
            ManualExecutionTimeCol.DataPropertyName = "ManualExecutionTime";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            ManualExecutionTimeCol.DefaultCellStyle = dataGridViewCellStyle9;
            ManualExecutionTimeCol.HeaderText = "Manual Time Secs";
            ManualExecutionTimeCol.Name = "ManualExecutionTimeCol";
            ManualExecutionTimeCol.ReadOnly = true;
            ManualExecutionTimeCol.Width = 75;
            // 
            // TotalSavingsCol
            // 
            TotalSavingsCol.DataPropertyName = "TotalSavings";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            TotalSavingsCol.DefaultCellStyle = dataGridViewCellStyle10;
            TotalSavingsCol.HeaderText = "TotalSavings";
            TotalSavingsCol.Name = "TotalSavingsCol";
            TotalSavingsCol.ReadOnly = true;
            // 
            // Type
            // 
            Type.DataPropertyName = "Type";
            Type.HeaderText = "Type";
            Type.Name = "Type";
            Type.ReadOnly = true;
            // 
            // DateModifiedCol
            // 
            DateModifiedCol.DataPropertyName = "DateModified";
            DateModifiedCol.HeaderText = "Date Modified";
            DateModifiedCol.Name = "DateModifiedCol";
            DateModifiedCol.ReadOnly = true;
            DateModifiedCol.Width = 150;
            // 
            // StatusCol
            // 
            StatusCol.DataPropertyName = "Status";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            StatusCol.DefaultCellStyle = dataGridViewCellStyle9;
            StatusCol.HeaderText = "Status";
            StatusCol.Name = "StatusCol";
            StatusCol.ReadOnly = true;
            StatusCol.Width = 75;
            // 
            // CustomCol
            // 
            CustomCol.DataPropertyName = "Custom";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            CustomCol.DefaultCellStyle = dataGridViewCellStyle9;
            CustomCol.HeaderText = "Custom";
            CustomCol.Name = "CustomCol";
            CustomCol.ReadOnly = true;
            CustomCol.Width = 75;
            // 
            // FullName
            // 
            FullName.DataPropertyName = "FullName";
            FullName.HeaderText = "FullName";
            FullName.Name = "FullName";
            FullName.ReadOnly = true;
            FullName.Visible = false;
            // 
            // NestingLevel
            // 
            NestingLevel.DataPropertyName = "NestingLevel";
            NestingLevel.HeaderText = "NestingLevel";
            NestingLevel.Name = "NestingLevel";
            NestingLevel.Visible = false;
            ArrayList myArrayList = new ArrayList();
            Methods myActions = new Methods();
            myArrayList = myActions.ReadAppDirectoryKeyToArrayList("ColumnOrder_" + pstrInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            if (myArrayList.Count > 0) {
                List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
                foreach (var item in myArrayList) {
                    string[] columnArray = item.ToString().Split('^');
                    ColumnOrderItem myColumnOrderItem = new ColumnOrderItem();
                    int myInt = 0;
                    Int32.TryParse(columnArray[0], out myInt);
                    myColumnOrderItem.DisplayIndex = myInt;
                    myInt = 0;
                    Int32.TryParse(columnArray[1], out myInt);
                    myColumnOrderItem.Width = myInt;
                    if (columnArray[2].ToLower() == "true") {
                        myColumnOrderItem.Visible = true;
                    } else {
                        myColumnOrderItem.Visible = false;
                    }
                    myInt = 0;
                    Int32.TryParse(columnArray[3], out myInt);
                    myColumnOrderItem.ColumnIndex = myInt;
                    columnOrder.Add(myColumnOrderItem);
                }


                if (columnOrder != null) {
                    var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
                    foreach (var item in sorted) {
                        myDataGridView.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                        myDataGridView.Columns[item.ColumnIndex].Visible = item.Visible;
                        myDataGridView.Columns[item.ColumnIndex].Width = item.Width;
                    }
                }
            }

            this.contextMenuStrip1.ResumeLayout(false);
            // 
            // FileViewBindingSource
            // 
            // this.FileViewBindingSource.DataSource = typeof(System.Windows.Forms.Samples.FileView);
            // 

            //&&&&&&&&&&&&&&&
            // 
            // _CurrentSplitContainer
            // 
            _CurrentSplitContainer = new SplitContainer();
            _CurrentSplitContainer.Dock = System.Windows.Forms.DockStyle.Top;
            _CurrentSplitContainer.Location = new System.Drawing.Point(0, 63);
            _CurrentSplitContainer.Name = "_CurrentSplitContainer" + _CurrentIndex.ToString();
            // 
            // _CurrentSplitContainer.Panel1
            // 
            //    _CurrentSplitContainer.Panel1.Controls.Add(this.tabControl1);
            _CurrentSplitContainer.Size = new System.Drawing.Size(856, Screen.PrimaryScreen.Bounds.Height - 155);
            _CurrentSplitContainer.SplitterDistance = 499;
            _CurrentSplitContainer.TabIndex = 11;
            //mySplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(mySplitContainer_SplitterMoved);
            //mySplitContainer.MouseEnter += new System.EventHandler(mySplitContainer_MouseEnter);
            //mySplitContainer.MouseLeave += new System.EventHandler(mySplitContainer_MouseLeave);

            myDataGridView.Height = Screen.PrimaryScreen.WorkingArea.Size.Height - 150;


            if (_CurrentIndex == tabControl1.TabCount - 1) {
                _CurrentSplitContainer.Panel1.Controls.Add(myDataGridView);
                tabControl1.TabPages[_CurrentIndex - 1].Controls.Add(_CurrentSplitContainer);

                //    tabControl1.TabPages[_CurrentIndex - 1].Controls.Add(myDataGridView);
            } else {
                _CurrentSplitContainer.Panel1.Controls.Add(myDataGridView);
                tabControl1.TabPages[_CurrentIndex].Controls.Add(_CurrentSplitContainer);

                // tabControl1.TabPages[_CurrentIndex].Controls.Add(myDataGridView);
            }
            foreach (DataGridViewColumn item in myDataGridView.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
            }

        }

        private void dataGridView1_Sorted(object sender, EventArgs e) {
            Methods myActions = new Methods();
            if (_CurrentDataGridView.SortedColumn != null) {
                myActions.SetValueByKey("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), _CurrentDataGridView.SortedColumn.Index.ToString());
                myActions.SetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), _CurrentDataGridView.SortOrder.ToString());
            }
        }

        private void selectIdealToolStripMenuItem_Click_1(object sender, EventArgs e) {
            string fullFileName = "";
            string fileNamea = "";
            FileView myFileView;
            bool isDirectory = false;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                //if (myCell.ColumnIndex != 0 && myCell.RowIndex != 0) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                fullFileName = myFileView.FullName;
                fileNamea = myFileView.Name;
                if (myFileView.IsDirectory) {
                    isDirectory = true;
                }
                //}

            }
            Methods myActions = new Methods();
            if (fullFileName == "") {
                myActions.MessageBoxShow("Please select a row to copy before selecting File/Copy");
                return;
            }

            if (isDirectory) {
                myActions.MessageBoxShow("Folders cannot be selected as ideal.txt");
                return;
            }

            if (!Directory.Exists(@"C:\Data")) {
                Directory.CreateDirectory(@"C:\Data");
            }

            FileInfo fi = new FileInfo(fullFileName);
            DirectoryInfo target = new DirectoryInfo(@"C:\Data");
            string newFile = Path.Combine(target.FullName, "ideal.txt");
            fi.CopyTo(newFile, true);
            //Close the running process
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            myActions.KillAllProcessesByProcessName("notepad++");
            string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
            string strContent = @"C:\Data\ideal.txt";
            //Close the running process
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            //tries to start the process 
            try {
                if (!File.Exists(@"C:\Program Files (x86)\Notepad++\notepad++.exe")) {
                    myActions.MessageBoxShow(" You need to download notepad++ to use this feature.\n\r\n\rFile not found: " + @"C:\Program Files (x86)\Notepad++\notepad++.exe");
                } else {
                    _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
                }
            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }


            System.Threading.Thread.Sleep(500);
            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                System.Threading.Thread.Sleep(10);
                _proc.Refresh();
            }

            _proc.WaitForInputIdle();
            _appHandle = _proc.MainWindowHandle;

            SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);           
            SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
            SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        private void selectActualToolStripMenuItem_Click_1(object sender, EventArgs e) {
            string fullFileName = "";
            string fileNamea = "";
            FileView myFileView;
            bool isDirectory = false;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                //if (myCell.ColumnIndex != 0 && myCell.RowIndex != 0) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                fullFileName = myFileView.FullName;
                fileNamea = myFileView.Name;
                if (myFileView.IsDirectory) {
                    isDirectory = true;
                }
                //}

            }
            Methods myActions = new Methods();
            if (fullFileName == "") {
                myActions.MessageBoxShow("Please select a row to copy before selecting File/Copy");
                return;
            }

            if (isDirectory) {
                myActions.MessageBoxShow("Folders cannot be selected as actual.txt");
                return;
            }

            if (!Directory.Exists(@"C:\Data")) {
                Directory.CreateDirectory(@"C:\Data");
            }

            FileInfo fi = new FileInfo(fullFileName);
            DirectoryInfo target = new DirectoryInfo(@"C:\Data");
            string newFile = Path.Combine(target.FullName, "Actual.txt");
            fi.CopyTo(newFile, true);
            //Close the running process
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            myActions.KillAllProcessesByProcessName("notepad++");
            string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
            string strContent = @"C:\Data\Actual.txt";
            //Close the running process
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            //tries to start the process 
            try {
                if (!File.Exists(@"C:\Program Files (x86)\Notepad++\notepad++.exe")) {
                    myActions.MessageBoxShow(" You need to download notepad++ to use this feature.\n\r\n\rFile not found: " + @"C:\Program Files (x86)\Notepad++\notepad++.exe");
                } else {
                    _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
                }
            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }


            System.Threading.Thread.Sleep(500);
            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                System.Threading.Thread.Sleep(10);
                _proc.Refresh();
            }

            _proc.WaitForInputIdle();
            _appHandle = _proc.MainWindowHandle;

            SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);           
            SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
            SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            Methods myActions = new Methods();
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            this._CurrentDataGridView.DataSource = null;
            this._CurrentDataGridView.DataSource = this._CurrentFileViewBindingSource;
            //   this._CurrentDataGridView.Sort(_CurrentDataGridView.Columns[1], ListSortDirection.Ascending);
            // AddGlobalHotKeys();
        }

        private void ExplorerView_ClientSizeChanged(object sender, EventArgs e) {
        }

        private void buildStripMenuItem4_Click_1(object sender, EventArgs e) {
            FileView myFileView;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myFileView.FullName.ToString(),
                       myFileView.Name + ".sln",
                        SearchOption.AllDirectories)) {
                        // Display file path.
                        ev_Build_File(file);
                    }

                } else {
                    ev_Build_File(myFileView.FullName.ToString());
                }
            }
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            Methods myActions = new Methods();
            myActions.SetValueByKey("ExpandCollapseAll", "Collapse");
            RefreshDataGrid();
            RefreshDataGrid();
            this.Cursor = Cursors.Default;
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            Methods myActions = new Methods();
            myActions.SetValueByKey("ExpandCollapseAll", "Expand");
            RefreshDataGrid();
            this.Cursor = Cursors.Default;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            RefreshDataGrid();
            this.Cursor = Cursors.Default;
        }

        private void totalSavingsToolStripMenuItem_Click(object sender, EventArgs e) {
            int intTotalSavingsForAllScripts = 0;
            foreach (DataGridViewRow item in _CurrentDataGridView.Rows) {
                intTotalSavingsForAllScripts += (int)item.Cells["TotalSavingsCol"].Value;
            }
            TimeSpan diff = TimeSpan.FromSeconds(intTotalSavingsForAllScripts);
            string formatted = string.Format(
                  CultureInfo.CurrentCulture,
                  "{0} years, {1} months, {2} days, {3} hours, {4} minutes, {5} seconds",
                  diff.Days / 365,
                  (diff.Days - (diff.Days / 365) * 365) / 30,
                  (diff.Days - (diff.Days / 365) * 365) - ((diff.Days - (diff.Days / 365) * 365) / 30) * 30,
                  diff.Hours,
                  diff.Minutes,
                  diff.Seconds);

            MessageBox.Show("Total Savings shows total savings for current tab.\r\nTo see total savings for current tab, you need to use toolbar View\\ExpandAll and View\\Refresh prior to using View\\Total Savings.\r\nTotal Savings: " + formatted);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myFileView.FullName.ToString(),
                       myFileView.Name + ".exe",
                        SearchOption.AllDirectories)) {
                        // Display file path.
                        if (file.Contains("bin\\Debug")) {
                            ev_Process_File(file);
                        }
                    }

                } else {
                    ev_Process_File(myFileView.FullName.ToString());
                }

            }
        }

        private void visualStudioToolStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myFileView.FullName.ToString(),
                       myFileView.Name + ".sln",
                        SearchOption.AllDirectories)) {
                        // Display file path.
                        ev_Process_File(file);
                    }

                } else {
                    ev_Process_File(myFileView.FullName.ToString());
                }
            }
        }

        private void copyStripMenuItem_Click(object sender, EventArgs e) {
            string fullFileName = "";
            string fileNamea = "";
            FileView myFileView;
            bool isDirectory = false;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                //if (myCell.ColumnIndex != 0 && myCell.RowIndex != 0) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                fullFileName = myFileView.FullName;
                fileNamea = myFileView.Name;
                if (myFileView.IsDirectory) {
                    isDirectory = true;
                }
                //}

            }
            Methods myActions = new Methods();
            if (fullFileName == "") {
                myActions.MessageBoxShow("Please select a row to copy before selecting File/Copy");
                return;
            }

            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            DisplayCopyWindow:
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
            myControlEntity.Text = "Copy File/Folder";
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
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxCopyFolderSelectedValue");
            myControlEntity.ID = "cbxCopyFolder";
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



            DisplayCopyWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayCopyWindow:
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }


            string strFolder = myListControlEntity.Find(x => x.ID == "cbxCopyFolder").SelectedValue;
            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;

            myActions.SetValueByKey("cbxCopyFolderSelectedValue", strFolder);

            if (strButtonPressed == "btnSelectFolder") {
                var dialog1 = new System.Windows.Forms.FolderBrowserDialog();
                dialog1.SelectedPath = myActions.GetValueByKey("LastSearchCopyFolder");


                System.Windows.Forms.DialogResult result = dialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && Directory.Exists(dialog1.SelectedPath)) {
                    myListControlEntity.Find(x => x.ID == "cbxCopyFolder").SelectedValue = dialog1.SelectedPath;
                    myListControlEntity.Find(x => x.ID == "cbxCopyFolder").SelectedKey = dialog1.SelectedPath;
                    myListControlEntity.Find(x => x.ID == "cbxCopyFolder").Text = dialog1.SelectedPath;

                    myActions.SetValueByKey("LastSearchCopyFolder", dialog1.SelectedPath);
                    strFolder = dialog1.SelectedPath;
                    myActions.SetValueByKey("cbxCopyFolderSelectedValue", strFolder);
                    string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                    string fileName = "cbxCopyFolder.txt";
                    string strApplicationBinDebug = Application.StartupPath;
                    string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

                    string settingsDirectory =
       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
                    string strNewHostName = dialog1.SelectedPath;
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
                    goto DisplayCopyWindowAgain;
                }
            }

            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {

                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplayCopyWindow;
                }

                strFolderToUse = strFolder;
                if (isDirectory) {
                    Copy(fullFileName, strFolder);
                } else {
                    CopyFile(fullFileName, strFolder);
                }



                this.Cursor = Cursors.WaitCursor;
                RefreshDataGrid();
                this.Cursor = Cursors.Default;
                return;
            }

            if (strButtonPressed == "btnOkay") {
                strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 300, 1200, 100, 100);
                goto LineAfterDisplayCopyWindow;
            }
        }

        private void addHotKeyToolStripMenuItem_Click_1(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Add HotKey";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter HotKey Character";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "You need to restart explorer after setting new hotkey";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 2;
            myControlEntity.BackgroundColor = Media.Colors.Red;
            myControlEntity.ForegroundColor = Media.Colors.White;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string myHotKey = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myFileView.FullName.ToString(),
                       myFileView.Name + ".exe",
                        SearchOption.AllDirectories)) {
                        string newHotKeyScriptUniqueName = myActions.ConvertFullFileNameToScriptPath(myFileView.FullName) + "-" + myFileView.Name;
                        // Display file path.
                        if (file.Contains("bin\\Debug")) {
                            ArrayList myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
                            ArrayList newArrayList = new ArrayList();
                            bool boolScriptFound = false;
                            foreach (var item in myArrayList) {
                                string[] myScriptInfoFields = item.ToString().Split('^');
                                string scriptName = myScriptInfoFields[0];
                                string strHotKeyExecutable = myScriptInfoFields[5];
                                if (scriptName == newHotKeyScriptUniqueName && file == strHotKeyExecutable) {
                                    boolScriptFound = true;
                                    string strHotKey = myScriptInfoFields[1];
                                    string strTotalExecutions = myScriptInfoFields[2];
                                    string strSuccessfulExecutions = myScriptInfoFields[3];
                                    string strLastExecuted = myScriptInfoFields[4];
                                    int intTotalExecutions = 0;
                                    Int32.TryParse(strTotalExecutions, out intTotalExecutions);
                                    int intSuccessfulExecutions = 0;
                                    Int32.TryParse(strSuccessfulExecutions, out intSuccessfulExecutions);
                                    DateTime dateLastExecuted = DateTime.MinValue;
                                    DateTime.TryParse(strLastExecuted, out dateLastExecuted);
                                    newArrayList.Add(scriptName + "^" +
                                        "Ctrl+Alt+" + myHotKey + "^" +
                                         myScriptInfoFields[2] + "^" +
                                         myScriptInfoFields[3] + "^" +
                                         myScriptInfoFields[4] + "^" +
                                         myScriptInfoFields[5] + "^"
                                        );
                                } else {
                                    newArrayList.Add(item.ToString());
                                }
                            }
                            if (boolScriptFound == false) {
                                newArrayList.Add(newHotKeyScriptUniqueName + "^" +
                                       "Ctrl+Alt+" + myHotKey + "^" +
                                        "0" + "^" +
                                        "0" + "^" +
                                       null + "^" +
                                       file + "^"
                                       );
                            }
                            myActions.WriteArrayListToAppDirectoryKeyGlobal("ScriptInfo", newArrayList);
                        }
                    }

                } else {
                    ev_Process_File(myFileView.FullName.ToString());
                }

            }
            // refresh datagridview

            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            this._CurrentDataGridView.DataSource = null;
            this._CurrentDataGridView.DataSource = this._CurrentFileViewBindingSource;
            //   this._CurrentDataGridView.Sort(_CurrentDataGridView.Columns[1], ListSortDirection.Ascending);
            // AddGlobalHotKeys();
        }

        private void removeHotKeyToolStripMenuItem_Click_1(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                bool boolScriptFound = false;
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myFileView.FullName.ToString(),
                       myFileView.Name + ".exe",
                        SearchOption.AllDirectories)) {
                        // Display file path.
                        if (file.Contains("bin\\Debug")) {
                            ArrayList myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
                            ArrayList newArrayList = new ArrayList();
                            foreach (var item in myArrayList) {
                                string[] myScriptInfoFields = item.ToString().Split('^');
                                string scriptName = myScriptInfoFields[0];
                                if (scriptName == myActions.ConvertFullFileNameToScriptPath(myFileView.FullName) + "-" + myFileView.Name) {
                                    boolScriptFound = true;
                                } else {
                                    newArrayList.Add(item.ToString());
                                }
                            }
                            myActions.WriteArrayListToAppDirectoryKeyGlobal("ScriptInfo", newArrayList);
                        }
                    }
                    if (boolScriptFound == false) {
                        myActions.MessageBoxShow(@"Could not find the HotKey; Script may have been moved; you can manually delete the row from AppData\Roaming\IdealAutomate\ScriptInfo.txt");
                    }
                } else {
                    ev_Process_File(myFileView.FullName.ToString());
                }

            }
            // refresh datagridview

            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory, _myArrayList);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            this._CurrentDataGridView.DataSource = null;
            this._CurrentDataGridView.DataSource = this._CurrentFileViewBindingSource;
            //  this._CurrentDataGridView.Sort(_CurrentDataGridView.Columns[1], ListSortDirection.Ascending);
            // AddGlobalHotKeys();
        }

        private void manualTimeStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Add Manual Time";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter Manual Time in Seconds";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string myManualExecutionTime = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myFileView.FullName.ToString(),
                       myFileView.Name + ".exe",
                        SearchOption.AllDirectories)) {
                        // Display file path.
                        if (file.Contains("bin\\Debug")) {
                            string fileFullName = myFileView.FullName;
                            myActions.SetValueByKeyForNonCurrentScript("ManualExecutionTime", myManualExecutionTime, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
                        }
                    }

                } else {
                    ev_Process_File(myFileView.FullName.ToString());
                }

            }
        }
        private void descriptionStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            FileView myManualTimeFileView;
            string fileFullName = "";
            string fileManualTimeFullName = "";
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());                
                fileFullName = myFileView.FullName;
            }

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myManualTimeFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myManualTimeFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    foreach (string file in Directory.EnumerateFiles(myManualTimeFileView.FullName.ToString(),
                       myManualTimeFileView.Name + ".exe",
                        SearchOption.AllDirectories)) {
                        // Display file path.
                        if (file.Contains("bin\\Debug")) {
                            fileManualTimeFullName = myManualTimeFileView.FullName;

                        }
                    }

                }

            }
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Add Metadata Fields";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            int intRowCtr = 0;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblCustom";
            myControlEntity.Text = "Custom";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByPublicKeyInCurrentFolder("custom", fileFullName);
            myControlEntity.ID = "cbxCustom";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxCustom) {
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
            myControlEntity.ID = "lbldescription";
            myControlEntity.Text = "Description";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.Text = myActions.GetValueByPublicKeyInCurrentFolder("description", fileFullName);
            myControlEntity.ID = "txtDescription";
            myControlEntity.Multiline = true;
            myControlEntity.Height = 200;
            myControlEntity.TextWrap = true;
            myControlEntity.Width = 650;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxdescription) {
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
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter Manual Time in Seconds";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = myActions.GetValueByKeyForNonCurrentScript("ManualExecutionTime", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileManualTimeFullName));
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblStatus";
            myControlEntity.Text = "Status";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByPublicKeyInCurrentFolder("status", fileFullName);
            myControlEntity.ID = "cbxStatus";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxStatus) {
            //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
            //}
            //myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;

            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            DisplaydescriptionWindow:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }
            string strCustom = myListControlEntity.Find(x => x.ID == "cbxCustom").SelectedValue;
            string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;
            string myManualExecutionTime = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;
            myActions.SetValueByPublicKeyInCurrentFolder("cbxCustomSelectedValue", strCustom, fileFullName);
            myActions.SetValueByPublicKeyInCurrentFolder("custom", strCustom, fileFullName);
            myActions.SetValueByPublicKeyInCurrentFolder("description", strdescription, fileFullName);
            myActions.SetValueByPublicKeyInCurrentFolder("status", strStatus, fileFullName);
            myActions.SetValueByPublicKeyInCurrentFolder("cbxStatusSelectedValue", strStatus, fileFullName);
            myActions.SetValueByKeyForNonCurrentScript("ManualExecutionTime", myManualExecutionTime, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileManualTimeFullName));



        }

        private void favoritesToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
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
            myControlEntity.Text = "Favorites";
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
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFavoriteFolderSelectedValue");
            myControlEntity.ID = "cbxFavoriteFolder";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = @"Here is an example: C:\Users\harve\Documents\GitHub";
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSelectFolder";
            myControlEntity.Text = "Add Folder...";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 3;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDeleteFolder";
            myControlEntity.Text = "Delete Folder...";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 3;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            DisplayWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }


            string strFolder = myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").SelectedValue;
            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").SelectedKey;

            myActions.SetValueByKey("cbxFavoriteFolderSelectedValue", strFolder);

            if (strButtonPressed == "btnDeleteFolder") {
                strFolder = myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").SelectedValue;
                string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                string fileName = "cbxFavoriteFolder.txt";
                string strApplicationBinDebug = Application.StartupPath;
                string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

                string settingsDirectory =
       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
                string strNewHostName = myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").Text;
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
                    alHostsNew.OrderBy(x => x._Value);
                    for (int i = 0; i < alHostsNew.Count; i++) {
                        if (alHostsNew[i]._Key != "" && alHostsNew[i]._Key != strFolder) {
                            objSWFile.WriteLine(alHostsNew[i]._Key + '^' + alHostsNew[i]._Value);
                        }
                    }
                    objSWFile.Close();
                }
                goto DisplayWindowAgain;

            }

            if (strButtonPressed == "btnSelectFolder") {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");


                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && Directory.Exists(dialog.SelectedPath)) {
                    myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").SelectedValue = dialog.SelectedPath;
                    myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").SelectedKey = dialog.SelectedPath;
                    myListControlEntity.Find(x => x.ID == "cbxFavoriteFolder").Text = dialog.SelectedPath;

                    myActions.SetValueByKey("LastSearchFolder", dialog.SelectedPath);
                    strFolder = dialog.SelectedPath;
                    myActions.SetValueByKey("cbxFavoriteFolderSelectedValue", strFolder);
                    string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                    string fileName = "cbxFavoriteFolder.txt";
                    string strApplicationBinDebug = Application.StartupPath;
                    string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");

                    string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
                        alHostsNew.OrderBy(x => x._Value);
                        for (int i = 0; i < alHostsNew.Count; i++) {
                            if (alHostsNew[i]._Key != "") {
                                objSWFile.WriteLine(alHostsNew[i]._Key + '^' + alHostsNew[i]._Value);
                            }
                        }
                        objSWFile.Close();
                    }
                    goto DisplayWindowAgain;
                }
            }

            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {

                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }

                strFolderToUse = strFolder;
                myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strFolder);
                this.Cursor = Cursors.WaitCursor;
                RefreshDataGrid();
                this.Cursor = Cursors.Default;
                return;
            }

            if (strButtonPressed == "btnOkay") {
                strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 300, 1200, 100, 100);
                goto LineAfterDisplayWindow;
            }
        }

        private void cbxCurrentPath_SelectedIndexChanged(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), ((ComboBoxPair)(cbxCurrentPath.SelectedItem))._Value);
            if (_newTab == false) {
                RefreshDataGrid();
            }
        }

        private void cbxCurrentPath_TextChanged(object sender, EventArgs e) {

        }

        private void cbxCurrentPath_Leave(object sender, EventArgs e) {
            if (_ignoreSelectedIndexChanged == true) {
                return;
            }
            string strNewHostName = ((ComboBox)sender).Text;
            Methods myActions = new Methods();
            System.Windows.Forms.DialogResult myResult;
            if (!Directory.Exists(strNewHostName)) {

                myResult = myActions.MessageBoxShowWithYesNo("I could not find folder " + strNewHostName + ". Do you want me to create it ? ");
                if (myResult == System.Windows.Forms.DialogResult.Yes) {
                    Directory.CreateDirectory(strNewHostName);
                } else {
                    return;
                }

            }
            List<ComboBoxPair> alHosts = ((ComboBox)sender).Items.Cast<ComboBoxPair>().ToList();
            List<ComboBoxPair> alHostsNew = new List<ComboBoxPair>();

            ComboBoxPair myCbp = new ComboBoxPair(strNewHostName, strNewHostName);
            bool boolNewItem = false;

            alHostsNew.Add(myCbp);

            foreach (ComboBoxPair item in alHosts) {
                if (strNewHostName.ToLower() != item._Key.ToLower()) {
                    boolNewItem = true;
                    alHostsNew.Add(item);
                }
            }
            if (alHostsNew.Count > 24) {
                for (int i = alHostsNew.Count - 1; i > 0; i--) {
                    if (alHostsNew[i]._Key.Trim() != "--Select Item ---") {
                        alHostsNew.RemoveAt(i);
                        break;
                    }
                }
            }

            string fileName = ((ComboBox)sender).Name + ".txt";


            string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";

            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                foreach (ComboBoxPair item in alHostsNew) {
                    if (item._Key != "") {
                        objSWFile.WriteLine(item._Key + '^' + item._Value);
                    }
                }
                objSWFile.Close();
            }

            //  alHosts = alHostsNew;
            if (boolNewItem) {
                ((ComboBox)sender).Items.Clear();
                foreach (var item in alHostsNew) {
                    ((ComboBox)sender).Items.Add(item);
                }
            }
            string strCurrentPath = ((ComboBox)(sender)).Text;

            if (strCurrentPath != myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString())) {
                RefreshDataGrid();
            }

            myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strCurrentPath);

        }

        private void WindowsExplorerStripMenuItem2_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    string strExecutable = @"C:\Windows\explorer.exe";
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You can only open directories with Windows Explorer");
                }
            }
        }

        private void _CurrentSplitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
            //host the started process in the panel 

            try {
                if (_proc != null) {
                    if (_WebBrowserLoaded) {
                        webBrowser1.Size = new System.Drawing.Size(_CurrentSplitContainer.Panel2.ClientSize.Width, _CurrentSplitContainer.Panel2.Height - 50);
                    } else {
                        System.Threading.Thread.Sleep(500);
                        int ctr = 0;
                        while (ctr < 150 && (_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            ctr++;
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                        SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    }
                    Methods myActions = new Methods();
                    myActions.SetValueByKey("_CurrentSplitContainerWidth" + _selectedTabIndex.ToString(), e.X.ToString());
                }
            } catch (Exception ex) {


            }


        }

        private async void search_ClickAsync(object sender, EventArgs e) {
            var myForm = new Search();
            myForm.Show();
       //     _searchErrors.Length = 0;
       //     Methods myActions = new Methods();
       //     myActions = new Methods();
       //     string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");








       //     DisplayFindTextInFilesWindow:
       //     int intRowCtr = 0;
       //     ControlEntity myControlEntity = new ControlEntity();
       //     List<ControlEntity> myListControlEntity = new List<ControlEntity>();
       //     List<ComboBoxPair> cbp = new List<ComboBoxPair>();
       //     List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
       //     List<ComboBoxPair> cbp2 = new List<ComboBoxPair>();
       //     List<ComboBoxPair> cbp3 = new List<ComboBoxPair>();
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.Heading;
       //     myControlEntity.ID = "lbl";
       //     myControlEntity.Text = "Find Text In Files";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ColumnNumber = 0;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());

       //     intRowCtr++;
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.Label;
       //     myControlEntity.ID = "lblFindWhat";
       //     myControlEntity.Text = "FindWhat";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.Width = 150;
       //     myControlEntity.ColumnNumber = 0;
       //     myControlEntity.ColumnSpan = 1;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());



       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.ComboBox;
       //     myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFindWhatSelectedValue");
       //     myControlEntity.ID = "cbxFindWhat";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ToolTipx = "";
       //     //foreach (var item in alcbxFindWhat) {
       //     //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
       //     //}
       //     //myControlEntity.ListOfKeyValuePairs = cbp;
       //     myControlEntity.ComboBoxIsEditable = true;
       //     myControlEntity.ColumnNumber = 1;

       //     myControlEntity.ColumnSpan = 2;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());


       //     intRowCtr++;
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.Label;
       //     myControlEntity.ID = "lblFileType";
       //     myControlEntity.Text = "FileType";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.Width = 150;
       //     myControlEntity.ColumnNumber = 0;
       //     myControlEntity.ColumnSpan = 1;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());


       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.ComboBox;
       //     myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFileTypeSelectedValue");
       //     myControlEntity.ID = "cbxFileType";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ToolTipx = "Here is an example: *.*";
       //     //foreach (var item in alcbxFileType) {
       //     //    cbp1.Add(new ComboBoxPair(item.ToString(), item.ToString()));
       //     //}
       //     //myControlEntity.ListOfKeyValuePairs = cbp1;
       //     myControlEntity.ComboBoxIsEditable = true;
       //     myControlEntity.ColumnNumber = 1;
       //     myControlEntity.ColumnSpan = 2;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());


       //     intRowCtr++;
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.Label;
       //     myControlEntity.ID = "lblExclude";
       //     myControlEntity.Text = "Exclude";
       //     myControlEntity.Width = 150;
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ColumnNumber = 0;
       //     myControlEntity.ColumnSpan = 1;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());


       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.ComboBox;
       //     myControlEntity.SelectedValue = myActions.GetValueByKey("cbxExcludeSelectedValue");
       //     myControlEntity.ID = "cbxExclude";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ToolTipx = "Here is an example: *.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy";
       //     myControlEntity.ComboBoxIsEditable = true;
       //     myControlEntity.ColumnNumber = 1;
       //     myControlEntity.ColumnSpan = 2;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());


       //     intRowCtr++;
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.Label;
       //     myControlEntity.ID = "lblFolder";
       //     myControlEntity.Text = "Folder";
       //     myControlEntity.Width = 150;
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ColumnNumber = 0;
       //     myControlEntity.ColumnSpan = 1;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());

       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.ComboBox;
       //     myControlEntity.SelectedValue = myActions.GetValueByKey("cbxFolderSelectedValue");
       //     myControlEntity.ID = "cbxFolder";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ToolTipx = @"Here is an example: C:\Users\harve\Documents\GitHub";
       //     myControlEntity.ComboBoxIsEditable = true;
       //     myControlEntity.ColumnNumber = 1;
       //     myControlEntity.ColumnSpan = 2;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());

       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.Button;
       //     myControlEntity.ID = "btnSelectFolder";
       //     myControlEntity.Text = "Select Folder or File...";
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ColumnNumber = 3;
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());

       //     intRowCtr++;
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.CheckBox;
       //     myControlEntity.ID = "chkMatchCase";
       //     myControlEntity.Text = "Match Case";
       //     myControlEntity.Width = 150;
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ColumnNumber = 0;
       //     myControlEntity.ColumnSpan = 1;
       //     string strMatchCase = myActions.GetValueByKey("chkMatchCase");

       //     if (strMatchCase.ToLower() == "true") {
       //         myControlEntity.Checked = true;
       //     } else {
       //         myControlEntity.Checked = false;
       //     }
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());

       //     intRowCtr++;
       //     myControlEntity.ControlEntitySetDefaults();
       //     myControlEntity.ControlType = ControlType.CheckBox;
       //     myControlEntity.ID = "chkUseRegularExpression";
       //     myControlEntity.Text = "UseRegularExpression";
       //     myControlEntity.Width = 150;
       //     myControlEntity.RowNumber = intRowCtr;
       //     myControlEntity.ColumnNumber = 0;
       //     myControlEntity.ColumnSpan = 1;
       //     string strUseRegularExpression = myActions.GetValueByKey("chkUseRegularExpression");
       //     if (strUseRegularExpression.ToLower() == "true") {
       //         myControlEntity.Checked = true;
       //     } else {
       //         myControlEntity.Checked = false;
       //     }
       //     myListControlEntity.Add(myControlEntity.CreateControlEntity());

       //     DisplayWindowAgain:
       //     string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
       //     LineAfterDisplayWindow:
       //     if (strButtonPressed == "btnCancel") {
       //         myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
       //         return;
       //     }

       //     boolMatchCase = myListControlEntity.Find(x => x.ID == "chkMatchCase").Checked;
       //     boolUseRegularExpression = myListControlEntity.Find(x => x.ID == "chkUseRegularExpression").Checked;

       //     strFindWhat = myListControlEntity.Find(x => x.ID == "cbxFindWhat").SelectedValue;
       //     //  string strFindWhatKey = myListControlEntity.Find(x => x.ID == "cbxFindWhat").SelectedKey;

       //     string strFileType = myListControlEntity.Find(x => x.ID == "cbxFileType").SelectedValue;
       //     //     string strFileTypeKey = myListControlEntity.Find(x => x.ID == "cbxFileType").SelectedKey;

       //     string strExclude = myListControlEntity.Find(x => x.ID == "cbxExclude").SelectedValue;
       //     //      string strExcludeKey = myListControlEntity.Find(x => x.ID == "cbxExclude").SelectedKey;

       //     string strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
       //     //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;
       //     myActions.SetValueByKey("chkMatchCase", boolMatchCase.ToString());
       //     myActions.SetValueByKey("chkUseRegularExpression", boolUseRegularExpression.ToString());
       //     myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);
       //     myActions.SetValueByKey("cbxFileTypeSelectedValue", strFileType);
       //     myActions.SetValueByKey("cbxExcludeSelectedValue", strExclude);
       //     myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
       //     string settingsDirectory = "";
       //     if (strButtonPressed == "btnSelectFolder") {
       //         FileFolderDialog dialog = new FileFolderDialog();
       //         dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");
       //         string str = "LastSearchFolder";


       //         System.Windows.Forms.DialogResult result = dialog.ShowDialog();

       //         if (result == System.Windows.Forms.DialogResult.OK && (Directory.Exists(dialog.SelectedPath) || File.Exists(dialog.SelectedPath))) {
       //             myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue = dialog.SelectedPath;
       //             myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey = dialog.SelectedPath;
       //             myListControlEntity.Find(x => x.ID == "cbxFolder").Text = dialog.SelectedPath;
       //             myActions.SetValueByKey("LastSearchFolder", dialog.SelectedPath);
       //             strFolder = dialog.SelectedPath;
       //             myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
       //             string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
       //             string fileName = "cbxFolder.txt";
       //             string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
       //             string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
       //             settingsDirectory =
       //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
       //             string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
       //             ArrayList alHosts = new ArrayList();
       //             cbp = new List<ComboBoxPair>();
       //             cbp.Clear();
       //             cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
       //             ComboBox myComboBox = new ComboBox();


       //             if (!File.Exists(settingsPath)) {
       //                 using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
       //                     objSWFile.Close();
       //                 }
       //             }
       //             using (StreamReader objSRFile = File.OpenText(settingsPath)) {
       //                 string strReadLine = "";
       //                 while ((strReadLine = objSRFile.ReadLine()) != null) {
       //                     string[] keyvalue = strReadLine.Split('^');
       //                     if (keyvalue[0] != "--Select Item ---") {
       //                         cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));
       //                     }
       //                 }
       //                 objSRFile.Close();
       //             }
       //             string strNewHostName = dialog.SelectedPath;
       //             List<ComboBoxPair> alHostx = cbp;
       //             List<ComboBoxPair> alHostsNew = new List<ComboBoxPair>();
       //             ComboBoxPair myCbp = new ComboBoxPair(strNewHostName, strNewHostName);
       //             bool boolNewItem = false;
       //             // add what they selected in select folder dialog
       //             alHostsNew.Add(myCbp);
       //             // if we have more than 24, remove the first one that is not select item
       //             if (alHostx.Count > 24) {
       //                 for (int i = alHostx.Count - 1; i > 0; i--) {
       //                     if (alHostx[i]._Key.Trim() != "--Select Item ---") {
       //                         alHostx.RemoveAt(i);
       //                         break;
       //                     }
       //                 }
       //             }
       //             // add all the items in the original dropdown that are not 
       //             // select item and are not the same as what was selected in
       //             // the select folder dialog
       //             foreach (ComboBoxPair item in alHostx) {
       //                 if (strNewHostName != item._Key && item._Key != "--Select Item ---") {
       //                     boolNewItem = true;
       //                     alHostsNew.Add(item);
       //                 }
       //             }
       //             // write updated dropdown list to Explorer/cbxFolder.txt
       //             try {
       //                 using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
       //                     foreach (ComboBoxPair item in alHostsNew) {
       //                         if (item._Key != "") {
       //                             objSWFile.WriteLine(item._Key + '^' + item._Value);
       //                         }
       //                     }
       //                     objSWFile.Close();
       //                 }
       //             } catch (Exception ex) {

       //                 myActions.MessageBoxShow(ex.Message);
       //             }
       //             goto DisplayWindowAgain;
       //         }
       //     }
       //     string strFindWhatToUse = "";
       //     string strFileTypeToUse = "";
       //     string strExcludeToUse = "";
       //     string strFolderToUse = "";
       //     if (strButtonPressed == "btnOkay") {
       //         if ((strFindWhat == "--Select Item ---" || strFindWhat == "")) {
       //             myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox; else press Cancel to Exit");
       //             goto DisplayFindTextInFilesWindow;
       //         }
       //         if ((strFileType == "--Select Item ---" || strFileType == "")) {
       //             myActions.MessageBoxShow("Please enter File Type or select File Type from ComboBox; else press Cancel to Exit");
       //             goto DisplayFindTextInFilesWindow;
       //         }
       //         if ((strExclude == "--Select Item ---" || strExclude == "")) {
       //             myActions.MessageBoxShow("Please enter Exclude or select Exclude from ComboBox; else press Cancel to Exit");
       //             goto DisplayFindTextInFilesWindow;
       //         }
       //         if ((strFolder == "--Select Item ---" || strFolder == "")) {
       //             myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
       //             goto DisplayFindTextInFilesWindow;
       //         }



       //         strFindWhatToUse = strFindWhat;

       //         if (boolUseRegularExpression) {
       //             strFindWhatToUse = strFindWhatToUse.Replace(")", "\\)").Replace("(", "\\(");
       //         }


       //         strFileTypeToUse = strFileType;



       //         strExcludeToUse = strExclude;


       //         strFolderToUse = strFolder;


       //     }
       //     strPathToSearch = strFolderToUse;

       //     strSearchPattern = strFileTypeToUse;

       //     strSearchExcludePattern = strExcludeToUse;

       //     strSearchText = strFindWhatToUse;

       //     strLowerCaseSearchText = strFindWhatToUse.ToLower();
       //     myActions.SetValueByKey("FindWhatToUse", strFindWhatToUse);
       //     try {
       //         var damageResult = await Task.Run(() => Search(settingsDirectory));

       //         myActions.MessageBoxShow(damageResult);

       //     } catch (Exception ex) {
       //         // MessageBox.Show(ex.Message);

       //     }
        }

        private async Task<string> Search(string settingsDirectory) {
            string myResult = "";
            Methods myActions = new Methods();


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
            foreach (var item in matchInfoList) {
                lines.Add("\"" + item.FullName + "\"(" + item.LineNumber + "," + item.LinePosition + "): " + item.LineText.Length.ToString() + " " + item.LineText.Substring(0, item.LineText.Length > 5000 ? 5000 : item.LineText.Length));


            }
            string strApplicationBinDebug1 = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath1 = strApplicationBinDebug1.Replace("\\bin\\Debug", "");





            settingsDirectory =
     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
                myResult = "RunTime: " + elapsedTime + "\n\r\n\rHits: " + intHits.ToString() + "\n\r\n\rFiles with hits: " + intUniqueFiles.ToString() + "\n\r\n\rPut Cursor on line and\n\r press Ctrl+Alt+N\n\rto view detail page. ";
                if (_searchErrors.Length > 0) {
                    myResult += "\n\r\n\rErrors: " + _searchErrors.ToString();
                }
            }


            return myResult;
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
                    _searchErrors.AppendLine(ex.InnerException.ToString() + " filename is:" + flname + " - Line 5733 in ExplorerView");
                } else {
                    _searchErrors.AppendLine(ex.Message + " filename is:" + flname + " - Line 5735 in ExplorerView");
                }
            }

            if (docText != null && docText.Contains(text)) {
                return true;
            } else {
                return false;
            }
        }

        private void textFileToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            FileView myFileView;
            string fileFullName = "";

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                }
            }

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                //MessageBox.Show(myFileView.FullName.ToString());                
                fileFullName = myFileView.FullName;
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                }
            }



            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New TextDocument";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New TextDocument Name";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblCustom";
            myControlEntity.Text = "Custom";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = "";
            myControlEntity.ID = "cbxCustom";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxCustom) {
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
            myControlEntity.ID = "lbldescription";
            myControlEntity.Text = "Description";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.Text = "";
            myControlEntity.ID = "txtDescription";
            myControlEntity.Multiline = true;
            myControlEntity.Height = 200;
            myControlEntity.TextWrap = true;
            myControlEntity.Width = 650;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxdescription) {
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
            myControlEntity.ID = "lblStatus";
            myControlEntity.Text = "Status";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = "";
            myControlEntity.ID = "cbxStatus";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxStatus) {
            //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
            //}
            //myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;

            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewTextDocumentDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
            string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strCustom = myListControlEntity.Find(x => x.ID == "cbxCustom").SelectedValue;

            string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;
            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;


            if (!myNewTextDocumentName.EndsWith(".txt")) {
                myNewTextDocumentName = myNewTextDocumentName + ".txt";
            }
            string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
            myActions.SetValueByPublicKeyInCurrentFolder("custom", strCustom, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("cbxCustomSelectedValue", strCustom, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("description", strdescription, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("status", strStatus, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("cbxStatusSelectedValue", strStatus, strNewTextDocumentDir);
            if (!File.Exists(strNewTextDocumentDir)) {
                string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".txt", "");
                //  myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);

                // File.Create(strNewTextDocumentDir);

            }
            _NotepadppLoaded = true;
            string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";

            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (detailsMenuItemChecked == "False") {
                myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                _CurrentSplitContainer.Panel2Collapsed = false;
                this.detailsMenuItem.Checked = true;
                this.listMenuItem.Checked = false;
                //tries to start the process 
                try {
                    if (!File.Exists(strExecutable)) {
                        myActions.MessageBoxShow(" File not found: " + strExecutable);
                    } else {
                        _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                    }
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                //disables button and textbox
                //txtProcess.Enabled = false;
                //btnStart.Enabled = false;

                //host the started process in the panel 
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                AddAppHandleToOpenFiles(strNewTextDocumentDir, _appHandle);
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            } else {
                //Close the running process
                if (_appHandle != IntPtr.Zero) {
                    PostMessage(_appHandle, WM_CLOSE, 0, 0);
                    System.Threading.Thread.Sleep(1000);
                    _appHandle = IntPtr.Zero;
                }
                //tries to start the process 
                try {
                    if (!File.Exists(strExecutable)) {
                        myActions.MessageBoxShow(" File not found: " + strExecutable);
                    } else {
                        _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                    }
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                //disables button and textbox
                //txtProcess.Enabled = false;
                //btnStart.Enabled = false;

                //host the started process in the panel 
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                AddAppHandleToOpenFiles(strNewTextDocumentDir, _appHandle);
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }
            _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);
            RefreshDataGrid();


        }

        private void wordPadFileToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                    fileFullName = myFileView.FullName;
                }
            }


            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New WordPad Document";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            int intRowCtr = 0;

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New WordPad Document Name";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBox";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblCustom";
            myControlEntity.Text = "Custom";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = "";
            myControlEntity.ID = "cbxCustom";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxCustom) {
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
            myControlEntity.ID = "lbldescription";
            myControlEntity.Text = "Description";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.Text = "";
            myControlEntity.ID = "txtDescription";
            myControlEntity.Multiline = true;
            myControlEntity.Height = 200;
            myControlEntity.TextWrap = true;
            myControlEntity.Width = 650;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxdescription) {
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
            myControlEntity.ID = "lblStatus";
            myControlEntity.Text = "Status";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = "";
            myControlEntity.ID = "cbxStatus";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            //foreach (var item in alcbxStatus) {
            //    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
            //}
            //myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.ComboBoxIsEditable = true;
            myControlEntity.ColumnNumber = 1;

            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewTextDocumentDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }
            string strCustom = myListControlEntity.Find(x => x.ID == "cbxCustom").SelectedValue;
            string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;

            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;




            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
            string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            if (!myNewTextDocumentName.EndsWith(".rtf")) {
                myNewTextDocumentName = myNewTextDocumentName + ".rtf";
            }
            string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
            myActions.SetValueByPublicKeyInCurrentFolder("cbxCustomSelectedValue", strCustom, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("custom", strCustom, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("description", strdescription, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("status", strStatus, strNewTextDocumentDir);
            myActions.SetValueByPublicKeyInCurrentFolder("cbxStatusSelectedValue", strStatus, strNewTextDocumentDir);

            if (!File.Exists(strNewTextDocumentDir)) {
                string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".rtf", "");
                //   myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                using (StreamWriter sw = new StreamWriter(strNewTextDocumentDir)) {



                }
                //   File.Create(strNewTextDocumentDir);

            }
            _WordPadLoaded = true;
            string strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (detailsMenuItemChecked == "False") {
                myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                _CurrentSplitContainer.Panel2Collapsed = false;
                this.detailsMenuItem.Checked = true;
                this.listMenuItem.Checked = false;
                //tries to start the process 
                try {
                    if (!File.Exists(strExecutable)) {
                        myActions.MessageBoxShow(" File not found: " + strExecutable);
                    } else {
                        _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                    }
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                //disables button and textbox
                //txtProcess.Enabled = false;
                //btnStart.Enabled = false;

                //host the started process in the panel 
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                AddAppHandleToOpenFiles(strNewTextDocumentDir, _appHandle);
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            } else {
                //Close the running process
                if (_appHandle != IntPtr.Zero) {
                    PostMessage(_appHandle, WM_CLOSE, 0, 0);
                    System.Threading.Thread.Sleep(1000);
                    _appHandle = IntPtr.Zero;
                }
                //tries to start the process 
                try {
                    if (!File.Exists(strExecutable)) {
                        myActions.MessageBoxShow(" File not found: " + strExecutable);
                    } else {
                        _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                    }
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                //disables button and textbox
                //txtProcess.Enabled = false;
                //btnStart.Enabled = false;

                //host the started process in the panel 
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, _CurrentSplitContainer.Panel2.Handle);
                AddAppHandleToOpenFiles(strNewTextDocumentDir, _appHandle);
                SetWindowLongA(_appHandle, WS_CAPTION, WS_VISIBLE);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }
            _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);
            RefreshDataGrid();

        }



        private void ExplorerView_KeyUp(object sender, KeyEventArgs e) {

        }

        private void ExplorerView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                _CurrentDataGridView.ClearSelection();
                _selectedRow = 0;
                Methods myActions = new Methods();
                myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString() + "SelectedRow", "0");
                TryToCloseAllOpenFilesInTab();
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
        }
        private void CreateShortcut(string name, string url) {

            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + name + ".url")) {

                writer.WriteLine("[InternetShortcut]");

                writer.WriteLine("URL=" + url);

                writer.Flush();

            }

        }

        private void urlShortcutFileToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                    fileFullName = myFileView.FullName;
                }
            }


            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New Url Shortcut";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            int intRowCtr = 0;

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New Url Shortcut Name";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtShortcutName";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblShortcutUrl";
            myControlEntity.Text = "Shortcut Url";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtShortcutUrl";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewTextDocumentDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string strShortcutName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
            string strShortcutUrl = myListControlEntity.Find(x => x.ID == "txtShortcutUrl").Text;


            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
            string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
            if (!myNewTextDocumentName.EndsWith(".url")) {
                myNewTextDocumentName = myNewTextDocumentName + ".url";
            }
            string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);

            myActions.SetValueByKeyForNonCurrentScript("shortcutName", strShortcutName, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            myActions.SetValueByKeyForNonCurrentScript("shortcutUrl", strShortcutUrl, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            if (!File.Exists(strNewTextDocumentDir)) {
                string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".rtf", "");
                //   myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                using (StreamWriter writer = new StreamWriter(strNewTextDocumentDir)) {

                    writer.WriteLine("[InternetShortcut]");

                    writer.WriteLine("URL=" + strShortcutUrl);

                    writer.Flush();

                }
            }
            _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);
            RefreshDataGrid();
        }

        private void InitializeComponentWebBrowser() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton5,
            this.toolStripButton2,
            this.toolStripComboBox1,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(971, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(700, 25);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 391);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(971, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // webBrowser1
            // 
            //  this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;          
            this.webBrowser1.Location = new System.Drawing.Point(0, 25);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(971, 366);
            this.webBrowser1.TabIndex = 2;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::System.Windows.Forms.Samples.Properties.Resources.prev;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::System.Windows.Forms.Samples.Properties.Resources.home;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::System.Windows.Forms.Samples.Properties.Resources.next;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::System.Windows.Forms.Samples.Properties.Resources.GoSign;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::System.Windows.Forms.Samples.Properties.Resources.refresh;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::System.Windows.Forms.Samples.Properties.Resources.printButton;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);

            // 
            // ExplorerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 413);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExplorerView";
            this.Text = "ExplorerView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ExplorerView_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private void Popup() {
            if (_Panel2KeyPress) {
                Thread th = new Thread(() => {
                    try {
                        Open();
                    } catch (Exception) {

                    }
                });
                th.Start();
                Thread.Sleep(500);   //you can update this time as your requirement
                th.Abort();
            }
        }

        private void Open() {
            Saved frm = new Saved();
            frm.ShowDialog();   // frm.Show(); if MDI Parent form            
        }
        private void _CurrentSplitContainer_MouseLeave(object sender, EventArgs e) {
            if (_Panel2KeyPress && (_WordPadLoaded || _NotepadppLoaded)) {
                Methods myActions = new Methods();
                myActions.TypeText("^(s)", 200);
                Popup();
                _Panel2KeyPress = false;
            }
        }

        private void toolBar_MouseEnter(object sender, EventArgs e) {
            if (_Panel2KeyPress && (_WordPadLoaded || _NotepadppLoaded)) {
                Methods myActions = new Methods();
                myActions.TypeText("^(s)", 200);
                Popup();
                _Panel2KeyPress = false;
            }
        }

        private void toolBar_MouseLeave(object sender, EventArgs e) {
            if (_Panel2KeyPress && (_WordPadLoaded || _NotepadppLoaded)) {
                Methods myActions = new Methods();
                myActions.TypeText("^(s)", 200);
                Popup();
                _Panel2KeyPress = false;
            }
        }

        private void _CurrentSplitContainer_MouseEnter(object sender, EventArgs e) {
            if (_Panel2KeyPress && (_WordPadLoaded || _NotepadppLoaded)) {
                Methods myActions = new Methods();
                myActions.TypeText("^(s)", 200);
                Popup();
                _Panel2KeyPress = false;
            }
        }

        private void showHideColumnsToolStripMenuItem_Click(object sender, EventArgs e) {
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(_CurrentDataGridView);
            cs.MaxHeight = 100;
            cs.Width = 110;
            cs.ShowHideColumns();
        }




        private void fileShortcutFileToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                    fileFullName = myFileView.FullName;
                }
            }


            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Create New File Shortcut";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            int intRowCtr = 0;

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabel";
            myControlEntity.Text = "Enter New File Shortcut Name";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtShortcutName";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblShortcutFile";
            myControlEntity.Text = "Shortcut File";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 150;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtShortcutFile";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            ReDisplayNewTextDocumentDialog:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string strShortcutName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
            string strShortcutFile = myListControlEntity.Find(x => x.ID == "txtShortcutFile").Text;


            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
            string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
            if (!myNewTextDocumentName.EndsWith(".lnk")) {
                myNewTextDocumentName = myNewTextDocumentName + ".lnk";
            }
            string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);

            myActions.SetValueByKeyForNonCurrentScript("shortcutName", strShortcutName, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            myActions.SetValueByKeyForNonCurrentScript("shortcutFile", strShortcutFile, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));

            if (!File.Exists(strNewTextDocumentDir)) {
                Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
                dynamic shell = Activator.CreateInstance(t);
                try {
                    var lnk = shell.CreateShortcut(strNewTextDocumentDir);
                    try {
                        lnk.TargetPath = strShortcutFile;
                        lnk.IconLocation = "shell32.dll, 1";
                        lnk.Save();
                    } finally {
                        Marshal.FinalReleaseComObject(lnk);
                    }
                } finally {
                    Marshal.FinalReleaseComObject(shell);

                }
            }
            _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);
            RefreshDataGrid();
        }
        private void fileShortcutToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                string fileName = (_CurrentDataGridView).Rows[myCell.RowIndex].Cells["FullName"].Value.ToString();
                int myIndex = GetIndexForCurrentFileViewBindingSourceForFullName(fileName);
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                    fileFullName = myFileView.FullName;
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Create New File Shortcut";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    int intRowCtr = 0;

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Enter New File Shortcut Name";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtShortcutName";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblShortcutFile";
                    myControlEntity.Text = "Shortcut File";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.Width = 150;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.ColumnSpan = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());



                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtShortcutFile";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = intRowCtr;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    ReDisplayNewTextDocumentDialog:

                    string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                    if (strButtonPressed == "btnCancel") {
                        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                        return;
                    }

                    string strShortcutName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
                    string strShortcutFile = myListControlEntity.Find(x => x.ID == "txtShortcutFile").Text;


                    string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "txtShortcutName").Text;
                    if (!myNewTextDocumentName.EndsWith(".lnk")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".lnk";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);

                    myActions.SetValueByKeyForNonCurrentScript("shortcutName", strShortcutName, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
                    myActions.SetValueByKeyForNonCurrentScript("shortcutFile", strShortcutFile, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
                    if (!File.Exists(strNewTextDocumentDir)) {
                        Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
                        dynamic shell = Activator.CreateInstance(t);
                        try {
                            var lnk = shell.CreateShortcut(strNewTextDocumentDir);
                            try {
                                lnk.TargetPath = strShortcutFile;
                                lnk.IconLocation = "shell32.dll, 1";
                                lnk.Save();
                            } finally {
                                Marshal.FinalReleaseComObject(lnk);
                            }
                        } finally {
                            Marshal.FinalReleaseComObject(shell);
                        }
                    }
                    _CurrentSplitContainer.SplitterDistance = (int)(ClientSize.Width * .2);

                } else {
                    myActions.MessageBoxShow("You can not create a shortcut inside a file; first select folder and right click");
                }

            }
        }


        private void findColumnsToolStripMenuItem_Click(object sender, EventArgs e) {
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();            
            //Switcher.Switch(new FindColumns());
            //mainWindow.Focus();
            FindColumns dlg = new FindColumns();
            ElementHost.EnableModelessKeyboardInterop(dlg);
            dlg.Show();

            //   FindColumns dlg = new FindColumns();
            //   dlg.Owner = (Window)Window.GetWindow(this);
            // Shadow.Visibility = Visibility.Visible;

            //Shadow.Visibility = Visibility.Collapsed;
        }

        private void sqlToGridToolStripMenuItem_Click(object sender, EventArgs e) {
            SQLToGrid dlg = new SQLToGrid();
            ElementHost.EnableModelessKeyboardInterop(dlg);
            dlg.Show();
        }

        private void idealSqlTracerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            //string strApplicationPath = strApplicationBinDebug.Replace("\\IdealAutomateExplorer\\bin\\Debug", "");
            string strIdealSqlTracerExe = Path.Combine(strApplicationBinDebug, @"IdealSqlTracer.exe");
            myActions.Run(strIdealSqlTracerExe, "");
        }

        private void sqlLiteToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string sqlLiteSaved = myActions.GetValueByKey("sqlLiteSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", sqlLiteSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "sqlLiteSaved");
            DialogForGettingExe();
        }

        private void sqlProfilerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string sqlProfilerSaved = myActions.GetValueByKey("sqlProfilerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", sqlProfilerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "sqlProfilerSaved");
            DialogForGettingExe();
        }

        private void sSMSToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string sSMSSaved = myActions.GetValueByKey("sSMSSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", sSMSSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "sSMSSaved");
            DialogForGettingExe();
        }

        private void instantCToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string instantCSaved = myActions.GetValueByKey("instantCSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", instantCSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "instantCSaved");
            DialogForGettingExe();
        }

        private void instantVBToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string instantVBSaved = myActions.GetValueByKey("instantVBSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", instantVBSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "instantVBSaved");
            DialogForGettingExe();
        }

        private void paintNETToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string paintNETSaved = myActions.GetValueByKey("paintNETSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", paintNETSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "paintNETSaved");
            DialogForGettingExe();
        }

        private void visualStudio2015ToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string visualStudio2015Saved = myActions.GetValueByKey("visualStudio2015Saved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", visualStudio2015Saved);
            myActions.SetValueByKey("whatToolDefaultToSave", "visualStudio2015Saved");
            DialogForGettingExe();
        }

        private void visualStudio2017ToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string visualStudio2017Saved = myActions.GetValueByKey("visualStudio2017Saved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", visualStudio2017Saved);
            myActions.SetValueByKey("whatToolDefaultToSave", "visualStudio2017Saved");
            DialogForGettingExe();
        }

        private void componentServicesToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string componentServicesSaved = myActions.GetValueByKey("componentServicesSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", componentServicesSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "componentServicesSaved");
            DialogForGettingExe();
        }

        private void eventViewerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string eventViewerSaved = myActions.GetValueByKey("eventViewerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", eventViewerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "eventViewerSaved");
            DialogForGettingExe();
        }

        private void iISToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string iISSaved = myActions.GetValueByKey("iISSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", iISSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "iISSaved");
            DialogForGettingExe();
        }

        private void programsAndFeaturesToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string programsAndFeaturesSaved = myActions.GetValueByKey("programsAndFeaturesSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", programsAndFeaturesSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "programsAndFeaturesSaved");
            DialogForGettingExe();
        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string servicesSaved = myActions.GetValueByKey("servicesSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", servicesSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "servicesSaved");
            DialogForGettingExe();
        }

        private void taskManagerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string taskManagerSaved = myActions.GetValueByKey("taskManagerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", taskManagerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "taskManagerSaved");
            DialogForGettingExe();
        }

        private void fiddlerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string fiddlerSaved = myActions.GetValueByKey("fiddlerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", fiddlerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "fiddlerSaved");
            DialogForGettingExe();
        }

        private void iISToolStripMenuItem1_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string iISSaved = myActions.GetValueByKey("iISSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", iISSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "iISSaved");
            DialogForGettingExe();
        }

        private void iWB2LearnerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string iWB2Saved = myActions.GetValueByKey("iWB2Saved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", iWB2Saved);
            myActions.SetValueByKey("whatToolDefaultToSave", "iWB2Saved");
            DialogForGettingExe();
        }

        private void notepadToolStripMenuItem2_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string notepadSaved = myActions.GetValueByKey("notepadSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", notepadSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "notepadSaved");
            DialogForGettingExe();
        }

        private void processExplorerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string processExplorerSaved = myActions.GetValueByKey("processExplorerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", processExplorerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "processExplorerSaved");
            DialogForGettingExe();
        }

        private void taskManagerToolStripMenuItem1_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string taskManagerSaved = myActions.GetValueByKey("taskManagerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", taskManagerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "taskManagerSaved");
            DialogForGettingExe();
        }

        private void winListerToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string winlisterSaved = myActions.GetValueByKey("winlisterSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", winlisterSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "winlisterSaved");
            DialogForGettingExe();
        }

        private void curlToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string curlSaved = myActions.GetValueByKey("curlSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", curlSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "curlSaved");
            DialogForGettingExe();
        }

        private void fiddlerToolStripMenuItem1_Click(object sender, EventArgs e) {

            Methods myActions = new Methods();
            string fiddlerSaved = myActions.GetValueByKey("fiddlerSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", fiddlerSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "fiddlerSaved");
            DialogForGettingExe();


        }

        private void postmanToolStripMenuItem_Click(object sender, EventArgs e) {

            Methods myActions = new Methods();
            string postmanSaved = myActions.GetValueByKey("postmanSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", postmanSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "postmanSaved");
            DialogForGettingExe();


        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e) {
            NavWindowAutomater dlg = new NavWindowAutomater();
            ElementHost.EnableModelessKeyboardInterop(dlg);
            dlg.Topmost = true;
            dlg.Show();

        }

        private void videosToolStripMenuItem_Click(object sender, EventArgs e) {
            VideoTutorials dlg = new VideoTutorials();
            ElementHost.EnableModelessKeyboardInterop(dlg);
            dlg.Topmost = true;
            dlg.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutDialog dlg = new AboutDialog();
            ElementHost.EnableModelessKeyboardInterop(dlg);
            dlg.Topmost = true;
            dlg.Show();
        }

        private void DialogForGettingExe() {
            Methods myActions = new Methods();
            string myExe = "";
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
            myControlEntity.Text = "Specify Executable";
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


            string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string fileName = "cbxFolder.txt";
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            string settingsDirectory =
       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
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
            string strNewHostName = myActions.GetValueByKey("cbxToolExeSelectedValue");
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

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxToolExeSelectedValue");
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
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnGetExeByClick";
            myControlEntity.Text = "Populate Exe by Clicking on running App";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            DisplayWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            string strFolder = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedValue;
            if (strButtonPressed == "btnCancel") {
                string whatToolDefaultToSave = myActions.GetValueByKey("whatToolDefaultToSave");
                myActions.SetValueByKey(whatToolDefaultToSave, strFolder);
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }



            //     string strFolderKey = myListControlEntity.Find(x => x.ID == "cbxFolder").SelectedKey;

            myActions.SetValueByKey("cbxToolExeSelectedValue", strFolder);
            settingsDirectory = "";
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
                    myActions.SetValueByKey("cbxToolExeSelectedValue", strFolder);
                    strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                    fileName = "cbxFolder.txt";
                    strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
                    myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
                    settingsDirectory =
     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
                    settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
                    alHosts = new ArrayList();
                    cbp = new List<ComboBoxPair>();
                    cbp.Clear();
                    cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
                    myComboBox = new ComboBox();


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
                    strNewHostName = dialog.SelectedPath;
                    alHostx = cbp;
                    alHostsNew = new List<ComboBoxPair>();
                    myCbp = new ComboBoxPair(strNewHostName, strNewHostName);
                    boolNewItem = false;

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
            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {
                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }
                strFolderToUse = strFolder;
                // MessageBox.Show(strFolder);
                myActions.Run(strFolder, "");
                string whatToolDefaultToSave = myActions.GetValueByKey("whatToolDefaultToSave");
                myActions.SetValueByKey(whatToolDefaultToSave, strFolder);
            }
            if (strButtonPressed == "btnGetExeByClick") {
                GetExecutableByClicking();
                return;
                //myActions.Sleep(1000);
                //strFolderToUse = myActions.GetValueByKey("ClickedExecutable");
                //myActions.MessageBoxShow("Executable " + strFolderToUse + " is in clipboard");
                //goto DisplayFindTextInFilesWindow;
            }
            //myExe = strFolderToUse;
            return;
        }

        private void DialogForAddingUrl() {
            Methods myActions = new Methods();
            string myExe = "";
            DisplayFindTextInFilesWindow:
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "Specify Url";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblUrl";
            myControlEntity.Text = "Url";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.Text = myActions.GetValueByKey("cbxToolExeSelectedValue");
            myControlEntity.ID = "txtDefaultUrl";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = @"Here is an example: http://idealprogrammer.com";
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 2;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            intRowCtr++;


            DisplayWindowAgain:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 1200, 100, 100);
            LineAfterDisplayWindow:
            string strDefaultUrl = myListControlEntity.Find(x => x.ID == "txtDefaultUrl").Text;
            if (strButtonPressed == "btnCancel") {
                string whatToolDefaultToSave = myActions.GetValueByKey("whatToolDefaultToSave");
                myActions.SetValueByKey(whatToolDefaultToSave, strDefaultUrl);
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {
                if ((strDefaultUrl == "")) {
                    myActions.MessageBoxShow("Please enter url; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }

                Process.Start("IExplore.exe", strDefaultUrl);
                string whatToolDefaultToSave = myActions.GetValueByKey("whatToolDefaultToSave");
                myActions.SetValueByKey(whatToolDefaultToSave, strDefaultUrl);
            }

            //myExe = strFolderToUse;
            return;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            // code goes here

            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) {
                return;
            }

            //----- Find the Length of the Window's Title ----- 

            int TitleLength = 0;

            TitleLength = GetWindowTextLengthx(hWnd.ToInt32());

            //----- Find the Window's Title ----- 

            string WindowTitle = new String('*', TitleLength + 1);

            GetWindowText(hWnd, WindowTitle, TitleLength + 1);

            //----- Find the PID of the Application that Owns the Window ----- 

            int pid = 0;

            GetWindowThreadProcessId(hWnd, ref pid);

            if (pid == 0) {
                return;
            }
            Methods myActions = new Methods();
            if (myhWnd != hWnd) {
                string myFileName = Keyboard.GetMainModuleFilepath(pid);
                if (myFileName == null || myFileName.EndsWith("IdealAutomateExplorer.exe") ||
                    myFileName.EndsWith("ApplicationHostFrame.exe") ||
                    myFileName.EndsWith("Wordpad.exe")) {
                    return;
                }
                myActions.SetValueByKey("cbxToolExeSelectedValue", myFileName);
                dispatcherTimer.Stop();
                DialogForGettingExe();

            }

        }
        public void GetExecutableByClicking() {
            Methods myActions = new Methods();
            myActions.SetValueByKey("ClickedExecutable", "");

            // SetWindowPos(myhWnd.ToInt32(), HWND_TOPMOST, this.Left, this.Top, this.Width, this.Height, SWP_NOACTIVATE);
            IntPtr hWnd = GetForegroundWindow();

            myhWnd = hWnd;
            MakeTopMost(hWnd.ToInt32());
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            return;
        }
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetWindowPos", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int SetWindowPos(int hwnd, int hWndInsertAfter, double x, double y, double cx, double cy, uint wFlags);
        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;
        //private const int SWP_NOMOVE = 0X2;
        //private const int SWP_NOSIZE = 0X1;
        private const int TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        //INSTANT C# TODO TASK: Insert the following converted event handler wireups at the end of the 'InitializeComponent' method for forms, 'Page_Init' for web pages, or into a constructor for other classes:


        public void MakeNormal(int hwnd) {


            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }
        public void MakeTopMost(int hwnd) {
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void cbxCurrentPath_MouseDown(object sender, MouseEventArgs e) {
            _newTab = false;
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e) {
            Methods myActions = new Methods();
            if (toolStripComboBox2.SelectedIndex == 0) {
                myActions.SetValueByKey("LaunchMode", "Admin");
            } else {
                myActions.SetValueByKey("LaunchMode", "NonAdmin");
            }
        }

        private void courseraToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "https://click.linksynergy.com/fs-bin/click?id=ur0PwtPl4wY&offerid=467035.30&type=3&subid=0");
        }

        private void pluralsightToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "http://pluralsight.pxf.io/c/1194222/431393/7490");
        }

        private void udemyToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "https://click.linksynergy.com/fs-bin/click?id=ur0PwtPl4wY&offerid=323058.1626&subid=0&type=4");
        }

        private void videoTrafficBlueprintToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "http://www.contentsamurai.com/c/harvey007-the-ultimate-video-traffic-blueprint");
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "http://www.marketsamurai.com/c/harvey007");
        }

        private void googleDriveToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "https://drive.google.com/drive/my-drive");
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "https://www.dropbox.com/h");
        }

        private void oneDriveToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "https://onedrive.live.com/about/en-IE/");
        }

        private void compareItToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string compareItSaved = myActions.GetValueByKey("compareItSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", compareItSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "compareItSaved");
            DialogForGettingExe();
        }

        private void synchronizeItToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string synchronizeItSaved = myActions.GetValueByKey("synchronizeItSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", synchronizeItSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "synchronizeItSaved");
            DialogForGettingExe();
        }

        private void filezillaToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string filezillaSaved = myActions.GetValueByKey("filezillaSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", filezillaSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "filezillaSaved");
            DialogForGettingExe();
        }

        private void flashbackExpressToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string flashbackExpressSaved = myActions.GetValueByKey("flashbackExpressSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", flashbackExpressSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "flashbackExpressSaved");
            DialogForGettingExe();
        }

        private void facebookToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string facebookSaved = myActions.GetValueByKey("facebookSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", facebookSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "facebookSaved");
            DialogForAddingUrl();
        }

        private void instagramToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string instagramSaved = myActions.GetValueByKey("instagramSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", instagramSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "instagramSaved");
            DialogForAddingUrl();
        }

        private void linkedInPersonalToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string linkedInPersonalSaved = myActions.GetValueByKey("linkedInPersonalSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", linkedInPersonalSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "linkedInPersonalSaved");
            DialogForAddingUrl();
        }

        private void linkedInCompanyToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string linkedInCompanySaved = myActions.GetValueByKey("linkedInCompanySaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", linkedInCompanySaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "linkedInCompanySaved");
            DialogForAddingUrl();
        }

        private void stumbleUponToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string stumbleUponSaved = myActions.GetValueByKey("stumbleUponSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", stumbleUponSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "stumbleUponSaved");
            DialogForAddingUrl();
        }

        private void tumblrToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string tumblrSaved = myActions.GetValueByKey("tumblrSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", tumblrSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "tumblrSaved");
            DialogForAddingUrl();
        }

        private void twitterToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string twitterSaved = myActions.GetValueByKey("twitterSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", twitterSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "twitterSaved");
            DialogForAddingUrl();
        }

        private void centralAccessReaderToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string centralAccessReaderSaved = myActions.GetValueByKey("centralAccessReaderSaved");
            myActions.SetValueByKey("cbxToolExeSelectedValue", centralAccessReaderSaved);
            myActions.SetValueByKey("whatToolDefaultToSave", "centralAccessReaderSaved");
            DialogForGettingExe();
        }

        private void speakItToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("IExplore.exe", "https://chrome.google.com/webstore/detail/speak-it/fginjphhpgkicbhibgafbpfjeahmjdfc?hl=en");
        }

        private async void snippingToolAutomationToolStripMenuItem_ClickAsync(object sender, EventArgs e) {
           
           

        }

        private async Task<string> SnippingTask() {
            string myResult = "";
            IdealAutomate.Core.Methods myActions = new Methods();

            int intTop = (int)SystemParameters.WorkArea.Height - 200;
            int intLeft = (int)SystemParameters.WorkArea.Width - 200;
            myActions.SetValueByKeyGlobal("WindowTop", "-1");
            myActions.SetValueByKeyGlobal("WindowLeft", "-1");
            List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("wordpad");
            myWindowTitles.RemoveAll(item => item == "");
            if (myWindowTitles.Count == 0) {
               
                myActions.MessageBoxShow("You need to have an instance of wordpad running in order to save your snippets into it - aborting");
                return myResult;
            }
            snipDialog:

            myActions.Sleep(1000);
            myActions.Run(@"C:\WINDOWS\system32\SnippingTool.exe", "");
            myActions.Sleep(1000);
            myActions.TypeText("%(n)", 1000);
            myActions.TypeText("{ESC}", 1000);
            myActions.TypeText("%(\" \")n", 1000);


            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSnipWithComments";
            myControlEntity.Text = "Snip w/ Comments";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSnip";
            myControlEntity.Text = "Snip";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnExit";
            myControlEntity.Text = "Exit";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            int intSavedTop = myActions.GetValueByKeyAsIntGlobal("WindowTop");
            int intSavedLeft = myActions.GetValueByKeyAsIntGlobal("WindowLeft");
            if (intSavedTop > 0) {
                intTop = intSavedTop;
            }
            if (intSavedLeft > 0) {
                intLeft = intSavedLeft;
            }
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 200, 200, intTop, intLeft);
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
               
                return myResult;
            }
            if (strButtonPressed == "btnExit") {
                // myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return myResult;
            }

            bool snipWithComments = false;
            if (strButtonPressed == "btnSnipWithComments") {
                snipWithComments = true;
            }
            //SnippingTool.Snip();

            //if (SnippingTool.Image != null) {
            //    Clipboard.SetImage(BitmapSourceFromImage(SnippingTool.Image));
            //}
            myActions.TypeText("^({PRTSC})", 1000);


            myActions.SetValueByKey("Mouseup", "false");
            //mh = new MouseHook();
            //mh.Install();
            intRowCtr = 0;
            myControlEntity = new ControlEntity();
            myListControlEntity = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDoneSnipping";
            myControlEntity.Text = "Done Snipping";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnAddComments";
            myControlEntity.Text = "Add Comments";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnExit";
            myControlEntity.Text = "Exit";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            intSavedTop = myActions.GetValueByKeyAsIntGlobal("WindowTop");
            intSavedLeft = myActions.GetValueByKeyAsIntGlobal("WindowLeft");
            if (intSavedTop > 0) {
                intTop = intSavedTop;
            }
            if (intSavedLeft > 0) {
                intLeft = intSavedLeft;
            }
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 200, 200, intTop, intLeft);
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return myResult;
            }
            if (strButtonPressed == "btnExit") {
                // myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return myResult;
            }

            if (strButtonPressed == "btnAddComments") {
                snipWithComments = true;
            }

            myActions.ActivateWindowByTitle("Snipping Tool");
            myActions.TypeText("^(c)", 500);
            //mh.MouseMove += new MouseHook.MouseHookCallback(mouseHook_MouseMove);
            string mouseUp = "false";
            //mouseWatcher = eventHookFactory.GetMouseWatcher();
            //mouseWatcher.Start();
            //mouseWatcher.OnMouseInput += (s, e) => {
            //    if (e.Message.ToString() == "WM_LBUTTONUP") {
            //        myActions.SetValueByKey("MouseUp", "true");
            //    }
            //};
            //while (mouseUp == "false") {
            //    myActions.TypeText("^(c)", 500);
            //    mouseUp = myActions.GetValueByKey("MouseUp");

            // //   myActions.Sleep(5000);
            //}
            string strComments = "";
            if (snipWithComments) {
                intRowCtr = 0;
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lbl";
                myControlEntity.Text = "Comments";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblComments";
                myControlEntity.Text = "Comments";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtComments";
                myControlEntity.Text = ""; //myActions.GetValueByKey("Comments"); ;
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.Width = 800;
                myControlEntity.Height = 300;
                myControlEntity.TextWrap = true;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);
                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    return myResult;
                }

                strComments = myListControlEntity.Find(x => x.ID == "txtComments").Text;
                myActions.SetValueByKey("Comments", strComments);
            }
            myActions.TypeText("^(c)", 500);
            myActions.TypeText("%(\" \")n", 1000);
            myWindowTitles = myActions.GetWindowTitlesByProcessName("wordpad");
            myWindowTitles.RemoveAll(item => item == "");
            if (myWindowTitles.Count > 0) {
                myActions.ActivateWindowByTitle(myWindowTitles[0]);
                if (strComments != "") {
                    myActions.TypeText(strComments, 1000);
                }
                myActions.TypeText("^(v)", 1000);
                myActions.TypeText("{ENTER}", 1000);
                myActions.TypeText("%(\" \")n", 500);
                myActions.TypeText(" ", 1000);
            } else {
                myActions.MessageBoxShow("You need to have an instance of wordpad running in order to save your snippets into it - aborting");
                return myResult;
            }

            goto snipDialog;
        }

        private async void snippingAutomationToolToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Thread thread = new Thread(() => {
                    SnippingTask();


                    System.Windows.Threading.Dispatcher.Run();
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();



            } catch (Exception ex) {
                // MessageBox.Show(ex.Message);

            }
            Methods myActions = new Methods();
        }

        private void toolStripMenuItemMousePositionShow_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            //string strApplicationPath = strApplicationBinDebug.Replace("\\IdealAutomateExplorer\\bin\\Debug", "");
            string strIdealSqlTracerExe = Path.Combine(strApplicationBinDebug, @"GetCursorPosDemo.exe");
            myActions.Run(strIdealSqlTracerExe, "");
        }
    }

}
internal static class Keyboard {
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static IntPtr GetModuleHandle(string lpModuleName);
    [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "UnhookWindowsHookEx", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
    public static extern int UnhookWindowsHookEx(int hHook);
    //[System.Runtime.InteropServices.DllImport("user32", EntryPoint="SetWindowsHookExA", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
    //public static extern int SetWindowsHookEx(int idHook, KeyboardHookDelegate lpfn, IntPtr hmod, int dwThreadId);
    [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "GetAsyncKeyState", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
    private static extern int GetAsyncKeyState(int vKey);
    //[System.Runtime.InteropServices.DllImport("user32", EntryPoint="CallNextHookEx", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
    //private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
    public struct KBDLLHOOKSTRUCT {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }
    // Low-Level Keyboard Constants
    private const int HC_ACTION = 0;
    private const int LLKHF_EXTENDED = 0X1;
    private const int LLKHF_INJECTED = 0X10;
    private const int LLKHF_ALTDOWN = 0X20;
    private const int LLKHF_UP = 0X80;
    private const int LLKHF_DOWN = 0X81;
    // Virtual Keys
    public const int VK_TAB = 0X9;
    public const int VK_CONTROL = 0X11;
    public const int VK_ESCAPE = 0X1B;
    public const int VK_DELETE = 0X2E;
    private const int WH_KEYBOARD_LL = 13;
    // public static int KeyboardHandle;





    public static string GetMainModuleFilepath(int processId) {
        string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
        using (var searcher = new ManagementObjectSearcher(wmiQueryString)) {
            using (var results = searcher.Get()) {
                ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                if (mo != null) {
                    return (string)mo["ExecutablePath"];
                }
            }
        }
        return null;
    }

}



