using System.ComponentModel;
using System.Text;
using Octokit;

namespace GitHubApiDemo
{
	/// <summary>
	/// A broker that exposes properties of the <see cref="SearchIssuesRequestExclusions"/>,
	/// in a controlled fashion, to the <see cref="System.Windows.Forms.PropertyGrid"/>
	/// control.
	/// </summary>
	public class IssueExclusionsBroker : Broker
	{
		#region Constructors
		/// <summary>
		/// Create an instance from the previous search criteria.
		/// </summary>
		public IssueExclusionsBroker()
			: this(null)
		{
		}

		/// <summary>
		/// Create an instance from another instance.
		/// </summary>
		/// <param name="other">The other instance from which values are copied.</param>
		internal IssueExclusionsBroker(IssueExclusionsBroker other)
		{
			if (other == null)
				return;

			Assignee = other.Assignee;
			Author = other.Author;
			Base = other.Base;
			Commenter = other.Commenter;
			Head = other.Head;
			Involves = other.Involves;
			Labels = other.Labels;
			Language = other.Language;
			Mentions = other.Mentions;
			Milestone = other.Milestone;
			State = other.State;
			Status = other.Status;
		}
		#endregion // Constructors

		#region Properties
		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Assignee), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Assignee { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Author), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Author { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Base), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Base { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Commenter), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Commenter { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Head), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Head { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Involves), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Involves { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Labels), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Labels { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Language), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public Language? Language { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Mentions), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Mentions { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Milestone), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public string Milestone { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(State), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public ItemState? State { get; set; }

		[Category(CategoryText.Behavior)]
		[DescriptionKey(nameof(Status), typeof(IssueExclusionsBroker))]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public CommitState? Status { get; set; }
		#endregion // Properties

		#region Public methods
		/// <summary>
		/// Create the search issue request exclusions.
		/// </summary>
		/// <returns>The search issue request exclusions (or null).</returns>
		public SearchIssuesRequestExclusions CreateSearchIssuesRequestExclusions() =>
			IsDefault() ? null : new SearchIssuesRequestExclusions
			{
				Assignee = Assignee,
				Author = Author,
				Base = Base,
				Commenter = Commenter,
				Head = Head,
				Involves = Involves,
				Labels = ToList(Labels),
				Language = Language,
				Mentions = Mentions,
				Milestone = Milestone,
				State = State,
				Status = Status
			};

		/// <summary>
		/// Reset the search terms.
		/// </summary>
		public void Reset()
		{
			Assignee = null;
			Author = null;
			Base = null;
			Commenter = null;
			Head = null;
			Involves = null;
			Labels = null;
			Language = null;
			Mentions = null;
			Milestone = null;
			State = null;
			Status = null;
		}

		/// <summary>
		/// Create a text representation of this instance.
		/// </summary>
		/// <returns>A text representation of this instance.</returns>
		public override string ToString()
		{
			var builder = new StringBuilder();
			AddField(builder, nameof(Assignee), Assignee);
			AddField(builder, nameof(Author), Author);
			AddField(builder, nameof(Base), Base);
			AddField(builder, nameof(Commenter), Commenter);
			AddField(builder, nameof(Head), Head);
			AddField(builder, nameof(Involves), Involves);
			AddField(builder, nameof(Labels), Labels);
			AddField(builder, nameof(Language), Language);
			AddField(builder, nameof(Mentions), Mentions);
			AddField(builder, nameof(Milestone), Milestone);
			AddField(builder, nameof(State), State);
			AddField(builder, nameof(Status), Status);
			return builder.ToString();
		}
		#endregion // Public methods

		#region Private methods
		private void AddField(StringBuilder builder, string name, object value)
		{
			if (value == null)
				return;

			if (builder.Length > 0)
				builder.Append(", ");

			builder.Append(name);
		}

		private bool IsDefault() =>
			Assignee == null && Author == null && Base == null && Commenter == null &&
			Head == null && Involves == null && Labels == null && Language == null &&
			Mentions == null && State == null && Status == null;
		#endregion // Private methods
	}
}
