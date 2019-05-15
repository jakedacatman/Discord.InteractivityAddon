using System.Collections.Generic;

namespace InteractivityAddon.Extensions
{
    internal static class ListOfStringExtensions
    {
        public static List<string> LowerAll(this List<string> list)
        {
            for (int i = 0; i < list.Count; i++) {
                list[i] = list[i].ToLower();
            }

            return list;
        }
    }
}
