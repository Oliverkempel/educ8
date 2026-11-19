# NativeApp

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Some utility methods for native apps

## Endpoints

- `GET restapi/nativeapp/versioncheck/{applicationKey}/v1` — Check if the current version for the application is ok or not. This requires that the native app has registere…

---

### `GET restapi/nativeapp/versioncheck/{applicationKey}/v1?currentVersion={currentVersion}&operatingSystem={operatingSystem}`

Check if the current version for the application is ok or not. This requires that the native app has registered minimum required version and latest version.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| applicationKey | Application key for the native app (same as client_id used for oauth2) | string | Required |
| currentVersion | Current version of the native app containing only . and numbers. For example: 2.3.44.114 | string | Required |
| operatingSystem | A string for the operating system the native app runs on. For example: Android | string | Required |

**Response:** `Itslearning.Common.RestApi.Entities.VersionResponse`

<details><summary>Response JSON sample</summary>

```json
"Unknown"
```
</details>

