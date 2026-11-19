using System.Net;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class AssignmentViewParserTests
{
    [Fact]
    public void Graded_assignment_has_status_score_feedback_and_submission()
    {
        var view = WebPageParsers.Assignment(Fixtures.Read("assignment_graded.html"));

        Assert.Equal("Completed", view.Status);
        Assert.Equal("5 of 5 (100%)", view.Score);
        Assert.Equal("Assessed on 29. marts 2026 12:00 by Person Name", view.AssessedBy);
        Assert.Contains("Task 1: Good ER diagram.", view.FeedbackHtml);
        Assert.Equal("Submitted: 2. marts 2026 15:53 by Person Name", view.SubmittedLine);
        Assert.True(view.HasAssessment);
    }

    [Fact]
    public void Feedback_without_score()
    {
        var view = WebPageParsers.Assignment(Fixtures.Read("assignment_feedback.html"));

        Assert.Null(view.Score);
        Assert.StartsWith("Assessed on 16. september 2026 11:34", view.AssessedBy);
        Assert.Equal("Cool idea! Keep the data model simple to begin with.", view.FeedbackHtml);
    }

    [Fact]
    public void Unsubmitted_assignment_has_status_only()
    {
        var view = WebPageParsers.Assignment(Fixtures.Read("assignment_unsubmitted.html"));

        Assert.Equal("Not submitted", view.Status);
        Assert.False(view.HasAssessment);
        Assert.Null(view.SubmittedLine);
    }
}

public class AssignmentClientTests
{
    [Fact]
    public async Task Missing_answer_returned_as_500_means_null()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Error calling API") };
        using var sp = t.Build();

        Assert.Null(await sp.GetRequiredService<ItslearningClient>().GetAnswerAsync(1, 2));
    }

    [Fact]
    public async Task Parses_answer_and_assignment()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/assignments/7/answers/persons/2/v1" => FakeItslearning.Json("""{"AnswerId":11,"IsDraft":false,"Submitted":"2026-03-02T14:53:22Z","Text":"","Files":[{"FileId":5,"Name":"a.zip","Size":10}]}"""),
            "/restapi/personal/assignments/7/v1" => FakeItslearning.Json("""{"ElementId":7,"Title":"PA1","Deadline":"2026-03-02T15:32:00Z","MaxScore":5.0,"SubmissionType":"Individual","Files":[{"FileId":9,"Name":"spec.pdf","Size":100}],"TemplateFile":null,"RawText":"<p>Do it</p>"}"""),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        using var sp = t.Build();
        var client = sp.GetRequiredService<ItslearningClient>();

        var answer = await client.GetAnswerAsync(7, 2);
        Assert.Equal(11, answer!.AnswerId);
        Assert.Equal("a.zip", Assert.Single(answer.Files).Name);

        var assignment = await client.GetAssignmentAsync(7);
        Assert.Equal(5m, assignment.MaxScore);
        Assert.Equal(9, Assert.Single(assignment.Files).FileId);
    }

    [Fact]
    public async Task Downloads_rest_file_with_name_from_content_disposition()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent([1, 2, 3]) };
            response.Content.Headers.ContentDisposition = new("attachment") { FileName = "\"spec.pdf\"" };
            return response;
        };
        using var sp = t.Build();
        var client = sp.GetRequiredService<ItslearningClient>();

        var file = await client.DownloadAsync(ItslearningClient.AssignmentFilePath(7, 9), "fallback", 100);
        Assert.Equal("spec.pdf", file.FileName);
        Assert.Equal([1, 2, 3], file.Content);
        Assert.Equal("/restapi/personal/assignments/7/files/9/v1", t.Http.Requests.Single().PathAndQuery);

        await Assert.ThrowsAsync<ContentUnavailableException>(() => client.DownloadAsync("x", "f", maxBytes: 2));
    }
}
