# MediaRecorder

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to Video Recording API.

## Endpoints

- `GET RestApi/personal/mediarecorder/native/job/{jobId}/recording/v1` — Polls the transcoding job for a native recording used in the file uploader flow. This endpoint is read-only; o…
- `POST RestApi/personal/mediarecorder/native/job/recording/finalize/v1` — Finalizes a completed native recording by uploading it to the file repository and then cleaning up the media r…
- `POST RestApi/personal/mediarecorder/native/upload/finish/v1` — Finalizes a native multipart upload and schedules background processing to persist the recording.
- `POST RestApi/personal/mediarecorder/native/upload/start/v1` — Starts a native multipart upload session and returns presigned URLs for each chunk.

---

### `GET RestApi/personal/mediarecorder/native/job/{jobId}/recording/v1?signature={signature}`

Polls the transcoding job for a native recording used in the file uploader flow. This endpoint is read-only; once it reports ready, the client must explicitly finalize the recording to upload it to the file repository and clean up the media recorder job.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| jobId | Identifier of the transcoding job returned when finishing the upload. | string | Required |
| signature | User-scoped signature for . | string | Required |

**Response:** `?`


### `POST RestApi/personal/mediarecorder/native/job/recording/finalize/v1`

Finalizes a completed native recording by uploading it to the file repository and then cleaning up the media recorder job. This endpoint has side effects and must only be called after polling reports that the recording is ready.

**Body** (Personal.MediaRecorderController+NativeRecordingFinalizeRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| jobId | Identifier of the transcoding job returned from finish upload. | string |  |
| signature | User-scoped signature returned together with . | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "jobId": "sample string 1",
  "signature": "sample string 2"
}
```
</details>

**Response:** `?`


### `POST RestApi/personal/mediarecorder/native/upload/finish/v1`

Finalizes a native multipart upload and schedules background processing to persist the recording.

**Body** (Personal.MediaRecorderController+NativeUploadFinishRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| recordingId | Identifier of the recording created when the upload started. | string |  |
| uploadId | Identifier of the multipart upload session being completed. | string |  |
| objectKey | Storage object key for the uploaded media. | string |  |
| recordingName | User-provided name for the saved recording. | string |  |
| recorderType | Recorder implementation used to create the media. | string |  |
| isVideoRecording | Indicates whether the uploaded recording contains video. | boolean |  |
| usedInFileUploader | Indicates whether the recorder is used inside the file uploader flow. | boolean |  |
| parts | Uploaded multipart chunks and their returned entity tags. | Collection of Personal.MediaRecorderController+NativeUploadPart |  |

<details><summary>Request JSON sample</summary>

```json
{
  "recordingId": "sample string 1",
  "uploadId": "sample string 2",
  "objectKey": "sample string 3",
  "recordingName": "sample string 4",
  "recorderType": "sample string 5",
  "isVideoRecording": true,
  "usedInFileUploader": true,
  "parts": [
    {
      "partNumber": 1,
      "etag": "sample string 2"
    }
  ]
}
```
</details>

**Response:** `?` — An HTTP action result containing either validation errors or a describing the outcome.


### `POST RestApi/personal/mediarecorder/native/upload/start/v1`

Starts a native multipart upload session and returns presigned URLs for each chunk.

**Body** (Personal.MediaRecorderController+NativeUploadStartRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileName | Original file name reported by the client. | string |  |
| contentType | MIME type of the recording being uploaded. | string |  |
| size | Total size of the recording in bytes, as declared by the client. | integer, 64 bit |  |
| chunkCount | Number of upload parts the client will send. | integer |  |
| recorderType | Recorder implementation used to create the media. | string |  |
| isVideoRecording | Indicates whether the recording contains video. | boolean |  |
| usedInFileUploader | Indicates whether the recorder is used inside the file uploader flow. | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "fileName": "sample string 1",
  "contentType": "sample string 2",
  "size": 3,
  "chunkCount": 4,
  "recorderType": "sample string 5",
  "isVideoRecording": true,
  "usedInFileUploader": true
}
```
</details>

**Response:** `?` — An HTTP action result containing either validation errors or a .

