using System.Text;

namespace Educ8.Documents.Tests;

public class PdfTests
{
    [Fact]
    public void Extracts_text_per_page_and_removes_template_lines_and_logos()
    {
        var doc = DocumentExtractor.Extract("slides.pdf", "application/pdf", TestFiles.Pdf(6, imagePages: [3]));

        Assert.Equal(DocumentKind.Pdf, doc.Kind);
        Assert.Equal(6, doc.Parts.Count);
        Assert.Contains("Unique heading for page 2", doc.Parts[1].Markdown);
        Assert.DoesNotContain("Template footer", doc.Parts[1].Markdown);
        Assert.Contains(doc.Notes, n => n.Contains("repeated on most pages (template text"));
        Assert.Contains(doc.Notes, n => n.Contains("repeated on most pages (logos"));
        // Only page 3 has a real figure; the logo on every page doesn't count.
        Assert.Equal([3], doc.Parts.Where(p => p.ImageCount > 0).Select(p => p.Number));
        Assert.All(doc.Parts, p => Assert.Empty(p.Images)); // images only on request
    }

    [Fact]
    public void Renders_requested_pages_as_whole_page_images()
    {
        var doc = DocumentExtractor.Extract("slides.pdf", null, TestFiles.Pdf(4, imagePages: [3]),
            new ExtractOptions { IncludeImages = true, ImageParts = new HashSet<int> { 2, 3 } });

        Assert.Empty(doc.Parts[0].Images);
        var image = Assert.Single(doc.Parts[2].Images);
        Assert.Equal("image/png", image.MimeType);
        Assert.Contains("Page 3 (whole page)", image.Description);
        Assert.Single(doc.Parts[1].Images); // pages without figures can be rendered too
    }

    [Fact]
    public void Flags_pages_without_text()
    {
        var doc = DocumentExtractor.Extract("scan.pdf", null, TestFiles.Pdf(5, blankPage: 4));
        Assert.Contains(doc.Notes, n => n.Contains("Pages without text") && n.Contains('4'));
    }

    [Fact]
    public void Detects_pdf_without_extension_and_reports_damaged_files()
    {
        Assert.Equal(DocumentKind.Pdf, DocumentExtractor.Extract("download", null, TestFiles.Pdf(1)).Kind);

        var broken = DocumentExtractor.Extract("broken.pdf", null, Encoding.ASCII.GetBytes("%PDF-1.7 garbage"));
        Assert.Empty(broken.Parts);
        Assert.NotEmpty(broken.Notes);
    }
}

public class OfficeTests
{
    [Fact]
    public void Word_headings_lists_and_tables()
    {
        var doc = DocumentExtractor.Extract("assignment.docx", null, TestFiles.Docx());
        var md = Assert.Single(doc.Parts).Markdown;

        Assert.Equal(DocumentKind.Word, doc.Kind);
        Assert.Contains("## Assignment 2", md);
        Assert.Contains("Write a short report about databases.", md);
        Assert.Contains("- First requirement\n- Second requirement", md);
        Assert.Contains("| Criterion | Points |\n|---|---|\n| Design \\| ER | 40 |", md);
    }

    [Fact]
    public void Excel_sheets_numbers_and_dates()
    {
        var doc = DocumentExtractor.Extract("data.xlsx", null, TestFiles.Xlsx());
        var sheet = Assert.Single(doc.Parts);

        Assert.Equal("Sheet: Data", sheet.Label);
        Assert.Contains("|  | A | B | C |", sheet.Markdown);
        Assert.Contains("| 1 | Year | Players | Due |", sheet.Markdown);
        Assert.Contains("| 2 | 2015 | 2.03 | 2025-12-09 |", sheet.Markdown); // serial 46000 with date format 14
        Assert.Contains("| 3 | 2016 | 2.17 |  |", sheet.Markdown);
    }

    [Fact]
    public void PowerPoint_titles_bullets_tables_notes_and_images()
    {
        var doc = DocumentExtractor.Extract("lecture.pptx", null, TestFiles.Pptx());

        Assert.Equal(DocumentKind.PowerPoint, doc.Kind);
        Assert.Equal(2, doc.Parts.Count);
        Assert.Equal("Slide 1: Singleton pattern", doc.Parts[0].Label);
        Assert.Contains("- Only one instance\n\n  - Lazy creation", doc.Parts[0].Markdown);
        Assert.Contains("Speaker notes: Mention thread safety", doc.Parts[0].Markdown);
        Assert.Equal(1, doc.Parts[0].ImageCount);
        Assert.Contains("| Pattern | Use |\n|---|---|\n| Facade | Simplify |", doc.Parts[1].Markdown);

        var withImages = DocumentExtractor.Extract("lecture.pptx", null, TestFiles.Pptx(), new ExtractOptions { IncludeImages = true });
        Assert.Equal("image/png", Assert.Single(withImages.Parts[0].Images).MimeType);
    }

    [Fact]
    public void Office_file_detected_by_content_when_extension_is_missing() =>
        Assert.Equal(DocumentKind.Word, DocumentExtractor.Extract("download", null, TestFiles.Docx()).Kind);

    [Fact]
    public void Old_binary_office_formats_are_explained() =>
        Assert.Contains("old binary Office", Assert.Single(DocumentExtractor.Extract("notes.doc", null, [1, 2, 3]).Notes));
}

public class TextImageZipTests
{
    [Fact]
    public void Code_is_fenced_with_language()
    {
        var doc = DocumentExtractor.Extract("Main.java", null, Encoding.UTF8.GetBytes("class Main {}\n"));
        Assert.Equal("```java\nclass Main {}\n```", Assert.Single(doc.Parts).Markdown);
    }

    [Fact]
    public void Long_text_is_split_into_line_ranges()
    {
        var text = string.Join('\n', Enumerable.Range(1, 3000).Select(i => $"line {i} with some padding text"));
        var doc = DocumentExtractor.Extract("log.txt", null, Encoding.UTF8.GetBytes(text));
        Assert.True(doc.Parts.Count > 3);
        Assert.StartsWith("Lines 1–", doc.Parts[0].Label);
        Assert.Contains("line 3000", doc.Parts[^1].Markdown);
    }

    [Fact]
    public void Notebook_cells_and_outputs()
    {
        var nb = """{"metadata":{"language_info":{"name":"python"}},"cells":[{"cell_type":"markdown","source":["# Exercise 1"]},{"cell_type":"code","source":["print(1+1)"],"outputs":[{"text":["2\n"]}]}]}""";
        var md = Assert.Single(DocumentExtractor.Extract("ex.ipynb", null, Encoding.UTF8.GetBytes(nb)).Parts).Markdown;
        Assert.Equal("# Exercise 1\n\n```python\nprint(1+1)\n```\n\nOutput:\n```\n2\n```", md);
    }

    [Fact]
    public void Latin1_text_is_decoded() =>
        Assert.Contains("Ærø", Assert.Single(DocumentExtractor.Extract("a.txt", null, Encoding.Latin1.GetBytes("Ærø")).Parts).Markdown);

    [Fact]
    public void Image_file_is_returned_as_image()
    {
        var png = TestFiles.Png(200, 100);
        var doc = DocumentExtractor.Extract("diagram.png", "image/png", png);
        var image = Assert.Single(Assert.Single(doc.Parts).Images);
        Assert.Equal(DocumentKind.Image, doc.Kind);
        Assert.Equal("image/png", image.MimeType);
        Assert.Contains("200×100", image.Description);
    }

    [Fact]
    public void Zip_lists_entries_and_reads_one()
    {
        var zip = TestFiles.Zip(new() { ["src/app.py"] = "print('hi')", ["docs/report.docx"] = TestFiles.Docx() });

        var listing = DocumentExtractor.Extract("project.zip", null, zip);
        Assert.Equal(DocumentKind.Archive, listing.Kind);
        Assert.Contains("| src/app.py |", listing.Parts[0].Markdown);

        var entry = DocumentExtractor.Extract("project.zip", null, zip, new ExtractOptions { ZipEntry = "report.docx" });
        Assert.Equal(DocumentKind.Word, entry.Kind);
        Assert.Equal("project.zip › docs/report.docx", entry.FileName);

        var missing = DocumentExtractor.Extract("project.zip", null, zip, new ExtractOptions { ZipEntry = "nope.txt" });
        Assert.Contains("No file \"nope.txt\"", Assert.Single(missing.Notes));
    }
}

public class RangesTests
{
    [Theory]
    [InlineData("3", 10, new[] { 3 })]
    [InlineData("1-3", 10, new[] { 1, 2, 3 })]
    [InlineData("8-", 10, new[] { 8, 9, 10 })]
    [InlineData("2,5,7-8", 10, new[] { 2, 5, 7, 8 })]
    [InlineData("9-20", 10, new[] { 9, 10 })]
    public void Parses(string spec, int max, int[] expected) => Assert.Equal(expected, Ranges.Parse(spec, max)!);

    [Fact]
    public void Blank_means_all() => Assert.Null(Ranges.Parse(" ", 5));

    [Theory]
    [InlineData("x")]
    [InlineData("5-2")]
    [InlineData("20")]
    public void Rejects_bad_specs(string spec) => Assert.Throws<FormatException>(() => Ranges.Parse(spec, 10));

    [Fact]
    public void Formats_runs() => Assert.Equal("1–3, 5, 7–8", Ranges.Format([1, 2, 3, 5, 7, 8]));
}
