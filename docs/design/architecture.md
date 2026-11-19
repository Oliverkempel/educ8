# Architecture

Status: **Phase 1 in progress (2026-11-18).** Based on [../mcp/csharp-sdk.md](../mcp/csharp-sdk.md), [../api/README.md](../api/README.md) and [../api/live-findings.md](../api/live-findings.md).

## Goals that shape the design

1. **Now:** an MCP server (stdio) that gives an LLM read access to a student's itslearning courses.
2. **Later (not built yet):** a cross-platform **desktop app**, a native itslearning front end. The MCP server is part of it and can be started and stopped from a menu inside the app.

So the itslearning access code (login, tokens, API client, content readers) must not depend on MCP or on any UI. MCP is one front end and the desktop app is another, and both sit on the same core.

## Layers

```
┌────────────────────────── hosts ──────────────────────────┐
│ Educ8.Mcp.Server (console, now)  │  Educ8.App (desktop, later)          │
│  - stdio MCP host                │   - UI (MAUI or Avalonia, TBD)       │
│  - CLI: login/status/logout,     │   - WebView login (intercepts        │
│    URL-scheme handler            │     itsl-itslearning://)             │
│                                  │   - menu: start/stop MCP server      │
│                                  │     (loopback Streamable HTTP)       │
├──────────────────────────────────┴──────────────────────────────────────┤
│ Educ8.Mcp (library)        MCP tools + Markdown formatting.              │
│                            Transport-agnostic: AddEduc8Tools() on any    │
│                            IMcpServerBuilder (stdio or HTTP)             │
├─────────────────────────────────────────────────────────────────────────┤
│ Educ8.Documents (library)  File bytes → Markdown parts + images          │
│                            (PDF text + page rendering, Office, text,     │
│                            images, zip). No itslearning/MCP/UI code.     │
├─────────────────────────────────────────────────────────────────────────┤
│ Educ8.Itslearning (library) No MCP, no UI.                               │
│   Auth/    OAuthClient, LoginService, TokenManager, ITokenStore,         │
│            ICallbackInbox (how the redirect URL reaches us)              │
│   Http/    ItslearningClient (typed REST, paging, 401→refresh+retry,     │
│            errors), BearerTokenHandler                                   │
│   Models/  DTOs for the endpoints we use                                 │
│   Content/ SSO web chain: pages, links, file downloads (+ cache)         │
└─────────────────────────────────────────────────────────────────────────┘
```

Rules:
- `Educ8.Itslearning` returns **domain objects** (DTOs). Formatting for an LLM (Markdown) lives in `Educ8.Mcp`. The desktop app renders the same objects with its own UI.
- Everything is wired through DI (`services.AddItslearning(...)`, `mcp.AddEduc8Tools()`), so a host only composes pieces.
- All libraries target `net10.0` with no platform-specific APIs, so MAUI, Avalonia or a console host can reference them.

## Shared session between the app and the MCP server

- **One per-user data directory**: `LocalApplicationData/Educ8` (Linux `~/.local/share/Educ8`), which can be overridden with `EDUC8_DATA_DIR`. It holds `config.json`, `tokens.json` (0600) and `callbacks/`.
- A user who logs in once (in the app or through MCP) is logged in for both.
- **Rotating refresh tokens + several processes = race.** If two processes refresh with the same refresh token, one of them fails. `TokenManager` therefore refreshes under a **cross-process file lock** (`tokens.lock`). Inside the lock it re-reads `tokens.json` first, and uses the stored token if another process already refreshed. Access tokens last 300 s.
- `ITokenStore` is an interface, so the plaintext-file store can later be replaced by OS secret storage (Keychain, Credential Manager, libsecret). The desktop app may provide its own.

## Login (authorization code flow)

Facts: redirect URI `itsl-itslearning://login`. itslearning's final page runs `top.location.replace('itsl-itslearning://login/?state=…&code=…')`, so there is no HTTP redirect we can catch on localhost. See [live-findings.md](../api/live-findings.md).

`LoginService.Begin()` creates a state and returns the authorize URL. `LoginService.CompleteAsync(redirectUrl)` checks the state, exchanges the code and saves the tokens. How the redirect URL reaches `CompleteAsync` depends on the host:

| Host | How the `itsl-itslearning://` URL is received |
|---|---|
| Desktop app (later) | Embedded WebView. Cancel the navigation to the `itsl-itslearning` scheme and take the URL. Cleanest; no OS setup |
| MCP server, with URL handler registered | `Educ8.Mcp.Server register-url-handler` registers the OS handler for `itsl-itslearning` (Linux `.desktop` + `xdg-mime`; Windows `HKCU\Software\Classes`; macOS needs an app bundle, so not supported). The browser opens `Educ8.Mcp.Server handle-url <url>`, which drops the URL in `callbacks/<state>.url`. The waiting server (`FileCallbackInbox`) picks it up |
| MCP server, fallback | The user copies the final `itsl-itslearning://login/?…` URL and the `login_complete` tool takes it |

Note: registering the handler takes over the `itsl-itslearning` scheme on that machine. The official itslearning app is mobile-only, so this rarely clashes on desktops.

## MCP hosting

- **Now:** `Educ8.Mcp.Server`, a console app on the stdio transport. MCP clients launch it (`claude mcp add …`, Claude Desktop config).
- **Later, inside the desktop app:** the app hosts the same tools over **Streamable HTTP on loopback** (`ModelContextProtocol.AspNetCore` + Kestrel inside the app process), started and stopped from a menu. It reuses the app's logged-in `TokenManager`/`ItslearningClient`. Following the MCP site's guidance, it should bind to `127.0.0.1` only, restrict `AllowedHosts`, and use a per-install bearer secret so other local processes can't use it. The stdio console host can still ship alongside it for clients that only support stdio. Both share the data directory.

## Desktop app framework (decide later)

Wanted: cross-platform, "probably MAUI". **.NET MAUI doesn't support Linux desktops** (Windows, macOS via Mac Catalyst, iOS and Android only), and the current dev machine runs Linux. Avalonia UI (or Uno Platform) covers Windows, macOS and Linux. The core libraries are UI-agnostic either way, so this can wait.

## Stack

- .NET 10 (`net10.0`), C#, nullable enabled. Central package management (`Directory.Packages.props`).
- `ModelContextProtocol` 2.4.x, `Microsoft.Extensions.Hosting`/`Http` 10.0.x.
- `System.Text.Json`. Enums the API sends as strings are kept as `string` in DTOs where values are unknown (tolerant parsing).
- Tests: xUnit, with a fake `HttpMessageHandler`.

## Solution layout

```
Educ8.slnx
Directory.Build.props / Directory.Packages.props
src/
  Educ8.Itslearning/      core library (no MCP, no UI)
  Educ8.Mcp/              MCP tools + formatting library
  Educ8.Mcp.Server/       console host: stdio MCP + CLI commands
  (Educ8.App/             desktop app — later)
tests/
  Educ8.Itslearning.Tests/
  Educ8.Mcp.Tests/
spikes/Phase0.Spike/      throwaway, not part of the solution
```

## Configuration

Sources, later ones overriding earlier: built-in defaults → `<data dir>/config.json` → environment variables.

| Setting | Env var | Default |
|---|---|---|
| BaseUrl | `ITSLEARNING_BASE_URL` | `https://sdu.itslearning.com` |
| ClientId | `ITSLEARNING_CLIENT_ID` | **required** (never hard-coded) |
| RedirectUri | `ITSLEARNING_REDIRECT_URI` | `itsl-itslearning://login` |
| Scope | `ITSLEARNING_SCOPE` | `All` |
| Data dir | `EDUC8_DATA_DIR` | `LocalApplicationData/Educ8` |

## Read vs write

The core (`ItslearningClient`) has both read and write methods (`UploadTempFileAsync`, `SubmitAnswerAsync`). The **MCP layer** gates writes. `Educ8McpOptions.AllowWriteActions` (off by default) decides whether `WriteTools` is registered at all, and each write tool re-checks it and previews unless `confirm=true`. The desktop app will gate writes with its own UI.

## Errors

`ItslearningClient` throws `ItslearningApiException` (status, path) and `NotAuthenticatedException`. `Educ8.Mcp` turns them into `McpException` with model-friendly messages, e.g. "Not logged in to itslearning. Call the `login` tool."

## Logging

The stdio host logs to **stderr only**. Tokens, codes, SSO URLs and signed file URLs are never logged.
