# Building MCP servers in .NET — reference notes

Condensed from the official sources, read 2026-11-18:

- MCP site, "Build an MCP server" (C# tab): https://modelcontextprotocol.io/docs/2026-07-28/develop/build-server
- MCP site, SDK list: https://modelcontextprotocol.io/docs/2026-07-28/sdk (C# SDK is **Tier 1**)
- MCP site, authorization tutorial: https://modelcontextprotocol.io/docs/2026-07-28/tutorials/security/authorization
- MCP site, local server security: https://modelcontextprotocol.io/docs/2026-07-28/tutorials/security/local-server-security
- C# SDK docs: https://csharp.sdk.modelcontextprotocol.io/v2/concepts/index.html
- C# SDK repo and samples: https://github.com/modelcontextprotocol/csharp-sdk (see `samples/QuickstartWeatherServer`)

Current protocol version on the site: **2026-07-28**.

## Packages (NuGet, latest stable 2.4.0)

| Package | Use when |
|---|---|
| `ModelContextProtocol.Core` | Client or low-level server APIs only, minimal dependencies |
| `ModelContextProtocol` | **stdio server** with hosting, DI, and attribute-based tool discovery. The default starting point |
| `ModelContextProtocol.AspNetCore` | HTTP (Streamable HTTP) server in ASP.NET Core. Includes the above |

Plus `Microsoft.Extensions.Hosting`. The MCP site quickstart still says `dotnet add package ModelContextProtocol --prerelease`; that flag is no longer needed now that 2.x is stable. Requires .NET 8+. (.NET SDK 10.0.115 is installed on the dev machine.)

## Minimal stdio server

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace); // all logs -> stderr

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";
}
```

The MCP site's version uses `Host.CreateEmptyApplicationBuilder(settings: null)` so that nothing is written to the console. Either way the rule is the same:

> **stdio servers must never write to stdout** (`Console.WriteLine` etc.). stdout carries the JSON-RPC stream. Log to stderr or a file.

## Minimal HTTP server (for later)

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMcpServer()
    .WithHttpTransport(o => o.SessionMode = HttpServerSessionMode.Stateless)
    .WithToolsFromAssembly();
var app = builder.Build();
app.MapMcp();
app.Run("http://localhost:3001");
```

Security notes for HTTP: restrict `AllowedHosts` to loopback names locally (Kestrel doesn't validate Host headers, which allows DNS rebinding). Don't enable CORS unless needed. Stateless is the default and recommended mode, but stateless servers **cannot call `ElicitAsync`** (see below).

## Tools

- `[McpServerToolType]` on a class, `[McpServerTool]` on each method, `[Description]` on the method and each parameter. `WithToolsFromAssembly()` finds them. Prompts and resources work the same way (`[McpServerPromptType]`/`[McpServerPrompt]`, `[McpServerResourceType]`/`[McpServerResource]`).
- Tool methods can take, besides their arguments: `McpServer`, `IProgress<ProgressNotificationValue>`, `ClaimsPrincipal`, `CancellationToken`, and **any DI-registered service** (e.g. our itslearning API client). These are not exposed to the model.
- `McpServerToolAttribute` has `Title`, `ReadOnly`, `Destructive`, `Idempotent`, `OpenWorld`. Set `ReadOnly = true` on read tools, and `Destructive`/`ReadOnly = false` on writes, so clients can ask the user before calling them.
- `UseStructuredContent = true` also serializes the return value to `structuredContent`.
- **Errors**: a thrown exception becomes a `CallToolResult` with `IsError = true`, which the model can see and react to. Only `McpException` messages are passed through; other exceptions become a generic message. So throw `McpException("Course 123 not found or no access")` for errors the model should understand.

## Elicitation, URL mode (relevant to itslearning login)

A server can ask the client to send the user to a URL. The spec names "third-party OAuth flows" as the use case, which matches our itslearning login.

- `server.ElicitAsync(new ElicitRequestParams { Mode = "url", Url = ..., ElicitationId = ..., Message = ... })`. The client shows the URL and asks the user for consent.
- Alternatively, throw `UrlElicitationRequiredException` (JSON-RPC error -32042) from a tool. The client completes the URL flow, then retries the call.
- Only works if the client advertises the `elicitation.url` capability. Many clients may not yet, so we need a fallback (return the login URL as tool text).
- `ElicitAsync` doesn't work in stateless HTTP mode (all 2026-07-28 Streamable HTTP requests are served stateless). It **does work over stdio**. On stateless HTTP, throw `InputRequiredException` or `UrlElicitationRequiredException` instead.

## Authorization guidance from the MCP site

- The MCP OAuth flow (server as protected resource, PRM, audience validation) is **for HTTP transports only**. "Servers that use the stdio transport should retrieve credentials from the environment instead", or from libraries embedded in the server.
- Calling another API (itslearning) needs **its own token**. Never pass the MCP client's token through to an upstream API ("token passthrough").
- A server that proxies OAuth with **one static client ID** (our case if hosted for many users) must get each user's consent per MCP client ("confused deputy").
- Never log tokens, `Authorization` headers, or auth codes.
- Local-server security: pass secrets per server, not through a global env. Keep long-lived secrets out of plaintext config files; prefer the OS secret store. The site also says a server that only wraps a remote API gains little from running locally. That favours HTTP **if** we ever distribute this to other users.

## Running it in a client

Claude Desktop (`~/.config/Claude/claude_desktop_config.json` on Linux):

```json
{
  "mcpServers": {
    "itslearning": {
      "command": "dotnet",
      "args": ["run", "--project", "/ABSOLUTE/PATH/TO/src/Educ8.Mcp.Server", "--no-build"]
    }
  }
}
```

Claude Code: `claude mcp add itslearning -- dotnet run --project /ABSOLUTE/PATH/TO/src/Educ8.Mcp.Server --no-build`.

Test and debug with the **MCP Inspector**: https://modelcontextprotocol.io/docs/2026-07-28/tools/inspector

## Other SDK features we may use

Progress notifications (long downloads), cancellation (pass `CancellationToken` through to `HttpClient`), resources (expose course files/pages as `itslearning://...` resources), pagination, logging to the client. The SDK can also serve MCP Apps, Tasks and Skills; we don't need those now.
