using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace YulsnApiClient.Models.V2
{
    public class ProblemDetails : Exception
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int? Status { get; set; }

        public string Detail { get; set; }

        public string Instance { get; set; }

        public override string Message => Title + " " + Detail;

        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>(StringComparer.Ordinal);
    }
}