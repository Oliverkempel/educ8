# FileUpload

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Controller to handle file uploads, for oauth2 the scope is required.

## Endpoints

- `POST restapi/personal/fileupload/temp/v1` — Upload a file to temporary storage. The file must be included in the post so we can extract it from HttpContex…

---

### `POST restapi/personal/fileupload/temp/v1`

Upload a file to temporary storage. The file must be included in the post so we can extract it from HttpContext.Current.Request.Files (multipart/form-data). The file can only be used once, and will be deleted automatically after 24 hours (depending on FileRepo's config for temp file).

**Response:** `Personal.FileUpload.UploadedTempFile` — An object that can be used in e.g. assignment answers.

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileRepoId | File repo ID. | string |  |
| SignedFileRepoId | The signature of the file. Useful when file is still saved as a temporary file. | string |  |
| Name | File name. | string |  |
| MimeType | File MIME type. | string |  |
| SourceType | File source type which defines the way file was uploaded. E.g. via local upload or cloud integration. | Itslearning.FileRepo.Common.Models.SourceType |  |
| IsTemplate | Gets or sets the file should be a template. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "FileRepoId": "sample string 1",
  "SignedFileRepoId": "sample string 2",
  "Name": "sample string 3",
  "MimeType": "sample string 4",
  "SourceType": 1,
  "IsTemplate": true
}
```
</details>

