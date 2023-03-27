using RestSharp;
using System.Collections.Generic;

namespace MicrosoftGraphClient.Models.GraphAPI
{
    public class GraphAPICallerParams
    {
        public string Token { get; set; }
        public string Url { get; set; }
        public Method Method { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> RequestParameters { get; set; }
        public object RequestBody { get; set; }
    }
}