namespace Educ8.Itslearning.Models;

/// <summary>A course the user is a member of (<c>GET restapi/personal/courses/v4</c>).</summary>
public sealed record PersonCourse
{
    public int CourseId { get; init; }
    public string Title { get; init; } = "";
    public string? FriendlyName { get; init; }

    /// <summary>e.g. "T510046101-1-E26"; the suffix is the semester.</summary>
    public string? CourseCode { get; init; }

    public DateTimeOffset? LastUpdatedUtc { get; init; }
    public int NewNotificationsCount { get; init; }
    public int NewBulletinsCount { get; init; }
    public bool HasAdminPermissions { get; init; }
    public bool HasStudentPermissions { get; init; }

    /// <summary>Course role as sent by the API (numeric; observed 2 for students).</summary>
    public int Role { get; init; }

    /// <summary>Link to the course in the itslearning web UI.</summary>
    public string? Url { get; init; }

    public IReadOnlyList<CourseTeacher> TeachersInCourse { get; init; } = [];
}

public sealed record CourseTeacher
{
    public int PersonId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get; init; }

    public string DisplayName => $"{FirstName} {LastName}".Trim() is { Length: > 0 } n ? n : FullName ?? "";
}
