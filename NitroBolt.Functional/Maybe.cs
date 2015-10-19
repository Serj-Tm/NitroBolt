using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.Functional.Maybe
{
    public static class MaybeHelper
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static TValue Maybe<TItem, TValue>(this TItem item, Func<TItem, TValue> f)
        {
            if (item == null || f == null)
                return default(TValue);
            return f(item);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static TValue? MaybeValue<TItem, TValue>(this TItem item, Func<TItem, TValue> f)
          where TValue : struct
        {
            if (item == null || f == null)
                return null;
            return f(item);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void Maybe<TItem>(this TItem item, Action<TItem> action)
        {
            if (item == null || action == null)
                return;
            action(item);
        }
    }
}
