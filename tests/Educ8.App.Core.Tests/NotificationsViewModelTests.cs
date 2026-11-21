using System.Net;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.App.Core.Tests;

public class NotificationsViewModelTests
{
    private const string List = """
        {"EntityArray":[
          {"NotificationId":7,"Text":"Assessment is available for <b>Hand-in 3</b>","PublishedDate":"2026-11-18T09:00:00Z","PublishedBy":{"FullName":"Ann Teacher"},"Type":"Assessment","Url":"https://school.itslearning.com/ContentArea/ContentArea.aspx?LocationType=1&LocationID=10&ElementID=80&ElementType=131072&FromNotification=True","ContentUrl":"https://school.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=80","IsRead":false},
          {"NotificationId":6,"Text":"New announcement available in Statistics","PublishedDate":"2026-11-16T08:00:00Z","Type":"Unknown","Url":"/ContentArea/ContentArea.aspx?LocationType=1&LocationID=10&FromNotification=true","ContentUrl":"/Course/Course.aspx?CourseID=10","IsRead":true},
          {"NotificationId":5,"Text":"Bo submitted Report","PublishedDate":"2026-11-15T08:00:00Z","Type":"Unknown","IsRead":false}],"Total":3}
        """;

    /// <summary>Fake itslearning that keeps the unread ids and lowers the count when they're marked read.</summary>
    private static (AppTestHost Host, HashSet<int> Unread) Host(bool failPut = false)
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        var unread = new HashSet<int> { 7, 5, 3 }; // 3: unread but older than the list
        h.T.Http.Respond = (req, body) =>
        {
            switch (req.RequestUri!.AbsolutePath)
            {
                case "/restapi/personal/notifications/v2" when req.Method == HttpMethod.Get:
                    return FakeItslearning.Json(List);
                case "/restapi/personal/notifications/v2" when req.Method == HttpMethod.Put:
                    if (failPut) return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    foreach (var id in new[] { 7, 6, 5 }) if (body!.Contains($"\"NotificationId\":{id},")) unread.Remove(id);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                case "/restapi/personal/courses/v4":
                    return FakeItslearning.Json(AppTestHost.CoursesJson(AppTestHost.Course(10, "Statistics, (E26)", "T700075101-1-E26", "2026-11-18T08:00:00Z")));
                case "/restapi/personal/notifications/unread/count/v1":
                    return FakeItslearning.Json(unread.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
                default:
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        };
        return (h, unread);
    }

    [Fact]
    public async Task Lists_notifications_newest_first_with_unread_count()
    {
        var (h, _) = Host();
        using var _h = h;
        var vm = h.Provider.GetRequiredService<NotificationsViewModel>();
        await vm.EnsureLoadedAsync();

        Assert.Equal([7, 6, 5], vm.Items.Select(i => i.Id));
        var first = vm.Items[0];
        Assert.Equal(("Assessment is available for Hand-in 3", "Ann Teacher · Today, 09:00", false), (first.Text, first.MetaText, first.IsRead));
        Assert.Equal(Presentation.AppIcon.Assignment, first.Icon);
        Assert.Equal("https://school.itslearning.com/ContentArea/ContentArea.aspx?LocationType=1&LocationID=10&FromNotification=true", vm.Items[1].Url!.ToString()); // relative link resolved
        Assert.Equal((false, true, false), (vm.Items[0].OpensInApp, vm.Items[1].OpensInApp, vm.Items[2].OpensInApp)); // element → browser; course → app
        Assert.Null(vm.Items[2].Url);
        Assert.Equal((3, "3", true), (vm.UnreadCount, vm.UnreadText, vm.HasUnread));
        Assert.Null(vm.EmptyText);
    }

    [Fact]
    public async Task Opening_marks_read_on_itslearning_and_opens_the_page()
    {
        var (h, unread) = Host();
        using var _h = h;
        var vm = h.Provider.GetRequiredService<NotificationsViewModel>();
        await vm.EnsureLoadedAsync();

        await vm.OpenCommand.ExecuteAsync(vm.Items[0]);
        Assert.Equal("https://school.itslearning.com/ContentArea/ContentArea.aspx?LocationType=1&LocationID=10&ElementID=80&ElementType=131072&FromNotification=True",
            Assert.Single(h.OpenUrl.Opened).ToString());
        Assert.True(vm.Items[0].IsRead);
        Assert.Equal("""[{"NotificationId":7,"IsRead":true}]""", h.T.Http.Requests.Single(r => r.Method == HttpMethod.Put).Body);
        Assert.Equal(2, vm.UnreadCount);

        // An announcement opens its course page in the app, not the browser; it's already read, so nothing is written.
        CourseViewModel? course = null;
        vm.CourseOpened += (_, c) => course = c;
        await vm.OpenCommand.ExecuteAsync(vm.Items[1]);
        Assert.Equal((10, "Statistics, (E26)"), (course?.CourseId, course?.Title));
        Assert.Single(h.OpenUrl.Opened);
        Assert.Single(h.T.Http.Requests, r => r.Method == HttpMethod.Put);

        // Mark all: the remaining unread ones in the list; the count comes from the server (3 is older than the list).
        await vm.MarkAllReadCommand.ExecuteAsync(null);
        Assert.Equal("""[{"NotificationId":5,"IsRead":true}]""", h.T.Http.Requests.Last(r => r.Method == HttpMethod.Put).Body);
        Assert.All(vm.Items, i => Assert.True(i.IsRead));
        Assert.Equal([3], unread);
        Assert.Equal(1, vm.UnreadCount);
    }

    [Fact]
    public async Task Failed_mark_read_says_so_and_keeps_the_item_unread()
    {
        var (h, _) = Host(failPut: true);
        using var _h = h;
        var vm = h.Provider.GetRequiredService<NotificationsViewModel>();
        await vm.EnsureLoadedAsync();

        await vm.OpenCommand.ExecuteAsync(vm.Items[0]);
        Assert.Single(h.OpenUrl.Opened); // still opens
        Assert.False(vm.Items[0].IsRead);
        Assert.StartsWith("Couldn't mark it as read on itslearning", vm.ErrorMessage);
    }

    [Fact]
    public async Task Shell_lists_the_page_and_shows_the_unread_count_after_sign_in()
    {
        var (h, _) = Host();
        using var _h = h;
        h.T.Http.Respond = (req, body) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/person/v1" => FakeItslearning.Json("""{"PersonId":5,"FirstName":"Test","LastName":"Student"}"""),
            "/restapi/personal/courses/v4" => FakeItslearning.Json(AppTestHost.CoursesJson()),
            "/restapi/personal/notifications/unread/count/v1" => FakeItslearning.Json("4"),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        var shell = h.Shell;
        Assert.Contains(shell.NavigationItems, n => n.Page == AppPage.Notifications);
        await shell.InitializeAsync();
        await Task.Delay(50); // the count loads in the background
        Assert.Equal(4, shell.Notifications.UnreadCount);
    }
}
