using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// The Messages page: conversations as a boxed list (unread ones with an accent dot); a conversation and the "new conversation"
/// form are pushed on top. Header: new conversation, refresh.
/// </summary>
internal sealed class MessagesView
{
    public Widget Root { get; }

    /// <summary>The navigation stack (list → conversation / new conversation); exposed for the screenshot aid.</summary>
    public Adw.NavigationView Navigation { get; }

    public MessagesView(MessagesViewModel vm)
    {
        var compose = IconButton(Icons.Name(AppIcon.NewMessage), Strings.MessagesNew);
        Bind.Command(compose, vm.NewConversationCommand);
        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);
        var header = Adw.HeaderBar.New();
        header.PackStart(compose);
        header.PackEnd(refresh);

        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetValign(Align.Start);
        Bind.Collection(vm.Threads, () =>
        {
            list.RemoveAll();
            foreach (var t in vm.Threads) list.Append(ThreadRow(t, vm));
        });

        var more = Button.NewWithLabel(Strings.MessagesLoadMore);
        more.AddCssClass("pill");
        more.SetHalign(Align.Center);
        Bind.Command(more, vm.LoadMoreCommand);
        Bind.Visible(more, vm, nameof(MessagesViewModel.HasMore), () => vm.HasMore);

        var page = VBox(18, list, more);
        page.SetMarginTop(18);
        page.SetMarginBottom(24);
        page.SetMarginStart(18);
        page.SetMarginEnd(18);

        var empty = Adw.StatusPage.New();
        empty.SetIconName(Icons.Name(AppIcon.Messages));
        empty.SetTitle(Strings.MessagesNone);

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(empty, "empty");
        states.AddNamed(Scrolled(Clamp(page, 900)), "list");
        Bind.Property(vm, () => states.SetVisibleChildName(
                vm.IsLoading && vm.Threads.Count == 0 ? "loading" : vm.EmptyText is not null ? "empty" : "list"),
            nameof(MessagesViewModel.IsLoading), nameof(MessagesViewModel.EmptyText));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(VBox(0, Bind.ErrorBanner(vm, nameof(MessagesViewModel.ErrorMessage), () => vm.ErrorMessage, vm.RefreshCommand), states));

        var navigation = Adw.NavigationView.New();
        navigation.Add(Adw.NavigationPage.New(toolbar, Strings.NavMessages));
        vm.ConversationOpened += (_, c) => navigation.Push(Conversation(c));
        vm.NewConversationStarted += (_, form) => navigation.Push(NewConversation(form, navigation));
        Navigation = navigation;
        Root = navigation;
    }

    private static Adw.ActionRow ThreadRow(ThreadItemViewModel t, MessagesViewModel vm)
    {
        var row = Row(t.Title, t.PreviewText);
        row.SetSubtitleLines(1);
        row.AddPrefix(Image.NewFromIconName(Icons.Name(t.Icon)));
        var unread = Image.NewFromIconName("media-record-symbolic");
        unread.AddCssClass("accent");
        unread.SetTooltipText(Strings.MessagesUnread);
        Bind.Property(t, () => unread.SetVisible(t.IsUnread), nameof(ThreadItemViewModel.IsUnread));
        row.AddSuffix(Label(t.TimeText, "caption", "dim-label"));
        row.AddSuffix(unread);
        row.SetActivatable(true);
        row.OnActivated += (_, _) => vm.OpenCommand.Execute(t);
        return row;
    }

    /// <summary>A conversation: messages (oldest at the top, scrolled to the newest), "show older", and the reply box at the bottom.</summary>
    private static Adw.NavigationPage Conversation(ConversationViewModel c)
    {
        var older = Button.NewWithLabel(Strings.MessagesLoadOlder);
        older.AddCssClass("flat");
        older.SetHalign(Align.Center);
        Bind.Command(older, c.LoadOlderCommand);
        Bind.Visible(older, c, nameof(ConversationViewModel.HasOlder), () => c.HasOlder);

        var messages = VBox(12);
        Bind.Collection(c.Messages, () =>
        {
            while (messages.GetFirstChild() is { } child) messages.Remove(child);
            foreach (var m in c.Messages) messages.Append(MessageBubble(m));
        });

        var column = VBox(12, older, messages);
        column.SetMarginTop(18);
        column.SetMarginBottom(18);
        column.SetMarginStart(18);
        column.SetMarginEnd(18);
        var scrolled = Scrolled(Clamp(column, 800));
        // Newest at the bottom: scroll there once the new messages are laid out.
        c.NewestShown += (_, _) =>
        {
            var adjustment = scrolled.GetVadjustment();
            GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_LOW, () =>
            {
                adjustment.SetValue(adjustment.GetUpper() - adjustment.GetPageSize());
                return false;
            });
        };

        var loading = Spinner(32);
        Bind.Visible(loading, c, nameof(ConversationViewModel.IsLoading), () => c.IsLoading && c.Messages.Count == 0);
        var overlay = Overlay.New();
        overlay.SetChild(scrolled);
        overlay.AddOverlay(loading);

        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(Adw.WindowTitle.New(c.Title, c.ParticipantsText));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(VBox(0, Bind.ErrorBanner(c, nameof(ConversationViewModel.ErrorMessage), () => c.ErrorMessage, null), overlay));
        toolbar.AddBottomBar(ReplyBar(c));
        toolbar.SetBottomBarStyle(Adw.ToolbarStyle.Raised);
        overlay.SetVexpand(true);
        return Adw.NavigationPage.New(toolbar, c.Title);
    }

    private static Widget ReplyBar(ConversationViewModel c)
    {
        if (c.ReplyDisabledText is { } off)
        {
            var label = Label(off, "dim-label");
            label.SetXalign(0.5f);
            label.SetMarginTop(12);
            label.SetMarginBottom(12);
            return label;
        }
        var entry = Entry.New();
        entry.SetPlaceholderText(Strings.MessagesWrite);
        entry.SetHexpand(true);
        entry.OnChanged += (_, _) => { if (c.Draft != entry.GetText()) c.Draft = entry.GetText(); };
        Bind.Property(c, () => { if (entry.GetText() != c.Draft) entry.SetText(c.Draft); }, nameof(ConversationViewModel.Draft));
        entry.OnActivate += (_, _) => { if (c.SendCommand.CanExecute(null)) c.SendCommand.Execute(null); };
        Bind.Property(c, () => entry.SetSensitive(!c.IsSending), nameof(ConversationViewModel.IsSending));

        var send = IconButton(Icons.Name(AppIcon.Send), Strings.MessagesSend);
        send.AddCssClass("suggested-action");
        send.AddCssClass("circular");
        Bind.Command(send, c.SendCommand);

        var bar = HBox(6, entry, send);
        bar.SetMarginTop(6);
        bar.SetMarginBottom(6);
        bar.SetMarginStart(12);
        bar.SetMarginEnd(12);
        return Clamp(bar, 800);
    }

    /// <summary>One message as a card: sender and time, the text, and an attachment button. Own messages sit on the right.</summary>
    private static Widget MessageBubble(MessageItemViewModel m)
    {
        var sender = Label(m.SenderName, "caption-heading");
        var time = Label(m.TimeText, "caption", "dim-label");
        var text = WrapLabel(m.Text);
        text.SetSelectable(true);
        text.SetMaxWidthChars(60);
        if (m.IsSystem) text.AddCssClass("dim-label");
        var content = VBox(4, HBox(8, sender, time), text);

        if (m.AttachmentName is { } name)
        {
            var open = Button.New();
            open.AddCssClass("flat");
            open.SetHalign(Align.Start);
            var spinner = Spinner(16);
            open.SetChild(HBox(6, Image.NewFromIconName(Icons.Name(AppIcon.Attachment)), Label(name), spinner));
            Bind.Property(m.OpenAttachmentCommand, () => spinner.SetVisible(m.OpenAttachmentCommand.IsRunning), nameof(m.OpenAttachmentCommand.IsRunning));
            open.OnClicked += (_, _) =>
            {
                if (m.OpenAttachmentCommand.IsRunning) m.OpenAttachmentCancelCommand.Execute(null);
                else m.OpenAttachmentCommand.Execute(null);
            };
            var error = WrapLabel("", "caption", "error");
            Bind.Property(m, () => { error.SetText(m.AttachmentError ?? ""); error.SetVisible(m.AttachmentError is not null); },
                nameof(MessageItemViewModel.AttachmentError));
            content.Append(open);
            content.Append(error);
        }

        content.SetMarginTop(10);
        content.SetMarginBottom(10);
        content.SetMarginStart(12);
        content.SetMarginEnd(12);
        var card = VBox(0, content);
        card.AddCssClass("card");
        card.SetHalign(m.IsMine ? Align.End : Align.Start);
        return card;
    }

    /// <summary>The new conversation form: recipients (search people, add, remove), the message, Send in the header.</summary>
    private static Adw.NavigationPage NewConversation(NewConversationViewModel form, Adw.NavigationView navigation)
    {
        var recipients = ListBox.New();
        recipients.AddCssClass("boxed-list");
        recipients.SetSelectionMode(SelectionMode.None);
        Bind.Collection(form.Recipients, () =>
        {
            recipients.RemoveAll();
            foreach (var r in form.Recipients)
            {
                var row = Row(r.Name, r.Detail);
                row.AddPrefix(Image.NewFromIconName(Icons.Name(AppIcon.Person)));
                var remove = IconButton("list-remove-symbolic", Strings.MessagesRemoveRecipient);
                remove.AddCssClass("flat");
                remove.SetValign(Align.Center);
                remove.OnClicked += (_, _) => form.RemoveCommand.Execute(r);
                row.AddSuffix(remove);
                recipients.Append(row);
            }
            recipients.SetVisible(form.Recipients.Count > 0);
        });

        var search = SearchEntry.New();
        search.SetPlaceholderText(Strings.MessagesSearchPeople);
        search.OnSearchChanged += (_, _) =>
        {
            if (form.SearchText == search.GetText()) return;
            form.SearchText = search.GetText();
            form.SearchCommand.Execute(null);
        };
        Bind.Property(form, () => { if (search.GetText() != form.SearchText) search.SetText(form.SearchText); }, nameof(NewConversationViewModel.SearchText));

        var results = ListBox.New();
        results.AddCssClass("boxed-list");
        results.SetSelectionMode(SelectionMode.None);
        Bind.Collection(form.Results, () =>
        {
            results.RemoveAll();
            foreach (var r in form.Results)
            {
                var row = Row(r.Name, r.Detail);
                row.AddPrefix(Image.NewFromIconName(Icons.Name(AppIcon.Person)));
                row.AddSuffix(Image.NewFromIconName("list-add-symbolic"));
                row.SetActivatable(true);
                row.OnActivated += (_, _) => form.AddCommand.Execute(r);
                results.Append(row);
            }
            results.SetVisible(form.Results.Count > 0);
        });
        var status = Label("", "dim-label");
        Bind.Property(form, () => { status.SetText(form.SearchStatus ?? ""); status.SetVisible(form.SearchStatus is not null); },
            nameof(NewConversationViewModel.SearchStatus));

        var to = Adw.PreferencesGroup.New();
        to.SetTitle(Escape(Strings.MessagesTo));
        to.Add(VBox(12, recipients, search, results, status));

        var text = TextView.New();
        text.SetWrapMode(WrapMode.WordChar);
        text.SetTopMargin(10);
        text.SetBottomMargin(10);
        text.SetLeftMargin(12);
        text.SetRightMargin(12);
        text.GetBuffer().OnChanged += (_, _) =>
        {
            var buffer = text.GetBuffer();
            buffer.GetBounds(out var start, out var end);
            form.Draft = buffer.GetText(start, end, false);
        };
        var textScroll = ScrolledWindow.New();
        textScroll.SetChild(text);
        textScroll.SetMinContentHeight(140);
        textScroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        var textCard = Frame.New(null);
        textCard.SetChild(textScroll);

        var message = Adw.PreferencesGroup.New();
        message.SetTitle(Escape(Strings.MessagesWrite.TrimEnd('…')));
        message.Add(textCard);

        var error = WrapLabel("", "error");
        Bind.Property(form, () => { error.SetText(form.ErrorMessage ?? ""); error.SetVisible(form.ErrorMessage is not null); },
            nameof(NewConversationViewModel.ErrorMessage));
        var errors = Adw.PreferencesGroup.New();
        errors.Add(error);

        var content = Adw.PreferencesPage.New();
        content.Add(to);
        content.Add(message);
        content.Add(errors);

        var send = Button.NewWithLabel(Strings.MessagesSend);
        send.AddCssClass("suggested-action");
        Bind.Command(send, form.SendCommand);
        var header = Adw.HeaderBar.New();
        header.PackEnd(send);

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(content);
        var page = Adw.NavigationPage.New(toolbar, Strings.MessagesNew);
        form.Sent += (_, _) => navigation.Pop(); // the conversation opens next
        return page;
    }
}
