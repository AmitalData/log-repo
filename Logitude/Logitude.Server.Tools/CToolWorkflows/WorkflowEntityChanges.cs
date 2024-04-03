using Logitude.Server.Tools.CToolWorkflows.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logitude.Server.Tools.CToolWorkflows
{
    public static class WorkflowEntityChanges
    {
        public static List<PropertyChange> GetChangedProperties(object entityPoco, object entityPM)
        {
            if (entityPoco == null || entityPM == null)
            {
                return new List<PropertyChange>();
            }

            List<PropertyInfo> entityPropertiesInfo = GetEntityPropertiesInfo(entityPoco);
            return GetEntityChangedProperties(entityPropertiesInfo, entityPoco, entityPM);
        }

        private static List<PropertyInfo> GetEntityPropertiesInfo(object entity)
        {
            return entity.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList()
                .Where(p => (!p.PropertyType.IsClass || 
                              p.PropertyType == typeof(string) || 
                              p.PropertyType == typeof(object) || 
                              p.PropertyType.Name == "CustomFieldClass"))
                .ToList();
        }

        private static List<PropertyChange> GetEntityChangedProperties(List<PropertyInfo> entityPropertiesInfo, object oldEntity, object newEntity)
        {
            return (from propertyInfo in entityPropertiesInfo

                    let oldValue = GetValue(oldEntity, propertyInfo.Name)

                    let newValue = propertyInfo.Name.StartsWith("Field") ?
                    GetValue(GetValue(newEntity, propertyInfo.Name), "Value") :
                    GetValue(newEntity, propertyInfo.Name)

                    where oldValue != newValue && (oldValue == null || !oldValue.Equals(newValue))
                    select new PropertyChange
                    {
                        Property = propertyInfo.Name,
                        OldValue = oldValue,
                        NewValue = newValue
                    }).ToList();
        }

        public static object GetValue(object model, string field)
        {
            return model?.GetType()?.GetProperty(field)?.GetValue(model, null);
        }
    }
}
