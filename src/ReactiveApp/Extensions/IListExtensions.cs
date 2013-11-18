using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp
{
    public static class IListExtensions
    {
        public static T RemoveLast<T>(this IList<T> This)
        {
            int lastIndex = This.Count - 1;
            T lastValue = This[lastIndex];
            This.RemoveAt(lastIndex);
            return lastValue;
        }
    }
}
