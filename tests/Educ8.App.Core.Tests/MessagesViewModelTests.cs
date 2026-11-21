using System.Net;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using static Educ8.Itslearning.Tests.MessagesClientTests;

namespace Educ8.App.Core.Tests;

public class MessagesViewModelTests
{
    /// <summary>A fake itslearning messenger: thread 1 (unread, with Ann), thread 2 (teacher broadcast, read), thread 3 created by sending.</summary>
    private sealed class Messenger
    {
        public long LastRead1 = 11;
        public bool FailSend;
        public string? AttachmentUrl;
        public List<string> Sent { get; } = [];

        public HttpResponseMessage Respond(HttpRequestMessage req, string? body)
        {
            var path = req.RequestUri!.AbsolutePath;
            var query = req.RequestUri.Query;
            if (req.Method == HttpMethod.Put && path == "/restapi/personal/instantmessages/messagethreads/1/updatelastread/12/v1")
            {
                LastRead1 = 12;
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            if (req.Method == HttpMethod.Post && path == "/restapi/personal/instantmessages/v2")
            {
                if (FailSend) return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                Sent.Add(body!);
                return FakeItslearning.Json(Thread(3, "", Message(30, 3, 5, "Test Student", "Hi Bo"), 30, 900));
            }
            var attachment = AttachmentUrl is null ? "null" : $"\"{AttachmentUrl}\"";
            return path switch
            {
                "/restapi/personal/person/v1" => FakeItslearning.Json("""{"PersonId":5,"FirstName":"Test","LastName":"Student"}"""),
                "/restapi/personal/instantmessages/messagethreads/v4" => FakeItslearning.Json($$"""
                    {"Threads":{"EntityArray":[{{Thread(1, "", Message(12, 1, 9, "Ann Teacher", "See you\\nTomorrow"), LastRead1, 500)}},
                      {{Thread(2, "Course news", Message(20, 2, 5, "Test Student", "Noted"), 20, 400, adminOnly: true, type: "Group")}}],"Total":2},"HasMore":false}
                    """),
                "/restapi/personal/instantmessages/messagethreads/1/messages/v3" when query.Contains("fromId=11") => FakeItslearning.Json($$"""
                    {"Messages":{"EntityArray":[{{Message(10, 1, 9, "Ann Teacher", "Hello")}}],"Total":1},"HasMore":false}
                    """),
                "/restapi/personal/instantmessages/messagethreads/1/messages/v3" => FakeItslearning.Json($$"""
                    {"Messages":{"EntityArray":[{{Message(12, 1, 9, "Ann Teacher", "See you\\nTomorrow").Replace("\"AttachmentUrl\":null", $"\"AttachmentUrl\":{attachment}").Replace("\"AttachmentName\":null", "\"AttachmentName\":\"notes.pdf\"")}},{{Message(11, 1, 5, "Test Student", "Thanks!")}}],"Total":3},"HasMore":true}
                    """),
                "/restapi/personal/instantmessages/messagethreads/2/messages/v3" => FakeItslearning.Json($$"""
                    {"Messages":{"EntityArray":[{{Message(20, 2, 9, "Ann Teacher", "Exam moved")}}],"Total":1},"HasMore":false}
                    """),
                "/restapi/personal/instantmessages/messagethreads/3/messages/v3" => FakeItslearning.Json($$"""
                    {"Messages":{"EntityArray":[{{Message(30, 3, 5, "Test Student", "Hi Bo")}}],"Total":1},"HasMore":false}
                    """),
                "/restapi/personal/instantmessages/messagethreads/unread/count/v1" => FakeItslearning.Json(LastRead1 < 12 ? "1" : "0"),
                "/restapi/personal/instantmessages/recipients/search/v1" => FakeItslearning.Json("""
                    [{"TypeName":"Person","Id":7,"Label":"Bo Student","Organization":"University","CanReceive":true},{"TypeName":"Course","Id":50,"Label":"Statistics","CanReceive":true}]
                    """),
                "/file/42" => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent([1, 2, 3]) },
                _ => new HttpResponseMessage(HttpStatusCode.NotFound),
            };
        }
    }

    private static (AppTestHost Host, Messenger Fake, MessagesViewModel Vm) Host()
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        var fake = new Messenger();
        h.T.Http.Respond = fake.Respond;
        return (h, fake, h.Provider.GetRequiredService<MessagesViewModel>());
    }

    private static async Task<ConversationViewModel> OpenAsync(MessagesViewModel vm, int index)
    {
        ConversationViewModel? opened = null;
        void Handler(object? s, ConversationViewModel c) => opened = c;
        vm.ConversationOpened += Handler;
        vm.OpenCommand.Execute(vm.Threads[index]);
        vm.ConversationOpened -= Handler;
        for (var i = 0; i < 100 && opened!.IsLoading; i++) await Task.Delay(10);
        return opened!;
    }

    [Fact]
    public async Task Lists_conversations_with_previews_and_unread_badge()
    {
        var (h, _, vm) = Host();
        using var _h = h;
        await vm.RefreshCommand.ExecuteAsync(null);

        Assert.Equal(["Ann Teacher", "Course news"], vm.Threads.Select(t => t.Title)); // unnamed one-to-one: the other person
        Assert.Equal(("See you", "You: Noted"), (vm.Threads[0].PreviewText, vm.Threads[1].PreviewText)); // no "Ann Teacher:" under "Ann Teacher"
        Assert.Equal((true, false), (vm.Threads[0].IsUnread, vm.Threads[1].IsUnread));
        Assert.Equal((true, false), (vm.Threads[0].CanReply, vm.Threads[1].CanReply));
        Assert.Equal((1, "1"), (vm.UnreadCount, vm.UnreadText));
    }

    [Fact]
    public async Task Opening_shows_oldest_first_marks_read_and_loads_older()
    {
        var (h, fake, vm) = Host();
        using var _h = h;
        await vm.RefreshCommand.ExecuteAsync(null);

        var c = await OpenAsync(vm, 0);
        Assert.Equal(["Thanks!", "See you\nTomorrow"], c.Messages.Select(m => m.Text));
        Assert.Equal((true, "You"), (c.Messages[0].IsMine, c.Messages[0].SenderName));
        Assert.Equal(("Ann Teacher", ""), (c.Messages[1].SenderName, c.ParticipantsText)); // the title already names her
        Assert.Equal(12, fake.LastRead1); // marked read up to the newest message
        Assert.False(vm.Threads[0].IsUnread);
        Assert.Equal(0, vm.UnreadCount);

        Assert.True(c.LoadOlderCommand.CanExecute(null));
        await c.LoadOlderCommand.ExecuteAsync(null);
        Assert.Equal(["Hello", "Thanks!", "See you\nTomorrow"], c.Messages.Select(m => m.Text));
        Assert.False(c.HasOlder);

        // Already read: opening again writes nothing.
        var puts = h.T.Http.Requests.Count(r => r.Method == HttpMethod.Put);
        await OpenAsync(vm, 0);
        Assert.Equal(puts, h.T.Http.Requests.Count(r => r.Method == HttpMethod.Put));
    }

    [Fact]
    public async Task Replying_sends_to_the_thread_and_broadcasts_take_no_replies()
    {
        var (h, fake, vm) = Host();
        using var _h = h;
        await vm.RefreshCommand.ExecuteAsync(null);
        var c = await OpenAsync(vm, 0);

        Assert.False(c.SendCommand.CanExecute(null)); // empty draft
        c.Draft = "  On my way  ";
        await c.SendCommand.ExecuteAsync(null);
        Assert.Equal("""{"InstantMessageThreadId":1,"Text":"On my way"}""", Assert.Single(fake.Sent));
        Assert.Equal("", c.Draft);

        fake.FailSend = true;
        c.Draft = "Again";
        await c.SendCommand.ExecuteAsync(null);
        Assert.Equal("Again", c.Draft); // kept for another try
        Assert.StartsWith("The message wasn't sent", c.ErrorMessage);

        var broadcast = await OpenAsync(vm, 1);
        broadcast.Draft = "Hi";
        Assert.False(broadcast.SendCommand.CanExecute(null));
        Assert.Equal("Replies are turned off in this conversation.", broadcast.ReplyDisabledText);
    }

    [Fact]
    public async Task New_conversation_searches_people_and_opens_the_thread()
    {
        var (h, fake, vm) = Host();
        using var _h = h;
        await vm.RefreshCommand.ExecuteAsync(null);
        NewConversationViewModel? form = null;
        vm.NewConversationStarted += (_, f) => form = f;
        vm.NewConversationCommand.Execute(null);

        form!.SearchText = "b";
        await form.SearchCommand.ExecuteAsync(null);
        Assert.Empty(form.Results); // too short to search
        form.SearchText = "bo";
        await form.SearchCommand.ExecuteAsync(null);
        var bo = Assert.Single(form.Results); // persons who can receive only
        Assert.Equal(("Bo Student", "University"), (bo.Name, bo.Detail));

        form.AddCommand.Execute(bo);
        Assert.Equal([7], form.Recipients.Select(r => r.Id));
        Assert.Equal("", form.SearchText);
        Assert.False(form.SendCommand.CanExecute(null)); // no text yet

        var sent = false;
        form.Sent += (_, _) => sent = true;
        ConversationViewModel? opened = null;
        vm.ConversationOpened += (_, c) => opened = c;
        form.Draft = "Hi Bo";
        await form.SendCommand.ExecuteAsync(null);

        Assert.Equal("""{"ToPersonIds":[7],"Text":"Hi Bo"}""", Assert.Single(fake.Sent));
        Assert.True(sent);
        Assert.Equal(3, opened?.ThreadId);
    }

    [Fact]
    public async Task Attachments_on_the_site_open_in_the_default_app_others_in_the_browser()
    {
        var (h, fake, vm) = Host();
        using var _h = h;
        fake.AttachmentUrl = "https://school.itslearning.com/file/42";
        await vm.RefreshCommand.ExecuteAsync(null);
        var c = await OpenAsync(vm, 0);

        var message = c.Messages[1];
        Assert.Equal("notes.pdf", message.AttachmentName);
        await message.OpenAttachmentCommand.ExecuteAsync(null);
        Assert.Equal("notes.pdf", Path.GetFileName(Assert.Single(h.OpenFile.Opened)));

        fake.AttachmentUrl = "https://files.example.com/notes.pdf";
        var other = await OpenAsync(vm, 0);
        await other.Messages[1].OpenAttachmentCommand.ExecuteAsync(null);
        Assert.Equal("https://files.example.com/notes.pdf", Assert.Single(h.OpenUrl.Opened).ToString());
        Assert.DoesNotContain(h.T.Http.Requests, r => r.PathAndQuery.Contains("example.com"));
    }
}
