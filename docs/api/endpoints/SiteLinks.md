# SiteLinks

> Auto-generated from https://sdu.itslearning.com/restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for retrieving the site links. Secured with Oauth2, and requires OAuth2 API scope Person.

## Endpoints

- `GET restapi/personal/person/sitelinks/v1` — Gets the site links those are visible for the user.

---

### `GET restapi/personal/person/sitelinks/v1`

Gets the site links those are visible for the user.

**Response:** `EntityList<SiteLink>` — List of site links

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of SiteLink |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "Title": "sample string 1",
      "Url": "sample string 2",
      "DisplayInSeparateWindow": true
    }
  ],
  "Total": 1,
  "CurrentPageIndex": 2,
  "PageSize": 3
}
```
</details>

