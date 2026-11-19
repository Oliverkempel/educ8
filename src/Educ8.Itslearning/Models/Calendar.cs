namespace Educ8.Itslearning.Models;

/// <summary>
/// A calendar event (<c>calendar/events/v1</c>, <c>courses/{id}/calendarevents/v1</c>). At SDU, timetable events are imported:
/// the room is in <see cref="EventTitle"/> ("Lecture - Odense U55") and <see cref="ImportDescription"/> ("Odense U55 teacher names").
/// </summary>
public record CalendarEvent
{
    public int EventId { get; init; }
    public DateTimeOffset FromDate { get; init; }
    public DateTimeOffset ToDate { get; init; }
    public string EventTitle { get; init; } = "";

    /// <summary>Description (HTML stripped by the API; often empty for imported events).</summary>
    public string? Description { get; init; }

    /// <summary>Text from the timetable import (room, teachers).</summary>
    public string? ImportDescription { get; init; }

    /// <summary>"Course", "Project", "Personal" or "Hierarchy".</summary>
    public string? EventType { get; init; }

    /// <summary>Course id for course events.</summary>
    public int LocationId { get; init; }

    public string? LocationTitle { get; init; }
}

/// <summary>
/// <c>courses/{id}/calendarevents/v1</c> uses different names than the user calendar (<c>Id</c>, <c>CourseId</c>, no <c>EventType</c>);
/// mapped to <see cref="CalendarEvent"/>.
/// </summary>
internal sealed record CourseCalendarEventDto
{
    public int Id { get; init; }
    public DateTimeOffset FromDate { get; init; }
    public DateTimeOffset ToDate { get; init; }
    public string EventTitle { get; init; } = "";
    public string? Description { get; init; }
    public string? ImportDescription { get; init; }
    public int CourseId { get; init; }
    public string? LocationTitle { get; init; }

    public CalendarEvent ToEvent() => new()
    {
        EventId = Id, FromDate = FromDate, ToDate = ToDate, EventTitle = EventTitle, Description = Description,
        ImportDescription = ImportDescription, EventType = "Course", LocationId = CourseId, LocationTitle = LocationTitle,
    };
}

/// <summary>One event with its links (<c>calendar/events/{id}/v1</c>).</summary>
public sealed record CalendarEventDetails : CalendarEvent
{
    public string? EventUrl { get; init; }

    /// <summary>True when the description had HTML the API stripped; the web view shows more.</summary>
    public bool HasMoreContent { get; init; }

    public IReadOnlyList<CalendarBooking> Bookings { get; init => field = value ?? []; } = []; // the API sends null for empty lists
    public IReadOnlyList<ElementLink> ElementLinks { get; init => field = value ?? []; } = []; // the API sends null for empty lists
    public IReadOnlyList<CalendarPlanLink> PlanLinks { get; init => field = value ?? []; } = []; // the API sends null for empty lists
}

public sealed record CalendarBooking
{
    public string Name { get; init; } = "";
}

public sealed record CalendarPlanLink
{
    public int PlanId { get; init; }
    public string PlanName { get; init; } = "";
    public string? TopicName { get; init; }
    public string? Url { get; init; }
}

/// <summary>A link to a course element (event links, items behind a grouped notification).</summary>
public sealed record ElementLink
{
    public int ElementId { get; init; }
    public string ElementType { get; init; } = "";
    public int LearningToolId { get; init; }
    public string Title { get; init; } = "";
    public string? Url { get; init; }
    public bool Homework { get; init; }

    public ResourceKind Kind => LearningTools.KindOf(ElementType, LearningToolId);
}
