using System;

namespace YulsnApiClient.Models.V1
{
    public class YulsnReadAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }
}
