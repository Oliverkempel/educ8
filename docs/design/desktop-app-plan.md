# Desktop app plan

Status: **D0 (shared core) done 2026-11-19.** No platform UI yet; next is D1 (Linux). Progress per platform: [desktop-parity.md](desktop-parity.md).

### D0 result
- Moved to `Educ8.Itslearning`: `Models/Plans.cs`, which has `PlanLookups.GetPlanMaterialsAsync` / `GetEventPlansAsync` (plan ↔ material folder, event → lesson plans), `PlanMaterials`, `LinkedPlan`, and `CourseCodes.Semester` / `CurrentSemester`. The MCP tools use them; its tests are unchanged and green. `PersonCourse` now maps `CourseColor`/`CourseFillColor` (hex).
- New `src/Educ8.App.Core` with CommunityToolkit.Mvvm 8.4 (partial-property `[ObservableProperty]`):
  - `ShellViewModel`: Starting/SignedOut/SignedIn, sidebar items, account text, sign out. Follows `TokenManager.SessionChanged` and loads the shown page on first view.
  - `LoginViewModel`: `Start` gives `AuthorizeUrl`. The platform web view calls `OnNavigating(uri)` for every navigation and cancels when it returns true (the `itsl-itslearning://` redirect).
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
  | Linux | **GTK 4 + libadwaita** (GNOME look) | `net10.0` | Gir.Core (`GirCore.Gtk-4.0`, `GirCore.Adw-1`, `GirCore.WebKit-6.0`, 0.8.x) |
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
│ WebKitGTK login view     │ WKWebView login view      │ WebView2 login    │
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
- **Platform services** (interfaces in App.Core, implemented per platform): `IUiDispatcher` (run on the UI thread), `IOpenUrl` (open a link in the default browser), `ILoginView` (show the sign-in page, report the redirect), and later `ISecretStore` and notifications.
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

The redirect is `itsl-itslearning://login/?state&code`, sent by page script. Each platform's web view can cancel that navigation and pass the URL to `LoginService.CompleteAsync`. No URL handler or pasting is needed.

| Platform | Web view | Hook |
|---|---|---|
| GTK | WebKitGTK 6 (`WebKit.WebView`) | `decide-policy` signal → navigation action URI starts with `itsl-itslearning:` → `ignore()` |
| macOS | `WKWebView` | `WKNavigationDelegate.DecidePolicy(navigationAction)` → `.Cancel` |
| Windows | WebView2 (`Microsoft.UI.Xaml.Controls.WebView2`) | `NavigationStarting` → `args.Cancel = true` |

Risk: Microsoft's sign-in page could reject an embedded browser. WebView2 and WKWebView are normally accepted. If WebKitGTK is rejected, set a standard browser user agent; as a last resort, fall back to the system browser with the existing URL handler.

## First features

### Shell (all platforms)
- A window with a **sidebar**: "Courses" and "Calendar", plus an account area with the user's name and **Log out**.
  - GTK: `Adw.NavigationSplitView` + `Adw.HeaderBar`
  - macOS: `NSSplitViewController` with a sidebar and a unified toolbar
  - Windows: `NavigationView` with Mica backdrop
- **Signed-out state:** a welcome page with **"Sign in with itslearning"**, which opens the login web view (in a sheet/dialog).
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
| **D1: Linux app** | Linux | `Educ8.App.Gtk` with an `Adw.Application` (app id e.g. `io.github.educ8.Educ8`), shell, WebKitGTK login, courses page and calendar. Tried live against SDU: log in, see all courses, browse weeks. Shares the login with the MCP server. |
| **D2: Windows app** | Windows PC | `Educ8.App.Windows` (WinUI 3, unpackaged to start with): the same three screens in XAML with `x:Bind` to the same view models, plus WebView2 login. Tried live. |
| **D3: macOS app** | Mac | `Educ8.App.Mac` (AppKit): the same screens, WKWebView login, standard menu bar. Tried live. |
| **D4: Polish** | all | OS secret stores for tokens (libsecret / Keychain / Credential Manager), settings, app icons, keyboard shortcuts, accessibility pass, and packaging (Flatpak, MSIX, signed `.app`/`.dmg`). Then the **"Start MCP server"** menu (loopback Streamable HTTP host in the app). |

## Testing

- **View models:** xUnit in `Educ8.App.Core.Tests` with fake itslearning HTTP. Covers loading and error states, search and filter, week navigation across month and year boundaries, time zones, and the login redirect handling.
- **Platform UIs:** a short manual checklist per platform for each phase (log in, courses, calendar, log out, refresh, keyboard navigation, dark mode). UI automation can come later (AT-SPI on Linux, UI Automation on Windows, XCUITest is not available for AppKit through .NET).
- **CI:** GitHub Actions matrix builds the core and tests on ubuntu-latest, the WinUI app on windows-latest and the AppKit app on macos-latest.

## Risks

| Risk | Mitigation |
|---|---|
| Gir.Core is pre-1.0 (0.8.x; 0.9 in preview) | Pin versions centrally and keep GTK code thin; most code is in App.Core |
| Microsoft sign-in in embedded WebKitGTK | User-agent override; system-browser + URL-handler fallback (already built) |
| Three UIs triple the UI work | All behaviour in shared view models; each UI is mostly layout |
| macOS/Windows builds need those machines | CI builds on every push; manual testing on the Mac and the PC per phase |
| itslearning web markup changes | Already isolated in `WebPageParsers` with fixture tests |
