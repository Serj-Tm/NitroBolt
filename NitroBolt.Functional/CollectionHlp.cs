using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.Functional
{
  public static class CollectionHlp
  {
    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> items)
    {
      return items ?? EmptyCollection<T>.Empty;
    }
  }
  public class EmptyCollection<T> : IEnumerable<T>
  {
    public IEnumerator<T> GetEnumerator()
    {
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    public static readonly EmptyCollection<T> Empty = new EmptyCollection<T>();
  }
}
