using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    /// <summary>
    /// Email campaigns, built and sent the way the admin does it: settings, content blocks, preview and test, then
    /// schedule. A trigger message is not a campaign, so its id is not found. Every write takes the id of the
    /// administrator it is made by.
    /// </summary>
    partial class YulsnClient
    {
        private string email2CampaignsUrl => $"api/v2/{AccountId}/Messages/Email/Campaigns";

        /// <summary>
        /// Creates a campaign in Draft status, without content.
        /// </summary>
        /// <returns>The campaign as it is saved.</returns>
        /// <exception cref="ProblemDetails">A setting is missing or invalid, like the name, the subject or the
        /// template, or the account has no frontdoor domain.</exception>
        public Task<YulsnEmail2Campaign> CreateEmail2CampaignAsync(YulsnEmail2CampaignCreateRequest request) =>
            SendAsync<YulsnEmail2Campaign>(HttpMethod.Post, email2CampaignsUrl, request, YulsnApiVersion.V2);

        /// <summary>
        /// Returns the settings of a campaign.
        /// </summary>
        /// <returns>The campaign, null when no campaign has the id.</returns>
        public Task<YulsnEmail2Campaign> GetEmail2CampaignAsync(int campaignId) =>
            SendAsync<YulsnEmail2Campaign>(HttpMethod.Get, $"{email2CampaignsUrl}/{campaignId}", YulsnApiVersion.V2);

        /// <summary>
        /// Replaces all the settings of a campaign in Draft status.
        /// </summary>
        /// <returns>The campaign as it is saved and the content blocks removed for the template, null when no
        /// campaign has the id.</returns>
        /// <exception cref="ProblemDetails">A setting is invalid (400), the campaign is not a draft, or the template
        /// does not allow some of its content blocks and <see cref="YulsnEmail2CampaignUpdateRequest.RemoveIncompatibleBlocks"/>
        /// is not set (409).</exception>
        public Task<YulsnEmail2CampaignUpdateResponse> UpdateEmail2CampaignAsync(int campaignId, YulsnEmail2CampaignUpdateRequest request) =>
            SendAsync<YulsnEmail2CampaignUpdateResponse>(HttpMethod.Put, $"{email2CampaignsUrl}/{campaignId}", request, YulsnApiVersion.V2);

        /// <summary>
        /// Archives a campaign in Draft or Sent status: it is taken off the listings and its data is kept.
        /// </summary>
        /// <param name="adminId">The administrator it is archived by.</param>
        /// <returns>True when it is archived (or already was), false when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">The campaign is scheduled and must be cancelled first (409), or the
        /// administrator is unknown (400).</exception>
        public async Task<bool> ArchiveEmail2CampaignAsync(int campaignId, int adminId) =>
            await SendAsync<string>(HttpMethod.Delete, $"{email2CampaignsUrl}/{campaignId}?{nameof(adminId)}={adminId}", YulsnApiVersion.V2).ConfigureAwait(false) != null;

        /// <summary>
        /// Returns the email templates a campaign can be created or updated with, by name.
        /// </summary>
        public Task<List<YulsnEmail2MessageTemplate>> GetEmail2CampaignTemplatesAsync() =>
            SendAsync<List<YulsnEmail2MessageTemplate>>(HttpMethod.Get, $"{email2CampaignsUrl}/Templates", YulsnApiVersion.V2);

        /// <summary>
        /// Returns the content blocks of a campaign in order, with their input values and condition.
        /// </summary>
        /// <returns>The content blocks, null when no campaign has the id.</returns>
        public Task<List<YulsnEmail2MessageBlock>> GetEmail2CampaignBlocksAsync(int campaignId) =>
            SendAsync<List<YulsnEmail2MessageBlock>>(HttpMethod.Get, $"{email2CampaignsUrl}/{campaignId}/Blocks", YulsnApiVersion.V2);

        /// <summary>
        /// Returns the content blocks that can be added to a campaign - not hidden, and allowed on its template -
        /// with their inputs.
        /// </summary>
        /// <returns>The content blocks, null when no campaign has the id.</returns>
        public Task<List<YulsnEmail2Block>> GetAvailableEmail2CampaignBlocksAsync(int campaignId) =>
            SendAsync<List<YulsnEmail2Block>>(HttpMethod.Get, $"{email2CampaignsUrl}/{campaignId}/Blocks/Available", YulsnApiVersion.V2);

        /// <summary>
        /// Adds a content block to a campaign in Draft status, at the end or after
        /// <see cref="YulsnEmail2CampaignBlockAddRequest.AfterId"/>.
        /// </summary>
        /// <returns>The content block as it is saved, null when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">The block cannot be added, an input value or the condition is invalid
        /// (400), or the campaign is not a draft (409).</exception>
        public Task<YulsnEmail2MessageBlock> AddEmail2CampaignBlockAsync(int campaignId, YulsnEmail2CampaignBlockAddRequest request) =>
            SendAsync<YulsnEmail2MessageBlock>(HttpMethod.Post, $"{email2CampaignsUrl}/{campaignId}/Blocks", request, YulsnApiVersion.V2);

        /// <summary>
        /// Replaces the input values of a content block of a campaign in Draft status. The condition is kept.
        /// </summary>
        /// <param name="blockId">Id of the content block of the campaign.</param>
        /// <returns>The content block as it is saved, null when the campaign has no content block with the id.</returns>
        /// <exception cref="ProblemDetails">An input value is invalid (400), or the campaign is not a draft (409).</exception>
        public Task<YulsnEmail2MessageBlock> UpdateEmail2CampaignBlockAsync(int campaignId, int blockId, YulsnEmail2CampaignBlockUpdateRequest request) =>
            SendAsync<YulsnEmail2MessageBlock>(HttpMethod.Put, $"{email2CampaignsUrl}/{campaignId}/Blocks/{blockId}", request, YulsnApiVersion.V2);

        /// <summary>
        /// Sets or removes the condition of a content block of a campaign in Draft status: the contacts it is shown
        /// to. The input values are kept.
        /// </summary>
        /// <param name="blockId">Id of the content block of the campaign.</param>
        /// <returns>The content block as it is saved, null when the campaign has no content block with the id.</returns>
        /// <exception cref="ProblemDetails">The condition is invalid (400), or the campaign is not a draft (409).</exception>
        public Task<YulsnEmail2MessageBlock> SetEmail2CampaignBlockConditionAsync(int campaignId, int blockId, YulsnEmail2CampaignBlockConditionRequest request) =>
            SendAsync<YulsnEmail2MessageBlock>(HttpMethod.Put, $"{email2CampaignsUrl}/{campaignId}/Blocks/{blockId}/Condition", request, YulsnApiVersion.V2);

        /// <summary>
        /// Removes a content block from a campaign in Draft status; the blocks after it move up.
        /// </summary>
        /// <param name="blockId">Id of the content block of the campaign.</param>
        /// <param name="adminId">The administrator it is removed by.</param>
        /// <returns>True when it is removed, false when the campaign has no content block with the id.</returns>
        /// <exception cref="ProblemDetails">The campaign is not a draft (409), or the administrator is unknown (400).</exception>
        public async Task<bool> DeleteEmail2CampaignBlockAsync(int campaignId, int blockId, int adminId) =>
            await SendAsync<string>(HttpMethod.Delete, $"{email2CampaignsUrl}/{campaignId}/Blocks/{blockId}?{nameof(adminId)}={adminId}", YulsnApiVersion.V2).ConfigureAwait(false) != null;

        /// <summary>
        /// Returns the public link to the preview of a campaign, which opens without logging in.
        /// </summary>
        /// <param name="email">The preview as the contact with the address gets it. An address that is not of a
        /// contact, or no receiver, gives the public preview.</param>
        /// <param name="contactId">The preview as the contact gets it.</param>
        /// <returns>The link, null when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">The contact is unknown, or the account has no frontdoor domain (400).</exception>
        public Task<YulsnEmail2MessagePreviewUrl> GetEmail2CampaignPreviewUrlAsync(int campaignId, string email = null, int? contactId = null)
        {
            List<string> query = new List<string>();
            if (email != null)
                query.Add($"{nameof(email)}={Uri.EscapeDataString(email)}");
            if (contactId != null)
                query.Add($"{nameof(contactId)}={contactId}");

            return SendAsync<YulsnEmail2MessagePreviewUrl>(HttpMethod.Get,
                $"{email2CampaignsUrl}/{campaignId}/PreviewUrl{(query.Count != 0 ? "?" + string.Join("&", query) : null)}",
                YulsnApiVersion.V2);
        }

        /// <summary>
        /// Sends a test of a campaign in Draft or Sent status, as it is now, to at most 25 email addresses and
        /// contacts. It uses the campaign's dynamic dictionary and is left out of the statistics.
        /// </summary>
        /// <returns>The recipients with the dispatch that sends each one, null when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">A recipient is invalid or there are too many (400), or the campaign
        /// cannot be tested in its status (409).</exception>
        public Task<List<YulsnEmail2MessageTestRecipient>> SendEmail2CampaignTestAsync(int campaignId, YulsnEmail2CampaignTestRequest request) =>
            SendAsync<List<YulsnEmail2MessageTestRecipient>>(HttpMethod.Post, $"{email2CampaignsUrl}/{campaignId}/Test", request, YulsnApiVersion.V2);

        /// <summary>
        /// Returns the schedule saved on a campaign and the base segment of its template.
        /// </summary>
        /// <param name="includeCount">Also counts the contacts the schedule reaches. Counting a large segment can
        /// take a while.</param>
        /// <returns>The schedule, null when no campaign has the id.</returns>
        public Task<YulsnEmail2MessageScheduleResponse> GetEmail2CampaignScheduleAsync(int campaignId, bool includeCount = false) =>
            SendAsync<YulsnEmail2MessageScheduleResponse>(HttpMethod.Get,
                $"{email2CampaignsUrl}/{campaignId}/Schedule{(includeCount ? $"?{nameof(includeCount)}=true" : null)}",
                YulsnApiVersion.V2);

        /// <summary>
        /// Saves a schedule on a campaign in Draft status, over the one saved before, without scheduling it.
        /// </summary>
        /// <returns>The saved schedule, null when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">A segment is unknown or not enabled for email messages, the limit or
        /// the sending speed is invalid (400), or the campaign is not a draft (409).</exception>
        public Task<YulsnEmail2MessageScheduleResponse> SetEmail2CampaignScheduleAsync(int campaignId, YulsnEmail2CampaignScheduleRequest request) =>
            SendAsync<YulsnEmail2MessageScheduleResponse>(HttpMethod.Put, $"{email2CampaignsUrl}/{campaignId}/Schedule", request, YulsnApiVersion.V2);

        /// <summary>
        /// Schedules a campaign in Draft status: creates the dispatch that sends it and sets it to Scheduled. A
        /// schedule in the request is saved over the saved one first; without one, the saved schedule is used.
        /// </summary>
        /// <returns>The scheduled campaign and its dispatch, null when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">The schedule is invalid, the template has no base segment or the
        /// campaign has no content (400), or the campaign is not a draft (409).</exception>
        public Task<YulsnEmail2CampaignScheduledResponse> ScheduleEmail2CampaignAsync(int campaignId, YulsnEmail2CampaignScheduleRequest request) =>
            SendAsync<YulsnEmail2CampaignScheduledResponse>(HttpMethod.Post, $"{email2CampaignsUrl}/{campaignId}/Schedule", request, YulsnApiVersion.V2);

        /// <summary>
        /// Cancels a scheduled campaign that has not started sending: it is set back to Draft, keeping its schedule.
        /// </summary>
        /// <param name="adminId">The administrator it is cancelled by.</param>
        /// <returns>The campaign, null when no campaign has the id.</returns>
        /// <exception cref="ProblemDetails">The campaign is not scheduled or is already being sent (409).</exception>
        public Task<YulsnEmail2Campaign> CancelEmail2CampaignScheduleAsync(int campaignId, int adminId) =>
            SendAsync<YulsnEmail2Campaign>(HttpMethod.Post, $"{email2CampaignsUrl}/{campaignId}/Schedule/Cancel",
                new YulsnEmail2Request { AdminId = adminId }, YulsnApiVersion.V2);
    }
}
