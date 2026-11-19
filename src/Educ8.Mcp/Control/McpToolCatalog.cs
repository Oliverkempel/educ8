using System.ComponentModel;
using System.Reflection;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Control;

/// <summary>One Educ8 MCP tool, as the desktop app's MCP page lists it.</summary>
public sealed record McpToolInfo(string Name, string Title, string Description, bool IsWrite);

/// <summary>All Educ8 tools (read and write), read from the tool classes' attributes, in registration order.</summary>
public static class McpToolCatalog
{
    private static readonly Lazy<IReadOnlyList<McpToolInfo>> AllTools = new(() =>
        McpServerBuilderExtensions.ReadToolTypes.SelectMany(t => Tools(t, isWrite: false))
            .Concat(McpServerBuilderExtensions.WriteToolTypes.SelectMany(t => Tools(t, isWrite: true)))
            .ToList());

    private static readonly Lazy<IReadOnlyDictionary<string, McpToolInfo>> ByName = new(() => AllTools.Value.ToDictionary(t => t.Name));

    public static IReadOnlyList<McpToolInfo> All => AllTools.Value;

    public static McpToolInfo? Find(string name) => ByName.Value.GetValueOrDefault(name);

    private static IEnumerable<McpToolInfo> Tools(Type type, bool isWrite) =>
        type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Select(m => (Method: m, Tool: m.GetCustomAttribute<McpServerToolAttribute>()))
            .Where(x => x.Tool?.Name is not null)
            .Select(x => new McpToolInfo(
                x.Tool!.Name!,
                x.Tool.Title ?? x.Tool.Name!,
                x.Method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "",
                isWrite));
}
