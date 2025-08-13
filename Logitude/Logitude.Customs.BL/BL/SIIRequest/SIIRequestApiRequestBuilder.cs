using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.RestRequestExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public static class ApiRequestBuilder
    {
        public static ApiRequest<TData> Build<TData>(
            int tenant,
            SIIRequestWebApiEndpointConfig config,
            TData data, ApiCommunicationConstants requestComm,
            ApiCommunicationConstants responseComm)
        {
            return new ApiRequest<TData>
            {
                Tenant = tenant,
                Url = config.Url,
                Header = config.Header,
                Data = data,
                RequestComm = requestComm,
                ResponseComm = responseComm,
            };
        }
    }
}
