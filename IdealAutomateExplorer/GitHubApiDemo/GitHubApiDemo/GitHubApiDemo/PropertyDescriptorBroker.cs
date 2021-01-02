using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker, which implements a custom property descriptor, to expose properties of the
	/// to the <see cref="System.Windows.Forms.PropertyGrid"/>.  For properties where the type is
	/// in the same namespace as the parent component, an attribute specifying
	/// a type converter of <see cref="ExpandableObjectConverter"/> is added.  This allows
	/// the sub-properties, for that property, to be visible in the property grid.
	/// </summary>
	public class PropertyDescriptorBroker : PropertyDescriptor
	{
		#region Constructors
		/// <summary>
		/// Create an instance for the specified actual component, at the specified depth, with
		/// the specified actual property, using the original property's attributes.
		/// </summary>
		/// <param name="actualComponent">The actual component that owns the property.</param>
		/// <param name="depth">The depth of the property.</param>
		/// <param name="actual">The actual property.</param>
		public PropertyDescriptorBroker(object actualComponent, int depth, PropertyDescriptor actual)
			: this(actualComponent, depth, actual, actual.Attributes?.Cast<Attribute>())
		{
		}

		/// <summary>
		/// Create an instance for the specified actual component, at the specified depth, with
		/// the specified actual property, using the specified attributes.
		/// </summary>
		/// <param name="actualComponent">The actual component that owns the property.</param>
		/// <param name="depth">The depth of the property.</param>
		/// <param name="actual">The actual property.</param>
		/// <param name="attributes">The attributes for the property.</param>
		public PropertyDescriptorBroker(object actualComponent, int depth, PropertyDescriptor actual,
			IEnumerable<Attribute> attributes)
			: base(actual, CreateAttribtues(actualComponent, depth, actual, attributes))
		{
			ActualComponent = actualComponent;
			Actual = actual;
			Depth = depth;
		}

		/// <summary>
		/// Create an instance for the specified actual component, at the specified depth, with
		/// the specified actual property, using the specified attributes.
		/// </summary>
		/// <param name="actualComponent">The actual component that owns the property.</param>
		/// <param name="depth">The depth of the property.</param>
		/// <param name="actual">The actual property.</param>
		/// <param name="attributes">The attributes for the property.</param>
		public PropertyDescriptorBroker(object actualComponent, int depth, PropertyDescriptor actual,
			params Attribute[] attributes)
			: this(actualComponent, depth, actual, (IEnumerable<Attribute>)attributes)
		{
		}
		#endregion // Constructors

		#region Private data
		private static readonly Attribute[] emptyAttributes = new Attribute[0];
		#endregion // Private data

		#region Properties
		/// <summary>
		/// Gets the actual component to which this property is attached.
		/// </summary>
		public object ActualComponent { get; }

		/// <summary>
		/// Gets the actual property descriptor that this broker services.
		/// </summary>
		public PropertyDescriptor Actual { get; }

		/// <summary>
		/// Gets the type of component to which this property is attached.
		/// </summary>
		public override Type ComponentType => Actual.ComponentType;

		/// <summary>
		/// Gets the depth of this property.
		/// </summary>
		public int Depth { get; }

		/// <summary>
		/// Get a value indicating if the field is read-only.
		/// </summary>
		public override bool IsReadOnly => Actual.IsReadOnly;

		/// <summary>
		/// Gets the type of this property.
		/// </summary>
		public override Type PropertyType => Actual.PropertyType;
		#endregion // Properties

		#region Public methods
		// Override to disable reset menu item in property grid
		public override bool CanResetValue(object component) =>
			false;

		// Override to get the value from the actual component
		public override object GetValue(object component) =>
			Actual.GetValue(ActualComponent);

		// Override to prevent users from change values in property grid (via reset)
		public override void ResetValue(object component) { }

		// Override to prevent users from changing values in property grid
		public override void SetValue(object component, object value) { }

		// Override to disable reset menu item in property grid
		public override bool ShouldSerializeValue(object component) => false;
		#endregion // Public methods

		#region Private methods
		// Create attributes adding a type converter to make the first level of sub-properties,
		// in the same namespace, expandable in property grids.
		private static Attribute[] CreateAttribtues(object actualComponent, int depth,
			PropertyDescriptor actual, IEnumerable<Attribute> attributes)
		{
			if (actualComponent == null)
				throw new ArgumentNullException(nameof(actualComponent));

			if (actual == null)
				throw new ArgumentNullException(nameof(actual));

			attributes = attributes ?? emptyAttributes;

			if (ShouldBeExpandable(actualComponent, depth, actual) &&
				actual.GetValue(actualComponent) != null)
				attributes = attributes.Concat(new Attribute[]
				{
					new TypeConverterAttribute(typeof(ExpandableObjectConverter))
				});

			return attributes.ToArray();
		}

		private bool ShouldBeExpandable() =>
			ShouldBeExpandable(ActualComponent, Depth, Actual);

		private static bool ShouldBeExpandable(object actualComponent, int depth,
			PropertyDescriptor actual) =>
			depth == 0 && actualComponent.GetType().Namespace == actual.PropertyType.Namespace;
		#endregion // Private methods
	}
}
