# YourFiles

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Get attachments for the current user

## Endpoints

- `GET restapi/personal/yourfiles/attachments/v1` — Get attachments for the current user
- `GET restapi/personal/yourfiles/courses/v1` — Get courses for the current user
- `GET restapi/personal/yourfiles/files/illegal/v1` — Illegal extensions to upload
- `POST restapi/personal/yourfiles/files/restore/v1` — Undo delete files in your private files area
- `POST restapi/personal/yourfiles/files/temp/v1` — Upload files to a file area
- `GET restapi/personal/yourfiles/files/usedspace/v1` — Get used space in bytes
- `PUT restapi/personal/yourfiles/files/v1` — Moves files in your files
- `GET restapi/personal/yourfiles/files/zip/{zipJobId}/v1` — Gets zip job by id.
- `POST restapi/personal/yourfiles/files/zip/v1` — Start zipping files in your files area
- `GET restapi/personal/yourfiles/files/zip/v1` — Gets the latest zip job.
- `DELETE restapi/personal/yourfiles/files/zip/v1` — Deletes zip jobs.
- `GET restapi/personal/yourfiles/folders/{folderId}/v1` — Get public and private files for the current user
- `POST restapi/personal/yourfiles/folders/restore/v1` — Undo delete folders in your private files area
- `PUT restapi/personal/yourfiles/folders/v1` — Moves folders in your private files
- `PUT restapi/personal/yourfiles/private/files/{fileId}/quota/v1` — Updates used quota for your files
- `PUT restapi/personal/yourfiles/private/files/{fileId}/v1` — Renames or/and moves a file in your private files
- `GET restapi/personal/yourfiles/private/files/{fileId}/v1` — Get download url for a file. Will be valid for 1 minute.
- `DELETE restapi/personal/yourfiles/private/files/v1` — Delete files in your private files area
- `POST restapi/personal/yourfiles/private/files/v1` — Copy files to a different location
- `POST restapi/personal/yourfiles/private/folders/{folderId}/files/empty/v1` — Saves new private files for person.
- `POST restapi/personal/yourfiles/private/folders/{folderId}/files/v1` — Saves new private files for person.
- `PUT restapi/personal/yourfiles/private/folders/{folderId}/v1` — Renames/moves a folder in your private files
- `POST restapi/personal/yourfiles/private/folders/{folderId}/v1` — Add new folder in private files area
- `GET restapi/personal/yourfiles/private/folders/{folderId}/v1` — Get private files for the current user
- `POST restapi/personal/yourfiles/private/folders/v1` — Copy folders to a different location
- `DELETE restapi/personal/yourfiles/private/folders/v1` — Deletes a folder in private files area
- `PUT restapi/personal/yourfiles/public/files/{fileId}/v1` — Renames or/and moves a file in your public files
- `GET restapi/personal/yourfiles/public/files/{fileId}/v1` — Get download url for a file. Will be valid for 1 minute.
- `DELETE restapi/personal/yourfiles/public/files/v1` — Delete files in your public files area
- `POST restapi/personal/yourfiles/public/files/v1` — Copy files to a different location
- `POST restapi/personal/yourfiles/public/folders/{folderId}/files/v1` — Saves new public files for person.
- `PUT restapi/personal/yourfiles/public/folders/{folderId}/v1` — Renames a folder in your public files
- `POST restapi/personal/yourfiles/public/folders/{folderId}/v1` — Add new folder in public files area
- `GET restapi/personal/yourfiles/public/folders/{folderId}/v1` — Get public files for the current user
- `POST restapi/personal/yourfiles/public/folders/v1` — Copy folders to a different location
- `DELETE restapi/personal/yourfiles/public/folders/v1` — Deletes a folder in public files area

---

### `GET restapi/personal/yourfiles/attachments/v1?PageIndex={PageIndex}&PageSize={PageSize}&searchPhrase={searchPhrase}&sortField={sortField}&sortOrder={sortOrder}`

Get attachments for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| searchPhrase | Optional. Get only the attachments that matches the search phrase | string | Default value is |
| sortField | The sort field. FileName or Uploaded date | string | Default value is |
| sortOrder | The sort order. Ascending or Descending | string | Default value is |

**Response:** `EntityList<YourFiles.MessageAttachmentModel>` — EntityList of message attachments

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of YourFiles.MessageAttachmentModel |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "InstantMessageId": 1,
      "InstantMessageThreadId": 2,
      "FileName": "sample string 3",
      "FileType": "sample string 4",
      "UploadedDateString": "sample string 5",
      "UploadedDate": "2026-11-18T16:33:46Z",
      "Icon": "sample string 7"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/yourfiles/courses/v1?PageIndex={PageIndex}&PageSize={PageSize}&searchPhrase={searchPhrase}&sortField={sortField}&sortOrder={sortOrder}`

Get courses for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| searchPhrase | Optional. Get only the courses that matches the search phrase | string | Default value is |
| sortField | The sort field. Title or LastUpdated | string | Default value is |
| sortOrder | The sort order. Ascending or Descending | string | Default value is |

**Response:** `EntityList<YourFiles.CourseModel>` — EntityList of courses

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of YourFiles.CourseModel |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CourseId": 1,
      "Title": "sample string 2",
      "LastUpdated": "2026-11-18T16:31:46Z",
      "LastUpdatedString": "sample string 4",
      "Teachers": [
        "sample string 1"
      ],
      "IsActive": true,
      "IsDeleted": true,
      "NumberOfElements": 7,
      "CourseStatus": "sample string 8",
      "ColorSchemaClass": "sample string 9"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/yourfiles/files/illegal/v1`

Illegal extensions to upload

**Response:** `Collection of string` — List of illegal file extensions

<details><summary>Response JSON sample</summary>

```json
[
  "sample string 1"
]
```
</details>


### `POST restapi/personal/yourfiles/files/restore/v1`

Undo delete files in your private files area

**Body:** A list of files to restore Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:33:46Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "635f0cca-d373-4cbc-a6b5-d75bf8b04ecc",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:33:46Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "777282ef-d0e9-42be-a563-0e44dacdb18c",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `POST restapi/personal/yourfiles/files/temp/v1`

Upload files to a file area

**Response:** `YourFiles.UploadedFileModel` — List of uploaded files

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The file identifier | string |  |
| Name | The name of the file | string |  |
| MimeType | The MIME type of the file | string |  |
| SignedFileRepoId | The signed file repository identifier | string |  |
| Signature | The signature for the file | string |  |
| Error | The error message if any error occurred during the upload to file repo | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Id": "sample string 1",
  "Name": "sample string 2",
  "MimeType": "sample string 3",
  "SignedFileRepoId": "sample string 4",
  "Signature": "sample string 5",
  "Error": "sample string 6"
}
```
</details>


### `GET restapi/personal/yourfiles/files/usedspace/v1?folderIds[0]={folderIds[0]}&folderIds[1]={folderIds[1]}`

Get used space in bytes

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderIds |  | Collection of integer |  |

**Response:** `integer, 64 bit`

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `PUT restapi/personal/yourfiles/files/v1`

Moves files in your files

**Body** (YourFiles.MoveRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be moved to | integer |  |
| Ids | The list of identifiers of the files or folders to be moved | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Ids": [
    1
  ]
}
```
</details>

**Response:** `?` — The updated file model

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:33:14Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "84e267bc-85fd-4f25-beac-9b76aac531f8",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:33:14Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "d6d61f06-cd22-4e30-9fb6-2c53da277540",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `GET restapi/personal/yourfiles/files/zip/{zipJobId}/v1`

Gets zip job by id.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| zipJobId |  | integer | Required |

**Response:** `?`


### `POST restapi/personal/yourfiles/files/zip/v1`

Start zipping files in your files area

**Body** (YourFiles.ZipRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileIds | The file identifiers of the files to be included in the zip archive. | Collection of integer |  |
| FolderIds | The folder identifiers of the folders to be included in the zip archive. | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "FileIds": [
    1
  ],
  "FolderIds": [
    1
  ]
}
```
</details>

**Response:** `YourFiles.ZipFileJobModel` — The zip file job

| Name | Description | Type | Additional information |
|---|---|---|---|
| JobId | The job identifier | integer |  |
| FileRepoId | The file repository identifier for the zip file | globally unique identifier |  |
| ZipFileName | The name of the zip file | string |  |
| Created | When the job was created | date |  |
| Status | Status for the job | string |  |
| DownloadUrl | The url to download the file if status is completed | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "JobId": 1,
  "FileRepoId": "fcb6e95b-b8ee-4b6c-8c41-f8dd142ad90a",
  "ZipFileName": "sample string 3",
  "Created": "2026-11-18T16:32:22Z",
  "Status": "sample string 5",
  "DownloadUrl": "sample string 6"
}
```
</details>


### `GET restapi/personal/yourfiles/files/zip/v1`

Gets the latest zip job.

**Response:** `YourFiles.ZipFileJobModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| JobId | The job identifier | integer |  |
| FileRepoId | The file repository identifier for the zip file | globally unique identifier |  |
| ZipFileName | The name of the zip file | string |  |
| Created | When the job was created | date |  |
| Status | Status for the job | string |  |
| DownloadUrl | The url to download the file if status is completed | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "JobId": 1,
  "FileRepoId": "0d485b1f-cc91-4344-b32f-03d01636bb56",
  "ZipFileName": "sample string 3",
  "Created": "2026-11-18T16:32:22Z",
  "Status": "sample string 5",
  "DownloadUrl": "sample string 6"
}
```
</details>


### `DELETE restapi/personal/yourfiles/files/zip/v1`

Deletes zip jobs.

**Response:** `?`


### `GET restapi/personal/yourfiles/folders/{folderId}/v1`

Get public and private files for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId |  | integer | Default value is 0 |

**Response:** `?` — Folder with children

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:32:22Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "0339419b-10a0-46d9-832c-1c61b7b79946",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:32:22Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "27cfe82b-1296-48f3-b94d-1e4df20e0105",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `POST restapi/personal/yourfiles/folders/restore/v1`

Undo delete folders in your private files area

**Body:** A list of folders to restore Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:33:46Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "57dd4e12-d4c7-4446-9156-9ff3dbfecde8",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:33:46Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "1cafb86e-923d-41e8-95a1-ea0d98d84f44",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `PUT restapi/personal/yourfiles/folders/v1`

Moves folders in your private files

**Body** (YourFiles.MoveRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be moved to | integer |  |
| Ids | The list of identifiers of the files or folders to be moved | Collection of integer |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Ids": [
    1
  ]
}
```
</details>

**Response:** `?` — The updated file model

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:30:06Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "a329d1cb-5b32-4c1d-9ed4-cec296c59bad",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:30:06Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "e6366fb8-4d56-4083-9838-7039c7cef1f4",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `PUT restapi/personal/yourfiles/private/files/{fileId}/quota/v1`

Updates used quota for your files

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileId | The file identifier | integer | Required |

**Response:** `YourFiles.FileModel` — The updated file model

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:35:31Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "a9c528d2-c3a0-417e-b05c-77adf0a1ffbb",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:35:31Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "dd208863-1e05-4175-bba6-f89bf115d606",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `PUT restapi/personal/yourfiles/private/files/{fileId}/v1`

Renames or/and moves a file in your private files

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileId | The file identifier | integer | Required |

**Body** (YourFiles.UpdateRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier where the file or folder is located | integer |  |
| Name | The new name of the file or folder to be updated | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Name": "sample string 2"
}
```
</details>

**Response:** `YourFiles.FileModel` — The updated file model

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:30:06Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "00ad3fd3-d6df-4b19-97cc-291899951bdb",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:30:06Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "d665a466-c799-44af-af81-6a87e97e1d01",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `GET restapi/personal/yourfiles/private/files/{fileId}/v1?attachment={attachment}`

Get download url for a file. Will be valid for 1 minute.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileId | The file identifier | integer | Required |
| attachment | If it is a view or download url to get | boolean | Default value is True |

**Response:** `string` — The file

<details><summary>Response JSON sample</summary>

```json
"sample string"
```
</details>


### `DELETE restapi/personal/yourfiles/private/files/v1`

Delete files in your private files area

**Body:** A list of files to delete Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId |  | integer |  |
| Name |  | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "FileId": 1,
    "Name": "sample string 2"
  }
]
```
</details>


### `POST restapi/personal/yourfiles/private/files/v1`

Copy files to a different location

**Body** (YourFiles.CopyRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be copied to | integer |  |
| Ids | The list of identifiers of the files or folders to be copied | Collection of integer |  |
| DuplicateBehaviour | How to handle duplicates in destination folder | YourFiles.DuplicateBehaviour |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Ids": [
    1
  ],
  "DuplicateBehaviour": 1
}
```
</details>

**Response:** `?` — The file model for destination folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:32:42Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "3117d26f-deee-439f-9572-148f6223acbd",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:32:42Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "c3dcf906-9bf7-4d58-8a9e-690fdcf50024",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `POST restapi/personal/yourfiles/private/folders/{folderId}/files/empty/v1`

Saves new private files for person.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier to store the files | integer | Required |

**Body:** File name string

<details><summary>Request JSON sample</summary>

```json
"sample string"
```
</details>

**Response:** `YourFiles.FileModel` — The new file

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:32:42Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "932361a2-d37e-47a1-980e-4fc320519a6d",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:32:42Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "b5b23be3-e79c-4505-8aee-9ced37fd3599",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `POST restapi/personal/yourfiles/private/folders/{folderId}/files/v1`

Saves new private files for person.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier to store the files | integer | Required |

**Body** (YourFiles.UploadedFileModel)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The file identifier | string |  |
| Name | The name of the file | string |  |
| MimeType | The MIME type of the file | string |  |
| SignedFileRepoId | The signed file repository identifier | string |  |
| Signature | The signature for the file | string |  |
| Error | The error message if any error occurred during the upload to file repo | string |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "Id": "sample string 1",
    "Name": "sample string 2",
    "MimeType": "sample string 3",
    "SignedFileRepoId": "sample string 4",
    "Signature": "sample string 5",
    "Error": "sample string 6"
  }
]
```
</details>

**Response:** `?` — List of files that are saved

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:33:14Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "816dea90-bcc6-414f-aeae-aa0da7c0a6e8",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:33:14Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "fc152e83-4840-4489-8e98-9ed988486cc4",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `PUT restapi/personal/yourfiles/private/folders/{folderId}/v1`

Renames/moves a folder in your private files

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier to rename | integer | Required |

**Body** (YourFiles.UpdateRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier where the file or folder is located | integer |  |
| Name | The new name of the file or folder to be updated | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Name": "sample string 2"
}
```
</details>

**Response:** `YourFiles.FileModel` — The file model for the renamed folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:31:28Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "1eba1335-bce6-4379-acac-605a3043839e",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:31:28Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "eb38a7c4-ae55-4722-9768-04b7f8d68d48",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `POST restapi/personal/yourfiles/private/folders/{folderId}/v1`

Add new folder in private files area

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The parent folder identifier | integer | Required |

**Body:** The name of the folder string

<details><summary>Request JSON sample</summary>

```json
"sample string"
```
</details>

**Response:** `YourFiles.FileModel` — The newly created folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:35:31Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "8da0485e-4879-4421-9d9e-005b6bb30b3e",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:35:31Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "36633c5c-d118-4fed-baf0-317bd92ec188",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `GET restapi/personal/yourfiles/private/folders/{folderId}/v1?searchPhrase={searchPhrase}`

Get private files for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier | integer | Default value is 0 |
| searchPhrase | Optional. Get only the files that matches the search phrase | string | Default value is |

**Response:** `YourFiles.FileModel` — Folder with children

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:31:46Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "e2afc471-cd82-4545-bace-5bac2a806583",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:31:46Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "a3f68217-8358-455d-93fb-0b4bc1f2e1d2",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `POST restapi/personal/yourfiles/private/folders/v1`

Copy folders to a different location

**Body** (YourFiles.CopyRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be copied to | integer |  |
| Ids | The list of identifiers of the files or folders to be copied | Collection of integer |  |
| DuplicateBehaviour | How to handle duplicates in destination folder | YourFiles.DuplicateBehaviour |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Ids": [
    1
  ],
  "DuplicateBehaviour": 1
}
```
</details>

**Response:** `YourFiles.FileModel` — The file model for destination folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:30:06Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "c3a22c57-e183-41fb-8347-3632a1d58606",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:30:06Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "a630889d-aecc-4c20-8da0-92c64aa27538",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `DELETE restapi/personal/yourfiles/private/folders/v1`

Deletes a folder in private files area

**Body:** The folders to delete Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`


### `PUT restapi/personal/yourfiles/public/files/{fileId}/v1`

Renames or/and moves a file in your public files

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileId | The file identifier | integer | Required |

**Body** (YourFiles.UpdateRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier where the file or folder is located | integer |  |
| Name | The new name of the file or folder to be updated | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Name": "sample string 2"
}
```
</details>

**Response:** `YourFiles.FileModel` — The updated file model

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:33:14Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "8bafd01f-b4be-4ee8-913f-7c27e105d93b",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:33:14Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "eae41008-f1ad-407c-987c-2f2d3b7065ce",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `GET restapi/personal/yourfiles/public/files/{fileId}/v1?attachment={attachment}`

Get download url for a file. Will be valid for 1 minute.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileId | The file identifier | integer | Required |
| attachment | If it is a view or download url to get | boolean | Default value is True |

**Response:** `string` — The file

<details><summary>Response JSON sample</summary>

```json
"sample string"
```
</details>


### `DELETE restapi/personal/yourfiles/public/files/v1`

Delete files in your public files area

**Body:** A list of files to delete Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId |  | integer |  |
| Name |  | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "FileId": 1,
    "Name": "sample string 2"
  }
]
```
</details>


### `POST restapi/personal/yourfiles/public/files/v1`

Copy files to a different location

**Body** (YourFiles.CopyRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be copied to | integer |  |
| Ids | The list of identifiers of the files or folders to be copied | Collection of integer |  |
| DuplicateBehaviour | How to handle duplicates in destination folder | YourFiles.DuplicateBehaviour |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Ids": [
    1
  ],
  "DuplicateBehaviour": 1
}
```
</details>

**Response:** `?` — The file model for destination folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:33:14Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "68363c06-0f1a-4fa2-9b4d-302da450d6e5",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:33:14Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "29591de6-da1a-4c84-9ad1-5b4e30519294",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `POST restapi/personal/yourfiles/public/folders/{folderId}/files/v1`

Saves new public files for person.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier to store the files | integer | Required |

**Body** (YourFiles.UploadedFileModel)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The file identifier | string |  |
| Name | The name of the file | string |  |
| MimeType | The MIME type of the file | string |  |
| SignedFileRepoId | The signed file repository identifier | string |  |
| Signature | The signature for the file | string |  |
| Error | The error message if any error occurred during the upload to file repo | string |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "Id": "sample string 1",
    "Name": "sample string 2",
    "MimeType": "sample string 3",
    "SignedFileRepoId": "sample string 4",
    "Signature": "sample string 5",
    "Error": "sample string 6"
  }
]
```
</details>

**Response:** `?` — List of files that are saved

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Name": "sample string 1",
    "LastModified": "2026-11-18T16:32:42Z",
    "LastModifiedString": "sample string 3",
    "FileType": "sample string 4",
    "FileSize": 5,
    "FileId": 6,
    "FileRepoId": "df549429-e362-4b6b-ab45-44dc769fc37d",
    "SignedFileRepoId": "sample string 8",
    "IsFolder": true,
    "VirtualPath": "sample string 10",
    "Icon": "sample string 11",
    "FolderId": 12,
    "ParentFolderId": 1,
    "NameWithoutExtension": "sample string 13",
    "Extension": "sample string 14",
    "Children": [
      {
        "Name": "sample string 1",
        "LastModified": "2026-11-18T16:32:42Z",
        "LastModifiedString": "sample string 3",
        "FileType": "sample string 4",
        "FileSize": 5,
        "FileId": 6,
        "FileRepoId": "0d9dcd04-c941-4eb9-9de4-b8b041a15aea",
        "SignedFileRepoId": "sample string 8",
        "IsFolder": true,
        "VirtualPath": "sample string 10",
        "Icon": "sample string 11",
        "FolderId": 12,
        "ParentFolderId": 1,
        "NameWithoutExtension": "sample string 13",
        "Extension": "sample string 14",
        "Children": null,
        "IsEditable": true,
        "IsViewable": true,
        "FolderType": "sample string 17",
        "PublicLink": "sample string 18"
      }
    ],
    "IsEditable": true,
    "IsViewable": true,
    "FolderType": "sample string 17",
    "PublicLink": "sample string 18"
  }
]
```
</details>


### `PUT restapi/personal/yourfiles/public/folders/{folderId}/v1`

Renames a folder in your public files

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier to rename | integer | Required |

**Body** (YourFiles.UpdateRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier where the file or folder is located | integer |  |
| Name | The new name of the file or folder to be updated | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Name": "sample string 2"
}
```
</details>

**Response:** `YourFiles.FileModel` — The file model for the renamed folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:34:02Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "7619ac84-460b-4f49-b7bf-52277928fa55",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:34:02Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "d5d3c677-8e10-4cc2-b6e4-7b0d36b5d58c",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `POST restapi/personal/yourfiles/public/folders/{folderId}/v1`

Add new folder in public files area

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The parent folder identifier | integer | Required |

**Body:** The name of the folder string

<details><summary>Request JSON sample</summary>

```json
"sample string"
```
</details>

**Response:** `YourFiles.FileModel` — The newly created folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:34:02Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "48c5f98f-bb96-4745-a4a2-8769cfc0e9c8",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:34:02Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "24297886-aff0-4e2f-8e0f-b57b3fab9b64",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `GET restapi/personal/yourfiles/public/folders/{folderId}/v1?searchPhrase={searchPhrase}`

Get public files for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| folderId | The folder identifier | integer | Default value is 0 |
| searchPhrase | Optional. Get only the files that matches the search phrase | string | Default value is |

**Response:** `YourFiles.FileModel` — Folder with children

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:35:34Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "286f9757-1909-4528-be71-e10079e21cb1",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:35:34Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "ff811be0-8b3d-4a28-af66-dcb5bd9e6add",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `POST restapi/personal/yourfiles/public/folders/v1`

Copy folders to a different location

**Body** (YourFiles.CopyRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be copied to | integer |  |
| Ids | The list of identifiers of the files or folders to be copied | Collection of integer |  |
| DuplicateBehaviour | How to handle duplicates in destination folder | YourFiles.DuplicateBehaviour |  |

<details><summary>Request JSON sample</summary>

```json
{
  "ParentFolderId": 1,
  "Ids": [
    1
  ],
  "DuplicateBehaviour": 1
}
```
</details>

**Response:** `YourFiles.FileModel` — The file model for destination folder

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "LastModified": "2026-11-18T16:34:59Z",
  "LastModifiedString": "sample string 3",
  "FileType": "sample string 4",
  "FileSize": 5,
  "FileId": 6,
  "FileRepoId": "1f823a59-f19c-421d-a581-af3f77a19058",
  "SignedFileRepoId": "sample string 8",
  "IsFolder": true,
  "VirtualPath": "sample string 10",
  "Icon": "sample string 11",
  "FolderId": 12,
  "ParentFolderId": 1,
  "NameWithoutExtension": "sample string 13",
  "Extension": "sample string 14",
  "Children": [
    {
      "Name": "sample string 1",
      "LastModified": "2026-11-18T16:34:59Z",
      "LastModifiedString": "sample string 3",
      "FileType": "sample string 4",
      "FileSize": 5,
      "FileId": 6,
      "FileRepoId": "acf14d55-2173-4098-93aa-e621781c6fdc",
      "SignedFileRepoId": "sample string 8",
      "IsFolder": true,
      "VirtualPath": "sample string 10",
      "Icon": "sample string 11",
      "FolderId": 12,
      "ParentFolderId": 1,
      "NameWithoutExtension": "sample string 13",
      "Extension": "sample string 14",
      "Children": null,
      "IsEditable": true,
      "IsViewable": true,
      "FolderType": "sample string 17",
      "PublicLink": "sample string 18"
    }
  ],
  "IsEditable": true,
  "IsViewable": true,
  "FolderType": "sample string 17",
  "PublicLink": "sample string 18"
}
```
</details>


### `DELETE restapi/personal/yourfiles/public/folders/v1`

Deletes a folder in public files area

**Body:** The folders to delete Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?`

