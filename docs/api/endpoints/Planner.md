# Planner

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to get planner information. Secured with Oauth2, and requires OAuth2 API scope Planner.

## Endpoints

- `GET restapi/personal/planner/plansinfo/v1` — Gets list of plans with learning objective and elements attached.
- `POST restapi/personal/planner/targetCourse/{targetCourseId}/targetTopic/{targetTopicId}/targetPlan/{targetPlanId}/v1` — Adds shared plan elements to the course plan.
- `GET restapi/personal/planner/v1` — Get previews of plans for the specified course.

---

### `GET restapi/personal/planner/plansinfo/v1?plansIds[0]={plansIds[0]}&plansIds[1]={plansIds[1]}`

Gets list of plans with learning objective and elements attached.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| plansIds | Requested plan IDs. | Collection of integer | Required |

**Response:** `?` — list of plans with learning objective and elements attached.

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | Id of the course the plan is created for. | integer |  |
| CourseName | Name of the course the plan is created for. | string |  |
| CourseSyncKey | Sync key of the course the plan is created for. | string |  |
| PlanId | Identifier of the plan. | integer |  |
| PlanTopicName | Name of the topic the plan belongs to. | string |  |
| PlanTopicColor | Color of the topic the plan belongs to. | string |  |
| PlanTitle | Title of the plan. | string |  |
| PlanDescription | Description of the plan. | string |  |
| PlanStartDateTime | The date and time the plan starts. | date |  |
| PlanEndDateTime | The date and time the plan ends. | date |  |
| CoursePlannerUrl | The URL to pointing to Planner of the course the plans is created for. | string |  |
| LearningObjectives | The list of learning objectives specified for the plan. | Collection of Personal.PlanLearningObjectiveBasic |  |
| Elements | The list of course elements attached to the plan. | Collection of Personal.PlanElementBasic |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "CourseId": 1,
    "CourseName": "sample string 2",
    "CourseSyncKey": "sample string 3",
    "PlanId": 4,
    "PlanTopicName": "sample string 5",
    "PlanTopicColor": "sample string 6",
    "PlanTitle": "sample string 7",
    "PlanDescription": "sample string 8",
    "PlanStartDateTime": "2026-11-18T16:34:59Z",
    "PlanEndDateTime": "2026-11-18T16:34:59Z",
    "CoursePlannerUrl": "sample string 11",
    "LearningObjectives": [
      {
        "Id": 1,
        "Title": "sample string 2",
        "Description": "sample string 3",
        "IsFromRepository": true,
        "Type": 1
      }
    ],
    "Elements": [
      {
        "Id": 1,
        "Title": "sample string 2",
        "Type": "Unknown",
        "IsMarkedAsHomework": true,
        "IconUri": "sample string 4"
      }
    ]
  }
]
```
</details>


### `POST restapi/personal/planner/targetCourse/{targetCourseId}/targetTopic/{targetTopicId}/targetPlan/{targetPlanId}/v1`

Adds shared plan elements to the course plan.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| targetCourseId | ID of the course where the elements are added. | integer | Required |
| targetTopicId | ID of the course topic where the elements are added. Can be 0. | integer | Required |
| targetPlanId | ID of the course plan where the elements are added. | integer | Required |

**Body** (Internal.Models.Planner.ShareablePlanner.AddSharedResourcesToCoursePlanRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Elements | Elements to be added. | Collection of Itslearning.Platform.RestApi.Sdk.ItslEntities.Planner.PlanElement |  |
| AddOnlyExistingInCourseLearningObjectives | Indicates if only existing in the target course learning objectives can be used. | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Elements": [
    {
      "LearningObjectInstanceId": 1,
      "ExtensionId": 2,
      "AuthorPersonId": 3,
      "AuthorName": "sample string 4",
      "SharedWith": "sample string 5",
      "SharingScope": 0,
      "VendorDefaultName": "sample string 6",
      "VendorAuthorName": "sample string 7",
      "ElementLearningObjectives": [
        1
      ],
      "IconTypeId": 1,
      "IconsVersion": 8,
      "ElementId": 9,
      "Title": "sample string 10",
      "LearningObjectId": 11,
      "LearningToolType": 0,
      "Order": 12
    }
  ],
  "AddOnlyExistingInCourseLearningObjectives": true
}
```
</details>

**Response:** `?`


### `GET restapi/personal/planner/v1?courseId={courseId}&startDate={startDate}&stopDate={stopDate}`

Get previews of plans for the specified course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | [Required] The course identifier. Required to get a plans. | integer | Required |
| startDate | [Optional] The start date for plans. Time zone is UTC. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| stopDate | [Optional] The plan stop date for plans. Time zone is UTC. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |

**Response:** `EntityList<Planner.PlanPreview>` — This method returns a list of plans for the specified course.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Planner.PlanPreview |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PlanName": "sample string 1",
      "PlanDescription": "sample string 2",
      "Start": "2026-11-18T16:34:02Z",
      "Stop": "2026-11-18T16:34:02Z",
      "ClassHours": 3,
      "PlanId": 1,
      "TopicId": 2,
      "Order": 3,
      "LocationId": 4,
      "LocationType": "Course",
      "LocationTitle": "sample string 5",
      "LocationColor": "sample string 6",
      "TopicColor": "sample string 7"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

