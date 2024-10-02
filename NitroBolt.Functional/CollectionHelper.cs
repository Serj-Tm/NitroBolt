using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NitroBolt.Functional
{
    public static partial class CollectionHelper
  {
    [System.Diagnostics.DebuggerStepThrough]
    public static IEnumerable<TItem> OrEmpty<TItem>(this IEnumerable<TItem>? items)
    {
      return items ?? EmptyCollection<TItem>.Empty;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static ICollection<TItem> OrEmpty<TItem>(this ICollection<TItem>? items)
    {
      return items ?? EmptyCollection<TItem>.Empty;
    }
    public static IList<TItem> OrEmpty<TItem>(this IList<TItem>? items)
    {
      if (items == null)
        return Array<TItem>.Empty;
      return items;
    }
    public static TItem[] OrEmpty<TItem>(this TItem[]? items)
    {
      if (items == null)
        return Array<TItem>.Empty;
      return items;
    }
    public static List<TItem> OrEmpty<TItem>(this List<TItem>? items)
    {
      if (items == null)
        return new List<TItem>();
      return items;
    }

    public static T? MinObject<T>(this IEnumerable<T> items, Func<T, IComparable> keyer) where T : class
    {
      IComparable? min = null;
      T? minItem = null;
      foreach (var item in items)
      {
        var value = keyer(item);
        if (min == null || value.CompareTo(min) < 0)
        {
          min = value;
          minItem = item;
        }
      }
      return minItem;
    }
    public static T? MinValue<T>(this IEnumerable<T> items, Func<T, IComparable> keyer) where T : struct
    {
      IComparable? min = null;
      T? minItem = null;
      foreach (var item in items)
      {
        var value = keyer(item);
        if (min == null || value.CompareTo(min) < 0)
        {
          min = value;
          minItem = item;
        }
      }
      return minItem;
    }

    public static T? MaxObject<T>(this IEnumerable<T> items, Func<T, IComparable> keyer) where T : class
    {
      IComparable? max = null;
      T? maxItem = null;
      foreach (var item in items)
      {
        var value = keyer(item);
        if (max == null || value.CompareTo(max) > 0)
        {
          max = value;
          maxItem = item;
        }
      }
      return maxItem;
    }
    public static T? MaxValue<T>(this IEnumerable<T> items, Func<T, IComparable> keyer) where T : struct
    {
      IComparable? max = null;
      T? maxItem = null;
      foreach (var item in items)
      {
        var value = keyer(item);
        if (max == null || value.CompareTo(max) > 0)
        {
          max = value;
          maxItem = item;
        }
      }
      return maxItem;
    }

    public static IEnumerable<TResult> OuterJoin<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> lefts, IEnumerable<TRight> rights, Func<TLeft, TKey> leftKeyer, Func<TRight, TKey> rightKeyer, Func<TLeft, TRight, TResult> result)
    {
      var rightIndex = rights.GroupBy(rightKeyer).ToDictionary(group => group.Key, group => group.First());
      var rightMarkers = new Dictionary<TKey, bool>();
      foreach (var left in lefts)
      {
        TRight right;
        var key = leftKeyer(left);
        if (rightIndex.TryGetValue(key, out right))
        {
          yield return result(left, right);
          rightMarkers[key] = true;
        }
        else
          yield return result(left, default!);
      }
      foreach (var right in rights.Where(_right => !rightMarkers.ContainsKey(rightKeyer(_right))))
      {
        yield return result(default!, right);
      }
    }

    public static IEnumerable<SplitGroup<T, TKey>> SplitBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keyer)
    {
      var prevs = new List<T>();
      var prevKey = default(TKey);
      foreach (var item in items)
      {
        var key = keyer(item);
        if (prevs.Count != 0)
        {
          if (!object.Equals(prevKey, key))
          {
            yield return new SplitGroup<T, TKey>(prevKey!, prevs.ToArray());
            prevs.Clear();
            prevKey = key;
          }
        }
        else
        {
          prevKey = key;
        }
        prevs.Add(item);
      }
      if (prevs.Count > 0)
        yield return new SplitGroup<T, TKey>(prevKey!, prevs.ToArray());
    }
  }

  public class SplitGroup<T, TKey>
  {
    public SplitGroup([AllowNull]TKey key, T[] items)
    {
      this.Key = key;
      this.Items = items;
    }
    [AllowNull]
    public readonly TKey Key;
    public readonly T[] Items;
  }


  public class EmptyCollection<TItem> : IEnumerable<TItem>, ICollection<TItem>
  {
    public IEnumerator<TItem> GetEnumerator()
    {
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    void ICollection<TItem>.Add(TItem item)
    {
    }

    void ICollection<TItem>.Clear()
    {
    }

    bool ICollection<TItem>.Contains(TItem item)
    {
      return false;
    }

    void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
    {
    }

    bool ICollection<TItem>.Remove(TItem item)
    {
      return false;
    }

    int ICollection<TItem>.Count
    {
      get
      {
        return 0;
      }
    }

    bool ICollection<TItem>.IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public static readonly EmptyCollection<TItem> Empty = new EmptyCollection<TItem>();

  }


}
