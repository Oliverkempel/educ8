using System.Globalization;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Educ8.Documents;

internal static partial class ExcelExtractor
{
    private const int MaxRows = 300;
    private const int MaxColumns = 40;

    public static ExtractedDocument Extract(string fileName, byte[] bytes, ExtractOptions options)
    {
        var notes = new List<string>();
        var parts = new List<DocumentPart>();
        using var doc = SpreadsheetDocument.Open(new MemoryStream(bytes), false);
        var wb = doc.WorkbookPart;
        if (wb?.Workbook?.Sheets is null) return new ExtractedDocument(fileName, DocumentKind.Excel, parts, ["The workbook has no sheets."]);

        var shared = wb.SharedStringTablePart?.SharedStringTable?.Elements<SharedStringItem>().Select(i => i.InnerText).ToList() ?? [];
        var dateStyles = DateStyleIndexes(wb.WorkbookStylesPart?.Stylesheet);

        foreach (var sheet in wb.Workbook.Sheets.Elements<Sheet>())
        {
            var n = parts.Count + 1;
            var name = sheet.Name?.Value ?? $"Sheet {n}";
            var hidden = sheet.State?.Value is { } st && st != SheetStateValues.Visible;
            var label = $"Sheet: {name}" + (hidden ? " (hidden)" : "");
            if (sheet.Id?.Value is not { } id || wb.GetPartById(id) is not WorksheetPart ws)
            {
                parts.Add(new DocumentPart(n, label, "_(chart sheet or unreadable sheet)_", 0, []));
                continue;
            }

            var grid = new SortedDictionary<int, SortedDictionary<int, string>>();
            var totalRows = 0;
            var truncatedColumns = false;
            foreach (var row in ws.Worksheet?.Descendants<Row>() ?? [])
            {
                var rowIndex = (int)(row.RowIndex?.Value ?? (uint)(totalRows + 1));
                var cells = new SortedDictionary<int, string>();
                foreach (var cell in row.Elements<Cell>())
                {
                    var col = ColumnIndex(cell.CellReference?.Value);
                    if (col < 0) continue;
                    if (col >= MaxColumns) { truncatedColumns = true; continue; }
                    var value = CellText(cell, shared, dateStyles);
                    if (value.Length > 0) cells[col] = value.Replace("|", "\\|").Replace("\n", " ");
                }
                if (cells.Count == 0) continue;
                totalRows++;
                if (grid.Count < MaxRows) grid[rowIndex] = cells;
            }

            var md = grid.Count == 0 ? "_(empty sheet)_" : Render(grid);
            if (totalRows > grid.Count) md += $"\n\n_Showing the first {grid.Count} of {totalRows} non-empty rows._";
            if (truncatedColumns) md += $"\n\n_Columns after {ColumnName(MaxColumns - 1)} are not shown._";
            var images = ws.DrawingsPart?.ImageParts.Count() ?? 0;
            var collected = new List<DocumentImage>();
            if (images > 0 && options.WantImagesFor(n))
            {
                var collector = new ImageCollector(options);
                var k = 0;
                foreach (var img in ws.DrawingsPart!.ImageParts) collector.TryAdd(collected, WordExtractor.ReadAll(img), $"{name}, image {++k}");
                notes.AddRange(collector.Notes());
            }
            if (ws.DrawingsPart?.ChartParts.Any() == true) md += "\n\n_This sheet contains charts (not shown)._";
            parts.Add(new DocumentPart(n, label, md, images, collected));
        }
        notes.Add("Cells show stored values; formulas are shown as their last calculated result.");
        return new ExtractedDocument(fileName, DocumentKind.Excel, parts, notes);
    }

    private static string Render(SortedDictionary<int, SortedDictionary<int, string>> grid)
    {
        var maxCol = grid.Values.Max(r => r.Keys.Max());
        var rows = new List<List<string>> { new[] { "" }.Concat(Enumerable.Range(0, maxCol + 1).Select(ColumnName)).ToList() };
        foreach (var (rowIndex, cells) in grid)
            rows.Add(new[] { rowIndex.ToString(CultureInfo.InvariantCulture) }.Concat(Enumerable.Range(0, maxCol + 1).Select(c => cells.GetValueOrDefault(c, ""))).ToList());
        return MarkdownTable.Render(rows);
    }

    private static string CellText(Cell cell, List<string> shared, HashSet<uint> dateStyles)
    {
        var raw = cell.CellValue?.Text;
        var type = cell.DataType?.Value;
        if (type == CellValues.InlineString) return cell.InlineString?.InnerText ?? "";
        if (raw is null) return "";
        if (type == CellValues.SharedString) return int.TryParse(raw, out var i) && i >= 0 && i < shared.Count ? shared[i] : "";
        if (type == CellValues.Boolean) return raw == "1" ? "TRUE" : "FALSE";
        if (type == CellValues.String || type == CellValues.Error) return raw;
        if (cell.StyleIndex?.Value is { } style && dateStyles.Contains(style)
            && double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var oa) && oa is > 0 and < 2958466)
        {
            var dt = DateTime.FromOADate(oa);
            return dt.TimeOfDay == TimeSpan.Zero ? dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : dt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
        // Stored doubles carry binary noise (2.0299999999999998); 15 significant digits is what Excel itself shows.
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
            return number.ToString("G15", CultureInfo.InvariantCulture);
        return raw;
    }

    /// <summary>Style indexes whose number format is a date/time.</summary>
    private static HashSet<uint> DateStyleIndexes(Stylesheet? styles)
    {
        var result = new HashSet<uint>();
        if (styles?.CellFormats is null) return result;
        var custom = styles.NumberingFormats?.Elements<NumberingFormat>()
            .Where(f => f.NumberFormatId?.Value is not null)
            .ToDictionary(f => f.NumberFormatId!.Value, f => f.FormatCode?.Value ?? "") ?? [];
        uint index = 0;
        foreach (var format in styles.CellFormats.Elements<CellFormat>())
        {
            var id = format.NumberFormatId?.Value ?? 0;
            if (id is >= 14 and <= 22 or >= 45 and <= 47 || (custom.TryGetValue(id, out var code) && IsDateFormat(code))) result.Add(index);
            index++;
        }
        return result;
    }

    private static bool IsDateFormat(string code)
    {
        var stripped = QuotedOrEscaped().Replace(code, "");
        return DateTokens().IsMatch(stripped);
    }

    private static int ColumnIndex(string? reference)
    {
        if (string.IsNullOrEmpty(reference)) return -1;
        var index = 0;
        foreach (var ch in reference)
        {
            if (!char.IsLetter(ch)) break;
            index = index * 26 + (char.ToUpperInvariant(ch) - 'A' + 1);
        }
        return index - 1;
    }

    private static string ColumnName(int index)
    {
        var name = "";
        for (index++; index > 0; index = (index - 1) / 26) name = (char)('A' + (index - 1) % 26) + name;
        return name;
    }

    [GeneratedRegex("\"[^\"]*\"|\\\\.|\\[[^\\]]*\\]")]
    private static partial Regex QuotedOrEscaped();

    [GeneratedRegex("[dDyY]|[mM]{3,}|h{1,2}:|:s")]
    private static partial Regex DateTokens();
}
