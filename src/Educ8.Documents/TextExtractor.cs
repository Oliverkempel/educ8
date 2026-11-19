using System.Text;
using System.Text.Json;

namespace Educ8.Documents;

internal static class TextExtractor
{
    private const int SectionChars = 10_000;

    private static readonly Dictionary<string, string> CodeLanguages = new(StringComparer.OrdinalIgnoreCase)
    {
        [".c"] = "c", [".h"] = "c", [".cpp"] = "cpp", [".hpp"] = "cpp", [".cc"] = "cpp", [".cs"] = "csharp", [".java"] = "java",
        [".kt"] = "kotlin", [".py"] = "python", [".js"] = "javascript", [".mjs"] = "javascript", [".ts"] = "typescript",
        [".tsx"] = "tsx", [".jsx"] = "jsx", [".go"] = "go", [".rs"] = "rust", [".rb"] = "ruby", [".php"] = "php",
        [".swift"] = "swift", [".scala"] = "scala", [".r"] = "r", [".m"] = "matlab", [".sql"] = "sql", [".sh"] = "bash",
        [".bash"] = "bash", [".ps1"] = "powershell", [".bat"] = "bat", [".yml"] = "yaml", [".yaml"] = "yaml",
        [".toml"] = "toml", [".json"] = "json", [".xml"] = "xml", [".css"] = "css", [".scss"] = "scss",
        [".gradle"] = "groovy", [".dockerfile"] = "dockerfile", [".tex"] = "latex", [".hs"] = "haskell", [".asm"] = "asm",
        [".s"] = "asm", [".vhd"] = "vhdl", [".v"] = "verilog", [".ini"] = "ini", [".cfg"] = "ini", [".properties"] = "properties",
    };

    private static readonly HashSet<string> PlainExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".txt", ".md", ".markdown", ".csv", ".tsv", ".log", ".rst", ".srt", ".vtt",
    };

    private static readonly HashSet<string> SpecialNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Dockerfile", "Makefile", "README", "LICENSE", ".gitignore", "CMakeLists.txt", "Vagrantfile", "Jenkinsfile",
    };

    public static bool Handles(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        return PlainExtensions.Contains(ext) || CodeLanguages.ContainsKey(ext) || ext.Equals(".html", StringComparison.OrdinalIgnoreCase)
               || ext.Equals(".htm", StringComparison.OrdinalIgnoreCase) || ext.Equals(".ipynb", StringComparison.OrdinalIgnoreCase)
               || ext.Equals(".svg", StringComparison.OrdinalIgnoreCase) || SpecialNames.Contains(Path.GetFileName(fileName));
    }

    /// <summary>Heuristic: decodable as UTF-8 with few control characters.</summary>
    public static bool LooksLikeText(byte[] bytes)
    {
        var sample = bytes.AsSpan(0, Math.Min(bytes.Length, 8192));
        if (sample.IndexOf((byte)0) >= 0) return false;
        try
        {
            new UTF8Encoding(false, throwOnInvalidBytes: true).GetString(sample.Length == bytes.Length ? sample : TrimPartialUtf8(sample));
            return true;
        }
        catch (DecoderFallbackException)
        {
            return false;
        }
    }

    public static ExtractedDocument Extract(string fileName, byte[] bytes)
    {
        var text = Decode(bytes);
        var ext = Path.GetExtension(fileName);
        string markdown;
        if (ext.Equals(".ipynb", StringComparison.OrdinalIgnoreCase)) markdown = Notebook(text);
        else if (ext.Equals(".html", StringComparison.OrdinalIgnoreCase) || ext.Equals(".htm", StringComparison.OrdinalIgnoreCase)) markdown = HtmlToMarkdown.Convert(text);
        else if (CodeLanguages.TryGetValue(ext, out var lang)) return Chunked(fileName, text, lang);
        else if (ext.Equals(".svg", StringComparison.OrdinalIgnoreCase)) return Chunked(fileName, text, "xml");
        else return Chunked(fileName, text, null);

        return new ExtractedDocument(fileName, DocumentKind.Text, Split(markdown).Select((s, i) => new DocumentPart(i + 1, $"Section {i + 1}", s, 0, [])).ToList(), []);
    }

    /// <summary>Plain/code text split by lines, labelled with line numbers, fenced for code.</summary>
    private static ExtractedDocument Chunked(string fileName, string text, string? language)
    {
        var lines = text.Replace("\r\n", "\n").Split('\n');
        var parts = new List<DocumentPart>();
        var start = 0;
        while (start < lines.Length)
        {
            var end = start;
            var size = 0;
            while (end < lines.Length && (size + lines[end].Length < SectionChars || end == start)) size += lines[end++].Length + 1;
            var chunk = string.Join('\n', lines[start..end]).TrimEnd();
            var body = language is null ? chunk : $"```{language}\n{chunk}\n```";
            parts.Add(new DocumentPart(parts.Count + 1, $"Lines {start + 1}–{end}", body, 0, []));
            start = end;
        }
        if (parts.Count == 0) parts.Add(new DocumentPart(1, "Lines 1–0", "", 0, []));
        return new ExtractedDocument(fileName, DocumentKind.Text, parts, []);
    }

    private static IEnumerable<string> Split(string markdown)
    {
        if (markdown.Length <= SectionChars) { yield return markdown; yield break; }
        var sb = new StringBuilder();
        foreach (var para in markdown.Split("\n\n"))
        {
            if (sb.Length > 0 && sb.Length + para.Length > SectionChars) { yield return sb.ToString().Trim(); sb.Clear(); }
            sb.Append(para).Append("\n\n");
        }
        if (sb.Length > 0) yield return sb.ToString().Trim();
    }

    /// <summary>Jupyter notebook → Markdown cells, fenced code cells and short text outputs.</summary>
    private static string Notebook(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var language = doc.RootElement.TryGetProperty("metadata", out var meta) && meta.TryGetProperty("language_info", out var li) && li.TryGetProperty("name", out var ln)
                ? ln.GetString() ?? "python"
                : "python";
            var sb = new StringBuilder();
            foreach (var cell in doc.RootElement.GetProperty("cells").EnumerateArray())
            {
                var type = cell.GetProperty("cell_type").GetString();
                var source = Joined(cell.GetProperty("source"));
                if (type == "markdown") sb.Append(source.Trim()).Append("\n\n");
                else if (type == "code")
                {
                    sb.Append($"```{language}\n{source.TrimEnd()}\n```\n\n");
                    if (cell.TryGetProperty("outputs", out var outputs))
                        foreach (var output in outputs.EnumerateArray())
                        {
                            var text = output.TryGetProperty("text", out var t) ? Joined(t)
                                : output.TryGetProperty("data", out var d) && d.TryGetProperty("text/plain", out var tp) ? Joined(tp) : "";
                            if (text.Trim().Length > 0) sb.Append("Output:\n```\n").Append(HtmlToMarkdown.Truncate(text.TrimEnd(), 1500)).Append("\n```\n\n");
                        }
                }
            }
            return sb.ToString().Trim();
        }
        catch (Exception e) when (e is JsonException or KeyNotFoundException or InvalidOperationException)
        {
            return "```json\n" + HtmlToMarkdown.Truncate(json, 20_000) + "\n```";
        }

        static string Joined(JsonElement e) => e.ValueKind == JsonValueKind.Array
            ? string.Concat(e.EnumerateArray().Select(x => x.GetString()))
            : e.GetString() ?? "";
    }

    private static string Decode(byte[] bytes)
    {
        try
        {
            return new UTF8Encoding(false, throwOnInvalidBytes: true).GetString(bytes).TrimStart('﻿');
        }
        catch (DecoderFallbackException)
        {
            return Encoding.Latin1.GetString(bytes);
        }
    }

    private static ReadOnlySpan<byte> TrimPartialUtf8(ReadOnlySpan<byte> s)
    {
        // Don't fail on a multi-byte character cut off at the end of the sample.
        var end = s.Length;
        for (var i = 0; i < 3 && end > 0 && (s[end - 1] & 0xC0) == 0x80; i++) end--;
        if (end > 0 && (s[end - 1] & 0xC0) == 0xC0) end--;
        return s[..end];
    }
}
