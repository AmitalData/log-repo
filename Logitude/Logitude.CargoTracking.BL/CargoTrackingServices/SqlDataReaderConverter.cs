using FastMember;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices
{
    public static class SqlDataReaderConverter
    {
        public static T ConvertToObject<T>(SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers().ToDictionary(e=>e.Name.ToLower(),e=>e);
            var t = new T();
            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.ContainsKey(fieldName.ToLower()))
                    {
                        try
                        {
                            accessor[t, fieldName] = rd.GetValue(i);

                        }
                        catch (Exception e)
                        {
                            throw new Exception("Field Name:" + fieldName + ", see inner exception",e);
                        }
                    }
                }
            }

            return t;
        }
    }
}
