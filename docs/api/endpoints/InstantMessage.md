# InstantMessage

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Endpoint for internal instant messages for personal API. Secured with Oauth2, and requires OAuth2 API scope InstantMessage.

## Endpoints

- `DELETE restapi/personal/instantmessages/{instantMessageId}/abusereport/v1` — Cancels abuse report.
- `PATCH restapi/personal/instantmessages/{instantMessageId}/restore/v1` — Undo deletion for an instant message.
- `DELETE restapi/personal/instantmessages/{instantMessageId}/v1` — Deletes an instant message.
- `POST restapi/personal/instantmessages/attachment/v1` — Upload file attachment to temporary storage. The file should be included in the post call so we can extract it…
- `PUT restapi/personal/instantmessages/cancelabusethreadreport/{threadId}/v1` — Cancels abuse report for thread.
- `POST restapi/personal/instantmessages/communicationchannel/{channelId}/v1` — Register a (SignalR) communication channel
- `GET restapi/personal/instantmessages/messagethreads/{instantMessageThreadId}/unread/count/v1` — Gets number of unread messages in a specific thread for the current user
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/messages/search/v1` — Searches inside the given thread for messages matching the given search text.
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/messages/v2` — Gets the messages in a specific thread. The impersonated user need to be a member of the thread. One cannot us…
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/messages/v3` — Gets a collection of instant messages extended with hasMore field.
- `PUT restapi/personal/instantmessages/messagethreads/{threadId}/moderators/v1` — Adds moderator to an instant message thread.
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/participants/messages-disabled/v1` — Gets a collection of thread participants who cannot see messages.
- `PUT restapi/personal/instantmessages/messagethreads/{threadId}/participants/v1` — Adds participants to an instant message thread.
- `DELETE restapi/personal/instantmessages/messagethreads/{threadId}/participants/v1` — Remove participants from an instant message thread.
- `PATCH restapi/personal/instantmessages/messagethreads/{threadId}/restore/v1` — Restores deletion for thread.
- `PUT restapi/personal/instantmessages/messagethreads/{threadId}/toggleEnableDisableReplies/v1` — Toggle whether students can send replies to this thread or not
- `PUT restapi/personal/instantmessages/messagethreads/{threadId}/updatelastread/{lastReadInstantMessageId}/v1` — Updates last read instant message id for a thread
- `DELETE restapi/personal/instantmessages/messagethreads/{threadId}/v1` — Deletes a thread.
- `PUT restapi/personal/instantmessages/messagethreads/{threadId}/v1` — Updates a instant message thread. Supports updating the name of the thread or the list of the participants. Wi…
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/v1` ⚠️ deprecated — DEPRECATED use "messagethreads/{threadId}/messages/v2" instead. Gets the messages in a specific thread if the …
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/v2` — Gets the instant message thread and the messages in a specific thread. The impersonated user need to be a memb…
- `GET restapi/personal/instantmessages/messagethreads/{threadId}/v3` — Gets the instant message thread and the messages in a specific thread. The impersonated user need to be a memb…
- `GET restapi/personal/instantmessages/messagethreads/collaborations/{collaborationId}/v1` — Gets the instant message thread id for group answer. Creates it if not there yet.
- `GET restapi/personal/instantmessages/messagethreads/onetoone/{personId}/v1` — Gets the instant message thread id for a one to one conversation between the caller and a user.
- `POST restapi/personal/instantmessages/messagethreads/search/v1` — Searches the logged in user's message threads. PersonId is gotten from the authorization (session or oauth)
- `GET restapi/personal/instantmessages/messagethreads/suggestions/v1` — Suggests conversations which names contain search string.
- `GET restapi/personal/instantmessages/messagethreads/unread/count/v1` — Gets number of threads that contain unread messages
- `GET restapi/personal/instantmessages/messagethreads/v1` — Gets the logged in user's message threads. PersonId is gotten from the authorization (session or oauth)
- `GET restapi/personal/instantmessages/messagethreads/v2` — Gets the logged in user's message threads. PersonId is gotten from the authorization (session or oauth)
- `GET restapi/personal/instantmessages/messagethreads/v3` — Gets a collection of threads for logged in user extended with hasMore field. Supports paging by sort index fie…
- `GET restapi/personal/instantmessages/messagethreads/v4` — Gets a collection of threads for logged in user extended with hasMore field. Supports paging by sort index fie…
- `GET restapi/personal/instantmessages/permissions/v1` — Gets permissions for the instant message system.
- `GET restapi/personal/instantmessages/privacy/{personId}/v1` — Checks privacy rules to determine if user is allowed to initiate communication with a person.
- `POST restapi/personal/instantmessages/recipients/persons/v1` — Gets recipient persons requested
- `GET restapi/personal/instantmessages/recipients/search/v1` — Gets persons that the user is allowed to communicate with given the customer's esafety rules configured in its…
- `GET restapi/personal/instantmessages/recipients/v1` — Gets recipients(persons, course or project) requested
- `GET restapi/personal/instantmessages/recipients/v2` — Gets recipients(persons, course or project) requested
- `GET restapi/personal/instantmessages/settings/v1` — Gets the settings for the instant message system. Boundaries for attachment size, max participants in a group …
- `GET restapi/personal/instantmessages/settings/v2` — Gets the settings for the instant message system. Boundaries for attachment size, max participants in a group …
- `GET restapi/personal/instantmessages/starred/count/v1` — Gets the number of starred messages for the current user
- `GET restapi/personal/instantmessages/suggestions/v1` — Searches for suggestions of person names or thread names that match the input search query for the current use…
- `PUT restapi/personal/instantmessages/togglemessagestar/{instantMessageId}/v1` — Toggle whether the given message is starred or not
- `PUT restapi/personal/instantmessages/togglepersonblock/{personId}/v1` — Toggles block for specific .
- `POST restapi/personal/instantmessages/v1` ⚠️ deprecated — DEPRECATED use v2 instead
- `PATCH restapi/personal/instantmessages/v1` — Updates the text for an instant message.
- `POST restapi/personal/instantmessages/v2` — Send an instant message.

---

### `DELETE restapi/personal/instantmessages/{instantMessageId}/abusereport/v1`

Cancels abuse report.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| instantMessageId | The original abuse message identifier. | integer, 64 bit | Required |

**Response:** `InstantMessage` — Returns the object for the specific to refresh its status on the client side.

| Name | Description | Type | Additional information |
|---|---|---|---|
| MessageId | The unique id for the message, in scope of a site (itslearning customer) | integer, 64 bit |  |
| MessageThreadId | Obsolete use InstantMessageThreadId instead | integer |  |
| InstantMessageThreadId | The unique id for the thread in scope of a site | integer |  |
| MessageThreadParticipants | Recipients for new message threads - does not need to be filled if not used when posting a new thread | Collection of InstantMessageParticipant |  |
| Created | DateTime when the message was created | date |  |
| CreatedFormatted | Formatted DateTime when the message was created | string |  |
| CreatedRelative | How long ago the message was created | string |  |
| EditedRelative | How long ago the message was edited | string |  |
| CreatedBy | Id of the author of the message | integer |  |
| CreatedByName | Name of the author of the message | string |  |
| HomeOrganization | Home organization of the author of the message. | string |  |
| CreatedByAvatar | Url to the avatar of the author | string |  |
| Text | The message text | string |  |
| AttachmentUrl | The url to the attached file | string |  |
| AttachmentName | File name of the attached file | string |  |
| Link | The link to combine together with the message text | string |  |
| LinkTitle | Link title to create a friendly name for the link | string |  |
| LinkTarget | The target to the link eg. _blank | string |  |
| IsSystemMessage | Is the message a system message | boolean |  |
| IsDeleted | Is the message deleted | boolean |  |
| DeletedBy | Id of the person, who deleted this message. | integer |  |
| CanDelete | If the current user is allowed to delete the message | boolean |  |
| IsEdited | Is the message edited | boolean |  |
| CanEdit | If the current user is allowed to edit the message | boolean |  |
| IsStarred | If this message has been marked as starred by the user | boolean |  |
| IsSilent | Flag used for messages we should not update the UI for | boolean |  |
| IsAbuse | This message was reported as abusive by the current user. | boolean |  |
| HasShared | The message is used to share another message. | boolean |  |
| HasRepliedTo | The message is used to reply to another message. | boolean |  |
| HasAbuseReported | The message is an abuse report. It has a child message that was marked as abusive message. | boolean |  |
| HasAbuseThreadReported | The message is an report for abusive thread. | boolean |  |
| OriginInstantMessage | The origin message for child message. | InstantMessage |  |
| OriginInstantMessageThreadId | The origin instant message thread identifier. | integer |  |
| OriginInstantMessageThreadDeletedBy | Id of the person, who delete original instant message thread. | integer |  |
| OriginInstantMessageThreadName | The origin instant message thread name. | string |  |
| ChildMessage | The child message is shared by the message. | InstantMessage |  |
| AdditionalHeading | Additional heading to display. Extra information about where the message had its origin if a message was shared or replied to. | string |  |
| IsBroadcastMassMessage | This message was sent as broadcast mass message. | boolean |  |
| IsChildMessageDeletedPermanently | Defines that child message has been deleted permanently | boolean |  |
| CreatedLocal | DateTime when the message was created converted to user's timezone | date |  |
| CreatedLocalTimeStamp | Time when the message was created in user's timezone. | string |  |
| CreatedLocalDateStamp | Date when the message was created in user's timezone formatted considering culture. | string |  |
| CreatedByShortName | First name of the author. | string |  |
| CreatedLocalShortNumericDate | Date when the message was created in user's timezone formatted as short numeric date without year. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "MessageId": 1,
  "MessageThreadId": 2,
  "InstantMessageThreadId": 3,
  "MessageThreadParticipants": [
    {
      "PersonId": 1,
      "Name": "sample string 1",
      "AvatarUrl": "sample string 2",
      "LocationType": 1,
      "LocationId": 1
    }
  ],
  "Created": "2026-11-18T16:32:22Z",
  "CreatedFormatted": "sample string 5",
  "CreatedRelative": "sample string 6",
  "EditedRelative": "sample string 7",
  "CreatedBy": 8,
  "CreatedByName": "sample string 9",
  "HomeOrganization": "sample string 10",
  "CreatedByAvatar": "sample string 11",
  "Text": "sample string 12",
  "AttachmentUrl": "sample string 13",
  "AttachmentName": "sample string 14",
  "Link": "sample string 15",
  "LinkTitle": "sample string 16",
  "LinkTarget": "sample string 17",
  "IsSystemMessage": true,
  "IsDeleted": true,
  "DeletedBy": 20,
  "CanDelete": true,
  "IsEdited": true,
  "CanEdit": true,
  "IsStarred": true,
  "IsSilent": true,
  "IsAbuse": true,
  "HasShared": true,
  "HasRepliedTo": true,
  "HasAbuseReported": true,
  "HasAbuseThreadReported": true,
  "OriginInstantMessage": {
    "MessageId": 1,
    "MessageThreadId": 2,
    "InstantMessageThreadId": 3,
    "MessageThreadParticipants": [
      {
        "PersonId": 1,
        "Name": "sample string 1",
        "AvatarUrl": "sample string 2",
        "LocationType": 1,
        "LocationId": 1
      }
    ],
    "Created": "2026-11-18T16:32:22Z",
    "CreatedFormatted": "sample string 5",
    "CreatedRelative": "sample string 6",
    "EditedRelative": "sample string 7",
    "CreatedBy": 8,
    "CreatedByName": "sample string 9",
    "HomeOrganization": "sample string 10",
... (truncated)
```
</details>


### `PATCH restapi/personal/instantmessages/{instantMessageId}/restore/v1`

Undo deletion for an instant message.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| instantMessageId | The instant message identifier for the message to be restored. | integer, 64 bit | Required |

**Response:** `InstantMessage`

| Name | Description | Type | Additional information |
|---|---|---|---|
| MessageId | The unique id for the message, in scope of a site (itslearning customer) | integer, 64 bit |  |
| MessageThreadId | Obsolete use InstantMessageThreadId instead | integer |  |
| InstantMessageThreadId | The unique id for the thread in scope of a site | integer |  |
| MessageThreadParticipants | Recipients for new message threads - does not need to be filled if not used when posting a new thread | Collection of InstantMessageParticipant |  |
| Created | DateTime when the message was created | date |  |
| CreatedFormatted | Formatted DateTime when the message was created | string |  |
| CreatedRelative | How long ago the message was created | string |  |
| EditedRelative | How long ago the message was edited | string |  |
| CreatedBy | Id of the author of the message | integer |  |
| CreatedByName | Name of the author of the message | string |  |
| HomeOrganization | Home organization of the author of the message. | string |  |
| CreatedByAvatar | Url to the avatar of the author | string |  |
| Text | The message text | string |  |
| AttachmentUrl | The url to the attached file | string |  |
| AttachmentName | File name of the attached file | string |  |
| Link | The link to combine together with the message text | string |  |
| LinkTitle | Link title to create a friendly name for the link | string |  |
| LinkTarget | The target to the link eg. _blank | string |  |
| IsSystemMessage | Is the message a system message | boolean |  |
| IsDeleted | Is the message deleted | boolean |  |
| DeletedBy | Id of the person, who deleted this message. | integer |  |
| CanDelete | If the current user is allowed to delete the message | boolean |  |
| IsEdited | Is the message edited | boolean |  |
| CanEdit | If the current user is allowed to edit the message | boolean |  |
| IsStarred | If this message has been marked as starred by the user | boolean |  |
| IsSilent | Flag used for messages we should not update the UI for | boolean |  |
| IsAbuse | This message was reported as abusive by the current user. | boolean |  |
| HasShared | The message is used to share another message. | boolean |  |
| HasRepliedTo | The message is used to reply to another message. | boolean |  |
| HasAbuseReported | The message is an abuse report. It has a child message that was marked as abusive message. | boolean |  |
| HasAbuseThreadReported | The message is an report for abusive thread. | boolean |  |
| OriginInstantMessage | The origin message for child message. | InstantMessage |  |
| OriginInstantMessageThreadId | The origin instant message thread identifier. | integer |  |
| OriginInstantMessageThreadDeletedBy | Id of the person, who delete original instant message thread. | integer |  |
| OriginInstantMessageThreadName | The origin instant message thread name. | string |  |
| ChildMessage | The child message is shared by the message. | InstantMessage |  |
| AdditionalHeading | Additional heading to display. Extra information about where the message had its origin if a message was shared or replied to. | string |  |
| IsBroadcastMassMessage | This message was sent as broadcast mass message. | boolean |  |
| IsChildMessageDeletedPermanently | Defines that child message has been deleted permanently | boolean |  |
| CreatedLocal | DateTime when the message was created converted to user's timezone | date |  |
| CreatedLocalTimeStamp | Time when the message was created in user's timezone. | string |  |
| CreatedLocalDateStamp | Date when the message was created in user's timezone formatted considering culture. | string |  |
| CreatedByShortName | First name of the author. | string |  |
| CreatedLocalShortNumericDate | Date when the message was created in user's timezone formatted as short numeric date without year. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "MessageId": 1,
  "MessageThreadId": 2,
  "InstantMessageThreadId": 3,
  "MessageThreadParticipants": [
    {
      "PersonId": 1,
      "Name": "sample string 1",
      "AvatarUrl": "sample string 2",
      "LocationType": 1,
      "LocationId": 1
    }
  ],
  "Created": "2026-11-18T16:30:06Z",
  "CreatedFormatted": "sample string 5",
  "CreatedRelative": "sample string 6",
  "EditedRelative": "sample string 7",
  "CreatedBy": 8,
  "CreatedByName": "sample string 9",
  "HomeOrganization": "sample string 10",
  "CreatedByAvatar": "sample string 11",
  "Text": "sample string 12",
  "AttachmentUrl": "sample string 13",
  "AttachmentName": "sample string 14",
  "Link": "sample string 15",
  "LinkTitle": "sample string 16",
  "LinkTarget": "sample string 17",
  "IsSystemMessage": true,
  "IsDeleted": true,
  "DeletedBy": 20,
  "CanDelete": true,
  "IsEdited": true,
  "CanEdit": true,
  "IsStarred": true,
  "IsSilent": true,
  "IsAbuse": true,
  "HasShared": true,
  "HasRepliedTo": true,
  "HasAbuseReported": true,
  "HasAbuseThreadReported": true,
  "OriginInstantMessage": {
    "MessageId": 1,
    "MessageThreadId": 2,
    "InstantMessageThreadId": 3,
    "MessageThreadParticipants": [
      {
        "PersonId": 1,
        "Name": "sample string 1",
        "AvatarUrl": "sample string 2",
        "LocationType": 1,
        "LocationId": 1
      }
    ],
    "Created": "2026-11-18T16:30:06Z",
    "CreatedFormatted": "sample string 5",
    "CreatedRelative": "sample string 6",
    "EditedRelative": "sample string 7",
    "CreatedBy": 8,
    "CreatedByName": "sample string 9",
    "HomeOrganization": "sample string 10",
... (truncated)
```
</details>


### `DELETE restapi/personal/instantmessages/{instantMessageId}/v1`

Deletes an instant message.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| instantMessageId | The instant message identifier for the message to delete. | integer, 64 bit | Required |

**Response:** `InstantMessage` — Returns instant message .

| Name | Description | Type | Additional information |
|---|---|---|---|
| MessageId | The unique id for the message, in scope of a site (itslearning customer) | integer, 64 bit |  |
| MessageThreadId | Obsolete use InstantMessageThreadId instead | integer |  |
| InstantMessageThreadId | The unique id for the thread in scope of a site | integer |  |
| MessageThreadParticipants | Recipients for new message threads - does not need to be filled if not used when posting a new thread | Collection of InstantMessageParticipant |  |
| Created | DateTime when the message was created | date |  |
| CreatedFormatted | Formatted DateTime when the message was created | string |  |
| CreatedRelative | How long ago the message was created | string |  |
| EditedRelative | How long ago the message was edited | string |  |
| CreatedBy | Id of the author of the message | integer |  |
| CreatedByName | Name of the author of the message | string |  |
| HomeOrganization | Home organization of the author of the message. | string |  |
| CreatedByAvatar | Url to the avatar of the author | string |  |
| Text | The message text | string |  |
| AttachmentUrl | The url to the attached file | string |  |
| AttachmentName | File name of the attached file | string |  |
| Link | The link to combine together with the message text | string |  |
| LinkTitle | Link title to create a friendly name for the link | string |  |
| LinkTarget | The target to the link eg. _blank | string |  |
| IsSystemMessage | Is the message a system message | boolean |  |
| IsDeleted | Is the message deleted | boolean |  |
| DeletedBy | Id of the person, who deleted this message. | integer |  |
| CanDelete | If the current user is allowed to delete the message | boolean |  |
| IsEdited | Is the message edited | boolean |  |
| CanEdit | If the current user is allowed to edit the message | boolean |  |
| IsStarred | If this message has been marked as starred by the user | boolean |  |
| IsSilent | Flag used for messages we should not update the UI for | boolean |  |
| IsAbuse | This message was reported as abusive by the current user. | boolean |  |
| HasShared | The message is used to share another message. | boolean |  |
| HasRepliedTo | The message is used to reply to another message. | boolean |  |
| HasAbuseReported | The message is an abuse report. It has a child message that was marked as abusive message. | boolean |  |
| HasAbuseThreadReported | The message is an report for abusive thread. | boolean |  |
| OriginInstantMessage | The origin message for child message. | InstantMessage |  |
| OriginInstantMessageThreadId | The origin instant message thread identifier. | integer |  |
| OriginInstantMessageThreadDeletedBy | Id of the person, who delete original instant message thread. | integer |  |
| OriginInstantMessageThreadName | The origin instant message thread name. | string |  |
| ChildMessage | The child message is shared by the message. | InstantMessage |  |
| AdditionalHeading | Additional heading to display. Extra information about where the message had its origin if a message was shared or replied to. | string |  |
| IsBroadcastMassMessage | This message was sent as broadcast mass message. | boolean |  |
| IsChildMessageDeletedPermanently | Defines that child message has been deleted permanently | boolean |  |
| CreatedLocal | DateTime when the message was created converted to user's timezone | date |  |
| CreatedLocalTimeStamp | Time when the message was created in user's timezone. | string |  |
| CreatedLocalDateStamp | Date when the message was created in user's timezone formatted considering culture. | string |  |
| CreatedByShortName | First name of the author. | string |  |
| CreatedLocalShortNumericDate | Date when the message was created in user's timezone formatted as short numeric date without year. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "MessageId": 1,
  "MessageThreadId": 2,
  "InstantMessageThreadId": 3,
  "MessageThreadParticipants": [
    {
      "PersonId": 1,
      "Name": "sample string 1",
      "AvatarUrl": "sample string 2",
      "LocationType": 1,
      "LocationId": 1
    }
  ],
  "Created": "2026-11-18T16:33:14Z",
  "CreatedFormatted": "sample string 5",
  "CreatedRelative": "sample string 6",
  "EditedRelative": "sample string 7",
  "CreatedBy": 8,
  "CreatedByName": "sample string 9",
  "HomeOrganization": "sample string 10",
  "CreatedByAvatar": "sample string 11",
  "Text": "sample string 12",
  "AttachmentUrl": "sample string 13",
  "AttachmentName": "sample string 14",
  "Link": "sample string 15",
  "LinkTitle": "sample string 16",
  "LinkTarget": "sample string 17",
  "IsSystemMessage": true,
  "IsDeleted": true,
  "DeletedBy": 20,
  "CanDelete": true,
  "IsEdited": true,
  "CanEdit": true,
  "IsStarred": true,
  "IsSilent": true,
  "IsAbuse": true,
  "HasShared": true,
  "HasRepliedTo": true,
  "HasAbuseReported": true,
  "HasAbuseThreadReported": true,
  "OriginInstantMessage": {
    "MessageId": 1,
    "MessageThreadId": 2,
    "InstantMessageThreadId": 3,
    "MessageThreadParticipants": [
      {
        "PersonId": 1,
        "Name": "sample string 1",
        "AvatarUrl": "sample string 2",
        "LocationType": 1,
        "LocationId": 1
      }
    ],
    "Created": "2026-11-18T16:33:14Z",
    "CreatedFormatted": "sample string 5",
    "CreatedRelative": "sample string 6",
    "EditedRelative": "sample string 7",
    "CreatedBy": 8,
    "CreatedByName": "sample string 9",
    "HomeOrganization": "sample string 10",
... (truncated)
```
</details>


### `POST restapi/personal/instantmessages/attachment/v1`

Upload file attachment to temporary storage. The file should be included in the post call so we can extract it as HttpContext.Current.Request.Files

**Response:** `?` — A list of tuples with file repo guids to identify temporary file, and the name of the file

| Name | Description | Type | Additional information |
|---|---|---|---|
| Item1 |  | globally unique identifier |  |
| Item2 |  | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "m_Item1": "3b55ce80-5182-45f3-8663-c661adc62a7d",
    "m_Item2": "sample string 2"
  }
]
```
</details>


### `PUT restapi/personal/instantmessages/cancelabusethreadreport/{threadId}/v1`

Cancels abuse report for thread.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The original abuse thread identifier. | integer | Required |

**Response:** `?`


### `POST restapi/personal/instantmessages/communicationchannel/{channelId}/v1?unregister={unregister}`

Register a (SignalR) communication channel

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| channelId | The string identifier recieved from the SignalR server | string | Required |
| unregister | The old string identifier recieved from the SignalR server that we want to remove. | string | Default value is |

**Response:** `?` — 200 OK or exception


### `GET restapi/personal/instantmessages/messagethreads/{instantMessageThreadId}/unread/count/v1`

Gets number of unread messages in a specific thread for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| instantMessageThreadId | The id of the thread we want the number of unread messages from | integer | Required |

**Response:** `integer` — The number of unread messages in the given thread

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/messages/search/v1?searchText={searchText}`

Searches inside the given thread for messages matching the given search text.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread id. | integer | Required |
| searchText | Text to search in message thread | string | Required |

**Response:** `Collection of integer, 64 bit` — Array with message ids that match the text

<details><summary>Response JSON sample</summary>

```json
[
  1
]
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/messages/v2?maxMessages={maxMessages}&upperBoundInstantMessageId={upperBoundInstantMessageId}&lowerBoundInstantMessageId={lowerBoundInstantMessageId}`

Gets the messages in a specific thread. The impersonated user need to be a member of the thread. One cannot use both lowerBoundInstantMessageId and upperBoundInstantMessageId

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread id | integer | Required |
| maxMessages | Maximum number of messages to get. Default value is 100. Maximum value is 500 | integer |  |
| upperBoundInstantMessageId | Load messages before this instant message id | integer, 64 bit |  |
| lowerBoundInstantMessageId | Load messages after this instant message id | integer, 64 bit |  |

**Response:** `EntityList<InstantMessage>` — List of InstantMessage entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of InstantMessage |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "MessageId": 1,
      "MessageThreadId": 2,
      "InstantMessageThreadId": 3,
      "MessageThreadParticipants": [
        {
          "PersonId": 1,
          "Name": "sample string 1",
          "AvatarUrl": "sample string 2",
          "LocationType": 1,
          "LocationId": 1
        }
      ],
      "Created": "2026-11-18T16:30:06Z",
      "CreatedFormatted": "sample string 5",
      "CreatedRelative": "sample string 6",
      "EditedRelative": "sample string 7",
      "CreatedBy": 8,
      "CreatedByName": "sample string 9",
      "HomeOrganization": "sample string 10",
      "CreatedByAvatar": "sample string 11",
      "Text": "sample string 12",
      "AttachmentUrl": "sample string 13",
      "AttachmentName": "sample string 14",
      "Link": "sample string 15",
      "LinkTitle": "sample string 16",
      "LinkTarget": "sample string 17",
      "IsSystemMessage": true,
      "IsDeleted": true,
      "DeletedBy": 20,
      "CanDelete": true,
      "IsEdited": true,
      "CanEdit": true,
      "IsStarred": true,
      "IsSilent": true,
      "IsAbuse": true,
      "HasShared": true,
      "HasRepliedTo": true,
      "HasAbuseReported": true,
      "HasAbuseThreadReported": true,
      "OriginInstantMessage": {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:30:06Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/messages/v3?pageSize={pageSize}&fromId={fromId}`

Gets a collection of instant messages extended with hasMore field.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | Thread to get messages for. | integer | Required |
| pageSize | Max number of messages to get. | integer |  |
| fromId | Only messages with lower ids will be retrieved. | integer, 64 bit |  |

**Response:** `InstantMessageListModel` — to send.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Messages | List of the messages itself. | EntityList<InstantMessage> |  |
| HasMore | If there is more data left to retrieve. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:34:29Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
              "LocationId": 1
            }
          ],
          "Created": "2026-11-18T16:34:29Z",
          "CreatedFormatted": "sample string 5",
          "CreatedRelative": "sample string 6",
          "EditedRelative": "sample string 7",
... (truncated)
```
</details>


### `PUT restapi/personal/instantmessages/messagethreads/{threadId}/moderators/v1`

Adds moderator to an instant message thread.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The instant message thread Id | integer | Required |

**Body** (Updatable.InstantMessageThreadUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The Id of the message thread to update | integer |  |
| UpdateName | Determines if the name should be updated | boolean |  |
| UpdateParticipants | Determines if the list of participants should be updated | boolean |  |
| Name | The new name of the thread | string |  |
| PersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId need to have a valid value. | Collection of integer |  |
| ToggleEnableDisableReplies | Determines if the option to send to close the conversation should be updated | boolean |  |
| RepliesAreEnabled | Whether students can reply to the conversation or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "UpdateName": true,
  "UpdateParticipants": true,
  "Name": "sample string 4",
  "PersonIds": [
    1
  ],
  "ToggleEnableDisableReplies": true,
  "RepliesAreEnabled": true
}
```
</details>

**Response:** `InstantMessageThread` — The updated instant message thread

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:35:34Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:35:34Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/participants/messages-disabled/v1`

Gets a collection of thread participants who cannot see messages.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | Thread ID. | integer | Required |

**Response:** `MessagesDisabledThreadParticipants` — List of InstantMessageThread entities.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Count | Number of instant message conversation participants who have messages disabled. | integer |  |
| ThreadParticipants | The list of instant message conversation participants. Can be null in case if there's no such participants or if there are too many of them. | Collection of ThreadParticipant |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Count": 1,
  "ThreadParticipants": [
    {
      "SourceLocationId": 1,
      "SourceLocationType": 1,
      "IsDeleted": true,
      "CanReceive": true,
      "RoleInHierarchyTitle": "sample string 3",
      "RoleInHierarchy": 0,
      "ExtraUserInformation": "sample string 4",
      "PersonId": 5,
      "FirstName": "sample string 6",
      "LastName": "sample string 7",
      "FullName": "sample string 8",
      "ProfileUrl": "sample string 9",
      "AdditionalInfo": "sample string 10",
      "ProfileImageUrl": "sample string 11",
      "ProfileImageUrlSmall": "sample string 12"
    }
  ]
}
```
</details>


### `PUT restapi/personal/instantmessages/messagethreads/{threadId}/participants/v1`

Adds participants to an instant message thread.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The instant message thread Id | integer | Required |

**Body** (Updatable.InstantMessageThreadUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The Id of the message thread to update | integer |  |
| UpdateName | Determines if the name should be updated | boolean |  |
| UpdateParticipants | Determines if the list of participants should be updated | boolean |  |
| Name | The new name of the thread | string |  |
| PersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId need to have a valid value. | Collection of integer |  |
| ToggleEnableDisableReplies | Determines if the option to send to close the conversation should be updated | boolean |  |
| RepliesAreEnabled | Whether students can reply to the conversation or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "UpdateName": true,
  "UpdateParticipants": true,
  "Name": "sample string 4",
  "PersonIds": [
    1
  ],
  "ToggleEnableDisableReplies": true,
  "RepliesAreEnabled": true
}
```
</details>

**Response:** `InstantMessageThread` — The updated instant message thread

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:35:31Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:35:31Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `DELETE restapi/personal/instantmessages/messagethreads/{threadId}/participants/v1`

Remove participants from an instant message thread.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The instant message thread Id | integer | Required |

**Body** (Updatable.InstantMessageThreadUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The Id of the message thread to update | integer |  |
| UpdateName | Determines if the name should be updated | boolean |  |
| UpdateParticipants | Determines if the list of participants should be updated | boolean |  |
| Name | The new name of the thread | string |  |
| PersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId need to have a valid value. | Collection of integer |  |
| ToggleEnableDisableReplies | Determines if the option to send to close the conversation should be updated | boolean |  |
| RepliesAreEnabled | Whether students can reply to the conversation or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "UpdateName": true,
  "UpdateParticipants": true,
  "Name": "sample string 4",
  "PersonIds": [
    1
  ],
  "ToggleEnableDisableReplies": true,
  "RepliesAreEnabled": true
}
```
</details>

**Response:** `InstantMessageThread` — The updated instant message thread

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:32:22Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:32:22Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `PATCH restapi/personal/instantmessages/messagethreads/{threadId}/restore/v1`

Restores deletion for thread.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread identifier to restore. | integer | Required |

**Response:** `InstantMessageThread`

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:31:28Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:31:28Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `PUT restapi/personal/instantmessages/messagethreads/{threadId}/toggleEnableDisableReplies/v1`

Toggle whether students can send replies to this thread or not

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId |  | string |  |

**Body** (Updatable.InstantMessageThreadUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The Id of the message thread to update | integer |  |
| UpdateName | Determines if the name should be updated | boolean |  |
| UpdateParticipants | Determines if the list of participants should be updated | boolean |  |
| Name | The new name of the thread | string |  |
| PersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId need to have a valid value. | Collection of integer |  |
| ToggleEnableDisableReplies | Determines if the option to send to close the conversation should be updated | boolean |  |
| RepliesAreEnabled | Whether students can reply to the conversation or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "UpdateName": true,
  "UpdateParticipants": true,
  "Name": "sample string 4",
  "PersonIds": [
    1
  ],
  "ToggleEnableDisableReplies": true,
  "RepliesAreEnabled": true
}
```
</details>

**Response:** `InstantMessageThread`

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:35:34Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:35:34Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `PUT restapi/personal/instantmessages/messagethreads/{threadId}/updatelastread/{lastReadInstantMessageId}/v1?sortIndex={sortIndex}&operation={operation}`

Updates last read instant message id for a thread

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The id of the thread to update | integer | Required |
| lastReadInstantMessageId | The id for the last read instant message | integer, 64 bit | Required |
| sortIndex | The ID of the last meaningful message for the person which can optionally be set. | integer, 64 bit |  |
| operation | The operaiton type for KPI which can optionally be set. | string |  |

**Response:** `?` — Returns 200 OK


### `DELETE restapi/personal/instantmessages/messagethreads/{threadId}/v1`

Deletes a thread.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread identifier to delete. | integer | Required |

**Response:** `?`


### `PUT restapi/personal/instantmessages/messagethreads/{threadId}/v1`

Updates a instant message thread. Supports updating the name of the thread or the list of the participants. Will return HTTP status 404 if the InstantMessageThreadId is not found or the user does not have access to it.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The instant message thread Id | integer | Required |

**Body** (Updatable.InstantMessageThreadUpdateV1)

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The Id of the message thread to update | integer |  |
| UpdateName | Determines if the name should be updated | boolean |  |
| UpdateParticipants | Determines if the list of participants should be updated | boolean |  |
| Name | The new name of the thread | string |  |
| PersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId need to have a valid value. | Collection of integer |  |
| ToggleEnableDisableReplies | Determines if the option to send to close the conversation should be updated | boolean |  |
| RepliesAreEnabled | Whether students can reply to the conversation or not | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "UpdateName": true,
  "UpdateParticipants": true,
  "Name": "sample string 4",
  "PersonIds": [
    1
  ],
  "ToggleEnableDisableReplies": true,
  "RepliesAreEnabled": true
}
```
</details>

**Response:** `InstantMessageThread` — The updated instant message thread

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:34:59Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:34:59Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/v1?maxMessages={maxMessages}&upperBoundInstantMessageId={upperBoundInstantMessageId}&lowerBoundInstantMessageId={lowerBoundInstantMessageId}`

DEPRECATED use "messagethreads/{threadId}/messages/v2" instead. Gets the messages in a specific thread if the user is a recipient in the thread. One cannot use both lowerBoundInstantMessageId and upperBoundInstantMessageId

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread id | integer | Required |
| maxMessages | Maximum number of messages to get | integer |  |
| upperBoundInstantMessageId | Load messages before this instant message id | integer, 64 bit |  |
| lowerBoundInstantMessageId | Load messages after this instant message id | integer, 64 bit |  |

**Response:** `EntityList<InstantMessage>` — List of InstantMessage entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of InstantMessage |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "MessageId": 1,
      "MessageThreadId": 2,
      "InstantMessageThreadId": 3,
      "MessageThreadParticipants": [
        {
          "PersonId": 1,
          "Name": "sample string 1",
          "AvatarUrl": "sample string 2",
          "LocationType": 1,
          "LocationId": 1
        }
      ],
      "Created": "2026-11-18T16:30:06Z",
      "CreatedFormatted": "sample string 5",
      "CreatedRelative": "sample string 6",
      "EditedRelative": "sample string 7",
      "CreatedBy": 8,
      "CreatedByName": "sample string 9",
      "HomeOrganization": "sample string 10",
      "CreatedByAvatar": "sample string 11",
      "Text": "sample string 12",
      "AttachmentUrl": "sample string 13",
      "AttachmentName": "sample string 14",
      "Link": "sample string 15",
      "LinkTitle": "sample string 16",
      "LinkTarget": "sample string 17",
      "IsSystemMessage": true,
      "IsDeleted": true,
      "DeletedBy": 20,
      "CanDelete": true,
      "IsEdited": true,
      "CanEdit": true,
      "IsStarred": true,
      "IsSilent": true,
      "IsAbuse": true,
      "HasShared": true,
      "HasRepliedTo": true,
      "HasAbuseReported": true,
      "HasAbuseThreadReported": true,
      "OriginInstantMessage": {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:30:06Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/v2?maxMessages={maxMessages}&upperBoundInstantMessageId={upperBoundInstantMessageId}&lowerBoundInstantMessageId={lowerBoundInstantMessageId}`

Gets the instant message thread and the messages in a specific thread. The impersonated user need to be a member of the thread. One cannot use both lowerBoundInstantMessageId and upperBoundInstantMessageId. If you only need messages from the thread, consider using "messagethreads/{threadId}/messages/v2" instead.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread id | integer | Required |
| maxMessages | Maximum number of messages to get. Default value is 100. Maximum value is 500 | integer |  |
| upperBoundInstantMessageId | Load messages before this instant message id | integer, 64 bit |  |
| lowerBoundInstantMessageId | Load messages after this instant message id | integer, 64 bit |  |

**Response:** `InstantMessageThread` — List of InstantMessage entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:32:22Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:32:22Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/{threadId}/v3?maxMessages={maxMessages}&upperBoundInstantMessageId={upperBoundInstantMessageId}&lowerBoundInstantMessageId={lowerBoundInstantMessageId}`

Gets the instant message thread and the messages in a specific thread. The impersonated user need to be a member of the thread. One cannot use both lowerBoundInstantMessageId and upperBoundInstantMessageId. If you only need messages from the thread, consider using "messagethreads/{threadId}/messages/v2" instead. This version doesn't return thread participants for course threads for performance reasons.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| threadId | The thread id | integer | Required |
| maxMessages | Maximum number of messages to get. Default value is 100. Maximum value is 500 | integer |  |
| upperBoundInstantMessageId | Load messages before this instant message id | integer, 64 bit |  |
| lowerBoundInstantMessageId | Load messages after this instant message id | integer, 64 bit |  |

**Response:** `InstantMessageThread` — List of InstantMessage entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:33:14Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:33:14Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/collaborations/{collaborationId}/v1`

Gets the instant message thread id for group answer. Creates it if not there yet.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| collaborationId | The collaboration identifier. | integer | Required |

**Response:** `InstantMessageThread` — an instant message thread instance.

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:32:42Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:32:42Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/onetoone/{personId}/v1`

Gets the instant message thread id for a one to one conversation between the caller and a user.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | The person identifier. | integer | Required |

**Response:** `integer` — a instant message thread id for the one to one conversation. 0 if the instant message thread does not exists.

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `POST restapi/personal/instantmessages/messagethreads/search/v1`

Searches the logged in user's message threads. PersonId is gotten from the authorization (session or oauth)

**Body** (InstantMessageThreadSearchParams)

| Name | Description | Type | Additional information |
|---|---|---|---|
| MaxThreadCount | Maximum number of threads to get (if unspecified a default of 15 will be used) | integer |  |
| ThreadPage | The page number of the result set to get (if unspecified, the first page will be returned) | integer |  |
| MaxMessages | The maximum number of messages to preload for each search result (if unspecified a default of 15 will be used) | integer |  |
| SearchText | The text to search for in messages from the threads in the result set | string |  |
| PersonIdFilter | An optional filter to search only in messages from the specified person id | integer |  |
| ThreadIdFilter | An optional filter to limit the results to a single thread | integer |  |
| AttachmentFilter | An optional filter to limit results to have at least one attachment | boolean |  |
| SentFilter | An optional filter to limit results to have at least one sent message from the current user | boolean |  |
| UnreadFilter | An optional filter to limit results to have at least one unread message | boolean |  |
| StarredFilter | An optional filter to limit results to messages starred by the current user | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "MaxThreadCount": 1,
  "ThreadPage": 1,
  "MaxMessages": 1,
  "SearchText": "sample string 1",
  "PersonIdFilter": 1,
  "ThreadIdFilter": 1,
  "AttachmentFilter": true,
  "SentFilter": true,
  "UnreadFilter": true,
  "StarredFilter": true
}
```
</details>

**Response:** `EntityList<InstantMessageThread>` — List of InstantMessageThread entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of InstantMessageThread |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "InstantMessageThreadId": 1,
      "Name": "sample string 2",
      "Created": "2026-11-18T16:33:46Z",
      "CreatedByTeacher": true,
      "CreatedBy": 5,
      "Type": "sample string 6",
      "ColorSchema": "sample string 7",
      "Messages": {
        "EntityArray": [
          {
            "MessageId": 1,
            "MessageThreadId": 2,
            "InstantMessageThreadId": 3,
            "MessageThreadParticipants": [
              {
                "PersonId": 1,
                "Name": "sample string 1",
                "AvatarUrl": "sample string 2",
                "LocationType": 1,
                "LocationId": 1
              }
            ],
            "Created": "2026-11-18T16:33:46Z",
            "CreatedFormatted": "sample string 5",
            "CreatedRelative": "sample string 6",
            "EditedRelative": "sample string 7",
            "CreatedBy": 8,
            "CreatedByName": "sample string 9",
            "HomeOrganization": "sample string 10",
            "CreatedByAvatar": "sample string 11",
            "Text": "sample string 12",
            "AttachmentUrl": "sample string 13",
            "AttachmentName": "sample string 14",
            "Link": "sample string 15",
            "LinkTitle": "sample string 16",
            "LinkTarget": "sample string 17",
            "IsSystemMessage": true,
            "IsDeleted": true,
            "DeletedBy": 20,
            "CanDelete": true,
            "IsEdited": true,
            "CanEdit": true,
            "IsStarred": true,
            "IsSilent": true,
            "IsAbuse": true,
            "HasShared": true,
            "HasRepliedTo": true,
            "HasAbuseReported": true,
            "HasAbuseThreadReported": true,
            "OriginInstantMessage": {
              "MessageId": 1,
              "MessageThreadId": 2,
              "InstantMessageThreadId": 3,
              "MessageThreadParticipants": [
                {
                  "PersonId": 1,
                  "Name": "sample string 1",
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/suggestions/v1?searchText={searchText}`

Suggests conversations which names contain search string.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| searchText | Text to search. | string | Required |

**Response:** `?` — The list of conversations which names contain search string.

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "InstantMessageThreadId": 1,
    "Name": "sample string 2",
    "Created": "2026-11-18T16:35:31Z",
    "CreatedByTeacher": true,
    "CreatedBy": 5,
    "Type": "sample string 6",
    "ColorSchema": "sample string 7",
    "Messages": {
      "EntityArray": [
        {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
              "LocationId": 1
            }
          ],
          "Created": "2026-11-18T16:35:31Z",
          "CreatedFormatted": "sample string 5",
          "CreatedRelative": "sample string 6",
          "EditedRelative": "sample string 7",
          "CreatedBy": 8,
          "CreatedByName": "sample string 9",
          "HomeOrganization": "sample string 10",
          "CreatedByAvatar": "sample string 11",
          "Text": "sample string 12",
          "AttachmentUrl": "sample string 13",
          "AttachmentName": "sample string 14",
          "Link": "sample string 15",
          "LinkTitle": "sample string 16",
          "LinkTarget": "sample string 17",
          "IsSystemMessage": true,
          "IsDeleted": true,
          "DeletedBy": 20,
          "CanDelete": true,
          "IsEdited": true,
          "CanEdit": true,
          "IsStarred": true,
          "IsSilent": true,
          "IsAbuse": true,
          "HasShared": true,
          "HasRepliedTo": true,
          "HasAbuseReported": true,
          "HasAbuseThreadReported": true,
          "OriginInstantMessage": {
            "MessageId": 1,
            "MessageThreadId": 2,
            "InstantMessageThreadId": 3,
            "MessageThreadParticipants": [
              {
                "PersonId": 1,
                "Name": "sample string 1",
                "AvatarUrl": "sample string 2",
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/unread/count/v1`

Gets number of threads that contain unread messages

**Response:** `integer` — Number of threads that contain unread messages

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/v1?maxThreadCount={maxThreadCount}&threadPage={threadPage}&maxMessages={maxMessages}`

Gets the logged in user's message threads. PersonId is gotten from the authorization (session or oauth)

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| maxThreadCount | Maximum number of threads to get | integer |  |
| threadPage | Page index for threads | integer |  |
| maxMessages | Maximum number of messages to get | integer |  |

**Response:** `EntityList<InstantMessageThread>` — List of InstantMessageThread entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of InstantMessageThread |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "InstantMessageThreadId": 1,
      "Name": "sample string 2",
      "Created": "2026-11-18T16:31:28Z",
      "CreatedByTeacher": true,
      "CreatedBy": 5,
      "Type": "sample string 6",
      "ColorSchema": "sample string 7",
      "Messages": {
        "EntityArray": [
          {
            "MessageId": 1,
            "MessageThreadId": 2,
            "InstantMessageThreadId": 3,
            "MessageThreadParticipants": [
              {
                "PersonId": 1,
                "Name": "sample string 1",
                "AvatarUrl": "sample string 2",
                "LocationType": 1,
                "LocationId": 1
              }
            ],
            "Created": "2026-11-18T16:31:28Z",
            "CreatedFormatted": "sample string 5",
            "CreatedRelative": "sample string 6",
            "EditedRelative": "sample string 7",
            "CreatedBy": 8,
            "CreatedByName": "sample string 9",
            "HomeOrganization": "sample string 10",
            "CreatedByAvatar": "sample string 11",
            "Text": "sample string 12",
            "AttachmentUrl": "sample string 13",
            "AttachmentName": "sample string 14",
            "Link": "sample string 15",
            "LinkTitle": "sample string 16",
            "LinkTarget": "sample string 17",
            "IsSystemMessage": true,
            "IsDeleted": true,
            "DeletedBy": 20,
            "CanDelete": true,
            "IsEdited": true,
            "CanEdit": true,
            "IsStarred": true,
            "IsSilent": true,
            "IsAbuse": true,
            "HasShared": true,
            "HasRepliedTo": true,
            "HasAbuseReported": true,
            "HasAbuseThreadReported": true,
            "OriginInstantMessage": {
              "MessageId": 1,
              "MessageThreadId": 2,
              "InstantMessageThreadId": 3,
              "MessageThreadParticipants": [
                {
                  "PersonId": 1,
                  "Name": "sample string 1",
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/v2?maxThreadCount={maxThreadCount}&threadPage={threadPage}&maxMessages={maxMessages}`

Gets the logged in user's message threads. PersonId is gotten from the authorization (session or oauth)

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| maxThreadCount | Maximum number of threads to get | integer |  |
| threadPage | Page index for threads | integer |  |
| maxMessages | Maximum number of messages to get | integer |  |

**Response:** `EntityList<InstantMessageThread>` — List of InstantMessageThread entities

| Name | Description | Type | Additional information |
|---|---|---|---|
| EntityArray |  | Collection of InstantMessageThread |  |
| Total |  | integer |  |
| CurrentPageIndex |  | integer |  |
| PageSize |  | integer |  |

<details><summary>Response JSON sample</summary>

```json
{
  "EntityArray": [
    {
      "InstantMessageThreadId": 1,
      "Name": "sample string 2",
      "Created": "2026-11-18T16:34:59Z",
      "CreatedByTeacher": true,
      "CreatedBy": 5,
      "Type": "sample string 6",
      "ColorSchema": "sample string 7",
      "Messages": {
        "EntityArray": [
          {
            "MessageId": 1,
            "MessageThreadId": 2,
            "InstantMessageThreadId": 3,
            "MessageThreadParticipants": [
              {
                "PersonId": 1,
                "Name": "sample string 1",
                "AvatarUrl": "sample string 2",
                "LocationType": 1,
                "LocationId": 1
              }
            ],
            "Created": "2026-11-18T16:34:59Z",
            "CreatedFormatted": "sample string 5",
            "CreatedRelative": "sample string 6",
            "EditedRelative": "sample string 7",
            "CreatedBy": 8,
            "CreatedByName": "sample string 9",
            "HomeOrganization": "sample string 10",
            "CreatedByAvatar": "sample string 11",
            "Text": "sample string 12",
            "AttachmentUrl": "sample string 13",
            "AttachmentName": "sample string 14",
            "Link": "sample string 15",
            "LinkTitle": "sample string 16",
            "LinkTarget": "sample string 17",
            "IsSystemMessage": true,
            "IsDeleted": true,
            "DeletedBy": 20,
            "CanDelete": true,
            "IsEdited": true,
            "CanEdit": true,
            "IsStarred": true,
            "IsSilent": true,
            "IsAbuse": true,
            "HasShared": true,
            "HasRepliedTo": true,
            "HasAbuseReported": true,
            "HasAbuseThreadReported": true,
            "OriginInstantMessage": {
              "MessageId": 1,
              "MessageThreadId": 2,
              "InstantMessageThreadId": 3,
              "MessageThreadParticipants": [
                {
                  "PersonId": 1,
                  "Name": "sample string 1",
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/v3?fromSortIndex={fromSortIndex}&pageSize={pageSize}`

Gets a collection of threads for logged in user extended with hasMore field. Supports paging by sort index field. If parameter is not provided, threads with highest sort index will be returned. If is not provided, default page size will be used instead.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fromSortIndex | Only threads with lower index will be retrieved. | integer |  |
| pageSize | Max number of threads. | integer |  |

**Response:** `InstantMessageThreadListModel` — List of InstantMessageThread entities.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Threads | List of threads itself. | EntityList<InstantMessageThread> |  |
| HasMore | If there is more data left to retrieve. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Threads": {
    "EntityArray": [
      {
        "InstantMessageThreadId": 1,
        "Name": "sample string 2",
        "Created": "2026-11-18T16:31:46Z",
        "CreatedByTeacher": true,
        "CreatedBy": 5,
        "Type": "sample string 6",
        "ColorSchema": "sample string 7",
        "Messages": {
          "EntityArray": [
            {
              "MessageId": 1,
              "MessageThreadId": 2,
              "InstantMessageThreadId": 3,
              "MessageThreadParticipants": [
                {
                  "PersonId": 1,
                  "Name": "sample string 1",
                  "AvatarUrl": "sample string 2",
                  "LocationType": 1,
                  "LocationId": 1
                }
              ],
              "Created": "2026-11-18T16:31:46Z",
              "CreatedFormatted": "sample string 5",
              "CreatedRelative": "sample string 6",
              "EditedRelative": "sample string 7",
              "CreatedBy": 8,
              "CreatedByName": "sample string 9",
              "HomeOrganization": "sample string 10",
              "CreatedByAvatar": "sample string 11",
              "Text": "sample string 12",
              "AttachmentUrl": "sample string 13",
              "AttachmentName": "sample string 14",
              "Link": "sample string 15",
              "LinkTitle": "sample string 16",
              "LinkTarget": "sample string 17",
              "IsSystemMessage": true,
              "IsDeleted": true,
              "DeletedBy": 20,
              "CanDelete": true,
              "IsEdited": true,
              "CanEdit": true,
              "IsStarred": true,
              "IsSilent": true,
              "IsAbuse": true,
              "HasShared": true,
              "HasRepliedTo": true,
              "HasAbuseReported": true,
              "HasAbuseThreadReported": true,
              "OriginInstantMessage": {
                "MessageId": 1,
                "MessageThreadId": 2,
                "InstantMessageThreadId": 3,
                "MessageThreadParticipants": [
                  {
                    "PersonId": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/messagethreads/v4?fromSortIndex={fromSortIndex}&pageSize={pageSize}`

Gets a collection of threads for logged in user extended with hasMore field. Supports paging by sort index field. If parameter is not provided, threads with highest sort index will be returned. If is not provided, default page size will be used instead. This version doesn't return thread participants for course threads for performance reasons.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| fromSortIndex | Only threads with lower index will be retrieved. | integer |  |
| pageSize | Max number of threads. | integer |  |

**Response:** `InstantMessageThreadListModel` — List of InstantMessageThread entities.

| Name | Description | Type | Additional information |
|---|---|---|---|
| Threads | List of threads itself. | EntityList<InstantMessageThread> |  |
| HasMore | If there is more data left to retrieve. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "Threads": {
    "EntityArray": [
      {
        "InstantMessageThreadId": 1,
        "Name": "sample string 2",
        "Created": "2026-11-18T16:34:29Z",
        "CreatedByTeacher": true,
        "CreatedBy": 5,
        "Type": "sample string 6",
        "ColorSchema": "sample string 7",
        "Messages": {
          "EntityArray": [
            {
              "MessageId": 1,
              "MessageThreadId": 2,
              "InstantMessageThreadId": 3,
              "MessageThreadParticipants": [
                {
                  "PersonId": 1,
                  "Name": "sample string 1",
                  "AvatarUrl": "sample string 2",
                  "LocationType": 1,
                  "LocationId": 1
                }
              ],
              "Created": "2026-11-18T16:34:29Z",
              "CreatedFormatted": "sample string 5",
              "CreatedRelative": "sample string 6",
              "EditedRelative": "sample string 7",
              "CreatedBy": 8,
              "CreatedByName": "sample string 9",
              "HomeOrganization": "sample string 10",
              "CreatedByAvatar": "sample string 11",
              "Text": "sample string 12",
              "AttachmentUrl": "sample string 13",
              "AttachmentName": "sample string 14",
              "Link": "sample string 15",
              "LinkTitle": "sample string 16",
              "LinkTarget": "sample string 17",
              "IsSystemMessage": true,
              "IsDeleted": true,
              "DeletedBy": 20,
              "CanDelete": true,
              "IsEdited": true,
              "CanEdit": true,
              "IsStarred": true,
              "IsSilent": true,
              "IsAbuse": true,
              "HasShared": true,
              "HasRepliedTo": true,
              "HasAbuseReported": true,
              "HasAbuseThreadReported": true,
              "OriginInstantMessage": {
                "MessageId": 1,
                "MessageThreadId": 2,
                "InstantMessageThreadId": 3,
                "MessageThreadParticipants": [
                  {
                    "PersonId": 1,
... (truncated)
```
</details>


### `GET restapi/personal/instantmessages/permissions/v1`

Gets permissions for the instant message system.

**Response:** `InstantMessagePermissions` — The permissions that the current user has for InstantMessaging

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanUseSendAsIndividualMessages | Gets if the user is allowed to send a message to multiple recipients as individual messages (as opposed to a group conversation) | boolean |  |
| CanAccessInstantMessageSystem | Gets if a user can access instant message system. | boolean |  |
| CanSearchForCourses | Gets if a user can search for courses to use them as recipients. | boolean |  |
| CanShareInstantMessages | Get if the user can share messages. | boolean |  |
| CanReplyToInstantMessages | Get if the user can reply to instant messages. | boolean |  |
| CanStarInstantMessages | Get if the user can star instant messages. | boolean |  |
| CanReportInstantMessages | Gets if the user can report instant messages or threads. | boolean |  |
| IsUserBlockingEnabled | Gets if the user can access instant message system and blocking system is enabled. | boolean |  |
| CanSendBroadcastMessages | Get if the user can send broadcast messages. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "CanUseSendAsIndividualMessages": true,
  "CanAccessInstantMessageSystem": true,
  "CanSearchForCourses": true,
  "CanShareInstantMessages": true,
  "CanReplyToInstantMessages": true,
  "CanStarInstantMessages": true,
  "CanReportInstantMessages": true,
  "IsUserBlockingEnabled": true,
  "CanSendBroadcastMessages": true
}
```
</details>


### `GET restapi/personal/instantmessages/privacy/{personId}/v1`

Checks privacy rules to determine if user is allowed to initiate communication with a person.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | The person id to check privacy rules against | integer | Required |

**Response:** `boolean`

<details><summary>Response JSON sample</summary>

```json
true
```
</details>


### `POST restapi/personal/instantmessages/recipients/persons/v1`

Gets recipient persons requested

**Body:** Persons to get data for Collection of integer

<details><summary>Request JSON sample</summary>

```json
[
  1
]
```
</details>

**Response:** `?` — List of Person Simple

| Name | Description | Type | Additional information |
|---|---|---|---|
| TypeName | TypeName can be: Person, Course, Project | string |  |
| Id | Id for recipient | integer |  |
| TypeNameId | Unique identifier for a Person, Course or Project. Formatted like "TypeName-Id" | string |  |
| UserName | UserName for a Person, for other types is empty | string |  |
| ExtraUserInformation | Contains extra user information which can be different depending on customer settings. It can be either username, synchronization key, any of 5 custom strings or none. | string |  |
| Label | Label for recipient. Stores one of the following: Person Name, Course Title, Project Title | string |  |
| EncodedLabel | Js-encoded Label for recipient. | string |  |
| Organization | Organization for recipient. | string |  |
| ShortLabel | Label for recipient. If Type = 0 this is set to persons initials | string |  |
| SearchLabel | Additional label for search recipient. If Type = 0 this is set to person name in the general format "FirstName LastName". | string |  |
| SortField | Label to sort on | integer |  |
| ParticipantsCount | Used for Course or Projects. The number of participants in the conversation | integer |  |
| ProfileImageUrl | URL to avatar image for this recipient | string |  |
| ColorSchema | Color schema for this recipient | string |  |
| Code | Extra code to be displayed after the recipients name | string |  |
| CanReceive | Check if recipient can receive new messages | boolean |  |
| RoleInHierarchyTitle | Name of the role in the home hierarchy. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "TypeName": "sample string 1",
    "Id": 2,
    "TypeNameId": "sample string 3",
    "UserName": "sample string 4",
    "ExtraUserInformation": "sample string 5",
    "Label": "sample string 6",
    "EncodedLabel": "sample string 7",
    "Organization": "sample string 8",
    "ShortLabel": "sample string 9",
    "SearchLabel": "sample string 10",
    "SortField": 11,
    "ParticipantsCount": 12,
    "ProfileImageUrl": "sample string 13",
    "ColorSchema": "sample string 14",
    "Code": "sample string 15",
    "CanReceive": true,
    "RoleInHierarchyTitle": "sample string 16"
  }
]
```
</details>


### `GET restapi/personal/instantmessages/recipients/search/v1?searchText={searchText}&instantMessageThreadId={instantMessageThreadId}&recipientRoles[0]={recipientRoles[0]}&recipientRoles[1]={recipientRoles[1]}`

Gets persons that the user is allowed to communicate with given the customer's esafety rules configured in itslearning.

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| searchText | The person name to search for. | string | Required |
| instantMessageThreadId | People already included in this thread will be excluded from search result. | integer |  |
| recipientRoles | List of recipient roles to filter. If the value is not set all allowed recipients are returned. | Collection of InstantMessageRecipientRole |  |

**Response:** `?` — List of Person Simple

| Name | Description | Type | Additional information |
|---|---|---|---|
| TypeName | TypeName can be: Person, Course, Project | string |  |
| Id | Id for recipient | integer |  |
| TypeNameId | Unique identifier for a Person, Course or Project. Formatted like "TypeName-Id" | string |  |
| UserName | UserName for a Person, for other types is empty | string |  |
| ExtraUserInformation | Contains extra user information which can be different depending on customer settings. It can be either username, synchronization key, any of 5 custom strings or none. | string |  |
| Label | Label for recipient. Stores one of the following: Person Name, Course Title, Project Title | string |  |
| EncodedLabel | Js-encoded Label for recipient. | string |  |
| Organization | Organization for recipient. | string |  |
| ShortLabel | Label for recipient. If Type = 0 this is set to persons initials | string |  |
| SearchLabel | Additional label for search recipient. If Type = 0 this is set to person name in the general format "FirstName LastName". | string |  |
| SortField | Label to sort on | integer |  |
| ParticipantsCount | Used for Course or Projects. The number of participants in the conversation | integer |  |
| ProfileImageUrl | URL to avatar image for this recipient | string |  |
| ColorSchema | Color schema for this recipient | string |  |
| Code | Extra code to be displayed after the recipients name | string |  |
| CanReceive | Check if recipient can receive new messages | boolean |  |
| RoleInHierarchyTitle | Name of the role in the home hierarchy. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "TypeName": "sample string 1",
    "Id": 2,
    "TypeNameId": "sample string 3",
    "UserName": "sample string 4",
    "ExtraUserInformation": "sample string 5",
    "Label": "sample string 6",
    "EncodedLabel": "sample string 7",
    "Organization": "sample string 8",
    "ShortLabel": "sample string 9",
    "SearchLabel": "sample string 10",
    "SortField": 11,
    "ParticipantsCount": 12,
    "ProfileImageUrl": "sample string 13",
    "ColorSchema": "sample string 14",
    "Code": "sample string 15",
    "CanReceive": true,
    "RoleInHierarchyTitle": "sample string 16"
  }
]
```
</details>


### `GET restapi/personal/instantmessages/recipients/v1?personIds[0]={personIds[0]}&personIds[1]={personIds[1]}&courseId={courseId}&projectId={projectId}`

Gets recipients(persons, course or project) requested

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personIds | [optional] Persons to get data for | Collection of integer |  |
| courseId | [optional] A course to get data for. | integer |  |
| projectId | [optional] A project to get data for. | integer |  |

**Response:** `?` — List of Person Simple

| Name | Description | Type | Additional information |
|---|---|---|---|
| TypeName | TypeName can be: Person, Course, Project | string |  |
| Id | Id for recipient | integer |  |
| TypeNameId | Unique identifier for a Person, Course or Project. Formatted like "TypeName-Id" | string |  |
| UserName | UserName for a Person, for other types is empty | string |  |
| ExtraUserInformation | Contains extra user information which can be different depending on customer settings. It can be either username, synchronization key, any of 5 custom strings or none. | string |  |
| Label | Label for recipient. Stores one of the following: Person Name, Course Title, Project Title | string |  |
| EncodedLabel | Js-encoded Label for recipient. | string |  |
| Organization | Organization for recipient. | string |  |
| ShortLabel | Label for recipient. If Type = 0 this is set to persons initials | string |  |
| SearchLabel | Additional label for search recipient. If Type = 0 this is set to person name in the general format "FirstName LastName". | string |  |
| SortField | Label to sort on | integer |  |
| ParticipantsCount | Used for Course or Projects. The number of participants in the conversation | integer |  |
| ProfileImageUrl | URL to avatar image for this recipient | string |  |
| ColorSchema | Color schema for this recipient | string |  |
| Code | Extra code to be displayed after the recipients name | string |  |
| CanReceive | Check if recipient can receive new messages | boolean |  |
| RoleInHierarchyTitle | Name of the role in the home hierarchy. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "TypeName": "sample string 1",
    "Id": 2,
    "TypeNameId": "sample string 3",
    "UserName": "sample string 4",
    "ExtraUserInformation": "sample string 5",
    "Label": "sample string 6",
    "EncodedLabel": "sample string 7",
    "Organization": "sample string 8",
    "ShortLabel": "sample string 9",
    "SearchLabel": "sample string 10",
    "SortField": 11,
    "ParticipantsCount": 12,
    "ProfileImageUrl": "sample string 13",
    "ColorSchema": "sample string 14",
    "Code": "sample string 15",
    "CanReceive": true,
    "RoleInHierarchyTitle": "sample string 16"
  }
]
```
</details>


### `GET restapi/personal/instantmessages/recipients/v2?personIds[0]={personIds[0]}&personIds[1]={personIds[1]}&courseIds[0]={courseIds[0]}&courseIds[1]={courseIds[1]}&projectIds[0]={projectIds[0]}&projectIds[1]={projectIds[1]}`

Gets recipients(persons, course or project) requested

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personIds | [optional] Persons to get data for | Collection of integer |  |
| courseIds | [optional] Courses to get data for. | Collection of integer |  |
| projectIds | [optional] Projects to get data for. | Collection of integer |  |

**Response:** `?` — List of Person Simple

| Name | Description | Type | Additional information |
|---|---|---|---|
| TypeName | TypeName can be: Person, Course, Project | string |  |
| Id | Id for recipient | integer |  |
| TypeNameId | Unique identifier for a Person, Course or Project. Formatted like "TypeName-Id" | string |  |
| UserName | UserName for a Person, for other types is empty | string |  |
| ExtraUserInformation | Contains extra user information which can be different depending on customer settings. It can be either username, synchronization key, any of 5 custom strings or none. | string |  |
| Label | Label for recipient. Stores one of the following: Person Name, Course Title, Project Title | string |  |
| EncodedLabel | Js-encoded Label for recipient. | string |  |
| Organization | Organization for recipient. | string |  |
| ShortLabel | Label for recipient. If Type = 0 this is set to persons initials | string |  |
| SearchLabel | Additional label for search recipient. If Type = 0 this is set to person name in the general format "FirstName LastName". | string |  |
| SortField | Label to sort on | integer |  |
| ParticipantsCount | Used for Course or Projects. The number of participants in the conversation | integer |  |
| ProfileImageUrl | URL to avatar image for this recipient | string |  |
| ColorSchema | Color schema for this recipient | string |  |
| Code | Extra code to be displayed after the recipients name | string |  |
| CanReceive | Check if recipient can receive new messages | boolean |  |
| RoleInHierarchyTitle | Name of the role in the home hierarchy. | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "TypeName": "sample string 1",
    "Id": 2,
    "TypeNameId": "sample string 3",
    "UserName": "sample string 4",
    "ExtraUserInformation": "sample string 5",
    "Label": "sample string 6",
    "EncodedLabel": "sample string 7",
    "Organization": "sample string 8",
    "ShortLabel": "sample string 9",
    "SearchLabel": "sample string 10",
    "SortField": 11,
    "ParticipantsCount": 12,
    "ProfileImageUrl": "sample string 13",
    "ColorSchema": "sample string 14",
    "Code": "sample string 15",
    "CanReceive": true,
    "RoleInHierarchyTitle": "sample string 16"
  }
]
```
</details>


### `GET restapi/personal/instantmessages/settings/v1`

Gets the settings for the instant message system. Boundaries for attachment size, max participants in a group thread etc.

**Response:** `InstantMessageSettings` — The settings that the current user has for instant messaging.

| Name | Description | Type | Additional information |
|---|---|---|---|
| MaxAttachmentFileSizeInBytes | Gets the maximum file size in bytes an attachment can have. | integer |  |
| MaxParticipantsInAMessageThread | Gets the maximum participants in a message thread. | integer |  |
| MaxParticipantsInAOneWayThread | Gets the maximum participants in a one way thread. | integer |  |
| IllegalFileExtensions | Gets an array of illegal file extensions. Files with illegal file extensions are not allowed to upload as an attachment to a message. | Collection of string |  |
| SignalRHubEndpoint | The signalR endpoint for instant message system for the current user. This varies by customer | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "MaxAttachmentFileSizeInBytes": 1,
  "MaxParticipantsInAMessageThread": 2,
  "MaxParticipantsInAOneWayThread": 3,
  "IllegalFileExtensions": [
    "sample string 1"
  ],
  "SignalRHubEndpoint": "sample string 4"
}
```
</details>


### `GET restapi/personal/instantmessages/settings/v2`

Gets the settings for the instant message system. Boundaries for attachment size, max participants in a group thread etc.

**Response:** `InstantMessageSettingsV2` — The settings that the current user has for instant messaging.

| Name | Description | Type | Additional information |
|---|---|---|---|
| SignalRServiceVersion | Determines which version of SignalR service is used. This varies by customer. | InstantMessageSignalRServiceVersion |  |
| MaxAttachmentFileSizeInBytes | Gets the maximum file size in bytes an attachment can have. | integer |  |
| MaxParticipantsInAMessageThread | Gets the maximum participants in a message thread. | integer |  |
| MaxParticipantsInAOneWayThread | Gets the maximum participants in a one way thread. | integer |  |
| IllegalFileExtensions | Gets an array of illegal file extensions. Files with illegal file extensions are not allowed to upload as an attachment to a message. | Collection of string |  |
| SignalRHubEndpoint | The signalR endpoint for instant message system for the current user. This varies by customer | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "SignalRServiceVersion": 0,
  "MaxAttachmentFileSizeInBytes": 1,
  "MaxParticipantsInAMessageThread": 2,
  "MaxParticipantsInAOneWayThread": 3,
  "IllegalFileExtensions": [
    "sample string 1"
  ],
  "SignalRHubEndpoint": "sample string 4"
}
```
</details>


### `GET restapi/personal/instantmessages/starred/count/v1`

Gets the number of starred messages for the current user

**Response:** `integer` — The number of starred messages for the current user

<details><summary>Response JSON sample</summary>

```json
1
```
</details>


### `GET restapi/personal/instantmessages/suggestions/v1?searchText={searchText}&maxSuggestions={maxSuggestions}`

Searches for suggestions of person names or thread names that match the input search query for the current user

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| searchText | The query to get suggestions for | string | Required |
| maxSuggestions | The maximum number of suggestions to return (defaults to 10 if not supplied) | integer |  |

**Response:** `?` — Array of suggestions that match the specified query

| Name | Description | Type | Additional information |
|---|---|---|---|
| Type | Type can be: OneToOne, Course, Project, Group | string |  |
| Id | PersonId if TypeName == OneToOne, or ThreadId if otherwise | integer |  |
| Label | Label for the suggestion | string |  |

<details><summary>Response JSON sample</summary>

```json
[
  {
    "Type": "sample string 1",
    "Id": 2,
    "Label": "sample string 3"
  }
]
```
</details>


### `PUT restapi/personal/instantmessages/togglemessagestar/{instantMessageId}/v1`

Toggle whether the given message is starred or not

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| instantMessageId | The id of the message to toggle the starred flag on | integer, 64 bit | Required |

**Response:** `InstantMessage` — An InstantMessage with the current value of the starred flag

| Name | Description | Type | Additional information |
|---|---|---|---|
| MessageId | The unique id for the message, in scope of a site (itslearning customer) | integer, 64 bit |  |
| MessageThreadId | Obsolete use InstantMessageThreadId instead | integer |  |
| InstantMessageThreadId | The unique id for the thread in scope of a site | integer |  |
| MessageThreadParticipants | Recipients for new message threads - does not need to be filled if not used when posting a new thread | Collection of InstantMessageParticipant |  |
| Created | DateTime when the message was created | date |  |
| CreatedFormatted | Formatted DateTime when the message was created | string |  |
| CreatedRelative | How long ago the message was created | string |  |
| EditedRelative | How long ago the message was edited | string |  |
| CreatedBy | Id of the author of the message | integer |  |
| CreatedByName | Name of the author of the message | string |  |
| HomeOrganization | Home organization of the author of the message. | string |  |
| CreatedByAvatar | Url to the avatar of the author | string |  |
| Text | The message text | string |  |
| AttachmentUrl | The url to the attached file | string |  |
| AttachmentName | File name of the attached file | string |  |
| Link | The link to combine together with the message text | string |  |
| LinkTitle | Link title to create a friendly name for the link | string |  |
| LinkTarget | The target to the link eg. _blank | string |  |
| IsSystemMessage | Is the message a system message | boolean |  |
| IsDeleted | Is the message deleted | boolean |  |
| DeletedBy | Id of the person, who deleted this message. | integer |  |
| CanDelete | If the current user is allowed to delete the message | boolean |  |
| IsEdited | Is the message edited | boolean |  |
| CanEdit | If the current user is allowed to edit the message | boolean |  |
| IsStarred | If this message has been marked as starred by the user | boolean |  |
| IsSilent | Flag used for messages we should not update the UI for | boolean |  |
| IsAbuse | This message was reported as abusive by the current user. | boolean |  |
| HasShared | The message is used to share another message. | boolean |  |
| HasRepliedTo | The message is used to reply to another message. | boolean |  |
| HasAbuseReported | The message is an abuse report. It has a child message that was marked as abusive message. | boolean |  |
| HasAbuseThreadReported | The message is an report for abusive thread. | boolean |  |
| OriginInstantMessage | The origin message for child message. | InstantMessage |  |
| OriginInstantMessageThreadId | The origin instant message thread identifier. | integer |  |
| OriginInstantMessageThreadDeletedBy | Id of the person, who delete original instant message thread. | integer |  |
| OriginInstantMessageThreadName | The origin instant message thread name. | string |  |
| ChildMessage | The child message is shared by the message. | InstantMessage |  |
| AdditionalHeading | Additional heading to display. Extra information about where the message had its origin if a message was shared or replied to. | string |  |
| IsBroadcastMassMessage | This message was sent as broadcast mass message. | boolean |  |
| IsChildMessageDeletedPermanently | Defines that child message has been deleted permanently | boolean |  |
| CreatedLocal | DateTime when the message was created converted to user's timezone | date |  |
| CreatedLocalTimeStamp | Time when the message was created in user's timezone. | string |  |
| CreatedLocalDateStamp | Date when the message was created in user's timezone formatted considering culture. | string |  |
| CreatedByShortName | First name of the author. | string |  |
| CreatedLocalShortNumericDate | Date when the message was created in user's timezone formatted as short numeric date without year. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "MessageId": 1,
  "MessageThreadId": 2,
  "InstantMessageThreadId": 3,
  "MessageThreadParticipants": [
    {
      "PersonId": 1,
      "Name": "sample string 1",
      "AvatarUrl": "sample string 2",
      "LocationType": 1,
      "LocationId": 1
    }
  ],
  "Created": "2026-11-18T16:34:29Z",
  "CreatedFormatted": "sample string 5",
  "CreatedRelative": "sample string 6",
  "EditedRelative": "sample string 7",
  "CreatedBy": 8,
  "CreatedByName": "sample string 9",
  "HomeOrganization": "sample string 10",
  "CreatedByAvatar": "sample string 11",
  "Text": "sample string 12",
  "AttachmentUrl": "sample string 13",
  "AttachmentName": "sample string 14",
  "Link": "sample string 15",
  "LinkTitle": "sample string 16",
  "LinkTarget": "sample string 17",
  "IsSystemMessage": true,
  "IsDeleted": true,
  "DeletedBy": 20,
  "CanDelete": true,
  "IsEdited": true,
  "CanEdit": true,
  "IsStarred": true,
  "IsSilent": true,
  "IsAbuse": true,
  "HasShared": true,
  "HasRepliedTo": true,
  "HasAbuseReported": true,
  "HasAbuseThreadReported": true,
  "OriginInstantMessage": {
    "MessageId": 1,
    "MessageThreadId": 2,
    "InstantMessageThreadId": 3,
    "MessageThreadParticipants": [
      {
        "PersonId": 1,
        "Name": "sample string 1",
        "AvatarUrl": "sample string 2",
        "LocationType": 1,
        "LocationId": 1
      }
    ],
    "Created": "2026-11-18T16:34:29Z",
    "CreatedFormatted": "sample string 5",
    "CreatedRelative": "sample string 6",
    "EditedRelative": "sample string 7",
    "CreatedBy": 8,
    "CreatedByName": "sample string 9",
    "HomeOrganization": "sample string 10",
... (truncated)
```
</details>


### `PUT restapi/personal/instantmessages/togglepersonblock/{personId}/v1`

Toggles block for specific .

**URI parameters**

| Name | Description | Type | Additional information |
|---|---|---|---|
| personId | Person identifier. | integer | Required |

**Response:** `boolean` — Returns Ok response if no errors occurred.

<details><summary>Response JSON sample</summary>

```json
true
```
</details>


### `POST restapi/personal/instantmessages/v1`

DEPRECATED use v2 instead

**Body** (InstantMessageToSend)

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | CourseId to send instant message to. | integer |  |
| ProjectId | ProjectId to send instant message to. | integer |  |
| FileId | Guids of files already uploaded to temporary file repo storage | Collection of string |  |
| ToPersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId or CourseIds or ProjectIds need to have a valid value. | Collection of integer |  |
| InstantMessageThreadId | The thread to send to, if this is null or zero we will assume it's a new thread. | integer |  |
| Text | The text of the message | string |  |
| SendAsIndividualMessages | Gets whether a message to multiple recipients should be sent as individual messages | boolean |  |

**Response:** `InstantMessageThread` — The instant message thread

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:35:31Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:35:31Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>


### `PATCH restapi/personal/instantmessages/v1`

Updates the text for an instant message.

**Body** (InstantMessageToPatch)

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageId | The instant messageId to patch. | integer, 64 bit |  |
| Text | The text of the message | string |  |

<details><summary>Request JSON sample</summary>

```json
{
  "InstantMessageId": 1,
  "Text": "sample string 2"
}
```
</details>

**Response:** `InstantMessage`

| Name | Description | Type | Additional information |
|---|---|---|---|
| MessageId | The unique id for the message, in scope of a site (itslearning customer) | integer, 64 bit |  |
| MessageThreadId | Obsolete use InstantMessageThreadId instead | integer |  |
| InstantMessageThreadId | The unique id for the thread in scope of a site | integer |  |
| MessageThreadParticipants | Recipients for new message threads - does not need to be filled if not used when posting a new thread | Collection of InstantMessageParticipant |  |
| Created | DateTime when the message was created | date |  |
| CreatedFormatted | Formatted DateTime when the message was created | string |  |
| CreatedRelative | How long ago the message was created | string |  |
| EditedRelative | How long ago the message was edited | string |  |
| CreatedBy | Id of the author of the message | integer |  |
| CreatedByName | Name of the author of the message | string |  |
| HomeOrganization | Home organization of the author of the message. | string |  |
| CreatedByAvatar | Url to the avatar of the author | string |  |
| Text | The message text | string |  |
| AttachmentUrl | The url to the attached file | string |  |
| AttachmentName | File name of the attached file | string |  |
| Link | The link to combine together with the message text | string |  |
| LinkTitle | Link title to create a friendly name for the link | string |  |
| LinkTarget | The target to the link eg. _blank | string |  |
| IsSystemMessage | Is the message a system message | boolean |  |
| IsDeleted | Is the message deleted | boolean |  |
| DeletedBy | Id of the person, who deleted this message. | integer |  |
| CanDelete | If the current user is allowed to delete the message | boolean |  |
| IsEdited | Is the message edited | boolean |  |
| CanEdit | If the current user is allowed to edit the message | boolean |  |
| IsStarred | If this message has been marked as starred by the user | boolean |  |
| IsSilent | Flag used for messages we should not update the UI for | boolean |  |
| IsAbuse | This message was reported as abusive by the current user. | boolean |  |
| HasShared | The message is used to share another message. | boolean |  |
| HasRepliedTo | The message is used to reply to another message. | boolean |  |
| HasAbuseReported | The message is an abuse report. It has a child message that was marked as abusive message. | boolean |  |
| HasAbuseThreadReported | The message is an report for abusive thread. | boolean |  |
| OriginInstantMessage | The origin message for child message. | InstantMessage |  |
| OriginInstantMessageThreadId | The origin instant message thread identifier. | integer |  |
| OriginInstantMessageThreadDeletedBy | Id of the person, who delete original instant message thread. | integer |  |
| OriginInstantMessageThreadName | The origin instant message thread name. | string |  |
| ChildMessage | The child message is shared by the message. | InstantMessage |  |
| AdditionalHeading | Additional heading to display. Extra information about where the message had its origin if a message was shared or replied to. | string |  |
| IsBroadcastMassMessage | This message was sent as broadcast mass message. | boolean |  |
| IsChildMessageDeletedPermanently | Defines that child message has been deleted permanently | boolean |  |
| CreatedLocal | DateTime when the message was created converted to user's timezone | date |  |
| CreatedLocalTimeStamp | Time when the message was created in user's timezone. | string |  |
| CreatedLocalDateStamp | Date when the message was created in user's timezone formatted considering culture. | string |  |
| CreatedByShortName | First name of the author. | string |  |
| CreatedLocalShortNumericDate | Date when the message was created in user's timezone formatted as short numeric date without year. | string |  |

<details><summary>Response JSON sample</summary>

```json
{
  "MessageId": 1,
  "MessageThreadId": 2,
  "InstantMessageThreadId": 3,
  "MessageThreadParticipants": [
    {
      "PersonId": 1,
      "Name": "sample string 1",
      "AvatarUrl": "sample string 2",
      "LocationType": 1,
      "LocationId": 1
    }
  ],
  "Created": "2026-11-18T16:34:59Z",
  "CreatedFormatted": "sample string 5",
  "CreatedRelative": "sample string 6",
  "EditedRelative": "sample string 7",
  "CreatedBy": 8,
  "CreatedByName": "sample string 9",
  "HomeOrganization": "sample string 10",
  "CreatedByAvatar": "sample string 11",
  "Text": "sample string 12",
  "AttachmentUrl": "sample string 13",
  "AttachmentName": "sample string 14",
  "Link": "sample string 15",
  "LinkTitle": "sample string 16",
  "LinkTarget": "sample string 17",
  "IsSystemMessage": true,
  "IsDeleted": true,
  "DeletedBy": 20,
  "CanDelete": true,
  "IsEdited": true,
  "CanEdit": true,
  "IsStarred": true,
  "IsSilent": true,
  "IsAbuse": true,
  "HasShared": true,
  "HasRepliedTo": true,
  "HasAbuseReported": true,
  "HasAbuseThreadReported": true,
  "OriginInstantMessage": {
    "MessageId": 1,
    "MessageThreadId": 2,
    "InstantMessageThreadId": 3,
    "MessageThreadParticipants": [
      {
        "PersonId": 1,
        "Name": "sample string 1",
        "AvatarUrl": "sample string 2",
        "LocationType": 1,
        "LocationId": 1
      }
    ],
    "Created": "2026-11-18T16:34:59Z",
    "CreatedFormatted": "sample string 5",
    "CreatedRelative": "sample string 6",
    "EditedRelative": "sample string 7",
    "CreatedBy": 8,
    "CreatedByName": "sample string 9",
    "HomeOrganization": "sample string 10",
... (truncated)
```
</details>


### `POST restapi/personal/instantmessages/v2`

Send an instant message.

**Body** (InstantMessageToSendV2)

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseIds | CourseIds to send instant message to. | Collection of integer |  |
| ProjectIds | ProjectIds to send instant message to. | Collection of integer |  |
| FileIds | Guids of files already uploaded to temporary file repo storage | Collection of string |  |
| InstantMessageIdToShare | Instant message Id to a message that is to be shared. | integer, 64 bit |  |
| InstantMessageIdToReplyTo | Instant message Id to a message that is to be replied to. | integer, 64 bit |  |
| OriginInstantMessageId | Identifier of origin instant message that referenced by current message. | integer, 64 bit |  |
| ReferencedInstantMessageType | Type of origin instant message that referenced by current message. | ReferencedInstantMessageType |  |
| OriginMessageThreadId | Identifier of origin instant message thread that referenced by current message. | integer |  |
| HasYourFilesAttachment | when at least one of attached file ids belong to yourFiles file. | boolean |  |
| ToPersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId or CourseIds or ProjectIds need to have a valid value. | Collection of integer |  |
| InstantMessageThreadId | The thread to send to, if this is null or zero we will assume it's a new thread. | integer |  |
| Text | The text of the message | string |  |
| SendAsIndividualMessages | Gets whether a message to multiple recipients should be sent as individual messages | boolean |  |

<details><summary>Request JSON sample</summary>

```json
{
  "CourseIds": [
    1
  ],
  "ProjectIds": [
    1
  ],
  "FileIds": [
    "sample string 1"
  ],
  "InstantMessageIdToShare": 1,
  "InstantMessageIdToReplyTo": 1,
  "OriginInstantMessageId": 1,
  "ReferencedInstantMessageType": "None",
  "OriginMessageThreadId": 1,
  "HasYourFilesAttachment": true,
  "ToPersonIds": [
    1
  ],
  "InstantMessageThreadId": 1,
  "Text": "sample string 3",
  "SendAsIndividualMessages": true
}
```
</details>

**Response:** `InstantMessageThread` — The instant message thread

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The identifier for the thread | integer |  |
| Name | The name of the thread | string |  |
| Created | The date the thread was created | date |  |
| CreatedByTeacher | Gets whether the user who created thread is a teacher. | boolean |  |
| CreatedBy | The person identifier who has created the thread. | integer |  |
| Type | The type of thread this is Valid values are 'Course', 'Project', 'Group', 'OneToOne' and 'GroupPlaceHolder' | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |
| Messages | Some message in the thread | EntityList<InstantMessage> |  |
| LastMessage | The last message in the thread | InstantMessage |  |
| MatchingMessageIds | The id's of all messages in the thread that match the search query (if any) | Collection of integer, 64 bit |  |
| Participants | An array of participants in the thread | Collection of ThreadParticipant |  |
| ParticipantsCount | Number of participants in the thread. | integer |  |
| HasActiveParticipants | Defines whether current thread has any recipients except current user who are able to receive messages. | boolean |  |
| HasInactiveParticipants | Defines whether current thread has any recipients except current user who are not able to receive messages. | boolean |  |
| NumberOfInactiveParticipants | Number of persons who are not able to receive messages in a conversation. | integer |  |
| InactiveParticipantFullName | Full name of person who are not able to receive messages in case if there's the only one such person in a conversation. | string |  |
| HasCreatorInThread | Defines whether person who created current thread is still in the list of its participants. | boolean |  |
| IsCurrentUserARecipientWithSourceLocation | Defines whether current person is in the list of current thread participants as course/project/collaboration/hierarchy member. | boolean |  |
| LocationRecipients | An array of location recipients in the thread | Collection of LocationRecipient |  |
| CanShowParticipantsPage | Defines whether course participants page is allowed to be seen. | boolean |  |
| LastReadInstantMessageId | The id of the last read instant message if there is one | integer, 64 bit |  |
| SortIndex | A number that the threads will be sorted by. Highest number is shown as the thread with most recent update (contains newest message). | integer, 64 bit |  |
| OnlyThreadAdminCanSendToThread | Gets whether only thread admins can send to this thread | boolean |  |
| IsThreadAdministrator | Gets whether the current user is an administrator of this thread Only course threads have administrators | boolean |  |
| IsThreadModerator | Thread moderator have additional permissions to moderate the thread: to close or delete the thread, delete individual messages. | boolean |  |
| InstantMessageLocationId | The location identifier for the thread | integer |  |
| IsBlocked | Determines whether the thread is blocked. | boolean |  |
| UserAllowToBlockThread | Determines whether the thread can be blocked by current user. | boolean |  |
| IsAbuse | Determines whether the thread is abusive (marked as abusive by the current user or the current user got a report message about abuse). | boolean |  |
| IsAbuseReportedByCurrentUser | Contains true if the current person has sent a report message that the thread is abusive. | boolean |  |
| MassMessageType | Determines types of messages in thread | InstantMessageMassMessageType |  |
| Tooltip | Tooltip to be displayed on the thread title. | string |  |
| CanDelete | Determines whether the thread can be hard deleted. | boolean |  |

<details><summary>Response JSON sample</summary>

```json
{
  "InstantMessageThreadId": 1,
  "Name": "sample string 2",
  "Created": "2026-11-18T16:31:46Z",
  "CreatedByTeacher": true,
  "CreatedBy": 5,
  "Type": "sample string 6",
  "ColorSchema": "sample string 7",
  "Messages": {
    "EntityArray": [
      {
        "MessageId": 1,
        "MessageThreadId": 2,
        "InstantMessageThreadId": 3,
        "MessageThreadParticipants": [
          {
            "PersonId": 1,
            "Name": "sample string 1",
            "AvatarUrl": "sample string 2",
            "LocationType": 1,
            "LocationId": 1
          }
        ],
        "Created": "2026-11-18T16:31:46Z",
        "CreatedFormatted": "sample string 5",
        "CreatedRelative": "sample string 6",
        "EditedRelative": "sample string 7",
        "CreatedBy": 8,
        "CreatedByName": "sample string 9",
        "HomeOrganization": "sample string 10",
        "CreatedByAvatar": "sample string 11",
        "Text": "sample string 12",
        "AttachmentUrl": "sample string 13",
        "AttachmentName": "sample string 14",
        "Link": "sample string 15",
        "LinkTitle": "sample string 16",
        "LinkTarget": "sample string 17",
        "IsSystemMessage": true,
        "IsDeleted": true,
        "DeletedBy": 20,
        "CanDelete": true,
        "IsEdited": true,
        "CanEdit": true,
        "IsStarred": true,
        "IsSilent": true,
        "IsAbuse": true,
        "HasShared": true,
        "HasRepliedTo": true,
        "HasAbuseReported": true,
        "HasAbuseThreadReported": true,
        "OriginInstantMessage": {
          "MessageId": 1,
          "MessageThreadId": 2,
          "InstantMessageThreadId": 3,
          "MessageThreadParticipants": [
            {
              "PersonId": 1,
              "Name": "sample string 1",
              "AvatarUrl": "sample string 2",
              "LocationType": 1,
... (truncated)
```
</details>

