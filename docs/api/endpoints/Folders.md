# Folders

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Update a folder.

## Endpoints

- `GET restapi/personal/elements/folders/{folderId}/participants/v1` — Update a folder.
- `GET restapi/personal/elements/folders/{folderId}/v1` — Get a folder by folderId.
- `PUT restapi/personal/elements/folders/{folderId}/v1` — Update a folder.
- `POST restapi/personal/elements/folders/{folderId}/v1` — Create a folder.

---

### `GET restapi/personal/elements/folders/{folderId}/participants/v1`

Update a folder.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder to get participants | integer | Required |

**Response:** `Folder.ParticipantsResponse` — ParticipantResponse containing the participants data

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | List of participants. | Collection of Folder.ParticipantModel |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Participants": [
    {
      "PersonId": 1,
      "FullName": "sample string 2",
      "ExtraInformation": "sample string 3",
      "GroupId": 1,
      "GroupName": "sample string 4",
      "PictureUrl": "sample string 5"
    }
  ]
}
```
</details>


### `GET restapi/personal/elements/folders/{folderId}/v1`

Get a folder by folderId.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The unique identifier of the folder to retrieve | integer | Required |

**Response:** `Folder.FolderResponse` — FolderResponse containing the folder data

| Name | Description | Type | Additional information |
|---|---|---|---|
| FolderId | The folder identifier. Same as elementId | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "FolderId": 1
}
```
</details>


### `PUT restapi/personal/elements/folders/{folderId}/v1`

Update a folder.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier to update | integer | Required |

**Body** (Folder.FolderRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier. | integer |  |
| Title | The folder title | string |  |
| Description | The folder description | string |  |
| Visibility | Indicates the activity setting for the folder . | string |  |
| VisibleFrom | When the folder is visible from | date |  |
| VisibleTo | When the folder is visible to | date |  |
| Deadline | The folder deadline. After this date the folder is no longer editable | date |  |
| AvailableTools | The list of available tools for the folder | Collection of string |  |
| AssignTo | Who the folder is assigned to. If null or empty all participants are assigned | Folder.AssignToModel |  |
| ReplaceVisibilityInSubElements | Will replace this folder visibility settings in all sub elements. | boolean |  |
| PlanId | If present. The created element should be added to the plan with this id | integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Title": "sample string 2",
  "Description": "sample string 3",
  "Visibility": "sample string 4",
  "VisibleFrom": "2026-11-18T16:32:22Z",
  "VisibleTo": "2026-11-18T16:32:22Z",
  "Deadline": "2026-11-18T16:32:22Z",
  "AvailableTools": [
    "sample string 1"
  ],
  "AssignTo": {
    "ParticipantIds": [
      1
    ],
    "GroupIds": [
      1
    ]
  },
  "ReplaceVisibilityInSubElements": true,
  "PlanId": 1
}
```
</details>

**Response:** `Folder.FolderResponse` — FolderResponse containing the new folder data

| Name | Description | Type | Additional information |
|---|---|---|---|
| FolderId | The folder identifier. Same as elementId | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "FolderId": 1
}
```
</details>


### `POST restapi/personal/elements/folders/{folderId}/v1`

Create a folder.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId |  | string |  |

**Body** (Folder.FolderRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier. | integer |  |
| Title | The folder title | string |  |
| Description | The folder description | string |  |
| Visibility | Indicates the activity setting for the folder . | string |  |
| VisibleFrom | When the folder is visible from | date |  |
| VisibleTo | When the folder is visible to | date |  |
| Deadline | The folder deadline. After this date the folder is no longer editable | date |  |
| AvailableTools | The list of available tools for the folder | Collection of string |  |
| AssignTo | Who the folder is assigned to. If null or empty all participants are assigned | Folder.AssignToModel |  |
| ReplaceVisibilityInSubElements | Will replace this folder visibility settings in all sub elements. | boolean |  |
| PlanId | If present. The created element should be added to the plan with this id | integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Title": "sample string 2",
  "Description": "sample string 3",
  "Visibility": "sample string 4",
  "VisibleFrom": "2026-11-18T16:35:31Z",
  "VisibleTo": "2026-11-18T16:35:31Z",
  "Deadline": "2026-11-18T16:35:31Z",
  "AvailableTools": [
    "sample string 1"
  ],
  "AssignTo": {
    "ParticipantIds": [
      1
    ],
    "GroupIds": [
      1
    ]
  },
  "ReplaceVisibilityInSubElements": true,
  "PlanId": 1
}
```
</details>

**Response:** `Folder.FolderResponse` — FolderResponse containing the new folder data

| Name | Description | Type | Additional information |
|---|---|---|---|
| FolderId | The folder identifier. Same as elementId | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "FolderId": 1
}
```
</details>

