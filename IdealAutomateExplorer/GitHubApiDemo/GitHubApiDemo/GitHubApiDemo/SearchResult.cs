using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace GitHubApiDemo
{
	/// <summary>
	/// The base class for search results.
	/// </summary>
	public abstract class SearchResult
	{
		#region Constructors
		/// <summary>
		/// Create an instance for the specified total count, incomplete indicator, and
		/// item type.
		/// </summary>
		/// <param name="totalCount">The total number of items in the search result.</param>
		/// <param name="incompleteResults">A value indicating if the search results are incomplete.</param>
		/// <param name="itemType">The type of items in the search results.</param>
		public SearchResult(int totalCount, bool incompleteResults, Type itemType = null)
		{
			IncompleteResults = incompleteResults;
			ItemType = itemType ?? typeof(object);
			TotalCount = totalCount >= 0 ? totalCount :
				throw new ArgumentOutOfRangeException(nameof(totalCount));
		}
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Gets a list that can be used as a data source for the search results.
		/// </summary>
		public abstract IList DataSource { get; }

		/// <summary>
		/// Gets a value indicating if the search results are incomplete.
		/// </summary>
		public bool IncompleteResults { get; }

		/// <summary>
		/// Gets the total number of items in the search results.
		/// </summary>
		public int TotalCount { get; }

		/// <summary>
		/// Gets the type of items in the search results.
		/// </summary>
		public Type ItemType { get; }
		#endregion // Properties
	}

	/// <summary>
	/// The type-safe, generic results of a search.
	/// </summary>
	/// <typeparam name="TItem">The type of item in the search results.</typeparam>
	/// <remarks>
	/// This servers the same purpose as <see cref="Octokit.Internal.SearchResult{T}"/>.
	/// However, since the former is in an "Internal" namespace, this author was reluctant to use it.
	/// </remarks>
	public class SearchResult<TItem> : SearchResult
	{
		#region Constructors
		/// <summary>
		/// Create an instance for the specified total count, incomplete indicator, and
		/// items.
		/// </summary>
		/// <param name="totalCount">The total number of items in the search result.</param>
		/// <param name="incompleteResults">A value indicating if the search results are incomplete.</param>
		public SearchResult(int totalCount, bool incompleteResults, ReadOnlyCollection<TItem> items)
			: base(totalCount, incompleteResults, typeof(TItem)) =>
			Items = items ?? throw new ArgumentNullException(nameof(items));
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Gets a list that can be used as a data source for the search results.
		/// </summary>
		public override IList DataSource => Items;

		/// <summary>
		/// Gets a read-only list of the items in the search results.
		/// </summary>
		public ReadOnlyCollection<TItem> Items { get; }
		#endregion // Properties
	}
}
