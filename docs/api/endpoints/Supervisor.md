# Supervisor

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for retrieving data for supervisors - mentors and headteachers. Secured with Oauth2, and requires OAuth2 API scope Supervisor.

## Endpoints

- `GET restapi/personal/supervisor/supervised/v1` — Gets a list of all persons current user is a supervisor for - mentorees.

---

### `GET restapi/personal/supervisor/supervised/v1?supervisorType={supervisorType}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets a list of all persons current user is a supervisor for - mentorees.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| supervisorType | [Optional] Required type of a supervisor. If not set or set to NotDefined, method returns only headteacher learners if any, otherwise mentor learners only. | SupervisorType | Default value is NotDefined |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonSimple>` — This method returns a collection of entity . Each entity is a person for which the current user is a supervisor.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
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

