using System;
using System.Collections.Generic;

namespace YulsnApiClient.Models.V1
{
    public class YulsnCreateEmail2DispatchDto
    {
        /// <summary>Trigger id of email2 message</summary>
        public string TriggerId { get; set; }
        /// <summary>DateTimeOffset of dispatch to be sent out. If null, set to UtcNow.</summary>
        public DateTimeOffset? Schedule { get; set; }
        /// <summary>Indicates if dispatch should be created as test.</summary>
        public bool? IsTest { get; set; }
        /// <summary>Delay between batches in seconds.</summary>
        public V2.YulsnDispatchBatchDelaySec? BatchDelaySec { get; set; }
        /// <summary>One (and only one) of the following fields must be defined: Contacts, Emails or Segments.</summary>
        public YulsnEmail2ScopeDto Scope { get; set; }
    }

    public class YulsnEmail2ScopeDto
    {
        public Dictionary<int, YulsnEmail2ContactSettingsDto> Contacts { get; set; }
        public Dictionary<string, YulsnEmail2ContactSettingsDto> Emails { get; set; }
        public YulsnEmail2SegmentSettingsDto Segments { get; set; }
    }

    public class YulsnEmail2ContactSettingsDto
    {
        public Dictionary<string, object> Dynamic { get; set; }
    }

    public class YulsnEmail2SegmentSettingsDto
    {
        public List<int> InSegmentIds { get; set; }
        public List<int> NotInSegmentIds { get; set; }
        public int? TakeRandom { get; set; }
        public Dictionary<string, object> Dynamic { get; set; }
    }
}
