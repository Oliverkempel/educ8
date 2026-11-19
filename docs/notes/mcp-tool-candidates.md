# MCP tool candidates (draft)

> v1 scope is defined in [../design/v1-study-assistant-plan.md](../design/v1-study-assistant-plan.md). This file is the wider list of ideas.

A first mapping from things a student or teacher would ask an assistant to the endpoints that answer them. Not a commitment; nothing is built yet. Paths are relative to the site base URL. See [../api/README.md](../api/README.md) for conventions.

## Read-only (start here)

| Possible tool | Endpoint(s) | Notes |
|---|---|---|
| `whoami` | `GET restapi/personal/person/v1` | Profile, language. Also the auth check. |
| `list_courses` | `GET restapi/personal/courses/v4?filter=` (or v3 for task counts) | Paged, max 100 |
| `get_course` | `GET restapi/personal/courses/{courseId}/v1`, `.../features/v2` | |
| `list_course_resources` | `GET restapi/personal/courses/{courseId}/resources/v1`, `.../folders/{folderId}/resources/v1` | Root folder, then drill into folders |
| `search_course_resources` | `GET restapi/personal/courses/{locationId}/resources/search/v1?locationType=&searchText=` | |
| `get_resource` | `GET restapi/personal/courses/resources/{resourceId}/v1` | Returns the resource, its course, and the user's role |
| `read_page` | `GET restapi/personal/pages/{elementId}/v1` (+ content blocks) | HTML content, convert to markdown |
| `list_tasks` / `upcoming_deadlines` | `GET restapi/personal/tasks/v1?status=&deadline=`, `GET restapi/personal/tasklistdailyworkflow/v1` | Daily workflow groups by overdue/today/tomorrow/later |
| `get_assignment` | `GET restapi/personal/assignments/{elementId}/v1`, `.../settings/v1`, `.../answers/persons/{personId}/v1` | Includes own submission and feedback |
| `download_file` | assignment/page/yourfiles file endpoints | Follow the redirect |
| `calendar` | `GET restapi/personal/calendar/events/v1?fromDate=`, `GET restapi/personal/courses/{courseId}/calendarevents/v1` | |
| `notifications` / `whats_new` | `GET restapi/personal/notifications/v2`, `.../stream/v2`, `.../newsfeed/v1`, `.../unread/count/v1` | Cursor paging with FromId/UseNewerThan |
| `course_announcements` | `GET restapi/personal/courses/{courseId}/bulletins/v1`, `.../lightbulletins/{id}/comments/v1` | |
| `list_messages` / `read_thread` | `GET restapi/personal/instantmessages/messagethreads/v4`, `.../messagethreads/{threadId}/messages/v3` | |
| `course_participants` | `GET restapi/personal/courses/{courseId}/participants/v3` | |
| `lesson_plans` | `GET restapi/personal/planner/v1?courseId=&startDate=&stopDate=` | |
| `my_files` | `GET restapi/personal/yourfiles/private/folders/{folderId}/v1` | |
| `open_in_browser` | `GET restapi/personal/sso/url/v1?url=` | Short-lived SSO link |

## Write (later, needs confirmation in the client)

| Possible tool | Endpoint(s) |
|---|---|
| `send_message` | `POST restapi/personal/instantmessages/v2` |
| `submit_assignment` | `POST restapi/personal/fileupload/temp/v1` then `POST restapi/personal/assignments/{elementId}/answers/v1` |
| `comment_on_announcement` | `POST restapi/personal/lightbulletins/{id}/comments/v2` |
| `mark_notifications_read` | `PUT restapi/personal/notifications/v2`, `PUT .../seenmark/all/v1` |

Submitting assignments and sending messages are irreversible and visible to others. The MCP server should make that clear in tool descriptions.
