# educ8-mcp docs

Project knowledge base for **educ8-mcp**, an MCP server for the itslearning LMS. Read this before working on the project, and keep it current.

## Contents

| Path | What |
|---|---|
| [api/README.md](api/README.md) | **Start here.** Hand-written overview of the itslearning REST API: base URL, OAuth2, scopes, pagination, versioning, conventions, controller map, deprecated endpoints |
| [api/live-findings.md](api/live-findings.md) | **What live calls showed** (auth details, element/tool types, the SSO web chain for files/pages, Office download proxy, plan↔folder mapping). Takes precedence over the help-page docs |
| [api/endpoints/](api/endpoints/) | Generated reference, one file per controller (55 files, 413 endpoints) |
| [api/models.md](api/models.md) | Generated reference of all request/response models and enums |
| [api/_generator/](api/_generator/) | Scripts that crawl the help page and regenerate the reference |
| [mcp/csharp-sdk.md](mcp/csharp-sdk.md) | How to build MCP servers in .NET: packages, setup, tools, errors, elicitation, auth guidance from modelcontextprotocol.io |
| [design/architecture.md](design/architecture.md) | Proposed architecture: .NET 10, stdio, solution layout, auth flow, tool design |
| [design/v1-study-assistant-plan.md](design/v1-study-assistant-plan.md) | **v1 plan**: read-only course study assistant — tools, phases, risks |
| [notes/open-questions.md](notes/open-questions.md) | Unknowns to resolve (auth is the big one) |
| [notes/decisions.md](notes/decisions.md) | Decisions log, newest first |
| [notes/mcp-tool-candidates.md](notes/mcp-tool-candidates.md) | Draft mapping of MCP tools to endpoints |

## Conventions for maintaining these docs

- Don't hand-edit `api/endpoints/` or `api/models.md`. Regenerate them (see the api README). Put corrections and live-test findings in `api/README.md` or `notes/`.
- When a live call contradicts the help page, write it down in `api/README.md` and mark it "verified YYYY-MM-DD".
- Record decisions in `notes/decisions.md`. Move answered questions out of `open-questions.md`.
- Use absolute dates.
