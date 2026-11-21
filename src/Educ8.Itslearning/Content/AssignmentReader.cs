using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.Itslearning.Content;

/// <summary>Everything known about one assignment for the signed-in student, from REST and the assignment's web view.</summary>
public sealed record AssignmentInfo(
    CourseResourceWithCourse? Element,
    AssignmentDetails Details,
    AssignmentSettings? Settings,
    AssignmentAnswer? Answer,
    IReadOnlyList<AssignmentFile> FeedbackFiles,
    AssignmentView? View)
{
    /// <summary>
    /// True when the web view (status, score, feedback, "submitted" line) couldn't be read. The REST answer lookup can't tell
    /// "nothing submitted" from an error (both are a 500), so without the view a missing answer isn't proof nothing was handed in.
    /// </summary>
    public bool ViewUnavailable => View is null;
}

/// <summary>Loads <see cref="AssignmentInfo"/>; shared by the MCP tools and the desktop app.</summary>
public static class AssignmentReader
{
    /// <summary>Gathers REST data and the web view in parallel. Optional parts that fail are left out (see <see cref="AssignmentInfo"/>).</summary>
    public static async Task<AssignmentInfo> LoadAssignmentAsync(this ItslearningClient client, LearningToolContentReader content, int elementId,
        CancellationToken ct = default)
    {
        var detailsTask = client.GetAssignmentAsync(elementId, ct);
        var elementTask = Optional(() => client.GetResourceAsync(elementId, ct));
        var settingsTask = Optional(() => client.GetAssignmentSettingsAsync(elementId, ct));
        var meTask = client.GetCurrentPersonAsync(ct);
        var viewTask = Optional(() => content.ReadAssignmentViewAsync(elementId, ct));
        await Task.WhenAll(detailsTask, elementTask, settingsTask, meTask, viewTask).ConfigureAwait(false);

        var answer = await client.GetAnswerAsync(elementId, meTask.Result.PersonId, ct).ConfigureAwait(false);
        IReadOnlyList<AssignmentFile> feedbackFiles = [];
        if (answer is not null)
            feedbackFiles = await Optional(() => client.GetFeedbackFilesAsync(elementId, answer.AnswerId, ct)).ConfigureAwait(false) ?? [];
        return new AssignmentInfo(elementTask.Result, detailsTask.Result, settingsTask.Result, answer, feedbackFiles, viewTask.Result);
    }

    private static async Task<T?> Optional<T>(Func<Task<T>> load) where T : class
    {
        try { return await load().ConfigureAwait(false); }
        catch (NotAuthenticatedException) { throw; }
        catch (Exception e) when (e is ItslearningApiException or ContentUnavailableException or HttpRequestException) { return null; }
    }
}
