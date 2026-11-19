# PersonRelation

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to get the person relations. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/person/{personId}/relatedpersons/v1` — Gets the relation between persons with detailed info about related person and relation type.

---

### `GET restapi/personal/person/{personId}/relatedpersons/v1?relationType={relationType}`

Gets the relation between persons with detailed info about related person and relation type.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | The id of the person we want the relation information for. | integer | Required |
| relationType | Type of the relation. | RelationType | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Person | Related person. | PersonSimple |  |
| RelationType | Relation type between persons. | RelationType |  |
| RelationInfo | Extra information about the relation. | RelationInfo |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Person": {
      "PersonId": 1,
      "FirstName": "sample string 2",
      "LastName": "sample string 3",
      "FullName": "sample string 4",
      "ProfileUrl": "sample string 5",
      "AdditionalInfo": "sample string 6",
      "ProfileImageUrl": "sample string 7",
      "ProfileImageUrlSmall": "sample string 8"
    },
    "RelationType": "Teacher",
    "RelationInfo": {
      "Courses": [
        {
          "CourseId": 1,
          "Title": "sample string 2",
          "FriendlyName": "sample string 3",
          "CourseColor": "sample string 4",
          "CourseFillColor": "sample string 5",
          "CourseCode": "sample string 6"
        }
      ]
    }
  }
]
```
</details>

