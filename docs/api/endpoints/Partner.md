# Partner

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** No documentation available.

## Endpoints

- `GET restapi/personal/learningrecordstore/partners/{partnerId}/organisations/v1` — 
- `POST restapi/personal/learningrecordstore/partners/{partnerId}/organisations/v1` — 
- `PUT restapi/personal/learningrecordstore/partners/{partnerId}/secrets/{secretId}/v1` — 
- `POST restapi/personal/learningrecordstore/partners/{partnerId}/secrets/v1` — 
- `GET restapi/personal/learningrecordstore/partners/{partnerId}/v1` — 
- `PUT restapi/personal/learningrecordstore/partners/{partnerId}/v1` — 
- `DELETE restapi/personal/learningrecordstore/partners/{partnerId}/v1` — 
- `POST restapi/personal/learningrecordstore/partners/v1` — 
- `GET restapi/personal/learningrecordstore/partners/v1` — 

---

### `GET restapi/personal/learningrecordstore/partners/{partnerId}/organisations/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |

**Response:** `LearningRecordStore.OrganisationTreeModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| ReadOnly | Gets or sets a value indicating whether the organisation is read-only. If it is a read only it cannot be selected. It is there to maintain the tree structure. | boolean |  |
| IsOrganisation | If this is a organisation level. Sometimes this is only a hierarchy node to group organisations. | boolean |  |
| Children | Gets or sets the child organisations. | Collection of LearningRecordStore.OrganisationTreeModel |  |
| Selected | If the organisation is associated with the partner or not. This is used when updating the organisations for a partner, to know which organisations to add or remove. | boolean |  |
| OrganisationId | Gets or sets the organisation ID. | integer |  |
| Name | Gets or sets the name of the organisation. | string |  |
| Type | Type of organisation. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "readOnly": true,
  "isOrganisation": true,
  "children": [
    {
      "readOnly": true,
      "isOrganisation": true,
      "children": null,
      "selected": true,
      "organisationId": 4,
      "name": "sample string 5",
      "type": "sample string 6"
    }
  ],
  "selected": true,
  "organisationId": 4,
  "name": "sample string 5",
  "type": "sample string 6"
}
```
</details>


### `POST restapi/personal/learningrecordstore/partners/{partnerId}/organisations/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |

**Body** (LearningRecordStore.OrganisationTreeModel)

| Name | Description | Type | Additional information |
|---|---|---|---|
| ReadOnly | Gets or sets a value indicating whether the organisation is read-only. If it is a read only it cannot be selected. It is there to maintain the tree structure. | boolean |  |
| IsOrganisation | If this is a organisation level. Sometimes this is only a hierarchy node to group organisations. | boolean |  |
| Children | Gets or sets the child organisations. | Collection of LearningRecordStore.OrganisationTreeModel |  |
| Selected | If the organisation is associated with the partner or not. This is used when updating the organisations for a partner, to know which organisations to add or remove. | boolean |  |
| OrganisationId | Gets or sets the organisation ID. | integer |  |
| Name | Gets or sets the name of the organisation. | string |  |
| Type | Type of organisation. | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "readOnly": true,
  "isOrganisation": true,
  "children": [
    {
      "readOnly": true,
      "isOrganisation": true,
      "children": null,
      "selected": true,
      "organisationId": 4,
      "name": "sample string 5",
      "type": "sample string 6"
    }
  ],
  "selected": true,
  "organisationId": 4,
  "name": "sample string 5",
  "type": "sample string 6"
}
```
</details>

**Response:** `?`


### `PUT restapi/personal/learningrecordstore/partners/{partnerId}/secrets/{secretId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |
| secretId |  | integer | Required |

**Response:** `LearningRecordStore.LearningRecordStoreSecretModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| SecretId | Gets or sets the secret ID. | integer |  |
| Secret | Gets or sets the client secret. | string |  |
| ActivationDate | Gets or sets the value of the activation date. | string |  |
| CreatedDate | Gets or sets the value of the creation date. | string |  |
| SecretStatus | If the secret is active or not. | string |  |
| IsExpired | If the secret has expired. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "secretId": 1,
  "secret": "sample string 2",
  "activationDate": "sample string 3",
  "createdDate": "sample string 4",
  "secretStatus": "sample string 5",
  "isExpired": true
}
```
</details>


### `POST restapi/personal/learningrecordstore/partners/{partnerId}/secrets/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |

**Response:** `LearningRecordStore.LearningRecordStoreSecretModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| SecretId | Gets or sets the secret ID. | integer |  |
| Secret | Gets or sets the client secret. | string |  |
| ActivationDate | Gets or sets the value of the activation date. | string |  |
| CreatedDate | Gets or sets the value of the creation date. | string |  |
| SecretStatus | If the secret is active or not. | string |  |
| IsExpired | If the secret has expired. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "secretId": 1,
  "secret": "sample string 2",
  "activationDate": "sample string 3",
  "createdDate": "sample string 4",
  "secretStatus": "sample string 5",
  "isExpired": true
}
```
</details>


### `GET restapi/personal/learningrecordstore/partners/{partnerId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |

**Response:** `LearningRecordStore.PartnerExtendedModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Organisations | Gets or sets notes about the partner. | Collection of LearningRecordStore.OrganisationModel |  |
| Secrets | Keys for the partner. A partner can have multiple keys. | Collection of LearningRecordStore.LearningRecordStoreSecretModel |  |
| CanEdit | If permissions allow, the partner can be edited, otherwise it is read-only and the edit/delete option should not be shown in the UI. | boolean |  |
| LearningRecordStorePartnerId | Gets or sets the LRS Partner ID. | integer |  |
| Name | Gets or sets the name of the partner. | string |  |
| ContactPerson | Gets or sets the contact person name. | string |  |
| ContactEmail | Gets or sets the contact person email. | string |  |
| ContactPhoneNumber | Gets or sets the contact phone number. | string |  |
| Notes | Gets or sets notes about the partner. | string |  |
| ClientKey | The client key for the partner. | string |  |
| Created | Date the partner was created. Local time string | date |  |
| CreatedString | Date the partner was created. Local time string | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "organisations": [
    {
      "organisationId": 1,
      "name": "sample string 2",
      "type": "sample string 3"
    }
  ],
  "secrets": [
    {
      "secretId": 1,
      "secret": "sample string 2",
      "activationDate": "sample string 3",
      "createdDate": "sample string 4",
      "secretStatus": "sample string 5",
      "isExpired": true
    }
  ],
  "canEdit": true,
  "learningRecordStorePartnerId": 2,
  "name": "sample string 3",
  "contactPerson": "sample string 4",
  "contactEmail": "sample string 5",
  "contactPhoneNumber": "sample string 6",
  "notes": "sample string 7",
  "clientKey": "sample string 8",
  "created": "2026-11-18T16:34:02Z",
  "createdString": "sample string 10"
}
```
</details>


### `PUT restapi/personal/learningrecordstore/partners/{partnerId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |

**Body** (LearningRecordStore.PartnerModel)

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningRecordStorePartnerId | Gets or sets the LRS Partner ID. | integer |  |
| Name | Gets or sets the name of the partner. | string |  |
| ContactPerson | Gets or sets the contact person name. | string |  |
| ContactEmail | Gets or sets the contact person email. | string |  |
| ContactPhoneNumber | Gets or sets the contact phone number. | string |  |
| Notes | Gets or sets notes about the partner. | string |  |
| ClientKey | The client key for the partner. | string |  |
| Created | Date the partner was created. Local time string | date |  |
| CreatedString | Date the partner was created. Local time string | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "learningRecordStorePartnerId": 1,
  "name": "sample string 2",
  "contactPerson": "sample string 3",
  "contactEmail": "sample string 4",
  "contactPhoneNumber": "sample string 5",
  "notes": "sample string 6",
  "clientKey": "sample string 7",
  "created": "2026-11-18T16:35:31Z",
  "createdString": "sample string 9"
}
```
</details>

**Response:** `LearningRecordStore.PartnerModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningRecordStorePartnerId | Gets or sets the LRS Partner ID. | integer |  |
| Name | Gets or sets the name of the partner. | string |  |
| ContactPerson | Gets or sets the contact person name. | string |  |
| ContactEmail | Gets or sets the contact person email. | string |  |
| ContactPhoneNumber | Gets or sets the contact phone number. | string |  |
| Notes | Gets or sets notes about the partner. | string |  |
| ClientKey | The client key for the partner. | string |  |
| Created | Date the partner was created. Local time string | date |  |
| CreatedString | Date the partner was created. Local time string | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "learningRecordStorePartnerId": 1,
  "name": "sample string 2",
  "contactPerson": "sample string 3",
  "contactEmail": "sample string 4",
  "contactPhoneNumber": "sample string 5",
  "notes": "sample string 6",
  "clientKey": "sample string 7",
  "created": "2026-11-18T16:35:31Z",
  "createdString": "sample string 9"
}
```
</details>


### `DELETE restapi/personal/learningrecordstore/partners/{partnerId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| partnerId |  | integer | Required |

**Response:** `?`


### `POST restapi/personal/learningrecordstore/partners/v1`

_No description._

**Body** (LearningRecordStore.PartnerModel)

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningRecordStorePartnerId | Gets or sets the LRS Partner ID. | integer |  |
| Name | Gets or sets the name of the partner. | string |  |
| ContactPerson | Gets or sets the contact person name. | string |  |
| ContactEmail | Gets or sets the contact person email. | string |  |
| ContactPhoneNumber | Gets or sets the contact phone number. | string |  |
| Notes | Gets or sets notes about the partner. | string |  |
| ClientKey | The client key for the partner. | string |  |
| Created | Date the partner was created. Local time string | date |  |
| CreatedString | Date the partner was created. Local time string | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "learningRecordStorePartnerId": 1,
  "name": "sample string 2",
  "contactPerson": "sample string 3",
  "contactEmail": "sample string 4",
  "contactPhoneNumber": "sample string 5",
  "notes": "sample string 6",
  "clientKey": "sample string 7",
  "created": "2026-11-18T16:30:06Z",
  "createdString": "sample string 9"
}
```
</details>

**Response:** `LearningRecordStore.PartnerModel`

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningRecordStorePartnerId | Gets or sets the LRS Partner ID. | integer |  |
| Name | Gets or sets the name of the partner. | string |  |
| ContactPerson | Gets or sets the contact person name. | string |  |
| ContactEmail | Gets or sets the contact person email. | string |  |
| ContactPhoneNumber | Gets or sets the contact phone number. | string |  |
| Notes | Gets or sets notes about the partner. | string |  |
| ClientKey | The client key for the partner. | string |  |
| Created | Date the partner was created. Local time string | date |  |
| CreatedString | Date the partner was created. Local time string | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "learningRecordStorePartnerId": 1,
  "name": "sample string 2",
  "contactPerson": "sample string 3",
  "contactEmail": "sample string 4",
  "contactPhoneNumber": "sample string 5",
  "notes": "sample string 6",
  "clientKey": "sample string 7",
  "created": "2026-11-18T16:30:06Z",
  "createdString": "sample string 9"
}
```
</details>


### `GET restapi/personal/learningrecordstore/partners/v1?PageIndex={PageIndex}&PageSize={PageSize}&sortField={sortField}&sortOrder={sortOrder}`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sortField |  | string | Default value is |
| sortOrder |  | string | Default value is |

**Response:** `EntityList<LearningRecordStore.PartnerExtendedModel>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of LearningRecordStore.PartnerExtendedModel |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "entityArray": [
    {
      "organisations": [
        {
          "organisationId": 1,
          "name": "sample string 2",
          "type": "sample string 3"
        }
      ],
      "secrets": [
        {
          "secretId": 1,
          "secret": "sample string 2",
          "activationDate": "sample string 3",
          "createdDate": "sample string 4",
          "secretStatus": "sample string 5",
          "isExpired": true
        }
      ],
      "canEdit": true,
      "learningRecordStorePartnerId": 2,
      "name": "sample string 3",
      "contactPerson": "sample string 4",
      "contactEmail": "sample string 5",
      "contactPhoneNumber": "sample string 6",
      "notes": "sample string 7",
      "clientKey": "sample string 8",
      "created": "2026-11-18T16:32:42Z",
      "createdString": "sample string 10"
    }
  ],
  "total": 1,
  "currentPageIndex": 2,
  "pageSize": 3
}
```
</details>

