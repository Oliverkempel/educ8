# Resource models

> Auto-generated from the help page ResourceModel pages. `Itslearning.RestApi.Entities.` prefix is stripped.
> Some enum types from other namespaces (e.g. `Itsolutions.*`) return HTTP 500 on the help site and are listed as unresolved.


## Itslearning.Common.Core.SortOrder

| Name | Value | Description |
|---|---|---|
| Undefined | -1 |  |
| Ascending | 0 |  |
| Descending | 1 |  |

## Itslearning.Common.RestApi.Entities.VersionResponse

| Name | Value | Description |
|---|---|---|
| Unknown | 0 |  |
| LatestVersion | 1 |  |
| NewVersionAvailable | 2 |  |
| MustUpdate | 3 |  |

## Itslearning.FileRepo.Common.Models.SourceType

| Name | Value | Description |
|---|---|---|
| Dropbox | 1 |  |
| GoogleDrive | 2 |  |
| OneDriveForBusiness | 3 |  |
| AjaxUploader | 4 |  |
| OneDrivePersonal | 5 |  |
| WebDav | 6 |  |
| Ziggeo | 7 |  |
| YourFiles | 8 |  |

## Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode

| Name | Value | Description |
|---|---|---|
| NotApplicable | 1 |  |
| RequiresSubmission | 2 |  |
| ManualByTeacher | 3 |  |
| ManualByStudent | 4 |  |

## Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolCopyStatus

| Name | Value | Description |
|---|---|---|
| None | 1 |  |
| InProgress | 2 |  |
| Completed | 3 |  |
| Failed | 4 |  |

## Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolType

| Name | Value | Description |
|---|---|---|
| LearningResource | 0 |  |
| LearningActivity | 1 |  |

## Itslearning.Platform.RestApi.Sdk.Common.Entities.SharingScope

| Name | Value | Description |
|---|---|---|
| None | 0 |  |
| School | 1 |  |
| Site | 2 |  |
| Community | 3 |  |
| Custom | 4 |  |
| State | 5 |  |

## Itslearning.Platform.RestApi.Sdk.Common.Entities.WhenToShowResult

| Name | Value | Description |
|---|---|---|
| Undefined | 0 |  |
| Never | 1 |  |
| AfterEachAttempt | 2 |  |
| AfterDeadline | 3 |  |
| AfterAllAttempts | 4 |  |
| WhenTeacherDecides | 5 |  |

## Itslearning.Platform.RestApi.Sdk.ItslEntities.Planner.PlanElement

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningObjectInstanceId |  | integer |  |
| ExtensionId |  | integer |  |
| AuthorPersonId |  | integer |  |
| AuthorName |  | string |  |
| SharedWith |  | string |  |
| SharingScope |  | Itslearning.Platform.RestApi.Sdk.Common.Entities.SharingScope |  |
| VendorDefaultName |  | string |  |
| VendorAuthorName |  | string |  |
| ElementLearningObjectives |  | Collection of integer |  |
| IconTypeId |  | integer |  |
| IconsVersion |  | integer |  |
| ElementId |  | integer |  |
| Title |  | string |  |
| LearningObjectId |  | integer |  |
| LearningToolType |  | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolType |  |
| Order |  | integer |  |

## Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.AssessmentStatusItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentStatusItemId |  | integer |  |
| AssessmentStatusId |  | integer |  |
| Title |  | string |  |
| Editable |  | boolean |  |
| Completed |  | boolean |  |
| Submitted |  | boolean |  |
| NotSubmitted |  | boolean |  |

## Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.ElementPermission

| Name | Value | Description |
|---|---|---|
| All | 0 |  |
| Modifier | 1 |  |
| Evaluator | 2 |  |
| Participant | 3 |  |
| Read | 4 |  |

## Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType

| Name | Value | Description |
|---|---|---|
| Undefined | 0 |  |
| Site | 1 |  |
| School | 2 |  |
| Course | 3 |  |
| CourseGroup | 4 |  |

## Itslearning.RestApi.Client.Requests.SearchDataItslearningRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| CustomerId |  | integer |  |
| ElementIds |  | Collection of integer |  |
| SearchData |  | string |  |

## AggregatedOrganisation

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsFronterUpgradedSite | IsFronterUpgradedSite flag | boolean |  |
| IsParentAppEnabled | Determines whether parent app is enabled. | boolean |  |
| CustomerId | The customer id for the site. | integer |  |
| SiteName | The site name. | string |  |
| SchoolName | The school name. Only set if IsSite=false. | string |  |
| HierarchyId | The school id (hierarchy id). Will be 0 for sites. | integer |  |
| IsSite | True if Organisation is of type Site. False if Organisation is of type School. | boolean |  |

## AggregatedOrganisationLite

| Name | Description | Type | Additional information |
|---|---|---|---|
| CustomerId | The customer id for the site. | integer |  |
| SiteName | The site name. | string |  |
| SchoolName | The school name. Only set if IsSite=false. | string |  |
| HierarchyId | The school id (hierarchy id). Will be 0 for sites. | integer |  |
| IsSite | True if Organisation is of type Site. False if Organisation is of type School. | boolean |  |

## AssessmentRecord.AssessmentRecordCategory

| Name | Description | Type | Additional information |
|---|---|---|---|
| CategoryId | Category Id | integer |  |
| Title | Category Title | string |  |
| Weight | Category weight | decimal number |  |
| Position | The way the category is ordered. | decimal number |  |

## AssessmentRecord.AssessmentRecordElements

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentRecordItems | Returns the assessment record items | Collection of AssessmentRecord.AssessmentRecordItem |  |
| AssessmentRecordItemsCount | Returns the assessment record items' count | integer |  |
| FinalAssessmentRecordItems | Returns the final assessment record items | Collection of AssessmentRecord.AssessmentRecordItem |  |
| AssessmentScales | Returns the assessment scales | Collection of AssessmentRecord.AssessmentScale |  |
| IsScoreInUse | Returns true if any asessment record items uses score | boolean |  |

## AssessmentRecord.AssessmentRecordGroup

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupId | Group Id | integer |  |
| Title | Course group title | string |  |

## AssessmentRecord.AssessmentRecordItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| GradeBookItemId | The id of the assessment record item | integer |  |
| Title | The title of the assessment record item | string |  |
| Weight | The weight of the assessment record item | decimal number |  |
| WhenToShowResult | When to show test 3.0 result to student. | Itslearning.Platform.RestApi.Sdk.Common.Entities.WhenToShowResult |  |
| AssessmentId | The assessment scale of the assessment record item | integer |  |
| IsFinal | Whether assessment record item is used as final assessment | boolean |  |
| IsPassedDeadline | Whether assessment record item is passed the deadline | boolean |  |
| TermId | The id of the term which the assessment record item assigned to | integer |  |
| CategoryId | The category id of the assessment record item | integer |  |
| ElementType | The element type of the assessment record item | string |  |
| ElementId | The element id of the assessment record item | integer |  |
| MaxScore | Highest score value | decimal number |  |
| UseScore | Score is used as assessment scale | boolean |  |
| IsActive | Whether the assessment record item is active or inactive | boolean |  |
| IsAssessmentVisible | Whether the assessment record item is visible or invisible | boolean |  |
| IsCustomAssessmentVisible | Whether the custom assessment is visible or invisible | boolean |  |
| IsReadOnly | Whether the assessement record is read-only or editable. | boolean |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. | integer |  |
| Deadline | The deadline for the assessment record item. | date |  |
| FormattedDeadline | The formatted deadline for the assessment record item. | string |  |
| IsLearningPathSubElement | Whether the element is learning path sub element. | boolean |  |

## AssessmentRecord.AssessmentRecordPerson

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonAssessments | The assessments of the person in a gradebook | Collection of AssessmentRecord.PersonAssessmentValue |  |
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

## AssessmentRecord.AssessmentRecordTerm

| Name | Description | Type | Additional information |
|---|---|---|---|
| TermId | Term Id | integer |  |
| Title | Term Title | string |  |
| Status | Term Status | integer |  |
| IsCompleted | Whether the term is completed | boolean |  |
| CompletedDate | Date time when the term was completed | string |  |
| CompletedBy | User who completed the term | string |  |
| IsDeleted | Whether the term is deleted | boolean |  |

## AssessmentRecord.AssessmentScale

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentScaleId | The id of the assessment scale | integer |  |
| Title | The title of the assessment scale | string |  |
| AssessmentTotals | Flag indicate if this scale should be participated in overall average calculation. | integer |  |
| Values | The values of the assessment scale | Collection of AssessmentRecord.AssessmentScaleValue |  |

## AssessmentRecord.AssessmentScaleValue

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentItemId | The id of the assessment item | integer |  |
| Title | The title of the assessment item | string |  |
| OrderPosition | The order position of the assessment item | integer |  |
| PercentFrom | Percent from and including | decimal number |  |
| PercentTo | Percent to | decimal number |  |

## AssessmentRecord.PersonAssessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| Score | Assessment record item score value | decimal number |  |
| AssessmentItemId | Assessment record item scale value | integer |  |
| IsOverwritten | Whether the assessment has been overwritten | boolean |  |
| IsAssessmentVisible | Whether the assessment is visible to the student | boolean |  |

## AssessmentRecord.PersonAssessmentValue

| Name | Description | Type | Additional information |
|---|---|---|---|
| GradeBookItemId | The id of the assessment record item | integer |  |
| IsExempt | Whether the person is exempted | boolean |  |
| IsManualAssessmentRequired | Whether assessment record item should be assessed by teacher manually. | boolean |  |
| Score | Assessment record item score value | decimal number |  |
| Scale | Assessment record item scale value | decimal number |  |
| OverwrittenAssessmentItemId | Overwritten scale value for test 3.0 | integer |  |
| OverwrittenScore | Overwritten score value for test 3.0 | decimal number |  |
| AnswerSubmitted | Whether the person has submitted an answer | boolean |  |
| AttemptId | The attempt id that the student submitted. | integer |  |
| AnswerDateTime | Gets or sets the answer date time. | date |  |
| AssessmentVisible | Whether the assessment from test 3.0 is visible for the student | boolean |  |
| ElementSessionStatus | Gets the element session status. | Itsolutions.ItslUtils.Constants.SessionStatus |  |
| ElementResultId | Gets the element result id. | integer |  |
| IsAnonymousSubmission | Whether the assessment is visible for the teacher. | boolean |  |
| ElementPersonCanTake | Gets a value indicating whether the person can take the element. | boolean |  |

## BroadcastInstantMessageToSend

| Name | Description | Type | Additional information |
|---|---|---|---|
| SenderName | The name of the sender (can be site or a school name or custromized by admin) to assign as the thread's name | string |  |
| SenderHierarchyId | Id of sender's hierarchy | integer |  |
| CourseIds | CourseIds to send instant message to. | Collection of integer |  |
| ProjectIds | ProjectIds to send instant message to. | Collection of integer |  |
| HierarchyIds | HierarchyIds to send instant message to. | Collection of integer |  |
| RecipientRoles | The list of roles in specified hierarchies to send instant message to. | Collection of InstantMessageRecipientRole |  |
| ProfileIds | The list of person profile ids to send instant message to. | Collection of integer |  |
| FileIds | Guids of files already uploaded to temporary file repo storage | Collection of string |  |
| ToPersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId or CourseIds or ProjectIds need to have a valid value. | Collection of integer |  |
| InstantMessageThreadId | The thread to send to, if this is null or zero we will assume it's a new thread. | integer |  |
| Text | The text of the message | string |  |
| SendAsIndividualMessages | Gets whether a message to multiple recipients should be sent as individual messages | boolean |  |

## CalendarEventSimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| EventId | Identifier of the event | integer |  |
| EventUrl | The URL in itslearning web to open the calendar event in the web view. | string |  |
| EventTitle | Title of the event | string |  |
| FromDate | The plan's start date | date |  |
| ToDate | The plan's end date | date |  |
| EventType | The event's location type. | Personal.Calendar.CalendarEventType |  |
| LocationId | The event location identifier. | integer |  |
| LocationType | The event's location type. | LocationType |  |

## Child

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The child's unique id. | integer |  |
| FirstName | The child's first name. | string |  |
| LastName | The child's last name. | string |  |
| FullName | The child's full name. | string |  |

## ChildCourse

| Name | Description | Type | Additional information |
|---|---|---|---|
| Teachers | Teachers of the learner in the course. | Collection of PersonSimpleWithHierarchyExtendedRole |  |
| CourseId | The unique id for the course, in scope of a site (itslearning customer). | integer |  |
| Title | The course title. | string |  |
| FriendlyName | The course friendly name. | string |  |
| CourseColor | Color code for the course. | string |  |
| CourseFillColor | Fill color code for the course. | string |  |
| CourseCode | Code for the course. | string |  |

## ChildOrganisationPermissions

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanUseMessaging | Defines that a parent can use messages: receive and view them. | boolean |  |
| CanInitiateMessages | Defines that a parent can create and send a new message. | boolean |  |
| CanViewCourseContent | Defines that a parent can view course content where their children participate. | boolean |  |

## ChildV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| School | The child's school. | string |  |
| ProfileImageUrl | The url of child's image. | string |  |
| PersonId | The child's unique id. | integer |  |
| FirstName | The child's first name. | string |  |
| LastName | The child's last name. | string |  |
| FullName | The child's full name. | string |  |

## Comment

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Comment identifier. | integer |  |
| Author | The person who created the comment. | PersonSimple |  |
| CommentText | Text message. | string |  |
| AllowEditComment | true if it's allowed to edit the comment. Always true if current person is an author of the comment. | boolean |  |
| AllowDeleteComment | true if it's allowed to delete the comment. | boolean |  |
| CreatedDateTime | Date time in UTC when the comment was created. | date |  |
| IsRead | true if the comment has been read by current person. | boolean |  |

## Course

| Name | Description | Type | Additional information |
|---|---|---|---|
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

## CourseCard

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The unique id for the course | integer |  |
| FriendlyName | Course friendly name. | string |  |
| Title | Course title. | string |  |
| TeacherName | Teacher name to be shown on card. Empty if not a single teacher in course. | string |  |
| TeacherPictureUrl | Teacher picture URL to be shown on card. Empty if not a single teacher in course. | string |  |
| NumberOfTeachers | Number of teachers in course. | integer |  |
| Teachers | Teachers to be displayed on the card | Collection of CourseCardTeacher |  |
| CourseColorClass | Course colour. | string |  |
| NumberOfAnnouncements | Number of announcements. | integer |  |
| NumberOfTasks | Number of tasks. | integer |  |
| NumberOfFollowUpTasks | Number of follow-up tasks. | integer |  |
| CourseCardImageFileRepoId | FileRepoId of course card image | globally unique identifier |  |
| CourseCardImageUrl | Signed FileRepo URL of course card image. | string |  |
| CourseCardImageDescription | Course card image description text. | string |  |
| LastUpdatedDisplayTime | Course last updated in a human-readable way | string |  |
| LastUpdated | Course last updated in UTC. Can be null if it was never updated. | date |  |
| LastOnlineDisplayTime | Course last visited in a human-readable way | string |  |
| LastOnline | Course last visited in UTC. Can be null if it was never visited. | date |  |
| IsFavouriteCourse | Is course marked as favourite | boolean |  |
| NotificationsEnabled | Whether this course's notifications are enabled for the user. | boolean |  |
| CanAdminCourse | Can admin course | boolean |  |
| IsCompletedCourse | Whether the course is completed. | boolean |  |
| CourseType | Course Type | Itsolutions.ItslUtils.Constants.CourseType |  |

## CourseCardSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseTitle | Course title. | string |  |
| FriendlyName | Friendly course name. | string |  |
| CourseColorClass | Course colour. | string |  |
| VisibleCourseCardCandidateIds | Teacher representatives id's, comma separated | string |  |
| VisibleCourseCardCandidates | Teacher representatives | Collection of CourseCardTeacher |  |
| CourseCardImageFileRepoId | The ID of course card image in the FileRepo. | globally unique identifier |  |
| CourseCardImageUrl | Signed FileRepo URL of course card image which is used to display image on the user interface. | string |  |
| CourseCardImageSignedFileRepoId | Signed FileRepo URL of course card image which is used to check the validation when uploading a new image. | string |  |
| CanEditCourseColour | True if person can change the colour | boolean |  |
| CanEditFriendlyName | True if person can change the friendly name | boolean |  |
| CourseCardImageDescription | Image description for course card | string |  |
| SaveImageToYourImagesForEasyReuse | Defines whether newly uploaded course card image should be saved into media repository to be reused in "Your images" tab later. | boolean |  |

## CourseCardTeacher

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FullName | The person's full name. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| IsVisibleOnCourseCard | Flag to indicate if this is a candidate being visible on course card | boolean |  |

## CourseFocus

| Name | Value | Description |
|---|---|---|
| Subject | 1 | Relevant to one subject, for example ‘Geography’ or ‘Mathematics’. Often used with deadlines. |
| Class | 2 | Group multiple topics or subjects within a single course and display all learning content on the course overview page. Suitable for younger students. |
| SelfPaced | 3 | Let your students work at their own pace without time limits. Require use of plans. |
| InfoHub | 4 | Non-academic course used for information and file sharing, e.g. staff room. |

## CourseGroups.CourseGroup

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupId | Course group identifier. | integer |  |
| Title | Course group title. | string |  |
| ParticipantIds | Course group participants identifiers. | Collection of integer |  |

## CourseGroups.CourseGroupSet

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupSetId | Group set identifier. | integer |  |
| Title | Group set title. | string |  |
| Groups | Array of groups in the current group set. | Collection of CourseGroups.CourseGroup |  |

## CourseGroups.CourseGroupSetSimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupSetId | Group set identifier. | integer |  |
| GroupIds | Array of groups identifiers in the current group set. | Collection of integer |  |

## CourseGroups.CourseGroupsToRemove

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupIds | Course groups identifiers of course groups to be removed. | Collection of integer |  |

## CourseHierarchyToSync

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyId | Gets or sets the hierarchy identifier. | integer |  |

## CourseParticipantToAdd

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | Gets or sets the person identifier. | integer |  |
| CourseProfileId | Gets or sets the course profile identifier for the user. | integer |  |

## CourseParticipants.CourseParticipantsTemporaryAccess

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The ids of the participants | integer |  |
| TemporaryAccess | If the person has temporary access | boolean |  |

## CourseParticipants.CourseParticipantsToRemove

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantIdsToBeRemoved | The ids of the participants to be removed from a course | Collection of integer |  |

## CourseTopic

| Name | Description | Type | Additional information |
|---|---|---|---|
| Color | Topic color schema HEX | string |  |
| TopicId | Topic ID. | integer |  |
| TopicName | Topic name. | string |  |
| TopicMode | Topic presentation type | Courses.CourseTopicMode |  |
| FolderId | Folder connected to the topic if any | integer |  |

## CourseUpcoming

| Name | Description | Type | Additional information |
|---|---|---|---|
| Tasks | List of tasks with deadline on the day. | Collection of TaskSimple |  |
| Plans | List of plans on the day. | Collection of PlanSimple |  |
| CalendarEvents | List of events scheduled on the day. | Collection of CalendarEventSimple |  |
| CourseId | The unique id for the course, in scope of a site (itslearning customer). | integer |  |
| Title | The course title. | string |  |
| FriendlyName | The course friendly name. | string |  |
| CourseColor | Color code for the course. | string |  |
| CourseFillColor | Fill color code for the course. | string |  |
| CourseCode | Code for the course. | string |  |

## CourseWithContextRole

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContextRole | The context role for the current person . | PersonContextRole |  |
| OrganisationLegalId | External identifier of the organisation the course is connected to. | string |  |
| OrganisationSyncKey | Sync key of the organisation the course is connected to. | string |  |
| Code |  | string |  |
| CourseId |  | integer |  |
| CreatedDateTimeUtc |  | date |  |
| Title |  | string |  |
| OrganisationId |  | integer |  |
| CourseColorClass |  | string |  |
| SyncKey |  | string |  |

## CourseWithWorkload

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId |  | integer |  |
| Title |  | string |  |
| Workload | The workload for this person. | Workload |  |
| CanAccessAssignmentList | To check the status of the Course profile setting "Include profile in learner reports". | boolean |  |

## Courses.CourseTopicMode

| Name | Value | Description |
|---|---|---|
| Resources | 1 | Resources type, this will show a list of resources |
| Plans | 2 | Plans type, this will show a list of plans |

## CriteriaAssessmentReport.CriteriaAssessmentReport

| Name | Description | Type | Additional information |
|---|---|---|---|
| ReportItems | The assessment report items. | Collection of CriteriaAssessmentReport.CriteriaAssessmentReportItem |  |

## CriteriaAssessmentReport.CriteriaAssessmentReportItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| Criterion | The criterion. | CriteriaAssessmentReport.Criterion |  |
| Assessments | The criterion assessments. | Collection of CriteriaAssessmentReport.CriterionAssessment |  |

## CriteriaAssessmentReport.Criterion

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The criteria id. | integer |  |
| Title | The criteria title. | string |  |
| Description | The criteria description. | string |  |

## CriteriaAssessmentReport.CriterionAssessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| AchievementLevelTitle | The achievement level title. | string |  |
| AssessmentDate | The date when assessment was created. | date |  |
| Comment | The teacher comment. | string |  |
| PercentScore | Score in percent of the assessment. | decimal number |  |
| AssessedBy | The of person who assesst the assessment. | PersonSimple |  |
| Element | The assessment element . | Element |  |

## DeletePersonalNotificationsForLocationTriggerV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| Location | The location for which notifications should be deleted. | TypedId |  |

## DeletePersonalNotificationsForObjectTriggerV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| Object | Object for which notifications should be deleted. | TypedId |  |

## Element

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId |  | integer |  |
| ElementType |  | integer |  |
| Title |  | string |  |
| LearningToolId |  | integer |  |
| ElementOrder |  | integer |  |
| ActiveFromDT |  | date |  |
| ActiveToDT |  | date |  |
| DeadlineDT |  | date |  |

## ElementLink

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | The ID of element. | integer |  |
| ElementType | Related element's type. | ElementType |  |
| Title | The element's title. | string |  |
| Url | Url to the element's page in itslearning. | string |  |
| ContentUrl | Url to the location in itslearning opened without master page. | string |  |
| IconUrl | Url to the element's icon used by itslearning. | string |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. It is applicable only if ElementType is LearningToolElement. | integer |  |
| Homework | Marks the element as homework. | boolean |  |

## ElementType

| Name | Value | Description |
|---|---|---|
| Unknown | 0 | Element type is not defined. |
| Discussion | 1 | Discussion. |
| PictureWithDescription | 2 | Picture with description. |
| Folder | 4 | Folder. |
| Note | 8 | Note. |
| WebLink | 32 | Weblink. |
| FolderFile | 64 | File. |
| Survey | 128 | Survey. |
| Assignment | 512 | Assignment. |
| Lesson | 2048 | Lesson. |
| Test | 65536 | Test 2.0. |
| LearningToolElement | 131072 | Learning tool element (any element, which belongs to learning tool). |
| CustomActivity | 524288 | Custom activity. |
| LearningPath | 1048576 | Learning path. |

## FederatedLoginProvider

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The id of the federated login provider. | integer |  |
| Name | The name of the federated login provider. | string |  |
| IconUrl | The icon url for the federated login provider. | string |  |
| OpenInExternalBrowser | Flag which is set to true if federated login provider page should be opened in external browser and in particular it cannot be opened in a webview control. | boolean |  |
| ButtonText | The text to show in the button to login using this particular federated login type. | string |  |
| LogoutType | Logout types in itslearning. Will specify if user should be logged out from itslearning and/or from federated login provider. | LogoutType |  |

## Folder.AssignToModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantIds | The person identifiers the folder is assigned to | Collection of integer |  |
| GroupIds | The group identifiers the folder is assigned to | Collection of integer |  |

## Folder.FolderRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier. | integer |  |
| Title | The folder title | string |  |
| Description | The folder description | string |  |
| Visibility | Indicates the activity setting for the folder . | string |  |
| VisibleFrom | When the folder is visible from | date |  |
| VisibleTo | When the folder is visible to | date |  |
| Deadline | The folder deadline. After this date the folder is no longer editable | date |  |
| AvailableTools | The list of available tools for the folder | Collection of string |  |
| AssignTo | Who the folder is assigned to. If null or empty all participants are assigned | Folder.AssignToModel |  |
| ReplaceVisibilityInSubElements | Will replace this folder visibility settings in all sub elements. | boolean |  |
| PlanId | If present. The created element should be added to the plan with this id | integer |  |

## Folder.FolderResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| FolderId | The folder identifier. Same as elementId | integer |  |

## Folder.ParticipantModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId |  | integer |  |
| FullName |  | string |  |
| ExtraInformation |  | string |  |
| GroupId | The group identifier if the person is joined as a group, otherwise null. | integer |  |
| GroupName | The group name if the person is joined as a group, otherwise null. | string |  |
| PictureUrl |  | string |  |

## Folder.ParticipantsResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | List of participants. | Collection of Folder.ParticipantModel |  |

## FollowUpTask

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | Task title. | string |  |
| LocationTitle | The name of the course that the task belong. | string |  |
| NumberOfNewOrUpdatedSubmissions | The number of new and updated submissions for activities where the teacher does not need to assess manually. Submissions are considered new or updated if they happen since teacher last visited the activity. | integer |  |
| NumberOfSubmissionsToAssess | The number of submissions for activities where the teacher need to assess manually. | integer |  |
| TotalNumberOfEvaluateSubmissions | Sum of and . | integer |  |
| Url | The itslearning internal URL for the task. | string |  |
| ContentUrl | The itslearning content URL for the task. | string |  |
| IconUrl | Url to related element icon. | string |  |
| BrandIconUrl | Url to a branded element icon. Used in tasklist's teacher view | string |  |
| ElementId | The identifier of related element. | integer |  |
| ElementType | Type of related element. | ElementType |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. This field applicable only if ElementType is LearningToolElement. | integer |  |

## HierarchyExtendedRole

| Name | Value | Description |
|---|---|---|
| Unknown | 0 | Hierarchy role is not defined. |
| Administrator | 1 | Administrators. |
| Teacher | 2 | Teachers. |
| Student | 3 | Learners. |
| Guest | 4 | All the others users, also external profiles. |

## HierarchySimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyId | Gets or sets the hierarchy id. | integer |  |
| ParentHierarchyId | Gets or sets the parent hierarchy id. | integer |  |
| Title | Gets or sets the title. | string |  |
| Path | Gets or sets the hierarchy's path within the hierarchy tree. | string |  |
| OrganizationType | Gets or sets the type of the hierarchy. | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.OrganizationType |  |
| OrganizationHierarchyId | Gets or sets the organization id to which the hierarchy belongs to. | integer |  |

## HierarchySimpleWithMemberCount

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

## ImmersiveReaderInfo

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | The title to send to the immersive reader | string |  |
| Text | The text to send to the immersive reader | string |  |
| AccessToken | The access token to use with the immersive reader | string |  |
| SubDomain | The subdomain to use with the immersive reader | string |  |
| UiLang | The current language used in the UI | string |  |

## InstantMessage

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

## InstantMessageFilterSuggestion

| Name | Description | Type | Additional information |
|---|---|---|---|
| Type | Type can be: OneToOne, Course, Project, Group | string |  |
| Id | PersonId if TypeName == OneToOne, or ThreadId if otherwise | integer |  |
| Label | Label for the suggestion | string |  |

## InstantMessageListModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Messages | List of the messages itself. | EntityList<InstantMessage> |  |
| HasMore | If there is more data left to retrieve. | boolean |  |

## InstantMessageMassMessageType

| Name | Value | Description |
|---|---|---|
| None | 1 | Common way to deliver message |
| OneWay | 2 | One way message (used when there is more than 300 recipients) |
| Broadcast | 3 | Broadcast message (created by admin to send to hierarchies) |

## InstantMessageParticipant

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The unique id of a person | integer |  |
| Name | Name to display | string |  |
| AvatarUrl | URL to avatar image for this participant | string |  |
| LocationType | LocationType tells which type of "resource" the LocationId refers to, Course = 1, Project = 2, Hierarchy = 3 etc. | integer |  |
| LocationId | Id of Course, Project, Hierarchy etc. see LocationType | integer |  |

## InstantMessagePermissions

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

## InstantMessageRecipient

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

## InstantMessageRecipientRole

| Name | Value | Description |
|---|---|---|
| Guest | 1 | Instant Message recipient with role Guest. |
| Student | 2 | Instant Message recipient with role Student. |
| Teacher | 3 | Instant Message recipient with role Teacher. |
| Administrator | 4 | Instant Message recipient with role Administrator. |
| Parent | 5 | Instant Message recipient with role Parent. |

## InstantMessageSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| MaxAttachmentFileSizeInBytes | Gets the maximum file size in bytes an attachment can have. | integer |  |
| MaxParticipantsInAMessageThread | Gets the maximum participants in a message thread. | integer |  |
| MaxParticipantsInAOneWayThread | Gets the maximum participants in a one way thread. | integer |  |
| IllegalFileExtensions | Gets an array of illegal file extensions. Files with illegal file extensions are not allowed to upload as an attachment to a message. | Collection of string |  |
| SignalRHubEndpoint | The signalR endpoint for instant message system for the current user. This varies by customer | string |  |

## InstantMessageSettingsV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| SignalRServiceVersion | Determines which version of SignalR service is used. This varies by customer. | InstantMessageSignalRServiceVersion |  |
| MaxAttachmentFileSizeInBytes | Gets the maximum file size in bytes an attachment can have. | integer |  |
| MaxParticipantsInAMessageThread | Gets the maximum participants in a message thread. | integer |  |
| MaxParticipantsInAOneWayThread | Gets the maximum participants in a one way thread. | integer |  |
| IllegalFileExtensions | Gets an array of illegal file extensions. Files with illegal file extensions are not allowed to upload as an attachment to a message. | Collection of string |  |
| SignalRHubEndpoint | The signalR endpoint for instant message system for the current user. This varies by customer | string |  |

## InstantMessageSignalRServiceVersion

| Name | Value | Description |
|---|---|---|
| V1 | 0 | Version one, deprecated in the end of 2024. Entry left for compatibility. |
| V2 | 1 | Version two. |

## InstantMessageThread

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

## InstantMessageThreadListModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Threads | List of threads itself. | EntityList<InstantMessageThread> |  |
| HasMore | If there is more data left to retrieve. | boolean |  |

## InstantMessageThreadSearchParams

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

## InstantMessageToPatch

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageId | The instant messageId to patch. | integer, 64 bit |  |
| Text | The text of the message | string |  |

## InstantMessageToSend

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | CourseId to send instant message to. | integer |  |
| ProjectId | ProjectId to send instant message to. | integer |  |
| FileId | Guids of files already uploaded to temporary file repo storage | Collection of string |  |
| ToPersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId or CourseIds or ProjectIds need to have a valid value. | Collection of integer |  |
| InstantMessageThreadId | The thread to send to, if this is null or zero we will assume it's a new thread. | integer |  |
| Text | The text of the message | string |  |
| SendAsIndividualMessages | Gets whether a message to multiple recipients should be sent as individual messages | boolean |  |

## InstantMessageToSendV2

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

## LearningObjectiveProgressReport.AggregatedReport

| Name | Description | Type | Additional information |
|---|---|---|---|
| SubjectId | The subject id a report is constructed for. | integer |  |
| SubjectSummary | A list of assessment status entities representing summary information about the subject. (e.g. how many Meets, Exceeds etc. assessment status items are there). | Collection of LearningObjectiveProgressReport.AggregatedReportAssessmentStatus |  |
| SubjectMasteryPercentage | The percentage of mastered learning objectives in the subject (across all clusters). (it is calculated as percentage from all assessed learning objectives in the cluster). | decimal number |  |
| Clusters | The list of learning objective clusters in the current subject. | Collection of LearningObjectiveProgressReport.ReportCluster |  |
| UseMastery | Indicates whether mastery is used. | boolean |  |
| FullReportUrl | Full report URL. Directs to a person profile page with learning objectives tab selected. | string |  |

## LearningObjectiveProgressReport.AggregatedReportAssessmentStatus

| Name | Description | Type | Additional information |
|---|---|---|---|
| Count | The count of each learning objectives statuses types. | integer |  |
| Label | The learning objective assessment status label. | string |  |
| StatusType | The type of the learning objective status (meets, doesn't meet, etc.). | integer |  |

## LearningObjectiveProgressReport.ReportAssessmentStatus

| Name | Description | Type | Additional information |
|---|---|---|---|
| Label | The learning objective assessment status label. | string |  |
| StatusType | The type of the learning objective status (meets, doesn't meet, etc.). | integer |  |

## LearningObjectiveProgressReport.ReportCluster

| Name | Description | Type | Additional information |
|---|---|---|---|
| ClusterId | Gets or sets the cluster identifier. | integer |  |
| Title | Gets or sets the title. | string |  |
| Description | Report cluster description. | string |  |
| ClusterSummary | A list of assessment status us entities representing summary information about the cluster. (e.g. how many Meets, Exceeds etc. assessment status items are there). | Collection of LearningObjectiveProgressReport.AggregatedReportAssessmentStatus |  |
| ClusterMasteryPercentage | The percentage of mastered learning objectives in the cluster (it is calculated as percentage from all assessed learning objectives in the cluster). | decimal number |  |
| Groups | The list of learning objective groups in the current cluster grouped by their parent. | Collection of LearningObjectiveProgressReport.ReportGroup |  |

## LearningObjectiveProgressReport.ReportGroup

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupId | Repository learning objective id of direct parent of learning objective. | integer |  |
| Parents | The list of all parents from the cluster learning objective for the direct parent of learning objectives in this group. | Collection of LearningObjectiveProgressReport.ReportLearningObjectiveParent |  |
| ReportGroupItems | The list of learning objective report items in the current group. | Collection of LearningObjectiveProgressReport.ReportGroupItem |  |

## LearningObjectiveProgressReport.ReportGroupItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningObjectiveId | Gets or sets the learning objective identifier. | integer |  |
| Title | Gets or sets the title. | string |  |
| Description | Gets or sets the description. | string |  |
| Status | Gets or sets the status. | LearningObjectiveProgressReport.ReportAssessmentStatus |  |
| Mastered | Gets or sets a value indicating whether this is mastered. | boolean |  |
| MasteredDate | Gets or sets the mastered date. Can be null if item is not mastered. | date |  |

## LearningObjectiveProgressReport.ReportLearningObjectiveParent

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Gets or sets the learning objective identifier. | integer |  |
| Title | Gets or sets the title. | string |  |
| Description | Gets or sets the description. | string |  |

## LearningObjectiveProgressReport.ReportSubject

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The subject identifier. | integer |  |
| Title | The subject name. | string |  |

## LearningObjectiveRepository.SubjectLabel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Id of the subject label | integer |  |
| Name | Name of the subject label | string |  |
| Aliases | Collection of aliases belonging to this subject label | Collection of string |  |
| CountryCode | Country code belonging to this subject label | string |  |

## LearningObjectiveType

| Name | Value | Description |
|---|---|---|
| Subject | 1 | Subject |
| Folder | 2 | Folder |
| LearningObjective | 3 | Single learning objective |
| Category | 4 | Category |

## LearningRecordStore.LearningRecordStoreSecretModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| SecretId | Gets or sets the secret ID. | integer |  |
| Secret | Gets or sets the client secret. | string |  |
| ActivationDate | Gets or sets the value of the activation date. | string |  |
| CreatedDate | Gets or sets the value of the creation date. | string |  |
| SecretStatus | If the secret is active or not. | string |  |
| IsExpired | If the secret has expired. | boolean |  |

## LearningRecordStore.OrganisationModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| OrganisationId | Gets or sets the organisation ID. | integer |  |
| Name | Gets or sets the name of the organisation. | string |  |
| Type | Type of organisation. | string |  |

## LearningRecordStore.OrganisationTreeModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| ReadOnly | Gets or sets a value indicating whether the organisation is read-only. If it is a read only it cannot be selected. It is there to maintain the tree structure. | boolean |  |
| IsOrganisation | If this is a organisation level. Sometimes this is only a hierarchy node to group organisations. | boolean |  |
| Children | Gets or sets the child organisations. | Collection of LearningRecordStore.OrganisationTreeModel |  |
| Selected | If the organisation is associated with the partner or not. This is used when updating the organisations for a partner, to know which organisations to add or remove. | boolean |  |
| OrganisationId | Gets or sets the organisation ID. | integer |  |
| Name | Gets or sets the name of the organisation. | string |  |
| Type | Type of organisation. | string |  |

## LearningRecordStore.PartnerExtendedModel

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

## LearningRecordStore.PartnerModel

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

## LearningToolElementAnswerWithWorkload

| Name | Description | Type | Additional information |
|---|---|---|---|
| AnswerId | The ID of this particular answer | integer |  |
| UserIds | The IDs of the users who will answer the assignment | Collection of integer |  |
| AssessmentStatus | The Assessment Status of this particular answer | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.AssessmentStatusItem |  |
| Deadline | The deadline as a representation string | string |  |
| Url | The url | string |  |
| LearningObjectId | The learning object ID | integer |  |
| LearningObjectInstanceId | The ID of the learning object instance | integer |  |
| UserId | The ID of the current user in the assignment service | integer |  |
| Workload | The workload (not set, low, medium, high) of the assignment | WorkloadType |  |
| WorkloadDisplay | The workload term | string |  |
| WorkloadEstimation | The estimated workload of the assignment in minutes | integer |  |
| ElementId |  | integer |  |
| ElementType |  | integer |  |
| Title |  | string |  |
| LearningToolId |  | integer |  |
| ElementOrder |  | integer |  |
| ActiveFromDT |  | date |  |
| ActiveToDT |  | date |  |
| DeadlineDT |  | date |  |

## LightBulletins.LightBulletin

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The unique id for for the light bulletin, in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| EmbedUrl | A link to 3rd party content, which can be embedded or inserted as OGP. | string |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. If there are some comments already, they can still be seen. | boolean |  |
| ElementLinks | The collection of element links attached to the light bulletin. | Collection of ElementLink |  |

## LightBulletins.LightBulletinIdentifier

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The light bulletin identifier | integer |  |

## LightBulletins.LightBulletinImage

| Name | Description | Type | Additional information |
|---|---|---|---|
| PreviewFileId | The identifier of the preview file for the attached image. | integer |  |
| PreviewFileUrl | The Url to the preview file for the attached image. | string |  |
| OriginalFileId | The identifier of the original file for the attached image. | integer |  |
| OriginalFileUrl | The Url to the original file for the attached image. | string |  |
| PreviewFilePngUrl | The Url to an extra raster (png) duplicate of the preview file, populated only when the preview file is an svg (which mobile clients cannot render). null when the preview file is already a raster image and therefore needs no duplicate. | string |  |

## LightBulletins.LightBulletinOutputImageData

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsError | True if an error occurred while uploading the image. | boolean |  |
| ErrorMessage | The error message when uploading an image. | LocalizedText |  |
| PreviewFile | The file information, that contains preview for attached image. | Updatable.FileDataV1 |  |
| OriginalFile | The file information, that contains attached image. | Updatable.FileDataV1 |  |

## LightBulletins.LightBulletinV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The unique id for for the light bulletin, in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| EmbedUrl | A link to 3rd party content, which can be embedded or inserted as OGP. | string |  |
| Pinned | Defines whether current announcement is pinned. | boolean |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. If there are some comments already, they can still be seen. | boolean |  |
| HasResources | DEPRECATED Use ResourcesCount instead. Defines whether any resources were attached to the bulletin. | boolean |  |
| AttachedImages | The list of images attached to the light bulletin. | Collection of LightBulletins.LightBulletinImage |  |
| ResourcesCount | The count of resources attached to the light bulletin. | integer |  |
| CommentsCount | The count of comments for the light bulletin. | integer |  |
| ActiveFromDate | The time when to show the bulletin. | date |  |
| ActiveToDate | The time when to remove the bulletin. | date |  |
| IsSubscribed | Defines if person is subscribed on light bulletin notifications. | boolean |  |
| LexicalJson | Raw Lexical JSON content | string |  |
| PublishedDate | The time when the bulletin was created. | date |  |
| PublishedBy | The person who created the light bulletin. | PersonSimple |  |

## LocalizedText

| Name | Description | Type | Additional information |
|---|---|---|---|
| LocalizedKey | Localized key. Use this if the mobile-app should translate messages itself. | string |  |
| LocalizedArgs | List of separated strings holding arguments for the localized key. | Collection of string |  |

## LocationRecipient

| Name | Description | Type | Additional information |
|---|---|---|---|
| LocationRecipientId | The id for the location. | integer |  |
| Type | Gets or sets the type of the location. | LocationRecipientType |  |
| Title | The title for the location | string |  |
| ColorSchema | The colorSchema for this thread. Only applies if type is 'Course' | string |  |

## LocationRecipientType

| Name | Value | Description |
|---|---|---|
| Course | 1 | Location type course |
| Project | 2 | Location type project |
| Hierarchy | 3 | Location type hierarchy |
| Collaboration | 4 | Location type collaboration |

## LocationType

| Name | Value | Description |
|---|---|---|
| Course | 0 | Course location. |
| Project | 1 | Project location. |

## LogoutType

| Name | Value | Description |
|---|---|---|
| Full | 1 | Log out from itslearning and from IdP. |
| ItslearningOnly | 2 | Log out from itslearning, but keep session with IdP. |
| CloseBrowser | 3 | Do not log out from itslearning nor IdP. When user clicks 'Log out' in itslearning, a message is displayed telling the user to close the browser in order to log out. |
| RedirectLogout | 4 | Log out from itslearning and redirect to a customer specified Url. |
| DirectIdpHttpRequest | 5 | Google specific. Since Google does not support SAML Single Log Out scenario, this is a workaround (recommended by G Suite Support) to logout Google sessions with direct HTTP request to Google - after logout from itslearning. |

## MessagesDisabledThreadParticipants

| Name | Description | Type | Additional information |
|---|---|---|---|
| Count | Number of instant message conversation participants who have messages disabled. | integer |  |
| ThreadParticipants | The list of instant message conversation participants. Can be null in case if there's no such participants or if there are too many of them. | Collection of ThreadParticipant |  |

## NewsFeedItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| NewsFeedItemId | The unique id for for the news feed item, in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the news feed item | string |  |
| LocationTitle | The news feed item's project or coursetitle. | string |  |
| PublishedDate | Date time when the news feed item was published. Time zone is UTC. | date |  |
| PublishedBy | The person who published the news feed item. | PersonSimple |  |
| Url | Url to the location in itslearning connected to the news feed item. | string |  |
| IconUrl | Url to the icon used by itslearning for the news feed item. | string |  |

## Notification

| Name | Description | Type | Additional information |
|---|---|---|---|
| NotificationId | The unique id for for the notification, in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the notification | string |  |
| PublishedDate | Date time when the notification was published. Time zone is UTC. | date |  |
| PublishedBy | The person who published the notification. | PersonSimple |  |
| Type | The type of notification. | NotificationType |  |
| Url | Url to the location in itslearning connected to the notification. | string |  |
| ContentUrl | Url to the location in itslearning connected to the notification that open without master page. | string |  |
| IsRead | True if the user has read the notification. | boolean |  |
| IsAnonymous | True if notification is anonymous | boolean |  |

## NotificationType

| Name | Value | Description |
|---|---|---|
| Unknown | 0 | Unknown type, this should normally not be returned. |
| Assessment | 1 | Assessment, could be that e.g. an assignment has been assessed. |
| Behaviour | 5 | A notification related to behaviour. |

## PersonContextRole

| Name | Value | Description |
|---|---|---|
| Member | 1 | The member role. It is a default role. |
| Learner | 2 | The learner role. |
| Instructor | 3 | The instructor role. |
| ContentDeveloper | 4 | The content developer role. |
| Manager | 5 | The manager role. |
| Mentor | 6 | The mentor role. |
| Administrator | 7 | The administrator role. |
| TeachingAssistant | 8 | The teaching assistant role. |

## PersonCourse

| Name | Description | Type | Additional information |
|---|---|---|---|
| LastUpdatedUtc | The course last updated date. Time zone is UTC. Can be null if it was never updated. | date |  |
| NewNotificationsCount | The new notifications count in course for person. | integer |  |
| NewBulletinsCount | The new bulletins in course for person. | integer |  |
| Url | The itslearning internal URL for the course. | string |  |
| HasAdminPermissions | Defines if person has administration permissions in the course. | boolean |  |
| HasStudentPermissions | Defines if person has student permissions in the course. | boolean |  |
| CourseFocus | Defines type of the course. | CourseFocus |  |
| TopicColumnName | Defines the name of the topic column the course in case is is custom, otherwise the value is null. | string |  |
| CourseId | The unique id for the course, in scope of a site (itslearning customer). | integer |  |
| Title | The course title. | string |  |
| FriendlyName | The course friendly name. | string |  |
| CourseColor | Color code for the course. | string |  |
| CourseFillColor | Fill color code for the course. | string |  |
| CourseCode | Code for the course. | string |  |

## PersonCourseBase

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The unique id for the course, in scope of a site (itslearning customer). | integer |  |
| Title | The course title. | string |  |
| FriendlyName | The course friendly name. | string |  |
| CourseColor | Color code for the course. | string |  |
| CourseFillColor | Fill color code for the course. | string |  |
| CourseCode | Code for the course. | string |  |

## PersonCourseCardData

| Name | Description | Type | Additional information |
|---|---|---|---|
| FollowUpTaskCount | The follow-up tasks in course for person. | integer |  |
| TaskCount | The tasks in course for person. | integer |  |
| TeachersInCourse | The list of teachers in course. | Collection of PersonSimple |  |
| Role | Role of the (current) user in the course | PersonContextRole |  |
| PlanType | The plan type in the course | Itsolutions.ItslUtils.Constants.PlannerConstants+PlanType |  |
| LastUpdatedUtc | The course last updated date. Time zone is UTC. Can be null if it was never updated. | date |  |
| NewNotificationsCount | The new notifications count in course for person. | integer |  |
| NewBulletinsCount | The new bulletins in course for person. | integer |  |
| Url | The itslearning internal URL for the course. | string |  |
| HasAdminPermissions | Defines if person has administration permissions in the course. | boolean |  |
| HasStudentPermissions | Defines if person has student permissions in the course. | boolean |  |
| CourseFocus | Defines type of the course. | CourseFocus |  |
| TopicColumnName | Defines the name of the topic column the course in case is is custom, otherwise the value is null. | string |  |
| CourseId | The unique id for the course, in scope of a site (itslearning customer). | integer |  |
| Title | The course title. | string |  |
| FriendlyName | The course friendly name. | string |  |
| CourseColor | Color code for the course. | string |  |
| CourseFillColor | Fill color code for the course. | string |  |
| CourseCode | Code for the course. | string |  |

## PersonCourseCardDataV4

| Name | Description | Type | Additional information |
|---|---|---|---|
| TeachersInCourse | The list of teachers in course. | Collection of PersonSimple |  |
| Role | Role of the (current) user in the course. | PersonContextRole |  |
| PlanType | The plan type in the course. | Itsolutions.ItslUtils.Constants.PlannerConstants+PlanType |  |
| LastUpdatedUtc | The course last updated date. Time zone is UTC. Can be null if it was never updated. | date |  |
| NewNotificationsCount | The new notifications count in course for person. | integer |  |
| NewBulletinsCount | The new bulletins in course for person. | integer |  |
| Url | The itslearning internal URL for the course. | string |  |
| HasAdminPermissions | Defines if person has administration permissions in the course. | boolean |  |
| HasStudentPermissions | Defines if person has student permissions in the course. | boolean |  |
| CourseFocus | Defines type of the course. | CourseFocus |  |
| TopicColumnName | Defines the name of the topic column the course in case is is custom, otherwise the value is null. | string |  |
| CourseId | The unique id for the course, in scope of a site (itslearning customer). | integer |  |
| Title | The course title. | string |  |
| FriendlyName | The course friendly name. | string |  |
| CourseColor | Color code for the course. | string |  |
| CourseFillColor | Fill color code for the course. | string |  |
| CourseCode | Code for the course. | string |  |

## PersonCourseCardTaskCount

| Name | Description | Type | Additional information |
|---|---|---|---|
| FollowUpTaskCount | The follow-up tasks in course for a person. | integer |  |
| TaskCount | The tasks in course for a person. | integer |  |

## PersonCourseRank

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The courseId the rank belongs to. | integer |  |
| Rank | The sort order rank. The one with lowest rank will be sorted first in the collection | integer |  |

## PersonImageDimensions

| Name | Description | Type | Additional information |
|---|---|---|---|
| PixelWidth |  | integer |  |
| PixelHeight |  | integer |  |

## PersonProfile

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

## PersonRole

| Name | Value | Description |
|---|---|---|
| None | 0 | Person has no defined role. |
| Admin | 1 | Person is an administrator. The person has the role administrator in one or more hierarchies. |
| Supervisor | 2 | Person is a supervisor (mentor or headteacher). |
| Teacher | 3 | Person is a teacher. The person has the role teacher in one or more courses. |
| Student | 4 | Person is a student. The person has the role student in one or more courses |
| Parent | 5 | Person is a parent. |

## PersonSimple

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

## PersonSimpleWithHierarchyExtendedRole

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyRole | The role in the hierarchy to which a personal profile in itslearning is mapped. | HierarchyExtendedRole |  |
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

## PersonSimpleWithHierarchyRole

| Name | Description | Type | Additional information |
|---|---|---|---|
| HierarchyRole | The role the persons has in the hierarchy. | string |  |
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

## PersonSimpleWithHierarchyRoleAndCourseMembership

| Name | Description | Type | Additional information |
|---|---|---|---|
| MemberInCourse | Wether the person is a member in a given course | boolean |  |
| HierarchyRole | The role the persons has in the hierarchy. | string |  |
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

## PersonSimpleWithWorkload

| Name | Description | Type | Additional information |
|---|---|---|---|
| Workload | The workload for this person. | Workload |  |
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

## PersonUpcoming

| Name | Description | Type | Additional information |
|---|---|---|---|
| Date | Date of upcoming events, plans and tasks. | date |  |
| Courses | List of course items, each item contains basic information about course and related plans/events/tasks. | Collection of CourseUpcoming |  |

## Personal.Ai.AiFeatureType

| Name | Value | Description |
|---|---|---|
| None | 0 |  |
| Article | 1 |  |
| Assignment | 2 |  |
| Translation | 3 |  |
| Glossary | 4 |  |
| Rewrite | 5 |  |
| InspireMe | 6 |  |
| Plan | 7 |  |
| Quiz | 8 |  |
| Worksheet | 9 |  |
| Test | 10 |  |

## Personal.Ai.AiResponseRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Feature | The feature to use. | Personal.Ai.AiFeatureType |  |
| LocationId | Course or project id. | integer |  |
| LocationType | Course or project. | Itsolutions.ItslUtils.Constants.LocationType |  |
| PlanId | Plan identifier. | integer |  |
| SystemPrompt | System prompt for the AI (template). Normally is empty, only taken into account when the prompt engineering feature is enabled. | string |  |
| UserPrompt | User prompt for the AI. | string |  |
| Content | Content if already present. | string |  |
| PreviousResponseId | The previous response identifier for conversation continuation. | string |  |
| StoreToken | Token for the store containing attached files. | string |  |

## Personal.Ai.CreateElementRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementType | The element type | ElementType |  |
| Title | Title of the content | string |  |
| Text | The text | string |  |
| FolderId | The folder id | integer |  |
| PlanId | If created element belongs to a plan | integer |  |
| Feature | The AI feature type of the element (e.g. Assignment, Article, Inspire me etc.). Used to measure KPI when a user saves an element created by AI. | Personal.Ai.AiFeatureType |  |

## Personal.Calendar.Attendance.AttendanceDetails

| Name | Description | Type | Additional information |
|---|---|---|---|
| KeepAttendancePageUrl | Url of internal page for tracking attendance for event. | string |  |
| AttendanceStatistics | Information about attendance statistics in calendar event. | Personal.Calendar.Attendance.AttendanceStatistics |  |

## Personal.Calendar.Attendance.AttendanceStatistics

| Name | Description | Type | Additional information |
|---|---|---|---|
| Present | Number of people which were present. | integer |  |
| Absent | Number of people which were absent. | integer |  |
| Excluded | Number of people which were excluded. | integer |  |

## Personal.Calendar.Booking

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | Room or equipment name. | string |  |

## Personal.Calendar.CalendarEventDetails

| Name | Description | Type | Additional information |
|---|---|---|---|
| EventUrl | The URL in itslearning web to open the calendar event in the web view. In case of course event there's the URL pointing to the course dashboard with opened calendar event pop-up dialog. In case of project or personal events there's the URL pointing to the calendar day view. | string |  |
| HasMoreContent | Indicates whether calendar event contains more information than API can consume. For now it indicates whether Description or ImportDescription contain HTML text which is striped by API. | boolean |  |
| Bookings | Rooms or equipment booked for the event. | Collection of Personal.Calendar.Booking |  |
| ElementLinks | Elements connected to the event. | Collection of ElementLink |  |
| PlanLinks | Plans relevant for the event. | Collection of Personal.Calendar.PlanLink |  |
| EventId |  | integer |  |
| FromDate |  | date |  |
| ToDate |  | date |  |
| EventTitle |  | string |  |
| Description |  | string |  |
| ImportDescription |  | string |  |
| EventType |  | Personal.Calendar.CalendarEventType |  |
| LocationId |  | integer |  |
| LocationGroupId |  | integer |  |
| LocationColor |  | string |  |
| LocationFillColor |  | string |  |
| LocationTitle |  | string |  |
| LocationFriendlyName |  | string |  |
| HidePersonalWordForPersonalEvent | If true and `EventType` is `Personal` then the word "Personal" must be hidden from UI for this event. This property is always false for non personal event types. | boolean |  |
| AttendanceDetails |  | Personal.Calendar.Attendance.AttendanceDetails |  |

## Personal.Calendar.CalendarEventType

| Name | Value | Description |
|---|---|---|
| Course | 0 | Event which happens in a specific course. |
| Project | 1 | Event which happens in a specific project. |
| Personal | 2 | Event which happens in calendar specific to a certain person. |
| Hierarchy | 3 | Event which happens in a specific hierarchy. |

## Personal.Calendar.CalendarEventV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| EventId |  | integer |  |
| FromDate |  | date |  |
| ToDate |  | date |  |
| EventTitle |  | string |  |
| Description |  | string |  |
| ImportDescription |  | string |  |
| EventType |  | Personal.Calendar.CalendarEventType |  |
| LocationId |  | integer |  |
| LocationGroupId |  | integer |  |
| LocationColor |  | string |  |
| LocationFillColor |  | string |  |
| LocationTitle |  | string |  |
| LocationFriendlyName |  | string |  |
| HidePersonalWordForPersonalEvent | If true and `EventType` is `Personal` then the word "Personal" must be hidden from UI for this event. This property is always false for non personal event types. | boolean |  |
| AttendanceDetails |  | Personal.Calendar.Attendance.AttendanceDetails |  |

## Personal.Calendar.PlanLink

| Name | Description | Type | Additional information |
|---|---|---|---|
| PlanId | The plan identifier. | integer |  |
| PlanName | The plan name. | string |  |
| TopicName | The topic name. | string |  |
| Url | The url to view plan in itslearning. | string |  |

## Personal.CalendarEvent

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The calendar event identifier. | integer |  |
| FromDate |  | date |  |
| ToDate |  | date |  |
| EventTitle |  | string |  |
| Description |  | string |  |
| ImportDescription |  | string |  |
| CourseId | The course identifier. | integer |  |
| CourseGroupId | The course group identifier. | integer |  |
| LocationColor |  | string |  |
| LocationFillColor |  | string |  |
| LocationTitle |  | string |  |
| LocationFriendlyName |  | string |  |
| AttendanceDetails |  | Personal.Calendar.Attendance.AttendanceDetails |  |

## Personal.Course.CourseAction

| Name | Value | Description |
|---|---|---|
| UserImportsAnImsccPackage | 1 | User imports an IMSCC package |
| UserImportsAMbzPackage | 2 | User imports a MBZ package |
| UserExportsCourseAsImsccPackage | 3 | User exports course as IMSCC package |
| UserExportsCourseAsMbzPackage | 4 | User exports course as MBZ package |
| CourseIsCreatedByBulkImportForImsccPackage | 5 | Course is created by bulk import for IMSCC package |
| CourseIsCreatedByBulkImportForMbzPackage | 6 | Course is created by bulk import for MBZ package |

## Personal.Course.CourseAiSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The course identifier | integer |  |
| Subject | The Subject of the course | string |  |
| Curriculum | Curriculum for the course | string |  |
| Language | Language for the AI generated content | string |  |
| Age | Age group of students in the course | string |  |

## Personal.Course.CourseApplicant

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| PictureUrl | Avatar Url | string |  |
| Status | EnrrollmentStatus | integer |  |
| Mode | EnrrollmentMode | integer |  |
| Email | Email | string |  |
| IsExternal | Defines if the person is external. | boolean |  |
| CourseId | The course's identifier. | integer |  |
| IsMember | If true then the person is a member of the course. | boolean |  |
| DefaultOrganisation | Default organisation for the person | string |  |
| Role | Person role | string |  |
| CustomerName | The name of the customer site the person belongs to. | string |  |

## Personal.Course.CourseFolderDetails

| Name | Description | Type | Additional information |
|---|---|---|---|
| Resources | List of resources in a folder. | EntityList<Personal.Course.CourseResource> |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |

## Personal.Course.CourseParticipant

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| LastVisited | Last time the course participant visited this course | date |  |
| LastVisitedRelative | Last time the course participant visited this course relative to the current time Examples: 3 days ago, 2 hours ago, Just now, etc. | string |  |
| PictureUrl | Avatar Url | string |  |
| Role | The role that the course participant has in the course | string |  |
| RoleId | The role id that the course participant has in the course | integer |  |
| CompletedTasks | Number of tasks in this course that the course participant has completed | integer |  |
| TotalTasks | Total number of tasks in this course for the course participant | integer |  |
| ExtraInformation | Extra information for this participant | string |  |
| CanHaveTasks | Show if course participant can have tasks | boolean |  |
| Groups | Comma-separated list of course groups the participant is a member of | string |  |
| TemporaryAccess | Boolean value indicating if the participant has been granted temporary access to the course | boolean |  |

## Personal.Course.CourseParticipantUnenrolled

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| TemporaryAccess | When the course participant was deleted | boolean |  |
| DateUnenrolled | When the course participant was deleted | date |  |
| DateUnenrolledRelative | When the course participant was deleted from course relative to the current time Examples: 3 days ago, 2 hours ago, Just now, etc. | string |  |
| MemberOfCourseGroups | Full name of the course groups | string |  |
| PictureUrl | Avatar Url | string |  |
| AssessmentUrl | Url to assessment record | string |  |
| ExtraInformation | Extra information. | string |  |

## Personal.Course.CourseResource

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | Title of element. | string |  |
| ElementId | Id of element. | integer |  |
| ElementType | Type of element. | ElementType |  |
| CourseId | Id of a course where the element is located. | integer |  |
| Url | Url to the itslearning web page for the element. | string |  |
| ContentUrl | Url to the content itslearning web page for the element. | string |  |
| IconUrl | Url to element icon. | string |  |
| BrandIconUrl | Url to a branded element icon. | string |  |
| ExtraImageUrlForMobile | Image that is set for the resource. Non-svg so mobile apps can use it. | string |  |
| Active | Whether element/folder is active or inactive. | boolean |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. | integer |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |
| Homework | Marks the element as homework. | boolean |  |
| Path | The resource's path in the course folder tree. | string |  |
| LearningObjectId | ID of learning object | integer |  |
| LearningObjectInstanceId | ID of learning object instance | integer |  |
| IsTopicOrPlanFolder | True when the element is a folder connected to a topic or a plan. Such folders must remain visible and expandable but must not be selectable when adding resources to a plan. | boolean |  |

## Personal.Course.CourseResourceWithRole

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseCode | Code of the course. | string |  |
| CourseTitle | Title of the course. | string |  |
| ContextRole | The context role for the current person . | PersonContextRole |  |
| Title | Title of element. | string |  |
| ElementId | Id of element. | integer |  |
| ElementType | Type of element. | ElementType |  |
| CourseId | Id of a course where the element is located. | integer |  |
| Url | Url to the itslearning web page for the element. | string |  |
| ContentUrl | Url to the content itslearning web page for the element. | string |  |
| IconUrl | Url to element icon. | string |  |
| BrandIconUrl | Url to a branded element icon. | string |  |
| ExtraImageUrlForMobile | Image that is set for the resource. Non-svg so mobile apps can use it. | string |  |
| Active | Whether element/folder is active or inactive. | boolean |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. | integer |  |
| AddElementUrl | Url to itslearning to add new element. It's empty if user doesn't have permissions to add new element. | string |  |
| Homework | Marks the element as homework. | boolean |  |
| Path | The resource's path in the course folder tree. | string |  |
| LearningObjectId | ID of learning object | integer |  |
| LearningObjectInstanceId | ID of learning object instance | integer |  |
| IsTopicOrPlanFolder | True when the element is a folder connected to a topic or a plan. Such folders must remain visible and expandable but must not be selectable when adding resources to a plan. | boolean |  |

## Personal.Course.CreateFileResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Element id for the file created | integer |  |
| CloudId | The cloud id of the file created | string |  |
| ErrorMessage | The error message if creation failed | string |  |
| ErrorCodes | The error message if creation failed | Collection of Personal.Course.FileErrorCode |  |
| Success | If the creation was successful or not | boolean |  |
| Title | The title for the newly created file element | string |  |

## Personal.Course.ElementSimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Element ID. | integer |  |

## Personal.Course.FileErrorCode

| Name | Value | Description |
|---|---|---|
| InvalidFileExtension | 1 | Invalid file extension |
| InvalidUploadedContent | 2 | Invalid uploaded content |
| FileSizeIsTooBig | 3 | File size is too big |
| ContentSizeExceedsQuota | 4 | Content of file exceeds quota |
| StructureIsEmpty | 5 | The folder structure is empty |
| FileIsEmpty | 6 | The file is empty |
| NotZipFile | 7 | The file is not a zip file |
| ZipFileIsCorrupted | 8 | The zip file is corrupted |
| ZipFileIsPasswordProtected | 9 | The zip file is password protected |
| FileNameContainsInvalidCharacters | 10 | The file name contains invalid characters |
| IllegalFirstPartOfFileName | 11 | The first part of the file name is illegal |
| InvalidParentFolderName | 12 | The parent folder name is invalid |
| InvalidFolderName | 13 | The folder name is illegal |
| FileNameTooLong | 14 | The file name is too long |
| FolderNameTooLong | 15 | The folder name is too long |
| PathTooLong | 16 | The path is too long |
| InvalidElementType | 17 | Invalid element type |
| TooManyZipEntries | 18 | Too many zip entries |
| UnexpectedError | 19 | Unexpected error |
| InvalidCustomerId | 20 | Invalid customer id |
| InvalidPersonId | 21 | Invalid person id |
| InvalidParentFolderId | 22 | Invalid parent folder id |
| FileNameIsEmpty | 23 | File name is empty |
| InvalidFileIdentifier | 24 | Invalid file identifier |
| InvalidFileSignature | 25 | Invalid file signature |
| ViewUrlIsEmpty | 26 | The view url is empty |
| InvalidFolder | 27 | Invalid folder |
| InvalidFile | 28 | Invalid file |
| InvalidCloudId | 29 | Invalid cloud id |
| InvalidOAuthUser | 30 | Invalid OAuth user |

## Personal.Course.GetCourseResourcesPayload

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementType | Type of element | Itsolutions.ItslUtils.Constants.ElementType |  |
| LearningToolIds | IDs of learning tool (extension IDs) | Collection of integer |  |
| LearningToolType | Type of learning tool | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolType |  |
| ElementPermission | Permissions for the element | Itslearning.Platform.RestApi.Sdk.LearningToolApp.Entities.ElementPermission |  |
| ElementIds | Element IDs | Collection of integer |  |
| IncludeDeleted | Whether to include deleted resources | boolean |  |
| IncludeExtraWorkElements | Whether to include extra work elements | boolean |  |

## Personal.Course.PageSetup.AssessmentRecordLanguage

| Name | Description | Type | Additional information |
|---|---|---|---|
| AddAssessment | Add Assessment to assessment record | string |  |
| AddCategories | Add categories | string |  |
| AddGroups | Add groups | string |  |
| AddToGradebook | Add to Gradebook | string |  |
| AllAssessments | All Assessments | string |  |
| AllGroups | All groups | string |  |
| AllParticipants | Assessment Record all participants | string |  |
| AnErrorOccurred | General error message | string |  |
| Apply | Apply | string |  |
| Assess | Assess | string |  |
| AssessmentRecordCategory | Assessment Record category | string |  |
| AssessmentRecordSettings | Assessment Record settings | string |  |
| AssessmentWeight | Assessment weight | string |  |
| AssessmentWeighting | Assessment weighting | string |  |
| AverageAndCount | Average and count | string |  |
| Cancel | Cancel label for button | string |  |
| CannotDeleteFinalAssessment | Cannot delete a final assessment | string |  |
| Categories | categories | string |  |
| CategoryExistsButNotInUseMessage | Category exists but not in use message | string |  |
| CategoryWeight | Category weight | string |  |
| CategoryWeighting | Assessment Record Category weighting | string |  |
| Close | Close | string |  |
| ColumnsNotConnectedToTerms | Columns not connected to terms | string |  |
| CompleteTerm | Complete term | string |  |
| CompletedTermMessage | Term completed message | string |  |
| Confirm | Confirm | string |  |
| Deadline | Deadline | string |  |
| Delete | Delete an element | string |  |
| Dismiss | Dismiss | string |  |
| Edit | Edit an element | string |  |
| ErrorDuringPageLoad | Error message when something goes wrong during page load | string |  |
| ErrorMessage | Something went wrong. Please try again. | string |  |
| Ex | Exemption Abbreviation | string |  |
| Exempt | Exempt the student | string |  |
| ExistingAssessment | Existing assessment | string |  |
| ExportToCsv | Export Assessment Record in CSV | string |  |
| FilterBy | Assessment Record Filter By | string |  |
| FinalAssessment | Final assessment | string |  |
| Functions | Functions | string |  |
| Grade | Grade | string |  |
| GradebookGroup | Assessment Record Group | string |  |
| Groups | Groups | string |  |
| Information | Information | string |  |
| MoreOptions | More options | string |  |
| Name | Student Name | string |  |
| NewGradebookColumn | New gradebook column | string |  |
| NoAssessmentWeighting | There are no assessments in this term; once assessments are added, you will be able to change the weight | string |  |
| NoCategories | There are no categories defined | string |  |
| NoCategoriesCreatedMessage | No categories created message | string |  |
| NoGrading | No grading | string |  |
| NoGroups | There are no groups in this assessment record | string |  |
| NoGroupsCreatedMessage | You don't have any groups set up yet | string |  |
| NoMoreExistingAssessments | All existing assessments are already in the gradebook | string |  |
| NoParticipantsAvailable | No Participant Available | string |  |
| NoStudentsMatchSearch | No students match search text | string |  |
| LearningPathWarning | Displayed when there are not applicable assessment records | string |  |
| NSlashA | Not Applicable | string |  |
| Options | Options | string |  |
| OverallAverage | Student Overall Average | string |  |
| Overdue | Course deadline passed | string |  |
| PageTitle | Assessment Record Page Title | string |  |
| ParticipantsNotInAGroup | Participants not in a group | string |  |
| Percent | Percent | string |  |
| PleaseConfirm | Please confirm | string |  |
| Points | Points | string |  |
| Refresh | Refresh | string |  |
| Retry | Retry label for button | string |  |
| RevertExemption | Revert Exemption | string |  |
| Score | Dismiss | string |  |
| Search | Search participant in assessment record | string |  |
| SearchStudents | Search Students | string |  |
| Sort | Sort | string |  |
| Summary | Assessment Record Table summary | string |  |
| SubmissionLink | Assessment Record submission link. e.g. Tommy's submission for Math. | string |  |
| Term | Term | string |  |
| Title | Title | string |  |
| TransferTo | Transfer to {external system} | string |  |
| TrendsAndProgress | Trends and progress | string |  |
| UnableToExempt | Error message when we are unable to exempt the grade | string |  |
| UnableToOverwrite | Error message when we are unable to overwrite the grade since there are new attempts on the test. | string |  |
| UnableToSetGrade | Error message when we are unable to save the grade | string |  |
| UnableToSetScore | Error message when we are unable to save the score | string |  |
| Uncategorised | Uncategorised | string |  |
| ValidationNumberBetween | Validation message for number between x and y | string |  |
| View | View | string |  |
| ViewInExcel | View Assessment Record in Excel | string |  |
| WarningForCompletingTerm | Warning message for completing the term | string |  |
| WarningForDeletingElement | Warning message for deleting element manually | string |  |
| NotAvailable | Assessment not available. | string |  |

## Personal.Course.PageSetup.AssessmentRecordPaths

| Name | Description | Type | Additional information |
|---|---|---|---|
| StaticContent | The Path for the Static Content | string |  |

## Personal.Course.PageSetup.AssessmentRecordPermissions

| Name | Description | Type | Additional information |
|---|---|---|---|
| HasGeneralAdministrationPermission | Indicates whether the current user has general administration permission in the course | boolean |  |

## Personal.Course.PageSetup.AssessmentRecordSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| Categories | Categories available in the course | Collection of AssessmentRecord.AssessmentRecordCategory |  |
| DefaultProfilePicture | Default picture profile | string |  |
| DefaultProfilePictureSmall | Default picture profile (small) | string |  |
| AssessmentRecordIcon | Assessment record icon | string |  |
| ExternalSystemName | The name of external system for grade transfer | string |  |
| ExtraUserInformation | Extra User information to show | string |  |
| Groups | The course groups defined in the course | Collection of AssessmentRecord.AssessmentRecordGroup |  |
| HasExistingAssessment | Checks if there is any available existing assessments to add to gradebook | boolean |  |
| IsAccessibilityModeEnabled | Checks if accessibility mode is enabled | boolean |  |
| IsAssessmentRecordEnabledForCourse | Checks if the assessment record is enabled for course | boolean |  |
| IsAssessmentRecordSettingsLinkVisible | Checks if assessment record settings link is visible | boolean |  |
| IsAverageAndCountEnabled | Checks if the average and count tab is enabled for course | boolean |  |
| IsCategoryWeightingEnabled | Checks if the categories weighting is enabled | boolean |  |
| IsExemptEnabled | Checks if the exempt is enabled for the site | boolean |  |
| IsSummaryEnabled | Checks if assessment record summary is enabled | boolean |  |
| IsTransferToExternalSystemEnabled | Checks if exportGradebook is enabled and exportAPI is configured | boolean |  |
| LastUsedCategoryIds | Customer last used category ids | Collection of integer |  |
| LocaleDecimalSeparator | Locale decimal separator. | string |  |
| NameFormat | Name format of the person name | Itsolutions.ItslUtils.Constants.NameFormat |  |
| ShowOverallAverage | Show overall average | boolean |  |
| SelectedCategoryIds | Customer last selected category ids | Collection of integer |  |
| SelectedGroupIds | Customer last selected group ids | Collection of integer |  |
| SelectedScoreFormatId | Customer last selected score format id | integer |  |
| SelectedSortOptionId | Customer last selected sort option Id | integer |  |
| SelectedTermId | Customer last selected term Id | integer |  |
| Terms | List of terms associated with the assessment record | Collection of AssessmentRecord.AssessmentRecordTerm |  |
| UseAssessmentWeighting | Checks if assessment weighting is enabled | boolean |  |
| UserName | User name | string |  |
| UseScorePointsForAverage | Use Score Points for average | boolean |  |
| ExtensionModules | Collection of assessment record extension modules. | Collection of Personal.Course.PageSetup.ExtensionModuleModel |  |

## Personal.Course.PageSetup.AssessmentRecordSetup

| Name | Description | Type | Additional information |
|---|---|---|---|
| Language | Language for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordLanguage |  |
| Paths | Paths for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordPaths |  |
| Settings | Settings for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordSettings |  |
| Permissions | Gets or sets the permissions for the assessment record page. | Personal.Course.PageSetup.AssessmentRecordPermissions |  |

## Personal.Course.PageSetup.CourseParticipantLanguage

| Name | Description | Type | Additional information |
|---|---|---|---|
| All | Text for All (used as a value in the role filter) | string |  |
| Add | Add | string |  |
| AddHierarchy | Add hierarchy | string |  |
| AddParticipant | Add participant | string |  |
| BrowseHierarchies | Header text for browsing hierarchies | string |  |
| BrowseUsers | Text for Browse users label | string |  |
| Cancel | Cancel | string |  |
| ChangeRole | Change Role | string |  |
| ChangeRoleFor | Change role for {participant} | string |  |
| Close | Close | string |  |
| ConfirmRemoveParticipant | Confirm to remove 1 participant | string |  |
| ConfirmRemoveMultipleParticipants | Confirm to remove several participants | string |  |
| CouldNotRemoveParticipant | Inform that a participant could not be removed due to hierarchy synchronization | string |  |
| CouldNotRemoveMultipleParticipants | Inform that several participants could not be removed due to hierarchy synchronization | string |  |
| Email | Email | string |  |
| FilteredParticipantsCount | Filtered participants count. | string |  |
| Freely | Freely | string |  |
| Hierarchy | Hierarchy | string |  |
| LastVisited | Last visited language. | string |  |
| MaxAllowedInstantMessageRecipientsExceeded | Text for error message when the max allowed number of recipients for instant message is exceeded | string |  |
| Name | Name language. | string |  |
| NoParticipants | Text for when there are no participants besides the teacher in a course | string |  |
| NoSearchResults | Text for empty search result | string |  |
| Ok | Label for the Done button | string |  |
| OpenProfileCardFor | Open profile card tooltip text | string |  |
| Options | Options | string |  |
| Participants | Participants language. | string |  |
| ParticipantsCount | Participants count. | string |  |
| Password | Used for applications through registration key | string |  |
| Unenrolled | Unenrolled language. | string |  |
| Remove | Label for the Remove button | string |  |
| RemoveParticipantsConfirmation | Text used in confirm dialog when removing participant | string |  |
| Role | Role language. | string |  |
| Save | Save | string |  |
| Search | Search language. | string |  |
| SelectAll | Select All | string |  |
| SelectUsersFrom | Text for Select users from (Hierarchy) label | string |  |
| SendMessage | Text for the button used to send a message to selected participants | string |  |
| Registration | Header text for the application tab | string |  |
| Tasks | Tasks language. | string |  |
| User | Text for user label | string |  |
| Users | Text for users label | string |  |
| AddParticipantsEmptyState | Text for empty state adding participants | string |  |
| SynchronizedHierarchiesEmptyState | Text for empty state adding hierachies | string |  |
| UnerolledParticipantsEmptyState | Text for empty state unenrolled | string |  |
| ApplicantsEmptyState | Text for empty state applicants | string |  |
| Applicants | Text used for the Applicants Tab | string |  |
| ApplicationState | State of the Application | string |  |
| Approve | Text used to approve an application | string |  |
| Reject | Text used to reject an application | string |  |
| Accepted | Text indicating an accepted status in the applicants | string |  |
| Pending | Text indicating a pending status in the applicants | string |  |
| Rejected | Text indicating an rejected status in the applicants | string |  |
| NoApplicants | Text used in empty state for applicant tab | string |  |
| CourseGroups | Heading text in unenrolled list | string |  |
| DateUnenrolled | Heading text in unenrolled list | string |  |
| TemporaryAccess | Heading text in unenrolled list | string |  |
| Assessment | Tootltip for assesment | string |  |
| SendMessageToolTip | Tooltip for sending message to participants | string |  |

## Personal.Course.PageSetup.CourseParticipantPaths

| Name | Description | Type | Additional information |
|---|---|---|---|
| StaticContent | The Path for the Static Content | string |  |
| EmptyStateImage | The subpath to the image for empty state | string |  |

## Personal.Course.PageSetup.CourseParticipantPermissions

| Name | Description | Type | Additional information |
|---|---|---|---|
| HasManageParticipantsAndGroupsPermission | Indicates whether the current user has permission to manage members and groups in the course | boolean |  |
| HasGeneralAdministrationPermission | Indicates whether the current user has general administration permission in the course | boolean |  |
| CanViewDetailedInformation | Reflects the "Can view detailed information" setting | boolean |  |

## Personal.Course.PageSetup.CourseParticipantSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseProfiles | The course profiles available in the course. | Collection of Personal.Course.PageSetup.CourseProfile |  |
| DefaultCourseProfileId | The default course profile identifier. This is the setting is fetched from the course profiles. | integer |  |
| InstantMessageMaxPersonsPerThread | The max number of Instant Message recipients it is allowed to send to | integer |  |
| ShowApplicantTab | Indicates wether the Applicants Tab should be shown to relevant users. | boolean |  |
| FirstNameFirst | Culture for Nameformatting | boolean |  |
| ExtraInformationTitle | Extra Information column | string |  |
| FunctionalityMessages | Is messages functionality enabled | boolean |  |
| CurrentPersonId | The logged in user person id | integer |  |
| TemporaryAccessToCourses | Is temporary access to courses enabled | boolean |  |

## Personal.Course.PageSetup.CourseParticipantSetup

| Name | Description | Type | Additional information |
|---|---|---|---|
| Language | Language for the course participant page. | Personal.Course.PageSetup.CourseParticipantLanguage |  |
| Paths | Paths for the course participant page. | Personal.Course.PageSetup.CourseParticipantPaths |  |
| Settings | Settings for the course participant page. | Personal.Course.PageSetup.CourseParticipantSettings |  |
| Permissions | Gets or sets the permissions for the course participant page. | Personal.Course.PageSetup.CourseParticipantPermissions |  |

## Personal.Course.PageSetup.CourseProfile

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseProfileId | The course profile identifier. | integer |  |
| Title | The title for the course profile | string |  |
| ExistsInCourse | This indicates if the course profile is used by any participants in the course | boolean |  |
| CanBeUsedForAddingParticipantsToCourse | This indicating whether this instance can be used for adding participants to course. | boolean |  |

## Personal.Course.PageSetup.ExtensionModuleModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| ModuleExtensionSetupId | References module extension ID. | integer |  |
| Title | Module title. | string |  |
| OpenInNewWindow | Open all extension modules in a new window. | boolean |  |

## Personal.Course.YourStudent

| Name | Description | Type | Additional information |
|---|---|---|---|
| FallbackPictureUrl | Used if main picture will not be loaded on client side. | string |  |
| CompletedTasks | Completed Tasks for this Course. | integer |  |
| TotalTasks | Total Tasks for this Course. | integer |  |
| PersonId | The id of the course participant | integer |  |
| FullName | Full name of the course participant | string |  |
| LastVisited | Last time the course participant visited this course | date |  |
| LastVisitedRelative | Last time the course participant visited this course relative to the current time Examples: 3 days ago, 2 hours ago, Just now, etc. | string |  |
| PictureUrl | Avatar Url | string |  |
| Role | The role that the course participant has in the course | string |  |
| RoleId | The role id that the course participant has in the course | integer |  |
| ExtraInformation | Extra information for this participant | string |  |
| CanHaveTasks | Show if course participant can have tasks | boolean |  |
| Groups | Comma-separated list of course groups the participant is a member of | string |  |
| TemporaryAccess | Boolean value indicating if the participant has been granted temporary access to the course | boolean |  |

## Personal.Course.YourStudentCourse

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | Title of the Course. | string |  |
| CourseId | Course ID. | integer |  |

## Personal.CourseWithActivePlans

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | Id of the course. | integer |  |
| CourseSyncKey | Course synchronization key. | string |  |
| CourseTitle | Title of the course. | string |  |
| PlanIds | List of active plans' ids. | Collection of string |  |

## Personal.Feature

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The feature name. | string |  |
| Url | Url to the itslearning web page for the feature if it exists. | string |  |
| Features | The list of sub-features. | Collection of Personal.Feature |  |

## Personal.ItslearningUrl

| Name | Description | Type | Additional information |
|---|---|---|---|
| Url | SSO Url to itslearning. | string |  |

## Personal.LtiExtensionCustomerSetup

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeploymentId | Deployment Id. | string |  |
| ClientId | Identifier of itslearning as LTI connected LMS. | string |  |

## Personal.LtiExtensionDetails

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeploymentId | Deployment Id. | string |  |
| LoginUri | Login URL. | string |  |
| LaunchUrls | List of launch URLs. | Collection of string |  |
| TargetLinkUri | Target link URL. | string |  |
| PublicKey | Public key. | string |  |
| PublicJwk | JWK key. | string |  |
| PublicJwksUri | JWKS URL. | string |  |

## Personal.Permission

| Name | Description | Type | Additional information |
|---|---|---|---|
| CanRead | Whether user can read. | boolean |  |
| CanParticipate | Whether user can participate. | boolean |  |
| CanEvaluate | Whether user can evaluate. | boolean |  |
| CanModify | Whether user can modify. | boolean |  |
| TimeToLiveInSeconds | Permission time to live in seconds. | integer |  |

## Personal.PlanElementBasic

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The ID of element. | integer |  |
| Title | The element's title. | string |  |
| Type | The type of the element. | ElementType |  |
| IsMarkedAsHomework | Whether the element is marked as the homework task. | boolean |  |
| IconUri | Url to the element's icon used and hosted by itslearning. | string |  |

## Personal.PlanLearningObjectiveBasic

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The learning objective identifier. Unique in scope of a site (itslearning customer). | integer |  |
| Title | The title of learning objective. | string |  |
| Description | The description of learning objective. | string |  |
| IsFromRepository | Determines whether learning objective is from LO repository. | boolean |  |
| Type | The type of learning objective attached | LearningObjectiveType |  |

## Personal.PlanWithObjectivesAndElements

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | Id of the course the plan is created for. | integer |  |
| CourseName | Name of the course the plan is created for. | string |  |
| CourseSyncKey | Sync key of the course the plan is created for. | string |  |
| PlanId | Identifier of the plan. | integer |  |
| PlanTopicName | Name of the topic the plan belongs to. | string |  |
| PlanTopicColor | Color of the topic the plan belongs to. | string |  |
| PlanTitle | Title of the plan. | string |  |
| PlanDescription | Description of the plan. | string |  |
| PlanStartDateTime | The date and time the plan starts. | date |  |
| PlanEndDateTime | The date and time the plan ends. | date |  |
| CoursePlannerUrl | The URL to pointing to Planner of the course the plans is created for. | string |  |
| LearningObjectives | The list of learning objectives specified for the plan. | Collection of Personal.PlanLearningObjectiveBasic |  |
| Elements | The list of course elements attached to the plan. | Collection of Personal.PlanElementBasic |  |

## Personal.ScoreBasedGrade

| Name | Description | Type | Additional information |
|---|---|---|---|
| MaxScore | Max score. | decimal number |  |
| Score | Given score. | decimal number |  |

## PersonalNotificationsPostV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| Message | Notification message in a default locale. | string |  |
| LocalizedMessages | The list of localized messages where Key - locale name by ISO 3166-1 alpha-2 and ISO 639-1 which separated by the symbol '-' (f.e. "en-GB"), Value - the notification message text in this locale. | Dictionary of string [key] and string [value] |  |
| NotificationType | The any string that represents a specific type of notification. | string |  |
| Object | The identifier of object to which notifcation is related. Notification belongs to one object only. | TypedId |  |
| Locations | List of identifiers of all locations to which notification is related. Usually notification belongs to one location, but it's possible to specify any number of locations. | Collection of TypedId |  |
| Url | The absolute URL that pointing to a notification related information. | string |  |
| ToPersonIds | List of person identifiers to send notification to. | Collection of integer |  |

## PlanSimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| PlanId | The unique id for plan, in scope of a site (itslearning customer). | integer |  |
| TopicId | The id of topic. | integer |  |
| Order | The plan order. | integer |  |
| Name | The plan name. | string |  |
| StartDate | The plan's start date. | date |  |
| StopDate | The plan's end date. | date |  |
| LocationId | The plan location identifier. | integer |  |
| LocationType | The plan location type. | LocationType |  |
| TopicColor | The topic color (e.g. #ffffff). | string |  |
| TopicName | The topic name. | string |  |

## Planner.Plan

| Name | Description | Type | Additional information |
|---|---|---|---|
| ThumbnailColumn | The 'Thumbnail' plan column data. | Planner.PlanThumbnailColumn |  |
| NameColumn | The 'Plan' plan column data. | Planner.PlanColumn |  |
| DescriptionColumn | The 'Description' plan column data. | Planner.PlanColumn |  |
| DateColumn | The 'Date' plan column data. | Planner.PlanDateColumn |  |
| ClassHoursColumn | The 'Class hours' plan column data. | Planner.PlanClassHoursColumn |  |
| LearningObjectivesColumn | The 'Learning objectives' plan column data. | Planner.PlanLearningObjectivesColumn |  |
| ResourcesColumn | The 'Resources' plan column data. | Planner.PlanElementsColumn |  |
| ActivitiesColumn | The 'Activities' plan column data. | Planner.PlanElementsColumn |  |
| ElementsColumn | The 'Resources and activities' plan column data. | Planner.PlanElementsColumn |  |
| Columns | The list of custom columns for plan. | Collection of Planner.PlanColumn |  |
| PlanId | The unique id for plan, in scope of a site (itslearning customer). | integer |  |
| TopicId | The id of topic. | integer |  |
| Order | The plan order. | integer |  |
| LocationId | The plan location identifier. | integer |  |
| LocationType | The plan location type. | LocationType |  |
| LocationTitle | The plan location title. | string |  |
| LocationColor | The location color (e.g. #ffffff). | string |  |
| TopicColor | The topic color (e.g. #ffffff). | string |  |
| TopicName | The topic name. | string |  |

## Planner.PlanClassHoursColumn

| Name | Description | Type | Additional information |
|---|---|---|---|
| ClassHours | The plan class hours. | integer |  |
| Name | The column custom name. | string |  |
| Order | The column order. | integer |  |
| LocalizedName | The localization term. | LocalizedText |  |

## Planner.PlanColumn

| Name | Description | Type | Additional information |
|---|---|---|---|
| Value | The column value. | string |  |
| Name | The column custom name. | string |  |
| Order | The column order. | integer |  |
| LocalizedName | The localization term. | LocalizedText |  |

## Planner.PlanDateColumn

| Name | Description | Type | Additional information |
|---|---|---|---|
| StartDate | The plan start date. Null if not set. | date |  |
| StopDate | The plan stop date. Null if not set. | date |  |
| Name | The column custom name. | string |  |
| Order | The column order. | integer |  |
| LocalizedName | The localization term. | LocalizedText |  |

## Planner.PlanElement

| Name | Description | Type | Additional information |
|---|---|---|---|
| Order | The element order number. | integer |  |
| ElementId | The ID of element. | integer |  |
| ElementType | Related element's type. | ElementType |  |
| Title | The element's title. | string |  |
| Url | Url to the element's page in itslearning. | string |  |
| ContentUrl | Url to the location in itslearning opened without master page. | string |  |
| IconUrl | Url to the element's icon used by itslearning. | string |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. It is applicable only if ElementType is LearningToolElement. | integer |  |
| Homework | Marks the element as homework. | boolean |  |

## Planner.PlanElementsColumn

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The column custom name. | string |  |
| Order | The column order. | integer |  |
| LocalizedName | The localization term. | LocalizedText |  |
| Elements | The list of elements connected to the plan. | Collection of Planner.PlanElement |  |

## Planner.PlanLearningObjective

| Name | Description | Type | Additional information |
|---|---|---|---|
| RepositoryId | Identifier of the learning objective inside the repository. Unique globally for all sites. Null means that learning objective is not from the repository. | integer |  |
| Id | The learning objective identifier. Unique in scope of a site (itslearning customer). | integer |  |
| Title | The learning objective title. | string |  |
| Description | The learning objective description. | string |  |

## Planner.PlanLearningObjectivesColumn

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The column custom name. | string |  |
| Order | The column order. | integer |  |
| LocalizedName | The localization term. | LocalizedText |  |
| LearningObjectives | The list of learning objectives for plan. | Collection of Planner.PlanLearningObjective |  |

## Planner.PlanPreview

| Name | Description | Type | Additional information |
|---|---|---|---|
| PlanId | The unique id for plan, in scope of a site (itslearning customer). | integer |  |
| TopicId | The id of topic. | integer |  |
| Order | The plan order. | integer |  |
| LocationId | The plan location identifier. | integer |  |
| LocationType | The plan location type. | LocationType |  |
| LocationTitle | The plan location title. | string |  |
| LocationColor | The location color (e.g. #ffffff). | string |  |
| TopicColor | The topic color (e.g. #ffffff). | string |  |
| PlanName | The plan name. | string |  |
| PlanDescription | The plan description. | string |  |
| Start | The plan start date. Null if not set. | date |  |
| Stop | The plan stop date. Null if not set. | date |  |
| ClassHours | The plan class hours. | integer |  |

## Planner.PlanPreviewV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| PlanId | The unique id for plan, in scope of a site (itslearning customer). | integer |  |
| TopicId | The id of topic. | integer |  |
| Order | The plan order. | integer |  |
| LocationId | The plan location identifier. | integer |  |
| LocationType | The plan location type. | LocationType |  |
| LocationTitle | The plan location title. | string |  |
| LocationColor | The location color (e.g. #ffffff). | string |  |
| TopicColor | The topic color (e.g. #ffffff). | string |  |
| PlanName | The plan name. | string |  |
| PlanDescription | The plan description. | string |  |
| Start | The plan start date. Null if not set. | date |  |
| Stop | The plan stop date. Null if not set. | date |  |
| TopicName | The topic name. | string |  |

## Planner.PlanThumbnailColumn

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileUrl | The Url to the preview file for the thumbnail image. | string |  |
| Name | The column custom name. | string |  |
| Order | The column order. | integer |  |
| LocalizedName | The localization term. | LocalizedText |  |

## PushSetting

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeviceId | Current (or new) unique DeviceId for the application on the phone or the tablet. This ID must be recognized by Google Cloud Messaging or Apple Push Notification Service. When PushProvider=Apple: the same as DeviceToken When PushProvider=Google: the same as RegistrationId | string |  |
| OldDeviceId | Old DeviceId for the application on the phone or the tablet. When the application receives a new DeviceId, the application should also provide the old one here, so that itslearning can replace the old one. This is to make sure that we use the correct deviceId as soon as possible, and to prevent itslearning from sending multiple push notification messages with the same content. | string |  |
| PushProvider | Type of push provider (e.g. "Google" or "Apple") | string |  |
| IsEnabled | If push notifications should be enabled or not | boolean |  |

## ReferencedInstantMessageType

| Name | Value | Description |
|---|---|---|
| None | 1 | The instant message does not have a referenced id |
| Shared | 2 | If the instant message reference id is a shared message |
| Replied | 3 | If the instant message reference id is a replied message |
| AbuseReported | 4 | If the instant message reference id is a abuse reported message |
| AbuseReportedThread | 5 | If the instant message has reference on abuse thread |

## RelatedPerson

| Name | Description | Type | Additional information |
|---|---|---|---|
| Person | Related person. | PersonSimple |  |
| RelationType | Relation type between persons. | RelationType |  |
| RelationInfo | Extra information about the relation. | RelationInfo |  |

## Relation

| Name | Description | Type | Additional information |
|---|---|---|---|
| Text | A text describing the relation between two persons | string |  |
| Items | Names for items in the relation. The names can be tilte for courses and projects, Name of persons etc. | Collection of string |  |

## RelationInfo

| Name | Description | Type | Additional information |
|---|---|---|---|
| Courses | List of courses where the requested person is a learner and the related person is a teacher. | Collection of PersonCourseBase |  |

## RelationType

| Name | Value | Description |
|---|---|---|
| Teacher | 1 | Person is a teacher of the given person. |
| Parent | 2 | Person is a parent of the given person. |
| Supervisor | 3 | Person is a supervisor of the given person. |

## Site

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

## SiteBase

| Name | Description | Type | Additional information |
|---|---|---|---|
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

## SiteLink

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | The title of the site link. | string |  |
| Url | The Url of the site link. | string |  |
| DisplayInSeparateWindow | True if the site link should be opened in a separate window. Otherwise false. | boolean |  |

## SiteLoginDetails

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

## StreamItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| NotificationId | The unique id for for the stream item (the same as news feed item and notification item), in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the item. | string |  |
| LocationTitle | The item's project or course title. | string |  |
| PublishedDate | Date time when the news feed item was published. Time zone is UTC. | date |  |
| PublishedBy | The person who published the item. | PersonSimple |  |
| Url | Url to the location in itslearning connected to the notification. | string |  |
| ContentUrl | Url to the location in itslearning connected to the notification that open without master page. | string |  |
| IconUrl | Url to the icon used by itslearning for the item. | string |  |
| LightBulletin | Related light bulletin (in case if current stream item is lightbulletin-related). | LightBulletins.LightBulletin |  |
| ElementId | Related element's ID. It will have the value of NULL in case if current stream item is not element-related. | integer |  |
| ElementType | Related element's type. It will have the value of Unknown in case if current stream item is not element-related. | ElementType |  |

## StreamItemV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| NotificationId | The unique id for the stream item (the same as news feed item and notification item), in scope of a site (itslearning customer). | integer |  |
| Text | Descriptive text for the item. | string |  |
| LocationTitle | The item's project or course title. | string |  |
| PublishedDate | Date time when the news feed item was published. Time zone is UTC. | date |  |
| PublishedBy | The person who published the item. | PersonSimple |  |
| Url | Url to the location in itslearning connected to the notification. | string |  |
| ContentUrl | Url to the location in itslearning connected to the notification that open without master page. | string |  |
| IconUrl | Url to the icon used by itslearning for the item. | string |  |
| LightBulletin | Related light bulletin (in case if current stream item is lightbulletin-related). | LightBulletins.LightBulletinV2 |  |
| ElementId | Related element's ID. If there are more than one element related to the notification, then the list should be fetched by separate request. Use ElementsCount value to get exact count of such elements. | integer |  |
| ElementType | Related element's type. It will have the value of Unknown in case if current stream item is not element-related. If there are more than one element related to the notification, then the list should be fetched by separate request. Use ElementsCount value to get exact count of such elements. | ElementType |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. If there are more than one element related to the notification, then the list should be fetched by separate request. Use ElementsCount value to get exact count of such elements. | integer |  |
| HasMoreElements | DEPRECATED Use ElementsCount instead. Defines whether the notification is about operations made with more than one element. | boolean |  |
| ElementsCount | The count of related elements. | integer |  |
| LocationType | The item's location type. | LocationType |  |
| LocationId | The item's location identifier. | integer |  |

## StudentPlan.AchievementGoals.AchievementGoal

| Name | Description | Type | Additional information |
|---|---|---|---|
| GoalId | The goal identifier. | integer |  |
| Status | The status. | StudentPlan.AchievementGoals.AchievementGoalStatusType |  |
| Description | The description. | string |  |
| TeacherAssessment | The teacher assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| StudentAssessment | The student assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| AssessmentItems | The assessment items. | Collection of StudentPlan.AchievementGoals.AssessmentItem |  |

## StudentPlan.AchievementGoals.AchievementGoalAssessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentPerson | The assessment person. | StudentPlan.AchievementGoals.AssessmentPerson |  |
| AssessmentTitle | The assessment title. | string |  |
| AssessmentLevel | The assessment level. | integer |  |

## StudentPlan.AchievementGoals.AchievementGoalStatusType

| Name | Value | Description |
|---|---|---|
| Active | 0 | Goal is active. |
| Archived | 1 | Goal is archived. |

## StudentPlan.AchievementGoals.AssessmentItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| Value | The value - assessment identifier. | integer |  |
| Name | The name - assessment title. | string |  |

## StudentPlan.AchievementGoals.AssessmentPerson

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonName | The name of the person. | string |  |
| ProfileImageUrl | The profile image URL. | string |  |

## StudentPlan.AchievementGoals.CourseWideAchievementGoal

| Name | Description | Type | Additional information |
|---|---|---|---|
| GoalId | The goal identifier. | integer |  |
| Description | The description. | string |  |
| Course | The course. | StudentPlan.StudentPlanCourseSimple |  |
| TeacherAssessment | The teacher assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |
| StudentAssessment | The student assessment. | StudentPlan.AchievementGoals.AchievementGoalAssessment |  |

## StudentPlan.Assessments.Activity

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Element id. | integer |  |
| Title | Element title. | string |  |
| Url | The absolute URL to this activity. | string |  |
| IconUrl | Element icon. | string |  |
| Assessments | Assessments for the element. | Collection of StudentPlan.Assessments.Assessment |  |
| AssessmentTitle | Global assessment for the element. | string |  |
| Feedback | Global feedback for the element. | StudentPlan.Assessments.GradeComment |  |

## StudentPlan.Assessments.Assessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningObjectiveId | Learning objective id. | integer |  |
| Url | The URL to Learning objective report. | string |  |
| Comment | Given comments for the assessment. | string |  |
| LearningObjectiveTitle | Learning objective title. | string |  |
| AssessmentTitle | Assessment title. | string |  |

## StudentPlan.Assessments.GradeComment

| Name | Description | Type | Additional information |
|---|---|---|---|
| RichContent | Rich content of the comment. | string |  |
| PlainContent | Plain content of the comment. | string |  |
| HasRichContent | Whether comment has Rich content. | boolean |  |

## StudentPlan.Assessments.StudentPlanPlan

| Name | Description | Type | Additional information |
|---|---|---|---|
| StartDate | Plan start date. | date |  |
| StopDate | Plan end date. | date |  |
| Activities | Plan activities. | Collection of StudentPlan.Assessments.Activity |  |

## StudentPlan.Assessments.StudentPlanTopic

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Topic id. | integer |  |
| Plans | List of plans. | Collection of StudentPlan.Assessments.StudentPlanPlan |  |
| Name | Topic name. | string |  |
| ColorCode | The topic color schema. | string |  |
| StartDateTime | Topic start date and time. | date |  |
| EndDateTime | Topic end date and time. | date |  |

## StudentPlan.AssessmentsErrorType

| Name | Value | Description |
|---|---|---|
| None | 0 | No errors. |
| TopicsDisabled | 1 | Topics are disabled for the planner. |
| PlannerDisabled | 2 | Planner is disabled for the course. |

## StudentPlan.Comments.StudentPlanComment

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Comment identifier. | integer |  |
| SourceId | The identifier of source entity that comment belongs to. | integer |  |
| SourceType | The type of source identifier that comment belongs to. | string |  |
| PictureUrl | Url of the person picture. | string |  |
| UserName | Name of user who created the comment. | string |  |
| CommentText | Text message. | string |  |
| PublishedDateTime | Date and time when comment was published. | date |  |
| LastModifiedDateTime | Date and time when comment was last modified. | date |  |

## StudentPlan.StudentPlanCourse

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The course identifier. | integer |  |
| Title | Title of the course. | string |  |
| Color | The course selected color. | string |  |
| NumberOfAssignedAchievementGoals | The number of assigned achievement goals. | integer |  |
| NumberOfUnreadComments | The number of unread comments. | integer |  |
| IsEmpty | Flag indicating that student plan is empty (i.e. no comments, assessments or goals) for the course. | boolean |  |

## StudentPlan.StudentPlanCourseSimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The course identifier. | integer |  |
| Title | Title of the course. | string |  |
| Color | The course selected color. | string |  |

## StudentPlan.StudentPlanOverview

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentsError | Error state for the assessments. | StudentPlan.AssessmentsErrorType |  |
| CourseColor | The course color. | string |  |
| CourseTitle | The course title. | string |  |
| UnreadCommentsCount | Number of unread comments for the current person. | integer |  |

## Task

| Name | Description | Type | Additional information |
|---|---|---|---|
| Description | Task description. | string |  |
| LocationTitle | The name of the course or project that the task belong. Empty if the task is part of a individual learning plan. | string |  |
| LocationFriendlyName | Friendly name of event location. Can be used instead of LocationTitle which is usually more official. | string |  |
| TaskId | The unique id for for the task, in scope of a site (itslearning customer). | integer, 64 bit |  |
| Title | Task title. | string |  |
| Status | Task status. | TaskStatus |  |
| Deadline | Task deadline. Time zone is UTC. Can be null if there is no deadline. | date |  |
| Url | The itslearning internal URL for the task. | string |  |
| ContentUrl | The itslearning content URL for the task. | string |  |
| IconUrl | Url to related element icon. | string |  |
| ElementId | The identifier of related element. | integer |  |
| ElementType | Type of related element. | ElementType |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. It is applicable only if ElementType is LearningToolElement. | integer |  |
| Homework | Marks the task as homework. | boolean |  |
| LocationType | The task location type. | LocationType |  |
| LocationId | The task location identifier. | integer |  |

## TaskDailyWorkflow

| Name | Description | Type | Additional information |
|---|---|---|---|
| CompletedTaskAssessmentText | The Assessment for this task | string |  |
| CourseId | The unique id for for the course the task belongs to | integer, 64 bit |  |
| CourseTitle | Course title. | string |  |
| CourseColorClass | Course colour. | string |  |
| Status | The status of the tasks | string |  |
| TaskId | The unique id for the task. | integer, 64 bit |  |
| TaskTitle | Task title. | string |  |
| TaskUrl | The content URL for the task. | string |  |
| TaskUrlTarget | Target for the task URL. | string |  |
| IconUrl | Url to related element icon. | string |  |
| BrandIconUrl | Url to a branded element icon. Used in tasklist's student/parent view | string |  |
| Deadline | Task deadline. | date |  |
| DeadlineDisplayTime | Task deadline displayed in an easy readable way | string |  |
| IsHomework | Indicates this task is marked as homework | boolean |  |
| IsPeerAssessment | Indicates this task is a peer assessment | boolean |  |
| IsSelfAssessment | Indicates this task is a self assessment | boolean |  |

## TaskDailyWorkflowSection

| Name | Description | Type | Additional information |
|---|---|---|---|
| SectionTitle | Title of the section | string |  |
| TaskDailyWorkflow | Tasks that fall into this section | EntityList<TaskDailyWorkflow> |  |
| BackToText | The text to come back to this section | string |  |
| StartOpen | Initial state of this list in the UI | boolean |  |

## TaskSimple

| Name | Description | Type | Additional information |
|---|---|---|---|
| TaskId | The unique id for for the task, in scope of a site (itslearning customer). | integer, 64 bit |  |
| Title | Task title. | string |  |
| Status | Task status. | TaskStatus |  |
| Deadline | Task deadline. Time zone is UTC. Can be null if there is no deadline. | date |  |
| Url | The itslearning internal URL for the task. | string |  |
| ContentUrl | The itslearning content URL for the task. | string |  |
| IconUrl | Url to related element icon. | string |  |
| ElementId | The identifier of related element. | integer |  |
| ElementType | Type of related element. | ElementType |  |
| LearningToolId | Identifier of learning tool (Assignment extension, test extension etc.). It's 0 for native itslearning elements. It is applicable only if ElementType is LearningToolElement. | integer |  |
| Homework | Marks the task as homework. | boolean |  |
| LocationType | The task location type. | LocationType |  |
| LocationId | The task location identifier. | integer |  |

## TaskStatus

| Name | Value | Description |
|---|---|---|
| Unknown | 0 |  |
| NotStarted | 1 |  |
| InProgress | 2 |  |
| Completed | 3 |  |

## TaskStatusFilter

| Name | Value | Description |
|---|---|---|
| Active | 0 | Return active tasks. |
| Completed | 1 | Return completed tasks. |
| Hidden | 2 | Return hidden tasks. |
| All | 10 | Return all tasks. |
| ActiveViewAsInCourse | 20 | Return active tasks for view as in course. |
| CompletedViewAsInCourse | 21 | Return completed tasks for view as in course. |

## ThreadParticipant

| Name | Description | Type | Additional information |
|---|---|---|---|
| SourceLocationId | The id of how the recipient have relation to the Instant Message Thread, if the recipient was added to the thread by a course or project | integer |  |
| SourceLocationType | The type of the id of how the recipient have relation to the Instant Message Thread, if the recipient was added to the thread by a course or project | LocationRecipientType |  |
| IsDeleted | Flag indicates the recipient has been deleted yet. | boolean |  |
| CanReceive | Flag indicates if the instant message is on or off for a participant. | boolean |  |
| RoleInHierarchyTitle | Role in hierarchy for a participant as text. | string |  |
| RoleInHierarchy | Role in hierarchy for a participant. | Itsolutions.ItslUtils.Constants.HierarchyRole |  |
| ExtraUserInformation | Contains extra user information which can be different depending on customer settings. It can be either username, synchronization key, any of 5 custom strings or none. | string |  |
| PersonId | The unique id for for the person, in scope of a site (itslearning customer). | integer |  |
| FirstName | The person's first name. It can only be used when first name is displayed without last name. Otherwise must be used. | string |  |
| LastName | The person's last name. It can only be used when last name is displayed without first name. Otherwise must be used. | string |  |
| FullName | The person's full name formatted according to regional settings of a site. | string |  |
| ProfileUrl | The url to view the person's profile in itslearning. | string |  |
| AdditionalInfo | Additional information about the person. This can be context sensitive. For example when using the rest api method for message recipients search, this field can contain extra information about the person, that helps the user distinguish between persons with identical names. | string |  |
| ProfileImageUrl | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |
| ProfileImageUrlSmall | The absolute URL to the person's profile image. This can be null or empty if the person does not have a profile image. | string |  |

## TypedId

| Name | Description | Type | Additional information |
|---|---|---|---|
| Type | The type of object. | string |  |
| Id | The identifier of object. | string |  |

## Updatable.FileDataV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId | The file identifier. | string |  |
| Signature | Signature. | string |  |
| FileName | File name. | string |  |

## Updatable.InstantMessageThreadUpdateV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId | The Id of the message thread to update | integer |  |
| UpdateName | Determines if the name should be updated | boolean |  |
| UpdateParticipants | Determines if the list of participants should be updated | boolean |  |
| Name | The new name of the thread | string |  |
| PersonIds | PersonIds to send instant message to. A new thread will be created if the persons don't have an existing thread already. The ToPersonIds or the InstantMessageThreadId need to have a valid value. | Collection of integer |  |
| ToggleEnableDisableReplies | Determines if the option to send to close the conversation should be updated | boolean |  |
| RepliesAreEnabled | Whether students can reply to the conversation or not | boolean |  |

## Updatable.LightBulletin.LightBulletinAddInputDataV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The course identifier. | integer |  |
| Text | The descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| ElementIdsToAttach | The collection of elements to attach. | Collection of integer |  |
| AttachedImages | The collection of attached images. | Collection of Updatable.LightBulletin.LightBulletinInputImageDataV1 |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. | boolean |  |
| ActiveFromDate | The time when to show the bulletin. | date |  |
| ActiveToDate | The time when to remove the bulletin. | date |  |

## Updatable.LightBulletin.LightBulletinInputImageDataV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| PreviewFile | The file information, that contains preview for attached image. | Updatable.FileDataV1 |  |
| OriginalFile | The file information, that contains attached image. | Updatable.FileDataV1 |  |

## Updatable.LightBulletin.LightBulletinUpdateInputDataV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The unique id for for the light bulletin, in scope of a site (itslearning customer). | integer |  |
| ElementIdsToDetach | The collection of elements to detach. | Collection of integer |  |
| DeleteAttachedImage | The deletion flag of attached image. | boolean |  |
| Text | The descriptive text for the light bulletin. It's intended to be a plain text, so should be encoded. | string |  |
| ElementIdsToAttach | The collection of elements to attach. | Collection of integer |  |
| AttachedImages | The collection of attached images. | Collection of Updatable.LightBulletin.LightBulletinInputImageDataV1 |  |
| AllowComments | Defines whether comments creation is allowed for current light bulletin. | boolean |  |
| ActiveFromDate | The time when to show the bulletin. | date |  |
| ActiveToDate | The time when to remove the bulletin. | date |  |

## Updatable.LightBulletinCommentUpdateV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| Comment | Light bulletin comment | string |  |

## Updatable.LightBulletinSubscriptionUpdateV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsSubscribed | Determines if notifications for the current light bulletin comments should be sent to the current person. | boolean |  |

## Updatable.LightBulletinUpdateV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| LightBulletinId | The Id of the light bulletin to update | integer |  |
| IsSeen | Determines if the current light bulletin is seen by current person. | boolean |  |

## Updatable.NotificationUpdateV1

| Name | Description | Type | Additional information |
|---|---|---|---|
| NotificationId | The notification id | integer |  |
| IsRead | Whether the notification has been read or not | boolean |  |

## Workload

| Name | Description | Type | Additional information |
|---|---|---|---|
| FinishedAssignments | The number of assignments finished by this person. | integer |  |
| TotalAvailableAssignments | The number of assignments available to this person. | integer |  |
| FinishedWorkload | The number of workload minutes finished by this person. | integer |  |
| TotalAvailableWorkload | The number of workload minutes available to this person. | integer |  |

## WorkloadType

| Name | Value | Description |
|---|---|---|
| NotSet | 0 | Workload type is not defined. |
| Low | 1 | Workload type is low. |
| Medium | 2 | Workload type is medium. |
| High | 3 | Workload type is high. |

## YourFiles.CopyRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be copied to | integer |  |
| Ids | The list of identifiers of the files or folders to be copied | Collection of integer |  |
| DuplicateBehaviour | How to handle duplicates in destination folder | YourFiles.DuplicateBehaviour |  |

## YourFiles.CourseModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| CourseId | The course identifier | integer |  |
| Title | The course title | string |  |
| LastUpdated | When the course folder in your files was last modified | date |  |
| LastUpdatedString | The date in string format that will be displayed in UI | string |  |
| Teachers | List of teachers in the course. Name following the name format standard | Collection of string |  |
| IsActive | The status of the course is active | boolean |  |
| IsDeleted | The status of the course is deleted | boolean |  |
| NumberOfElements | Number of elements in that course for the person | integer |  |
| CourseStatus | The status of the course. CourseStatusType.ToString() will be used to get the string value | string |  |
| ColorSchemaClass | Color schema for course | string |  |

## YourFiles.DuplicateBehaviour

| Name | Value | Description |
|---|---|---|
| Skip | 1 | Skip the copied file |
| KeepBoth | 2 | Rename the copied file or folder |
| Overwrite | 3 | Overwrite the existing file or folder |

## YourFiles.FileModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name | The name of the file/folder | string |  |
| LastModified | The date and time the file was last modified | date |  |
| LastModifiedString | The date and time the file was last modified as string | string |  |
| FileType | The type of file | string |  |
| FileSize | The file size in Kb | integer, 64 bit |  |
| FileId | The file identifier | integer |  |
| FileRepoId | The file repository identifier | globally unique identifier |  |
| SignedFileRepoId | Signed file repository identifier | string |  |
| IsFolder | If the item is folder | boolean |  |
| VirtualPath | Virtual Path | string |  |
| Icon | Icon for the item | string |  |
| FolderId | The folder identifier | integer |  |
| ParentFolderId | The parent folder identifier. If this is a root folder or IsFolder = false, then this value is null. | integer |  |
| NameWithoutExtension | The name without file extension | string |  |
| Extension | The file extension | string |  |
| Children | The children of the FileModel if the file model is a folder | Collection of YourFiles.FileModel |  |
| IsEditable | If file can be edited | boolean |  |
| IsViewable | Gets or sets a value indicating whether the item is viewable in the browser. | boolean |  |
| FolderType | Gets or sets a value indicating whether the item is viewable in the browser. | string |  |
| PublicLink | Embed link for files. | string |  |

## YourFiles.MessageAttachmentModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageId | The instant message identifier where the attachment is sent | integer, 64 bit |  |
| InstantMessageThreadId | The identifier for the instant message thread the attachment belongs to. | integer |  |
| FileName | The name of the file attached to the message. | string |  |
| FileType | The type of file | string |  |
| UploadedDateString | The date when the attachment was uploaded. In string format displayed in the UI | string |  |
| UploadedDate | The date when the attachment was uploaded. | date |  |
| Icon | Icon for the file. | string |  |

## YourFiles.MoveRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The new parent folder identifier where the files or folders will be moved to | integer |  |
| Ids | The list of identifiers of the files or folders to be moved | Collection of integer |  |

## YourFiles.SimpleFileModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId |  | integer |  |
| Name |  | string |  |

## YourFiles.UpdateRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The parent folder identifier where the file or folder is located | integer |  |
| Name | The new name of the file or folder to be updated | string |  |

## YourFiles.UploadedFileModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The file identifier | string |  |
| Name | The name of the file | string |  |
| MimeType | The MIME type of the file | string |  |
| SignedFileRepoId | The signed file repository identifier | string |  |
| Signature | The signature for the file | string |  |
| Error | The error message if any error occurred during the upload to file repo | string |  |

## YourFiles.ZipFileJobModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| JobId | The job identifier | integer |  |
| FileRepoId | The file repository identifier for the zip file | globally unique identifier |  |
| ZipFileName | The name of the zip file | string |  |
| Created | When the job was created | date |  |
| Status | Status for the job | string |  |
| DownloadUrl | The url to download the file if status is completed | string |  |

## YourFiles.ZipRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileIds | The file identifiers of the files to be included in the zip archive. | Collection of integer |  |
| FolderIds | The folder identifiers of the folders to be included in the zip archive. | Collection of integer |  |

## Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthAccessToken

| Name | Description | Type | Additional information |
|---|---|---|---|
| value |  | string |  |
| expirationDateUtc |  | date |  |

## Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthContext

| Name | Description | Type | Additional information |
|---|---|---|---|
| oAuthToken |  | Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthToken |  |
| oAuthUser |  | Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthUser |  |

## Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthToken

| Name | Description | Type | Additional information |
|---|---|---|---|
| accessToken |  | Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthAccessToken |  |
| refreshToken |  | string |  |
| refreshTokenExpired |  | boolean |  |

## Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthUser

| Name | Description | Type | Additional information |
|---|---|---|---|
| email |  | string |  |
| clientId |  | string |  |
| mustBeImpersonated |  | boolean |  |

## Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | string |  |
| Name |  | string |  |
| SourceType |  | Itslearning.FileRepo.Common.Models.SourceType |  |
| MimeType |  | string |  |
| viewUrl |  | string |  |
| downloadUrl |  | string |  |
| editUrl |  | string |  |
| oAuthContext |  | Itslearning.Web.UI.Controls.Uploaders.Authorisation.OAuthContext |  |
| isTemplate |  | boolean |  |
| isRegisteredWithCasper |  | boolean |  |
| isSupportedByCasper |  | boolean |  |
| PlagiarismReportUrl |  | string |  |

## Itslearning.Web.UI.Controls.Uploaders.Metadata.UploadMetadata

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | globally unique identifier |  |
| Name |  | string |  |
| SourceType |  | Itslearning.FileRepo.Common.Models.SourceType |  |
| MimeType |  | string |  |
| OriginalFileId |  | string |  |
| signedFileRepoId |  | string |  |
| signature |  | string |  |
| isTemplate |  | boolean |  |
| customId |  | string |  |

## Itsolutions.ItslUtils.Constants.CourseType

| Name | Value | Description |
|---|---|---|
| Subject | 1 |  |
| Class | 2 |  |
| SelfPaced | 3 |  |
| InformationHub | 4 |  |

## Itsolutions.ItslUtils.Constants.CustomerEducationSegment

| Name | Value | Description |
|---|---|---|
| Higher | 0 |  |
| Secondary | 1 |  |
| Primary | 2 |  |
| Other | 3 |  |

## Itsolutions.ItslUtils.Constants.ElementType

| Name | Value | Description |
|---|---|---|
| None | -1 |  |
| Post | 0 |  |
| Discussion | 1 |  |
| Folder | 4 |  |
| Note | 8 |  |
| WebLink | 32 |  |
| Essay | 512 |  |
| CMProject | 2048 |  |
| All | 1073741823 |  |
| ErrorType | 32768 |  |
| NttTest | 65536 |  |
| LearningToolElement | 131072 |  |
| Planner | 262144 |  |
| CustomActivity | 524288 |  |
| LearningPath | 1048576 |  |
| ManualAssessment | 2097152 |  |
| AchievementGoal | 4194304 |  |
| ExternallyPublishable | 556 |  |
| MaxAllowedInCourse | 1073741823 |  |
| MaxAllowedInProject | 1072102911 |  |
| MaxAllowedInModule | 2052 |  |
| AllForMultipleNotification | 1073741819 |  |
| TaskListElements | 721408 |  |
| Resources | 2092 |  |
| Activities | 1638913 |  |
| CoursePersonReportItems | 721444 |  |
| GradeBookItems | 2818560 |  |
| DiscontinueItems | 2048 |  |
| CopyNotSupportedItems | 2048 |  |
| CopyWithAnswersSupportedItems | 590337 |  |
| LearningObjectivesNotSupportedItems | 1050628 |  |
| LearningObjectiveAssessmentSupported | 721408 |  |
| LearningObjectiveCriteriaBasedAssessmentSupported | 524800 |  |
| AllowedForParents | 721416 |  |
| CanContainSubElements | 1048580 |  |
| AllowedInLearningPath | 1072690683 |  |
| Hidden | 4194304 |  |
| AllowedInCrossSiteTemplate | 1703949 |  |
| AllowedInAiToolkit | 12 |  |

## Itsolutions.ItslUtils.Constants.HierarchyRole

| Name | Value | Description |
|---|---|---|
| None | 0 |  |
| Guest | 1 |  |
| Student | 2 |  |
| Teacher | 3 |  |
| Administrator | 4 |  |

## Itsolutions.ItslUtils.Constants.LocationType

| Name | Value | Description |
|---|---|---|
| Course | 1 |  |
| Project | 2 |  |
| Hierarchy | 3 |  |
| Personal | 5 |  |
| Site | 6 |  |
| PersonCache | 7 |  |
| Library | 8 |  |
| Calendar | 9 |  |

## Itsolutions.ItslUtils.Constants.NameFormat

| Name | Value | Description |
|---|---|---|
| LastFirst | 0 |  |
| FirstLast | 1 |  |
| PrefixLast | 2 |  |
| PrefixFirstLast | 3 |  |

## Itsolutions.ItslUtils.Constants.PlannerConstants_2BPlanType

| Name | Value | Description |
|---|---|---|
| DeadLineDriven | 0 |  |
| OnePlanAtATime | 1 |  |
| AllPlansAtOnce | 2 |  |

## Itsolutions.ItslUtils.Constants.SessionStatus

| Name | Value | Description |
|---|---|---|
| NotCompleted | 0 |  |
| NotAssessed | 1 |  |
| Submitted | 2 |  |

## Itsolutions.Itslearning.ItslCore.BusinessLayer.InstantMessage.Broadcast.BroadcastMessageStatisticsEntity

| Name | Description | Type | Additional information |
|---|---|---|---|
| InstantMessageThreadId |  | integer |  |
| ReadMessageRecipientsCount |  | integer |  |
| TotalRecipientsCount |  | integer |  |
| ReadMessagePercentage |  | decimal number |  |

## Itsolutions.Itslearning.ItslCore.BusinessLayer.Person.CustomInfo.ExtraUserInformationField

| Name | Description | Type | Additional information |
|---|---|---|---|
| Name |  | string |  |
| Value |  | string |  |

## Itsolutions.Itslearning.Web.Areas.Administration.CustomLabels.Models.AddEditCustomLabelRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Identifier, if is 0 that means it's add action | integer |  |
| Name | Custom label name | string |  |
| Description | Custom label description | string |  |
| Color | Custom label color name, chosen from the UI | string |  |

## Itsolutions.Itslearning.Web.Areas.Administration.CustomLabels.Models.BaseCustomLabelModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id |  | integer |  |
| Name |  | string |  |
| Description |  | string |  |
| Color |  | string |  |
| LabelsSummary |  | string |  |

## Itsolutions.Itslearning.Web.Areas.Common.Components.Models.LearningObjectiveModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | The learning objective identifier. Unique in scope of a site (itslearning customer). | integer |  |
| Title | The title of learning objective. | string |  |
| Description | The description of learning objective. | string |  |
| IsFromRepository | Determines whether learning objective is from LO repository. | boolean |  |
| Type | The type of learning objective attached | LearningObjectiveType |  |

## Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveProgressReportModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| LearningObjectiveProgressReportUrlPlaceHolder | Learning objective progress report URL placeholder or empty string if there's no permissions. | string |  |
| LearningObjectiveIdParameterName | Replacement parameter for learning objective ID. | string |  |

## Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveSettingsModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsLearningObjectiveEnabledForLocation | Defines whether learning objectives are enabled on the site, in the location (e.g. course). | boolean |  |
| CourseHasLearningObjectives | Defines whether course has at least one learning objective. | boolean |  |
| LearningObjectives | The collection of learning objectives linked to the element. | Collection of Itsolutions.Itslearning.Web.Areas.Common.Components.Models.LearningObjectiveModel |  |
| CourseId | The ID of course where learning objective editor is being rendered. | integer |  |
| CourseTitle | The title of course where learning objective editor is being rendered. | string |  |
| ViewLearningObjectivesPageUrl | The URL to View learning objectives in course page. | string |  |
| ProgressReportSettings | Contains settings for rendering learning objective progress report in the learning objective progress report link. | Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveProgressReportModel |  |

## Itsolutions.Itslearning.Web.Areas.Elements.Models.SelectItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| Value |  | integer |  |
| Name |  | string |  |

## Internal.Models.Planner.ShareablePlanner.AddSharedResourcesToCoursePlanRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Elements | Elements to be added. | Collection of Itslearning.Platform.RestApi.Sdk.ItslEntities.Planner.PlanElement |  |
| AddOnlyExistingInCourseLearningObjectives | Indicates if only existing in the target course learning objectives can be used. | boolean |  |

## Models.KpiOperation

| Name | Value | Description |
|---|---|---|
| None | -1 |  |
| Begin | 1 |  |
| Forward | 2 |  |
| Backward | 3 |  |
| End | 4 |  |
| DragAndDrop | 5 |  |

## Models.PersonCourseRankWithKpi

| Name | Description | Type | Additional information |
|---|---|---|---|
| courseRanks |  | Collection of PersonCourseRank |  |
| kpiOperation |  | Models.KpiOperation |  |

## Personal.Assignment.Enums.AssignmentSubmissionType

| Name | Value | Description |
|---|---|---|
| Individual | 0 | Each student submits their own answer |
| CourseGroups | 1 | Each course group submits one answer |
| StudentDefinedGroups | 2 | Students can define their own groups and submit one answer per group |

## Personal.Assignment.Requests.CreateAssignmentRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId | The folder to create the assignment in (must be a folder in a course). | integer |  |
| Title | The title of the assignment. | string |  |
| Visibility | Defines visibility settings. | Personal.Element.Models.VisibilitySettings |  |
| AssignTo | Defines who to assign the element (e.g. assignment) to (specific users and/or groups). If null, or if no specific Ids are provided, the assignment is assigned to all users in the parent folder. | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds | List of learning objective IDs to associate with this assignment. | Collection of integer |  |
| DeadlineSettings | Deadline settings. | Personal.Element.Models.DeadlineSettings |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the assignment, will be null if no status is used. | integer |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the assignment, will be null if assessment scale is not used. | integer |  |
| MaxScore |  | decimal number |  |
| IsAssessmentVisibleForStudents |  | boolean |  |
| AddToAssessmentRecord |  | boolean |  |
| AssessmentRecordCategoryId |  | integer |  |
| TermId |  | integer |  |
| IsAnonymous | Defines whether submission is anonymous. | boolean |  |
| IsHomework | Defines whether the assignment is a homework assignment. | boolean |  |
| IsMandatory | Defines whether submission is mandatory. | boolean |  |
| CompletionMode | Indicates whether submission of an answer is required for this assignment, or if it can be marked as completed without submission. ElementCompletionMode.ManualByTeacher and ElementCompletionMode.ManualByStudent means that assignment is in "task" mode, otherwise submission is required - standard old assignment functionality. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. | boolean |  |
| Text |  | string |  |
| UsePlagiarismCheck |  | boolean |  |
| ShowPlagiarismResultsToStudents |  | boolean |  |
| SubmissionType |  | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| PeersToAssessCount |  | integer |  |
| IsPeerAssessmentAnonymous |  | boolean |  |
| IsSelfAssessmentRequired |  | boolean |  |
| CustomLabelIds |  | Collection of integer |  |
| WorkloadEstimationInMinutes |  | integer |  |
| Files | Attachments to be added to the assignment. Files (e.g. images) embedded in , should NOT be included here. | Collection of Personal.FileUpload.UploadedTempFile |  |
| FilesInCloud | Newly added files in cloud. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| PlannerSettings | Planner context for linking the assignment to a plan. Null when not connected to the planner. | Personal.Element.Models.PlannerSettings |  |

## Personal.Assignment.Requests.SubmitAnswerRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Text | Text for the answer, which may contain HTML. | string |  |
| Files | A collection of files that are uploaded as part of the answer, temp files uploaded using the . | Collection of Personal.FileUpload.UploadedTempFile |  |

## Personal.Assignment.Requests.SubmitAssignmentAssessmentRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to submit the assessment for. If the list contains more than one person, the same assessment will be applied to all of them. | Collection of integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used. Null also means "no change". | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. Null also means "no change". | integer |  |
| Feedback | Feedback from teacher, can contain HTML. Null to not change existing feedback, empty string to clear existing feedback. | string |  |
| Score | Entered score value in case when ElementSettingsResponse.UseScore is true. Null means "no change", pass -1 to clear existing score. | decimal number |  |
| TaskStatus | If this is passed, AssessmentStatusItemId will be ignored. Used when the assignment uses simple statuses. Null also means "no change". | TaskStatus |  |
| FeedbackFiles | A collection of files that are uploaded as part of the assessment, temp files uploaded using the . | Collection of Personal.FileUpload.UploadedTempFile |  |

## Personal.Assignment.Requests.SubmitAssignmentAssessmentRequestV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to submit the assessment for. If the list contains more than one person, the same assessment will be applied to all of them. | Collection of integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used. Null also means "no change". | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. Null also means "no change". | integer |  |
| Feedback | Feedback from teacher, can contain HTML. Null to not change existing feedback, empty string to clear existing feedback. | string |  |
| Score | Entered score value in case when ElementSettingsResponse.UseScore is true. Null means "no change", pass -1 to clear existing score. | decimal number |  |
| TaskStatus | If this is passed, AssessmentStatusItemId will be ignored. Used when the assignment uses simple statuses. Null also means "no change". | TaskStatus |  |
| DeletedFileIds | List of assessment related file ids to be deleted. | Collection of integer |  |
| NewFiles | List of temp files to add to the list of attached to assessment files. | Collection of Personal.FileUpload.UploadedTempFile |  |
| ModifiedFiles | List of assessment's modified files. | Collection of Personal.FileUpload.ModifiedElementFileModel |  |
| FilesInCloud | Files added to the cloud. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |

## Personal.Assignment.Requests.UpdateAssignmentRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title | The title of the element. | string |  |
| Visibility | Defines visibility settings. Pass null if you don't want to make any changes. | Personal.Element.Models.VisibilitySettings |  |
| AssignTo | Defines who to assign the element to (specific users and/or groups). If null, or if no specific Ids are provided: When creating element: Permissions will be the same as for parent folder.When updating element: No changes will be made to permissions. | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds | List of learning objective IDs to associate with this assignment. If null, no changes will be made to learning objectives associations. If empty, all associations will be removed. | Collection of integer |  |
| DeadlineSettings | Deadline settings. Pass null if you don't want to make any changes. | Personal.Element.Models.DeadlineSettings |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the element. If null, it will not be changed when updating the element. | integer |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the element. If null, it will not be changed when updating the element. To remove assessmentId, use 0 | integer |  |
| MaxScore | Max score in case if score is in use instead of assessment scale. If null, it will not be changed when updating the element. To remove maxScore, use any negative value, e.g. -1 | decimal number |  |
| IsAssessmentVisibleForStudents | Defines whether the assessment is visible for students (aka "Results are visible to students"). If null, it will not be changed when updating the element. | boolean |  |
| AddToAssessmentRecord | Defines whether current element should be added into assessment record. If null, it will not be changed when updating the element. | boolean |  |
| AssessmentRecordCategoryId | Which category in assessment record should be used for current element, if it is added into assessment record. Null means that it will not be changed when updating the element, or not connected. To remove connection to category, use 0. | integer |  |
| TermId | The ID of term in case of adding element into assessment record. If null, it will not be changed when updating the element. | integer |  |
| IsAnonymous | Defines whether submission is anonymous. If null, it will not be changed when updating the element. | boolean |  |
| IsHomework | Defines whether the element is homework. If null, it will not be changed when updating the element. | boolean |  |
| IsMandatory | Defines whether submission is mandatory. If null, it will not be changed when updating the element. | boolean |  |
| CompletionMode | Indicates whether submission is required for this element to complete the connected task, or if it can be marked as completed without submission. If null, it will not be changed when updating the element. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. If null, it will not be changed when updating the element. | boolean |  |
| DeletedFileIds | List of file ids to deleted, connected to the element. | Collection of integer |  |
| NewFiles | List of files to be added to the element, attachments to the element. These files should be already uploaded as temporary files using file upload API, and here you should provide their temporary file ids. | Collection of Personal.FileUpload.UploadedTempFile |  |
| ModifiedFiles |  | Collection of Personal.FileUpload.ModifiedElementFileModel |  |
| FilesInCloud |  | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| Text |  | string |  |
| UsePlagiarismCheck |  | boolean |  |
| ShowPlagiarismResultsToStudents |  | boolean |  |
| SubmissionType |  | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| PeersToAssessCount |  | integer |  |
| IsPeerAssessmentAnonymous |  | boolean |  |
| IsSelfAssessmentRequired |  | boolean |  |
| CustomLabelIds |  | Collection of integer |  |
| WorkloadEstimationInMinutes |  | integer |  |

## Personal.Assignment.Responses.AssignmentAnswerResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| AnswerId | The unique identifier for the answer to the assignment. | integer |  |
| IsDraft | True if the answer is a draft. | boolean |  |
| Submitted | The time when answer was submitted, or last modified in case of a draft. | date |  |
| Text | The text of the answer, which may contain HTML. | string |  |
| Files | The files attached to the answer. | Collection of Personal.Assignment.Responses.AssignmentFileModel |  |

## Personal.Assignment.Responses.AssignmentFileModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId | The unique ID of the file. | integer |  |
| FileRepoId | File repo ID. | string |  |
| Name | Name of the file, including the extension. | string |  |
| MimeType | File MIME type. | string |  |
| Size | Size of the file in bytes. Only applicable for files stored in the file repository. | integer, 64 bit |  |
| LastModified | When the file was last modified, useful for clients that can cache a file in a secure way. Only applicable for files stored in the file repository. | date |  |
| DownloadAsAttachment | Defines whether current file cannot be previewed in a new browser tab and should rather be downloaded as attachment. | boolean |  |
| SignedFileRepoId | Signed file repo identifier. | string |  |

## Personal.Assignment.Responses.AssignmentModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | The unique identifier for the assignment element (which is used in a course) in scope of a customer. | integer |  |
| Title | The title of the assignment | string |  |
| IsHomework | Defines whether the assignment is a homework assignment. | boolean |  |
| IsMandatory | Defines whether the assignment is mandatory. | boolean |  |
| IsLearningPathSubElement | True if the assignment is part of a learning path. | boolean |  |
| Deadline | Deadline for the assignment. If null, there is no deadline. | date |  |
| UsePlagiarismCheck | Defines whether answers will be checked for plagiarism. When true there are some restrictions on the files that can be uploaded. | boolean |  |
| ShowPlagiarismResultsToStudents | Defines whether the plagiarism results should be visible for students. | boolean |  |
| AssessmentId | The assessment id (grades / assessment scale) used for the assignment, will be null if assessment scale is not used. | integer |  |
| MaxScore | Max score used for assessment using score. | decimal number |  |
| AssessmentStatusId | The assessment status id ("not submitted", "completed" etc) used for the assignment, will be null if no status is used. | integer |  |
| IsAssessmentVisibleForStudents | Defines whether the assessment is visible for students. | boolean |  |
| IsActive | Indicates whether the element visibility setting is set to "Active" or "Scheduled". The value of false means "Inactive". | boolean |  |
| ActiveFrom | Active from date and time in UTC. If set, the element will be active from this date. | date |  |
| ActiveTo | Active to date and time in UTC. If set, the element will be active until this date. | date |  |
| IsSubmissionAfterDeadlineAllowed | Defines whether submission after the deadline is allowed. | boolean |  |
| SubmissionType | Submission type for the assignment | Personal.Assignment.Enums.AssignmentSubmissionType |  |
| CompletionMode | Indicates whether submission is required for this assignment, or if it can be marked as completed without submission. ElementCompletionMode.ManualByTeacher and ElementCompletionMode.ManualByStudent means that assignment is in "task" mode, otherwise submission is required - standard old assignment functionality. | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList | Defines whether to exclude from teacher's follow-up tasks or not. | boolean |  |
| IsAnonymous | Defines whether submission is anonymous. | boolean |  |
| IsSelfAssessmentRequired | Defines whether self-assessment is required. | boolean |  |
| PeersToAssessCount | How many other students should each student peer assess. null means that peer assessment is not used. | integer |  |
| IsPeerAssessmentAnonymous | Defines whether peer assessment is anonymous. Only relevant if is set. | boolean |  |
| Files | Files attached to the assignment. | Collection of Personal.Assignment.Responses.AssignmentFileModel |  |
| TemplateFile | Template file. Students get their own copy of this file. There could only be not more than one template file per assignment. | Personal.Assignment.Responses.AssignmentFileModel |  |
| FilesInCloud | Cloud files. | Collection of Itslearning.Web.UI.Controls.Uploaders.Metadata.FileInCloudMetadata |  |
| RawText | The raw text of the assignment, suitable for loading into the rich text editor when editing the assignment. It is not display-rendered, so it can be edited and saved again without corrupting embedded content (tree links, media, plugins). | string |  |
| Text | The assignment text processed for display: embedded files, media and plugins are rendered. Use this for showing the assignment, and for editing. | string |  |
| AnswerId | If the user has an answer, this will be the ID of that answer. Otherwise, it will be null. | integer |  |
| CanEditAssignment | True if the user can edit the assignment itself - Text and Files, false otherwise. | boolean |  |
| CanEvaluateAnswers | Returns true if the user can evaluate answers, normally a teacher permission. | boolean |  |
| CanSubmitOrEditAnswer | Returns true if the user can edit or submit answer (or set own completion, in case of CompletionMode = "ManualByStudent"). Rules for deadline, status and if the user can participate are taken into consideration. | boolean |  |
| CanParticipate | Returns true if the user is allowed to participate in this assignment. A user could have guest role in course, which only allows to view the assignment, but not to submit or edit answer. | boolean |  |
| TermId | The ID of term if the element is added into assessment record. | integer |  |
| AssessmentRecordCategoryId | Which category in assessment record should be used for current element, if it is added into assessment record. | integer |  |
| AddedToAssessmentRecord | If the current element is added into assessment record. | boolean |  |
| CustomLabelId | Custom label id | integer |  |
| LearningObjectiveSettings | Learning objective-related settings. | Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveSettingsModel |  |

## Personal.Assignment.Responses.StatusItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentStatusItemId | Assessment status item id. | integer |  |
| Title | Assessment status item title. Example can be "Not submitted". | string |  |
| IsEditable | Gets or sets a value indicating whether status is "editable", meaning that the learner can modify the work. There can be multiple status items in the list with this property set to true. | boolean |  |
| IsCompleted | Gets or sets a value indicating whether status is "completed", meaning that the learner has completed the work. There can be multiple status items in the list with this property set to true, e.g. both "Satisfactory" and "Not satisfactory" can be completed statuses. | boolean |  |
| IsSubmitted | Gets or sets a value indicating whether this status is "submitted", meaning that the learner has submitted something. Only one status item in the list should have this property set to true. | boolean |  |
| IsInitialStatus | Gets or sets a value indicating whether this status is the initial status, i.e. "not submitted", meaning that the learner has not submitted anything. Only one status item in the list should have this property set to true. | boolean |  |

## Personal.Assignment.Responses.SubmitAnswerResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| AnswerId | The id for the answer. | integer |  |

## Personal.ContentAreaSettings.Models.ParticipantGroupItemModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| GroupId | Group Id | integer |  |
| Title | Group title | string |  |

## Personal.ContentAreaSettings.Models.ParticipantItemModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId |  | integer |  |
| FullName |  | string |  |
| ExtraInformation |  | string |  |
| PictureUrl |  | string |  |

## Personal.ContentAreaSettings.Models.ParticipantsGroupModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Groups | The array of participant group items. | Collection of Personal.ContentAreaSettings.Models.ParticipantGroupItemModel |  |
| TotalCount | The total count of groups available. | integer |  |

## Personal.ContentAreaSettings.Models.ParticipantsModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | The array of participant items. | Collection of Personal.ContentAreaSettings.Models.ParticipantItemModel |  |
| TotalCount | The total count of participants available. | integer |  |

## Personal.Element.Models.AssessmentItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentItemId | Assessment item Id. | integer |  |
| Title | Assessment item title, examples: "A", "B", "C" or "Pass", "Fail". | string |  |
| Description | Assessment item description, in many cases not used. | string |  |
| PercentTo | Upper border of the assessment percent range, exclusive. | decimal number |  |
| PercentFromAndIncl | Lower border of the assessment percent range, inclusive. | decimal number |  |

## Personal.Element.Models.AssessmentScale

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentId | Id of the Assessment (can be grades) scale which has a list of items connected to it. | integer |  |
| Title | Title of the assessment scale (e.g. "Grades A - F"). | string |  |
| Description | Description of the assessment scale, this is in many cases not used. | string |  |
| AssessmentItems | List of possible "assessment items" which belong to this scale, that can be for the element (e.g. an assignment). | Collection of Personal.Element.Models.AssessmentItem |  |

## Personal.Element.Models.AssignTo

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantIds | List of PersonIds to assign the element to. | Collection of integer |  |
| GroupIds | List of GroupIds to assign the element to. | Collection of integer |  |
| GroupName | Title of the group if only one group is selected. Else it will be empty string. | string |  |

## Personal.Element.Models.CreateElementProperties

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParentFolderId |  | integer |  |
| PlannerSettings | Planner context for linking the element to a plan. Null when not connected to the planner. | Personal.Element.Models.PlannerSettings |  |

## Personal.Element.Models.DeadlineSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| Deadline | Deadline date and time in UTC. If null, there is no deadline. See also . | date |  |
| IsSubmissionAfterDeadlineAllowed | Defines whether submission after the deadline is allowed. | boolean |  |

## Personal.Element.Models.ElementLibrarySharingInfo

| Name | Description | Type | Additional information |
|---|---|---|---|
| EditInLibraryUrl | Absolute url to library where the user can edit the element. Can be null. | string |  |
| AuthorName | The name of the author of the element | string |  |
| LicenseVisible | True if there is license info which should be displayed | boolean |  |
| LicenseUrl | The url to the license page, e.g. a Creative Commons license. | string |  |
| LicenseHint | The license hint - a description for the license | string |  |
| LicenseImageUrl | The license icon url | string |  |
| LicenseName | The name of a license | string |  |

## Personal.Element.Models.ElementProperties

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title |  | string |  |
| Visibility |  | Personal.Element.Models.VisibilitySettings |  |
| AssignTo |  | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds |  | Collection of integer |  |

## Personal.Element.Models.ElementSupportsAssessment

| Name | Description | Type | Additional information |
|---|---|---|---|
| DeadlineSettings |  | Personal.Element.Models.DeadlineSettings |  |
| AssessmentStatusId |  | integer |  |
| AssessmentId |  | integer |  |
| MaxScore |  | decimal number |  |
| IsAssessmentVisibleForStudents |  | boolean |  |
| AddToAssessmentRecord |  | boolean |  |
| AssessmentRecordCategoryId |  | integer |  |
| TermId |  | integer |  |
| IsAnonymous |  | boolean |  |
| IsHomework |  | boolean |  |
| IsMandatory |  | boolean |  |
| IsOptional | Whether it is optional. | boolean |  |
| CompletionMode |  | Itslearning.Platform.RestApi.Sdk.Common.Entities.ElementCompletionMode |  |
| ExcludeFromTeacherTaskList |  | boolean |  |
| IncludeToTeacherTaskList | Opposite of . | boolean |  |

## Personal.Element.Models.EmbeddedElementContent

| Name | Description | Type | Additional information |
|---|---|---|---|
| ResourceId |  | string |  |

## Personal.Element.Models.ParticipantReport

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The unique identifier of the person (in context of the current site). | integer |  |
| FirstName | The first name of the person. | string |  |
| LastName | The last name of the person. | string |  |
| FullName | The full name of the person (formatted according to site configuration). | string |  |
| ExtraInfo | Extra information about the person. Normally something that can help identify the person, e.g. username. Note that this field may be null or empty, if for example the user who requests such information does not have sufficient permission. | string |  |
| ProfilePictureUrl | Profile picture URL of the participant, null if not available. | string |  |
| Submitted | Date and time when the answer was submitted, null if not submitted. | date |  |
| AssessmentStatusItemId | Identifier to the "assessment status item", see . Can be null if no status is set, or if status is not applicable for the element. | integer |  |
| AssessmentItemId | Identifier to the "assessment item" (grade), see . Can be null if no grade is set, or if grade is not applicable for the element, or if assessment is not visible for the user. | integer |  |
| RawFeedback | The teacher's feedback comment for the participant's answer / work. This can contain HTML. Can be null if no feedback is set, or if feedback is not applicable for the element, or if assessment is not visible for the user. This is meant to be used when e.g. using the editor to modify the text. | string |  |
| Feedback | Text of the feedback to be displayed in view of assessment. Video and plugin content is preprocessed. Can be null if no feedback is set, or if feedback is not applicable for the element, or if assessment is not visible for the user. This is meant for displaying in e.g. a browser. | string |  |
| Score | The score for the participant's answer / work. Can be null if no score is set, or if score is not applicable for the element, or if assessment is not visible for the user. | decimal number |  |
| TaskStatus | The status of the participant's task. Null means that there is no task status applicable for the participant. | TaskStatus |  |

## Personal.Element.Models.ParticipantReportV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The unique identifier of the person (in context of the current site). | integer |  |
| Submitted | Date and time when the answer was submitted, null if not submitted. | date |  |
| AssessmentStatusItemId | Identifier to the "assessment status item", see . Can be null if no status is set, or if status is not applicable for the element. | integer |  |
| AssessmentItemId | Identifier to the "assessment item" (grade), see . Can be null if no grade is set, or if grade is not applicable for the element, or if assessment is not visible for the user. | integer |  |
| AssessmentByPersonId | PersonId who set the . Can be null. | integer |  |
| AssessmentDateTime | When assessment was set. Can be null. | date |  |
| RawFeedback | The teacher's feedback comment for the participant's answer / work. This can contain HTML. Can be null if no feedback is set, or if feedback is not applicable for the element, or if assessment is not visible for the user. This is meant to be used when e.g. using the editor to modify the text. | string |  |
| Feedback | Text of the feedback to be displayed in view of assessment. Video and plugin content is preprocessed. Can be null if no feedback is set, or if feedback is not applicable for the element, or if assessment is not visible for the user. This is meant for displaying in e.g. a browser. | string |  |
| FeedbackByPersonId | PersonId who set the . Can be null. | integer |  |
| FeedbackDateTime | When feedback was set. Can be null. | date |  |
| Score | The score for the participant's answer / work. Can be null if no score is set, or if score is not applicable for the element, or if assessment is not visible for the user. | decimal number |  |
| TaskStatus | The status of the participant's task. Null means that there is no task status applicable for the participant. | TaskStatus |  |

## Personal.Element.Models.PlannerSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| PlanId | The ID of the plan to link the element to. | integer |  |
| ColumnId | The ID of the planner column. | integer |  |

## Personal.Element.Models.StatusScale

| Name | Description | Type | Additional information |
|---|---|---|---|
| AssessmentStatusId | Id of the Assessment status which has a list of status items connected to it. | integer |  |
| Title | Title of the assessment status. | string |  |
| Description | Description of the assessment status, this is in many cases not used. | string |  |
| StatusItems | List of possible "assessment status items" for the element (e.g. an assignment). These can define the status of a given answer (submission). | Collection of Personal.Assignment.Responses.StatusItem |  |
| UseSimpleStatus | Whether the element uses simple statuses instead of custom statuses to submit assessment. If true, TaskStatus is expected when saving an assessment. | boolean |  |

## Personal.Element.Models.VisibilitySettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| IsActive | Set to true or false to make the element active or inactive. Or you can use and to set a time range when the element is active. | boolean |  |
| ActiveFrom | Active from date and time in UTC. If set, the element will be active from this date. If is set, and this one is null, the element will be active until . | date |  |
| ActiveTo | Active to date and time in UTC. If set, the element will be active until this date. If is set, and this one is null, the element will be active from and onwards. | date |  |

## Personal.Element.Requests.SubmitElementAssessmentRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to submit the assessment for. If the list contains more than one person, the same assessment will be applied to all of them. | Collection of integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used. Null also means "no change". | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. Null also means "no change". | integer |  |
| Feedback | Feedback from teacher, can contain HTML. Null to not change existing feedback, empty string to clear existing feedback. | string |  |
| Score | Entered score value in case when ElementSettingsResponse.UseScore is true. Null means "no change", pass -1 to clear existing score. | decimal number |  |
| TaskStatus | If this is passed, AssessmentStatusItemId will be ignored. Used when the assignment uses simple statuses. Null also means "no change". | TaskStatus |  |

## Personal.Element.Requests.SubmitTaskStatusRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonIds | List of person Ids to set the task status for. | Collection of integer |  |
| TaskStatus | Task status to set, for a task which is associated with an element. Allowed values are "NotStarted", "InProgress", "Completed". | TaskStatus |  |

## Personal.Element.Responses.CustomLabelModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Id | Custom label ID. | integer |  |
| Name | Custom label name. | string |  |
| Description | Custom label description. | string |  |
| Color | Custom label color. | string |  |

## Personal.Element.Responses.ElementAssessmentResult

| Name | Description | Type | Additional information |
|---|---|---|---|
| PersonId | The person for whom the assessment was submitted. | integer |  |
| AssessmentItemId | Assessment scale item. Null when no scale is used or not set. | integer |  |
| AssessmentStatusItemId | Assessment status item. Null when the assignment uses simple statuses. | integer |  |
| IsFeedbackChanged | True if feedback was changed. | boolean |  |
| Score | Score for the assessment, can be null if no score is used or not set. The scale for the score is defined by the assignment. | decimal number |  |
| TaskStatus | Simple status for the element in the task list. | TaskStatus |  |

## Personal.Element.Responses.ElementSettingsResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| StatusScale | The assessment status which is used to set a status item on an answer (submission). Can be null if no assessment status is defined for the element. | Personal.Element.Models.StatusScale |  |
| AssessmentScale | The assessment scale which is used to set a grade on an answer (submission). Can be null if no assessment scale is defined for the element. | Personal.Element.Models.AssessmentScale |  |
| CustomLabels | Custom labels associated with the element. | Collection of Personal.Element.Responses.CustomLabelModel |  |
| Terms | The list of terms used in course. | Collection of Itsolutions.Itslearning.Web.Areas.Elements.Models.SelectItem |  |
| Categories | The list of assessment record categories used in course. | Collection of Itsolutions.Itslearning.Web.Areas.Elements.Models.SelectItem |  |

## Personal.Element.Responses.OwnReportResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| ParticipantReport | Current person report. | Personal.Element.Models.ParticipantReportV2 |  |
| FeedbackPerson | Current person report feedback person. | Personal.ContentAreaSettings.Models.ParticipantItemModel |  |
| AssessmentByPerson | Current person report assessment person. | Personal.ContentAreaSettings.Models.ParticipantItemModel |  |

## Personal.Element.Responses.ParticipantsReportResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | The list of participants who currently have access to the element (e.g. an assignment). Participants who have submitted work but no longer have access to the element are not included. | Collection of Personal.Element.Models.ParticipantReport |  |
| TotalCount | Total count of participants who have access to the element, for paging purposes. This can be higher than the number of items in the "Participants" list, if paging is used. | integer |  |
| ExtraInfoDisplayName | The localized display name for what the "ExtraInfo" holds, if any. Can be e.g. "Username". Can be null if no extra info is provided or if the user does not have access to such data. | string |  |

## Personal.Element.Responses.ParticipantsReportResponseV2

| Name | Description | Type | Additional information |
|---|---|---|---|
| Participants | The list of participants who currently have access to the element (e.g. an assignment). Participants who have submitted work but no longer have access to the element are not included. | Collection of Personal.Element.Models.ParticipantReportV2 |  |
| TotalCount | Total count of participants who have access to the element, for paging purposes. This can be higher than the number of items in the "Participants" list, if paging is used. | integer |  |
| Persons | Contains more detailed information about PersonIds mentioned in the list. | Collection of Personal.ContentAreaSettings.Models.ParticipantItemModel |  |
| ExtraInfoDisplayName | The localized display name for what the "ExtraInformation" holds, if any. Can be e.g. "Username". Can be null if no extra info is provided or if the user does not have access to such data. | string |  |

## Personal.Element.Responses.SubmitElementAssessmentResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| Results |  | Collection of Personal.Element.Responses.ElementAssessmentResult |  |
| Feedback | Text to be displayed in view content block mode. Video and plugin content is preprocessed. | string |  |

## Personal.EmbeddedElement.Models.CreateEmbeddedElementRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ExtensionId | Id of extension. | integer |  |
| ElementProperties | Element properties. | Personal.Element.Models.ElementProperties |  |
| ElementContent | Embedded element content. | Personal.Element.Models.EmbeddedElementContent |  |
| CreateElementProperties | Create element properties. | Personal.Element.Models.CreateElementProperties |  |
| ElementSupportsAssessment | Element supports assessment. | Personal.Element.Models.ElementSupportsAssessment |  |

## Personal.EmbeddedElement.Models.CreateUpdateEmbeddedElementResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | Id of an element. | integer |  |

## Personal.EmbeddedElement.Models.UpdateEmbeddedElementRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementProperties | Element properties. | Personal.Element.Models.ElementProperties |  |
| ElementSupportsAssessment | Element supports assessment. | Personal.Element.Models.ElementSupportsAssessment |  |

## Personal.FileUpload.ModifiedElementFileModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileId | File ID. It's not FileRepo ID. | integer |  |
| Name | New filename (including file extension). | string |  |

## Personal.FileUpload.UploadedTempFile

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileRepoId | File repo ID. | string |  |
| SignedFileRepoId | The signature of the file. Useful when file is still saved as a temporary file. | string |  |
| Name | File name. | string |  |
| MimeType | File MIME type. | string |  |
| SourceType | File source type which defines the way file was uploaded. E.g. via local upload or cloud integration. | Itslearning.FileRepo.Common.Models.SourceType |  |
| IsTemplate | Gets or sets the file should be a template. | boolean |  |

## Personal.MediaRecorderController_2BNativeRecordingFinalizeRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| jobId | Identifier of the transcoding job returned from finish upload. | string |  |
| signature | User-scoped signature returned together with . | string |  |

## Personal.MediaRecorderController_2BNativeUploadFinishRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| recordingId | Identifier of the recording created when the upload started. | string |  |
| uploadId | Identifier of the multipart upload session being completed. | string |  |
| objectKey | Storage object key for the uploaded media. | string |  |
| recordingName | User-provided name for the saved recording. | string |  |
| recorderType | Recorder implementation used to create the media. | string |  |
| isVideoRecording | Indicates whether the uploaded recording contains video. | boolean |  |
| usedInFileUploader | Indicates whether the recorder is used inside the file uploader flow. | boolean |  |
| parts | Uploaded multipart chunks and their returned entity tags. | Collection of Personal.MediaRecorderController+NativeUploadPart |  |

## Personal.MediaRecorderController_2BNativeUploadPart

| Name | Description | Type | Additional information |
|---|---|---|---|
| partNumber | 1-based index of the uploaded multipart chunk. | integer |  |
| etag | Entity tag returned by storage for the uploaded part. | string |  |

## Personal.MediaRecorderController_2BNativeUploadStartRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| fileName | Original file name reported by the client. | string |  |
| contentType | MIME type of the recording being uploaded. | string |  |
| size | Total size of the recording in bytes, as declared by the client. | integer, 64 bit |  |
| chunkCount | Number of upload parts the client will send. | integer |  |
| recorderType | Recorder implementation used to create the media. | string |  |
| isVideoRecording | Indicates whether the recording contains video. | boolean |  |
| usedInFileUploader | Indicates whether the recorder is used inside the file uploader flow. | boolean |  |

## Personal.MobileSettings.Models.MobileSettingsModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| EmbedLoaderScriptUrl | URL of embed loader script to inject when displaying rich text | string |  |

## Personal.Page.PageApiModels.Enums.LayoutSchemaDto

| Name | Value | Description |
|---|---|---|
| OneColumn | 0 |  |
| TwoColumns | 1 |  |
| TwoColumnsWideLeft | 2 |  |

## Personal.Page.PageApiModels.Enums.LayoutZoneDto

| Name | Value | Description |
|---|---|---|
| Left | 1 |  |
| Right | 2 |  |

## Personal.Page.PageApiModels.ImageMetaData

| Name | Description | Type | Additional information |
|---|---|---|---|
| ImageId | The ID of the image to update. | integer |  |
| AltText | The alt text for the image (used for accessibility). Keep null to leave unchanged. | string |  |
| Caption | The caption for the image. Keep null to leave unchanged. | string |  |

## Personal.Page.PageApiModels.Requests.CreateResizedImageFilesRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| TempImageFile | A temp image file in file repo. | Personal.Page.PageApiModels.Requests.TempFileRepoFile |  |

## Personal.Page.PageApiModels.Requests.ImageItem

| Name | Description | Type | Additional information |
|---|---|---|---|
| ImageFile | The uploaded temporary image file. Must exist in "FileRepo" as a temporary file. | Personal.Page.PageApiModels.Requests.TempFileRepoFile |  |
| LargeImageFile | The uploaded temporary image file for the large image. Must exist in "FileRepo" as a temporary file. | Personal.Page.PageApiModels.Requests.TempFileRepoFile |  |
| MediumImageFile | The uploaded temporary image file for the medium image. Must exist in "FileRepo" as a temporary file. | Personal.Page.PageApiModels.Requests.TempFileRepoFile |  |
| SmallImageFile | The uploaded temporary image file for the small image. Must exist in "FileRepo" as a temporary file. | Personal.Page.PageApiModels.Requests.TempFileRepoFile |  |
| AltText | The alternative text for the image, used for accessibility purposes. | string |  |
| Caption | The caption text displayed below or alongside the image. | string |  |

## Personal.Page.PageApiModels.Requests.TempFileRepoFile

| Name | Description | Type | Additional information |
|---|---|---|---|
| FileRepoId | The unique identifier of the file in FileRepo | globally unique identifier |  |
| SignedFileRepoId | The unique identifier of the signed file in FileRepo. | string |  |
| Name | The name of the file | string |  |
| MimeType | The mime type of the file | string |  |

## Personal.Page.PageApiModels.Requests.UpdatePageRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Layout | The layout for the page, defining how content blocks should be arranged. | Personal.Page.PageApiModels.Enums.LayoutSchemaDto |  |
| BlocksPlacement |  | Personal.Page.PageApiModels.Requests.UpdatePlacementRequest |  |

## Personal.Page.PageApiModels.Requests.UpdatePlacementRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| LeftColumnContentBlockIds |  | Collection of integer |  |
| RightColumnContentBlockIds |  | Collection of integer |  |

## Personal.Page.Requests.CreateContentBlockFilesRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| Files | The collection of temporary files that have been uploaded for this content block. Defaults to an empty list. | Collection of Personal.FileUpload.UploadedTempFile |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.CreateContentBlockImagesRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| Images | The collection of temporary images that have been uploaded for this content block. Defaults to an empty list. | Collection of Personal.Page.PageApiModels.Requests.ImageItem |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.CreateContentBlockPollsRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| VotedAlternativeId | The identifier of the alternative that the current user has voted for. Null if the user has not voted yet. | integer |  |
| TotalVotes | The total number of votes cast across all poll alternatives. Null if vote count is not available. | integer |  |
| Question | The main question text displayed to users for the poll. | string |  |
| Settings | The configuration settings that control poll behavior and display options. | Personal.Page.Requests.PollSettings |  |
| Alternatives | The collection of available voting options for the poll. Defaults to an empty list. | Collection of Personal.Page.Requests.PollAlternative |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.CreateContentBlockRssRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| RssUrl | The URL of the RSS feed to display in this content block. | string |  |
| FeedsCount | The number of feed items to display from the RSS feed. | integer |  |
| ShowSummary | Indicates whether to show the summary of each feed item. | boolean |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.CreateContentBlockTextRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| RawText | The text content to display in this content block. Can contain formatted text or plain text. Doesn't contain plugin or video content preprocessing. | string |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.DeleteRestoreRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContentBlockIds | The list of content block IDs to be moved in or out from trashcan. | Collection of integer |  |
| IsDeleted | The boolean that describe the operation that needs to be done. If true, then the content blocks will be moved into trashcan, otherwise will be restored. | boolean |  |
| LayoutZone | The layout zone where the content block will be placed. Optional property that defaults to Left. Examples: - LayoutZone = Left => put content in the left column - LayoutZone = Right => put content in the right column | Personal.Page.PageApiModels.Enums.LayoutZoneDto |  |
| Order | The position of the content block within the specified layout zone. Optional property, default last in this layout zone. If there is an existing content block at this position, that one and all blocks below will be shifted down. Examples: - Order = 1, LayoutZone = Left => top left position - Order = 2, LayoutZone = Left => second position in left column - Order = 1, LayoutZone = Right => top right position | integer |  |

## Personal.Page.Requests.PermanentlyDeleteContentBlocksRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContentBlockIds | The list of content block IDs to be permanently deleted. | Collection of integer |  |

## Personal.Page.Requests.PollAlternative

| Name | Description | Type | Additional information |
|---|---|---|---|
| AlternativeId | The unique identifier for this poll alternative. | integer |  |
| Text | The display text shown to users for this voting option. | string |  |

## Personal.Page.Requests.PollSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| ShowResults | Controls when poll results are visible to users (e.g. "AfterVoting", "Always", "Never"). | string |  |
| DisplayNumberOfVotes | Indicates whether the number of votes for each alternative should be displayed. | boolean |  |
| UserCanChangeVote | Indicates whether users are allowed to change their vote after initially voting. | boolean |  |

## Personal.Page.Requests.UpdateContentBlockFilesRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| NewFiles |  | Collection of Personal.FileUpload.UploadedTempFile |  |
| DeletedFiles |  | Collection of integer |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.UpdateContentBlockImagesRequestUI

| Name | Description | Type | Additional information |
|---|---|---|---|
| NewImages | Images to add to the content block. | Collection of Personal.Page.PageApiModels.Requests.ImageItem |  |
| DeletedImageIds | Image IDs to delete from the content block. | Collection of integer |  |
| UpdatedImages | Images that should be updated in the content block. | Collection of Personal.Page.PageApiModels.ImageMetaData |  |
| ContentBlockId | The unique identifier for the content block. | integer |  |
| Type | The type of the content block (e.g. "text", "image", "files", "rss", "poll"). Defaults to an empty string. | string |  |
| Title | The title of the content block. Defaults to an empty string. | string |  |
| Order | The display order of the content block within its Layout Zone. Lower values appear first. | integer |  |
| LayoutZone | The layout zone where the content block should be placed (e.g. "Left", "Right"). Determines the positioning within the page layout. | string |  |
| Color | The color of the content block. | string |  |

## Personal.Page.Requests.UpdatePageSettingsRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| Title |  | string |  |
| Visibility |  | Personal.Element.Models.VisibilitySettings |  |
| AssignTo |  | Personal.Element.Models.AssignTo |  |
| LearningObjectivesIds |  | Collection of integer |  |

## Personal.Page.Requests.VoteRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| VoteAlternativeId | The unique identifier of the poll alternative that the user is voting for. | integer |  |

## Personal.Page.Responses.ColorSchema

| Name | Value | Description |
|---|---|---|
| Default | 0 |  |
| Yellow | 1 |  |
| Green | 2 |  |
| Grey | 3 |  |
| Pink | 4 |  |
| Red | 5 |  |
| Aqua | 6 |  |
| Lemon | 7 |  |
| Sand | 8 |  |
| Purple | 9 |  |

## Personal.Page.Responses.ContentBlock

| Name | Description | Type | Additional information |
|---|---|---|---|
| ContentBlockId | Content block identifier | integer |  |
| Type |  | Personal.Page.Responses.ContentBlockType |  |
| Title | Content block title | string |  |
| Order | Content block order in the page | integer |  |
| LayoutZone | Content block layout zone - left or right | Personal.Page.Responses.ContentBlockLayoutZone |  |
| Color | Content block color | Personal.Page.Responses.ColorSchema |  |

## Personal.Page.Responses.ContentBlockLayoutZone

| Name | Value | Description |
|---|---|---|
| Left | 0 |  |
| Right | 1 |  |

## Personal.Page.Responses.ContentBlockPoll

| Name | Description | Type | Additional information |
|---|---|---|---|
| VotedAlternativeId | Id of chosen alternative (null if not voted yet) | integer |  |
| TotalVotes | Total number of votes in the poll (null if not allowed to show) | integer |  |
| Question | Poll question | string |  |
| Settings |  | Personal.Page.Responses.ContentBlockPollSettings |  |
| Alternatives |  | Collection of Personal.Page.Responses.ContentBlockPollAlternativeData |  |
| ContentBlockId | Content block identifier | integer |  |
| Type |  | Personal.Page.Responses.ContentBlockType |  |
| Title | Content block title | string |  |
| Order | Content block order in the page | integer |  |
| LayoutZone | Content block layout zone - left or right | Personal.Page.Responses.ContentBlockLayoutZone |  |
| Color | Content block color | Personal.Page.Responses.ColorSchema |  |

## Personal.Page.Responses.ContentBlockPollAlternativeData

| Name | Description | Type | Additional information |
|---|---|---|---|
| AlternativeId |  | integer |  |
| Text |  | string |  |
| ResultPercentage |  | decimal number |  |
| VotesCount |  | integer |  |

## Personal.Page.Responses.ContentBlockPollSettings

| Name | Description | Type | Additional information |
|---|---|---|---|
| ShowResults |  | Personal.Page.Responses.PollShowResultsType |  |
| DisplayNumberOfVotes |  | boolean |  |
| UserCanChangeVote |  | boolean |  |

## Personal.Page.Responses.ContentBlockType

| Name | Value | Description |
|---|---|---|
| Text | 0 |  |
| Images | 2 |  |
| Files | 6 |  |
| Rss | 7 |  |
| Poll | 8 |  |

## Personal.Page.Responses.LayoutSchemaType

| Name | Value | Description |
|---|---|---|
| OneColumn | 0 |  |
| TwoColumns | 1 |  |
| TwoColumnsWideLeft | 2 |  |

## Personal.Page.Responses.PageIconUrls

| Name | Description | Type | Additional information |
|---|---|---|---|
| ArchiveIcon |  | string |  |
| ContentBlockChangeColumnIcon |  | string |  |
| ContentBlockDeleteIcon |  | string |  |
| ContentBlockEditIcon |  | string |  |
| ContentBlockFilesIcon |  | string |  |
| ContentBlockImagesIcon |  | string |  |
| ContentBlockMoveDownIcon |  | string |  |
| ContentBlockMoveUpIcon |  | string |  |
| ContentBlockPollIcon |  | string |  |
| ContentBlockRichTextIcon |  | string |  |
| ContentBlockRssIcon |  | string |  |
| LayoutOneColumnIcon |  | string |  |
| LayoutTwoColumnsIcon |  | string |  |
| LayoutTwoColumnsWideLeftIcon |  | string |  |
| LearningObjectiveIcon |  | string |  |
| ListenIcon |  | string |  |
| NotifyAboutChangesIcon |  | string |  |
| Permissions |  | string |  |
| RestoreIcon |  | string |  |
| EditInLibraryIcon |  | string |  |
| CreateNewVersionIcon |  | string |  |

## Personal.Page.Responses.PageKpiModel

| Name | Description | Type | Additional information |
|---|---|---|---|
| Endpoint |  | string |  |
| Context |  | integer |  |
| SectionPage |  | integer |  |
| MeasurementPrintPage |  | integer |  |

## Personal.Page.Responses.PageResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| ElementId | The unique identifier for the page element in scope of a customer. | integer |  |
| Title | The title of the page. | string |  |
| ContentBlocks | The content blocks on the page. | Collection of Personal.Page.Responses.ContentBlock |  |
| CanEditSettings | True if the user can modify the settings of the page (e.g. learning objectives, activation, layout). | boolean |  |
| CanEditTitle | True if the user can modify the title of the page. This can be true even when is false, e.g. for a student editing a page they collaborate on inside a collaboration folder. | boolean |  |
| CanEditContent | True if the page's content blocks can be edited by the current user. | boolean |  |
| CanChangePermissions | True if the current user is able to manage permissions for the current element. | boolean |  |
| CanDelete | True if the current user is allowed to delete the page (put in trashcan). An element which is part of learning path, must be removed from the learning path first. | boolean |  |
| IsLearningPathSubElement | Defines whether current element belongs to learning path. | boolean |  |
| PlansUsingResourceCount | Number of plans in the course that use the current page element. The "N plans using this resource" link is only shown when this is greater than one. | integer |  |
| Layout | The layout of the page (how many columns). | Personal.Page.Responses.LayoutSchemaType |  |
| Urls | URLs used in page. | Personal.Page.Responses.PageUrls |  |
| IconUrls | URLs for the icons used for page. | Personal.Page.Responses.PageIconUrls |  |
| Kpi | Contains KPI context, sections and measurements. | Personal.Page.Responses.PageKpiModel |  |
| IsCurrentlyActive | Indicates whether the page is visible in the present moment. | boolean |  |
| IsActive | Indicates whether the element visibility setting is set to "Active" or "Scheduled". The value of false means "Inactive". | boolean |  |
| ActiveFrom | When the page becomes active. Null means it is active immediately. | date |  |
| ActiveTo | When the page becomes inactive. Null means it remains active indefinitely. | date |  |
| LearningObjectiveSettings | Contains learning objective-related settings. | Itsolutions.Itslearning.Web.Areas.Elements.Models.LearningObjectiveSettingsModel |  |
| SharingInfo | Sharing info - contains information about how and if page is connected to library. If this is null, it means that the page is not connected to the library or that user does not have access to this information. | Personal.Element.Models.ElementLibrarySharingInfo |  |
| CopyStatus | The copy status of the page. | Itslearning.Platform.RestApi.Sdk.Common.Entities.LearningToolCopyStatus |  |
| HasPermissionToCreateNewVersion | Returns true if the user is allowed to create a new version of an existing page, which is disconnected from the library. Check for a full check, since tool or author can still deny and override a user's permission. | boolean |  |
| ToolSupportsCopying | Returns true if the learning tool supports copying, which is required for creating a new version of an existing learning tool element. | boolean |  |
| AuthorAllowsCreateNewVersion | Returns true if the author of the learning tool element allows a new version to be created. | boolean |  |
| CanCreateNewVersion | Returns true if the user is allowed to create a new version of an existing learning tool element. | boolean |  |
| CanAddToLibrary | Returns true if the user is allowed to add the learning tool elements to user's library collection. | boolean |  |
| CurrentLanguage | Current language which is used for UI. | string |  |
| FirstDayOfWeek | The first day of week. | integer |  |
| ShortDatePattern | Custom format string for a short date value. | string |  |
| FormatCultureName | The current format culture name. | string |  |
| Use24TimeFormat | If the user uses 24-hour time format. | boolean |  |

## Personal.Page.Responses.PageUrls

| Name | Description | Type | Additional information |
|---|---|---|---|
| Permissions | URL to element permissions page. | string |  |
| SharedElement | URL to the page listing all plans in the course that use the current page element. Only set when the element is used in more than one plan. | string |  |

## Personal.Page.Responses.PollShowResultsType

| Name | Value | Description |
|---|---|---|
| EditorsOnly | 0 |  |
| AnyUserAtAnyTime | 1 |  |
| AfterTheUserVotes | 2 |  |

## Personal.SavedCourseMaterials.SavedCourseMaterialsController_2BAddFileRequest

| Name | Description | Type | Additional information |
|---|---|---|---|
| ResourceId |  | integer |  |

## Personal.TokenResponse

| Name | Description | Type | Additional information |
|---|---|---|---|
| access_token | The accesstoken that's used for calling API | string |  |
| token_type | The token type. Refer to OAuth2 specs for more detailed information | string |  |
| expires_in | Seconds until expiry of the access token | integer |  |
| refresh_token | The refresh_token, used for getting a new access token and refresh token pair after expiry. | string |  |

## Public.Unauthenticated.SitesFilter

| Name | Value | Description |
|---|---|---|
| All | 0 |  |
| Itslearning | 1 |  |
| Parents | 2 |  |

## ServiceLogic.Statistics.Events.UserEventDto

| Name | Description | Type | Additional information |
|---|---|---|---|
| Type | Type of an event represented by this object | ServiceLogic.Statistics.Events.UserEventType |  |
| DateTime | Date and time when event has occurred | date |  |
| Payload | Additional event data, specific for of this event | Object |  |

## ServiceLogic.Statistics.Events.UserEventType

| Name | Value | Description |
|---|---|---|
| CourseVisit | 0 |  |
| ElementVisit | 1 |  |
| SessionMeasured | 2 |  |

## System.Net.Http.HttpContent

| Name | Description | Type | Additional information |
|---|---|---|---|
| Headers |  | Collection of Object |  |

## System.Net.Http.HttpMethod

| Name | Description | Type | Additional information |
|---|---|---|---|
| Method |  | string |  |

## System.Net.Http.HttpRequestMessage

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| Method |  | System.Net.Http.HttpMethod |  |
| RequestUri |  | URI |  |
| Headers |  | Collection of Object |  |
| Properties |  | Dictionary of string [key] and Object [value] |  |

## System.Net.Http.HttpResponseMessage

| Name | Description | Type | Additional information |
|---|---|---|---|
| Version |  | System.Version |  |
| Content |  | System.Net.Http.HttpContent |  |
| StatusCode |  | System.Net.HttpStatusCode |  |
| ReasonPhrase |  | string |  |
| Headers |  | Collection of Object |  |
| RequestMessage |  | System.Net.Http.HttpRequestMessage |  |
| IsSuccessStatusCode |  | boolean |  |

## System.Net.HttpStatusCode

| Name | Value | Description |
|---|---|---|
| Continue | 100 |  |
| SwitchingProtocols | 101 |  |
| OK | 200 |  |
| Created | 201 |  |
| Accepted | 202 |  |
| NonAuthoritativeInformation | 203 |  |
| NoContent | 204 |  |
| ResetContent | 205 |  |
| PartialContent | 206 |  |
| MultipleChoices | 300 |  |
| Ambiguous | 300 |  |
| MovedPermanently | 301 |  |
| Moved | 301 |  |
| Found | 302 |  |
| Redirect | 302 |  |
| SeeOther | 303 |  |
| RedirectMethod | 303 |  |
| NotModified | 304 |  |
| UseProxy | 305 |  |
| Unused | 306 |  |
| TemporaryRedirect | 307 |  |
| RedirectKeepVerb | 307 |  |
| BadRequest | 400 |  |
| Unauthorized | 401 |  |
| PaymentRequired | 402 |  |
| Forbidden | 403 |  |
| NotFound | 404 |  |
| MethodNotAllowed | 405 |  |
| NotAcceptable | 406 |  |
| ProxyAuthenticationRequired | 407 |  |
| RequestTimeout | 408 |  |
| Conflict | 409 |  |
| Gone | 410 |  |
| LengthRequired | 411 |  |
| PreconditionFailed | 412 |  |
| RequestEntityTooLarge | 413 |  |
| RequestUriTooLong | 414 |  |
| UnsupportedMediaType | 415 |  |
| RequestedRangeNotSatisfiable | 416 |  |
| ExpectationFailed | 417 |  |
| UpgradeRequired | 426 |  |
| InternalServerError | 500 |  |
| NotImplemented | 501 |  |
| BadGateway | 502 |  |
| ServiceUnavailable | 503 |  |
| GatewayTimeout | 504 |  |
| HttpVersionNotSupported | 505 |  |

## System.Version

| Name | Description | Type | Additional information |
|---|---|---|---|
| Major |  | integer |  |
| Minor |  | integer |  |
| Build |  | integer |  |
| Revision |  | integer |  |
| MajorRevision |  | integer |  |
| MinorRevision |  | integer |  |

## System.Web.Http.IHttpActionResult

_No fields documented (possibly an enum or primitive)._

## TupleOfSystem.GuidAndSystem.String

| Name | Description | Type | Additional information |
|---|---|---|---|
| Item1 |  | globally unique identifier |  |
| Item2 |  | string |  |

## Unresolved models

These are referenced but their help pages returned an error (HTTP 500) when crawled. Infer values from JSON samples or live responses.

- `Itslearning.RestApi.Entities.HierarchyRole`
- `Itslearning.RestApi.Entities.LightBulletins.LightBulletinTimePeriod`
- `Itslearning.RestApi.Entities.ResourceType`
- `Itslearning.RestApi.Entities.SupervisorType`
- `Itslearning.RestApi.Entities.TaskDeadlineFilter`
- `Itsolutions.ItslUtils.Constants.ContentAreaFilterType`
- `Itsolutions.ItslUtils.Constants.ItslearningSectionUrlParameter`
