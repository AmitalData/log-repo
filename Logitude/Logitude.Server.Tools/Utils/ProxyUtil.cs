using Newtonsoft.Json;
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
    }
}
