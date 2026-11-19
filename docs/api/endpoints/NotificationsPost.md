# NotificationsPost

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for posting a new notifications for personal api.

## Endpoints

- `POST restapi/personal/notifications/deletetriggers/location/v1` — Delete all personal notifications which related to a specific location.
- `POST restapi/personal/notifications/deletetriggers/object/v1` — Delete all personal notifications which related to a specific object.
- `POST restapi/personal/notifications/posts/course/{courseId}/module/{moduleId}/exportCompleted/v1` — Send Export completed notification. Current user will be set as an author and a recipient.
- `POST restapi/personal/notifications/posts/course/{courseId}/module/{moduleId}/importCompleted/v1` — Send Import completed notification. Current user will be set as an author and a recipient.
- `POST restapi/personal/notifications/posts/v1` — Send personal notifications. Current user will be set as an author of all notifications.

---

### `POST restapi/personal/notifications/deletetriggers/location/v1`

Delete all personal notifications which related to a specific location.

**Body** (DeletePersonalNotificationsForLocationTriggerV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Location | The location for which notifications should be deleted. | TypedId |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Location": {
    "Type": "sample string 1",
    "Id": "sample string 2"
  }
}
```
</details>

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


### `POST restapi/personal/notifications/deletetriggers/object/v1`

Delete all personal notifications which related to a specific object.

**Body** (DeletePersonalNotificationsForObjectTriggerV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Object | Object for which notifications should be deleted. | TypedId |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Object": {
    "Type": "sample string 1",
    "Id": "sample string 2"
  }
}
```
</details>

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


### `POST restapi/personal/notifications/posts/course/{courseId}/module/{moduleId}/exportCompleted/v1?section={section}&state={state}&fileName={fileName}&status={status}`

Send Export completed notification. Current user will be set as an author and a recipient.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |
| moduleId |  | integer | Required |
| section |  | Itsolutions.ItslUtils.Constants.ItslearningSectionUrlParameter | Required |
| state |  | string | Required |
| fileName |  | string | Required |
| status |  | boolean | Required |

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


### `POST restapi/personal/notifications/posts/course/{courseId}/module/{moduleId}/importCompleted/v1?section={section}&state={state}&folders={folders}&resources={resources}`

Send Import completed notification. Current user will be set as an author and a recipient.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |
| moduleId |  | integer | Required |
| section |  | Itsolutions.ItslUtils.Constants.ItslearningSectionUrlParameter | Required |
| state |  | string | Required |
| folders |  | integer | Required |
| resources |  | integer | Required |

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


### `POST restapi/personal/notifications/posts/v1`

Send personal notifications. Current user will be set as an author of all notifications.

**Body** (PersonalNotificationsPostV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Message | Notification message in a default locale. | string |  |
| LocalizedMessages | The list of localized messages where Key - locale name by ISO 3166-1 alpha-2 and ISO 639-1 which separated by the symbol '-' (f.e. "en-GB"), Value - the notification message text in this locale. | Dictionary of string [key] and string [value] |  |
| NotificationType | The any string that represents a specific type of notification. | string |  |
| Object | The identifier of object to which notifcation is related. Notification belongs to one object only. | TypedId |  |
| Locations | List of identifiers of all locations to which notification is related. Usually notification belongs to one location, but it's possible to specify any number of locations. | Collection of TypedId |  |
| Url | The absolute URL that pointing to a notification related information. | string |  |
| ToPersonIds | List of person identifiers to send notification to. | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Message": "sample string 1",
  "LocalizedMessages": {
    "sample string 1": "sample string 2",
    "sample string 3": "sample string 4"
  },
  "NotificationType": "sample string 2",
  "Object": {
    "Type": "sample string 1",
    "Id": "sample string 2"
  },
  "Locations": [
    {
      "Type": "sample string 1",
      "Id": "sample string 2"
    }
  ],
  "Url": "sample string 3",
  "ToPersonIds": [
    1
  ]
}
```
</details>

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

