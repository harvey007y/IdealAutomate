using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;

namespace GitHubApiDemo
{
	/// <summary>
	/// The form used to select and order the columns to display.
	/// </summary>
	public partial class ColumnForm : Form
	{
		#region Constructors
		/// <summary>
		/// Create an instance of this form.
		/// </summary>
		public ColumnForm() =>
			InitializeComponent();
		#endregion // Constructors

		#region Private data
		private static readonly ColumnSet emptyColumns = new ColumnSet();
		private ColumnSet initialColumns = emptyColumns;
		private HashSet<string> selectedColumnsHash;
		private BindingList<string> selectedColumns;
		private bool isFirstSelect = true;
		#endregion // Private data

		#region Properties
		/// <summary>
		/// Gets the initial column selection.
		/// </summary>
		public ColumnSet InitialColumns
		{
			get => initialColumns;
			set => Columns = initialColumns = value ?? emptyColumns;
		}

		/// <summary>
		/// Get the chosen column selection.
		/// </summary>
		public ColumnSet Columns { get; private set; } =
			emptyColumns;

		/// <summary>
		/// Gets the color used to display removed parent nodes in the tree.
		/// </summary>
		public Color RemovedParentColor { get; set; } =
			Color.DarkGray;
		#endregion // Properties

#pragma warning disable IDE1006 // Naming Styles
		#region Events
		private void addAllButton_Click(object sender, EventArgs e)
		{
		}

		private void addButton_Click(object sender, EventArgs e) =>
			AddTreeNode(availableTreeView.SelectedNode);

		private void availableTreeView_AfterSelect(object sender, TreeViewEventArgs e) =>
			SelectTreeNode(e.Node);

		private void availableTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (isFirstSelect)
			{
				// Swallow the initial selection of the first node that TreeView seems to force
				if (availableTreeView.SelectedNode == null && e.Node != null &&
					e.Action == TreeViewAction.Unknown)
					e.Cancel = true;

				isFirstSelect = false;
			}
		}
		private void ColumnForm_Load(object sender, EventArgs e)
		{
			selectedColumns = new BindingList<string>(InitialColumns.Selected.ToList());
			selectedColumnsHash = new HashSet<string>(InitialColumns.Selected);
			MakeAvailable(InitialColumns.Available);
			selectedListBox.DataSource = selectedColumns;

			availableTreeView.SelectedNode = null;
			selectedListBox.ClearSelected();
		}

		private void moveDownButton_Click(object sender, EventArgs e) =>
			MoveDown();

		private void moveUpButton_Click(object sender, EventArgs e) =>
			MoveUp();

		private void okButton_Click(object sender, EventArgs e)
		{
			Columns = new ColumnSet(GetAvailableColumns(availableTreeView.Nodes), selectedColumns);
			DialogResult = DialogResult.OK;
			Close();
		}

		private void removeAllButton_Click(object sender, EventArgs e) =>
			RemoveAllListItems();

		private void removeButton_Click(object sender, EventArgs e) =>
			RemoveSelectedListItem();

		private void selectedListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (selectedListBox.SelectedIndex >= 0)
				availableTreeView.SelectedNode = null;

			EnableButtons();
		}
		#endregion // Events
#pragma warning restore IDE1006 // Naming Styles

		#region Private methods
		private void AddTreeNode(TreeNode node)
		{
			if (node == null || !(node.Tag is string column))
				return;

			if (node.Nodes.Count > 0)
				node.ForeColor = RemovedParentColor;
			else
			{
				while (true)
				{
					TreeNode parent = node.Parent;
					node.Remove();

					if (parent == null || parent.Nodes.Count > 0 ||
						parent.ForeColor != RemovedParentColor)
						break;

					node = parent;
				}
			}

			selectedColumnsHash.Add(column);
			selectedColumns.Add(column);

			SelectListItem(selectedColumns.Count - 1);
		}

		private void EnableButtons()
		{
			addButton.Enabled = availableTreeView.SelectedNode != null &&
				availableTreeView.SelectedNode.ForeColor != RemovedParentColor;
			addAllButton.Enabled = availableTreeView.Nodes.Count > 0;
			removeButton.Enabled = selectedListBox.SelectedIndex >= 0;
			removeAllButton.Enabled = selectedColumns.Count > 0;

			moveUpButton.Enabled = selectedListBox.SelectedIndex > 0;
			moveDownButton.Enabled = selectedListBox.SelectedIndex >= 0 &&
				selectedListBox.SelectedIndex < selectedListBox.Items.Count - 1;
		}

		private IEnumerable<string> GetAvailableColumns(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (!(node.Tag is string column))
					continue;

				if (!selectedColumnsHash.Contains(column))
					yield return column;

				foreach (string child in GetAvailableColumns(node.Nodes))
					yield return child;
			}
		}

		private TreeNode MakeAvailable(string column)
		{
			string[] parts = column.Split('.');
			int length = parts.Length;

			TreeNodeCollection parent = availableTreeView.Nodes;
			TreeNode node = null;

			for (int index = 0; index < length; index++)
			{
				string name = parts[index];
				int comparison = -1;
				int location = 0;

				while (location < parent.Count)
				{
					node = parent[location];

					comparison = string.Compare(name, node.Text);
					if (comparison == 0)
						break;

					if (comparison < 0)
						break;

					location++;
				}

				if (comparison == 0)
				{
					parent = node.Nodes;
					continue;
				}

				string tag = index <= 0 ? name : string.Join(".", parts.Take(index + 1));

				node = new TreeNode(name)
				{
					ForeColor = selectedColumnsHash.Contains(tag) ?
						RemovedParentColor : availableTreeView.ForeColor,
					Tag = tag
				};

				if (location < parent.Count)
					parent.Insert(location, node);
				else
					parent.Add(node);
				parent = node.Nodes;
			}

			return node;
		}

		private void MakeAvailable(IEnumerable<string> columns)
		{
			try
			{
				availableTreeView.BeginUpdate();

				foreach (string column in columns)
					MakeAvailable(column);
			}
			finally
			{
				availableTreeView.EndUpdate();
			}
		}

		private void MoveDown()
		{
			int index = selectedListBox.SelectedIndex;
			if (index < 0 || index >= selectedColumns.Count - 1)
				return;

			string column = selectedColumns[index];
			selectedColumns.RemoveAt(index);

			if (++index < selectedColumns.Count)
				selectedColumns.Insert(index, column);
			else
				selectedColumns.Add(column);

			SelectListItem(index);
		}

		private void MoveUp()
		{
			int index = selectedListBox.SelectedIndex;
			if (index <= 0)
				return;

			string column = selectedColumns[index];
			selectedColumns.RemoveAt(index);
			selectedColumns.Insert(--index, column);

			SelectListItem(index);
		}

		private void RemoveAllListItems()
		{
			selectedColumnsHash.Clear();

			try
			{
				availableTreeView.BeginUpdate();
				for (int index = 0; index < selectedColumns.Count; index++)
					MakeAvailable(selectedColumns[index]);
			}
			finally
			{
				availableTreeView.EndUpdate();
			}

			selectedColumns.Clear();
			EnableButtons();

			selectedListBox.ClearSelected();
			availableTreeView.SelectedNode = null;
		}

		private void RemoveSelectedListItem()
		{
			int index = selectedListBox.SelectedIndex;
			if (index < 0)
				return;

			string column = selectedColumns[index];
			selectedColumnsHash.Remove(column);
			selectedColumns.RemoveAt(index);

			availableTreeView.SelectedNode = MakeAvailable(column);
			selectedListBox.ClearSelected();
		}

		private void SelectListItem(int index)
		{
			selectedListBox.SelectedIndex = index;
			availableTreeView.SelectedNode = null;
		}

		private void SelectTreeNode(TreeNode node)
		{
			if (node != null)
			{
				selectedListBox.ClearSelected();
				availableTreeView.Focus();
				node.EnsureVisible();
			}

			EnableButtons();
		}
		#endregion // Private methods
	}
}
