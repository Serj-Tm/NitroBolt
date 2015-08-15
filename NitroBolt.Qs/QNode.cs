using NitroBolt.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.Qs
{
  public struct QNode
  {
    public QNode(object value, IEnumerable<QNode> nodes)
    {
      this.Value = value;
      this.Nodes = nodes.OrEmpty();
    }

    public readonly object Value;
    public readonly IEnumerable<QNode> Nodes;
  }
}
