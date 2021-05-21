using System;

namespace YulsnApiClient.Models
{
    public class YulsnReadStoreDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string JsonString { get; set; }
    }

    public class YulsnCreateStoreDto
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string JsonString { get; set; }
    }
}
