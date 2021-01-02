using System;
using System.Windows.Forms;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// The form used to get login/authentication information.
	/// </summary>
	public partial class LoginForm : Form
	{
		#region Constructors
		/// <summary>
		/// Create an instance of this form.
		/// </summary>
		public LoginForm() =>
			InitializeComponent();
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Gets the credentials provided by the user.
		/// </summary>
		public Credentials Credentials { get; private set; }
		#endregion // Properties

#pragma warning disable IDE1006 // Naming Styles
		#region Events
		private void LoginForm_Load(object sender, EventArgs e) =>
			authComboBox.SelectedIndex = (int)AuthenticationType.Basic;

		private void okButton_Click(object sender, EventArgs e) =>
			CloseForm();

		private void authComboBox_SelectedIndexChanged(object sender, EventArgs e) =>
			SetVisibility(GetAuthenticationType());

		private void passwordTextBox_TextChanged(object sender, EventArgs e) =>
			EnableOK();

		private void userTextBox_TextChanged(object sender, EventArgs e) =>
			EnableOK();
		#endregion // Events
#pragma warning restore IDE1006 // Naming Styles

		#region Private methods
		private void CloseForm()
		{
			switch (GetAuthenticationType())
			{
				case AuthenticationType.Basic:
					Credentials = new Credentials(userTextBox.Text.Trim(),
						passwordTextBox.Text.Trim());
					DialogResult = DialogResult.OK;
					break;

				case AuthenticationType.Token:
					Credentials = new Credentials(userTextBox.Text.Trim());
					DialogResult = DialogResult.OK;
					break;

				case AuthenticationType.Unauthenticated:
					DialogResult = DialogResult.OK;
					Credentials = null;
					break;

				default:
					DialogResult = DialogResult.Cancel;
					break;
			}

			Close();
		}

		private void EnableOK()
		{
			switch(GetAuthenticationType())
			{
				case AuthenticationType.Basic:
					okButton.Enabled = HasUsername() && HasPassword();
					break;

				case AuthenticationType.Token:
					okButton.Enabled = HasUsername();
					break;

				case AuthenticationType.Unauthenticated:
					okButton.Enabled = true;
					break;

				case AuthenticationType.Error:
				default:
					okButton.Enabled = false;
					break;
			}
		}

		private AuthenticationType GetAuthenticationType()
		{
			int index = authComboBox.SelectedIndex;
			return index < 0 ? AuthenticationType.Error : (AuthenticationType)index;
		}

		private bool HasPassword() =>
			!string.IsNullOrWhiteSpace(passwordTextBox.Text);

		private bool HasUsername() =>
			!string.IsNullOrWhiteSpace(userTextBox.Text);

		private void SetVisibility(AuthenticationType type = AuthenticationType.Error)
		{
			switch(type)
			{
				case AuthenticationType.Basic:
					userLabel.Text = "Username:";
					userLabel.Visible = userTextBox.Visible = true;
					passwordLabel.Visible = passwordTextBox.Visible = true;
					break;

				case AuthenticationType.Token:
					userLabel.Text = "Token:";
					userLabel.Visible = userTextBox.Visible = true;
					passwordLabel.Visible = passwordTextBox.Visible = false;
					break;


				case AuthenticationType.Error:
				case AuthenticationType.Unauthenticated:
				default:
					userLabel.Visible = userTextBox.Visible = false;
					passwordLabel.Visible = passwordTextBox.Visible = false;
					break;
			}

			userTextBox.Text = passwordTextBox.Text = string.Empty;
			EnableOK();
		}
		#endregion // Private methods

		#region Private enums
		private enum AuthenticationType
		{
			Error = -1,
			Unauthenticated,
			Basic,
			Token
		}
		#endregion // Private enums
	}
}
