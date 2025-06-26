using Logitude.BL.DataContracts;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.RestRequestExecutor;
using Simplog.Server.Infrastructure;
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
            if (apiResp == null)
                throw new InvalidOperationException($"No response was received from the SII for request '{siiRequestId}'.");

            var entity = new SIIRequestApiCallLog
            {
                SIIRequestId = siiRequestId,
                Tenant = _tenant,
                Success = apiResp.Success,
                ResponseCode = apiResp.Result?.ResponseCode ?? -1,
                RequestNumber = apiResp.Result?.RequestNumber,
                ValidationMessages = apiResp.Result?.ValidationMessages
            };
            UpdateSIIRequest(entity);
        }
        private void UpdateSIIRequest(SIIRequestApiCallLog response)
        {

            var ctx = CustomContext.GetContext(_tenant);
            try
            {
                if (response.ResponseCode == 0)
                {
                    var repo = new SIIRequestQueryService(ctx);
                    var updater = new SIIRequestUpdateService(ctx,
                                      new Dictionary<string, IContext>(), _tenant);

                    var siiReq = repo.GetSingle(response.SIIRequestId,
                                                    true,
                                                    false);

                    if (siiReq != null)
                    {
                        siiReq.RequestNo = response.RequestNumber;
                        siiReq.ChangeSetOp = ChangeSetOperation.Update;

                        updater.Update(siiReq, true);
                        return;
                    }
                }
            }
            finally
            {
                if (ctx is IDisposable d) d.Dispose();
            }

        }
        private class SIIRequestApiCallLog
        {
            public string SIIRequestId { get; set; }
            public int Tenant { get; set; }
            public bool Success { get; set; }
            public int ResponseCode { get; set; }
            public string RequestNumber { get; set; }
            public string ValidationMessages { get; set; }
        }
    }
}
