using System.Collections.Generic;

namespace YulsnApiClient.Test.Abstractions
{
    public class Node
    {
        public string Name { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();

        public override string ToString() => $"{Name} {Nodes?.Count.ToString().Surround("<", ">")}";
    }
}
