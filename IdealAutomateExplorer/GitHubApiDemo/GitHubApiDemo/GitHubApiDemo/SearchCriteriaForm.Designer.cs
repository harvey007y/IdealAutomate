namespace GitHubApiDemo
{
	partial class SearchCriteriaForm
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
			this.propertiesPanel = new System.Windows.Forms.Panel();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.propertyGridContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.propertyGridResetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGridResetAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGridSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.propertyGridCommandsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGridDescriptionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonPanel = new System.Windows.Forms.Panel();
			this.resetAllButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.propertiesPanel.SuspendLayout();
			this.propertyGridContextMenuStrip.SuspendLayout();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// propertiesPanel
			// 
			this.propertiesPanel.Controls.Add(this.propertyGrid);
			this.propertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertiesPanel.Location = new System.Drawing.Point(0, 0);
			this.propertiesPanel.Name = "propertiesPanel";
			this.propertiesPanel.Size = new System.Drawing.Size(800, 421);
			this.propertiesPanel.TabIndex = 0;
			// 
			// propertyGrid
			// 
			this.propertyGrid.ContextMenuStrip = this.propertyGridContextMenuStrip;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(800, 421);
			this.propertyGrid.TabIndex = 0;
			// 
			// propertyGridContextMenuStrip
			// 
			this.propertyGridContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertyGridResetMenuItem,
            this.propertyGridResetAllMenuItem,
            this.propertyGridSeparator1,
            this.propertyGridCommandsMenuItem,
            this.propertyGridDescriptionMenuItem});
			this.propertyGridContextMenuStrip.Name = "propertyGridContextMenuStrip";
			this.propertyGridContextMenuStrip.Size = new System.Drawing.Size(181, 120);
			this.propertyGridContextMenuStrip.Opened += new System.EventHandler(this.propertyGridContextMenuStrip_Opened);
			// 
			// propertyGridResetMenuItem
			// 
			this.propertyGridResetMenuItem.Name = "propertyGridResetMenuItem";
			this.propertyGridResetMenuItem.Size = new System.Drawing.Size(180, 22);
			this.propertyGridResetMenuItem.Text = "Reset";
			this.propertyGridResetMenuItem.Click += new System.EventHandler(this.propertyGridResetMenuItem_Click);
			// 
			// propertyGridResetAllMenuItem
			// 
			this.propertyGridResetAllMenuItem.Name = "propertyGridResetAllMenuItem";
			this.propertyGridResetAllMenuItem.Size = new System.Drawing.Size(180, 22);
			this.propertyGridResetAllMenuItem.Text = "Reset All";
			this.propertyGridResetAllMenuItem.Click += new System.EventHandler(this.propertyGridResetAllMenuItem_Click);
			// 
			// propertyGridSeparator1
			// 
			this.propertyGridSeparator1.Name = "propertyGridSeparator1";
			this.propertyGridSeparator1.Size = new System.Drawing.Size(177, 6);
			// 
			// propertyGridCommandsMenuItem
			// 
			this.propertyGridCommandsMenuItem.Checked = true;
			this.propertyGridCommandsMenuItem.CheckOnClick = true;
			this.propertyGridCommandsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.propertyGridCommandsMenuItem.Name = "propertyGridCommandsMenuItem";
			this.propertyGridCommandsMenuItem.Size = new System.Drawing.Size(180, 22);
			this.propertyGridCommandsMenuItem.Text = "Commands";
			this.propertyGridCommandsMenuItem.CheckedChanged += new System.EventHandler(this.propertyGridCommandsMenuItem_CheckedChanged);
			// 
			// propertyGridDescriptionMenuItem
			// 
			this.propertyGridDescriptionMenuItem.Checked = true;
			this.propertyGridDescriptionMenuItem.CheckOnClick = true;
			this.propertyGridDescriptionMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.propertyGridDescriptionMenuItem.Name = "propertyGridDescriptionMenuItem";
			this.propertyGridDescriptionMenuItem.Size = new System.Drawing.Size(180, 22);
			this.propertyGridDescriptionMenuItem.Text = "Description";
			this.propertyGridDescriptionMenuItem.CheckedChanged += new System.EventHandler(this.propertyGridDescriptionMenuItem_CheckedChanged);
			// 
			// buttonPanel
			// 
			this.buttonPanel.Controls.Add(this.resetAllButton);
			this.buttonPanel.Controls.Add(this.cancelButton);
			this.buttonPanel.Controls.Add(this.okButton);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.Location = new System.Drawing.Point(0, 421);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size(800, 29);
			this.buttonPanel.TabIndex = 1;
			// 
			// resetAllButton
			// 
			this.resetAllButton.Location = new System.Drawing.Point(165, 3);
			this.resetAllButton.Name = "resetAllButton";
			this.resetAllButton.Size = new System.Drawing.Size(75, 23);
			this.resetAllButton.TabIndex = 2;
			this.resetAllButton.Text = "Reset All";
			this.resetAllButton.UseVisualStyleBackColor = true;
			this.resetAllButton.Click += new System.EventHandler(this.resetAllButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(84, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.okButton.Location = new System.Drawing.Point(3, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// SearchCriteriaForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.propertiesPanel);
			this.Controls.Add(this.buttonPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "SearchCriteriaForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Search Criteria...";
			this.Load += new System.EventHandler(this.SearchRepositoriesForm_Load);
			this.propertiesPanel.ResumeLayout(false);
			this.propertyGridContextMenuStrip.ResumeLayout(false);
			this.buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel propertiesPanel;
		private System.Windows.Forms.Panel buttonPanel;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ContextMenuStrip propertyGridContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem propertyGridResetMenuItem;
		private System.Windows.Forms.ToolStripSeparator propertyGridSeparator1;
		private System.Windows.Forms.ToolStripMenuItem propertyGridCommandsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem propertyGridDescriptionMenuItem;
		private System.Windows.Forms.Button resetAllButton;
		private System.Windows.Forms.ToolStripMenuItem propertyGridResetAllMenuItem;
	}
}