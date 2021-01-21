using RestSharp;

namespace Logitude.Test.Base.Models
{
    public class ApiRequestParameters
    {
        public Method Method { set; get; }
        public string Url { set; get; }
        public string Token { set; get; }
        public object RequestBody { set; get; }
    }
}