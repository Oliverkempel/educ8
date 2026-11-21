namespace Educ8.Itslearning.Models;

/// <summary>
/// One page of the user's conversations (<c>instantmessages/messagethreads/v4</c>), most recently active first. Page further
/// with the last thread's <see cref="MessageThread.SortIndex"/> as <c>fromSortIndex</c>.
/// </summary>
public sealed record MessageThreadPage
{
    public EntityList<MessageThread> Threads { get; init => field = value ?? new(); } = new();
    public bool HasMore { get; init; }
}

/// <summary>A conversation. Types seen live: "Group", "OneToOne", "Collaboration" (group answers); documented also "Course", "Project".</summary>
public sealed record MessageThread
{
    public int InstantMessageThreadId { get; init; }
    public string? Name { get; init; }
    public string? Type { get; init; }
    public InstantMessage? LastMessage { get; init; }
    public IReadOnlyList<ThreadParticipant> Participants { get; init => field = value ?? []; } = [];
    public int ParticipantsCount { get; init; }
    public long LastReadInstantMessageId { get; init; }
    public long SortIndex { get; init; }

    /// <summary>Teacher broadcasts: only the thread's administrators may write.</summary>
    public bool OnlyThreadAdminCanSendToThread { get; init; }
    public bool IsThreadAdministrator { get; init; }

    /// <summary>False when nobody else in the thread can receive messages any more.</summary>
    public bool HasActiveParticipants { get; init; }
    public bool IsBlocked { get; init; }

    /// <summary>True when the newest message is newer than the user's last read one.</summary>
    public bool IsUnread => LastMessage is { } m && m.MessageId > LastReadInstantMessageId;

    /// <summary>Whether the user can write in this thread.</summary>
    public bool CanReply => !IsBlocked && HasActiveParticipants && (!OnlyThreadAdminCanSendToThread || IsThreadAdministrator);
}

public sealed record ThreadParticipant
{
    public int PersonId { get; init; }
    public string? FullName { get; init; }
}

/// <summary>One page of a thread's messages (<c>…/messagethreads/{id}/messages/v3</c>), <b>newest first</b>. Older ones: <c>fromId</c> = the oldest id.</summary>
public sealed record InstantMessagePage
{
    public EntityList<InstantMessage> Messages { get; init => field = value ?? new(); } = new();
    public bool HasMore { get; init; }
}

/// <summary>A message. <see cref="Text"/> is plain text (live: no HTML in 26 messages).</summary>
public sealed record InstantMessage
{
    public long MessageId { get; init; }
    public int InstantMessageThreadId { get; init; }
    public DateTimeOffset? Created { get; init; }
    public int CreatedBy { get; init; }
    public string? CreatedByName { get; init; }
    public string? Text { get; init; }
    public string? AttachmentUrl { get; init; }
    public string? AttachmentName { get; init; }
    public bool IsSystemMessage { get; init; }
    public bool IsDeleted { get; init; }
}

/// <summary>A person (or course/project) the user may write to (<c>instantmessages/recipients/search/v1</c>).</summary>
public sealed record MessageRecipient
{
    /// <summary>"Person", "Course" or "Project".</summary>
    public string? TypeName { get; init; }
    public int Id { get; init; }
    public string? Label { get; init; }
    public string? Organization { get; init; }
    public bool CanReceive { get; init; }

    public bool IsPerson => string.Equals(TypeName, "Person", StringComparison.OrdinalIgnoreCase);
}

/// <summary>Body of <c>POST instantmessages/v2</c>: a reply (<see cref="InstantMessageThreadId"/>) or a new conversation (<see cref="ToPersonIds"/>).</summary>
internal sealed record SendMessageRequest(
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] int? InstantMessageThreadId,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<int>? ToPersonIds,
    string Text);
