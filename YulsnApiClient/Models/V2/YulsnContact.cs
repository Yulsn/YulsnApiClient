using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models.V2
{
    public class YulsnContactMergeRequest
    {
        public List<string> EventTypes { get; set; }
        public List<string> OrderTypes { get; set; }
        public List<YulsnContactMergeDispatchLogFormVersions> DispatchLogFormVersions { get; set; }
    }

    public class YulsnContactMergeDispatchLogFormVersions
    {
        public YulsnMessageForm Form { get; set; }
        public YulsnMessageVersion Version { get; set; }
    }

    public class YulsnContactMergeResult
    {
        public Dictionary<string, int> MovedEventTypes { get; set; }
        public Dictionary<string, int> MovedOrderTypes { get; set; }
        public Dictionary<string, int> MovedDispatchLogFormVersions { get; set; }
    }
}
