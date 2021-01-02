using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Linq;
using System.ComponentModel;

namespace GitHubApiDemo
{
	/// <summary>
	/// UI editor for list of <see langword="enum"/> values.
	/// </summary>
	/// <typeparam name="TEnum">The type of <see langword="enum"/>.</typeparam>
	public class EnumListUITypeEditor<TEnum> : UITypeEditor
		where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		#region Constructors
		/// <summary>
		/// Create a new instance.
		/// </summary>
		public EnumListUITypeEditor()
		{
			Type type = typeof(TEnum);
			if (!type.IsEnum)
				throw new InvalidOperationException();

			nameValues = GetNameValues(type)
				.OrderBy(nameValue => nameValue.Name)
				.ToArray();
		}
		#endregion // Constructors

		#region Private data
		private readonly NameValue[] nameValues;
		#endregion // Private data

		#region Public methods
		/// <summary>
		/// Get the editor style.
		/// </summary>
		/// <param name="context">The contextual information for this editor.</param>
		/// <returns>The editor style (<see cref="UITypeEditorEditStyle.DropDown"/>).</returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) =>
			UITypeEditorEditStyle.DropDown;

		/// <summary>
		/// Edit the value.
		/// </summary>
		/// <param name="context">The contextual information for this editor.</param>
		/// <param name="provider">The service provider for this editor.</param>
		/// <param name="value">The value to edit.</param>
		/// <returns>The edited value.</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,
			object value)
		{
			var list = value as IList<TEnum>;

			var service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			if (service == null || list == null)
				throw new InvalidOperationException();

			CheckedListBox listBox = CreateListBox(list);

			service.DropDownControl(listBox);

			return CreateList(listBox);
		}
		#endregion // Public methods

		#region Private methods
		// Check the specified value in the list box
		private void CheckItem(CheckedListBox listBox, TEnum value)
		{
			int length = nameValues.Length;
			for (int index = 0; index < length; index++)
				if (nameValues[index].Value.Equals(value))
					listBox.SetItemChecked(index, true);
		}

		// Check the specified values in the list box
		private void CheckItems(CheckedListBox listBox, IEnumerable<TEnum> values)
		{
			foreach (TEnum value in values)
				CheckItem(listBox, value);
		}

		// Update the list to include only checked items
		private IList<TEnum> CreateList(CheckedListBox listBox) =>
			listBox.CheckedItems
				.Cast<NameValue>()
				.Select(nameValue => nameValue.Value)
				.ToList();

		// Create a list box, checking the specified values
		private CheckedListBox CreateListBox(IEnumerable<TEnum> values)
		{
			var listBox = new CheckedListBox { CheckOnClick = true };
			listBox.Items.AddRange(nameValues);
			CheckItems(listBox, values);
			return listBox;
		}

		// Get the name/value pairs for the specified enum type
		private static IEnumerable<NameValue> GetNameValues(Type type) =>
			Enum.GetNames(type)
				.Zip(Enum.GetValues(type).Cast<TEnum>(),
					(name, value) => new NameValue(name, value));
		#endregion // Private methods

		#region Nested types
		// A simple class to hold a name/value pair and display the name when ToString is called
		private class NameValue
		{
			public NameValue(string name, TEnum value)
			{
				Name = name;
				Value = value;
			}

			public string Name { get; }

			public TEnum Value { get; }

			public override string ToString() =>
				Name;
		}
		#endregion // Nested types
	}
}
