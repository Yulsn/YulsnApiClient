using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

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

        public static bool TryGetValueAsDouble(this IDictionary<string, object> source, string key, out double? value)
        {
            if (source.TryGetValue(key, out object v1) && v1 is double v2)
            {
                value = v2;
                return true;
            }

            value = null;
            return false;
        }

        public static string Truncate(this string value, int maxLength, string suffix = null)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength ? value : (value.Substring(0, maxLength) + suffix ?? string.Empty);
        }

        public static IDictionary<string, object> ConvertJsonElements(this IDictionary<string, object> dictionary, string mapping = null, List<Node> tree = null)
        {
            if (dictionary is null)
                return null;

            Node node;
            if (mapping != null) tree = mapping.ToTree();

            foreach (var key in dictionary.Keys.ToArray())
            {
                node = tree?.FirstOrDefault(n => n.Name == key);
                if (tree?.Any() == true && node == null)
                {
                    dictionary.Remove(key);
                    continue;
                }

                switch (dictionary[key])
                {
                    case JToken e: dictionary[key] = e.AsObject(); break;
                    default: break;
                }
            }

            return dictionary;
        }

        /// <summary></summary>
        /// <param name="mapping">Format: (field),(field),(field.(field2),(field2.(field3)))</param>        
        public static List<Node> ToTree(this string mapping, Node parent = null)
        {
            if (mapping == null) return null;
            if (parent == null) parent = new Node();

            int open = 0, pos; string part = null;
            foreach (char ch in mapping)
                switch (ch)
                {
                    case ' ': break; // skip space
                    case ',': if (open != 0) goto default; break; // next
                    case '(': if (++open != 1) goto default; break; // start
                    case ')': // end
                        if (--open != 0) goto default;
                        else
                        {
                            if ((pos = part.IndexOf('.')) > 0) // sub
                            {
                                Node child = new Node { Name = part.Substring(0, pos) };
                                parent.Nodes.Add(child);
                                child.Nodes = ToTree(part.Substring(pos + 1), child);
                            }
                            else
                                parent.Nodes.Add(new Node { Name = part });

                            part = null;
                        }
                        break;
                    default: part += ch; break;
                }

            return parent.Nodes;
        }

        public static object AsObject(this JToken element, Node node = null)
        {
            return element.Type switch
            {
                JTokenType.Undefined => null,
                JTokenType.Null => null,
                JTokenType.Object => ConvertJsonElements(
                    ((JObject)element).Properties().ToDictionary(
                        o => o.Name,
                        o => (object)o.Value
                    ),
                    tree: node?.Nodes
                ),
                JTokenType.Array => element.Select(e => e.AsObject(node)).ToList(),
                JTokenType.String => element.Value<string>(),
                JTokenType.Integer => element.Value<long>(),
                JTokenType.Float => element.Value<double>(),
                JTokenType.Boolean => element.Value<bool>(),
                _ => element
            };
        }
    }
}
