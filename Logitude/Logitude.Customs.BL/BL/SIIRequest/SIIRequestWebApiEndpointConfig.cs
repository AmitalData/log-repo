using Logitude.Server.Tools.RestRequestExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestWebApiEndpointConfig
    {
        public string Url { get; set; }
        public ApiRequestHeader Header { get; set; }

        public SIIRequestWebApiEndpointConfig()
        {
            Header = new ApiRequestHeader
            {
                Method = HttpMethod.Post,
                ContentType = "application/json",
                Accept = "application/json",
                Timeout = 20_000
            };
        }
    }
}
