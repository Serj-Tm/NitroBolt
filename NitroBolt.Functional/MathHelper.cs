using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  public static class MathHelper
  {
    public static TItem Bound<TItem>(this TItem value, TItem? minValue, TItem? maxValue) where TItem:struct, IComparable
    {
      if (minValue != null && value.CompareTo(minValue) < 0)
        return minValue.Value;
      if (maxValue != null && value.CompareTo(maxValue) > 0)
        return maxValue.Value;
      return value;
    }
  }
}
