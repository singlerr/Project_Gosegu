using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ScriptEngine.Utils
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (var item in newItems) collection.Add(item);
        }

        public static Collection<T> ToCollection<T>(this IList<T> list)
        {
            return new Collection<T>(list);
        }
    }
}