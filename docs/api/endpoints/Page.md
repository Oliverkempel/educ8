# Page

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for page for personal API. Requests will be forwarded to the page api. We want to abstract SharableObjectId (learningObjectId) and LearningObjectInstanceId from the client (e.g. mobile), therefore endpoints use ElementId. Secured with Oauth2, and requires OAuth2 API scope .

## Endpoints

- `GET restapi/personal/pages/{elementId}/contentblocks/{contentBlockId}/files/{fileId}/v1` — Download a file from a content block by redirecting to the secure download URL.
- `GET restapi/personal/pages/{elementId}/contentblocks/{contentBlockId}/v1` — Get a content block by contentBlockId.
- `PUT restapi/personal/pages/{elementId}/contentblocks/files/{contentBlockId}/v1` — Update a poll content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/files/v1` — Create a new files content block.
- `PUT restapi/personal/pages/{elementId}/contentblocks/images/{contentBlockId}/v1` — Updates images content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/images/size/v1` — 
- `POST restapi/personal/pages/{elementId}/contentblocks/images/v1` — Creates a new images content block.
- `PUT restapi/personal/pages/{elementId}/contentblocks/placement/v1` — Updates the placement position of a content block within the page layout. Returns 404 if the block is not foun…
- `PUT restapi/personal/pages/{elementId}/contentblocks/polls/{contentBlockId}/v1` — Update a poll content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/polls/{contentBlockId}/votes/v1` — Vote on a content block poll.
- `DELETE restapi/personal/pages/{elementId}/contentblocks/polls/{contentBlockId}/votes/v1` — Delete all the votes for a poll content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/polls/v1` — Create a new poll content block.
- `PUT restapi/personal/pages/{elementId}/contentblocks/rss/{contentBlockId}/v1` — Update an RSS content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/rss/v1` — Create a new RSS content block.
- `PUT restapi/personal/pages/{elementId}/contentblocks/text/{contentBlockId}/v1` — Updates a text content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/text/v1` — Create a new text content block.
- `POST restapi/personal/pages/{elementId}/contentblocks/trashcan/v1` — Performs trashcan operations (delete or restore) on content blocks.
- `GET restapi/personal/pages/{elementId}/contentblocks/trashcan/v1` — Get all content blocks currently in the trashcan for a page element.
- `DELETE restapi/personal/pages/{elementId}/contentblocks/v1` — Permanently delete content blocks from the trashcan. This action cannot be undone.
- `PUT restapi/personal/pages/{elementId}/settings/v1` — Updates itslearning-related settings of Page element.
- `POST restapi/personal/pages/{elementId}/settings/v1/dismissed` — This endpoint is called when page settings were dismissed without saving.
- `GET restapi/personal/pages/{elementId}/v1` — Get a page element by elementId.
- `PUT restapi/personal/pages/{elementId}/v1` — Update an existing page for a learning object.
- `POST restapi/personal/pages/{elementId}/v1/presentationMode` — This endpoint is called when a user views a page in presentation mode.
- `POST restapi/personal/pages/{elementId}/v1/viewLearningObjectives` — This endpoint is called when the learning objectives modal is viewed.
- `POST restapi/personal/pages/yourcollection/{elementId}/v1` — 

---

### `GET restapi/personal/pages/{elementId}/contentblocks/{contentBlockId}/files/{fileId}/v1`

Download a file from a content block by redirecting to the secure download URL.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |
| contentBlockId | The unique identifier of the content block containing the file | integer | Required |
| fileId | The unique identifier of the file to download | integer | Required |

**Response:** `?` — A redirect to the secure download URL, or NotFound if the file doesn't exist


### `GET restapi/personal/pages/{elementId}/contentblocks/{contentBlockId}/v1`

Get a content block by contentBlockId.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element to retrieve | integer | Required |
| contentBlockId | The unique identifier of the content block poll | integer | Required |

**Response:** `?` — PageResponse containing the page data


### `PUT restapi/personal/pages/{elementId}/contentblocks/files/{contentBlockId}/v1`

Update a poll content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |
| contentBlockId | The unique identifier of the content block | integer | Required |

**Body** (Personal.Page.Requests.UpdateContentBlockFilesRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| NewFiles |  | Collection of Personal.FileUpload.UploadedTempFile |  |
| DeletedFiles |  | Collection of integer |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "newFiles": [
    {
      "fileRepoId": "sample string 1",
      "signedFileRepoId": "sample string 2",
      "name": "sample string 3",
      "mimeType": "sample string 4",
      "sourceType": 1,
      "isTemplate": true
    }
  ],
  "deletedFiles": [
    1
  ],
  "contentBlockId": 1,
  "type": "sample string 2",
  "title": "sample string 3",
  "order": 4,
  "layoutZone": "sample string 5",
  "color": "sample string 6"
}
```
</details>

**Response:** `?` — Updated poll content block


### `POST restapi/personal/pages/{elementId}/contentblocks/files/v1`

Create a new files content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockFilesRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Files | The collection of temporary files that have been uploaded for this content block. Defaults to an empty list. | Collection of Personal.FileUpload.UploadedTempFile |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "files": [
    {
      "fileRepoId": "sample string 1",
      "signedFileRepoId": "sample string 2",
      "name": "sample string 3",
      "mimeType": "sample string 4",
      "sourceType": 1,
      "isTemplate": true
    }
  ],
  "contentBlockId": 1,
  "type": "sample string 2",
  "title": "sample string 3",
  "order": 4,
  "layoutZone": "sample string 5",
  "color": "sample string 6"
}
```
</details>

**Response:** `?` — Created files content block


### `PUT restapi/personal/pages/{elementId}/contentblocks/images/{contentBlockId}/v1`

Updates images content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element. | integer | Required |
| contentBlockId | The unique identifier of the content block. | integer | Required |

**Body** (Personal.Page.Requests.UpdateContentBlockImagesRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| NewImages | Images to add to the content block. | Collection of Personal.Page.PageApiModels.Requests.ImageItem |  |
| DeletedImageIds | Image IDs to delete from the content block. | Collection of integer |  |
| UpdatedImages | Images that should be updated in the content block. | Collection of Personal.Page.PageApiModels.ImageMetaData |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "newImages": [
    {
      "imageFile": {
        "fileRepoId": "dbad816c-bf8f-4b03-9b8e-7e49cdabbb98",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "largeImageFile": {
        "fileRepoId": "e173cced-5255-4b80-bbe9-98a69134fa31",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "mediumImageFile": {
        "fileRepoId": "ace22626-72d5-4c02-887e-acf7bdbc7dcb",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "smallImageFile": {
        "fileRepoId": "ef83bb93-d25c-492b-b3be-76919447a16b",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "altText": "sample string 1",
      "caption": "sample string 2"
    }
  ],
  "deletedImageIds": [
    1
  ],
  "updatedImages": [
    {
      "imageId": 1,
      "altText": "sample string 2",
      "caption": "sample string 3"
    }
  ],
  "contentBlockId": 1,
  "type": "sample string 2",
  "title": "sample string 3",
  "order": 4,
  "layoutZone": "sample string 5",
  "color": "sample string 6"
}
```
</details>

**Response:** `?` — Updated image content block.


### `POST restapi/personal/pages/{elementId}/contentblocks/images/size/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | integer | Required |

**Body** (Personal.Page.PageApiModels.Requests.CreateResizedImageFilesRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| TempImageFile | A temp image file in file repo. | Personal.Page.PageApiModels.Requests.TempFileRepoFile |  |

<details><summary>Request JSON sample</summary>

```json
{
  "tempImageFile": {
    "fileRepoId": "73f0f9c0-18e1-4305-801e-d16e0f79de1f",
    "signedFileRepoId": "sample string 2",
    "name": "sample string 3",
    "mimeType": "sample string 4"
  }
}
```
</details>

**Response:** `?`


### `POST restapi/personal/pages/{elementId}/contentblocks/images/v1`

Creates a new images content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element. | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockImagesRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Images | The collection of temporary images that have been uploaded for this content block. Defaults to an empty list. | Collection of Personal.Page.PageApiModels.Requests.ImageItem |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "images": [
    {
      "imageFile": {
        "fileRepoId": "f30fe5c3-4b8d-4dc7-81a3-e3e4d4b7d91f",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "largeImageFile": {
        "fileRepoId": "f0901391-58d8-4a6f-a5da-a38a99459004",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "mediumImageFile": {
        "fileRepoId": "4b045dfa-fbcb-4fa1-bc20-dc6bee9d60b3",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "smallImageFile": {
        "fileRepoId": "6b657ea4-7f9d-4431-aee6-b30eb9e533c1",
        "signedFileRepoId": "sample string 2",
        "name": "sample string 3",
        "mimeType": "sample string 4"
      },
      "altText": "sample string 1",
      "caption": "sample string 2"
    }
  ],
  "contentBlockId": 1,
  "type": "sample string 2",
  "title": "sample string 3",
  "order": 4,
  "layoutZone": "sample string 5",
  "color": "sample string 6"
}
```
</details>

**Response:** `?` — Created images content block.


### `PUT restapi/personal/pages/{elementId}/contentblocks/placement/v1`

Updates the placement position of a content block within the page layout. Returns 404 if the block is not found or 403 if the user lacks permission.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The learning object identifier associated with the page. | integer | Required |

**Body** (Personal.Page.PageApiModels.Requests.UpdatePlacementRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| LeftColumnContentBlockIds |  | Collection of integer |  |
| RightColumnContentBlockIds |  | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "leftColumnContentBlockIds": [
    1
  ],
  "rightColumnContentBlockIds": [
    1
  ]
}
```
</details>

**Response:** `?` — The updated content block with the new placement information.


### `PUT restapi/personal/pages/{elementId}/contentblocks/polls/{contentBlockId}/v1`

Update a poll content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |
| contentBlockId | The unique identifier of the content block | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockPollsRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| VotedAlternativeId | The identifier of the alternative that the current user has voted for. Null if the user has not voted yet. | integer |  |
| TotalVotes | The total number of votes cast across all poll alternatives. Null if vote count is not available. | integer |  |
| Question | The main question text displayed to users for the poll. | string |  |
| Settings | The configuration settings that control poll behavior and display options. | Personal.Page.Requests.PollSettings |  |
| Alternatives | The collection of available voting options for the poll. Defaults to an empty list. | Collection of Personal.Page.Requests.PollAlternative |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "votedAlternativeId": 1,
  "totalVotes": 1,
  "question": "sample string 1",
  "settings": {
    "showResults": "sample string 1",
    "displayNumberOfVotes": true,
    "userCanChangeVote": true
  },
  "alternatives": [
    {
      "alternativeId": 1,
      "text": "sample string 2"
    }
  ],
  "contentBlockId": 2,
  "type": "sample string 3",
  "title": "sample string 4",
  "order": 5,
  "layoutZone": "sample string 6",
  "color": "sample string 7"
}
```
</details>

**Response:** `?` — Updated poll content block


### `POST restapi/personal/pages/{elementId}/contentblocks/polls/{contentBlockId}/votes/v1`

Vote on a content block poll.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element containing the poll | integer | Required |
| contentBlockId | The unique identifier of the content block poll | integer | Required |

**Body** (Personal.Page.Requests.VoteRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| VoteAlternativeId | The unique identifier of the poll alternative that the user is voting for. | integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "voteAlternativeId": 1
}
```
</details>

**Response:** `Personal.Page.Responses.ContentBlockPoll` — ContentBlockPoll with updated voting results

| Name | Description | Type | Additional information |
|---|---|---|---|
| VotedAlternativeId | Id of chosen alternative (null if not voted yet) | integer |  |
| TotalVotes | Total number of votes in the poll (null if not allowed to show) | integer |  |
| Question | Poll question | string |  |
| Settings |  | Personal.Page.Responses.ContentBlockPollSettings |  |
| Alternatives |  | Collection of Personal.Page.Responses.ContentBlockPollAlternativeData |  |
| ContentBlockId | Content block identifier | integer |  |
| Type |  | Personal.Page.Responses.ContentBlockType |  |
| Title | Content block title | string |  |
| Order | Content block order in the page | integer |  |
| LayoutZone | Content block layout zone - left or right | Personal.Page.Responses.ContentBlockLayoutZone |  |
| Color | Content block color | Personal.Page.Responses.ColorSchema |  |

<details><summary>Response JSON sample</summary>

```json
{
  "votedAlternativeId": 1,
  "totalVotes": 1,
  "question": "sample string 1",
  "settings": {
    "showResults": "EditorsOnly",
    "displayNumberOfVotes": true,
    "userCanChangeVote": true
  },
  "alternatives": [
    {
      "alternativeId": 1,
      "text": "sample string 2",
      "resultPercentage": 1.1,
      "votesCount": 1
    }
  ],
  "contentBlockId": 2,
  "type": "Text",
  "title": "sample string 3",
  "order": 4,
  "layoutZone": "Left",
  "color": "Default"
}
```
</details>


### `DELETE restapi/personal/pages/{elementId}/contentblocks/polls/{contentBlockId}/votes/v1`

Delete all the votes for a poll content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The ID of the element. | integer | Required |
| contentBlockId | The ID of the content block to remove votes for. | integer | Required |

**Response:** `?` — The poll content block DTO.


### `POST restapi/personal/pages/{elementId}/contentblocks/polls/v1`

Create a new poll content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockPollsRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| VotedAlternativeId | The identifier of the alternative that the current user has voted for. Null if the user has not voted yet. | integer |  |
| TotalVotes | The total number of votes cast across all poll alternatives. Null if vote count is not available. | integer |  |
| Question | The main question text displayed to users for the poll. | string |  |
| Settings | The configuration settings that control poll behavior and display options. | Personal.Page.Requests.PollSettings |  |
| Alternatives | The collection of available voting options for the poll. Defaults to an empty list. | Collection of Personal.Page.Requests.PollAlternative |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "votedAlternativeId": 1,
  "totalVotes": 1,
  "question": "sample string 1",
  "settings": {
    "showResults": "sample string 1",
    "displayNumberOfVotes": true,
    "userCanChangeVote": true
  },
  "alternatives": [
    {
      "alternativeId": 1,
      "text": "sample string 2"
    }
  ],
  "contentBlockId": 2,
  "type": "sample string 3",
  "title": "sample string 4",
  "order": 5,
  "layoutZone": "sample string 6",
  "color": "sample string 7"
}
```
</details>

**Response:** `?` — Created poll content block


### `PUT restapi/personal/pages/{elementId}/contentblocks/rss/{contentBlockId}/v1`

Update an RSS content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |
| contentBlockId | The unique identifier of the content block | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockRssRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| RssUrl | The URL of the RSS feed to display in this content block. | string |  |
| FeedsCount | The number of feed items to display from the RSS feed. | integer |  |
| ShowSummary | Indicates whether to show the summary of each feed item. | boolean |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "rssUrl": "sample string 1",
  "feedsCount": 2,
  "showSummary": true,
  "contentBlockId": 4,
  "type": "sample string 5",
  "title": "sample string 6",
  "order": 7,
  "layoutZone": "sample string 8",
  "color": "sample string 9"
}
```
</details>

**Response:** `?` — Updated RSS content block


### `POST restapi/personal/pages/{elementId}/contentblocks/rss/v1`

Create a new RSS content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockRssRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| RssUrl | The URL of the RSS feed to display in this content block. | string |  |
| FeedsCount | The number of feed items to display from the RSS feed. | integer |  |
| ShowSummary | Indicates whether to show the summary of each feed item. | boolean |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "rssUrl": "sample string 1",
  "feedsCount": 2,
  "showSummary": true,
  "contentBlockId": 4,
  "type": "sample string 5",
  "title": "sample string 6",
  "order": 7,
  "layoutZone": "sample string 8",
  "color": "sample string 9"
}
```
</details>

**Response:** `?` — Created RSS content block


### `PUT restapi/personal/pages/{elementId}/contentblocks/text/{contentBlockId}/v1`

Updates a text content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |
| contentBlockId | The unique identifier of the content block | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockTextRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| RawText | The text content to display in this content block. Can contain formatted text or plain text. Doesn't contain plugin or video content preprocessing. | string |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "rawText": "sample string 1",
  "contentBlockId": 2,
  "type": "sample string 3",
  "title": "sample string 4",
  "order": 5,
  "layoutZone": "sample string 6",
  "color": "sample string 7"
}
```
</details>

**Response:** `?` — Updated text content block


### `POST restapi/personal/pages/{elementId}/contentblocks/text/v1`

Create a new text content block.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Body** (Personal.Page.Requests.CreateContentBlockTextRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| RawText | The text content to display in this content block. Can contain formatted text or plain text. Doesn't contain plugin or video content preprocessing. | string |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "rawText": "sample string 1",
  "contentBlockId": 2,
  "type": "sample string 3",
  "title": "sample string 4",
  "order": 5,
  "layoutZone": "sample string 6",
  "color": "sample string 7"
}
```
</details>

**Response:** `?` — Created text content block


### `POST restapi/personal/pages/{elementId}/contentblocks/trashcan/v1`

Performs trashcan operations (delete or restore) on content blocks.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Body** (Personal.Page.Requests.DeleteRestoreRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContentBlockIds | The list of content block IDs to be moved in or out from trashcan. | Collection of integer |  |
| IsDeleted | The boolean that describe the operation that needs to be done. If true, then the content blocks will be moved into trashcan, otherwise will be restored. | boolean |  |
| LayoutZone | The layout zone where the content block will be placed. Optional property that defaults to Left. Examples: - LayoutZone = Left => put content in the left column - LayoutZone = Right => put content in the right column | Personal.Page.PageApiModels.Enums.LayoutZoneDto |  |
| Order | The position of the content block within the specified layout zone. Optional property, default last in this layout zone. If there is an existing content block at this position, that one and all blocks below will be shifted down. Examples: - Order = 1, LayoutZone = Left => top left position - Order = 2, LayoutZone = Left => second position in left column - Order = 1, LayoutZone = Right => top right position | integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "contentBlockIds": [
    1
  ],
  "isDeleted": true,
  "layoutZone": 1,
  "order": 1
}
```
</details>

**Response:** `?` — Success response if the operation completes successfully


### `GET restapi/personal/pages/{elementId}/contentblocks/trashcan/v1`

Get all content blocks currently in the trashcan for a page element.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Response:** `?` — List of content blocks that are in the trashcan


### `DELETE restapi/personal/pages/{elementId}/contentblocks/v1`

Permanently delete content blocks from the trashcan. This action cannot be undone.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element | integer | Required |

**Body** (Personal.Page.Requests.PermanentlyDeleteContentBlocksRequestUI)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContentBlockIds | The list of content block IDs to be permanently deleted. | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "contentBlockIds": [
    1
  ]
}
```
</details>

**Response:** `?` — Success response if the deletion completes successfully


### `PUT restapi/personal/pages/{elementId}/settings/v1`

Updates itslearning-related settings of Page element.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | Element ID. | integer | Required |

**Body** (Personal.Page.Requests.UpdatePageSettingsRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title |  | string |  |
| Visibility |  | Personal.Element.Models.VisibilitySettings |  |
| AssignTo |  | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds |  | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "title": "sample string 1",
  "visibility": {
    "isActive": true,
    "activeFrom": "2026-11-18T16:32:42Z",
    "activeTo": "2026-11-18T16:32:42Z"
  },
  "assignTo": {
    "participantIds": [
      1
    ],
    "groupIds": [
      1
    ],
    "groupName": "sample string 1"
  },
  "learningObjectivesIds": [
    1
  ]
}
```
</details>

**Response:** `?` — Empty result.


### `POST restapi/personal/pages/{elementId}/settings/v1/dismissed`

This endpoint is called when page settings were dismissed without saving.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | string |  |

**Response:** `?`


### `GET restapi/personal/pages/{elementId}/v1`

Get a page element by elementId.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The unique identifier of the page element to retrieve | integer | Required |

**Response:** `Personal.Page.Responses.PageResponse` — PageResponse containing the page data

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | The unique identifier for the page element in scope of a customer. | integer |  |
| Title | The title of the page. | string |  |
| ContentBlocks | The content blocks on the page. | Collection of Personal.Page.Responses.ContentBlock |  |
| CanEditSettings | True if the user can modify the settings of the page (e.g. learning objectives, activation, layout). | boolean |  |
| CanEditTitle | True if the user can modify the title of the page. This can be true even when is false, e.g. for a student editing a page they collaborate on inside a collaboration folder. | boolean |  |
| CanEditContent | True if the page's content blocks can be edited by the current user. | boolean |  |
| CanChangePermissions | True if the current user is able to manage permissions for the current element. | boolean |  |
| CanDelete | True if the current user is allowed to delete the page (put in trashcan). An element which is part of learning path, must be removed from the learning path first. | boolean |  |
| IsLearningPathSubElement | Defines whether current element belongs to learning path. | boolean |  |
| PlansUsingResourceCount | Number of plans in the course that use the current page element. The "N plans using this resource" link is only shown when this is greater than one. | integer |  |
| Layout | The layout of the page (how many columns). | Personal.Page.Responses.LayoutSchemaType |  |
| Urls | URLs used in page. | Personal.Page.Responses.PageUrls |  |
| IconUrls | URLs for the icons used for page. | Personal.Page.Responses.PageIconUrls |  |
| Kpi | Contains KPI context, sections and measurements. | Personal.Page.Responses.PageKpiModel |  |
| IsCurrentlyActive | Indicates whether the page is visible in the present moment. | boolean |  |
| IsActive | Indicates whether the element visibility setting is set to "Active" or "Scheduled". The value of false means "Inactive". | boolean |  |
| ActiveFrom | When the page becomes active. Null means it is active immediately. | date |  |
| ActiveTo | When the page becomes inactive. Null means it remains active indefinitely. | date |  |
| LearningObjectiveSettings | Contains learning objective-related settings. | Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveSettingsModel |  |
| SharingInfo | Sharing info - contains information about how and if page is connected to library. If this is null, it means that the page is not connected to the library or that user does not have access to this information. | Personal.Element.Models.ElementLibrarySharingInfo |  |
| CopyStatus | The copy status of the page. | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolCopyStatus |  |
| HasPermissionToCreateNewVersion | Returns true if the user is allowed to create a new version of an existing page, which is disconnected from the library. Check for a full check, since tool or author can still deny and override a user's permission. | boolean |  |
| ToolSupportsCopying | Returns true if the learning tool supports copying, which is required for creating a new version of an existing learning tool element. | boolean |  |
| AuthorAllowsCreateNewVersion | Returns true if the author of the learning tool element allows a new version to be created. | boolean |  |
| CanCreateNewVersion | Returns true if the user is allowed to create a new version of an existing learning tool element. | boolean |  |
| CanAddToLibrary | Returns true if the user is allowed to add the learning tool elements to user's library collection. | boolean |  |
| CurrentLanguage | Current language which is used for UI. | string |  |
| FirstDayOfWeek | The first day of week. | integer |  |
| ShortDatePattern | Custom format string for a short date value. | string |  |
| FormatCultureName | The current format culture name. | string |  |
| Use24TimeFormat | If the user uses 24-hour time format. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "elementId": 1,
  "title": "sample string 2",
  "contentBlocks": null,
  "canEditSettings": true,
  "canEditTitle": true,
  "canEditContent": true,
  "canChangePermissions": true,
  "canDelete": true,
  "isLearningPathSubElement": true,
  "plansUsingResourceCount": 9,
  "layout": "OneColumn",
  "urls": {
    "permissions": "sample string 1",
    "sharedElement": "sample string 2"
  },
  "iconUrls": {
    "archiveIcon": "sample string 1",
    "contentBlockChangeColumnIcon": "sample string 2",
    "contentBlockDeleteIcon": "sample string 3",
    "contentBlockEditIcon": "sample string 4",
    "contentBlockFilesIcon": "sample string 5",
    "contentBlockImagesIcon": "sample string 6",
    "contentBlockMoveDownIcon": "sample string 7",
    "contentBlockMoveUpIcon": "sample string 8",
    "contentBlockPollIcon": "sample string 9",
    "contentBlockRichTextIcon": "sample string 10",
    "contentBlockRssIcon": "sample string 11",
    "layoutOneColumnIcon": "sample string 12",
    "layoutTwoColumnsIcon": "sample string 13",
    "layoutTwoColumnsWideLeftIcon": "sample string 14",
    "learningObjectiveIcon": "sample string 15",
    "listenIcon": "sample string 16",
    "notifyAboutChangesIcon": "sample string 17",
    "permissions": "sample string 18",
    "restoreIcon": "sample string 19",
    "editInLibraryIcon": "sample string 20",
    "createNewVersionIcon": "sample string 21"
  },
  "kpi": {
    "endpoint": "/RestApi/common/kpi/v1",
    "context": 1,
    "sectionPage": 60,
    "measurementPrintPage": 759
  },
  "isCurrentlyActive": true,
  "isActive": true,
  "activeFrom": "2026-11-18T16:34:29Z",
  "activeTo": "2026-11-18T16:34:29Z",
  "learningObjectiveSettings": {
    "isLearningObjectiveEnabledForLocation": true,
    "courseHasLearningObjectives": true,
    "learningObjectives": [
      {
        "id": 1,
        "title": "sample string 2",
        "description": "sample string 3",
        "isFromRepository": true,
        "type": 1
      }
... (truncated)
```
</details>


### `PUT restapi/personal/pages/{elementId}/v1`

Update an existing page for a learning object.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The learning object identifier associated with the page. | integer | Required |

**Body** (Personal.Page.PageApiModels.Requests.UpdatePageRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Layout | The layout for the page, defining how content blocks should be arranged. | Personal.Page.PageApiModels.Enums.LayoutSchemaDto |  |
| BlocksPlacement |  | Personal.Page.PageApiModels.Requests.UpdatePlacementRequest |  |

<details><summary>Request JSON sample</summary>

```json
{
  "layout": 0,
  "blocksPlacement": {
    "leftColumnContentBlockIds": [
      1
    ],
    "rightColumnContentBlockIds": [
      1
    ]
  }
}
```
</details>

**Response:** `?` — The updated page


### `POST restapi/personal/pages/{elementId}/v1/presentationMode`

This endpoint is called when a user views a page in presentation mode.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | string |  |

**Response:** `?`


### `POST restapi/personal/pages/{elementId}/v1/viewLearningObjectives`

This endpoint is called when the learning objectives modal is viewed.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId | The learning object identifier associated with the page. | integer | Required |

**Response:** `?`


### `POST restapi/personal/pages/yourcollection/{elementId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | integer | Required |

**Response:** `?`

