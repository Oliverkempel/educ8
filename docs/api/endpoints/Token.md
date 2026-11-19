# Token

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** API that handles OAuth2 access control:

## Endpoints

- `POST restapi/oauth2/token` — Get a new access token from either refresh_token, authorization_code or JWT bearer.
- `DELETE restapi/oauth2/token/v1` — Revoke refresh token and disable push notifications if it's last one.

---

### `POST restapi/oauth2/token`

Get a new access token from either refresh_token, authorization_code or JWT bearer.

**Response:** `Personal.TokenResponse` — The new access token and refresh token

| Name | Description | Type | Additional information |
|---|---|---|---|
| access_token | The accesstoken that's used for calling API | string |  |
| token_type | The token type. Refer to OAuth2 specs for more detailed information | string |  |
| expires_in | Seconds until expiry of the access token | integer |  |
| refresh_token | The refresh_token, used for getting a new access token and refresh token pair after expiry. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "access_token": "WTwJWDdWq8aSY3DcBuuEI04rBzFC_L8ddnlzkm138KzxoiEKHU2bjVW-tg7tmaNfXxK1ToL8mUzLEzeL3WVDITiTL4AE5asyWwGE9-39HK6W-02VHMuh7k0T57H_q_Ck",
  "token_type": "Bearer",
  "expires_in": 3600,
  "refresh_token": "1-a93f13a473ec45d7a24494cf9c9b27663Rw8M"
}
```
</details>


### `DELETE restapi/oauth2/token/v1`

Revoke refresh token and disable push notifications if it's last one.

**Response:** `System.Net.Http.HttpResponseMessage`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |

