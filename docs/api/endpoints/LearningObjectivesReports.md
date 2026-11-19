# LearningObjectivesReports

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to get users' learning objectives aggregated progress reports and / or corresponding information. Secured with Oauth2, and requires OAuth2 API scope Reports.

## Endpoints

- `GET restapi/personal/progressreport/{personId}/v1` — Gets the person learning objectives aggregated progress report.
- `GET restapi/personal/progressreport/courses/v1` — Gets the favourite courses where current user is a teacher and where the learning objective used.
- `GET restapi/personal/progressreport/criteria/{personId}/v1` — Gets the person criteria assessment report.
- `GET restapi/personal/progressreport/criteria/v1` — Gets the criteria list for learning objective at specified course.
- `GET restapi/personal/progressreport/reportsubjects/{personId}/v1` — Gets list of subjects the person learning objectives aggregated progress report is based on.
- `GET restapi/personal/progressreport/reportsubjects/v1` — Gets list of subjects the person learning objectives aggregated progress report is based on.
- `GET restapi/personal/progressreport/statuses/{personId}/v1` — Gets list of available learning objective statuses.
- `GET restapi/personal/progressreport/statuses/v1` — Gets list of available learning objective statuses.
- `GET restapi/personal/progressreport/v1` — Gets the person learning objectives aggregated progress report.

---

### `GET restapi/personal/progressreport/{personId}/v1?subjectId={subjectId}&fromDateTime={fromDateTime}&toDateTime={toDateTime}&statuses={statuses}`

Gets the person learning objectives aggregated progress report.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | [Required] The person identifier. Required to get a report. | integer | Required |
| subjectId | [Required] The subject id. Required to get a report. | integer | Required |
| fromDateTime | [Optional] Report period start date. Optional, if not supplied a date of the earliest learning objective assessment date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| toDateTime | [Optional] Report period stop date. Optional, if not supplied the current date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| statuses | [Optional] The learning objective statuses types to filter by. | string |  |

**Response:** `LearningObjectiveProgressReport.AggregatedReport` — Person learning objectives aggregated progress report.

| Name | Description | Type | Additional information |
|---|---|---|---|
| SubjectId | The subject id a report is constructed for. | integer |  |
| SubjectSummary | A list of assessment status entities representing summary information about the subject. (e.g. how many Meets, Exceeds etc. assessment status items are there). | Collection of LearningObjectiveProgressReport.AggregatedReportAssessmentStatus |  |
| SubjectMasteryPercentage | The percentage of mastered learning objectives in the subject (across all clusters). (it is calculated as percentage from all assessed learning objectives in the cluster). | decimal number |  |
| Clusters | The list of learning objective clusters in the current subject. | Collection of LearningObjectiveProgressReport.ReportCluster |  |
| UseMastery | Indicates whether mastery is used. | boolean |  |
| FullReportUrl | Full report URL. Directs to a person profile page with learning objectives tab selected. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "SubjectId": 1,
  "SubjectSummary": [
    {
      "Count": 1,
      "Label": "sample string 2",
      "StatusType": 3
    }
  ],
  "SubjectMasteryPercentage": 2.1,
  "Clusters": [
    {
      "ClusterId": 1,
      "Title": "sample string 2",
      "Description": "sample string 3",
      "ClusterSummary": [
        {
          "Count": 1,
          "Label": "sample string 2",
          "StatusType": 3
        }
      ],
      "ClusterMasteryPercentage": 4.1,
      "Groups": [
        {
          "GroupId": 1,
          "Parents": [
            {
              "Id": 1,
              "Title": "sample string 2",
              "Description": "sample string 3"
            }
          ],
          "ReportGroupItems": [
            {
              "LearningObjectiveId": 1,
              "Title": "sample string 2",
              "Description": "sample string 3",
              "Status": {
                "Label": "sample string 1",
                "StatusType": 2
              },
              "Mastered": true,
              "MasteredDate": "2026-11-18T16:34:02Z"
            }
          ]
        }
      ]
    }
  ],
  "UseMastery": true,
  "FullReportUrl": "sample string 4"
}
```
</details>


### `GET restapi/personal/progressreport/courses/v1?learningObjectiveId={learningObjectiveId}&onlyWithCriteria={onlyWithCriteria}`

Gets the favourite courses where current user is a teacher and where the learning objective used.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| learningObjectiveId | The learning objective repository identifier. | integer | Required |
| onlyWithCriteria | If set to true will be return courses where this learning objective has criteria. Otherwise will be returned all courses where the learning objective used. | boolean | Default value is True |

**Response:** `?`

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
[
  {
    "Code": "sample string 1",
    "CourseId": 2,
    "CreatedDateTimeUtc": "2026-11-18T16:35:34Z",
    "Title": "sample string 3",
    "OrganisationId": 4,
    "CourseColorClass": "sample string 5",
    "SyncKey": "sample string 6"
  }
]
```
</details>


### `GET restapi/personal/progressreport/criteria/{personId}/v1?courseId={courseId}&criteriaIds={criteriaIds}&fromDateTime={fromDateTime}&toDateTime={toDateTime}`

Gets the person criteria assessment report.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | The identifier of a person to get assessments for. | integer | Required |
| courseId | The identifier of a course to get assessments from. | integer | Required |
| criteriaIds | The criteria ids separated by comma. Only criteria from the specified course will be returned. | string | Required |
| fromDateTime | [Optional] Report period start date. Optional, if not supplied a date of the earliest criteria assessment date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| toDateTime | [Optional] Report period stop date. Optional, if not supplied the current date is used. Date has to contain also time in, for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |

**Response:** `CriteriaAssessmentReport.CriteriaAssessmentReport` — Person criteria assessment report.

| Name | Description | Type | Additional information |
|---|---|---|---|
| ReportItems | The assessment report items. | Collection of CriteriaAssessmentReport.CriteriaAssessmentReportItem |  |

<details><summary>Response JSON sample</summary>

```json
{
  "ReportItems": [
    {
      "Criterion": {
        "Id": 1,
        "Title": "sample string 2",
        "Description": "sample string 3"
      },
      "Assessments": [
        {
          "AchievementLevelTitle": "sample string 1",
          "AssessmentDate": "2026-11-18T16:33:14Z",
          "Comment": "sample string 3",
          "PercentScore": 4.1,
          "AssessedBy": {
            "PersonId": 1,
            "FirstName": "sample string 2",
            "LastName": "sample string 3",
            "FullName": "sample string 4",
            "ProfileUrl": "sample string 5",
            "AdditionalInfo": "sample string 6",
            "ProfileImageUrl": "sample string 7",
            "ProfileImageUrlSmall": "sample string 8"
          },
          "Element": {
            "ElementId": 1,
            "ElementType": 2,
            "Title": "sample string 3",
            "LearningToolId": 4,
            "ElementOrder": 5,
            "ActiveFromDT": "2026-11-18T16:33:14Z",
            "ActiveToDT": "2026-11-18T16:33:14Z",
            "DeadlineDT": "2026-11-18T16:33:14Z"
          }
        }
      ]
    }
  ]
}
```
</details>


### `GET restapi/personal/progressreport/criteria/v1?courseId={courseId}&learningObjectiveId={learningObjectiveId}`

Gets the criteria list for learning objective at specified course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| learningObjectiveId | The learning objective repository identifier. | integer | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The criteria id. | integer |  |
| Title | The criteria title. | string |  |
| Description | The criteria description. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Id": 1,
    "Title": "sample string 2",
    "Description": "sample string 3"
  }
]
```
</details>


### `GET restapi/personal/progressreport/reportsubjects/{personId}/v1?fromDateTime={fromDateTime}&toDateTime={toDateTime}&subjectIds={subjectIds}`

Gets list of subjects the person learning objectives aggregated progress report is based on.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | [Required] The person identifier. Required to get a report. | integer | Required |
| fromDateTime | [Optional] Report period start date. Optional, if not supplied a date of the earliest learning objective assessment date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| toDateTime | [Optional] Report period stop date. Optional, if not supplied the current date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| subjectIds | [Optional] The subject ids to filter by. | string |  |

**Response:** `EntityList<LearningObjectiveProgressReport.ReportSubject>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LearningObjectiveProgressReport.ReportSubject |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Id": 1,
      "Title": "sample string 2"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/progressreport/reportsubjects/v1?personSyncKey={personSyncKey}&fromDateTime={fromDateTime}&toDateTime={toDateTime}&subjectIds={subjectIds}`

Gets list of subjects the person learning objectives aggregated progress report is based on.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personSyncKey | [Optional] The person sync key. Optional, if not supplied the report is constructed based on a caller of the this api method (gets identity from a authorization information). | string |  |
| fromDateTime | [Optional] Report period start date. Optional, if not supplied a date of the earliest learning objective assessment date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| toDateTime | [Optional] Report period stop date. Optional, if not supplied the current date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| subjectIds | [Optional] The subject ids to filter by. | string |  |

**Response:** `EntityList<LearningObjectiveProgressReport.ReportSubject>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LearningObjectiveProgressReport.ReportSubject |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Id": 1,
      "Title": "sample string 2"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/progressreport/statuses/{personId}/v1`

Gets list of available learning objective statuses.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | [Required] The person identifier. Required to get a report. | integer | Required |

**Response:** `EntityList<LearningObjectiveProgressReport.ReportAssessmentStatus>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LearningObjectiveProgressReport.ReportAssessmentStatus |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Label": "sample string 1",
      "StatusType": 2
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/progressreport/statuses/v1?personSyncKey={personSyncKey}`

Gets list of available learning objective statuses.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personSyncKey | [Optional] The person sync key. Optional, if not supplied the report is constructed based on a caller of the this api method (gets identity from a authorization information). | string |  |

**Response:** `EntityList<LearningObjectiveProgressReport.ReportAssessmentStatus>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LearningObjectiveProgressReport.ReportAssessmentStatus |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Label": "sample string 1",
      "StatusType": 2
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/progressreport/v1?subjectId={subjectId}&personSyncKey={personSyncKey}&fromDateTime={fromDateTime}&toDateTime={toDateTime}&statuses={statuses}`

Gets the person learning objectives aggregated progress report.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| subjectId | [Required] The subject id. Required to get a report. | integer | Required |
| personSyncKey | [Optional] The person sync key. Optional, if not supplied the report is constructed based on a caller of the this api method (gets identity from a authorization information). | string |  |
| fromDateTime | [Optional] Report period start date. Optional, if not supplied a date of the earliest learning objective assessment date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| toDateTime | [Optional] Report period stop date. Optional, if not supplied the current date is used. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |
| statuses | [Optional] The learning objective statuses types to filter by. | string |  |

**Response:** `LearningObjectiveProgressReport.AggregatedReport` — Person learning objectives aggregated progress report.

| Name | Description | Type | Additional information |
|---|---|---|---|
| SubjectId | The subject id a report is constructed for. | integer |  |
| SubjectSummary | A list of assessment status entities representing summary information about the subject. (e.g. how many Meets, Exceeds etc. assessment status items are there). | Collection of LearningObjectiveProgressReport.AggregatedReportAssessmentStatus |  |
| SubjectMasteryPercentage | The percentage of mastered learning objectives in the subject (across all clusters). (it is calculated as percentage from all assessed learning objectives in the cluster). | decimal number |  |
| Clusters | The list of learning objective clusters in the current subject. | Collection of LearningObjectiveProgressReport.ReportCluster |  |
| UseMastery | Indicates whether mastery is used. | boolean |  |
| FullReportUrl | Full report URL. Directs to a person profile page with learning objectives tab selected. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "SubjectId": 1,
  "SubjectSummary": [
    {
      "Count": 1,
      "Label": "sample string 2",
      "StatusType": 3
    }
  ],
  "SubjectMasteryPercentage": 2.1,
  "Clusters": [
    {
      "ClusterId": 1,
      "Title": "sample string 2",
      "Description": "sample string 3",
      "ClusterSummary": [
        {
          "Count": 1,
          "Label": "sample string 2",
          "StatusType": 3
        }
      ],
      "ClusterMasteryPercentage": 4.1,
      "Groups": [
        {
          "GroupId": 1,
          "Parents": [
            {
              "Id": 1,
              "Title": "sample string 2",
              "Description": "sample string 3"
            }
          ],
          "ReportGroupItems": [
            {
              "LearningObjectiveId": 1,
              "Title": "sample string 2",
              "Description": "sample string 3",
              "Status": {
                "Label": "sample string 1",
                "StatusType": 2
              },
              "Mastered": true,
              "MasteredDate": "2026-11-18T16:34:02Z"
            }
          ]
        }
      ]
    }
  ],
  "UseMastery": true,
  "FullReportUrl": "sample string 4"
}
```
</details>

