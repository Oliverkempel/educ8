# Tasks

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for Tasks API.

## Endpoints

- `GET restapi/personal/tasks/followuptasks/{courseId}/v1` — Gets follow up tasks for a course.
- `GET restapi/personal/tasks/followuptasks/v1` — Gets the user's follow up tasks.
- `GET restapi/personal/tasks/v1` — Gets a list of tasks assigned to the user.

---

### `GET restapi/personal/tasks/followuptasks/{courseId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets follow up tasks for a course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course id to return tasks for | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<FollowUpTask>` — List of FollowUpTask

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of FollowUpTask |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Title": "sample string 1",
      "LocationTitle": "sample string 2",
      "NumberOfNewOrUpdatedSubmissions": 3,
      "NumberOfSubmissionsToAssess": 4,
      "TotalNumberOfEvaluateSubmissions": 5,
      "Url": "sample string 6",
      "ContentUrl": "sample string 7",
      "IconUrl": "sample string 8",
      "BrandIconUrl": "sample string 9",
      "ElementId": 10,
      "ElementType": "Unknown",
      "LearningToolId": 11
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasks/followuptasks/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the user's follow up tasks.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<FollowUpTask>` — List of FollowUpTask

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of FollowUpTask |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Title": "sample string 1",
      "LocationTitle": "sample string 2",
      "NumberOfNewOrUpdatedSubmissions": 3,
      "NumberOfSubmissionsToAssess": 4,
      "TotalNumberOfEvaluateSubmissions": 5,
      "Url": "sample string 6",
      "ContentUrl": "sample string 7",
      "IconUrl": "sample string 8",
      "BrandIconUrl": "sample string 9",
      "ElementId": 10,
      "ElementType": "Unknown",
      "LearningToolId": 11
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasks/v1?PageIndex={PageIndex}&PageSize={PageSize}&status={status}&deadline={deadline}&isHomework={isHomework}`

Gets a list of tasks assigned to the user.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| status | Used for filtering tasks by status. | TaskStatusFilter | Default value is Active |
| deadline | Used for filtering tasks by deadline. | TaskDeadlineFilter | Default value is All |
| isHomework | Used for filtering tasks by homework property. | boolean |  |

**Response:** `EntityList<Task>` — This method returns a collection of entity . Each entity is a task that is assigned to the user and is not completed and not hidden.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Task |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Description": "sample string 1",
      "LocationTitle": "sample string 2",
      "LocationFriendlyName": "sample string 3",
      "TaskId": 4,
      "Title": "sample string 5",
      "Status": "Unknown",
      "Deadline": "2026-11-18T16:34:59Z",
      "Url": "sample string 6",
      "ContentUrl": "sample string 7",
      "IconUrl": "sample string 8",
      "ElementId": 9,
      "ElementType": "Unknown",
      "LearningToolId": 10,
      "Homework": true,
      "LocationType": "Course",
      "LocationId": 1
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

