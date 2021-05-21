using System;

namespace YulsnApiClient.Models
{
    public class YulsnCreateOrderLineDto
    {
        public string ExtProductId { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int? ContactId { get; set; }
    }

    public class YulsnReadOrderLineDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ExtProductId { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int? ContactId { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }
}
