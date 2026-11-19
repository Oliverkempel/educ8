# itslearning REST API — overview

Source: https://school.itslearning.com/restapi/help (ASP.NET Web API help page). Studied 2026-11-18.

This file is the hand-written summary. **[live-findings.md](live-findings.md) has what live calls showed (2026-11-18) and takes precedence**: 5-minute tokens, rotating refresh tokens, `LearningToolId` meanings, and the SSO web chain for files and pages.

The full per-endpoint reference is auto-generated:

- [`endpoints/<Controller>.md`](endpoints/) — one file per controller: every endpoint with URI params, body, response fields, and JSON samples.
- [`models.md`](models.md) — every resource model / enum the endpoints reference (418 models). Search it by type name.
- [`_generator/`](_generator/) — the scripts that produced the above. See "Regenerating" below.

## Base facts

| Thing | Value |
|---|---|
| Base URL | `https://school.itslearning.com/` (your school's site) — paths below are relative to it, e.g. `GET https://school.itslearning.com/restapi/personal/person/v1` |
| Customer id | per site, from `restapi/sites/shortnames/<short name>/v1` |
| Personal REST API enabled | Yes on the site we used (`IsPersonalRestApiEnabled: true` from `restapi/sites/shortnames/<short name>/v1`) |
| Login | Federated only on the site we used (`IsNativeLoginDefault: false`): the school's Microsoft (Office 365) provider and "WAYF" |
| Formats | JSON (`application/json`) and XML. Use JSON. |
| Size | 413 endpoints in 55 controllers: 222 GET, 103 POST, 60 PUT, 25 DELETE, 3 PATCH |
| Path casing | Mostly `restapi/...`, a few documented as `RestApi/...` (assume case-insensitive) |

## Authentication

OAuth2. Unauthenticated calls to personal endpoints return **401**.

1. **Authorize** (browser): `https://<site>.itslearning.com/oauth2/authorize.aspx?client_id=<id>&state=<state>&response_type=code&scope=<space-separated scopes>&redirect_uri=<registered uri>`
2. **Token**: `POST restapi/oauth2/token`, `Content-Type: application/x-www-form-urlencoded`
   - first exchange: `client_id=...&grant_type=authorization_code&code=...`
   - refresh: `client_id=...&grant_type=refresh_token&refresh_token=...`
   - server-to-server: `grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion=...`
   - response: `{ access_token, token_type: "Bearer", expires_in, refresh_token }`. **Live: `expires_in` is 300 and the refresh token rotates**, see live-findings.md
3. Call APIs with `Authorization: Bearer <access_token>`.
4. Revoke: `DELETE restapi/oauth2/token/v1` (also disables push notifications if it was the last token).

Notes:
- **Project decision (2026-11-18):** we have a registered client ID and use this authorization code flow. On our site the sign-in step in (1) goes through **Microsoft (the school's Office 365)**, a federated provider on itslearning's login page. Microsoft only identifies the user. The tokens come from itslearning's `restapi/oauth2/token` and are the only ones the API accepts. See [../notes/decisions.md](../notes/decisions.md).
- Flow for the MCP server: build the authorize URL → user opens it in a browser → itslearning login → the school's Office 365 provider → Microsoft sign-in → redirect to `itsl-itslearning://login/?state=...&code=...` (delivered by page script, not a 302) → check `state` → exchange the code → store the (rotating) refresh token → refresh before `expires_in` (300 s) runs out.
- No client secret is needed (verified 2026-11-18).
- Some endpoints are marked "not available using OAuth2" (e.g. `POST restapi/personal/assignments/v1`, create assignment). Treat them as unavailable to us.

### Scopes

Listed on the help page: `All, Assignment, Calendar, Children, CkEditor, Courses, FileUpload, Hierarchies, LearningObjectiveRepository, LearningObjectivesReports, LightBulletin, Messages, Notifications, Page, Person, Planner, Sso, Statistics, StudentPlan, Supervisor, TaskListDailyWorkflow, Tasks, Workload, YourFiles`.

Inconsistencies: controller docs mention scopes `InstantMessage` and `Reports`, which are not in that list. The list has `Messages` and `LearningObjectivesReports` instead, which are probably the real names. Some controllers (Assignment, Page, LibraryPage) leave the scope name blank. Requesting `All` is probably simplest if the client registration allows it.

### Public (no auth) endpoints

- `Sites` — `restapi/sites/...`: look up sites by shortname, customer id, country; login details.
- `PageUrl` — `restapi/pageurl/...`: URL helpers.
- `NativeApp` — version check.

## Conventions

### Versioning
Every path ends in `/vN`. Several versions often exist side by side (e.g. `courses/v1..v4`). **Use the highest version** unless the docs say otherwise. Versions are not strict supersets: `courses/v3` includes task counts, but `v4` drops them for speed.

### Pagination
- Query params `PageIndex` (zero-based, default 0) and `PageSize` (default 100, **max 100**).
- Some newer endpoints use `paging.PageIndex` / `paging.PageSize`, or lowercase `pageIndex`/`pageSize`.
- Paged responses are wrapped in `EntityList<T>`:
  ```json
  { "EntityArray": [ ... ], "Total": 1, "CurrentPageIndex": 0, "PageSize": 100 }
  ```
- Feed-style endpoints (notifications, stream, bulletin comments) also support cursor paging: `FromId` + `UseNewerThan` (true = newer than FromId; false/default = older).
- Instant messages v3/v4 use their own cursor: `fromSortIndex`/`fromId` + `pageSize`, and the response has `hasMore`.

### Array query params
Collections are passed indexed: `groupIds[0]=1&groupIds[1]=2`. Some older ones take a plain repeated/comma param (`personIds=...`). Check each endpoint.

### IDs
- `courseId`, `personId`, `elementId` (a resource or element inside a course; unique per customer), `folderId`, `hierarchyId`, `threadId`, `notificationId`.
- Assignment and Page endpoints use `elementId` on purpose, to hide the internal `LearningObjectId`/`LearningObjectInstanceId`. LibraryPage uses `learningObjectInstanceId` directly.
- `*SyncKey` params (`courseSyncKey`, `personSyncKey`) are external sync ids from the SIS integration.

### Enums
Enums serialize as **strings** in JSON (e.g. `"EventType": "Course"`, `"ElementType": "Unknown"`), For query strings, the name (e.g. `status=Active`) should work; ASP.NET usually accepts the number too, but that is unverified. Enum values are in `models.md`. A few enum pages error on the help site; see "Unresolved models" at the bottom of `models.md`.

### File downloads
File endpoints (`.../files/{fileId}/v1`) answer with a **redirect** to a signed URL; clients must follow it. YourFiles download URLs are valid for 1 minute.

### File uploads
Two-step: upload to temp storage first (`POST restapi/personal/fileupload/temp/v1`, multipart/form-data, valid 24h, single-use), then send the returned `UploadedTempFile` (`FileRepoId`, `SignedFileRepoId`, ...) in the body of the real call (e.g. submit assignment answer).

### HTML content
Many text fields (descriptions, answers, bulletins) contain HTML. Some endpoints strip HTML and set `HasMoreContent: true`. An MCP server should convert HTML to text/markdown before returning it to a model.

## Controller map

Grouped by usefulness for a **student/teacher-facing MCP server**. Counts are endpoints.

### Core (likely MCP tools)

| Controller | # | What it gives |
|---|---|---|
| [Person](endpoints/Person.md) | 15 | Current user's profile (`person/v1`), other people, roles, features |
| [Courses](endpoints/Courses.md) | 64 | List courses (`courses/v2..v4`), course details, resources/folders, participants, topics, tasks, course stream, calendar events, search resources |
| [CourseCards](endpoints/CourseCards.md) | 5 | Starred/unstarred course cards (dashboard view) |
| [CourseFeatures](endpoints/CourseFeatures.md) | 2 | Which features a course has enabled |
| [Tasks](endpoints/Tasks.md) | 3 | User's tasks (`status`, `deadline`, `isHomework` filters), teacher follow-up tasks |
| [TaskListDailyWorkflow](endpoints/TaskListDailyWorkflow.md) | 6 | Tasks grouped into overdue/today/tomorrow/later |
| [Assignment](endpoints/Assignment.md) | 17 | Assignment details, settings, own answer, submit answer, download files, assessments (teacher) |
| [Calendar](endpoints/Calendar.md) | 3 | Calendar events list (`fromDate`) and details |
| [Notifications](endpoints/Notifications.md) | 12 | Bell notifications, news feed, stream, unread counts, mark read |
| [LightBulletin](endpoints/LightBulletin.md) / [LightBulletinCourse](endpoints/LightBulletinCourse.md) | 8 / 9 | Course announcements ("bulletins") and comments |
| [InstantMessage](endpoints/InstantMessage.md) | 46 | Messaging: threads, messages, search, send (`instantmessages/v2`), unread counts |
| [Page](endpoints/Page.md) | 26 | Page elements (rich content pages) and their content blocks, by `elementId` |
| [Folders](endpoints/Folders.md) | 4 | Get/create/update course folders |
| [Planner](endpoints/Planner.md) | 3 | Lesson plans for a course in a date range |
| [YourFiles](endpoints/YourFiles.md) | 36 | User's personal file area (private/public folders, download URLs) |
| [Hierarchies](endpoints/Hierarchies.md) | 12 | Org structure (school/site), members |
| [Sso](endpoints/Sso.md) | 1 | Short-lived SSO URL into itslearning web, useful for "open in browser" links |

### Secondary / role-specific

| Controller | # | Notes |
|---|---|---|
| [AssessmentRecord](endpoints/AssessmentRecord.md) | 10 | Gradebook (teacher) |
| [EmbeddedElement](endpoints/EmbeddedElement.md) | 6 | Embedded elements, assessments |
| [Workload](endpoints/Workload.md) | 5 | Student workload overviews (teacher/mentor) |
| [StudentPlan](endpoints/StudentPlan.md) | 8 | Student plan goals/assessments |
| [LearningObjectivesReports](endpoints/LearningObjectivesReports.md) | 9 | Learning objective progress reports |
| [Supervisor](endpoints/Supervisor.md) | 1 | Mentees |
| [PersonRelation](endpoints/PersonRelation.md), [SiteLinks](endpoints/SiteLinks.md) | 1 / 1 | Relations, site links |
| [Children*](endpoints/Children.md) (5 controllers) | 11 | Parent portal: children's courses/tasks/plans/calendar. Not relevant for students (parent portal disabled on our site) |
| [CoursesGroups](endpoints/CoursesGroups.md), [Projects](endpoints/Projects.md) | 2 / 1 | Course groups admin, project favourites |
| [FileUpload](endpoints/FileUpload.md) | 1 | Temp file upload (needed for submitting answers) |
| [LibraryPage](endpoints/LibraryPage.md) | 21 | Same as Page, for library items |
| [LtiExtension](endpoints/LtiExtension.md), [ImmersiveReader](endpoints/ImmersiveReader.md) | 2 / 1 | Integrations |

### Not useful for us (internal/infra/admin)

`Ai`, `ApplicationInitialisationCacheWarmUp`, `CustomLabelsApi`, `CustomerSettings`, `InstantMessageBroadcast`, `LearningObjectiveRepository`, `MediaRecorder`, `MobileSettings`, `NativeApp`, `NotificationsPost`, `Partner`, `ProductRelease`, `SavedCourseMaterials`, `SearchData`, `Statistics`, `Token` (used by auth layer only), `PageUrl`, `Sites` (only for discovery).

## Deprecated endpoints (avoid)

| Deprecated | Use instead |
|---|---|
| `GET restapi/personal/courses/v1` (bug: only returns favourites) | `courses/v2`/`v3`/`v4` |
| `GET restapi/personal/hierarchies/organizations/v1` | `hierarchies/organisations/v1` |
| `GET restapi/personal/instantmessages/messagethreads/{threadId}/v1` | `messagethreads/{threadId}/messages/v2` |
| `POST restapi/personal/instantmessages/v1` | `instantmessages/v2` |
| `GET`/`PUT restapi/personal/notifications/v1` | `notifications/v2` |
| `POST restapi/personal/courses/convertfileorlinkto*` | none (removal planned) |

26 endpoints have no description ("No documentation available"). Their shape is still in the generated reference.

## Regenerating the reference

```bash
python3 -I docs/api/_generator/crawl.py <crawl_dir>   # crawl_dir must contain index.html (curl https://school.itslearning.com/restapi/help)
python3 -I docs/api/_generator/parse.py <crawl_dir> docs/api
```

`crawl.py` caches pages in `<crawl_dir>/pages` and `<crawl_dir>/models`. It takes ~10 minutes because the help site is slow. Re-run when itslearning ships a release (their versioned CDN path, e.g. `v2026.11.4.2952`, shows the current build).
