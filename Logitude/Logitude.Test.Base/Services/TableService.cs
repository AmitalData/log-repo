using System;
using System.Collections.Generic;

namespace System.Dynamic
{
    public static class TableService
    {
        public static T Get<T>(this ExpandoObject obj, string member)
        {
            if (obj == null) return default;
            if (member == null) return default;
            if (!IsPropertyExist(obj, member)) return default;

            return GetProperty<T>(obj, member);
        }

        private static bool IsPropertyExist(ExpandoObject obj, string name)
        {
            if (obj is ExpandoObject) return ((IDictionary<string, object>)obj).ContainsKey(name);
            return obj.GetType().GetProperty(name) != null;
        }

        private static T GetProperty<T>(ExpandoObject obj, string member)
        {
            if (!(obj is ExpandoObject)) return (T)Convert.ChangeType(obj.GetType().GetProperty(member).GetValue(obj, null), typeof(T));

            var value = ((IDictionary<String, Object>)obj)[member];
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
