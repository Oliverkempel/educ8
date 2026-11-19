using System.Runtime.Versioning;
using Educ8.App.Core;
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

// Gir.Core maps each library's short name ("WebKit") to its real file only once the module is initialised.
// Adw/Gtk are initialised by Adw.Application; WebKit is used later (sign-in dialog) and must be initialised here.
WebKit.Module.Initialize();

var screenshotDir = Environment.GetEnvironmentVariable("EDUC8_SCREENSHOT_DIR") is { Length: > 0 } d ? d : null;
// One instance per session: a second launch activates the running window and exits. Screenshot runs stay separate.
var app = Adw.Application.New("io.github.educ8.Educ8",
    screenshotDir is null ? Gio.ApplicationFlags.DefaultFlags : Gio.ApplicationFlags.NonUnique);
MainWindow? main = null;
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
return app.RunWithSynchronizationContext(args);
