using System.Globalization;
using Educ8.App.Core.Services;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.App.Core.Tests;

/// <summary>
/// The real app view models on a fake itslearning. Clock: Wed 2026-11-18 12:00 UTC, time zone UTC, culture en-GB
/// (weeks start on Monday). Runs UI-thread posts immediately.
/// </summary>
public sealed class AppTestHost : IDisposable
{
    public TestServices T { get; } = new();
    public RecordingOpenUrl OpenUrl { get; } = new();
    public ServiceProvider Provider { get; }

    public AppTestHost()
    {
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("en-GB");
        T.Services.AddSingleton<IUiDispatcher>(new ImmediateDispatcher());
        T.Services.AddSingleton<IOpenUrl>(OpenUrl);
        T.Services.AddEduc8AppCore();
        Provider = T.Build();
    }

    public ShellViewModel Shell => Provider.GetRequiredService<ShellViewModel>();
    public CoursesViewModel Courses => Provider.GetRequiredService<CoursesViewModel>();
    public CalendarViewModel Calendar => Provider.GetRequiredService<CalendarViewModel>();
    public LoginViewModel Login => Provider.GetRequiredService<LoginViewModel>();

    public void Dispose()
    {
        Provider.Dispose();
        T.Dispose();
    }

    public static string Course(int id, string title, string? code, string? updated, int notifications = 0, int bulletins = 0, string? teachers = null) =>
        $$"""{"CourseId":{{id}},"Title":"{{title}}","CourseCode":{{(code is null ? "null" : $"\"{code}\"")}},"LastUpdatedUtc":{{(updated is null ? "null" : $"\"{updated}\"")}},"NewNotificationsCount":{{notifications}},"NewBulletinsCount":{{bulletins}},"CourseColor":"#408AB4","CourseFillColor":"#C4ECFF","Url":"https://sdu.itslearning.com/main.aspx?CourseID={{id}}","TeachersInCourse":[{{teachers ?? ""}}]}""";

    public static string Teacher(string first, string last) => $$"""{"FirstName":"{{first}}","LastName":"{{last}}"}""";

    public static string CoursesJson(params string[] courses) => $$"""{"EntityArray":[{{string.Join(',', courses)}}],"Total":{{courses.Length}}}""";
}

public sealed class ImmediateDispatcher : IUiDispatcher
{
    public void Post(Action action) => action();
}

public sealed class RecordingOpenUrl : IOpenUrl
{
    public List<Uri> Opened { get; } = [];
    public void Open(Uri url) => Opened.Add(url);
}
