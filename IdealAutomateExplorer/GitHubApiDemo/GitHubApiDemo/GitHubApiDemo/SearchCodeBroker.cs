using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Design;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker that exposes properties of the <see cref="SearchCodeRequest"/>,
	/// in a controlled fashion, to the <see cref="System.Windows.Forms.PropertyGrid"/>
	/// control.
	/// </summary>
	public class SearchCodeBroker : SearchBroker<SearchCodeRequest>
	{
		#region Constructors
		/// <summary>
		/// Create an instance from the previous search criteria.
		/// </summary>
		public SearchCodeBroker()
			: this(previous)
		{
		}

		private SearchCodeBroker(SearchCodeBroker other)
			: base(other)
		{
			if (other == null)
				return;

			Extension = other.Extension;
			FileName = other.FileName;
			Forks = other.Forks;
			In = new List<CodeInQualifier>(other.In);
			Language = other.Language;
			Organization = other.Organization;
			Path = other.Path;
			Repos = other.Repos;
			Size = new Int32RangeBroker(other.Size);
			SortField = other.SortField;
			User = other.User;
		}
		#endregion // Constructors

		#region Private data
		private static SearchCodeBroker previous = new SearchCodeBroker(null);
		#endregion // Private data

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Extension), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public string Extension { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(FileName), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public string FileName { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Forks), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public bool? Forks { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(In), typeof(SearchCodeBroker))]
		[Editor(typeof(EnumListUITypeEditor<CodeInQualifier>), typeof(UITypeEditor))]
		[TypeConverter(typeof(EnumListTypeConverter<CodeInQualifier>))]
		public List<CodeInQualifier> In { get; set; } =
			new List<CodeInQualifier>();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Language), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public Language? Language { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Organization), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public string Organization { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Path), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public string Path { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Repos), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public string Repos { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Size), typeof(SearchCodeBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Size { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(SortField), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public CodeSearchSort? SortField { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(User), typeof(SearchCodeBroker))]
		[DefaultValue(null)]
		public string User { get; set; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Create a searcher.
		/// </summary>
		/// <returns>The searcher that is created.</returns>
		public override Searcher CreateSearcher(GitHubClient client, int maximumCount)
		{
			previous = this;
			return new CodeSearcher(client, maximumCount, CreateRequest());
		}

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public override void Reset()
		{
			base.Reset();

			Extension = null;
			FileName = null;
			Forks = null;
			In = new List<CodeInQualifier>();
			Language = null;
			Organization = null;
			Path = null;
			Repos = null;
			Size = new Int32RangeBroker();
			SortField = null;
			User = null;
		}
		#endregion // Public methods

		/// The methods in this region are all named ResetXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ResetXXX methods for PropertyGrid
		public void ResetIn() =>
			In.Clear();

		public void ResetSize() =>
			Size = new Int32RangeBroker();
		#endregion // ResetXXX methods for PropertyGrid

		/// The methods in this region are all named ShouldSerializeXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ShouldSerializeXXX methods for PropertyGrid
		public bool ShouldSerializeIn() =>
			In.Count > 0;

		public bool ShouldSerializeSize() =>
			Size.Qualifier != SearchQualifierValue.None;
		#endregion // ShouldSerializeXXX methods for PropertyGrid

		#region Protected methods
		/// <summary>
		/// Create a request with no search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchCodeRequest CreateNoTermRequest() =>
			new SearchCodeRequest
			{
				Extension = Extension,
				FileName = FileName,
				Forks = Forks,
				In = In.NullIfEmpty(),
				Language = Language,
				Order = Order,
				Organization = Organization,
				Path = Path,
				Repos = GetRepositories(Repos),
				Size = Size.CreateRange(),
				SortField = SortField,
				User = User
			};

		/// <summary>
		/// Create a request with a search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchCodeRequest CreateTermRequest(string term) =>
			new SearchCodeRequest(term)
			{
				Extension = Extension,
				FileName = FileName,
				Forks = Forks,
				In = In.NullIfEmpty(),
				Language = Language,
				Order = Order,
				Organization = Organization,
				Path = Path,
				Repos = GetRepositories(Repos),
				Size = Size.CreateRange(),
				SortField = SortField,
				User = User
			};
		#endregion // Protected methods
	}
}
