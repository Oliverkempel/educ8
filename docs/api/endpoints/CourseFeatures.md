# CourseFeatures

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to Courses features API.

## Endpoints

- `GET restapi/personal/courses/{courseId}/features/v1` — Gets features for the given course.
- `GET restapi/personal/courses/{courseId}/features/v2` — Gets features for the given course.

---

### `GET restapi/personal/courses/{courseId}/features/v1`

Gets features for the given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Response:** `Personal.Feature`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The feature name. | string |  |
| Url | Url to the itslearning web page for the feature if it exists. | string |  |
| Features | The list of sub-features. | Collection of Personal.Feature |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "Url": "sample string 2",
  "Features": [
    {
      "Name": "sample string 1",
      "Url": "sample string 2",
      "Features": null
    }
  ]
}
```
</details>


### `GET restapi/personal/courses/{courseId}/features/v2`

Gets features for the given course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |

**Response:** `Personal.Feature`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The feature name. | string |  |
| Url | Url to the itslearning web page for the feature if it exists. | string |  |
| Features | The list of sub-features. | Collection of Personal.Feature |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "Url": "sample string 2",
  "Features": [
    {
      "Name": "sample string 1",
      "Url": "sample string 2",
      "Features": null
    }
  ]
}
```
</details>

