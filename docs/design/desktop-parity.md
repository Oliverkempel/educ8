# Desktop feature parity

One row per feature, one column per platform. Update it with every feature (rule 9 in [desktop-app-plan.md](desktop-app-plan.md)).

Legend: ✅ done · 🚧 in progress · ⏳ not started · ➖ not applicable

| Feature | View model (App.Core) | Linux (GTK 4 + libadwaita) | Windows (WinUI 3) | macOS (AppKit) |
|---|---|---|---|---|
| Shell: sidebar, account, sign out | ✅ `ShellViewModel` | ✅ `MainWindow` | ⏳ | ⏳ |
| Sign in (default browser, OS hands back the `itsl-itslearning://` redirect) | ✅ `LoginViewModel` | ✅ `LoginDialog` + `open` signal | ⏳ | ⏳ |
| Start page: all courses (cards, search, semester filter) | ✅ `CoursesViewModel` | ✅ `CoursesView` | ⏳ | ⏳ |
| Course page: Overview (announcements, lesson plans + material, open tasks, recent changes), Plans (every plan in course order + material) and Resources (folders as pages) | ✅ `CourseViewModel`, `CoursePlansViewModel`, `FolderViewModel`, `CoursePageFactory` | ✅ `CoursePage` (`Adw.NavigationView`, `Adw.ViewSwitcher`) | ⏳ | ⏳ |
| MCP page: status of running servers, write-actions switch, per-tool switches, recent tool calls (settings apply live) | ✅ `McpViewModel` | ✅ `McpView` | ⏳ | ⏳ |
| Calendar: week agenda, navigation, course filter | ✅ `CalendarViewModel` | ✅ `CalendarView` | ⏳ | ⏳ |
| Event details: lesson plan + material | ✅ `EventDetailsViewModel` | ✅ `EventDetailsView` | ⏳ | ⏳ |
| Open course files in the default app (download to cache, `IOpenFile`) | ✅ `MaterialItemViewModel` + `MaterialFiles` | ✅ `GtkOpenFile` (`Gtk.FileLauncher`) | ⏳ | ⏳ |

## Platform mappings to keep complete

- **Icons:** every `AppIcon` value (`src/Educ8.App.Core/Presentation/AppIcon.cs`) needs a GNOME symbolic icon, an SF Symbol and a Segoe Fluent Icons glyph in each platform's icon map.
- **Platform services:** `IUiDispatcher`, `IOpenUrl` and `IOpenFile` (open a local file with its default app) each need an implementation per platform.
- **Strings:** UIs use only `Educ8.App.Core.Resources.Strings`; no hard-coded user-visible text.
