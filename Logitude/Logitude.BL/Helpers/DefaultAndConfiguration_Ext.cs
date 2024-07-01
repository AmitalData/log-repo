using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.Helpers
{
    public class DefaultAndConfiguration_Ext : DefaultAndConfiguration
    {
        public DefaultAndConfiguration_Ext(DefaultAndConfiguration a)
        {
            Id = a.Id;
            Tenant = a.Tenant;
            CreateDate = a.CreateDate;
            SearchFields = a.SearchFields;
            Is_Active = a.Is_Active;
            StoreInCache = a.StoreInCache;
            SetKey = a.SetKey;
            AdditionalKey = a.AdditionalKey;
            SortOrder = a.SortOrder;
            SetValueType1 = a.SetValueType1;
            Value1 = a.Value1;
            SetValueType2 = a.SetValueType2;
            Value2 = a.Value2;
            AllowInheritance = a.AllowInheritance;
        }

        public object ObjVal1
        {
            get
            {
                return Convert(Value1, SetValueType1);
            }
        }

        public object ObjVal2
        {
            get
            {
                return Convert(Value2, SetValueType2);
            }
        }

        private object Convert(string value, string typeString)
        {
            Console.WriteLine("deserializing value: " + value + ", type: " + typeString);
            try
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(typeString))
                {
                    Console.WriteLine("SetValueType or Value is null or empty.");
                    return null;
                }

                Type type = Type.GetType(typeString);

                if (type == typeof(string))
                    return value;

                //if (type == typeof(int) || type == typeof(double) || type == typeof(bool) || type == typeof(DateTime))
                //    return type.GetMethod("Parse").Invoke(null, new object[] { value });

                var deserializedObject = JsonConvert.DeserializeObject(value, type);

                
                if (deserializedObject is Array array)
                {
                    foreach (var item in array)
                        Console.WriteLine(item);

                    return array;
                }
                else
                    return deserializedObject;

            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while deserializing value: " + value + ", type: " + typeString + ", error: " + e);
                return null;
            }
        }
    }
}
