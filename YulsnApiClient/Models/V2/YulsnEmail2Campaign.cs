using System;
using System.Collections.Generic;

namespace YulsnApiClient.Models.V2
{
    /// <summary>
    /// A request to the email campaign endpoints, made on behalf of an administrator.
    /// </summary>
    public class YulsnEmail2Request
    {
        /// <summary>
        /// The administrator the request is made by, saved as the one who last changed the campaign.
        /// Required: a missing or unknown administrator is rejected with 400.
        /// </summary>
        public int AdminId { get; set; }
    }

    /// <summary>
    /// The settings of a campaign message. Its status is changed by scheduling it.
    /// </summary>
    public class YulsnEmail2CampaignRequest : YulsnEmail2Request
    {
        /// <summary>Required, at most 255 characters.</summary>
        public string Name { get; set; }
        /// <summary>One of <see cref="Client.YulsnClient.GetEmail2CampaignTemplatesAsync"/>.</summary>
        public int TemplateId { get; set; }
        /// <summary>
        /// Whether opens and clicks are tracked. Only with the tracking opt-out feature of the account can it
        /// be false; otherwise the campaign is always tracked. Tracked when left out.
        /// </summary>
        public bool? Tracking { get; set; }
        /// <summary>Required.</summary>
        public string Subject { get; set; }
        public string Preheader { get; set; }
        /// <summary>Values the content can use as Dynamic.{name}, also when the campaign is previewed or tested.</summary>
        public Dictionary<string, object> DynamicDictionary { get; set; }
    }

    public class YulsnEmail2CampaignCreateRequest : YulsnEmail2CampaignRequest
    {
    }

    /// <summary>
    /// All the settings of the campaign, replacing the ones it had: a setting left out is cleared.
    /// </summary>
    public class YulsnEmail2CampaignUpdateRequest : YulsnEmail2CampaignRequest
    {
        /// <summary>
        /// Removes the content blocks the new template does not allow. Without it, the update is rejected
        /// with 409 when there are any.
        /// </summary>
        public bool RemoveIncompatibleBlocks { get; set; }
    }

    public class YulsnEmail2Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>Draft until it is scheduled; then Scheduled, Sent, Failed or Archived.</summary>
        public YulsnMessageStatus Status { get; set; }
        public int TemplateId { get; set; }
        /// <summary>Base segment of the template: the campaign is only sent to its contacts. Without one, it cannot be scheduled.</summary>
        public int? BaseSegmentId { get; set; }
        public bool Tracking { get; set; }
        /// <summary>The dispatch of the campaign once it is scheduled or sent.</summary>
        public Guid? DispatchId { get; set; }
        public string Subject { get; set; }
        public string Preheader { get; set; }
        public Dictionary<string, object> DynamicDictionary { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }
        /// <summary>Id of the administrator who last changed the campaign.</summary>
        public int? LastModifiedBy { get; set; }
    }

    public class YulsnEmail2CampaignUpdateResponse
    {
        public YulsnEmail2Campaign Campaign { get; set; }
        /// <summary>Names of the content blocks the template does not allow, removed from the campaign.</summary>
        public List<string> RemovedBlocks { get; set; }
    }

    /// <summary>A template a campaign can be created or updated with.</summary>
    public class YulsnEmail2MessageTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>The template the admin preselects.</summary>
        public bool IsDefault { get; set; }
        public int? ParentTemplateId { get; set; }
        /// <summary>Without one, a campaign with the template cannot be scheduled.</summary>
        public int? BaseSegmentId { get; set; }
    }

    /// <summary>A content block of a campaign.</summary>
    public class YulsnEmail2MessageBlock
    {
        /// <summary>Id of the content block of the campaign.</summary>
        public int Id { get; set; }
        public int MessageId { get; set; }
        /// <summary>Id of the content block it is made from, one of the available blocks.</summary>
        public int BlockId { get; set; }
        /// <summary>Position in the campaign, from 1.</summary>
        public int SortOrder { get; set; }
        public string BlockName { get; set; }
        /// <summary>The values of the inputs of the content block, by input key.</summary>
        public Dictionary<string, YulsnEmail2MessageBlockInput> InputValues { get; set; }
        /// <summary>The contacts the content block is shown to, null when it is shown to everyone.</summary>
        public YulsnEmail2MessageBlockCondition Condition { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public int? LastModifiedBy { get; set; }
    }

    /// <summary>The value of an input of a content block.</summary>
    public class YulsnEmail2MessageBlockInput
    {
        /// <summary>The value of a Text, Html, Number, Link, Media or Bundle input.</summary>
        public string Value { get; set; }
        /// <summary>The values of the user inputs of a DataObject input, by input key.</summary>
        public Dictionary<string, string> Values { get; set; }
    }

    /// <summary>
    /// A contact field compared to a value: equal or not equal to one of the values a text field allows, or to
    /// true or false for a yes/no field; in or not in the values a text field allows; or null or not null.
    /// </summary>
    public class YulsnEmail2MessageBlockCondition
    {
        /// <summary>The contact field.</summary>
        public string Name { get; set; }
        public YulsnEmail2FieldFilterOperator Operator { get; set; }
        public string Value { get; set; }
        /// <summary>For the In and NotIn operators.</summary>
        public string[] Values { get; set; }
    }

    /// <summary>A content block that can be added to a campaign.</summary>
    public class YulsnEmail2Block
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>The media item of its thumbnail.</summary>
        public int? MediaId { get; set; }
        /// <summary>The templates it can be used with, empty when it can be used with any template.</summary>
        public List<int> TemplateIds { get; set; }
        /// <summary>The inputs a content block made from it takes, by input key.</summary>
        public Dictionary<string, YulsnEmail2BlockInput> Inputs { get; set; }
    }

    /// <summary>An input of a content block.</summary>
    public class YulsnEmail2BlockInput
    {
        public YulsnEmail2BlockInputType Type { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        /// <summary>The keys and names a Bundle input can be set to.</summary>
        public Dictionary<string, string> Options { get; set; }
        /// <summary>The user inputs of a DataObject input, by input key, with their default values.</summary>
        public Dictionary<string, string> DataObjectInputs { get; set; }
    }

    public class YulsnEmail2CampaignBlockAddRequest : YulsnEmail2Request
    {
        /// <summary>One of the available blocks of the campaign.</summary>
        public int BlockId { get; set; }
        /// <summary>The content block of the campaign to add it after, at the end when left out.</summary>
        public int? AfterId { get; set; }
        /// <summary>
        /// The values of the inputs, by input key. An input that is left out (or null) gets its default value;
        /// an empty value stays empty.
        /// </summary>
        public Dictionary<string, YulsnEmail2MessageBlockInput> InputValues { get; set; }
        /// <summary>The contacts the content block is shown to, everyone when left out.</summary>
        public YulsnEmail2MessageBlockCondition Condition { get; set; }
    }

    /// <summary>The values of the inputs of a content block, replacing all of them. The condition is kept.</summary>
    public class YulsnEmail2CampaignBlockUpdateRequest : YulsnEmail2Request
    {
        /// <summary>The values of the inputs, by input key. An input that is left out is emptied.</summary>
        public Dictionary<string, YulsnEmail2MessageBlockInput> InputValues { get; set; }
    }

    /// <summary>The contacts a content block is shown to. The input values are kept.</summary>
    public class YulsnEmail2CampaignBlockConditionRequest : YulsnEmail2Request
    {
        /// <summary>Null to show the content block to everyone.</summary>
        public YulsnEmail2MessageBlockCondition Condition { get; set; }
    }

    public class YulsnEmail2MessagePreviewUrl
    {
        /// <summary>The public link to the preview, which opens without logging in.</summary>
        public string Url { get; set; }
    }

    /// <summary>A test of a campaign as it is now, to at most 25 recipients. A test is left out of the statistics.</summary>
    public class YulsnEmail2CampaignTestRequest : YulsnEmail2Request
    {
        /// <summary>An address of a contact is sent to the contact, so the content can use its fields.</summary>
        public List<string> Emails { get; set; }
        public List<int> ContactIds { get; set; }
    }

    public class YulsnEmail2MessageTestRecipient
    {
        public string Email { get; set; }
        /// <summary>Null when the address is not of a contact.</summary>
        public int? ContactId { get; set; }
        /// <summary>The dispatch that sends the test.</summary>
        public Guid DispatchId { get; set; }
    }

    /// <summary>
    /// Who a campaign is sent to and when: the contacts of the base segment of its template, in the segments to
    /// include and not in the ones to exclude.
    /// </summary>
    public class YulsnEmail2MessageSchedule
    {
        /// <summary>
        /// Segments enabled for email messages, see <see cref="Client.YulsnClient.GetSegmentsAsync"/>. With none,
        /// the campaign is sent to the whole base segment.
        /// </summary>
        public List<int> IncludeSegments { get; set; }
        /// <summary>Segments enabled for email messages.</summary>
        public List<int> ExcludeSegments { get; set; }
        /// <summary>Sent to this many random contacts of the audience, to all when null.</summary>
        public int? LimitContacts { get; set; }
        /// <summary>The delay between batches, slower than the default speed. The default speed when null.</summary>
        public YulsnDispatchBatchDelaySec? LimitSendingSpeed { get; set; }
        /// <summary>When to start sending. Now when null; up to 30 minutes in the past is accepted.</summary>
        public DateTimeOffset? StartSending { get; set; }
    }

    public class YulsnEmail2MessageScheduleResponse
    {
        /// <summary>Null when none is saved.</summary>
        public YulsnEmail2MessageSchedule Schedule { get; set; }
        /// <summary>The base segment of the template, which the campaign needs to be scheduled.</summary>
        public int? BaseSegmentId { get; set; }
        /// <summary>The number of contacts the schedule reaches before the limit, only when asked for. Null when counting took too long.</summary>
        public int? ContactCount { get; set; }
        /// <summary>The count was asked for but took too long.</summary>
        public bool CountTimedOut { get; set; }
    }

    public class YulsnEmail2CampaignScheduleRequest : YulsnEmail2Request
    {
        /// <summary>
        /// Required when the schedule is saved. When the campaign is scheduled, it is saved over the saved one,
        /// and the saved one is used when left out.
        /// </summary>
        public YulsnEmail2MessageSchedule Schedule { get; set; }
    }

    public class YulsnEmail2CampaignScheduledResponse
    {
        /// <summary>The campaign, in Scheduled status.</summary>
        public YulsnEmail2Campaign Campaign { get; set; }
        /// <summary>The dispatch that sends the campaign.</summary>
        public Guid DispatchId { get; set; }
    }

    /// <summary>Sent as its number, so each value matches the server's.</summary>
    public enum YulsnEmail2FieldFilterOperator
    {
        Equal = 1,
        NotEqual = 2,
        MoreThan = 3,
        MoreThanOrEqual = 4,
        LessThan = 5,
        LessThanOrEqual = 6,
        Null = 7,
        NotNull = 8,
        In = 9,
        NotIn = 10,
        Like = 11
    }

    /// <summary>The kind of value an input of a content block takes.</summary>
    public enum YulsnEmail2BlockInputType
    {
        Text = 1,
        Html = 2,
        Number = 3,
        Link = 4,
        Media = 5,
        Bundle = 6,
        DataObject = 7
    }
}
