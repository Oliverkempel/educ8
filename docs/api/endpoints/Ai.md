# Ai

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Handles calling AI features, currently from the editor plugin.

## Endpoints

- `POST restapi/personal/ai/elements/v1` — Method for creating elements
- `PUT restapi/personal/ai/guidelines/v1` — Updates the AI Guidelines confirmation date for the user.
- `POST restapi/personal/ai/responses/v1` — Creates a response using the Responses API with optional file search.
- `DELETE restapi/personal/ai/stores/{storeToken}/files/{fileId}/v1` — Deletes a file from a store.
- `POST restapi/personal/ai/stores/{storeToken}/files/v1` — Creates a store from uploaded files or adds files to an existing store.
- `POST restapi/personal/ai/stores/{storeToken}/resources/v1` — Creates a store from itslearning resources or adds resources to an existing store.
- `GET restapi/personal/ai/stores/{storeToken}/status/v1` — Gets the status of all files in a store.
- `POST restapi/personal/ai/stores/files/v1` — Creates a store from uploaded files or adds files to an existing store.
- `POST restapi/personal/ai/stores/resources/v1` — Creates a store from itslearning resources or adds resources to an existing store.

---

### `POST restapi/personal/ai/elements/v1`

Method for creating elements

**Body** (Personal.Ai.CreateElementRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementType | The element type | ElementType |  |
| Title | Title of the content | string |  |
| Text | The text | string |  |
| FolderId | The folder id | integer |  |
| PlanId | If created element belongs to a plan | integer |  |
| Feature | The AI feature type of the element (e.g. Assignment, Article, Inspire me etc.). Used to measure KPI when a user saves an element created by AI. | Personal.Ai.AiFeatureType |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ElementType": "Unknown",
  "Title": "sample string 1",
  "Text": "sample string 2",
  "FolderId": 3,
  "PlanId": 4,
  "Feature": 0
}
```
</details>

**Response:** `?` — The element identifier for the newly created element


### `PUT restapi/personal/ai/guidelines/v1`

Updates the AI Guidelines confirmation date for the user.

**Response:** `?`


### `POST restapi/personal/ai/responses/v1`

Creates a response using the Responses API with optional file search.

**Body** (Personal.Ai.AiResponseRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Feature | The feature to use. | Personal.Ai.AiFeatureType |  |
| LocationId | Course or project id. | integer |  |
| LocationType | Course or project. | Itsolutions.ItslUtils.Constants.LocationType |  |
| PlanId | Plan identifier. | integer |  |
| SystemPrompt | System prompt for the AI (template). Normally is empty, only taken into account when the prompt engineering feature is enabled. | string |  |
| UserPrompt | User prompt for the AI. | string |  |
| Content | Content if already present. | string |  |
| PreviousResponseId | The previous response identifier for conversation continuation. | string |  |
| StoreToken | Token for the store containing attached files. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "Feature": 0,
  "LocationId": 1,
  "LocationType": 1,
  "PlanId": 2,
  "SystemPrompt": "sample string 3",
  "UserPrompt": "sample string 4",
  "Content": "sample string 5",
  "PreviousResponseId": "sample string 6",
  "StoreToken": "sample string 7"
}
```
</details>

**Response:** `?` — Text and response reference.


### `DELETE restapi/personal/ai/stores/{storeToken}/files/{fileId}/v1`

Deletes a file from a store.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| storeToken | The store token. | string | Required |
| fileId | The file ID to delete. | string | Required |

**Response:** `?`


### `POST restapi/personal/ai/stores/{storeToken}/files/v1`

Creates a store from uploaded files or adds files to an existing store.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| storeToken | Optional store token. If provided, adds to existing store; otherwise creates new one. | string |  |

**Response:** `?` — Response containing the store token and file information.


### `POST restapi/personal/ai/stores/{storeToken}/resources/v1`

Creates a store from itslearning resources or adds resources to an existing store.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| storeToken | Optional store token. If provided, adds to existing store; otherwise creates new one. | string |  |

**Body:** List of element IDs to add to the store. Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — Response containing the store token and file information.


### `GET restapi/personal/ai/stores/{storeToken}/status/v1`

Gets the status of all files in a store.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| storeToken | The store token. | string | Required |

**Response:** `?` — Response containing file statuses.


### `POST restapi/personal/ai/stores/files/v1?storeToken={storeToken}`

Creates a store from uploaded files or adds files to an existing store.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| storeToken | Optional store token. If provided, adds to existing store; otherwise creates new one. | string |  |

**Response:** `?` — Response containing the store token and file information.


### `POST restapi/personal/ai/stores/resources/v1?storeToken={storeToken}`

Creates a store from itslearning resources or adds resources to an existing store.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| storeToken | Optional store token. If provided, adds to existing store; otherwise creates new one. | string |  |

**Body:** List of element IDs to add to the store. Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — Response containing the store token and file information.

