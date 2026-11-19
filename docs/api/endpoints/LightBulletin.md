# LightBulletin

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for light bulletins for personal api. Secured with Oauth2, and requires OAuth2 API scope light bulletin.

## Endpoints

- `PUT restapi/personal/lightbulletins/{lightBulletinId}/comments/subscription/v1` — Turn on/off notifications for new comments for the light bulletin.
- `POST restapi/personal/lightbulletins/{lightBulletinId}/comments/v1` — Adds a new comment for light bulletin.
- `GET restapi/personal/lightbulletins/{lightBulletinId}/comments/v1` — Gets the list of all comments added for given light bulletin sorted in chronological order (by date creation) …
- `POST restapi/personal/lightbulletins/{lightBulletinId}/comments/v2` — Adds a new comment for light bulletin.
- `PUT restapi/personal/lightbulletins/comments/{commentId}/v1` — Updates comment's text.
- `DELETE restapi/personal/lightbulletins/comments/{commentId}/v1` — Deletes comment.
- `PUT restapi/personal/lightbulletins/comments/{commentId}/v2` — Updates comment's text.
- `PUT restapi/personal/lightbulletins/v1` — Marks light bulletins as seen.

---

### `PUT restapi/personal/lightbulletins/{lightBulletinId}/comments/subscription/v1`

Turn on/off notifications for new comments for the light bulletin.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| lightBulletinId | Identifier of light bulletin. | integer | Required |

**Body** (Updatable.LightBulletinSubscriptionUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsSubscribed | Determines if notifications for the current light bulletin comments should be sent to the current person. | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "IsSubscribed": true
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


### `POST restapi/personal/lightbulletins/{lightBulletinId}/comments/v1`

Adds a new comment for light bulletin.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| lightBulletinId | Identifier of light bulletin. | integer | Required |

**Body:** Comment's text. string

<details><summary>Request JSON sample</summary>

```json
"sample string"
```
</details>

**Response:** `Comment` — Returns just added comment.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Comment identifier. | integer |  |
| Author | The person who created the comment. | PersonSimple |  |
| CommentText | Text message. | string |  |
| AllowEditComment | true if it's allowed to edit the comment. Always true if current person is an author of the comment. | boolean |  |
| AllowDeleteComment | true if it's allowed to delete the comment. | boolean |  |
| CreatedDateTime | Date time in UTC when the comment was created. | date |  |
| IsRead | true if the comment has been read by current person. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Id": 1,
  "Author": {
    "PersonId": 1,
    "FirstName": "sample string 2",
    "LastName": "sample string 3",
    "FullName": "sample string 4",
    "ProfileUrl": "sample string 5",
    "AdditionalInfo": "sample string 6",
    "ProfileImageUrl": "sample string 7",
    "ProfileImageUrlSmall": "sample string 8"
  },
  "CommentText": "sample string 2",
  "AllowEditComment": true,
  "AllowDeleteComment": true,
  "CreatedDateTime": "2026-11-18T16:33:14Z",
  "IsRead": true
}
```
</details>


### `GET restapi/personal/lightbulletins/{lightBulletinId}/comments/v1?UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}`

Gets the list of all comments added for given light bulletin sorted in chronological order (by date creation) from newest to oldest.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| lightBulletinId | Identifier of light bulletin. | integer | Required |
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<Comment>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of Comment |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Id": 1,
      "Author": {
        "PersonId": 1,
        "FirstName": "sample string 2",
        "LastName": "sample string 3",
        "FullName": "sample string 4",
        "ProfileUrl": "sample string 5",
        "AdditionalInfo": "sample string 6",
        "ProfileImageUrl": "sample string 7",
        "ProfileImageUrlSmall": "sample string 8"
      },
      "CommentText": "sample string 2",
      "AllowEditComment": true,
      "AllowDeleteComment": true,
      "CreatedDateTime": "2026-11-18T16:34:29Z",
      "IsRead": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `POST restapi/personal/lightbulletins/{lightBulletinId}/comments/v2`

Adds a new comment for light bulletin.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| lightBulletinId | Identifier of light bulletin. | integer | Required |

**Body** (Updatable.LightBulletinCommentUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Comment | Light bulletin comment | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Comment": "sample string 1"
}
```
</details>

**Response:** `Comment` — Returns just added comment.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Comment identifier. | integer |  |
| Author | The person who created the comment. | PersonSimple |  |
| CommentText | Text message. | string |  |
| AllowEditComment | true if it's allowed to edit the comment. Always true if current person is an author of the comment. | boolean |  |
| AllowDeleteComment | true if it's allowed to delete the comment. | boolean |  |
| CreatedDateTime | Date time in UTC when the comment was created. | date |  |
| IsRead | true if the comment has been read by current person. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Id": 1,
  "Author": {
    "PersonId": 1,
    "FirstName": "sample string 2",
    "LastName": "sample string 3",
    "FullName": "sample string 4",
    "ProfileUrl": "sample string 5",
    "AdditionalInfo": "sample string 6",
    "ProfileImageUrl": "sample string 7",
    "ProfileImageUrlSmall": "sample string 8"
  },
  "CommentText": "sample string 2",
  "AllowEditComment": true,
  "AllowDeleteComment": true,
  "CreatedDateTime": "2026-11-18T16:31:28Z",
  "IsRead": true
}
```
</details>


### `PUT restapi/personal/lightbulletins/comments/{commentId}/v1`

Updates comment's text.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| commentId | Comment's identifier. | integer | Required |

**Body:** New comment. string

<details><summary>Request JSON sample</summary>

```json
"sample string"
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


### `DELETE restapi/personal/lightbulletins/comments/{commentId}/v1`

Deletes comment.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| commentId | Comment's identifier. | integer | Required |

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


### `PUT restapi/personal/lightbulletins/comments/{commentId}/v2`

Updates comment's text.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| commentId | Comment's identifier. | integer | Required |

**Body** (Updatable.LightBulletinCommentUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Comment | Light bulletin comment | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Comment": "sample string 1"
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


### `PUT restapi/personal/lightbulletins/v1`

Marks light bulletins as seen.

**Body** (Updatable.LightBulletinUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The Id of the light bulletin to update | integer |  |
| IsSeen | Determines if the current light bulletin is seen by current person. | boolean |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "LightBulletinId": 1,
    "IsSeen": true
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

