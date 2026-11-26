using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace YulsnApiClient.Models.V2
{
    /// <summary>A segment, without how it is defined.</summary>
    public class YulsnSegment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>Null when the segment has none.</summary>
        public string TriggerId { get; set; }
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        /// <summary>Offered on email messages: an email campaign can be scheduled to it.</summary>
        public bool OnEmailMessages { get; set; }
        public bool OnSmsMessages { get; set; }
        public bool OnPushMessages { get; set; }
        public DateTimeOffset LastModified { get; set; }
        /// <summary>External segments: the current refresh, the replace whose members the segment has now (see
        /// <c>GetExternalSegmentRefreshAsync</c>). Null for other segments.</summary>
        public int? SegmentRefreshId { get; set; }
    }

    public class YulsnSegmentContactStatus
    {
        public int SegmentId { get; set; }
        public bool Contains { get; set; }
    }

    /// <summary>A new external segment: its members are pushed with <c>ReplaceExternalSegmentMembersAsync</c> and
    /// <c>PatchExternalSegmentMembersAsync</c>, Yulsn never evaluates it. It starts empty.</summary>
    public class YulsnExternalSegmentCreate
    {
        /// <summary>A segment folder.</summary>
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>Optional, unique among the segments.</summary>
        public string TriggerId { get; set; }
        public bool DailyStatistic { get; set; }
        /// <summary>Offered on email messages: an email campaign can be scheduled to it.</summary>
        public bool OnEmailMessages { get; set; }
        public bool OnSmsMessages { get; set; }
        public bool OnPushMessages { get; set; }
    }

    /// <summary>Every contact of an external segment, replacing its members once they are loaded.</summary>
    public class YulsnSegmentMembersReplace
    {
        /// <summary>How the contacts are identified, one type per request.</summary>
        public YulsnSegmentIdentifierType IdentifierType { get; set; }
        /// <summary>Every contact of the segment; an empty list empties it.</summary>
        public List<string> Contacts { get; set; }
    }

    /// <summary>Contacts to add to and remove from an external segment, applied together.</summary>
    public class YulsnSegmentMembersPatch
    {
        /// <summary>How the contacts in Add and Remove are identified, one type per request.</summary>
        public YulsnSegmentIdentifierType IdentifierType { get; set; }
        /// <summary>Contacts to add; members already in the segment are skipped.</summary>
        public List<string> Add { get; set; }
        /// <summary>Contacts to remove; contacts that are not members are skipped.</summary>
        public List<string> Remove { get; set; }
    }

    /// <summary>One change to the members of an external segment: a replace or a patch.</summary>
    public class YulsnSegmentRefresh
    {
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public YulsnSegmentRefreshKind Kind { get; set; }
        public YulsnSegmentRefreshStatus Status { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        /// <summary>When the refresh completed or failed, null while it runs.</summary>
        public DateTimeOffset? CompletedAt { get; set; }
        /// <summary>Contacts that became members.</summary>
        public int RowsAdded { get; set; }
        /// <summary>Contacts that stopped being members.</summary>
        public int RowsRemoved { get; set; }
        /// <summary>Distinct identifiers that are malformed or match no contact in the account.</summary>
        public int RowsUnmatched { get; set; }
        /// <summary>Why the refresh failed, null otherwise.</summary>
        public string Error { get; set; }
    }

    /// <summary>How the contacts pushed to an external segment are identified.</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum YulsnSegmentIdentifierType
    {
        /// <summary>Yulsn's contact id.</summary>
        ContactId = 1,
        /// <summary>The contact's Secret as is. Prefer SecretSha256 outside Yulsn: the Secret opens the contact's profile.</summary>
        Secret,
        /// <summary>The hex SHA-256 (64 characters) of the UTF-8 bytes of the contact's Secret, like the Matomo userId.</summary>
        SecretSha256,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum YulsnSegmentRefreshKind
    {
        /// <summary>The whole list, loaded in the background.</summary>
        Replace = 1,
        /// <summary>Contacts added and removed.</summary>
        Patch,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum YulsnSegmentRefreshStatus
    {
        Running = 1,
        Completed,
        Failed,
    }
}
