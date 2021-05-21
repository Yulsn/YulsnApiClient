using System.Collections.Generic;

namespace YulsnApiClient.Models
{
    public class YulsnReadIframeToken
    {
        public string Id { get; set; }
        public int AccountId { get; set; }
        public int AdministratorId { get; set; }
        public string AdministratorEmail { get; set; }
        public List<int> StoreIds { get; set; }
        public List<string> Rights { get; set; }
        public int GroupId { get; set; }
        public string TimeZoneId { get; set; }
    }
}
