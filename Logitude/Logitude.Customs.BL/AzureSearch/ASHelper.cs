using Logitude.BL.Helpers;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.AzureSearch
{
    public static class ASHelper
    {
        public static Task<dynamic> GetIndexSettingsAsync(int tenant, string index) => Task.Run(() => GetIndexSettings(tenant, index));

        public static dynamic GetIndexSettings(int tenant, string index)
        {
            string settings = DefaultService.Instance.Get(tenant, "AzureSearchAI", index)?.Value1;
            if (string.IsNullOrEmpty(settings))
                throw new ArgumentNullException(nameof(settings), "AzureSearch settings default not found");

            return JsonConvert.DeserializeObject(settings);
        }
    }
}