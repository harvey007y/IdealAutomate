using System.ComponentModel;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// The base class for all search brokers.
	/// </summary>
	/// <typeparam name="TRequest">The type of request created by this search broker.</typeparam>
	[DefaultProperty(nameof(Term))]
	public abstract class SearchBroker<TRequest> : Broker, ISearchBroker
		where TRequest : BaseSearchRequest
	{
		#region Constructors
		/// <summary>
		/// Create an instance.
		/// </summary>
		protected SearchBroker(SearchBroker<TRequest> other = null)
		{
			if (other == null)
				return;

			Order = other.Order;
			Term = other.Term;
		}
		#endregion // Constructors

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Order), typeof(SearchBroker<>))]
		[DefaultValue(typeof(SortDirection), nameof(SortDirection.Ascending))]
		public virtual SortDirection Order { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Term), typeof(SearchBroker<>))]
		[DefaultValue(null)]
		public virtual string Term { get; set; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Create a searcher.
		/// </summary>
		/// <returns>The searcher that is created.</returns>
		public abstract Searcher CreateSearcher(GitHubClient client, int maximumCount);

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public virtual void Reset()
		{
			Order = SortDirection.Ascending;
			Term = null;
		}
		#endregion // Public methods

		#region Protected methods
		/// <summary>
		/// Create a request.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected TRequest CreateRequest() =>
			string.IsNullOrWhiteSpace(Term) ? CreateNoTermRequest() : CreateTermRequest(Term);

		/// <summary>
		/// Create a request with no search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected abstract TRequest CreateNoTermRequest();

		/// <summary>
		/// Create a request with a search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected abstract TRequest CreateTermRequest(string term);
		#endregion // Protected methods
	}
}
