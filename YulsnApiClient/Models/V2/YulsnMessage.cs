using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models.V2
{
    public class YulsnSendSinglePushMessageRequest
    {
        public string TriggerId { get; set; }
        public int ContactId { get; set; }
        public DateTimeOffset? Schedule { get; set; }
        public Dictionary<string, object> DynamicValues { get; set; }
        public bool? IsTest { get; set; }
    }

    public class YulsnSendBulkPushMessageRequest
    {
        public string TriggerId { get; set; }
        public List<int> InSegmentIds { get; set; }
        public List<int> NotInSegmentIds { get; set; }
        public int? TakeRandom { get; set; }
        public YulsnDispatchBatchDelaySec? BatchDelay { get; set; }
        public DateTimeOffset? Schedule { get; set; }
        public bool? IsTest { get; set; }
    }

    public class YulsnSendSingleSmsMessageRequest
    {
        string TriggerId { get; set; }
        public int ContactId { get; set; }
        public DateTimeOffset? Schedule { get; set; }
        public Dictionary<string, object> DynamicValues { get; set; }
        public bool? IsTest { get; set; }
    }

    public class YulsnSendBulkSmsMessageRequest
    {
        public string TriggerId { get; set; }
        public List<int> InSegmentIds { get; set; }
        public List<int> NotInSegmentIds { get; set; }
        public int? TakeRandom { get; set; }
        public YulsnDispatchBatchDelaySec? BatchDelay { get; set; }
        public DateTimeOffset? Schedule { get; set; }
        public bool? IsTest { get; set; }
    }

    public class YulsnSendMessageResponse
    {
        public int DispatchId { get; set; }
    }

    public enum YulsnMessageForm { Email, Sms, Push }

    public enum YulsnMessageVersion { V1 = 1, V2 }

    public enum YulsnMessageType { Campaign = 1, Trigger }

    public enum YulsnMessageStatus
    {
        Draft = 10,
        Active = 20,
        Inactive = 30,
        Scheduled = 40,
        Sent = 50,
        Failed = 60,
        Archived = 70
    }

    public enum YulsnMessagePriority
    {
        Normal = 1,
        High
    }

    public enum YulsnDispatchBatchDelaySec
    {
        _1 = 1,
        _2 = 2,
        _3 = 3,
        _4 = 4,
        _5 = 5,
        _6 = 6,
        _9 = 9,
        _12 = 12,
        _18 = 18,
        _30 = 30,
        _40 = 40,
        _60 = 60,
        _90 = 90,
        _120 = 120,
        _180 = 180,
        _300 = 300,
        _450 = 450,
        _600 = 600
    }
}
