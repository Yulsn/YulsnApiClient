using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnDynamicTableEntity
    {
        public int Id { get; set; }
        public string Secret { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}
