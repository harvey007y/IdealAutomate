namespace System.Windows.Forms.Samples {
    partial class Search {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxFindWhat = new System.Windows.Forms.ComboBox();
            this.cbxFileType = new System.Windows.Forms.ComboBox();
            this.cbxExclude = new System.Windows.Forms.ComboBox();
            this.cbxFolder = new System.Windows.Forms.ComboBox();
            this.btnFolder = new System.Windows.Forms.Button();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.chkUseRegularExpression = new System.Windows.Forms.CheckBox();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panelResults = new System.Windows.Forms.Panel();
            this.btnShowHideColumns = new System.Windows.Forms.Button();
            this.btnNotepad = new System.Windows.Forms.Button();
            this.btnVisualStudio = new System.Windows.Forms.Button();
            this.btnPeek = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find What";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "File Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Exclude";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Folder";
            // 
            // cbxFindWhat
            // 
            this.cbxFindWhat.FormattingEnabled = true;
            this.cbxFindWhat.Location = new System.Drawing.Point(75, 2);
            this.cbxFindWhat.Name = "cbxFindWhat";
            this.cbxFindWhat.Size = new System.Drawing.Size(600, 21);
            this.cbxFindWhat.TabIndex = 4;
            this.cbxFindWhat.SelectedIndexChanged += new System.EventHandler(this.cbxFindWhat_SelectedIndexChanged);
            this.cbxFindWhat.Leave += new System.EventHandler(this.cbxFindWhat_Leave);
            // 
            // cbxFileType
            // 
            this.cbxFileType.FormattingEnabled = true;
            this.cbxFileType.Location = new System.Drawing.Point(75, 24);
            this.cbxFileType.Name = "cbxFileType";
            this.cbxFileType.Size = new System.Drawing.Size(600, 21);
            this.cbxFileType.TabIndex = 5;
            // 
            // cbxExclude
            // 
            this.cbxExclude.FormattingEnabled = true;
            this.cbxExclude.Location = new System.Drawing.Point(75, 46);
            this.cbxExclude.Name = "cbxExclude";
            this.cbxExclude.Size = new System.Drawing.Size(600, 21);
            this.cbxExclude.TabIndex = 6;
            // 
            // cbxFolder
            // 
            this.cbxFolder.DropDownWidth = 600;
            this.cbxFolder.FormattingEnabled = true;
            this.cbxFolder.Location = new System.Drawing.Point(75, 68);
            this.cbxFolder.Name = "cbxFolder";
            this.cbxFolder.Size = new System.Drawing.Size(600, 21);
            this.cbxFolder.TabIndex = 7;
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(682, 66);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(113, 23);
            this.btnFolder.TabIndex = 8;
            this.btnFolder.Text = "Select Folder or File";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(19, 95);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(83, 17);
            this.chkMatchCase.TabIndex = 9;
            this.chkMatchCase.Text = "Match Case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // chkUseRegularExpression
            // 
            this.chkUseRegularExpression.AutoSize = true;
            this.chkUseRegularExpression.Location = new System.Drawing.Point(110, 95);
            this.chkUseRegularExpression.Name = "chkUseRegularExpression";
            this.chkUseRegularExpression.Size = new System.Drawing.Size(117, 17);
            this.chkUseRegularExpression.TabIndex = 10;
            this.chkUseRegularExpression.Text = "Regular Expression";
            this.chkUseRegularExpression.UseVisualStyleBackColor = true;
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(810, 8);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(44, 13);
            this.lblResults.TabIndex = 11;
            this.lblResults.Text = "Count 0";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(233, 91);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.search_ClickAsync);
            // 
            // panelResults
            // 
            this.panelResults.Location = new System.Drawing.Point(19, 118);
            this.panelResults.Name = "panelResults";
            this.panelResults.Size = new System.Drawing.Size(861, 282);
            this.panelResults.TabIndex = 14;
            // 
            // btnShowHideColumns
            // 
            this.btnShowHideColumns.Location = new System.Drawing.Point(327, 91);
            this.btnShowHideColumns.Name = "btnShowHideColumns";
            this.btnShowHideColumns.Size = new System.Drawing.Size(75, 23);
            this.btnShowHideColumns.TabIndex = 15;
            this.btnShowHideColumns.Text = "Show/Hide Columns";
            this.btnShowHideColumns.UseVisualStyleBackColor = true;
            this.btnShowHideColumns.Click += new System.EventHandler(this.btnShowHideColumns_Click);
            // 
            // btnNotepad
            // 
            this.btnNotepad.Location = new System.Drawing.Point(424, 91);
            this.btnNotepad.Name = "btnNotepad";
            this.btnNotepad.Size = new System.Drawing.Size(75, 23);
            this.btnNotepad.TabIndex = 16;
            this.btnNotepad.Text = "Notepad++";
            this.btnNotepad.UseVisualStyleBackColor = true;
            this.btnNotepad.Click += new System.EventHandler(this.btnNotepad_Click);
            // 
            // btnVisualStudio
            // 
            this.btnVisualStudio.Location = new System.Drawing.Point(516, 91);
            this.btnVisualStudio.Name = "btnVisualStudio";
            this.btnVisualStudio.Size = new System.Drawing.Size(89, 23);
            this.btnVisualStudio.TabIndex = 17;
            this.btnVisualStudio.Text = "Visual Studio";
            this.btnVisualStudio.UseVisualStyleBackColor = true;
            this.btnVisualStudio.Click += new System.EventHandler(this.btnVisualStudio_Click);
            // 
            // btnPeek
            // 
            this.btnPeek.Location = new System.Drawing.Point(630, 90);
            this.btnPeek.Name = "btnPeek";
            this.btnPeek.Size = new System.Drawing.Size(75, 23);
            this.btnPeek.TabIndex = 18;
            this.btnPeek.Text = "Peek";
            this.btnPeek.UseVisualStyleBackColor = true;
            this.btnPeek.Click += new System.EventHandler(this.btnPeek_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 450);
            this.Controls.Add(this.btnPeek);
            this.Controls.Add(this.btnVisualStudio);
            this.Controls.Add(this.btnNotepad);
            this.Controls.Add(this.btnShowHideColumns);
            this.Controls.Add(this.panelResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.chkUseRegularExpression);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.cbxFolder);
            this.Controls.Add(this.cbxExclude);
            this.Controls.Add(this.cbxFileType);
            this.Controls.Add(this.cbxFindWhat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Search";
            this.Text = "Search";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Search_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox cbxFindWhat;
        private ComboBox cbxFileType;
        private ComboBox cbxExclude;
        private ComboBox cbxFolder;
        private Button btnFolder;
        private CheckBox chkMatchCase;
        private CheckBox chkUseRegularExpression;
        private Label lblResults;        
        private Button btnSearch;
        private Panel panelResults;
        private Button btnShowHideColumns;
        private Button btnNotepad;
        private Button btnVisualStudio;
        private Button btnPeek;
    }
}