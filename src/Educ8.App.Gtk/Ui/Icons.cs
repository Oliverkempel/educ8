using Educ8.App.Core.Presentation;

namespace Educ8.App.Gtk.Ui;

/// <summary>AppIcon → GNOME (Adwaita) symbolic icon names. Keep complete: every AppIcon must map (see docs/design/desktop-parity.md).</summary>
internal static class Icons
{
    public static string Name(AppIcon icon) => icon switch
    {
        AppIcon.Courses => "view-grid-symbolic",
        AppIcon.Calendar => "x-office-calendar-symbolic",
        AppIcon.Refresh => "view-refresh-symbolic",
        AppIcon.Search => "system-search-symbolic",
        AppIcon.SignOut => "system-log-out-symbolic",
        AppIcon.OpenExternal => "web-browser-symbolic",
        AppIcon.OpenInApp => "go-next-symbolic",
        AppIcon.Video => "video-x-generic-symbolic",
        AppIcon.Audio => "audio-x-generic-symbolic",
        AppIcon.Image => "image-x-generic-symbolic",
        AppIcon.Embed => "application-x-addon-symbolic",
        AppIcon.Table => "x-office-spreadsheet-symbolic",
        AppIcon.CancelDownload => "process-stop-symbolic",
        AppIcon.Notification => "preferences-system-notifications-symbolic",
        AppIcon.Messages => "mail-unread-symbolic",
        AppIcon.Person => "avatar-default-symbolic",
        AppIcon.Group => "system-users-symbolic",
        AppIcon.NewMessage => "mail-message-new-symbolic",
        AppIcon.Send => "mail-send-symbolic",
        AppIcon.Attachment => "mail-attachment-symbolic",
        AppIcon.PreviousWeek => "go-previous-symbolic",
        AppIcon.NextWeek => "go-next-symbolic",
        AppIcon.Today => "find-location-symbolic",
        AppIcon.Event => "alarm-symbolic",
        AppIcon.Folder => "folder-symbolic",
        AppIcon.Page => "text-x-generic-symbolic",
        AppIcon.File => "x-office-document-symbolic",
        AppIcon.Link => "insert-link-symbolic",
        AppIcon.Assignment => "document-edit-symbolic",
        AppIcon.Test => "checkbox-checked-symbolic",
        AppIcon.Survey => "view-list-bullet-symbolic",
        AppIcon.Activity => "object-select-symbolic",
        AppIcon.LearningPath => "view-list-ordered-symbolic",
        AppIcon.Other => "text-x-generic-symbolic",
        AppIcon.Announcement => "preferences-system-notifications-symbolic",
        AppIcon.Task => "task-due-symbolic",
        AppIcon.RecentChange => "document-open-recent-symbolic",
        AppIcon.Pinned => "view-pin-symbolic",
        AppIcon.Mcp => "network-server-symbolic",
        AppIcon.CallOk => "object-select-symbolic",
        AppIcon.CallFailed => "dialog-error-symbolic",
        AppIcon.CallBlocked => "action-unavailable-symbolic",
        _ => "text-x-generic-symbolic",
    };
}
