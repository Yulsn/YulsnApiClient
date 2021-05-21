using System;

namespace YulsnApiClient.Models
{
    public class YulsnReadContactEvent
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public int ContactId { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string ParentId { get; set; }
        public string ParentType { get; set; }
        /// <summary>A batch insert only occures on unique or null values - BE CAREFUL</summary>
        public string UniqueExtId { get; set; }
        public object InfoJson { get; set; }
    }

    public class YulsnCreateContactEvent
    {
        /// <summary>ContactId or ContactSecret should have a value</summary>
        public int? ContactId { get; set; }
        /// <summary>ContactId or ContactSecret should have a value</summary>
        public string ContactSecret { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string ParentId { get; set; }
        public string ParentType { get; set; }
        /// <summary>A batch insert only occures on unique or null values - BE CAREFUL</summary>
        public string UniqueExtId { get; set; }
        public object InfoJson { get; set; }
    }
}
