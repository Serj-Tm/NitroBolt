﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.Immutable
{
    public static class ImmutableHelper
    {
      public static ImmutableArray<T> OrEmpty<T>(this ImmutableArray<T>? items)
      {
        return items ?? ImmutableArray<T>.Empty;
      }
      public static ImmutableList<T> OrEmpty<T>(this ImmutableList<T> items)
      {
        return items ?? ImmutableList<T>.Empty;
      }
    }
}
