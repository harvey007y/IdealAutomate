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

#endregion

namespace System.Windows.Forms.Samples
{
    partial class ExplorerView : Form
    {
        private DirectoryView _dir;


        public ExplorerView()
        {
            InitializeComponent();
        }

        #region Helper Methods
        private void SetTitle(FileView fv)
        {
            // Clicked on the Name property, update the title
            this.Text = fv.Name;
            this.Icon = fv.Icon;
            string[] strInitialDirectoryArray = new string[1];
            strInitialDirectoryArray[0] = fv.FullName;

            File.WriteAllLines(Path.Combine(Application.StartupPath, @"Text\InitialDirectory.txt"), strInitialDirectoryArray);
        }

        private void toolStripMenuItem4_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (sender as ToolStripMenuItem);

            foreach (ToolStripMenuItem child in viewSplitButton.DropDownItems)
            {
                if (child != item)
                {
                    child.Checked = false;
                }
                else
                {
                    item.Checked = true;
                }
            }
        }

        // Clear the one of many list
        private void ClearItems(ToolStripMenuItem selected)
        {
            // Clear items
            foreach (ToolStripMenuItem child in viewSplitButton.DropDownItems)
            {
                if (child != selected)
                {
                    child.Checked = false;
                }
            }
        }

        private bool DoActionRequired(object sender)
        {
            ToolStripMenuItem item = (sender as ToolStripMenuItem);
            bool doAction;

            // Clear other items
            ClearItems(item);

            // Check this one
            if (!item.Checked)
            {
                item.Checked = true;
                doAction = false;
            }
            else
            {
                // Item click and wasn't previously checked - Do action
                doAction = true;
            }

            return doAction;
        }
        #endregion

        #region Event Handlers
        private void ExplorerView_Load(object sender, EventArgs e)
        {
            string strInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set Initial Directory to My Documents
            string[] strIntialDirectoryArray = File.ReadAllLines(Path.Combine(Application.StartupPath, @"Text\InitialDirectory.txt"));

            if (strIntialDirectoryArray[0].Trim() != "Use Default")
            {
                strInitialDirectory = strIntialDirectoryArray[0].Trim();
            }
            _dir = new DirectoryView(strInitialDirectory);
            this.FileViewBindingSource.DataSource = _dir;

            // Set the title
            SetTitle(_dir.FileView);

            // Set Size column to right align
            DataGridViewColumn col = this.dataGridView1.Columns["Size"];

            if (null != col)
            {
                DataGridViewCellStyle style = col.HeaderCell.Style;

                style.Padding = new Padding(style.Padding.Left, style.Padding.Top, 6, style.Padding.Bottom);
                style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Select first item
            col = this.dataGridView1.Columns["Name"];

            if (null != col)
            {
                this.dataGridView1.Rows[0].Cells[col.Index].Selected = true;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "SizeCol")
            {
                long size = (long)e.Value;

                if (size < 0)
                {
                    e.Value = "";
                }
                else
                {
                    size = ((size + 999) / 1000);
                    e.Value = size.ToString() + " " + "KB";
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            Icon icon = (e.Value as Icon);

            if (null != icon)
            {
                using (SolidBrush b = new SolidBrush(e.CellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }

                // Draw right aligned icon (1 pixed padding)
                e.Graphics.DrawIcon(icon, e.CellBounds.Width - icon.Width - 1, e.CellBounds.Y + 1);
                e.Handled = true;
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Call Active on DirectoryView
            try
            {
                _dir.Activate(this.FileViewBindingSource[e.RowIndex] as FileView);
                SetTitle(_dir.FileView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void thumbnailsMenuItem_Click(object sender, EventArgs e)
        {
            if (DoActionRequired(sender))
            {
                MessageBox.Show("Thumbnails View");
            }
        }

        private void tilesMenuItem_Click(object sender, EventArgs e)
        {
            if (DoActionRequired(sender))
            {
                MessageBox.Show("Tiles View");
            }
        }

        private void iconsMenuItem_Click(object sender, EventArgs e)
        {
            if (DoActionRequired(sender))
            {
                MessageBox.Show("Icons View");
            }
        }

        private void listMenuItem_Click(object sender, EventArgs e)
        {
            if (DoActionRequired(sender))
            {
                MessageBox.Show("List View");
            }
        }

        private void detailsMenuItem_Click(object sender, EventArgs e)
        {
            if (DoActionRequired(sender))
            {
                MessageBox.Show("Details View");
            }
        }

        void Renderer_RenderToolStripBorder(object sender, ToolStripRenderEventArgs e)
        {
            e.Graphics.DrawLine(SystemPens.ButtonShadow, 0, 1, toolBar.Width, 1);
            e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 2, toolBar.Width, 2);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void upSplitButton_Click(object sender, EventArgs e)
        {
            _dir.Up();
            SetTitle(_dir.FileView);
        }

        private void backSplitButton_Click(object sender, EventArgs e)
        {
            _dir.Up();
            SetTitle(_dir.FileView);
        }
        #endregion

        private void btnTraceOn_Click(object sender, EventArgs e)
        {

        }
        private void ev_Process_File(string myFile)
        {           
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
                Directory.Delete(myFile,true);
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
                        ev_Process_File(file);
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

            string myNewProjectName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strNewProjectDir = Path.Combine(_dir.FileView.FullName, myNewProjectName);
            if (Directory.Exists(strNewProjectDir)) {
                myActions.MessageBoxShow(strNewProjectDir + "already exists");
                goto ReDisplayNewProjectDialog;
            }
            try {
                // create the directories
                Directory.CreateDirectory(strNewProjectDir);
                string binDir = Path.Combine(strNewProjectDir, "bin");
                Directory.CreateDirectory(binDir);
                string strDebug = Path.Combine(binDir, "Debug");
                Directory.CreateDirectory(strDebug);
                string strImages = Path.Combine(strNewProjectDir, "Images");
                Directory.CreateDirectory(strImages);
                string strObj = Path.Combine(strNewProjectDir, "obj");
                Directory.CreateDirectory(strObj);
                string strX86 = Path.Combine(strObj, "x86");
                Directory.CreateDirectory(strX86);
                string strObjDebug = Path.Combine(strX86, "Debug");
                Directory.CreateDirectory(strObjDebug);
                string strObjDebugTempPE = Path.Combine(strObjDebug, "TempPE");
                Directory.CreateDirectory(strObjDebugTempPE);
                string strProperties = Path.Combine(strNewProjectDir, "Properties");
                Directory.CreateDirectory(strProperties);

            } catch (Exception ex) {

                MessageBox.Show("Exception Message: " + ex.Message + " InnerException: " + ex.InnerException);
            }
            
            
            // TODO: Check if new project directory exists 
            // if so display a message, and redisplay dialog

            // TODO: Copy myNewProject files and replace myNewProject with
            // myNewProjectName
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            FileView myFileView;
            foreach (DataGridViewCell myCell in dataGridView1.SelectedCells) {
                myFileView = (FileView)this.FileViewBindingSource[myCell.RowIndex];
                //MessageBox.Show(myFileView.FullName.ToString());
                if (myFileView.IsDirectory) {
                    // Call EnumerateFiles in a foreach-loop.
                    
                        ev_Delete_Directory(myFileView.FullName.ToString());                 

                } else {
                    ev_Delete_File(myFileView.FullName.ToString());
                }

            }
        }
    }

}



