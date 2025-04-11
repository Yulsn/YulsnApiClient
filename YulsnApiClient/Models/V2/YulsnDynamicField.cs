using System;

namespace YulsnApiClient.Models.V2
{
    public class YulsnDynamicFieldBase
    {
        public bool IsUnique { get; set; }
        public bool IsSearchable { get; set; }
        public bool HasPersonData { get; set; }
        public bool CanMerge { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string ValuesJson { get; set; }
        public DynamicFieldTemplate? Template { get; set; }
    }

    public class YulsnDynamicField : YulsnDynamicFieldBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DynamicFieldType Type { get; set; }

        public TableOwner? RelationOwner { get; set; }
        public int? RelationDynamicTableId { get; set; }
        public DynamicFieldRelationOnDelete? RelationOnDelete { get; set; }

        public DateTimeOffset LastModified { get; set; }
    }

    public class YulsnDynamicFieldSaveBase : YulsnDynamicFieldBase { }

    public class YulsnDynamicFieldAddRequest : YulsnDynamicFieldSaveBase
    {
        public string Name { get; set; }
        public DynamicFieldType Type { get; set; }
    }

    public class YulsnDynamicFieldUpdateRequest : YulsnDynamicFieldSaveBase { }

    public class YulsnDynamicFieldRelationUpdateRequest
    {
        public TableOwner? RelationOwner { get; set; }
        public int? RelationDynamicTableId { get; set; }
        public DynamicFieldRelationOnDelete? RelationOnDelete { get; set; }
    }

    public enum DynamicFieldTemplate
    {
        TextHtml = 1,
        Json = 2,
        Link = 3,
        MediaUrl = 4,
        CkEditor = 5
    }

    public enum TableOwner : int
    {
        Orders = 1,
        OrderLines,
        ContactCompanies,
        Contacts,
        Stores,
        DynamicTable,
        VoucherGroup,
        Voucher,
        VoucherGroupVoucher
    }

    public enum DynamicFieldType : int
    {
        String = 1,
        Boolean = 2,
        Number = 3,
        DateTimeOffset = 4,
        Decimal = 5
    }

    public enum DynamicFieldRelationOnDelete : int
    {
        Cascade = 1,
        SetNull
    }
}
