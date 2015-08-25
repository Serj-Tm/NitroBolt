using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.Functional.Random
{
    public static class RandomHelper
    {
        public static T ElementAtRandomOrDefault<T>(this IList<T> items, System.Random random)
        {
            if (items == null || items.Count == 0)
                return default(T);
            return items[random.Next(items.Count)];
        }
    }
}
