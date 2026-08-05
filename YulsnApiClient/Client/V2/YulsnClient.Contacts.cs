using System;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>netstandard2.0 has no HttpMethod.Patch - that arrived in netstandard2.1.</summary>
        private static readonly HttpMethod patchMethod = new HttpMethod("PATCH");

        /// <summary>
        /// Will merge data from source contact to target contact.
        /// </summary>
        /// <param name="sourceId">Id of Source contact</param>
        /// <param name="targetId">Id of target contact</param>
        /// <returns></returns>
        public Task<YulsnContactMergeResult> MergeContactsAsync(int sourceId, int targetId, YulsnContactMergeRequest body) =>
            SendAsync<YulsnContactMergeResult>(HttpMethod.Post, $"api/v2/{AccountId}/Contacts/{sourceId}/Merge/{targetId}", body, YulsnApiVersion.V2);

        /// <summary>
        /// Patches the given fields on a contact, found by its id.
        /// Send <see cref="YulsnContactPatchRequest.Version"/> to have it checked as part of the update, so a
        /// patch built on an outdated read is refused instead of overwriting whoever got there first.
        /// </summary>
        /// <param name="contactId">Id of the contact to patch</param>
        /// <returns>True when the contact was patched, false when no contact has that id.</returns>
        /// <exception cref="ProblemDetails">
        /// The Version no longer matches, the new Email or Phone is already used by another contact, or the
        /// request was rejected - an unknown field name, a field that cannot be cleared, or a store or
        /// company that does not exist.
        /// </exception>
        public Task<bool> PatchContactAsync(int contactId, YulsnContactPatchRequest body) =>
            patchContactAsync($"api/v2/{AccountId}/Contacts/{contactId}", body);

        /// <summary>
        /// Patches the given fields on a contact, found by its secret. Identical to
        /// <see cref="PatchContactAsync(int, YulsnContactPatchRequest)"/> in every other way.
        /// </summary>
        /// <param name="secret">Secret of the contact to patch</param>
        /// <returns>True when the contact was patched, false when no contact has that secret.</returns>
        /// <exception cref="ProblemDetails">
        /// The Version no longer matches, the new Email or Phone is already used by another contact, or the
        /// request was rejected - an unknown field name, a field that cannot be cleared, or a store or
        /// company that does not exist.
        /// </exception>
        public Task<bool> PatchContactBySecretAsync(string secret, YulsnContactPatchRequest body) =>
            patchContactAsync($"api/v2/{AccountId}/Contacts/Secret/{Uri.EscapeDataString(secret ?? string.Empty)}", body);

        /// <summary>
        /// The endpoint answers 200 with an empty body, so there is nothing to deserialize. Asking for a
        /// string skips deserialization entirely and still separates the two outcomes: SendAsync returns null
        /// on a 404 without throwing, while a 200 hands back the (empty) body. Every other non-success status
        /// has already become a ProblemDetails by this point.
        /// </summary>
        private async Task<bool> patchContactAsync(string url, YulsnContactPatchRequest body) =>
            await SendAsync<string>(patchMethod, url, body, YulsnApiVersion.V2).ConfigureAwait(false) != null;
    }
}
