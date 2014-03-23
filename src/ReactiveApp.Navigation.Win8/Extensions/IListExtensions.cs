using System.Collections.Generic;

namespace ReactiveApp.Navigation
{
    public static class IListExtensions
    {
        /// <summary>
        /// Removes items starting at the end until the item specified has been removed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="This">The this.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static void RemoveUntil<T>(this IList<T> This, T item)
        {
            int lastIndex = This.Count - 1;
            T removed = default(T);
            do
            {
                removed = This[lastIndex];
                This.RemoveAt(lastIndex);
                lastIndex--;
            }
            while (!removed.Equals(item) && This.Count > 0);
        }
    }
}
