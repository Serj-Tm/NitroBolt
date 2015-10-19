using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  static public class DictionaryHelper
  {
// ReSharper disable CompareNonConstrainedGenericWithNull
    [System.Diagnostics.DebuggerStepThrough]
    public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
    {
      if (key != null && dictionary != null)

      {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
          return value;
      }
      return default(TValue);
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static TValue? FindValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
    {
      if (key != null && dictionary != null)
      {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
          return value;
      }
      return null;
    }
// ReSharper restore CompareNonConstrainedGenericWithNull
  }
}
