# Courses

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to Courses API.

## Endpoints

- `GET restapi/personal/courses/{courseId}/addparticipants/search/v1` — Search for persons to add to a course.
- `POST restapi/personal/courses/{courseId}/addparticipants/v1` — Add persons to add to a course. The user needs access to add persons to a course. The user can not add persons…
- `PUT restapi/personal/courses/{courseId}/aisettings/v1` — Updates the course.
- `GET restapi/personal/courses/{courseId}/applicants/v1` — 
- `GET restapi/personal/courses/{courseId}/calendarevents/v1` — Gets the calendar events for the given course.
- `POST restapi/personal/courses/{courseId}/changerole/bulk/v1` — 
- `POST restapi/personal/courses/{courseId}/changerole/v1` — 
- `POST restapi/personal/courses/{courseId}/folders/{folderId}/file/cloud/v1` — Creates a file element in the course containing the cloud file.
- `POST restapi/personal/courses/{courseId}/folders/{folderId}/file/filerepo/v1` — Creates a file element in the course containing the file.
- `GET restapi/personal/courses/{courseId}/folders/{folderId}/resources/v1` — Gets a list of resources for the given course and folder.
- `GET restapi/personal/courses/{courseId}/followuptasks/mobile/v1` — Should be used only by mobile app. Gets follow-up tasks together with tasks related to assignments that do not…
- `GET restapi/personal/courses/{courseId}/followuptasks/v1` — Gets the follow-up tasks for the given course.
- `PUT restapi/personal/courses/{courseId}/lastvisited/v1` — Updates the timestamp for when the user has last visited the course.
- `GET restapi/personal/courses/{courseId}/participantgroups/members/v1` — Retrieves mappings between course group identifiers and their member participant identifiers.
- `GET restapi/personal/courses/{courseId}/participantgroups/v1` — Retrieves a paginated list of participant groups in course.
- `GET restapi/personal/courses/{courseId}/participants/setup/v1` — Setup for the course participant and add participant page. Language settings and permissions.
- `POST restapi/personal/courses/{courseId}/participants/temporaryaccess/v1` — Give persons temporary access to a course.
- `GET restapi/personal/courses/{courseId}/participants/unenrolled/v1` — Gets a list of the unenrolled course participants for a given course.
- `DELETE restapi/personal/courses/{courseId}/participants/v1` — Removes one or more participants from the given courses
- `GET restapi/personal/courses/{courseId}/participants/v1` — Gets a list of the course participants for a given course.
- `DELETE restapi/personal/courses/{courseId}/participants/v2` — Removes one or more participants from the given courses
- `GET restapi/personal/courses/{courseId}/participants/v2` — Gets a list of the course participants for a given course.
- `GET restapi/personal/courses/{courseId}/participants/v3` — Gets a list of the course participants for a given course.
- `GET restapi/personal/courses/{courseId}/profiles/v1` — Information on available course profiles for this course
- `POST restapi/personal/courses/{courseId}/resources/extended/v1` — Returns all resources in course.
- `GET restapi/personal/courses/{courseId}/resources/lastupdated/v1` — Gets the last three updated resources for a given element type in a given course for the current user
- `GET restapi/personal/courses/{courseId}/resources/v1` — Gets a list of resources (in the root folder) for the given course.
- `GET restapi/personal/courses/{courseId}/searchhierarchies/v1` — Search syncable hierarchies for the course provided
- `GET restapi/personal/courses/{courseId}/stream/v1` — Gets user's stream feed, which is collected from the course.
- `GET restapi/personal/courses/{courseId}/students/v1` — Get students in course
- `GET restapi/personal/courses/{courseId}/synchronisedhierarchies/v1` — Get all hierarchies synchronised with the course provided
- `POST restapi/personal/courses/{courseId}/synchronisehierarchies/v1` — Sync hierarchies to course.
- `GET restapi/personal/courses/{courseId}/tasks/count/v1` — Retrieves followUp task count and task count for the given course for the current user.
- `GET restapi/personal/courses/{courseId}/tasks/v1` — Gets a list of tasks for the given course.
- `PUT RestApi/personal/courses/{courseId}/toggleFavorite/v1` — Toggles whether the user has this course favorite
- `GET restapi/personal/courses/{courseId}/topics/{topicId}/elementids/v1` — Provides the list of element ID's for a given topic in a course.
- `GET restapi/personal/courses/{courseId}/topics/v1` — Provides the list of topic ID's and names for a given course.
- `GET restapi/personal/courses/{courseId}/v1` — Retrieves some basic information on the given course for the current user.
- `PUT restapi/personal/courses/{courseId}/v1` — Updates the course.
- `GET restapi/personal/courses/{courseId}/yourstudents/v1` — Get a list of your students for a given course.
- `GET restapi/personal/courses/{locationId}/resources/search/v1` — Gets a list of resources in a given course, matching the search text provided
- `POST restapi/personal/courses/activeplans/v1` — Gets information about the courses with the specified sync keys and having active lesson plans within the spec…
- `GET restapi/personal/courses/addparticipants/search/v1` — Gets persons that the user is allowed to communicate with given the customer's eSafety rules configured in its…
- `POST restapi/personal/courses/convertfileorlinktofile` — Not in use will be removed in R108 (BMT-CONVERT)
- `POST restapi/personal/courses/convertfileorlinktogoogle` — Not in use will be removed in R108 (BMT-CONVERT)
- `POST restapi/personal/courses/convertfileorlinktolink` — Not in use will be removed in R108 (BMT-CONVERT)
- `POST restapi/personal/courses/convertfileorlinktoonedrive` — Not in use will be removed in R108 (BMT-CONVERT)
- `POST restapi/personal/courses/convertonedrivetofile` — Converts OneDriveResource to File
- `GET restapi/personal/courses/iteach/v1` — Retrieves a list with Courses for the given teacher.
- `POST restapi/personal/courses/measure/v1` — Measures course related kpis.
- `POST restapi/personal/courses/measurefilelinkingkpi/v1` — Endpoint for counting KPI's for files which are being linked when added to a course
- `POST restapi/personal/courses/measurefileuploadkpi/v1` — Endpoint for counting KPI's for files which are uploaded when added to a course
- `GET restapi/personal/courses/notifications/{notificationId}/elements/v1` — Gets the list of elements related to given notification
- `GET restapi/personal/courses/resources/{resourceId}/v1` — Gets the resource, the course where the resource is located and the current person's role in the course.
- `GET restapi/personal/courses/search/v1` — Gets the course by synchronization key.
- `GET restapi/personal/courses/students/v1` — Gets a list of my students.
- `POST restapi/personal/courses/v1` — Creates a new course.
- `GET restapi/personal/courses/v1` ⚠️ deprecated — Gets the list of courses. BUG: This endpoint returns only favourite courses USE v2 instead
- `GET restapi/personal/courses/v2` — Gets the list of courses.
- `GET restapi/personal/courses/v3` — Gets the list of courses with additional information about follow-up task count, task count and list of teache…
- `GET restapi/personal/courses/v4` — Gets the list of courses with additional information and list of teachers without followUp task count and task…
- `GET restapi/personal/courses/withrole/{courseId}/v1` — Retrieves basic information on the given course for the current user and their course role in this course.
- `GET restapi/personal/courses/yourstudents/courses/v1` — Gets all courses of your students of the current user.
- `GET restapi/personal/courses/yourstudents/v1` — Get a list of the all students for teacher in all courses where teacher has teacher's role.

---

### `GET restapi/personal/courses/{courseId}/addparticipants/search/v1?searchText={searchText}`

Search for persons to add to a course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| searchText | A phrase to search for | string |  |

**Response:** `EntityList<PersonSimple>` — A list of persons that can be added to the course. Will not contain users already added

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
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


### `POST restapi/personal/courses/{courseId}/addparticipants/v1`

Add persons to add to a course. The user needs access to add persons to a course. The user can not add persons with higher profiles than the user itself.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Body** (CourseParticipantToAdd)

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | Gets or sets the person identifier. | integer |  |
| CourseProfileId | Gets or sets the course profile identifier for the user. | integer |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "PersonId": 1,
    "CourseProfileId": 2
  }
]
```
</details>

**Response:** `?` — A list of persons that can be added to the course. Will not contain users already added


### `PUT restapi/personal/courses/{courseId}/aisettings/v1`

Updates the course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier | integer | Required |

**Body** (Personal.Course.CourseAiSettings)

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The course identifier | integer |  |
| Subject | The Subject of the course | string |  |
| Curriculum | Curriculum for the course | string |  |
| Language | Language for the AI generated content | string |  |
| Age | Age group of students in the course | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "CourseId": 1,
  "Subject": "sample string 2",
  "Curriculum": "sample string 3",
  "Language": "sample string 4",
  "Age": "sample string 5"
}
```
</details>

**Response:** `?`


### `GET restapi/personal/courses/{courseId}/applicants/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| PictureUrl | Avatar Url | string |  |
| Status | EnrrollmentStatus | integer |  |
| Mode | EnrrollmentMode | integer |  |
| Email | Email | string |  |
| IsExternal | Defines if the person is external. | boolean |  |
| CourseId | The course's identifier. | integer |  |
| IsMember | If true then the person is a member of the course. | boolean |  |
| DefaultOrganisation | Default organisation for the person | string |  |
| Role | Person role | string |  |
| CustomerName | The name of the customer site the person belongs to. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "PersonId": 1,
    "FullName": "sample string 2",
    "PictureUrl": "sample string 3",
    "Status": 4,
    "Mode": 5,
    "Email": "sample string 6",
    "IsExternal": true,
    "CourseId": 8,
    "IsMember": true,
    "DefaultOrganisation": "sample string 10",
    "Role": "sample string 11",
    "CustomerName": "sample string 12"
  }
]
```
</details>


### `GET restapi/personal/courses/{courseId}/calendarevents/v1?PageIndex={PageIndex}&PageSize={PageSize}&fromDate={fromDate}&toDate={toDate}`

Gets the calendar events for the given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| fromDate | From date for filtering calendar events. | date |  |
| toDate | To date for filtering calendar events. | date |  |

**Response:** `EntityList<Personal.CalendarEvent>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.CalendarEvent |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Id": 1,
      "FromDate": "2026-11-18T16:34:59Z",
      "ToDate": "2026-11-18T16:34:59Z",
      "EventTitle": "sample string 4",
      "Description": "sample string 5",
      "ImportDescription": "sample string 6",
      "CourseId": 7,
      "CourseGroupId": 8,
      "LocationColor": "sample string 9",
      "LocationFillColor": "sample string 10",
      "LocationTitle": "sample string 11",
      "LocationFriendlyName": "sample string 12",
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


### `POST restapi/personal/courses/{courseId}/changerole/bulk/v1?roleId={roleId}`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |
| roleId |  | integer | Required |

**Body:** Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`


### `POST restapi/personal/courses/{courseId}/changerole/v1?participantId={participantId}&roleId={roleId}`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |
| participantId |  | integer | Required |
| roleId |  | integer | Required |

**Response:** `?`


### `POST restapi/personal/courses/{courseId}/folders/{folderId}/file/cloud/v1?planId={planId}`

Creates a file element in the course containing the cloud file.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| folderId | The folder identifier. | integer | Required |
| planId | If files are added to a plan, supply this parameter so learning objectives and visibility settings are inherited from the plan | integer | Default value is 0 |

**Body** (Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | string |  |
| Name |  | string |  |
| SourceType |  | Itslearning.FileRepo.Common.Models.SourceType |  |
| MimeType |  | string |  |
| viewUrl |  | string |  |
| downloadUrl |  | string |  |
| editUrl |  | string |  |
| oAuthContext |  | Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthContext |  |
| isTemplate |  | boolean |  |
| isRegisteredWithCasper |  | boolean |  |
| isSupportedByCasper |  | boolean |  |
| PlagiarismReportUrl |  | string |  |

<details><summary>Request JSON sample</summary>

```json
[
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
          "expirationDateUtc": "2026-11-18T16:34:02Z"
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
```
</details>

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Element id for the file created | integer |  |
| CloudId | The cloud id of the file created | string |  |
| ErrorMessage | The error message if creation failed | string |  |
| ErrorCodes | The error message if creation failed | Collection of Personal.Course.FileErrorCode |  |
| Success | If the creation was successful or not | boolean |  |
| Title | The title for the newly created file element | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "ElementId": 1,
    "CloudId": "sample string 2",
    "ErrorMessage": "sample string 3",
    "ErrorCodes": [],
    "Success": true,
    "Title": "sample string 5"
  }
]
```
</details>


### `POST restapi/personal/courses/{courseId}/folders/{folderId}/file/filerepo/v1?planId={planId}`

Creates a file element in the course containing the file.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| folderId | The folder identifier. | integer | Required |
| planId | If files are added to a plan, supply this parameter so learning objectives and visibility settings are inherited from the plan | integer | Default value is 0 |

**Body** (Itslearning.Web.UI.Controls.Uploaders.Metadata.UploadMetadata)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | globally unique identifier |  |
| Name |  | string |  |
| SourceType |  | Itslearning.FileRepo.Common.Models.SourceType |  |
| MimeType |  | string |  |
| OriginalFileId |  | string |  |
| signedFileRepoId |  | string |  |
| signature |  | string |  |
| isTemplate |  | boolean |  |
| customId |  | string |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "id": "9723468c-ea3f-47d7-b640-56efac32a7e4",
    "name": "sample string 1",
    "sourceType": 1,
    "mimeType": "sample string 2",
    "OriginalFileId": "sample string 3",
    "signedFileRepoId": "sample string 4",
    "signature": "sample string 5",
    "isTemplate": true,
    "customId": "sample string 7"
  }
]
```
</details>

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Element id for the file created | integer |  |
| CloudId | The cloud id of the file created | string |  |
| ErrorMessage | The error message if creation failed | string |  |
| ErrorCodes | The error message if creation failed | Collection of Personal.Course.FileErrorCode |  |
| Success | If the creation was successful or not | boolean |  |
| Title | The title for the newly created file element | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "ElementId": 1,
    "CloudId": "sample string 2",
    "ErrorMessage": "sample string 3",
    "ErrorCodes": [],
    "Success": true,
    "Title": "sample string 5"
  }
]
```
</details>


### `GET restapi/personal/courses/{courseId}/folders/{folderId}/resources/v1?PageIndex={PageIndex}&PageSize={PageSize}&elementType={elementType}&learningToolType={learningToolType}`

Gets a list of resources for the given course and folder.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| folderId | The folder identifier. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| elementType | Filters returned resources that are not of the given type. Defaults to all. | Itsolutions.ItslUtils.Constants.ElementType | Default value is All |
| learningToolType | Filters returned learning tool elements that are not of the given type if provided. | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolType |  |

**Response:** `Personal.Course.CourseFolderDetails`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Resources | List of resources in a folder. | EntityList<Personal.Course.CourseResource> |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Resources": {
    "EntityArray": [
      {
        "Title": "sample string 1",
        "ElementId": 2,
        "ElementType": "Unknown",
        "CourseId": 3,
        "Url": "sample string 4",
        "ContentUrl": "sample string 5",
        "IconUrl": "sample string 6",
        "BrandIconUrl": "sample string 7",
        "ExtraImageUrlForMobile": "sample string 8",
        "Active": true,
        "LearningToolId": 10,
        "AddElementUrl": "sample string 11",
        "Homework": true,
        "Path": "sample string 13",
        "LearningObjectId": 14,
        "LearningObjectInstanceId": 15,
        "IsTopicOrPlanFolder": true
      }
    ],
    "Total": 1,
    "CurrentPageIndex": 2,
    "PageSize": 3
  },
  "AddElementUrl": "sample string 1"
}
```
</details>


### `GET restapi/personal/courses/{courseId}/followuptasks/mobile/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Should be used only by mobile app. Gets follow-up tasks together with tasks related to assignments that do not require a digital action from a student. This is a temp solution that will be addressed in ITS-104591

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<FollowUpTask>`

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


### `GET restapi/personal/courses/{courseId}/followuptasks/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the follow-up tasks for the given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
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


### `PUT restapi/personal/courses/{courseId}/lastvisited/v1`

Updates the timestamp for when the user has last visited the course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier | integer | Required |

**Response:** `System.Net.Http.HttpResponseMessage` — 200 OK or Exception

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `GET restapi/personal/courses/{courseId}/participantgroups/members/v1`

Retrieves mappings between course group identifiers and their member participant identifiers.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Course Id | integer | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupId | Course group identifier. | integer |  |
| Title | Course group title. | string |  |
| ParticipantIds | Course group participants identifiers. | Collection of integer |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "GroupId": 1,
    "Title": "sample string 2",
    "ParticipantIds": [
      1
    ]
  }
]
```
</details>


### `GET restapi/personal/courses/{courseId}/participantgroups/v1?pageIndex={pageIndex}&pageSize={pageSize}&search={search}`

Retrieves a paginated list of participant groups in course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Course Id | integer | Required |
| pageIndex | Page index for pagination | integer | Default value is 0 |
| pageSize | Number of items per page | integer | Default value is 100 |
| search | Search filter | string |  |

**Response:** `Personal.ContentAreaSettings.Models.ParticipantsGroupModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Groups | The array of participant group items. | Collection of Personal.ContentAreaSettings.Models.ParticipantGroupItemModel |  |
| TotalCount | The total count of groups available. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Groups": [
    {
      "GroupId": 1,
      "Title": "sample string 2"
    }
  ],
  "TotalCount": 1
}
```
</details>


### `GET restapi/personal/courses/{courseId}/participants/setup/v1`

Setup for the course participant and add participant page. Language settings and permissions.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Response:** `Personal.Course.PageSetup.CourseParticipantSetup` — Setup for the course participant page

| Name | Description | Type | Additional information |
|---|---|---|---|
| Language | Language for the course participant page. | Personal.Course.PageSetup.CourseParticipantLanguage |  |
| Paths | Paths for the course participant page. | Personal.Course.PageSetup.CourseParticipantPaths |  |
| Settings | Settings for the course participant page. | Personal.Course.PageSetup.CourseParticipantSettings |  |
| Permissions | Gets or sets the permissions for the course participant page. | Personal.Course.PageSetup.CourseParticipantPermissions |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Language": {
    "All": "sample string 1",
    "Add": "sample string 2",
    "AddHierarchy": "sample string 3",
    "AddParticipant": "sample string 4",
    "BrowseHierarchies": "sample string 5",
    "BrowseUsers": "sample string 6",
    "Cancel": "sample string 7",
    "ChangeRole": "sample string 8",
    "ChangeRoleFor": "sample string 9",
    "Close": "sample string 10",
    "ConfirmRemoveParticipant": "sample string 11",
    "ConfirmRemoveMultipleParticipants": "sample string 12",
    "CouldNotRemoveParticipant": "sample string 13",
    "CouldNotRemoveMultipleParticipants": "sample string 14",
    "Email": "sample string 15",
    "FilteredParticipantsCount": "sample string 16",
    "Freely": "sample string 17",
    "Hierarchy": "sample string 18",
    "LastVisited": "sample string 19",
    "MaxAllowedInstantMessageRecipientsExceeded": "sample string 20",
    "Name": "sample string 21",
    "NoParticipants": "sample string 22",
    "NoSearchResults": "sample string 23",
    "Ok": "sample string 24",
    "OpenProfileCardFor": "sample string 25",
    "Options": "sample string 26",
    "Participants": "sample string 27",
    "ParticipantsCount": "sample string 28",
    "Password": "sample string 29",
    "Unenrolled": "sample string 30",
    "Remove": "sample string 31",
    "RemoveParticipantsConfirmation": "sample string 32",
    "Role": "sample string 33",
    "Save": "sample string 34",
    "Search": "sample string 35",
    "SelectAll": "sample string 36",
    "SelectUsersFrom": "sample string 37",
    "SendMessage": "sample string 38",
    "Registration": "sample string 39",
    "Tasks": "sample string 40",
    "User": "sample string 41",
    "Users": "sample string 42",
    "AddParticipantsEmptyState": "sample string 43",
    "SynchronizedHierarchiesEmptyState": "sample string 44",
    "UnerolledParticipantsEmptyState": "sample string 45",
    "ApplicantsEmptyState": "sample string 46",
    "Applicants": "sample string 47",
    "ApplicationState": "sample string 48",
    "Approve": "sample string 49",
    "Reject": "sample string 50",
    "Accepted": "sample string 51",
    "Pending": "sample string 52",
    "Rejected": "sample string 53",
    "NoApplicants": "sample string 54",
    "CourseGroups": "sample string 55",
    "DateUnenrolled": "sample string 56",
    "TemporaryAccess": "sample string 57",
    "Assessment": "sample string 58",
... (truncated)
```
</details>


### `POST restapi/personal/courses/{courseId}/participants/temporaryaccess/v1`

Give persons temporary access to a course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Body** (CourseParticipants.CourseParticipantsTemporaryAccess)

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The ids of the participants | integer |  |
| TemporaryAccess | If the person has temporary access | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "PersonId": 1,
  "TemporaryAccess": true
}
```
</details>

**Response:** `?`


### `GET restapi/personal/courses/{courseId}/participants/unenrolled/v1?PageIndex={PageIndex}&PageSize={PageSize}&searchText={searchText}&orderByField={orderByField}&orderAscending={orderAscending}&dropDateFrom={dropDateFrom}`

Gets a list of the unenrolled course participants for a given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the course we want the participants from | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| searchText | [OPTIONAL] The text to search for | string |  |
| orderByField | [OPTIONAL] The field to sort by.Defaults to fullname. Example usage ?orderbyfield=droppeddt | string | Default value is fullName |
| orderAscending | [OPTIONAL] Whether we should sort ascending or not. Example usage ?orderAscending=false | boolean | Default value is True |
| dropDateFrom | [OPTIONAL] The field to filter participants by date when they are dropped. | date |  |

**Response:** `EntityList<Personal.Course.CourseParticipantUnenrolled>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.CourseParticipantUnenrolled |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "TemporaryAccess": true,
      "DateUnenrolled": "2026-11-18T16:32:42Z",
      "DateUnenrolledRelative": "sample string 4",
      "MemberOfCourseGroups": "sample string 5",
      "PictureUrl": "sample string 6",
      "AssessmentUrl": "sample string 7",
      "ExtraInformation": "sample string 8"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `DELETE restapi/personal/courses/{courseId}/participants/v1`

Removes one or more participants from the given courses

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the course | integer | Required |

**Body** (CourseParticipants.CourseParticipantsToRemove)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantIdsToBeRemoved | The ids of the participants to be removed from a course | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParticipantIdsToBeRemoved": [
    1
  ]
}
```
</details>

**Response:** `?`


### `GET restapi/personal/courses/{courseId}/participants/v1?PageIndex={PageIndex}&PageSize={PageSize}&courseProfileId={courseProfileId}&searchText={searchText}&orderByField={orderByField}&orderAscending={orderAscending}`

Gets a list of the course participants for a given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the course we want the participants from | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| courseProfileId | Participants with a specific course profile to get. If not present participants with all profiles are returned | integer |  |
| searchText | The text to search for (optional) | string |  |
| orderByField | [OPTIONAL] The field to sort by.Defaults to fullname. Example usage ?orderbyfield=lastvisited | string | Default value is fullName |
| orderAscending | [OPTIONAL] Whether we should sort ascending or not. Example usage ?orderAscending=false | boolean | Default value is True |

**Response:** `EntityList<Personal.Course.CourseParticipant>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.CourseParticipant |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "LastVisited": "2026-11-18T16:32:42Z",
      "LastVisitedRelative": "sample string 3",
      "PictureUrl": "sample string 4",
      "Role": "sample string 5",
      "RoleId": 6,
      "CompletedTasks": 7,
      "TotalTasks": 8,
      "ExtraInformation": "sample string 9",
      "CanHaveTasks": true,
      "Groups": "sample string 11",
      "TemporaryAccess": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `DELETE restapi/personal/courses/{courseId}/participants/v2`

Removes one or more participants from the given courses

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the course | integer | Required |

**Body** (CourseParticipants.CourseParticipantsToRemove)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantIdsToBeRemoved | The ids of the participants to be removed from a course | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParticipantIdsToBeRemoved": [
    1
  ]
}
```
</details>

**Response:** `System.Net.Http.HttpResponseMessage`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `GET restapi/personal/courses/{courseId}/participants/v2?paging.PageIndex={paging.PageIndex}&paging.PageSize={paging.PageSize}&courseProfileIds[0]={courseProfileIds[0]}&courseProfileIds[1]={courseProfileIds[1]}&searchText={searchText}&orderByField={orderByField}&orderAscending={orderAscending}`

Gets a list of the course participants for a given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the course we want the participants from | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| courseProfileIds | Participants with a specific course profiles to get. If not present participants with all profiles are returned | Collection of integer | Required |
| searchText | The text to search for (optional) | string |  |
| orderByField | [OPTIONAL] The field to sort by.Defaults to fullname. Example usage ?orderbyfield=lastvisited | string | Default value is fullName |
| orderAscending | [OPTIONAL] Whether we should sort ascending or not. Example usage ?orderAscending=false | boolean | Default value is True |

**Response:** `EntityList<Personal.Course.CourseParticipant>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.CourseParticipant |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "LastVisited": "2026-11-18T16:32:42Z",
      "LastVisitedRelative": "sample string 3",
      "PictureUrl": "sample string 4",
      "Role": "sample string 5",
      "RoleId": 6,
      "CompletedTasks": 7,
      "TotalTasks": 8,
      "ExtraInformation": "sample string 9",
      "CanHaveTasks": true,
      "Groups": "sample string 11",
      "TemporaryAccess": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/participants/v3?paging.PageIndex={paging.PageIndex}&paging.PageSize={paging.PageSize}&courseProfileIds[0]={courseProfileIds[0]}&courseProfileIds[1]={courseProfileIds[1]}&groupIds[0]={groupIds[0]}&groupIds[1]={groupIds[1]}&hierarchyIds[0]={hierarchyIds[0]}&hierarchyIds[1]={hierarchyIds[1]}&notInGroup={notInGroup}&searchText={searchText}&orderByField={orderByField}&orderAscending={orderAscending}`

Gets a list of the course participants for a given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The id of the course we want the participants from | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| courseProfileIds | Participants with a specific course profiles to get. If not present participants with all profiles are returned | Collection of integer | Required |
| groupIds | Participants within a specific group to get. If not present participants from all groups are returned | Collection of integer | Required |
| hierarchyIds | Participants within a specific hierarchy to get. If not present participants from all hierarchies are returned | Collection of integer | Required |
| notInGroup | If true then participants not belonging to any hierarchy groups are returned. If false and groupIds and hierarchyIds are not given then all participants are returned. | boolean | Default value is False |
| searchText | The text to search for (optional) | string |  |
| orderByField | [OPTIONAL] The field to sort by.Defaults to fullname. Example usage ?orderbyfield=lastvisited | string | Default value is fullName |
| orderAscending | [OPTIONAL] Whether we should sort ascending or not. Example usage ?orderAscending=false | boolean | Default value is True |

**Response:** `EntityList<Personal.Course.CourseParticipant>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.CourseParticipant |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "LastVisited": "2026-11-18T16:35:34Z",
      "LastVisitedRelative": "sample string 3",
      "PictureUrl": "sample string 4",
      "Role": "sample string 5",
      "RoleId": 6,
      "CompletedTasks": 7,
      "TotalTasks": 8,
      "ExtraInformation": "sample string 9",
      "CanHaveTasks": true,
      "Groups": "sample string 11",
      "TemporaryAccess": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/profiles/v1`

Information on available course profiles for this course

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Response:** `?` — Course profile information

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseProfileId | The course profile identifier. | integer |  |
| Title | The title for the course profile | string |  |
| ExistsInCourse | This indicates if the course profile is used by any participants in the course | boolean |  |
| CanBeUsedForAddingParticipantsToCourse | This indicating whether this instance can be used for adding participants to course. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "CourseProfileId": 1,
    "Title": "sample string 2",
    "ExistsInCourse": true,
    "CanBeUsedForAddingParticipantsToCourse": true
  }
]
```
</details>


### `POST restapi/personal/courses/{courseId}/resources/extended/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Returns all resources in course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| courseId | ID of course | integer | Required |

**Body** (Personal.Course.GetCourseResourcesPayload)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementType | Type of element | Itsolutions.ItslUtils.Constants.ElementType |  |
| LearningToolIds | IDs of learning tool (extension IDs) | Collection of integer |  |
| LearningToolType | Type of learning tool | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolType |  |
| ElementPermission | Permissions for the element | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.ElementPermission |  |
| ElementIds | Element IDs | Collection of integer |  |
| IncludeDeleted | Whether to include deleted resources | boolean |  |
| IncludeExtraWorkElements | Whether to include extra work elements | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ElementType": 0,
  "LearningToolIds": [
    1
  ],
  "LearningToolType": 0,
  "ElementPermission": 0,
  "ElementIds": [
    1
  ],
  "IncludeDeleted": true,
  "IncludeExtraWorkElements": true
}
```
</details>

**Response:** `EntityList<Personal.Course.CourseResource>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.CourseResource |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Title": "sample string 1",
      "ElementId": 2,
      "ElementType": "Unknown",
      "CourseId": 3,
      "Url": "sample string 4",
      "ContentUrl": "sample string 5",
      "IconUrl": "sample string 6",
      "BrandIconUrl": "sample string 7",
      "ExtraImageUrlForMobile": "sample string 8",
      "Active": true,
      "LearningToolId": 10,
      "AddElementUrl": "sample string 11",
      "Homework": true,
      "Path": "sample string 13",
      "LearningObjectId": 14,
      "LearningObjectInstanceId": 15,
      "IsTopicOrPlanFolder": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/resources/lastupdated/v1?elementType={elementType}&numberOfElements={numberOfElements}&excludeLearningPathSubElements={excludeLearningPathSubElements}`

Gets the last three updated resources for a given element type in a given course for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier | integer | Required |
| elementType | The element type | integer | Required |
| numberOfElements | [OPTIONAL] Number of elements to return for each element type. Value can be between 1 and 10. By default, this value is 3. | integer | Default value is 3 |
| excludeLearningPathSubElements | [OPTIONAL] If it is true sub-elements of a learning path will not be returned. | boolean | Default value is False |

**Response:** `EntityList<Personal.Course.CourseResource>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.CourseResource |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Title": "sample string 1",
      "ElementId": 2,
      "ElementType": "Unknown",
      "CourseId": 3,
      "Url": "sample string 4",
      "ContentUrl": "sample string 5",
      "IconUrl": "sample string 6",
      "BrandIconUrl": "sample string 7",
      "ExtraImageUrlForMobile": "sample string 8",
      "Active": true,
      "LearningToolId": 10,
      "AddElementUrl": "sample string 11",
      "Homework": true,
      "Path": "sample string 13",
      "LearningObjectId": 14,
      "LearningObjectInstanceId": 15,
      "IsTopicOrPlanFolder": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/resources/v1?PageIndex={PageIndex}&PageSize={PageSize}&elementType={elementType}&learningToolType={learningToolType}`

Gets a list of resources (in the root folder) for the given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| elementType | Filters returned resources that are not of the given type. Defaults to all. | Itsolutions.ItslUtils.Constants.ElementType | Default value is All |
| learningToolType | Filters returned learning tool elements that are not of the given type if provided. | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolType |  |

**Response:** `Personal.Course.CourseFolderDetails`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Resources | List of resources in a folder. | EntityList<Personal.Course.CourseResource> |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Resources": {
    "EntityArray": [
      {
        "Title": "sample string 1",
        "ElementId": 2,
        "ElementType": "Unknown",
        "CourseId": 3,
        "Url": "sample string 4",
        "ContentUrl": "sample string 5",
        "IconUrl": "sample string 6",
        "BrandIconUrl": "sample string 7",
        "ExtraImageUrlForMobile": "sample string 8",
        "Active": true,
        "LearningToolId": 10,
        "AddElementUrl": "sample string 11",
        "Homework": true,
        "Path": "sample string 13",
        "LearningObjectId": 14,
        "LearningObjectInstanceId": 15,
        "IsTopicOrPlanFolder": true
      }
    ],
    "Total": 1,
    "CurrentPageIndex": 2,
    "PageSize": 3
  },
  "AddElementUrl": "sample string 1"
}
```
</details>


### `GET restapi/personal/courses/{courseId}/searchhierarchies/v1?searchText={searchText}`

Search syncable hierarchies for the course provided

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course id. | integer | Required |
| searchText | The text to search for. Example usage ?searchText=class%20B2 | string | Required |

**Response:** `EntityList<HierarchySimpleWithMemberCount>` — The matching hierarchies with member count

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of HierarchySimpleWithMemberCount |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NumberOfMembers": 1,
      "HasChildren": true,
      "Synchable": true,
      "Inspectable": true,
      "HierarchyId": 5,
      "ParentHierarchyId": 6,
      "Title": "sample string 7",
      "Path": "sample string 8",
      "OrganizationType": 0,
      "OrganizationHierarchyId": 9
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/stream/v1?showLightBulletins={showLightBulletins}&UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets user's stream feed, which is collected from the course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Error if courseId equals 0The Course ID for the requested course | integer | Required |
| showLightBulletins | If True returns stream with light bulletins. | boolean | Default value is True |
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<StreamItemV2>` — The collection of StreamItems for the course.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of StreamItemV2 |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NotificationId": 1,
      "Text": "sample string 2",
      "LocationTitle": "sample string 3",
      "PublishedDate": "2026-11-18T16:35:31Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Url": "sample string 5",
      "ContentUrl": "sample string 6",
      "IconUrl": "sample string 7",
      "LightBulletin": {
        "LightBulletinId": 1,
        "Text": "sample string 2",
        "EmbedUrl": "sample string 3",
        "Pinned": true,
        "AllowComments": true,
        "HasResources": true,
        "AttachedImages": [
          {
            "PreviewFileId": 1,
            "PreviewFileUrl": "sample string 2",
            "OriginalFileId": 3,
            "OriginalFileUrl": "sample string 4",
            "PreviewFilePngUrl": "sample string 5"
          }
        ],
        "ResourcesCount": 7,
        "CommentsCount": 8,
        "ActiveFromDate": "2026-11-18T16:35:31Z",
        "ActiveToDate": "2026-11-18T16:35:31Z",
        "IsSubscribed": true,
        "LexicalJson": "sample string 12",
        "PublishedDate": "2026-11-18T16:35:31Z",
        "PublishedBy": {
          "PersonId": 1,
          "FirstName": "sample string 2",
          "LastName": "sample string 3",
          "FullName": "sample string 4",
          "ProfileUrl": "sample string 5",
          "AdditionalInfo": "sample string 6",
          "ProfileImageUrl": "sample string 7",
          "ProfileImageUrlSmall": "sample string 8"
        }
      },
      "ElementId": 8,
      "ElementType": "Unknown",
      "LearningToolId": 9,
      "HasMoreElements": true,
      "ElementsCount": 11,
      "LocationType": "Course",
... (truncated)
```
</details>


### `GET restapi/personal/courses/{courseId}/students/v1?pageIndex={pageIndex}&pageSize={pageSize}&search={search}`

Get students in course

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Course Id | integer | Required |
| pageIndex | Page index for pagination | integer | Default value is 0 |
| pageSize | Number of items per page | integer | Default value is 100 |
| search | Search filter | string |  |

**Response:** `Personal.ContentAreaSettings.Models.ParticipantsModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | The array of participant items. | Collection of Personal.ContentAreaSettings.Models.ParticipantItemModel |  |
| TotalCount | The total count of participants available. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Participants": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "ExtraInformation": "sample string 3",
      "PictureUrl": "sample string 4"
    }
  ],
  "TotalCount": 1
}
```
</details>


### `GET restapi/personal/courses/{courseId}/synchronisedhierarchies/v1`

Get all hierarchies synchronised with the course provided

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course id. | integer | Required |

**Response:** `EntityList<HierarchySimple>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of HierarchySimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "HierarchyId": 1,
      "ParentHierarchyId": 2,
      "Title": "sample string 3",
      "Path": "sample string 4",
      "OrganizationType": 0,
      "OrganizationHierarchyId": 5
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `POST restapi/personal/courses/{courseId}/synchronisehierarchies/v1`

Sync hierarchies to course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Body** (CourseHierarchyToSync)

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyId | Gets or sets the hierarchy identifier. | integer |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "HierarchyId": 1
  }
]
```
</details>

**Response:** `?`


### `GET restapi/personal/courses/{courseId}/tasks/count/v1`

Retrieves followUp task count and task count for the given course for the current user.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Course identifier of the course to retrieve | integer | Required |

**Response:** `PersonCourseCardTaskCount` — Object containing tasks count and followUp count

| Name | Description | Type | Additional information |
|---|---|---|---|
| FollowUpTaskCount | The follow-up tasks in course for a person. | integer |  |
| TaskCount | The tasks in course for a person. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "FollowUpTaskCount": 1,
  "TaskCount": 2
}
```
</details>


### `GET restapi/personal/courses/{courseId}/tasks/v1?PageIndex={PageIndex}&PageSize={PageSize}&status={status}&deadline={deadline}&isHomework={isHomework}`

Gets a list of tasks for the given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| status | Used for filtering tasks by status | TaskStatusFilter | Default value is Active |
| deadline | Used for filtering tasks by deadline | TaskDeadlineFilter | Default value is All |
| isHomework | Used for filtering tasks by homework property | boolean |  |

**Response:** `EntityList<Task>` — This method return a collection of entity . Each entity is a task that is assigned to the user and is not completed and not hidden.

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
      "Deadline": "2026-11-18T16:34:29Z",
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


### `PUT RestApi/personal/courses/{courseId}/toggleFavorite/v1`

Toggles whether the user has this course favorite

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course to toggle favorite for | integer | Required |

**Response:** `System.Net.Http.HttpResponseMessage` — OK

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `GET restapi/personal/courses/{courseId}/topics/{topicId}/elementids/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Provides the list of element ID's for a given topic in a course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | ID of course. | integer | Required |
| topicId | ID of topic. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<Personal.Course.ElementSimple>` — Returns a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Personal.Course.ElementSimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "ElementId": 1
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/topics/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Provides the list of topic ID's and names for a given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | ID of course to get topics. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<CourseTopic>` — Returns a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of CourseTopic |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Color": "sample string 1",
      "TopicId": 2,
      "TopicName": "sample string 3",
      "TopicMode": 1,
      "FolderId": 1
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/{courseId}/v1`

Retrieves some basic information on the given course for the current user.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Course identifier of the course to retrieve | integer | Required |

**Response:** `Course` — Object containing some course information

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Code": "sample string 1",
  "CourseId": 2,
  "CreatedDateTimeUtc": "2026-11-18T16:34:02Z",
  "Title": "sample string 3",
  "OrganisationId": 4,
  "CourseColorClass": "sample string 5",
  "SyncKey": "sample string 6"
}
```
</details>


### `PUT restapi/personal/courses/{courseId}/v1`

Updates the course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier | integer | Required |

**Body** (Course)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Code": "sample string 1",
  "CourseId": 2,
  "CreatedDateTimeUtc": "2026-11-18T16:31:46Z",
  "Title": "sample string 3",
  "OrganisationId": 4,
  "CourseColorClass": "sample string 5",
  "SyncKey": "sample string 6"
}
```
</details>

**Response:** `Course`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Code": "sample string 1",
  "CourseId": 2,
  "CreatedDateTimeUtc": "2026-11-18T16:31:46Z",
  "Title": "sample string 3",
  "OrganisationId": 4,
  "CourseColorClass": "sample string 5",
  "SyncKey": "sample string 6"
}
```
</details>


### `GET restapi/personal/courses/{courseId}/yourstudents/v1?PageIndex={PageIndex}&PageSize={PageSize}&sort={sort}&sortDescending={sortDescending}&search={search}`

Get a list of your students for a given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The Course ID for the requested course, if null it will fetch all students from all courses of the current user | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sort | Field to sort by | string | Default value is LastVisited |
| sortDescending | True if sorting ascending | boolean | Default value is True |
| search | Search term over student first and last name. | string | Default value is |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| FallbackPictureUrl | Used if main picture will not be loaded on client side. | string |  |
| CompletedTasks | Completed Tasks for this Course. | integer |  |
| TotalTasks | Total Tasks for this Course. | integer |  |
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| LastVisited | Last time the course participant visited this course | date |  |
| LastVisitedRelative | Last time the course participant visited this course relative to the current time Examples: 3 days ago, 2 hours ago, Just now, etc. | string |  |
| PictureUrl | Avatar Url | string |  |
| Role | The role that the course participant has in the course | string |  |
| RoleId | The role id that the course participant has in the course | integer |  |
| ExtraInformation | Extra information for this participant | string |  |
| CanHaveTasks | Show if course participant can have tasks | boolean |  |
| Groups | Comma-separated list of course groups the participant is a member of | string |  |
| TemporaryAccess | Boolean value indicating if the participant has been granted temporary access to the course | boolean |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "FallbackPictureUrl": "sample string 1",
    "CompletedTasks": 2,
    "TotalTasks": 3,
    "PersonId": 4,
    "FullName": "sample string 5",
    "LastVisited": "2026-11-18T16:34:59Z",
    "LastVisitedRelative": "sample string 6",
    "PictureUrl": "sample string 7",
    "Role": "sample string 8",
    "RoleId": 9,
    "ExtraInformation": "sample string 10",
    "CanHaveTasks": true,
    "Groups": "sample string 12",
    "TemporaryAccess": true
  }
]
```
</details>


### `GET restapi/personal/courses/{locationId}/resources/search/v1?locationType={locationType}&searchText={searchText}`

Gets a list of resources in a given course, matching the search text provided

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| locationId | The course or project identifier | integer | Required |
| locationType | The type of the location identifier | Itsolutions.ItslUtils.Constants.LocationType | Required |
| searchText | The search text | string | Default value is |

**Response:** `Personal.Course.CourseFolderDetails` — A collection of resources

| Name | Description | Type | Additional information |
|---|---|---|---|
| Resources | List of resources in a folder. | EntityList<Personal.Course.CourseResource> |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Resources": {
    "EntityArray": [
      {
        "Title": "sample string 1",
        "ElementId": 2,
        "ElementType": "Unknown",
        "CourseId": 3,
        "Url": "sample string 4",
        "ContentUrl": "sample string 5",
        "IconUrl": "sample string 6",
        "BrandIconUrl": "sample string 7",
        "ExtraImageUrlForMobile": "sample string 8",
        "Active": true,
        "LearningToolId": 10,
        "AddElementUrl": "sample string 11",
        "Homework": true,
        "Path": "sample string 13",
        "LearningObjectId": 14,
        "LearningObjectInstanceId": 15,
        "IsTopicOrPlanFolder": true
      }
    ],
    "Total": 1,
    "CurrentPageIndex": 2,
    "PageSize": 3
  },
  "AddElementUrl": "sample string 1"
}
```
</details>


### `POST restapi/personal/courses/activeplans/v1?startDate={startDate}&endDate={endDate}`

Gets information about the courses with the specified sync keys and having active lesson plans within the specified dates range.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| startDate |  | date | Required |
| endDate |  | date | Required |

**Body:** Collection of string

<details><summary>Request JSON sample</summary>

```json
[
  "sample string 1"
]
```
</details>

**Response:** `?` — List of objects containing course id, its name and the ids of active lesson plans.

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | Id of the course. | integer |  |
| CourseSyncKey | Course synchronization key. | string |  |
| CourseTitle | Title of the course. | string |  |
| PlanIds | List of active plans' ids. | Collection of string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "CourseId": 1,
    "CourseSyncKey": "sample string 2",
    "CourseTitle": "sample string 3",
    "PlanIds": [
      "sample string 1"
    ]
  }
]
```
</details>


### `GET restapi/personal/courses/addparticipants/search/v1?searchText={searchText}`

Gets persons that the user is allowed to communicate with given the customer's eSafety rules configured in itslearning.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| searchText | The person name to search for. | string | Required |

**Response:** `?` — List of Person Simple


### `POST restapi/personal/courses/convertfileorlinktofile`

Not in use will be removed in R108 (BMT-CONVERT)

**Body:** The elements to convert Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — OK


### `POST restapi/personal/courses/convertfileorlinktogoogle`

Not in use will be removed in R108 (BMT-CONVERT)

**Body:** The elements to convert Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — OK


### `POST restapi/personal/courses/convertfileorlinktolink`

Not in use will be removed in R108 (BMT-CONVERT)

**Body:** The elements to convert Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — OK


### `POST restapi/personal/courses/convertfileorlinktoonedrive`

Not in use will be removed in R108 (BMT-CONVERT)

**Body:** The elements to convert Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — OK


### `POST restapi/personal/courses/convertonedrivetofile`

Converts OneDriveResource to File

**Body:** The elements to convert Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — OK


### `GET restapi/personal/courses/iteach/v1?PageIndex={PageIndex}&PageSize={PageSize}&onlyWithResources={onlyWithResources}`

Retrieves a list with Courses for the given teacher.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| onlyWithResources | If sets to true gets only courses where resources are enabled. | boolean | Default value is False |

**Response:** `EntityList<Course>` — Entity list with Courses.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Course |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Code": "sample string 1",
      "CourseId": 2,
      "CreatedDateTimeUtc": "2026-11-18T16:35:34Z",
      "Title": "sample string 3",
      "OrganisationId": 4,
      "CourseColorClass": "sample string 5",
      "SyncKey": "sample string 6"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `POST restapi/personal/courses/measure/v1?count={count}`

Measures course related kpis.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| count |  | integer | Default value is 1 |

**Body** (Personal.Course.CourseAction)

<details><summary>Request JSON sample</summary>

```json
1
```
</details>

**Response:** `?`


### `POST restapi/personal/courses/measurefilelinkingkpi/v1`

Endpoint for counting KPI's for files which are being linked when added to a course

**Body** (Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | string |  |
| Name |  | string |  |
| SourceType |  | Itslearning.FileRepo.Common.Models.SourceType |  |
| MimeType |  | string |  |
| viewUrl |  | string |  |
| downloadUrl |  | string |  |
| editUrl |  | string |  |
| oAuthContext |  | Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthContext |  |
| isTemplate |  | boolean |  |
| isRegisteredWithCasper |  | boolean |  |
| isSupportedByCasper |  | boolean |  |
| PlagiarismReportUrl |  | string |  |

<details><summary>Request JSON sample</summary>

```json
[
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
          "expirationDateUtc": "2026-11-18T16:30:06Z"
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
```
</details>

**Response:** `System.Net.Http.HttpResponseMessage` — Returns an OK status code when succeeded

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `POST restapi/personal/courses/measurefileuploadkpi/v1`

Endpoint for counting KPI's for files which are uploaded when added to a course

**Body** (Itslearning.Web.UI.Controls.Uploaders.Metadata.UploadMetadata)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | globally unique identifier |  |
| Name |  | string |  |
| SourceType |  | Itslearning.FileRepo.Common.Models.SourceType |  |
| MimeType |  | string |  |
| OriginalFileId |  | string |  |
| signedFileRepoId |  | string |  |
| signature |  | string |  |
| isTemplate |  | boolean |  |
| customId |  | string |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "id": "0046a6ca-7077-4c6b-b2f4-f24187def550",
    "name": "sample string 1",
    "sourceType": 1,
    "mimeType": "sample string 2",
    "OriginalFileId": "sample string 3",
    "signedFileRepoId": "sample string 4",
    "signature": "sample string 5",
    "isTemplate": true,
    "customId": "sample string 7"
  }
]
```
</details>

**Response:** `System.Net.Http.HttpResponseMessage` — Returns an OK status code when succeeded

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `GET restapi/personal/courses/notifications/{notificationId}/elements/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the list of elements related to given notification

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| notificationId | The notification identifier | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<ElementLink>` — The collection of elements.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of ElementLink |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
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
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/resources/{resourceId}/v1`

Gets the resource, the course where the resource is located and the current person's role in the course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| resourceId | The resource identifier. | integer | Required |

**Response:** `Personal.Course.CourseResourceWithRole`

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseCode | Code of the course. | string |  |
| CourseTitle | Title of the course. | string |  |
| ContextRole | The context role for the current person . | PersonContextRole |  |
| Title | Title of element. | string |  |
| ElementId | Id of element. | integer |  |
| ElementType | Type of element. | ElementType |  |
| CourseId | Id of a course where the element is located. | integer |  |
| Url | Url to the itslearning web page for the element. | string |  |
| ContentUrl | Url to the content itslearning web page for the element. | string |  |
| IconUrl | Url to element icon. | string |  |
| BrandIconUrl | Url to a branded element icon. | string |  |
| ExtraImageUrlForMobile | Image that is set for the resource. Non-svg so mobile apps can use it. | string |  |
| Active | Whether element/folder is active or inactive. | boolean |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. | integer |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |
| Homework | Marks the element as homework. | boolean |  |
| Path | The resource's path in the course folder tree. | string |  |
| LearningObjectId | ID of learning object | integer |  |
| LearningObjectInstanceId | ID of learning object instance | integer |  |
| IsTopicOrPlanFolder | True when the element is a folder connected to a topic or a plan. Such folders must remain visible and expandable but must not be selectable when adding resources to a plan. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CourseCode": "sample string 1",
  "CourseTitle": "sample string 2",
  "ContextRole": 1,
  "Title": "sample string 3",
  "ElementId": 4,
  "ElementType": "Unknown",
  "CourseId": 5,
  "Url": "sample string 6",
  "ContentUrl": "sample string 7",
  "IconUrl": "sample string 8",
  "BrandIconUrl": "sample string 9",
  "ExtraImageUrlForMobile": "sample string 10",
  "Active": true,
  "LearningToolId": 12,
  "AddElementUrl": "sample string 13",
  "Homework": true,
  "Path": "sample string 15",
  "LearningObjectId": 16,
  "LearningObjectInstanceId": 17,
  "IsTopicOrPlanFolder": true
}
```
</details>


### `GET restapi/personal/courses/search/v1?courseSyncKey={courseSyncKey}`

Gets the course by synchronization key.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseSyncKey | The course synchronization key. | string | Required |

**Response:** `Course` — This method return the course entity

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Code": "sample string 1",
  "CourseId": 2,
  "CreatedDateTimeUtc": "2026-11-18T16:34:29Z",
  "Title": "sample string 3",
  "OrganisationId": 4,
  "CourseColorClass": "sample string 5",
  "SyncKey": "sample string 6"
}
```
</details>


### `GET restapi/personal/courses/students/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of my students.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonSimple>` — This method return a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
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


### `POST restapi/personal/courses/v1`

Creates a new course.

**Body** (Course)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Code": "sample string 1",
  "CourseId": 2,
  "CreatedDateTimeUtc": "2026-11-18T16:31:46Z",
  "Title": "sample string 3",
  "OrganisationId": 4,
  "CourseColorClass": "sample string 5",
  "SyncKey": "sample string 6"
}
```
</details>

**Response:** `Course`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Code": "sample string 1",
  "CourseId": 2,
  "CreatedDateTimeUtc": "2026-11-18T16:31:46Z",
  "Title": "sample string 3",
  "OrganisationId": 4,
  "CourseColorClass": "sample string 5",
  "SyncKey": "sample string 6"
}
```
</details>


### `GET restapi/personal/courses/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the list of courses. BUG: This endpoint returns only favourite courses USE v2 instead

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonCourse>` — This method return a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonCourse |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "LastUpdatedUtc": "2026-11-18T16:31:46Z",
      "NewNotificationsCount": 1,
      "NewBulletinsCount": 2,
      "Url": "sample string 3",
      "HasAdminPermissions": true,
      "HasStudentPermissions": true,
      "CourseFocus": 1,
      "TopicColumnName": "sample string 6",
      "CourseId": 7,
      "Title": "sample string 8",
      "FriendlyName": "sample string 9",
      "CourseColor": "sample string 10",
      "CourseFillColor": "sample string 11",
      "CourseCode": "sample string 12"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/v2?PageIndex={PageIndex}&PageSize={PageSize}&filter={filter}`

Gets the list of courses.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| filter | Option filter . | Itsolutions.ItslUtils.Constants.ContentAreaFilterType | Default value is All |

**Response:** `EntityList<PersonCourse>` — This method return a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonCourse |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "LastUpdatedUtc": "2026-11-18T16:34:59Z",
      "NewNotificationsCount": 1,
      "NewBulletinsCount": 2,
      "Url": "sample string 3",
      "HasAdminPermissions": true,
      "HasStudentPermissions": true,
      "CourseFocus": 1,
      "TopicColumnName": "sample string 6",
      "CourseId": 7,
      "Title": "sample string 8",
      "FriendlyName": "sample string 9",
      "CourseColor": "sample string 10",
      "CourseFillColor": "sample string 11",
      "CourseCode": "sample string 12"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/v3?PageIndex={PageIndex}&PageSize={PageSize}&filter={filter}`

Gets the list of courses with additional information about follow-up task count, task count and list of teachers.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| filter | Option filter . | Itsolutions.ItslUtils.Constants.ContentAreaFilterType | Default value is All |

**Response:** `EntityList<PersonCourseCardData>` — This method return a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonCourseCardData |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "FollowUpTaskCount": 1,
      "TaskCount": 2,
      "TeachersInCourse": [
        {
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
      "Role": 1,
      "PlanType": 0,
      "LastUpdatedUtc": "2026-11-18T16:32:22Z",
      "NewNotificationsCount": 3,
      "NewBulletinsCount": 4,
      "Url": "sample string 5",
      "HasAdminPermissions": true,
      "HasStudentPermissions": true,
      "CourseFocus": 1,
      "TopicColumnName": "sample string 8",
      "CourseId": 9,
      "Title": "sample string 10",
      "FriendlyName": "sample string 11",
      "CourseColor": "sample string 12",
      "CourseFillColor": "sample string 13",
      "CourseCode": "sample string 14"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/v4?PageIndex={PageIndex}&PageSize={PageSize}&filter={filter}`

Gets the list of courses with additional information and list of teachers without followUp task count and task count.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| filter | Option filter . | Itsolutions.ItslUtils.Constants.ContentAreaFilterType | Default value is All |

**Response:** `EntityList<PersonCourseCardDataV4>` — This method return a collection of entity .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonCourseCardDataV4 |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "TeachersInCourse": [
        {
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
      "Role": 1,
      "PlanType": 0,
      "LastUpdatedUtc": "2026-11-18T16:31:28Z",
      "NewNotificationsCount": 1,
      "NewBulletinsCount": 2,
      "Url": "sample string 3",
      "HasAdminPermissions": true,
      "HasStudentPermissions": true,
      "CourseFocus": 1,
      "TopicColumnName": "sample string 6",
      "CourseId": 7,
      "Title": "sample string 8",
      "FriendlyName": "sample string 9",
      "CourseColor": "sample string 10",
      "CourseFillColor": "sample string 11",
      "CourseCode": "sample string 12"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/withrole/{courseId}/v1`

Retrieves basic information on the given course for the current user and their course role in this course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Identifier of the course to retrieve | integer | Required |

**Response:** `CourseWithContextRole` — Object containing the course information and the role of the current user in this course.

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContextRole | The context role for the current person . | PersonContextRole |  |
| OrganisationLegalId | External identifier of the organisation the course is connected to. | string |  |
| OrganisationSyncKey | Sync key of the organisation the course is connected to. | string |  |
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ContextRole": 1,
  "OrganisationLegalId": "sample string 1",
  "OrganisationSyncKey": "sample string 2",
  "Code": "sample string 3",
  "CourseId": 4,
  "CreatedDateTimeUtc": "2026-11-18T16:30:06Z",
  "Title": "sample string 5",
  "OrganisationId": 6,
  "CourseColorClass": "sample string 7",
  "SyncKey": "sample string 8"
}
```
</details>


### `GET restapi/personal/courses/yourstudents/courses/v1`

Gets all courses of your students of the current user.

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | Title of the Course. | string |  |
| CourseId | Course ID. | integer |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Title": "sample string 1",
    "CourseId": 2
  }
]
```
</details>


### `GET restapi/personal/courses/yourstudents/v1?PageIndex={PageIndex}&PageSize={PageSize}&sort={sort}&sortDescending={sortDescending}&search={search}`

Get a list of the all students for teacher in all courses where teacher has teacher's role.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sort | Field to sort by | string | Default value is LastLogon |
| sortDescending | True if sorting ascending | boolean | Default value is True |
| search | Search term over student first and last name. | string | Default value is |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| FallbackPictureUrl | Used if main picture will not be loaded on client side. | string |  |
| CompletedTasks | Completed Tasks for this Course. | integer |  |
| TotalTasks | Total Tasks for this Course. | integer |  |
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| LastVisited | Last time the course participant visited this course | date |  |
| LastVisitedRelative | Last time the course participant visited this course relative to the current time Examples: 3 days ago, 2 hours ago, Just now, etc. | string |  |
| PictureUrl | Avatar Url | string |  |
| Role | The role that the course participant has in the course | string |  |
| RoleId | The role id that the course participant has in the course | integer |  |
| ExtraInformation | Extra information for this participant | string |  |
| CanHaveTasks | Show if course participant can have tasks | boolean |  |
| Groups | Comma-separated list of course groups the participant is a member of | string |  |
| TemporaryAccess | Boolean value indicating if the participant has been granted temporary access to the course | boolean |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "FallbackPictureUrl": "sample string 1",
    "CompletedTasks": 2,
    "TotalTasks": 3,
    "PersonId": 4,
    "FullName": "sample string 5",
    "LastVisited": "2026-11-18T16:31:46Z",
    "LastVisitedRelative": "sample string 6",
    "PictureUrl": "sample string 7",
    "Role": "sample string 8",
    "RoleId": 9,
    "ExtraInformation": "sample string 10",
    "CanHaveTasks": true,
    "Groups": "sample string 12",
    "TemporaryAccess": true
  }
]
```
</details>

