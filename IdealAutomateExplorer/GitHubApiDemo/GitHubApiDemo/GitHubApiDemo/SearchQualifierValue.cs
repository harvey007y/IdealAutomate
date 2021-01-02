using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// An enumeration of possible comparison operators.
	/// </summary>
	public enum SearchQualifierValue
	{
		None = -3,
		Between = -2,
		Equals = -1,
		GreaterThan = SearchQualifierOperator.GreaterThan,
		LessThan = SearchQualifierOperator.LessThan,
		LessThanOrEqualTo = SearchQualifierOperator.LessThanOrEqualTo,
		GreaterThanOrEqualTo = SearchQualifierOperator.GreaterThanOrEqualTo
	}
}
