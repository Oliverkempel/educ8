namespace Educ8.Documents.Tests;

/// <summary>Author HTML → native rich text. Snippets follow itslearning's real markup (from the scrubbed page fixtures).</summary>
public class HtmlToRichTextTests
{
    private static readonly Uri Site = new("https://school.itslearning.com/");

    private static RichParagraph Paragraph(RichDocument doc, int index = 0) => Assert.IsType<RichParagraph>(doc.Blocks[index]);

    [Fact]
    public void Nested_bold_links_become_one_bold_link_and_weight_400_stays_normal()
    {
        const string html = """
            <p><span style="color:rgb(51, 51, 51);font-style:normal;font-weight:400;">Read about </span><span style="font-weight:400;"><strong><a target="_blank" href="https://portal.example.edu/data">collection of&nbsp;</a></strong></span><a target="_blank" href="https://portal.example.edu/data"><strong style="font-weight:bold;"><strong>personal data</strong>.</strong></a></p>
            """;
        var p = Paragraph(HtmlToRichText.Convert(html));
        Assert.Equal(2, p.Spans.Count);
        Assert.Equal(new RichSpan("Read about "), p.Spans[0]);
        Assert.Equal(new RichSpan("collection of personal data.", RichStyle.Bold, new Uri("https://portal.example.edu/data")), p.Spans[1]);
    }

    [Fact]
    public void Styles_from_tags_and_style_attributes()
    {
        var p = Paragraph(HtmlToRichText.Convert("""<p><em>a</em> <span style="font-weight: 700">b</span> <u>c</u> <s>d</s> <code>e</code> H<sub>2</sub>O x<sup>2</sup></p>"""));
        Assert.Equal(
            [("a", RichStyle.Italic), (" ", RichStyle.None), ("b", RichStyle.Bold), (" ", RichStyle.None), ("c", RichStyle.Underline),
             (" ", RichStyle.None), ("d", RichStyle.Strikethrough), (" ", RichStyle.None), ("e", RichStyle.Code), (" H", RichStyle.None),
             ("2", RichStyle.Subscript), ("O x", RichStyle.None), ("2", RichStyle.Superscript)],
            p.Spans.Select(s => (s.Text, s.Style)));
    }

    [Fact]
    public void Empty_paragraphs_vanish_and_line_breaks_stay()
    {
        var doc = HtmlToRichText.Convert("<p>&nbsp;</p><p>One<br>two  <br/> three</p><p> </p>");
        Assert.Equal("One\ntwo\nthree", Assert.Single(Paragraph(doc).Spans).Text);
        Assert.Single(doc.Blocks);
    }

    [Fact]
    public void An_image_splits_its_paragraph_and_keeps_alt_text()
    {
        var doc = HtmlToRichText.Convert("""<p><img align="left" src="https://filerepository.itslearning.com/abc?Token=x" alt="Logo">Guides for students</p>""");
        var image = Assert.IsType<RichImage>(doc.Blocks[0]);
        Assert.Equal(("Logo", "filerepository.itslearning.com"), (image.Alt, image.Source!.Host));
        Assert.Equal("Guides for students", Assert.Single(Paragraph(doc, 1).Spans).Text);
    }

    [Fact]
    public void Videos_embeds_and_interactive_content_become_placeholders()
    {
        var doc = HtmlToRichText.Convert("""
            <p><strong>Video 1</strong></p>
            <oembed>https://www.youtube.com/watch?v=abc</oembed>
            <iframe src="https://school.h5p.com/content/1/embed"></iframe>
            <iframe src="https://example.com/widget"></iframe>
            <video><source src="https://example.com/v.mp4"></video>
            """);
        Assert.Equal("Video 1", Paragraph(doc).Spans[0].Text);
        Assert.Equal(
            [(RichUnsupportedKind.Video, "https://www.youtube.com/watch?v=abc"), (RichUnsupportedKind.Interactive, "https://school.h5p.com/content/1/embed"),
             (RichUnsupportedKind.Embed, "https://example.com/widget"), (RichUnsupportedKind.Video, "https://example.com/v.mp4")],
            doc.Blocks.Skip(1).Cast<RichUnsupported>().Select(u => (u.Kind, u.Link?.ToString())));
    }

    [Fact]
    public void Headings_lists_quotes_code_and_rules()
    {
        var doc = HtmlToRichText.Convert("""
            <h2>Week <em>1</em></h2>
            <ol start="3"><li>Read <a href="/Course/Course.aspx?CourseID=10">the notes</a><ul><li>chapter 1</li></ul></li><li>Exercises</li></ol>
            <blockquote><p>Quoted</p></blockquote>
            <pre>int x = 1;
              x++;</pre>
            <hr>
            """, Site);
        var heading = Assert.IsType<RichHeading>(doc.Blocks[0]);
        Assert.Equal((2, "Week ", RichStyle.Italic), (heading.Level, heading.Spans[0].Text, heading.Spans[1].Style));

        var list = Assert.IsType<RichList>(doc.Blocks[1]);
        Assert.True(list.Ordered);
        Assert.Equal(["3.", "4."], list.Items.Select(i => i.Marker));
        var first = list.Items[0];
        Assert.Equal("https://school.itslearning.com/Course/Course.aspx?CourseID=10", Assert.IsType<RichParagraph>(first.Blocks[0]).Spans[1].Link!.ToString());
        var nested = Assert.IsType<RichList>(first.Blocks[1]);
        Assert.Equal(("•", "chapter 1"), (nested.Items[0].Marker, Assert.IsType<RichParagraph>(nested.Items[0].Blocks[0]).Spans[0].Text));

        Assert.Equal("Quoted", Assert.IsType<RichParagraph>(Assert.IsType<RichQuote>(doc.Blocks[2]).Blocks[0]).Spans[0].Text);
        Assert.Equal("int x = 1;\n  x++;", Assert.IsType<RichCode>(doc.Blocks[3]).Text);
        Assert.IsType<RichRule>(doc.Blocks[4]);
    }

    [Fact]
    public void Unsafe_and_anchor_links_keep_their_text_only()
    {
        var p = Paragraph(HtmlToRichText.Convert("""<p><a href="javascript:alert(1)">x</a> <a href="#top">y</a> <a href="relative/page">z</a> <a href="mailto:a@example.com">m</a></p>"""));
        Assert.Equal([("x y z ", (Uri?)null), ("m", new Uri("mailto:a@example.com"))], p.Spans.Select(s => (s.Text, s.Link)));
    }

    [Fact]
    public void Simple_tables_are_kept_and_complex_ones_become_placeholders()
    {
        var simple = Assert.IsType<RichTable>(HtmlToRichText.Convert("""
            <table><thead><tr><th>Week</th><th>Topic</th></tr></thead>
            <tbody><tr><td>1</td><td><p>Intro</p><p><b>Read</b> ch. 1</p></td></tr></tbody></table>
            """).Blocks.Single());
        Assert.True(simple.HeaderRow);
        Assert.Equal(("Week", RichStyle.Bold), (simple.Rows[0][0][0].Text, simple.Rows[0][0][0].Style));
        Assert.Equal(["Intro", "\n", "Read", " ch. 1"], simple.Rows[1][1].Select(s => s.Text));

        var merged = HtmlToRichText.Convert("""<table><tr><td colspan="2">x</td></tr></table>""").Blocks.Single();
        Assert.Equal(RichUnsupportedKind.Table, Assert.IsType<RichUnsupported>(merged).Kind);
        var withList = HtmlToRichText.Convert("""<table><tr><td><ul><li>x</li></ul></td></tr></table>""").Blocks.Single();
        Assert.IsType<RichUnsupported>(withList);
    }

    [Fact]
    public void Plain_text_becomes_paragraphs()
    {
        var doc = HtmlToRichText.Convert("First line\nstill first paragraph\n\n  Second   paragraph  ");
        Assert.Equal(["First line\nstill first paragraph", "Second paragraph"], doc.Blocks.Cast<RichParagraph>().Select(p => p.Spans[0].Text));
        Assert.True(HtmlToRichText.Convert("  ").IsEmpty);
        Assert.True(HtmlToRichText.Convert(null).IsEmpty);
    }
}
