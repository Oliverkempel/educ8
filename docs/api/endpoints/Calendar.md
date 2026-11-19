# Calendar

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Calendar endpoint for personal API. Secured with Oauth2, and requires OAuth2 API scope Calendar.

## Endpoints

- `GET restapi/personal/calendar/events/{eventId}/v1` — Gets a single calendar event by the specified .
- `POST restapi/personal/calendar/events/areanyforgroups/v1` — 
- `GET restapi/personal/calendar/events/v1` — Gets a list of calendar events.

---

### `GET restapi/personal/calendar/events/{eventId}/v1`

Gets a single calendar event by the specified .

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| eventId | ID of event to get. | integer | Required |

**Response:** `Personal.Calendar.CalendarEventDetails` — Calendar event.

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
  "FromDate": "2026-11-18T16:31:46Z",
  "ToDate": "2026-11-18T16:31:46Z",
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


### `POST restapi/personal/calendar/events/areanyforgroups/v1`

_No description._

**Body:** Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `boolean`

<details><summary>Response JSON sample</summary>

```json
true
```
</details>


### `GET restapi/personal/calendar/events/v1?fromDate={fromDate}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of calendar events.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fromDate | From date for filtering calendar events. | date |  |
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
      "FromDate": "2026-11-18T16:31:28Z",
      "ToDate": "2026-11-18T16:31:28Z",
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

