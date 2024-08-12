using System;
using System.Collections.Generic;

namespace YulsnApiClient.Models.V1
{
    public class YulsnCreateOrderDto<CL> where CL : YulsnCreateOrderLineDto
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
        public string Email { get; set; }
        public DateTimeOffset? OriginDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public List<CL> OrderLines { get; set; }
    }

    public class YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto
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
        public string Email { get; set; }
        public DateTimeOffset? OriginDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public List<RL> OrderLines { get; set; }
    }
}
