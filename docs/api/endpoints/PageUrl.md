# PageUrl

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to get URLs from the itslearning.

## Endpoints

- `GET restapi/pageurl/ckeditorconfigurls` — Get the urls to configure the CkEditor.
- `GET restapi/pageurl/courseelement` — Get 'course element' page url.
- `GET restapi/pageurl/services/v1` — Get the base Url for different customer specific services.
- `GET restapi/pageurl/treelink` — Get the base Url to build a tree link navigation urls.
- `GET restapi/pageurl/viewLearningToolElement` — Get 'view learning tool element' page url.

---

### `GET restapi/pageurl/ckeditorconfigurls`

Get the urls to configure the CkEditor.

**Response:** `?`

<details><summary>Response JSON sample</summary>

```json
{
  "sample string 1": "sample string 2",
  "sample string 3": "sample string 4"
}
```
</details>


### `GET restapi/pageurl/courseelement?courseId={courseId}&elementId={elementId}&elementType={elementType}`

Get 'course element' page url.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId |  | integer | Required |
| elementId |  | integer | Required |
| elementType |  | integer |  |

**Response:** `string`

<details><summary>Response JSON sample</summary>

```json
"sample string"
```
</details>


### `GET restapi/pageurl/services/v1`

Get the base Url for different customer specific services.

**Response:** `?`

<details><summary>Response JSON sample</summary>

```json
{
  "sample string 1": "sample string 2",
  "sample string 3": "sample string 4"
}
```
</details>


### `GET restapi/pageurl/treelink`

Get the base Url to build a tree link navigation urls.

**Response:** `string`

<details><summary>Response JSON sample</summary>

```json
"sample string"
```
</details>


### `GET restapi/pageurl/viewLearningToolElement?elementId={elementId}`

Get 'view learning tool element' page url.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| elementId |  | integer | Required |

**Response:** `string`

<details><summary>Response JSON sample</summary>

```json
"sample string"
```
</details>

