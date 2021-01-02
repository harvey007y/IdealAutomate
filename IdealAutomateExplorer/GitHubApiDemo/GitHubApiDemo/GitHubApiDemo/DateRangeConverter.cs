using System;

namespace GitHubApiDemo
{
	/// <summary>
	/// A type converter for <see cref="DateRangeBroker"/>.
	/// </summary>
	public class DateRangeConverter : RangeConverter<DateTime, DateRangeBroker>
	{
		#region Protected methods
		/// <summary>
		/// Create a range from the specified information.
		/// </summary>
		/// <param name="qualifier">The qualifier for the range.</param>
		/// <param name="from">The lower limit (or value to compare) for the range.</param>
		/// <param name="to">The upper limit for the range.</param>
		/// <returns>The range that was created.</returns>
		protected override DateRangeBroker CreateRange(SearchQualifierValue qualifier, DateTime? from = null, DateTime? to = null) =>
			new DateRangeBroker
			{
				Qualifier = qualifier,
				From = from,
				To = to
			};

		/// <summary>
		/// Try to parse the specified text into a limit.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <param name="result">The resulting limit.</param>
		/// <returns>True, if successful; otherwise, false.</returns>
		protected override bool TryParse(string text, out DateTime result) =>
			DateTime.TryParse(text, out result);
		#endregion // Protected methods
	}
}
