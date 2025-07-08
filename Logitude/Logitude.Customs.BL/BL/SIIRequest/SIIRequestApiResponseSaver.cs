using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.RestRequestExecutor;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

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
                         string siiRequestId , ReleaseRequestApiDto dto)

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
                ValidationMessages = apiResp.Result?.ValidationMessages,
                FormApplicationId = dto.releaseRequestForm.formApplicationId
            };
            UpdateSIIRequest(entity);
        }
        private void UpdateSIIRequest(SIIRequestApiCallLog response)
        {
            var ctx = CustomContext.GetContext(_tenant);
            try
            {
                var repo = new SIIRequestQueryService(ctx);
                var updater = new SIIRequestUpdateService(ctx,
                                  new Dictionary<string, IContext>(), _tenant);

                var siiReq = repo.GetSingle(response.SIIRequestId,
                                            getComposition: true,
                                            getFromCache: false);

                if (siiReq == null)
                    return;

                if (!string.IsNullOrWhiteSpace(response.FormApplicationId))
                    siiReq.FromApplicationId = response.FormApplicationId;

                if (response.ResponseCode == (int)SIIResponseCode.Success ||
                    response.ResponseCode == (int)SIIResponseCode.ValidationError)
                {
                    siiReq.Status = response.ResponseCode.ToString();
                }

                if (response.ResponseCode == (int)SIIResponseCode.Success)
                    siiReq.RequestNo = response.RequestNumber;

                siiReq.Status = response.ResponseCode.ToString();
                siiReq.ChangeSetOp = ChangeSetOperation.Update;
                updater.Update(siiReq, true);
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
            public string FormApplicationId { get; set; } 

        }
    }
}
public enum SIIResponseCode
{
    Success = 0,
    ValidationError = 100
}