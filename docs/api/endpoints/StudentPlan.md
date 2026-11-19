# StudentPlan

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for Student Plans in Personal API. Secured with Oauth2, and requires OAuth2 API scope StudentPlan.

## Endpoints

- `GET restapi/personal/studentplan/achievementgoals/active/v1` — Gets the active goals. The course identifier.
- `GET restapi/personal/studentplan/achievementgoals/archived/v1` — Gets the archived goals. The course identifier.
- `GET restapi/personal/studentplan/achievementgoals/coursewide/v1` — 
- `GET restapi/personal/studentplan/assessments/topics/v1` — Returns list of topic assessments for the course and student.
- `GET restapi/personal/studentplan/assessments/topics/v1` — Returns list of topic assessments for the course and student.
- `GET restapi/personal/studentplan/comments/v1` — Retrieves a list of comments that relates to specific person student plan.
- `GET restapi/personal/studentplan/courses/v1` — Returns a list of courses for the Student Plan.
- `GET restapi/personal/studentplan/overview/v1` — Returns an overview entity for the Student Plan.

---

### `GET restapi/personal/studentplan/achievementgoals/active/v1?courseId={courseId}`

Gets the active goals. The course identifier.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| GoalId | The goal identifier. | integer |  |
| Status | The status. | StudentPlan.AchievementGoals.AchievementGoalStatusType |  |
| Description | The description. | string |  |
| TeacherAssessment | The teacher assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| StudentAssessment | The student assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| AssessmentItems | The assessment items. | Collection of StudentPlan.AchievementGoals.AssessmentItem |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "GoalId": 1,
    "Status": "Active",
    "Description": "sample string 2",
    "TeacherAssessment": {
      "AssessmentPerson": {
        "PersonName": "sample string 1",
        "ProfileImageUrl": "sample string 2"
      },
      "AssessmentTitle": "sample string 1",
      "AssessmentLevel": 2
    },
    "StudentAssessment": {
      "AssessmentPerson": {
        "PersonName": "sample string 1",
        "ProfileImageUrl": "sample string 2"
      },
      "AssessmentTitle": "sample string 1",
      "AssessmentLevel": 2
    },
    "AssessmentItems": [
      {
        "Value": 1,
        "Name": "sample string 2"
      }
    ]
  }
]
```
</details>


### `GET restapi/personal/studentplan/achievementgoals/archived/v1?courseId={courseId}`

Gets the archived goals. The course identifier.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| GoalId | The goal identifier. | integer |  |
| Status | The status. | StudentPlan.AchievementGoals.AchievementGoalStatusType |  |
| Description | The description. | string |  |
| TeacherAssessment | The teacher assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| StudentAssessment | The student assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| AssessmentItems | The assessment items. | Collection of StudentPlan.AchievementGoals.AssessmentItem |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "GoalId": 1,
    "Status": "Active",
    "Description": "sample string 2",
    "TeacherAssessment": {
      "AssessmentPerson": {
        "PersonName": "sample string 1",
        "ProfileImageUrl": "sample string 2"
      },
      "AssessmentTitle": "sample string 1",
      "AssessmentLevel": 2
    },
    "StudentAssessment": {
      "AssessmentPerson": {
        "PersonName": "sample string 1",
        "ProfileImageUrl": "sample string 2"
      },
      "AssessmentTitle": "sample string 1",
      "AssessmentLevel": 2
    },
    "AssessmentItems": [
      {
        "Value": 1,
        "Name": "sample string 2"
      }
    ]
  }
]
```
</details>


### `GET restapi/personal/studentplan/achievementgoals/coursewide/v1`

_No description._

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| GoalId | The goal identifier. | integer |  |
| Description | The description. | string |  |
| Course | The course. | StudentPlan.StudentPlanCourseSimple |  |
| TeacherAssessment | The teacher assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| StudentAssessment | The student assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "GoalId": 1,
    "Description": "sample string 2",
    "Course": {
      "Id": 1,
      "Title": "sample string 2",
      "Color": "sample string 3"
    },
    "TeacherAssessment": {
      "AssessmentPerson": {
        "PersonName": "sample string 1",
        "ProfileImageUrl": "sample string 2"
      },
      "AssessmentTitle": "sample string 1",
      "AssessmentLevel": 2
    },
    "StudentAssessment": {
      "AssessmentPerson": {
        "PersonName": "sample string 1",
        "ProfileImageUrl": "sample string 2"
      },
      "AssessmentTitle": "sample string 1",
      "AssessmentLevel": 2
    }
  }
]
```
</details>


### `GET restapi/personal/studentplan/assessments/topics/v1?courseId={courseId}`

Returns list of topic assessments for the course and student.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Response:** `?` — List of topics.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Topic id. | integer |  |
| Plans | List of plans. | Collection of StudentPlan.Assessments.StudentPlanPlan |  |
| Name | Topic name. | string |  |
| ColorCode | The topic color schema. | string |  |
| StartDateTime | Topic start date and time. | date |  |
| EndDateTime | Topic end date and time. | date |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Id": 1,
    "Plans": [
      {
        "StartDate": "2026-11-18T16:30:06Z",
        "StopDate": "2026-11-18T16:30:06Z",
        "Activities": [
          {
            "Id": 1,
            "Title": "sample string 2",
            "Url": "sample string 3",
            "IconUrl": "sample string 4",
            "Assessments": [
              {
                "LearningObjectiveId": 1,
                "Url": "sample string 2",
                "Comment": "sample string 3",
                "LearningObjectiveTitle": "sample string 4",
                "AssessmentTitle": "sample string 5"
              }
            ],
            "AssessmentTitle": "sample string 5",
            "Feedback": {
              "RichContent": "sample string 1",
              "PlainContent": "sample string 2",
              "HasRichContent": true
            }
          }
        ]
      }
    ],
    "Name": "sample string 2",
    "ColorCode": "sample string 3",
    "StartDateTime": "2026-11-18T16:30:06Z",
    "EndDateTime": "2026-11-18T16:30:06Z"
  }
]
```
</details>


### `GET restapi/personal/studentplan/assessments/topics/v1?courseId={courseId}&fromId={fromId}`

Returns list of topic assessments for the course and student.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| fromId | The identifier of the topic to start search from. | integer | Required |

**Response:** `?` — List of topics.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Topic id. | integer |  |
| Plans | List of plans. | Collection of StudentPlan.Assessments.StudentPlanPlan |  |
| Name | Topic name. | string |  |
| ColorCode | The topic color schema. | string |  |
| StartDateTime | Topic start date and time. | date |  |
| EndDateTime | Topic end date and time. | date |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Id": 1,
    "Plans": [
      {
        "StartDate": "2026-11-18T16:34:29Z",
        "StopDate": "2026-11-18T16:34:29Z",
        "Activities": [
          {
            "Id": 1,
            "Title": "sample string 2",
            "Url": "sample string 3",
            "IconUrl": "sample string 4",
            "Assessments": [
              {
                "LearningObjectiveId": 1,
                "Url": "sample string 2",
                "Comment": "sample string 3",
                "LearningObjectiveTitle": "sample string 4",
                "AssessmentTitle": "sample string 5"
              }
            ],
            "AssessmentTitle": "sample string 5",
            "Feedback": {
              "RichContent": "sample string 1",
              "PlainContent": "sample string 2",
              "HasRichContent": true
            }
          }
        ]
      }
    ],
    "Name": "sample string 2",
    "ColorCode": "sample string 3",
    "StartDateTime": "2026-11-18T16:34:29Z",
    "EndDateTime": "2026-11-18T16:34:29Z"
  }
]
```
</details>


### `GET restapi/personal/studentplan/comments/v1?courseId={courseId}&UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}`

Retrieves a list of comments that relates to specific person student plan.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Comment identifier. | integer |  |
| SourceId | The identifier of source entity that comment belongs to. | integer |  |
| SourceType | The type of source identifier that comment belongs to. | string |  |
| PictureUrl | Url of the person picture. | string |  |
| UserName | Name of user who created the comment. | string |  |
| CommentText | Text message. | string |  |
| PublishedDateTime | Date and time when comment was published. | date |  |
| LastModifiedDateTime | Date and time when comment was last modified. | date |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Id": 1,
    "SourceId": 2,
    "SourceType": "sample string 3",
    "PictureUrl": "sample string 4",
    "UserName": "sample string 5",
    "CommentText": "sample string 6",
    "PublishedDateTime": "2026-11-18T16:34:29Z",
    "LastModifiedDateTime": "2026-11-18T16:34:29Z"
  }
]
```
</details>


### `GET restapi/personal/studentplan/courses/v1?lastCommentReadDateTimesUtc={lastCommentReadDateTimesUtc}`

Returns a list of courses for the Student Plan.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| lastCommentReadDateTimesUtc | Comma separated date and time when comments has been last read for each course in the following format: "{courseId}.{date}". | string |  |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The course identifier. | integer |  |
| Title | Title of the course. | string |  |
| Color | The course selected color. | string |  |
| NumberOfAssignedAchievementGoals | The number of assigned achievement goals. | integer |  |
| NumberOfUnreadComments | The number of unread comments. | integer |  |
| IsEmpty | Flag indicating that student plan is empty (i.e. no comments, assessments or goals) for the course. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Id": 1,
    "Title": "sample string 2",
    "Color": "sample string 3",
    "NumberOfAssignedAchievementGoals": 4,
    "NumberOfUnreadComments": 5,
    "IsEmpty": true
  }
]
```
</details>


### `GET restapi/personal/studentplan/overview/v1?courseId={courseId}&lastCommentReadDateTimeUtc={lastCommentReadDateTimeUtc}`

Returns an overview entity for the Student Plan.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| lastCommentReadDateTimeUtc | Date and time when comments has been last read. | date |  |

**Response:** `StudentPlan.StudentPlanOverview`

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentsError | Error state for the assessments. | StudentPlan.AssessmentsErrorType |  |
| CourseColor | The course color. | string |  |
| CourseTitle | The course title. | string |  |
| UnreadCommentsCount | Number of unread comments for the current person. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "AssessmentsError": "None",
  "CourseColor": "sample string 1",
  "CourseTitle": "sample string 2",
  "UnreadCommentsCount": 3
}
```
</details>

