# Assignment

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for assignment for personal API. Requests will be forwarded to the assignment api. We want to abstract SharableObjectId (learningObjectId) and LearningObjectInstanceId from the client (e.g. mobile), therefore endpoints use ElementId. Secured with Oauth2, and requires OAuth2 API scope .

## Endpoints

- `GET restapi/personal/assignments/{elementId}/answers/{answerId}/assessments/files/{fileId}/v1` — Download the file attached to the assignment answer feedback.
- `GET restapi/personal/assignments/{elementId}/answers/{answerId}/assessments/files/v1` — Get info about files attached to assessment.
- `GET restapi/personal/assignments/{elementId}/answers/{answerId}/files/{fileId}/v1` — Download a file attached to an assignment answer. The user must have read access to the answer. The client mus…
- `GET restapi/personal/assignments/{elementId}/answers/{answerId}/v1` — Get assignment answer for an assignment
- `GET restapi/personal/assignments/{elementId}/answers/persons/{personId}/v1` — Get assignment answer for an assignment for a specific person. Not possible to use to get others' answers when…
- `POST restapi/personal/assignments/{elementId}/answers/v1` — Submit answer for an assignment, only to be used by students.
- `POST restapi/personal/assignments/{elementId}/assessments/v1` — 
- `POST restapi/personal/assignments/{elementId}/assessments/v2` — Submits an assessment for one or more students on a specific assignment. This endpoint is only accessible to u…
- `GET restapi/personal/assignments/{elementId}/files/{fileId}/v1` — Download a file attached to an assignment. The user must have read access to the assignment. The client must f…
- `GET restapi/personal/assignments/{elementId}/participants/v1` — Gets the participants report for a given assignment. Only users with evaluate permission can access this repor…
- `GET restapi/personal/assignments/{elementId}/participants/v2` — Gets the participants report for a given assignment. Only users with evaluate permission can access this repor…
- `GET restapi/personal/assignments/{elementId}/settings/v1` — Get settings for an assignment. This includes assessment scale (if used), and assessment status (if used).
- `POST restapi/personal/assignments/{elementId}/tasks/status/v1` — Set the task status for one or more students for a specific assignment where completion mode is ManualByStuden…
- `GET restapi/personal/assignments/{elementId}/v1` — Get information about an assignment
- `PUT restapi/personal/assignments/{elementId}/v1` — Update settings for an assignment.
- `POST restapi/personal/assignments/kpi/v1/openedMoreSettings` — This endpoint is called when the user opens the More settings section in the assignment form.
- `POST restapi/personal/assignments/v1` — Create a new assignment in a course, in a specific folder. This endpoint is currently not available using Oaut…

---

### `GET restapi/personal/assignments/{elementId}/answers/{answerId}/assessments/files/{fileId}/v1`

Download the file attached to the assignment answer feedback.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | Assignment element ID. | integer | Required |
| answerId | Answer the assessment is related to. | integer | Required |
| fileId | ID of the required file. | integer | Required |

**Response:** `?` — a Redirect (302) to the file download URL, or 404 if file not found


### `GET restapi/personal/assignments/{elementId}/answers/{answerId}/assessments/files/v1`

Get info about files attached to assessment.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | Assignment element ID. | integer | Required |
| answerId | Answer the assessment is related to. | integer | Required |

**Response:** `?` — List of

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId | The unique ID of the file. | integer |  |
| FileRepoId | File repo ID. | string |  |
| Name | Name of the file, including the extension. | string |  |
| MimeType | File MIME type. | string |  |
| Size | Size of the file in bytes. Only applicable for files stored in the file repository. | integer, 64 bit |  |
| LastModified | When the file was last modified, useful for clients that can cache a file in a secure way. Only applicable for files stored in the file repository. | date |  |
| DownloadAsAttachment | Defines whether current file cannot be previewed in a new browser tab and should rather be downloaded as attachment. | boolean |  |
| SignedFileRepoId | Signed file repo identifier. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "FileId": 1,
    "FileRepoId": "sample string 2",
    "Name": "sample string 3",
    "MimeType": "sample string 4",
    "Size": 1,
    "LastModified": "2026-11-18T16:34:59Z",
    "DownloadAsAttachment": true,
    "SignedFileRepoId": "sample string 6"
  }
]
```
</details>


### `GET restapi/personal/assignments/{elementId}/answers/{answerId}/files/{fileId}/v1`

Download a file attached to an assignment answer. The user must have read access to the answer. The client must follow the redirect to get the file.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |
| answerId | The answerId which belongs to the given elementId | integer | Required |
| fileId | The fileId connected to the requested answer. | integer | Required |

**Response:** `?` — a Redirect (302) to the file download URL, or 404 if file not found


### `GET restapi/personal/assignments/{elementId}/answers/{answerId}/v1`

Get assignment answer for an assignment

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |
| answerId | The answerId | integer | Required |

**Response:** `Personal.Assignment.Responses.AssignmentAnswerResponse` — when request was successful (200 - ok)

| Name | Description | Type | Additional information |
|---|---|---|---|
| AnswerId | The unique identifier for the answer to the assignment. | integer |  |
| IsDraft | True if the answer is a draft. | boolean |  |
| Submitted | The time when answer was submitted, or last modified in case of a draft. | date |  |
| Text | The text of the answer, which may contain HTML. | string |  |
| Files | The files attached to the answer. | Collection of Personal.Assignment.Responses.AssignmentFileModel |  |

<details><summary>Response JSON sample</summary>

```json
{
  "AnswerId": 1,
  "IsDraft": true,
  "Submitted": "2026-11-18T16:34:59Z",
  "Text": "sample string 4",
  "Files": [
    {
      "FileId": 1,
      "FileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "Size": 1,
      "LastModified": "2026-11-18T16:34:59Z",
      "DownloadAsAttachment": true,
      "SignedFileRepoId": "sample string 6"
    }
  ]
}
```
</details>


### `GET restapi/personal/assignments/{elementId}/answers/persons/{personId}/v1`

Get assignment answer for an assignment for a specific person. Not possible to use to get others' answers when assignment is anonymous.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |
| personId | The personId to get answer for | integer | Required |

**Response:** `Personal.Assignment.Responses.AssignmentAnswerResponse` — when request was successful (200 - ok)

| Name | Description | Type | Additional information |
|---|---|---|---|
| AnswerId | The unique identifier for the answer to the assignment. | integer |  |
| IsDraft | True if the answer is a draft. | boolean |  |
| Submitted | The time when answer was submitted, or last modified in case of a draft. | date |  |
| Text | The text of the answer, which may contain HTML. | string |  |
| Files | The files attached to the answer. | Collection of Personal.Assignment.Responses.AssignmentFileModel |  |

<details><summary>Response JSON sample</summary>

```json
{
  "AnswerId": 1,
  "IsDraft": true,
  "Submitted": "2026-11-18T16:31:28Z",
  "Text": "sample string 4",
  "Files": [
    {
      "FileId": 1,
      "FileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "Size": 1,
      "LastModified": "2026-11-18T16:31:28Z",
      "DownloadAsAttachment": true,
      "SignedFileRepoId": "sample string 6"
    }
  ]
}
```
</details>


### `POST restapi/personal/assignments/{elementId}/answers/v1`

Submit answer for an assignment, only to be used by students.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |

**Body** (Personal.Assignment.Requests.SubmitAnswerRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Text | Text for the answer, which may contain HTML. | string |  |
| Files | A collection of files that are uploaded as part of the answer, temp files uploaded using the . | Collection of Personal.FileUpload.UploadedTempFile |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Text": "sample string 1",
  "Files": [
    {
      "FileRepoId": "sample string 1",
      "SignedFileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "SourceType": 1,
      "IsTemplate": true
    }
  ]
}
```
</details>

**Response:** `Personal.Assignment.Responses.SubmitAnswerResponse`

| Name | Description | Type | Additional information |
|---|---|---|---|
| AnswerId | The id for the answer. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "AnswerId": 1
}
```
</details>


### `POST restapi/personal/assignments/{elementId}/assessments/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | integer | Required |

**Body** (Personal.Assignment.Requests.SubmitAssignmentAssessmentRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to submit the assessment for. If the list contains more than one person, the same assessment will be applied to all of them. | Collection of integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used. Null also means "no change". | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. Null also means "no change". | integer |  |
| Feedback | Feedback from teacher, can contain HTML. Null to not change existing feedback, empty string to clear existing feedback. | string |  |
| Score | Entered score value in case when ElementSettingsResponse.UseScore is true. Null means "no change", pass -1 to clear existing score. | decimal number |  |
| TaskStatus | If this is passed, AssessmentStatusItemId will be ignored. Used when the assignment uses simple statuses. Null also means "no change". | TaskStatus |  |
| FeedbackFiles | A collection of files that are uploaded as part of the assessment, temp files uploaded using the . | Collection of Personal.FileUpload.UploadedTempFile |  |

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
  "TaskStatus": "Unknown",
  "FeedbackFiles": [
    {
      "FileRepoId": "sample string 1",
      "SignedFileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "SourceType": 1,
      "IsTemplate": true
    }
  ]
}
```
</details>

**Response:** `Personal.Element.Responses.SubmitElementAssessmentResponse`

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


### `POST restapi/personal/assignments/{elementId}/assessments/v2`

Submits an assessment for one or more students on a specific assignment. This endpoint is only accessible to users with evaluate permission (typically teachers).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique within the customer scope. | integer | Required |

**Body** (Personal.Assignment.Requests.SubmitAssignmentAssessmentRequestV2)

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to submit the assessment for. If the list contains more than one person, the same assessment will be applied to all of them. | Collection of integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used. Null also means "no change". | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. Null also means "no change". | integer |  |
| Feedback | Feedback from teacher, can contain HTML. Null to not change existing feedback, empty string to clear existing feedback. | string |  |
| Score | Entered score value in case when ElementSettingsResponse.UseScore is true. Null means "no change", pass -1 to clear existing score. | decimal number |  |
| TaskStatus | If this is passed, AssessmentStatusItemId will be ignored. Used when the assignment uses simple statuses. Null also means "no change". | TaskStatus |  |
| DeletedFileIds | List of assessment related file ids to be deleted. | Collection of integer |  |
| NewFiles | List of temp files to add to the list of attached to assessment files. | Collection of Personal.FileUpload.UploadedTempFile |  |
| ModifiedFiles | List of assessment's modified files. | Collection of Personal.FileUpload.ModifiedElementFileModel |  |
| FilesInCloud | Files added to the cloud. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |

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
  "TaskStatus": "Unknown",
  "DeletedFileIds": [
    1
  ],
  "NewFiles": [
    {
      "FileRepoId": "sample string 1",
      "SignedFileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "SourceType": 1,
      "IsTemplate": true
    }
  ],
  "ModifiedFiles": [
    {
      "FileId": 1,
      "Name": "sample string 2"
    }
  ],
  "FilesInCloud": [
    {
      "id": "sample string 1",
      "name": "sample string 2",
      "sourceType": 1,
      "mimeType": "sample string 3",
      "viewUrl": "sample string 4",
      "downloadUrl": "sample string 5",
      "editUrl": "sample string 6",
      "oAuthContext": {
        "oAuthToken": {
          "accessToken": {
            "value": "sample string 1",
            "expirationDateUtc": "2026-11-18T16:31:28Z"
          },
          "refreshToken": "sample string 1",
          "refreshTokenExpired": true
        },
        "oAuthUser": {
          "email": "sample string 1",
          "clientId": "sample string 2",
          "mustBeImpersonated": true
        }
      },
      "isTemplate": true,
      "isRegisteredWithCasper": true,
      "isSupportedByCasper": true,
      "PlagiarismReportUrl": "sample string 10"
    }
  ]
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


### `GET restapi/personal/assignments/{elementId}/files/{fileId}/v1`

Download a file attached to an assignment. The user must have read access to the assignment. The client must follow the redirect to get the file.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |
| fileId | The fileId connected to the requested assignment. | integer | Required |

**Response:** `?` — a Redirect (302) to the file download URL, or 404 if file not found


### `GET restapi/personal/assignments/{elementId}/participants/v1?personIds={personIds}&groupIds={groupIds}&searchText={searchText}&PageIndex={PageIndex}&PageSize={PageSize}&sortBy={sortBy}&sortOrder={sortOrder}`

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

**Response:** `Personal.Element.Responses.ParticipantsReportResponse` — List of participants ( ) when request was successful (200 - ok)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | The list of participants who currently have access to the element (e.g. an assignment). Participants who have submitted work but no longer have access to the element are not included. | Collection of Personal.Element.Models.ParticipantReport |  |
| TotalCount | Total count of participants who have access to the element, for paging purposes. This can be higher than the number of items in the "Participants" list, if paging is used. | integer |  |
| ExtraInfoDisplayName | The localized display name for what the "ExtraInfo" holds, if any. Can be e.g. "Username". Can be null if no extra info is provided or if the user does not have access to such data. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Participants": [
    {
      "PersonId": 1,
      "FirstName": "sample string 2",
      "LastName": "sample string 3",
      "FullName": "sample string 4",
      "ExtraInfo": "sample string 5",
      "ProfilePictureUrl": "sample string 6",
      "Submitted": "2026-11-18T16:31:28Z",
      "AssessmentStatusItemId": 1,
      "AssessmentItemId": 1,
      "RawFeedback": "sample string 7",
      "Feedback": "sample string 8",
      "Score": 1.1,
      "TaskStatus": "Unknown"
    }
  ],
  "TotalCount": 1,
  "ExtraInfoDisplayName": "sample string 2"
}
```
</details>


### `GET restapi/personal/assignments/{elementId}/participants/v2?personIds={personIds}&groupIds={groupIds}&searchText={searchText}&PageIndex={PageIndex}&PageSize={PageSize}&sortBy={sortBy}&sortOrder={sortOrder}`

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
      "Submitted": "2026-11-18T16:34:29Z",
      "AssessmentStatusItemId": 1,
      "AssessmentItemId": 1,
      "AssessmentByPersonId": 1,
      "AssessmentDateTime": "2026-11-18T16:34:29Z",
      "RawFeedback": "sample string 2",
      "Feedback": "sample string 3",
      "FeedbackByPersonId": 1,
      "FeedbackDateTime": "2026-11-18T16:34:29Z",
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


### `GET restapi/personal/assignments/{elementId}/settings/v1`

Get settings for an assignment. This includes assessment scale (if used), and assessment status (if used).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |

**Response:** `Personal.Element.Responses.ElementSettingsResponse` — Settings for the assignment

| Name | Description | Type | Additional information |
|---|---|---|---|
| StatusScale | The assessment status which is used to set a status item on an answer (submission). Can be null if no assessment status is defined for the element. | Personal.Element.Models.StatusScale |  |
| AssessmentScale | The assessment scale which is used to set a grade on an answer (submission). Can be null if no assessment scale is defined for the element. | Personal.Element.Models.AssessmentScale |  |
| CustomLabels | Custom labels associated with the element. | Collection of Personal.Element.Responses.CustomLabelModel |  |
| Terms | The list of terms used in course. | Collection of Itsolutions.Itslearning.Web.Areas.Elements.Models.SelectItem |  |
| Categories | The list of assessment record categories used in course. | Collection of Itsolutions.Itslearning.Web.Areas.Elements.Models.SelectItem |  |

<details><summary>Response JSON sample</summary>

```json
{
  "StatusScale": {
    "AssessmentStatusId": 1,
    "Title": "sample string 2",
    "Description": "sample string 3",
    "StatusItems": [
      {
        "AssessmentStatusItemId": 1,
        "Title": "sample string 2",
        "IsEditable": true,
        "IsCompleted": true,
        "IsSubmitted": true,
        "IsInitialStatus": true
      }
    ],
    "UseSimpleStatus": true
  },
  "AssessmentScale": {
    "AssessmentId": 1,
    "Title": "sample string 2",
    "Description": "sample string 3",
    "AssessmentItems": [
      {
        "AssessmentItemId": 1,
        "Title": "sample string 2",
        "Description": "sample string 3",
        "PercentTo": 4.1,
        "PercentFromAndIncl": 5.1
      }
    ]
  },
  "CustomLabels": [
    {
      "Id": 1,
      "Name": "sample string 2",
      "Description": "sample string 3",
      "Color": "sample string 4"
    }
  ],
  "Terms": [
    {
      "Value": 1,
      "Name": "sample string 2"
    }
  ],
  "Categories": [
    {
      "Value": 1,
      "Name": "sample string 2"
    }
  ]
}
```
</details>


### `POST restapi/personal/assignments/{elementId}/tasks/status/v1`

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


### `GET restapi/personal/assignments/{elementId}/v1`

Get information about an assignment

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |

**Response:** `Personal.Assignment.Responses.AssignmentModel` — when request was successful (200 - ok)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | The unique identifier for the assignment element (which is used in a course) in scope of a customer. | integer |  |
| Title | The title of the assignment | string |  |
| IsHomework | Defines whether the assignment is a homework assignment. | boolean |  |
| IsMandatory | Defines whether the assignment is mandatory. | boolean |  |
| IsLearningPathSubElement | True if the assignment is part of a learning path. | boolean |  |
| Deadline | Deadline for the assignment. If null, there is no deadline. | date |  |
| UsePlagiarismCheck | Defines whether answers will be checked for plagiarism. When true there are some restrictions on the files that can be uploaded. | boolean |  |
| ShowPlagiarismResultsToStudents | Defines whether the plagiarism results should be visible for students. | boolean |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the assignment, will be null if assessment scale is not used. | integer |  |
| MaxScore | Max score used for assessment using score. | decimal number |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the assignment, will be null if no status is used. | integer |  |
| IsAssessmentVisibleForStudents | Defines whether the assessment is visible for students. | boolean |  |
| IsActive | Indicates whether the element visibility setting is set to "Active" or "Scheduled". The value of false means "Inactive". | boolean |  |
| ActiveFrom | Active from date and time in UTC. If set, the element will be active from this date. | date |  |
| ActiveTo | Active to date and time in UTC. If set, the element will be active until this date. | date |  |
| IsSubmissionAfterDeadlineAllowed | Defines whether submission after the deadline is allowed. | boolean |  |
| SubmissionType | Submission type for the assignment | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| CompletionMode | Indicates whether submission is required for this assignment, or if it can be marked as completed without submission. ElementCompletionMode.ManualByTeacher and ElementCompletionMode.ManualByStudent means that assignment is in "task" mode, otherwise submission is required - standard old assignment functionality. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. | boolean |  |
| IsAnonymous | Defines whether submission is anonymous. | boolean |  |
| IsSelfAssessmentRequired | Defines whether self-assessment is required. | boolean |  |
| PeersToAssessCount | How many other students should each student peer assess. null means that peer assessment is not used. | integer |  |
| IsPeerAssessmentAnonymous | Defines whether peer assessment is anonymous. Only relevant if is set. | boolean |  |
| Files | Files attached to the assignment. | Collection of Personal.Assignment.Responses.AssignmentFileModel |  |
| TemplateFile | Template file. Students get their own copy of this file. There could only be not more than one template file per assignment. | Personal.Assignment.Responses.AssignmentFileModel |  |
| FilesInCloud | Cloud files. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| RawText | The raw text of the assignment, suitable for loading into the rich text editor when editing the assignment. It is not display-rendered, so it can be edited and saved again without corrupting embedded content (tree links, media, plugins). | string |  |
| Text | The assignment text processed for display: embedded files, media and plugins are rendered. Use this for showing the assignment, and for editing. | string |  |
| AnswerId | If the user has an answer, this will be the ID of that answer. Otherwise, it will be null. | integer |  |
| CanEditAssignment | True if the user can edit the assignment itself - Text and Files, false otherwise. | boolean |  |
| CanEvaluateAnswers | Returns true if the user can evaluate answers, normally a teacher permission. | boolean |  |
| CanSubmitOrEditAnswer | Returns true if the user can edit or submit answer (or set own completion, in case of CompletionMode = "ManualByStudent"). Rules for deadline, status and if the user can participate are taken into consideration. | boolean |  |
| CanParticipate | Returns true if the user is allowed to participate in this assignment. A user could have guest role in course, which only allows to view the assignment, but not to submit or edit answer. | boolean |  |
| TermId | The ID of term if the element is added into assessment record. | integer |  |
| AssessmentRecordCategoryId | Which category in assessment record should be used for current element, if it is added into assessment record. | integer |  |
| AddedToAssessmentRecord | If the current element is added into assessment record. | boolean |  |
| CustomLabelId | Custom label id | integer |  |
| LearningObjectiveSettings | Learning objective-related settings. | Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveSettingsModel |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ElementId": 1,
  "Title": "sample string 2",
  "IsHomework": true,
  "IsMandatory": true,
  "IsLearningPathSubElement": true,
  "Deadline": "2026-11-18T16:35:31Z",
  "UsePlagiarismCheck": true,
  "ShowPlagiarismResultsToStudents": true,
  "AssessmentId": 1,
  "MaxScore": 1.1,
  "AssessmentStatusId": 1,
  "IsAssessmentVisibleForStudents": true,
  "IsActive": true,
  "ActiveFrom": "2026-11-18T16:35:31Z",
  "ActiveTo": "2026-11-18T16:35:31Z",
  "IsSubmissionAfterDeadlineAllowed": true,
  "SubmissionType": "Individual",
  "CompletionMode": "NotApplicable",
  "ExcludeFromTeacherTaskList": true,
  "IsAnonymous": true,
  "IsSelfAssessmentRequired": true,
  "PeersToAssessCount": 1,
  "IsPeerAssessmentAnonymous": true,
  "Files": [
    {
      "FileId": 1,
      "FileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "Size": 1,
      "LastModified": "2026-11-18T16:35:31Z",
      "DownloadAsAttachment": true,
      "SignedFileRepoId": "sample string 6"
    }
  ],
  "TemplateFile": {
    "FileId": 1,
    "FileRepoId": "sample string 2",
    "Name": "sample string 3",
    "MimeType": "sample string 4",
    "Size": 1,
    "LastModified": "2026-11-18T16:35:31Z",
    "DownloadAsAttachment": true,
    "SignedFileRepoId": "sample string 6"
  },
  "FilesInCloud": [
    {
      "id": "sample string 1",
      "name": "sample string 2",
      "sourceType": 1,
      "mimeType": "sample string 3",
      "viewUrl": "sample string 4",
      "downloadUrl": "sample string 5",
      "editUrl": "sample string 6",
      "oAuthContext": {
        "oAuthToken": {
          "accessToken": {
            "value": "sample string 1",
            "expirationDateUtc": "2026-11-18T16:35:31Z"
... (truncated)
```
</details>


### `PUT restapi/personal/assignments/{elementId}/v1`

Update settings for an assignment.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The elementId for the assignment, unique in scope of a customer. | integer | Required |

**Body** (Personal.Assignment.Requests.UpdateAssignmentRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | The title of the element. | string |  |
| Visibility | Defines visibility settings. Pass null if you don't want to make any changes. | Personal.Element.Models.VisibilitySettings |  |
| AssignTo | Defines who to assign the element to (specific users and/or groups). If null, or if no specific Ids are provided: When creating element: Permissions will be the same as for parent folder.When updating element: No changes will be made to permissions. | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds | List of learning objective IDs to associate with this assignment. If null, no changes will be made to learning objectives associations. If empty, all associations will be removed. | Collection of integer |  |
| DeadlineSettings | Deadline settings. Pass null if you don't want to make any changes. | Personal.Element.Models.DeadlineSettings |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the element. If null, it will not be changed when updating the element. | integer |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the element. If null, it will not be changed when updating the element. To remove assessmentId, use 0 | integer |  |
| MaxScore | Max score in case if score is in use instead of assessment scale. If null, it will not be changed when updating the element. To remove maxScore, use any negative value, e.g. -1 | decimal number |  |
| IsAssessmentVisibleForStudents | Defines whether the assessment is visible for students (aka "Results are visible to students"). If null, it will not be changed when updating the element. | boolean |  |
| AddToAssessmentRecord | Defines whether current element should be added into assessment record. If null, it will not be changed when updating the element. | boolean |  |
| AssessmentRecordCategoryId | Which category in assessment record should be used for current element, if it is added into assessment record. Null means that it will not be changed when updating the element, or not connected. To remove connection to category, use 0. | integer |  |
| TermId | The ID of term in case of adding element into assessment record. If null, it will not be changed when updating the element. | integer |  |
| IsAnonymous | Defines whether submission is anonymous. If null, it will not be changed when updating the element. | boolean |  |
| IsHomework | Defines whether the element is homework. If null, it will not be changed when updating the element. | boolean |  |
| IsMandatory | Defines whether submission is mandatory. If null, it will not be changed when updating the element. | boolean |  |
| CompletionMode | Indicates whether submission is required for this element to complete the connected task, or if it can be marked as completed without submission. If null, it will not be changed when updating the element. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. If null, it will not be changed when updating the element. | boolean |  |
| DeletedFileIds | List of file ids to deleted, connected to the element. | Collection of integer |  |
| NewFiles | List of files to be added to the element, attachments to the element. These files should be already uploaded as temporary files using file upload API, and here you should provide their temporary file ids. | Collection of Personal.FileUpload.UploadedTempFile |  |
| ModifiedFiles |  | Collection of Personal.FileUpload.ModifiedElementFileModel |  |
| FilesInCloud |  | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| Text |  | string |  |
| UsePlagiarismCheck |  | boolean |  |
| ShowPlagiarismResultsToStudents |  | boolean |  |
| SubmissionType |  | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| PeersToAssessCount |  | integer |  |
| IsPeerAssessmentAnonymous |  | boolean |  |
| IsSelfAssessmentRequired |  | boolean |  |
| CustomLabelIds |  | Collection of integer |  |
| WorkloadEstimationInMinutes |  | integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Title": "sample string 1",
  "Visibility": {
    "IsActive": true,
    "ActiveFrom": "2026-11-18T16:33:14Z",
    "ActiveTo": "2026-11-18T16:33:14Z"
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
  ],
  "DeadlineSettings": {
    "Deadline": "2026-11-18T16:33:14Z",
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
  "CompletionMode": 1,
  "ExcludeFromTeacherTaskList": true,
  "DeletedFileIds": [
    1
  ],
  "NewFiles": [
    {
      "FileRepoId": "sample string 1",
      "SignedFileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "SourceType": 1,
      "IsTemplate": true
    }
  ],
  "ModifiedFiles": [
    {
      "FileId": 1,
      "Name": "sample string 2"
    }
  ],
  "FilesInCloud": [
    {
      "id": "sample string 1",
      "name": "sample string 2",
      "sourceType": 1,
      "mimeType": "sample string 3",
... (truncated)
```
</details>

**Response:** `Personal.Assignment.Responses.AssignmentModel` — True if update was successful, false otherwise

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | The unique identifier for the assignment element (which is used in a course) in scope of a customer. | integer |  |
| Title | The title of the assignment | string |  |
| IsHomework | Defines whether the assignment is a homework assignment. | boolean |  |
| IsMandatory | Defines whether the assignment is mandatory. | boolean |  |
| IsLearningPathSubElement | True if the assignment is part of a learning path. | boolean |  |
| Deadline | Deadline for the assignment. If null, there is no deadline. | date |  |
| UsePlagiarismCheck | Defines whether answers will be checked for plagiarism. When true there are some restrictions on the files that can be uploaded. | boolean |  |
| ShowPlagiarismResultsToStudents | Defines whether the plagiarism results should be visible for students. | boolean |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the assignment, will be null if assessment scale is not used. | integer |  |
| MaxScore | Max score used for assessment using score. | decimal number |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the assignment, will be null if no status is used. | integer |  |
| IsAssessmentVisibleForStudents | Defines whether the assessment is visible for students. | boolean |  |
| IsActive | Indicates whether the element visibility setting is set to "Active" or "Scheduled". The value of false means "Inactive". | boolean |  |
| ActiveFrom | Active from date and time in UTC. If set, the element will be active from this date. | date |  |
| ActiveTo | Active to date and time in UTC. If set, the element will be active until this date. | date |  |
| IsSubmissionAfterDeadlineAllowed | Defines whether submission after the deadline is allowed. | boolean |  |
| SubmissionType | Submission type for the assignment | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| CompletionMode | Indicates whether submission is required for this assignment, or if it can be marked as completed without submission. ElementCompletionMode.ManualByTeacher and ElementCompletionMode.ManualByStudent means that assignment is in "task" mode, otherwise submission is required - standard old assignment functionality. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. | boolean |  |
| IsAnonymous | Defines whether submission is anonymous. | boolean |  |
| IsSelfAssessmentRequired | Defines whether self-assessment is required. | boolean |  |
| PeersToAssessCount | How many other students should each student peer assess. null means that peer assessment is not used. | integer |  |
| IsPeerAssessmentAnonymous | Defines whether peer assessment is anonymous. Only relevant if is set. | boolean |  |
| Files | Files attached to the assignment. | Collection of Personal.Assignment.Responses.AssignmentFileModel |  |
| TemplateFile | Template file. Students get their own copy of this file. There could only be not more than one template file per assignment. | Personal.Assignment.Responses.AssignmentFileModel |  |
| FilesInCloud | Cloud files. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| RawText | The raw text of the assignment, suitable for loading into the rich text editor when editing the assignment. It is not display-rendered, so it can be edited and saved again without corrupting embedded content (tree links, media, plugins). | string |  |
| Text | The assignment text processed for display: embedded files, media and plugins are rendered. Use this for showing the assignment, and for editing. | string |  |
| AnswerId | If the user has an answer, this will be the ID of that answer. Otherwise, it will be null. | integer |  |
| CanEditAssignment | True if the user can edit the assignment itself - Text and Files, false otherwise. | boolean |  |
| CanEvaluateAnswers | Returns true if the user can evaluate answers, normally a teacher permission. | boolean |  |
| CanSubmitOrEditAnswer | Returns true if the user can edit or submit answer (or set own completion, in case of CompletionMode = "ManualByStudent"). Rules for deadline, status and if the user can participate are taken into consideration. | boolean |  |
| CanParticipate | Returns true if the user is allowed to participate in this assignment. A user could have guest role in course, which only allows to view the assignment, but not to submit or edit answer. | boolean |  |
| TermId | The ID of term if the element is added into assessment record. | integer |  |
| AssessmentRecordCategoryId | Which category in assessment record should be used for current element, if it is added into assessment record. | integer |  |
| AddedToAssessmentRecord | If the current element is added into assessment record. | boolean |  |
| CustomLabelId | Custom label id | integer |  |
| LearningObjectiveSettings | Learning objective-related settings. | Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveSettingsModel |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ElementId": 1,
  "Title": "sample string 2",
  "IsHomework": true,
  "IsMandatory": true,
  "IsLearningPathSubElement": true,
  "Deadline": "2026-11-18T16:33:14Z",
  "UsePlagiarismCheck": true,
  "ShowPlagiarismResultsToStudents": true,
  "AssessmentId": 1,
  "MaxScore": 1.1,
  "AssessmentStatusId": 1,
  "IsAssessmentVisibleForStudents": true,
  "IsActive": true,
  "ActiveFrom": "2026-11-18T16:33:14Z",
  "ActiveTo": "2026-11-18T16:33:14Z",
  "IsSubmissionAfterDeadlineAllowed": true,
  "SubmissionType": "Individual",
  "CompletionMode": "NotApplicable",
  "ExcludeFromTeacherTaskList": true,
  "IsAnonymous": true,
  "IsSelfAssessmentRequired": true,
  "PeersToAssessCount": 1,
  "IsPeerAssessmentAnonymous": true,
  "Files": [
    {
      "FileId": 1,
      "FileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "Size": 1,
      "LastModified": "2026-11-18T16:33:14Z",
      "DownloadAsAttachment": true,
      "SignedFileRepoId": "sample string 6"
    }
  ],
  "TemplateFile": {
    "FileId": 1,
    "FileRepoId": "sample string 2",
    "Name": "sample string 3",
    "MimeType": "sample string 4",
    "Size": 1,
    "LastModified": "2026-11-18T16:33:14Z",
    "DownloadAsAttachment": true,
    "SignedFileRepoId": "sample string 6"
  },
  "FilesInCloud": [
    {
      "id": "sample string 1",
      "name": "sample string 2",
      "sourceType": 1,
      "mimeType": "sample string 3",
      "viewUrl": "sample string 4",
      "downloadUrl": "sample string 5",
      "editUrl": "sample string 6",
      "oAuthContext": {
        "oAuthToken": {
          "accessToken": {
            "value": "sample string 1",
            "expirationDateUtc": "2026-11-18T16:33:14Z"
... (truncated)
```
</details>


### `POST restapi/personal/assignments/kpi/v1/openedMoreSettings`

This endpoint is called when the user opens the More settings section in the assignment form.

**Response:** `?`


### `POST restapi/personal/assignments/v1`

Create a new assignment in a course, in a specific folder. This endpoint is currently not available using Oauth2.

**Body** (Personal.Assignment.Requests.CreateAssignmentRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The folder to create the assignment in (must be a folder in a course). | integer |  |
| Title | The title of the assignment. | string |  |
| Visibility | Defines visibility settings. | Personal.Element.Models.VisibilitySettings |  |
| AssignTo | Defines who to assign the element (e.g. assignment) to (specific users and/or groups). If null, or if no specific Ids are provided, the assignment is assigned to all users in the parent folder. | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds | List of learning objective IDs to associate with this assignment. | Collection of integer |  |
| DeadlineSettings | Deadline settings. | Personal.Element.Models.DeadlineSettings |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the assignment, will be null if no status is used. | integer |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the assignment, will be null if assessment scale is not used. | integer |  |
| MaxScore |  | decimal number |  |
| IsAssessmentVisibleForStudents |  | boolean |  |
| AddToAssessmentRecord |  | boolean |  |
| AssessmentRecordCategoryId |  | integer |  |
| TermId |  | integer |  |
| IsAnonymous | Defines whether submission is anonymous. | boolean |  |
| IsHomework | Defines whether the assignment is a homework assignment. | boolean |  |
| IsMandatory | Defines whether submission is mandatory. | boolean |  |
| CompletionMode | Indicates whether submission of an answer is required for this assignment, or if it can be marked as completed without submission. ElementCompletionMode.ManualByTeacher and ElementCompletionMode.ManualByStudent means that assignment is in "task" mode, otherwise submission is required - standard old assignment functionality. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. | boolean |  |
| Text |  | string |  |
| UsePlagiarismCheck |  | boolean |  |
| ShowPlagiarismResultsToStudents |  | boolean |  |
| SubmissionType |  | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| PeersToAssessCount |  | integer |  |
| IsPeerAssessmentAnonymous |  | boolean |  |
| IsSelfAssessmentRequired |  | boolean |  |
| CustomLabelIds |  | Collection of integer |  |
| WorkloadEstimationInMinutes |  | integer |  |
| Files | Attachments to be added to the assignment. Files (e.g. images) embedded in , should NOT be included here. | Collection of Personal.FileUpload.UploadedTempFile |  |
| FilesInCloud | Newly added files in cloud. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| PlannerSettings | Planner context for linking the assignment to a plan. Null when not connected to the planner. | Personal.Element.Models.PlannerSettings |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Title": "sample string 2",
  "Visibility": {
    "IsActive": true,
    "ActiveFrom": "2026-11-18T16:32:42Z",
    "ActiveTo": "2026-11-18T16:32:42Z"
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
  ],
  "DeadlineSettings": {
    "Deadline": "2026-11-18T16:32:42Z",
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
  "CompletionMode": 1,
  "ExcludeFromTeacherTaskList": true,
  "Text": "sample string 3",
  "UsePlagiarismCheck": true,
  "ShowPlagiarismResultsToStudents": true,
  "SubmissionType": "Individual",
  "PeersToAssessCount": 1,
  "IsPeerAssessmentAnonymous": true,
  "IsSelfAssessmentRequired": true,
  "CustomLabelIds": [
    1
  ],
  "WorkloadEstimationInMinutes": 1,
  "Files": [
    {
      "FileRepoId": "sample string 1",
      "SignedFileRepoId": "sample string 2",
      "Name": "sample string 3",
      "MimeType": "sample string 4",
      "SourceType": 1,
      "IsTemplate": true
    }
  ],
  "FilesInCloud": [
    {
      "id": "sample string 1",
... (truncated)
```
</details>

**Response:** `integer`

<details><summary>Response JSON sample</summary>

```json
1
```
</details>

