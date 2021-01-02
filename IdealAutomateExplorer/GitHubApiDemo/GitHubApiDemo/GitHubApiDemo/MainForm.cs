using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Octokit;
using GitHubApiDemo.Properties;
using System.Net;
using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CSharp;
using System.IO;
using System.Collections.ObjectModel;

namespace GitHubApiDemo
{
	/// <summary>
	/// The main window for this application.
	/// </summary>
	public partial class MainForm : Form
	{
		#region Constructors
		/// <summary>
		/// Create an instance of this form.
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			mainDataGridView.AutoGenerateColumns = false;

			// Trick to make read-only properties display using regular text color
			// See: https://social.msdn.microsoft.com/Forums/windows/en-US/9fd7591d-8925-43e4-bdf1-988c9bb5ca5e/changing-font-color-on-readonly-fields-in-propertygrid?forum=winforms
			detailPropertyGrid.ViewForeColor = Color.FromArgb(0, 0, 1);
		}
		#endregion // Constructors

		#region Constants
		/// <summary>
		/// A unique name that identifies the client to GitHub.  This should be the name of the
		/// product, GitHub organization, or the GitHub username (in that order of preference) that
		/// is using the Octokit framework.
		///</summary>
		public static readonly string GitHubIdentity = Assembly
			.GetEntryAssembly()
			.GetCustomAttribute<AssemblyProductAttribute>()
			.Product;
		#endregion // Constants

		#region Private data
		private BackgroundType backgroundType;
		private SearchResult searchResult;
		private Searcher activeSearcher;
		private Searcher searcher;
		private GitHubClient client;
		private User currentUser;
		private object fullDetail;
		private int maximumCount = 1000;
		private int previousCount;
		private bool isExitPending;
		private List<string> repos = new List<string>();
		private List<SearchCode> paths = new List<SearchCode>();
		#endregion // Private data

#pragma warning disable IDE1006 // Naming Styles
		#region Events
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e) =>
			SaveSettings();

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!mainBackgroundWorker.IsBusy)
				return;

			if (!isExitPending && mainBackgroundWorker.CancellationPending)
				mainBackgroundWorker.CancelAsync();

			isExitPending = true;
			e.Cancel = true;
			return;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
            LoadSettings();
            //BeginInvoke((MethodInvoker)ShowLoginForm);
            ShowLoginForm();
			Search2<SearchRepositoriesBroker>();
			
		
		}

		private void dataGridSelectColumnsMenuItem_Click(object sender, EventArgs e) =>
			ShowColumnForm();

		private void detailGetMenuItem_Click(object sender, EventArgs e) =>
			GetFullDetail();

		private void editFindCodeMenuItem_Click(object sender, EventArgs e)
		{
			Search<SearchCodeBroker>();
		}

		private void editFindIssueMenuItem_Click(object sender, EventArgs e) =>
			Search<SearchIssuesBroker>();

		private void editFindLabelMenuItem_Click(object sender, EventArgs e) =>
			Search<SearchLabelsBroker>();

		private void editFindRepositoryMenuItem_Click(object sender, EventArgs e) =>
			Search<SearchRepositoriesBroker>();

		private void editFindUserMenuItem_Click(object sender, EventArgs e) =>
			Search<SearchUsersBroker>();

		private void editSelectColumnsMenuItem_Click(object sender, EventArgs e) =>
			ShowColumnForm();

		private void helpAboutMenuItem_Click(object sender, EventArgs e)
		{
			using (var dialog = new MainAboutBox())
				dialog.ShowDialog(this);
		}

		private void mainBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) =>
			DoWork(e);

		private void mainBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
		}

		private void mainBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) =>
			CompleteWork(e);

		private void mainDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) =>
			FormatNestedProperty(sender as DataGridView, e);

		private void mainDataGridView_DoubleClick(object sender, EventArgs e) =>
			mainTabControl.SelectedTab = detailTabPage;

		private void mainDataGridView_SelectionChanged(object sender, EventArgs e) =>
			UpdateDetail();

		private void progressTimer_Tick(object sender, EventArgs e) =>
			UpdateProgress();

		private void viewDetailMenuItem_Click(object sender, EventArgs e) =>
			mainTabControl.SelectedTab = detailTabPage;

		private void viewFullDetailMenuItem_Click(object sender, EventArgs e) =>
			GetFullDetail();
		#endregion // Events
#pragma warning restore IDE1006 // Naming Styles

		#region Private methods
		private void AddColumns()
		{
			mainDataGridView.DataSource = null;
			mainDataGridView.Columns.Clear();

			DataGridViewColumnCollection columns = mainDataGridView.Columns;

			Type type = searchResult.ItemType;

			foreach (string name in searcher.Columns.Selected)
			{
				PropertyInfo property = ColumnSet.GetProperty(type, name);
				Type propertyType = property.PropertyType;

				columns.Add(new DataGridViewTextBoxColumn
				{
					DataPropertyName = name,
					HeaderText = name,
					Name = name,
					ValueType = property.PropertyType,
					Tag = property
				});
			}

			mainDataGridView.DataSource = searchResult.DataSource;

			foreach (DataGridViewColumn column in columns)
			{
				int width = column.Width;
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				column.Width = width;
			}

			mainTabControl.SelectedTab = listTabPage;
			EnableUI(true);
		}

		private void BeginWork(BackgroundType type, object argument)
		{
			activeSearcher = null;
			fullDetail = null;

			switch (type)
			{
				case BackgroundType.Search:
					if (argument is Searcher searcher)
					{
						mainStatusLabel.Text = $"Searching for {searcher.Type}...";
						mainProgressBar.Visible = true;
						mainProgressBar.Value = 0;
						activeSearcher = searcher;
					}
					break;

				case BackgroundType.Detail:
					mainStatusLabel.Text = $"Getting full detail...";
					break;

				default:
					return;
			}

			EnableUI(false);
			progressTimer.Start();
			backgroundType = type;
			mainBackgroundWorker.RunWorkerAsync(argument);
		}

		private void CompleteDetail()
		{
			if (fullDetail == null)
			{
				EndWork("Operation failed.");
				return;
			}

			detailPropertyGrid.SelectedObject = new TypeBroker(fullDetail);
			viewFullDetailMenuItem.Enabled = false;
			detailGetMenuItem.Enabled = false;
			fullDetail = null;
			EndWork("Full detail obtained.");
		}

		private void CompleteSearch()
		{
			if (searchResult == null)
			{
				mainDataGridView.DataSource = null;
				EndWork("Operation failed.");
				return;
			}

			AddColumns();

			string incompleteText = searchResult.IncompleteResults ?
				" (incomplete Results)" : string.Empty;

			EndWork($"{searchResult.DataSource.Count} of {searchResult.TotalCount} matches " +
				$"loaded{incompleteText}.");
		}

		private void CompleteWork(RunWorkerCompletedEventArgs e)
		{
			BackgroundType type = backgroundType;
			backgroundType = BackgroundType.None;

			progressTimer.Stop();
			mainProgressBar.Visible = false;
			mainProgressBar.Value = 0;
			activeSearcher = null;
			previousCount = 0;
			EnableUI(true);

			if (isExitPending)
			{
				EndWork("Operation cancelled, closing application...");
				Close();
				return;
			}

			if (e.Error != null)
			{
				MessageBox.Show(this, e.Error.Message, "Warning", MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				EndWork($"Error: {e.Error.Message}");
				return;
			}

			if (e.Cancelled)
			{
				EndWork("Operation cancelled.");
				return;
			}

			switch (type)
			{
				case BackgroundType.Search:
					CompleteSearch();
					break;

				case BackgroundType.Detail:
					CompleteDetail();
					break;

				case BackgroundType.None:
					EndWork("Operation failed.");
					break;
			}
		}

		private void CreateClient(Credentials credentials)
		{
			try
			{
				client = new GitHubClient(new ProductHeaderValue(GitHubIdentity));
				if (credentials == null)
				{
					currentUser = null;
					return;
				}

				client.Credentials = credentials;
				currentUser = client.User
					.Current()
					.GetAwaiter()
					.GetResult();
			}
			catch (Exception ex)
			{
				client = null;
				MessageBox.Show(this, ex.Message, "Authentication Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private Searcher CreateSearcher(ISearchBroker broker)
		{
			try
			{
				return broker.CreateSearcher(client, maximumCount);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Invalid Search", MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return null;
			}
		}

		private void DoWork(DoWorkEventArgs e)
		{
			switch (backgroundType)
			{
				case BackgroundType.Search:
					if (e.Argument is Searcher searcher)
						Search(searcher);
					break;

				case BackgroundType.Detail:
					fullDetail = e.Argument == null ?
						null : this.searcher.GetDetail(e.Argument);
					break;

				default:
					break;
			}

			e.Cancel = mainBackgroundWorker.CancellationPending;
		}

		private void EnableDetailUi(bool isEnabled)
		{
			isEnabled = isEnabled && !mainBackgroundWorker.IsBusy;

			DataGridViewSelectedRowCollection rows = mainDataGridView.SelectedRows;
			object item = rows.Count > 0 ? rows[0].DataBoundItem : null;

			object propertyItem = detailPropertyGrid.SelectedObject is TypeBroker broker ?
				broker.Actual : item;

			bool hasDetail = searcher?.CanGetDetail ?? false;

			bool lacksDetail = isEnabled && hasDetail && rows
				.OfType<DataGridViewRow>()
				.Any(row => row.DataBoundItem == propertyItem);

			viewFullDetailMenuItem.Enabled = lacksDetail;
			detailGetMenuItem.Enabled = lacksDetail;
			viewDetailMenuItem.Enabled = isEnabled;
		}

		private void EnableUI(bool isEnabled)
		{
			isEnabled = isEnabled && !mainBackgroundWorker.IsBusy;

			editSelectColumnsMenuItem.Enabled = isEnabled && mainDataGridView.DataSource != null;
			editFindCodeMenuItem.Enabled = isEnabled;
			editFindLabelMenuItem.Enabled = isEnabled;
			editFindIssueMenuItem.Enabled = isEnabled;
			editFindRepositoryMenuItem.Enabled = isEnabled;
			editFindUserMenuItem.Enabled = isEnabled;
			mainTabControl.Enabled = isEnabled;
			EnableDetailUi(isEnabled);
		}

		private void EndWork(string statusText) =>
			mainStatusLabel.Text = statusText;

		private void FormatNestedProperty(DataGridView grid, DataGridViewCellFormattingEventArgs e)
		{
			if (grid == null || e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count ||
				e.ColumnIndex < 0 || e.ColumnIndex >= grid.Columns.Count)
				return;

			DataGridViewColumn column = grid.Columns[e.ColumnIndex];
			DataGridViewRow row = grid.Rows[e.RowIndex];
			object item = row.DataBoundItem;

			if (item == null || !column.DataPropertyName.Contains('.'))
				return;

			if (ColumnSet.TryGetNestedPropertyValue(column.DataPropertyName, item, out object value))
				e.Value = value;
		}

		private void GetFullDetail()
		{
			if (searcher != null && (detailPropertyGrid.SelectedObject is TypeBroker broker))
				BeginWork(BackgroundType.Detail, broker.Actual);
		}

		private bool LoadColumnSettings()
		{
			bool isChanged = false;

			CodeSearcher.SavedColumns = ParseColumns(typeof(SearchCode),
				Settings.Default.ColumnsCode, CodeSearcher.DefaultColumns, ref isChanged);
			IssueSearcher.SavedColumns = ParseColumns(typeof(Issue),
				Settings.Default.ColumnsIssue, IssueSearcher.DefaultColumns, ref isChanged);
			LabelSearcher.SavedColumns = ParseColumns(typeof(Octokit.Label),
				Settings.Default.ColumnsLabel, LabelSearcher.DefaultColumns, ref isChanged);
			RepositorySearcher.SavedColumns = ParseColumns(typeof(Repository),
				Settings.Default.ColumnsRepository, RepositorySearcher.DefaultColumns, ref isChanged);
			UserSearcher.SavedColumns = ParseColumns(typeof(User),
				Settings.Default.ColumnsUser, UserSearcher.DefaultColumns, ref isChanged);

			return isChanged;
		}

		private void LoadSettings()
		{
			bool isChanged = UpgradeSettings();

			if (LoadWindowSettings())
			{
				SaveWindowSettings();
				isChanged = true;
			}

			if (LoadColumnSettings())
			{
				SaveColumnSettings();
				isChanged = true;
			}

			if (isChanged)
				Settings.Default.Save();
		}

		private bool LoadWindowSettings()
		{
			if (Settings.Default.WindowSize == Size.Empty)
				return true;

			Size = Settings.Default.WindowSize;
			Location = Settings.Default.WindowLocation;
			WindowState = Settings.Default.WindowState;
			return false;
		}

		private ColumnSet ParseColumns(Type type, string columns, ColumnSet defaultColumns,
			ref bool isChanged)
		{
			try
			{
				List<string> selectedColumns = columns
					.Split(',')
					.Where(column => !string.IsNullOrWhiteSpace(column))
					.ToList();

				if (selectedColumns.Count > 0)
				{
					ColumnSet result = new ColumnSet(type, Searcher.Depth, selectedColumns);
					isChanged |= columns != result.ToString();
					return result;
				}
			}
			catch
			{
			}

			isChanged = true;
			return defaultColumns;
		}

		private void SaveColumnSettings()
		{
			Settings.Default.ColumnsCode = CodeSearcher.SavedColumns.ToString();
			Settings.Default.ColumnsIssue = IssueSearcher.SavedColumns.ToString();
			Settings.Default.ColumnsLabel = LabelSearcher.SavedColumns.ToString();
			Settings.Default.ColumnsRepository = RepositorySearcher.SavedColumns.ToString();
			Settings.Default.ColumnsUser = UserSearcher.SavedColumns.ToString();
		}

		private void SaveSettings()
		{
			SaveWindowSettings();
			SaveColumnSettings();
			Settings.Default.Save();
		}

		private void SaveWindowSettings()
		{
			if (WindowState == FormWindowState.Normal)
			{
				Settings.Default.WindowLocation = Location;
				Settings.Default.WindowSize = Size;
			}

			if (WindowState != FormWindowState.Minimized)
				Settings.Default.WindowState = WindowState;
		}

		private void Search<TBroker>()
			where TBroker : ISearchBroker, new()
		{
			var broker = new TBroker();
			using (var dialog = new SearchCriteriaForm { SelectedObject = broker })
				if (dialog.ShowDialog(this) == DialogResult.OK)
					BeginWork(BackgroundType.Search, CreateSearcher(broker));
		}

		private async void Search2<TBroker>()
	where TBroker : ISearchBroker, new()
		{
			repos.Clear();
			paths.Clear();
			var broker = new SearchRepositoriesBroker();
			//using (var dialog = new SearchCriteriaForm { SelectedObject = broker })
			//	if (dialog.ShowDialog(this) == DialogResult.OK)
			
			broker.Term = "IdealAutomatex";
			searchResult = await CreateSearcher(broker).SearchAsync();
			//searchResult = searcher.Search();
            foreach (var item in ((SearchResult<Octokit.Repository>)(searchResult)).Items)
            {
				repos.Add(item.Owner.Login + "/" + item.Name);
				Console.WriteLine(item.Name);
            }
			//	BeginWork(BackgroundType.Search, CreateSearcher(broker));
			// ((SearchResult<Octokit.Repository>)(searchResult)).Items


			var brokerx = new SearchCodeBroker();
			using (var dialog = new SearchCriteriaForm { SelectedObject = brokerx })				
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{				
					int totalCount = 0;
					foreach (var item in repos)
					{
						brokerx.Repos = item;
						searchResult = await CreateSearcher(brokerx).SearchAsync();
						//searchResult = searcher.Search();
						foreach (var itemx in ((SearchResult<Octokit.SearchCode>)(searchResult)).Items)
						{
							totalCount++;
							paths.Add(itemx);
							// paths.Add(itemx.Repository.FullName + "/" + itemx.Path);
							Console.WriteLine(itemx.Repository.FullName + "/" + itemx.Path);
						}

					}
					SearchResult<Octokit.SearchCode> bigSearchResult = new SearchResult<SearchCode>(totalCount, false,
					new ReadOnlyCollection<SearchCode>(paths));

					mainDataGridView.DataSource = null;
					this.searcher = CreateSearcher(brokerx);
					mainDataGridView.DataSource = bigSearchResult.DataSource;
					


				}
			CompleteSearch();
		}
		
		

		private void Search(Searcher searcher)
		{
			searchResult = searcher.Search();
			this.searcher = searcher;
		}

		private void ShowColumnForm()
		{
			using (var dialog = new ColumnForm { InitialColumns = searcher.Columns })
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					searcher.Columns = dialog.Columns;
					AddColumns();
					SaveColumnSettings();
					Settings.Default.Save();
				}
			}
		}

		private void ShowLoginForm()
		{
			CreateClient(null);
			while (client == null)
				using (var dialog = new LoginForm())
					if (dialog.ShowDialog(this) == DialogResult.OK)
						CreateClient(dialog.Credentials);
					else
					{
						Close();
						return;
					}

			EnableUI(true);
		}

		private void UpdateDetail()
		{
			DataGridViewSelectedRowCollection rows = mainDataGridView.SelectedRows;
			object item = rows.Count > 0 ? rows[0].DataBoundItem : null;

			detailPropertyGrid.SelectedObject = item == null ?
				null : new TypeBroker(item);

			EnableDetailUi(true);
		}

		private void UpdateProgress()
		{
			if (!mainBackgroundWorker.IsBusy || activeSearcher == null)
				return;

			string type = activeSearcher.Type;

			int? totalCount = activeSearcher.TotalCount;
			if (!totalCount.HasValue)
				return;

			int count = activeSearcher.Count ?? 0;
			if (count == previousCount)
				return;

			int percent = totalCount.Value == 0 ? 0 : count * 100 / totalCount.Value;
			if (mainProgressBar.Value != percent)
				mainProgressBar.Value = percent;

			mainStatusLabel.Text = $"Searching for {type} ({count} of {totalCount.Value} results)...";
			previousCount = count;
		}

		private bool UpgradeSettings()
		{
			if (Settings.Default.IsUpgraded)
				return false;

			Settings.Default.Upgrade();
			Settings.Default.IsUpgraded = true;
			return true;
		}
		#endregion // Private methods

		#region Private enums
		private enum BackgroundType
		{
			None,
			Search,
			Detail
		}
        #endregion // Private enums

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
			DataGridViewSelectedRowCollection rows = mainDataGridView.SelectedRows;
			object item = rows.Count > 0 ? rows[0].DataBoundItem : null;

            string url = @"https://raw.githubusercontent.com/harvey007y/IdealAutomatex-harvey007y/main/" + ((Octokit.SearchCode)(item)).Path;
            // Create an instance of WebClient
            WebClient client = new WebClient();
            // Hookup DownloadFileCompleted Event
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

            // Start the download and copy the file to c:\temp
            client.DownloadFileAsync(new Uri(url), @"c:\data\atestdownload.txt");
        }
		void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			MessageBox.Show("File downloaded");
		}

		void client_DownloadFileCompleted2(object sender, AsyncCompletedEventArgs e)
		{
			Run();
		}

		private void downloadAndRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
			DataGridViewSelectedRowCollection rows = mainDataGridView.SelectedRows;
			object item = rows.Count > 0 ? rows[0].DataBoundItem : null;

			string url = @"https://raw.githubusercontent.com/harvey007y/IdealAutomatex-harvey007y/main/" + ((Octokit.SearchCode)(item)).Path;
			// Create an instance of WebClient
			WebClient client = new WebClient();
			// Hookup DownloadFileCompleted Event
			client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted2);

			// Start the download and copy the file to c:\temp
			client.DownloadFileAsync(new Uri(url), @"c:\data\atestdownload.txt");
		}

		private void Run()
        {
			int intWindowHeight = 110;
			int _maxLineChars = 0;
			string strBalloonArrowDirection = "NONE";
			double windowWidth = 0;
			System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
			List<string> _myContentList = new List<string>();
			string _PositionType = "Absolute";
			string _RelativeTop = "0";
			string _RelativeLeft = "0";
			string _RelativeFullFileName = "";
			string relativeCodeSnippet = "";

			bool boolRunningFromHome = false;
			//var window = new Window() //make sure the window is invisible
			//{
			//	Width = 0,
			//	Height = 0,
			//	Left = -2000,
			//	WindowStyle = WindowStyle.None,
			//	ShowInTaskbar = false,
			//	ShowActivated = false,
			//};
			//window.Show();
			//IdealAutomate.Core.Methods myActions = new Methods();
			//myActions.ScriptStartedUpdateStats();
			StringBuilder sb = new StringBuilder();


			InitializeComponent();
			this.Hide();

			//string strWindowTitle = myActions.PutWindowTitleInEntity();
			//if (strWindowTitle.StartsWith("MultipleWindowControls"))
			//{
			//	myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
			//}
			//myActions.Sleep(1000);


			using (StreamReader reader = File.OpenText(@"C:\Data\atestdownload.txt"))
			{
				sb.Clear();
				while (!reader.EndOfStream)
				{
					string myLine = reader.ReadLine();
					sb.AppendLine(myLine);
				}
			}

			if (intWindowHeight > 700)
			{
				intWindowHeight = 700;
			}
			int startPointXTemp = startPoint.X;
			int startPointYTemp = startPoint.Y;
			double windowWidthTemp = windowWidth * 2;
			if (windowWidthTemp > 1000)
			{ windowWidthTemp = 1000; }
			if (windowWidthTemp < 500)
			{ windowWidthTemp = 500; }
			if (_PositionType == "Relative")
			{
				startPointXTemp = 0;
				startPointYTemp = 0;
			}
			else
			{
				startPointXTemp = startPoint.X;
				startPointYTemp = startPoint.Y;
			}
			// http://www.codeproject.com/Tips/715891/Compiling-Csharp-Code-at-Runtime
			string code = sb.ToString();



			CSharpCodeProvider provider = new CSharpCodeProvider();
			CompilerParameters parameters = new CompilerParameters();
			// Reference to System.Drawing library
			parameters.ReferencedAssemblies.Add("System.Drawing.dll");
			parameters.ReferencedAssemblies.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"IdealAutomate\IdealAutomateCore.dll"));
			parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\PresentationFramework.dll");
			parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\PresentationCore.dll");
			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add("System.Core.dll");
			parameters.ReferencedAssemblies.Add("System.Data.dll");
			parameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
			parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
			parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			parameters.ReferencedAssemblies.Add("System.Xaml.dll");
			parameters.ReferencedAssemblies.Add("System.Xml.dll");
			parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\WindowsBase.dll");


			// True - memory generation, false - external file generation
			parameters.GenerateInMemory = true;
			// True - exe file generation, false - dll file generation
			parameters.GenerateExecutable = true;
			CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
			if (results.Errors.HasErrors)
			{
				StringBuilder sb5 = new StringBuilder();

				foreach (CompilerError error in results.Errors)
				{
					sb5.AppendLine(String.Format("Error ({0}): {1} Line: {2}", error.ErrorNumber, error.ErrorText, error.Line));
				}

				MessageBox.Show(sb5.ToString());
			}
			Assembly assembly = results.CompiledAssembly;
			Type program = assembly.GetType("First.Program");
			MethodInfo main = program.GetMethod("Main");
			main.Invoke(null, null);




			// Done --------------------
			//if (intWindowHeight > 700)
			//{
			//	intWindowHeight = 700;
			//}


			

		}

        private void idealAutomatexCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
			//LoadSettings();
			//BeginInvoke((MethodInvoker)ShowLoginForm);
			//	ShowLoginForm();
			
			Search2<SearchRepositoriesBroker>();
		}
    }
}
