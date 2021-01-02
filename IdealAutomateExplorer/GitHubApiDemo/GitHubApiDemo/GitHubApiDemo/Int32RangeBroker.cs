using System.Globalization;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A class that exposes properties of a <see cref="Range"/>, in a controlled fashion,
	/// to the <see cref="System.Windows.Forms.PropertyGrid"/> control.
	/// </summary>
	public class Int32RangeBroker : RangeBroker<int, Range>
	{
		#region Constructors
		/// <summary>
		///  Create the default range (<see cref="SearchQualifierValue.None"/>).
		/// </summary>
		public Int32RangeBroker()
		{
		}

		/// <summary>
		/// Create a new range based on the values of another range.
		/// </summary>
		/// <param name="other">The range upon which the values are based.</param>
		public Int32RangeBroker(Int32RangeBroker other)
			: base(other)
		{
		}
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Gets the default value for <see cref="From"/>.
		/// </summary>
		protected override int DefaultFromValue => 0;
		#endregion // Properties

		#region Protected methods
		/// <summary>
		/// Create a range equal to the specified value.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>A range equal to the specified value.</returns>
		protected override Range CreateRange(int value) =>
			new Range(value);

		/// <summary>
		/// Create a range that compares the specified value using the specified operator.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <param name="op">The operator used to compare the value.</param>
		/// <returns>A range that compares the specified value using the specified operator.</returns>
		protected override Range CreateRange(int value, SearchQualifierOperator op) =>
			new Range(value, op);

		/// <summary>
		/// Create a range between the specified values (inclusive).
		/// </summary>
		/// <param name="from">The lower limit (inclusive).</param>
		/// <param name="to">The upper limit (inclusive).</param>
		/// <returns>A range between the specified values (inclusive).</returns>
		protected override Range CreateRange(int from, int to) =>
			new Range(from, to);

		/// <summary>
		/// Get a text representation of the specified value.
		/// </summary>
		/// <param name="value">The value to convert to text.</param>
		/// <returns>A text representation of the specified value.</returns>
		protected override string ToString(int value) =>
			value.ToString(CultureInfo.InvariantCulture);
		#endregion // Protected methods
	}
}
