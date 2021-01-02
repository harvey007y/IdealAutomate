using System;
using System.ComponentModel;
using System.Globalization;

namespace GitHubApiDemo
{
	/// <summary>
	/// A type converter to convert dates to/from the text format used by the GitHub API.
	/// </summary>
	public class DateConverter : TypeConverter
	{
		#region Public methods
		/// <summary>
		/// Gets a value indicating if this converter can convert the specified source type
		/// into the type for this converter.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="sourceType">The source type to test.</param>
		/// <returns>True, if the conversion is possible; otherwise, false.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
			typeof(string).IsAssignableFrom(sourceType) ||
			typeof(DateTime?).IsAssignableFrom(sourceType) ||
			base.CanConvertFrom(context, sourceType);

		/// <summary>
		/// Gets a value indicating if this converter can convert the type for this converter
		/// into the specified destination type.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="destinationType">The destination type to test.</param>
		/// <returns>True, if the conversion is possible; otherwise, false.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
			typeof(string).IsAssignableFrom(destinationType) ||
			typeof(DateTime?).IsAssignableFrom(destinationType) ||
			base.CanConvertTo(context, destinationType);

		/// <summary>
		/// Convert the specified value to the type of this converter.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="culture">The culture used during the conversion.</param>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null || value is DateTime?)
				return (DateTime?)value;

			if (value is string text)
			{
				if (string.IsNullOrWhiteSpace(text))
					return (DateTime?)null;

				if (DateTime.TryParse(text, out DateTime dateTime))
					return (DateTime?)dateTime;
			}

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
			if (value == null)
			{
				if (destinationType.IsAssignableFrom(typeof(DateTime?)))
					return (DateTime?)null;

				if (destinationType.IsAssignableFrom(typeof(string)))
					return string.Empty;
			}

			var dateTime = value as DateTime?;
			if (dateTime != null)
			{
				if (destinationType.IsAssignableFrom(typeof(DateTime?)))
					return dateTime;

				if (destinationType.IsAssignableFrom(typeof(string)))
					return ToString(dateTime.Value);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Get a text representation of the specified value in the format used by the GitHub API.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A text representation of the specified value in the format used by the GitHub API.</returns>
		public static string ToString(DateTime value) =>
			value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		#endregion // Public methods
	}
}
