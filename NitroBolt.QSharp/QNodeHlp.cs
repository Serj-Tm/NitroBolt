using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.QSharp
{
    public static partial class QNodeHlp
    {
        public static string AsString(this QNode node)
        {
            return node?.Value?.ToString();
        }

        public static QNode P(this QNode node, params object[] path)
        {
            return P_s(node, path).FirstOrDefault();
        }
        public static QNode P(this IEnumerable<QNode> nodes, params object[] path)
        {
            return P_s(nodes, path).FirstOrDefault();
        }

        public static IEnumerable<QNode> P_s(this QNode node, params object[] path)
        {
            return P_s(Enumerable.Repeat(node, node != null ? 1 : 0), path);
        }
        public static IEnumerable<QNode> P_s(this IEnumerable<QNode> nodes, params object[] path)
        {
            foreach (var entry in path)
            {
                var i = entry as int?;
                if (i != null)
                {
                    nodes = nodes.Select(n => n.Nodes.ElementAtOrDefault(i.Value)).Where(n => n != null);
                }
                else
                {
                    var s = entry?.ToString();                    
                    if (s == "*")
                        nodes = nodes.SelectMany(n => n.Nodes);
                    else
                        nodes = nodes.SelectMany(n => n.Nodes.Where(child => child.AsString() == s));
                }
            }
            return nodes;
        }
    }

    public interface QNodeBuilder { }
}
