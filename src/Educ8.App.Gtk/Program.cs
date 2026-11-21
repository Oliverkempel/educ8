using System.Runtime.Versioning;
using Educ8.App.Core;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Services;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk;
using Educ8.App.Gtk.Services;
using Educ8.App.Gtk.Shell;
using Educ8.Itslearning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: SupportedOSPlatform("linux")]

// Educ8 for Linux: GTK 4 + libadwaita. Composition only; behaviour lives in Educ8.App.Core.
ItslearningOptions options;
try
{
    options = ItslearningSettingsLoader.Load();
    options.Validate();
}
catch (InvalidOperationException e)
{
    Console.Error.WriteLine(e.Message);
    return 1;
}

// Gir.Core loads a native library only once its module is initialised; Adw.Application would do it, but the clock setting
// below is read before that.
Adw.Module.Initialize();

// Dates and times in the system's formats, including the desktop's 12/24-hour choice (which .NET doesn't read).
AppCulture.Apply(AppCulture.Create(System.Globalization.CultureInfo.CurrentCulture, Environment.GetEnvironmentVariable,
    DesktopSettings.Use24HourClock()));

var openUrl = new GtkOpenUrl();
var openFile = new GtkOpenFile();
var services = new ServiceCollection();
services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
services.AddItslearning(options);
services.AddSingleton<IUiDispatcher, GtkDispatcher>();
services.AddSingleton<IOpenUrl>(openUrl);
services.AddSingleton<IOpenFile>(openFile);
services.AddEduc8AppCore();
using var provider = services.BuildServiceProvider();

var screenshotDir = Environment.GetEnvironmentVariable("EDUC8_SCREENSHOT_DIR") is { Length: > 0 } d ? d : null;
// One instance per session: a second launch activates the running window and exits. Screenshot runs stay separate.
// HandlesOpen: the desktop entry registers the app for itsl-itslearning:// links, so the browser hands the sign-in redirect
// to `educ8 <url>`, which GApplication forwards to the running instance's open signal.
var app = Adw.Application.New("io.github.educ8.Educ8",
    (screenshotDir is null ? Gio.ApplicationFlags.DefaultFlags : Gio.ApplicationFlags.NonUnique) | Gio.ApplicationFlags.HandlesOpen);
MainWindow? main = null;
app.OnOpen += (sender, args) =>
{
    var login = provider.GetRequiredService<ShellViewModel>().Login;
    var redirects = args.Files.Select(f => f.GetUri()).Where(login.IsRedirect).ToList();
    if (redirects.Count == 0)
    {
        app.Activate(); // `educ8 something-else`: just show the window
        return;
    }
    // Keep the process alive while the redirect is handled: without a window (a login started by educ8-mcp while the
    // app was closed) it only passes the link on and quits.
    app.Hold();
    _ = HandleRedirectsAsync();

    async Task HandleRedirectsAsync()
    {
        try
        {
            foreach (var url in redirects)
                if (await login.AcceptRedirectAsync(url)) main?.Window.Present();
        }
        finally
        {
            app.Release();
        }
    }
};
app.OnActivate += (_, _) =>
{
    if (main is not null)
    {
        main.Window.Present();
        return;
    }
    var shell = provider.GetRequiredService<ShellViewModel>();
    main = new MainWindow(app, shell);
    openUrl.Parent = openFile.Parent = main.Window;
    main.Window.Present();
    _ = shell.InitializeAsync();

    // Development aid: EDUC8_SCREENSHOT_DIR=… renders each screen to PNG and quits (used to check the UI headless).
    if (screenshotDir is not null)
        _ = Screenshots.RunAsync(app, main, shell, screenshotDir);
};
// GApplication expects argv[0] to be the program name (Gir.Core doesn't add it); without it the first URL is dropped.
return app.RunWithSynchronizationContext([Environment.ProcessPath ?? "educ8", .. args]);
