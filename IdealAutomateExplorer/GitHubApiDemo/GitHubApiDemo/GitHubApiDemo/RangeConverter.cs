using System;
using System.ComponentModel;
using System.Globalization;

namespace GitHubApiDemo
{
	/// <summary>
	/// A type converter for range properties.
	/// </summary>
	/// <typeparam name="TLimit">The type of limit for the range.</typeparam>
	/// <typeparam name="TRange">The range.</typeparam>
	public abstract class RangeConverter<TLimit, TRange> : ExpandableObjectConverter
		where TLimit : struct, IComparable
		where TRange : class
	{
		#region Constructors
		/// <summary>
		/// Create an instance.
		/// </summary>
		protected RangeConverter()
		{
		}
		#endregion // Constructors

		#region Public methods
		/// <summary>
		/// Gets a value indicating if this converter can convert the specified source type
		/// into the type for this converter.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="sourceType">The source type to test.</param>
		/// <returns>True, if the conversion is possible; otherwise, false.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			bool canConvertFrom = typeof(string).IsAssignableFrom(sourceType) ||
				typeof(TRange).IsAssignableFrom(sourceType) ||
				base.CanConvertFrom(context, sourceType);
			return canConvertFrom;
		}

		/// <summary>
		/// Gets a value indicating if this converter can convert the type for this converter
		/// into the specified destination type.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="destinationType">The destination type to test.</param>
		/// <returns>True, if the conversion is possible; otherwise, false.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			bool canConvertTo = typeof(string).IsAssignableFrom(destinationType) ||
				typeof(TRange).IsAssignableFrom(destinationType) ||
				base.CanConvertTo(context, destinationType);
			return canConvertTo;
		}

		/// <summary>
		/// Convert the specified value to the type of this converter.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="culture">The culture used during the conversion.</param>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is TRange result ||
				(value is string text && TryParse(text, out result)))
				return result;

			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Convert the specified value to the specified type.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="culture">The culture used during the conversion.</param>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The destination type.</param>
		/// <returns>The converted value.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType.IsAssignableFrom(typeof(TRange)))
			{
				if (value == null || value is TRange)
					return (TRange)value;
			}

			if (destinationType.IsAssignableFrom(typeof(string)))
			{
				if (value == null)
					return string.Empty;

				if (value is TRange)
					return value.ToString();

				if (value is string)
					return value;
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Try to parse the specified text.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <param name="result">The resulting range.</param>
		/// <returns>True, if the text was parsed successfully; otherwise, false.</returns>
		public bool TryParse(string text, out TRange result)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				result = CreateRange(SearchQualifierValue.None);
				return true;
			}

			SearchQualifierValue qualifier = SearchQualifierValue.Equals;
			int fromLength = text.Length;
			int fromPosition = 0;
			int toPosition = -1;
			TLimit? to = null;

			switch (text[0])
			{
				case '>':
					fromPosition++;

					if (text.Length > 1 && text[1] == '=')
					{
						qualifier = SearchQualifierValue.GreaterThanOrEqualTo;
						fromPosition++;
					}
					else
						qualifier = SearchQualifierValue.GreaterThan;

					fromLength -= fromPosition;
					break;

				case '<':
					fromPosition++;

					if (text.Length > 1 && text[1] == '=')
					{
						qualifier = SearchQualifierValue.LessThanOrEqualTo;
						fromPosition++;
					}
					else
						qualifier = SearchQualifierValue.LessThan;

					fromLength -= fromPosition;
					break;

				case '=':
					qualifier = SearchQualifierValue.Equals;
					fromLength = text.Length - 1;
					fromPosition++;
					break;

				default:
					int index = text.IndexOf("..");

					if (index >= 0)
					{
						qualifier = SearchQualifierValue.Between;
						toPosition = index + 2;
						fromLength = index;
					}

					break;
			}

			string toText = toPosition > 0 ? text.Substring(toPosition).Trim() : null;
			string fromText = text.Substring(fromPosition, fromLength).Trim();

			if (!TryParse(fromText, out TLimit from))
			{
				result = null;
				return false;
			}

			if (toText != null)
				if (TryParse(toText, out TLimit tmp))
					to = tmp;
				else
				{
					result = null;
					return false;
				}

			result = CreateRange(qualifier, from, to);
			return true;
		}
		#endregion // Public methods

		#region Protected methods
		/// <summary>
		/// Create a range from the specified information.
		/// </summary>
		/// <param name="qualifier">The qualifier for the range.</param>
		/// <param name="from">The lower limit (or value to compare) for the range.</param>
		/// <param name="to">The upper limit for the range.</param>
		/// <returns>The range that was created.</returns>
		protected abstract TRange CreateRange(SearchQualifierValue qualifier,
			TLimit? from = null, TLimit? to = null);

		/// <summary>
		/// Try to parse the specified text into a limit.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <param name="result">The resulting limit.</param>
		/// <returns>True, if successful; otherwise, false.</returns>
		protected abstract bool TryParse(string text, out TLimit result);
		#endregion // Protected methods
	}
}
