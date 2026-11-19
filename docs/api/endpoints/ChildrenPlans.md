# ChildrenPlans

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for retrieving children-related data. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/children/{childId}/plans/{planId}/v1` — Gets the child's single plan details.
- `GET restapi/personal/children/{childId}/plans/v1` — Gets a list of plans assigned to the child.

---

### `GET restapi/personal/children/{childId}/plans/{planId}/v1`

Gets the child's single plan details.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | [Required] The child identifier. | integer | Required |
| planId | [Required] The plan identifier. | integer | Required |

**Response:** `Planner.Plan` — This method returns plan details .

| Name | Description | Type | Additional information |
|---|---|---|---|
| ThumbnailColumn | The 'Thumbnail' plan column data. | Planner.PlanThumbnailColumn |  |
| NameColumn | The 'Plan' plan column data. | Planner.PlanColumn |  |
| DescriptionColumn | The 'Description' plan column data. | Planner.PlanColumn |  |
| DateColumn | The 'Date' plan column data. | Planner.PlanDateColumn |  |
| ClassHoursColumn | The 'Class hours' plan column data. | Planner.PlanClassHoursColumn |  |
| LearningObjectivesColumn | The 'Learning objectives' plan column data. | Planner.PlanLearningObjectivesColumn |  |
| ResourcesColumn | The 'Resources' plan column data. | Planner.PlanElementsColumn |  |
| ActivitiesColumn | The 'Activities' plan column data. | Planner.PlanElementsColumn |  |
| ElementsColumn | The 'Resources and activities' plan column data. | Planner.PlanElementsColumn |  |
| Columns | The list of custom columns for plan. | Collection of Planner.PlanColumn |  |
| PlanId | The unique id for plan, in scope of a site (itslearning customer). | integer |  |
| TopicId | The id of topic. | integer |  |
| Order | The plan order. | integer |  |
| LocationId | The plan location identifier. | integer |  |
| LocationType | The plan location type. | LocationType |  |
| LocationTitle | The plan location title. | string |  |
| LocationColor | The location color (e.g. #ffffff). | string |  |
| TopicColor | The topic color (e.g. #ffffff). | string |  |
| TopicName | The topic name. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "TopicName": "sample string 1",
  "ThumbnailColumn": {
    "FileUrl": "sample string 1",
    "Name": "sample string 2",
    "Order": 3,
    "LocalizedName": {
      "LocalizedKey": "sample string 1",
      "LocalizedArgs": [
        "sample string 1"
      ]
    }
  },
  "NameColumn": {
    "Value": "sample string 1",
    "Name": "sample string 2",
    "Order": 3,
    "LocalizedName": {
      "LocalizedKey": "sample string 1",
      "LocalizedArgs": [
        "sample string 1"
      ]
    }
  },
  "DescriptionColumn": {
    "Value": "sample string 1",
    "Name": "sample string 2",
    "Order": 3,
    "LocalizedName": {
      "LocalizedKey": "sample string 1",
      "LocalizedArgs": [
        "sample string 1"
      ]
    }
  },
  "DateColumn": {
    "StartDate": "2026-11-18T16:34:59Z",
    "StopDate": "2026-11-18T16:34:59Z",
    "Name": "sample string 1",
    "Order": 2,
    "LocalizedName": {
      "LocalizedKey": "sample string 1",
      "LocalizedArgs": [
        "sample string 1"
      ]
    }
  },
  "ClassHoursColumn": {
    "ClassHours": 1,
    "Name": "sample string 2",
    "Order": 3,
    "LocalizedName": {
      "LocalizedKey": "sample string 1",
      "LocalizedArgs": [
        "sample string 1"
      ]
    }
  },
  "LearningObjectivesColumn": {
    "LearningObjectives": [
... (truncated)
```
</details>


### `GET restapi/personal/children/{childId}/plans/v1?startDate={startDate}&stopDate={stopDate}&onlyWithoutDates={onlyWithoutDates}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of plans assigned to the child.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | [Required] The child person identifier. | integer | Required |
| startDate | [Optional] The start date for plans. Time zone is UTC. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| stopDate | [Optional] The plan stop date for plans. Time zone is UTC. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| onlyWithoutDates | [Optional] If true, then plans with no date are returned. | boolean | Default value is False |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<Planner.PlanPreviewV2>` — This method returns a collection of entities .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Planner.PlanPreviewV2 |  |
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
      "Start": "2026-11-18T16:31:28Z",
      "Stop": "2026-11-18T16:31:28Z",
      "TopicName": "sample string 3",
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

