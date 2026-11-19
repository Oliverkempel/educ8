using Educ8.App.Core.Services;

namespace Educ8.App.Gtk.Services;

/// <summary>Runs work on the GLib main loop (the GTK UI thread).</summary>
internal sealed class GtkDispatcher : IUiDispatcher
{
    public void Post(Action action) =>
        GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_DEFAULT, () =>
        {
            action();
            return false; // run once
        });
}

/// <summary>
/// Opens URLs with the desktop's default handler: GTK's launcher (portal aware), falling back to GIO's default handler.
/// Failures are written to stderr; a launch that fails silently is otherwise invisible ("nothing happens").
/// </summary>
internal sealed class GtkOpenUrl : IOpenUrl
{
    public global::Gtk.Window? Parent { get; set; }

    public void Open(Uri url) => _ = Launch.RunAsync($"URL {Launch.Describe(url)}", () => global::Gtk.UriLauncher.New(url.ToString()).LaunchAsync(Parent),
        () => Gio.AppInfoHelper.LaunchDefaultForUri(url.ToString(), null));
}

/// <summary>Opens local files in the desktop's default application for their type (same fallback and logging as URLs).</summary>
internal sealed class GtkOpenFile : IOpenFile
{
    public global::Gtk.Window? Parent { get; set; }

    public void Open(string path) => _ = Launch.RunAsync("file", () => global::Gtk.FileLauncher.New(Gio.FileHelper.NewForPath(path)).LaunchAsync(Parent),
        () => Gio.AppInfoHelper.LaunchDefaultForUri(new Uri(path).AbsoluteUri, null));
}

internal static class Launch
{
    private static readonly bool DebugOn = Environment.GetEnvironmentVariable("EDUC8_DEBUG") is "1" or "true";

    /// <summary>EDUC8_DEBUG=1: one line to stderr.</summary>
    public static void Debug(string message)
    {
        if (DebugOn) Console.Error.WriteLine($"[open] {message}");
    }

    /// <summary>Scheme, host and path only: queries can carry ids and tokens.</summary>
    public static string Describe(Uri? url) => url is null ? "(none)" : $"{url.Scheme}://{url.Host}{url.AbsolutePath}";

    public static async Task RunAsync(string what, Func<Task<bool>> launch, Func<bool> fallback)
    {
        Debug($"launching {what}");
        try
        {
            if (await launch())
            {
                Debug($"launched {what}");
                return;
            }
            Console.Error.WriteLine($"Opening the {what} returned false; trying the default handler.");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Opening the {what} failed ({e.GetType().Name}: {e.Message}); trying the default handler.");
        }
        try
        {
            if (!fallback()) Console.Error.WriteLine($"The default handler couldn't open the {what} either.");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"The default handler couldn't open the {what}: {e.Message}");
        }
    }
}
