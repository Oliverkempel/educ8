# Children

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for retrieving children-related data. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/children/{childId}/courses/{courseId}/tasks/v1` — Gets a list of tasks assigned to the child in the requested course.
- `GET restapi/personal/children/{childId}/tasks/v1` — Gets a list of tasks assigned to the child.
- `GET restapi/personal/children/v1` — Gets the related children details for a given person

---

### `GET restapi/personal/children/{childId}/courses/{courseId}/tasks/v1?PageIndex={PageIndex}&PageSize={PageSize}&status={status}&deadline={deadline}&isHomework={isHomework}&fromDate={fromDate}&toDate={toDate}`

Gets a list of tasks assigned to the child in the requested course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | The child person identifier. | integer | Required |
| courseId | The course identifier. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| status | Used for filtering tasks by status. | TaskStatusFilter | Default value is All |
| deadline | Used for filtering tasks by deadline. | TaskDeadlineFilter |  |
| isHomework | Used for filtering tasks by homework property. | boolean |  |
| fromDate | From date for filtering tasks. | date |  |
| toDate | To date for filtering tasks. | date |  |

**Response:** `EntityList<Task>` — This method returns a collection of entity . Each entity is a task that is assigned to the child and is not completed and not hidden.

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
      "Deadline": "2026-11-18T16:34:02Z",
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


### `GET restapi/personal/children/{childId}/tasks/v1?PageIndex={PageIndex}&PageSize={PageSize}&status={status}&deadline={deadline}&isHomework={isHomework}&fromDate={fromDate}&toDate={toDate}`

Gets a list of tasks assigned to the child.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | The child person identifier. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| status | Used for filtering tasks by status. | TaskStatusFilter | Default value is All |
| deadline | Used for filtering tasks by deadline. | TaskDeadlineFilter |  |
| isHomework | Used for filtering tasks by homework property. | boolean |  |
| fromDate | From date for filtering tasks by deadline. | date |  |
| toDate | To date for filtering tasks by deadline. | date |  |

**Response:** `EntityList<Task>` — This method returns a collection of entity . Each entity is a task that is assigned to the child and is not completed and not hidden.

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
      "Deadline": "2026-11-18T16:32:22Z",
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


### `GET restapi/personal/children/v1`

Gets the related children details for a given person

**Response:** `EntityList<ChildV2>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of ChildV2 |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PersonId": 3,
      "FirstName": "sample string 4",
      "LastName": "sample string 5",
      "School": "sample string 1",
      "FullName": "sample string 6",
      "ProfileImageUrl": "sample string 2"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

