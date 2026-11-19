namespace Educ8.Itslearning.Models;

/// <summary>An assignment (<c>GET assignments/{elementId}/v1</c>). <see cref="RawText"/> is the description as HTML.</summary>
public sealed record AssignmentDetails
{
    public int ElementId { get; init; }
    public string Title { get; init; } = "";
    public string? RawText { get; init; }
    public DateTimeOffset? Deadline { get; init; }
    public bool IsSubmissionAfterDeadlineAllowed { get; init; }
    public bool IsHomework { get; init; }
    public bool IsMandatory { get; init; }
    public bool IsActive { get; init; } = true;
    public DateTimeOffset? ActiveFrom { get; init; }
    public DateTimeOffset? ActiveTo { get; init; }
    public decimal? MaxScore { get; init; }

    /// <summary>"Individual" or "Group" (string: tolerant of unknown values).</summary>
    public string? SubmissionType { get; init; }

    public bool IsAnonymous { get; init; }
    public bool IsSelfAssessmentRequired { get; init; }
    public int? PeersToAssessCount { get; init; }
    public bool IsAssessmentVisibleForStudents { get; init; }

    /// <summary>Files the teacher attached to the assignment.</summary>
    public IReadOnlyList<AssignmentFile> Files { get; init; } = [];

    /// <summary>A template the student is meant to fill in, if any.</summary>
    public AssignmentFile? TemplateFile { get; init; }
}

public sealed record AssignmentFile
{
    public int FileId { get; init; }
    public string Name { get; init; } = "";
    public string? MimeType { get; init; }
    public long Size { get; init; }
    public DateTimeOffset? LastModified { get; init; }
}

/// <summary>The student's own submission (<c>answers/persons/{personId}/v1</c>).</summary>
public sealed record AssignmentAnswer
{
    public int AnswerId { get; init; }
    public bool IsDraft { get; init; }
    public DateTimeOffset? Submitted { get; init; }

    /// <summary>Answer text (HTML).</summary>
    public string? Text { get; init; }

    public IReadOnlyList<AssignmentFile> Files { get; init; } = [];
}

/// <summary>Assessment setup (<c>assignments/{elementId}/settings/v1</c>).</summary>
public sealed record AssignmentSettings
{
    public StatusScale? StatusScale { get; init; }
    public AssessmentScale? AssessmentScale { get; init; }
}

public sealed record StatusScale
{
    public string? Title { get; init; }
    public IReadOnlyList<StatusScaleItem> StatusItems { get; init; } = [];
}

public sealed record StatusScaleItem
{
    public string Title { get; init; } = "";
    public bool IsCompleted { get; init; }
    public bool IsSubmitted { get; init; }
}

public sealed record AssessmentScale
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<AssessmentScaleItem> AssessmentItems { get; init; } = [];
}

public sealed record AssessmentScaleItem
{
    public string Title { get; init; } = "";
    public string? Description { get; init; }
    public decimal? PercentFromAndIncl { get; init; }
    public decimal? PercentTo { get; init; }
}
