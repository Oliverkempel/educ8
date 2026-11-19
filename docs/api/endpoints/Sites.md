# Sites

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint to get information about all active itslearning sites.

## Endpoints

- `GET restapi/sites/{customerId}/logindetails/v1` — Get login details for an itslearning site by customerId.
- `GET restapi/sites/{customerId}/v1` — Get an itslearning site by customerId.
- `GET restapi/sites/all/organisations/search/v1` — Get organisations (sites and schools).
- `GET restapi/sites/all/organisations/v1` — Get all organisations (sites and schools).
- `GET restapi/sites/all/v1` — Get all customer sites (used e.g. by welcome page)
- `GET restapi/sites/countries/{countryCode}/organisations/v1` — Get organisations (sites and schools) for a specific country by country code (case insensitive).
- `GET restapi/sites/countries/{countryCode}/v1` — Get sites by country code (case insensitive).
- `GET restapi/sites/shortnames/{shortname}/v1` — Get an itslearning site by shortname. The site may be inactive. If a site with the given shortname does not ex…
- `GET restapi/sites/v1` — Get sites that have itslearning ordered by title.

---

### `GET restapi/sites/{customerId}/logindetails/v1`

Get login details for an itslearning site by customerId.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| customerId | The customerId for the site | integer | Required |

**Response:** `SiteLoginDetails`

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsNativeLoginDefault | True if native login is set as default for the site, and false otherwise. | boolean |  |
| FederatedLoginProviders | The list of federated login providers for the site. | Collection of FederatedLoginProvider |  |
| DisableForgottenPasswordLink | True if the forgotten password link not visible for the site. | boolean |  |
| IsParentPortalEnabled | True if the parent portal is enabled for the site. | boolean |  |
| IsFronterUpgradedSite | IsFronterUpgradedSite flag. | boolean |  |
| IsParentAppEnabled | Determines whether parent app is enabled. | boolean |  |
| OrgApiBaseUrl | The Url to Organisation Api (AKA Content Import). | string |  |
| CustomerId | The customer id for this site. | integer |  |
| Title | The title used for the site. | string |  |
| ShortName | The short name used for the site. | string |  |
| CultureName | The culture name used for the site. | string |  |
| BaseUrl | Base url for the site. | string |  |
| IsPersonalRestApiEnabled | Check to see if the Personal API has been enabled for this site. | boolean |  |
| ShowCustomerInDropdownList | Flag to identify wheteher customer should appear in dropdown lists. | boolean |  |
| CountryCode | The country code for the customer. | string |  |
| StateCode | The state code for the customer. | string |  |
| Segment | The education segment. | Itsolutions.ItslUtils.Constants.CustomerEducationSegment |  |

<details><summary>Response JSON sample</summary>

```json
{
  "IsNativeLoginDefault": true,
  "FederatedLoginProviders": [
    {
      "Id": 1,
      "Name": "sample string 2",
      "IconUrl": "sample string 3",
      "OpenInExternalBrowser": true,
      "ButtonText": "sample string 5",
      "LogoutType": 1
    }
  ],
  "DisableForgottenPasswordLink": true,
  "IsParentPortalEnabled": true,
  "IsFronterUpgradedSite": true,
  "IsParentAppEnabled": true,
  "OrgApiBaseUrl": "sample string 6",
  "CustomerId": 7,
  "Title": "sample string 8",
  "ShortName": "sample string 9",
  "CultureName": "sample string 10",
  "BaseUrl": "sample string 11",
  "IsPersonalRestApiEnabled": true,
  "ShowCustomerInDropdownList": true,
  "CountryCode": "sample string 14",
  "StateCode": "sample string 15",
  "Segment": 0
}
```
</details>


### `GET restapi/sites/{customerId}/v1`

Get an itslearning site by customerId.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| customerId | The customerId for the site | integer | Required |

**Response:** `Site`

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsFronterUpgradedSite | IsFronterUpgradedSite flag. | boolean |  |
| IsParentAppEnabled | Determines whether parent app is enabled. | boolean |  |
| OrgApiBaseUrl | The Url to Organisation Api (AKA Content Import). | string |  |
| CustomerId | The customer id for this site. | integer |  |
| Title | The title used for the site. | string |  |
| ShortName | The short name used for the site. | string |  |
| CultureName | The culture name used for the site. | string |  |
| BaseUrl | Base url for the site. | string |  |
| IsPersonalRestApiEnabled | Check to see if the Personal API has been enabled for this site. | boolean |  |
| ShowCustomerInDropdownList | Flag to identify wheteher customer should appear in dropdown lists. | boolean |  |
| CountryCode | The country code for the customer. | string |  |
| StateCode | The state code for the customer. | string |  |
| Segment | The education segment. | Itsolutions.ItslUtils.Constants.CustomerEducationSegment |  |

<details><summary>Response JSON sample</summary>

```json
{
  "IsFronterUpgradedSite": true,
  "IsParentAppEnabled": true,
  "OrgApiBaseUrl": "sample string 3",
  "CustomerId": 4,
  "Title": "sample string 5",
  "ShortName": "sample string 6",
  "CultureName": "sample string 7",
  "BaseUrl": "sample string 8",
  "IsPersonalRestApiEnabled": true,
  "ShowCustomerInDropdownList": true,
  "CountryCode": "sample string 11",
  "StateCode": "sample string 12",
  "Segment": 0
}
```
</details>


### `GET restapi/sites/all/organisations/search/v1?searchText={searchText}&PageIndex={PageIndex}&PageSize={PageSize}&sitesFilter={sitesFilter}`

Get organisations (sites and schools).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| searchText | E.g. searchText=ber gen, will return all sites where title has at least one word starting with 'ber' and one starting with 'gen'(not case sensitive). Rules of search could be changed. | string | Default value is |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sitesFilter | [All\|Itslearning\|Parents] filter for sites | Public.Unauthenticated.SitesFilter | Default value is All |

**Response:** `EntityList<AggregatedOrganisationLite>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of AggregatedOrganisationLite |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CustomerId": 1,
      "SiteName": "sample string 2",
      "SchoolName": "sample string 3",
      "HierarchyId": 4,
      "IsSite": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/sites/all/organisations/v1?sitesFilter={sitesFilter}`

Get all organisations (sites and schools).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| sitesFilter | [All\|Itslearning\|Parents] filter for sites | Public.Unauthenticated.SitesFilter | Default value is All |

**Response:** `EntityList<AggregatedOrganisationLite>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of AggregatedOrganisationLite |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CustomerId": 1,
      "SiteName": "sample string 2",
      "SchoolName": "sample string 3",
      "HierarchyId": 4,
      "IsSite": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/sites/all/v1?sitesFilter={sitesFilter}`

Get all customer sites (used e.g. by welcome page)

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| sitesFilter | [All\|Itslearning\|Parents] filter for sites | Public.Unauthenticated.SitesFilter | Default value is All |

**Response:** `EntityList<SiteBase>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of SiteBase |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CustomerId": 1,
      "Title": "sample string 2",
      "ShortName": "sample string 3",
      "CultureName": "sample string 4",
      "BaseUrl": "sample string 5",
      "IsPersonalRestApiEnabled": true,
      "ShowCustomerInDropdownList": true,
      "CountryCode": "sample string 8",
      "StateCode": "sample string 9",
      "Segment": 0
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/sites/countries/{countryCode}/organisations/v1`

Get organisations (sites and schools) for a specific country by country code (case insensitive).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| countryCode | Country code. | string | Required |

**Response:** `EntityList<AggregatedOrganisation>` — List of organisations (sites and schools) for the specified country.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of AggregatedOrganisation |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "IsFronterUpgradedSite": true,
      "IsParentAppEnabled": true,
      "CustomerId": 3,
      "SiteName": "sample string 4",
      "SchoolName": "sample string 5",
      "HierarchyId": 6,
      "IsSite": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/sites/countries/{countryCode}/v1?searchText={searchText}`

Get sites by country code (case insensitive).

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| countryCode | Country code. | string | Required |
| searchText | Search text. | string | Default value is |

**Response:** `EntityList<SiteBase>` — List of sites for the specified country.

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of SiteBase |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CustomerId": 1,
      "Title": "sample string 2",
      "ShortName": "sample string 3",
      "CultureName": "sample string 4",
      "BaseUrl": "sample string 5",
      "IsPersonalRestApiEnabled": true,
      "ShowCustomerInDropdownList": true,
      "CountryCode": "sample string 8",
      "StateCode": "sample string 9",
      "Segment": 0
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>


### `GET restapi/sites/shortnames/{shortname}/v1`

Get an itslearning site by shortname. The site may be inactive. If a site with the given shortname does not exist, null will be returned.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| shortname | The shortname for the site | string | Required |

**Response:** `Site`

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsFronterUpgradedSite | IsFronterUpgradedSite flag. | boolean |  |
| IsParentAppEnabled | Determines whether parent app is enabled. | boolean |  |
| OrgApiBaseUrl | The Url to Organisation Api (AKA Content Import). | string |  |
| CustomerId | The customer id for this site. | integer |  |
| Title | The title used for the site. | string |  |
| ShortName | The short name used for the site. | string |  |
| CultureName | The culture name used for the site. | string |  |
| BaseUrl | Base url for the site. | string |  |
| IsPersonalRestApiEnabled | Check to see if the Personal API has been enabled for this site. | boolean |  |
| ShowCustomerInDropdownList | Flag to identify wheteher customer should appear in dropdown lists. | boolean |  |
| CountryCode | The country code for the customer. | string |  |
| StateCode | The state code for the customer. | string |  |
| Segment | The education segment. | Itsolutions.ItslUtils.Constants.CustomerEducationSegment |  |

<details><summary>Response JSON sample</summary>

```json
{
  "IsFronterUpgradedSite": true,
  "IsParentAppEnabled": true,
  "OrgApiBaseUrl": "sample string 3",
  "CustomerId": 4,
  "Title": "sample string 5",
  "ShortName": "sample string 6",
  "CultureName": "sample string 7",
  "BaseUrl": "sample string 8",
  "IsPersonalRestApiEnabled": true,
  "ShowCustomerInDropdownList": true,
  "CountryCode": "sample string 11",
  "StateCode": "sample string 12",
  "Segment": 0
}
```
</details>


### `GET restapi/sites/v1?startsWith={startsWith}&PageIndex={PageIndex}&PageSize={PageSize}&sitesFilter={sitesFilter}`

Get sites that have itslearning ordered by title.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| startsWith | E.g. startsWith=ber gen, will return all sites where title has at least one word starting with 'ber' and one starting with 'gen'(not case sensitive) | string | Default value is |
| PageIndex |  | integer |  |
| PageSize |  | integer |  |
| sitesFilter | [All\|Itslearning\|Parents] filter for sites | Public.Unauthenticated.SitesFilter | Default value is All |

**Response:** `EntityList<SiteBase>`

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of SiteBase |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "CustomerId": 1,
      "Title": "sample string 2",
      "ShortName": "sample string 3",
      "CultureName": "sample string 4",
      "BaseUrl": "sample string 5",
      "IsPersonalRestApiEnabled": true,
      "ShowCustomerInDropdownList": true,
      "CountryCode": "sample string 8",
      "StateCode": "sample string 9",
      "Segment": 0
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

