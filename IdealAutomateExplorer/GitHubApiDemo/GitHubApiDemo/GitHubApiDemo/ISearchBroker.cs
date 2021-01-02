using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker to create searches.
	/// </summary>
	public interface ISearchBroker
	{
		/// <summary>
		/// Create a searcher.
		/// </summary>
		/// <returns>The searcher that is created.</returns>
		Searcher CreateSearcher(GitHubClient client, int maximumCount);

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		void Reset();
	}
}
