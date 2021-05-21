using System;
using System.Collections.Generic;

namespace YulsnApiClient.Models
{
    public class YulsnCreateOrderDto
    {
        public string Type { get; set; }
        public string ExtOrderId { get; set; }
        public int? ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
        public int? StoreId { get; set; }
        public List<YulsnCreateOrderLineDto> OrderLines { get; set; }
    }

    public class YulsnReadOrderDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ExtOrderId { get; set; }
        public int? ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
        public decimal PriceTotal { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public int? StoreId { get; set; }
        public List<YulsnReadOrderLineDto> OrderLines { get; set; }
    }
}
