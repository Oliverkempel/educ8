using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Presentation;

/// <summary>
/// Every icon the app uses, by meaning. Each platform maps these to its own icon set:
/// GNOME symbolic icons (GTK), SF Symbols (AppKit), Segoe Fluent Icons (WinUI). Keep the three mappings complete.
/// </summary>
public enum AppIcon
{
    Courses,
    Calendar,
    Refresh,
    Search,
    SignOut,
    OpenExternal,
    PreviousWeek,
    NextWeek,
    Today,
    Event,
    Folder,
    Page,
    File,
    Link,
    Assignment,
    Test,
    Survey,
    Activity,
    LearningPath,
    Other,
    Announcement,
    Task,
    RecentChange,
    Pinned,
    Mcp,
    CallOk,
    CallFailed,
    CallBlocked,
}

public static class AppIcons
{
    public static AppIcon For(ResourceKind kind) => kind switch
    {
        ResourceKind.Folder => AppIcon.Folder,
        ResourceKind.Page => AppIcon.Page,
        ResourceKind.File => AppIcon.File,
        ResourceKind.Link => AppIcon.Link,
        ResourceKind.Assignment => AppIcon.Assignment,
        ResourceKind.Test => AppIcon.Test,
        ResourceKind.Survey => AppIcon.Survey,
        ResourceKind.Activity => AppIcon.Activity,
        ResourceKind.LearningPath => AppIcon.LearningPath,
        _ => AppIcon.Other,
    };
}
