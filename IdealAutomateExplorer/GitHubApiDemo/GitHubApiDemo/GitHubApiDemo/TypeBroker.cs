using System;
using System.ComponentModel;
using System.Linq;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker, which implements a custom type descriptor, to expose properties of the
	/// specified component to the <see cref="System.Windows.Forms.PropertyGrid"/>.  For properties
	/// where the type is in the same namespace as the specified component, an attribute specifying
	/// a type converter of <see cref="ExpandableObjectConverter"/> is added.  This allows
	/// the sub-properties, for that property, to be visible in the property grid.
	/// </summary>
	public class TypeBroker : ICustomTypeDescriptor
	{
		#region Constructor
		/// <summary>
		/// Create a broker for the specified component where the properties are at the
		/// specified depth.
		/// </summary>
		/// <param name="actual">The actual component serviced by this instance.</param>
		/// <param name="depth">The depth of the properties for the actual component.</param>
		/// <remarks>
		/// The <paramref name="depth"/> value is used to limit the expansion of sub-properties.
		/// </remarks>
		public TypeBroker(object actual, int depth = 0)
		{
			Actual = actual ?? throw new ArgumentNullException(nameof(actual));
			Depth = depth >= 0 ? depth : throw new ArgumentOutOfRangeException(nameof(depth));
		}
		#endregion // Constructor

		#region Properties
		/// <summary>
		/// Gets the actual component serviced by this instance.
		/// </summary>
		public object Actual { get; }

		/// <summary>
		/// Gets the depth of the properties for the actual component.
		/// </summary>
		public int Depth { get; }
		#endregion // Properties

		#region ICustomTypeDescriptor members
		public AttributeCollection GetAttributes() =>
			TypeDescriptor.GetAttributes(Actual);

		public string GetClassName() =>
			TypeDescriptor.GetClassName(Actual);

		public string GetComponentName() =>
			TypeDescriptor.GetComponentName(Actual);

		public TypeConverter GetConverter() =>
			TypeDescriptor.GetConverter(Actual);

		public EventDescriptor GetDefaultEvent() =>
			TypeDescriptor.GetDefaultEvent(Actual);

		public PropertyDescriptor GetDefaultProperty() =>
			CreatePropertyBroker(TypeDescriptor.GetDefaultProperty(Actual));

		public object GetEditor(Type editorBaseType) =>
			TypeDescriptor.GetEditor(Actual, editorBaseType);

		public EventDescriptorCollection GetEvents() =>
			TypeDescriptor.GetEvents(Actual);

		public EventDescriptorCollection GetEvents(Attribute[] attributes) =>
			TypeDescriptor.GetEvents(Actual, attributes);

		/// <summary>
		/// Get the properties for the actual component, wrapping them in a property broker,
		/// so that we can make them read-only and allow them to be expanded (where appropriate).
		/// </summary>
		/// <returns>The properties of the actual component.</returns>
		public PropertyDescriptorCollection GetProperties() =>
			new PropertyDescriptorCollection(TypeDescriptor.GetProperties(Actual)
				.Cast<PropertyDescriptor>()
				.Select(CreatePropertyBroker)
				.ToArray());

		/// <summary>
		/// Get the properties for the actual component, wrapping them in a property broker,
		/// so that we can make them read-only and allow them to be expanded (where appropriate).
		/// </summary>
		/// <param name="attributes">The attributes used to filter the results.</param>
		/// <returns>The properties of the actual component.</returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes) =>
			new PropertyDescriptorCollection(TypeDescriptor.GetProperties(Actual, attributes)
				.Cast<PropertyDescriptor>()
				.Select(CreatePropertyBroker)
				.ToArray());

		/// <summary>
		/// Assume any properties are owned by the actual component.
		/// </summary>
		/// <param name="pd">The property descriptor.</param>
		/// <returns>The owner of the specified property.</returns>
		public object GetPropertyOwner(PropertyDescriptor pd) =>
			Actual;
		#endregion // ICustomTypeDescriptor members

		#region Private methods
		private PropertyDescriptorBroker CreatePropertyBroker(PropertyDescriptor property) =>
			property == null ? null : new PropertyDescriptorBroker(Actual, Depth, property);
		#endregion // Private methods
	}
}
