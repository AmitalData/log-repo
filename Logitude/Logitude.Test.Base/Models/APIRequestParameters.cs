using RestSharp;

namespace Logitude.Test.Base.Models
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
