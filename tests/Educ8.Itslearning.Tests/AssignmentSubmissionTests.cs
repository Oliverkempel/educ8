using System.Net;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class AssignmentSubmissionTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 18, 12, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(true, null, null, "2026-11-25T00:00:00Z", false, SubmissionBlock.None)]
    [InlineData(false, null, null, null, false, SubmissionBlock.Inactive)]
    [InlineData(true, null, "2026-11-01T00:00:00Z", null, false, SubmissionBlock.Closed)]
    [InlineData(true, "2026-12-01T00:00:00Z", null, null, false, SubmissionBlock.NotOpenYet)]
    [InlineData(true, null, null, "2026-11-01T00:00:00Z", false, SubmissionBlock.DeadlinePassed)]
    [InlineData(true, null, null, "2026-11-01T00:00:00Z", true, SubmissionBlock.None)] // late allowed
    public void Rules(bool active, string? from, string? to, string? deadline, bool lateAllowed, SubmissionBlock expected)
    {
        var a = new AssignmentDetails
        {
            IsActive = active,
            ActiveFrom = from is null ? null : DateTimeOffset.Parse(from, System.Globalization.CultureInfo.InvariantCulture),
            ActiveTo = to is null ? null : DateTimeOffset.Parse(to, System.Globalization.CultureInfo.InvariantCulture),
            Deadline = deadline is null ? null : DateTimeOffset.Parse(deadline, System.Globalization.CultureInfo.InvariantCulture),
            IsSubmissionAfterDeadlineAllowed = lateAllowed,
        };
        Assert.Equal(expected, AssignmentSubmission.Check(a, Now));
        Assert.Equal(a.Deadline < Now, AssignmentSubmission.IsLate(a, Now));
    }

    [Fact]
    public void Text_becomes_escaped_paragraphs()
    {
        Assert.Equal("<p>Hello &lt;b&gt;<br />world</p><p>Bye</p>", AssignmentSubmission.TextToHtml("Hello <b>\r\nworld\n\nBye"));
        Assert.Null(AssignmentSubmission.TextToHtml("  "));
        Assert.Equal("application/pdf", AssignmentSubmission.MimeType("x.PDF"));
    }

    [Fact]
    public async Task Rest_files_are_streamed_to_disk_and_failures_leave_nothing()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath.EndsWith("/files/1/v1")
            ? new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent([1, 2, 3]) { Headers = { ContentDisposition = new("attachment") { FileName = "\"../notes.pdf\"" } } } }
            : new HttpResponseMessage(HttpStatusCode.NotFound);
        using var sp = t.Build();
        var client = sp.GetRequiredService<ItslearningClient>();
        var dir = Path.Combine(t.DataDir, "files");

        var reported = new List<double>();
        var path = await client.SaveAsync(ItslearningClient.AssignmentFilePath(9, 1), dir, "fallback.bin", new SyncProgress(reported.Add));
        Assert.Equal(1.0, reported.Last()); // the size is known (3 bytes): progress ends at 100 %
        Assert.Equal(Path.Combine(dir, "notes.pdf"), path); // no directories from the server's name
        Assert.Equal([1, 2, 3], await File.ReadAllBytesAsync(path));

        await Assert.ThrowsAsync<ItslearningApiException>(() => client.SaveAsync(ItslearningClient.AssignmentFilePath(9, 2), dir, "x.bin"));
        Assert.Equal(["notes.pdf"], Directory.GetFiles(dir).Select(Path.GetFileName));
    }

    /// <summary>Reports synchronously (System.Progress posts to the thread pool).</summary>
    private sealed class SyncProgress(Action<double> report) : IProgress<double>
    {
        public void Report(double value) => report(value);
    }
}
