# Educ8

A native desktop app and an MCP server for the itslearning learning platform.

- **Desktop app** (Linux, GTK 4 + libadwaita): your courses, course pages with plans and resources, a week calendar with lesson plans and material. Files open in your default apps.
- **MCP server** (`educ8-mcp`): lets AI assistants like Claude read your courses, material (PDF, Office files, images, …), schedule, assignments and what's new. It's read-only unless you allow changes.
- Both share one login. The app's **MCP server** page shows which AI apps are connected, turns tools on and off, and lists recent tool calls.

Windows and macOS apps are planned. The MCP server already runs on all three.

## Getting started

### 1. Install

Download the latest release from the repository's **Releases** page:

| You want | Download |
|---|---|
| The Linux app (includes `educ8-mcp`) | `educ8-<version>-x86_64.flatpak`, then `flatpak install --user educ8-<version>-x86_64.flatpak`; or `educ8-<version>-linux-x64.tar.gz`, then unpack it and run `./install.sh` |
| Only the MCP server | `educ8-mcp-<version>-<platform>` (Linux, Windows, macOS; one self-contained file) |

The tar.gz needs GTK 4, libadwaita and WebKitGTK 6.0. On Ubuntu: `sudo apt install libgtk-4-1 libadwaita-1-0 libwebkitgtk-6.0-4`. The Flatpak brings its own. The macOS and Windows builds of the MCP server aren't signed yet: on macOS run `xattr -d com.apple.quarantine educ8-mcp` once.

### 2. Configure

Create `~/.local/share/Educ8/config.json` (Windows: `%LOCALAPPDATA%\Educ8\config.json`) with your school's itslearning site and an itslearning OAuth client id:

```json
{
  "BaseUrl": "https://yourschool.itslearning.com",
  "ClientId": "<your client id>"
}
```

The client id must be registered with itslearning for the redirect URI `itsl-itslearning://login`.

### 3. Sign in

Open **Educ8** and choose **Sign in with itslearning**. That's it.

Without the app, run `educ8-mcp login` and sign in in the browser. To let the browser finish the login by itself, register the link handler once with `educ8-mcp register-url-handler` (Linux, Windows); otherwise paste the final `itsl-itslearning://…` link into the terminal.

### 4. Connect Claude

Claude Desktop: add the server to `~/.config/Claude/claude_desktop_config.json` (Settings → Developer → Edit Config) and restart Claude:

```json
{
  "mcpServers": {
    "itslearning": { "command": "/home/<you>/.local/opt/educ8/educ8-mcp" }
  }
}
```

Use the path to `educ8-mcp` from your install: tar.gz `~/.local/opt/educ8/educ8-mcp`, or the file you downloaded. For the Flatpak, use `"command": "flatpak"` with `"args": ["run", "--command=educ8-mcp", "io.github.educ8.Educ8"]`.

Claude Code: `claude mcp add itslearning -- <path to educ8-mcp>`.

Then ask things like *"What do I have this week?"*, *"Summarize this week's lecture slides in Statistics"* or *"What's new in my courses?"*.

### 5. Control what the AI can do

In the app, open **MCP server**:

- **Allow changes in itslearning:** off by default. When on, the AI can hand in assignments. It always shows a preview and asks first.
- **Tools:** turn single tools off. They disappear for the AI right away.
- **Status and recent activity:** which AI apps are connected and what they called.

Changes apply immediately, also to running servers. Without the app: `educ8-mcp write-actions on|off`.

## Settings

All settings live in `config.json` (above). Environment variables override them.

| Setting | `config.json` | Environment |
|---|---|---|
| Site | `BaseUrl` (required) | `ITSLEARNING_BASE_URL` |
| Client id | `ClientId` (required) | `ITSLEARNING_CLIENT_ID` |
| Allow changes | `AllowWriteActions` | `EDUC8_ALLOW_WRITE_ACTIONS` |
| Turned-off tools | `DisabledTools` | |
| Data folder | | `EDUC8_DATA_DIR` |

The data folder holds the login (`tokens.json`, readable only by you), settings and the MCP activity log. Nothing leaves your machine except calls to itslearning.

## Build from source

Needs the .NET 10 SDK (and the GTK libraries above for the app).

```bash
dotnet build Educ8.slnx
dotnet test Educ8.slnx
dotnet run --project src/Educ8.App.Gtk        # the app
dotnet run --project src/Educ8.Mcp.Server     # the MCP server (stdio); `-- --help` for commands
```

Pull requests are built and tested by GitHub Actions; when one lands in `main`, a release is built. Versions are dates: `2026.11.19`, then `2026.11.19.1` for a later release that day. `packaging/flatpak/build-local.sh` builds the Flatpak locally.

## Project layout

| Path | What |
|---|---|
| `src/Educ8.Itslearning` | itslearning core: login, tokens, REST client, file downloads |
| `src/Educ8.Documents` | Files → Markdown and images (PDF, Office, text, images, zip) |
| `src/Educ8.Mcp`, `src/Educ8.Mcp.Server` | MCP tools and the `educ8-mcp` host and CLI |
| `src/Educ8.App.Core` | Desktop app view models, shared by every platform UI |
| `src/Educ8.App.Gtk` | Linux app (GTK 4 + libadwaita) |
| `packaging/`, `.github/workflows/` | Linux packaging (tar.gz, Flatpak) and CI/releases |
| `docs/` | Design notes, API findings and decisions; start at [docs/README.md](docs/README.md) |
