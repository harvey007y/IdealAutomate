using System.Collections.Generic;
using System.Linq;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// The base class for brokers that expose properties to a 
	/// <see cref="System.Windows.Forms.PropertyGrid"/> control.
	/// </summary>
	public abstract class Broker
	{
		#region Constructors
		/// <summary>
		/// Create an instance.
		/// </summary>
		protected Broker()
		{
		}
		#endregion // Constructors

		#region Private data
		private static readonly string[] emptyParts = new string[0];
		#endregion // Private data

		#region Protected methods
		/// <summary>
		/// Get the non-empty items in a text string, where individual items are
		/// separated by the specified character.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <param name="separator">The (optional) separator character (default ',').</param>
		/// <returns>The non-empty parts of the list.</returns>
		static protected IEnumerable<string> GetNonEmptyItems(string text, char separator = ',') =>
			text?.Split(separator)
				.Where(item => !string.IsNullOrWhiteSpace(item))
				.Select(part => part.Trim()) ?? emptyParts;

		/// <summary>
		/// Get a repository collection from a text string.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <returns>The repository collection (or null).</returns>
		static protected RepositoryCollection GetRepositories(string text)
		{
			var repositories = new RepositoryCollection();
			foreach (string part in GetNonEmptyItems(text))
				repositories.Add(part);

			return repositories.Count > 0 ? repositories : null;
		}

		/// <summary>
		/// Get a list of the non-empty items in a text string, where individual items are
		/// separated by the specified character.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <param name="separator">The (optional) separator character (default ',').</param>
		/// <returns>The list that was created (or <see langword="null"/>) if the list is empty.</returns>
		protected List<string> ToList(string text, char separatore = ',') =>
			GetNonEmptyItems(text).ToList().NullIfEmpty();
		#endregion // Protected methods
	}
}
