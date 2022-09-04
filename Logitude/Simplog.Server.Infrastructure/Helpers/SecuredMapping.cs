using System;
using System.Reflection;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class SecuredMapping
    {
        public static T GetMappedPM<T>(T originalPM, T mappedPM,string objecttablename,int tenant)
        {
            if (originalPM != null)
            {
                PropertyInfo[] propertiesInfo = originalPM.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in propertiesInfo)
                {
                    if (propertyInfo.Name != "EntityState" && propertyInfo.Name != "HasChanges" && propertyInfo.Name != "ChangedProperties" && propertyInfo.Name != "SuppressCreateNotifyPropertyChangeValues")
                    {
                        PropertyInfo newProp = mappedPM.GetType().GetProperty(propertyInfo.Name);
                        if (newProp != null)
                        {
                            object value = propertyInfo.GetValue(originalPM, null);
                            newProp.SetValue(mappedPM, value, null);

                            if (propertyInfo.Name == "Tenant")
                            {
                                if (int.Parse(value.ToString()) != tenant && int.Parse(value.ToString()) != 0)
                                {
                                    throw new Exception("Invalid tenant data (unsecured data)");
                                }
                            }
                        }

                    }
                }

                PropertyInfo issecuredProp = mappedPM.GetType().GetProperty("IsSecured");
                issecuredProp.SetValue(mappedPM, true, null);
                return mappedPM;
            }
            else
            {
                return originalPM;
            }
        }
    }
}