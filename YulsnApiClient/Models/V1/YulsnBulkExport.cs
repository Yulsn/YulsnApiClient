using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models.V1
{
    public class YulsnExportSettings
    {
        /// <summary>If you want to make a BulkExport from a DynamicTable the Type must be NULL</summary>
        public ExportType? Type { get; set; }
        /// <summary>If you want to make a BulkExport which is not from a DynamicTable the DynamicTableId must be NULL</summary>
        public int? DynamicTableId { get; set; }
        public string ColumnDelimiter { get; set; }
        public string RowDelimiter { get; set; }
        public DateTimeOffset? CreatedDateTimeFrom { get; set; }
        public DateTimeOffset? CreatedDateTimeTo { get; set; }

        /// <summary>
        /// Optional LastModified-based window for incremental sync. Only honored by exporters whose
        /// underlying table has a real LastModified column (Contacts, PointSums, PushTokens, SmsReceivers).
        /// For tables that only have Created, use CreatedDateTimeFrom/To instead.
        /// </summary>
        public DateTimeOffset? LastModifiedFrom { get; set; }
        public DateTimeOffset? LastModifiedTo { get; set; }

        /// <summary>
        /// Optional integer-id watermark for insert-only exports. Exporters that honor it add
        /// <c>AND Id &gt; @LastId</c>. Use this for tables like ContactEvents / Email2Interactions
        /// where a strict per-row cursor is preferable to a date window.
        /// </summary>
        public int? LastId { get; set; }

        public string ItemType { get; set; }
        public int? SegmentId { get; set; }
        public List<string> Fields { get; set; }
        public string SuffixFileName { get; set; }
        public string TimeZoneId { get; set; }
        public bool UseCompression { get; set; }

        /// <summary>
        /// Output format. Defaults to Csv. Parquet is not supported for ContactExport-scoped exports.
        /// </summary>
        public ExportFormat? Format { get; set; }
    }

    public class YulsnExportItem
    {
        public string Name { get; set; }
        public Uri DownloadUrl { get; set; }
        public long SizeInBytes { get; set; }
        public string Type { get; set; }
        public DateTimeOffset? CreatedDateTimeFrom { get; set; }
        public DateTimeOffset? CreatedDateTimeTo { get; set; }
        public string ItemType { get; set; }
        public int? RowCount { get; set; }
        public int? SegmentId { get; set; }
        public string Right { get; set; }
        public string TimeZoneId { get; set; }
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
        EmailCampaignClicks,
        ContactCompanies,
        VoucherCodes,
        PermissionLogs,
        Email2Dispatches,
        Email2Receivers,
        Email2Interactions,
        Email2Bounces,
        SmsDispatches,
        SmsReceivers,
        PushDispatches,
        PushReceivers,
        PushTokens,
        ContactPermissionSources,
        VoucherGroups,
        Vouchers,
        VoucherAssignments
    }

    public enum ExportFormat
    {
        Csv = 0,
        Parquet = 1
    }
}
