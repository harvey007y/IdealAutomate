using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Design;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker that exposes properties of the <see cref="SearchUsersRequest"/>,
	/// in a controlled fashion, to the <see cref="System.Windows.Forms.PropertyGrid"/>
	/// control.
	/// </summary>
	public class SearchUsersBroker : SearchBroker<SearchUsersRequest>
	{
		#region Constructors
		/// <summary>
		/// Create an instance from the previous search criteria.
		/// </summary>
		public SearchUsersBroker()
			: this(previous)
		{
		}

		private SearchUsersBroker(SearchUsersBroker other)
			: base(other)
		{
			if (other == null)
				return;

			AccountType = other.AccountType;
			Created = new DateRangeBroker(other.Created);
			Followers = new Int32RangeBroker(other.Followers);
			In = new List<UserInQualifier>(other.In);
			Language = other.Language;
			Location = other.Location;
			Repositories = new Int32RangeBroker(other.Repositories);
			SortField = other.SortField;
		}
		#endregion // Constructors

		#region Private data
		private static SearchUsersBroker previous = new SearchUsersBroker(null);
		#endregion // Private data

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(AccountType), typeof(SearchUsersBroker))]
		[DefaultValue(null)]
		public AccountSearchType? AccountType { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Created), typeof(SearchUsersBroker))]
		[TypeConverter(typeof(DateRangeConverter))]
		public DateRangeBroker Created { get; set; } =
			new DateRangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Followers), typeof(SearchUsersBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Followers { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(In), typeof(SearchUsersBroker))]
		[Editor(typeof(EnumListUITypeEditor<UserInQualifier>), typeof(UITypeEditor))]
		[TypeConverter(typeof(EnumListTypeConverter<UserInQualifier>))]
		public List<UserInQualifier> In { get; set; } =
			new List<UserInQualifier>();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Language), typeof(SearchUsersBroker))]
		[DefaultValue(null)]
		public Language? Language { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Location), typeof(SearchUsersBroker))]
		[DefaultValue(null)]
		public string Location { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Repositories), typeof(SearchUsersBroker))]
		[TypeConverter(typeof(Int32RangeConverter))]
		public Int32RangeBroker Repositories { get; set; } =
			new Int32RangeBroker();

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(SortField), typeof(SearchUsersBroker))]
		[DefaultValue(null)]
		public UsersSearchSort? SortField { get; set; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Create a searcher.
		/// </summary>
		/// <returns>The searcher that is created.</returns>
		public override Searcher CreateSearcher(GitHubClient client, int maximumCount)
		{
			previous = this;
			return new UserSearcher(client, maximumCount, CreateRequest());
		}

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			AccountType = null;
			Created = new DateRangeBroker();
			Followers = new Int32RangeBroker();
			In = new List<UserInQualifier>();
			Language = null;
			Location = null;
			Repositories = new Int32RangeBroker();
			SortField = null;
		}
		#endregion // Public methods

		/// The methods in this region are all named ResetXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ResetXXX methods for PropertyGrid
		public void ResetCreated() =>
			Created = new DateRangeBroker();

		public void ResetFollowers() =>
			Followers = new Int32RangeBroker();

		public void ResetIn() =>
			In.Clear();

		public void ResetRepositories() =>
			Repositories = new Int32RangeBroker();
		#endregion // ResetXXX methods for PropertyGrid

		/// The methods in this region are all named ShouldSerializeXXX, where XXX is the name of a
		/// property in this class.  PropertyGrid uses these methods to reset properties that
		/// require special handling not possible with DefaultValue attribute.
		#region ShouldSerializeXXX methods for PropertyGrid
		public bool ShouldSerializeCreated() =>
			Created.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeFollowers() =>
			Followers.Qualifier != SearchQualifierValue.None;

		public bool ShouldSerializeIn() =>
			In.Count > 0;

		public bool ShouldSerializeRepositories() =>
			Repositories.Qualifier != SearchQualifierValue.None;
		#endregion // ShouldSerializeXXX methods for PropertyGrid

		#region Protected methods
		/// <summary>
		/// Create a request with no search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchUsersRequest CreateNoTermRequest() =>
			CreateTermRequest(string.Empty);

		/// <summary>
		/// Create a request with a search term.
		/// </summary>
		/// <returns>The request that was created.</returns>
		protected override SearchUsersRequest CreateTermRequest(string term) =>
			new SearchUsersRequest(term)
			{
				AccountType = AccountType,
				Created = Created.CreateRange(),
				Followers = Followers.CreateRange(),
				In = In.NullIfEmpty(),
				Language = Language,
				Location = Location,
				Order = Order,
				Repositories = Repositories.CreateRange(),
				SortField = SortField
			};
		#endregion // Protected methods
	}
}
