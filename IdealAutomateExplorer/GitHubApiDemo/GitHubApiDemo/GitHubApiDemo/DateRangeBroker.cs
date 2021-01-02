using System;
using System.ComponentModel;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A class that exposes properties of a <see cref="DateRange"/>, in a controlled fashion,
	/// to the <see cref="System.Windows.Forms.PropertyGrid"/> control.
	/// </summary>
	public class DateRangeBroker : RangeBroker<DateTime, DateRange>
	{
		#region Constructors
		/// <summary>
		///  Create the default range (<see cref="SearchQualifierValue.None"/>).
		/// </summary>
		public DateRangeBroker()
		{
		}

		/// <summary>
		/// Create a new range based on the values of another range.
		/// </summary>
		/// <param name="other">The range upon which the values are based.</param>
		public DateRangeBroker(DateRangeBroker other)
			: base(other)
		{
		}
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Gets or sets the minimum value (or value to compare against), which is ignored if
		/// <see cref="Qualifier"/> is <see cref="SearchQualifierValue.None"/>.
		/// </summary>
		/// <remarks>
		/// Over-ridden to add a type converter.
		/// </remarks>
		[TypeConverter(typeof(DateConverter))]
		public override DateTime? From
		{
			get => base.From;
			set => base.From = value;
		}

		/// <summary>
		/// Gets or sets the maximum value, which is ignored unless
		/// <see cref="Qualifier"/> is <see cref="SearchQualifierValue.Between"/>.
		/// </summary>
		/// <remarks>
		/// Over-ridden to add a type converter.
		/// </remarks>
		[TypeConverter(typeof(DateConverter))]
		public override DateTime? To
		{
			get => base.To;
			set => base.To = value;
		}

		/// <summary>
		/// Gets the default value for <see cref="From"/>.
		/// </summary>
		protected override DateTime DefaultFromValue =>
			DateTime.Now;
		#endregion // Properties

		#region Protected methods
		/// <summary>
		/// Create a range equal to the specified value.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>A range equal to the specified value.</returns>
		protected override DateRange CreateRange(DateTime value) =>
			new DateRange(value, value);

		/// <summary>
		/// Create a range that compares the specified value using the specified operator.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <param name="op">The operator used to compare the value.</param>
		/// <returns>A range that compares the specified value using the specified operator.</returns>
		protected override DateRange CreateRange(DateTime value, SearchQualifierOperator op) =>
			new DateRange(value, op);

		/// <summary>
		/// Create a range between the specified values (inclusive).
		/// </summary>
		/// <param name="from">The lower limit (inclusive).</param>
		/// <param name="to">The upper limit (inclusive).</param>
		/// <returns>A range between the specified values (inclusive).</returns>
		protected override DateRange CreateRange(DateTime from, DateTime to) =>
			new DateRange(from, to);

		/// <summary>
		/// Get a text representation of the specified value.
		/// </summary>
		/// <param name="value">The value to convert to text.</param>
		/// <returns>A text representation of the specified value.</returns>
		protected override string ToString(DateTime value) =>
			DateConverter.ToString(value);
		#endregion // Protected methods
	}
}
