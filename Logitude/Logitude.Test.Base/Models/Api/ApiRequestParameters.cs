using RestSharp;

namespace Logitude.Base.Models.Api
{
    public class ApiRequestParameters
    {
        public Method Method { set; get; }
        public string Url { set; get; }
        public string Token { set; get; }
        public object RequestBody { set; get; }
        public bool IsApi = true;
    }
}