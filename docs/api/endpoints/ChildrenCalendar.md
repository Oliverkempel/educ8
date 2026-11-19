# ChildrenCalendar

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** The controller is used to get information about children events. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/children/{childId}/courses/{courseId}/events/v1` — Gets a list of calendar events for the child in course.
- `GET restapi/personal/children/{childId}/events/{eventId}/v1` — Gets a single course calendar event for the child by the specified .

---

### `GET restapi/personal/children/{childId}/courses/{courseId}/events/v1?fromDate={fromDate}&toDate={toDate}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of calendar events for the child in course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | [Required] The child person identifier. | integer | Required |
| courseId | [Required] The course identifier. | integer | Required |
| fromDate | [Optional] The start date for filtering calendar events. Time zone is UTC. Date has to contain also time, for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| toDate | [Optional] The stop date for filtering calendar events. Time zone is UTC. Date has to contain also time, for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<Personal.Calendar.CalendarEventV2>` — A list of entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Calendar.CalendarEventV2 |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "EventId": 1,
      "FromDate": "2026-11-18T16:33:46Z",
      "ToDate": "2026-11-18T16:33:46Z",
      "EventTitle": "sample string 4",
      "Description": "sample string 5",
      "ImportDescription": "sample string 6",
      "EventType": "Course",
      "LocationId": 7,
      "LocationGroupId": 8,
      "LocationColor": "sample string 9",
      "LocationFillColor": "sample string 10",
      "LocationTitle": "sample string 11",
      "LocationFriendlyName": "sample string 12",
      "HidePersonalWordForPersonalEvent": true,
      "AttendanceDetails": {
        "KeepAttendancePageUrl": "sample string 1",
        "AttendanceStatistics": {
          "Present": 1,
          "Absent": 2,
          "Excluded": 3
        }
      }
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/children/{childId}/events/{eventId}/v1`

Gets a single course calendar event for the child by the specified .

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | [Required] The child person identifier. | integer | Required |
| eventId | [Required] The course event identifier. | integer | Required |

**Response:** `Personal.Calendar.CalendarEventDetails` — Calendar event .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EventUrl | The URL in itslearning web to open the calendar event in the web view. In case of course event there's the URL pointing to the course dashboard with opened calendar event pop-up dialog. In case of project or personal events there's the URL pointing to the calendar day view. | string |  |
| HasMoreContent | Indicates whether calendar event contains more information than API can consume. For now it indicates whether Description or ImportDescription contain HTML text which is striped by API. | boolean |  |
| Bookings | Rooms or equipment booked for the event. | Collection of Personal.Calendar.Booking |  |
| ElementLinks | Elements connected to the event. | Collection of ElementLink |  |
| PlanLinks | Plans relevant for the event. | Collection of Personal.Calendar.PlanLink |  |
| EventId |  | integer |  |
| FromDate |  | date |  |
| ToDate |  | date |  |
| EventTitle |  | string |  |
| Description |  | string |  |
| ImportDescription |  | string |  |
| EventType |  | Personal.Calendar.CalendarEventType |  |
| LocationId |  | integer |  |
| LocationGroupId |  | integer |  |
| LocationColor |  | string |  |
| LocationFillColor |  | string |  |
| LocationTitle |  | string |  |
| LocationFriendlyName |  | string |  |
| HidePersonalWordForPersonalEvent | If true and `EventType` is `Personal` then the word "Personal" must be hidden from UI for this event. This property is always false for non personal event types. | boolean |  |
| AttendanceDetails |  | Personal.Calendar.Attendance.AttendanceDetails |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EventUrl": "sample string 1",
  "HasMoreContent": true,
  "Bookings": [
    {
      "Name": "sample string 1"
    }
  ],
  "ElementLinks": [
    {
      "ElementId": 1,
      "ElementType": "Unknown",
      "Title": "sample string 2",
      "Url": "sample string 3",
      "ContentUrl": "sample string 4",
      "IconUrl": "sample string 5",
      "LearningToolId": 6,
      "Homework": true
    }
  ],
  "PlanLinks": [
    {
      "PlanId": 1,
      "PlanName": "sample string 2",
      "TopicName": "sample string 3",
      "Url": "sample string 4"
    }
  ],
  "EventId": 3,
  "FromDate": "2026-11-18T16:33:46Z",
  "ToDate": "2026-11-18T16:33:46Z",
  "EventTitle": "sample string 6",
  "Description": "sample string 7",
  "ImportDescription": "sample string 8",
  "EventType": "Course",
  "LocationId": 9,
  "LocationGroupId": 10,
  "LocationColor": "sample string 11",
  "LocationFillColor": "sample string 12",
  "LocationTitle": "sample string 13",
  "LocationFriendlyName": "sample string 14",
  "HidePersonalWordForPersonalEvent": true,
  "AttendanceDetails": {
    "KeepAttendancePageUrl": "sample string 1",
    "AttendanceStatistics": {
      "Present": 1,
      "Absent": 2,
      "Excluded": 3
    }
  }
}
```
</details>

