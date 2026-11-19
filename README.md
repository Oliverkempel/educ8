# educ8-mcp

An MCP server that lets an AI assistant read your itslearning courses (first target: SDU, `https://sdu.itslearning.com`). Built with .NET 10 and the official MCP C# SDK. Later it will be part of a desktop itslearning app.

Status: login, course overview (including each lesson plan's material), browsing and searching course material, reading Pages and Links, and **reading files**: PDF (text, plus rendered pages on request), Word, PowerPoint, Excel, text/code, notebooks, images and zip archives. Also tasks and **assignments**: details, your submission, status, score and the teacher's feedback (see [docs/design/v1-study-assistant-plan.md](docs/design/v1-study-assistant-plan.md)).

## Install (releases)

Every push to `main` builds a release on the repository's **Releases** page (versions are dates: `2026.11.19`, then `2026.11.19.1`, … for later releases that day). Each release has:

| File | What |
|---|---|
| `educ8-<version>-linux-x64.tar.gz`, `…-linux-arm64.tar.gz` | Linux desktop app + `educ8-mcp`. Unpack and run `./install.sh` (installs to `~/.local/opt/educ8`, adds it to the app grid; `./uninstall.sh` removes it). Needs GTK 4, libadwaita and WebKitGTK 6.0: `sudo apt install libgtk-4-1 libadwaita-1-0 libwebkitgtk-6.0-4` (Ubuntu/Debian) or `sudo dnf install gtk4 libadwaita webkitgtk6.0` (Fedora). |
| `educ8-<version>-x86_64.flatpak`, `…-aarch64.flatpak` | The same app as a Flatpak (brings its own GNOME libraries): `flatpak install --user educ8-<version>-x86_64.flatpak`. |
| `educ8-mcp-<version>-<platform>` | Just the MCP server, one self-contained file, for `linux-x64`, `linux-arm64`, `win-x64` (zip), `osx-arm64`, `osx-x64`. |
| `SHA256SUMS` | Checksums of all of the above. |

Then set the client id (see [Configure](#configure)) and point your MCP client at the server:

| Installed as | `command` for Claude Desktop |
|---|---|
| tar.gz | `~/.local/opt/educ8/educ8-mcp` (write out your home folder) |
| Flatpak | `flatpak` with `args: ["run", "--command=educ8-mcp", "io.github.educ8.Educ8"]` |
| MCP server file | wherever you unpacked `educ8-mcp` |

The Flatpak app shares `~/.local/share/Educ8` (login, settings, MCP activity) with an `educ8-mcp` running outside it. The macOS and Windows builds aren't signed yet: on macOS run `xattr -d com.apple.quarantine educ8-mcp` once; on Windows choose "More info → Run anyway" if SmartScreen asks. `educ8-mcp --version` shows the version.

## Build and test

```bash
dotnet build Educ8.slnx
dotnet test Educ8.slnx
```

The debug build output is large (about 570 MB) because the PDF page renderer (PDFium + Skia) ships native libraries for every platform. For a lean install, publish for one platform:

```bash
dotnet publish src/Educ8.Mcp.Server -c Release -r linux-x64 --self-contained false -o ~/.local/share/Educ8/bin
```

## Configure

The server needs an itslearning OAuth client id. Set it with an environment variable, or put it in `<data dir>/config.json`:

```json
{ "ClientId": "<your client id>" }
```

| Setting | Env var | Default |
|---|---|---|
| Client id | `ITSLEARNING_CLIENT_ID` | (required) |
| Site | `ITSLEARNING_BASE_URL` | `https://sdu.itslearning.com` |
| Redirect URI | `ITSLEARNING_REDIRECT_URI` | `itsl-itslearning://login` |
| Scope | `ITSLEARNING_SCOPE` | `All` |
| Data dir | `EDUC8_DATA_DIR` | `~/.local/share/Educ8` (Linux), `%LOCALAPPDATA%\Educ8` (Windows) |

The data dir holds the login session (`tokens.json`, readable only by you). Everyone using the same data dir shares one login: the MCP server, the CLI and, later, the desktop app.

### Write actions (off by default)

The desktop app's **MCP server** page has a switch for this, plus a switch per tool, the running servers and the recent tool calls. Changes apply right away, also to servers that are already running.


The server is **read-only** unless you turn on write actions. These let the AI change things in itslearning, currently by handing in assignment answers:

```bash
educ8-mcp write-actions on
```

Turn them off again with `educ8-mcp write-actions off`. Check with `educ8-mcp write-actions`, or `"AllowWriteActions": true/false` in `config.json` (env `EDUC8_ALLOW_WRITE_ACTIONS` overrides it). Restart the MCP server afterwards. When enabled, every write first returns a preview, and the AI is told to ask you before confirming. Uploads are limited to non-hidden files given by absolute path.

## Log in

```bash
dotnet run --project src/Educ8.Mcp.Server -- login
```

The browser opens itslearning; choose **SDU Office 365**. After you sign in, itslearning hands the result to an `itsl-itslearning://` link. Either:

- register the link handler once, and the login then finishes by itself (Linux, Windows):

  ```bash
  dotnet run --project src/Educ8.Mcp.Server -- register-url-handler
  ```

- or copy the final `itsl-itslearning://login/?state=…&code=…` link and paste it into the terminal.

Inside an MCP client you can also ask the assistant to log in. The `login` and `login_complete` tools do the same steps.

Other commands: `status`, `logout`, `unregister-url-handler`.

## Use with Claude Code

```bash
dotnet build src/Educ8.Mcp.Server
claude mcp add itslearning --env ITSLEARNING_CLIENT_ID=<your client id> -- /ABSOLUTE/PATH/src/Educ8.Mcp.Server/bin/Debug/net10.0/educ8-mcp
```

Claude Desktop (`~/.config/Claude/claude_desktop_config.json`):

```json
{
  "mcpServers": {
    "itslearning": {
      "command": "/ABSOLUTE/PATH/src/Educ8.Mcp.Server/bin/Debug/net10.0/educ8-mcp",
      "env": { "ITSLEARNING_CLIENT_ID": "<your client id>" }
    }
  }
}
```

## Tools

| Tool | What it does |
|---|---|
| `auth_status` | Whether you're logged in, and as whom |
| `login` | Starts a login and returns the link to open |
| `login_complete` | Finishes a login from the pasted `itsl-itslearning://` link |
| `list_my_courses` | Your courses, most recently active first; optional `search` |
| `get_course_overview` | Topics, lesson plans around today, open tasks, latest announcements, top-level material |
| `browse_course_materials` | Contents of a course folder (or the top level) |
| `search_course_materials` | Find items by title anywhere in a course |
| `read_material` | Read one item: Pages as Markdown, Links as their URL, folders as a listing, files as below |
| `get_schedule` | Your calendar by day: lectures, exercises, rooms, teachers (default: the next 7 days; optional course) |
| `get_event` | One event, with its lesson plan and that plan's material |
| `whats_new` | Personal notifications plus recent course updates (new material, assignments, announcements) |
| `list_my_tasks` | Open or completed tasks (assignments, tests, activities) by course |
| `get_assignment` | An assignment: description, deadline, files, your submission, status, score, feedback |
| `read_assignment_file` | A file from an assignment: the teacher's, yours, or a feedback file |
| `submit_assignment` | **Write action, only when enabled.** Hand in answer text and/or local files. Previews first; submits only with `confirm=true` |
| `read_file` | Read a course file. `pages="1-10"` selects pages/slides/sheets; `includeImages=true` adds rendered PDF pages or embedded images; `zipEntry` reads a file inside a zip |

## Layout

See [docs/design/architecture.md](docs/design/architecture.md).

- `src/Educ8.Itslearning`: itslearning core (auth, tokens, REST client). No MCP and no UI, so the future desktop app can reuse it.
- `src/Educ8.Documents`: file → Markdown + images (PDF, Office, text, images, zip). No itslearning or MCP code.
- `src/Educ8.App.Core`: shared view models for the native desktop app (GTK on Linux; WinUI on Windows and AppKit on macOS are planned). See [docs/design/desktop-app-plan.md](docs/design/desktop-app-plan.md).
- `src/Educ8.App.Gtk`: the Linux desktop app (GTK 4 + libadwaita): courses and a week calendar. Run it with `dotnet run --project src/Educ8.App.Gtk`. It needs the GTK 4, libadwaita 1 and WebKitGTK 6.0 libraries, and shares its login with the MCP server.
- `src/Educ8.Mcp`: MCP tools and formatting, transport-agnostic.
- `src/Educ8.Mcp.Server`: stdio host + CLI (`educ8-mcp`).
- `tests/`: unit tests and an end-to-end MCP protocol test with a fake itslearning.
- `docs/`: project knowledge base, starting at [docs/README.md](docs/README.md).
