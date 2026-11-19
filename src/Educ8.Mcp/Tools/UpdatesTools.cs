using System.ComponentModel;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Educ8.Mcp.Formatting;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class UpdatesTools
{
    private const int MaxGroupsExpanded = 10;

    [McpServerTool(Name = "whats_new", Title = "What's new", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Recent changes: personal notifications (e.g. feedback available) and course updates (new or updated material, " +
                 "assignments, announcements), newest first, with element ids to open them. Use it for \"what's new / what did I miss?\". " +
                 "Reading this doesn't mark anything as read.")]
    public static Task<string> WhatsNew(
        LoginService login, ItslearningClient client, TimeProvider time,
        [Description("Only this course (id from `list_my_courses`). Omit for all your courses.")] int? courseId = null,
        [Description("How many days back to look (default 7, max 60).")] int days = 7,
        [Description("Maximum number of course updates to show (default 40).")] int maxResults = 40,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            days = Math.Clamp(days, 1, 60);
            var max = Math.Clamp(maxResults, 1, 200);
            var since = time.GetUtcNow().AddDays(-days);
            var notes = new List<string>();

            string? courseTitle = null;
            List<StreamItem> stream;
            List<Notification>? personal = null;
            if (courseId is { } id)
            {
                var courseTask = client.GetCourseAsync(id, ct);
                var streamTask = client.GetCourseStreamAsync(id, since, max, ct);
                await Task.WhenAll(courseTask, streamTask);
                courseTitle = courseTask.Result.Title;
                stream = streamTask.Result;
            }
            else
            {
                var streamTask = client.GetStreamAsync(since, max, ct);
                var personalTask = Section<List<Notification>>.LoadAsync(() => client.GetNotificationsAsync(since, 30, ct));
                await Task.WhenAll(streamTask, personalTask);
                stream = streamTask.Result;
                personal = WithoutStreamDuplicates(personalTask.Result.Value, stream);
                if (personalTask.Result.Error is not null) notes.Add("Personal notifications couldn't be loaded right now.");
                notes.Add("Course updates come from itslearning's \"latest changes\" feed, which follows your favourite courses. For another course, pass its courseId.");
            }

            var groups = await ExpandGroupsAsync(client, stream, ct);
            return UpdatesFormatter.Format(personal, stream, groups, days, courseTitle, time.LocalTimeZone, notes);
        }, ct);

    /// <summary>
    /// The bell repeats announcements ("New announcement available in …") that the stream already shows with their text.
    /// Drop bell items without an element that coincide (±5 min) with an announcement in the stream. Language-independent.
    /// </summary>
    internal static List<Notification>? WithoutStreamDuplicates(List<Notification>? personal, List<StreamItem> stream)
    {
        if (personal is null) return null;
        var announcements = stream.Where(s => s.LightBulletin is not null && s.PublishedDate is not null).Select(s => s.PublishedDate!.Value).ToList();
        return personal.Where(n => n.ElementId is not null || n.PublishedDate is not { } d
                                   || !announcements.Any(a => (a - d).Duration() <= TimeSpan.FromMinutes(5))).ToList();
    }

    /// <summary>Loads the elements behind grouped updates ("X added A and 5 other elements"). Best effort.</summary>
    private static async Task<IReadOnlyDictionary<int, List<ElementLink>>> ExpandGroupsAsync(ItslearningClient client, List<StreamItem> stream, CancellationToken ct)
    {
        var groupIds = stream.Where(s => s.IsGroup).Select(s => s.NotificationId).Distinct().Take(MaxGroupsExpanded).ToList();
        var loaded = await Task.WhenAll(groupIds.Select(async id =>
        {
            var section = await Section<List<ElementLink>>.LoadAsync(() => client.GetNotificationElementsAsync(id, ct));
            return (Id: id, Elements: section.Value);
        }));
        return loaded.Where(g => g.Elements is not null).ToDictionary(g => g.Id, g => g.Elements!);
    }
}
