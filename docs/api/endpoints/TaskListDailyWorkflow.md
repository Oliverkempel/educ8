# TaskListDailyWorkflow

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for tasklistdailyworkflow API

## Endpoints

- `GET restapi/personal/tasklistdailyworkflow/{courseId}/completed/v1` — Gets a list of completed tasks for a user in a course.
- `GET restapi/personal/tasklistdailyworkflow/{courseId}/v1` — Gets a list of tasks assigned to the user separated into sections.
- `GET restapi/personal/tasklistdailyworkflow/{courseId}/viewas/v1` — Gets a list of tasks assigned to a user in View As mode.
- `GET restapi/personal/tasklistdailyworkflow/child/{childId}/v1` — For a parent gets a list of tasks assigned to a child separated into sections.
- `GET restapi/personal/tasklistdailyworkflow/completed/child/{childId}/v1` — For a parent gets a list of tasks completed for a child.
- `GET restapi/personal/tasklistdailyworkflow/v1` — Gets a list of tasks assigned to the user separated into sections.

---

### `GET restapi/personal/tasklistdailyworkflow/{courseId}/completed/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of completed tasks for a user in a course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | A course id to get the tasks within a given course. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<TaskDailyWorkflow>` — This method returns a collection of daily workflow task entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of TaskDailyWorkflow |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CompletedTaskAssessmentText": "sample string 1",
      "CourseId": 2,
      "CourseTitle": "sample string 3",
      "CourseColorClass": "sample string 4",
      "Status": "sample string 5",
      "TaskId": 6,
      "TaskTitle": "sample string 7",
      "TaskUrl": "sample string 8",
      "TaskUrlTarget": "sample string 9",
      "IconUrl": "sample string 10",
      "BrandIconUrl": "sample string 11",
      "Deadline": "2026-11-18T16:32:42Z",
      "DeadlineDisplayTime": "sample string 13",
      "IsHomework": true,
      "IsPeerAssessment": true,
      "IsSelfAssessment": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasklistdailyworkflow/{courseId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of tasks assigned to the user separated into sections.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | A course id to get the tasks within a given course. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<TaskDailyWorkflowSection>` — This method returns a collection of task section entities . Each task section entity (e.g. today) contains a collection of daily workflow task entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of TaskDailyWorkflowSection |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "SectionTitle": "sample string 1",
      "TaskDailyWorkflow": {
        "EntityArray": [
          {
            "CompletedTaskAssessmentText": "sample string 1",
            "CourseId": 2,
            "CourseTitle": "sample string 3",
            "CourseColorClass": "sample string 4",
            "Status": "sample string 5",
            "TaskId": 6,
            "TaskTitle": "sample string 7",
            "TaskUrl": "sample string 8",
            "TaskUrlTarget": "sample string 9",
            "IconUrl": "sample string 10",
            "BrandIconUrl": "sample string 11",
            "Deadline": "2026-11-18T16:35:34Z",
            "DeadlineDisplayTime": "sample string 13",
            "IsHomework": true,
            "IsPeerAssessment": true,
            "IsSelfAssessment": true
          }
        ],
        "Total": 1,
        "CurrentPageIndex": 2,
        "PageSize": 3
      },
      "BackToText": "sample string 2",
      "StartOpen": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasklistdailyworkflow/{courseId}/viewas/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of tasks assigned to a user in View As mode.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | A course id to get the tasks within a given course. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<TaskDailyWorkflowSection>` — This method returns a collection of task section entities . Each task section entity (e.g. today) contains a collection of daily workflow task entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of TaskDailyWorkflowSection |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "SectionTitle": "sample string 1",
      "TaskDailyWorkflow": {
        "EntityArray": [
          {
            "CompletedTaskAssessmentText": "sample string 1",
            "CourseId": 2,
            "CourseTitle": "sample string 3",
            "CourseColorClass": "sample string 4",
            "Status": "sample string 5",
            "TaskId": 6,
            "TaskTitle": "sample string 7",
            "TaskUrl": "sample string 8",
            "TaskUrlTarget": "sample string 9",
            "IconUrl": "sample string 10",
            "BrandIconUrl": "sample string 11",
            "Deadline": "2026-11-18T16:34:59Z",
            "DeadlineDisplayTime": "sample string 13",
            "IsHomework": true,
            "IsPeerAssessment": true,
            "IsSelfAssessment": true
          }
        ],
        "Total": 1,
        "CurrentPageIndex": 2,
        "PageSize": 3
      },
      "BackToText": "sample string 2",
      "StartOpen": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasklistdailyworkflow/child/{childId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

For a parent gets a list of tasks assigned to a child separated into sections.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | The id of the child to get the tasks for | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<TaskDailyWorkflowSection>` — This method returns a collection of task section entities . Each task section entity (e.g. overdue, today, tomorrow, later) contains a collection of daily workflow task entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of TaskDailyWorkflowSection |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "SectionTitle": "sample string 1",
      "TaskDailyWorkflow": {
        "EntityArray": [
          {
            "CompletedTaskAssessmentText": "sample string 1",
            "CourseId": 2,
            "CourseTitle": "sample string 3",
            "CourseColorClass": "sample string 4",
            "Status": "sample string 5",
            "TaskId": 6,
            "TaskTitle": "sample string 7",
            "TaskUrl": "sample string 8",
            "TaskUrlTarget": "sample string 9",
            "IconUrl": "sample string 10",
            "BrandIconUrl": "sample string 11",
            "Deadline": "2026-11-18T16:35:34Z",
            "DeadlineDisplayTime": "sample string 13",
            "IsHomework": true,
            "IsPeerAssessment": true,
            "IsSelfAssessment": true
          }
        ],
        "Total": 1,
        "CurrentPageIndex": 2,
        "PageSize": 3
      },
      "BackToText": "sample string 2",
      "StartOpen": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasklistdailyworkflow/completed/child/{childId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

For a parent gets a list of tasks completed for a child.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | The id of the child to get the tasks for | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<TaskDailyWorkflow>` — This method returns a collection of completed tasks (daily workflow task entities) .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of TaskDailyWorkflow |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CompletedTaskAssessmentText": "sample string 1",
      "CourseId": 2,
      "CourseTitle": "sample string 3",
      "CourseColorClass": "sample string 4",
      "Status": "sample string 5",
      "TaskId": 6,
      "TaskTitle": "sample string 7",
      "TaskUrl": "sample string 8",
      "TaskUrlTarget": "sample string 9",
      "IconUrl": "sample string 10",
      "BrandIconUrl": "sample string 11",
      "Deadline": "2026-11-18T16:31:46Z",
      "DeadlineDisplayTime": "sample string 13",
      "IsHomework": true,
      "IsPeerAssessment": true,
      "IsSelfAssessment": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/tasklistdailyworkflow/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of tasks assigned to the user separated into sections.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<TaskDailyWorkflowSection>` — This method returns a collection of task section entities . Each task section entity (e.g. overdue, today, tomorrow, later) contains a collection of daily workflow task entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of TaskDailyWorkflowSection |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "SectionTitle": "sample string 1",
      "TaskDailyWorkflow": {
        "EntityArray": [
          {
            "CompletedTaskAssessmentText": "sample string 1",
            "CourseId": 2,
            "CourseTitle": "sample string 3",
            "CourseColorClass": "sample string 4",
            "Status": "sample string 5",
            "TaskId": 6,
            "TaskTitle": "sample string 7",
            "TaskUrl": "sample string 8",
            "TaskUrlTarget": "sample string 9",
            "IconUrl": "sample string 10",
            "BrandIconUrl": "sample string 11",
            "Deadline": "2026-11-18T16:32:22Z",
            "DeadlineDisplayTime": "sample string 13",
            "IsHomework": true,
            "IsPeerAssessment": true,
            "IsSelfAssessment": true
          }
        ],
        "Total": 1,
        "CurrentPageIndex": 2,
        "PageSize": 3
      },
      "BackToText": "sample string 2",
      "StartOpen": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

