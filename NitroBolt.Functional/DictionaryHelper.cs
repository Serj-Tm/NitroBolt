using System.Collections.Generic;

namespace NitroBolt.Functional
{
  static public class DictionaryHelper
  {
// ReSharper disable CompareNonConstrainedGenericWithNull
    [System.Diagnostics.DebuggerStepThrough]
    public static TValue? Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
    {
      if (key != null && dictionary != null)

      {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
          return value;
      }
      return default;
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

    [System.Diagnostics.DebuggerStepThrough]
    public static TValue? Find<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
    {
      if (key != null && dictionary != null)

      {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
          return value;
      }
      return default;
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static TValue? FindValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
    {
      if (key != null && dictionary != null)
      {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
          return value;
      }
      return null;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static TValue? Find<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : class
    {
      if (key != null && dictionary != null)

      {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
          return value;
      }
      return default;
    }
    [System.Diagnostics.DebuggerStepThrough]
    public static TValue? FindValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
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
