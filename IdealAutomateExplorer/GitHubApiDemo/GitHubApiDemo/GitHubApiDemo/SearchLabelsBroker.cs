using System.ComponentModel;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker that exposes properties of the <see cref="SearchLabelsRequest"/>,
	/// in a controlled fashion, to the <see cref="System.Windows.Forms.PropertyGrid"/>
	/// control.
	/// </summary>
	public class SearchLabelsBroker : SearchBroker<SearchLabelsRequest>
	{
		#region Constructors
		/// <summary>
		/// Create an instance from the previous search criteria.
		/// </summary>
		public SearchLabelsBroker()
			: this(previous)
		{
		}

		private SearchLabelsBroker(SearchLabelsBroker other)
			: base(other)
		{
			if (other == null)
				return;

			RepositoryId = other.RepositoryId;
			SortField = other.SortField;
		}
		#endregion // Constructors

		#region Private data
		private static SearchLabelsBroker previous = new SearchLabelsBroker(null);
		#endregion // Private data

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(RepositoryId), typeof(SearchLabelsBroker))]
		[DefaultValue(0)]
		public long RepositoryId { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(SortField), typeof(SearchLabelsBroker))]
		[DefaultValue(null)]
		public LabelSearchSort? SortField { get; set; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Create a searcher.
		/// </summary>
		/// <returns>The searcher that is created.</returns>
		public override Searcher CreateSearcher(GitHubClient client, int maximumCount)
		{
			previous = this;
			return new LabelSearcher(client, maximumCount, CreateRequest());
		}

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			RepositoryId = 0;
			SortField = null;
		}
		#endregion // Public methods

		#region Protected methods
		/// <summary>
		/// Create a request with no search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchLabelsRequest CreateNoTermRequest() =>
			new SearchLabelsRequest
			{
				RepositoryId = RepositoryId,
				Order = Order,
				SortField = SortField
			};

		/// <summary>
		/// Create a request with a search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchLabelsRequest CreateTermRequest(string term) =>
			new SearchLabelsRequest(term, RepositoryId)
			{
				Order = Order,
				SortField = SortField
			};
		#endregion // Protected methods
	}
}
