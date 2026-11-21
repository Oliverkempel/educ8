using System.Collections.Concurrent;
using CommunityToolkit.Mvvm.Input;
using Educ8.Documents;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>
/// itslearning Links (learning tool 5010) open their real target directly, skipping itslearning's wrapper page. A link whose
/// teacher wrote a description first shows it on a small page with "Open link". Targets are read once per session.
/// </summary>
public sealed class LinkModule(LearningToolContentReader content) : IElementModule
{
    private readonly ConcurrentDictionary<int, LinkContent> _links = new();

    public bool Handles(ElementRef element) => element.IsTool(LearningTools.Link);

    public OpenStyle Style => OpenStyle.Browser;

    public async Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, CancellationToken ct)
    {
        if (!_links.TryGetValue(element.ElementId, out var link))
            _links[element.ElementId] = link = await content.ReadLinkAsync(element.ElementId, ct);
        if (link.Url is not { } target || target.Scheme is not ("http" or "https" or "mailto"))
            throw new ContentUnavailableException("The link has no web address to open.");

        var description = HtmlToRichText.Convert(link.DescriptionHtml, navigator.Client().BaseAddress);
        if (description.IsEmpty)
        {
            navigator.OpenUrl.Open(target);
            return null;
        }
        return new LinkViewModel(element, navigator, target, description);
    }
}

/// <summary>A link with the teacher's description, and "Open link" for its target.</summary>
public sealed partial class LinkViewModel : ElementPageViewModel
{
    internal LinkViewModel(ElementRef element, ElementNavigator navigator, Uri target, RichDocument description)
        : base(element, navigator)
    {
        Target = target;
        Description = description;
    }

    public Uri Target { get; }

    /// <summary>Where the link goes, without its query (which can carry tokens): "example.com/path".</summary>
    public string TargetText => Target.Scheme == "mailto" ? Target.OriginalString["mailto:".Length..] : Target.Host + Target.AbsolutePath.TrimEnd('/');

    public RichDocument Description { get; }

    protected override bool HasContent => true;

    protected override Task LoadAsync(CancellationToken ct) => Task.CompletedTask; // read when the link was opened

    [RelayCommand]
    private void OpenTarget() => Navigator.OpenUrl.Open(Target);
}
