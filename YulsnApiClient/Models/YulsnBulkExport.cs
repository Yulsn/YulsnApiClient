using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnExportSettings
    {
        public ExportType Type { get; set; }
        public string ColumnDelimiter { get; set; }
        public string RowDelimiter { get; set; }
        public DateTimeOffset? CreatedDateTimeFrom { get; set; }
        public DateTimeOffset? CreatedDateTimeTo { get; set; }
        public string ItemType { get; set; }
        public int? SegmentId { get; set; }
        public List<string> Fields { get; set; }
        public string SuffixFileName { get; set; }
    }

    public class YulsnExportItem
    {
        public string Name { get; set; }
        public Uri DownloadUrl { get; set; }
        public int SizeInBytes { get; set; }
        public string Type { get; set; }
        public DateTimeOffset? CreatedDateTimeFrom { get; set; }
        public DateTimeOffset? CreatedDateTimeTo { get; set; }
        public string ItemType { get; set; }
        public int RowCount { get; set; }
        public int? SegmentId { get; set; }
        public string Right { get; set; }
    }

    public enum ExportType
    {
        Contacts = 0,
        Stores,
        Points,
        PointSums,
        Orders,
        OrderLines,
        Events,
        EmailCampaignClicks
    }
}
