using Educ8.Mcp.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Mcp;

public static class McpServerBuilderExtensions
{
    /// <summary>
    /// Adds the Educ8 itslearning tools. Transport-agnostic: the console host uses stdio; the desktop app can
    /// call this on an HTTP-transport builder. Requires <c>services.AddItslearning(...)</c>.
    /// Tools that change anything in itslearning are only registered when <see cref="Educ8McpOptions.AllowWriteActions"/> is on.
    /// </summary>
    public static IMcpServerBuilder AddEduc8Tools(this IMcpServerBuilder builder, Educ8McpOptions? options = null)
    {
        options ??= new Educ8McpOptions();
        builder.Services.AddSingleton(options);
        // Must be IEnumerable<Type>: a Type[] binds to the generic WithTools<T>(T target) overload and registers nothing.
        builder.WithTools(ReadToolTypes, serializerOptions: null);
        if (options.AllowWriteActions) builder.WithTools(WriteToolTypes, serializerOptions: null);
        return builder;
    }

    /// <summary>Tools that only read (static classes with [McpServerToolType]).</summary>
    internal static readonly IEnumerable<Type> ReadToolTypes =
        [typeof(AuthTools), typeof(CourseTools), typeof(CourseContentTools), typeof(FileTools), typeof(AssignmentTools), typeof(ScheduleTools), typeof(UpdatesTools)];

    /// <summary>Tools that change things in itslearning. Gated by <see cref="Educ8McpOptions.AllowWriteActions"/>.</summary>
    internal static readonly IEnumerable<Type> WriteToolTypes = [typeof(WriteTools)];
}
