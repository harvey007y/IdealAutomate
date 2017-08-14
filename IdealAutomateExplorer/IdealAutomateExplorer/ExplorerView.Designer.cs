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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerView));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
      this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.HotKeyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TotalExecutionsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SuccessfulExecutionsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PercentCorrectCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.LastExecutedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DateModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
      this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.createShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
      this.addHotKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.removeHotKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
      this.dataGridView1.AutoGenerateColumns = false;
      this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
      this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
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
            this.Type,
            this.DateModified});
      this.dataGridView1.DataSource = this.FileViewBindingSource;
      dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
      dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle7;
      this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridView1.Location = new System.Drawing.Point(0, 63);
      this.dataGridView1.Name = "dataGridView1";
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.RowTemplate.Height = 17;
      this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
      this.dataGridView1.Size = new System.Drawing.Size(646, 294);
      this.dataGridView1.TabIndex = 0;
      this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
      this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
      this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
      this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
      // 
      // dataGridViewImageColumn1
      // 
      this.dataGridViewImageColumn1.DataPropertyName = "Icon";
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
      this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridViewImageColumn1.HeaderText = "";
      this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
      this.dataGridViewImageColumn1.ReadOnly = true;
      this.dataGridViewImageColumn1.Width = 20;
      // 
      // NameCol
      // 
      this.NameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.NameCol.DataPropertyName = "Name";
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
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      this.TotalExecutionsCol.DefaultCellStyle = dataGridViewCellStyle3;
      this.TotalExecutionsCol.HeaderText = "Total Executions";
      this.TotalExecutionsCol.Name = "TotalExecutionsCol";
      this.TotalExecutionsCol.ReadOnly = true;
      // 
      // SuccessfulExecutionsCol
      // 
      this.SuccessfulExecutionsCol.DataPropertyName = "SuccessfulExecutions";
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      this.SuccessfulExecutionsCol.DefaultCellStyle = dataGridViewCellStyle4;
      this.SuccessfulExecutionsCol.HeaderText = "Successful Executions";
      this.SuccessfulExecutionsCol.Name = "SuccessfulExecutionsCol";
      this.SuccessfulExecutionsCol.ReadOnly = true;
      // 
      // PercentCorrectCol
      // 
      this.PercentCorrectCol.DataPropertyName = "PercentCorrect";
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      this.PercentCorrectCol.DefaultCellStyle = dataGridViewCellStyle5;
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
      // 
      // SizeCol
      // 
      this.SizeCol.DataPropertyName = "Size";
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.SizeCol.DefaultCellStyle = dataGridViewCellStyle6;
      this.SizeCol.HeaderText = "Size";
      this.SizeCol.Name = "SizeCol";
      this.SizeCol.ReadOnly = true;
      this.SizeCol.Width = 60;
      // 
      // Type
      // 
      this.Type.DataPropertyName = "Type";
      this.Type.HeaderText = "Type";
      this.Type.Name = "Type";
      this.Type.ReadOnly = true;
      this.Type.Width = 150;
      // 
      // DateModified
      // 
      this.DateModified.DataPropertyName = "DateModified";
      this.DateModified.HeaderText = "Date Modified";
      this.DateModified.Name = "DateModified";
      this.DateModified.ReadOnly = true;
      this.DateModified.Width = 150;
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
      this.toolBar.Size = new System.Drawing.Size(646, 39);
      this.toolBar.TabIndex = 0;
      this.toolBar.Text = "toolStrip1";
      // 
      // backSplitButton
      // 
      this.backSplitButton.Image = global::System.Windows.Forms.Samples.Properties.Resources.Back;
      this.backSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.backSplitButton.Name = "backSplitButton";
      this.backSplitButton.Size = new System.Drawing.Size(74, 36);
      this.backSplitButton.Text = " Back";
      this.backSplitButton.Click += new System.EventHandler(this.backSplitButton_Click);
      // 
      // forwardSplitButton
      // 
      this.forwardSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.forwardSplitButton.Image = global::System.Windows.Forms.Samples.Properties.Resources.Forward;
      this.forwardSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.forwardSplitButton.Name = "forwardSplitButton";
      this.forwardSplitButton.Size = new System.Drawing.Size(39, 36);
      this.forwardSplitButton.ToolTipText = "Forward";
      // 
      // upSplitButton
      // 
      this.upSplitButton.Image = global::System.Windows.Forms.Samples.Properties.Resources.Up;
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
      this.viewSplitButton.Image = global::System.Windows.Forms.Samples.Properties.Resources.Details;
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
      this.mainMenu.Size = new System.Drawing.Size(646, 24);
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
            this.toolStripMenuItem2,
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
            this.projectToolStripMenuItem});
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this.newToolStripMenuItem.Text = "New";
      // 
      // projectToolStripMenuItem
      // 
      this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
      this.projectToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
      this.projectToolStripMenuItem.Text = "Project";
      this.projectToolStripMenuItem.Click += new System.EventHandler(this.projectToolStripMenuItem_Click);
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
      this.tempToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
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
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
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
      this.btnRun.Location = new System.Drawing.Point(271, 34);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(75, 23);
      this.btnRun.TabIndex = 1;
      this.btnRun.Text = "Run";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // btnVisualStudio
      // 
      this.btnVisualStudio.Location = new System.Drawing.Point(370, 34);
      this.btnVisualStudio.Name = "btnVisualStudio";
      this.btnVisualStudio.Size = new System.Drawing.Size(81, 23);
      this.btnVisualStudio.TabIndex = 2;
      this.btnVisualStudio.Text = "Visual Studio";
      this.btnVisualStudio.UseVisualStyleBackColor = true;
      this.btnVisualStudio.Click += new System.EventHandler(this.btnVisualStudio_Click);
      // 
      // ExplorerView
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(646, 357);
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
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn HotKeyCol;
        private DataGridViewTextBoxColumn TotalExecutionsCol;
        private DataGridViewTextBoxColumn SuccessfulExecutionsCol;
        private DataGridViewTextBoxColumn PercentCorrectCol;
        private DataGridViewTextBoxColumn LastExecutedCol;
        private DataGridViewTextBoxColumn SizeCol;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn DateModified;
    }
}

