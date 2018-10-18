using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public static class DictionaryJsonConverter
    {

        public static string FromDictionaryToJson(this Dictionary<string, string> dictionary)
        {
            
            var json= JsonConvert.SerializeObject(dictionary);
            return json;
            var kvs = dictionary.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, string.Join(",", kvp.Value)));
            return string.Concat("{", string.Join(",", kvs), "}");
        }

        public static Dictionary<string, string> FromJsonToDictionary(this string json)
        {
            var dict=JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dict;

            string[] keyValueArray = json.Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty).Split(',');
            return keyValueArray.ToDictionary(item => item.Split(':')[0], item => item.Split(':')[1]);

            
        }


        
    }
}
