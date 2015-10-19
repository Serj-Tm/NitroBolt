using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  public static class ArrayHelper
  {
    [System.Diagnostics.DebuggerStepThrough]
    public static bool All<T>(this T[] arr, Func<T, bool> f)
    {
      for (var i = 0; i < arr.Length; ++i)
        if (!(f(arr[i])))
          return false;
      return true;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static bool Any<T>(this T[] arr, Func<T, bool> f)
    {
      if (arr == null)
        return false;
      for (var i = 0; i < arr.Length; ++i)
        if ((f(arr[i])))
          return true;
      return false;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static bool Any<T>(this T[] items)
    {
      return items != null && items.Length > 0;
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static T FirstOrDefault<T>(this T[] items)
    {
      if (items == null || items.Length == 0)
        return default(T);
      return items[0];
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static T First<T>(this T[] items)
    {
      return items[0];
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static T LastOrDefault<T>(this T[] items)
    {
      if (items == null || items.Length == 0)
        return default(T);
      return items[items.Length - 1];
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static T ElementAtOrDefault<T>(this T[] items, int index)
    {
      if (items == null || index < 0 || index >= items.Length)
        return default(T);
      return items[index];
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static IEnumerable<T> Where<T>(this T[] arr, Func<T, bool> f)
    {
      for (var i = 0; i < arr.Length; ++i)
      {
        var item = arr[i];
        if ((f(item)))
          yield return item;
      }
    }

  }

  public static class Array<TItem>
  {
    // ReSharper disable StaticFieldInGenericType
    public static readonly TItem[] Empty = new TItem[] { };
    // ReSharper restore StaticFieldInGenericType
  }

}
