# ChildrenOrganisation

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for retrieving settings of the organization where a child is a member. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/children/{childId}/permissions/v1` — Gets permissions of the parent in the child's organisation.

---

### `GET restapi/personal/children/{childId}/permissions/v1`

Gets permissions of the parent in the child's organisation.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| childId | Identifier of the child. | integer | Required |

**Response:** `ChildOrganisationPermissions`

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanUseMessaging | Defines that a parent can use messages: receive and view them. | boolean |  |
| CanInitiateMessages | Defines that a parent can create and send a new message. | boolean |  |
| CanViewCourseContent | Defines that a parent can view course content where their children participate. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CanUseMessaging": true,
  "CanInitiateMessages": true,
  "CanViewCourseContent": true
}
```
</details>

