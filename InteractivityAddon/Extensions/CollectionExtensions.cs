using System;
using System.Collections.Generic;
using System.Linq;
using Qommon.Collections;

namespace Interactivity.Extensions
{
    internal static partial class Extensions
    {
        public static ReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> collection)
            => new ReadOnlyCollection<T>(collection.ToArray());

        public static ReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> collection)
            => new ReadOnlyList<T>(collection.ToArray());

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
}