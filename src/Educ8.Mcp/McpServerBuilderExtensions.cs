using System.Diagnostics;
using Educ8.Mcp.Control;
using Educ8.Mcp.Tools;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Educ8.Mcp;

public static class McpServerBuilderExtensions
{
    /// <summary>
    /// Adds the Educ8 itslearning tools. Transport-agnostic: the console host uses stdio; the desktop app can
    /// call this on an HTTP-transport builder. Requires <c>services.AddItslearning(...)</c>.
    /// </summary>
    /// <remarks>
    /// Every tool is registered; what clients may use is decided per request from <paramref name="options"/>, so settings can
    /// change while the server runs (<see cref="McpSettingsWatcher"/>): tools the user turned off, and tools that change
    /// itslearning while <see cref="Educ8McpOptions.AllowWriteActions"/> is off, are left out of <c>tools/list</c> and refused
    /// if called. Each call is reported to <see cref="IMcpActivitySink"/> when one is registered.
    /// </remarks>
    public static IMcpServerBuilder AddEduc8Tools(this IMcpServerBuilder builder, Educ8McpOptions? options = null)
    {
        options ??= new Educ8McpOptions();
        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<McpSessionTracker>();
        builder.Services.Configure<McpServerOptions>(o =>
        {
            o.Capabilities ??= new ServerCapabilities();
            o.Capabilities.Tools ??= new ToolsCapability();
            o.Capabilities.Tools.ListChanged = true; // settings changes are announced
        });
        // Must be IEnumerable<Type>: a Type[] binds to the generic WithTools<T>(T target) overload and registers nothing.
        builder.WithTools(ReadToolTypes, serializerOptions: null);
        builder.WithTools(WriteToolTypes, serializerOptions: null);
        builder.WithRequestFilters(filters =>
        {
            filters.AddListToolsFilter(next => async (context, ct) =>
            {
                Track(context);
                var result = await next(context, ct);
                result.Tools = result.Tools.Where(t => options.IsAvailable(t.Name)).ToList();
                return result;
            });
            filters.AddCallToolFilter(next => async (context, ct) =>
            {
                Track(context);
                var name = context.Params?.Name ?? "";
                var sink = context.Services?.GetService<IMcpActivitySink>();
                var entry = new McpActivityEntry
                {
                    Time = DateTimeOffset.UtcNow,
                    Tool = name,
                    Client = context.Server.ClientInfo?.Name,
                    Arguments = McpActivityLog.Summarize(context.Params?.Arguments?.ToDictionary()),
                    ProcessId = Environment.ProcessId,
                };
                if (!options.IsAvailable(name))
                {
                    sink?.Record(entry with { Blocked = true, Error = "Turned off in Educ8" }, context.Server.ClientInfo?.Version);
                    return new CallToolResult
                    {
                        IsError = true,
                        Content = [new TextContentBlock { Text = McpToolCatalog.Find(name)?.IsWrite == true && !options.AllowWriteActions
                            ? "Write actions are turned off. The user can allow them in the Educ8 desktop app (MCP page) or with `educ8-mcp write-actions on`."
                            : $"The tool '{name}' is turned off in the Educ8 desktop app (MCP page)." }],
                    };
                }
                var watch = Stopwatch.StartNew();
                try
                {
                    var result = await next(context, ct);
                    var error = result.IsError == true ? result.Content.OfType<TextContentBlock>().FirstOrDefault()?.Text : null;
                    sink?.Record(entry with { Ok = result.IsError != true, DurationMs = watch.ElapsedMilliseconds, Error = Shorten(error) },
                        context.Server.ClientInfo?.Version);
                    return result;
                }
                catch (Exception e)
                {
                    sink?.Record(entry with { Ok = false, DurationMs = watch.ElapsedMilliseconds, Error = Shorten(e.Message) }, context.Server.ClientInfo?.Version);
                    throw;
                }
            });
        });
        return builder;

        static void Track<T>(RequestContext<T> context)
        {
            if (context.Services?.GetService<McpSessionTracker>() is { Server: null } tracker) tracker.Server = context.Server;
        }
    }

    private static string? Shorten(string? text) => text is null ? null : text.Length <= 200 ? text : text[..200] + "…";

    /// <summary>Tools that only read (static classes with [McpServerToolType]).</summary>
    internal static readonly IEnumerable<Type> ReadToolTypes =
        [typeof(AuthTools), typeof(CourseTools), typeof(CourseContentTools), typeof(FileTools), typeof(AssignmentTools), typeof(ScheduleTools), typeof(UpdatesTools)];

    /// <summary>Tools that change things in itslearning. Only usable while <see cref="Educ8McpOptions.AllowWriteActions"/> is on.</summary>
    internal static readonly IEnumerable<Type> WriteToolTypes = [typeof(WriteTools)];
}
