using System.Collections.Generic;
using System.Net.Http;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class ApiRequestHeader
    {
        // Standard headers
        public string Authorization { get; set; }
        public string ContentType { get; set; } = "application/json";
        public string Accept { get; set; } = "application/json";        
        public HttpMethod Method { get; set; }
        public int Timeout { get; set; } = 20000;  // Default timeout in milliseconds (20s)

        // Custom headers
        public Dictionary<string, string> CustomHeaders { get; set; }= new Dictionary<string, string>();
        public Dictionary<string, string> ToDictionary()
        {
            var headers = new Dictionary<string, string>();
            // Standard headers
            if (!string.IsNullOrEmpty(Authorization))
                headers["Authorization"] = Authorization;

            if (!string.IsNullOrEmpty(ContentType))
                headers["Content-Type"] = ContentType;

            if (!string.IsNullOrEmpty(Accept))
                headers["Accept"] = Accept;           

            // Custom headers
            if (CustomHeaders != null)
            {
                foreach (var custom in CustomHeaders)
                {
                    if (!headers.ContainsKey(custom.Key))
                        headers[custom.Key] = custom.Value;
                }
            }

            return headers;
        }
    }
}
