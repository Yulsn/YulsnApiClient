using System;

namespace YulsnApiClient.Models.V2
{
    public class YulsnDynamicTableBase
    {
        public string Label { get; set; }
        public string Right { get; set; }
        public string Description { get; set; }
        public string PersonDataLifetime { get; set; }
        public string EntityLifetime { get; set; }

        public string Icon { get; set; }
        public string Group { get; set; }
        public string GroupIcon { get; set; }
        public string GroupHeading { get; set; }

        public int? CreateConverterId { get; set; }
        public int? UpdateConverterId { get; set; }
        public int? DeleteConverterId { get; set; }
    }

    public class YulsnDynamicTable : YulsnDynamicTableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsShared { get; set; }

        public DateTimeOffset LastModified { get; set; }
    }

    public class YulsnDynamicTableSaveBase : YulsnDynamicTableBase { }

    public class YulsnDynamicTableAddRequest : YulsnDynamicTableSaveBase
    {
        public string Name { get; set; }
        public bool IsShared { get; set; }
    }

    public class YulsnDynamicTableUpdateRequest : YulsnDynamicTableSaveBase { }
}
