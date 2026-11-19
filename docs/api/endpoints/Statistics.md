# Statistics

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for statistics API Secured with Oauth2, and requires OAuth2 API scope Statistics.

## Endpoints

- `POST RestApi/personal/statistics/events/v1` — Endpoint for posting user events.

---

### `POST RestApi/personal/statistics/events/v1`

Endpoint for posting user events.

**Body** (ServiceLogic.Statistics.Events.UserEventDto)

| Name | Description | Type | Additional information |
|---|---|---|---|
| Type | Type of an event represented by this object | ServiceLogic.Statistics.Events.UserEventType |  |
| DateTime | Date and time when event has occurred | date |  |
| Payload | Additional event data, specific for of this event | Object |  |

<details><summary>Request JSON sample</summary>

```json
[
  {
    "Type": 0,
    "DateTime": "2026-11-18T16:34:59Z",
    "Payload": {}
  }
]
```
</details>

**Response:** `?` — 204 (No Content) if all of the events have been successfully processed. 400 (Bad request) if one of the events passed as argument are of not recognized type, or event data is invalid, or the event collection is empty.

