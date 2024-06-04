using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.Helpers
{
    public class DefaultAndConfiguration_Ext : DefaultAndConfiguration
    {
        public object ObjVal1
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(base.SetValueType1) || string.IsNullOrWhiteSpace(base.Value1))
                    {
                        Console.WriteLine("SetValueType1 or Value1 is null or empty.");
                        return null;
                    }

                    Type type1 = Type.GetType(base.SetValueType1);
                    if (type1 == null)
                    {
                        //Console.WriteLine($"Type.GetType failed for SetValueType1: {base.SetValueType1}");
                        return null;
                    }

                    var deserializedObject = Newtonsoft.Json.JsonConvert.DeserializeObject(base.Value1, type1);
                    
                    if (deserializedObject is Array array)
                    {
                        foreach (var item in array)
                        {
                            Console.WriteLine(item);
                        }
                        return array;
                    }
                    else
                    {
                        return deserializedObject;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deserializing ObjVal1: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
        }

        public object ObjVal2
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(base.SetValueType2) || string.IsNullOrWhiteSpace(base.Value2))
                    {
                        Console.WriteLine("SetValueType2 or Value2 is null or empty.");
                        return null;
                    }

                    Type type2 = Type.GetType(base.SetValueType2);
                    if (type2 == null)
                    {
                        //Console.WriteLine($"Type.GetType failed for SetValueType2: {base.SetValueType2}");
                        return null;
                    }

                    return Newtonsoft.Json.JsonConvert.DeserializeObject(base.Value2, type2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deserializing ObjVal2: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
        }

    }
}
