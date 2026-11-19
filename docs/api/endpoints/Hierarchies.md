# Hierarchies

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to Hierarchies API.

## Endpoints

- `GET restapi/personal/hierarchies/{hierarchyId}/children/v1` — Get all children of a specific hierarchy. Returns only hierarchies to which person has access to.
- `GET restapi/personal/hierarchies/{hierarchyId}/childrenwithmembercountforcourse/v1` — Get all children of a specific hierarchy, with a count of how many members the hierarchy has.
- `GET restapi/personal/hierarchies/{hierarchyId}/childrenwithmembercountforperson/v1` — Get all children of a specific hierarchy, with a count of how many members the hierarchy has.
- `GET restapi/personal/hierarchies/{hierarchyId}/members/v1` — Get all members and their role in the specified hierarchy. Will only return members the calling user is allowe…
- `GET restapi/personal/hierarchies/{hierarchyId}/members/v2` — Get all members and their role in the specified hierarchy. Will only return members the calling user is allowe…
- `GET restapi/personal/hierarchies/{hierarchyId}/parents/v1` — Get all parents of a specific hierarchy.
- `GET restapi/personal/hierarchies/{hierarchyId}/v1` — Get all members of a specific hierarchy with the given role.
- `GET restapi/personal/hierarchies/default/v1` — Get default hierarchy for the current person.
- `GET restapi/personal/hierarchies/defaultforcourse/{courseId}` — Get default hierarchy for the current course.
- `GET restapi/personal/hierarchies/defaultwithmembercount/v1` — Get default hierarchy for the current person.
- `GET restapi/personal/hierarchies/organisations/v1` — Get all organisations to which current person belongs to and has specific role.
- `GET restapi/personal/hierarchies/organizations/v1` ⚠️ deprecated — Get all organizations to which current person belongs to and has specific role. This method obsolete, use 'org…

---

### `GET restapi/personal/hierarchies/{hierarchyId}/children/v1`

Get all children of a specific hierarchy. Returns only hierarchies to which person has access to.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |

**Response:** `?` — List of children hierarchies.

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "HierarchyId": 1,
    "ParentHierarchyId": 2,
    "Title": "sample string 3",
    "Path": "sample string 4",
    "OrganizationType": 0,
    "OrganizationHierarchyId": 5
  }
]
```
</details>


### `GET restapi/personal/hierarchies/{hierarchyId}/childrenwithmembercountforcourse/v1?courseId={courseId}`

Get all children of a specific hierarchy, with a count of how many members the hierarchy has.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |
| courseId | [OPTIONAL] The course ID. Required if synchable is true. Example usage ?courseid=20 | integer | Default value is -1 |

**Response:** `?` — List of children hierarchies.

| Name | Description | Type | Additional information |
|---|---|---|---|
| NumberOfMembers | Numbers of members in the hierarchy level | integer |  |
| HasChildren | Has child hierarchies | boolean |  |
| Synchable | Can be synchronised | boolean |  |
| Inspectable | Can be inspected | boolean |  |
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "NumberOfMembers": 1,
    "HasChildren": true,
    "Synchable": true,
    "Inspectable": true,
    "HierarchyId": 5,
    "ParentHierarchyId": 6,
    "Title": "sample string 7",
    "Path": "sample string 8",
    "OrganizationType": 0,
    "OrganizationHierarchyId": 9
  }
]
```
</details>


### `GET restapi/personal/hierarchies/{hierarchyId}/childrenwithmembercountforperson/v1`

Get all children of a specific hierarchy, with a count of how many members the hierarchy has.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |

**Response:** `?` — List of children hierarchies.

| Name | Description | Type | Additional information |
|---|---|---|---|
| NumberOfMembers | Numbers of members in the hierarchy level | integer |  |
| HasChildren | Has child hierarchies | boolean |  |
| Synchable | Can be synchronised | boolean |  |
| Inspectable | Can be inspected | boolean |  |
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "NumberOfMembers": 1,
    "HasChildren": true,
    "Synchable": true,
    "Inspectable": true,
    "HierarchyId": 5,
    "ParentHierarchyId": 6,
    "Title": "sample string 7",
    "Path": "sample string 8",
    "OrganizationType": 0,
    "OrganizationHierarchyId": 9
  }
]
```
</details>


### `GET restapi/personal/hierarchies/{hierarchyId}/members/v1?PageIndex={PageIndex}&PageSize={PageSize}`

Get all members and their role in the specified hierarchy. Will only return members the calling user is allowed to see.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |

**Response:** `EntityList<PersonSimpleWithHierarchyRole>` — EntityList of members with role

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimpleWithHierarchyRole |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "HierarchyRole": "sample string 1",
      "PersonId": 2,
      "FirstName": "sample string 3",
      "LastName": "sample string 4",
      "FullName": "sample string 5",
      "ProfileUrl": "sample string 6",
      "AdditionalInfo": "sample string 7",
      "ProfileImageUrl": "sample string 8",
      "ProfileImageUrlSmall": "sample string 9"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/hierarchies/{hierarchyId}/members/v2?PageIndex={PageIndex}&PageSize={PageSize}&courseContext={courseContext}`

Get all members and their role in the specified hierarchy. Will only return members the calling user is allowed to see.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| courseContext | [OPTIONAL] A course context to consider while fetching members | integer |  |

**Response:** `EntityList<PersonSimpleWithHierarchyRoleAndCourseMembership>` — EntityList of members with role

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of PersonSimpleWithHierarchyRoleAndCourseMembership |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "MemberInCourse": true,
      "HierarchyRole": "sample string 2",
      "PersonId": 3,
      "FirstName": "sample string 4",
      "LastName": "sample string 5",
      "FullName": "sample string 6",
      "ProfileUrl": "sample string 7",
      "AdditionalInfo": "sample string 8",
      "ProfileImageUrl": "sample string 9",
      "ProfileImageUrlSmall": "sample string 10"
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/hierarchies/{hierarchyId}/parents/v1?visibleInSearchOnly={visibleInSearchOnly}`

Get all parents of a specific hierarchy.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |
| visibleInSearchOnly | If true then return only visible is search parents. | boolean | Default value is False |

**Response:** `?` — List of children hierarchies.

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "HierarchyId": 1,
    "ParentHierarchyId": 2,
    "Title": "sample string 3",
    "Path": "sample string 4",
    "OrganizationType": 0,
    "OrganizationHierarchyId": 5
  }
]
```
</details>


### `GET restapi/personal/hierarchies/{hierarchyId}/v1?role={role}`

Get all members of a specific hierarchy with the given role.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| hierarchyId | The identifier of the hierarchy. | integer | Required |
| role | Required role. | HierarchyRole | Default value is 0 |

**Response:** `?` — List of members.

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
[
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
]
```
</details>


### `GET restapi/personal/hierarchies/default/v1`

Get default hierarchy for the current person.

**Response:** `HierarchySimple` — Default hierarchy for the current person.

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "HierarchyId": 1,
  "ParentHierarchyId": 2,
  "Title": "sample string 3",
  "Path": "sample string 4",
  "OrganizationType": 0,
  "OrganizationHierarchyId": 5
}
```
</details>


### `GET restapi/personal/hierarchies/defaultforcourse/{courseId}`

Get default hierarchy for the current course.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseId | The course ID. | integer | Required |

**Response:** `HierarchySimpleWithMemberCount` — Default hierarchy for the current person.

| Name | Description | Type | Additional information |
|---|---|---|---|
| NumberOfMembers | Numbers of members in the hierarchy level | integer |  |
| HasChildren | Has child hierarchies | boolean |  |
| Synchable | Can be synchronised | boolean |  |
| Inspectable | Can be inspected | boolean |  |
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "NumberOfMembers": 1,
  "HasChildren": true,
  "Synchable": true,
  "Inspectable": true,
  "HierarchyId": 5,
  "ParentHierarchyId": 6,
  "Title": "sample string 7",
  "Path": "sample string 8",
  "OrganizationType": 0,
  "OrganizationHierarchyId": 9
}
```
</details>


### `GET restapi/personal/hierarchies/defaultwithmembercount/v1`

Get default hierarchy for the current person.

**Response:** `HierarchySimpleWithMemberCount` — Default hierarchy for the current person.

| Name | Description | Type | Additional information |
|---|---|---|---|
| NumberOfMembers | Numbers of members in the hierarchy level | integer |  |
| HasChildren | Has child hierarchies | boolean |  |
| Synchable | Can be synchronised | boolean |  |
| Inspectable | Can be inspected | boolean |  |
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "NumberOfMembers": 1,
  "HasChildren": true,
  "Synchable": true,
  "Inspectable": true,
  "HierarchyId": 5,
  "ParentHierarchyId": 6,
  "Title": "sample string 7",
  "Path": "sample string 8",
  "OrganizationType": 0,
  "OrganizationHierarchyId": 9
}
```
</details>


### `GET restapi/personal/hierarchies/organisations/v1?role={role}`

Get all organisations to which current person belongs to and has specific role.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| role | The required role of the person in organization. | HierarchyRole | Default value is 0 |

**Response:** `EntityList<HierarchySimple>` — List of organisations.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of HierarchySimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "HierarchyId": 1,
      "ParentHierarchyId": 2,
      "Title": "sample string 3",
      "Path": "sample string 4",
      "OrganizationType": 0,
      "OrganizationHierarchyId": 5
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/personal/hierarchies/organizations/v1?role={role}`

Get all organizations to which current person belongs to and has specific role. This method obsolete, use 'organisations/v1' instead.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| role | The required role of the person in organization. | HierarchyRole | Default value is 0 |

**Response:** `EntityList<HierarchySimple>` — List of organizations.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of HierarchySimple |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "HierarchyId": 1,
      "ParentHierarchyId": 2,
      "Title": "sample string 3",
      "Path": "sample string 4",
      "OrganizationType": 0,
      "OrganizationHierarchyId": 5
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

