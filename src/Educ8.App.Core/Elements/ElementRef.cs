using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>
/// A course element as every listing describes it (folders, plans, tasks, changes, event links): enough to pick the module that
/// opens it (<see cref="ElementNavigator"/>). <see cref="CourseId"/> is unknown for some sources (event links); modules that need it
/// look it up.
/// </summary>
public sealed record ElementRef(int ElementId, string Title, string ElementType, int LearningToolId, int? CourseId, Uri? Url)
{
    public ResourceKind Kind => LearningTools.KindOf(ElementType, LearningToolId);

    /// <summary>True for a learning tool element of the given tool (see <see cref="LearningTools"/>).</summary>
    public bool IsTool(int learningToolId) =>
        string.Equals(ElementType, "LearningToolElement", StringComparison.OrdinalIgnoreCase) && LearningToolId == learningToolId;

    public static ElementRef From(CourseResource r) =>
        new(r.ElementId, r.Title, r.ElementType, r.LearningToolId, r.CourseId > 0 ? r.CourseId : null, AbsoluteUrl(r.Url));

    public static ElementRef From(CourseTask t) =>
        new(t.ElementId, t.Title, t.ElementType, t.LearningToolId, t.LocationId > 0 ? t.LocationId : null, AbsoluteUrl(t.Url));

    public static ElementRef From(StreamItem i, string title) =>
        new(i.ElementId, title, i.ElementType ?? "", i.LearningToolId, i.LocationId > 0 ? i.LocationId : null, AbsoluteUrl(i.Url));

    public static ElementRef From(ElementLink l, int? courseId) =>
        new(l.ElementId, l.Title, l.ElementType, l.LearningToolId, courseId > 0 ? courseId : null, AbsoluteUrl(l.Url));

    internal static Uri? AbsoluteUrl(string? url) => Uri.TryCreate(url, UriKind.Absolute, out var u) ? u : null;
}
