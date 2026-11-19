# EmbeddedElement

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for embedded elements.

## Endpoints

- `POST restapi/personal/element/embedded/{elementId}/assessments/v1` — Submits an assessment for one or more students on a specific element. This endpoint is only accessible to user…
- `GET restapi/personal/element/embedded/{elementId}/participants/v2` — Gets the participants report for a given assignment. Only users with evaluate permission can access this repor…
- `GET restapi/personal/element/embedded/{elementId}/report/v1` — 
- `POST restapi/personal/element/embedded/{elementId}/tasks/status/v1` — Set the task status for one or more students for a specific assignment where completion mode is ManualByStuden…
- `PUT restapi/personal/element/embedded/{elementId}/v1` — Update embedded element.
- `POST restapi/personal/element/embedded/v1` — Create embedded element.

---

### `POST restapi/personal/element/embedded/{elementId}/assessments/v1`

Submits an assessment for one or more students on a specific element. This endpoint is only accessible to users with evaluate permission (typically teachers).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the element, unique within the customer scope. | integer | Required |

**Body** (Personal.Element.Requests.SubmitElementAssessmentRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to submit the assessment for. If the list contains more than one person, the same assessment will be applied to all of them. | Collection of integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used. Null also means "no change". | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. Null also means "no change". | integer |  |
| Feedback | Feedback from teacher, can contain HTML. Null to not change existing feedback, empty string to clear existing feedback. | string |  |
| Score | Entered score value in case when ElementSettingsResponse.UseScore is true. Null means "no change", pass -1 to clear existing score. | decimal number |  |
| TaskStatus | If this is passed, AssessmentStatusItemId will be ignored. Used when the assignment uses simple statuses. Null also means "no change". | TaskStatus |  |

<details><summary>Request JSON sample</summary>

```json
{
  "PersonIds": [
    1
  ],
  "AssessmentItemId": 1,
  "AssessmentStatusItemId": 1,
  "Feedback": "sample string 1",
  "Score": 1.1,
  "TaskStatus": "Unknown"
}
```
</details>

**Response:** `Personal.Element.Responses.SubmitElementAssessmentResponse` — containing the assessment results for each person.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Results |  | Collection of Personal.Element.Responses.ElementAssessmentResult |  |
| Feedback | Text to be displayed in view content block mode. Video and plugin content is preprocessed. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Results": [
    {
      "PersonId": 1,
      "AssessmentItemId": 1,
      "AssessmentStatusItemId": 1,
      "IsFeedbackChanged": true,
      "Score": 1.1,
      "TaskStatus": "Unknown"
    }
  ],
  "Feedback": "sample string 1"
}
```
</details>


### `GET restapi/personal/element/embedded/{elementId}/participants/v2?personIds={personIds}&groupIds={groupIds}&searchText={searchText}&PageIndex={PageIndex}&PageSize={PageSize}&sortBy={sortBy}&sortOrder={sortOrder}`

Gets the participants report for a given assignment. Only users with evaluate permission can access this report. Report will not be returned if the assignment is anonymous. The participant's extra info is only included if the current user has access to protected data (like "Username"). The participants who no longer have (participate) permission to the assignment are not included in the report, even if they have submitted an answer. Participants who have not submitted an answer are also included in the report. Participants are paged using the provided paging parameters, sorting will be with the newest submissions first and then sorted by last names. Filters use AND logic: if both and are provided, only participants whose person ID appears in AND who belong to at least one of the groups in will be returned (i.e. the intersection of both filters).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |
| personIds | Comma separated string, to filter by personIds. Get your own report by passing your personId, you will need participate permission | string |  |
| groupIds | Comma separated string, to filter by groupIds. Only participants belonging to one of the given groups will be returned | string |  |
| searchText | To filter by participant name, pass null to not use this filter. Search text is matched against first name and last name. | string |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sortBy | How the returned list of participants should be sorted. Possible values are ("Name", "SubmittedDate", "Assessment", "Score", "ExtraInfo", "TaskStatus"). When you select anything else than "Name", the list will be sorted by Name as secondary sorting. Name sorting is according to customer NameFormat. | string |  |
| sortOrder | Sort order for the returned list of participants | Itslearning.Common.Core.SortOrder |  |

**Response:** `Personal.Element.Responses.ParticipantsReportResponseV2` — List of participants ( ) when request was successful (200 - ok)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | The list of participants who currently have access to the element (e.g. an assignment). Participants who have submitted work but no longer have access to the element are not included. | Collection of Personal.Element.Models.ParticipantReportV2 |  |
| TotalCount | Total count of participants who have access to the element, for paging purposes. This can be higher than the number of items in the "Participants" list, if paging is used. | integer |  |
| Persons | Contains more detailed information about PersonIds mentioned in the list. | Collection of Personal.ContentAreaSettings.Models.ParticipantItemModel |  |
| ExtraInfoDisplayName | The localized display name for what the "ExtraInformation" holds, if any. Can be e.g. "Username". Can be null if no extra info is provided or if the user does not have access to such data. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Participants": [
    {
      "PersonId": 1,
      "Submitted": "2026-11-18T16:33:14Z",
      "AssessmentStatusItemId": 1,
      "AssessmentItemId": 1,
      "AssessmentByPersonId": 1,
      "AssessmentDateTime": "2026-11-18T16:33:14Z",
      "RawFeedback": "sample string 2",
      "Feedback": "sample string 3",
      "FeedbackByPersonId": 1,
      "FeedbackDateTime": "2026-11-18T16:33:14Z",
      "Score": 1.1,
      "TaskStatus": "Unknown"
    }
  ],
  "TotalCount": 1,
  "Persons": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "ExtraInformation": "sample string 3",
      "PictureUrl": "sample string 4"
    }
  ],
  "ExtraInfoDisplayName": "sample string 2"
}
```
</details>


### `GET restapi/personal/element/embedded/{elementId}/report/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | integer | Required |

**Response:** `Personal.Element.Responses.OwnReportResponse`

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantReport | Current person report. | Personal.Element.Models.ParticipantReportV2 |  |
| FeedbackPerson | Current person report feedback person. | Personal.ContentAreaSettings.Models.ParticipantItemModel |  |
| AssessmentByPerson | Current person report assessment person. | Personal.ContentAreaSettings.Models.ParticipantItemModel |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ParticipantReport": {
    "PersonId": 1,
    "Submitted": "2026-11-18T16:35:31Z",
    "AssessmentStatusItemId": 1,
    "AssessmentItemId": 1,
    "AssessmentByPersonId": 1,
    "AssessmentDateTime": "2026-11-18T16:35:31Z",
    "RawFeedback": "sample string 2",
    "Feedback": "sample string 3",
    "FeedbackByPersonId": 1,
    "FeedbackDateTime": "2026-11-18T16:35:31Z",
    "Score": 1.1,
    "TaskStatus": "Unknown"
  },
  "FeedbackPerson": {
    "PersonId": 1,
    "FullName": "sample string 2",
    "ExtraInformation": "sample string 3",
    "PictureUrl": "sample string 4"
  },
  "AssessmentByPerson": {
    "PersonId": 1,
    "FullName": "sample string 2",
    "ExtraInformation": "sample string 3",
    "PictureUrl": "sample string 4"
  }
}
```
</details>


### `POST restapi/personal/element/embedded/{elementId}/tasks/status/v1`

Set the task status for one or more students for a specific assignment where completion mode is ManualByStudent or ManualByTeacher.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |

**Body** (Personal.Element.Requests.SubmitTaskStatusRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to set the task status for. | Collection of integer |  |
| TaskStatus | Task status to set, for a task which is associated with an element. Allowed values are "NotStarted", "InProgress", "Completed". | TaskStatus |  |

<details><summary>Request JSON sample</summary>

```json
{
  "PersonIds": [
    1
  ],
  "TaskStatus": "Unknown"
}
```
</details>

**Response:** `?`


### `PUT restapi/personal/element/embedded/{elementId}/v1`

Update embedded element.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | integer | Required |

**Body** (Personal.EmbeddedElement.Models.UpdateEmbeddedElementRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementProperties | Element properties. | Personal.Element.Models.ElementProperties |  |
| ElementSupportsAssessment | Element supports assessment. | Personal.Element.Models.ElementSupportsAssessment |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ElementProperties": {
    "Title": "sample string 1",
    "Visibility": {
      "IsActive": true,
      "ActiveFrom": "2026-11-18T16:31:46Z",
      "ActiveTo": "2026-11-18T16:31:46Z"
    },
    "AssignTo": {
      "ParticipantIds": [
        1
      ],
      "GroupIds": [
        1
      ],
      "GroupName": "sample string 1"
    },
    "LearningObjectivesIds": [
      1
    ]
  },
  "ElementSupportsAssessment": {
    "DeadlineSettings": {
      "Deadline": "2026-11-18T16:31:46Z",
      "IsSubmissionAfterDeadlineAllowed": true
    },
    "AssessmentStatusId": 1,
    "AssessmentId": 1,
    "MaxScore": 1.1,
    "IsAssessmentVisibleForStudents": true,
    "AddToAssessmentRecord": true,
    "AssessmentRecordCategoryId": 1,
    "TermId": 1,
    "IsAnonymous": true,
    "IsHomework": true,
    "IsMandatory": true,
    "IsOptional": true,
    "CompletionMode": "NotApplicable",
    "ExcludeFromTeacherTaskList": true,
    "IncludeToTeacherTaskList": true
  }
}
```
</details>

**Response:** `Personal.EmbeddedElement.Models.CreateUpdateEmbeddedElementResponse`

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Id of an element. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ElementId": 1
}
```
</details>


### `POST restapi/personal/element/embedded/v1`

Create embedded element.

**Body** (Personal.EmbeddedElement.Models.CreateEmbeddedElementRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ExtensionId | Id of extension. | integer |  |
| ElementProperties | Element properties. | Personal.Element.Models.ElementProperties |  |
| ElementContent | Embedded element content. | Personal.Element.Models.EmbeddedElementContent |  |
| CreateElementProperties | Create element properties. | Personal.Element.Models.CreateElementProperties |  |
| ElementSupportsAssessment | Element supports assessment. | Personal.Element.Models.ElementSupportsAssessment |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ExtensionId": 1,
  "ElementProperties": {
    "Title": "sample string 1",
    "Visibility": {
      "IsActive": true,
      "ActiveFrom": "2026-11-18T16:33:46Z",
      "ActiveTo": "2026-11-18T16:33:46Z"
    },
    "AssignTo": {
      "ParticipantIds": [
        1
      ],
      "GroupIds": [
        1
      ],
      "GroupName": "sample string 1"
    },
    "LearningObjectivesIds": [
      1
    ]
  },
  "ElementContent": {
    "ResourceId": "sample string 1"
  },
  "CreateElementProperties": {
    "ParentFolderId": 1,
    "PlannerSettings": {
      "PlanId": 1,
      "ColumnId": 2
    }
  },
  "ElementSupportsAssessment": {
    "DeadlineSettings": {
      "Deadline": "2026-11-18T16:33:46Z",
      "IsSubmissionAfterDeadlineAllowed": true
    },
    "AssessmentStatusId": 1,
    "AssessmentId": 1,
    "MaxScore": 1.1,
    "IsAssessmentVisibleForStudents": true,
    "AddToAssessmentRecord": true,
    "AssessmentRecordCategoryId": 1,
    "TermId": 1,
    "IsAnonymous": true,
    "IsHomework": true,
    "IsMandatory": true,
    "IsOptional": true,
    "CompletionMode": "NotApplicable",
    "ExcludeFromTeacherTaskList": true,
    "IncludeToTeacherTaskList": true
  }
}
```
</details>

**Response:** `Personal.EmbeddedElement.Models.CreateUpdateEmbeddedElementResponse`

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Id of an element. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ElementId": 1
}
```
</details>

