using Educ8.Itslearning;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Mcp;
using Educ8.Mcp.Control;
using Educ8.Mcp.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// educ8-mcp: stdio MCP server for itslearning, plus CLI helpers.
// IMPORTANT: in `serve` mode stdout carries JSON-RPC. Never write to stdout there; logs go to stderr.

var command = args.Length == 0 ? "serve" : args[0];
try
{
    return command switch
    {
        "serve" => await Serve(),
        "login" => await Cli.Login(BuildServices(), noBrowser: args.Contains("--no-browser")),
        "status" => await Cli.Status(BuildServices()),
        "logout" => await Cli.Logout(BuildServices()),
        "handle-url" => await Cli.HandleUrl(args.Skip(1).ToArray()),
        "write-actions" => Cli.WriteActions(args.Skip(1).FirstOrDefault()),
        "register-url-handler" => UrlSchemeHandler.Register(),
        "unregister-url-handler" => UrlSchemeHandler.Unregister(),
        "version" or "--version" or "-v" => PrintVersion(),
        "help" or "--help" or "-h" => Usage(0),
        _ => Usage(1),
    };
}
catch (InvalidOperationException e) when (e.Message.Contains("not configured"))
{
    Console.Error.WriteLine(e.Message);
    return 1;
}

static async Task<int> Serve()
{
    var options = ItslearningSettingsLoader.Load();
    var builder = Host.CreateEmptyApplicationBuilder(settings: null);
    builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);
    builder.Logging.SetMinimumLevel(LogLevel.Information);
    builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning); // request URLs can contain search text
    builder.Services.AddItslearning(options);
    var mcpOptions = Educ8McpOptions.Load(options.DataDirectory);
    // Status and activity for the desktop app's MCP page (files in <data dir>/mcp; nothing leaves this machine).
    var registry = new McpServerRegistry(options.DataDirectory);
    builder.Services.AddSingleton(registry);
    builder.Services.AddSingleton(new McpActivityLog(options.DataDirectory));
    builder.Services.AddSingleton<IMcpActivitySink, McpActivityRecorder>();
    builder.Services
        .AddMcpServer(o => o.ServerInfo = new() { Name = "educ8-itslearning", Version = Educ8Version.Of(typeof(Program).Assembly) })
        .WithStdioServerTransport()
        .AddEduc8Tools(mcpOptions);
    var app = builder.Build();
    registry.Register(DateTimeOffset.UtcNow);
    using var watcher = new McpSettingsWatcher(mcpOptions, app.Services.GetRequiredService<McpSessionTracker>(), options.DataDirectory,
        app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Educ8.Settings"));
    app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Educ8").LogInformation(
        mcpOptions.AllowWriteActions
            ? "Write actions are ENABLED (submitting assignments etc.)."
            : "Write actions are disabled (read-only). Enable with `educ8-mcp write-actions on`.");
    try
    {
        await app.RunAsync();
    }
    finally
    {
        registry.Unregister();
    }
    return 0;
}

static ServiceProvider BuildServices()
{
    var services = new ServiceCollection();
    services.AddLogging(b => b.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace).SetMinimumLevel(LogLevel.Warning));
    services.AddItslearning(ItslearningSettingsLoader.Load());
    return services.BuildServiceProvider();
}

static int PrintVersion()
{
    Console.WriteLine($"educ8-mcp {Educ8Version.Full(typeof(Program).Assembly)}");
    return 0;
}

static int Usage(int code)
{
    Console.Error.WriteLine($"""
        educ8-mcp — itslearning MCP server

        Usage:
          educ8-mcp [serve]                 Run the MCP server on stdio (what MCP clients launch)
          educ8-mcp login [--no-browser]    Log in interactively (opens the browser; paste the final link or use the URL handler)
          educ8-mcp status                  Show who is logged in
          educ8-mcp logout                  Forget the stored session
          educ8-mcp --version               Show the version
          educ8-mcp write-actions [on|off]  Show or change whether the AI may change things (submit assignments...). Off by default
          educ8-mcp register-url-handler    Let the browser hand itsl-itslearning:// links to educ8 (Linux, Windows)
          educ8-mcp unregister-url-handler  Remove that registration
          educ8-mcp handle-url <url> [--data-dir <dir>]   (used by the URL handler)

        Configuration: {ItslearningSettingsLoader.ClientIdEnvVar} (required), {ItslearningSettingsLoader.BaseUrlEnvVar}, {ItslearningSettingsLoader.RedirectUriEnvVar},
        {ItslearningSettingsLoader.ScopeEnvVar}, {Educ8Paths.DataDirectoryEnvVar}; or "<data dir>/{Educ8Paths.ConfigFileName}". Data dir: {Educ8Paths.DefaultDataDirectory}
        """);
    return code;
}
