using System.Collections.Generic;
using Qommon.Collections;
using System.Linq;

namespace Interactivity.Extensions
{
    internal static partial class Extensions
    {
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> collection)
            => new ReadOnlyCollection<T>(collection.ToArray());

        public static ReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> collection)
            => new ReadOnlyList<T>(collection.ToArray());

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
}
