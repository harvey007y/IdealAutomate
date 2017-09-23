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

#endregion

namespace System.Windows.Forms.Samples {
    partial class ExplorerView : Form {
        private DirectoryView _dir;
        string strInitialDirectory = "";
        bool boolStopEvent = false;
        Rectangle _IconRectangle = new Rectangle();
        List<HotKeyRecord> listHotKeyRecords = new List<HotKeyRecord>();
        Dictionary<string, VirtualKeyCode> dictVirtualKeyCodes = new Dictionary<string, VirtualKeyCode>();

        public ExplorerView() {
            InitializeComponent();
        }

        #region Helper Methods
        private void SetTitle(FileView fv) {
            // Clicked on the Name property, update the title
            this.Text = fv.Name;
            this.Icon = fv.Icon;
            string[] strInitialDirectoryArray = new string[1];
            strInitialDirectoryArray[0] = fv.FullName;
            Methods myActions = new Methods();
            myActions.SetValueByKey("InitialDirectory", fv.FullName);

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
            dataGridView1.ClearSelection();
            int intTotalSavingsForAllScripts = 0;
            Methods myActions = new Methods();
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory);
            this.FileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);

            // Set Size column to right align
            DataGridViewColumn col = this.dataGridView1.Columns["Size"];

            if (null != col) {
                DataGridViewCellStyle style = col.HeaderCell.Style;

                style.Padding = new Padding(style.Padding.Left, style.Padding.Top, 6, style.Padding.Bottom);
                style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Select first item.
            col = this.dataGridView1.Columns["Name"];

            if (null != col) {
                this.dataGridView1.Rows[0].Cells[col.Index].Selected = true;
            }
            AddGlobalHotKeys();
            foreach (DataGridViewRow item in dataGridView1.Rows) {
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
            lblTotalSavings.Text = formatted;
            myActions.SetValueByKey("ExpandCollapseAll", "");
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "SizeCol") {
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

            if (null != icon) {
                using (SolidBrush b = new SolidBrush(e.CellStyle.BackColor)) {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }
                _IconRectangle = e.CellBounds;
                // Draw right aligned icon (1 pixed padding)
                e.Graphics.DrawIcon(icon, e.CellBounds.Width - icon.Width - 1, e.CellBounds.Y + 1);
                e.Handled = true;
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();
      string scriptName = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Value.ToString();
      Methods myActions = new Methods();
            string categoryState = myActions.GetValueByKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToScriptPath(fileName) + "-" + scriptName);
            int categoryLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", myActions.ConvertFullFileNameToScriptPath(fileName) + "-" + scriptName);
            int indent = categoryLevel * 20;
            if (categoryState == "Collapsed" || categoryState == "Expanded") {
                ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(indent, 0, 0, 0);
            }
            if (categoryState == "Child") {
              //  ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic);
                ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(indent, 0, 0, 0);
                DataGridViewCell iconCell = ((DataGridView)sender).Rows[e.RowIndex].Cells[0];
            }

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            // Call Active on DirectoryView
            string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();
      Methods myActions = new Methods();
      fileName = myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileName);
            string categoryState = myActions.GetValueByKeyForNonCurrentScript("CategoryState", fileName);
            if (categoryState == "Expanded") {
                myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Collapsed", fileName);                
                RefreshDataGrid();
                return;
            }
            if (categoryState == "Collapsed") {
                myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Expanded", fileName);                
                RefreshDataGrid();
                return;
            }
            try {
                _dir.Activate(this.FileViewBindingSource[e.RowIndex] as FileView);
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
            if (DoActionRequired(sender)) {
                MessageBox.Show("List View");
            }
        }

        private void detailsMenuItem_Click(object sender, EventArgs e) {
            if (DoActionRequired(sender)) {
                MessageBox.Show("Details View");
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



        private void btnRun_Click(object sender, EventArgs e) {

            FileView myFileView;
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
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

        private void btnVisualStudio_Click(object sender, EventArgs e) {
            FileView myFileView;
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
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
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                FileView myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewProject = myFileView.FullName;
                    basePathName = myFileView.Name;
                }
            }
            string parentScriptPath = myActions.ConvertFullFileNameToScriptPath(basePathForNewProject) + "-" + basePathName;
            int parentScriptLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", parentScriptPath);
            if (basePathForNewProject != _dir.FileView.FullName) {
                parentScriptLevel++;
            }
            
            string myNewProjectName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewProjectDir = Path.Combine(basePathForNewProject, myNewProjectName);
            string scriptPathNewProject = myActions.ConvertFullFileNameToScriptPath(strNewProjectDir) + "-" + myNewProjectName;
            myActions.SetValueByKeyForNonCurrentScript("CategoryLevel", parentScriptLevel.ToString(), scriptPathNewProject);
            myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Child", scriptPathNewProject);
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
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.

                    ev_Delete_Directory(myFileView.FullName.ToString());
                    string scriptPath = myActions.ConvertFullFileNameToScriptPath(myFileView.FullName) + "-" + myFileView.Name;
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

        private void addHotKeyToolStripMenuItem_Click(object sender, EventArgs e) {
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
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
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
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory);
            this.FileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.FileViewBindingSource;
            //   this.dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            // AddGlobalHotKeys();
        }

        private void removeHotKeyToolStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            Methods myActions = new Methods();

            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
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
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
            }
            _dir = new DirectoryView(strInitialDirectory);
            this.FileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.FileViewBindingSource;
            //  this.dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            // AddGlobalHotKeys();
        }
        private void AddGlobalHotKeys() {
            Methods myActions = new Methods();
            ArrayList myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            ArrayList newArrayList = new ArrayList();           
            foreach (var item in myArrayList) {
                string[] myScriptInfoFields = item.ToString().Split('^');
                string scriptName = myScriptInfoFields[0];

                string strHotKey = myScriptInfoFields[1];
                if (strHotKey != "") {

                    string strHotKeyExecutable = myScriptInfoFields[5];
                    HotKeyRecord myHotKeyRecord = new HotKeyRecord();
                    myHotKeyRecord.HotKeys = strHotKey.Split('+');
                    myHotKeyRecord.Executable = strHotKeyExecutable;
                    myHotKeyRecord.ExecuteContent = "";
                    bool boolHotKeysGood = true;
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

                        RunWaitTillStart(myHotKeyRecord.Executable, myHotKeyRecord.ExecuteContent ?? "");
                    }                   
                }
            }            
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
                p.StartInfo.Arguments = string.Concat("", strContent, "");
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
                    Process.Start(strExecutable, string.Concat("", strContent, ""));
                } catch (Exception ex) {

                    MessageBox.Show(ex.ToString());
                }
            }

        }
        private Process GetProcByID(int id) {
            Process[] processlist = Process.GetProcesses();
            return processlist.FirstOrDefault(pr => pr.Id == id);
        }

        private void toolStripMenuItemManualTime_Click(object sender, EventArgs e) {
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
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
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
                myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Collapsed", myActions.ConvertFullFileNameToScriptPath(strNewCategoryDir) + "-" + myNewCategoryName);



            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            RefreshDataGrid();
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            RefreshDataGrid();
        }

        private void RefreshDataGrid() {
            // refresh datagridview
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            Methods myActions = new Methods();
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


            if (Directory.Exists(strSavedDirectory)) {
                strInitialDirectory = strSavedDirectory;
                myActions.SetValueByKey("InitialDirectory", strSavedDirectory);
            }
            _dir = new DirectoryView(strInitialDirectory);
            this.FileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.FileViewBindingSource;
            //   this.dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
        }

        private void btnExpanAll_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("ExpandCollapseAll", "Expand");
            RefreshDataGrid();
        }

        private void btnCollapseAll_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            myActions.SetValueByKey("ExpandCollapseAll", "Collapse");
            RefreshDataGrid();
            RefreshDataGrid();
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

            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                FileView myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                int categoryLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", myActions.ConvertFullFileNameToScriptPath(myFileView.FullName) + "-" + myFileView.Name);
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {                    
                     strNewSubCategoryDir = Path.Combine(myFileView.FullName, myNewSubCategoryName);
                    if (Directory.Exists(strNewSubCategoryDir)) {
                        myActions.MessageBoxShow(strNewSubCategoryDir + "already exists");
                        goto ReDisplayNewSubCategoryDialog;
                    }
                    try {
                        // create the directories
                        Directory.CreateDirectory(strNewSubCategoryDir);
                        myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Collapsed", myActions.ConvertFullFileNameToScriptPath(strNewSubCategoryDir) + "-" + myNewSubCategoryName);
                        int newCategoryLevel = categoryLevel + 1;
                        myActions.SetValueByKeyForNonCurrentScript("CategoryLevel", newCategoryLevel.ToString(), myActions.ConvertFullFileNameToScriptPath(strNewSubCategoryDir) + "-" + myNewSubCategoryName);
                    } catch (Exception ex) {
                        MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
                    }
                }
            }

            RefreshDataGrid();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                if (myCell.ColumnIndex == 0 && e.RowIndex > -1) {
                    // Call Active on DirectoryView
                    string fileName = ((DataGridView)sender).Rows[e.RowIndex].Cells[13].Value.ToString();
                    Methods myActions = new Methods();
                    string categoryState = myActions.GetValueByKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileName));
                    if (categoryState == "Expanded") {
                        myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Collapsed", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileName));
                        RefreshDataGrid();
                        return;
                    }
                    if (categoryState == "Collapsed") {
                        myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Expanded", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileName));
                        RefreshDataGrid();
                        return;
                    }
                    try {
                        _dir.Activate(this.FileViewBindingSource[e.RowIndex] as FileView);
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
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


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

            string strFolderToUse = "";
            if (strButtonPressed == "btnOkay") {
  
                if ((strFolder == "--Select Item ---" || strFolder == "")) {
                    myActions.MessageBoxShow("Please enter Folder or select Folder from ComboBox; else press Cancel to Exit");
                    goto DisplayFindTextInFilesWindow;
                }

                strFolderToUse = strFolder;
                myActions.SetValueByKey("InitialDirectory", strFolder);

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

        private void copyStripMenuItem4_Click(object sender, EventArgs e) {
            string fullFileName = "";
            string fileNamea = "";
            FileView myFileView;
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                if (myCell.ColumnIndex != 0 && myCell.RowIndex != 0) {
                    myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                    fullFileName = myFileView.FullName;
                    fileNamea = myFileView.Name;
                }

            }
            Methods myActions = new Methods();
            if (fullFileName == "") {
                myActions.MessageBoxShow("Please select a row to copy before selecting File/Copy");
                return;
            }
           
            strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string strSavedDirectory = myActions.GetValueByKey("InitialDirectory");


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
                Copy(fullFileName, strFolder);



                RefreshDataGrid();
                return;
            }

            if (strButtonPressed == "btnOkay") {
                strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 300, 1200, 100, 100);
                goto LineAfterDisplayCopyWindow;
            }
        }
        public static void Copy(string sourceDirectory, string targetDirectory) {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);

        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target) {
            Directory.CreateDirectory(target.FullName);

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
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                FileView myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                if (myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    basePathForNewFolder = myFileView.FullName;
                    basePathName = myFileView.Name;
                }
            }
            string parentScriptPath = myActions.ConvertFullFileNameToScriptPath(basePathForNewFolder) + "-" + basePathName;
            int parentScriptLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", parentScriptPath);
            if (basePathForNewFolder != _dir.FileView.FullName) {
                parentScriptLevel++;
            }
           
            string myNewFolderName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewFolderDir = Path.Combine(basePathForNewFolder, myNewFolderName);
            if (Directory.Exists(strNewFolderDir)) {
                myActions.MessageBoxShow(strNewFolderDir + "already exists");
                goto ReDisplayNewFolderDialog;
            }
            try {
                // create the directories
                string newFolderScriptPath = myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(basePathForNewFolder) + "-" + myNewFolderName;
                myActions.SetValueByKeyForNonCurrentScript("CategoryLevel", parentScriptLevel.ToString(), newFolderScriptPath);
                myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Child", newFolderScriptPath);
                Directory.CreateDirectory(strNewFolderDir);     
            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            RefreshDataGrid();
        }

        private void notepadToolStripMenuItem_Click(object sender, EventArgs e) {
            Methods myActions = new Methods();
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                FileView myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                if (!myFileView.IsDirectory && !(myCell.ColumnIndex == 0 && myCell.RowIndex == 0)) {
                    string strExecutable = @"C:\Windows\system32\notepad.exe";
                    myActions.Run(strExecutable, myFileView.FullName);
                    
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right) {
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected) {
                    c.DataGridView.ClearSelection();
                    c.DataGridView.CurrentCell = c;
                    c.Selected = true;
                }
            }
        
    }
    }

}



