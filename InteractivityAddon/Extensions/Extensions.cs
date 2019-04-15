using System.ArrayExtensions;
using System.Collections.Generic;
using System.Reflection;
using Discord;

namespace System
{
    internal static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsPrimitive(this Type type) => type == typeof(string)
                                                          ? true
                                                          : type.IsValueType & type.IsPrimitive;

        public static object DeepClone(this object originalObject) => InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null) {
                return null;
            }

            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) {
                return originalObject;
            }

            if (visited.ContainsKey(originalObject)) {
                return visited[originalObject];
            }

            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) {
                return null;
            }

            object cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray) {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false) {
                    var clonedArray = (Array) cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null) {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags)) {
                if (filter != null && filter(fieldInfo) == false) {
                    continue;
                }

                if (IsPrimitive(fieldInfo.FieldType)) {
                    continue;
                }

                object originalFieldValue = fieldInfo.GetValue(originalObject);
                object clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        public static T DeepClone<T>(this T original) => (T) DeepClone((object) original);
    }

    internal class ReferenceEqualityComparer : EqualityComparer<object>
    {
        public override bool Equals(object x, object y) => ReferenceEquals(x, y);
        public override int GetHashCode(object obj) => obj == null
                                                       ? 0
                                                       : obj.GetHashCode();
    }

    namespace ArrayExtensions
    {
        internal static class ArrayExtensions
        {
            public static void ForEach(this Array array, Action<Array, int[]> action)
            {
                if (array.LongLength == 0) {
                    return;
                }

                var walker = new ArrayTraverse(array);
                do {
                    action(array, walker.Position);
                }
                while (walker.Step());
            }
        }

        internal class ArrayTraverse
        {
            public int[] Position;
            private readonly int[] maxLengths;

            public ArrayTraverse(Array array)
            {
                maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i) {
                    maxLengths[i] = array.GetLength(i) - 1;
                }
                Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (int i = 0; i < Position.Length; ++i) {
                    if (Position[i] < maxLengths[i]) {
                        Position[i]++;
                        for (int j = 0; j < i; j++) {
                            Position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }
        }
    }

    namespace EmbedFieldExtensions
    {
        internal static class EmbedFieldExtensions
        {
            public static EmbedFieldBuilder ToBuilder(this EmbedField field) => new EmbedFieldBuilder()
                    .WithIsInline(field.Inline)
                    .WithName(field.Name)
                    .WithValue(field.Value);
        }
    }

}