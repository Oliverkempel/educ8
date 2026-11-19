// Phase 0 spike: throwaway CLI for exploring the itslearning REST API live.
// Not production code. See docs/design/v1-study-assistant-plan.md (Phase 0).
//
// Config: spikes/.secrets/config.json or env vars ITSLEARNING_CLIENT_ID, ITSLEARNING_REDIRECT_URI,
//         ITSLEARNING_BASE_URL, ITSLEARNING_SCOPE, ITSLEARNING_CLIENT_SECRET (optional).
// Tokens: spikes/.secrets/token.json. Responses: spikes/out/. Both git-ignored. Never print tokens.

using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

var spikes = FindSpikesDir();
var secrets = Directory.CreateDirectory(Path.Combine(spikes, ".secrets")).FullName;
var outDir = Directory.CreateDirectory(Path.Combine(spikes, "out")).FullName;
var cfg = Config.Load(Path.Combine(secrets, "config.json"));
var tokenPath = Path.Combine(secrets, "token.json");
var json = new JsonSerializerOptions { WriteIndented = true };

if (args.Length == 0) { Usage(); return 1; }
try
{
    switch (args[0])
    {
        case "config": PrintConfig(); return 0;
        case "auth-url": { var (url, state) = AuthUrl(); File.WriteAllText(Path.Combine(secrets, "state"), state); Console.WriteLine(url); return 0; }
        case "login": return await LoginLoopback();
        case "exchange": return await Exchange(args[1]);
        case "refresh": await Refresh(); Console.WriteLine("refreshed"); return 0;
        case "status": return await Status();
        case "get": return await Get(args[1], args.Length > 2 ? int.Parse(args[2]) : 3000);
        case "probe": return await Probe(args[1]);
        case "sso": return await Sso(args[1]);
        case "download": return await Download(args[1], args.Length > 2 ? args[2] : null);
        default: Usage(); return 1;
    }
}
catch (Exception e) { Console.Error.WriteLine($"ERROR: {e.GetType().Name}: {e.Message}"); return 2; }

void Usage() => Console.Error.WriteLine("""
    usage: Phase0.Spike <command>
      config                 show effective config (client id masked)
      auth-url               print authorize URL (state saved)
      login                  print authorize URL and wait on the loopback redirect URI for the code
      exchange <code|url>    exchange an auth code (or the full redirected URL) for tokens
      refresh                refresh the access token
      status                 token expiry + GET person/v1 as a check
      get <path> [maxChars]  GET restapi path with bearer, save JSON to out/, print a preview
      probe <url>            GET without following redirects, with and without bearer; print status/headers
      sso <itslearning-url>  get an SSO URL for it, follow with a cookie jar, save the final page
      download <path|url> [file]  GET following redirects with bearer, save bytes to out/
    """);

void PrintConfig()
{
    Console.WriteLine($"BaseUrl:     {cfg.BaseUrl}");
    Console.WriteLine($"ClientId:    {(string.IsNullOrEmpty(cfg.ClientId) ? "(missing)" : cfg.ClientId[..Math.Min(8, cfg.ClientId.Length)] + "…")}");
    Console.WriteLine($"RedirectUri: {cfg.RedirectUri ?? "(missing)"}");
    Console.WriteLine($"Scope:       {cfg.Scope}");
    Console.WriteLine($"Secret:      {(string.IsNullOrEmpty(cfg.ClientSecret) ? "(none)" : "(set)")}");
    Console.WriteLine($"Token file:  {(File.Exists(tokenPath) ? "present" : "absent")}");
}

(string url, string state) AuthUrl()
{
    cfg.Require();
    var state = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
    var q = HttpUtility.ParseQueryString("");
    q["client_id"] = cfg.ClientId; q["state"] = state; q["response_type"] = "code";
    q["scope"] = cfg.Scope; q["redirect_uri"] = cfg.RedirectUri;
    return ($"{cfg.BaseUrl}/oauth2/authorize.aspx?{q}", state);
}

async Task<int> LoginLoopback()
{
    var (url, state) = AuthUrl();
    var redirect = new Uri(cfg.RedirectUri!);
    if (!redirect.IsLoopback) { Console.Error.WriteLine("Redirect URI is not loopback; use auth-url + exchange."); return 1; }
    using var listener = new HttpListener();
    listener.Prefixes.Add($"{redirect.Scheme}://{redirect.Host}:{redirect.Port}/");
    listener.Start();
    Console.WriteLine("Open this URL in a browser and sign in:");
    Console.WriteLine(url);
    Console.WriteLine($"Waiting for redirect on {redirect} (5 min timeout)...");
    var ctxTask = listener.GetContextAsync();
    if (await Task.WhenAny(ctxTask, Task.Delay(TimeSpan.FromMinutes(5))) != ctxTask) { Console.Error.WriteLine("Timed out."); return 1; }
    var ctx = ctxTask.Result;
    var qs = ctx.Request.QueryString;
    var ok = qs["state"] == state && !string.IsNullOrEmpty(qs["code"]);
    var msg = ok ? "Signed in. You can close this tab." : $"Login failed: {qs["error"]} {qs["error_description"]}";
    var bytes = Encoding.UTF8.GetBytes($"<html><body><p>{WebUtility.HtmlEncode(msg)}</p></body></html>");
    ctx.Response.ContentType = "text/html"; await ctx.Response.OutputStream.WriteAsync(bytes); ctx.Response.Close();
    if (!ok) { Console.Error.WriteLine(msg + (qs["state"] != state ? " (state mismatch)" : "")); return 1; }
    return await Exchange(qs["code"]!);
}

async Task<int> Exchange(string codeOrUrl)
{
    var code = codeOrUrl;
    if (codeOrUrl.Contains("code="))
    {
        var q = HttpUtility.ParseQueryString(new Uri(codeOrUrl).Query);
        var saved = Path.Combine(secrets, "state");
        if (File.Exists(saved) && q["state"] != File.ReadAllText(saved).Trim()) { Console.Error.WriteLine("State mismatch."); return 1; }
        code = q["code"]!;
    }
    var form = new Dictionary<string, string> { ["client_id"] = cfg.ClientId!, ["grant_type"] = "authorization_code", ["code"] = code };
    if (!string.IsNullOrEmpty(cfg.RedirectUri)) form["redirect_uri"] = cfg.RedirectUri;
    await TokenRequest(form);
    Console.WriteLine("Tokens saved.");
    return await Status();
}

async Task Refresh()
{
    var tok = LoadToken() ?? throw new InvalidOperationException("No token; log in first.");
    await TokenRequest(new() { ["client_id"] = cfg.ClientId!, ["grant_type"] = "refresh_token", ["refresh_token"] = tok["refresh_token"]!.GetValue<string>() });
}

async Task TokenRequest(Dictionary<string, string> form)
{
    if (!string.IsNullOrEmpty(cfg.ClientSecret)) form["client_secret"] = cfg.ClientSecret;
    using var http = new HttpClient();
    var res = await http.PostAsync($"{cfg.BaseUrl}/restapi/oauth2/token", new FormUrlEncodedContent(form));
    var body = await res.Content.ReadAsStringAsync();
    if (!res.IsSuccessStatusCode)
        throw new InvalidOperationException($"Token endpoint {(int)res.StatusCode}: {Redact(body)}");
    var node = JsonNode.Parse(body)!.AsObject();
    node["obtained_at"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    File.WriteAllText(tokenPath, node.ToJsonString(json));
    try { File.SetUnixFileMode(tokenPath, UnixFileMode.UserRead | UnixFileMode.UserWrite); } catch { }
    Console.WriteLine($"token_type={node["token_type"]} expires_in={node["expires_in"]} refresh_token={(node["refresh_token"] is null ? "absent" : "present")} keys=[{string.Join(",", node.Select(p => p.Key))}]");
}

JsonObject? LoadToken() => File.Exists(tokenPath) ? JsonNode.Parse(File.ReadAllText(tokenPath))!.AsObject() : null;

async Task<string> AccessToken()
{
    var tok = LoadToken() ?? throw new InvalidOperationException("No token; log in first.");
    var expires = tok["obtained_at"]!.GetValue<long>() + tok["expires_in"]!.GetValue<long>();
    if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expires - 60) { await Refresh(); tok = LoadToken()!; }
    return tok["access_token"]!.GetValue<string>();
}

async Task<int> Status()
{
    var tok = LoadToken();
    if (tok is null) { Console.WriteLine("Not logged in."); return 1; }
    var expires = DateTimeOffset.FromUnixTimeSeconds(tok["obtained_at"]!.GetValue<long>() + tok["expires_in"]!.GetValue<long>());
    Console.WriteLine($"Access token expires {expires:u}");
    return await Get("restapi/personal/person/v1", 600);
}

async Task<HttpClient> Api(bool follow = true, CookieContainer? cookies = null)
{
    var h = new HttpClientHandler { AllowAutoRedirect = follow };
    if (cookies != null) { h.CookieContainer = cookies; h.UseCookies = true; }
    var c = new HttpClient(h) { BaseAddress = new Uri(cfg.BaseUrl + "/") };
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await AccessToken());
    return c;
}

async Task<int> Get(string path, int max)
{
    using var c = await Api();
    var res = await c.GetAsync(path.TrimStart('/'));
    var body = await res.Content.ReadAsStringAsync();
    Console.WriteLine($"{(int)res.StatusCode} {res.Content.Headers.ContentType} {body.Length} chars  GET {path}");
    string pretty = body;
    try { pretty = JsonNode.Parse(body)!.ToJsonString(json); } catch { }
    var file = Path.Combine(outDir, Safe(path) + ".json");
    File.WriteAllText(file, pretty);
    Console.WriteLine($"saved {Path.GetRelativePath(spikes, file)}");
    Console.WriteLine(pretty.Length > max ? pretty[..max] + $"\n... ({pretty.Length - max} more chars)" : pretty);
    return res.IsSuccessStatusCode ? 0 : 1;
}

async Task<int> Probe(string url)
{
    foreach (var bearer in new[] { true, false })
    {
        using var h = new HttpClientHandler { AllowAutoRedirect = false };
        using var c = new HttpClient(h);
        var req = new HttpRequestMessage(HttpMethod.Get, Abs(url));
        if (bearer) req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await AccessToken());
        var res = await c.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
        Console.WriteLine($"[{(bearer ? "bearer" : "anon")}] {(int)res.StatusCode} {res.ReasonPhrase}");
        Console.WriteLine($"  Content-Type: {res.Content.Headers.ContentType}  Length: {res.Content.Headers.ContentLength}");
        if (res.Content.Headers.ContentDisposition is { } cd) Console.WriteLine($"  Content-Disposition: {cd}");
        if (res.Headers.Location is { } loc) Console.WriteLine($"  Location: {RedactUrl(loc.ToString())}");
        if (res.Headers.TryGetValues("Set-Cookie", out var sc)) Console.WriteLine($"  Set-Cookie names: {string.Join(", ", sc.Select(s => s.Split('=')[0]))}");
    }
    return 0;
}

async Task<int> Sso(string target)
{
    using var c = await Api();
    var res = await c.GetAsync($"restapi/personal/sso/url/v1?url={Uri.EscapeDataString(Abs(target))}");
    var body = await res.Content.ReadAsStringAsync();
    if (!res.IsSuccessStatusCode) { Console.WriteLine($"{(int)res.StatusCode} {body}"); return 1; }
    var ssoUrl = JsonNode.Parse(body)!["Url"]!.GetValue<string>();
    var jar = new CookieContainer();
    using var h = new HttpClientHandler { CookieContainer = jar, UseCookies = true, AllowAutoRedirect = true };
    using var web = new HttpClient(h);
    web.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (educ8-mcp spike)");
    var page = await web.GetAsync(ssoUrl);
    var html = await page.Content.ReadAsStringAsync();
    Console.WriteLine($"{(int)page.StatusCode} {page.Content.Headers.ContentType} final={RedactUrl(page.RequestMessage!.RequestUri!.ToString())} {html.Length} chars");
    Console.WriteLine($"cookies: {string.Join(", ", jar.GetAllCookies().Select(k => k.Name))}");
    var file = Path.Combine(outDir, "sso_" + Safe(target) + ".html");
    File.WriteAllText(file, html);
    Console.WriteLine($"saved {Path.GetRelativePath(spikes, file)}");

    // Follow the extension iframe (learning tool content) with the same cookie jar.
    var m = System.Text.RegularExpressions.Regex.Match(html, "<iframe[^>]*\\bsrc=\"([^\"]+)\"[^>]*ExtensionIframe|<iframe[^>]*id=\"[^\"]*ExtensionIframe\"[^>]*\\bsrc=\"([^\"]+)\"");
    if (!m.Success) m = System.Text.RegularExpressions.Regex.Match(html, "<iframe[^>]*\\bsrc=\"(https://platform[^\"]+)\"");
    if (!m.Success) { Console.WriteLine("no extension iframe"); return 0; }
    var src = WebUtility.HtmlDecode(m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value);
    var fr = await web.GetAsync(src);
    var fhtml = await fr.Content.ReadAsStringAsync();
    Console.WriteLine($"iframe: {(int)fr.StatusCode} {fr.Content.Headers.ContentType} final={RedactUrl(fr.RequestMessage!.RequestUri!.ToString())} {fhtml.Length} chars");
    var ffile = Path.Combine(outDir, "sso_iframe_" + Safe(target) + ".html");
    File.WriteAllText(ffile, fhtml);
    Console.WriteLine($"saved {Path.GetRelativePath(spikes, ffile)}");
    foreach (var link in System.Text.RegularExpressions.Regex.Matches(fhtml, "(?:href|src|data-[a-z-]+)=\"([^\"]*(?:[Dd]ownload|[Ff]ile[Rr]epo|filerepository|\\.pdf|GetFile|ViewFile)[^\"]*)\"").Select(x => WebUtility.HtmlDecode(x.Groups[1].Value)).Distinct().Take(15))
        Console.WriteLine($"  link: {RedactUrl(link)}");
    // Optionally try downloading the first download-looking link.
    var dl = System.Text.RegularExpressions.Regex.Matches(fhtml, "href=\"([^\"]*[Dd]ownload[^\"]*)\"").Select(x => WebUtility.HtmlDecode(x.Groups[1].Value)).FirstOrDefault();
    if (dl is null)
    {
        // No visible link (Office files): try the download proxy with the ids from the tool URL.
        var q = System.Web.HttpUtility.ParseQueryString(fr.RequestMessage!.RequestUri!.Query);
        if (q["LearningObjectId"] is { } lo && q["LearningObjectInstanceId"] is { } loi)
        {
            dl = $"/Proxy/DownloadRedirect.ashx?LearningObjectId={lo}&LearningObjectInstanceId={loi}";
            Console.WriteLine("  (constructed download proxy URL from tool ids)");
        }
    }
    if (dl != null)
    {
        var dlUri = new Uri(fr.RequestMessage!.RequestUri!, dl);
        var dres = await web.GetAsync(dlUri, HttpCompletionOption.ResponseHeadersRead);
        Console.WriteLine($"download: {(int)dres.StatusCode} {dres.Content.Headers.ContentType} len={dres.Content.Headers.ContentLength} disp={dres.Content.Headers.ContentDisposition} final={RedactUrl(dres.RequestMessage!.RequestUri!.GetLeftPart(UriPartial.Path))}");
        var bfile = Path.Combine(outDir, "ssodl_" + Safe(target));
        await using (var fs = File.Create(bfile)) await dres.Content.CopyToAsync(fs);
        Console.WriteLine($"saved {Path.GetRelativePath(spikes, bfile)} ({new FileInfo(bfile).Length} bytes)");
    }
    return 0;
}

async Task<int> Download(string pathOrUrl, string? name)
{
    using var c = await Api();
    var res = await c.GetAsync(Abs(pathOrUrl), HttpCompletionOption.ResponseHeadersRead);
    Console.WriteLine($"{(int)res.StatusCode} {res.Content.Headers.ContentType} len={res.Content.Headers.ContentLength} final={RedactUrl(res.RequestMessage!.RequestUri!.ToString())}");
    if (res.Content.Headers.ContentDisposition is { } cd) Console.WriteLine($"Content-Disposition: {cd}");
    var file = Path.Combine(outDir, name ?? ("dl_" + Safe(pathOrUrl)));
    await using (var fs = File.Create(file)) await res.Content.CopyToAsync(fs);
    Console.WriteLine($"saved {Path.GetRelativePath(spikes, file)} ({new FileInfo(file).Length} bytes)");
    return res.IsSuccessStatusCode ? 0 : 1;
}

string Abs(string u) => u.StartsWith("http") ? u : $"{cfg.BaseUrl}/{u.TrimStart('/')}";
static string Safe(string s) { var t = new string(s.Select(ch => char.IsLetterOrDigit(ch) || ch is '.' or '-' ? ch : '_').ToArray()); return t.Length > 120 ? t[..120] : t; }
static string Redact(string s) => System.Text.RegularExpressions.Regex.Replace(s, "(\"(?:access_token|refresh_token)\"\\s*:\\s*\")[^\"]+", "$1***");
static string RedactUrl(string s) => System.Text.RegularExpressions.Regex.Replace(s, "([?&](?:token|code|sig|sign|signature|ticket|access_token|ApiSessionId|SAMLRequest|SAMLResponse)=)[^&]+", "$1***", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

static string FindSpikesDir()
{
    for (var d = new DirectoryInfo(Directory.GetCurrentDirectory()); d != null; d = d.Parent)
    {
        if (d.Name == "spikes") return d.FullName;
        var s = Path.Combine(d.FullName, "spikes");
        if (Directory.Exists(s)) return s;
    }
    throw new InvalidOperationException("Run from inside the repo (spikes/ not found).");
}

record Config(string BaseUrl, string? ClientId, string? RedirectUri, string Scope, string? ClientSecret)
{
    public static Config Load(string path)
    {
        var f = File.Exists(path) ? JsonNode.Parse(File.ReadAllText(path))!.AsObject() : new JsonObject();
        string? V(string env, string key) => Environment.GetEnvironmentVariable(env) is { Length: > 0 } e ? e : f[key]?.GetValue<string>();
        return new(
            (V("ITSLEARNING_BASE_URL", "BaseUrl") ?? throw new InvalidOperationException("Set ITSLEARNING_BASE_URL or \"BaseUrl\" in spikes/.secrets/config.json.")).TrimEnd('/'),
            V("ITSLEARNING_CLIENT_ID", "ClientId"),
            V("ITSLEARNING_REDIRECT_URI", "RedirectUri"),
            V("ITSLEARNING_SCOPE", "Scope") ?? "All",
            V("ITSLEARNING_CLIENT_SECRET", "ClientSecret"));
    }
    public void Require()
    {
        if (string.IsNullOrEmpty(ClientId)) throw new InvalidOperationException("ClientId missing (spikes/.secrets/config.json or ITSLEARNING_CLIENT_ID).");
        if (string.IsNullOrEmpty(RedirectUri)) throw new InvalidOperationException("RedirectUri missing (spikes/.secrets/config.json or ITSLEARNING_REDIRECT_URI).");
    }
}
