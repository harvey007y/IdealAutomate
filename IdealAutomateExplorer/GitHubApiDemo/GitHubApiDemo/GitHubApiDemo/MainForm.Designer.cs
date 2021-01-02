namespace GitHubApiDemo
{
	partial class MainForm
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
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFindMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFindCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFindIssueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFindLabelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFindRepositoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFindUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectColumnsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDetailMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewFullDetailMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadAndRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.listTabPage = new System.Windows.Forms.TabPage();
            this.mainDataGridView = new System.Windows.Forms.DataGridView();
            this.mainDataGridViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataGridSelectColumnsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailTabPage = new System.Windows.Forms.TabPage();
            this.detailPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.detailContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.detailGetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            this.idealAutomatexCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.listTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).BeginInit();
            this.mainDataGridViewContextMenuStrip.SuspendLayout();
            this.detailTabPage.SuspendLayout();
            this.detailContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.viewMenuItem,
            this.helpMenuItem,
            this.downloadToolStripMenuItem,
            this.downloadAndRunToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mainMenuStrip.TabIndex = 0;
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileExitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "&File";
            // 
            // fileExitMenuItem
            // 
            this.fileExitMenuItem.Name = "fileExitMenuItem";
            this.fileExitMenuItem.Size = new System.Drawing.Size(93, 22);
            this.fileExitMenuItem.Text = "E&xit";
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editFindMenuItem,
            this.editSelectColumnsMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editMenuItem.Text = "&Edit";
            // 
            // editFindMenuItem
            // 
            this.editFindMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editFindCodeMenuItem,
            this.editFindIssueMenuItem,
            this.editFindLabelMenuItem,
            this.editFindRepositoryMenuItem,
            this.editFindUserMenuItem,
            this.idealAutomatexCodeToolStripMenuItem});
            this.editFindMenuItem.Name = "editFindMenuItem";
            this.editFindMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editFindMenuItem.Text = "&Find...";
            // 
            // editFindCodeMenuItem
            // 
            this.editFindCodeMenuItem.Enabled = false;
            this.editFindCodeMenuItem.Name = "editFindCodeMenuItem";
            this.editFindCodeMenuItem.Size = new System.Drawing.Size(189, 22);
            this.editFindCodeMenuItem.Text = "&Code";
            this.editFindCodeMenuItem.Click += new System.EventHandler(this.editFindCodeMenuItem_Click);
            // 
            // editFindIssueMenuItem
            // 
            this.editFindIssueMenuItem.Enabled = false;
            this.editFindIssueMenuItem.Name = "editFindIssueMenuItem";
            this.editFindIssueMenuItem.Size = new System.Drawing.Size(189, 22);
            this.editFindIssueMenuItem.Text = "&Issue";
            this.editFindIssueMenuItem.Click += new System.EventHandler(this.editFindIssueMenuItem_Click);
            // 
            // editFindLabelMenuItem
            // 
            this.editFindLabelMenuItem.Enabled = false;
            this.editFindLabelMenuItem.Name = "editFindLabelMenuItem";
            this.editFindLabelMenuItem.Size = new System.Drawing.Size(189, 22);
            this.editFindLabelMenuItem.Text = "&Label";
            this.editFindLabelMenuItem.Click += new System.EventHandler(this.editFindLabelMenuItem_Click);
            // 
            // editFindRepositoryMenuItem
            // 
            this.editFindRepositoryMenuItem.Enabled = false;
            this.editFindRepositoryMenuItem.Name = "editFindRepositoryMenuItem";
            this.editFindRepositoryMenuItem.Size = new System.Drawing.Size(189, 22);
            this.editFindRepositoryMenuItem.Text = "&Repository";
            this.editFindRepositoryMenuItem.Click += new System.EventHandler(this.editFindRepositoryMenuItem_Click);
            // 
            // editFindUserMenuItem
            // 
            this.editFindUserMenuItem.Enabled = false;
            this.editFindUserMenuItem.Name = "editFindUserMenuItem";
            this.editFindUserMenuItem.Size = new System.Drawing.Size(189, 22);
            this.editFindUserMenuItem.Text = "&User";
            this.editFindUserMenuItem.Click += new System.EventHandler(this.editFindUserMenuItem_Click);
            // 
            // editSelectColumnsMenuItem
            // 
            this.editSelectColumnsMenuItem.Enabled = false;
            this.editSelectColumnsMenuItem.Name = "editSelectColumnsMenuItem";
            this.editSelectColumnsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editSelectColumnsMenuItem.Text = "&Select Columns...";
            this.editSelectColumnsMenuItem.Click += new System.EventHandler(this.editSelectColumnsMenuItem_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDetailMenuItem,
            this.viewFullDetailMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewMenuItem.Text = "&View";
            // 
            // viewDetailMenuItem
            // 
            this.viewDetailMenuItem.Enabled = false;
            this.viewDetailMenuItem.Name = "viewDetailMenuItem";
            this.viewDetailMenuItem.Size = new System.Drawing.Size(135, 22);
            this.viewDetailMenuItem.Text = "&Detail";
            this.viewDetailMenuItem.Click += new System.EventHandler(this.viewDetailMenuItem_Click);
            // 
            // viewFullDetailMenuItem
            // 
            this.viewFullDetailMenuItem.Enabled = false;
            this.viewFullDetailMenuItem.Name = "viewFullDetailMenuItem";
            this.viewFullDetailMenuItem.Size = new System.Drawing.Size(135, 22);
            this.viewFullDetailMenuItem.Text = "&Full Detail...";
            this.viewFullDetailMenuItem.Click += new System.EventHandler(this.viewFullDetailMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpAboutMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpMenuItem.Text = "&Help";
            // 
            // helpAboutMenuItem
            // 
            this.helpAboutMenuItem.Name = "helpAboutMenuItem";
            this.helpAboutMenuItem.Size = new System.Drawing.Size(213, 22);
            this.helpAboutMenuItem.Text = "&About GitHub API Demo...";
            this.helpAboutMenuItem.Click += new System.EventHandler(this.helpAboutMenuItem_Click);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.downloadToolStripMenuItem.Text = "Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // downloadAndRunToolStripMenuItem
            // 
            this.downloadAndRunToolStripMenuItem.Name = "downloadAndRunToolStripMenuItem";
            this.downloadAndRunToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.downloadAndRunToolStripMenuItem.Text = "Download and Run";
            this.downloadAndRunToolStripMenuItem.Click += new System.EventHandler(this.downloadAndRunToolStripMenuItem_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Location = new System.Drawing.Point(0, 24);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(800, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Visible = false;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainStatusLabel,
            this.mainProgressBar});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 428);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(800, 22);
            this.mainStatusStrip.TabIndex = 2;
            // 
            // mainStatusLabel
            // 
            this.mainStatusLabel.Name = "mainStatusLabel";
            this.mainStatusLabel.Size = new System.Drawing.Size(683, 17);
            this.mainStatusLabel.Spring = true;
            this.mainStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.mainTabControl);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 24);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(800, 404);
            this.mainPanel.TabIndex = 3;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.listTabPage);
            this.mainTabControl.Controls.Add(this.detailTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Enabled = false;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(800, 404);
            this.mainTabControl.TabIndex = 3;
            // 
            // listTabPage
            // 
            this.listTabPage.Controls.Add(this.mainDataGridView);
            this.listTabPage.Location = new System.Drawing.Point(4, 22);
            this.listTabPage.Name = "listTabPage";
            this.listTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.listTabPage.Size = new System.Drawing.Size(792, 378);
            this.listTabPage.TabIndex = 0;
            this.listTabPage.Text = "List";
            this.listTabPage.UseVisualStyleBackColor = true;
            // 
            // mainDataGridView
            // 
            this.mainDataGridView.AllowUserToAddRows = false;
            this.mainDataGridView.AllowUserToDeleteRows = false;
            this.mainDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.mainDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.mainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mainDataGridView.ContextMenuStrip = this.mainDataGridViewContextMenuStrip;
            this.mainDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDataGridView.Location = new System.Drawing.Point(3, 3);
            this.mainDataGridView.Name = "mainDataGridView";
            this.mainDataGridView.ReadOnly = true;
            this.mainDataGridView.RowHeadersVisible = false;
            this.mainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mainDataGridView.Size = new System.Drawing.Size(786, 372);
            this.mainDataGridView.TabIndex = 2;
            this.mainDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.mainDataGridView_CellFormatting);
            this.mainDataGridView.SelectionChanged += new System.EventHandler(this.mainDataGridView_SelectionChanged);
            this.mainDataGridView.DoubleClick += new System.EventHandler(this.mainDataGridView_DoubleClick);
            // 
            // mainDataGridViewContextMenuStrip
            // 
            this.mainDataGridViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataGridSelectColumnsMenuItem});
            this.mainDataGridViewContextMenuStrip.Name = "mainDataGridViewContextMenuStrip";
            this.mainDataGridViewContextMenuStrip.Size = new System.Drawing.Size(163, 26);
            // 
            // dataGridSelectColumnsMenuItem
            // 
            this.dataGridSelectColumnsMenuItem.Name = "dataGridSelectColumnsMenuItem";
            this.dataGridSelectColumnsMenuItem.Size = new System.Drawing.Size(162, 22);
            this.dataGridSelectColumnsMenuItem.Text = "&Select Columns..";
            this.dataGridSelectColumnsMenuItem.Click += new System.EventHandler(this.dataGridSelectColumnsMenuItem_Click);
            // 
            // detailTabPage
            // 
            this.detailTabPage.Controls.Add(this.detailPropertyGrid);
            this.detailTabPage.Location = new System.Drawing.Point(4, 22);
            this.detailTabPage.Name = "detailTabPage";
            this.detailTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.detailTabPage.Size = new System.Drawing.Size(792, 378);
            this.detailTabPage.TabIndex = 1;
            this.detailTabPage.Text = "Detail";
            this.detailTabPage.UseVisualStyleBackColor = true;
            // 
            // detailPropertyGrid
            // 
            this.detailPropertyGrid.ContextMenuStrip = this.detailContextMenuStrip;
            this.detailPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.detailPropertyGrid.Name = "detailPropertyGrid";
            this.detailPropertyGrid.Size = new System.Drawing.Size(786, 372);
            this.detailPropertyGrid.TabIndex = 0;
            // 
            // detailContextMenuStrip
            // 
            this.detailContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailGetMenuItem});
            this.detailContextMenuStrip.Name = "detailContextMenuStrip";
            this.detailContextMenuStrip.Size = new System.Drawing.Size(148, 26);
            // 
            // detailGetMenuItem
            // 
            this.detailGetMenuItem.Name = "detailGetMenuItem";
            this.detailGetMenuItem.Size = new System.Drawing.Size(147, 22);
            this.detailGetMenuItem.Text = "&Get Full Detail";
            this.detailGetMenuItem.Click += new System.EventHandler(this.detailGetMenuItem_Click);
            // 
            // mainBackgroundWorker
            // 
            this.mainBackgroundWorker.WorkerReportsProgress = true;
            this.mainBackgroundWorker.WorkerSupportsCancellation = true;
            this.mainBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mainBackgroundWorker_DoWork);
            this.mainBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.mainBackgroundWorker_ProgressChanged);
            this.mainBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mainBackgroundWorker_RunWorkerCompleted);
            // 
            // progressTimer
            // 
            this.progressTimer.Tick += new System.EventHandler(this.progressTimer_Tick);
            // 
            // idealAutomatexCodeToolStripMenuItem
            // 
            this.idealAutomatexCodeToolStripMenuItem.Name = "idealAutomatexCodeToolStripMenuItem";
            this.idealAutomatexCodeToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.idealAutomatexCodeToolStripMenuItem.Text = "IdealAutomatex Code";
            this.idealAutomatexCodeToolStripMenuItem.Click += new System.EventHandler(this.idealAutomatexCodeToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "GitHub API Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainTabControl.ResumeLayout(false);
            this.listTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).EndInit();
            this.mainDataGridViewContextMenuStrip.ResumeLayout(false);
            this.detailTabPage.ResumeLayout(false);
            this.detailContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileExitMenuItem;
		private System.Windows.Forms.ToolStrip mainToolStrip;
		private System.Windows.Forms.StatusStrip mainStatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusLabel;
		private System.Windows.Forms.ToolStripProgressBar mainProgressBar;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.DataGridView mainDataGridView;
		private System.Windows.Forms.ToolStripMenuItem editMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editFindMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editFindCodeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editFindIssueMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editFindLabelMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editFindRepositoryMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editFindUserMenuItem;
		private System.ComponentModel.BackgroundWorker mainBackgroundWorker;
		private System.Windows.Forms.ContextMenuStrip mainDataGridViewContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem dataGridSelectColumnsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editSelectColumnsMenuItem;
		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage listTabPage;
		private System.Windows.Forms.TabPage detailTabPage;
		private System.Windows.Forms.PropertyGrid detailPropertyGrid;
		private System.Windows.Forms.Timer progressTimer;
		private System.Windows.Forms.ContextMenuStrip detailContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem detailGetMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewDetailMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewFullDetailMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpAboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadAndRunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem idealAutomatexCodeToolStripMenuItem;
    }
}

