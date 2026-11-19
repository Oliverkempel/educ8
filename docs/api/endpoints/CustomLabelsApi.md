# CustomLabelsApi

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Controller to handle api calls from the custom labels page.

## Endpoints

- `PUT restapi/administration/customlabels/{id}/v1` — Edit custom label
- `DELETE restapi/administration/customlabels/{id}/v1` — Delete custom label.
- `GET restapi/administration/customlabels/v1` — Get custom labels
- `POST restapi/administration/customlabels/v1` — Create new custom label

---

### `PUT restapi/administration/customlabels/{id}/v1`

Edit custom label

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| id | The custom label identifier. | integer | Required |

**Body** (Itsolutions.Itslearning.Web.Areas.Administration.CustomLabels.Models.AddEditCustomLabelRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Identifier, if is 0 that means it's add action | integer |  |
| Name | Custom label name | string |  |
| Description | Custom label description | string |  |
| Color | Custom label color name, chosen from the UI | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "id": 1,
  "name": "sample string 2",
  "description": "sample string 3",
  "color": "sample string 4"
}
```
</details>

**Response:** `?`


### `DELETE restapi/administration/customlabels/{id}/v1`

Delete custom label.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| id | The custom label identifier to delete. | integer | Required |

**Response:** `?`


### `GET restapi/administration/customlabels/v1`

Get custom labels

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | integer |  |
| Name |  | string |  |
| Description |  | string |  |
| Color |  | string |  |
| LabelsSummary |  | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "id": 1,
    "name": "sample string 2",
    "description": "sample string 3",
    "color": "sample string 4",
    "labelsSummary": "sample string 5"
  }
]
```
</details>


### `POST restapi/administration/customlabels/v1`

Create new custom label

**Body** (Itsolutions.Itslearning.Web.Areas.Administration.CustomLabels.Models.AddEditCustomLabelRequest)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Identifier, if is 0 that means it's add action | integer |  |
| Name | Custom label name | string |  |
| Description | Custom label description | string |  |
| Color | Custom label color name, chosen from the UI | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "id": 1,
  "name": "sample string 2",
  "description": "sample string 3",
  "color": "sample string 4"
}
```
</details>

**Response:** `?`

