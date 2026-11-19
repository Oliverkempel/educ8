# CourseCards

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for CourseCards API

## Endpoints

- `GET restapi/personal/courses/cards/{courseId}/settings/v1` — Gets the course card settings
- `POST restapi/personal/courses/cards/{courseId}/settings/v1` — Apply course card information to course.
- `POST restapi/personal/courses/cards/rank/v1` — 
- `GET restapi/personal/courses/cards/starred/v1` — Gets the list of cards for user's starred courses
- `GET restapi/personal/courses/cards/unstarred/v1` — Gets the list of cards for user's unstarred courses

---

### `GET restapi/personal/courses/cards/{courseId}/settings/v1`

Gets the course card settings

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course Id | integer | Required |

**Response:** `CourseCardSettings` — This method returns the course card settings .

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseTitle | Course title. | string |  |
| FriendlyName | Friendly course name. | string |  |
| CourseColorClass | Course colour. | string |  |
| VisibleCourseCardCandidateIds | Teacher representatives id's, comma separated | string |  |
| VisibleCourseCardCandidates | Teacher representatives | Collection of CourseCardTeacher |  |
| CourseCardImageFileRepoId | The ID of course card image in the FileRepo. | globally unique identifier |  |
| CourseCardImageUrl | Signed FileRepo URL of course card image which is used to display image on the user interface. | string |  |
| CourseCardImageSignedFileRepoId | Signed FileRepo URL of course card image which is used to check the validation when uploading a new image. | string |  |
| CanEditCourseColour | True if person can change the colour | boolean |  |
| CanEditFriendlyName | True if person can change the friendly name | boolean |  |
| CourseCardImageDescription | Image description for course card | string |  |
| SaveImageToYourImagesForEasyReuse | Defines whether newly uploaded course card image should be saved into media repository to be reused in "Your images" tab later. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CourseTitle": "sample string 1",
  "FriendlyName": "sample string 2",
  "CourseColorClass": "sample string 3",
  "VisibleCourseCardCandidateIds": "sample string 4",
  "VisibleCourseCardCandidates": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "ProfileUrl": "sample string 3",
      "IsVisibleOnCourseCard": true
    }
  ],
  "CourseCardImageFileRepoId": "805fc41c-079b-4f36-a209-81559a559689",
  "CourseCardImageUrl": "sample string 6",
  "CourseCardImageSignedFileRepoId": "sample string 7",
  "CanEditCourseColour": true,
  "CanEditFriendlyName": true,
  "CourseCardImageDescription": "sample string 10",
  "SaveImageToYourImagesForEasyReuse": true
}
```
</details>


### `POST restapi/personal/courses/cards/{courseId}/settings/v1`

Apply course card information to course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Body** (CourseCardSettings)

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseTitle | Course title. | string |  |
| FriendlyName | Friendly course name. | string |  |
| CourseColorClass | Course colour. | string |  |
| VisibleCourseCardCandidateIds | Teacher representatives id's, comma separated | string |  |
| VisibleCourseCardCandidates | Teacher representatives | Collection of CourseCardTeacher |  |
| CourseCardImageFileRepoId | The ID of course card image in the FileRepo. | globally unique identifier |  |
| CourseCardImageUrl | Signed FileRepo URL of course card image which is used to display image on the user interface. | string |  |
| CourseCardImageSignedFileRepoId | Signed FileRepo URL of course card image which is used to check the validation when uploading a new image. | string |  |
| CanEditCourseColour | True if person can change the colour | boolean |  |
| CanEditFriendlyName | True if person can change the friendly name | boolean |  |
| CourseCardImageDescription | Image description for course card | string |  |
| SaveImageToYourImagesForEasyReuse | Defines whether newly uploaded course card image should be saved into media repository to be reused in "Your images" tab later. | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "CourseTitle": "sample string 1",
  "FriendlyName": "sample string 2",
  "CourseColorClass": "sample string 3",
  "VisibleCourseCardCandidateIds": "sample string 4",
  "VisibleCourseCardCandidates": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "ProfileUrl": "sample string 3",
      "IsVisibleOnCourseCard": true
    }
  ],
  "CourseCardImageFileRepoId": "eb01ffd0-5177-47e0-be1e-d0171674f372",
  "CourseCardImageUrl": "sample string 6",
  "CourseCardImageSignedFileRepoId": "sample string 7",
  "CanEditCourseColour": true,
  "CanEditFriendlyName": true,
  "CourseCardImageDescription": "sample string 10",
  "SaveImageToYourImagesForEasyReuse": true
}
```
</details>

**Response:** `CourseCardSettings`

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseTitle | Course title. | string |  |
| FriendlyName | Friendly course name. | string |  |
| CourseColorClass | Course colour. | string |  |
| VisibleCourseCardCandidateIds | Teacher representatives id's, comma separated | string |  |
| VisibleCourseCardCandidates | Teacher representatives | Collection of CourseCardTeacher |  |
| CourseCardImageFileRepoId | The ID of course card image in the FileRepo. | globally unique identifier |  |
| CourseCardImageUrl | Signed FileRepo URL of course card image which is used to display image on the user interface. | string |  |
| CourseCardImageSignedFileRepoId | Signed FileRepo URL of course card image which is used to check the validation when uploading a new image. | string |  |
| CanEditCourseColour | True if person can change the colour | boolean |  |
| CanEditFriendlyName | True if person can change the friendly name | boolean |  |
| CourseCardImageDescription | Image description for course card | string |  |
| SaveImageToYourImagesForEasyReuse | Defines whether newly uploaded course card image should be saved into media repository to be reused in "Your images" tab later. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CourseTitle": "sample string 1",
  "FriendlyName": "sample string 2",
  "CourseColorClass": "sample string 3",
  "VisibleCourseCardCandidateIds": "sample string 4",
  "VisibleCourseCardCandidates": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "ProfileUrl": "sample string 3",
      "IsVisibleOnCourseCard": true
    }
  ],
  "CourseCardImageFileRepoId": "b83ed2f8-8043-473b-961a-251847b54703",
  "CourseCardImageUrl": "sample string 6",
  "CourseCardImageSignedFileRepoId": "sample string 7",
  "CanEditCourseColour": true,
  "CanEditFriendlyName": true,
  "CourseCardImageDescription": "sample string 10",
  "SaveImageToYourImagesForEasyReuse": true
}
```
</details>


### `POST restapi/personal/courses/cards/rank/v1`

_No description._

**Body** (Models.PersonCourseRankWithKpi)

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseRanks |  | Collection of PersonCourseRank |  |
| kpiOperation |  | Models.KpiOperation |  |

<details><summary>Request JSON sample</summary>

```json
{
  "courseRanks": [
    {
      "CourseId": 1,
      "Rank": 2
    }
  ],
  "kpiOperation": 1
}
```
</details>

**Response:** `?`


### `GET restapi/personal/courses/cards/starred/v1?PageIndex={PageIndex}&PageSize={PageSize}&sortBy={sortBy}&searchText={searchText}&isShowMore={isShowMore}`

Gets the list of cards for user's starred courses

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sortBy | The property to sort by. | string | Default value is Title |
| searchText | Text search in course title, friendly name field. | string |  |
| isShowMore | Request is from a show more button click | boolean | Default value is False |

**Response:** `EntityList<CourseCard>` — This method returns a collection of course cards entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of CourseCard |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CourseId": 1,
      "FriendlyName": "sample string 2",
      "Title": "sample string 3",
      "TeacherName": "sample string 4",
      "TeacherPictureUrl": "sample string 5",
      "NumberOfTeachers": 6,
      "Teachers": [
        {
          "PersonId": 1,
          "FullName": "sample string 2",
          "ProfileUrl": "sample string 3",
          "IsVisibleOnCourseCard": true
        }
      ],
      "CourseColorClass": "sample string 7",
      "NumberOfAnnouncements": 8,
      "NumberOfTasks": 9,
      "NumberOfFollowUpTasks": 10,
      "CourseCardImageFileRepoId": "0d7ff5d0-9eff-4c3c-8027-2bcedf3ee0f5",
      "CourseCardImageUrl": "sample string 11",
      "CourseCardImageDescription": "sample string 12",
      "LastUpdatedDisplayTime": "sample string 13",
      "LastUpdated": "2026-11-18T16:30:06Z",
      "LastOnlineDisplayTime": "sample string 14",
      "LastOnline": "2026-11-18T16:30:06Z",
      "IsFavouriteCourse": true,
      "NotificationsEnabled": true,
      "CanAdminCourse": true,
      "IsCompletedCourse": true,
      "CourseType": 1
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/courses/cards/unstarred/v1?PageIndex={PageIndex}&PageSize={PageSize}&sortBy={sortBy}&searchText={searchText}&isShowMore={isShowMore}`

Gets the list of cards for user's unstarred courses

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sortBy | The property to sort by. | string | Default value is Title |
| searchText | Text search in course title, friendly name field. | string |  |
| isShowMore | Request is from a show more button click | boolean | Default value is False |

**Response:** `EntityList<CourseCard>` — This method returns a collection of course cards entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of CourseCard |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CourseId": 1,
      "FriendlyName": "sample string 2",
      "Title": "sample string 3",
      "TeacherName": "sample string 4",
      "TeacherPictureUrl": "sample string 5",
      "NumberOfTeachers": 6,
      "Teachers": [
        {
          "PersonId": 1,
          "FullName": "sample string 2",
          "ProfileUrl": "sample string 3",
          "IsVisibleOnCourseCard": true
        }
      ],
      "CourseColorClass": "sample string 7",
      "NumberOfAnnouncements": 8,
      "NumberOfTasks": 9,
      "NumberOfFollowUpTasks": 10,
      "CourseCardImageFileRepoId": "038d2dcd-c95f-4ac3-b423-565c13ea72b2",
      "CourseCardImageUrl": "sample string 11",
      "CourseCardImageDescription": "sample string 12",
      "LastUpdatedDisplayTime": "sample string 13",
      "LastUpdated": "2026-11-18T16:33:14Z",
      "LastOnlineDisplayTime": "sample string 14",
      "LastOnline": "2026-11-18T16:33:14Z",
      "IsFavouriteCourse": true,
      "NotificationsEnabled": true,
      "CanAdminCourse": true,
      "IsCompletedCourse": true,
      "CourseType": 1
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

