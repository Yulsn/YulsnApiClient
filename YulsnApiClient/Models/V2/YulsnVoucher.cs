using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models.V2
{
    public class YulsnVoucherGroup
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? WinnerChance { get; set; }
    }

    public class YulsnVoucher
    {
        public int VoucherId { get; set; }
        public int? AssignWeight { get; set; }
        public int? AssignLimit { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VoucherRedeemTime? RedeemTime { get; set; }
        public int? RedeemHours { get; set; }
        public DateTimeOffset? RedeemCustom { get; set; }
    }

    public class YulsnVoucherCodeImportRequest
    {
        public string FileUrl { get; set; }
        public string ColumnDelimiter { get; set; }
        public string Encoding { get; set; }
        public bool FirstLineIsHeader { get; set; }
        public int VoucherId { get; set; }
        public List<YulsnVoucherCodeImportField> Fields { get; set; }
    }

    public class YulsnVoucherCodeImportField
    {
        public string CsvField { get; set; }
        public string LoyaltiiField { get; set; }
    }

    public class YulsnVoucherCodeImportResponse
    {
        public int BulkImportId { get; set; }
    }

    public class YulsnPostVoucherAssignmentRequest
    {
        public int VoucherGroupId { get; set; }
        public int VoucherId { get; set; }
        public int ContactId { get; set; }
        public bool Reserve { get; set; }
    }

    public class YulsnPostVoucherBulkAssignmentRequest
    {
        public int VoucherGroupId { get; set; }
        public int VoucherId { get; set; }
        public int SegmentId { get; set; }
        public int? SegmentLimit { get; set; }
        public DateTimeOffset? Scheduled { get; set; }
        public string CreatedBy { get; set; }
    }

    public class YulsnPostVoucherAssignmentResponse
    {
        public int VoucherAssignmentId { get; set; }
    }

    public class YulsnPatchVoucherAssignmentRequest
    {
        public int VoucherAssingmentId { get; set; }
        public VoucherAssignmentStatus Status { get; set; }
        public DateTimeOffset? StatusChanged { get; set; }
    }

    public class YulsnVoucherAssignment<T> where T : YulsnVoucher
    {
        public int Id { get; set; }
        public int? VoucherCodeId { get; set; }
        public int? ContactId { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public VoucherAssignmentStatus Status { get; set; }
        public DateTimeOffset? StatusChanged { get; set; }

        public T Voucher { get; set; }
        public YulsnVoucherCode VoucherCode { get; set; }
    }

    public class YulsnVoucherCode
    {
        public int VoucherId { get; set; }
        public string Code { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsAssigned { get; set; }
    }

    public enum VoucherRedeemTime
    {
        EndOfDay,
        Hour,
        Custom
    }

    public enum VoucherAssignmentStatus
    {
        Reserved = 10,
        Assigned = 20,
        Viewed = 30,
        Redeemed = 40,
        Expired = 50,
        Claimed = 60
    }
}
