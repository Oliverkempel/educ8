# educ8-mcp

MCP server for the itslearning LMS (first target: https://sdu.itslearning.com). Later part of a cross-platform desktop itslearning app (not built yet; see the architecture doc).

Project docs live in [docs/](docs/README.md). Before working here, read `docs/README.md`, `docs/design/architecture.md`, `docs/api/README.md` and `docs/api/live-findings.md`. Live findings are verified behaviour and win over the help-page docs. For endpoint details, grep `docs/api/endpoints/` and `docs/api/models.md` instead of fetching the help site.

## Code

- `Educ8.slnx`: `dotnet build Educ8.slnx`, `dotnet test Educ8.slnx`. .NET 10, central package versions in `Directory.Packages.props`, warnings are errors.
- `src/Educ8.Itslearning`: core (auth, tokens, REST client, models, `Content/` = SSO web chain for Pages, Links and Files). **No MCP or UI dependencies.** The desktop app will reuse it. All knowledge of itslearning's web markup stays in `Content/WebPageParsers.cs`.
- Test fixtures (`tests/Educ8.Itslearning.Tests/Fixtures`) are real pages: scrub personal data (name, email, person/user ids, signatures, viewstate) and grep before adding one.
- `src/Educ8.Documents`: turns downloaded files into Markdown parts + images (PdfPig, OpenXml, PDFtoImage for page rendering). No itslearning/MCP/UI dependencies. Its tests build documents in code (`tests/Educ8.Documents.Tests/TestFiles.cs`); never commit real course files.
- `src/Educ8.App.Core`: desktop-app view models (CommunityToolkit.Mvvm), shared strings (`Resources/Strings.resx`), `AppIcon`, platform service interfaces. **All app behaviour goes here; platform UIs (GTK/AppKit/WinUI, planned) only bind.** Follow the rules in `docs/design/desktop-app-plan.md` ("Keeping three UIs maintainable") and update `docs/design/desktop-parity.md`. Long-lived objects take `Func<ItslearningClient>`, never an `ItslearningClient` instance.
- `src/Educ8.Mcp`: MCP tools (`[McpServerToolType]` static classes, registered in `AddEduc8Tools`) + Markdown formatting. Transport-agnostic.
- `src/Educ8.Mcp.Server`: `educ8-mcp` stdio host + CLI. **Never write to stdout in serve mode**; log to stderr.
- `spikes/Phase0.Spike`: throwaway CLI for live API experiments; not in the solution. `spikes/.secrets` and `spikes/out` are git-ignored and contain tokens and personal data.

**Write actions:** anything that changes itslearning goes in `Tools/WriteTools.cs` (registered only when `Educ8McpOptions.AllowWriteActions`). It must call `EnsureAllowed`, be `Destructive = true`, and preview unless `confirm=true`. Read tools must stay `ReadOnly = true` (a test checks this). Never run a real write against the user's itslearning without their explicit consent.

Gotcha: register tools with `WithTools(IEnumerable<Type>)`. A `Type[]` binds to the generic `WithTools<T>(T target)` overload and silently registers nothing.

## Conventions

- Auth: itslearning OAuth2 authorization code flow with our client ID (from config/env, never hard-coded). Users sign in through Microsoft (SDU Office 365). Access tokens last 5 minutes and refresh tokens rotate, so refresh only through `TokenManager` (it uses a cross-process lock).
- Never log tokens, auth codes, SSO URLs or signed file URLs.
- Keep `docs/` current: decisions in `docs/notes/decisions.md`, open questions in `docs/notes/open-questions.md`, live-API findings in `docs/api/live-findings.md`.
