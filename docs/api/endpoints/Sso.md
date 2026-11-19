# Sso

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for SSO related functionality such as SSO Urls to itslearning.

## Endpoints

- `GET restapi/personal/sso/url/v1` — Creates a short-lived SSO Url to itslearning. Users will be not be prompted to log in when using this Url.

---

### `GET restapi/personal/sso/url/v1?url={url}`

Creates a short-lived SSO Url to itslearning. Users will be not be prompted to log in when using this Url.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| url | Encoded itslearning url. | string | Required |

**Response:** `Personal.ItslearningUrl`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Url | SSO Url to itslearning. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Url": "sample string 1"
}
```
</details>

