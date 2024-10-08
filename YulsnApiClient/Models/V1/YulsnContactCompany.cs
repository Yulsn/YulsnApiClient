﻿using System;

namespace YulsnApiClient.Models.V1
{
    public class YulsnReadContactCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int? PrimaryContactId { get; set; }
        public int? ContactCompanyId { get; set; }
        public string EmailDomains { get; set; }
        public string Logo { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }

    public class YulsnCreateContactCompany
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public int? PrimaryContactId { get; set; }
        public int? ContactCompanyId { get; set; }
        public string EmailDomains { get; set; }
        public string Logo { get; set; }
    }
}
