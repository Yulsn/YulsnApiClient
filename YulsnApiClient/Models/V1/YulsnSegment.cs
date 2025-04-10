using System.Collections.Generic;

namespace YulsnApiClient.Models.V1
{
    public enum YulsnSegmentType : int
    {
        VisualBuilder = 1,
        Sql,
        List,
        SegmentTemplate
    }

    public class YulsnReadSegmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FolderId { get; set; }
        public YulsnSegmentType Type { get; set; }
        public string Description { get; set; }
    }

    public class YulsnPostSegmentTemplateDto
    {
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DailyStatistic { get; set; }
        public bool OnEmailMessages { get; set; }
        public bool OnSmsMessages { get; set; }
        public bool OnPushMessages { get; set; }
        public int TemplateId { get; set; }
        public Dictionary<string, string> Values { get; set; }
    }
}
