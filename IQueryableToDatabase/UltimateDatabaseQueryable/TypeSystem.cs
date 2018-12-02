using System;
using System.Collections.Generic;

namespace UltimateDatabaseQueryable
{
    public static class TypeSystem
    {
        public static Type GetElementType(Type seqType)
        {
            var ienum = FindIEnumerable(seqType);
            if (ienum == null)
            {
                return seqType;
            }
            return ienum.GetGenericArguments()[0];
        }

        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
            {
                return null;
            }
            if (seqType.IsArray)
            {
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            }
            if (seqType.IsGenericType)
            {
                foreach (var arg in seqType.GetGenericArguments())
                {
                    var ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }
            var interfaces = seqType.GetInterfaces();
            if (interfaces != null && interfaces.Length > 0)
            {
                foreach (var iface in interfaces)
                {
                    var ienum = FindIEnumerable(iface);
                    if (ienum != null)
                    {
                        return ienum;
                    }
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }
    }
}