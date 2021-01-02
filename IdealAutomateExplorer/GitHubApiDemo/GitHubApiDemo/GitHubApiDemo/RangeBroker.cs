using System;
using System.ComponentModel;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A class that exposes properties of a range of values in a controlled fashion,
	/// to the <see cref="System.Windows.Forms.PropertyGrid"/> control.
	/// </summary>
	/// <typeparam name="TLimit">The type of limit on the range.</typeparam>
	/// <typeparam name="TRange">The type of range.</typeparam>
	public abstract class RangeBroker<TLimit, TRange>
		where TLimit : struct, IComparable
		where TRange : class
	{
		#region Constructors
		/// <summary>
		///  Create the default range (<see cref="SearchQualifierValue.None"/>).
		/// </summary>
		protected RangeBroker()
		{
		}

		/// <summary>
		/// Create a new range based on the values of another range.
		/// </summary>
		/// <param name="other">The range upon which the values are based.</param>
		protected RangeBroker(RangeBroker<TLimit, TRange> other)
		{
			Qualifier = other.Qualifier;
			From = other.From;
			To = other.To;
		}
		#endregion // Constructors

		#region Private data
		private SearchQualifierValue qualifier = SearchQualifierValue.None;
		private TLimit? from;
		private TLimit? to;
		#endregion // Private data

		#region Properties
		/// <summary>
		/// Gets or sets the minimum value (or value to compare against), which is ignored if
		/// <see cref="Qualifier"/> is <see cref="SearchQualifierValue.None"/>.
		/// </summary>
		[Category(CategoryText.Behavior)]
		[Description("The minimum value (or value to compare against), which is ignored if " +
			nameof(Qualifier) + " is " + nameof(SearchQualifierValue.None) + ".")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public virtual TLimit? From
		{
			get => from;
			set
			{
				if (qualifier != SearchQualifierValue.None)
					from = value ?? DefaultFromValue;
			}
		}

		/// <summary>
		/// Gets or sets a value that qualifies the range (e.g. less than, greater than, etc.).
		/// </summary>
		[Category(CategoryText.Behavior)]
		[Description("A value that qualifies the range (e.g. less than, greater than, etc.).")]
		[DefaultValue(typeof(SearchQualifierValue), "None")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public SearchQualifierValue Qualifier
		{
			get => qualifier;
			set
			{
				if (qualifier == value)
					return;

				qualifier = value;
				if (qualifier != SearchQualifierValue.None)
					from = from ?? DefaultFromValue;
				else
					from = null;

				qualifier = value;
				if (qualifier == SearchQualifierValue.Between)
					to = from;
				else
					to = null;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value, which is ignored unless
		/// <see cref="Qualifier"/> is <see cref="SearchQualifierValue.Between"/>.
		/// </summary>
		[Category(CategoryText.Behavior)]
		[Description("The maximum value, which is ignored unless " +
			nameof(Qualifier) + " is " + nameof(SearchQualifierValue.Between) + ".")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public virtual TLimit? To
		{
			get => to;
			set
			{
				if (qualifier == SearchQualifierValue.Between)
					to = value != null && value.Value.CompareTo(from) > 0 ? value : from;
			}
		}

		/// <summary>
		/// Gets the default value for <see cref="From"/>.
		/// </summary>
		protected abstract TLimit DefaultFromValue { get; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Create the range from the properties of this class.
		/// </summary>
		/// <returns>The range created from the properties of this class.</returns>
		public TRange CreateRange()
		{
			switch (Qualifier)
			{
				case SearchQualifierValue.None: return null;
				case SearchQualifierValue.Between: return CreateRange(From.Value, To.Value);
				case SearchQualifierValue.Equals: return CreateRange(From.Value);
				default: return CreateRange(From.Value, (SearchQualifierOperator)Qualifier);
			}
		}

		/// <summary>
		/// Get a text representation of this instance.
		/// </summary>
		/// <returns>A text representation of this instance.</returns>
		public override string ToString()
		{
			switch (Qualifier)
			{
				case SearchQualifierValue.Between: return $"{ToString(From.Value)}..{ToString(To.Value)}";
				case SearchQualifierValue.Equals: return ToString(From.Value);
				case SearchQualifierValue.GreaterThan: return $">{ToString(From.Value)}";
				case SearchQualifierValue.LessThan: return $"<{ToString(From.Value)}";
				case SearchQualifierValue.LessThanOrEqualTo: return $"<={ToString(From.Value)}";
				case SearchQualifierValue.GreaterThanOrEqualTo: return $">={ToString(From.Value)}";
				default: return string.Empty;
			}
		}
		#endregion // Public methods

		/// The methods in this region are all named ResetXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ResetXXX methods for PropertyGrid
		public void ResetFrom() =>
			Qualifier = SearchQualifierValue.None;

		public void ResetTo() =>
			Qualifier = SearchQualifierValue.None;
		#endregion // ResetXXX methods for PropertyGrid

		/// The methods in this region are all named ShouldSerializeXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ShouldSerializeXXX methods for PropertyGrid
		public bool ShouldSerializeFrom() =>
			From.HasValue;

		public bool ShouldSerializeTo() =>
			To.HasValue;
		#endregion // ShouldSerializeXXX methods for PropertyGrid

		#region Protected methods
		/// <summary>
		/// Create a range equal to the specified value.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>A range equal to the specified value.</returns>
		protected abstract TRange CreateRange(TLimit value);

		/// <summary>
		/// Create a range that compares the specified value using the specified operator.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <param name="op">The operator used to compare the value.</param>
		/// <returns>A range that compares the specified value using the specified operator.</returns>
		protected abstract TRange CreateRange(TLimit value, SearchQualifierOperator op);

		/// <summary>
		/// Create a range between the specified values (inclusive).
		/// </summary>
		/// <param name="from">The lower limit (inclusive).</param>
		/// <param name="to">The upper limit (inclusive).</param>
		/// <returns>A range between the specified values (inclusive).</returns>
		protected abstract TRange CreateRange(TLimit from, TLimit to);

		/// <summary>
		/// Get a text representation of the specified value.
		/// </summary>
		/// <param name="value">The value to convert to text.</param>
		/// <returns>A text representation of the specified value.</returns>
		protected virtual string ToString(TLimit value) =>
			value.ToString();
		#endregion // Protected methods
	}
}
