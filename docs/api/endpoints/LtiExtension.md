# LtiExtension

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** API controller for Lti extensions.

## Endpoints

- `GET restapi/personal/lti/{extensionId}/details/v1` — Gets the LTI details for user and for the given course (if defined).
- `GET restapi/personal/lti/{extensionId}/details/v2` — Gets the LTI customer setup for user and for the given course (if defined).

---

### `GET restapi/personal/lti/{extensionId}/details/v1?courseId={courseId}`

Gets the LTI details for user and for the given course (if defined).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| extensionId |  | integer | Required |
| courseId |  | integer | Default value is 0 |

**Response:** `Personal.LtiExtensionDetails`

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeploymentId | Deployment Id. | string |  |
| LoginUri | Login URL. | string |  |
| LaunchUrls | List of launch URLs. | Collection of string |  |
| TargetLinkUri | Target link URL. | string |  |
| PublicKey | Public key. | string |  |
| PublicJwk | JWK key. | string |  |
| PublicJwksUri | JWKS URL. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "DeploymentId": "sample string 1",
  "LoginUri": "sample string 2",
  "LaunchUrls": [
    "sample string 1"
  ],
  "TargetLinkUri": "sample string 3",
  "PublicKey": "sample string 4",
  "PublicJwk": "sample string 5",
  "PublicJwksUri": "sample string 6"
}
```
</details>


### `GET restapi/personal/lti/{extensionId}/details/v2?courseId={courseId}`

Gets the LTI customer setup for user and for the given course (if defined).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| extensionId |  | integer | Required |
| courseId |  | integer | Default value is 0 |

**Response:** `Personal.LtiExtensionCustomerSetup`

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeploymentId | Deployment Id. | string |  |
| ClientId | Identifier of itslearning as LTI connected LMS. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "DeploymentId": "sample string 1",
  "ClientId": "sample string 2"
}
```
</details>

