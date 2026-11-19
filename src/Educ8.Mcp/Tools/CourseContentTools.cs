using System.ComponentModel;
using System.Text;
using Educ8.Documents;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Educ8.Mcp.Formatting;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class CourseContentTools
{
    /// <summary>Cap on text returned by one call, to protect the model's context.</summary>
    internal const int MaxOutputChars = 40_000;

    [McpServerTool(Name = "get_course_overview", Title = "Course overview", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("One-shot orientation for a course: topics (with folder ids), lesson plans around today, open tasks, " +
                 "latest announcements and top-level material. Start here when the user wants help with a course.")]
    public static Task<string> GetCourseOverview(
        LoginService login, ItslearningClient client, TimeProvider time,
        [Description("Course id from `list_my_courses`.")] int courseId,
        [Description("How many weeks ahead to include lesson plans for (default 3; one week back is always included).")] int weeksAhead = 3,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var course = await client.GetCourseAsync(courseId, ct);
            var today = DateOnly.FromDateTime(time.GetLocalNow().DateTime);
            var from = today.AddDays(-7);
            var to = today.AddDays(7 * Math.Clamp(weeksAhead, 0, 26));

            var topics = Section<List<CourseTopic>>.LoadAsync(() => client.GetCourseTopicsAsync(courseId, ct));
            var root = Section<List<CourseResource>>.LoadAsync(() => client.GetFolderResourcesAsync(courseId, null, ct));
            var plans = Section<List<PlanPreview>>.LoadAsync(() => client.GetCoursePlansAsync(courseId, from, to, ct));
            var tasks = Section<List<CourseTask>>.LoadAsync(() => client.GetCourseTasksAsync(courseId, "Active", ct));
            var bulletins = Section<EntityList<LightBulletin>>.LoadAsync(() => client.GetCourseBulletinsAsync(courseId, 5, ct));
            var changes = Section<List<StreamItem>>.LoadAsync(() =>
                client.GetCourseStreamAsync(courseId, time.GetUtcNow().AddDays(-CourseOverviewFormatter.RecentChangesDays), 10, ct));
            var now = time.GetUtcNow();
            var events = Section<List<CalendarEvent>>.LoadAsync(() =>
                client.GetCourseCalendarEventsAsync(courseId, now, now.AddDays(CourseOverviewFormatter.UpcomingDays), ct));
            await Task.WhenAll(topics, root, plans, tasks, bulletins, changes, events);
            var planMaterials = await client.GetPlanMaterialsAsync(courseId, topics.Result.Value, plans.Result.Value, ct);

            var overview = new CourseOverview(course, topics.Result, root.Result, plans.Result, tasks.Result, bulletins.Result, from, to, planMaterials, changes.Result, events.Result);
            return HtmlToMarkdown.Truncate(CourseOverviewFormatter.Format(overview, time.LocalTimeZone), MaxOutputChars);
        }, ct);

    [McpServerTool(Name = "browse_course_materials", Title = "Browse course folder", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Lists the items in a course folder (the course's top level if folderId is omitted): element id, type and title.")]
    public static Task<string> BrowseCourseMaterials(
        LoginService login, ItslearningClient client,
        [Description("Course id from `list_my_courses`.")] int courseId,
        [Description("Folder element id (from a previous listing or a topic). Omit for the top level.")] int? folderId = null,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var itemsTask = client.GetFolderResourcesAsync(courseId, folderId, ct);
            var folderTask = folderId is { } id ? client.GetResourceAsync(id, ct) : Task.FromResult<CourseResourceWithCourse>(null!);
            await Task.WhenAll(itemsTask, folderTask);
            var items = itemsTask.Result;

            var sb = new StringBuilder();
            if (folderId is null) sb.Append($"## Top level of course {courseId}\n\n");
            else
            {
                var folder = folderTask.Result;
                sb.Append($"## Folder: {folder.Title}\n\nFolder id {folder.ElementId} in {folder.CourseTitle} (course {folder.CourseId})");
                var path = ResourceFormatter.CleanPath(folder.Path);
                if (path.Length > 0) sb.Append($" · location: {path}");
                sb.Append("\n\n");
            }
            if (items.Count == 0) return sb.Append("This folder is empty (or you can't see its contents).").ToString();
            sb.Append(ResourceFormatter.Table(items, includePath: false)).Append('\n').Append(ResourceFormatter.UseLegend);
            return HtmlToMarkdown.Truncate(sb.ToString(), MaxOutputChars);
        }, ct);

    [McpServerTool(Name = "search_course_materials", Title = "Search course material", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Searches item titles anywhere in a course's folder tree, e.g. \"linux\" or \"week 5\". Returns element ids, types and where each item is.")]
    public static Task<string> SearchCourseMaterials(
        LoginService login, ItslearningClient client,
        [Description("Course id from `list_my_courses`.")] int courseId,
        [Description("Words to look for in item titles.")] string query,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            if (string.IsNullOrWhiteSpace(query)) return "Give a search text, e.g. a topic or file name.";
            var results = await client.SearchCourseResourcesAsync(courseId, query.Trim(), ct);
            if (results.Count == 0)
                return $"Nothing in course {courseId} has a title matching \"{query}\". The search only matches titles; try a shorter or different word, or browse the topics.";
            var sb = new StringBuilder($"## {results.Count} items matching \"{query}\" in course {courseId}\n\n");
            sb.Append(ResourceFormatter.Table(results, includePath: true)).Append('\n').Append(ResourceFormatter.UseLegend);
            return HtmlToMarkdown.Truncate(sb.ToString(), MaxOutputChars);
        }, ct);

    [McpServerTool(Name = "read_material", Title = "Read course material", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Reads one course item by element id. Pages come back as Markdown, links as their target URL, folders as a listing, " +
                 "and files (PDF, Word, PowerPoint, Excel, text, images) as their content (same as `read_file`, first chunk). " +
                 "Assignments come back like `get_assignment`. Tests and other tools give a short summary and an itslearning link.")]
    public static Task<CallToolResult> ReadMaterial(
        LoginService login, ItslearningClient client, LearningToolContentReader content, TimeProvider time,
        [Description("Element id from a listing, search or overview.")] int elementId,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var item = await client.GetResourceAsync(elementId, ct);
            if (item.Kind == ResourceKind.File)
                return await FileTools.ReadAsync(content, item, pages: null, includeImages: false, zipEntry: null, ct);
            if (item.Kind == ResourceKind.Assignment)
            {
                var info = await AssignmentTools.LoadAsync(client, content, elementId, ct);
                return Text(AssignmentFormatter.Format(info, time.LocalTimeZone, time.GetUtcNow()));
            }
            return Text(await ReadNonFileAsync(client, content, item, ct));
        }, ct);

    private static CallToolResult Text(string text) => new() { Content = [new TextContentBlock { Text = text }] };

    private static async Task<string> ReadNonFileAsync(ItslearningClient client, LearningToolContentReader content, CourseResourceWithCourse item, CancellationToken ct)
    {
            var sb = new StringBuilder();
            sb.Append($"# {item.Title}\n\n");
            sb.Append($"{ResourceFormatter.KindLabel(item.Kind, item.LearningToolId)} · element {item.ElementId} · {item.CourseTitle} (course {item.CourseId})");
            var path = ResourceFormatter.CleanPath(item.Path);
            if (path.Length > 0) sb.Append($" · location: {path}");
            if (!string.IsNullOrEmpty(item.Url)) sb.Append($"\nOpen in itslearning: {item.Url}");
            sb.Append("\n\n");

            switch (item.Kind)
            {
                case ResourceKind.Folder or ResourceKind.LearningPath:
                    var children = await client.GetFolderResourcesAsync(item.CourseId, item.ElementId, ct);
                    sb.Append(children.Count == 0 ? "Empty." : ResourceFormatter.Table(children, includePath: false) + "\n" + ResourceFormatter.UseLegend);
                    break;

                case ResourceKind.Page:
                    var page = await content.ReadPageAsync(item.ElementId, ct);
                    AppendPage(sb, page);
                    break;

                case ResourceKind.Link:
                    var link = await content.ReadLinkAsync(item.ElementId, ct);
                    sb.Append(link.Url is null ? "The link target couldn't be found." : $"Link target: <{link.Url}>");
                    var description = HtmlToMarkdown.Convert(link.DescriptionHtml);
                    if (description.Length > 0) sb.Append($"\n\n{description}");
                    sb.Append("\n\nThis server doesn't fetch external websites; open the link if you need its contents.");
                    break;

                default:
                    sb.Append("This kind of item can't be read through this server; the user can open it in itslearning.");
                    break;
            }
            return HtmlToMarkdown.Truncate(sb.ToString().TrimEnd(), MaxOutputChars);
    }

    internal static void AppendPage(StringBuilder sb, PageContent page)
    {
        var any = false;
        foreach (var block in page.Blocks)
        {
            var body = HtmlToMarkdown.Convert(block.Html);
            if (body.Length == 0 && block.Title is null) continue;
            any = true;
            if (block.Title is not null) sb.Append($"## {block.Title}\n\n");
            if (block.Kind != "text") sb.Append($"_[{block.Kind} block]_\n\n");
            if (body.Length > 0) sb.Append(body).Append("\n\n");
        }
        if (!any) sb.Append("This page is empty.");
    }
}
