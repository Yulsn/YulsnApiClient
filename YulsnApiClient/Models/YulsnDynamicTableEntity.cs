using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnReadDynamicTableEntity
    {
        public int Id { get; set; }
        public string Secret { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }

    public class YulsnCreateDynamicTableEntity
    {
        public string Secret { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}
