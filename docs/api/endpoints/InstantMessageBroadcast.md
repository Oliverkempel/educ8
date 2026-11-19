# InstantMessageBroadcast

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for internal instant messages broadcasting for personal API. Secured with Oauth2, and requires OAuth2 API scope InstantMessage.

## Endpoints

- `POST restapi/personal/instantmessages/broadcast/v1` — Send a broadcast instant message. Creates a new thread for a new broadcast message.
- `DELETE restapi/personal/instantmessages/broadcast/v1` — Marks broadcast threads as deleted and created the async task to delete permanently them and all associated da…
- `GET restapi/personal/instantmessages/broadcastStatistics/{threadId}/v1` — Gets statistics how many recipients read messages of specified threads

---

### `POST restapi/personal/instantmessages/broadcast/v1`

Send a broadcast instant message. Creates a new thread for a new broadcast message.

**Body** (BroadcastInstantMessageToSend)

| Name | Description | Type | Additional information |
|---|---|---|---|
| SenderName | The name of the sender (can be site or a school name or custromized by admin) to assign as the thread's name | string |  |
| SenderHierarchyId | Id of sender's hierarchy | integer |  |
| CourseIds | CourseIds to send instant message to. | Collection of integer |  |
| ProjectIds | ProjectIds to send instant message to. | Collection of integer |  |
| HierarchyIds | HierarchyIds to send instant message to. | Collection of integer |  |
| RecipientRoles | The list of roles in specified hierarchies to send instant message to. | Collection of InstantMessageRecipientRole |  |
| ProfileIds | The list of person profile ids to send instant message to. | Collection of integer |  |
| FileIds | Guids of files already uploaded to temporary file repo storage | Collection of string |  |
| ToPersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId or CourseIds or ProjectIds need to have a valid value. | Collection of integer |  |
| InstantMessageThreadId | The thread to send to, if this is null or zero we will assume it's a new thread. | integer |  |
| Text | The text of the message | string |  |
| SendAsIndividualMessages | Gets whether a message to multiple recipients should be sent as individual messages | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "SenderName": "sample string 1",
  "SenderHierarchyId": 2,
  "CourseIds": [
    1
  ],
  "ProjectIds": [
    1
  ],
  "HierarchyIds": [
    1
  ],
  "RecipientRoles": [
    "Guest"
  ],
  "ProfileIds": [
    1
  ],
  "FileIds": [
    "sample string 1"
  ],
  "ToPersonIds": [
    1
  ],
  "InstantMessageThreadId": 1,
  "Text": "sample string 3",
  "SendAsIndividualMessages": true
}
```
</details>

**Response:** `?`


### `DELETE restapi/personal/instantmessages/broadcast/v1`

Marks broadcast threads as deleted and created the async task to delete permanently them and all associated data.

**Body:** Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`


### `GET restapi/personal/instantmessages/broadcastStatistics/{threadId}/v1`

Gets statistics how many recipients read messages of specified threads

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId |  | integer | Required |

**Response:** `Itsolutions.Itslearning.ItslCore.BusinessLayer.InstantMessage.Broadcast.BroadcastMessageStatisticsEntity`

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId |  | integer |  |
| ReadMessageRecipientsCount |  | integer |  |
| TotalRecipientsCount |  | integer |  |
| ReadMessagePercentage |  | decimal number |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "ReadMessageRecipientsCount": 2,
  "TotalRecipientsCount": 3,
  "ReadMessagePercentage": 66.66666666666667
}
```
</details>

