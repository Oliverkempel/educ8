using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>
/// A row for a course element (file, page, folder, assignment, …). How it opens comes from its module
/// (<see cref="ElementNavigator"/>): in the app, in the default application (files), or in the browser.
/// Create with <see cref="ElementNavigator.Item"/>.
/// </summary>
public sealed partial class MaterialItemViewModel : ObservableObject
{
    private readonly ElementNavigator _navigator;

    internal MaterialItemViewModel(ElementRef element, ElementNavigator navigator, string? detail = null)
    {
        _navigator = navigator;
        Element = element;
        KindText = Texts.Kind(element.Kind);
        SubtitleText = string.IsNullOrEmpty(detail) ? KindText : $"{KindText} · {detail}";
        Icon = AppIcons.For(element.Kind);
        OpenStyle = navigator.StyleOf(element);
        CanSaveCopy = navigator.ModuleFor(element) is ISavesCopies;
    }

    /// <summary>True for files: besides opening, a copy can be saved where the user chooses.</summary>
    public bool CanSaveCopy { get; }

    /// <summary>How far a running download got (0–1), or null when unknown (show an indeterminate bar).</summary>
    [ObservableProperty]
    public partial double? DownloadFraction { get; private set; }

    /// <summary>"Saved to Downloads" after a copy was saved (shown in place of the subtitle).</summary>
    [ObservableProperty]
    public partial string? NoticeText { get; private set; }

    /// <summary>Downloads the file if needed and asks where to save a copy. <c>SaveCopyCancelCommand</c> stops the download.</summary>
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task SaveCopyAsync(CancellationToken ct)
    {
        ErrorMessage = NoticeText = null;
        DownloadFraction = null;
        if (_navigator.ModuleFor(Element) is not ISavesCopies module) return;
        try
        {
            if (await module.SaveCopyAsync(Element, new Progress<double>(p => DownloadFraction = p), ct) is { } saved) NoticeText = Texts.SavedTo(saved);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            ErrorMessage = string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.SaveFailed, Texts.Error(e));
        }
    }

    public ElementRef Element { get; }
    public int ElementId => Element.ElementId;
    public string Title => Element.Title;
    public ResourceKind Kind => Element.Kind;
    public string KindText { get; }

    /// <summary>The row's second line: the kind, plus any detail ("Assignment · Due Fri 20 Nov, 23:59").</summary>
    public string SubtitleText { get; }
    public AppIcon Icon { get; }
    public Uri? Url => Element.Url;

    public OpenStyle OpenStyle { get; }
    public bool OpensInApp => OpenStyle == OpenStyle.InApp;

    /// <summary>True for files, which open in a local application rather than the browser.</summary>
    public bool OpensLocally => OpenStyle == OpenStyle.LocalFile;

    /// <summary>False when there's nothing to open (no module and no link).</summary>
    public bool CanOpen => OpenStyle != OpenStyle.Browser || Url is not null;

    /// <summary>The row's trailing icon: a chevron for in-app pages, the external icon otherwise.</summary>
    public AppIcon OpenIcon => OpensInApp ? AppIcon.OpenInApp : AppIcon.OpenExternal;

    public string OpenTooltip => OpenStyle switch
    {
        OpenStyle.InApp => Strings.OpenHere,
        OpenStyle.LocalFile => Strings.OpenFile,
        _ => Strings.OpenInItslearning,
    };

    /// <summary>Shown while <see cref="OpenCommand"/> runs ("Downloading…" for files).</summary>
    public string BusyText => OpensLocally ? Strings.Downloading : Strings.Opening;

    /// <summary>Set when opening failed (the item was opened in itslearning instead).</summary>
    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Opens the item. While it runs (a file downloading), <c>OpenCancelCommand</c> stops it (nothing opens, no partial file stays).</summary>
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task OpenAsync(CancellationToken ct)
    {
        ErrorMessage = NoticeText = null;
        DownloadFraction = null;
        ErrorMessage = await _navigator.OpenAsync(Element, new Progress<double>(p => DownloadFraction = p), ct);
    }
}
