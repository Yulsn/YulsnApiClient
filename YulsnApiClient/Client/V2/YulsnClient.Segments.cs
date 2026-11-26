using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Returns the segments by name, with what says what they are and not how they are defined.
        /// </summary>
        /// <param name="messageForm">Only the segments offered on that kind of message, like
        /// <see cref="YulsnMessageForm.Email"/> for the segments an email campaign can be scheduled to. All segments
        /// when null.</param>
        public Task<List<YulsnSegment>> GetSegmentsAsync(YulsnMessageForm? messageForm = null) =>
            SendAsync<List<YulsnSegment>>(
                HttpMethod.Get,
                $"api/v2/{AccountId}/Segments{(messageForm != null ? $"?{nameof(messageForm)}={messageForm}" : null)}",
                YulsnApiVersion.V2);

        /// <summary>
        /// Will get contact status for each segment.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="segmentIds">Array of segment ids</param>
        /// <param name="timeoutSec">Timeout in seconds. 30 is max</param>
        /// <returns></returns>
        public Task<YulsnSegmentContactStatus[]> GetSegmentContactStatusAsync(int contactId, IEnumerable<int> segmentIds, int? timeoutSec = null) =>
            SendAsync<YulsnSegmentContactStatus[]>(
                HttpMethod.Get,
                $"api/v2/{AccountId}/Segments/GetContactStatus/{contactId}/{string.Join(",", segmentIds)}{(timeoutSec != null ? $"?{nameof(timeoutSec)}={timeoutSec}" : null)}",
                nameof(GetSegmentContactStatusAsync),
                YulsnApiVersion.V2);

        /// <summary>
        /// Will get contact status for a segment.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="segmentId">Id of the segment</param>
        /// <param name="timeoutSec">Timeout in seconds. 30 is max</param>
        /// <returns></returns>
        public async Task<YulsnSegmentContactStatus> GetSegmentContactStatusAsync(int contactId, int segmentId, int? timeoutSec = null)
        {
            return (await GetSegmentContactStatusAsync(contactId, new int[] { segmentId }, timeoutSec).ConfigureAwait(false)).FirstOrDefault();
        }

        /// <summary>
        /// Creates an external segment: its members are pushed with <see cref="ReplaceExternalSegmentMembersAsync"/>
        /// and <see cref="PatchExternalSegmentMembersAsync"/>, Yulsn never evaluates it. It starts empty.
        /// Permission: WriteSegments.
        /// </summary>
        public Task<YulsnSegment> CreateExternalSegmentAsync(YulsnExternalSegmentCreate model) =>
            SendAsync<YulsnSegment>(
                HttpMethod.Post,
                $"api/v2/{AccountId}/Segments/External",
                model,
                YulsnApiVersion.V2);

        /// <summary>
        /// Replaces every member of an external segment. Accepted at once and loaded in the background: the members
        /// change when the returned refresh is Completed, see <see cref="GetExternalSegmentRefreshAsync"/>. One replace
        /// runs per segment at a time, and patches are rejected while it runs (409). Max 100 MB.
        /// Permission: WriteSegments.
        /// </summary>
        public Task<YulsnSegmentRefresh> ReplaceExternalSegmentMembersAsync(int segmentId, YulsnSegmentMembersReplace model) =>
            SendAsync<YulsnSegmentRefresh>(
                HttpMethod.Put,
                $"api/v2/{AccountId}/Segments/External/{segmentId}/Members",
                model,
                YulsnApiVersion.V2);

        /// <summary>
        /// Adds and removes members of an external segment, applied together when the request returns. The returned
        /// refresh is Failed when a replace completed in between; nothing is changed then. Rejected while a replace
        /// runs (409). Max 100 MB.
        /// Permission: WriteSegments.
        /// </summary>
        public Task<YulsnSegmentRefresh> PatchExternalSegmentMembersAsync(int segmentId, YulsnSegmentMembersPatch model) =>
            SendAsync<YulsnSegmentRefresh>(
                new HttpMethod("PATCH"), // HttpMethod.Patch is not in netstandard2.0
                $"api/v2/{AccountId}/Segments/External/{segmentId}/Members",
                model,
                YulsnApiVersion.V2);

        /// <summary>
        /// Returns a replace or a patch of an external segment's members, with its status and row counts. Null when
        /// the refresh is not in the account.
        /// Permission: ReadSegments.
        /// </summary>
        public Task<YulsnSegmentRefresh> GetExternalSegmentRefreshAsync(int refreshId) =>
            SendAsync<YulsnSegmentRefresh>(
                HttpMethod.Get,
                $"api/v2/{AccountId}/Segments/External/Refreshes/{refreshId}",
                YulsnApiVersion.V2);

    }
}