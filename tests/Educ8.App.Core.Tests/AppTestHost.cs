using System.Globalization;
using Educ8.App.Core.Services;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Content;
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
    public RecordingOpenFile OpenFile { get; } = new();
    public FakeCourseFileSource Files { get; } = new();
    public string CacheDirectory { get; } = Path.Combine(Path.GetTempPath(), "educ8-tests", Guid.NewGuid().ToString("N"));
    public ServiceProvider Provider { get; }

    public AppTestHost()
    {
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("en-GB");
        T.Services.AddSingleton<IUiDispatcher>(new ImmediateDispatcher());
        T.Services.AddSingleton<IOpenUrl>(OpenUrl);
        T.Services.AddSingleton<IOpenFile>(OpenFile);
        T.Services.AddEduc8AppCore();
        // Registered after AddEduc8AppCore, so these win: no web downloads, and a throwaway cache folder.
        T.Services.AddSingleton<ICourseFileSource>(Files);
        T.Services.AddSingleton(sp => new MaterialFiles(Files, OpenFile) { CacheDirectory = CacheDirectory });
        T.Services.AddSingleton(sp => new MessageAttachments(OpenFile, OpenUrl) { CacheDirectory = Path.Combine(CacheDirectory, "messages") });
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
        try { Directory.Delete(CacheDirectory, recursive: true); } catch (DirectoryNotFoundException) { }
    }

    public static string Course(int id, string title, string? code, string? updated, int notifications = 0, int bulletins = 0, string? teachers = null) =>
        $$"""{"CourseId":{{id}},"Title":"{{title}}","CourseCode":{{(code is null ? "null" : $"\"{code}\"")}},"LastUpdatedUtc":{{(updated is null ? "null" : $"\"{updated}\"")}},"NewNotificationsCount":{{notifications}},"NewBulletinsCount":{{bulletins}},"CourseColor":"#408AB4","CourseFillColor":"#C4ECFF","Url":"https://school.itslearning.com/main.aspx?CourseID={{id}}","TeachersInCourse":[{{teachers ?? ""}}]}""";

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

public sealed class RecordingOpenFile : IOpenFile
{
    public List<string> Opened { get; } = [];
    public void Open(string path) => Opened.Add(path);
}

/// <summary>Writes a small file named after the element instead of downloading; can be told to fail.</summary>
public sealed class FakeCourseFileSource : ICourseFileSource
{
    public List<int> Downloads { get; } = [];
    public Exception? Fail { get; set; }

    /// <summary>When set, downloads wait for it (or for cancellation), like a slow file.</summary>
    public TaskCompletionSource? Gate { get; set; }

    public async Task<string> SaveFileAsync(int elementId, string directory, CancellationToken ct = default)
    {
        Downloads.Add(elementId);
        if (Fail is not null) throw Fail;
        if (Gate is not null) await Gate.Task.WaitAsync(ct);
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, $"element-{elementId}.pdf");
        await File.WriteAllTextAsync(path, "%PDF-1.4", ct);
        return path;
    }
}
