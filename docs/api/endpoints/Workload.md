# Workload

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for Workload API.

## Endpoints

- `GET restapi/personal/workload/assignments/{studentId}/{courseId}/v1` — Gets a list of assignments for a particular student in a particular course with corresponding workload
- `GET restapi/personal/workload/course/{courseId}/v1` — Gets a list of students in a specified course with corresponding workload.
- `GET restapi/personal/workload/courses/{studentId}/v1` — Gets a list of courses with corresponding workload for a student.
- `GET restapi/personal/workload/mentees/v1` — Gets a list of mentees with corresponding workload.
- `GET restapi/personal/workload/students/v1` — Gets a list of all students with their corresponding workload.

---

### `GET restapi/personal/workload/assignments/{studentId}/{courseId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of assignments for a particular student in a particular course with corresponding workload

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| studentId | The PersonID of the student. | integer | Required |
| courseId | The ID of the course. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<LearningToolElementAnswerWithWorkload>` — This method returns a collection of entity . Each entity contains the details of an assignment for a particular student, and its workload.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LearningToolElementAnswerWithWorkload |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "AnswerId": 1,
      "UserIds": [
        1
      ],
      "AssessmentStatus": {
        "AssessmentStatusItemId": 1,
        "AssessmentStatusId": 2,
        "Title": "sample string 3",
        "Editable": true,
        "Completed": true,
        "Submitted": true,
        "NotSubmitted": true
      },
      "Deadline": "sample string 2",
      "Url": "sample string 3",
      "LearningObjectId": 4,
      "LearningObjectInstanceId": 5,
      "UserId": 6,
      "Workload": "NotSet",
      "WorkloadDisplay": "sample string 7",
      "WorkloadEstimation": 8,
      "ElementId": 9,
      "ElementType": 10,
      "Title": "sample string 11",
      "LearningToolId": 12,
      "ElementOrder": 13,
      "ActiveFromDT": "2026-11-18T16:33:46Z",
      "ActiveToDT": "2026-11-18T16:33:46Z",
      "DeadlineDT": "2026-11-18T16:33:46Z"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/workload/course/{courseId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of students in a specified course with corresponding workload.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the requested course | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonSimpleWithWorkload>` — This method returns a collection of entity . Each entity contains the details of a student and their workload.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimpleWithWorkload |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Workload": {
        "FinishedAssignments": 1,
        "TotalAvailableAssignments": 2,
        "FinishedWorkload": 3,
        "TotalAvailableWorkload": 4
      },
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
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/workload/courses/{studentId}/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of courses with corresponding workload for a student.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| studentId | The student's PersonID | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<CourseWithWorkload>` — This method returns a collection of entity . Each entity contains the details of a student and their workload in courses.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of CourseWithWorkload |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CourseId": 1,
      "Title": "sample string 2",
      "Workload": {
        "FinishedAssignments": 1,
        "TotalAvailableAssignments": 2,
        "FinishedWorkload": 3,
        "TotalAvailableWorkload": 4
      },
      "CanAccessAssignmentList": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/workload/mentees/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of mentees with corresponding workload.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonSimpleWithWorkload>` — This method returns a collection of entity . Each entity contains the details of a student and their workload.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimpleWithWorkload |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Workload": {
        "FinishedAssignments": 1,
        "TotalAvailableAssignments": 2,
        "FinishedWorkload": 3,
        "TotalAvailableWorkload": 4
      },
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
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/workload/students/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of all students with their corresponding workload.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonSimpleWithWorkload>` — This method returns a collection of entity . Each entity contains the details of a student and their workload.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimpleWithWorkload |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Workload": {
        "FinishedAssignments": 1,
        "TotalAvailableAssignments": 2,
        "FinishedWorkload": 3,
        "TotalAvailableWorkload": 4
      },
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
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

