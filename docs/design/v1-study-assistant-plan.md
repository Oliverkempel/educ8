# v1 plan — course study assistant

Status: **proposed 2026-11-18**, awaiting approval. Builds on [architecture.md](architecture.md) (.NET 10, MCP C# SDK, stdio, itslearning OAuth with Microsoft login).

## Goal

A student is reading up on a course or working on an assignment with help from an LLM. The LLM should be able to, without the student copy-pasting anything:

1. find out which courses the student has and which one they mean,
2. understand the course: topics/plans, material structure, recent announcements, upcoming work,
3. find and **read course materials** (pages, notes, files, links),
4. read the **assignment** being worked on: description, deadline, attached files, rubric/scale, and the student's own submission and feedback.

**Read-only.** v1 does not submit, post or change anything. Student perspective only: no tools that expose other students' data (participants, gradebooks).

## Interaction shape

Typical session:

```
Student: "Help me with assignment 2 in Distributed Systems"
LLM → list_my_courses                       → finds course 12345
LLM → get_course_overview(12345)            → topics, folder tree, upcoming tasks (assignment 2 = element 678)
LLM → get_assignment(678)                   → description, deadline, attached spec.pdf (file 9)
LLM → read_file(assignment 678, file 9)     → text of spec.pdf
LLM → search_course_materials(12345, "consensus") → lecture page 555
LLM → read_material(555)                    → page content as Markdown
```

Tools are the main interface because every MCP client supports them. Two optional additions:

- MCP **prompts** (`study_course`, `work_on_assignment`): slash-command entry points that run the first steps for the student.
- MCP **resources** (`itslearning://course/{id}/element/{id}`): let clients that support resources attach materials directly.

## Tools (v1)

Every tool returns compact Markdown with IDs in brackets (e.g. `[element 678]`) so the model can chain calls. All are `ReadOnly = true`. Lists are capped, with a "N more, use page=2" note.

| Tool | Parameters | Backed by | Returns |
|---|---|---|---|
| `auth_status` / `login` | – | OAuth flow ([architecture.md](architecture.md)) | Whether logged in, as whom; starts login (URL elicitation or link) |
| `list_my_courses` | `includeInactive?` | `GET courses/v4` (+`person/v1` once) | Course id, code, title, last updated |
| `get_course_overview` | `courseId` | `courses/{id}/v1`, `courses/{id}/topics/v1`, `planner/v1?courseId=` (current ± weeks), root `courses/{id}/resources/v1`, `courses/{id}/bulletins/v1` (latest few), `courses/{id}/tasks/v1` | One orientation document: course info, topics/plans, top-level material, latest announcements, open tasks with deadlines |
| `browse_course_materials` | `courseId`, `folderId?` | `courses/{id}/resources/v1`, `courses/{id}/folders/{folderId}/resources/v1` | Folder listing: title, type, element id, path, homework flag |
| `search_course_materials` | `courseId`, `query` | `courses/{locationId}/resources/search/v1?locationType=Course&searchText=` | Matching elements |
| `read_material` | `elementId`, `courseId?` | `courses/resources/{id}/v1` to get the type, then dispatch (below) | The element's content as Markdown, or what can't be read plus a link |
| `list_my_tasks` | `courseId?`, `status?` | `tasks/v1` or `courses/{id}/tasks/v1` | Assignments/tests with deadlines and status |
| `get_assignment` | `elementId` | `assignments/{id}/v1`, `.../settings/v1`, `.../answers/persons/{me}/v1` | Description (RawText → MD), deadline, submission type, max score/scale, attached files (ids), template file, own submission + feedback + feedback files |
| `read_file` | `source` (assignment / answer / feedback / page block / element), ids, `pages?` | file download endpoints (follow redirect), then text extraction | Extracted text, paged for big documents ("pages 1–15 of 80") |

### `read_material` dispatch (updated after Phase 0)

Real courses use `ElementType: LearningToolElement`, with the kind given by `LearningToolId`. Details in [../api/live-findings.md](../api/live-findings.md).

| Kind | How |
|---|---|
| **Folder** (`Folder`) / LearningPath | List children (as `browse_course_materials`) |
| **Page** (tool 5) | REST Page API returns 500, so use the **SSO web chain** → `page.itslearning.com/PageInstance.aspx` → each `li.il-widget` (title `h2`, HTML in `.h-userinput`) → Markdown. Files and images in blocks are listed |
| **File** (tool 5009) | **SSO web chain** → `File/View.aspx` → `/Proxy/DownloadRedirect.ashx` → bytes → `read_file` extraction |
| **Link** (tool 5010) | SSO web chain → `Link/View.aspx` → external URL. Return title + URL; don't fetch external sites in v1 |
| **Assignment** (tool 5006) | REST, same as `get_assignment` |
| CustomActivity | Title + description from the tasks API |
| Test (5002), Survey (5050), other tools | Title, status, and an SSO "open in browser" link. Content not readable |

The SSO web chain is scraping of undocumented pages. Keep it behind one interface (`IWebContentFetcher`) with tests on saved HTML, and never log its URLs (they contain personal data and signatures).

## File text extraction

- PDF: `UglyToad.PdfPig`. DOCX/PPTX/XLSX: `DocumentFormat.OpenXml`. Plain text, Markdown, code, CSV, HTML: as-is or HTML → Markdown. (Library choices still to confirm.)
- Images and scanned PDFs: v1 says "no text layer". Returning images as MCP image content is a later improvement.
- Size control: return at most ~40k characters per call, with page ranges for paging. Cache extracted text in memory, keyed by file id, for the session.

## Phases

### Phase 0 — Live spikes ✅ done 2026-11-18
Result: OAuth works. All core REST endpoints work. Files, Pages and Links are readable through the SSO web chain. Assignments and own answers are readable through REST. See [../api/live-findings.md](../api/live-findings.md). Original plan kept below for reference.
Small throwaway console or `.http` experiments against SDU with your client ID. Record findings in `docs/api/README.md`.
1. **OAuth end to end**: authorize → Microsoft login → code → token → refresh. Settles the open questions on redirect URI, client secret and scopes.
2. Call the core endpoints on one real course: `courses/v4`, resources, topics, planner, a Page, an assignment, own answer. Compare real JSON with the docs, especially enum values and HTML in fields.
3. **File/Note content gap.** This is the main risk, because lecture slides and PDFs are usually `FolderFile` elements. Try in order:
   a. `ContentUrl`/`Url` from the resource, with the bearer token: does it redirect to a file?
   b. `sso/url/v1?url=<ContentUrl>` → follow with a cookie jar → find the download link on the web page.
   c. Look at what the official itslearning mobile app calls for files, since it can open them.
   If nothing works, v1 lists such files with an "open in browser" link, and only assignment and Page files are readable.

### Phase 1 — Skeleton + auth ✅ done 2026-11-18
Solution layout from [architecture.md](architecture.md) (desktop-ready); stdio host + CLI (`login`, `status`, `logout`, `register-url-handler`, `handle-url`); configuration (defaults → `config.json` → env); OAuth, `TokenManager` with cross-process refresh lock, file token store; `ItslearningClient` with paging, 401 → refresh + retry, and error mapping. Tools: `auth_status`, `login`, `login_complete`, `list_my_courses`. 44 tests, including an end-to-end MCP protocol test. Verified live against SDU with the real stdio server (login → courses). Not yet tried in Claude Code/Desktop, and the URL-scheme handler hasn't been registered on a real desktop.

### Phase 2 — Understanding the course ✅ done 2026-11-18
`get_course_overview` (topics with folder ids, lesson plans from 1 week back to N weeks ahead, open tasks, 5 latest announcements, top-level material; a section that fails shows a note instead of failing the whole call), `browse_course_materials`, `search_course_materials` (titles only), and `read_material`: Page → Markdown through the SSO web chain, Link → target URL, Folder → listing, everything else → summary + itslearning link. `LearningToolContentReader` (core) keeps one web session per process and re-SSOs when it expires. Our own HTML→Markdown converter on AngleSharp. Parsers are tested against real pages saved under `tests/Educ8.Itslearning.Tests/Fixtures`, scrubbed of personal data. 86 tests. Verified live on two courses.

### Phase 4 — Files ✅ done 2026-11-18 (done before Phase 3, at the user's request)
`read_file(elementId, pages?, includeImages?, zipEntry?)`, also used by `read_material` for files. Files are downloaded through the web chain (with the Office download-proxy fallback) and cached in memory (12 files / 200 MB / 30 min). A new **`Educ8.Documents`** library (no itslearning/MCP dependencies) handles the formats:
- PDF: text per page with PdfPig (layout-aware order); template lines and logos repeated on most pages are removed. With `includeImages`, whole pages are **rendered** (PDFtoImage/PDFium, 110 dpi PNG), so slides, diagrams, formulas and scanned pages are seen as they look. The response lists which pages have figures.
- Word: headings, lists, tables → Markdown, split into ~8k-char sections; embedded images on request.
- PowerPoint: per slide, with title, bullets (with levels), tables, speaker notes; embedded images on request (no slide rendering).
- Excel: per sheet, as a grid with row numbers and column letters (up to 300 rows × 40 columns); dates converted; numbers shown with 15 significant digits.
- Text/code (fenced, with a language), Jupyter notebooks (cells and outputs), HTML → Markdown, images (returned as MCP image content), zip (listing; read one entry with `zipEntry`).
- Page ranges (`"1-10"`, `"12-"`, `"2,5"`) and a 40k-character text budget per call with a "continue with pages=…" hint; images capped at 10 per call and 12 MB.
- `get_course_overview` now lists each lesson plan's material (the plan-name subfolder of its topic folder).

### Phase 3 — Assignments ✅ done 2026-11-19
- `list_my_tasks(courseId?, status)`: open or completed tasks, grouped by course (most recently active first), with assignments and tests ahead of teacher "activities".
- `get_assignment(elementId)`: description, deadline (local time, with "in N days"/"passed"), late policy, submission type, max score, mandatory/anonymous/peer flags, grading scale, the teacher's files, the student's own submission (text + files), and **status, score and the teacher's written feedback**, plus feedback files. `read_material` on an assignment gives the same.
- `read_assignment_file(elementId, fileId, …)`: the teacher's, own or feedback files, through the same extractor as `read_file`.
- REST covers the details, the answer and the files. Status, score and feedback come from the **assignment web view** (`assignment.itslearning.com/view.aspx`, via the SSO chain), because REST doesn't return them to students. If the web view fails, the rest is still shown, with a note.


### Schedule and "what's new" ✅ done 2026-11-19
- `get_schedule(from?, days=7, courseId?)`: events by day in local time, with rooms and teachers from the timetable import (no detail calls needed).
- `get_event(eventId)`: time, place, linked lesson plans with their description and **material** (planner + plan-name folder mapping), linked elements, and the itslearning link.
- `whats_new(courseId?, days=7)`: the bell ("For you": feedback available, group submissions; announcement duplicates removed) plus course updates grouped by day, with grouped updates expanded to their elements and announcements shown with text.
- `get_course_overview` gained a "Recent changes" section (course stream, last 14 days) and an **"Upcoming calendar events"** section (course calendar, next 7 days). The lesson-plan section now says plans aren't the timetable. Why: a Claude session running an old server (no `get_schedule`) built "next week" from lesson plans and wrongly said a project day was free. That course has calendar events but no plans.

### Phase 5 — Polish
MCP prompts (`study_course`, `work_on_assignment`), optional resources, caching (course list and folder trees cached for a few minutes), setup README, packaging (e.g. `dotnet tool` so `claude mcp add` can run it).

## Testing

- Unit tests: `ItslearningClient` and formatters against JSON fixtures (start from the doc samples, later sanitized real responses with no personal data), using a fake `HttpMessageHandler`.
- Tool tests: call tools in-process and snapshot the Markdown output.
- Manual: MCP Inspector, then a real study session in Claude Code/Desktop.

## Out of scope for v1

Submitting answers, messaging, posting, notifications, teacher/admin features, other students' data, fetching external web links, hosted multi-user deployment.

## Risks

| Risk | Mitigation |
|---|---|
| No API for `FolderFile`/Note content | Phase 0 spike; fallback to an "open in browser" link |
| Large documents overflow the context | Page ranges, character caps, an overview first |
| Content in external tools (video, H5P, LTI) | Out of reach; say so clearly and give a link |
| Login friction (federated, client support for URL elicitation) | Refresh token stored securely; fallback login link in tool output |
| Undocumented enum values / HTML quirks | Phase 0 real-response check; tolerant JSON (unknown enum → string) |
