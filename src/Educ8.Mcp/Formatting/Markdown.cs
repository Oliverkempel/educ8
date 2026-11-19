namespace Educ8.Mcp.Formatting;

internal static class Markdown
{
    /// <summary>Makes text safe inside a Markdown table cell.</summary>
    public static string Cell(string? text) =>
        string.IsNullOrWhiteSpace(text) ? "" : text.Replace("|", "\\|").Replace("\r", " ").Replace("\n", " ").Trim();

    public static string Date(DateTimeOffset? value) => value?.ToString("yyyy-MM-dd") ?? "";
}
