namespace Educ8.Itslearning.Models;

/// <summary>Basic course info (<c>GET restapi/personal/courses/{id}/v1</c>).</summary>
public sealed record CourseInfo
{
    public int CourseId { get; init; }
    public string Title { get; init; } = "";
    public string? Code { get; init; }
    public DateTimeOffset? CreatedDateTimeUtc { get; init; }
}

/// <summary>A course topic (week/module). Each topic has a folder with its material.</summary>
public sealed record CourseTopic
{
    public int TopicId { get; init; }
    public string TopicName { get; init; } = "";
    public int? FolderId { get; init; }
    public string? Color { get; init; }
}

/// <summary>An element in a course folder (<c>courses/{id}/resources</c>, folder contents, search).</summary>
public record CourseResource
{
    public int ElementId { get; init; }

    /// <summary>Raw element type, e.g. "Folder", "LearningToolElement", "CustomActivity".</summary>
    public string ElementType { get; init; } = "";

    /// <summary>For learning tool elements, which tool it is (see <see cref="LearningTools"/>). 0 for native elements.</summary>
    public int LearningToolId { get; init; }

    public string Title { get; init; } = "";
    public int CourseId { get; init; }

    /// <summary>Link to the element in the itslearning web UI.</summary>
    public string? Url { get; init; }

    public string? ContentUrl { get; init; }
    public bool Active { get; init; } = true;
    public bool Homework { get; init; }

    /// <summary>Folder path, e.g. " / Course title / 02: Operating Systems / Lecture".</summary>
    public string? Path { get; init; }

    public ResourceKind Kind => LearningTools.KindOf(ElementType, LearningToolId);
}

/// <summary>A single element looked up by id, with its course (<c>courses/resources/{id}/v1</c>).</summary>
public sealed record CourseResourceWithCourse : CourseResource
{
    public string? CourseCode { get; init; }
    public string? CourseTitle { get; init; }
}

internal sealed record CourseFolderDetails
{
    public EntityList<CourseResource> Resources { get; init; } = new();
}

/// <summary>A task (assignment, test, activity...) for the user (<c>tasks/v1</c>, <c>courses/{id}/tasks/v1</c>).</summary>
public sealed record CourseTask
{
    public int TaskId { get; init; }
    public int ElementId { get; init; }
    public string ElementType { get; init; } = "";
    public int LearningToolId { get; init; }
    public string Title { get; init; } = "";
    public string? Description { get; init; }

    /// <summary>e.g. "NotStarted", "Completed".</summary>
    public string? Status { get; init; }

    /// <summary>Often null in practice.</summary>
    public DateTimeOffset? Deadline { get; init; }

    public string? LocationTitle { get; init; }
    public int LocationId { get; init; }
    public string? Url { get; init; }
    public bool Homework { get; init; }

    public ResourceKind Kind => LearningTools.KindOf(ElementType, LearningToolId);
}

/// <summary>A course announcement ("light bulletin"). <see cref="Text"/> is plain text.</summary>
public sealed record LightBulletin
{
    public int LightBulletinId { get; init; }
    public string Text { get; init; } = "";
    public string? EmbedUrl { get; init; }
    public bool Pinned { get; init; }
    public DateTimeOffset? PublishedDate { get; init; }
    public BulletinAuthor? PublishedBy { get; init; }
    public int CommentsCount { get; init; }
    public int ResourcesCount { get; init; }
}

public sealed record BulletinAuthor
{
    public int PersonId { get; init; }
    public string? FullName { get; init; }
}

/// <summary>A lesson plan entry (<c>planner/v1</c>). <see cref="PlanDescription"/> may contain HTML.</summary>
public sealed record PlanPreview
{
    public int PlanId { get; init; }
    public string PlanName { get; init; } = "";
    public string? PlanDescription { get; init; }
    public DateTimeOffset? Start { get; init; }
    public DateTimeOffset? Stop { get; init; }
    public int TopicId { get; init; }
}

internal sealed record SsoUrl
{
    public string Url { get; init; } = "";
}
