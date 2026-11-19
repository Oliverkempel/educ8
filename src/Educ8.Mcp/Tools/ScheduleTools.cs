using System.ComponentModel;
using System.Globalization;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Educ8.Mcp.Formatting;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class ScheduleTools
{
    internal const int MaxDays = 42;

    [McpServerTool(Name = "get_schedule", Title = "My schedule", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("The user's calendar/timetable: lectures, exercise classes, project days and other events with times, rooms and teachers, " +
                 "grouped by day. Default: today and the next 6 days. ALWAYS use this for schedule questions (\"what do I have this week / " +
                 "next week / tomorrow?\", \"am I free on Monday?\"); course lesson plans are not the timetable.")]
    public static Task<string> GetSchedule(
        LoginService login, ItslearningClient client, TimeProvider time,
        [Description("First day, as yyyy-MM-dd, or \"today\" / \"tomorrow\". Default: today.")] string? from = null,
        [Description($"Number of days to include (default 7, max 42).")] int days = 7,
        [Description("Only this course (id from `list_my_courses`). Omit for all courses.")] int? courseId = null,
        [Description("Maximum number of events to show (default 80).")] int maxResults = 80,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var zone = time.LocalTimeZone;
            var startDay = ParseDay(from, time.GetLocalNow().Date);
            var endDay = startDay.AddDays(Math.Clamp(days, 1, MaxDays));
            var start = AtMidnight(startDay, zone);
            var end = AtMidnight(endDay, zone);

            string? courseTitle = null;
            List<CalendarEvent> events;
            if (courseId is { } id)
            {
                var courseTask = client.GetCourseAsync(id, ct);
                var eventsTask = client.GetCourseCalendarEventsAsync(id, start, end, ct);
                await Task.WhenAll(courseTask, eventsTask);
                courseTitle = courseTask.Result.Title;
                events = eventsTask.Result;
            }
            else events = await client.GetCalendarEventsAsync(start, end, ct);

            return ScheduleFormatter.FormatSchedule(events, startDay, endDay, courseTitle, Math.Clamp(maxResults, 1, 500), zone);
        }, ct);

    [McpServerTool(Name = "get_event", Title = "Calendar event details", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("One calendar event: time, place, description, the lesson plan it belongs to with that plan's material (element ids), " +
                 "and linked material. Use it to prepare for a specific lecture or class.")]
    public static Task<string> GetEvent(
        LoginService login, ItslearningClient client, TimeProvider time,
        [Description("Event id from `get_schedule`.")] int eventId,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var e = await client.GetCalendarEventAsync(eventId, ct);
            var plans = await client.GetEventPlansAsync(e, ct);
            return ScheduleFormatter.FormatEvent(e, plans, time.LocalTimeZone);
        }, ct);

    internal static DateTime ParseDay(string? value, DateTime today) => value?.Trim().ToLowerInvariant() switch
    {
        null or "" or "today" => today,
        "tomorrow" => today.AddDays(1),
        "yesterday" => today.AddDays(-1),
        var s when DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) => d.Date,
        _ => throw new McpException($"\"{value}\" isn't a date; use yyyy-MM-dd, \"today\" or \"tomorrow\"."),
    };

    /// <summary>Local midnight of <paramref name="day"/> in <paramref name="zone"/> as an absolute time.</summary>
    internal static DateTimeOffset AtMidnight(DateTime day, TimeZoneInfo zone)
    {
        var local = DateTime.SpecifyKind(day.Date, DateTimeKind.Unspecified);
        return new DateTimeOffset(local, zone.GetUtcOffset(local));
    }
}
