# Live API findings (Phase 0)

Verified **2026-11-18** against `https://sdu.itslearning.com` with a student account, using the spike CLI in `spikes/Phase0.Spike` (throwaway; see its `Usage()` for commands). Where this file and the generated reference disagree, **this file wins**.

## Auth

| Finding | Detail |
|---|---|
| Authorization code flow works | `oauth2/authorize.aspx` → SDU login page → "SDU Office 365" → back to authorize page → code |
| Redirect URI | `itsl-itslearning://login` (exact). `itsl-itslearning`, `…://`, `…://login/` and others are rejected with "The Client id and/or redirect URI is missing or invalid." |
| Client ID | Looks like the itslearning mobile app's public client (custom `itsl-itslearning://` scheme). Fine for personal use. A distributed product should get its own registration |
| How the code is delivered | The authorize page returns **HTML (200)** whose script navigates to `itsl-itslearning://login/?state=…&code=…`. There is no HTTP 302. A desktop browser can't open that scheme, so **the code has to be read from the page** (in the spike: from the browser pane's DOM). The code looks like `900937-<hex><suffix>` (customer id prefix) |
| Client secret | **Not needed.** The token request only sends `client_id`, `grant_type`, `code` and `redirect_uri` |
| Scope | `All` is accepted |
| Access token lifetime | **`expires_in: 300` (5 min)**, not the 3600 shown in the docs sample |
| Refresh | `grant_type=refresh_token` works. The **refresh token rotates** on every refresh, so always store the new one. Lifetime is unknown so far |
| Web pages + bearer | `*.aspx` web pages **ignore** the bearer token (302 to `/log_out.aspx`) |
| SSO into the web | `GET restapi/personal/sso/url/v1?url=<encoded itslearning url>` → `{ "Url": … }`. Opening it with a cookie jar gives a full web session (cookies `ASP.NET_SessionId`, `login`, …) without a login prompt |

Implication for the MCP server's login step: because the redirect is a custom scheme and is delivered by page script, a plain "open browser + localhost listener" can't catch the code. Options: (1) the user pastes the final `itsl-itslearning://login/?…code=…` URL, or the code, into a `login_complete` tool; (2) an embedded or automated browser reads it. To be decided in Phase 1 (see open questions).

## Element types in real courses

Course content is almost entirely `ElementType: "LearningToolElement"`. The **`LearningToolId`** says what it really is:

| LearningToolId | Meaning (observed) | How to read the content |
|---|---|---|
| 0 + `Folder` | Folder | REST: `courses/{c}/folders/{f}/resources/v1` |
| **5** | **Page** (`page.itslearning.com/PageInstance.aspx`) | REST `pages/{elementId}/v1` → **500**; `library/pages/{loiId}/v1` → **500**. Works via the **SSO web chain**; content is server-rendered HTML (below) |
| **5009** | **File** (PDF, zip, …) | No REST endpoint. Works via the **SSO web chain** → download (below) |
| **5010** | **Link** (articles, docs, books) | SSO web chain → `resource.itslearning.com/Link/View.aspx`. The target URL is the external `href` |
| **5006** | **Assignment** | REST `assignments/{elementId}/v1` ✅ |
| 5002 | Test | Not readable (title, status, link only) |
| 5050 | Survey | Not readable |
| 13221 | Unknown external tool (used for math hand-ins) | Not checked |
| 0 + `CustomActivity` | Teacher-made task/activity ("Before class: read chapter…") | Title and description from the tasks API |

The documented `FolderFile`, `Note` and `WebLink` element types did not appear in the courses checked.

## Course endpoints (REST) — all ✅ 200

| Endpoint | Notes |
|---|---|
| `courses/v4?PageSize=100` | 23 courses incl. old semesters and non-course "communities". Fields: `CourseId, Title, CourseCode, LastUpdatedUtc, Role, TeachersInCourse[]…`. Sort by `LastUpdatedUtc` to find current ones; the course code ends in the semester (e.g. `-E26`) |
| `courses/{id}/v1` | Minimal: `Code, CourseId, Title, CreatedDateTimeUtc, OrganisationId` |
| `courses/{id}/topics/v1` | `TopicId, TopicName, FolderId, Color, TopicMode`. Topics map to folders |
| `courses/{id}/resources/v1` | Root folder. Each item: `ElementId, ElementType, LearningToolId, Title, Path, Url, ContentUrl, Homework, Active` |
| `courses/{id}/folders/{folderId}/resources/v1` | Folder contents. Typical lecture layout: week folder → `Preparation` / `Lecture` / `Exercise` subfolders |
| `courses/{id}/resources/search/v1?locationType=Course&searchText=` | Searches the **whole course tree** (titles). Returns `{ Resources: EntityList, AddElementUrl }` |
| `courses/resources/{elementId}/v1` | Element + course + `Path` + `LearningToolId` |
| `courses/{id}/tasks/v1`, `tasks/v1` | Default `status` = Active. `status=Completed` works. Fields: `ElementId, ElementType, LearningToolId, Title, Description, Status, Deadline, LocationTitle, ContentUrl`. **`Deadline` is often null** |
| `courses/{id}/bulletins/v1` | Works |
| `planner/v1?courseId=` | Works (11 KB for one course) |
| `courses/{id}/features/v2` | Works |

## Assignments

- `assignments/{elementId}/v1` ✅ `Title, Deadline, SubmissionType, RawText (HTML description), Text, Files[], TemplateFile, MaxScore, AssessmentStatusId, IsActive…`
- `assignments/{elementId}/answers/persons/{myPersonId}/v1` ✅ when an answer exists: `AnswerId, IsDraft, Submitted, Text, Files[{FileId, Name, MimeType, Size…}]`. **Returns HTTP 500 "Error calling API" when the student hasn't submitted.** Treat 500 here as "no submission".
- `assignments/{e}/answers/{answerId}/files/{fileId}/v1` ✅ 302 → signed `filecache.itslearning.com` URL (valid ~10 min) → file bytes.
- Not yet checked: feedback/assessment files, assignments with teacher-attached `Files`.

## The SSO web chain (files, pages, links)

For content without a REST endpoint (Page, File, Link):

1. `GET restapi/personal/sso/url/v1?url=<ContentUrl>` (bearer) → `Url`.
2. GET `Url` with a cookie jar → lands on `ViewLearningToolElement.aspx?LearningToolElementId=<id>`. The page has `<iframe id="ctl00_ContentPlaceHolder_ExtensionIframe" src="https://platform.itslearning.com/LoginSSO.aspx?…&Signature=…">`.
3. GET the iframe `src` with the same jar → redirects to the tool:
   - **File (5009)**: `resource.itslearning.com/File/View.aspx?…` contains `href="/Proxy/DownloadRedirect.ashx?LearningObjectId=…&LearningObjectInstanceId=…"`. GET that on `resource.itslearning.com` → `filecache.itslearning.com/...` → **the file** (verified: 201 KB `application/pdf`, `Content-Disposition: attachment; filename="OS&Linux.pdf"`).
   - **Page (5)**: `page.itslearning.com/PageInstance.aspx?LearningObjectId=…&LearningObjectInstanceId=…&PageId=…`. Blocks are `li.il-widget` (class `ilw-cb-text` for text). Content is in `div.il-widget-content > div.h-userinput` (HTML) and the block title is in `h2`. → HTML to Markdown.
   - **Link (5010)**: `resource.itslearning.com/Link/View.aspx?…`. The target is the external `href`.

Caveats: this is **web scraping of undocumented pages**, so it may break with itslearning releases. All markup knowledge lives in `src/Educ8.Itslearning/Content/WebPageParsers.cs`, tested against saved pages. The iframe URL carries personal data (name, email) and a signature, so **never log it**.

### More web-chain details (verified 2026-11-18, Phase 2)

- **One web session can be reused.** After one SSO, `ViewLearningToolElement.aspx?LearningToolElementId=<id>` works for other elements with the same cookies; each element still needs its own signed iframe hop. When the session has expired, the view request ends on `/log_out.aspx?SessionExpired=0` (or a login page), so do a new SSO. Timing: first Page read about 1.5–1.9 s, later ones about 0.6 s.
- **Page blocks**: every block observed so far is `li.il-widget.ilw-cb-text` (other kinds are expected to be `ilw-cb-files`, `-images`, `-rss`, `-poll`). Title is in `.il-widget-titleBlock h2` (often empty). Content is in `.il-widget-content .h-userinput`. Videos are embedded as `<oembed>https://www.youtube.com/watch?v=…</oembed>`. Images use `data-richtextfile-id` and `filerepository.itslearning.com` URLs. Author HTML is heavily inline-styled, with nested `<strong>` and `<br>` inside bold.
- **Link page**: target in `a#…ResourceContent_Link` (`a[id$='ResourceContent_Link']`). Description is in `section.file-link-description` (empty in the samples).

### Files (verified 2026-11-18, Phase 4)

- **Download link**: on `resource.itslearning.com/File/View.aspx` it's `a#…DownloadLinkForViewType` (`a[id$='DownloadLinkForViewType']`), with the file name in its `Download` attribute and `href="/Proxy/DownloadRedirect.ashx?LearningObjectId=…&LearningObjectInstanceId=…"`.
- **Office files (pptx, docx, xlsx) have no download link.** The page only offers "Open in PowerPoint for the web" (a postback) plus a preview iframe. But `/Proxy/DownloadRedirect.ashx?LearningObjectId=…&LearningObjectInstanceId=…` **does** serve the file when you build it yourself; the ids are in the tool page URL. Verified with a 1.8 MB pptx and an xlsx.
- The proxy redirects to a signed `filecache.itslearning.com` URL (valid about 10 min). The response has the right `Content-Type` and `Content-Disposition: attachment; filename=…; filename*=UTF-8''…`.
- File types seen in SDU courses: pdf (lecture slides exported from PowerPoint; lab sheets), pptx, docx, xlsx, png/jpg (whiteboard photos, diagram solutions), zip (exercise resources/code).
- Slide PDFs from SDU's template carry **hidden template text** on every page ("Skift baggrundsfarve. Højreklik på slidet…") and the same logo image. The extractor removes lines and images that repeat on ≥ 50% of pages.

### Assignments (verified 2026-11-19, Phase 3)

- `assignments/{e}/v1`: `RawText` (description, HTML), `Deadline` (often null), `IsSubmissionAfterDeadlineAllowed`, `MaxScore`, `SubmissionType`, `Files[]` (`FileId, Name, MimeType, Size`), `TemplateFile`, `IsAssessmentVisibleForStudents`, `AssessmentStatusId`.
- `assignments/{e}/settings/v1`: `StatusScale` (titles like "Ikke sendt", "Sendt", "Send igen", "Fuldført"), `AssessmentScale` (null when not graded on a scale), `Terms`.
- `assignments/{e}/answers/persons/{me}/v1`: `AnswerId, IsDraft, Submitted, Text, Files[]`. It has **no status, score or feedback**. Returns **500 when nothing was submitted**.
- `assignments/{e}/answers/{a}/assessments/files/v1`: feedback files (`[]` when none).
- Downloads through `assignments/{e}/files/{f}/v1`, `…/answers/{a}/files/{f}/v1` and `…/assessments/files/{f}/v1` work with the bearer token and 302 to a signed `filecache` URL (verified for a teacher's zip and an own submission).
- **Status, score and feedback are only on the web view**: tool 5006 → `assignment.itslearning.com/view.aspx`, rendered on the server:
  - `.as-submission-status-title`: "Not submitted" / "Completed" / …
  - `#…AssessmentOrFeedbackSectionContent`: `h2` "Score" or "Feedback"; a `div` "Assessed on 29. marts 2026 12:00 by …"; `.ccl-assess-badge` "5 of 5 (100%)" (missing when there's feedback only); feedback HTML in `.h-userinput`.
  - `#…AnswerSectionContent .h-color-grey`: "Submitted: 2. marts 2026 15:53 by …". Dates are in the user's itslearning language (Danish month names here).
- `tasks/v1` (all courses) includes many old "activities" (`CustomActivity`) from past semesters; 93 open tasks for this student, 4 of them assignments. Group by course and sort by course activity.
- A transient **504 Gateway Time-out** from `assignment.itslearning.com` was seen once; retrying worked.

### Calendar (verified 2026-11-19)

- `calendar/events/v1?fromDate=yyyy-MM-dd&PageIndex&PageSize` takes **no end date**. It returns every event from `fromDate` on, **sorted by start**, and pages normally (127 events from 1 Sep, over 2 pages). Fetch until an event starts after the window.
- SDU timetable events are imported. **The room is in `EventTitle`** ("Lecture - Odense U55", "T1 - Exercises - Odense U173"), and **`ImportDescription`** holds "room + teacher names". `Description` is empty. `EventType` was always "Course" here.
- Exercise classes for all groups appear (T1 and T2), not only the student's own.
- `calendar/events/{id}/v1`: `PlanLinks` (`PlanId, PlanName, TopicName` (often null), `Url`) link the event to **lesson plans**. Their material can be found through the planner + plan-name folder mapping. `Bookings` and `ElementLinks` were empty in the samples. `HasMoreContent` can be true even with an empty description.
- `courses/{id}/calendarevents/v1?fromDate&toDate` uses **different field names**: `Id` (not `EventId`), `CourseId` (not `LocationId`) and no `EventType`.

### What's new (verified 2026-11-19)

- `notifications/stream/v2?showLightBulletins=true` ("latest changes"): newest first. Each item has `Text`, `LocationTitle/LocationId`, `PublishedDate`, `PublishedBy`, either one element (`ElementId, ElementType, LearningToolId`) **or** a group (`ElementId` 0, `ElementsCount` > 1, `HasMoreElements`), and announcements carry the full `LightBulletin` (text, comment count). It covered the 4 active courses (47 items over about a month). The docs say it follows *favourite* courses; an inactive course simply had an empty `courses/{id}/stream/v1`, so this isn't confirmed either way.
- `notifications/{notificationId}/elements/v1`: the elements behind a group (`ElementId, ElementType, LearningToolId, Title, Url, Homework`).
- `notifications/newsfeed/v1`: the same items as the stream but with fewer fields, so it isn't used.
- `notifications/v2` (the bell): personal items. "Assessment is available for X" (`Type: "Assessment"`) and "<group member> submitted X" carry the element in `ContentUrl` (`…LearningToolElementId=…`). Also "New announcement available in <course>", which duplicates the stream's announcements (same timestamp).
- `notifications/unread/count/v1` and `unseen/count/v1` were both 0 before and after listing. All bell items were already read, so whether listing marks items read isn't proven. A separate `PUT notifications/v2` exists for marking read, which suggests GET doesn't.

### Lesson plans ↔ material

- `planner/plansinfo/v1?plansIds[0]=…` (documented as "plans with … elements attached") returns **`[]` for a student**, whatever the parameter format (`plansIds[0]=`, repeated, comma-separated).
- In practice a plan's material is in a **subfolder of its topic's folder named like the plan**: topic "02: Operating Systems & Linux" (folder 2358864) has subfolders "Preparation", "Lecture" and "Exercise", matching plans of those names with that `TopicId`. `courses/{id}/topics/{topicId}/elementids/v1` returns exactly those subfolder ids. Their `IsTopicOrPlanFolder` is `false`, so it can't be used to detect them.

## Other REST details (Phase 2)

- `planner/v1?courseId=&startDate=yyyy-MM-dd&stopDate=yyyy-MM-dd` works; full ISO timestamps work too. It returns plans **overlapping** the window (including long-running ones like "Recommended Reading", 2026-09-03 → 2026-11-26), **unsorted**. `PlanDescription` is plain text or HTML; `TopicId` 0 means no topic.
- `courses/{id}/bulletins/v1`: `Text` is **plain text** with `\n`. There's also `EmbedUrl`, `Pinned`, `PublishedBy.FullName`, `CommentsCount`. Newest first.
- `courses/{id}/topics/v1`: `TopicId, TopicName, FolderId, Color, TopicMode`. Topic folders are the week/module folders.
- `courses/resources/{id}/v1` works for folders too (`ElementType: "Folder"`).
- `courses/{id}/resources/search/v1` matches **titles** across the whole course tree.
