using System.Threading.Tasks;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A class that uses the <see cref="ISearchClient.SearchRepo(SearchRepositoriesRequest)"/>,
	/// from the GitHub API, to aggregate one or more pages of search results.
	/// </summary>
	public class RepositorySearcher : Searcher<SearchRepositoriesRequest, Repository>
	{
		#region Constructor
		/// <summary>
		/// Create a searcher for the specified client, limited to the specified number of items,
		/// with the specified search criteria.
		/// </summary>
		/// <param name="client">The GitHub client.</param>
		/// <param name="maximumCount">The maximum number of items to return.</param>
		/// <param name="request">The request/search criteria.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="client"/> or <paramref name="request"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="maximumCount"/> is less than or equal to zero.
		/// </exception>
		public RepositorySearcher(GitHubClient client, int maximumCount,
			SearchRepositoriesRequest request)
			: base(client, maximumCount, request)
		{
		}
		#endregion // Constructor

		#region Public data
		/// <summary>
		/// The default set of column names selected for this type of searcher.
		/// </summary>
		public static readonly ColumnSet DefaultColumns = new ColumnSet(typeof(Repository), Depth,
			nameof(Repository.Id), nameof(Repository.FullName), nameof(Repository.Description));
		#endregion // Public data

		#region Private data
		private static ColumnSet columns = DefaultColumns;
		#endregion // Private data

		#region Properties
		/// <summary>
		/// Gets a value indicating if this search can get additional detail.
		/// </summary>
		public override bool CanGetDetail =>
			true;

		/// <summary>
		/// Gets or sets the columns that are available/selected for this searcher.
		/// </summary>
		public override ColumnSet Columns
		{
			get => columns;
			set => columns = value ?? DefaultColumns;
		}

		/// <summary>
		/// Gets or sets the columns that are available/selected for this type of searcher.
		/// </summary>
		public static ColumnSet SavedColumns
		{
			get => columns;
			set => columns = value ?? DefaultColumns;
		}

		/// <summary>
		/// Gets the type of entity that is the target of this search.
		/// </summary>
		public override string Type => "repositories";
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Reset the selected column names for this type of searcher.
		/// </summary>
		public override void ResetColumns() =>
			columns = DefaultColumns;
		#endregion // Public methods

		#region Protected methods
		/// <summary>
		/// Get the next page of search results asynchronously.
		/// </summary>
		/// <returns>The next page of search results.</returns>
		protected override async Task<SearchResultsPage> GetNextPageAsync()
		{
			SearchRepositoryResult result = await Client.Search.SearchRepo(Request);
			return new SearchResultsPage(result.TotalCount, result.IncompleteResults, result.Items);
		}

		/// <summary>
		/// Asynchronously get the full detail for the specified value.
		/// </summary>
		/// <param name="value">The value for which full detail is obtained.</param>
		/// <returns>The full detail for the specified value.</returns>
		protected override async Task<object> GetDetailInternal(object value) =>
			value is Repository repository ? await Client.Repository.Get(repository.Id) : value;
		#endregion // Protected methods
	}
}
