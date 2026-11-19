using System.Text;
using Educ8.Itslearning.Models;

namespace Educ8.Mcp.Formatting;

internal static class ResourceFormatter
{
    public static string KindLabel(ResourceKind kind, int learningToolId) => kind switch
    {
        ResourceKind.Folder => "Folder",
        ResourceKind.Page => "Page",
        ResourceKind.File => "File",
        ResourceKind.Link => "Link",
        ResourceKind.Assignment => "Assignment",
        ResourceKind.Test => "Test",
        ResourceKind.Survey => "Survey",
        ResourceKind.Activity => "Activity",
        ResourceKind.LearningPath => "Learning path",
        _ => learningToolId > 0 ? $"Other (tool {learningToolId})" : "Other",
    };

    /// <summary>What the model can do next with an item of this kind.</summary>
    public static string NextStep(ResourceKind kind) => kind switch
    {
        ResourceKind.Folder or ResourceKind.LearningPath => "browse",
        ResourceKind.Page or ResourceKind.Link => "read",
        ResourceKind.File => "read_file",
        ResourceKind.Assignment => "get_assignment",
        _ => "open in itslearning",
    };

    /// <summary>" / Course title / 02: Topic / Lecture" → "02: Topic / Lecture" (course title dropped).</summary>
    public static string CleanPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return "";
        var parts = path.Split(" / ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim('/', ' ')).Where(p => p.Length > 0).ToList();
        return string.Join(" / ", parts.Skip(1));
    }

    public static string Table(IEnumerable<CourseResource> resources, bool includePath)
    {
        var sb = new StringBuilder();
        sb.Append(includePath ? "| Element id | Type | Title | Location | Use |\n|---|---|---|---|---|\n" : "| Element id | Type | Title | Use |\n|---|---|---|---|\n");
        foreach (var r in resources)
        {
            var title = Markdown.Cell(r.Title) + (r.Homework ? " (homework)" : "") + (r.Active ? "" : " (inactive)");
            sb.Append($"| {r.ElementId} | {KindLabel(r.Kind, r.LearningToolId)} | {title} |");
            if (includePath) sb.Append($" {Markdown.Cell(CleanPath(r.Path))} |");
            sb.Append($" {NextStep(r.Kind)} |\n");
        }
        return sb.ToString();
    }

    public const string UseLegend =
        "Use: **browse** → `browse_course_materials` with the element id as folderId; **read** → `read_material` with the element id; " +
        "**read_file** → `read_file` with the element id (supports pages=… and includeImages=true); **get_assignment** → `get_assignment` with the element id.";
}
