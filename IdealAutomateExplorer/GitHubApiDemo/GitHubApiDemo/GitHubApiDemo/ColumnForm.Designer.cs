namespace GitHubApiDemo
{
	partial class ColumnForm
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
			this.availablePanel = new System.Windows.Forms.Panel();
			this.availableTreeView = new System.Windows.Forms.TreeView();
			this.availableLabel = new System.Windows.Forms.Label();
			this.controlPanel = new System.Windows.Forms.Panel();
			this.addAllButton = new System.Windows.Forms.Button();
			this.removeAllButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.moveDownButton = new System.Windows.Forms.Button();
			this.moveUpButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.selectedPanel = new System.Windows.Forms.Panel();
			this.selectedListBox = new System.Windows.Forms.ListBox();
			this.selectedLabel = new System.Windows.Forms.Label();
			this.availablePanel.SuspendLayout();
			this.controlPanel.SuspendLayout();
			this.selectedPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// availablePanel
			// 
			this.availablePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.availablePanel.Controls.Add(this.availableTreeView);
			this.availablePanel.Controls.Add(this.availableLabel);
			this.availablePanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.availablePanel.Location = new System.Drawing.Point(0, 0);
			this.availablePanel.Name = "availablePanel";
			this.availablePanel.Size = new System.Drawing.Size(211, 450);
			this.availablePanel.TabIndex = 0;
			// 
			// availableTreeView
			// 
			this.availableTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.availableTreeView.HideSelection = false;
			this.availableTreeView.Location = new System.Drawing.Point(0, 20);
			this.availableTreeView.Name = "availableTreeView";
			this.availableTreeView.Size = new System.Drawing.Size(207, 426);
			this.availableTreeView.TabIndex = 1;
			this.availableTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.availableTreeView_BeforeSelect);
			this.availableTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.availableTreeView_AfterSelect);
			// 
			// availableLabel
			// 
			this.availableLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.availableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.availableLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.availableLabel.Location = new System.Drawing.Point(0, 0);
			this.availableLabel.Name = "availableLabel";
			this.availableLabel.Size = new System.Drawing.Size(207, 20);
			this.availableLabel.TabIndex = 0;
			this.availableLabel.Text = "Available Columns";
			this.availableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// controlPanel
			// 
			this.controlPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.controlPanel.Controls.Add(this.addAllButton);
			this.controlPanel.Controls.Add(this.removeAllButton);
			this.controlPanel.Controls.Add(this.cancelButton);
			this.controlPanel.Controls.Add(this.okButton);
			this.controlPanel.Controls.Add(this.moveDownButton);
			this.controlPanel.Controls.Add(this.moveUpButton);
			this.controlPanel.Controls.Add(this.removeButton);
			this.controlPanel.Controls.Add(this.addButton);
			this.controlPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.controlPanel.Location = new System.Drawing.Point(211, 0);
			this.controlPanel.Name = "controlPanel";
			this.controlPanel.Size = new System.Drawing.Size(83, 450);
			this.controlPanel.TabIndex = 2;
			// 
			// addAllButton
			// 
			this.addAllButton.Enabled = false;
			this.addAllButton.Location = new System.Drawing.Point(3, 78);
			this.addAllButton.Name = "addAllButton";
			this.addAllButton.Size = new System.Drawing.Size(75, 23);
			this.addAllButton.TabIndex = 1;
			this.addAllButton.Text = "Add All";
			this.addAllButton.UseVisualStyleBackColor = true;
			this.addAllButton.Click += new System.EventHandler(this.addAllButton_Click);
			// 
			// removeAllButton
			// 
			this.removeAllButton.Enabled = false;
			this.removeAllButton.Location = new System.Drawing.Point(3, 165);
			this.removeAllButton.Name = "removeAllButton";
			this.removeAllButton.Size = new System.Drawing.Size(75, 23);
			this.removeAllButton.TabIndex = 3;
			this.removeAllButton.Text = "Remove All";
			this.removeAllButton.UseVisualStyleBackColor = true;
			this.removeAllButton.Click += new System.EventHandler(this.removeAllButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(3, 339);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(3, 310);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// moveDownButton
			// 
			this.moveDownButton.Enabled = false;
			this.moveDownButton.Location = new System.Drawing.Point(3, 252);
			this.moveDownButton.Name = "moveDownButton";
			this.moveDownButton.Size = new System.Drawing.Size(75, 23);
			this.moveDownButton.TabIndex = 5;
			this.moveDownButton.Text = "Move Down";
			this.moveDownButton.UseVisualStyleBackColor = true;
			this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
			// 
			// moveUpButton
			// 
			this.moveUpButton.Enabled = false;
			this.moveUpButton.Location = new System.Drawing.Point(3, 223);
			this.moveUpButton.Name = "moveUpButton";
			this.moveUpButton.Size = new System.Drawing.Size(75, 23);
			this.moveUpButton.TabIndex = 4;
			this.moveUpButton.Text = "Move Up";
			this.moveUpButton.UseVisualStyleBackColor = true;
			this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Enabled = false;
			this.removeButton.Location = new System.Drawing.Point(3, 136);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 23);
			this.removeButton.TabIndex = 2;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// addButton
			// 
			this.addButton.Enabled = false;
			this.addButton.Location = new System.Drawing.Point(3, 49);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 0;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// selectedPanel
			// 
			this.selectedPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.selectedPanel.Controls.Add(this.selectedListBox);
			this.selectedPanel.Controls.Add(this.selectedLabel);
			this.selectedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedPanel.Location = new System.Drawing.Point(294, 0);
			this.selectedPanel.Name = "selectedPanel";
			this.selectedPanel.Size = new System.Drawing.Size(212, 450);
			this.selectedPanel.TabIndex = 4;
			// 
			// selectedListBox
			// 
			this.selectedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedListBox.FormattingEnabled = true;
			this.selectedListBox.Location = new System.Drawing.Point(0, 20);
			this.selectedListBox.Name = "selectedListBox";
			this.selectedListBox.Size = new System.Drawing.Size(208, 426);
			this.selectedListBox.TabIndex = 2;
			this.selectedListBox.SelectedIndexChanged += new System.EventHandler(this.selectedListBox_SelectedIndexChanged);
			// 
			// selectedLabel
			// 
			this.selectedLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.selectedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.selectedLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.selectedLabel.Location = new System.Drawing.Point(0, 0);
			this.selectedLabel.Name = "selectedLabel";
			this.selectedLabel.Size = new System.Drawing.Size(208, 20);
			this.selectedLabel.TabIndex = 1;
			this.selectedLabel.Text = "Selected Columns";
			this.selectedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ColumnForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(506, 450);
			this.Controls.Add(this.selectedPanel);
			this.Controls.Add(this.controlPanel);
			this.Controls.Add(this.availablePanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ColumnForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Columns...";
			this.Load += new System.EventHandler(this.ColumnForm_Load);
			this.availablePanel.ResumeLayout(false);
			this.controlPanel.ResumeLayout(false);
			this.selectedPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel availablePanel;
		private System.Windows.Forms.Label availableLabel;
		private System.Windows.Forms.Panel controlPanel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button moveDownButton;
		private System.Windows.Forms.Button moveUpButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Panel selectedPanel;
		private System.Windows.Forms.ListBox selectedListBox;
		private System.Windows.Forms.Label selectedLabel;
		private System.Windows.Forms.Button removeAllButton;
		private System.Windows.Forms.Button addAllButton;
		private System.Windows.Forms.TreeView availableTreeView;
	}
}