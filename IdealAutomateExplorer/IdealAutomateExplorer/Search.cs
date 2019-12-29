using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using AODL.Document.Content;
using AODL.Document.TextDocuments;
using DgvFilterPopup;
using DocumentFormat.OpenXml.Packaging;
using IdealAutomate.Core;

namespace System.Windows.Forms.Samples {
    public partial class Search : Form {
        private DirectoryView _dir;
        string strInitialDirectory = "";
        ArrayList _myArrayList;
        int _CurrentIndex = 0;
        const int LEADING_SPACE = 12;
        const int CLOSE_SPACE = 15;
        const int CLOSE_AREA = 15;
        bool boolStopEvent = false;
        bool _ignoreFolderIndexChanged = true;
        string _scanningDir = "";
        string myResult = "";
        private bool _NotepadppLoaded = false;
        DataGridViewExt dgvResults = new DataGridViewExt("SearchResults");       
        static StringBuilder _searchErrors = new StringBuilder();
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
        string strFileType = "";
        string strExclude = "";
        string strFolder = "";
        List<BindingSource> listBindingSource = new List<BindingSource>();
        List<Panel> listSplitContainer = new List<Panel>();
        List<int> listLoadedIndexes = new List<int>();
        BindingSource _CurrentFileViewBindingSource = new BindingSource();
      
        
        bool _newTab = true;
        bool _ignoreSelectedIndexChanged = true;
        int _selectedTabIndex = 0;
        Rectangle _IconRectangle = new Rectangle();
        DataGridViewExt _CurrentDataGridView;

        

        public static List<MatchInfo> matchInfoList;
        public Search() {
            InitializeComponent();
            for (int i = 0; i < 20; i++) {
                BindingSource myNewBindingSource = new BindingSource();
                listBindingSource.Add(myNewBindingSource);
            }
            for (int i = 0; i < 20; i++) {
                Panel myNewSplitContainer = new Panel();
                listSplitContainer.Add(myNewSplitContainer);
            }
           // _CurrentDataGridView = new DataGridViewExt(myActions.GetValueByKey("InitialDirectory" + i.ToString()));
            _CurrentFileViewBindingSource = FileViewBindingSource;
          
            tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tabControl1.HotTrack = true;

            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.Padding = new System.Drawing.Point(20, 3);
            tabControl1.MouseClick += TabControl1_MouseClick;

        }

        private async void TabControl1_MouseClick(object sender, MouseEventArgs e) {
            Methods myActions = new Methods();
            _CurrentIndex = tabControl1.SelectedIndex;
            myActions.SetValueByKey("CurrentIndexSearch", _CurrentIndex.ToString());
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
           // Methods myActions = new Methods();
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
            //if (_ignoreSelectedIndexChanged == true) {
            //    _ignoreSelectedIndexChanged = false;
            //    //  return;
            //}

            
            this.Cursor = Cursors.WaitCursor;
            myActions.SetValueByKey("CurrentIndexSearch", tabControl1.SelectedIndex.ToString());
 
                _newTab = true;
 
            string strInitialDirectory = "SearchResults";
            new DgvFilterManager(dgvResults);
            int sortedColumn = myActions.GetValueByKeyAsInt("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            string myDirection = myActions.GetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));


            //if (sortedColumn > -1 && dgvResults.ColumnCount >= sortedColumn) {
            //    if (myDirection == "Ascending") {
            //        dgvResults.Sort(dgvResults.Columns[sortedColumn], ListSortDirection.Ascending);
            //    } else {
            //        dgvResults.Sort(dgvResults.Columns[sortedColumn], ListSortDirection.Descending);
            //    }
            //    myActions.SetValueByKey("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), "-1");
            //    myActions.SetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), ListSortDirection.Ascending.ToString());
            //}
            //   this.dgvResults.Sort(dgvResults.Columns[1], ListSortDirection.Ascending);
            // Use of the DataGridViewColumnSelector
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(dgvResults);
            cs.MaxHeight = 100;
            cs.Width = 110;

            dgvResults.Width = ClientSize.Width - 100;
            dgvResults.Height = ClientSize.Height - 150;
            dgvResults.ScrollBars = ScrollBars.Both;
            dgvResults.AllowUserToResizeColumns = true;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _searchErrors.Length = 0;

            matchInfoList = new List<MatchInfo>();
            MatchInfo myMatchInfo = new MatchInfo();
            myMatchInfo.FullName = "";
            myMatchInfo.LineNumber = 0;
            myMatchInfo.LinePosition = 0;
            myMatchInfo.LineText = "";
            matchInfoList.Add(myMatchInfo);
            dgvResults.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse Show\\Hide button to Show\\Hide Columns.\nLeft-Click column heading to sort.";
            }
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.Dock = DockStyle.Fill;
            this._CurrentFileViewBindingSource.DataSource = dgvResults.DataSource;
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Count > 0) {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.RemoveAt(0);
            }
            tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(dgvResults);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
            }
            listLoadedIndexes.Add(tabControl1.SelectedIndex);
           //}
            
            _newTab = true;           
            this.Cursor = Cursors.Default;
            
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
        private async void RefreshDataGrid() {


            // refresh datagridview
            Methods myActions = new Methods();
            string strInitialDirectory = "SearchResults";
            new DgvFilterManager(dgvResults);
            int sortedColumn = myActions.GetValueByKeyAsInt("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            string myDirection = myActions.GetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            

            //if (sortedColumn > -1 && dgvResults.ColumnCount >= sortedColumn) {
            //    if (myDirection == "Ascending") {
            //        dgvResults.Sort(dgvResults.Columns[sortedColumn], ListSortDirection.Ascending);
            //    } else {
            //        dgvResults.Sort(dgvResults.Columns[sortedColumn], ListSortDirection.Descending);
            //    }
            //    myActions.SetValueByKey("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), "-1");
            //    myActions.SetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), ListSortDirection.Ascending.ToString());
            //}
            //   this.dgvResults.Sort(dgvResults.Columns[1], ListSortDirection.Ascending);
            // Use of the DataGridViewColumnSelector
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(dgvResults);
            cs.MaxHeight = 100;
            cs.Width = 110;
           
            dgvResults.Width = ClientSize.Width - 100;
            dgvResults.Height = ClientSize.Height - 150;
            dgvResults.ScrollBars = ScrollBars.Both;
            dgvResults.AllowUserToResizeColumns = true;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _searchErrors.Length = 0;
           // Methods myActions = new Methods();
            myActions = new Methods();
            boolMatchCase = chkMatchCase.Checked;
            boolUseRegularExpression = chkUseRegularExpression.Checked;

            strFindWhat = cbxFindWhat.Text;
            string strFileType = cbxFileType.Text;
            string strExclude = cbxExclude.Text;
            string strFolder = cbxFolder.Text;

            myActions.SetValueByKey("chkMatchCase", boolMatchCase.ToString());
            myActions.SetValueByKey("chkUseRegularExpression", boolUseRegularExpression.ToString());
            myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);
            myActions.SetValueByKey("cbxFileTypeSelectedValue", strFileType);
            myActions.SetValueByKey("cbxExcludeSelectedValue", strExclude);
            myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);


            string strFindWhatToUse = "";
            string strFileTypeToUse = "";
            string strExcludeToUse = "";
            string strFolderToUse = "";

            if ((strFindWhat == "--Select Item ---" || strFindWhat == "")) {
                myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox");
                return;
            }
            if ((strFileType == "--Select Item ---" || strFileType == "")) {
                myActions.MessageBoxShow("Please enter File Type or select File Type from ComboBox");
                return;
            }
            if ((strExclude == "--Select Item ---" || strExclude == "")) {
                myActions.MessageBoxShow("Please enter Exclude or select Exclude from ComboBox");
                return;
            }
            if ((strFolder == "--Select Item ---" || strFolder == "")) {
                myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox");
                return;
            }



            strFindWhatToUse = strFindWhat;

            if (boolUseRegularExpression) {
                strFindWhatToUse = strFindWhatToUse.Replace(")", "\\)").Replace("(", "\\(");
            }


            strFileTypeToUse = strFileType;
            strExcludeToUse = strExclude;
            strFolderToUse = strFolder;
            strPathToSearch = strFolderToUse;
            strSearchPattern = strFileTypeToUse;
            strSearchExcludePattern = strExcludeToUse;
            strSearchText = strFindWhatToUse;

            strLowerCaseSearchText = strFindWhatToUse.ToLower();
            myActions.SetValueByKey("FindWhatToUse", strFindWhatToUse);
            myActions.SetValueByKey("FileTypeToUse", strFileTypeToUse);
            myActions.SetValueByKey("ExcludeToUse", strExcludeToUse);
            myActions.SetValueByKey("FolderToUse", strFolderToUse);
            try {
 //               var damageResult = await Task.Run(() => SearchTask());
 //               progressBar1.BeginInvoke(
 //        new Action(() => {
 //            progressBar1.Value = 0;
 //        }
 //));
 //               lblResults.Text = damageResult;
 //               myActions.MessageBoxShow(damageResult);
                this.WindowState = FormWindowState.Maximized;

            } catch (Exception ex) {
                // MessageBox.Show(ex.Message);

            }
            // Initialize the DataGridView.
           dgvResults.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse Show\\Hide button to Show\\Hide Columns.\nLeft-Click column heading to sort.";
            }
 
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.Dock = DockStyle.Fill;
            this._CurrentFileViewBindingSource.DataSource = dgvResults.DataSource;
            //    this._CurrentDataGridView.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Count > 0) {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.RemoveAt(0);
            }
            tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(dgvResults);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
            }

        }
        private string GetAppDirectoryForScript() {
            Methods myActions = new Methods();
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            string settingsDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + myActions.ConvertFullFileNameToScriptPath(directory);
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }
 


        private void search_ClickAsync(object sender, EventArgs e) {
            _searchErrors.Length = 0;
            Methods myActions = new Methods();
            myActions = new Methods();  
            boolMatchCase = chkMatchCase.Checked;
            boolUseRegularExpression = chkUseRegularExpression.Checked;

            strFindWhat = cbxFindWhat.Text;           
            string strFileType = cbxFileType.Text; 
            string strExclude = cbxExclude.Text;
            string strFolder = cbxFolder.Text; 

            myActions.SetValueByKey("chkMatchCase", boolMatchCase.ToString());
            myActions.SetValueByKey("chkUseRegularExpression", boolUseRegularExpression.ToString());
            myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);
            myActions.SetValueByKey("cbxFileTypeSelectedValue", strFileType);
            myActions.SetValueByKey("cbxExcludeSelectedValue", strExclude);
            myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
         

            string strFindWhatToUse = "";
            string strFileTypeToUse = "";
            string strExcludeToUse = "";
            string strFolderToUse = "";

            if ((strFindWhat == "--Select Item ---" || strFindWhat == "")) {
                myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox");
                return;
            }
            if ((strFileType == "--Select Item ---" || strFileType == "")) {
                myActions.MessageBoxShow("Please enter File Type or select File Type from ComboBox");
                return;
            }
            if ((strExclude == "--Select Item ---" || strExclude == "")) {
                myActions.MessageBoxShow("Please enter Exclude or select Exclude from ComboBox");
                return;
            }
            if ((strFolder == "--Select Item ---" || strFolder == "")) {
                myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox");
                return;
            }



            strFindWhatToUse = strFindWhat;

            if (boolUseRegularExpression) {
                strFindWhatToUse = strFindWhatToUse.Replace(")", "\\)").Replace("(", "\\(");
            }


            strFileTypeToUse = strFileType;
            strExcludeToUse = strExclude;
            strFolderToUse = strFolder;
            strPathToSearch = strFolderToUse;
            strSearchPattern = strFileTypeToUse;
            strSearchExcludePattern = strExcludeToUse;
            strSearchText = strFindWhatToUse;

            strLowerCaseSearchText = strFindWhatToUse.ToLower();
            myActions.SetValueByKey("FindWhatToUse", strFindWhatToUse);
            myActions.SetValueByKey("FileTypeToUse", strFileTypeToUse);
            myActions.SetValueByKey("ExcludeToUse", strExcludeToUse);
            myActions.SetValueByKey("FolderToUse", strFolderToUse);
            try {
 //               var damageResult =  SearchTask();
 //               progressBar1.BeginInvoke(
 //        new Action(() => {
 //            progressBar1.Value = 0;
 //        }
 //));
 //               lblResults.Text = damageResult;
 //             //  myActions.MessageBoxShow(damageResult);
 //               this.WindowState = FormWindowState.Maximized;

            } catch (Exception ex) {
                // MessageBox.Show(ex.Message);

            }
            // Initialize the DataGridView.
            dgvResults.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse Show\\Hide button to Show\\Hide Columns.\nLeft-Click column heading to sort.";
            }
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.Dock = DockStyle.Fill;
            RefreshDataGrid();
        }
        private string SearchTask() {
           
            //backgroundWorker1.RunWorkerAsync();
            string myResult = "";


            return  myResult;
        }
        private void startBtn_Click(object sender, EventArgs e) {
            _searchErrors.Length = 0;
            // Methods myActions = new Methods();
            Methods myActions = new Methods();
            boolMatchCase = chkMatchCase.Checked;
            boolUseRegularExpression = chkUseRegularExpression.Checked;
            lblResults.Text = "Count: 0";
            strFindWhat = cbxFindWhat.Text;
            string strFileType = cbxFileType.Text;
            string strExclude = cbxExclude.Text;
            string strFolder = cbxFolder.Text;

            myActions.SetValueByKey("chkMatchCase", boolMatchCase.ToString());
            myActions.SetValueByKey("chkUseRegularExpression", boolUseRegularExpression.ToString());
            myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);
            myActions.SetValueByKey("cbxFileTypeSelectedValue", strFileType);
            myActions.SetValueByKey("cbxExcludeSelectedValue", strExclude);
            myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);


            string strFindWhatToUse = "";
            string strFileTypeToUse = "";
            string strExcludeToUse = "";
            string strFolderToUse = "";

            if ((strFindWhat == "--Select Item ---" || strFindWhat == "")) {
                myActions.MessageBoxShow("Please enter Find What or select Find What from ComboBox");
                return;
            }
            if ((strFileType == "--Select Item ---" || strFileType == "")) {
                myActions.MessageBoxShow("Please enter File Type or select File Type from ComboBox");
                return;
            }
            if ((strExclude == "--Select Item ---" || strExclude == "")) {
                myActions.MessageBoxShow("Please enter Exclude or select Exclude from ComboBox");
                return;
            }
            if ((strFolder == "--Select Item ---" || strFolder == "")) {
                myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox");
                return;
            }



            strFindWhatToUse = strFindWhat;

            if (boolUseRegularExpression) {
                strFindWhatToUse = strFindWhatToUse.Replace(")", "\\)").Replace("(", "\\(");
            }


            strFileTypeToUse = strFileType;
            strExcludeToUse = strExclude;
            strFolderToUse = strFolder;
            strPathToSearch = strFolderToUse;
            strSearchPattern = strFileTypeToUse;
            strSearchExcludePattern = strExcludeToUse;
            strSearchText = strFindWhatToUse;

            strLowerCaseSearchText = strFindWhatToUse.ToLower();
            myActions.SetValueByKey("FindWhatToUse", strFindWhatToUse);
            myActions.SetValueByKey("FileTypeToUse", strFileTypeToUse);
            myActions.SetValueByKey("ExcludeToUse", strExcludeToUse);
            myActions.SetValueByKey("FolderToUse", strFolderToUse);
            strPathToSearch = tabControl1.TabPages[_CurrentIndex].ToolTipText.Replace("Click on this tab to search in ", "");
            //START BG WORKER
            progressBar1.BeginInvoke(
new Action(() => {
progressBar1.Value = 0;
}
));
            backgroundWorker1.RunWorkerAsync();
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            //REQUEST CANCELLATION
            backgroundWorker1.CancelAsync();
        }


        //RUN BG STUFF HERE.NO GUI HERE PLEASE
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            
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
            int intProcessedFiles = 0;
            progressBar1.Maximum = 100;
            int tenPercent = myFileList.Count / 10;
            int prevPercent = 0;
            matchInfoList = new List<MatchInfo>();
            //         myFileList = myFileList.OrderBy(fi => fi.FullName).ToList();
            Parallel.ForEach(myFileList, myFileInfo => {
                intLineCtr = 0;
                boolStringFoundInFile = false;
                intProcessedFiles++;

                //CHECK FOR CANCELLATION FIRST
                if (backgroundWorker1.CancellationPending) {
                    //CANCEL
                    e.Cancel = true;
                } else {
                    ReadFileToString(myFileInfo.FullName, intLineCtr, matchInfoList);
                    if (boolStringFoundInFile) {
                        intFiles++;
                    }
                    int currentPercent = (intProcessedFiles * 100) / myFileList.Count;
                    if (currentPercent > prevPercent + 10) {
                        prevPercent = currentPercent;
                        int intUniqueFiles = 0;
                        try {
                            intUniqueFiles = matchInfoList.Select(x => x.FullName).Distinct().Count();
                        } catch (Exception ex) {

                            intUniqueFiles = 0;
                        }
                       
                        // Get the elapsed time as a TimeSpan value.
                        TimeSpan ts = st.Elapsed;
                       
                        // Format and display the TimeSpan value.
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
  
                        myResult = "  RunTime: " + elapsedTime + "  Hits: " + intHits.ToString() + "  Files with hits: " + intUniqueFiles.ToString();
                        if (_searchErrors.Length > 0) {
                            myResult += "\n\rErrors: " + _searchErrors.ToString();
                        }
                        backgroundWorker1.ReportProgress(currentPercent);           
                    }
                }


            });
            matchInfoList = matchInfoList.Where(mi => mi != null).OrderBy(mi => mi.FullName).ThenBy(mi => mi.LineNumber).ToList();

            List<string> lines = new List<string>();
            foreach (var item in matchInfoList) {
                lines.Add("\"" + item.FullName + "\"(" + item.LineNumber + "," + item.LinePosition + "): " + item.LineText.Length.ToString() + " " + item.LineText.Substring(0, item.LineText.Length > 5000 ? 5000 : item.LineText.Length));
            }

            string settingsDirectory =
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
                // myActions.Run(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + strContent + "\"");
                myResult = "RunTime: " + elapsedTime + "\n\rHits: " + intHits.ToString() + "\n\rFiles with hits: " + intUniqueFiles.ToString();
                if (_searchErrors.Length > 0) {
                    myResult += "\n\rErrors: " + _searchErrors.ToString();
                }
            }             
            dgvResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResults_CellMouseDown);
        }

        //THIS UPDATES GUI.OUR PROGRESSBAR
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage == -1) {
                percentageLabel.Text = _scanningDir;
            } else {
                progressBar1.Value = e.ProgressPercentage;
                percentageLabel.Text = e.ProgressPercentage.ToString() + " % " + myResult;
            }
        }

        //WHEN JOB IS DONE THIS IS CALLED.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled) {
                display("You have Cancelled");
                progressBar1.Value = 0;
                percentageLabel.Text = "0";
            } else {
                percentageLabel.Text = "100 %";
                progressBar1.Value = 100;
                display(myResult);
                dgvResults.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
                foreach (DataGridViewColumn item in dgvResults.Columns) {
                    item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse Show\\Hide button to Show\\Hide Columns.\nLeft-Click column heading to sort.";
                }
 
                dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                dgvResults.Dock = DockStyle.Fill;
                this._CurrentFileViewBindingSource.DataSource = dgvResults.DataSource;
                if (tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Count > 0) {
                    tabControl1.TabPages[tabControl1.SelectedIndex].Controls.RemoveAt(0);
                }
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(dgvResults);
                foreach (DataGridViewColumn item in dgvResults.Columns) {
                    item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
                }
                lblResults.Text = myResult;
            }
        }

        //DISPLAY MSG BOX
        private void display(String text) {
            MessageBox.Show(text);
        }
        public  List<FileInfo> TraverseTree(string filterPattern, string root) {
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
                //CHECK FOR CANCELLATION FIRST
                _scanningDir = currentDir;
                    backgroundWorker1.ReportProgress(-1);
                

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
                try {
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
                } catch (Exception ex) {

                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
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

        private void Search_Load(object sender, EventArgs e) {

            dgvResults.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvResults_CellPainting);

            Methods myActions = new Methods();

            myActions = new Methods();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            string settingsDirectory =
     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            string fileName = "cbxFindWhat.txt";
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
                    if (keyvalue[0] != "--Select Item ---" && keyvalue[0] != "") {
                        cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));

                    }
                }
                objSRFile.Close();
            }

            foreach (var item in cbp) {
                cbxFindWhat.Items.Add(item);
            }
            cbxFindWhat.DisplayMember = "_Value";
            cbxFindWhat.SelectedValue = myActions.GetValueByKey("cbxFindWhatSelectedValue");
            cbxFindWhat.Text = myActions.GetValueByKey("cbxFindWhatSelectedValue");

            //------------------------------------------
            fileName = "cbxFileType.txt";
            settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            alHosts = new ArrayList();
            cbp = new List<ComboBoxPair>();
            cbp.Clear();
            cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
            cbp.Add(new ComboBoxPair("*.*", "*.*"));
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
                    if (keyvalue[0] != "--Select Item ---" && keyvalue[0] != "" && keyvalue[0] != "*.*") {
                        cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));

                    }
                }
                objSRFile.Close();
            }

            foreach (var item in cbp) {
                cbxFileType.Items.Add(item);
            }
            cbxFileType.DisplayMember = "_Value";
            cbxFileType.SelectedValue = myActions.GetValueByKey("cbxFileTypeSelectedValue");
            cbxFileType.Text = myActions.GetValueByKey("cbxFileTypeSelectedValue");

            //------------------------------------------
            fileName = "cbxExclude.txt";
            settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            alHosts = new ArrayList();
            cbp = new List<ComboBoxPair>();
            cbp.Clear();
            cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
            cbp.Add(new ComboBoxPair("*.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy", "*.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy"));
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
                    if (keyvalue[0] != "--Select Item ---" && keyvalue[0] != "" && keyvalue[0] != "*.dll;*.exe;*.png;*.xml;*.cache;*.sln;*.suo;*.pdb;*.csproj;*.deploy") {
                        cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));

                    }
                }
                objSRFile.Close();
            }

            foreach (var item in cbp) {
                cbxExclude.Items.Add(item);
            }
            cbxExclude.DisplayMember = "_Value";
            cbxExclude.SelectedValue = myActions.GetValueByKey("cbxExcludeSelectedValue");
            cbxExclude.Text = myActions.GetValueByKey("cbxExcludeSelectedValue");

            //------------------------------------------
            fileName = "cbxFolder.txt";
            settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            alHosts = new ArrayList();
            cbp = new List<ComboBoxPair>();
            cbp.Clear();
            cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
            cbp.Add(new ComboBoxPair("C:\\Github", "C:\\Github"));
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
                    if (keyvalue[0] != "--Select Item ---" && keyvalue[0] != "" && keyvalue[0] != "C:\\Github") {
                        cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));

                    }
                }
                objSRFile.Close();
            }

            foreach (var item in cbp) {
                cbxFolder.Items.Add(item);
            }
            cbxFolder.DisplayMember = "_Value";
            cbxFolder.SelectedValue = myActions.GetValueByKey("cbxFolderSelectedValue");
            cbxFolder.Text = myActions.GetValueByKey("cbxFolderSelectedValue");
            //------------------------------------------           

            string strMatchCase = myActions.GetValueByKey("chkMatchCase");

            if (strMatchCase.ToLower() == "true") {
                chkMatchCase.Checked = true;
            } else {
                chkMatchCase.Checked = false;
            }

            string strUseRegularExpression = myActions.GetValueByKey("chkUseRegularExpression");
            if (strUseRegularExpression.ToLower() == "true") {
                chkUseRegularExpression.Checked = true;
            } else {
                chkUseRegularExpression.Checked = false;
            }
            matchInfoList = new List<MatchInfo>();
            MatchInfo myMatchInfo = new MatchInfo();
            myMatchInfo.FullName = "";
            myMatchInfo.LineNumber = 0;
            myMatchInfo.LinePosition = 0;
            myMatchInfo.LineText = "";
            matchInfoList.Add(myMatchInfo);
            dgvResults.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse Show\\Hide button to Show\\Hide Columns.\nLeft-Click column heading to sort.";
            }
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.Dock = DockStyle.Fill;

            // T A B S
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
                _dir = new DirectoryView(strInitialDirectory, false, _myArrayList, myActions);
                this._CurrentFileViewBindingSource.DataSource = _dir;

                tabControl1.TabPages[i].Text = _dir.FileView.Name;
                tabControl1.TabPages[i].ToolTipText = "Click on this tab to search in " + _dir.FileView.FullName;
                _CurrentIndex = i;
                AddDataGridToTab(strInitialDirectory);


            }
            // do last tab
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory2 = cbxFolder.Text;
            if (Directory.Exists(strSavedDirectory2)) {
                strInitialDirectory = strSavedDirectory2;
            }

            // do not fill the directory because first time through
            // we are just addding name and tooltip to each tab
            _dir = new DirectoryView(strInitialDirectory, false, _myArrayList, myActions);
            this._CurrentFileViewBindingSource.DataSource = _dir;


            _CurrentIndex = numOfTabs - 1;
           AddDataGridToLastTab();

            // simulate clicking  tab
            _CurrentIndex = myActions.GetValueByKeyAsInt("CurrentIndexSearch");
            tabControl1.SelectedIndex = _CurrentIndex;
            this.Cursor = Cursors.WaitCursor;
          //  myActions.SetValueByKey("CurrentIndexSearch", tabControl1.SelectedIndex.ToString());

            _newTab = true;

            strInitialDirectory = "SearchResults";
            new DgvFilterManager(dgvResults);
            int sortedColumn = myActions.GetValueByKeyAsInt("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));
            string myDirection = myActions.GetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"));


            //if (sortedColumn > -1 && dgvResults.ColumnCount >= sortedColumn) {
            //    if (myDirection == "Ascending") {
            //        dgvResults.Sort(dgvResults.Columns[sortedColumn], ListSortDirection.Ascending);
            //    } else {
            //        dgvResults.Sort(dgvResults.Columns[sortedColumn], ListSortDirection.Descending);
            //    }
            //    myActions.SetValueByKey("SortedColumn_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), "-1");
            //    myActions.SetValueByKey("SortOrder_" + strInitialDirectory.Replace(":", "+").Replace("\\", "-"), ListSortDirection.Ascending.ToString());
            //}
            //   this.dgvResults.Sort(dgvResults.Columns[1], ListSortDirection.Ascending);
            // Use of the DataGridViewColumnSelector
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(dgvResults);
            cs.MaxHeight = 100;
            cs.Width = 110;

            dgvResults.Width = ClientSize.Width - 100;
            dgvResults.Height = ClientSize.Height - 150;
            dgvResults.ScrollBars = ScrollBars.Both;
            dgvResults.AllowUserToResizeColumns = true;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _searchErrors.Length = 0;

            matchInfoList = new List<MatchInfo>();
            myMatchInfo = new MatchInfo();
            myMatchInfo.FullName = "";
            myMatchInfo.LineNumber = 0;
            myMatchInfo.LinePosition = 0;
            myMatchInfo.LineText = "";
            matchInfoList.Add(myMatchInfo);
            dgvResults.DataSource = ConvertToDataTable<MatchInfo>(matchInfoList);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse Show\\Hide button to Show\\Hide Columns.\nLeft-Click column heading to sort.";
            }
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.Dock = DockStyle.Fill;
            this._CurrentFileViewBindingSource.DataSource = dgvResults.DataSource;
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Count > 0) {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.RemoveAt(0);
            }
            tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(dgvResults);
            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
            }
            listLoadedIndexes.Add(tabControl1.SelectedIndex);
            //}

            _newTab = true;
            this.Cursor = Cursors.Default;

            // tabControl1.Visible = false;
        }

        private void AddDataGridToLastTab() {
            // do not need to insert last tab because it was already inserted
          //  tabControl1.TabPages.Insert(_CurrentIndex + 1, "Search Folder or File");
        //    tabControl1.TabPages[_CurrentIndex + 1].Text = _dir.FileView.Name;
            tabControl1.TabPages[_CurrentIndex].ToolTipText = "Click on this tab to search in " + _dir.FileView.FullName;
            DataGridViewExt dgvResults = new DataGridViewExt("SearchResults");
           
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



            ((System.ComponentModel.ISupportInitialize)(dgvResults)).BeginInit();

            this.tabControl1.SuspendLayout();
            //this.tabPage1.SuspendLayout();
            //this.tabPage2.SuspendLayout();

            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvResults.AutoGenerateColumns = false;
            dgvResults.BackgroundColor = System.Drawing.Color.White;
            dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvResults.ColumnHeadersHeight = 22;
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

            // 
            // FileViewBindingSource
            // 
            // this.FileViewBindingSource.DataSource = typeof(System.Windows.Forms.Samples.FileView);
            // 

            //&&&&&&&&&&&&&&&


            //mySplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(mySplitContainer_SplitterMoved);
            //mySplitContainer.MouseEnter += new System.EventHandler(mySplitContainer_MouseEnter);
            //mySplitContainer.MouseLeave += new System.EventHandler(mySplitContainer_MouseLeave);

            dgvResults.Height = Screen.PrimaryScreen.WorkingArea.Size.Height - 150;
            tabControl1.TabPages[_CurrentIndex].Controls.Add(dgvResults);

            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
            }

           
        }

        private void dgvResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                ContextMenuStrip m =  this.contextMenuStrip1;
               // MessageBox.Show(e.RowIndex.ToString());

                if (e.RowIndex > -1) {
                    dgvResults.Rows[e.RowIndex].Selected = true;
                    m.Show(dgvResults, new System.Drawing.Point(MousePosition.X - 20, MousePosition.Y - 175));
                }
         

            }
        }

        private void AddDataGridToTab(string pstrInitialDirectory) {
            tabControl1.TabPages.Insert(_CurrentIndex + 1, "** Search A Folder **");
            DataGridViewExt dgvResults = new DataGridViewExt("SearchResults");
            dgvResults.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvResults_CellMouseClick);
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

           

            ((System.ComponentModel.ISupportInitialize)(dgvResults)).BeginInit();            
  

            this.tabControl1.SuspendLayout();
            //this.tabPage1.SuspendLayout();
            //this.tabPage2.SuspendLayout();

            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvResults.AutoGenerateColumns = false;
            dgvResults.BackgroundColor = System.Drawing.Color.White;
            dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvResults.ColumnHeadersHeight = 22;
            

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

            // 
            // FileViewBindingSource
            // 
            // this.FileViewBindingSource.DataSource = typeof(System.Windows.Forms.Samples.FileView);
            // 

            //&&&&&&&&&&&&&&&


            //mySplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(mySplitContainer_SplitterMoved);
            //mySplitContainer.MouseEnter += new System.EventHandler(mySplitContainer_MouseEnter);
            //mySplitContainer.MouseLeave += new System.EventHandler(mySplitContainer_MouseLeave);

            dgvResults.Height = Screen.PrimaryScreen.WorkingArea.Size.Height - 150;

            foreach (DataGridViewColumn item in dgvResults.Columns) {
                item.ToolTipText = "Right-Click Column Header to add remove filter.\nUse menu item View to Show\\Hide Columns.\nLeft-Click column heading to sort.\nUse View\\Refresh to remove sort.";
            }

        }

        private void dgvResults_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Cut"));
                m.MenuItems.Add(new MenuItem("Copy"));
                m.MenuItems.Add(new MenuItem("Paste"));

                int currentMouseOverRow = dgvResults.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0) {
                    m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                }

                m.Show(dgvResults, new System.Drawing.Point(e.X, e.Y));

            }
        }

        private void dgvResults_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.Value == null) return;

            StringFormat sf = StringFormat.GenericTypographic;
            sf.FormatFlags = sf.FormatFlags | StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.DisplayFormatControl;
            e.PaintBackground(e.CellBounds, true);

            SolidBrush br = new SolidBrush(Color.White);
            if (((int)e.State & (int)DataGridViewElementStates.Selected) == 0)
                br.Color = Color.Black;

            string text = e.Value.ToString();
            SizeF textSize = e.Graphics.MeasureString(text, Font, e.CellBounds.Width, sf);

            int keyPos = text.IndexOf(strFindWhat, StringComparison.OrdinalIgnoreCase);
            if (keyPos >= 0) {
                SizeF textMetricSize = new SizeF(0, 0);
                if (keyPos >= 1) {
                    string textMetric = text.Substring(0, keyPos);
                    textMetricSize = e.Graphics.MeasureString(textMetric, Font, e.CellBounds.Width, sf);
                }

                SizeF keySize = e.Graphics.MeasureString(text.Substring(keyPos, strFindWhat.Length), Font, e.CellBounds.Width, sf);
                float left = e.CellBounds.Left + (keyPos <= 0 ? 0 : textMetricSize.Width) + 2;
                RectangleF keyRect = new RectangleF(left, e.CellBounds.Top + 1, keySize.Width, e.CellBounds.Height - 2);

                var fillBrush = new SolidBrush(Color.Orange);
                e.Graphics.FillRectangle(fillBrush, keyRect);
                fillBrush.Dispose();
            }
            e.Graphics.DrawString(text, Font, br, new PointF(e.CellBounds.Left + 2, e.CellBounds.Top + (e.CellBounds.Height - textSize.Height) / 2), sf);
            e.Handled = true;

            br.Dispose();
        }

        private void btnFolder_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions = new Methods();
            FileFolderDialog dialog = new FileFolderDialog();
         //   dialog.ShowDialog();
            dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");
            string str = "LastSearchFolder";


            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && (Directory.Exists(dialog.SelectedPath) || File.Exists(dialog.SelectedPath))) {
                cbxFolder.SelectedValue = dialog.SelectedPath;
                cbxFolder.Text = dialog.SelectedPath;
                myActions.SetValueByKey("LastSearchFolder", dialog.SelectedPath);
                string strFolder = dialog.SelectedPath;
                myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                _CurrentIndex = tabControl1.SelectedIndex;
                myActions.SetValueByKey("CurrentIndexSearch", _CurrentIndex.ToString());
                string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                string fileName = "cbxFolder.txt";
                string strApplicationBinDebug = System.Windows.Forms.Application.StartupPath;
                string myNewProjectSourcePath = strApplicationBinDebug.Replace("\\bin\\Debug", "");
                string settingsDirectory =
   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
                string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
                ArrayList alHosts = new ArrayList();
                List<ComboBoxPair> cbp = new List<ComboBoxPair>();
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));



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
                // add what they selected in select folder dialog
                alHostsNew.Add(myCbp);
                // if we have more than 24, remove the first one that is not select item
                if (alHostx.Count > 24) {
                    for (int i = alHostx.Count - 1; i > 0; i--) {
                        if (alHostx[i]._Key.Trim() != "--Select Item ---") {
                            alHostx.RemoveAt(i);
                            break;
                        }
                    }
                }
                // add all the items in the original dropdown that are not 
                // select item and are not the same as what was selected in
                // the select folder dialog
                foreach (ComboBoxPair item in alHostx) {
                    if (strNewHostName != item._Key && item._Key != "--Select Item ---") {
                        boolNewItem = true;
                        alHostsNew.Add(item);
                    }
                }
                // write updated dropdown list to Explorer/cbxFolder.txt
                try {
                    using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                        foreach (ComboBoxPair item in alHostsNew) {
                            if (item._Key != "") {
                                objSWFile.WriteLine(item._Key + '^' + item._Value);
                            }
                        }
                        objSWFile.Close();
                    }
                } catch (Exception ex) {

                    myActions.MessageBoxShow(ex.Message);
                }

            }
          
            myActions.SetValueByKey("cbxFolderSelectedValue", ((ComboBoxPair)(cbxFolder.SelectedItem))._Value);
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
            _CurrentIndex = tabControl1.SelectedIndex;
            myActions.SetValueByKey("CurrentIndexSearch", _CurrentIndex.ToString());
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].ToolTipText = "Click on this tab to search in " + ((ComboBoxPair)(cbxFolder.SelectedItem))._Value;
        }

        private void btnShowHideColumns_Click(object sender, EventArgs e) {
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(dgvResults);
            cs.MaxHeight = 100;
            cs.Width = 110;
            cs.ShowHideColumns();
        }

        private void btnNotepad_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string strFullFileName = "";
            string strLineNumber = "";
            
            if (dgvResults.SelectedRows.Count != 0) {
                DataGridViewRow row = this.dgvResults.SelectedRows[0];
                strFullFileName = row.Cells["FullName"].Value.ToString();
                strLineNumber = row.Cells["LineNumber"].Value.ToString();
               
            } else {
                myActions.MessageBoxShow("Please select a row that points to a file to open with Notepad++");
                return;
            }
    
            
            
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
            int intLineNumber = 0;
            int.TryParse(strLineNumber, out intLineNumber);
            if (intLineNumber > 0) {
                intLineNumber--;
            }
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

        private void btnVisualStudio_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string strFullName = "";
            string strLineNumber = "";
            
            if (dgvResults.SelectedRows.Count != 0) {
                DataGridViewRow row = this.dgvResults.SelectedRows[0];
                strFullName = row.Cells["FullName"].Value.ToString();
                strLineNumber = row.Cells["LineNumber"].Value.ToString();
               
            } else {
                myActions.MessageBoxShow("Please select a row that points to a file to open with Notepad++");
                return;
            }
            //List<string> myBeginDelim = new List<string>();
            //List<string> myEndDelim = new List<string>();
  
            int intLastSlash = strFullName.LastIndexOf('\\');
            if (intLastSlash < 1) {
                myActions.MessageBoxShow("Could not find last slash in filename - aborting");
                return;
            }
            string strPathOnly = strFullName.SubstringBetweenIndexes(0, intLastSlash);
            string strFileNameOnly = strFullName.Substring(intLastSlash + 1);
            List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
            

            //========
           
            string strSolutionFullFileName = "";
            bool boolSolutionFileFound = true;
            string strSolutionName = "";
            string currentTempName = strFullName;
            while (currentTempName.IndexOf("\\") > -1) {
                currentTempName = currentTempName.Substring(0, currentTempName.LastIndexOf("\\"));
                FileInfo fi = new FileInfo(currentTempName);
                if (Directory.Exists(currentTempName)) {
                    string[] files = null;
                    try {
                        files = System.IO.Directory.GetFiles(currentTempName, "*.sln");
                        if (files.Length > 0) {
                            // TODO: Currently defaulting to last one, but should ask the user which one to use if there is more than one                               
                            strSolutionFullFileName = files[files.Length - 1];
                            boolSolutionFileFound = true;
                            strSolutionName = strSolutionFullFileName.Substring(strSolutionFullFileName.LastIndexOf("\\") + 1).Replace(".sln", "");
                            myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
                            myWindowTitles.RemoveAll(vsItem => vsItem == "");
                            bool boolVSMatchingSolutionFound = false;
                            foreach (var vsTitle in myWindowTitles) {
                                if (vsTitle.StartsWith(strSolutionName + " (Debugging) - ") || vsTitle.StartsWith(strSolutionName.Replace(" ", ".sln ") + "(Debugging) - ")) {
                                    myActions.MessageBoxShow("Visual Studio is currently running in debug mode - aborting");
                                    return;
                                }
                                if (vsTitle.StartsWith(strSolutionName + " - ") || vsTitle.StartsWith(strSolutionName + " (Running) - ") || vsTitle.StartsWith(strSolutionName.Replace(" ", ".sln ") + " - ") || vsTitle.StartsWith(strSolutionName.Replace(" ", ".sln ") + " (Running) - ")) {
                                    boolVSMatchingSolutionFound = true;
                                    myActions.ActivateWindowByTitle(vsTitle, 3);
                                    myActions.Sleep(1000);
                                    myActions.TypeText("{ESCAPE}", 500);
                                     intLastSlash = strFullName.LastIndexOf('\\');
                                    if (intLastSlash < 1) {
                                        myActions.MessageBoxShow("Could not find last slash in filename - aborting");
                                        return;
                                    }
                                     strPathOnly = strFullName.SubstringBetweenIndexes(0, intLastSlash);
                                     strFileNameOnly = strFullName.Substring(intLastSlash + 1);

                                    myActions.TypeText("{ESC}", 2000);
                                    myActions.TypeText("%(f)", 1000);
                                    myActions.TypeText("a", 1000);
                                    myActions.TypeText("^(c)", 1000);

                                    myActions.TypeText("{ESC}", 1000);
                                    string currentFile = myActions.PutClipboardInEntity();
                                    if (strFullName != currentFile) {
                                        myActions.TypeText("%(f)", 1000);
                                        myActions.TypeText("{HOME}", 1000);
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
                                    }
                                    break;
                                }
                            }
                            if (boolVSMatchingSolutionFound == false) {
                                System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("I could not find the solution (" + strSolutionName + ") currently running.\n\r\n\r Do you want me to launch it in Visual Studio for you.\n\r\n\rTo go ahead and launch the solution, click yes, otherwise, click no to cancel");
                                if (myResult == System.Windows.Forms.DialogResult.No) {
                                    return;
                                }
                                string strVSPath = myActions.GetValueByKeyGlobal("VS2013Path");
                                // C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe
                                if (strVSPath == "") {
                                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                                    ControlEntity myControlEntity = new ControlEntity();
                                    myControlEntity.ControlEntitySetDefaults();
                                    myControlEntity.ControlType = ControlType.Heading;
                                    myControlEntity.Text = "Specify location of Visual Studio";
                                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                                    myControlEntity.ControlEntitySetDefaults();
                                    myControlEntity.ControlType = ControlType.Label;
                                    myControlEntity.ID = "myLabel";
                                    myControlEntity.Text = "Visual Studio Executable:";
                                    myControlEntity.RowNumber = 0;
                                    myControlEntity.ColumnNumber = 0;
                                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                                    myControlEntity.ControlEntitySetDefaults();
                                    myControlEntity.ControlType = ControlType.TextBox;
                                    myControlEntity.ID = "myAltExecutable";
                                    myControlEntity.Text = "";
                                    myControlEntity.RowNumber = 0;
                                    myControlEntity.ColumnNumber = 1;
                                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                                    myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
                                    string strAltExecutable = myListControlEntity.Find(x => x.ID == "myAltExecutable").Text;
                                    myActions.SetValueByKeyGlobal("VS2013Path", strAltExecutable);
                                    strVSPath = strAltExecutable;
                                }
                                myActions.Run(strVSPath, "\"" + strSolutionFullFileName + "\"");
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
                                         intLastSlash = strFullName.LastIndexOf('\\');
                                        if (intLastSlash < 1) {
                                            myActions.MessageBoxShow("Could not find last slash in filename - aborting");
                                            return;
                                        }
                                         strPathOnly = strFullName.SubstringBetweenIndexes(0, intLastSlash);
                                         strFileNameOnly = strFullName.Substring(intLastSlash + 1);

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
                    } catch (UnauthorizedAccessException ex) {

                        Console.WriteLine(ex.Message);
                        continue;
                    } catch (System.IO.DirectoryNotFoundException ex) {
                        Console.WriteLine(ex.Message);
                        continue;
                    } catch (System.IO.PathTooLongException ex) {
                        Console.WriteLine(ex.Message);
                        continue;
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
            }

            myActions.TypeText("^(g)", 500);
            myActions.TypeText(strLineNumber, 500);
            myActions.TypeText("{ENTER}", 500);
        }

        private void btnPeek_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            string strFullName = "";
            string strLineNumber = "";

            if (dgvResults.SelectedRows.Count != 0) {
                DataGridViewRow row = this.dgvResults.SelectedRows[0];
                strFullName = row.Cells["FullName"].Value.ToString();
                strLineNumber = row.Cells["LineNumber"].Value.ToString();

            } else {
                myActions.MessageBoxShow("Please select a row that points to a file to peek at 10 lines above and below current line");
                return;
            }
            Peek myPeek = new Peek(strFullName, strLineNumber, strFindWhat);
            myPeek.Show();
        }

        private void cbxFindWhat_SelectedIndexChanged(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("cbxFindWhatSelectedValue", ((ComboBoxPair)(cbxFindWhat.SelectedItem))._Value);

        }


        private void cbxFindWhat_Leave(object sender, EventArgs e) {

            string strNewHostName = ((ComboBox)sender).Text;
            Methods myActions = new Methods();
            System.Windows.Forms.DialogResult myResult;
            //if (!Directory.Exists(strNewHostName)) {

            //    myResult = myActions.MessageBoxShowWithYesNo("I could not find folder " + strNewHostName + ". Do you want me to create it ? ");
            //    if (myResult == System.Windows.Forms.DialogResult.Yes) {
            //        Directory.CreateDirectory(strNewHostName);
            //    } else {
            //        return;
            //    }

            //}
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
            strFindWhat = ((ComboBox)(sender)).Text;


            myActions.SetValueByKey("cbxFindWhatSelectedValue", strFindWhat);

        }
        private void cbxFileType_SelectedIndexChanged(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("cbxFileTypeSelectedValue", ((ComboBoxPair)(cbxFileType.SelectedItem))._Value);

        }


        private void cbxFileType_Leave(object sender, EventArgs e) {
            string strNewHostName = ((ComboBox)sender).Text;
            Methods myActions = new Methods();
            System.Windows.Forms.DialogResult myResult;
            //if (!Directory.Exists(strNewHostName)) {

            //    myResult = myActions.MessageBoxShowWithYesNo("I could not find folder " + strNewHostName + ". Do you want me to create it ? ");
            //    if (myResult == System.Windows.Forms.DialogResult.Yes) {
            //        Directory.CreateDirectory(strNewHostName);
            //    } else {
            //        return;
            //    }

            //}
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
            strFileType = ((ComboBox)(sender)).Text;


            myActions.SetValueByKey("cbxFileTypeSelectedValue", strFileType);
        }

        private void cbxExclude_SelectedIndexChanged(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("cbxExcludeSelectedValue", ((ComboBoxPair)(cbxExclude.SelectedItem))._Value);

        }


        private void cbxExclude_Leave(object sender, EventArgs e) {
            string strNewHostName = ((ComboBox)sender).Text;
            Methods myActions = new Methods();
            System.Windows.Forms.DialogResult myResult;
            //if (!Directory.Exists(strNewHostName)) {

            //    myResult = myActions.MessageBoxShowWithYesNo("I could not find folder " + strNewHostName + ". Do you want me to create it ? ");
            //    if (myResult == System.Windows.Forms.DialogResult.Yes) {
            //        Directory.CreateDirectory(strNewHostName);
            //    } else {
            //        return;
            //    }

            //}
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
            strExclude = ((ComboBox)(sender)).Text;


            myActions.SetValueByKey("cbxExcludeSelectedValue", strExclude);
        }

        private void cbxFolder_SelectedIndexChanged(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("cbxFolderSelectedValue", ((ComboBoxPair)(cbxFolder.SelectedItem))._Value);
            if (_ignoreFolderIndexChanged == true) {
                _ignoreFolderIndexChanged = false;
            } else {
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                _CurrentIndex = tabControl1.SelectedIndex;
                myActions.SetValueByKey("CurrentIndexSearch", _CurrentIndex.ToString());
            }
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].ToolTipText = "Click on this tab to search in " + ((ComboBoxPair)(cbxFolder.SelectedItem))._Value;
        }


        private void cbxFolder_Leave(object sender, EventArgs e) {            
            Methods myActions = new Methods();
            myActions.SetValueByKey("cbxFolderSelectedValue", cbxFolder.SelectedText);
            strFolder = cbxFolder.SelectedText;
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
            _CurrentIndex = tabControl1.SelectedIndex;
            myActions.SetValueByKey("CurrentIndexSearch", _CurrentIndex.ToString());
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].ToolTipText = "Click on this tab to search in " + cbxFolder.Text;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {

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

        private void btnOpenInIAE_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            int numOfTabs = myActions.GetValueByKeyAsInt("NumOfTabs");
            if (_CurrentIndex == numOfTabs - 1) {
                myActions.MessageBoxShow("You can only open files in Ideal Automate Explorer when you are not in the **Search A Folder** tab");
                return;
            }
            for (int index = Application.OpenForms.Count - 1; index >= 0; index--) {
                if (Application.OpenForms[index].Name == "ExplorerView") {
                    ExplorerView myExplorerView = (ExplorerView)Application.OpenForms[index];
                    myExplorerView.Show();
                    myExplorerView.BringToFront();
                    TabControl myTabControl = (TabControl)myExplorerView.Controls[1];
                    myTabControl.SelectedIndex = _CurrentIndex;       
                    
                    myActions.SetValueByKey("CurrentIndex", _CurrentIndex.ToString());
                    DataGridViewExt myDataGridView = (DataGridViewExt)myExplorerView.Controls[1].Controls[0].Controls[0].Controls[0].Controls[0];
                    string strFullName = "";
                    string strLineNumber = "";

                    if (this.dgvResults.SelectedRows.Count != 0) {
                        DataGridViewRow row = this.dgvResults.SelectedRows[0];
                        strFullName = row.Cells["FullName"].Value.ToString();
                        strLineNumber = row.Cells["LineNumber"].Value.ToString();

                    } else {
                        myActions.MessageBoxShow("Please select a row that points to a file to open with Ideal Automate Explorer");
                        return;
                    }
                     myDataGridView.ClearSelection();
                    MouseEventArgs myMouseEventArgs = new MouseEventArgs(MouseButtons.Left,1,0,0,0);
                    myExplorerView.TabControl1_MouseClick(sender, myMouseEventArgs);
                    int mySelectedRow = -1;

                    for (int i = 0; i < myExplorerView._CurrentDataGridView.Rows.Count; i++) {
                        DataGridViewRow item = myExplorerView._CurrentDataGridView.Rows[i];
                        if (item.Cells["FullName"].Value.ToString() == strFullName) {
                            mySelectedRow = i;
                            break;
                        }
                    }
                    myActions.SetValueByKey("InitialDirectory" + myTabControl.SelectedIndex.ToString() + "SelectedRow", mySelectedRow.ToString());
                    myExplorerView.RefreshDataGrid();

                  //  myExplorerView._CurrentDataGridView.Rows[mySelectedRow].Selected = true;
                    myActions.TypeText("^(f)", 1500);
                    myActions.TypeText(cbxFindWhat.Text, 500);
                    myActions.TypeText("{ENTER}", 500);



                    //myDataGridView.FirstDisplayedScrollingRowIndex = 18;
                    //myDataGridView.CurrentCell = myDataGridView.Rows[18].Cells[1];
                    //myDataGridView.Rows[18].Selected = true;
                    //myDataGridView.Refresh();
                }
            }


        }
        internal void Search_Click(object p1, object p2) {
            //interactiveToolTip1.Hide();
        }
    }
}
