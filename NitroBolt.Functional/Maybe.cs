using System;
using System.Diagnostics.CodeAnalysis;

namespace NitroBolt.Functional.Maybe
{
    public static class MaybeHelper
    {
        [System.Diagnostics.DebuggerStepThrough]
        [return: MaybeNull]
        public static TValue Maybe<TItem, TValue>([AllowNull] this TItem item, Func<TItem, TValue>? f)
        {
            if (item == null || f == null)
                return default;
            return f(item);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static TValue? MaybeValue<TItem, TValue>([AllowNull] this TItem item, Func<TItem, TValue>? f)
          where TValue : struct
        {
            if (item == null || f == null)
                return null;
            return f(item);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void Maybe<TItem>([AllowNull] this TItem item, Action<TItem>? action)
        {
            if (item == null || action == null)
                return;
            action(item);
        }
    }
}
