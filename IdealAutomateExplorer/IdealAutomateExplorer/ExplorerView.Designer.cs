namespace System.Windows.Forms.Samples
{
    partial class ExplorerView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HotKeyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalExecutionsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuccessfulExecutionsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercentCorrectCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastExecutedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvgExecutionTimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ManualExecutionTimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSavingsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateModifiedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.backSplitButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.forwardSplitButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.upSplitButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.thumbnailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.categoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.createShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemManualTime = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.addHotKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHotKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tempToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sepToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnVisualStudio = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalSavings = new System.Windows.Forms.Label();
            this.btnCollapseAll = new System.Windows.Forms.Button();
            this.btnExpanAll = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileViewBindingSource)).BeginInit();
            this.toolBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 22;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewImageColumn1,
            this.NameCol,
            this.HotKeyCol,
            this.TotalExecutionsCol,
            this.SuccessfulExecutionsCol,
            this.PercentCorrectCol,
            this.LastExecutedCol,
            this.SizeCol,
            this.AvgExecutionTimeCol,
            this.ManualExecutionTimeCol,
            this.TotalSavingsCol,
            this.Type,
            this.DateModifiedCol,
            this.FullName});
            this.dataGridView1.DataSource = this.FileViewBindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 63);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(856, 294);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "Icon";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle3.NullValue")));
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewImageColumn1.Width = 20;
            // 
            // NameCol
            // 
            this.NameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameCol.DataPropertyName = "Name";
            this.NameCol.FillWeight = 200F;
            this.NameCol.HeaderText = "Name";
            this.NameCol.Name = "NameCol";
            // 
            // HotKeyCol
            // 
            this.HotKeyCol.DataPropertyName = "HotKey";
            this.HotKeyCol.HeaderText = "HotKey";
            this.HotKeyCol.Name = "HotKeyCol";
            this.HotKeyCol.ReadOnly = true;
            // 
            // TotalExecutionsCol
            // 
            this.TotalExecutionsCol.DataPropertyName = "TotalExecutions";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TotalExecutionsCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.TotalExecutionsCol.HeaderText = "Total Executions";
            this.TotalExecutionsCol.Name = "TotalExecutionsCol";
            this.TotalExecutionsCol.ReadOnly = true;
            // 
            // SuccessfulExecutionsCol
            // 
            this.SuccessfulExecutionsCol.DataPropertyName = "SuccessfulExecutions";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SuccessfulExecutionsCol.DefaultCellStyle = dataGridViewCellStyle5;
            this.SuccessfulExecutionsCol.HeaderText = "Successful Executions";
            this.SuccessfulExecutionsCol.Name = "SuccessfulExecutionsCol";
            this.SuccessfulExecutionsCol.ReadOnly = true;
            this.SuccessfulExecutionsCol.Width = 75;
            // 
            // PercentCorrectCol
            // 
            this.PercentCorrectCol.DataPropertyName = "PercentCorrect";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PercentCorrectCol.DefaultCellStyle = dataGridViewCellStyle6;
            this.PercentCorrectCol.HeaderText = "Percent Correct";
            this.PercentCorrectCol.Name = "PercentCorrectCol";
            this.PercentCorrectCol.ReadOnly = true;
            // 
            // LastExecutedCol
            // 
            this.LastExecutedCol.DataPropertyName = "LastExecuted";
            this.LastExecutedCol.HeaderText = "Last Executed";
            this.LastExecutedCol.Name = "LastExecutedCol";
            this.LastExecutedCol.ReadOnly = true;
            this.LastExecutedCol.Width = 125;
            // 
            // SizeCol
            // 
            this.SizeCol.DataPropertyName = "Size";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SizeCol.DefaultCellStyle = dataGridViewCellStyle7;
            this.SizeCol.HeaderText = "Size";
            this.SizeCol.Name = "SizeCol";
            this.SizeCol.ReadOnly = true;
            this.SizeCol.Width = 60;
            // 
            // AvgExecutionTimeCol
            // 
            this.AvgExecutionTimeCol.DataPropertyName = "AvgExecutionTime";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AvgExecutionTimeCol.DefaultCellStyle = dataGridViewCellStyle8;
            this.AvgExecutionTimeCol.HeaderText = "Avg Time Secs";
            this.AvgExecutionTimeCol.Name = "AvgExecutionTimeCol";
            this.AvgExecutionTimeCol.ReadOnly = true;
            // 
            // ManualExecutionTimeCol
            // 
            this.ManualExecutionTimeCol.DataPropertyName = "ManualExecutionTime";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ManualExecutionTimeCol.DefaultCellStyle = dataGridViewCellStyle9;
            this.ManualExecutionTimeCol.HeaderText = "Manual Time Secs";
            this.ManualExecutionTimeCol.Name = "ManualExecutionTimeCol";
            this.ManualExecutionTimeCol.ReadOnly = true;
            this.ManualExecutionTimeCol.Width = 75;
            // 
            // TotalSavingsCol
            // 
            this.TotalSavingsCol.DataPropertyName = "TotalSavings";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TotalSavingsCol.DefaultCellStyle = dataGridViewCellStyle10;
            this.TotalSavingsCol.HeaderText = "TotalSavings";
            this.TotalSavingsCol.Name = "TotalSavingsCol";
            this.TotalSavingsCol.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // DateModifiedCol
            // 
            this.DateModifiedCol.DataPropertyName = "DateModified";
            this.DateModifiedCol.HeaderText = "Date Modified";
            this.DateModifiedCol.Name = "DateModifiedCol";
            this.DateModifiedCol.ReadOnly = true;
            this.DateModifiedCol.Width = 150;
            // 
            // FullName
            // 
            this.FullName.DataPropertyName = "FullName";
            this.FullName.HeaderText = "FullName";
            this.FullName.Name = "FullName";
            this.FullName.ReadOnly = true;
            this.FullName.Visible = false;
            // 
            // FileViewBindingSource
            // 
            this.FileViewBindingSource.DataSource = typeof(System.Windows.Forms.Samples.FileView);
            // 
            // toolBar
            // 
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backSplitButton,
            this.forwardSplitButton,
            this.upSplitButton,
            this.toolStripSeparator1,
            this.viewSplitButton});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar.Size = new System.Drawing.Size(856, 39);
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "toolStrip1";
            // 
            // backSplitButton
            // 
            this.backSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("backSplitButton.Image")));
            this.backSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.backSplitButton.Name = "backSplitButton";
            this.backSplitButton.Size = new System.Drawing.Size(74, 36);
            this.backSplitButton.Text = " Back";
            this.backSplitButton.Click += new System.EventHandler(this.backSplitButton_Click);
            // 
            // forwardSplitButton
            // 
            this.forwardSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.forwardSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("forwardSplitButton.Image")));
            this.forwardSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.forwardSplitButton.Name = "forwardSplitButton";
            this.forwardSplitButton.Size = new System.Drawing.Size(39, 36);
            this.forwardSplitButton.ToolTipText = "Forward";
            // 
            // upSplitButton
            // 
            this.upSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("upSplitButton.Image")));
            this.upSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.upSplitButton.Name = "upSplitButton";
            this.upSplitButton.Size = new System.Drawing.Size(36, 36);
            this.upSplitButton.ToolTipText = "Up";
            this.upSplitButton.Click += new System.EventHandler(this.upSplitButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // viewSplitButton
            // 
            this.viewSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.viewSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thumbnailsMenuItem,
            this.tilesMenuItem,
            this.iconsMenuItem,
            this.listMenuItem,
            this.detailsMenuItem});
            this.viewSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("viewSplitButton.Image")));
            this.viewSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.viewSplitButton.Name = "viewSplitButton";
            this.viewSplitButton.Size = new System.Drawing.Size(48, 36);
            // 
            // thumbnailsMenuItem
            // 
            this.thumbnailsMenuItem.CheckOnClick = true;
            this.thumbnailsMenuItem.Name = "thumbnailsMenuItem";
            this.thumbnailsMenuItem.Size = new System.Drawing.Size(137, 22);
            this.thumbnailsMenuItem.Text = "Thumbnails";
            this.thumbnailsMenuItem.Click += new System.EventHandler(this.thumbnailsMenuItem_Click);
            // 
            // tilesMenuItem
            // 
            this.tilesMenuItem.CheckOnClick = true;
            this.tilesMenuItem.Name = "tilesMenuItem";
            this.tilesMenuItem.Size = new System.Drawing.Size(137, 22);
            this.tilesMenuItem.Text = "Tiles";
            this.tilesMenuItem.Click += new System.EventHandler(this.tilesMenuItem_Click);
            // 
            // iconsMenuItem
            // 
            this.iconsMenuItem.CheckOnClick = true;
            this.iconsMenuItem.Name = "iconsMenuItem";
            this.iconsMenuItem.Size = new System.Drawing.Size(137, 22);
            this.iconsMenuItem.Text = "Icons";
            this.iconsMenuItem.Click += new System.EventHandler(this.iconsMenuItem_Click);
            // 
            // listMenuItem
            // 
            this.listMenuItem.CheckOnClick = true;
            this.listMenuItem.Name = "listMenuItem";
            this.listMenuItem.Size = new System.Drawing.Size(137, 22);
            this.listMenuItem.Text = "List";
            this.listMenuItem.Click += new System.EventHandler(this.listMenuItem_Click);
            // 
            // detailsMenuItem
            // 
            this.detailsMenuItem.Checked = true;
            this.detailsMenuItem.CheckOnClick = true;
            this.detailsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.detailsMenuItem.Name = "detailsMenuItem";
            this.detailsMenuItem.Size = new System.Drawing.Size(137, 22);
            this.detailsMenuItem.Text = "Details";
            this.detailsMenuItem.Click += new System.EventHandler(this.detailsMenuItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.favoritesToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenu.Size = new System.Drawing.Size(856, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripMenuItem1,
            this.createShortcutToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItemManualTime,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.copyStripMenuItem4,
            this.renameToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.toolStripSeparator2,
            this.propertiesToolStripMenuItem1,
            this.sepToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.categoryToolStripMenuItem,
            this.projectToolStripMenuItem,
            this.subCategoryToolStripMenuItem,
            this.folderToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // categoryToolStripMenuItem
            // 
            this.categoryToolStripMenuItem.Name = "categoryToolStripMenuItem";
            this.categoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.categoryToolStripMenuItem.Text = "Category";
            this.categoryToolStripMenuItem.Click += new System.EventHandler(this.categoryToolStripMenuItem_Click);
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.projectToolStripMenuItem.Text = "Project";
            this.projectToolStripMenuItem.Click += new System.EventHandler(this.projectToolStripMenuItem_Click);
            // 
            // subCategoryToolStripMenuItem
            // 
            this.subCategoryToolStripMenuItem.Name = "subCategoryToolStripMenuItem";
            this.subCategoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.subCategoryToolStripMenuItem.Text = "SubCategory";
            this.subCategoryToolStripMenuItem.Click += new System.EventHandler(this.subCategoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // createShortcutToolStripMenuItem
            // 
            this.createShortcutToolStripMenuItem.Name = "createShortcutToolStripMenuItem";
            this.createShortcutToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.createShortcutToolStripMenuItem.Text = "Create Shortcut";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItemManualTime
            // 
            this.toolStripMenuItemManualTime.Name = "toolStripMenuItemManualTime";
            this.toolStripMenuItemManualTime.Size = new System.Drawing.Size(156, 22);
            this.toolStripMenuItemManualTime.Text = "Manual Time";
            this.toolStripMenuItemManualTime.Click += new System.EventHandler(this.toolStripMenuItemManualTime_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHotKeyToolStripMenuItem,
            this.removeHotKeyToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(156, 22);
            this.toolStripMenuItem2.Text = "HotKeys";
            // 
            // addHotKeyToolStripMenuItem
            // 
            this.addHotKeyToolStripMenuItem.Name = "addHotKeyToolStripMenuItem";
            this.addHotKeyToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addHotKeyToolStripMenuItem.Text = "Add HotKey";
            this.addHotKeyToolStripMenuItem.Click += new System.EventHandler(this.addHotKeyToolStripMenuItem_Click);
            // 
            // removeHotKeyToolStripMenuItem
            // 
            this.removeHotKeyToolStripMenuItem.Name = "removeHotKeyToolStripMenuItem";
            this.removeHotKeyToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.removeHotKeyToolStripMenuItem.Text = "Remove HotKey";
            this.removeHotKeyToolStripMenuItem.Click += new System.EventHandler(this.removeHotKeyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(156, 22);
            this.toolStripMenuItem3.Text = "Open";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.openStripMenuItem3_Click);
            // 
            // copyStripMenuItem4
            // 
            this.copyStripMenuItem4.Name = "copyStripMenuItem4";
            this.copyStripMenuItem4.Size = new System.Drawing.Size(156, 22);
            this.copyStripMenuItem4.Text = "Copy";
            this.copyStripMenuItem4.Click += new System.EventHandler(this.copyStripMenuItem4_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
            // 
            // propertiesToolStripMenuItem1
            // 
            this.propertiesToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tempToolStripMenuItem1});
            this.propertiesToolStripMenuItem1.Name = "propertiesToolStripMenuItem1";
            this.propertiesToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.propertiesToolStripMenuItem1.Text = "Properties";
            // 
            // tempToolStripMenuItem1
            // 
            this.tempToolStripMenuItem1.Name = "tempToolStripMenuItem1";
            this.tempToolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.tempToolStripMenuItem1.Text = "Temp";
            // 
            // sepToolStripMenuItem
            // 
            this.sepToolStripMenuItem.Name = "sepToolStripMenuItem";
            this.sepToolStripMenuItem.Size = new System.Drawing.Size(153, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // favoritesToolStripMenuItem
            // 
            this.favoritesToolStripMenuItem.Name = "favoritesToolStripMenuItem";
            this.favoritesToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.favoritesToolStripMenuItem.Text = "Favorites";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(224, 34);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnVisualStudio
            // 
            this.btnVisualStudio.Location = new System.Drawing.Point(305, 34);
            this.btnVisualStudio.Name = "btnVisualStudio";
            this.btnVisualStudio.Size = new System.Drawing.Size(81, 23);
            this.btnVisualStudio.TabIndex = 2;
            this.btnVisualStudio.Text = "Visual Studio";
            this.btnVisualStudio.UseVisualStyleBackColor = true;
            this.btnVisualStudio.Click += new System.EventHandler(this.btnVisualStudio_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(728, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Savings:";
            // 
            // lblTotalSavings
            // 
            this.lblTotalSavings.AutoSize = true;
            this.lblTotalSavings.Location = new System.Drawing.Point(809, 39);
            this.lblTotalSavings.Name = "lblTotalSavings";
            this.lblTotalSavings.Size = new System.Drawing.Size(35, 13);
            this.lblTotalSavings.TabIndex = 4;
            this.lblTotalSavings.Text = "label2";
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.Location = new System.Drawing.Point(566, 34);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(81, 23);
            this.btnCollapseAll.TabIndex = 5;
            this.btnCollapseAll.Text = "Collapse All";
            this.btnCollapseAll.UseVisualStyleBackColor = true;
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            // 
            // btnExpanAll
            // 
            this.btnExpanAll.Location = new System.Drawing.Point(479, 34);
            this.btnExpanAll.Name = "btnExpanAll";
            this.btnExpanAll.Size = new System.Drawing.Size(81, 23);
            this.btnExpanAll.TabIndex = 6;
            this.btnExpanAll.Text = "Expand All";
            this.btnExpanAll.UseVisualStyleBackColor = true;
            this.btnExpanAll.Click += new System.EventHandler(this.btnExpanAll_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(392, 34);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(81, 23);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.folderToolStripMenuItem.Text = "Folder";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.folderToolStripMenuItem_Click);
            // 
            // ExplorerView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(856, 357);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnExpanAll);
            this.Controls.Add(this.btnCollapseAll);
            this.Controls.Add(this.lblTotalSavings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnVisualStudio);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Name = "ExplorerView";
            this.Text = "[File Name]";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ExplorerView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileViewBindingSource)).EndInit();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource FileViewBindingSource;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripDropDownButton backSplitButton;
        private System.Windows.Forms.ToolStripDropDownButton forwardSplitButton;
        private System.Windows.Forms.ToolStripButton upSplitButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton viewSplitButton;
        private System.Windows.Forms.ToolStripMenuItem thumbnailsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tilesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iconsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsMenuItem;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem favoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createShortcutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tempToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator sepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private Button btnRun;
        private Button btnVisualStudio;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem addHotKeyToolStripMenuItem;
        private ToolStripMenuItem removeHotKeyToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemManualTime;
        private Label label1;
        private Label lblTotalSavings;
        private ToolStripMenuItem categoryToolStripMenuItem;
        private Button btnCollapseAll;
        private Button btnExpanAll;
        private Button btnRefresh;
        private ToolStripMenuItem subCategoryToolStripMenuItem;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn HotKeyCol;
        private DataGridViewTextBoxColumn TotalExecutionsCol;
        private DataGridViewTextBoxColumn SuccessfulExecutionsCol;
        private DataGridViewTextBoxColumn PercentCorrectCol;
        private DataGridViewTextBoxColumn LastExecutedCol;
        private DataGridViewTextBoxColumn SizeCol;
        private DataGridViewTextBoxColumn AvgExecutionTimeCol;
        private DataGridViewTextBoxColumn ManualExecutionTimeCol;
        private DataGridViewTextBoxColumn TotalSavingsCol;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn DateModifiedCol;
        private ToolStripMenuItem toolStripMenuItem3;
        private DataGridViewTextBoxColumn FullName;
        private ToolStripMenuItem copyStripMenuItem4;
        private ToolStripMenuItem folderToolStripMenuItem;
    }
}

