# Person

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to get and modify a user's profile. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/person/{personId}/v1` — Gets basic information about the requested person. The person should be allowed to search for by Esafety rules…
- `GET restapi/personal/person/extrauserinformation/{personId}/v1` — Gets extra user information (if such exists) for a given person If no such field has been configured on the cu…
- `GET restapi/personal/person/features/v1` — Gets available features for the current person.
- `GET restapi/personal/person/image/dimensions/v1` — Get the dimensions that should be used for profile images that you upload through this API.
- `POST restapi/personal/person/image/v1` — Upload and update a person's profile image.
- `PUT restapi/personal/person/online/v1` — This method is used to mark the online status of a user.
- `GET restapi/personal/person/permission/lte/{learningObjectId}/{learningObjectInstanceId}/content/v1` — 
- `GET restapi/personal/person/permission/lte/{learningObjectId}/{learningObjectInstanceId}/rte/{pluginResourceId}/{rtfId}/v1` — 
- `GET restapi/personal/person/permission/rte/{pluginResourceId}/{rtfId}/v1` — 
- `PUT restapi/personal/person/PersonSetting/RemindConnectGoogleOnLogin/v1` — Set the person setting "RemindConnectGoogleOnLogin". When this setting is on the user will be reminded to conn…
- `POST restapi/personal/person/progress/lte/{learningObjectId}/{learningObjectInstanceId}/grade/v1` — 
- `POST restapi/personal/person/push/v1` — Set push notifications in itslearning (on or off) for a device. This method will also register the device for …
- `GET restapi/personal/person/relations/{personId}/v1` — Get list of relations for the person.
- `GET restapi/personal/person/roles/v1` — Get list of roles for person.
- `GET restapi/personal/person/v1` — Get the user's profile details.

---

### `GET restapi/personal/person/{personId}/v1`

Gets basic information about the requested person. The person should be allowed to search for by Esafety rules.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | Identifier of the person. | integer | Required |

**Response:** `PersonSimple`

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "PersonId": 1,
  "FirstName": "sample string 2",
  "LastName": "sample string 3",
  "FullName": "sample string 4",
  "ProfileUrl": "sample string 5",
  "AdditionalInfo": "sample string 6",
  "ProfileImageUrl": "sample string 7",
  "ProfileImageUrlSmall": "sample string 8"
}
```
</details>


### `GET restapi/personal/person/extrauserinformation/{personId}/v1`

Gets extra user information (if such exists) for a given person If no such field has been configured on the customer site, or if the current user does not have access to see it, a field with empty name and value will be returned. If such a field exists, but no value has been set for the given personId, then a field with the correct name, but empty value will be returned.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | The id of the person we want the extra user information for | integer | Required |

**Response:** `Itsolutions.Itslearning.ItslCore.BusinessLayer.Person.CustomInfo.ExtraUserInformationField` — A field containing name and value of the extra information field for the given personId. Note that both name and value can be empty

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name |  | string |  |
| Value |  | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "",
  "Value": ""
}
```
</details>


### `GET restapi/personal/person/features/v1`

Gets available features for the current person.

**Response:** `Personal.Feature`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The feature name. | string |  |
| Url | Url to the itslearning web page for the feature if it exists. | string |  |
| Features | The list of sub-features. | Collection of Personal.Feature |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Name": "sample string 1",
  "Url": "sample string 2",
  "Features": [
    {
      "Name": "sample string 1",
      "Url": "sample string 2",
      "Features": null
    }
  ]
}
```
</details>


### `GET restapi/personal/person/image/dimensions/v1`

Get the dimensions that should be used for profile images that you upload through this API.

**Response:** `PersonImageDimensions` — The maximum height and width of the persons uploaded image.

| Name | Description | Type | Additional information |
|---|---|---|---|
| PixelWidth |  | integer |  |
| PixelHeight |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "PixelWidth": 1,
  "PixelHeight": 2
}
```
</details>


### `POST restapi/personal/person/image/v1`

Upload and update a person's profile image.

**Response:** `System.Net.Http.HttpResponseMessage` — Returns HTTP status 201 if the image has been successfully created. The response will contain the Location header which has the absolute URI of the uploaded image. If request is not successful there will be an error in the response.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `PUT restapi/personal/person/online/v1`

This method is used to mark the online status of a user.

**Response:** `integer` — The interval in seconds before you should call this method again

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `GET restapi/personal/person/permission/lte/{learningObjectId}/{learningObjectInstanceId}/content/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| learningObjectId |  | integer | Required |
| learningObjectInstanceId |  | integer | Required |

**Response:** `Personal.Permission`

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanRead | Whether user can read. | boolean |  |
| CanParticipate | Whether user can participate. | boolean |  |
| CanEvaluate | Whether user can evaluate. | boolean |  |
| CanModify | Whether user can modify. | boolean |  |
| TimeToLiveInSeconds | Permission time to live in seconds. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CanRead": true,
  "CanParticipate": true,
  "CanEvaluate": true,
  "CanModify": true,
  "TimeToLiveInSeconds": 5
}
```
</details>


### `GET restapi/personal/person/permission/lte/{learningObjectId}/{learningObjectInstanceId}/rte/{pluginResourceId}/{rtfId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| learningObjectId |  | integer | Required |
| learningObjectInstanceId |  | integer | Required |
| pluginResourceId |  | string | Required |
| rtfId |  | integer | Required |

**Response:** `Personal.Permission`

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanRead | Whether user can read. | boolean |  |
| CanParticipate | Whether user can participate. | boolean |  |
| CanEvaluate | Whether user can evaluate. | boolean |  |
| CanModify | Whether user can modify. | boolean |  |
| TimeToLiveInSeconds | Permission time to live in seconds. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CanRead": true,
  "CanParticipate": true,
  "CanEvaluate": true,
  "CanModify": true,
  "TimeToLiveInSeconds": 5
}
```
</details>


### `GET restapi/personal/person/permission/rte/{pluginResourceId}/{rtfId}/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| pluginResourceId |  | string | Required |
| rtfId |  | integer | Required |

**Response:** `Personal.Permission`

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanRead | Whether user can read. | boolean |  |
| CanParticipate | Whether user can participate. | boolean |  |
| CanEvaluate | Whether user can evaluate. | boolean |  |
| CanModify | Whether user can modify. | boolean |  |
| TimeToLiveInSeconds | Permission time to live in seconds. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CanRead": true,
  "CanParticipate": true,
  "CanEvaluate": true,
  "CanModify": true,
  "TimeToLiveInSeconds": 5
}
```
</details>


### `PUT restapi/personal/person/PersonSetting/RemindConnectGoogleOnLogin/v1`

Set the person setting "RemindConnectGoogleOnLogin". When this setting is on the user will be reminded to connect his/her the Google account if a IsGoogleIntegrationEnabled and IsGlobalSsoFromGoogleEnabled on the site

**Body:** True will enable reminder, false will disable reminder boolean

<details><summary>Request JSON sample</summary>

```json
true
```
</details>

**Response:** `?`


### `POST restapi/personal/person/progress/lte/{learningObjectId}/{learningObjectInstanceId}/grade/v1`

_No description._

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| learningObjectId |  | integer | Required |
| learningObjectInstanceId |  | integer | Required |

**Body** (Personal.ScoreBasedGrade)

| Name | Description | Type | Additional information |
|---|---|---|---|
| MaxScore | Max score. | decimal number |  |
| Score | Given score. | decimal number |  |

<details><summary>Request JSON sample</summary>

```json
{
  "MaxScore": 1.1,
  "Score": 2.1
}
```
</details>

**Response:** `?`


### `POST restapi/personal/person/push/v1`

Set push notifications in itslearning (on or off) for a device. This method will also register the device for push notifications with the push provider (if needed).

**Body** (PushSetting)

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeviceId | Current (or new) unique DeviceId for the application on the phone or the tablet. This ID must be recognized by Google Cloud Messaging or Apple Push Notification Service. When PushProvider=Apple: the same as DeviceToken When PushProvider=Google: the same as RegistrationId | string |  |
| OldDeviceId | Old DeviceId for the application on the phone or the tablet. When the application receives a new DeviceId, the application should also provide the old one here, so that itslearning can replace the old one. This is to make sure that we use the correct deviceId as soon as possible, and to prevent itslearning from sending multiple push notification messages with the same content. | string |  |
| PushProvider | Type of push provider (e.g. "Google" or "Apple") | string |  |
| IsEnabled | If push notifications should be enabled or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "DeviceId": "sample string 1",
  "OldDeviceId": "sample string 2",
  "PushProvider": "sample string 3",
  "IsEnabled": true
}
```
</details>

**Response:** `System.Net.Http.HttpResponseMessage` — 200 ok if operation was successful

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |


### `GET restapi/personal/person/relations/{personId}/v1`

Get list of relations for the person.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId |  | integer | Required |

**Response:** `?`

| Name | Description | Type | Additional information |
|---|---|---|---|
| Text | A text describing the relation between two persons | string |  |
| Items | Names for items in the relation. The names can be tilte for courses and projects, Name of persons etc. | Collection of string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Text": "sample string 1",
    "Items": [
      "sample string 1"
    ]
  }
]
```
</details>


### `GET restapi/personal/person/roles/v1`

Get list of roles for person.

**Response:** `?`

<details><summary>Response JSON sample</summary>

```json
[
  "None"
]
```
</details>


### `GET restapi/personal/person/v1`

Get the user's profile details.

**Response:** `PersonProfile` — Details about the user like name, profile image URL, language etc.

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The unique ID of the person | integer |  |
| FirstName | The users first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The users last name. | string |  |
| Language | The users preferred language. | string |  |
| ProfileImageUrl | The absolute URL to the user uploaded profile image. | string |  |
| Children | DEPRECATED Use GET restapi/personal/children/v1 to get the list of children | Collection of Child |  |
| iCalUrl | The iCal URL to the person's calendar. Calendar feed returns events from all courses. | string |  |
| CanAccessMessageSystem | True if user can access the internal messages system | boolean |  |
| CanAccessCalendar | True if user can access the itslearning calendar system | boolean |  |
| CanAccessPersonalSettings | True if user can access personal settings. Need this in order to change profile picture. | boolean |  |
| CanAccessInstantMessageSystem | True if user can access the internal instant messages system | boolean |  |
| TimeZoneId | Id of timezone that can be used in .net framework: TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId). | string |  |
| Use12HTimeFormat | True if user uses 12h time format. | boolean |  |
| SyncKey | The unique person synchronization identifier. Returned only if API client has access to extended data. | string |  |
| CanAccessCourses | True if user can courses. | boolean |  |
| iCalFavoriteOnlyUrl | The iCal URL to the person's calendar. Calendar feed returns events only from favourite courses. | string |  |
| HasHigherEducationLanguage | If the user should see Higher Education language. | boolean |  |
| FullName | The users full name. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "PersonId": 1,
  "FirstName": "sample string 2",
  "LastName": "sample string 3",
  "Language": "sample string 4",
  "ProfileImageUrl": "sample string 5",
  "Children": [
    {
      "PersonId": 1,
      "FirstName": "sample string 2",
      "LastName": "sample string 3",
      "FullName": "sample string 4"
    }
  ],
  "iCalUrl": "sample string 6",
  "CanAccessMessageSystem": true,
  "CanAccessCalendar": true,
  "CanAccessPersonalSettings": true,
  "CanAccessInstantMessageSystem": true,
  "TimeZoneId": "sample string 11",
  "Use12HTimeFormat": true,
  "SyncKey": "sample string 13",
  "CanAccessCourses": true,
  "iCalFavoriteOnlyUrl": "sample string 15",
  "HasHigherEducationLanguage": true,
  "FullName": "sample string 17"
}
```
</details>

