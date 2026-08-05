using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models.V2
{
    public class YulsnContactMergeRequest
    {
        public List<string> EventTypes { get; set; }
        public List<string> OrderTypes { get; set; }
        public List<YulsnContactMergeDispatchLogFormVersions> DispatchLogFormVersions { get; set; }
    }

    public class YulsnContactMergeDispatchLogFormVersions
    {
        public YulsnMessageForm Form { get; set; }
        public YulsnMessageVersion Version { get; set; }
    }

    public class YulsnContactMergeResult
    {
        public Dictionary<string, int> MovedEventTypes { get; set; }
        public Dictionary<string, int> MovedOrderTypes { get; set; }
        public Dictionary<string, int> MovedDispatchLogFormVersions { get; set; }
    }

    public class YulsnContactPatchRequest
    {
        /// <summary>
        /// Optional. The Version of the contact being patched, exactly as returned by the last read
        /// of it - an opaque token, not something to parse or compare.
        /// When given it is checked as part of the update itself, so a patch built on an outdated
        /// version is rejected with 409 rather than silently overwriting the other change.
        /// When left out the patch is applied unconditionally and can never return 409 - send it if
        /// you read the contact first, or if the Actions triggered by a contact update assign points
        /// or vouchers, since without it two concurrent patches both apply and both trigger them.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Field name to new value. Both static contact fields and the account's dynamic fields
        /// may be used; an unknown or non-patchable field name is rejected with 400.
        /// Leave a field out to keep it unchanged, pass null to clear it. A blank string (empty,
        /// spaces or tabs) is stored as null as well, so a contact field never holds an empty string.
        /// Note that null differs from the v1 endpoint, where it meant "unchanged" and an empty string
        /// was the only way to clear Email, Phone and PhoneCountryCode.
        /// </summary>
        public Dictionary<string, object> Fields { get; set; }
    }
}
