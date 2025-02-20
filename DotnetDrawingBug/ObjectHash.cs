using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace DotnetDrawingBug
{
    public static class ObjectHash
    {
        private const int INIT = 0;
        private const int PRIME = 7;

        public static long ContentBased(object obj)
        {
            var hashes = new Collection<int>();
            GetHashesRecursively(hashes, obj);

            long hash = INIT;
            unchecked
            {
                foreach (var i in hashes)
                {
                    hash = PRIME * hash + i;
                }
            }

            return hash;
        }

        private static void GetHashesRecursively(ICollection<int> hashes, object obj)
        {
            if (obj == null || hashes == null)
            {
                return;
            }

            var objType = obj.GetType();

            if (objType != typeof(string))
            {
                var genericType = typeof(IEnumerable<>).FullName;
                if (genericType != null && objType.GetInterface(genericType) != null)
                {
                    foreach (var e in (IEnumerable)obj)
                    {
                        GetHashesRecursively(hashes, e);
                    }
                    return;
                }

                if (objType == typeof(IEnumerable))
                {
                    foreach (var e in (IEnumerable)obj)
                    {
                        GetHashesRecursively(hashes, e);
                    }
                    return;
                }
            }
            else
            {
                hashes.Add(obj.GetHashCode());
                return;
            }

            var toStringType = objType.GetMethod("ToString", [])?.DeclaringType;
            if (toStringType != typeof(object))
            {
                var toString = obj.ToString();
                if (toString != null)
                {
                    hashes.Add(toString.GetHashCode());
                }
                return;
            }

            var fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var fieldObject = field.GetValue(obj);
                if (fieldObject != null
                    && !field.IsStatic
                    && field.FieldType != typeof(IntPtr)
                    && field.FieldType != typeof(Pointer)
                    //&& field.FieldType != typeof(void*) // <-- This line prevents the StackOverflowException
                    && field.FieldType != typeof(void))
                {
                    if (field.FieldType.IsPrimitive)
                    {
                        hashes.Add(fieldObject.GetHashCode());
                    }
                    else
                    {
                        GetHashesRecursively(hashes, fieldObject);
                    }
                }
            }
        }
    }
}
