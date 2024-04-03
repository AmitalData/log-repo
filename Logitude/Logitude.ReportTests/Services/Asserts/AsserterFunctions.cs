using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ReportTests.Services.Asserts
{
    public static class AsserterFunctions
    {
        public static object ChangeType(this object searchTerm, Type type)
        {
            if (searchTerm == null) return null;
            return Convert.ChangeType(searchTerm, type);
        }

        public static bool IsClassProperty(this PropertyInfo property)
        {
            return property.PropertyType.IsClass && !property.PropertyType.FullName.StartsWith("System.") && !property.PropertyType.FullName.StartsWith("Microsoft.");
        }

        public static bool IsListProperty(this PropertyInfo property)
        {
            return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var objType = obj.GetType();
            var prop = objType.GetProperty(propertyName);

            return prop.GetValue(obj, null);
        }   

        public static Type GetPropertyType(this object obj, string propertyName)
        {
            var objType = obj.GetType();
            return objType.GetProperty(propertyName).PropertyType;
        }

        public static object GetNestedPropertyValue(this object obj, string propertyName)
        {
            foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
                obj = prop.GetValue(obj, null);

            return obj;
        }

        public static Type GetNestedPropertyType(this object obj, string propertyName)
        {
            Type type = null;
            foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
            {
                obj = prop.GetValue(obj, null);
                type = prop.PropertyType;
            }

            return type;
        }
    }
}
