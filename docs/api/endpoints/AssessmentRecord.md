# AssessmentRecord

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** End point for Assessmentrecord API

## Endpoints

- `GET restapi/personal/course/{courseId}/assessmentrecord/assessmentrecordelements/v1` — Gets the elements for the gradebook
- `GET restapi/personal/course/{courseId}/assessmentrecord/assessmentrecordpersonitems/v1` — Gets the persons with details and assessment values
- `DELETE restapi/personal/course/{courseId}/assessmentrecord/personassessmentgrade/v1` — Delete a person assessment for an assessment record item
- `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentgrade/v1` — Save a person assessment for an assessment record item
- `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentoverwritten/v1` — Clear a person assessment overwritten grade/score.
- `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentoverwrittengrade/v1` — Save a person assessment overwritten grade.
- `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentoverwrittenscore/v1` — Save a person assessment overwritten score.
- `DELETE restapi/personal/course/{courseId}/assessmentrecord/personassessmentscore/v1` — Delete a person assessment for an assessment record item
- `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentscore/v1` — Save a person assessment for an assessment record item
- `GET restapi/personal/course/{courseId}/assessmentrecord/settings/v1` — Setup for the assessment record page, language, settings, paths, and permissions

---

### `GET restapi/personal/course/{courseId}/assessmentrecord/assessmentrecordelements/v1?termId={termId}`

Gets the elements for the gradebook

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course to get assessment record elements from. | integer | Required |
| termId | [optional] Used for filtering assessment record elements by term. | integer |  |

**Response:** `AssessmentRecord.AssessmentRecordElements`

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentRecordItems | Returns the assessment record items | Collection of AssessmentRecord.AssessmentRecordItem |  |
| AssessmentRecordItemsCount | Returns the assessment record items' count | integer |  |
| FinalAssessmentRecordItems | Returns the final assessment record items | Collection of AssessmentRecord.AssessmentRecordItem |  |
| AssessmentScales | Returns the assessment scales | Collection of AssessmentRecord.AssessmentScale |  |
| IsScoreInUse | Returns true if any asessment record items uses score | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "AssessmentRecordItems": [
    {
      "GradeBookItemId": 1,
      "Title": "sample string 2",
      "Weight": 3.1,
      "WhenToShowResult": 0,
      "AssessmentId": 4,
      "IsFinal": true,
      "IsPassedDeadline": true,
      "TermId": 7,
      "CategoryId": 8,
      "ElementType": "sample string 9",
      "ElementId": 1,
      "MaxScore": 10.1,
      "UseScore": true,
      "IsActive": true,
      "IsAssessmentVisible": true,
      "IsCustomAssessmentVisible": true,
      "IsReadOnly": true,
      "LearningToolId": 16,
      "Deadline": "2026-11-18T16:34:59Z",
      "FormattedDeadline": "sample string 17",
      "IsLearningPathSubElement": true
    }
  ],
  "AssessmentRecordItemsCount": 1,
  "FinalAssessmentRecordItems": [
    {
      "GradeBookItemId": 1,
      "Title": "sample string 2",
      "Weight": 3.1,
      "WhenToShowResult": 0,
      "AssessmentId": 4,
      "IsFinal": true,
      "IsPassedDeadline": true,
      "TermId": 7,
      "CategoryId": 8,
      "ElementType": "sample string 9",
      "ElementId": 1,
      "MaxScore": 10.1,
      "UseScore": true,
      "IsActive": true,
      "IsAssessmentVisible": true,
      "IsCustomAssessmentVisible": true,
      "IsReadOnly": true,
      "LearningToolId": 16,
      "Deadline": "2026-11-18T16:34:59Z",
      "FormattedDeadline": "sample string 17",
      "IsLearningPathSubElement": true
    }
  ],
  "AssessmentScales": [
    {
      "AssessmentScaleId": 1,
      "Title": "sample string 2",
      "AssessmentTotals": 3,
      "Values": [
        {
          "AssessmentItemId": 1,
... (truncated)
```
</details>


### `GET restapi/personal/course/{courseId}/assessmentrecord/assessmentrecordpersonitems/v1?termId={termId}&paging.PageIndex={paging.PageIndex}&paging.PageSize={paging.PageSize}&searchText={searchText}&groupIds[0]={groupIds[0]}&groupIds[1]={groupIds[1]}&includePersonsNotInAnyGroups={includePersonsNotInAnyGroups}`

Gets the persons with details and assessment values

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course to get person items from. | integer | Required |
| termId | [optional] Used for filtering assessment record elements by term. | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| searchText | [optional] Used for searching for student names | string |  |
| groupIds | [optional] Used for filtering assessment record elements by group. | Collection of integer |  |
| includePersonsNotInAnyGroups | [Optional] bool to indicate whether to include persons not in any group | boolean | Default value is False |

**Response:** `EntityList<AssessmentRecord.AssessmentRecordPerson>` — List of persons filtered by groups or search text

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of AssessmentRecord.AssessmentRecordPerson |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "PersonAssessments": [
        {
          "GradeBookItemId": 1,
          "IsExempt": true,
          "IsManualAssessmentRequired": true,
          "Score": 1.1,
          "Scale": 1.1,
          "OverwrittenAssessmentItemId": 1,
          "OverwrittenScore": 1.1,
          "AnswerSubmitted": true,
          "AttemptId": 1,
          "AnswerDateTime": "2026-11-18T16:31:28Z",
          "AssessmentVisible": true,
          "ElementSessionStatus": 0,
          "ElementResultId": 1,
          "IsAnonymousSubmission": true,
          "ElementPersonCanTake": true
        }
      ],
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


### `DELETE restapi/personal/course/{courseId}/assessmentrecord/personassessmentgrade/v1?gradebookItemId={gradebookItemId}&personId={personId}`

Delete a person assessment for an assessment record item

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |

**Response:** `?` — 200 OK or exception


### `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentgrade/v1?gradebookItemId={gradebookItemId}&personId={personId}&assessmentId={assessmentId}&assessmentItemId={assessmentItemId}`

Save a person assessment for an assessment record item

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |
| assessmentId | The identifier of which assessment scale is in use | integer | Required |
| assessmentItemId | The assessmentItemId should match assessmentId items which is in use | integer | Required |

**Response:** `?` — 200 OK or exception


### `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentoverwritten/v1?gradebookItemId={gradebookItemId}&personId={personId}`

Clear a person assessment overwritten grade/score.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |

**Response:** `AssessmentRecord.PersonAssessment`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Score | Assessment record item score value | decimal number |  |
| AssessmentItemId | Assessment record item scale value | integer |  |
| IsOverwritten | Whether the assessment has been overwritten | boolean |  |
| IsAssessmentVisible | Whether the assessment is visible to the student | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Score": 1.1,
  "AssessmentItemId": 1,
  "IsOverwritten": true,
  "IsAssessmentVisible": true
}
```
</details>


### `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentoverwrittengrade/v1?gradebookItemId={gradebookItemId}&personId={personId}&oldAssessmentItemId={oldAssessmentItemId}&assessmentId={assessmentId}&overwrittenAssessmentItemId={overwrittenAssessmentItemId}`

Save a person assessment overwritten grade.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |
| oldAssessmentItemId | The assessmentItemId since last load. | integer | Required |
| assessmentId | The identifier of which assessment scale is in use | integer | Required |
| overwrittenAssessmentItemId | The new asssessment user tries to overwrite | integer | Required |

**Response:** `AssessmentRecord.PersonAssessment` — person assessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| Score | Assessment record item score value | decimal number |  |
| AssessmentItemId | Assessment record item scale value | integer |  |
| IsOverwritten | Whether the assessment has been overwritten | boolean |  |
| IsAssessmentVisible | Whether the assessment is visible to the student | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Score": 1.1,
  "AssessmentItemId": 1,
  "IsOverwritten": true,
  "IsAssessmentVisible": true
}
```
</details>


### `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentoverwrittenscore/v1?gradebookItemId={gradebookItemId}&personId={personId}&overwrittenScore={overwrittenScore}&oldScore={oldScore}`

Save a person assessment overwritten score.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |
| overwrittenScore | OverwrittenScore value within 0 to 99999.99 range | string | Required |
| oldScore | The student's score since last load | string | Required |

**Response:** `AssessmentRecord.PersonAssessment` — person assessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| Score | Assessment record item score value | decimal number |  |
| AssessmentItemId | Assessment record item scale value | integer |  |
| IsOverwritten | Whether the assessment has been overwritten | boolean |  |
| IsAssessmentVisible | Whether the assessment is visible to the student | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Score": 1.1,
  "AssessmentItemId": 1,
  "IsOverwritten": true,
  "IsAssessmentVisible": true
}
```
</details>


### `DELETE restapi/personal/course/{courseId}/assessmentrecord/personassessmentscore/v1?gradebookItemId={gradebookItemId}&personId={personId}`

Delete a person assessment for an assessment record item

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |

**Response:** `?` — 200 OK or exception


### `PUT restapi/personal/course/{courseId}/assessmentrecord/personassessmentscore/v1?gradebookItemId={gradebookItemId}&personId={personId}&score={score}`

Save a person assessment for an assessment record item

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course which user is in | integer | Required |
| gradebookItemId | The identifier of an assessment record item to update person assessment | integer | Required |
| personId | The identifier of a person to update assessment | integer | Required |
| score | Score value within 0 to 99999.99 range | string | Required |

**Response:** `?` — 200 OK or exception


### `GET restapi/personal/course/{courseId}/assessmentrecord/settings/v1`

Setup for the assessment record page, language, settings, paths, and permissions

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The identifier of a course to get assessment record setup from. | integer | Required |

**Response:** `Personal.Course.PageSetup.AssessmentRecordSetup`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Language | Language for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordLanguage |  |
| Paths | Paths for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordPaths |  |
| Settings | Settings for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordSettings |  |
| Permissions | Gets or sets the permissions for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordPermissions |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Language": {
    "AddAssessment": "sample string 1",
    "AddCategories": "sample string 2",
    "AddGroups": "sample string 3",
    "AddToGradebook": "sample string 4",
    "AllAssessments": "sample string 5",
    "AllGroups": "sample string 6",
    "AllParticipants": "sample string 7",
    "AnErrorOccurred": "sample string 8",
    "Apply": "sample string 9",
    "Assess": "sample string 10",
    "AssessmentRecordCategory": "sample string 11",
    "AssessmentRecordSettings": "sample string 12",
    "AssessmentWeight": "sample string 13",
    "AssessmentWeighting": "sample string 14",
    "AverageAndCount": "sample string 15",
    "Cancel": "sample string 16",
    "CannotDeleteFinalAssessment": "sample string 17",
    "Categories": "sample string 18",
    "CategoryExistsButNotInUseMessage": "sample string 19",
    "CategoryWeight": "sample string 20",
    "CategoryWeighting": "sample string 21",
    "Close": "sample string 22",
    "ColumnsNotConnectedToTerms": "sample string 23",
    "CompleteTerm": "sample string 24",
    "CompletedTermMessage": "sample string 25",
    "Confirm": "sample string 26",
    "Deadline": "sample string 27",
    "Delete": "sample string 28",
    "Dismiss": "sample string 29",
    "Edit": "sample string 30",
    "ErrorDuringPageLoad": "sample string 31",
    "ErrorMessage": "sample string 32",
    "Ex": "sample string 33",
    "Exempt": "sample string 34",
    "ExistingAssessment": "sample string 35",
    "ExportToCsv": "sample string 36",
    "FilterBy": "sample string 37",
    "FinalAssessment": "sample string 38",
    "Functions": "sample string 39",
    "Grade": "sample string 40",
    "GradebookGroup": "sample string 41",
    "Groups": "sample string 42",
    "Information": "sample string 43",
    "MoreOptions": "sample string 44",
    "Name": "sample string 45",
    "NewGradebookColumn": "sample string 46",
    "NoAssessmentWeighting": "sample string 47",
    "NoCategories": "sample string 48",
    "NoCategoriesCreatedMessage": "sample string 49",
    "NoGrading": "sample string 50",
    "NoGroups": "sample string 51",
    "NoGroupsCreatedMessage": "sample string 52",
    "NoMoreExistingAssessments": "sample string 53",
    "NoParticipantsAvailable": "sample string 54",
    "NoStudentsMatchSearch": "sample string 55",
    "LearningPathWarning": "sample string 56",
    "NSlashA": "sample string 57",
    "Options": "sample string 58",
... (truncated)
```
</details>

