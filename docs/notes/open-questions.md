# Open questions

Things we don't know yet that affect the design. Move answered items to [decisions.md](decisions.md) with the answer.

## Auth

Resolved 2026-11-18: we have a client ID and use the OAuth2 authorization code flow with Microsoft (Office 365) login. See [decisions.md](decisions.md).

Answered by Phase 0 (2026-11-18): the redirect URI is `itsl-itslearning://login`, no client secret is needed, scope `All` works, and the refresh token rotates. See [../api/live-findings.md](../api/live-findings.md).

1. ~~How does the MCP server capture the code?~~ Phase 1: an OS URL-scheme handler (Linux/Windows) plus a paste fallback; the desktop app will use its WebView. Still to check: whether browsers actually prompt for and launch the registered `itsl-itslearning` handler from `top.location.replace`, and how easy copying the link is in practice (Chrome shows nothing; the user needs the handler).
1b. ~~Desktop UI framework~~. Decided 2026-11-19: native per platform (GTK 4/libadwaita, AppKit, WinUI 3); see [../design/desktop-app-plan.md](../design/desktop-app-plan.md).
1c. ~~Does Microsoft's sign-in work inside an embedded WebKitGTK view?~~ Yes (2026-11-19, WebKitGTK 2.52, default user agent). The flow goes school.itslearning.com → login.microsoftonline.com → the school's Defender for Cloud Apps proxy (`*.access.mcas.ms`) → eu1.itslearning.com → school.itslearning.com → `itsl-itslearning://login`, which the navigation policy catches.
2. **Refresh-token lifetime**: how long the user can stay logged in without signing in again. Observe over days.
3. **Can a `prompt`/`login_hint`-style parameter preselect the school's Office 365 provider?** It would skip the provider picker. Nice to have.
4. ~~Can one SSO web session be reused?~~ Yes (2026-11-18). Lifetime is still unknown; expiry is detected and handled with a new SSO.
4b. Page block kinds other than `text` (files, images, rss, poll) haven't been seen live yet. They're passed through as HTML with a `[kind block]` note. Check them when a course has them.

## MCP server

6a. **Transport**: is stdio (local, personal use) acceptable for v1, or must it be hosted over HTTP for other users from the start? The proposal is stdio; see [../design/architecture.md](../design/architecture.md).
6b. Which MCP clients will be used (Claude Desktop, Claude Code, others)? This affects whether URL-mode elicitation can be relied on for login.
6c. HTML → Markdown library choice.

## Course content

6d. ~~No REST endpoint for course files and pages~~. Resolved 2026-11-18 with the SSO web chain (see live-findings.md). Still open: feedback/assessment files on assignments, and teacher-attached assignment `Files` (not yet seen live).
6e. What is `LearningToolId` 13221 (used for math hand-ins)?
6f. PowerPoint slides are read as text + embedded images, not rendered. If diagrams drawn with shapes matter, options are rendering through LibreOffice (heavy dependency) or asking teachers for PDFs. Revisit if it's a problem in practice.
6g. Images embedded in Page content blocks (`filerepository.itslearning.com` URLs) aren't fetched yet; Page images show as `[Image]`.

## Schedule and updates

8a. Does `notifications/stream/v2` only cover favourite courses (as documented)? `whats_new` says so and offers a `courseId` fallback. Check with a course that has activity but isn't a favourite.
8b. Does listing `notifications/v2` mark items read? It couldn't be tested because nothing was unread. Check the unread count before and after when there's an unread notification.
8c. Next read areas not done yet: announcement history and comments.
8d. Messages (desktop app, 2026-11-21): the `AttachmentUrl` format of message attachments (none in the test account), and live confirmation of `updatelastread` and `POST instantmessages/v2` (both unit-tested only, waiting for a real unread message and a send the user does themselves).

## Write actions

7a. `submit_assignment` hasn't been run for real yet (only preview, plus tests against a fake). Verify on a real assignment, with the user's consent: the multipart field name for `fileupload/temp/v1`, the request body casing, and the response.
7b. What happens on resubmission? Possibly a new answer version, a replaced answer, or a refusal, depending on the status scale ("Send igen"). Group assignments are untested too.
7c. Next write candidates: send a message, comment on an announcement, mark notifications read. Each must go in `WriteTools` with the same preview/confirm pattern.

## API behaviour (verify with live calls)

6. Do numeric enum values work in query strings, or only names?
7. Values for unresolved enums (`ContentAreaFilterType`, `TaskDeadlineFilter`, `SupervisorType`, `ResourceType`, `HierarchyRole`, `LightBulletinTimePeriod`). `courses/v2?filter=` defaults to `All`.
8. Rate limits: none documented.
9. Is the API identical on other itslearning sites? Assume yes (same product), with `BaseUrl` from `restapi/sites/...`. This would let the MCP server support any site, not just the school.
10. Which endpoints return HTML in text fields, and how much do they strip?

## Tests and CI

1. A web-content test (`WebContentTests`: `Finds_signed_tool_iframe_on_view_page` once, `Re_establishes_expired_web_session` once) failed twice in a full-solution run and then passed in 17+ reruns. The cause isn't found. CI keeps the `.trx` results of failed runs (artifact `test-results`) so the next occurrence can be diagnosed. Since tests gate releases, a flaky failure means re-running the workflow.
