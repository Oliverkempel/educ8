# Desktop feature parity

One row per feature, one column per platform. Update it with every feature (rule 9 in [desktop-app-plan.md](desktop-app-plan.md)).

Legend: ✅ done · 🚧 in progress · ⏳ not started · ➖ not applicable

| Feature | View model (App.Core) | Linux (GTK 4 + libadwaita) | Windows (WinUI 3) | macOS (AppKit) |
|---|---|---|---|---|
| Shell: sidebar, account, sign out | ✅ `ShellViewModel` | ✅ `MainWindow` | ⏳ | ⏳ |
| Sign in (default browser, OS hands back the `itsl-itslearning://` redirect) | ✅ `LoginViewModel` | ✅ `LoginDialog` + `open` signal | ⏳ | ⏳ |
| Start page: all courses (search, semester filter; grid of cards or list of collapsible rows with current plans + teachers, choice remembered in `app.json`) | ✅ `CoursesViewModel`, `CourseCardViewModel`, `AppPreferences` | ✅ `CoursesView` (`FlowBox` cards / boxed list of expander-style rows: click opens, button folds out; linked grid/list toggles) | ⏳ | ⏳ |
| Plan page (from a card's plan: when, description, material, open course) | ✅ `PlanPageViewModel` | ✅ `PlanPage` | ⏳ | ⏳ |
| Dates and times in the system's formats (`AppCulture`: `LC_TIME`/`LC_NUMERIC`, desktop 12/24-hour clock) | ✅ `AppCulture`, `Texts` | ✅ `DesktopSettings` (settings portal `clock-format`) | ➖ .NET reads the user's regional settings | ➖ same |
| Course page: Overview (announcements, lesson plans + material, open tasks, recent changes), Plans (every plan in course order + material) and Resources (folders as pages) | ✅ `CourseViewModel`, `CoursePlansViewModel`, `FolderViewModel`, `CoursePageFactory` | ✅ `CoursePage` (`Adw.NavigationView`, `Adw.ViewSwitcher`) | ⏳ | ⏳ |
| Notifications page: itslearning's bell list (180 days, 100 items), unread badge in the sidebar (checked every 5 min), open = mark read on itslearning + course page in the app (announcements) or the browser (elements), mark all as read | ✅ `NotificationsViewModel` | ✅ `NotificationsView`, badge in `MainWindow` | ⏳ | ⏳ |
| Messages: conversations (unread badge), conversation view (oldest first, older on request, marks read), reply, new conversation (people search), received attachments | ✅ `MessagesViewModel`, `ConversationViewModel`, `NewConversationViewModel`, `MessageAttachments` | ✅ `MessagesView` | ⏳ | ⏳ |
| MCP page: status of running servers, write-actions switch, per-tool switches, recent tool calls (settings apply live) | ✅ `McpViewModel` | ✅ `McpView` | ⏳ | ⏳ |
| Calendar: week agenda, navigation, course filter | ✅ `CalendarViewModel` | ✅ `CalendarView` | ⏳ | ⏳ |
| Event details: lesson plan + material | ✅ `EventDetailsViewModel` | ✅ `EventDetailsView` | ⏳ | ⏳ |
| Element modules: router with browser fallback, row affordances from the module, element pages pushed per section | ✅ `ElementNavigator`, `IElementModule`, `ElementPageViewModel` | ✅ `ElementPages` (one arm per page VM, generic fallback) | ⏳ | ⏳ |
| Folder page (in-app, also from event details) | ✅ `FolderModule`, `FolderViewModel` | ✅ `FolderPage` | ⏳ | ⏳ |
| Page (itslearning Page, tool 5) as native rich text | ✅ `PageModule`, `ContentPageViewModel`, `HtmlToRichText` | ✅ `ContentPage`, `RichTextView` | ⏳ | ⏳ |
| Link (tool 5010): opens the real target; description page when there is one | ✅ `LinkModule`, `LinkViewModel` | ✅ `LinkPage` | ⏳ | ⏳ |
| Assignment (tool 5006): description, files, answer, status, feedback | ✅ `AssignmentModule`, `AssignmentViewModel` | ✅ `AssignmentPage` | ⏳ | ⏳ |
| Assignment hand-in (first hand-in, individual; review + confirm) | ✅ `AssignmentHandIn`, `HandInViewModel`, `IPickFiles` | ✅ `HandInDialog` (`Adw.Dialog` + `Adw.AlertDialog`), `GtkPickFiles` | ⏳ | ⏳ |
| Open course files in the default app (download to cache, `IOpenFile`; stop button cancels a running download via `OpenCancelCommand`) | ✅ `MaterialItemViewModel` + `MaterialFiles` | ✅ `GtkOpenFile` (`Gtk.FileLauncher`), `MaterialRows` | ⏳ | ⏳ |

## Platform mappings to keep complete

- **Element pages:** every `ElementPageViewModel` subclass needs a page builder on each platform (GTK: an arm in `Views/Elements/ElementPages.cs`). Without one the generic page shows, with "Open in itslearning".
- **Rich text:** each platform renders `RichDocument` (GTK: `Ui/RichTextView.cs`).
- **`IPickFiles`:** one implementation per platform (GTK: `GtkPickFiles`, `Gtk.FileDialog`).

- **Icons:** every `AppIcon` value (`src/Educ8.App.Core/Presentation/AppIcon.cs`) needs a GNOME symbolic icon, an SF Symbol and a Segoe Fluent Icons glyph in each platform's icon map.
- **Platform services:** `IUiDispatcher`, `IOpenUrl` and `IOpenFile` (open a local file with its default app) each need an implementation per platform.
- **Strings:** UIs use only `Educ8.App.Core.Resources.Strings`; no hard-coded user-visible text.
