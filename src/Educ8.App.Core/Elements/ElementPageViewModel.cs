using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;

namespace Educ8.App.Core.Elements;

/// <summary>What an element page shows right now.</summary>
public enum ElementPageState
{
    /// <summary>Loading for the first time (a spinner).</summary>
    Loading,

    /// <summary>An error or an empty state (<see cref="ElementPageViewModel.MessageText"/>, with "Open in itslearning").</summary>
    Message,

    Content,
}

/// <summary>
/// Base for in-app element pages: title, icon and link of the element, loading with cancellation (a refresh replaces a running
/// load), error and empty states, and "Open in itslearning". Subclasses load their content in <see cref="LoadAsync"/>.
/// </summary>
public abstract partial class ElementPageViewModel : ObservableObject
{
    private CancellationTokenSource? _loading;
    private bool _loaded;

    protected ElementPageViewModel(ElementRef element, ElementNavigator navigator)
    {
        Element = element;
        Navigator = navigator;
    }

    public ElementRef Element { get; }
    protected ElementNavigator Navigator { get; }

    public string Title => Element.Title;
    public string KindText => Texts.Kind(Element.Kind);
    public AppIcon Icon => AppIcons.For(Element.Kind);
    public Uri? Url => Element.Url;
    public bool HasUrl => Url is not null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(State))]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(State), nameof(MessageText))]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Shown instead of content once loaded with nothing to show; null otherwise.</summary>
    public virtual string? EmptyText => null;

    public string? MessageText => ErrorMessage ?? EmptyText;

    public ElementPageState State =>
        ErrorMessage is not null ? ElementPageState.Message
        : IsLoading && !HasContent ? ElementPageState.Loading
        : _loaded && !IsLoading && EmptyText is not null ? ElementPageState.Message
        : ElementPageState.Content;

    /// <summary>Whether content from an earlier load is still on screen (a refresh then doesn't show the spinner page).</summary>
    protected virtual bool HasContent => false;

    public Task EnsureLoadedAsync() => _loaded ? Task.CompletedTask : RefreshAsync();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        _loading?.Cancel();
        var cts = _loading = new CancellationTokenSource();
        _loaded = true;
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            await LoadAsync(cts.Token);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            if (!cts.IsCancellationRequested) ErrorMessage = Texts.Error(e);
        }
        finally
        {
            if (_loading == cts) IsLoading = false;
            ContentChanged();
        }
    }

    /// <summary>Loads the page's content; throwing shows the error (with "Open in itslearning").</summary>
    protected abstract Task LoadAsync(CancellationToken ct);

    /// <summary>Call when what <see cref="EmptyText"/> or <see cref="HasContent"/> depend on changed.</summary>
    protected void ContentChanged()
    {
        OnPropertyChanged(nameof(EmptyText));
        OnPropertyChanged(nameof(MessageText));
        OnPropertyChanged(nameof(State));
    }

    /// <summary>Opens a link from the page's text (itslearning elements in the app where possible, the rest in the browser).</summary>
    [RelayCommand]
    private Task OpenLinkAsync(Uri url) => Navigator.OpenLinkAsync(url);

    [RelayCommand(CanExecute = nameof(HasUrl))]
    private void OpenInItslearning()
    {
        if (Url is not null) Navigator.OpenUrl.Open(Url);
    }
}
