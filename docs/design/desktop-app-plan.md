# Desktop app plan

Status: **D0 (shared core) done 2026-11-19. D1 (Linux app) done 2026-11-19**: sign-in (now in the default browser, 2026-11-21), courses and calendar all checked live against a real itslearning site. Next is D2 (Windows). Progress per platform: [desktop-parity.md](desktop-parity.md).

### D1 result
- `src/Educ8.App.Gtk` (assembly `educ8`, app id `io.github.educ8.Educ8`), GirCore.Adw-1 0.8.1 (WebKit was dropped with the browser sign-in, 2026-11-21). It is in `Educ8.slnx`. Run it with `dotnet run --project src/Educ8.App.Gtk`. It needs the GTK 4 and libadwaita 1 runtime libraries. `[assembly: SupportedOSPlatform("linux")]`.
- Files:
  - `Program.cs`: composition only (same DI as D0).
  - `Services/GtkServices.cs`: `GtkDispatcher` uses `GLib.Functions.IdleAdd`, `GtkOpenUrl` uses `Gtk.UriLauncher`, and `GtkOpenFile` uses `Gtk.FileLauncher`.
- **Course page** (`Views/CoursePage.cs`). Clicking a course card pushes it onto an `Adw.NavigationView`, which provides the standard back button. The header holds an `Adw.ViewSwitcher` with three tabs (Overview, Plans and Resources), plus Open in itslearning and Refresh. Below 820px wide, an `Adw.Breakpoint` (in an `Adw.BreakpointBin`) moves the tabs to an `Adw.ViewSwitcherBar` at the bottom and puts the course title in the header.
  - Overview: four `PreferencesGroup` sections that load independently, so a failing section shows its own error. They are: announcements (expander rows), lesson plans from 7 days back to 21 days ahead (expander rows with their material, read from the plan endpoint), open tasks sorted by deadline, and recent changes from the last 14 days.
  - Plans: every plan in the course, from the undocumented `plans/topics/{id}/all/v1` endpoint (topic 0 = no topic). It's one list in itslearning's course order. Each plan is an expander with its material, and the topic is shown only when it differs from the plan name. Loaded when the tab is first shown.
  - Resources: the folder tree. Each folder is pushed as its own page, and inactive items are hidden.
  - View models: `CourseViewModel`, `CourseSection<T>`, `FolderViewModel` and `CoursePageFactory` (App.Core). The courses grid raises `CoursesViewModel.CourseOpened`, and folders raise `FolderViewModel.FolderOpened`. The platform only pushes the pages.
- **MCP page** (sidebar entry "MCP server", `Views/McpView.cs` + `McpViewModel`). Decided 2026-11-19: the app is a **control panel for the stdio server** that MCP clients start themselves. Claude Desktop can't reach an HTTP server on localhost without a bridge, so the in-app HTTP host stays in D4.
  - Shows the running servers and the client that started each, a write-actions switch, a switch per tool (write tools are only switchable while write actions are on), and the 40 most recent tool calls (tool, client, time, duration, arguments, ok/failed/refused). The page refreshes every 3 s while shown.
  - Shared code in `Educ8.Mcp/Control`, used by both server and app: `McpToolCatalog`, `McpActivityLog` (`<data>/mcp/activity.jsonl`, trimmed past 512 KB), `McpServerRegistry` (`<data>/mcp/servers/<pid>.json`, files of dead processes are dropped), and `McpSettingsWatcher`. Settings live in `config.json` (`AllowWriteActions`, `DisabledTools`).
  - Settings apply live. Every tool is always registered; a list-tools filter hides unavailable tools, and a call-tool filter refuses them and records each call. The server watches `config.json` and sends `notifications/tools/list_changed`, advertising `tools.listChanged`.
- Course files open in the default app. Clicking a File in the event details downloads it with `MaterialFiles` (App.Core) into the cache folder (`~/.cache/Educ8/files/<elementId>/`, honouring `XDG_CACHE_HOME`) and hands it to `IOpenFile`. Each file downloads once per app session. A failed download opens the item in itslearning instead and shows why on the row. Pages, links and assignments still open in the browser.
  - `Ui/Bind.cs`: small one-way binding helpers (property, text, visibility, collection rebuild, command + sensitivity, error banner). `Ui/Widgets.cs`: label, box and row factories. `Ui/Icons.cs`: the `AppIcon` → Adwaita symbolic map. No custom CSS: the UI uses stock libadwaita widgets and style classes (`card`, `boxed-list`, `heading`, `dim-label`, …). itslearning's course colours are not shown, which is the user's choice (2026-11-19). `AccentColor` stays in the view models for platforms or settings that want it.
  - `Shell/MainWindow.cs`: an `Adw.NavigationSplitView` with a sidebar (pages + account + sign out), and a stack for starting / signed out / signed in.
  - `Views/WelcomeView`, `LoginDialog` (an `Adw.Dialog` with a `StatusPage`: "Continue in your browser", spinner, "Open browser again"; closing it cancels. The redirect arrives through the application's `open` signal in `Program.cs`), `CoursesView` (a `FlowBox` of `card` boxes), `CalendarView` (header with previous/today/next, week title, `Gtk.Calendar` date picker, course `DropDown`, refresh; agenda as boxed lists per day) and `EventDetailsView` (an `Adw.OverlaySplitView` side pane).
- GTK gotchas:
  - Gir.Core's `Application.Run(args)` passes `args` to GApplication as argv unchanged, and GApplication treats argv[0] as the program name. `Program.cs` therefore puts `Environment.ProcessPath` in front; without it, `educ8 <url>` drops the URL and only activates the window.
  - If Gir.Core is used for another native library (e.g. WebKit), call its `Module.Initialize()` at startup. Gir.Core maps the short library name to the real file only after that; `Adw.Application` does it only for Adw/Gtk.
  - `Adw.ActionRow` uses Pango markup by default. `Widgets.Row` turns it off, and `Widgets.Escape` is only for setters that parse markup: `StatusPage` **description** and `PreferencesGroup` title/description. `StatusPage` **title** and `WindowTitle` are plain text; escaping them showed `&quot;` in the UI. This was checked against libadwaita at runtime, so check again before escaping any other setter.
  - Never replace a `DropDown`'s model, or re-set its selection, from inside its own `notify::selected` handling. The calendar's course filter did that (selection → view model → re-sync) and crashed the app natively, with no .NET exception. `CalendarView.SyncFilter` now touches the model and selection only when they really differ.
  - Opening links: `GtkOpenUrl` and `GtkOpenFile` log failures to stderr and fall back to `Gio.AppInfo.LaunchDefaultForUri`. `EDUC8_DEBUG=1` logs every row click and launch (scheme, host and path only). A launch reported as successful only means the desktop accepted it. On 2026-11-19, links "did nothing" because the Flatpak **document portal mount** (`/run/user/<uid>/doc`) had disappeared. A headless test run with `dbus-run-session` had unmounted it (see "Never run the app with `dbus-run-session`" below), so the Zen Flatpak died at start (`bwrap: Can't find source path /run/user/1000/doc/by-app/…`, visible in `journalctl --user`). The fix was `systemctl --user restart xdg-document-portal.service`, or logging out and in. Nothing in the app needed changing.
  - `Widgets.Label` keeps GTK's default (one line, no wrapping); use `Widgets.WrapLabel` for multi-line text. Wrapping on by default broke short labels in narrow places (the sidebar showed "Cou-rses").
  - Wrapping labels report their whole text as natural width, which makes a homogeneous `FlowBox` give every card the width of the longest title. Cap them with `max-width-chars`.
  - Cards in a `FlowBox`: put `card` + `activatable` on the `FlowBoxChild` itself, not on a box inside it. Otherwise the focus ring is drawn around the child with a different corner radius than the card.
  - Only one copy of the app runs per session. A second launch activates the first one's window and exits silently. Screenshot runs use `NonUnique`, so they work while the app is open.
  - `PreferencesGroup` titles are one line, so long lesson-plan names use a heading label instead.
- **Headless check** (no Xvfb on this machine): start `gtk4-broadwayd :7`, open `http://127.0.0.1:8087` in a **visible** browser, then run `GDK_BACKEND=broadway BROADWAY_DISPLAY=:7 EDUC8_SCREENSHOT_DIR=<dir> ./educ8` from the build output. It saves `courses.png`, `courses-focus.png` (first card keyboard-focused), `course.png` and `course-resources.png` (first course), `calendar.png`, `calendar-filter.png` (a course picked in the drop-down) and `event.png` (or `welcome.png` when signed out) and quits. Broadway draws frames only while a viewer is connected and visible. Without one, the captures are stale or empty. **Never run the app with `dbus-run-session`** (e.g. to get around the single-instance hand-off). A private session bus starts a second `xdg-document-portal`, which takes over `/run/user/<uid>/doc` and unmounts it when it exits. That breaks every Flatpak app (Zen couldn't open links) until `systemctl --user restart xdg-document-portal.service`. Screenshot runs already use `NonUnique`.

### D0 result
- Moved to `Educ8.Itslearning`: `Models/Plans.cs`, which has `PlanLookups.GetPlanMaterialsAsync` / `GetEventPlansAsync` (plan ↔ material folder, event → lesson plans), `PlanMaterials`, `LinkedPlan`, and `CourseCodes.Semester` / `CurrentSemester`. The MCP tools use them; its tests are unchanged and green. `PersonCourse` now maps `CourseColor`/`CourseFillColor` (hex).
- New `src/Educ8.App.Core` with CommunityToolkit.Mvvm 8.4 (partial-property `[ObservableProperty]`):
  - `ShellViewModel`: Starting/SignedOut/SignedIn, sidebar items, account text, sign out. Follows `TokenManager.SessionChanged` and loads the shown page on first view.
  - `LoginViewModel`: `Start` opens `AuthorizeUrl` in the default browser (`IOpenUrl`) and watches the callback inbox. The platform passes URLs the OS hands it to `AcceptRedirectAsync` (true = this app's sign-in, show the window).
  - `CoursesViewModel` + `CourseCardViewModel`: current semester = code suffix matches the running semester (`CurrentSemester`: Aug–Jan E, Feb–Jul F), or no code and active within 120 days. Has search, ready-made texts (teachers, "Updated yesterday", badges) and course colours.
  - `CalendarViewModel` + day/event item VMs: week from the culture's first day, ISO week number, range text (handles month and year crossings), previous/today/next/go-to-date, a course filter of courses with events that week, stale-load cancellation, and the selected event → `EventDetailsViewModel`.
  - `EventDetailsViewModel`: when/where/course/description as plain text, lesson plans with description and material, linked material, open in itslearning.
  - `CourseCatalog` (courses cached once, shared), `Texts` (friendly errors, relative dates, time ranges), `AppIcon` (+ kind mapping), `Strings.resx` (65 strings, strongly typed at build), and `IUiDispatcher` / `IOpenUrl` (implemented by each platform).
  - DI: `services.AddItslearning(...)` + `services.AddEduc8AppCore()` + the platform's `IUiDispatcher`/`IOpenUrl`, then resolve `ShellViewModel`.
- `Educ8.Documents.HtmlToText`: HTML → plain text for native labels.
- Tests: `tests/Educ8.App.Core.Tests` (15, en-GB culture, UTC clock, fake itslearning) plus new helper tests. 186 tests in total.

## Goals

- A native itslearning front end that feels **native on Linux, macOS and Windows**, built with each platform's **own UI toolkit**:

  | Platform | Toolkit | .NET target | Bindings |
  |---|---|---|---|
  | Linux | **GTK 4 + libadwaita** (GNOME look) | `net10.0` | Gir.Core (`GirCore.Gtk-4.0`, `GirCore.Adw-1`, 0.8.x) |
  | macOS | **AppKit** | `net10.0-macos` | .NET for macOS workload (`dotnet workload install macos`; needs a Mac with Xcode) |
  | Windows | **WinUI 3** | `net10.0-windows10.0.19041.0` | Windows App SDK 2.7 (needs Windows to build) |

- Later, the MCP server becomes part of the app and can be started from a menu (see [architecture.md](architecture.md), "MCP hosting").
- First features: **start page with all courses** and a **calendar**.
- Order: **Linux first** (it can be developed and tried on the current machine), then Windows, then macOS. All three machines are available.

Cross-platform UI frameworks (MAUI, Avalonia, Uno) are ruled out. MAUI has no Linux desktop and uses Mac Catalyst rather than AppKit. Avalonia and Uno draw their own controls instead of using each platform's native ones.

## Architecture: one shared core, three native UIs

```
┌──────────────── platform UIs (one per OS, only UI code) ────────────────┐
│ Educ8.App.Gtk            │ Educ8.App.Mac             │ Educ8.App.Windows │
│ GTK4 + libadwaita        │ AppKit                    │ WinUI 3 (XAML)    │
│ URL handler (.desktop)   │ URL handler (Info.plist)  │ URL protocol      │
├──────────────────────────┴───────────────────────────┴───────────────────┤
│ Educ8.App.Core (net10.0): view models, navigation, app services          │
│   CommunityToolkit.Mvvm (ObservableObject, RelayCommand)                 │
├──────────────────────────────────────────────────────────────────────────┤
│ Educ8.Itslearning (existing): auth, tokens, REST client, web chain       │
│ Educ8.Documents   (existing): file previews (later)                      │
└──────────────────────────────────────────────────────────────────────────┘
```

- **All logic lives in `Educ8.App.Core`**: loading, filtering, week navigation and error/empty states. Each platform UI only builds widgets, wires them to view-model properties and commands, and provides a few platform services. It has no business logic, so features are written once and tested once.
- **View models expose plain .NET data** (`ObservableCollection`, `INotifyPropertyChanged`, `IAsyncRelayCommand`), with no UI types.
- **Binding:**
  - WinUI binds natively (`x:Bind`).
  - GTK and AppKit have no data binding, so each gets a small `Bind` helper. It subscribes to `PropertyChanged` and `CollectionChanged`, and feeds collections into `Gio.ListStore` (GTK) or an `NSTableView` data source (AppKit).
- **Platform services** (interfaces in App.Core, implemented per platform): `IUiDispatcher` (run on the UI thread), `IOpenUrl` (open a link in the default browser), receiving the `itsl-itslearning://` redirect from the OS (passed to `LoginViewModel.AcceptRedirectAsync`), and later `ISecretStore` and notifications.
- **Shared session with the MCP server:** the same data directory and `TokenManager` with its cross-process lock (already built). Logging in to the app also logs in the MCP server.
- **UI definition:**
  - GTK and AppKit UIs are written in C#: no `.ui` XML files or storyboards. That keeps refactoring easy and avoids designer tooling.
  - WinUI uses XAML, which is its native way.

### Keeping three UIs maintainable (decided 2026-11-19)

Three native UIs are only affordable if they stay **thin**. These rules make that concrete:

1. **One view model per screen, written first.** Each screen's view-model API (properties, commands, collections) is the contract all three UIs bind to. Behaviour changes happen in the view model and are tested there once.
2. **No logic in UI projects.** That includes formatting, sorting, filtering, date maths, error texts and empty-state decisions. The view model exposes ready-to-show values (e.g. `TimeRangeText`, `BadgeText`, `IsEmpty`, `ErrorMessage`). If a UI file needs an `if` about data, it belongs in the view model.
3. **Shared presentation resources in `Educ8.App.Core`:**
   - all user-visible **strings** (resx, so translation happens once);
   - **course colours** as hex values;
   - **icon names** as one enum mapped to each platform's icons (GNOME symbolic icons, SF Symbols, Segoe Fluent Icons).
4. **A size budget per screen:** roughly 300 lines per platform. Going over it is a signal to move code into the view model or a shared helper.
5. **One small binding helper per platform** (`Bind.cs` for GTK and for AppKit; WinUI uses `x:Bind`). All widget↔view-model wiring goes through it, so binding bugs are fixed in one place.
6. **Platform differences go through services, not `#if`.** Native conventions like Ctrl vs Cmd shortcuts, the macOS menu bar and window chrome live in each platform's shell. Anything behavioural is behind an interface in App.Core.
7. **Same structure in every UI project:** `Shell`, `Views/<Screen>View`, `Services`, `Bind`. A screen added to one platform is easy to find and mirror in the others.
8. **Feature workflow:** view model + tests → Linux UI → Windows UI → macOS UI. A feature is "done" only when it's in all three, or explicitly marked as lagging.
9. **Parity tracking:** `docs/design/desktop-parity.md` has one row per feature and one column per platform. It's updated with each feature, so gaps are visible.
10. **CI builds all three UIs on every change.** A view-model change that breaks one UI fails immediately, not weeks later.

Expected effort split: most work and nearly all bugs land in the shared layers. Per-platform work is layout and is mostly touched when the design changes.

### Login in the app

Sign-in runs in the **default browser** (decided 2026-11-21; D1 first shipped an embedded WebKitGTK view). The redirect is `itsl-itslearning://login/?state&code`, sent by page script. The app registers as that scheme's handler, so the browser hands the URL to the app, which passes it to `LoginViewModel.AcceptRedirectAsync`. Redirects for logins the app didn't start go to the callback inbox for `educ8-mcp`.

| Platform | Registration | Where the URL arrives |
|---|---|---|
| GTK | Desktop entry `MimeType=x-scheme-handler/itsl-itslearning`, `Exec=educ8 %u` (+ `xdg-mime default` in `install.sh`; Flatpak/snap export the entry) | `Gio.ApplicationFlags.HandlesOpen` → `open` signal in the running instance |
| macOS | `CFBundleURLTypes` in Info.plist | `NSApplicationDelegate.OpenUrls` |
| Windows | `HKCU\Software\Classes\itsl-itslearning` (or an MSIX protocol extension) | Single instance: redirect activation to the running instance (`AppInstance.RedirectActivationToAsync`) |

## First features

### Shell (all platforms)
- A window with a **sidebar**: "Courses" and "Calendar", plus an account area with the user's name and **Log out**.
  - GTK: `Adw.NavigationSplitView` + `Adw.HeaderBar`
  - macOS: `NSSplitViewController` with a sidebar and a unified toolbar
  - Windows: `NavigationView` with Mica backdrop
- **Signed-out state:** a welcome page with **"Sign in with itslearning"**, which opens sign-in in the default browser and shows "Continue in your browser" (in a sheet/dialog).
- **Errors** appear as native toasts or info bars (`Adw.Toast`, `InfoBar`, an `NSAlert` sheet), not modal crashes.

### Start page: all courses
- Course cards in a responsive grid (`Gtk.FlowBox` / `NSCollectionView` / `ItemsRepeater` + `UniformGridLayout`). Each card shows:
  - title, course code and semester;
  - teachers (first 2–3);
  - last activity;
  - badges for new notifications and new announcements (`NewNotificationsCount`, `NewBulletinsCount` from `courses/v4`);
  - the course colour as an accent.
- A **search** field that filters by title and code.
- A **"Current semester" / "All"** filter. The default is courses with activity in the last ~4 months, falling back to the latest semester code (E26/F26).
- Sorted by last activity. Refresh with the toolbar button or Ctrl/Cmd+R.
- Clicking a card opens a **course page placeholder** for now: title, upcoming events, and "Open in itslearning" through the SSO link. The full course page comes later.

### Calendar
- A **week agenda**: days of the selected week, each with its events. Each event shows time, title, room/teachers and course colour, and today is highlighted.
- Navigation: previous, today, next, and a date picker (`Gtk.Calendar` in a popover / `NSDatePicker` / `CalendarDatePicker`).
- A **course filter** (all courses or one).
- **Event details** pane or sheet: time, place, timetable info, the linked lesson plan with its description and material list, and "Open in itslearning". Opening material inside the app comes later; for now it opens in the browser.
- A visual week grid (time axis, columns per day) can come later on top of the same view model.

## Changes needed in the existing code

1. **Move reusable logic out of `Educ8.Mcp` into `Educ8.Itslearning`** so the app doesn't depend on MCP:
   - plan → material folder mapping (`CourseContentTools.LoadPlanMaterialsAsync`) → e.g. a `PlanMaterialsService`;
   - plan details for an event (`ScheduleTools.PlanDetailsAsync`);
   - semester from a course code (`CourseFormatter.Semester`).

   The MCP tools then call the core, and the MCP tests guard the move.
2. **`Educ8.App.Core`** project plus **`Educ8.App.Core.Tests`**: view-model tests with the existing fake itslearning (`TestServices`, `FakeItslearning`).
3. Solution layout: platform UI projects build only on their OS. Keep `Educ8.slnx` for the shared and Linux projects. Add `Educ8.Windows.slnx` and `Educ8.Mac.slnx` (or solution filters), so `dotnet build Educ8.slnx` still works on Linux.

## Phases

| Phase | Where | Done when |
|---|---|---|
| **D0: Shared core** | Linux | Logic moved to `Educ8.Itslearning`. Shared strings (resx), course colours and the icon-name enum in App.Core. `desktop-parity.md` created. `Educ8.App.Core` has `ShellViewModel` (session/login state, navigation), `LoginViewModel` (begin login, accept the redirect URL), `CoursesViewModel` (+ card VM, search, semester filter), `CalendarViewModel` (week, days, events, filter) and `EventDetailsViewModel`. Unit tests are green and the MCP tests still pass. |
| **D1: Linux app** | Linux | `Educ8.App.Gtk` with an `Adw.Application` (app id e.g. `io.github.educ8.Educ8`), shell, login, courses page and calendar. Tried live against a real itslearning site: log in, see all courses, browse weeks. Shares the login with the MCP server. |
| **D2: Windows app** | Windows PC | `Educ8.App.Windows` (WinUI 3, unpackaged to start with): the same three screens in XAML with `x:Bind` to the same view models, plus browser login (URL protocol). Tried live. |
| **D3: macOS app** | Mac | `Educ8.App.Mac` (AppKit): the same screens, browser login (URL type), standard menu bar. Tried live. |
| **D4: Polish** | all | OS secret stores for tokens (libsecret / Keychain / Credential Manager), settings, app icons, keyboard shortcuts, accessibility pass, and packaging (Linux tar.gz + Flatpak bundles are **done** via the release workflow on 2026-11-19; still open: Flathub, MSIX, signed `.app`/`.dmg`). Then the **"Start MCP server"** menu (loopback Streamable HTTP host in the app). |

## Testing

- **View models:** xUnit in `Educ8.App.Core.Tests` with fake itslearning HTTP. Covers loading and error states, search and filter, week navigation across month and year boundaries, time zones, and the login redirect handling.
- **Platform UIs:** a short manual checklist per platform for each phase (log in, courses, calendar, log out, refresh, keyboard navigation, dark mode). UI automation can come later (AT-SPI on Linux, UI Automation on Windows, XCUITest is not available for AppKit through .NET).
- **CI:** GitHub Actions matrix builds the core and tests on ubuntu-latest, the WinUI app on windows-latest and the AppKit app on macos-latest.

## Risks

| Risk | Mitigation |
|---|---|
| Gir.Core is pre-1.0 (0.8.x; 0.9 in preview) | Pin versions centrally and keep GTK code thin; most code is in App.Core |
| The browser doesn't hand the `itsl-itslearning://` link to the app (handler not registered, or blocked) | The app also watches the callback inbox, so `educ8-mcp register-url-handler` works as a fallback; `educ8-mcp login` can take a pasted link |
| Three UIs triple the UI work | All behaviour in shared view models; each UI is mostly layout |
| macOS/Windows builds need those machines | CI builds on every push; manual testing on the Mac and the PC per phase |
| itslearning web markup changes | Already isolated in `WebPageParsers` with fixture tests |
