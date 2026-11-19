using System.ComponentModel;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Mcp.Formatting;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class CourseTools
{
    [McpServerTool(Name = "list_my_courses", Title = "List my itslearning courses", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Lists the user's itslearning courses (id, title, code, semester, last activity, teachers), most recently active first. " +
                 "Use it to find the course id when the user mentions a course by name.")]
    public static Task<string> ListMyCourses(
        LoginService login, ItslearningClient client,
        [Description("Optional text to match against course title or code, e.g. \"distributed\" or \"T510046\".")] string? search = null,
        [Description("Maximum number of courses to show (default 50).")] int maxResults = 50,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var all = new List<Educ8.Itslearning.Models.PersonCourse>();
            await foreach (var course in client.GetAllCoursesAsync(ct)) all.Add(course);
            var selected = CourseFormatter.Select(all, search);
            return CourseFormatter.FormatList(selected, all.Count, search, Math.Clamp(maxResults, 1, 500));
        }, ct);
}
