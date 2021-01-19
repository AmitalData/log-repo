using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Services
{
    public class APIRequestParameters
    {
        public Method method { set; get; }
        public string URL { set; get; }
        public string token { set; get; }
        public string jsonElement { set; get; }
        public object requestBody { set; get; }
    }
}
