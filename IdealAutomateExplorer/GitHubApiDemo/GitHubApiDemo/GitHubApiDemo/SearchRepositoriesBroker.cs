using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker that exposes properties of the <see cref="SearchRepositoriesRequest"/>,
	/// in a controlled fashion, to the <see cref="System.Windows.Forms.PropertyGrid"/>
	/// control.
	/// </summary>
	public class SearchRepositoriesBroker : SearchBroker<SearchRepositoriesRequest>
	{
		#region Constructors
		/// <summary>
		/// Create an instance from the previous search criteria.
		/// </summary>
		public SearchRepositoriesBroker()
			: this(previous)
		{
		}

		private SearchRepositoriesBroker(SearchRepositoriesBroker other)
			: base(other)
		{
			if (other == null)
				return;

			Archived = other.Archived;
			Created = new DateRangeBroker(other.Created);
			Fork = other.Fork;
			Forks = new Int32RangeBroker(other.Forks);
			In = new List<InQualifier>(other.In);
			Language = other.Language;
			Size = new Int32RangeBroker(other.Size);
			SortField = other.SortField;
			Stars = new Int32RangeBroker(other.Stars);
			Updated = new DateRangeBroker(other.Updated);
			User = other.User;
		}
		#endregion // Constructors

		#region Private data
		private static SearchRepositoriesBroker previous = new SearchRepositoriesBroker(null);
		#endregion // Private data

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Archived), typeof(SearchRepositoriesBroker))]
		[DefaultValue(null)]
		public bool? Archived { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Created), typeof(SearchRepositoriesBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Created { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Fork), typeof(SearchRepositoriesBroker))]
		[DefaultValue(null)]
		public ForkQualifier? Fork { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Forks), typeof(SearchRepositoriesBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Forks { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(In), typeof(SearchRepositoriesBroker))]
		[Editor(typeof(EnumListUITypeEditor<InQualifier>), typeof(UITypeEditor))]
		[TypeConverter(typeof(EnumListTypeConverter<InQualifier>))]
		public List<InQualifier> In { get; set; } =
			new List<InQualifier>();

		[Category(CategoryText.Behavior)]
		[DefaultValue(null)]
		public Language? Language { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Size), typeof(SearchRepositoriesBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Size { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Stars), typeof(SearchRepositoriesBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Stars { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(SortField), typeof(SearchRepositoriesBroker))]
		[DefaultValue(null)]
		public RepoSearchSort? SortField { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Updated), typeof(SearchRepositoriesBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Updated { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(User), typeof(SearchRepositoriesBroker))]
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
			return new RepositorySearcher(client, maximumCount, CreateRequest());
		}

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			Archived = null;
			Created = new DateRangeBroker();
			Fork = null;
			Forks = new Int32RangeBroker();
			In = new List<InQualifier>();
			Language = null; 
			Size = new Int32RangeBroker();
			SortField = null;
			Stars = new Int32RangeBroker();
			Updated = new DateRangeBroker();
			User = null;
		}
		#endregion // Public methods

		/// The methods in this region are all named ResetXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ResetXXX methods for PropertyGrid
		public void ResetCreated() =>
			Created = new DateRangeBroker();

		public void ResetForks() =>
			Forks = new Int32RangeBroker();

		public void ResetIn() =>
			In.Clear();

		public void ResetSize() =>
			Size = new Int32RangeBroker();

		public void ResetStars() =>
			Stars = new Int32RangeBroker();

		public void ResetUpdated() =>
			Updated = new DateRangeBroker();
		#endregion // ResetXXX methods for PropertyGrid

		/// The methods in this region are all named ShouldSerializeXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ShouldSerializeXXX methods for PropertyGrid
		public bool ShouldSerializeCreated() =>
			Created.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeForks() =>
			Forks.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeIn() =>
			In.Count > 0;

		public bool ShouldSerializeSize() =>
			Size.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeStars() =>
			Stars.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeUpdated() =>
			Updated.Qualifier != SearchQualifierValue.None;
		#endregion // ShouldSerializeXXX methods for PropertyGrid

		#region Protected methods
		/// <summary>
		/// Create a request with no search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchRepositoriesRequest CreateNoTermRequest() =>
			new SearchRepositoriesRequest()
			{
				Archived = Archived,
				Created = Created.CreateRange(),
				Fork = Fork,
				Forks = Forks.CreateRange(),
				In = In.NullIfEmpty(),
				Language = Language,
				Order = Order,
				Size = Size.CreateRange(),
				Stars = Stars.CreateRange(),
				SortField = SortField,
				Updated = Updated.CreateRange(),
				User = string.IsNullOrWhiteSpace(User) ? null : User
			};

		/// <summary>
		/// Create a request with a search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchRepositoriesRequest CreateTermRequest(string term) =>
			new SearchRepositoriesRequest(term)
			{
				Archived = Archived,
				Created = Created.CreateRange(),
				Fork = Fork,
				Forks = Forks.CreateRange(),
				In = In.NullIfEmpty(),
				Language = Language,
				Order = Order,
				Size = Size.CreateRange(),
				Stars = Stars.CreateRange(),
				SortField = SortField,
				Updated = Updated.CreateRange(),
				User = string.IsNullOrWhiteSpace(User) ? null : User
			};
		#endregion // Protected methods
	}
}
