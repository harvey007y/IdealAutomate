#region Using directives

using System;
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




#endregion

namespace System.Windows.Forms.Samples {
    partial class ExplorerView : Form {
        private DirectoryView _dir;
        string strInitialDirectory = "";
        int _CurrentIndex = 0;
        DataGridView _CurrentDataGridView = new DataGridView();
        DataGridView dataGridView3 = new DataGridView();
        BindingSource _CurrentFileViewBindingSource = new BindingSource();
        bool boolStopEvent = false;
        bool _ignoreSelectedIndexChanged = false;
        Rectangle _IconRectangle = new Rectangle();
        List<HotKeyRecord> listHotKeyRecords = new List<HotKeyRecord>();
        Dictionary<string, VirtualKeyCode> dictVirtualKeyCodes = new Dictionary<string, VirtualKeyCode>();
        List<BindingSource> listBindingSource = new List<BindingSource>();
        private Point _imageLocation = new Point(13, 5);
        private Point _imgHitArea = new Point(13, 2);
        const int LEADING_SPACE = 12;
        const int CLOSE_SPACE = 15;
        const int CLOSE_AREA = 15;

        private const int SC_MAXIMIZE = 61488;
        private const int WM_CLOSE = 16; //0x10
        private const int WM_SYSCOMMAND = 274;


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

        private IntPtr _appHandle;
        Process _proc;
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


        public ExplorerView() {
            InitializeComponent();
            for (int i = 0; i < 20; i++) {
                BindingSource myNewBindingSource = new BindingSource();
                listBindingSource.Add(myNewBindingSource);
            }
            _CurrentDataGridView = new DataGridView();
            _CurrentFileViewBindingSource = FileViewBindingSource;
            tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tabControl1.HotTrack = true;

            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.Padding = new System.Drawing.Point(20, 3);
            tabControl1.MouseClick += TabControl1_MouseClick;

        }



        private void TabControl1_MouseClick(object sender, MouseEventArgs e) {
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

                }
                myActions.SetValueByKey("NumOfTabs", this.tabControl1.TabPages.Count.ToString());
            }
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
            Methods myActions = new Methods();           
            _CurrentDataGridView.ClearSelection();
      
            splitContainer1.Height = Screen.PrimaryScreen.Bounds.Height - 175;
           
            int splitContainer1Width = myActions.GetValueByKeyAsInt("SplitContainer1Width");

            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (splitContainer1Width > 0) {
                splitContainer1.SplitterDistance = splitContainer1Width;
            } else {
                splitContainer1.Width = Screen.PrimaryScreen.Bounds.Width;
            }

            if (detailsMenuItemChecked == "True") {
                splitContainer1.Panel2Collapsed = false;
                this.detailsMenuItem.Checked = true;
                this.listMenuItem.Checked = false;
                //tries to start the process 
                try {
                    _proc = Process.Start(@"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            } else {
                splitContainer1.Panel2Collapsed = true;
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
                _dir = new DirectoryView(strInitialDirectory, false);
                this._CurrentFileViewBindingSource.DataSource = _dir;

                tabControl1.TabPages[i].Text = _dir.FileView.Name;
                tabControl1.TabPages[i].ToolTipText = _dir.FileView.FullName;
                _CurrentIndex = i;
                AddDataGridToTab();


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
            _dir = new DirectoryView(strInitialDirectory);
            this._CurrentFileViewBindingSource.DataSource = _dir;
            tabControl1.TabPages[tabControl1.SelectedIndex].Text = _dir.FileView.Name;
            _CurrentIndex = tabControl1.SelectedIndex;
            // AddDataGridToTab();

            testskip:
            int currentIndex = myActions.GetValueByKeyAsInt("CurrentIndex");
            tabControl1.SelectedIndex = currentIndex;
            _CurrentDataGridView = (DataGridView)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            _CurrentFileViewBindingSource = listBindingSource[tabControl1.SelectedIndex];

            RefreshDataGrid();
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);

            string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
            string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
            string settingsDirectory = GetAppDirectoryForScript(myActions.ConvertFullFileNameToScriptPath(myNewProjectSourcePath));
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
            Icon icon = (e.Value as Icon);
            if (e.Value == null) {
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

                    MessageBox.Show(ex.Message);
                } finally {
                    e.Handled = true;
                }
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            if (e.RowIndex < 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Value == null) {
                return;
            }
            string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();
            string scriptName = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Value.ToString();
            Methods myActions = new Methods();
            string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToPublicPath(fileName) + "\\" + scriptName.Replace(".txt", "").Replace(".rtf", ""));
            string strNestingLevel = "";
            if (((DataGridView)sender).Rows.Count == 0) {
                return;
            }
            strNestingLevel = ((DataGridView)sender).Rows[e.RowIndex].Cells[14].Value.ToString();
            int nestingLevel = 0;
            Int32.TryParse(strNestingLevel, out nestingLevel);
            int indent = nestingLevel * 20;
            if (categoryState == "Collapsed" || categoryState == "Expanded") {
                ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(indent, 0, 0, 0);
            } else {
                ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(indent, 0, 0, 0);
                DataGridViewCell iconCell = ((DataGridView)sender).Rows[e.RowIndex].Cells[0];
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            // Call Active on DirectoryView
            string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();
            Methods myActions = new Methods();
            // fileName = fileName;
            string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", fileName);
            if (categoryState == "Expanded") {
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", fileName);
                RefreshDataGrid();
                return;
            }
            if (categoryState == "Collapsed") {
                myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Expanded", fileName);
                RefreshDataGrid();
                return;
            }
            try {
                _dir.Activate(this._CurrentFileViewBindingSource[e.RowIndex] as FileView);
                SetTitle(_dir.FileView);
                RefreshDataGrid();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
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
                splitContainer1.Panel2Collapsed = true;
                myActions.SetValueByKey("DetailsMenuItemChecked", "False");
            }
        }

        private void detailsMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            if (DoActionRequired(sender)) {
                splitContainer1.Panel2Collapsed = false;
                myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                //tries to start the process 
                try {
                    _proc = Process.Start(@"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }
        }

        void Renderer_RenderToolStripBorder(object sender, ToolStripRenderEventArgs e) {
            e.Graphics.DrawLine(SystemPens.ButtonShadow, 0, 1, toolBar.Width, 1);
            e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 2, toolBar.Width, 2);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }


        private void upSplitButton_Click(object sender, EventArgs e) {
            _dir.Up();
            SetTitle(_dir.FileView);
            RefreshDataGrid();
        }

        private void backSplitButton_Click(object sender, EventArgs e) {
            _dir.Up();
            SetTitle(_dir.FileView);
            RefreshDataGrid();
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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewProject = myFileView.FullName;
                    basePathName = myFileView.Name;
                }
            }
            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewProject) + "\\" + basePathName;
            string myNewProjectName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewProjectDir = Path.Combine(basePathForNewProject, myNewProjectName);
            string scriptPathNewProject = myActions.ConvertFullFileNameToPublicPath(strNewProjectDir) + "\\-" + myNewProjectName;
            myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", scriptPathNewProject);
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.

                    ev_Delete_Directory(myFileView.FullName.ToString());
                    string scriptPath = myActions.ConvertFullFileNameToPublicPath(myFileView.FullName) + "\\" + myFileView.Name;
                    string settingsDirectory = myActions.GetAppDirectoryForIdealAutomate();
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

        private void AddGlobalHotKeys() {
            Methods myActions = new Methods();
            ArrayList myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            ArrayList newArrayList = new ArrayList();
            string strHotKey = "";
            HotKeyRecord myHotKeyRecord = new HotKeyRecord();
            bool boolHotKeysGood = true;
            foreach (var item in myArrayList) {
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


        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e) {
            InputSimulator myInputSimulator = new InputSimulator();

            if (myInputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.ESCAPE)) {
                _CurrentDataGridView.ClearSelection();
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
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            //tries to start the process 
            try {
                _proc = Process.Start(strExecutable, "\"" + strContent + "\"");
            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            System.Threading.Thread.Sleep(500);
            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                System.Threading.Thread.Sleep(10);
                _proc.Refresh();
            }

            _proc.WaitForInputIdle();
            _appHandle = _proc.MainWindowHandle;

            SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
            try {
                _proc = Process.Start(strExecutable, "\"" + strContent + "\"");
            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
            splitContainer1.Panel1.Focus();


            _ignoreSelectedIndexChanged = true;
            tabControl1.SelectedIndex = tabControl1.TabCount - 1;
            myActions.TypeText("{TAB}", 1500);
            myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), di.FullName);

            _CurrentIndex = tabControl1.SelectedIndex;




            AddDataGridToTab();

            myActions.SetValueByKey("NumOfTabs", (tabControl1.TabCount).ToString());
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory1 = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
            if (Directory.Exists(strSavedDirectory1)) {
                strInitialDirectory = strSavedDirectory1;
            }
            _dir = new DirectoryView(strInitialDirectory);
            this._CurrentFileViewBindingSource.DataSource = _dir;
            tabControl1.TabPages[tabControl1.SelectedIndex].Text = _dir.FileView.Name;
            tabControl1.TabPages[tabControl1.SelectedIndex].ToolTipText = _dir.FileView.FullName;


            _CurrentDataGridView = (DataGridView)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            _CurrentFileViewBindingSource = listBindingSource[tabControl1.SelectedIndex];
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
            if (strContent == "") {
                Process.Start(strExecutable);
            } else {
                try {
                    Process.Start(strExecutable, string.Concat("\"", strContent, "\""));
                } catch (Exception ex) {

                    MessageBox.Show(ex.ToString());
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

        private void RefreshDataGrid() {
            // refresh datagridview
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            Methods myActions = new Methods();
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
                myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strSavedDirectory);
            }
            _dir = new DirectoryView(strInitialDirectory);
            this._CurrentFileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            _CurrentDataGridView.DataSource = null;
            _CurrentDataGridView.DataSource = _CurrentFileViewBindingSource;
            //   this._CurrentDataGridView.Sort(_CurrentDataGridView.Columns[1], ListSortDirection.Ascending);
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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                if (myCell.ColumnIndex == 0 && e.RowIndex > -1) {
                    // Call Active on DirectoryView
                    string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();

                    Methods myActions = new Methods();
                    string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", fileName);
                    if (categoryState == "Expanded") {
                        myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", fileName);
                        RefreshDataGrid();
                        return;
                    }
                    if (categoryState == "Collapsed") {
                        myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Expanded", fileName);
                        RefreshDataGrid();
                        return;
                    }
                    try {
                        _dir.Activate(this._CurrentFileViewBindingSource[e.RowIndex] as FileView);
                        SetTitle(_dir.FileView);
                        RefreshDataGrid();
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
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
                return;
            }

            if (strButtonPressed == "btnOkay") {
                strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 300, 1200, 100, 100);
                goto LineAfterDisplayWindow;
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
                string convertedPath = "";
                string settingsDirectory = "";
                convertedPath = myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(diSourceSubDir.FullName);
                settingsDirectory = myActions.GetAppDirectoryForIdealAutomate();
                string fromRoamingDirectory = Path.Combine(settingsDirectory, convertedPath);
                convertedPath = myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(target.FullName);
                settingsDirectory = myActions.GetAppDirectoryForIdealAutomate();
                string toRoamingDirectory = Path.Combine(settingsDirectory, convertedPath);
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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    string strExecutable = @"C:\Windows\system32\notepad.exe";
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You cannot open folders with Notepad");
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Left) {
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected) {
                    c.Selected = true;
                    string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();
                    if (fileName.EndsWith(".url")
                    
                     ) {
                        //Close the running process.
                        if (_appHandle != IntPtr.Zero) {
                            PostMessage(_appHandle, WM_CLOSE, 0, 0);
                            System.Threading.Thread.Sleep(1000);
                            _appHandle = IntPtr.Zero;
                        }
                        //tries to start the process 
                        try {
                            Methods myActions = new Methods();
                            myActions.KillAllProcessesByProcessName("iexplore");
                            _proc = Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", GetInternetShortcut(fileName));
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        System.Threading.Thread.Sleep(500);
                        while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);

                    }
                    if (fileName.EndsWith(".rtf")
                        || fileName.EndsWith(".odt")
                        || fileName.EndsWith(".doc")
                        || fileName.EndsWith(".docx")
                        ) {
                        //Close the running process
                        if (_appHandle != IntPtr.Zero) {
                            PostMessage(_appHandle, WM_CLOSE, 0, 0);
                            System.Threading.Thread.Sleep(1000);
                            _appHandle = IntPtr.Zero;
                        }
                        //tries to start the process 
                        try {
                            _proc = Process.Start(@"C:\Program Files\Windows NT\Accessories\wordpad.exe", "\"" + fileName + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        System.Threading.Thread.Sleep(500);
                        while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                            System.Threading.Thread.Sleep(10);
                            _proc.Refresh();
                        }

                        _proc.WaitForInputIdle();
                        _appHandle = _proc.MainWindowHandle;

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);

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
                            //Close the running process
                            if (_appHandle != IntPtr.Zero) {
                                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                                System.Threading.Thread.Sleep(1000);
                                _appHandle = IntPtr.Zero;
                            }
                            //tries to start the process 
                            try {
                                Methods myActions = new Methods();
                                myActions.KillAllProcessesByProcessName("notepad++");
                                _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", fileName);
                            } catch (Exception) {
                                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }


                            System.Threading.Thread.Sleep(500);
                            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                                System.Threading.Thread.Sleep(10);
                                _proc.Refresh();
                            }

                            _proc.WaitForInputIdle();
                            _appHandle = _proc.MainWindowHandle;

                            SetParent(_appHandle, splitContainer1.Panel2.Handle);
                            SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                        }
                    }

                }
            }
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right) {
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected) {
                    c.DataGridView.ClearSelection();
                    c.DataGridView.CurrentCell = c;
                    c.Selected = true;
                }
            }


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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    myActions.KillAllProcessesByProcessName("notepad++");
                    string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You can not open folders with Notepad++");
                }
            }
        }

        private void textDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

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


                    ReDisplayNewTextDocumentDialog:
                    string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                    if (strButtonPressed == "btnCancel") {
                        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                        return;
                    }
                    string basePathForNewTextDocument = _dir.FileView.FullName;
                    string basePathName = _dir.FileView.Name;

                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;

                    string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
                    if (!myNewTextDocumentName.EndsWith(".txt")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".txt";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
                    if (!File.Exists(strNewTextDocumentDir)) {
                        string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".txt", "");
                        //  myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);

                        // File.Create(strNewTextDocumentDir);

                    }
                    string strExecutable = @"C:\Windows\system32\notepad.exe";

                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "False") {
                        myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                        splitContainer1.Panel2Collapsed = false;
                        this.detailsMenuItem.Checked = true;
                        this.listMenuItem.Checked = false;
                        //tries to start the process 
                        try {
                            _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
                            _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    }
                    splitContainer1.SplitterDistance = (int)(ClientSize.Width * .2);

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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    string strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
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
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Create New WordPad Document";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Enter New WordPad Document Name";
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


                    ReDisplayNewTextDocumentDialog:
                    string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                    if (strButtonPressed == "btnCancel") {
                        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                        return;
                    }
                    basePathForNewTextDocument = _dir.FileView.FullName;
                    basePathName = _dir.FileView.Name;

                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;

                    string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
                    if (!myNewTextDocumentName.EndsWith(".rtf")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".rtf";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
                    if (!File.Exists(strNewTextDocumentDir)) {
                        string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".rtf", "");
                        //   myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                        using (StreamWriter sw = new StreamWriter(strNewTextDocumentDir)) {



                        }
                        //   File.Create(strNewTextDocumentDir);

                    }
                    string strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "False") {
                        myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                        splitContainer1.Panel2Collapsed = false;
                        this.detailsMenuItem.Checked = true;
                        this.listMenuItem.Checked = false;
                        //tries to start the process 
                        try {
                            _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
                            _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    }
                    splitContainer1.SplitterDistance = (int)(ClientSize.Width * .2);

                } else {
                    myActions.MessageBoxShow("You can not create a text document inside a file; first select folder and right click");
                }
            }
        }

        private void DeleteStripMenuItem4_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.

                    ev_Delete_Directory(myFileView.FullName.ToString());
                    string scriptPath = myActions.ConvertFullFileNameToPublicPath(myFileView.FullName) + "\\" + myFileView.Name;
                    string settingsDirectory = myActions.GetAppDirectoryForIdealAutomate();
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
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            if (_ignoreSelectedIndexChanged == true) {
                _ignoreSelectedIndexChanged = false;
                return;
            }
            Methods myActions = new Methods();
            myActions.SetValueByKey("CurrentIndex", tabControl1.SelectedIndex.ToString());
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
                
                AddDataGridToTab();

                myActions.SetValueByKey("NumOfTabs", (tabControl1.TabCount).ToString());
                strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                // Set Initial Directory to My Documents
                string strSavedDirectory1 = myActions.GetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString());
                if (Directory.Exists(strSavedDirectory1)) {
                    strInitialDirectory = strSavedDirectory1;
                }
                _dir = new DirectoryView(strInitialDirectory);
                this._CurrentFileViewBindingSource.DataSource = _dir;
                tabControl1.TabPages[tabControl1.SelectedIndex].Text = _dir.FileView.Name;
                tabControl1.TabPages[tabControl1.SelectedIndex].ToolTipText = _dir.FileView.FullName;

            }
            _CurrentDataGridView = (DataGridView)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            _CurrentFileViewBindingSource = listBindingSource[tabControl1.SelectedIndex];

            RefreshDataGrid();
        }

        private void AddDataGridToTab() {
            tabControl1.TabPages.Insert(_CurrentIndex + 1, "    +");
            DataGridView myDataGridView = new DataGridView();
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
             NestingLevel,
             CustomCol,
             StatusCol,
             DescriptionCol
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
            myDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.wordPadToolStripMenuItem});
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
            this.contextMenuStrip1.ResumeLayout(false);
            // 
            // FileViewBindingSource
            // 
            // this.FileViewBindingSource.DataSource = typeof(System.Windows.Forms.Samples.FileView);
            // 

            //&&&&&&&&&&&&&&&

            if (_CurrentIndex == tabControl1.TabCount - 1) {
                tabControl1.TabPages[_CurrentIndex - 1].Controls.Add(myDataGridView);
            } else {
                tabControl1.TabPages[_CurrentIndex].Controls.Add(myDataGridView);
            }
        }

        private void selectIdealToolStripMenuItem_Click_1(object sender, EventArgs e) {
            string fullFileName = "";
            string fileNamea = "";
            FileView myFileView;
            bool isDirectory = false;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                //if (myCell.ColumnIndex != 0 && myCell.RowIndex != 0) {
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
                _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            System.Threading.Thread.Sleep(500);
            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                System.Threading.Thread.Sleep(10);
                _proc.Refresh();
            }

            _proc.WaitForInputIdle();
            _appHandle = _proc.MainWindowHandle;

            SetParent(_appHandle, splitContainer1.Panel2.Handle);
            SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        private void selectActualToolStripMenuItem_Click_1(object sender, EventArgs e) {
            string fullFileName = "";
            string fileNamea = "";
            FileView myFileView;
            bool isDirectory = false;
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                //if (myCell.ColumnIndex != 0 && myCell.RowIndex != 0) {
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            string strContent =  @"C:\Data\Actual.txt";
            //Close the running process
            if (_appHandle != IntPtr.Zero) {
                PostMessage(_appHandle, WM_CLOSE, 0, 0);
                System.Threading.Thread.Sleep(1000);
                _appHandle = IntPtr.Zero;
            }
            //tries to start the process 
            try {
                _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
            } catch (Exception) {
                MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            System.Threading.Thread.Sleep(500);
            while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                System.Threading.Thread.Sleep(10);
                _proc.Refresh();
            }

            _proc.WaitForInputIdle();
            _appHandle = _proc.MainWindowHandle;

            SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
            _dir = new DirectoryView(strInitialDirectory);
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            Methods myActions = new Methods();
            myActions.SetValueByKey("ExpandCollapseAll", "Collapse");
            RefreshDataGrid();
            RefreshDataGrid();
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("ExpandCollapseAll", "Expand");
            RefreshDataGrid();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            RefreshDataGrid();
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
                    myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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




                RefreshDataGrid();
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            _dir = new DirectoryView(strInitialDirectory);
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            _dir = new DirectoryView(strInitialDirectory);
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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

        private void customStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Add Custom Field";
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
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxCustomSelectedValue");
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


            DisplayCustomWindow:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }
            
            string strCustom = myListControlEntity.Find(x => x.ID == "cbxCustom").SelectedValue;
            myActions.SetValueByKey("cbxCustomSelectedValue", strCustom);

            if ((strCustom == "--Select Item ---" || strCustom == "")) {
                myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox; else press Cancel to Exit");
                goto DisplayCustomWindow;
            }

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {                    
                            string fileFullName = myFileView.FullName;
                            myActions.SetValueByKeyForNonCurrentScript("custom", strCustom, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
                 } else {
                    string fileFullName = myFileView.FullName;
                    myActions.SetValueByKeyForNonCurrentScript("custom", strCustom, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());                
                     fileFullName = myFileView.FullName;      
            }

            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                myManualTimeFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            myControlEntity.SelectedValue = myActions.GetValueByKeyForNonCurrentScript("custom", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
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
            myControlEntity.Text = myActions.GetValueByKeyForNonCurrentScript("description",  myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
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
            myControlEntity.SelectedValue = myActions.GetValueByKeyForNonCurrentScript("status", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
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
            myActions.SetValueByKey("cbxCustomSelectedValue", strCustom);
            string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;           
            string myManualExecutionTime = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;
            myActions.SetValueByKey("cbxStatusSelectedValue", strStatus);

            myActions.SetValueByKeyForNonCurrentScript("custom", strCustom, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            myActions.SetValueByKeyForNonCurrentScript("description", strdescription, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            myActions.SetValueByKeyForNonCurrentScript("ManualExecutionTime", myManualExecutionTime, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileManualTimeFullName));
            myActions.SetValueByKeyForNonCurrentScript("status", strStatus, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));


        }

        private void statusStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Add Status";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            int intRowCtr = 0;
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
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxStatusSelectedValue");
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


            DisplayStatusWindow:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                return;
            }

            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;
            myActions.SetValueByKey("cbxStatusSelectedValue", strStatus);

            if ((strStatus == "--Select Item ---" || strStatus == "")) {
                myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox; else press Cancel to Exit");
                goto DisplayStatusWindow;
            }
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {                    
                            string fileFullName = myFileView.FullName;
                            myActions.SetValueByKeyForNonCurrentScript("status", strStatus, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
                } else {
                    string fileFullName = myFileView.FullName;
                    myActions.SetValueByKeyForNonCurrentScript("status", strStatus, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
                }

            }
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

                RefreshDataGrid();
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

            RefreshDataGrid();
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
            string settingsDirectory = GetAppDirectoryForScript(myActions.ConvertFullFileNameToScriptPath(myNewProjectSourcePath));

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




            myActions.SetValueByKey("InitialDirectory" + tabControl1.SelectedIndex.ToString(), strCurrentPath);

            RefreshDataGrid();
        }

        private void WindowsExplorerStripMenuItem2_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    string strExecutable = @"C:\Windows\explorer.exe";
                    myActions.Run(strExecutable, "\"" + myFileView.FullName + "\"");
                } else {
                    myActions.MessageBoxShow("You can only open directories with Windows Explorer");
                }
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) {
            //host the started process in the panel 

            if (_proc != null) {
                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                Methods myActions = new Methods();
                myActions.SetValueByKey("SplitContainer1Width", e.X.ToString());
            }


        }

        private void search_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions = new Methods();
            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (detailsMenuItemChecked == "False") {
                myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                splitContainer1.Panel2Collapsed = false;
                this.detailsMenuItem.Checked = true;
                this.listMenuItem.Checked = false;
                //tries to start the process 
                try {
                    _proc = Process.Start(@"C:\Program Files\Windows NT\Accessories\wordpad.exe");
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                //SendMessage(proc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }
            splitContainer1.SplitterDistance = (int)(ClientSize.Width * .2);

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
                    if (alHostx.Count > 14) {
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
            List<FileInfo> myFileList = TraverseTree(strSearchPattern, strPathToSearch);
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
                myActions.KillAllProcessesByProcessName("notepad++");
                string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                string strContent = settingsDirectory + @"\MatchInfo.txt";
                //Close the running process
                if (_appHandle != IntPtr.Zero) {
                    PostMessage(_appHandle, WM_CLOSE, 0, 0);
                    System.Threading.Thread.Sleep(1000);
                    _appHandle = IntPtr.Zero;
                }
                //tries to start the process 
                try {
                    _proc = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                System.Threading.Thread.Sleep(500);
                while ((_proc.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(_proc.MainWindowHandle))) {
                    System.Threading.Thread.Sleep(10);
                    _proc.Refresh();
                }

                _proc.WaitForInputIdle();
                _appHandle = _proc.MainWindowHandle;

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                myActions.MessageBoxShow("RunTime: " + elapsedTime + "\n\r\n\rHits: " + intHits.ToString() + "\n\r\n\rFiles with hits: " + intUniqueFiles.ToString() + "\n\r\n\rPut Cursor on line and\n\r press Ctrl+Alt+N\n\rto view detail page. ");
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
                    myActions.MessageBoxShow(ex.InnerException.ToString());
                } else {
                    myActions.MessageBoxShow(ex.Message);
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
                    myActions.MessageBoxShow(ex.InnerException.ToString());
                } else {
                    myActions.MessageBoxShow(ex.Message);
                }
            }

            if (docText.Contains(text)) {
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
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewTextDocument = myFileView.FullName;
                    basePathName = myFileView.Name;
                    basePathForNewFolder = myFileView.FullName;
                }
            }
                        
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            myActions.SetValueByKey("cbxCustomSelectedValue", strCustom);
            string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;            
            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;
            myActions.SetValueByKey("cbxStatusSelectedValue", strStatus);

            
            if (!myNewTextDocumentName.EndsWith(".txt")) {
                myNewTextDocumentName = myNewTextDocumentName + ".txt";
            }
            string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
            myActions.SetValueByKeyForNonCurrentScript("custom", strCustom, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            myActions.SetValueByKeyForNonCurrentScript("description", strdescription, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            myActions.SetValueByKeyForNonCurrentScript("status", strStatus, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            if (!File.Exists(strNewTextDocumentDir)) {
                string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".txt", "");
                //  myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);

                // File.Create(strNewTextDocumentDir);

            }
            string strExecutable = @"C:\Windows\system32\notepad.exe";

            string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
            if (detailsMenuItemChecked == "False") {
                myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                splitContainer1.Panel2Collapsed = false;
                this.detailsMenuItem.Checked = true;
                this.listMenuItem.Checked = false;
                //tries to start the process 
                try {
                    _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");                    
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
                    _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                } catch (Exception) {
                    MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                SetParent(_appHandle, splitContainer1.Panel2.Handle);
                SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }        
            splitContainer1.SplitterDistance = (int)(ClientSize.Width * .2);
        
                
            
        }

        private void wordPadFileToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string basePathForNewFolder = _dir.FileView.FullName;
            string basePathName = _dir.FileView.Name;
            string basePathForNewTextDocument = _dir.FileView.FullName;
            string fileFullName = "";
            foreach (DataGridViewCell myCell in _CurrentDataGridView.SelectedCells) {
                FileView myFileView = (FileView)this._CurrentFileViewBindingSource[myCell.RowIndex];
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
            myActions.SetValueByKey("cbxCustomSelectedValue", strCustom);
            string strdescription = myListControlEntity.Find(x => x.ID == "txtDescription").Text;

            string strStatus = myListControlEntity.Find(x => x.ID == "cbxStatus").SelectedValue;
            myActions.SetValueByKey("cbxStatusSelectedValue", strStatus);

           

            string parentScriptPath = myActions.ConvertFullFileNameToPublicPath(basePathForNewTextDocument) + "\\" + basePathName;
                    string myNewTextDocumentName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
                    if (!myNewTextDocumentName.EndsWith(".rtf")) {
                        myNewTextDocumentName = myNewTextDocumentName + ".rtf";
                    }
                    string strNewTextDocumentDir = Path.Combine(basePathForNewTextDocument, myNewTextDocumentName);
            myActions.SetValueByKeyForNonCurrentScript("custom", strCustom, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            myActions.SetValueByKeyForNonCurrentScript("description", strdescription, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            myActions.SetValueByKeyForNonCurrentScript("status", strStatus, myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(strNewTextDocumentDir));
            if (!File.Exists(strNewTextDocumentDir)) {
                        string newFolderScriptPath = basePathForNewTextDocument + "\\" + myNewTextDocumentName.Replace(".rtf", "");
                        //   myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                        using (StreamWriter sw = new StreamWriter(strNewTextDocumentDir)) {



                        }
                        //   File.Create(strNewTextDocumentDir);

                    }
                    string strExecutable = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";
                    string detailsMenuItemChecked = myActions.GetValueByKey("DetailsMenuItemChecked");
                    if (detailsMenuItemChecked == "False") {
                        myActions.SetValueByKey("DetailsMenuItemChecked", "True");
                        splitContainer1.Panel2Collapsed = false;
                        this.detailsMenuItem.Checked = true;
                        this.listMenuItem.Checked = false;
                        //tries to start the process 
                        try {
                            _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
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
                            _proc = Process.Start(strExecutable, "\"" + strNewTextDocumentDir + "\"");
                        } catch (Exception) {
                            MessageBox.Show("Something went wrong trying to start your process", "App Hoster", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        SetParent(_appHandle, splitContainer1.Panel2.Handle);
                        SendMessage(_appHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                    }
                    splitContainer1.SplitterDistance = (int)(ClientSize.Width * .2);

                }



        private void ExplorerView_KeyUp(object sender, KeyEventArgs e) {
  
                if (e.KeyCode == Keys.Escape) {
                _CurrentDataGridView.ClearSelection();
            }
           
        }

        private void ExplorerView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                _CurrentDataGridView.ClearSelection();
            }
        }
    }
       

}



