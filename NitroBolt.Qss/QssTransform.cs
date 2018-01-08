using NitroBolt.QSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NitroBolt.Functional;

namespace NitroBolt.Qss
{
    public static class QssTransform
    {
        public static IEnumerable<QNode> TreeProcess(IEnumerable<QNode> nodes, Func<IEnumerable<QNode>, IEnumerable<QNode>> f)
        {
            return f(nodes.Select(node => node?.Nodes?.Any() == true ? new QNode(node?.Value, TreeProcess(node?.Nodes, f)) : node).ToArray());
        }
        public static IEnumerable<QNode> ArrowCollapse(IEnumerable<QNode> nodes)
        {
            if (nodes?.Any(node => node?.Value.As<string>() == "->") != true)
                return nodes;
            var results = new List<QNode>();
            var prevs = new List<QNode>();
            foreach (var node in nodes)
            {
                if (node?.Value.As<string>() == "->")
                {
                    if (prevs.Count == 1)
                        results.Add(prevs[0]);
                    else if (prevs.Count != 0)
                        results.Add(new QNode("-", prevs.ToArray()));
                    prevs.Clear();
                    continue;
                }
                prevs.Add(node);
            }
            if (true)
            {
                if (prevs.Count == 1)
                    results.Add(prevs[0]);
                else if (prevs.Count != 0)
                    results.Add(new QNode("-", prevs.ToArray()));

            }
            return new QNode[] { new QNode("->", results.ToArray()) };
        }

    }
}
