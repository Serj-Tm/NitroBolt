using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NitroBolt.Functional.Random
{
    public static class RandomHelper
    {
        [return: MaybeNull]
        public static T ElementAtRandomOrDefault<T>(this IList<T>? items, System.Random random)
        {
            if (items == null || items.Count == 0)
                return default;
            return items[random.Next(items.Count)];
        }
    }
}
