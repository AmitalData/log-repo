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
        public static string JsonConvertSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
