using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Models;

namespace Educ8.Itslearning.Http;

/// <summary>
/// Typed client for the itslearning personal REST API. Returns domain objects; formatting for any front end
/// (LLM Markdown, desktop UI) happens in that front end.
/// </summary>
public sealed class ItslearningClient(HttpClient http)
{
    /// <summary>Maximum page size the API allows.</summary>
    public const int MaxPageSize = 100;

    public Task<Person> GetCurrentPersonAsync(CancellationToken ct = default) =>
        GetAsync<Person>("restapi/personal/person/v1", ct);

    /// <summary>One page of the user's courses (v4: includes teachers, no task counts).</summary>
    public Task<EntityList<PersonCourse>> GetCoursesPageAsync(int pageIndex = 0, int pageSize = MaxPageSize, CancellationToken ct = default) =>
        GetAsync<EntityList<PersonCourse>>($"restapi/personal/courses/v4?PageIndex={pageIndex}&PageSize={Math.Clamp(pageSize, 1, MaxPageSize)}", ct);

    /// <summary>All of the user's courses, across pages.</summary>
    public IAsyncEnumerable<PersonCourse> GetAllCoursesAsync(CancellationToken ct = default) =>
        PageAllAsync((i, c) => GetCoursesPageAsync(i, MaxPageSize, c), ct);

    public Task<CourseInfo> GetCourseAsync(int courseId, CancellationToken ct = default) =>
        GetAsync<CourseInfo>($"restapi/personal/courses/{courseId}/v1", ct);

    public Task<List<CourseTopic>> GetCourseTopicsAsync(int courseId, CancellationToken ct = default) =>
        ToListAsync(PageAllAsync((i, c) => GetAsync<EntityList<CourseTopic>>($"restapi/personal/courses/{courseId}/topics/v1?PageIndex={i}&PageSize={MaxPageSize}", c), ct), ct);

    /// <summary>Contents of a course folder; the root folder if <paramref name="folderId"/> is null.</summary>
    public Task<List<CourseResource>> GetFolderResourcesAsync(int courseId, int? folderId = null, CancellationToken ct = default)
    {
        var basePath = folderId is null
            ? $"restapi/personal/courses/{courseId}/resources/v1"
            : $"restapi/personal/courses/{courseId}/folders/{folderId}/resources/v1";
        return ToListAsync(PageAllAsync(async (i, c) =>
            (await GetAsync<CourseFolderDetails>($"{basePath}?PageIndex={i}&PageSize={MaxPageSize}", c).ConfigureAwait(false)).Resources, ct), ct);
    }

    /// <summary>Searches element titles in the whole course tree.</summary>
    public async Task<List<CourseResource>> SearchCourseResourcesAsync(int courseId, string searchText, CancellationToken ct = default)
    {
        var details = await GetAsync<CourseFolderDetails>(
            $"restapi/personal/courses/{courseId}/resources/search/v1?locationType=Course&searchText={Uri.EscapeDataString(searchText)}", ct).ConfigureAwait(false);
        return details.Resources.EntityArray.ToList();
    }

    /// <summary>One element by id, with its course and folder path.</summary>
    public Task<CourseResourceWithCourse> GetResourceAsync(int elementId, CancellationToken ct = default) =>
        GetAsync<CourseResourceWithCourse>($"restapi/personal/courses/resources/{elementId}/v1", ct);

    /// <summary>The user's tasks in a course. <paramref name="status"/>: "Active" (default) or "Completed".</summary>
    public Task<List<CourseTask>> GetCourseTasksAsync(int courseId, string status = "Active", CancellationToken ct = default) =>
        ToListAsync(PageAllAsync((i, c) => GetAsync<EntityList<CourseTask>>(
            $"restapi/personal/courses/{courseId}/tasks/v1?PageIndex={i}&PageSize={MaxPageSize}&status={Uri.EscapeDataString(status)}", c), ct), ct);

    /// <summary>Latest announcements in a course (newest first).</summary>
    public Task<EntityList<LightBulletin>> GetCourseBulletinsAsync(int courseId, int pageSize = 5, CancellationToken ct = default) =>
        GetAsync<EntityList<LightBulletin>>($"restapi/personal/courses/{courseId}/bulletins/v1?PageIndex=0&PageSize={Math.Clamp(pageSize, 1, MaxPageSize)}", ct);

    /// <summary>Lesson plans overlapping the date range, sorted by start.</summary>
    public async Task<List<PlanPreview>> GetCoursePlansAsync(int courseId, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var plans = await ToListAsync(PageAllAsync((i, c) => GetAsync<EntityList<PlanPreview>>(
            $"restapi/personal/planner/v1?courseId={courseId}&startDate={from:yyyy-MM-dd}&stopDate={to:yyyy-MM-dd}&PageIndex={i}&PageSize={MaxPageSize}", c), ct), ct).ConfigureAwait(false);
        return plans.OrderBy(p => p.Start ?? DateTimeOffset.MaxValue).ThenBy(p => p.PlanName).ToList();
    }

    /// <summary>One lesson plan with its elements (the plan's material and activities). Undocumented endpoint; see <see cref="CoursePlan"/>.</summary>
    public Task<CoursePlan> GetPlanAsync(int courseId, int planId, CancellationToken ct = default) =>
        GetAsync<CoursePlan>($"restapi/personal/course/{courseId}/plans/{planId}/v2?ChildId=0", ct);

    /// <summary>The course's planner topics (undocumented; itslearning's plan page uses it). Plans without a topic aren't counted here.</summary>
    public Task<List<PlanTopic>> GetPlanTopicsAsync(int courseId, CancellationToken ct = default) =>
        GetAsync<List<PlanTopic>>($"restapi/personal/course/{courseId}/plans/topics/v1?ChildId=0&FilterPersonId=0", ct);

    /// <summary>
    /// All plans in a planner topic, with their elements (undocumented). <paramref name="topicId"/> 0 = plans without a topic.
    /// </summary>
    public Task<List<CoursePlan>> GetTopicPlansAsync(int courseId, int topicId, CancellationToken ct = default) =>
        ToListAsync(PageAllAsync((i, c) => GetAsync<EntityList<CoursePlan>>(
            $"restapi/personal/course/{courseId}/plans/topics/{topicId}/all/v1?PageIndex={i}&PageSize={MaxPageSize}&ChildId=0", c), ct), ct);

    /// <summary>The site root (e.g. <c>https://yourschool.itslearning.com/</c>), for turning site-relative links absolute.</summary>
    public Uri? BaseAddress => http.BaseAddress;

    /// <summary>The user's tasks across all courses. <paramref name="status"/>: "Active" (default) or "Completed".</summary>
    public Task<List<CourseTask>> GetMyTasksAsync(string status = "Active", CancellationToken ct = default) =>
        ToListAsync(PageAllAsync((i, c) => GetAsync<EntityList<CourseTask>>(
            $"restapi/personal/tasks/v1?PageIndex={i}&PageSize={MaxPageSize}&status={Uri.EscapeDataString(status)}", c), ct), ct);

    public Task<AssignmentDetails> GetAssignmentAsync(int elementId, CancellationToken ct = default) =>
        GetAsync<AssignmentDetails>($"restapi/personal/assignments/{elementId}/v1", ct);

    public Task<AssignmentSettings> GetAssignmentSettingsAsync(int elementId, CancellationToken ct = default) =>
        GetAsync<AssignmentSettings>($"restapi/personal/assignments/{elementId}/settings/v1", ct);

    /// <summary>
    /// The person's submission, or null if there is none. itslearning answers "no submission" with HTTP 500
    /// ("Error calling API") rather than 404, so both mean null here.
    /// </summary>
    public async Task<AssignmentAnswer?> GetAnswerAsync(int elementId, int personId, CancellationToken ct = default)
    {
        try
        {
            return await GetAsync<AssignmentAnswer>($"restapi/personal/assignments/{elementId}/answers/persons/{personId}/v1", ct).ConfigureAwait(false);
        }
        catch (ItslearningApiException e) when (e.StatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    /// <summary>Files the teacher attached to the feedback on an answer.</summary>
    public Task<List<AssignmentFile>> GetFeedbackFilesAsync(int elementId, int answerId, CancellationToken ct = default) =>
        GetAsync<List<AssignmentFile>>($"restapi/personal/assignments/{elementId}/answers/{answerId}/assessments/files/v1", ct);

    /// <summary>Path of an assignment-related file download (each 302s to a signed file URL).</summary>
    public static string AssignmentFilePath(int elementId, int fileId) => $"restapi/personal/assignments/{elementId}/files/{fileId}/v1";

    public static string AnswerFilePath(int elementId, int answerId, int fileId) => $"restapi/personal/assignments/{elementId}/answers/{answerId}/files/{fileId}/v1";

    public static string FeedbackFilePath(int elementId, int answerId, int fileId) => $"restapi/personal/assignments/{elementId}/answers/{answerId}/assessments/files/{fileId}/v1";

    /// <summary>Downloads a file from a REST endpoint that redirects to the file (redirects are followed; the bearer header isn't sent to other hosts).</summary>
    public async Task<Content.DownloadedFile> DownloadAsync(string relativePath, string fallbackName, long maxBytes, CancellationToken ct = default)
    {
        using var response = await http.GetAsync(relativePath, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, relativePath, ct).ConfigureAwait(false);
        if (response.Content.Headers.ContentLength > maxBytes)
            throw new Content.ContentUnavailableException($"The file is too large to read here ({response.Content.Headers.ContentLength / (1024 * 1024)} MB; limit {maxBytes / (1024 * 1024)} MB).");
        var disposition = response.Content.Headers.ContentDisposition;
        var name = disposition?.FileNameStar ?? disposition?.FileName?.Trim('"') ?? fallbackName;
        await using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var buffer = new MemoryStream();
        var chunk = new byte[81920];
        int read;
        while ((read = await stream.ReadAsync(chunk, ct).ConfigureAwait(false)) > 0)
        {
            if (buffer.Length + read > maxBytes)
                throw new Content.ContentUnavailableException($"The file is too large to read here (limit {maxBytes / (1024 * 1024)} MB).");
            buffer.Write(chunk, 0, read);
        }
        return new Content.DownloadedFile(name, response.Content.Headers.ContentType?.MediaType, buffer.ToArray());
    }

    // ---- Calendar ----

    /// <summary>Upper bound on calendar pages fetched per call (100 events each).</summary>
    private const int MaxCalendarPages = 10;

    /// <summary>
    /// The user's events overlapping [<paramref name="from"/>, <paramref name="to"/>), sorted by start. The endpoint only takes a
    /// start date and returns everything after it (sorted), so pages are fetched until events start after <paramref name="to"/>.
    /// </summary>
    public async Task<List<CalendarEvent>> GetCalendarEventsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default)
    {
        var fromDate = DateOnly.FromDateTime(from.UtcDateTime).AddDays(-1); // events may have started the day before
        var events = new List<CalendarEvent>();
        for (var page = 0; page < MaxCalendarPages; page++)
        {
            var list = await GetAsync<EntityList<CalendarEvent>>(
                $"restapi/personal/calendar/events/v1?fromDate={fromDate:yyyy-MM-dd}&PageIndex={page}&PageSize={MaxPageSize}", ct).ConfigureAwait(false);
            events.AddRange(list.EntityArray);
            if (list.EntityArray.Count == 0 || list.EntityArray[^1].FromDate >= to || events.Count >= list.Total) break;
        }
        return Overlapping(events, from, to);
    }

    /// <summary>A course's events overlapping [<paramref name="from"/>, <paramref name="to"/>), sorted by start.</summary>
    public async Task<List<CalendarEvent>> GetCourseCalendarEventsAsync(int courseId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default)
    {
        var fromDate = DateOnly.FromDateTime(from.UtcDateTime).AddDays(-1);
        var toDate = DateOnly.FromDateTime(to.UtcDateTime).AddDays(1);
        var events = await ToListAsync(PageAllAsync((i, c) => GetAsync<EntityList<CourseCalendarEventDto>>(
            $"restapi/personal/courses/{courseId}/calendarevents/v1?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}&PageIndex={i}&PageSize={MaxPageSize}", c), ct), ct).ConfigureAwait(false);
        return Overlapping(events.Select(e => e.ToEvent()), from, to);
    }

    public Task<CalendarEventDetails> GetCalendarEventAsync(int eventId, CancellationToken ct = default) =>
        GetAsync<CalendarEventDetails>($"restapi/personal/calendar/events/{eventId}/v1", ct);

    private static List<CalendarEvent> Overlapping(IEnumerable<CalendarEvent> events, DateTimeOffset from, DateTimeOffset to) =>
        events.Where(e => e.FromDate < to && e.ToDate > from)
            .DistinctBy(e => e.EventId)
            .OrderBy(e => e.FromDate).ThenBy(e => e.EventTitle, StringComparer.OrdinalIgnoreCase)
            .ToList();

    // ---- What's new ----

    /// <summary>"Latest changes" across the user's courses (itslearning: favourite courses), newest first, since <paramref name="since"/>.</summary>
    public Task<List<StreamItem>> GetStreamAsync(DateTimeOffset since, int max, CancellationToken ct = default) =>
        NewestSinceAsync((i, c) => GetAsync<EntityList<StreamItem>>(
            $"restapi/personal/notifications/stream/v2?showLightBulletins=true&PageIndex={i}&PageSize={MaxPageSize}", c), s => s.PublishedDate, since, max, ct);

    /// <summary>"Latest changes" in one course, newest first, since <paramref name="since"/>.</summary>
    public Task<List<StreamItem>> GetCourseStreamAsync(int courseId, DateTimeOffset since, int max, CancellationToken ct = default) =>
        NewestSinceAsync((i, c) => GetAsync<EntityList<StreamItem>>(
            $"restapi/personal/courses/{courseId}/stream/v1?showLightBulletins=true&PageIndex={i}&PageSize={MaxPageSize}", c), s => s.PublishedDate, since, max, ct);

    /// <summary>Personal notifications (the bell), newest first, since <paramref name="since"/>. Listing doesn't mark them read (that's a separate PUT).</summary>
    public Task<List<Notification>> GetNotificationsAsync(DateTimeOffset since, int max, CancellationToken ct = default) =>
        NewestSinceAsync((i, c) => GetAsync<EntityList<Notification>>(
            $"restapi/personal/notifications/v2?PageIndex={i}&PageSize={MaxPageSize}", c), n => n.PublishedDate, since, max, ct);

    /// <summary>How many personal notifications (the bell) are unread.</summary>
    public Task<int> GetUnreadNotificationCountAsync(CancellationToken ct = default) =>
        GetAsync<int>("restapi/personal/notifications/unread/count/v1", ct);

    // ---- Messages ----

    /// <summary>The user's conversations, most recently active first. Next page: <paramref name="fromSortIndex"/> = the last thread's SortIndex.</summary>
    public Task<MessageThreadPage> GetMessageThreadsAsync(long? fromSortIndex = null, int pageSize = 30, CancellationToken ct = default) =>
        GetAsync<MessageThreadPage>($"restapi/personal/instantmessages/messagethreads/v4?pageSize={Math.Clamp(pageSize, 1, MaxPageSize)}"
            + (fromSortIndex is { } from ? $"&fromSortIndex={from}" : ""), ct);

    /// <summary>A thread's messages, <b>newest first</b>. Older ones: <paramref name="fromId"/> = the oldest message id so far.</summary>
    public Task<InstantMessagePage> GetThreadMessagesAsync(int threadId, long? fromId = null, int pageSize = 30, CancellationToken ct = default) =>
        GetAsync<InstantMessagePage>($"restapi/personal/instantmessages/messagethreads/{threadId}/messages/v3?pageSize={Math.Clamp(pageSize, 1, MaxPageSize)}"
            + (fromId is { } from ? $"&fromId={from}" : ""), ct);

    /// <summary>How many conversations have unread messages.</summary>
    public Task<int> GetUnreadThreadCountAsync(CancellationToken ct = default) =>
        GetAsync<int>("restapi/personal/instantmessages/messagethreads/unread/count/v1", ct);

    /// <summary>People the user may write to, by name (itslearning's e-safety rules apply). Persons that can receive only.</summary>
    public async Task<List<MessageRecipient>> SearchMessageRecipientsAsync(string searchText, CancellationToken ct = default) =>
        (await GetAsync<List<MessageRecipient>>($"restapi/personal/instantmessages/recipients/search/v1?searchText={Uri.EscapeDataString(searchText)}", ct).ConfigureAwait(false))
        .Where(r => r.IsPerson && r.CanReceive).ToList();

    /// <summary>The elements behind a grouped update ("X added A and 5 other elements").</summary>
    public async Task<List<ElementLink>> GetNotificationElementsAsync(int notificationId, CancellationToken ct = default) =>
        (await GetAsync<EntityList<ElementLink>>($"restapi/personal/notifications/{notificationId}/elements/v1?PageIndex=0&PageSize={MaxPageSize}", ct).ConfigureAwait(false))
        .EntityArray.ToList();

    /// <summary>Pages a newest-first feed until items are older than <paramref name="since"/> or <paramref name="max"/> are collected.</summary>
    private static async Task<List<T>> NewestSinceAsync<T>(
        Func<int, CancellationToken, Task<EntityList<T>>> fetchPage, Func<T, DateTimeOffset?> date, DateTimeOffset since, int max, CancellationToken ct)
    {
        var items = new List<T>();
        for (var page = 0; page < 5 && items.Count < max; page++)
        {
            var list = await fetchPage(page, ct).ConfigureAwait(false);
            items.AddRange(list.EntityArray.Where(x => date(x) is not { } d || d >= since));
            var oldest = list.EntityArray.Count > 0 ? date(list.EntityArray[^1]) : null;
            if (list.EntityArray.Count == 0 || oldest < since || (page + 1) * MaxPageSize >= list.Total) break;
        }
        return items.Take(max).ToList();
    }

    // ---- Writes. These change things in itslearning; front ends must gate them (the MCP server: AllowWriteActions). ----

    /// <summary>Uploads a file to itslearning's temporary storage (single use, about 24 h), e.g. for an assignment answer.</summary>
    public async Task<UploadedTempFile> UploadTempFileAsync(string fileName, Stream content, string? mimeType = null, CancellationToken ct = default)
    {
        const string path = "restapi/personal/fileupload/temp/v1";
        using var form = new MultipartFormDataContent();
        var file = new StreamContent(content);
        file.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType ?? "application/octet-stream");
        form.Add(file, "file", fileName);
        using var response = await http.PostAsync(path, form, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, path, ct).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<UploadedTempFile>(ItslearningJson.Options, ct).ConfigureAwait(false)
               ?? throw new ItslearningApiException(response.StatusCode, path, "empty upload response");
    }

    /// <summary>Submits the student's answer (text as HTML, plus uploaded temp files). Returns the answer id.</summary>
    public async Task<int> SubmitAnswerAsync(int elementId, string? textHtml, IReadOnlyList<UploadedTempFile> files, CancellationToken ct = default)
    {
        var path = $"restapi/personal/assignments/{elementId}/answers/v1";
        using var response = await http.PostAsJsonAsync(path, new SubmitAnswerRequest(textHtml, files), ItslearningJson.Request, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, path, ct).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<SubmitAnswerResponse>(ItslearningJson.Options, ct).ConfigureAwait(false);
        return result?.AnswerId ?? 0;
    }

    /// <summary>Marks personal notifications (the bell) as read, as opening them on itslearning does.</summary>
    public async Task MarkNotificationsReadAsync(IEnumerable<int> notificationIds, CancellationToken ct = default)
    {
        const string path = "restapi/personal/notifications/v2";
        var body = notificationIds.Distinct().Select(id => new NotificationReadUpdate(id, true)).ToList();
        if (body.Count == 0) return;
        using var response = await http.PutAsJsonAsync(path, body, ItslearningJson.Request, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, path, ct).ConfigureAwait(false);
    }

    /// <summary>Marks a thread read up to <paramref name="lastMessageId"/> (its newest message), as opening it on itslearning does.</summary>
    public async Task MarkThreadReadAsync(int threadId, long lastMessageId, CancellationToken ct = default)
    {
        var path = $"restapi/personal/instantmessages/messagethreads/{threadId}/updatelastread/{lastMessageId}/v1";
        using var response = await http.PutAsync(path, null, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, path, ct).ConfigureAwait(false);
    }

    /// <summary>Sends a message: a reply in <paramref name="threadId"/>, or a new conversation with <paramref name="toPersonIds"/> (itslearning reuses an existing thread with the same people). Returns the thread.</summary>
    public async Task<MessageThread> SendMessageAsync(int? threadId, IReadOnlyList<int>? toPersonIds, string text, CancellationToken ct = default)
    {
        if (threadId is null && (toPersonIds is null || toPersonIds.Count == 0)) throw new ArgumentException("A thread or at least one recipient is needed.");
        const string path = "restapi/personal/instantmessages/v2";
        using var response = await http.PostAsJsonAsync(path, new SendMessageRequest(threadId, threadId is null ? toPersonIds : null, text), ItslearningJson.Request, ct)
            .ConfigureAwait(false);
        await EnsureSuccessAsync(response, path, ct).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<MessageThread>(ItslearningJson.Options, ct).ConfigureAwait(false)
               ?? throw new ItslearningApiException(response.StatusCode, path, "empty response body");
    }

    /// <summary>
    /// Downloads a file from the itslearning site itself with the user's login (e.g. a message attachment) into
    /// <paramref name="directory"/>. Returns null for links elsewhere (the login is never sent to other hosts) or responses that
    /// aren't a file (an HTML page); the caller then opens the link in the browser.
    /// </summary>
    public async Task<string?> TryDownloadSiteFileAsync(Uri url, string directory, string? fileName, CancellationToken ct = default)
    {
        if (http.BaseAddress is not { } site || !url.IsAbsoluteUri || url.Scheme != Uri.UriSchemeHttps
            || !string.Equals(url.Host, site.Host, StringComparison.OrdinalIgnoreCase)) return null;
        using var response = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode || response.Content.Headers.ContentType?.MediaType == "text/html") return null;
        var disposition = response.Content.Headers.ContentDisposition;
        var name = Content.LearningToolContentReader.SafeFileName(disposition?.FileNameStar ?? disposition?.FileName?.Trim('"') ?? fileName, 0);
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, name);
        var partial = path + ".part";
        try
        {
            await using (var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false))
            await using (var file = File.Create(partial))
                await stream.CopyToAsync(file, ct).ConfigureAwait(false);
            File.Move(partial, path, overwrite: true);
        }
        catch
        {
            File.Delete(partial);
            throw;
        }
        return path;
    }

    /// <summary>A short-lived URL that signs the user in to the itslearning web UI and opens <paramref name="itslearningUrl"/>.</summary>
    public async Task<Uri> GetSsoUrlAsync(Uri itslearningUrl, CancellationToken ct = default)
    {
        var result = await GetAsync<SsoUrl>($"restapi/personal/sso/url/v1?url={Uri.EscapeDataString(itslearningUrl.ToString())}", ct).ConfigureAwait(false);
        return new Uri(result.Url);
    }

    private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> items, CancellationToken ct)
    {
        var list = new List<T>();
        await foreach (var item in items.WithCancellation(ct).ConfigureAwait(false)) list.Add(item);
        return list;
    }

    /// <summary>GET a JSON resource. Throws <see cref="NotAuthenticatedException"/> or <see cref="ItslearningApiException"/>.</summary>
    public async Task<T> GetAsync<T>(string relativePath, CancellationToken ct = default)
    {
        using var response = await http.GetAsync(relativePath, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, relativePath, ct).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<T>(ItslearningJson.Options, ct).ConfigureAwait(false)
               ?? throw new ItslearningApiException(response.StatusCode, StripQuery(relativePath), "empty response body");
    }

    internal static async IAsyncEnumerable<T> PageAllAsync<T>(
        Func<int, CancellationToken, Task<EntityList<T>>> fetchPage,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var seen = 0;
        for (var pageIndex = 0; ; pageIndex++)
        {
            var page = await fetchPage(pageIndex, ct).ConfigureAwait(false);
            foreach (var item in page.EntityArray) yield return item;
            seen += page.EntityArray.Count;
            if (page.EntityArray.Count == 0 || seen >= page.Total) yield break;
        }
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, string path, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode) return;
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new NotAuthenticatedException("itslearning rejected the session (401). Log in again.");
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var detail = body.Length > 200 ? body[..200] + "…" : body;
        throw new ItslearningApiException(response.StatusCode, StripQuery(path), detail);
    }

    private static string StripQuery(string path) => path.Split('?', 2)[0];
}
