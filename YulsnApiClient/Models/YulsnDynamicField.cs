using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnReadDynamicField
    {
        /// <summary>
        /// Name of dynamic field
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type of dynamic field
        /// </summary>
        public YulsnDynamicFieldType Type { get; set; }
        /// <summary>
        /// Label of dynamic field(preferred name for displaying).
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// List of allowed dynamic field values. Null means no restrictions. Dynamic field value can always be null.
        /// </summary>
        public List<object> Values { get; set; }
    }


    public enum YulsnDynamicFieldType
    {
        String = 1,
        Boolean = 2,
        Number = 3,
        DateTimeOffset = 4,
        Decimal = 5
    }

    public enum YulsnTableOwner
    {
        Orders = 1,
        OrderLines,
        ContactCompanies,
        Contacts,
        Stores,
        DynamicTable,
    }
}
