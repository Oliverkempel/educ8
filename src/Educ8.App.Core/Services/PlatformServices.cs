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
