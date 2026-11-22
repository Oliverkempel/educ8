using System.Net;
using Educ8.App.Core.Elements;
using Educ8.Documents;
using Educ8.Itslearning.Tests;

namespace Educ8.App.Core.Tests;

/// <summary>
/// The assignment module: read view and hand-in, on a fake itslearning (REST shaped like live responses + the scrubbed assignment
/// pages). Clock: Wed 18 Nov 2026 12:00 UTC. Nothing here reaches a real itslearning.
/// </summary>
public class AssignmentTests
{
    private const string Site = "https://school.itslearning.com";
    private const int Graded = 3001, Open = 3002, Me = 77;

    private sealed class Fake
    {
        public bool WebBroken;
        public bool AnswerExists;
        public HttpStatusCode? SubmitStatus;
        public bool SubmitTimesOut;
        public bool AnswerAppearsAfterTimeout;
        public string OpenDetails = """{"ElementId":3002,"Title":"Task 3 - Reflection","Deadline":"2026-11-25T22:59:00Z","IsSubmissionAfterDeadlineAllowed":true,"SubmissionType":"Individual","Files":[],"RawText":"<ul><li>Which vulnerabilities?</li></ul>"}""";
        public int LastElement;
        public int Submits;

        public HttpResponseMessage Api(HttpRequestMessage req, string? body)
        {
            var p = req.RequestUri!.AbsolutePath;
            if (req.Method == HttpMethod.Post && p == "/restapi/personal/fileupload/temp/v1")
                return FakeItslearning.Json("""{"FileRepoId":"repo-1","SignedFileRepoId":"sig","Name":"answer.txt","MimeType":"text/plain","SourceType":1,"IsTemplate":false}""");
            if (req.Method == HttpMethod.Post && p == "/restapi/personal/assignments/3002/answers/v1")
            {
                Submits++;
                if (SubmitTimesOut)
                {
                    AnswerExists = AnswerAppearsAfterTimeout;
                    throw new TaskCanceledException("timeout");
                }
                if (SubmitStatus is { } status) return new HttpResponseMessage(status);
                AnswerExists = true;
                return FakeItslearning.Json("""{"AnswerId":999}""");
            }
            return p switch
            {
                "/restapi/personal/person/v1" => FakeItslearning.Json($$"""{"PersonId":{{Me}},"FirstName":"Test","LastName":"Student"}"""),
                "/restapi/personal/sso/url/v1" => Sso(req),
                "/restapi/personal/courses/resources/3001/v1" => FakeItslearning.Json(Element(Graded, "Point-Giving Activity 1")),
                "/restapi/personal/courses/resources/3002/v1" => FakeItslearning.Json(Element(Open, "Task 3 - Reflection")),
                "/restapi/personal/assignments/3001/v1" => FakeItslearning.Json("""{"ElementId":3001,"Title":"Point-Giving Activity 1","Deadline":"2026-03-02T15:32:00Z","IsSubmissionAfterDeadlineAllowed":false,"MaxScore":5.0,"SubmissionType":"Individual","IsMandatory":true,"IsAssessmentVisibleForStudents":true,"Files":[{"FileId":901,"Name":"instructions.docx","Size":2048}],"TemplateFile":null,"RawText":"<p>Zip <b>Task1</b> and Task2.</p>"}"""),
                "/restapi/personal/assignments/3002/v1" => FakeItslearning.Json(OpenDetails),
                "/restapi/personal/assignments/3001/settings/v1" => FakeItslearning.Json("""{"StatusScale":null,"AssessmentScale":{"Title":"Pass/fail","AssessmentItems":[{"Title":"Passed","PercentFromAndIncl":50,"PercentTo":100}]}}"""),
                "/restapi/personal/assignments/3002/settings/v1" => FakeItslearning.Json("""{"StatusScale":null,"AssessmentScale":null}"""),
                "/restapi/personal/assignments/3001/answers/persons/77/v1" => FakeItslearning.Json("""{"AnswerId":555,"IsDraft":false,"Submitted":"2026-03-02T14:53:22Z","Text":"<p>My answer</p>","Files":[{"FileId":902,"Name":"student.zip","Size":4096}]}"""),
                "/restapi/personal/assignments/3002/answers/persons/77/v1" => AnswerExists
                    ? FakeItslearning.Json("""{"AnswerId":999,"IsDraft":false,"Submitted":"2026-11-18T12:00:00Z","Text":"<p>Hi</p>","Files":[]}""")
                    : new HttpResponseMessage(HttpStatusCode.InternalServerError),
                "/restapi/personal/assignments/3001/answers/555/assessments/files/v1" => FakeItslearning.Json("""[{"FileId":903,"Name":"feedback.txt","Size":20}]"""),
                "/restapi/personal/assignments/3001/files/901/v1" => FileResponse("instructions.docx", [1, 2, 3]),
                "/restapi/personal/assignments/3001/answers/555/assessments/files/903/v1" => FileResponse("feedback.txt", "Nice work"u8.ToArray()),
                _ => new HttpResponseMessage(HttpStatusCode.NotFound),
            };
        }

        public HttpResponseMessage Web(HttpRequestMessage req, string? body)
        {
            var u = req.RequestUri!;
            if (WebBroken) return new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
            if (u.AbsolutePath == "/sso-login" || u.AbsolutePath.Contains("ViewLearningToolElement"))
                return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), $"{Site}/LearningToolElement/ViewLearningToolElement.aspx");
            if (u.Host == "platform.itslearning.com")
                return Fixtures.Html(Fixtures.Read(LastElement == Graded ? "assignment_graded.html" : "assignment_unsubmitted.html"), "https://assignment.itslearning.com/view.aspx");
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage Sso(HttpRequestMessage req)
        {
            LastElement = int.Parse(Uri.UnescapeDataString(req.RequestUri!.Query).Split("LearningToolElementId=")[1].Split('&')[0]);
            return FakeItslearning.Json($$"""{"Url":"{{Site}}/sso-login"}""");
        }

        private static string Element(int id, string title) =>
            $$"""{"CourseTitle":"Data Management, (F26)","ElementId":{{id}},"ElementType":"LearningToolElement","LearningToolId":5006,"Title":"{{title}}","CourseId":46355,"Url":"{{Site}}/main.aspx?ElementID={{id}}","Active":true}""";

        private static HttpResponseMessage FileResponse(string name, byte[] bytes)
        {
            var r = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
            r.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = name };
            return r;
        }
    }

    private static (AppTestHost Host, Fake Fake) Host()
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        var fake = new Fake();
        h.T.Http.Respond = fake.Api;
        h.T.Web.Respond = fake.Web;
        return (h, fake);
    }

    private static async Task<AssignmentViewModel> OpenAsync(AppTestHost h, int id)
    {
        var row = h.Elements.Item(new ElementRef(id, "Assignment", "LearningToolElement", 5006, 46355, new Uri($"{Site}/main.aspx?ElementID={id}")));
        Assert.True(row.OpensInApp);
        await row.OpenCommand.ExecuteAsync(null);
        var page = Assert.IsType<AssignmentViewModel>(h.Pages.Last());
        await page.EnsureLoadedAsync();
        Assert.Null(page.ErrorMessage);
        return page;
    }

    [Fact]
    public async Task A_graded_assignment_shows_facts_answer_feedback_and_files()
    {
        var (h, _) = Host();
        using var _h = h;
        var a = await OpenAsync(h, Graded);

        Assert.Equal(ElementPageState.Content, a.State);
        Assert.Equal("Data Management, (F26)", a.CourseTitle);
        Assert.Equal("Due Mon 2 Mar, 15:32 · passed", a.DeadlineText);
        Assert.Equal([("Hand-in", "Individual"), ("Max score", "5"), ("Mandatory", "Yes")], a.Facts.Select(f => (f.Label, f.Value)));
        Assert.Equal("Task1", Assert.IsType<RichParagraph>(a.Description.Blocks.Single()).Spans[1].Text);
        Assert.Equal(("instructions.docx", "2 KB"), (a.TeacherFiles.Single().Name, a.TeacherFiles.Single().DetailText));
        Assert.Equal("Handed in Mon 2 Mar, 14:53", a.SubmissionText);
        Assert.Equal("student.zip", a.AnswerFiles.Single().Name);
        Assert.NotNull(a.StatusText);
        Assert.Null(a.ViewUnavailableText);
        Assert.Equal("feedback.txt", a.FeedbackFiles.Single().Name);
        Assert.Equal(["Passed (50–100 %)"], a.GradingScale);
        Assert.False(a.CanHandIn); // deadline passed, and already handed in
        Assert.NotNull(a.HandInDisabledText);

        await a.TeacherFiles[0].OpenCommand.ExecuteAsync(null);
        await a.FeedbackFiles[0].OpenCommand.ExecuteAsync(null);
        Assert.Equal(["instructions.docx", "feedback.txt"], h.OpenFile.Opened.Select(Path.GetFileName));
        Assert.Equal([1, 2, 3], await File.ReadAllBytesAsync(h.OpenFile.Opened[0]));

        var downloads = Path.Combine(h.CacheDirectory, "Downloads");
        Directory.CreateDirectory(downloads);
        h.SaveLocation.Folder = downloads;
        await a.TeacherFiles[0].SaveCopyCommand.ExecuteAsync(null);
        Assert.Equal([1, 2, 3], await File.ReadAllBytesAsync(Path.Combine(downloads, "instructions.docx")));
        Assert.Equal("Saved to Downloads", a.TeacherFiles[0].NoticeText);
    }

    [Fact]
    public async Task An_open_assignment_can_be_handed_in_after_review_and_confirm()
    {
        var (h, fake) = Host();
        using var _h = h;
        var a = await OpenAsync(h, Open);
        Assert.Equal("Due Wed 25 Nov, 22:59 · in 7 days", a.DeadlineText);
        Assert.Equal("Not handed in yet.", a.SubmissionText);
        Assert.Null(a.HandInDisabledText);
        Assert.True(a.StartHandInCommand.CanExecute(null));

        HandInViewModel? form = null;
        a.HandInStarted += (_, f) => form = f;
        var handedIn = false;
        a.HandedIn += (_, _) => handedIn = true;
        a.StartHandInCommand.Execute(null);
        Assert.NotNull(form);
        Assert.False(form.ReviewCommand.CanExecute(null)); // nothing to hand in yet

        var file = Path.Combine(h.CacheDirectory, "answer.txt");
        Directory.CreateDirectory(h.CacheDirectory);
        await File.WriteAllTextAsync(file, "my solution");
        h.PickFiles.Next = [file, file];
        await form.AddFilesCommand.ExecuteAsync(null);
        Assert.Equal("answer.txt", form.Files.Single().Name); // duplicates ignored
        form.Draft = "Hello\nworld";

        HandInConfirmation? confirmation = null;
        form.ConfirmationRequested += (_, c) => confirmation = c;
        form.ReviewCommand.Execute(null);
        Assert.NotNull(confirmation);
        Assert.Equal("Hand in “Task 3 - Reflection”?", confirmation.Title);
        Assert.Contains("Answer text: 2 words", confirmation.Body);
        Assert.Contains("File: answer.txt (11 B)", confirmation.Body);
        Assert.Contains("Teachers will see this", confirmation.Body);
        Assert.Equal(0, fake.Submits); // reviewing sends nothing

        var submitted = false;
        form.Submitted += (_, _) => submitted = true;
        await form.SubmitCommand.ExecuteAsync(confirmation);

        Assert.True(submitted);
        Assert.Null(form.ErrorMessage);
        Assert.Equal(1, fake.Submits);
        var submit = h.T.Http.Requests.Single(r => r.Method == HttpMethod.Post && r.PathAndQuery.EndsWith("/answers/v1"));
        using var json = System.Text.Json.JsonDocument.Parse(submit.Body!);
        Assert.Equal("<p>Hello<br />world</p>", json.RootElement.GetProperty("Text").GetString());
        Assert.Contains("\"FileRepoId\":\"repo-1\"", submit.Body);
        for (var i = 0; i < 100 && a.IsLoading; i++) await Task.Delay(10);
        Assert.True(handedIn);
        Assert.Equal("You've already handed in. To hand in again, use itslearning.", a.HandInDisabledText); // reloaded
    }

    [Fact]
    public async Task Nothing_is_sent_without_a_current_review()
    {
        var (h, fake) = Host();
        using var _h = h;
        var a = await OpenAsync(h, Open);
        HandInViewModel? form = null;
        a.HandInStarted += (_, f) => form = f;
        a.StartHandInCommand.Execute(null);
        form!.Draft = "First";
        HandInConfirmation? confirmation = null;
        form.ConfirmationRequested += (_, c) => confirmation = c;
        form.ReviewCommand.Execute(null);

        form.Draft = "Changed after the review";
        await form.SubmitCommand.ExecuteAsync(confirmation);

        Assert.Equal(0, fake.Submits);
        Assert.Equal("Your answer changed after the review. Review it again.", form.ErrorMessage);
    }

    [Fact]
    public async Task The_check_right_before_sending_stops_when_itslearning_changed()
    {
        var (h, fake) = Host();
        using var _h = h;
        var form = await ReviewedFormAsync(h);
        fake.AnswerExists = true; // handed in elsewhere meanwhile

        await form.Form.SubmitCommand.ExecuteAsync(form.Confirmation);

        Assert.Equal(0, fake.Submits);
        Assert.StartsWith("Something changed on itslearning: You've already handed in.", form.Form.ErrorMessage);
    }

    [Fact]
    public async Task A_refused_hand_in_keeps_the_draft_and_says_nothing_was_sent()
    {
        var (h, fake) = Host();
        using var _h = h;
        var form = await ReviewedFormAsync(h);
        fake.SubmitStatus = HttpStatusCode.BadRequest;

        var submitted = false;
        form.Form.Submitted += (_, _) => submitted = true;
        await form.Form.SubmitCommand.ExecuteAsync(form.Confirmation);

        Assert.False(submitted);
        Assert.Equal("itslearning refused the hand-in (400). Nothing was handed in.", form.Form.ErrorMessage);
        Assert.Equal("My answer", form.Form.Draft);
    }

    [Theory]
    [InlineData(true, null)]
    [InlineData(false, "It's unclear whether it was handed in")]
    public async Task A_timeout_on_submit_looks_whether_it_arrived(bool arrived, string? error)
    {
        var (h, fake) = Host();
        using var _h = h;
        var form = await ReviewedFormAsync(h);
        fake.SubmitTimesOut = true;
        fake.AnswerAppearsAfterTimeout = arrived;

        var submitted = false;
        form.Form.Submitted += (_, _) => submitted = true;
        await form.Form.SubmitCommand.ExecuteAsync(form.Confirmation);

        Assert.Equal(arrived, submitted);
        if (error is null) Assert.Null(form.Form.ErrorMessage);
        else Assert.StartsWith(error, form.Form.ErrorMessage);
    }

    [Theory]
    [InlineData("web", "Your hand-in status couldn't be checked")]
    [InlineData("group", "Group assignments can't be handed in from the app yet")]
    [InlineData("closed", "The deadline has passed and late hand-ins aren't allowed.")]
    [InlineData("draft", "You have a draft for this assignment in itslearning")]
    public async Task Hand_in_is_off_when_it_isnt_safe(string situation, string reason)
    {
        var (h, fake) = Host();
        using var _h = h;
        switch (situation)
        {
            case "web": fake.WebBroken = true; break;
            case "group": fake.OpenDetails = fake.OpenDetails.Replace("\"Individual\"", "\"Group\""); break;
            case "closed": fake.OpenDetails = fake.OpenDetails.Replace("2026-11-25T22:59:00Z", "2026-11-01T22:59:00Z").Replace("\"IsSubmissionAfterDeadlineAllowed\":true", "\"IsSubmissionAfterDeadlineAllowed\":false"); break;
            case "draft":
                h.T.Http.Respond = (req, body) => req.RequestUri!.AbsolutePath == "/restapi/personal/assignments/3002/answers/persons/77/v1"
                    ? FakeItslearning.Json("""{"AnswerId":5,"IsDraft":true,"Submitted":null,"Files":[]}""")
                    : fake.Api(req, body);
                break;
        }

        var a = await OpenAsync(h, Open);

        Assert.False(a.CanHandIn);
        Assert.False(a.StartHandInCommand.CanExecute(null));
        Assert.StartsWith(reason, a.HandInDisabledText);
        if (situation == "web") Assert.Equal("Your status, score and feedback couldn't be loaded right now.", a.ViewUnavailableText);
    }

    [Fact]
    public async Task Files_over_the_limits_are_refused_when_added()
    {
        var (h, _) = Host();
        using var _h = h;
        var form = (await ReviewedFormAsync(h)).Form;
        form.AddFiles([Path.Combine(h.CacheDirectory, "missing.pdf")]);
        Assert.Equal("“missing.pdf” can't be read.", form.ErrorMessage);
    }

    [Fact]
    public async Task An_assessment_notification_opens_the_assignment_in_the_app()
    {
        var (h, _) = Host();
        using var _h = h;
        var routes = h.T.Http.Respond;
        h.T.Http.Respond = (req, body) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/notifications/v2" => FakeItslearning.Json($$"""
                {"EntityArray":[{"NotificationId":7,"Text":"Assessment is available for Point-Giving Activity 1","PublishedDate":"2026-11-18T09:00:00Z","Type":"Assessment","Url":"{{Site}}/ContentArea/ContentArea.aspx?LocationType=1&LocationID=46355&ElementID=3001&ElementType=131072","ContentUrl":"{{Site}}/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=3001","IsRead":true}],"Total":1}
                """),
            "/restapi/personal/notifications/unread/count/v1" => FakeItslearning.Json("0"),
            _ => routes(req, body),
        };
        var notifications = h.Shell.Notifications;
        await notifications.RefreshCommand.ExecuteAsync(null);

        await notifications.OpenCommand.ExecuteAsync(notifications.Items.Single());

        Assert.Equal(Graded, Assert.IsType<AssignmentViewModel>(h.Pages.Single()).Element.ElementId);
        Assert.Empty(h.OpenUrl.Opened);
    }

    private static async Task<(HandInViewModel Form, HandInConfirmation Confirmation)> ReviewedFormAsync(AppTestHost h)
    {
        var a = await OpenAsync(h, Open);
        HandInViewModel? form = null;
        a.HandInStarted += (_, f) => form = f;
        a.StartHandInCommand.Execute(null);
        form!.Draft = "My answer";
        HandInConfirmation? confirmation = null;
        form.ConfirmationRequested += (_, c) => confirmation = c;
        form.ReviewCommand.Execute(null);
        return (form, confirmation!);
    }
}
