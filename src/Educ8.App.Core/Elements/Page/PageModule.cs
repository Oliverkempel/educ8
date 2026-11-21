using System.Collections.ObjectModel;
using Educ8.App.Core.Resources;
using Educ8.Documents;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>itslearning Pages (learning tool 5) open in the app: their text blocks as native rich text.</summary>
public sealed class PageModule(LearningToolContentReader content) : IElementModule
{
    public bool Handles(ElementRef element) => element.IsTool(LearningTools.Page);

    public OpenStyle Style => OpenStyle.InApp;

    public Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, CancellationToken ct) =>
        Task.FromResult<ElementPageViewModel?>(new ContentPageViewModel(element, navigator, content));
}

/// <summary>
/// An itslearning Page: its blocks in order, each with an optional title. Text blocks are rich text; other block kinds (files,
/// images, RSS, polls) are shown as a placeholder with "Open in itslearning".
/// </summary>
public sealed class ContentPageViewModel : ElementPageViewModel
{
    private readonly LearningToolContentReader _content;
    private bool _loaded;

    internal ContentPageViewModel(ElementRef element, ElementNavigator navigator, LearningToolContentReader content)
        : base(element, navigator)
    {
        _content = content;
    }

    public ObservableCollection<PageSectionViewModel> Sections { get; } = [];

    public override string? EmptyText => _loaded && Sections.All(s => s.IsSupported && s.Content.IsEmpty && s.Title is null) ? Strings.PageEmpty : null;

    protected override bool HasContent => Sections.Count > 0;

    protected override async Task LoadAsync(CancellationToken ct)
    {
        var page = await _content.ReadPageAsync(Element.ElementId, ct);
        ct.ThrowIfCancellationRequested();
        var site = Navigator.Client().BaseAddress;
        Sections.Clear();
        foreach (var block in page.Blocks)
        {
            var supported = string.Equals(block.Kind, "text", StringComparison.OrdinalIgnoreCase);
            Sections.Add(new PageSectionViewModel(string.IsNullOrWhiteSpace(block.Title) ? null : block.Title.Trim(),
                supported ? HtmlToRichText.Convert(block.Html, site) : RichDocument.Empty, supported));
        }
        _loaded = true;
    }
}

/// <summary>One block of a page.</summary>
public sealed record PageSectionViewModel(string? Title, RichDocument Content, bool IsSupported)
{
    /// <summary>Shown for blocks the app can't display (with "Open in itslearning").</summary>
    public string? UnsupportedText => IsSupported ? null : Strings.PageBlockUnsupported;
}
