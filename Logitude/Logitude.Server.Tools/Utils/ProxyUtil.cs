using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Utils
{
    public static class ProxyUtil// MiscService
    {
        public static Func<string, string, int, bool> SecurityUtilityCheckFeature { get; set; }

        public static string JsonConvertSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T JsonConvertDeserializeTyped<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        public static object JsonConvertDeserialize(string  jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }
        public static List<string> GetRequiredField(string onedimensionalarrayJson,List<string> mustFields)
        {
            var emptyFields = new List<string>();
            var obj=JToken.Parse(onedimensionalarrayJson);
            foreach (var field in mustFields)
            {
                string val = (string)obj[field]??"";
                if (string.IsNullOrWhiteSpace(val))
                {
                    emptyFields.Add(field);
                }

            }
            return emptyFields;


        }
    }
}
