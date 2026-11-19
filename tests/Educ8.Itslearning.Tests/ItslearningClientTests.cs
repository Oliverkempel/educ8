using System.Net;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class ItslearningClientTests
{
    [Fact]
    public async Task Sends_bearer_token_and_parses_person()
    {
        using var t = new TestServices();
        t.SignedIn("access-1");
        t.Http.Respond = (_, _) => FakeItslearning.Json("""{"PersonId":5,"FirstName":"Ada","LastName":"Lovelace","Language":"en-GB","Unknown":1}""");
        using var sp = t.Build();

        var me = await sp.GetRequiredService<ItslearningClient>().GetCurrentPersonAsync();

        Assert.Equal("Ada Lovelace", me.DisplayName);
        var request = Assert.Single(t.Http.Requests);
        Assert.Equal("/restapi/personal/person/v1", request.PathAndQuery);
        Assert.Equal("Bearer access-1", request.Authorization);
    }

    [Fact]
    public async Task Pages_through_all_courses()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.Query.Contains("PageIndex=0")
            ? FakeItslearning.Json(CoursePage(0, 150, Enumerable.Range(1, 100)))
            : FakeItslearning.Json(CoursePage(1, 150, Enumerable.Range(101, 50)));
        using var sp = t.Build();

        var ids = new List<int>();
        await foreach (var c in sp.GetRequiredService<ItslearningClient>().GetAllCoursesAsync()) ids.Add(c.CourseId);

        Assert.Equal(150, ids.Count);
        Assert.Equal(2, t.Http.Requests.Count);
        Assert.All(t.Http.Requests, r => Assert.Contains("PageSize=100", r.PathAndQuery));
    }

    [Fact]
    public async Task Retries_once_with_refreshed_token_after_401()
    {
        using var t = new TestServices();
        t.SignedIn("stale", "refresh-1");
        t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/oauth2/token" => FakeItslearning.Json(FakeItslearning.TokenJson("fresh", "refresh-2")),
            _ when req.Headers.Authorization?.Parameter == "fresh" => FakeItslearning.Json("""{"PersonId":5,"FirstName":"A","LastName":"B"}"""),
            _ => new HttpResponseMessage(HttpStatusCode.Unauthorized),
        };
        using var sp = t.Build();

        var me = await sp.GetRequiredService<ItslearningClient>().GetCurrentPersonAsync();

        Assert.Equal(5, me.PersonId);
        Assert.Equal("refresh-2", t.Store.Tokens!.RefreshToken);
    }

    [Fact]
    public async Task Persistent_401_means_not_authenticated()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath == "/restapi/oauth2/token"
            ? FakeItslearning.Json(FakeItslearning.TokenJson("x", "y"))
            : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        using var sp = t.Build();

        await Assert.ThrowsAsync<NotAuthenticatedException>(() => sp.GetRequiredService<ItslearningClient>().GetCurrentPersonAsync());
    }

    [Fact]
    public async Task Server_error_is_reported_without_query_string()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Error calling API") };
        using var sp = t.Build();

        var e = await Assert.ThrowsAsync<ItslearningApiException>(() =>
            sp.GetRequiredService<ItslearningClient>().GetAsync<object>("restapi/personal/x/v1?searchText=secret"));
        Assert.Equal(HttpStatusCode.InternalServerError, e.StatusCode);
        Assert.Equal("restapi/personal/x/v1", e.Path);
        Assert.DoesNotContain("secret", e.Message);
    }

    public static string CoursePage(int pageIndex, int total, IEnumerable<int> ids) =>
        $$"""{"EntityArray":[{{string.Join(',', ids.Select(i => $$"""{"CourseId":{{i}},"Title":"Course {{i}}","CourseCode":"C{{i}}-1-E26"}"""))}}],"Total":{{total}},"CurrentPageIndex":{{pageIndex}},"PageSize":100}""";
}
