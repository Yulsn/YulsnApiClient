using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace YulsnApiClient.Test.Abstractions
{
    public static class Extensions
    {
        public static string Surround(this string source, string start = null, string end = null, bool @if = true)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            if (@if)
                return $"{start ?? string.Empty}{source}{end ?? start ?? string.Empty}";

            return source;
        }

        public static bool TryGetValueAsString(this IDictionary<string, object> source, string key, out string value)
        {
            if (source.TryGetValue(key, out object v1) && v1 is string v2)
            {
                value = v2;
                return true;
            }

            value = null;
            return false;
        }

        public static bool TryGetValueAsLong(this IDictionary<string, object> source, string key, out long? value)
        {
            if (source.TryGetValue(key, out object v1) && v1 is long v2)
            {
                value = v2;
                return true;
            }

            value = null;
            return false;
        }
    }
}
