namespace Educ8.App.Core.Services;

/// <summary>Runs work on the UI thread. Each platform app implements it (GLib main loop, AppKit main queue, WinUI DispatcherQueue).</summary>
public interface IUiDispatcher
{
    void Post(Action action);
}

/// <summary>Opens a URL in the user's default browser.</summary>
public interface IOpenUrl
{
    void Open(Uri url);
}

/// <summary>Opens a local file in the user's default application for its type.</summary>
public interface IOpenFile
{
    void Open(string path);
}

/// <summary>Lets the user choose files from their computer (the platform's own file dialog). An empty list when they cancel.</summary>
public interface IPickFiles
{
    Task<IReadOnlyList<string>> PickFilesAsync(string title, CancellationToken ct = default);
}
