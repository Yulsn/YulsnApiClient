using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnCreateEmailBuilderCampaign
    {
        /// <summary>
        /// The type of message
        /// </summary>
        public YulsnEmailBuilderCampaignType Type { get; set; }
        /// <summary>
        /// The campaigns name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The subject of the email campaign
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The emails preheader
        /// </summary>
        public string Preheader { get; set; }
        /// <summary>
        /// The template used for the email campaign
        /// </summary>
        public int EmailTemplateId { get; set; }
        /// <summary>
        /// Dynamic dictionary - only for Trigger messages
        /// </summary>
        public Dictionary<string, object> Dynamic { get; set; }
    }

    public class YulsnReadEmailBuilderCampaign : YulsnCreateEmailBuilderCampaign
    {
        /// <summary>
        /// The ID of the Email Builder Campaign
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the Email Campaign. This is only used for Trigger and Test mails.
        /// </summary>
        public int EmailCampaignId { get; set; }
    }

    public class YulsnCreateEmailBuilderCampaignBlock
    {
        /// <summary>
        /// Id of the email builder block
        /// </summary>
        public int EmailBuilderBlockId { get; set; }
        /// <summary>
        /// Input values used by the block
        /// </summary>
        public Dictionary<string, YulsnEmailBuilderCampaignBlockInputValue> InputValues { get; set; }

        public YulsnEmailBuilderCampaignBlockSettings Settings { get; set; }
    }

    public class YulsnReadEmailBuilderCampaignBlock : YulsnCreateEmailBuilderCampaignBlock
    {
        /// <summary>
        /// The ID of the email builder campaign block
        /// </summary>
        public int Id { get; set; }
    }

    public class YulsnCreateScheduleEmailBuilderCampaign
    {
        /// <summary>
        /// If you want to narrow the number of receivers The contacts must be in of the specified segments
        /// </summary>
        public List<int> IncludeSegments { get; set; }
        /// <summary>
        /// If you want to narrow the number of receivers The contacts must NOT be in of the specified segments
        /// </summary>
        public List<int> ExcludeSegments { get; set; }
        /// <summary>
        /// Only send to the specified number of contacts NULL equals no limit
        /// </summary>
        public int? LimitContacts { get; set; }
        /// <summary>
        /// Limit sending the sending speed
        /// NULL equals no limit
        /// </summary>
        public YulsnDispatchBatchSpeed? LimitSendingSpeed { get; set; }
        /// <summary>
        /// When should the campaign be sent NULL means now
        /// </summary>
        public DateTimeOffset? StartSending { get; set; }

    }

    public class YulsnEmailBuilderCampaignBlockInputValue
    {
        public string Value { get; set; }
    }

    public class YulsnEmailBuilderCampaignBlockSettings
    {
        public YulsnSearchFileldDto Condition { get; set; }
    }

    public enum YulsnEmailBuilderCampaignType
    {
        Newsletter = 0,
        Transactional = 1
    }

    public enum YulsnDispatchBatchSpeed
    {
        /// <summary>
        /// 90.000 per hour
        /// </summary>
        _20 = 20,
        /// <summary>
        /// 60.000 per hour
        /// </summary>
        _30 = 30,
        /// <summary>
        /// 30.000 per hour
        /// </summary>
        _60 = 60,
        /// <summary>
        /// 15.000 per hour
        /// </summary>
        _120 = 120,
        /// <summary>
        /// 6.000 per hour
        /// </summary>
        _300 = 300,
        /// <summary>
        /// 3.000 per hour
        /// </summary>
        _600 = 600
    }
}
