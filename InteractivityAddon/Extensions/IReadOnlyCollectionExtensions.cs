using System;
using System.Collections.Generic;

namespace Interactivity.Extensions
{
    internal static partial class Extensions
    {
        public static int FindIndex<T>(this IReadOnlyCollection<T> collection, Predicate<T> match)
        {
            int i = 0;

            foreach(var item in collection)
            {
                if (match.Invoke(item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}
