using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.ViewModels;

/// <summary>
/// The Messages page: itslearning's conversations, most recently active first, and the unread count for the sidebar.
/// Opening a conversation shows it (<see cref="ConversationOpened"/>) and marks it read on itslearning; replies and new
/// conversations are sent from the app (the user chose read, reply and new conversations, 2026-11-21).
/// </summary>
public sealed partial class MessagesViewModel(Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, MessageAttachments attachments)
    : ObservableObject
{
    internal const int ThreadPageSize = 30;

    private CancellationTokenSource? _loading;
    private bool _loaded;
    private long? _nextSortIndex;
    private int? _myPersonId;

    public ObservableCollection<ThreadItemViewModel> Threads { get; } = [];

    /// <summary>Raised with a conversation to show (opened from the list, or just started); the platform pushes it.</summary>
    public event EventHandler<ConversationViewModel>? ConversationOpened;

    /// <summary>Raised with a blank "new conversation" form; the platform pushes it.</summary>
    public event EventHandler<NewConversationViewModel>? NewConversationStarted;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>More (older) conversations can be loaded.</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadMoreCommand))]
    public partial bool HasMore { get; private set; }

    /// <summary>Conversations with unread messages (the sidebar badge); 0 hides it.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UnreadText), nameof(HasUnread))]
    public partial int UnreadCount { get; private set; }

    public bool HasUnread => UnreadCount > 0;

    public string UnreadText => UnreadCount switch { <= 0 => "", > 99 => "99+", _ => UnreadCount.ToString(CultureInfo.CurrentCulture) };

    public string? EmptyText => _loaded && !IsLoading && ErrorMessage is null && Threads.Count == 0 ? Strings.MessagesNone : null;

    /// <summary>Reloads the first page of conversations and the unread count.</summary>
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
            var c = client();
            _myPersonId ??= (await c.GetCurrentPersonAsync(cts.Token)).PersonId;
            var page = await c.GetMessageThreadsAsync(null, ThreadPageSize, cts.Token);
            if (cts.IsCancellationRequested) return;
            Threads.Clear();
            Add(page);
            await RefreshUnreadCountAsync();
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
            OnPropertyChanged(nameof(EmptyText));
        }
    }

    [RelayCommand(CanExecute = nameof(HasMore))]
    private async Task LoadMoreAsync()
    {
        try
        {
            Add(await client().GetMessageThreadsAsync(_nextSortIndex, ThreadPageSize));
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = Texts.Error(e);
        }
    }

    private void Add(MessageThreadPage page)
    {
        var now = time.GetUtcNow();
        foreach (var t in page.Threads.EntityArray)
            if (Threads.All(x => x.Id != t.InstantMessageThreadId))
                Threads.Add(new ThreadItemViewModel(t, _myPersonId, now, time.LocalTimeZone));
        _nextSortIndex = page.Threads.EntityArray.Count > 0 ? page.Threads.EntityArray[^1].SortIndex : null;
        HasMore = page.HasMore && _nextSortIndex is not null;
    }

    /// <summary>Updates <see cref="UnreadCount"/> only (the shell calls it after sign-in and every few minutes). Errors are ignored.</summary>
    public async Task RefreshUnreadCountAsync()
    {
        try
        {
            UnreadCount = await client().GetUnreadThreadCountAsync();
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
        }
    }

    [RelayCommand]
    private void Open(ThreadItemViewModel thread)
    {
        var conversation = new ConversationViewModel(this, client, time, openUrl, attachments, thread.Id, thread.Title, thread.Thread, thread.LastReadId, _myPersonId);
        ConversationOpened?.Invoke(this, conversation);
        _ = conversation.LoadAsync();
    }

    [RelayCommand]
    private void NewConversation() =>
        NewConversationStarted?.Invoke(this, new NewConversationViewModel(this, client));

    /// <summary>Shows a conversation that was just started (from the reply of the send call).</summary>
    internal void ShowStarted(MessageThread thread)
    {
        Open(new ThreadItemViewModel(thread, _myPersonId, time.GetUtcNow(), time.LocalTimeZone));
        _ = RefreshAsync();
    }

    /// <summary>A conversation was read on itslearning: update its row and the badge.</summary>
    internal async Task MarkedReadAsync(int threadId, long lastMessageId)
    {
        if (Threads.FirstOrDefault(t => t.Id == threadId) is { } item)
        {
            item.IsUnread = false;
            item.LastReadId = Math.Max(item.LastReadId, lastMessageId);
        }
        await RefreshUnreadCountAsync();
    }

    /// <summary>A message was sent: reload the list so the conversation moves to the top.</summary>
    internal Task SentAsync() => RefreshAsync();

    public void Reset()
    {
        _loading?.Cancel();
        _loaded = false;
        _myPersonId = null;
        _nextSortIndex = null;
        Threads.Clear();
        ErrorMessage = null;
        HasMore = false;
        UnreadCount = 0;
    }
}

/// <summary>One conversation in the list.</summary>
public sealed partial class ThreadItemViewModel : ObservableObject
{
    internal ThreadItemViewModel(MessageThread t, int? me, DateTimeOffset now, TimeZoneInfo zone)
    {
        Thread = t;
        Id = t.InstantMessageThreadId;
        var others = t.Participants.Where(p => p.PersonId != me).Select(p => p.FullName?.Trim()).OfType<string>().Where(n => n.Length > 0).ToList();
        Title = !string.IsNullOrWhiteSpace(t.Name) ? t.Name.Trim() : others.Count > 0 ? string.Join(", ", others) : Strings.MessagesUntitled;
        if (t.LastMessage is { } last)
        {
            var sender = last.CreatedBy == me ? Strings.MessagesYou : last.CreatedByName?.Trim();
            var text = FirstLine(last.Text);
            // No sender prefix when the title already names them (one-to-one chats, broadcasts named after the sender).
            PreviewText = string.IsNullOrEmpty(sender) || string.Equals(sender, Title, StringComparison.CurrentCultureIgnoreCase)
                ? text
                : string.Format(CultureInfo.CurrentCulture, Strings.MessagesPreview, sender, text);
            TimeText = last.Created is { } created ? Texts.Date(created, now, zone) : "";
        }
        IsUnread = t.IsUnread;
        LastReadId = t.LastReadInstantMessageId;
        CanReply = t.CanReply;
        Icon = t.Participants.Count > 2 || t.Type is "Group" or "Course" or "Project" or "Collaboration" ? AppIcon.Group : AppIcon.Person;
    }

    internal MessageThread Thread { get; }

    /// <summary>The newest message read on itslearning (updated when the conversation is marked read here).</summary>
    internal long LastReadId { get; set; }
    public int Id { get; }
    public string Title { get; }

    /// <summary>"Ann Teacher: See you tomorrow" (first line of the newest message).</summary>
    public string PreviewText { get; } = "";
    public string TimeText { get; } = "";
    public bool CanReply { get; }
    public AppIcon Icon { get; }

    [ObservableProperty]
    public partial bool IsUnread { get; internal set; }

    internal static string FirstLine(string? text) =>
        (text ?? "").Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault() ?? "";
}

/// <summary>
/// One conversation: messages oldest first (older ones on request), the reply box, and marking read on itslearning when it opens.
/// </summary>
public sealed partial class ConversationViewModel : ObservableObject
{
    internal const int PageSize = 30;

    private readonly MessagesViewModel _parent;
    private readonly Func<ItslearningClient> _client;
    private readonly TimeProvider _time;
    private readonly IOpenUrl _openUrl;
    private readonly MessageAttachments _attachments;
    private readonly int? _me;
    private long _lastRead;

    internal ConversationViewModel(MessagesViewModel parent, Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl,
        MessageAttachments attachments, int threadId, string title, MessageThread thread, long lastRead, int? me)
    {
        _parent = parent;
        _client = client;
        _time = time;
        _openUrl = openUrl;
        _attachments = attachments;
        _me = me;
        _lastRead = lastRead;
        ThreadId = threadId;
        Title = title;
        var others = thread.Participants.Where(p => p.PersonId != me).Select(p => p.FullName?.Trim()).OfType<string>().Where(n => n.Length > 0).ToList();
        ParticipantsText = others.Count switch
        {
            0 => "",
            1 when string.Equals(others[0], title, StringComparison.CurrentCultureIgnoreCase) => "", // the title already says who
            <= 3 => string.Join(", ", others),
            _ => string.Format(CultureInfo.CurrentCulture, Strings.TeachersMore, string.Join(", ", others.Take(2)), others.Count - 2),
        };
        CanReply = thread.CanReply;
    }

    public int ThreadId { get; }
    public string Title { get; }
    public string ParticipantsText { get; }

    /// <summary>Oldest first.</summary>
    public ObservableCollection<MessageItemViewModel> Messages { get; } = [];

    public bool CanReply { get; }

    /// <summary>Shown instead of the reply box when the thread doesn't take replies (teacher broadcasts).</summary>
    public string? ReplyDisabledText => CanReply ? null : Strings.MessagesRepliesOff;

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadOlderCommand))]
    public partial bool HasOlder { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    public partial string Draft { get; set; } = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    public partial bool IsSending { get; private set; }

    /// <summary>Raised after messages were added at the end (loaded or sent), so the platform can scroll to the newest.</summary>
    public event EventHandler? NewestShown;

    /// <summary>Loads the newest page and marks the conversation read on itslearning.</summary>
    internal async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var page = await _client().GetThreadMessagesAsync(ThreadId, null, PageSize);
            Messages.Clear();
            foreach (var m in page.Messages.EntityArray.Reverse()) Messages.Add(Item(m));
            HasOlder = page.HasMore;
            NewestShown?.Invoke(this, EventArgs.Empty);
            await MarkReadAsync();
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = Texts.Error(e);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand(CanExecute = nameof(HasOlder))]
    private async Task LoadOlderAsync()
    {
        if (Messages.Count == 0) return;
        try
        {
            var page = await _client().GetThreadMessagesAsync(ThreadId, Messages[0].Id, PageSize);
            foreach (var m in page.Messages.EntityArray.Where(m => Messages.All(x => x.Id != m.MessageId))) // newest first → insert each at the top
                Messages.Insert(0, Item(m));
            HasOlder = page.HasMore;
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = Texts.Error(e);
        }
    }

    private bool CanSend() => CanReply && !IsSending && Draft.Trim().Length > 0;

    /// <summary>Sends <see cref="Draft"/> to the conversation (visible to its participants), then reloads it.</summary>
    [RelayCommand(CanExecute = nameof(CanSend))]
    private async Task SendAsync()
    {
        IsSending = true;
        ErrorMessage = null;
        try
        {
            await _client().SendMessageAsync(ThreadId, null, Draft.Trim());
            Draft = "";
            await LoadAsync();
            _ = _parent.SentAsync();
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.MessagesSendFailed, Texts.Error(e)); // the draft stays
        }
        finally
        {
            IsSending = false;
        }
    }

    private async Task MarkReadAsync()
    {
        if (Messages.Count == 0 || Messages[^1].Id <= _lastRead) return;
        try
        {
            await _client().MarkThreadReadAsync(ThreadId, Messages[^1].Id);
            _lastRead = Messages[^1].Id;
            await _parent.MarkedReadAsync(ThreadId, _lastRead);
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.MessagesMarkReadFailed, Texts.Error(e));
        }
    }

    private MessageItemViewModel Item(InstantMessage m) =>
        new(m, m.CreatedBy == _me, _time.GetUtcNow(), _time.LocalTimeZone, _client, _openUrl, _attachments);
}

/// <summary>One message in a conversation.</summary>
public sealed partial class MessageItemViewModel : ObservableObject
{
    private readonly MessageAttachments _attachments;
    private readonly IOpenUrl _openUrl;
    private readonly Func<ItslearningClient> _client;
    private readonly string? _attachmentUrl;

    internal MessageItemViewModel(InstantMessage m, bool mine, DateTimeOffset now, TimeZoneInfo zone, Func<ItslearningClient> client, IOpenUrl openUrl,
        MessageAttachments attachments)
    {
        _client = client;
        _openUrl = openUrl;
        _attachments = attachments;
        _attachmentUrl = m.AttachmentUrl;
        Id = m.MessageId;
        IsMine = mine;
        SenderName = mine ? Strings.MessagesYou : m.CreatedByName?.Trim() ?? "";
        TimeText = m.Created is { } created ? Texts.Date(created, now, zone) : "";
        Text = m.IsDeleted ? Strings.MessagesDeleted : (m.Text ?? "").Replace("\r\n", "\n").Trim();
        IsSystem = m.IsSystemMessage;
        AttachmentName = string.IsNullOrWhiteSpace(m.AttachmentUrl) ? null : m.AttachmentName?.Trim() is { Length: > 0 } n ? n : Strings.MessagesAttachment;
    }

    public long Id { get; }
    public bool IsMine { get; }
    public bool IsSystem { get; }
    public string SenderName { get; }
    public string TimeText { get; }
    public string Text { get; }

    /// <summary>Set when the message has an attachment.</summary>
    public string? AttachmentName { get; }
    public bool HasAttachment => AttachmentName is not null;

    [ObservableProperty]
    public partial string? AttachmentError { get; private set; }

    /// <summary>Opens the attachment in the default app (downloaded with the user's login from the itslearning site), else the browser.</summary>
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task OpenAttachmentAsync(CancellationToken ct)
    {
        if (_attachmentUrl is null) return;
        AttachmentError = null;
        try
        {
            await _attachments.OpenAsync(_client(), _attachmentUrl, AttachmentName, Id, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or IOException or NotAuthenticatedException or TaskCanceledException)
        {
            AttachmentError = string.Format(CultureInfo.CurrentCulture, Strings.OpenFileFailed, Texts.Error(e));
            if (Uri.TryCreate(_attachmentUrl, UriKind.Absolute, out var url)) _openUrl.Open(url);
        }
    }
}

/// <summary>Starting a conversation: search people by name, pick one or more, write the first message.</summary>
public sealed partial class NewConversationViewModel : ObservableObject
{
    internal const int MaxResults = 25;

    private readonly MessagesViewModel _parent;
    private readonly Func<ItslearningClient> _client;
    private CancellationTokenSource? _search;

    internal NewConversationViewModel(MessagesViewModel parent, Func<ItslearningClient> client)
    {
        _parent = parent;
        _client = client;
        Recipients.CollectionChanged += (_, _) => SendCommand.NotifyCanExecuteChanged();
    }

    /// <summary>Search results (people who can receive, not already picked).</summary>
    public ObservableCollection<RecipientItemViewModel> Results { get; } = [];

    /// <summary>The people the message goes to.</summary>
    public ObservableCollection<RecipientItemViewModel> Recipients { get; } = [];

    [ObservableProperty]
    public partial string SearchText { get; set; } = "";

    [ObservableProperty]
    public partial bool IsSearching { get; private set; }

    /// <summary>"No one found" after a search with no results; null otherwise.</summary>
    [ObservableProperty]
    public partial string? SearchStatus { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    public partial string Draft { get; set; } = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    public partial bool IsSending { get; private set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Raised when the conversation was sent (the platform closes this form; the conversation opens).</summary>
    public event EventHandler? Sent;

    /// <summary>Searches for <see cref="SearchText"/> (platforms call it as the user types; at least 2 characters).</summary>
    [RelayCommand]
    private async Task SearchAsync()
    {
        _search?.Cancel();
        var cts = _search = new CancellationTokenSource();
        var text = SearchText.Trim();
        if (text.Length < 2)
        {
            Results.Clear();
            SearchStatus = null;
            return;
        }
        IsSearching = true;
        try
        {
            var found = await _client().SearchMessageRecipientsAsync(text, cts.Token);
            if (cts.IsCancellationRequested) return;
            Results.Clear();
            // itslearning returns up to 100 matches (live: "anna" → 100); the first few are enough, typing more narrows it.
            foreach (var r in found.Where(r => Recipients.All(x => x.Id != r.Id)).Take(MaxResults)) Results.Add(new RecipientItemViewModel(r));
            SearchStatus = Results.Count == 0 ? Strings.MessagesNoOneFound : null;
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            SearchStatus = Texts.Error(e);
        }
        finally
        {
            if (_search == cts) IsSearching = false;
        }
    }

    [RelayCommand]
    private void Add(RecipientItemViewModel person)
    {
        if (Recipients.Any(r => r.Id == person.Id)) return;
        Recipients.Add(person);
        Results.Remove(person);
        SearchText = "";
        Results.Clear();
        SearchStatus = null;
    }

    [RelayCommand]
    private void Remove(RecipientItemViewModel person) => Recipients.Remove(person);

    private bool CanSend() => Recipients.Count > 0 && Draft.Trim().Length > 0 && !IsSending;

    /// <summary>Sends the first message (itslearning reuses an existing conversation with the same people) and opens the conversation.</summary>
    [RelayCommand(CanExecute = nameof(CanSend))]
    private async Task SendAsync()
    {
        IsSending = true;
        ErrorMessage = null;
        try
        {
            var thread = await _client().SendMessageAsync(null, Recipients.Select(r => r.Id).ToList(), Draft.Trim());
            Sent?.Invoke(this, EventArgs.Empty);
            _parent.ShowStarted(thread);
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.MessagesSendFailed, Texts.Error(e));
        }
        finally
        {
            IsSending = false;
        }
    }
}

public sealed class RecipientItemViewModel(MessageRecipient r)
{
    public int Id { get; } = r.Id;
    public string Name { get; } = r.Label?.Trim() ?? "";
    public string Detail { get; } = r.Organization?.Trim() ?? "";
}
