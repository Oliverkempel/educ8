using System.Net;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

/// <summary>Instant messages. JSON shaped like the live responses (2026-11-21), synthetic content.</summary>
public class MessagesClientTests
{
    public static string Message(long id, int thread, int by, string name, string text, string created = "2026-11-18T10:00:00Z") =>
        $$"""{"MessageId":{{id}},"InstantMessageThreadId":{{thread}},"Created":"{{created}}","CreatedBy":{{by}},"CreatedByName":"{{name}}","Text":"{{text}}","AttachmentUrl":null,"AttachmentName":null,"IsSystemMessage":false,"IsDeleted":false}""";

    public static string Thread(int id, string name, string last, long lastRead, long sort, bool adminOnly = false, string type = "OneToOne") =>
        $$"""{"InstantMessageThreadId":{{id}},"Name":"{{name}}","Type":"{{type}}","LastMessage":{{last}},"Participants":[{"PersonId":5,"FullName":"Test Student"},{"PersonId":9,"FullName":"Ann Teacher"}],"ParticipantsCount":2,"LastReadInstantMessageId":{{lastRead}},"SortIndex":{{sort}},"OnlyThreadAdminCanSendToThread":{{(adminOnly ? "true" : "false")}},"IsThreadAdministrator":false,"HasActiveParticipants":true,"IsBlocked":false}""";

    [Fact]
    public async Task Threads_messages_and_unread_count()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/instantmessages/messagethreads/v4" => FakeItslearning.Json($$"""
                {"Threads":{"EntityArray":[{{Thread(1, "Ann Teacher", Message(12, 1, 9, "Ann Teacher", "See you"), 11, 500)}},{{Thread(2, "Course news", Message(20, 2, 9, "Ann Teacher", "Exam moved"), 20, 400, adminOnly: true, type: "Group")}}],"Total":2},"HasMore":true}
                """),
            "/restapi/personal/instantmessages/messagethreads/1/messages/v3" => FakeItslearning.Json($$"""
                {"Messages":{"EntityArray":[{{Message(12, 1, 9, "Ann Teacher", "See you")}},{{Message(11, 1, 5, "Test Student", "Thanks!")}}],"Total":2},"HasMore":false}
                """),
            "/restapi/personal/instantmessages/messagethreads/unread/count/v1" => FakeItslearning.Json("1"),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        using var sp = t.Build();
        var client = sp.GetRequiredService<ItslearningClient>();

        var page = await client.GetMessageThreadsAsync(fromSortIndex: 600, pageSize: 20);
        Assert.True(page.HasMore);
        Assert.Equal([(1, true, true), (2, false, false)], page.Threads.EntityArray.Select(th => (th.InstantMessageThreadId, th.IsUnread, th.CanReply)));
        Assert.Contains("pageSize=20&fromSortIndex=600", t.Http.Requests[^1].PathAndQuery);

        var messages = await client.GetThreadMessagesAsync(1, fromId: 13);
        Assert.Equal([12L, 11L], messages.Messages.EntityArray.Select(m => m.MessageId));
        Assert.Contains("fromId=13", t.Http.Requests[^1].PathAndQuery);
        Assert.Equal(1, await client.GetUnreadThreadCountAsync());
    }

    [Fact]
    public async Task Recipient_search_keeps_persons_who_can_receive()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => FakeItslearning.Json("""
            [{"TypeName":"Person","Id":9,"Label":"Ann Teacher","CanReceive":true},{"TypeName":"Person","Id":10,"Label":"Ann Old","CanReceive":false},
             {"TypeName":"Course","Id":50,"Label":"Statistics","CanReceive":true}]
            """);
        using var sp = t.Build();

        var found = await sp.GetRequiredService<ItslearningClient>().SearchMessageRecipientsAsync("ann b");
        Assert.Equal([9], found.Select(r => r.Id));
        Assert.EndsWith("searchText=ann%20b", t.Http.Requests[0].PathAndQuery);
    }

    [Fact]
    public async Task Mark_read_reply_and_new_conversation_requests()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.Method == HttpMethod.Post
            ? FakeItslearning.Json(Thread(3, "Bo", Message(30, 3, 5, "Test Student", "Hi"), 30, 900))
            : new HttpResponseMessage(HttpStatusCode.OK);
        using var sp = t.Build();
        var client = sp.GetRequiredService<ItslearningClient>();

        await client.MarkThreadReadAsync(1, 12);
        Assert.Equal((HttpMethod.Put, "/restapi/personal/instantmessages/messagethreads/1/updatelastread/12/v1"), (t.Http.Requests[0].Method, t.Http.Requests[0].PathAndQuery));

        await client.SendMessageAsync(1, null, "Thanks!");
        Assert.Equal("""{"InstantMessageThreadId":1,"Text":"Thanks!"}""", t.Http.Requests[1].Body);

        var thread = await client.SendMessageAsync(null, [9, 10], "Hi");
        Assert.Equal("""{"ToPersonIds":[9,10],"Text":"Hi"}""", t.Http.Requests[2].Body);
        Assert.Equal(3, thread.InstantMessageThreadId);

        await Assert.ThrowsAsync<ArgumentException>(() => client.SendMessageAsync(null, [], "Hi"));
    }

    [Fact]
    public async Task Site_files_download_with_the_login_but_other_hosts_never_get_it()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath == "/page"
            ? new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("<html></html>", System.Text.Encoding.UTF8, "text/html") }
            : new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent([1, 2, 3]) };
        using var sp = t.Build();
        var client = sp.GetRequiredService<ItslearningClient>();
        var dir = Path.Combine(t.DataDir, "attachments");

        var path = await client.TryDownloadSiteFileAsync(new Uri("https://school.itslearning.com/file/42"), dir, "notes.pdf");
        Assert.Equal([1, 2, 3], await File.ReadAllBytesAsync(path!));
        Assert.Equal("notes.pdf", Path.GetFileName(path));

        Assert.Null(await client.TryDownloadSiteFileAsync(new Uri("https://example.com/file"), dir, "x.pdf"));
        Assert.Null(await client.TryDownloadSiteFileAsync(new Uri("https://school.itslearning.com/page"), dir, "x.pdf")); // a web page
        Assert.DoesNotContain(t.Http.Requests, r => r.PathAndQuery == "/file" || r.Authorization is not null && r.PathAndQuery.Contains("example.com"));
        Assert.Equal(2, t.Http.Requests.Count); // example.com was never requested
    }
}
