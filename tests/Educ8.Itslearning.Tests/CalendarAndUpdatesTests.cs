using System.Net;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

/// <summary>Calendar and "what's new" client methods. JSON is shaped like the live responses (2026-11-19), with synthetic content.</summary>
public class CalendarAndUpdatesTests
{
    public static string Event(int id, string from, string to, string title, int course = 10) =>
        $$"""{"EventId":{{id}},"FromDate":"{{from}}","ToDate":"{{to}}","EventTitle":"{{title}}","Description":"","ImportDescription":"Room U1 Teacher","EventType":"Course","LocationId":{{course}},"LocationTitle":"Course {{course}}"}""";

    [Fact]
    public async Task Calendar_pages_until_events_pass_the_end_and_keeps_only_overlapping_ones()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.Query.Contains("PageIndex=0")
            ? FakeItslearning.Json($$"""{"EntityArray":[{{Event(1, "2026-11-17T10:00:00Z", "2026-11-17T12:00:00Z", "Before")}},{{Event(2, "2026-11-18T13:00:00Z", "2026-11-18T15:00:00Z", "Lecture")}}],"Total":250}""")
            : FakeItslearning.Json($$"""{"EntityArray":[{{Event(3, "2026-11-20T08:00:00Z", "2026-11-20T10:00:00Z", "Exercises")}},{{Event(4, "2026-12-30T08:00:00Z", "2026-12-30T10:00:00Z", "Later")}}],"Total":250}""");
        using var sp = t.Build();

        var events = await sp.GetRequiredService<ItslearningClient>().GetCalendarEventsAsync(
            new DateTimeOffset(2026, 11, 18, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 11, 25, 0, 0, 0, TimeSpan.Zero));

        Assert.Equal([2, 3], events.Select(e => e.EventId));
        Assert.Equal(2, t.Http.Requests.Count); // stopped after the page that passed the end
        Assert.Contains("fromDate=2026-11-17", t.Http.Requests[0].PathAndQuery); // a day early, for events already running
    }

    [Fact]
    public async Task Course_calendar_maps_its_different_field_names()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => FakeItslearning.Json("""
            {"EntityArray":[{"Id":1284319,"FromDate":"2026-11-19T11:00:00Z","ToDate":"2026-11-19T13:00:00Z","EventTitle":"Lecture - Campus U150","Description":"","ImportDescription":"Campus U150 Teacher","CourseId":50352,"CourseGroupId":0,"LocationTitle":"OSDS"}],"Total":1}
            """);
        using var sp = t.Build();

        var e = Assert.Single(await sp.GetRequiredService<ItslearningClient>().GetCourseCalendarEventsAsync(50352,
            new DateTimeOffset(2026, 11, 19, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 11, 26, 0, 0, 0, TimeSpan.Zero)));
        Assert.Equal(1284319, e.EventId);
        Assert.Equal(50352, e.LocationId);
        Assert.Equal("Course", e.EventType);
    }

    [Fact]
    public async Task Stream_stops_at_items_older_than_since()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => FakeItslearning.Json("""
            {"EntityArray":[
              {"NotificationId":1,"Text":"Teacher added Intro","LocationTitle":"Web","LocationId":2,"PublishedDate":"2026-11-18T10:00:00Z","ElementId":55,"ElementType":"LearningToolElement","LearningToolId":5,"ElementsCount":1},
              {"NotificationId":2,"Text":"Teacher added A and 5 other elements","LocationTitle":"Web","LocationId":2,"PublishedDate":"2026-11-17T10:00:00Z","ElementId":0,"ElementType":"Unknown","ElementsCount":6,"HasMoreElements":true},
              {"NotificationId":3,"Text":"Old","PublishedDate":"2026-10-01T10:00:00Z","ElementId":0}
            ],"Total":300}
            """);
        using var sp = t.Build();

        var items = await sp.GetRequiredService<ItslearningClient>().GetStreamAsync(new DateTimeOffset(2026, 11, 11, 0, 0, 0, TimeSpan.Zero), 40);

        Assert.Equal([1, 2], items.Select(i => i.NotificationId));
        Assert.Equal(ResourceKind.Page, items[0].Kind);
        Assert.True(items[1].IsGroup);
        Assert.Single(t.Http.Requests); // the first page already reached older items
    }

    [Theory]
    [InlineData("https://school.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=2363975", 2363975)]
    [InlineData("https://school.itslearning.com/Course/Course.aspx?CourseID=50352", null)]
    [InlineData(null, null)]
    public void Notification_element_id_from_content_url(string? url, int? expected) =>
        Assert.Equal(expected, new Notification { ContentUrl = url }.ElementId);

    [Fact]
    public async Task Event_details_include_plan_links()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => FakeItslearning.Json("""
            {"EventUrl":"https://school.itslearning.com/Course/Course.aspx?CourseID=50103&EventId=7","HasMoreContent":true,"Bookings":[],"ElementLinks":[],
             "PlanLinks":[{"PlanId":420060,"PlanName":"Lecture 10","TopicName":null,"Url":"https://school.itslearning.com/plans/courses/50103/plan/420060"}],
             "EventId":7,"FromDate":"2026-11-17T13:00:00Z","ToDate":"2026-11-17T15:00:00Z","EventTitle":"Lecture - Campus U55","EventType":"Course","LocationId":50103}
            """);
        using var sp = t.Build();

        var e = await sp.GetRequiredService<ItslearningClient>().GetCalendarEventAsync(7);
        Assert.Equal(420060, Assert.Single(e.PlanLinks).PlanId);
        Assert.True(e.HasMoreContent);
    }
}
