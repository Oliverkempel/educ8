# CoursesGroups

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to Courses groups API.

## Endpoints

- `DELETE restapi/personal/courses/{courseId}/groups/v1` — Remove course groups with specified identifiers.
- `PUT restapi/personal/courses/{courseId}/groupsets/v1` — Create or replace existing course groups sets in specified course.

---

### `DELETE restapi/personal/courses/{courseId}/groups/v1`

Remove course groups with specified identifiers.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Identifier of a course. | integer | Required |

**Body** (CourseGroups.CourseGroupsToRemove)

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupIds | Course groups identifiers of course groups to be removed. | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "GroupIds": [
    1
  ]
}
```
</details>

**Response:** `?`


### `PUT restapi/personal/courses/{courseId}/groupsets/v1`

Create or replace existing course groups sets in specified course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | Identifier of a course. | integer | Required |

**Body** (CourseGroups.CourseGroupSet)

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupSetId | Group set identifier. | integer |  |
| Title | Group set title. | string |  |
| Groups | Array of groups in the current group set. | Collection of CourseGroups.CourseGroup |  |

<details><summary>Request JSON sample</summary>

```json
{
  "GroupSetId": 1,
  "Title": "sample string 2",
  "Groups": [
    {
      "GroupId": 1,
      "Title": "sample string 2",
      "ParticipantIds": [
        1
      ]
    }
  ]
}
```
</details>

**Response:** `CourseGroups.CourseGroupSetSimple` — Object containing CourseGroupSetId and array of CourseGroupIds in the same order that was presented by input object.

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupSetId | Group set identifier. | integer |  |
| GroupIds | Array of groups identifiers in the current group set. | Collection of integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "GroupSetId": 1,
  "GroupIds": [
    1
  ]
}
```
</details>

