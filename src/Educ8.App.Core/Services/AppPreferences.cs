using System.Text.Json;
using System.Text.Json.Nodes;
using Educ8.Itslearning;
using Microsoft.Extensions.Options;

namespace Educ8.App.Core.Services;

/// <summary>How the start page shows courses.</summary>
public enum CoursesLayout
{
    /// <summary>Cards in a grid: name, code, teachers, activity, badges. Click opens the course.</summary>
    Grid,

    /// <summary>One collapsible row per course: expanded it shows the current plans and the teachers.</summary>
    List,
}

/// <summary>
/// The desktop app's own view preferences, in <c>&lt;data dir&gt;/app.json</c> (separate from <c>config.json</c>, which holds
/// itslearning and MCP settings shared with <c>educ8-mcp</c>). A missing or unreadable file gives the defaults.
/// </summary>
public sealed class AppPreferences(IOptions<ItslearningOptions> options)
{
    private const string FileName = "app.json";
    private const string CoursesLayoutKey = "CoursesLayout";

    private string PathName => Path.Combine(options.Value.DataDirectory, FileName);

    public CoursesLayout CoursesLayout
    {
        get => Read()?[CoursesLayoutKey]?.GetValue<string>() is { } s && Enum.TryParse<CoursesLayout>(s, out var layout) ? layout : CoursesLayout.Grid;
        set => Write(CoursesLayoutKey, value.ToString());
    }

    private JsonObject? Read()
    {
        try
        {
            return File.Exists(PathName) ? JsonNode.Parse(File.ReadAllText(PathName)) as JsonObject : null;
        }
        catch (Exception e) when (e is JsonException or IOException or InvalidOperationException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    private void Write(string key, string value)
    {
        try
        {
            var json = Read() ?? new JsonObject();
            json[key] = value;
            Educ8Paths.EnsureDirectory(options.Value.DataDirectory);
            var temp = PathName + ".tmp";
            File.WriteAllText(temp, json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) + "\n");
            File.Move(temp, PathName, overwrite: true);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            // A view preference that isn't saved is not worth an error.
        }
    }
}
