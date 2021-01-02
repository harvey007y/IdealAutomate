using System.Collections;

namespace GitHubApiDemo
{
	/// <summary>
	/// Extension methods for <see cref="IList"/>.
	/// </summary>
	public static class IListExtensions
	{
		/// <summary>
		/// Return this list or <see langword="null"/> if this list is empty.
		/// </summary>
		/// <typeparam name="TList">The type of list.</typeparam>
		/// <param name="list">The list that is extended by this method.</param>
		/// <returns>This list or <see langword="null"/> if this list is empty.</returns>
		public static TList NullIfEmpty<TList>(this TList list) where TList : class, IList =>
			list.Count > 0 ? list : null;
	}
}
