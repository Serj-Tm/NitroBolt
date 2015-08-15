using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.QSharp
{
  public static class QNodeHlp
  {
    public static string AsString(this QNode node)
    {
      return node.Value?.ToString();
    }
    public static string AsString(this QNode? node)
    {
      return node?.Value?.ToString();
    }
  }

  public interface QNodeBuilder { }
}
