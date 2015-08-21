using NitroBolt.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.QSharp
{
    public struct QNode
    {
        public QNode(object value, IEnumerable<QNode> nodes = null)
        {
            this.Value = value;
            this.Nodes = nodes.OrEmpty();
        }

        public readonly object Value;
        public readonly IEnumerable<QNode> Nodes;

        public static bool operator == (QNode q1, QNode q2) => q1.Value == q2.Value && q1.Nodes == q2.Nodes;
        public static bool operator !=(QNode q1, QNode q2) => q1.Value != q2.Value || q1.Nodes != q2.Nodes;

        public override int GetHashCode()
        {
            return (Value?.GetHashCode()??0) ^ Nodes.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return obj is QNode && this == (QNode)obj;
        }
    }
}
