using System.ComponentModel;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Educ8.Mcp.Formatting;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class AssignmentTools
{
    [McpServerTool(Name = "list_my_tasks", Title = "My tasks and assignments", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Lists the user's open (or completed) tasks: assignments, tests and activities, with deadlines and status, grouped by course. " +
                 "Use it to find what the user has to do, or the element id of an assignment they mention.")]
    public static Task<string> ListMyTasks(
        LoginService login, ItslearningClient client, TimeProvider time,
        [Description("Only this course (id from `list_my_courses`). Omit for all courses.")] int? courseId = null,
        [Description("\"active\" (default) for open tasks, or \"completed\".")] string status = "active",
        [Description("Maximum number of tasks to show (default 60).")] int maxResults = 60,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var apiStatus = status.Trim().ToLowerInvariant() switch
            {
                "active" or "open" or "" => "Active",
                "completed" or "done" => "Completed",
                _ => throw new McpException("status must be \"active\" or \"completed\"."),
            };
            var tasksTask = courseId is { } id ? client.GetCourseTasksAsync(id, apiStatus, ct) : client.GetMyTasksAsync(apiStatus, ct);
            var coursesTask = ListCoursesAsync(client, ct);
            await Task.WhenAll(tasksTask, coursesTask);
            var courses = coursesTask.Result.OrderByDescending(c => c.LastUpdatedUtc ?? DateTimeOffset.MinValue).ToList();
            return AssignmentFormatter.FormatTasks(tasksTask.Result, courses, apiStatus, Math.Clamp(maxResults, 1, 500), time.LocalTimeZone);
        }, ct);

    [McpServerTool(Name = "get_assignment", Title = "Assignment details", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Everything about one assignment: description, deadline, submission type, max score, grading scale, the teacher's files, " +
                 "the user's own submission (text and files), status, score and the teacher's written feedback. " +
                 "Use it when the user is working on an assignment or asks about feedback. Read files with `read_assignment_file`.")]
    public static Task<string> GetAssignment(
        LoginService login, ItslearningClient client, LearningToolContentReader content, TimeProvider time,
        [Description("Element id of the assignment (from `list_my_tasks`, an overview, listing or search).")] int elementId,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var info = await LoadAsync(client, content, elementId, ct);
            return AssignmentFormatter.Format(info, time.LocalTimeZone, time.GetUtcNow());
        }, ct);

    [McpServerTool(Name = "read_assignment_file", Title = "Read an assignment file", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Reads a file belonging to an assignment: one the teacher attached, the user's own submitted file, or a feedback file " +
                 "(file ids are listed by `get_assignment`). Same formats and options as `read_file`.")]
    public static Task<CallToolResult> ReadAssignmentFile(
        LoginService login, ItslearningClient client,
        [Description("Element id of the assignment.")] int elementId,
        [Description("fileId from `get_assignment`.")] int fileId,
        [Description("Pages/slides/sheets to read, e.g. \"1-10\". Default: from the start until the response is full.")] string? pages = null,
        [Description("Also return rendered PDF pages or embedded images for the selected pages.")] bool includeImages = false,
        [Description("For zip archives: path of the file inside the archive to read.")] string? zipEntry = null,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var assignmentTask = client.GetAssignmentAsync(elementId, ct);
            var meTask = client.GetCurrentPersonAsync(ct);
            await Task.WhenAll(assignmentTask, meTask);
            var assignment = assignmentTask.Result;

            string? path = null, role = null, name = null;
            if (assignment.Files.Concat(assignment.TemplateFile is null ? [] : [assignment.TemplateFile]).FirstOrDefault(f => f.FileId == fileId) is { } teacherFile)
                (path, role, name) = (ItslearningClient.AssignmentFilePath(elementId, fileId), "the teacher's file for", teacherFile.Name);
            else if (await client.GetAnswerAsync(elementId, meTask.Result.PersonId, ct) is { } answer)
            {
                if (answer.Files.FirstOrDefault(f => f.FileId == fileId) is { } own)
                    (path, role, name) = (ItslearningClient.AnswerFilePath(elementId, answer.AnswerId, fileId), "your submitted file for", own.Name);
                else if ((await client.GetFeedbackFilesAsync(elementId, answer.AnswerId, ct)).FirstOrDefault(f => f.FileId == fileId) is { } fb)
                    (path, role, name) = (ItslearningClient.FeedbackFilePath(elementId, answer.AnswerId, fileId), "a feedback file for", fb.Name);
            }
            if (path is null)
                throw new McpException($"File {fileId} isn't attached to assignment {elementId}, your submission or its feedback. Call `get_assignment` to see the file ids.");

            var file = await client.DownloadAsync(path, name ?? "file", LearningToolContentReader.MaxFileBytes, ct);
            return FileTools.Render(file, $"{role} assignment \"{assignment.Title}\" (element {elementId})", pages, includeImages, zipEntry);
        }, ct);

    /// <summary>Gathers REST data and the web view in parallel; parts that fail are reported, not fatal.</summary>
    internal static async Task<AssignmentInfo> LoadAsync(ItslearningClient client, LearningToolContentReader content, int elementId, CancellationToken ct)
    {
        var problems = new List<string>();
        var detailsTask = client.GetAssignmentAsync(elementId, ct);
        var elementTask = Optional(() => client.GetResourceAsync(elementId, ct));
        var settingsTask = Optional(() => client.GetAssignmentSettingsAsync(elementId, ct));
        var meTask = client.GetCurrentPersonAsync(ct);
        var viewTask = Optional(() => content.ReadAssignmentViewAsync(elementId, ct));
        await Task.WhenAll(detailsTask, elementTask, settingsTask, meTask, viewTask);

        var answer = await client.GetAnswerAsync(elementId, meTask.Result.PersonId, ct);
        IReadOnlyList<AssignmentFile> feedbackFiles = [];
        if (answer is not null)
            feedbackFiles = await Optional(() => client.GetFeedbackFilesAsync(elementId, answer.AnswerId, ct)) ?? [];
        if (viewTask.Result is null)
            problems.Add("Your status, score and the teacher's feedback couldn't be loaded from the assignment page right now.");

        return new AssignmentInfo(elementTask.Result, detailsTask.Result, settingsTask.Result, answer, feedbackFiles, viewTask.Result, problems);
    }

    private static async Task<T?> Optional<T>(Func<Task<T>> load) where T : class
    {
        try { return await load(); }
        catch (NotAuthenticatedException) { throw; }
        catch (Exception e) when (e is ItslearningApiException or ContentUnavailableException or HttpRequestException) { return null; }
    }

    private static async Task<List<PersonCourse>> ListCoursesAsync(ItslearningClient client, CancellationToken ct)
    {
        var list = new List<PersonCourse>();
        await foreach (var c in client.GetAllCoursesAsync(ct)) list.Add(c);
        return list;
    }
}
