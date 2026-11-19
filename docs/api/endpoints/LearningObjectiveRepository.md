# LearningObjectiveRepository

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for learning objectives for the personal api. Secured with Oauth2, and requires OAuth2 API scope LearningObjectiveRepository.

## Endpoints

- `GET restapi/learningobjectiverepository/subjects/{countryCode}/v1` — Retrieves the Subjects that have been defined for the given Country code.

---

### `GET restapi/learningobjectiverepository/subjects/{countryCode}/v1`

Retrieves the Subjects that have been defined for the given Country code.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| countryCode | Country to retrieve subjects for | string | Required |

**Response:** `?` — A collection with Subjects.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Id of the subject label | integer |  |
| Name | Name of the subject label | string |  |
| Aliases | Collection of aliases belonging to this subject label | Collection of string |  |
| CountryCode | Country code belonging to this subject label | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Id": 1,
    "Name": "sample string 2",
    "Aliases": [
      "sample string 1"
    ],
    "CountryCode": "sample string 3"
  }
]
```
</details>

