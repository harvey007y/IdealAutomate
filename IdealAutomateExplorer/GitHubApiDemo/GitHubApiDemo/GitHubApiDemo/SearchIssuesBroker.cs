using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Design;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker that exposes properties of the <see cref="SearchIssuesRequest"/>,
	/// in a controlled fashion, to the <see cref="System.Windows.Forms.PropertyGrid"/>
	/// control.
	/// </summary>
	public class SearchIssuesBroker : SearchBroker<SearchIssuesRequest>
	{
		#region Constructors
		/// <summary>
		/// Create an instance from the previous search criteria.
		/// </summary>
		public SearchIssuesBroker()
			: this(previous)
		{
		}

		private SearchIssuesBroker(SearchIssuesBroker other)
			: base(other)
		{
			if (other == null)
				return;

			Archived = other.Archived;
			Assignee = other.Assignee;
			Author = other.Author;
			Base = other.Base;
			Closed = new DateRangeBroker(other.Closed);
			Commenter = other.Commenter;
			Comments = new Int32RangeBroker(other.Comments);
			Created = new DateRangeBroker(other.Created);
			Exclusions = new IssueExclusionsBroker(other.Exclusions);
			Head = other.Head;
			In = new List<IssueInQualifier>(other.In);
			Involves = other.Involves;
			Is = new List<IssueIsQualifier>(other.Is);
			Labels = other.Labels;
			Language = other.Language;
			Mentions = other.Mentions;
			Merged = new DateRangeBroker(other.Merged);
			Milestone = other.Milestone;
			No = other.No;
			Repos = other.Repos;
			SortField = other.SortField;
			State = other.State;
			Status = other.Status;
			Team = other.Team;
			Type = other.Type;
			Updated = new DateRangeBroker(other.Updated);
			User = other.User;
		}
		#endregion // Constructors

		#region Private data
		private static SearchIssuesBroker previous = new SearchIssuesBroker(null);
		#endregion // Private data

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Archived), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public bool? Archived { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Assignee), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Assignee { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Author), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Author { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Base), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Base { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Closed), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Closed { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Commenter), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Commenter { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Comments), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Comments { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Created), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Created { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Exclusions), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DefaultValue(null)]
		public IssueExclusionsBroker Exclusions { get; } =
			new IssueExclusionsBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Head), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Head { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(In), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(EnumListTypeConverter<IssueInQualifier>))]
		[Editor(typeof(EnumListUITypeEditor<IssueInQualifier>), typeof(UITypeEditor))]
		public List<IssueInQualifier> In { get; set; } =
			new List<IssueInQualifier>();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Involves), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Involves { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Is), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(EnumListTypeConverter<IssueIsQualifier>))]
		[Editor(typeof(EnumListUITypeEditor<IssueIsQualifier>), typeof(UITypeEditor))]
		public List<IssueIsQualifier> Is { get; set; } =
			new List<IssueIsQualifier>();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Labels), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Labels { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Language), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public Language? Language { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Mentions), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Mentions { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Merged), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Merged { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Milestone), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Milestone { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(No), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public IssueNoMetadataQualifier? No { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Repos), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Repos { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(SortField), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public IssueSearchSort? SortField { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(State), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public ItemState? State { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Status), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public CommitState? Status { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Team), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public string Team { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Type), typeof(SearchIssuesBroker))]
		[DefaultValue(null)]
		public IssueTypeQualifier? Type { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Updated), typeof(SearchIssuesBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Updated { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(User), typeof(SearchIssuesBroker))]
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
			return new IssueSearcher(client, maximumCount, CreateRequest());
		}

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			Exclusions.Reset();

			Archived = null;
			Assignee = null;
			Author = null;
			Base = null;
			Closed = new DateRangeBroker();
			Commenter = null;
			Comments = new Int32RangeBroker();
			Created = new DateRangeBroker();
			Head = null;
			In = new List<IssueInQualifier>();
			Involves = null;
			Is = new List<IssueIsQualifier>();
			Labels = null;
			Language = null;
			Mentions = null;
			Merged = new DateRangeBroker();
			Milestone = null;
			No = null;
			Repos = null;
			SortField = null;
			State = null;
			Status = null;
			Team = null;
			Type = null;
			Updated = new DateRangeBroker();
			User = null;
		}
		#endregion // Public methods

		/// The methods in this region are all named ResetXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ResetXXX methods for PropertyGrid
		public void ResetClosed() =>
			Closed = new DateRangeBroker();

		public void ResetComments() =>
			Comments = new Int32RangeBroker();

		public void ResetCreated() =>
			Created = new DateRangeBroker();

		public void ResetIn() =>
			In.Clear();

		public void ResetIs() =>
			Is.Clear();

		public void ResetMerged() =>
			Merged = new DateRangeBroker();

		public void ResetUpdated() =>
			Updated = new DateRangeBroker();
		#endregion // ResetXXX methods for PropertyGrid

		/// The methods in this region are all named ShouldSerializeXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ShouldSerializeXXX methods for PropertyGrid
		public bool ShouldSerializeClosed() =>
			Closed.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeComments() =>
			Comments.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeCreated() =>
			Created.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeIn() =>
			In.Count > 0;

		public bool ShouldSerializeIs() =>
			Is.Count > 0;

		public bool ShouldSerializeMerged() =>
			Merged.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeUpdated() =>
			Updated.Qualifier != SearchQualifierValue.None;
		#endregion // ShouldSerializeXXX methods for PropertyGrid

		#region Protected methods
		/// <summary>
		/// Create a request with no search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchIssuesRequest CreateNoTermRequest() =>
			new SearchIssuesRequest
			{
				Archived = Archived,
				Assignee = Assignee,
				Author = Author,
				Base = Base,
				Closed = Closed.CreateRange(),
				Commenter = Commenter,
				Comments = Comments.CreateRange(),
				Created = Created.CreateRange(),
				Exclusions = Exclusions.CreateSearchIssuesRequestExclusions(),
				Head = Head,
				In = In.NullIfEmpty(),
				Involves = Involves,
				Is = Is.NullIfEmpty(),
				Labels = ToList(Labels),
				Language = Language,
				Mentions = Mentions,
				Merged = Merged.CreateRange(),
				Milestone = Milestone,
				No = No,
				Order = Order,
				Repos = GetRepositories(Repos) ?? new RepositoryCollection(), // A null value causes a delayed bug
				SortField = SortField,
				State = State,
				Status = Status,
				Team = Team,
				Type = Type,
				Updated = Updated.CreateRange(),
				User = User
			};

		/// <summary>
		/// Create a request with a search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchIssuesRequest CreateTermRequest(string term) =>
			new SearchIssuesRequest(term)
			{
				Archived = Archived,
				Assignee = Assignee,
				Author = Author,
				Base = Base,
				Closed = Closed.CreateRange(),
				Commenter = Commenter,
				Comments = Comments.CreateRange(),
				Created = Created.CreateRange(),
				Exclusions = Exclusions.CreateSearchIssuesRequestExclusions(),
				Head = Head,
				In = In.NullIfEmpty(),
				Involves = Involves,
				Is = Is.NullIfEmpty(),
				Labels = ToList(Labels),
				Language = Language,
				Mentions = Mentions,
				Merged = Merged.CreateRange(),
				Milestone = Milestone,
				No = No,
				Order = Order,
				Repos = GetRepositories(Repos) ?? new RepositoryCollection(), // A null value causes a delayed bug
				SortField = SortField,
				State = State,
				Status = Status,
				Team = Team,
				Type = Type,
				Updated = Updated.CreateRange(),
				User = User
			};
		#endregion // Protected methods
	}
}
