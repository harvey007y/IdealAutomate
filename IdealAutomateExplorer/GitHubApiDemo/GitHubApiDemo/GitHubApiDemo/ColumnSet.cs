using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace GitHubApiDemo
{
	/// <summary>
	/// An immutable set of available and selected column names, where each column name
	/// appears once, and only once, in the set.
	/// </summary>
	public class ColumnSet
	{
		#region Constructors
		/// <summary>
		/// Create an empty column set.
		/// </summary>
		public ColumnSet()
		{
			Available = new ReadOnlyCollection<string>(emptySequence);
			Selected = new ReadOnlyCollection<string>(emptySequence);
		}

		/// <summary>
		/// Create an immutable set of available and selected columns, using the names
		/// of public properties of the specified type.
		/// </summary>
		/// <param name="type">The type from which property names are obtained.</param>
		/// <param name="selected">The (optional) names of the properties that are selected (or null).</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="type"/> is null
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// <paramref name="selected"/> contains duplicate names or specifies the name of a
		/// non-existent property.
		/// </exception>
		public ColumnSet(Type type, int depth, params string[] selected)
			: this(type, depth, (IEnumerable<string>)selected)
		{
		}

		/// <summary>
		/// Create an immutable set of available and selected columns, using the names
		/// of public properties of the specified type.
		/// </summary>
		/// <param name="type">The type from which property names are obtained.</param>
		/// <param name="selected">The (optional) names of the properties that are selected (or null).</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="type"/> is null
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// <paramref name="selected"/> contains duplicate names or specifies the name of a
		/// non-existent property.
		/// </exception>
		public ColumnSet(Type type, int depth, IEnumerable<string> selected = null)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			if (depth <= 0)
				throw new ArgumentOutOfRangeException(nameof(depth));

			selected = selected ?? emptySequence;

			List<string> selectedList = selected.ToList();
			var selectedHash = new HashSet<string>(selectedList);

			List<string> availableList = GetProperties(type, depth).ToList();
			var availableHash = new HashSet<string>(availableList);

			if (selectedList.Count != selectedHash.Count ||
				selectedHash.Any(name => !availableHash.Contains(name)))
				throw new InvalidOperationException();

			availableList = availableHash
				.Where(name => !selectedHash.Contains(name))
				.ToList();

			Available = new ReadOnlyCollection<string>(availableList);
			Selected = new ReadOnlyCollection<string>(selectedList);
		}

		/// <summary>
		/// Create an immutable set of available and selected columns.
		/// </summary>
		/// <param name="available">The (optional) available columns (or null).</param>
		/// <param name="selected">The (optional) selected columns (or null).</param>
		/// <exception cref="System.InvalidOperationException">
		/// <paramref name="available"/> and/or <paramref name="selected"/> contain
		/// duplicate names or share the same names.
		/// </exception>
		public ColumnSet(IEnumerable<string> available, IEnumerable<string> selected)
		{
			available = available ?? emptySequence;
			selected = selected ?? emptySequence;

			List<string> availableList = available.ToList();
			List<string> selectedList = selected.ToList();

			var selectedHash = new HashSet<string>(selectedList);
			var availableHash = new HashSet<string>(availableList);

			if (selectedHash.Count != selectedList.Count ||
				availableHash.Count != availableList.Count ||
				selectedHash.Any(name => availableHash.Contains(name)))
				throw new InvalidOperationException();

			Available = new ReadOnlyCollection<string>(availableList);
			Selected = new ReadOnlyCollection<string>(selectedList);
		}
		#endregion // Constructors

		#region Constants
		private const char PathSeparator = '.';
		#endregion // Constants

		#region Private data
		private static readonly string[] emptySequence = new string[0];
		#endregion // Private data

		#region Properties
		/// <summary>
		/// Gets the available column names.
		/// </summary>
		public ReadOnlyCollection<string> Available { get; }

		/// <summary>
		/// Gets the selected column names.
		/// </summary>
		public ReadOnlyCollection<string> Selected { get; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Gets the property information for the specified property.
		/// </summary>
		/// <param name="type">The type containing the property specified in the first segment of the path.</param>
		/// <param name="path">The path for the property (or nested property).</param>
		/// <returns>The property information for the specified property.</returns>
		public static PropertyInfo GetProperty(Type type, string path) =>
			path.Contains(PathSeparator) ? GetNestedProperty(type, path) : type.GetProperty(path);

		/// <summary>
		/// Get a string representation of this instance.
		/// </summary>
		/// <returns>A string representation of this instance.</returns>
		public override string ToString() =>
			string.Join(",", Selected);

		/// <summary>
		/// Try to get the value of the specified property.
		/// </summary>
		/// <param name="path">The path for the property (or nested property).</param>
		/// <param name="component">The component/instance containing the property specified in the first segment of the path.</param>
		/// <param name="result">The resulting value, if successful; otherwise, <see langword="null"/>.</param>
		/// <returns>True, if successful; otherwise, false.</returns>
		public static bool TryGetNestedPropertyValue(string path, object component,
			out object result)
		{
			string[] parts = path?.Split('.') ?? throw new ArgumentNullException(nameof(path));
			int length = parts.Length;
			int index = 0;
			result = null;

			while (index < length && component != null)
			{
				PropertyInfo property = component.GetType().GetProperty(parts[index++]);
				component = property?.GetValue(component);
			}

			if (length == 0 || index < length)
				return false;

			result = component;
			return true;
		}
		#endregion // Public methods

		#region Private methods
		private static PropertyInfo GetNestedProperty(Type type, string path)
		{
			string[] parts = path.Split(PathSeparator);
			int maxIndex = parts.Length - 1;

			for (int index = 0; index < maxIndex; index++)
			{
				PropertyInfo property = type.GetProperty(parts[index]);
				if (property == null)
					return null;

				type = property.PropertyType;
			}

			return type.GetProperty(parts[maxIndex]);
		}

		private static IEnumerable<string> GetProperties(Type type, int depth,
			string baseName = null)
		{
			foreach (PropertyInfo property in type.GetProperties())
			{
				string name = baseName == null ? property.Name : $"{baseName}.{property.Name}";

				Type propertyType = property.PropertyType;
				int childDepth = depth - 1;

				if (childDepth > 0 && propertyType.Namespace == type.Namespace)
				{
					foreach (string childName in GetProperties(propertyType, childDepth, name))
						yield return childName;
				}

				yield return name;
			}
		}
		#endregion // Private methods
	}
}
