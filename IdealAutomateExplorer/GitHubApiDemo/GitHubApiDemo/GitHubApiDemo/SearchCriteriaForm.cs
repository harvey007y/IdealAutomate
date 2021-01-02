using System;
using System.Windows.Forms;

namespace GitHubApiDemo
{
	/// <summary>
	/// A form to edit search criteria.
	/// </summary>
	public partial class SearchCriteriaForm : Form
	{
		#region Constructors
		/// <summary>
		/// Create an instance.
		/// </summary>
		public SearchCriteriaForm() =>
			InitializeComponent();
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Gets or sets the object used for search criteria.
		/// </summary>
		public object SelectedObject { get; set; }
		#endregion // Properties

		#region Private methods
		private void okButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void propertyGridCommandsMenuItem_CheckedChanged(object sender, EventArgs e) =>
			propertyGrid.CommandsVisibleIfAvailable = propertyGridCommandsMenuItem.Checked;

		private void propertyGridContextMenuStrip_Opened(object sender, EventArgs e)
		{
			var gridItem = propertyGrid.SelectedGridItem;
			propertyGridResetMenuItem.Enabled = gridItem?.PropertyDescriptor.CanResetValue(
				gridItem?.Parent.Value ?? propertyGrid.SelectedObject) ?? false;
		}

		private void propertyGridDescriptionMenuItem_CheckedChanged(object sender, EventArgs e) =>
			propertyGrid.HelpVisible = propertyGridDescriptionMenuItem.Checked;

		private void propertyGridResetAllMenuItem_Click(object sender, EventArgs e) =>
			resetAll();

		private void propertyGridResetMenuItem_Click(object sender, EventArgs e) =>
			propertyGrid.ResetSelectedProperty();

		private void SearchRepositoriesForm_Load(object sender, EventArgs e)
		{
			resetAllButton.Enabled = SelectedObject is ISearchBroker;
			propertyGridResetAllMenuItem.Enabled = resetAllButton.Enabled;
			propertyGrid.SelectedObject = SelectedObject;
			propertyGrid.Select();
			propertyGrid.Focus();
		}

		private void resetAllButton_Click(object sender, EventArgs e) =>
			resetAll();

		private void resetAll()
		{
			if (!(SelectedObject is ISearchBroker broker))
				return;

			broker.Reset();
			propertyGrid.Refresh();
		}
		#endregion // Private methods
	}
}
