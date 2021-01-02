using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;

namespace GitHubApiDemo
{
	/// <summary>
	/// A derivation of <see cref="DescriptionAttribute"/> that allows the description
	/// to be stored in a resource file keyed by type and name.
	/// </summary>
	public class DescriptionKeyAttribute : DescriptionAttribute
	{
		#region Constructors
		/// <summary>
		/// Create an instance for the specified name and type.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="type">The type containing the property.</param>
		public DescriptionKeyAttribute(string name, Type type)
			: base()
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Type = type ?? throw new ArgumentNullException(nameof(type));
		}
		#endregion // Constructors

		#region Private data
		private static readonly Dictionary<string, ResourceManager> resourceManagers =
			new Dictionary<string, ResourceManager>();
		private string description;
		#endregion // Private data

		#region Properties
		/// <summary>
		/// Gets the description.
		/// </summary>
		public override string Description =>
			description = description ?? GetDescription(Type, Name);

		/// <summary>
		/// Gets the type containing the property.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		public string Name { get; }
		#endregion // Properties

		#region Private methods
		private static string GetDescription(Type type, string name) =>
			GetResourceManager(type)?.GetString(name) ?? name;

		private static ResourceManager GetResourceManager(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			Type baseType = typeof(DescriptionKeyAttribute);
			string baseName = $"{baseType.Namespace}.Descriptions.{GetTypeName(type)}";

			if (!resourceManagers.TryGetValue(baseName, out ResourceManager resourceManager))
			{
				resourceManager = new ResourceManager(baseName, baseType.Assembly);
				resourceManagers.Add(baseName, resourceManager);
			}

			return resourceManager;
		}

		private static string GetTypeName(Type type)
		{
			string name = type.FullName;
			int index = name.IndexOf('`');
			return index < 0 ? name : name.Substring(0, index);
		}
		#endregion // Private methods
	}
}
