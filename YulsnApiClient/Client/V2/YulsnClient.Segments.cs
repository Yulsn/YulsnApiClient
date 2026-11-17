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

    }
}