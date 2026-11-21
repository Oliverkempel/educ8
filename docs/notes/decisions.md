# Decisions log

Newest first. Each entry: date, decision, why.

## 2026-11-21 — Desktop sign-in in the default browser

- The app no longer embeds a web view for sign-in. **Sign in with itslearning** opens the authorize page in the default browser and shows "Continue in your browser". This replaces the embedded WebKitGTK login of 2026-11-19.
- Why: it's what desktop apps usually do now. It reuses the browser's existing school/Microsoft session (often one click), and the user signs in where they trust the address bar. It also drops the WebKitGTK dependency (smaller tar.gz requirements, no WebKit sandbox workaround in the snap).
- How the code gets back: itslearning's last page script-navigates to `itsl-itslearning://login/?state&code`. The app's desktop entry declares `MimeType=x-scheme-handler/itsl-itslearning` with `Exec=educ8 %u`, and `install.sh` makes it the default handler with `xdg-mime`. The Flatpak and the snap export the same entry. GApplication (`HandlesOpen`) forwards the URL to the running instance's `open` signal → `LoginViewModel.AcceptRedirectAsync` → `LoginService.AcceptRedirectAsync`.
- **One handler serves both hosts.** A redirect whose state the app didn't start (a login begun by `educ8-mcp`) goes into the shared callback inbox, where the MCP server picks it up. If the app wasn't running, it does just that and quits without a window. The other way round, while the app waits it also watches the inbox, so `educ8-mcp register-url-handler` as the handler works for the app too.
- Windows and macOS will do the same: a registered URL protocol on Windows, `CFBundleURLTypes` on macOS.

## 2026-11-19 — Releases: every push to main, date-based versions

- **CD:** `.github/workflows/release.yml` runs when a change lands in `main` (and manually). It tests first, and only if everything passes builds and publishes a GitHub release. `ci.yml` builds and tests pull requests only (not every push).
- **Versions are dates (CalVer, UTC):** `YYYY.M.D` for the first release of a day, then `YYYY.M.D.1`, `.2`, … (`.github/scripts/next-version.sh` counts the existing `v…` tags). They're valid .NET versions (every part ≤ 65535) and Flatpak versions. CI passes `-p:Version=…`; the SDK appends `+<commit>` to the informational version. Local builds are `0.0.0-dev`. `gh release create` makes the tag `v<version>` and notes from the commits/PRs since the last one.
- **Each release contains:** the Linux desktop app with its `educ8-mcp` for x64 and arm64, as a tar.gz (with `install.sh`), a Flatpak bundle and a strict snap; `educ8-mcp` alone as a self-contained single file for linux-x64/arm64, win-x64 and osx-arm64/x64; and `SHA256SUMS`. Everything is cross-published from Linux runners (no macOS/Windows runners). aarch64 Flatpaks build under QEMU, the slowest part.
- **The Flatpak packages the prebuilt self-contained publish** (`packaging/flatpak/io.github.educ8.Educ8.yml`, GNOME 51 runtime) instead of building .NET inside flatpak-builder. That way there's no .NET SDK extension and no offline NuGet source list. Inside the sandbox `Educ8Paths` uses the real `~/.local/share/Educ8` (granted with `--filesystem=xdg-data/Educ8`), so the app shares its login and MCP settings with an `educ8-mcp` started outside it.
- **The snap** (`packaging/snap/snapcraft.yaml`, added 2026-11-20) is strict, on `core24` with the `gnome` extension (`gnome-46-2404`: GTK 4, libadwaita 1.5). No GNOME platform exists for `core26` yet. Like the Flatpak it packages the prebuilt publish. It's packed on an Ubuntu 24.04 runner with `--destructive-mode`; arm64 is cross-packed. libadwaita 1.5 lacks `Adw.Spinner`, so `Widgets.Spinner` falls back to `Gtk.Spinner`. A `personal-files` plug (`educ8-data`, connected by hand after `--dangerous` installs) gives access to the real `~/.local/share/Educ8` (`Educ8Paths` uses `SNAP_REAL_HOME`). Not in the Snap Store.
- **Test a branch without releasing:** run the Release workflow manually on the branch with "publish" off. It builds everything and keeps the files as run artifacts.
- **Not yet:** code signing (macOS/Windows), Flathub, Snap Store, an About dialog showing the version.

## 2026-11-19 — The desktop app controls the stdio MCP server (no in-app server yet)

- Claude Desktop starts local MCP servers itself (stdio) and can't reach an HTTP server on localhost without a bridge (e.g. `npx mcp-remote`). So the app doesn't host the server. It controls the one clients start: write-actions switch, per-tool switches, status of running servers, and recent tool calls. It doesn't edit Claude's config (not wanted).
- Settings in `config.json` apply live: tools stay registered, request filters decide per call, and the server sends `tools/list_changed` when the file changes.
- Activity (tool, client, time, shortened arguments, result) is logged to `<data dir>/mcp/activity.jsonl` and stays on the machine.
- The in-app Streamable HTTP host stays planned (D4) for clients that support HTTP.

## 2026-11-19 — Desktop UIs use stock platform components

- Each platform UI uses its toolkit's standard widgets and styles (on Linux: libadwaita widgets and its built-in style classes, no custom CSS). No custom visuals such as itslearning course-colour stripes unless the user asks for them.
- Why: the user wants the app to look native and default. Custom touches added without asking were removed.

## 2026-11-19 — Desktop app: native UI per platform on a shared C# core

- The app must feel native everywhere and use each platform's own UI library: **GTK 4 + libadwaita** (Linux, via Gir.Core), **AppKit** (macOS, .NET for macOS, C#), **WinUI 3** (Windows).
- All behaviour lives in a shared `Educ8.App.Core` (MVVM, CommunityToolkit.Mvvm) on top of `Educ8.Itslearning`. The platform projects contain only UI code. Login uses each platform's web view, intercepting the `itsl-itslearning://` redirect (replaced on 2026-11-21: sign-in in the default browser).
- Linux first (develop and test on the current machine), then Windows, then macOS; you have all three machines.
- First features: start page with all courses, and a calendar. Plan: [../design/desktop-app-plan.md](../design/desktop-app-plan.md).
- Rejected: MAUI (no Linux; Mac Catalyst instead of AppKit), Avalonia/Uno (draw their own controls), SwiftUI (a second codebase/language), Qt (weak C# bindings).
- Considered for maintenance cost: Uno (one WinUI codebase, but drawn with Skia on Linux/macOS) and MAUI for Windows+macOS with GTK for Linux (two UIs, but Catalyst on the Mac). Chose **three thin native UIs**, with strict rules: logic only in view models, shared strings/colours/icons, a per-screen size budget, a parity tracker and CI for all three. See "Keeping three UIs maintainable" in the plan.

## 2026-11-19 — Write actions behind a setting (off by default)

- Anything that changes itslearning (first: `submit_assignment`) is a **write tool**. Write tools are only registered when `AllowWriteActions` is on (`config.json`, env `EDUC8_ALLOW_WRITE_ACTIONS`, or `educ8-mcp write-actions on|off`). When off, the model can't see them at all.
- Defence in depth: each write tool checks the setting again at call time, is annotated `Destructive`, and is **two-step**. Without `confirm=true` it only returns a preview, and the tool description tells the model to show it and get the user's explicit approval.
- Local file uploads accept only absolute paths to regular, non-hidden files (no `.ssh`, `.config`, …), at most 20 files, 100 MB each and 250 MB in total. This limits what a prompt injection in course content could make the model upload.
- The gate is in the MCP layer, not the core: the desktop app will offer writes through its own UI and confirmation.
- Read-only tools now all carry `ReadOnly = true`, so clients can tell them apart.

## 2026-11-19 — Phase 3: assignment feedback from the web view

- REST has no status, score or feedback for a student's own answer, so `get_assignment` reads them from the assignment web view (`WebPageParsers.Assignment`, same SSO chain as Pages/Files). Everything else comes from REST.
- The web view is optional: if it fails, the tool still returns the REST data with a note, rather than an error.
- Assignment fixtures are scrubbed beyond the usual: teacher and student names, person ids, **the feedback text and the student's answer text** are replaced with synthetic content (they're personal academic records).

## 2026-11-18 — Files before assignments; Educ8.Documents; PDF page rendering

- The user prioritised reading course files (PDF, Word, Excel, images) over Phase 3, so Phase 4 was done first.
- File-to-content conversion lives in a new **`Educ8.Documents`** library (no itslearning, MCP or UI dependencies), so the desktop app can reuse it (previews, search). `HtmlToMarkdown` moved there too.
- Libraries: **PdfPig** (Apache-2.0) for PDF text; **DocumentFormat.OpenXml** (MIT) for docx/xlsx/pptx; **PDFtoImage** (MIT; bundles native PDFium + SkiaSharp for Linux, Windows and macOS) to render PDF pages.
- **Whole-page rendering instead of embedded-image extraction for PDFs.** Lecture slides are mostly PDFs. Extracting their embedded images gave fragments: 37 pieces of one collage, and none of the diagrams drawn as shapes. A 110 dpi render is legible and small (150–340 KB). If the native library can't load, the extractor falls back to embedded images.
- PowerPoint isn't rendered: there's no cross-platform .NET renderer without LibreOffice. Slide text, notes and embedded images are good enough for now.
- Model-facing size limits: 40k text characters per call (page through with `pages`), at most 10 images and 12 MB per call, files up to 50 MB.

## 2026-11-18 — Phase 2: web-chain reader in the core, own HTML→Markdown

- `LearningToolContentReader` and `WebPageParsers` live in `Educ8.Itslearning/Content`, so the desktop app can show the same Pages, Links and Files. The reader returns raw author HTML; Markdown conversion belongs to the MCP layer (the app can render HTML directly).
- HTML parsing uses **AngleSharp**. HTML→Markdown is our own small converter (`Educ8.Mcp/Formatting/HtmlToMarkdown.cs`) tuned for itslearning's author HTML: nested bold, `<oembed>` videos, inline styles. We didn't add a second HTML library such as ReverseMarkdown.
- Test fixtures are real pages saved live and **scrubbed** (name, email, person/user ids, signatures, viewstate). Scrub any new fixture the same way, and grep it for personal data before saving.
- `get_course_overview` loads its sections in parallel and degrades per section, so one failing endpoint doesn't break the overview.

## 2026-11-18 — Phase 1: desktop-ready architecture, shared session, login capture

- The design must allow a future **cross-platform desktop app** (a native itslearning front end) that contains the MCP server and can start it from a menu. Not built now.
- Consequences: `Educ8.Itslearning` (core) has no MCP or UI dependencies, and `Educ8.Mcp` (tools) is transport-agnostic (`AddEduc8Tools()` works for stdio now and in-app loopback HTTP later). The stdio host is just one composition. See [../design/architecture.md](../design/architecture.md).
- **Shared session**: one per-user data dir (`LocalApplicationData/Educ8`). Refresh happens under a cross-process file lock, because refresh tokens rotate.
- **Login capture**: itslearning delivers the code by page script to `itsl-itslearning://login/?…`. The desktop app will intercept that in its WebView. The MCP server offers an OS URL-scheme handler (`register-url-handler`: Linux/Windows) that hands the URL over through a file inbox, plus a paste fallback (`login_complete` tool / CLI paste).
- Flagged, not decided: **.NET MAUI has no Linux desktop support**; Avalonia or Uno would cover Linux. The core is UI-agnostic, so this can wait.

## 2026-11-18 — Phase 0 results: use REST plus the SSO web chain

- Use the REST API for everything it covers: courses, folders, search, tasks, assignments, own answers, answer files.
- For Pages (tool 5), Files (tool 5009) and Links (tool 5010), which REST can't serve (Page API returns 500; no file endpoint), use `sso/url/v1` to get a web session, then scrape the tool page or download the file. Keep this isolated and tested, because it depends on undocumented pages.
- Auth facts: redirect URI `itsl-itslearning://login`, no secret, 5-minute access tokens, rotating refresh tokens. Details: [../api/live-findings.md](../api/live-findings.md).
- Why: course material (PDF slides, pages) is the core of v1, and this was the only working path found.

## 2026-11-18 — v1 purpose: course study assistant (proposed)

- The first version helps a student who is studying or doing assignments with an LLM. The LLM can read course materials and understand the course and the current assignment. The server is read-only and works from the student's perspective.
- Plan: [../design/v1-study-assistant-plan.md](../design/v1-study-assistant-plan.md). Phase 0 (live spikes) comes before any real code.

## 2026-11-18 — Build the MCP server in .NET with the official C# SDK

- Language/runtime: C# on .NET 10, using the official MCP C# SDK (`ModelContextProtocol`, a Tier 1 SDK), following the MCP site's C# guide. Notes: [../mcp/csharp-sdk.md](../mcp/csharp-sdk.md).
- Proposed, not yet confirmed: stdio transport first, with transport-independent libraries so HTTP can be added later. See [../design/architecture.md](../design/architecture.md).
- Why: the user's choice of .NET. The C# SDK is official and Tier 1, and it supports URL-mode elicitation, which fits the itslearning OAuth login.

## 2026-11-18 — Auth: itslearning OAuth2 with Microsoft login

- We have a registered itslearning OAuth2 **client ID**.
- The MCP server uses the OAuth2 **authorization code flow** against itslearning (`oauth2/authorize.aspx` → `restapi/oauth2/token`). The user signs in with Microsoft: itslearning's login page sends them to the school's "Office 365" federated provider, then back to itslearning, which redirects to our `redirect_uri` with a `code`.
- Microsoft is only the identity provider here. The API accepts **itslearning-issued** bearer tokens; we don't call Microsoft (Entra ID) ourselves or send Microsoft tokens to the API.
- Session-cookie auth is dropped as a fallback.
- Keep the client ID in config or an environment variable (e.g. `ITSLEARNING_CLIENT_ID`), not hard-coded in the code or docs.
- Why: it's the officially supported flow, works with a federated-only school login, and gives refresh tokens, so the user only signs in once.

## 2026-11-18 — Project start: study the API before building

- Goal: an MCP server that lets an AI assistant work with the itslearning LMS (starting with one university's itslearning site).
- First step: document the REST API only. No code yet, at the user's request.
- The API reference in `docs/api/` is generated from the official help page by scripts in `docs/api/_generator/`, so it can be refreshed instead of hand-maintained.
