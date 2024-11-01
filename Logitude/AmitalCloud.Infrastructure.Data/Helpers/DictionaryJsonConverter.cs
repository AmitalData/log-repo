using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public static class DictionaryJsonConverter
    {
        public static string FromDictionaryToJson(this Dictionary<string, string> dictionary)
        {
            var json= JsonConvert.SerializeObject(dictionary);
            return json;
        }
        public static Dictionary<string, string> FromJsonToDictionary(this string json)
        {
            var dict=JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dict;
        }
    }
}
