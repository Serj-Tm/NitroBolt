using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.Immutable
{
    public class QItem
    {
        public QItem(ImmutableDictionary<string, object> data = null)
        {
            this.Data = data ?? ImmutableDictionary<string, object>.Empty;
        }

        public readonly ImmutableDictionary<string, object> Data;
    }
}
