using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;

namespace GitHubApiDemo
{
	/// <summary>
	/// A type converter for <see langword="enum"/> lists.
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	public class EnumListTypeConverter<TEnum> : TypeConverter
		where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		#region Constructors
		public EnumListTypeConverter()
		{
			if (!typeof(TEnum).IsEnum)
				throw new InvalidOperationException();
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
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
			typeof(string).IsAssignableFrom(sourceType) ||
			typeof(IList<TEnum>).IsAssignableFrom(sourceType) ||
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
			typeof(IList<TEnum>).IsAssignableFrom(destinationType) ||
			base.CanConvertFrom(context, destinationType);

		/// <summary>
		/// Convert the specified value to the type of this converter.
		/// </summary>
		/// <param name="context">The contextual information for the conversion.</param>
		/// <param name="culture">The culture used during the conversion.</param>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null)
			{
				if (value is string text)
					return ToList(text);

				if (value is IList<TEnum> list)
					return list;
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
			if (typeof(IList<TEnum>).IsAssignableFrom(destinationType))
			{
				if (value == null)
					return new List<TEnum>();

				if (value is IList<TEnum> list)
					return list;

				if (value is string text)
					return ToList(text);
			}

			if (typeof(string).IsAssignableFrom(destinationType))
			{
				if (value == null)
					return string.Empty;

				if (value is IList<TEnum> list)
					return ToString(list);

				if (value is string text)
					return text;
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion // Public methods

		#region Private methods
		private static IList<TEnum> ToList(string text)
		{
			TEnum prevPart = default(TEnum);
			var list = new List<TEnum>();
			bool isFirst = true;

			IEnumerable<TEnum> parts = text
				.Split(',')
				.Where(part => !string.IsNullOrWhiteSpace(part))
				.OrderBy(part => part, StringComparer.InvariantCultureIgnoreCase)
				.Select(part => (TEnum)Enum.Parse(typeof(TEnum), part.Trim(), true));

			foreach (TEnum part in parts)
			{
				if (!isFirst && part.Equals(prevPart))
					continue;

				list.Add(prevPart = part);
				isFirst = false;
			}

			return list;
		}

		private static string ToString(IEnumerable<TEnum> list) =>
			string.Join(", ", list
				.Distinct()
				.Select(item => item.ToString())
				.OrderBy(text => text, StringComparer.InvariantCultureIgnoreCase));
		#endregion // Private methods
	}
}
