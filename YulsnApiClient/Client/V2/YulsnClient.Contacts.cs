using System;
using System.Collections.Generic;
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
        /// Patches the given fields on a contact, found by its id, checking <paramref name="version"/> as
        /// part of the update so a patch built on an outdated read is refused instead of overwriting
        /// whoever got there first.
        /// </summary>
        /// <param name="contactId">Id of the contact to patch</param>
        /// <param name="fields">
        /// Field name to new value. Both static contact fields and the account's dynamic fields may be
        /// used; an unknown or non-patchable field name is rejected with 400. Leave a field out to keep
        /// it unchanged, pass null to clear it.
        /// </param>
        /// <param name="version">
        /// The Version of the contact, exactly as returned by the read this patch was built on. Checked
        /// as part of the update, so an outdated one is rejected with 409.
        /// </param>
        /// <returns>True when the contact was patched, false when no contact has that id.</returns>
        /// <exception cref="ProblemDetails">
        /// The Version no longer matches, the new Email or Phone is already used by another contact, or the
        /// request was rejected - an unknown field name, a field that cannot be cleared, or a store or
        /// company that does not exist.
        /// </exception>
        public Task<bool> UpdateContactAsync(int contactId, Dictionary<string, object> fields, string version) =>
            updateContactAsync($"api/v2/{AccountId}/Contacts/{contactId}", fields, version);

        /// <summary>
        /// Patches the given fields on a contact, found by its id, without checking a Version - the patch
        /// is applied unconditionally and can never return 409. Prefer
        /// <see cref="UpdateContactAsync(int, Dictionary{string, object}, string)"/> when you read the
        /// contact first, or when the Actions triggered by a contact update assign points or vouchers,
        /// since without a Version two concurrent patches both apply and both trigger them.
        /// </summary>
        /// <param name="contactId">Id of the contact to patch</param>
        /// <param name="fields">Field name to new value, as in the Version-checked overload.</param>
        /// <returns>True when the contact was patched, false when no contact has that id.</returns>
        /// <exception cref="ProblemDetails">
        /// The new Email or Phone is already used by another contact, or the request was rejected - an
        /// unknown field name, a field that cannot be cleared, or a store or company that does not exist.
        /// </exception>
        public Task<bool> UpdateContactAsync(int contactId, Dictionary<string, object> fields) =>
            updateContactAsync($"api/v2/{AccountId}/Contacts/{contactId}", fields, null);

        /// <summary>
        /// Patches the given fields on a contact, found by its secret. Identical to
        /// <see cref="UpdateContactAsync(int, Dictionary{string, object}, string)"/> in every other way.
        /// </summary>
        /// <param name="secret">Secret of the contact to patch</param>
        /// <param name="fields">Field name to new value, as in the by-id overload.</param>
        /// <param name="version">The Version of the contact, as returned by the read this patch was built on.</param>
        /// <returns>True when the contact was patched, false when no contact has that secret.</returns>
        /// <exception cref="ProblemDetails">
        /// The Version no longer matches, the new Email or Phone is already used by another contact, or the
        /// request was rejected - an unknown field name, a field that cannot be cleared, or a store or
        /// company that does not exist.
        /// </exception>
        public Task<bool> UpdateContactAsync(string secret, Dictionary<string, object> fields, string version) =>
            updateContactAsync($"api/v2/{AccountId}/Contacts/Secret/{Uri.EscapeDataString(secret ?? string.Empty)}", fields, version);

        /// <summary>
        /// Patches the given fields on a contact, found by its secret, without checking a Version.
        /// Identical to <see cref="UpdateContactAsync(int, Dictionary{string, object})"/> in every other way.
        /// </summary>
        /// <param name="secret">Secret of the contact to patch</param>
        /// <param name="fields">Field name to new value, as in the by-id overload.</param>
        /// <returns>True when the contact was patched, false when no contact has that secret.</returns>
        /// <exception cref="ProblemDetails">
        /// The new Email or Phone is already used by another contact, or the request was rejected - an
        /// unknown field name, a field that cannot be cleared, or a store or company that does not exist.
        /// </exception>
        public Task<bool> UpdateContactAsync(string secret, Dictionary<string, object> fields) =>
            updateContactAsync($"api/v2/{AccountId}/Contacts/Secret/{Uri.EscapeDataString(secret ?? string.Empty)}", fields, null);

        /// <summary>
        /// The endpoint answers 200 with an empty body, so there is nothing to deserialize. Asking for a
        /// string skips deserialization entirely and still separates the two outcomes: SendAsync returns null
        /// on a 404 without throwing, while a 200 hands back the (empty) body. Every other non-success status
        /// has already become a ProblemDetails by this point.
        /// </summary>
        private async Task<bool> updateContactAsync(string url, Dictionary<string, object> fields, string version) =>
            await SendAsync<string>(patchMethod, url, new YulsnContactPatchRequest { Fields = fields, Version = version }, YulsnApiVersion.V2)
                .ConfigureAwait(false) != null;
    }
}
