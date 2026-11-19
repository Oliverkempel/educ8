# LightBulletinCourse

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for light bulletins in course scope.

## Endpoints

- `GET restapi/personal/courses/{courseId}/bulletins/v1` — Gets light bulletins by course identifier.
- `PUT restapi/personal/courses/bulletins/{bulletinId}/comments/v1` — Enables or disables comments for the bulletin
- `GET restapi/personal/courses/bulletins/{bulletinId}/elements/v1` — Gets the list of resources attached to given bulletin
- `PUT restapi/personal/courses/bulletins/{bulletinId}/pinned/v1` — Toggles announcement Pinned status.
- `POST restapi/personal/courses/bulletins/{bulletinId}/uploadimage/v1` — Upload file attachment to temporary storage. The file should be included in the post call so we can extract it…
- `DELETE restapi/personal/courses/bulletins/{bulletinId}/v1` — Deletes bulletin created in course and the bulletin's comments, embedded links, attached images and resources.
- `GET restapi/personal/courses/bulletins/{bulletinId}/v1` — Gets light bulletin by identifier.
- `POST restapi/personal/courses/bulletins/v1` — Create new light bulletin.
- `PUT restapi/personal/courses/bulletins/v1` — Updates the light bulletin

---

### `GET restapi/personal/courses/{courseId}/bulletins/v1?timePeriod={timePeriod}&UseNewerThan={UseNewerThan}&FromId={FromId}&PageIndex={PageIndex}&PageSize={PageSize}&includeLexicalJson={includeLexicalJson}`

Gets light bulletins by course identifier.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course identifier. | integer | Required |
| timePeriod | Provides filtering bulletins by time period. | LightBulletins.LightBulletinTimePeriod | Default value is Current |
| UseNewerThan |  | boolean |  |
| FromId |  | integer |  |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| includeLexicalJson | If true, populates with the raw Lexical JSON for bulletins that use rich-text format. | boolean | Default value is False |

**Response:** `EntityList<LightBulletins.LightBulletinV2>` — Light bulletins

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LightBulletins.LightBulletinV2 |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
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
      "ActiveFromDate": "2026-11-18T16:33:14Z",
      "ActiveToDate": "2026-11-18T16:33:14Z",
      "IsSubscribed": true,
      "LexicalJson": "sample string 12",
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
      }
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `PUT restapi/personal/courses/bulletins/{bulletinId}/comments/v1?allowComments={allowComments}`

Enables or disables comments for the bulletin

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | The bulletin identifier. | integer | Required |
| allowComments | If true then comments are not allowed. | boolean | Required |

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


### `GET restapi/personal/courses/bulletins/{bulletinId}/elements/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Gets the list of resources attached to given bulletin

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | The bulletin identifier. | integer | Required |
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


### `PUT restapi/personal/courses/bulletins/{bulletinId}/pinned/v1?pinned={pinned}`

Toggles announcement Pinned status.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | Announcement ID. | integer | Required |
| pinned | true if announcement should be pinned. | boolean | Required |

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


### `POST restapi/personal/courses/bulletins/{bulletinId}/uploadimage/v1`

Upload file attachment to temporary storage. The file should be included in the post call so we can extract it as HttpContext.Current.Request.Files.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | The bulletin identifier. | integer | Required |

**Response:** `LightBulletins.LightBulletinOutputImageData` — Represents information about uploading result.

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsError | True if an error occurred while uploading the image. | boolean |  |
| ErrorMessage | The error message when uploading an image. | LocalizedText |  |
| PreviewFile | The file information, that contains preview for attached image. | Updatable.FileDataV1 |  |
| OriginalFile | The file information, that contains attached image. | Updatable.FileDataV1 |  |

<details><summary>Response JSON sample</summary>

```json
{
  "IsError": true,
  "ErrorMessage": {
    "LocalizedKey": "sample string 1",
    "LocalizedArgs": [
      "sample string 1"
    ]
  },
  "PreviewFile": {
    "FileId": "sample string 1",
    "Signature": "sample string 2",
    "FileName": "sample string 3"
  },
  "OriginalFile": {
    "FileId": "sample string 1",
    "Signature": "sample string 2",
    "FileName": "sample string 3"
  }
}
```
</details>


### `DELETE restapi/personal/courses/bulletins/{bulletinId}/v1`

Deletes bulletin created in course and the bulletin's comments, embedded links, attached images and resources.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | The bulletin identifier | integer | Required |

**Response:** `System.Net.Http.HttpResponseMessage` — 200 OK or Exception

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `GET restapi/personal/courses/bulletins/{bulletinId}/v1`

Gets light bulletin by identifier.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| bulletinId | The light bulletin identifier. | integer | Required |

**Response:** `LightBulletins.LightBulletinV2` — Light bulletin

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The unique id for for the light bulletin, in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| EmbedUrl | A link to 3rd party content, which can be embedded or inserted as OGP. | string |  |
| Pinned | Defines whether current announcement is pinned. | boolean |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. If there are some comments already, they can still be seen. | boolean |  |
| HasResources | DEPRECATED Use ResourcesCount instead. Defines whether any resources were attached to the bulletin. | boolean |  |
| AttachedImages | The list of images attached to the light bulletin. | Collection of LightBulletins.LightBulletinImage |  |
| ResourcesCount | The count of resources attached to the light bulletin. | integer |  |
| CommentsCount | The count of comments for the light bulletin. | integer |  |
| ActiveFromDate | The time when to show the bulletin. | date |  |
| ActiveToDate | The time when to remove the bulletin. | date |  |
| IsSubscribed | Defines if person is subscribed on light bulletin notifications. | boolean |  |
| LexicalJson | Raw Lexical JSON content | string |  |
| PublishedDate | The time when the bulletin was created. | date |  |
| PublishedBy | The person who created the light bulletin. | PersonSimple |  |

<details><summary>Response JSON sample</summary>

```json
{
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
  "ActiveFromDate": "2026-11-18T16:31:46Z",
  "ActiveToDate": "2026-11-18T16:31:46Z",
  "IsSubscribed": true,
  "LexicalJson": "sample string 12",
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
  }
}
```
</details>


### `POST restapi/personal/courses/bulletins/v1`

Create new light bulletin.

**Body** (Updatable.LightBulletin.LightBulletinAddInputDataV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The course identifier. | integer |  |
| Text | The descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| ElementIdsToAttach | The collection of elements to attach. | Collection of integer |  |
| AttachedImages | The collection of attached images. | Collection of Updatable.LightBulletin.LightBulletinInputImageDataV1 |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. | boolean |  |
| ActiveFromDate | The time when to show the bulletin. | date |  |
| ActiveToDate | The time when to remove the bulletin. | date |  |

<details><summary>Request JSON sample</summary>

```json
{
  "CourseId": 1,
  "Text": "sample string 2",
  "ElementIdsToAttach": [
    1
  ],
  "AttachedImages": [
    {
      "PreviewFile": {
        "FileId": "sample string 1",
        "Signature": "sample string 2",
        "FileName": "sample string 3"
      },
      "OriginalFile": {
        "FileId": "sample string 1",
        "Signature": "sample string 2",
        "FileName": "sample string 3"
      }
    }
  ],
  "AllowComments": true,
  "ActiveFromDate": "2026-11-18T16:34:29Z",
  "ActiveToDate": "2026-11-18T16:34:29Z"
}
```
</details>

**Response:** `LightBulletins.LightBulletinIdentifier` — The bulletin identifier.

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The light bulletin identifier | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "LightBulletinId": 1
}
```
</details>


### `PUT restapi/personal/courses/bulletins/v1`

Updates the light bulletin

**Body** (Updatable.LightBulletin.LightBulletinUpdateInputDataV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The unique id for for the light bulletin, in scope of a site (itslearning customer). | integer |  |
| ElementIdsToDetach | The collection of elements to detach. | Collection of integer |  |
| DeleteAttachedImage | The deletion flag of attached image. | boolean |  |
| Text | The descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| ElementIdsToAttach | The collection of elements to attach. | Collection of integer |  |
| AttachedImages | The collection of attached images. | Collection of Updatable.LightBulletin.LightBulletinInputImageDataV1 |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. | boolean |  |
| ActiveFromDate | The time when to show the bulletin. | date |  |
| ActiveToDate | The time when to remove the bulletin. | date |  |

<details><summary>Request JSON sample</summary>

```json
{
  "LightBulletinId": 1,
  "ElementIdsToDetach": [
    1
  ],
  "DeleteAttachedImage": true,
  "Text": "sample string 3",
  "ElementIdsToAttach": [
    1
  ],
  "AttachedImages": [
    {
      "PreviewFile": {
        "FileId": "sample string 1",
        "Signature": "sample string 2",
        "FileName": "sample string 3"
      },
      "OriginalFile": {
        "FileId": "sample string 1",
        "Signature": "sample string 2",
        "FileName": "sample string 3"
      }
    }
  ],
  "AllowComments": true,
  "ActiveFromDate": "2026-11-18T16:32:42Z",
  "ActiveToDate": "2026-11-18T16:32:42Z"
}
```
</details>

**Response:** `System.Net.Http.HttpResponseMessage` — Light bulletin id

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |

