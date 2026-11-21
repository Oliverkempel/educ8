using System.Text.RegularExpressions;

namespace Educ8.Itslearning.Models;

/// <summary>
/// An item in the "latest changes" stream (<c>notifications/stream/v2</c>, <c>courses/{id}/stream/v1</c>), newest first.
/// Either one element (<see cref="ElementId"/>), a group of elements (<see cref="ElementsCount"/> &gt; 1, ElementId 0;
/// see <c>notifications/{id}/elements/v1</c>), or an announcement (<see cref="LightBulletin"/>).
/// </summary>
public sealed record StreamItem
{
    public int NotificationId { get; init; }
    public string Text { get; init; } = "";
    public string? LocationTitle { get; init; }
    public int LocationId { get; init; }
    public DateTimeOffset? PublishedDate { get; init; }
    public BulletinAuthor? PublishedBy { get; init; }
    public string? Url { get; init; }
    public int ElementId { get; init; }
    public string? ElementType { get; init; }
    public int LearningToolId { get; init; }
    public bool HasMoreElements { get; init; }
    public int ElementsCount { get; init; }
    public LightBulletin? LightBulletin { get; init; }

    public ResourceKind Kind => LearningTools.KindOf(ElementType, LearningToolId);

    public bool IsGroup => ElementId == 0 && ElementsCount > 1;
}

/// <summary>Body item of <c>PUT notifications/v2</c>.</summary>
internal sealed record NotificationReadUpdate(int NotificationId, bool IsRead);

/// <summary>A personal notification (the bell, <c>notifications/v2</c>): e.g. "Assessment is available for …".</summary>
public sealed partial record Notification
{
    public int NotificationId { get; init; }
    public string Text { get; init; } = "";
    public DateTimeOffset? PublishedDate { get; init; }
    public BulletinAuthor? PublishedBy { get; init; }

    /// <summary>e.g. "Unknown", "Assessment" (string: tolerant of new values).</summary>
    public string? Type { get; init; }

    public string? Url { get; init; }
    public string? ContentUrl { get; init; }
    public bool IsRead { get; init; }

    /// <summary>Element id from <see cref="ContentUrl"/> (<c>…ViewLearningToolElement.aspx?LearningToolElementId=…</c>), if any.</summary>
    public int? ElementId =>
        ContentUrl is not null && ElementIdInUrl().Match(ContentUrl) is { Success: true } m && int.TryParse(m.Groups[1].Value, out var id) ? id : null;

    [GeneratedRegex(@"(?:LearningToolElementId|ElementID)=(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex ElementIdInUrl();

    /// <summary>
    /// The course the notification belongs to: <c>ContentUrl</c> <c>…Course.aspx?CourseID=…</c> (announcements), or <c>Url</c>
    /// <c>…ContentArea.aspx?LocationType=1&amp;LocationID=…</c> (type 1 = course; verified live 2026-11-21).
    /// </summary>
    public int? CourseId =>
        (ContentUrl is not null && CourseIdInUrl().Match(ContentUrl) is { Success: true } c ? c.Groups[1].Value
            : Url is not null && CourseLocationInUrl().Match(Url) is { Success: true } l ? l.Groups[1].Value
            : null) is { } id && int.TryParse(id, out var courseId) ? courseId : null;

    [GeneratedRegex(@"[?&]CourseID=(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex CourseIdInUrl();

    [GeneratedRegex(@"[?&]LocationType=1&LocationID=(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex CourseLocationInUrl();
}
