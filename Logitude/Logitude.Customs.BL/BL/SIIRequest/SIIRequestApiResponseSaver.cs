using Logitude.BL.DataContracts;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.RestRequestExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiResponseSaver
    {

        private readonly int _tenant;
        public SIIRequestApiResponseSaver(int tenant)
        {
            _tenant = tenant;
        }

        public void Save(ApiResponse<ReleaseRequestApiResponseDto> apiResp,
                         string siiRequestId)
        {
            var entity = new SIIRequestApiCallLog
            {
                SIIRequestId = siiRequestId,
                Tenant = _tenant,
                Success = apiResp.Success,
                ResponseCode = apiResp.Result?.ResponseCode ?? -1,
                RequestNumber = apiResp.Result?.RequestNumber ?? 0,
                ValidationMessages = apiResp.Result?.ValidationMessages
            };
            UpdateSIIRequest(entity);
        }
        private void UpdateSIIRequest(SIIRequestApiCallLog  response)
        {
            // save to db 
        }
        private class SIIRequestApiCallLog
        {
            public string SIIRequestId { get; set; }
            public int Tenant { get; set; }
            public bool Success { get; set; }
            public int ResponseCode { get; set; }
            public int RequestNumber { get; set; }
            public string ValidationMessages { get; set; }
        }
    }
}
