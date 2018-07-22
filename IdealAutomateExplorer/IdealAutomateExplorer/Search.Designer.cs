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
            this.components = new System.ComponentModel.Container();
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
            this.btnShowHideColumns = new System.Windows.Forms.Button();
            this.btnNotepad = new System.Windows.Forms.Button();
            this.btnVisualStudio = new System.Windows.Forms.Button();
            this.btnPeek = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.FileViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOpenInIAE = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileViewBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find What";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "File Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Exclude";
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
            // cbxFindWhat
            // 
            this.cbxFindWhat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxFindWhat.FormattingEnabled = true;
            this.cbxFindWhat.Location = new System.Drawing.Point(75, 2);
            this.cbxFindWhat.Name = "cbxFindWhat";
            this.cbxFindWhat.Size = new System.Drawing.Size(686, 23);
            this.cbxFindWhat.TabIndex = 4;
            this.cbxFindWhat.SelectedIndexChanged += new System.EventHandler(this.cbxFindWhat_SelectedIndexChanged);
            this.cbxFindWhat.Leave += new System.EventHandler(this.cbxFindWhat_Leave);
            // 
            // cbxFileType
            // 
            this.cbxFileType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxFileType.FormattingEnabled = true;
            this.cbxFileType.Location = new System.Drawing.Point(75, 27);
            this.cbxFileType.Name = "cbxFileType";
            this.cbxFileType.Size = new System.Drawing.Size(686, 23);
            this.cbxFileType.TabIndex = 5;
            this.cbxFileType.SelectedIndexChanged += new System.EventHandler(this.cbxFileType_SelectedIndexChanged);
            this.cbxFileType.Leave += new System.EventHandler(this.cbxFileType_Leave);
            // 
            // cbxExclude
            // 
            this.cbxExclude.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxExclude.FormattingEnabled = true;
            this.cbxExclude.Location = new System.Drawing.Point(75, 52);
            this.cbxExclude.Name = "cbxExclude";
            this.cbxExclude.Size = new System.Drawing.Size(686, 23);
            this.cbxExclude.TabIndex = 6;
            this.cbxExclude.SelectedIndexChanged += new System.EventHandler(this.cbxExclude_SelectedIndexChanged);
            this.cbxExclude.Leave += new System.EventHandler(this.cbxExclude_Leave);
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
            this.btnFolder.Text = "Select Folder for Search A Folder Tab";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(16, 107);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(83, 17);
            this.chkMatchCase.TabIndex = 9;
            this.chkMatchCase.Text = "Match Case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // chkUseRegularExpression
            // 
            this.chkUseRegularExpression.AutoSize = true;
            this.chkUseRegularExpression.Location = new System.Drawing.Point(110, 107);
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
            // btnShowHideColumns
            // 
            this.btnShowHideColumns.Location = new System.Drawing.Point(329, 103);
            this.btnShowHideColumns.Name = "btnShowHideColumns";
            this.btnShowHideColumns.Size = new System.Drawing.Size(75, 23);
            this.btnShowHideColumns.TabIndex = 15;
            this.btnShowHideColumns.Text = "Show/Hide Columns";
            this.btnShowHideColumns.UseVisualStyleBackColor = true;
            this.btnShowHideColumns.Click += new System.EventHandler(this.btnShowHideColumns_Click);
            // 
            // btnNotepad
            // 
            this.btnNotepad.Location = new System.Drawing.Point(423, 103);
            this.btnNotepad.Name = "btnNotepad";
            this.btnNotepad.Size = new System.Drawing.Size(75, 23);
            this.btnNotepad.TabIndex = 16;
            this.btnNotepad.Text = "Notepad++";
            this.btnNotepad.UseVisualStyleBackColor = true;
            this.btnNotepad.Click += new System.EventHandler(this.btnNotepad_Click);
            // 
            // btnVisualStudio
            // 
            this.btnVisualStudio.Location = new System.Drawing.Point(516, 103);
            this.btnVisualStudio.Name = "btnVisualStudio";
            this.btnVisualStudio.Size = new System.Drawing.Size(89, 23);
            this.btnVisualStudio.TabIndex = 17;
            this.btnVisualStudio.Text = "Visual Studio";
            this.btnVisualStudio.UseVisualStyleBackColor = true;
            this.btnVisualStudio.Click += new System.EventHandler(this.btnVisualStudio_Click);
            // 
            // btnPeek
            // 
            this.btnPeek.Location = new System.Drawing.Point(627, 103);
            this.btnPeek.Name = "btnPeek";
            this.btnPeek.Size = new System.Drawing.Size(75, 23);
            this.btnPeek.TabIndex = 18;
            this.btnPeek.Text = "Peek";
            this.btnPeek.UseVisualStyleBackColor = true;
            this.btnPeek.Click += new System.EventHandler(this.btnPeek_Click);
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
            this.progressBar1.Location = new System.Drawing.Point(735, 103);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(235, 23);
            this.progressBar1.TabIndex = 20;
            this.progressBar1.Tag = "";
            this.progressBar1.UseWaitCursor = true;
            // 
            // percentageLabel
            // 
            this.percentageLabel.AutoSize = true;
            this.percentageLabel.Location = new System.Drawing.Point(991, 107);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(35, 13);
            this.percentageLabel.TabIndex = 21;
            this.percentageLabel.Text = "label5";
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
            this.btnCancel.Location = new System.Drawing.Point(981, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(981, 52);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 23;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // btnOpenInIAE
            // 
            this.btnOpenInIAE.Location = new System.Drawing.Point(234, 103);
            this.btnOpenInIAE.Name = "btnOpenInIAE";
            this.btnOpenInIAE.Size = new System.Drawing.Size(75, 23);
            this.btnOpenInIAE.TabIndex = 24;
            this.btnOpenInIAE.Text = "OpenInIAE";
            this.btnOpenInIAE.UseVisualStyleBackColor = true;
            this.btnOpenInIAE.Click += new System.EventHandler(this.btnOpenInIAE_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 450);
            this.Controls.Add(this.btnOpenInIAE);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnPeek);
            this.Controls.Add(this.btnVisualStudio);
            this.Controls.Add(this.btnNotepad);
            this.Controls.Add(this.btnShowHideColumns);
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
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FileViewBindingSource)).EndInit();
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
        private Button btnShowHideColumns;
        private Button btnNotepad;
        private Button btnVisualStudio;
        private Button btnPeek;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private BindingSource FileViewBindingSource;
        private ProgressBar progressBar1;
        private Label percentageLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btnCancel;
        private Button btnSearch;
        private Button btnOpenInIAE;
    }
}