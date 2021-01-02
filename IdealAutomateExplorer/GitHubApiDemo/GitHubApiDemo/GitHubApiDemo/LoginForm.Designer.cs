namespace GitHubApiDemo
{
	partial class LoginForm
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
			this.authLabel = new System.Windows.Forms.Label();
			this.authComboBox = new System.Windows.Forms.ComboBox();
			this.userLabel = new System.Windows.Forms.Label();
			this.userTextBox = new System.Windows.Forms.TextBox();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// authLabel
			// 
			this.authLabel.AutoSize = true;
			this.authLabel.Location = new System.Drawing.Point(12, 15);
			this.authLabel.Name = "authLabel";
			this.authLabel.Size = new System.Drawing.Size(59, 13);
			this.authLabel.TabIndex = 0;
			this.authLabel.Text = "Auth Type:";
			// 
			// authComboBox
			// 
			this.authComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.authComboBox.FormattingEnabled = true;
			this.authComboBox.Items.AddRange(new object[] {
            "Unauthenticated Access",
            "Basic Authentication",
            "OAuth Token Authentication"});
			this.authComboBox.Location = new System.Drawing.Point(76, 12);
			this.authComboBox.Name = "authComboBox";
			this.authComboBox.Size = new System.Drawing.Size(241, 21);
			this.authComboBox.TabIndex = 1;
			this.authComboBox.SelectedIndexChanged += new System.EventHandler(this.authComboBox_SelectedIndexChanged);
			// 
			// userLabel
			// 
			this.userLabel.AutoSize = true;
			this.userLabel.Location = new System.Drawing.Point(12, 42);
			this.userLabel.Name = "userLabel";
			this.userLabel.Size = new System.Drawing.Size(58, 13);
			this.userLabel.TabIndex = 2;
			this.userLabel.Text = "Username:";
			this.userLabel.Visible = false;
			// 
			// userTextBox
			// 
			this.userTextBox.Location = new System.Drawing.Point(76, 39);
			this.userTextBox.Name = "userTextBox";
			this.userTextBox.Size = new System.Drawing.Size(241, 20);
			this.userTextBox.TabIndex = 3;
			this.userTextBox.Visible = false;
			this.userTextBox.TextChanged += new System.EventHandler(this.userTextBox_TextChanged);
			// 
			// passwordLabel
			// 
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(14, 68);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(56, 13);
			this.passwordLabel.TabIndex = 4;
			this.passwordLabel.Text = "Password:";
			this.passwordLabel.Visible = false;
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(76, 65);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '*';
			this.passwordTextBox.Size = new System.Drawing.Size(241, 20);
			this.passwordTextBox.TabIndex = 5;
			this.passwordTextBox.Visible = false;
			this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBox_TextChanged);
			// 
			// okButton
			// 
			this.okButton.Enabled = false;
			this.okButton.Location = new System.Drawing.Point(17, 91);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// LoginForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 124);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.passwordTextBox);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.userTextBox);
			this.Controls.Add(this.userLabel);
			this.Controls.Add(this.authComboBox);
			this.Controls.Add(this.authLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Login";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.LoginForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label authLabel;
		private System.Windows.Forms.ComboBox authComboBox;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.TextBox userTextBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.Button okButton;
	}
}