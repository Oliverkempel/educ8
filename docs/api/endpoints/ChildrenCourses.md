# ChildrenCourses

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** The controller is used to get information about children courses and teachers.

## Endpoints

- `GET restapi/personal/children/{childId}/courses/{courseId}/features/v1` — Returns available feature and sub-features in a course where the child is a learner.
- `GET restapi/personal/children/{childId}/courses/v1` — Returns list of courses where the child is a learner.
- `GET restapi/personal/children/{childId}/upcoming/v1` — Returns list of courses upcoming plans, tasks and events in a dates range (one day only so far).

---

### `GET restapi/personal/children/{childId}/courses/{courseId}/features/v1`

Returns available feature and sub-features in a course where the child is a learner.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | Identifier of the child. | integer | Required |
| courseId | Identifier of the course. | integer | Required |

**Response:** `Personal.Feature`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The feature name. | string |  |
| Url | Url to the itslearning web page for the feature if it exists. | string |  |
| Features | The list of sub-features. | Collection of Personal.Feature |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "Url": "sample string 2",
  "Features": [
    {
      "Name": "sample string 1",
      "Url": "sample string 2",
      "Features": null
    }
  ]
}
```
</details>


### `GET restapi/personal/children/{childId}/courses/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Returns list of courses where the child is a learner.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | Identifier of the child. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<ChildCourse>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of ChildCourse |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Teachers": [
        {
          "HierarchyRole": 0,
          "PersonId": 1,
          "FirstName": "sample string 2",
          "LastName": "sample string 3",
          "FullName": "sample string 4",
          "ProfileUrl": "sample string 5",
          "AdditionalInfo": "sample string 6",
          "ProfileImageUrl": "sample string 7",
          "ProfileImageUrlSmall": "sample string 8"
        }
      ],
      "CourseId": 1,
      "Title": "sample string 2",
      "FriendlyName": "sample string 3",
      "CourseColor": "sample string 4",
      "CourseFillColor": "sample string 5",
      "CourseCode": "sample string 6"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/children/{childId}/upcoming/v1?fromDate={fromDate}&toDate={toDate}`

Returns list of courses upcoming plans, tasks and events in a dates range (one day only so far).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | Identifier if a child. | integer | Required |
| fromDate | Start date. | date | Required |
| toDate | End date. | date | Required |

**Response:** `?` — Aggregated list of events, plans and tasks

| Name | Description | Type | Additional information |
|---|---|---|---|
| Date | Date of upcoming events, plans and tasks. | date |  |
| Courses | List of course items, each item contains basic information about course and related plans/events/tasks. | Collection of CourseUpcoming |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Date": "2026-11-18T16:33:14Z",
    "Courses": [
      {
        "Tasks": [
          {
            "TaskId": 1,
            "Title": "sample string 2",
            "Status": "Unknown",
            "Deadline": "2026-11-18T16:33:14Z",
            "Url": "sample string 3",
            "ContentUrl": "sample string 4",
            "IconUrl": "sample string 5",
            "ElementId": 6,
            "ElementType": "Unknown",
            "LearningToolId": 7,
            "Homework": true,
            "LocationType": "Course",
            "LocationId": 1
          }
        ],
        "Plans": [
          {
            "PlanId": 1,
            "TopicId": 2,
            "Order": 3,
            "Name": "sample string 4",
            "StartDate": "2026-11-18T16:33:14Z",
            "StopDate": "2026-11-18T16:33:14Z",
            "LocationId": 7,
            "LocationType": "Course",
            "TopicColor": "sample string 8",
            "TopicName": "sample string 9"
          }
        ],
        "CalendarEvents": [
          {
            "EventId": 1,
            "EventUrl": "sample string 2",
            "EventTitle": "sample string 3",
            "FromDate": "2026-11-18T16:33:14Z",
            "ToDate": "2026-11-18T16:33:14Z",
            "EventType": "Course",
            "LocationId": 6,
            "LocationType": "Course"
          }
        ],
        "CourseId": 1,
        "Title": "sample string 2",
        "FriendlyName": "sample string 3",
        "CourseColor": "sample string 4",
        "CourseFillColor": "sample string 5",
        "CourseCode": "sample string 6"
      }
    ]
  }
]
```
</details>

