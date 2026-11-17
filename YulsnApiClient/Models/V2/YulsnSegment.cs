using System;

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
    }

    public class YulsnSegmentContactStatus
    {
        public int SegmentId { get; set; }
        public bool Contains { get; set; }
    }
}
