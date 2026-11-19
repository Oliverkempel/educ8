using System.IO.Compression;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
using W = DocumentFormat.OpenXml.Wordprocessing;

namespace Educ8.Documents.Tests;

/// <summary>Builds small real documents in memory, so no course material is stored in the repo.</summary>
public static class TestFiles
{
    /// <summary>A valid RGB PNG with noisy pixels (so it isn't tiny after compression).</summary>
    public static byte[] Png(int width, int height, int seed = 1)
    {
        var rng = new Random(seed);
        var raw = new MemoryStream();
        for (var y = 0; y < height; y++)
        {
            raw.WriteByte(0); // filter: none
            for (var x = 0; x < width * 3; x++) raw.WriteByte((byte)rng.Next(256));
        }
        var compressed = new MemoryStream();
        using (var z = new ZLibStream(compressed, CompressionLevel.Fastest, leaveOpen: true)) raw.WriteTo(z);

        var png = new MemoryStream();
        png.Write([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]);
        var ihdr = new byte[13];
        BE(ihdr, 0, width); BE(ihdr, 4, height);
        ihdr[8] = 8; ihdr[9] = 2; // 8-bit RGB
        Chunk(png, "IHDR", ihdr);
        Chunk(png, "IDAT", compressed.ToArray());
        Chunk(png, "IEND", []);
        return png.ToArray();

        static void BE(byte[] b, int o, int v) { b[o] = (byte)(v >> 24); b[o + 1] = (byte)(v >> 16); b[o + 2] = (byte)(v >> 8); b[o + 3] = (byte)v; }
        static void Chunk(Stream s, string type, byte[] data)
        {
            var len = new byte[4]; BE(len, 0, data.Length); s.Write(len);
            var typeBytes = Encoding.ASCII.GetBytes(type);
            s.Write(typeBytes); s.Write(data);
            var crc = new byte[4]; BE(crc, 0, (int)Crc32(typeBytes.Concat(data).ToArray())); s.Write(crc);
        }
        static uint Crc32(byte[] data)
        {
            var c = 0xFFFFFFFFu;
            foreach (var b in data)
            {
                c ^= b;
                for (var k = 0; k < 8; k++) c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
            }
            return c ^ 0xFFFFFFFFu;
        }
    }

    /// <summary>A PDF where each page has a repeated template line plus its own text; optional image on some pages; optional blank page.</summary>
    public static byte[] Pdf(int pages, int[]? imagePages = null, int? blankPage = null)
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);
        var logo = Png(64, 64, seed: 99);
        for (var i = 1; i <= pages; i++)
        {
            var page = builder.AddPage(595, 842);
            page.AddPng(logo, new PdfRectangle(500, 780, 564, 830)); // same logo on every page
            if (i == blankPage) continue;
            page.AddText("Template footer text on every page", 9, new PdfPoint(50, 30), font);
            page.AddText($"Unique heading for page {i}", 18, new PdfPoint(50, 760), font);
            page.AddText($"Body text explaining topic {i}.", 12, new PdfPoint(50, 720), font);
            if (imagePages?.Contains(i) == true) page.AddPng(Png(120, 90, seed: i), new PdfRectangle(50, 400, 290, 580));
        }
        return builder.Build();
    }

    public static byte[] Docx()
    {
        var ms = new MemoryStream();
        using (var doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
        {
            var main = doc.AddMainDocumentPart();
            main.Document = new W.Document(new W.Body(
                Para("Assignment 2", "Heading1"),
                Para("Write a short report about databases."),
                ListItem("First requirement"),
                ListItem("Second requirement"),
                new W.Table(
                    Row("Criterion", "Points"),
                    Row("Design | ER", "40"),
                    Row("Report", "60")),
                Para("Good luck!")));
        }
        return ms.ToArray();

        static W.Paragraph Para(string text, string? style = null)
        {
            var p = new W.Paragraph(new W.Run(new W.Text(text)));
            if (style is not null) p.PrependChild(new W.ParagraphProperties(new W.ParagraphStyleId { Val = style }));
            return p;
        }
        static W.Paragraph ListItem(string text) => new(
            new W.ParagraphProperties(new W.NumberingProperties(new W.NumberingLevelReference { Val = 0 }, new W.NumberingId { Val = 1 })),
            new W.Run(new W.Text(text)));
        static W.TableRow Row(params string[] cells) => new(cells.Select(c => new W.TableCell(new W.Paragraph(new W.Run(new W.Text(c))))));
    }

    public static byte[] Xlsx()
    {
        var ms = new MemoryStream();
        using (var doc = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
        {
            var wb = doc.AddWorkbookPart();
            wb.Workbook = new Workbook();
            var styles = wb.AddNewPart<WorkbookStylesPart>();
            styles.Stylesheet = new Stylesheet(
                new Fonts(new Font()), new Fills(new Fill()), new Borders(new Border()),
                new CellFormats(new CellFormat(), new CellFormat { NumberFormatId = 14, ApplyNumberFormat = true }));
            var sst = wb.AddNewPart<SharedStringTablePart>();
            sst.SharedStringTable = new SharedStringTable(new SharedStringItem(new Text("Year")), new SharedStringItem(new Text("Players")), new SharedStringItem(new Text("Due")));

            var ws = wb.AddNewPart<WorksheetPart>();
            ws.Worksheet = new Worksheet(new SheetData(
                new Row(S("A1", 0), S("B1", 1), S("C1", 2)) { RowIndex = 1 },
                new Row(N("A2", "2015"), N("B2", "2.0299999999999998"), new Cell { CellReference = "C2", StyleIndex = 1, CellValue = new CellValue("46000") }) { RowIndex = 2 },
                new Row(N("A3", "2016"), N("B3", "2.17")) { RowIndex = 3 }));
            wb.Workbook.AppendChild(new Sheets(new Sheet { Id = wb.GetIdOfPart(ws), SheetId = 1, Name = "Data" }));
        }
        return ms.ToArray();

        static Cell S(string r, int i) => new() { CellReference = r, DataType = CellValues.SharedString, CellValue = new CellValue(i.ToString()) };
        static Cell N(string r, string v) => new() { CellReference = r, CellValue = new CellValue(v) };
    }

    /// <summary>A minimal two-slide PPTX built from raw XML parts (title, bullets, table, image, speaker notes).</summary>
    public static byte[] Pptx()
    {
        const string P = "http://schemas.openxmlformats.org/presentationml/2006/main";
        const string A = "http://schemas.openxmlformats.org/drawingml/2006/main";
        const string R = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        const string Rel = "http://schemas.openxmlformats.org/package/2006/relationships";
        static string Shape(string text, string? ph, int level = 0) =>
            $"""<p:sp><p:nvSpPr><p:cNvPr id="2" name="s"/><p:cNvSpPr/><p:nvPr>{(ph is null ? "" : $"<p:ph type=\"{ph}\"/>")}</p:nvPr></p:nvSpPr><p:spPr/><p:txBody><a:bodyPr/><a:p><a:pPr lvl="{level}"/><a:r><a:t>{text}</a:t></a:r></a:p></p:txBody></p:sp>""";
        static string Slide(string body) =>
            $"""<?xml version="1.0" encoding="UTF-8"?><p:sld xmlns:p="{P}" xmlns:a="{A}" xmlns:r="{R}"><p:cSld><p:spTree><p:nvGrpSpPr><p:cNvPr id="1" name=""/><p:cNvGrpSpPr/><p:nvPr/></p:nvGrpSpPr><p:grpSpPr/>{body}</p:spTree></p:cSld></p:sld>""";

        var files = new Dictionary<string, object>
        {
            ["[Content_Types].xml"] = """<?xml version="1.0" encoding="UTF-8"?><Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Default Extension="png" ContentType="image/png"/><Override PartName="/ppt/presentation.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml"/><Override PartName="/ppt/slides/slide1.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slide+xml"/><Override PartName="/ppt/slides/slide2.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slide+xml"/><Override PartName="/ppt/notesSlides/notesSlide1.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.notesSlide+xml"/></Types>""",
            ["_rels/.rels"] = $"""<?xml version="1.0" encoding="UTF-8"?><Relationships xmlns="{Rel}"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="ppt/presentation.xml"/></Relationships>""",
            ["ppt/presentation.xml"] = $"""<?xml version="1.0" encoding="UTF-8"?><p:presentation xmlns:p="{P}" xmlns:r="{R}"><p:sldIdLst><p:sldId id="256" r:id="rId1"/><p:sldId id="257" r:id="rId2"/></p:sldIdLst></p:presentation>""",
            ["ppt/_rels/presentation.xml.rels"] = $"""<?xml version="1.0" encoding="UTF-8"?><Relationships xmlns="{Rel}"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide" Target="slides/slide1.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide" Target="slides/slide2.xml"/></Relationships>""",
            ["ppt/slides/slide1.xml"] = Slide(Shape("Singleton pattern", "title") + Shape("Only one instance", null) + Shape("Lazy creation", null, 1) +
                $"""<p:pic><p:nvPicPr><p:cNvPr id="5" name="img"/><p:cNvPicPr/><p:nvPr/></p:nvPicPr><p:blipFill><a:blip r:embed="rIdImg"/></p:blipFill><p:spPr/></p:pic>"""),
            ["ppt/slides/_rels/slide1.xml.rels"] = $"""<?xml version="1.0" encoding="UTF-8"?><Relationships xmlns="{Rel}"><Relationship Id="rIdImg" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/image" Target="../media/image1.png"/><Relationship Id="rIdN" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/notesSlide" Target="../notesSlides/notesSlide1.xml"/></Relationships>""",
            ["ppt/notesSlides/notesSlide1.xml"] = $"""<?xml version="1.0" encoding="UTF-8"?><p:notes xmlns:p="{P}" xmlns:a="{A}"><p:cSld><p:spTree><p:nvGrpSpPr><p:cNvPr id="1" name=""/><p:cNvGrpSpPr/><p:nvPr/></p:nvGrpSpPr><p:grpSpPr/>{Shape("Mention thread safety", "body")}</p:spTree></p:cSld></p:notes>""",
            ["ppt/media/image1.png"] = Png(80, 60, seed: 7),
            ["ppt/slides/slide2.xml"] = Slide(Shape("Comparison", "title") +
                $"""<p:graphicFrame><p:nvGraphicFramePr><p:cNvPr id="3" name="t"/><p:cNvGraphicFramePr/><p:nvPr/></p:nvGraphicFramePr><p:xfrm/><a:graphic><a:graphicData uri="http://schemas.openxmlformats.org/drawingml/2006/table"><a:tbl><a:tr h="1"><a:tc><a:txBody><a:bodyPr/><a:p><a:r><a:t>Pattern</a:t></a:r></a:p></a:txBody></a:tc><a:tc><a:txBody><a:bodyPr/><a:p><a:r><a:t>Use</a:t></a:r></a:p></a:txBody></a:tc></a:tr><a:tr h="1"><a:tc><a:txBody><a:bodyPr/><a:p><a:r><a:t>Facade</a:t></a:r></a:p></a:txBody></a:tc><a:tc><a:txBody><a:bodyPr/><a:p><a:r><a:t>Simplify</a:t></a:r></a:p></a:txBody></a:tc></a:tr></a:tbl></a:graphicData></a:graphic></p:graphicFrame>"""),
            ["ppt/slides/_rels/slide2.xml.rels"] = $"""<?xml version="1.0" encoding="UTF-8"?><Relationships xmlns="{Rel}"/>""",
        };
        return Zip(files);
    }

    public static byte[] Zip(Dictionary<string, object> files)
    {
        var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
            foreach (var (name, content) in files)
            {
                using var s = zip.CreateEntry(name).Open();
                s.Write(content is byte[] b ? b : Encoding.UTF8.GetBytes((string)content));
            }
        return ms.ToArray();
    }
}
