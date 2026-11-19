# ImmersiveReader

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** API controller for Immersive Reader.

## Endpoints

- `GET restapi/personal/immersivereader/v1` — Gets the required info for use with Immersive Reader.

---

### `GET restapi/personal/immersivereader/v1?resourceId={resourceId}&resourceType={resourceType}&contentBlockId={contentBlockId}`

Gets the required info for use with Immersive Reader.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| resourceId | The unique identifier of the resource | integer | Required |
| resourceType | The type of resource being accessed | ResourceType | Required |
| contentBlockId | Used for page text content block identification. Defaults to 0 when not applicable | integer | Default value is 0 |

**Response:** `ImmersiveReaderInfo`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | The title to send to the immersive reader | string |  |
| Text | The text to send to the immersive reader | string |  |
| AccessToken | The access token to use with the immersive reader | string |  |
| SubDomain | The subdomain to use with the immersive reader | string |  |
| UiLang | The current language used in the UI | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Title": "sample string 1",
  "Text": "sample string 2",
  "AccessToken": "sample string 3",
  "SubDomain": "sample string 4",
  "UiLang": "sample string 5"
}
```
</details>

