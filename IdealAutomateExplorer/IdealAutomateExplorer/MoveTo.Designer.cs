namespace System.Windows.Forms.Samples
{
    partial class MoveTo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxFolder = new System.Windows.Forms.ComboBox();
            this.btnFolder = new System.Windows.Forms.Button();
            this.lblResults = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.FileViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnMoveTo = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openWithToolStripMenuItemOpenWith = new System.Windows.Forms.ToolStripMenuItem();
            this.idealAutomateExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.peekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileViewBindingSource)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Folder";
            // 
            // cbxFolder
            // 
            this.cbxFolder.DropDownWidth = 600;
            this.cbxFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxFolder.FormattingEnabled = true;
            this.cbxFolder.Location = new System.Drawing.Point(75, 77);
            this.cbxFolder.Name = "cbxFolder";
            this.cbxFolder.Size = new System.Drawing.Size(686, 23);
            this.cbxFolder.TabIndex = 7;
            this.cbxFolder.SelectedIndexChanged += new System.EventHandler(this.cbxFolder_SelectedIndexChanged);
            this.cbxFolder.Leave += new System.EventHandler(this.cbxFolder_Leave);
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(767, 76);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(208, 23);
            this.btnFolder.TabIndex = 8;
            this.btnFolder.Text = "Select Folder for MoveTo A Folder Tab";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(767, 7);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(44, 13);
            this.lblResults.TabIndex = 11;
            this.lblResults.Text = "Count 0";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(17, 132);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(1166, 295);
            this.tabControl1.TabIndex = 19;
            this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabControl1_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1158, 269);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(428, 103);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(235, 23);
            this.progressBar1.TabIndex = 20;
            this.progressBar1.Tag = "";
            this.progressBar1.UseWaitCursor = true;
            // 
            // percentageLabel
            // 
            this.percentageLabel.AutoSize = true;
            this.percentageLabel.Location = new System.Drawing.Point(669, 107);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(466, 13);
            this.percentageLabel.TabIndex = 21;
            this.percentageLabel.Text = "Instructions: 1. Select tab to move file to or select folder to move file to - th" +
    "en click move to button";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(327, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // btnMoveTo
            // 
            this.btnMoveTo.Location = new System.Drawing.Point(233, 103);
            this.btnMoveTo.Name = "btnMoveTo";
            this.btnMoveTo.Size = new System.Drawing.Size(75, 23);
            this.btnMoveTo.TabIndex = 23;
            this.btnMoveTo.Text = "MoveTo";
            this.btnMoveTo.UseVisualStyleBackColor = true;
            this.btnMoveTo.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWithToolStripMenuItemOpenWith,
            this.peekToolStripMenuItem,
            this.showHideColumnsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 70);
            // 
            // openWithToolStripMenuItemOpenWith
            // 
            this.openWithToolStripMenuItemOpenWith.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.idealAutomateExplorerToolStripMenuItem,
            this.notepadToolStripMenuItem,
            this.visualStudioToolStripMenuItem});
            this.openWithToolStripMenuItemOpenWith.Name = "openWithToolStripMenuItemOpenWith";
            this.openWithToolStripMenuItemOpenWith.Size = new System.Drawing.Size(184, 22);
            this.openWithToolStripMenuItemOpenWith.Text = "Open With...";
            // 
            // idealAutomateExplorerToolStripMenuItem
            // 
            this.idealAutomateExplorerToolStripMenuItem.Name = "idealAutomateExplorerToolStripMenuItem";
            this.idealAutomateExplorerToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.idealAutomateExplorerToolStripMenuItem.Text = "Ideal Automate Explorer";
            this.idealAutomateExplorerToolStripMenuItem.Click += new System.EventHandler(this.btnOpenInIAE_Click);
            // 
            // notepadToolStripMenuItem
            // 
            this.notepadToolStripMenuItem.Name = "notepadToolStripMenuItem";
            this.notepadToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.notepadToolStripMenuItem.Text = "Notepad++";
            this.notepadToolStripMenuItem.Click += new System.EventHandler(this.btnNotepad_Click);
            // 
            // visualStudioToolStripMenuItem
            // 
            this.visualStudioToolStripMenuItem.Name = "visualStudioToolStripMenuItem";
            this.visualStudioToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.visualStudioToolStripMenuItem.Text = "Visual Studio";
            this.visualStudioToolStripMenuItem.Click += new System.EventHandler(this.btnVisualStudio_Click);
            // 
            // peekToolStripMenuItem
            // 
            this.peekToolStripMenuItem.Name = "peekToolStripMenuItem";
            this.peekToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.peekToolStripMenuItem.Text = "Peek";
            this.peekToolStripMenuItem.Click += new System.EventHandler(this.btnPeek_Click);
            // 
            // showHideColumnsToolStripMenuItem
            // 
            this.showHideColumnsToolStripMenuItem.Name = "showHideColumnsToolStripMenuItem";
            this.showHideColumnsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.showHideColumnsToolStripMenuItem.Text = "Show/Hide Columns";
            this.showHideColumnsToolStripMenuItem.Click += new System.EventHandler(this.btnShowHideColumns_Click);
            // 
            // MoveTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 450);
            this.Controls.Add(this.btnMoveTo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.cbxFolder);
            this.Controls.Add(this.label4);
            this.Name = "MoveTo";
            this.Text = "MoveTo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MoveTo_Load);
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FileViewBindingSource)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label4;
        private ComboBox cbxFolder;
        private Button btnFolder;
        private Label lblResults;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private BindingSource FileViewBindingSource;
        private ProgressBar progressBar1;
        private Label percentageLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btnCancel;
        private Button btnMoveTo;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openWithToolStripMenuItemOpenWith;
        private ToolStripMenuItem idealAutomateExplorerToolStripMenuItem;
        private ToolStripMenuItem notepadToolStripMenuItem;
        private ToolStripMenuItem visualStudioToolStripMenuItem;
        private ToolStripMenuItem peekToolStripMenuItem;
        private ToolStripMenuItem showHideColumnsToolStripMenuItem;
    }
}