# Notifications

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for notifications and news feed for personal api.

## Endpoints

- `GET restapi/personal/notifications/{notificationId}/elements/v1` — Gets the list of elements related to given notification
- `GET restapi/personal/notifications/bulletins/{bulletinId}/elements/v1` — Gets the list of resources attached to given bulletin
- `GET restapi/personal/notifications/newsfeed/v1` — Gets the user's news feed. New activities, resources and bulletins. The news feed is collected from all course…
- `PUT restapi/personal/notifications/seenmark/all/v1` — Updates all the personal notifications to be seen.
- `GET restapi/personal/notifications/stream/v1` — Gets user's stream feed, which is collected from user's favorite courses or projects.
- `GET restapi/personal/notifications/stream/v2` — Gets user's stream feed, which is collected from user's favorite courses or projects.
- `GET restapi/personal/notifications/unread/count/v1` — Returns number of unread personal notifications for the user.
- `GET restapi/personal/notifications/unseen/count/v1` — Returns number of unseen personal notifications for the user.
- `PUT restapi/personal/notifications/v1` ⚠️ deprecated — Updates the personal notifications to be read. Obsolete. Use v2 instead.
- `GET restapi/personal/notifications/v1` ⚠️ deprecated — Get personal notifications (read and unread). Obsolete. Use v2 instead.
- `PUT restapi/personal/notifications/v2` — Updates the personal notifications to be read.
- `GET restapi/personal/notifications/v2` — Get personal notifications (read and unread).

---

### `GET restapi/personal/notifications/{notificationId}/elements/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the list of elements related to given notification

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| notificationId | The notification identifier | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<ElementLink>` — The collection of elements.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of ElementLink |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "ElementId": 1,
      "ElementType": "Unknown",
      "Title": "sample string 2",
      "Url": "sample string 3",
      "ContentUrl": "sample string 4",
      "IconUrl": "sample string 5",
      "LearningToolId": 6,
      "Homework": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/notifications/bulletins/{bulletinId}/elements/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the list of resources attached to given bulletin

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | The bulletin identifier | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<ElementLink>` — The collection of .

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of ElementLink |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "ElementId": 1,
      "ElementType": "Unknown",
      "Title": "sample string 2",
      "Url": "sample string 3",
      "ContentUrl": "sample string 4",
      "IconUrl": "sample string 5",
      "LearningToolId": 6,
      "Homework": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/notifications/newsfeed/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the user's news feed. New activities, resources and bulletins. The news feed is collected from all courses, all courses or projects, favorite courses or favorite courses or projects depending on the setting for "Latest changes" content block on the "My Page" dashboard.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<NewsFeedItem>` — List of NewsFeedItems

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of NewsFeedItem |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NewsFeedItemId": 1,
      "Text": "sample string 2",
      "LocationTitle": "sample string 3",
      "PublishedDate": "2026-11-18T16:33:14Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Url": "sample string 5",
      "IconUrl": "sample string 6"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `PUT restapi/personal/notifications/seenmark/all/v1`

Updates all the personal notifications to be seen.

**Response:** `?`


### `GET restapi/personal/notifications/stream/v1?UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}&fromPublishedDate={fromPublishedDate}`

Gets user's stream feed, which is collected from user's favorite courses or projects.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| fromPublishedDate | [Optional] The PublishedDate of last stream item to get the collection of items next to it. Optional, if not supplied then the list will be populated with the latest items. Date has to contain also time in , for ex. YYYY-MM-DDTHH:MM:SS.sssssss or YYYY-MM-DD HH:MM:SS. | date |  |

**Response:** `EntityList<StreamItem>` — The collection of StreamItems.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of StreamItem |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NotificationId": 1,
      "Text": "sample string 2",
      "LocationTitle": "sample string 3",
      "PublishedDate": "2026-11-18T16:31:46Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Url": "sample string 5",
      "ContentUrl": "sample string 6",
      "IconUrl": "sample string 7",
      "LightBulletin": {
        "LightBulletinId": 1,
        "Text": "sample string 2",
        "EmbedUrl": "sample string 3",
        "AllowComments": true,
        "ElementLinks": [
          {
            "ElementId": 1,
            "ElementType": "Unknown",
            "Title": "sample string 2",
            "Url": "sample string 3",
            "ContentUrl": "sample string 4",
            "IconUrl": "sample string 5",
            "LearningToolId": 6,
            "Homework": true
          }
        ]
      },
      "ElementId": 1,
      "ElementType": "Unknown"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/notifications/stream/v2?showLightBulletins={showLightBulletins}&UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets user's stream feed, which is collected from user's favorite courses or projects.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| showLightBulletins | If True returns stream with light bulletins. | boolean | Default value is True |
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<StreamItemV2>` — The collection of StreamItems.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of StreamItemV2 |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NotificationId": 1,
      "Text": "sample string 2",
      "LocationTitle": "sample string 3",
      "PublishedDate": "2026-11-18T16:31:28Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Url": "sample string 5",
      "ContentUrl": "sample string 6",
      "IconUrl": "sample string 7",
      "LightBulletin": {
        "LightBulletinId": 1,
        "Text": "sample string 2",
        "EmbedUrl": "sample string 3",
        "Pinned": true,
        "AllowComments": true,
        "HasResources": true,
        "AttachedImages": [
          {
            "PreviewFileId": 1,
            "PreviewFileUrl": "sample string 2",
            "OriginalFileId": 3,
            "OriginalFileUrl": "sample string 4",
            "PreviewFilePngUrl": "sample string 5"
          }
        ],
        "ResourcesCount": 7,
        "CommentsCount": 8,
        "ActiveFromDate": "2026-11-18T16:31:28Z",
        "ActiveToDate": "2026-11-18T16:31:28Z",
        "IsSubscribed": true,
        "LexicalJson": "sample string 12",
        "PublishedDate": "2026-11-18T16:31:28Z",
        "PublishedBy": {
          "PersonId": 1,
          "FirstName": "sample string 2",
          "LastName": "sample string 3",
          "FullName": "sample string 4",
          "ProfileUrl": "sample string 5",
          "AdditionalInfo": "sample string 6",
          "ProfileImageUrl": "sample string 7",
          "ProfileImageUrlSmall": "sample string 8"
        }
      },
      "ElementId": 8,
      "ElementType": "Unknown",
      "LearningToolId": 9,
      "HasMoreElements": true,
      "ElementsCount": 11,
      "LocationType": "Course",
... (truncated)
```
</details>


### `GET restapi/personal/notifications/unread/count/v1`

Returns number of unread personal notifications for the user.

**Response:** `integer` — Count of unread notifications

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `GET restapi/personal/notifications/unseen/count/v1`

Returns number of unseen personal notifications for the user.

**Response:** `integer` — Count of unseen notifications

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `PUT restapi/personal/notifications/v1`

Updates the personal notifications to be read. Obsolete. Use v2 instead.

**Body** (Updatable.NotificationUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| NotificationId | The notification id | integer |  |
| IsRead | Whether the notification has been read or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "NotificationId": 1,
    "IsRead": true
  }
]
```
</details>

**Response:** `EntityList<Notification>` — Updated notifications

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Notification |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NotificationId": 1,
      "Text": "sample string 2",
      "PublishedDate": "2026-11-18T16:30:06Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Type": "Unknown",
      "Url": "sample string 4",
      "ContentUrl": "sample string 5",
      "IsRead": true,
      "IsAnonymous": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/notifications/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Get personal notifications (read and unread). Obsolete. Use v2 instead.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<Notification>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Notification |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NotificationId": 1,
      "Text": "sample string 2",
      "PublishedDate": "2026-11-18T16:33:14Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Type": "Unknown",
      "Url": "sample string 4",
      "ContentUrl": "sample string 5",
      "IsRead": true,
      "IsAnonymous": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `PUT restapi/personal/notifications/v2`

Updates the personal notifications to be read.

**Body** (Updatable.NotificationUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| NotificationId | The notification id | integer |  |
| IsRead | Whether the notification has been read or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "NotificationId": 1,
    "IsRead": true
  }
]
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


### `GET restapi/personal/notifications/v2?UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}`

Get personal notifications (read and unread).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<Notification>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Notification |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "NotificationId": 1,
      "Text": "sample string 2",
      "PublishedDate": "2026-11-18T16:34:29Z",
      "PublishedBy": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "Type": "Unknown",
      "Url": "sample string 4",
      "ContentUrl": "sample string 5",
      "IsRead": true,
      "IsAnonymous": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

