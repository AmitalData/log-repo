using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.RestRequestExecutor;
using Newtonsoft.Json.Linq;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiResponseSaver
    {

        private readonly int _tenant;
        public SIIRequestApiResponseSaver(int tenant)
        {
            _tenant = tenant;
        }

        public bool Save(ApiResponse<ReleaseRequestApiResponseDto> apiResp,
                         string siiRequestId , ReleaseRequestApiDto dto)

        {
            if (apiResp == null)
                throw new InvalidOperationException($"No response was received from the SII for request '{siiRequestId}'.");

            int responseCode = apiResp.Result != null
                               ? apiResp.Result.ResponseCode
                               : TryExtractResponseCode(apiResp.ErrorMessage) ?? -1;

            string requestNumber = apiResp.Result?.RequestNumber;
            if (string.IsNullOrWhiteSpace(requestNumber) &&
                !string.IsNullOrWhiteSpace(apiResp.ErrorMessage))
            {
                requestNumber = TryExtractRequestNumber(apiResp.ErrorMessage);
            }
            var entity = new SIIRequestApiCallLog
            {
                SIIRequestId = siiRequestId,
                Tenant = _tenant,
                Success = apiResp.Success,
                ResponseCode = responseCode,
                RequestNumber = requestNumber,
                ValidationMessages = apiResp.Result?.ValidationMessages,
                FormApplicationId = dto.releaseRequestForm.formApplicationId
            };
            return UpdateSIIRequest(entity,dto);
        }
        private bool UpdateSIIRequest(SIIRequestApiCallLog response, ReleaseRequestApiDto dto)
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
                    return false;

                if (!string.IsNullOrWhiteSpace(response.FormApplicationId))
                    siiReq.FromApplicationId = response.FormApplicationId;

                bool hadRequestNoBefore = !string.IsNullOrWhiteSpace(siiReq.RequestNo);
                bool hasNewRequestNo = !string.IsNullOrWhiteSpace(response.RequestNumber);

                SIIResponseCode code = (SIIResponseCode)response.ResponseCode;

                bool shouldUpdateLines = true;
                bool isFinal = false;

                if (code == SIIResponseCode.Success ||
                    code == SIIResponseCode.ValidationError)
                {
                    siiReq.Status = ((int)code).ToString();
                }

                if (code == SIIResponseCode.Success && hasNewRequestNo)
                {
                    siiReq.RequestNo = response.RequestNumber;
                    isFinal = true;
                }
                
                else if (code == SIIResponseCode.ValidationError &&
                         !hadRequestNoBefore &&
                         hasNewRequestNo)
                {
                    siiReq.RequestNo = response.RequestNumber;
                    siiReq.Status = ((int)SIIResponseCode.Success).ToString();
                    shouldUpdateLines = false;
                    isFinal = true;
                }

                if (shouldUpdateLines && dto?.releaseRequestForm?.releaseRequestLinesForm != null)
                {
                    UpdateRequestLinesAfterSuccessOnComposition(siiReq, dto);
                }

                siiReq.ChangeSetOp = ChangeSetOperation.Update;
                updater.Update(siiReq, true);
                return isFinal;
            }
            finally
            {
                if (ctx is IDisposable d) d.Dispose();
            }
        }

        private void UpdateRequestLinesAfterSuccessOnComposition(SIIRequestPM siiReq, ReleaseRequestApiDto dto)
        {
            if (dto == null ||
                dto.releaseRequestForm == null ||
                dto.releaseRequestForm.releaseRequestLinesForm == null)
                return;

            var dtoLines = dto.releaseRequestForm.releaseRequestLinesForm;
            var children = siiReq.SupplierInvoiceItemsReqLists;

            if (children == null || children.Count == 0)
                return;

            var dtoLookup = dtoLines.ToDictionary(
                l => (l.InvoiceCounterKey, l.InvoiceItemLineNumber),
                l => l.lineSerialNumber  
            );

            foreach (var child in children)
            {
                var key = (child.InvoiceCounterKey, child.InvoiceItemLineNumber);

                if (dtoLookup.TryGetValue(key, out int newLineNumber))
                {
                    if (child.LineNumber != newLineNumber)
                    {
                        child.LineNumber = newLineNumber;
                        child.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                else
                {
                    if (child.LineNumber != 0)
                    {
                        child.LineNumber = 0;
                        child.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
        }

        private static int? TryExtractResponseCode(string errorMessage)
        {
            var json = TryExtractJson(errorMessage);
            if (json == null) return null;

            try
            {
                JToken token = json["responseCode"];
                return token != null && token.Type == JTokenType.Integer
                       ? (int)token
                       : (int?)null;
            }
            catch
            {
                return null;
            }
        }
        private static string TryExtractRequestNumber(string errorMessage)
        {
            var json = TryExtractJson(errorMessage);
            if (json == null) return null;

            try
            {
                JToken token = json["requestNumber"];
                if (token == null) return null;

                if (token.Type == JTokenType.Integer)
                {
                    int rn = token.Value<int>();
                    return rn > 0 ? rn.ToString() : null; 
                }

                if (token.Type == JTokenType.String)
                {
                    string rn = token.Value<string>();
                    return !string.IsNullOrWhiteSpace(rn) && rn != "0" ? rn : null;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static JObject TryExtractJson(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return null;

            Match m = Regex.Match(errorMessage, @"Response:\s*(\{.*\})");
            string jsonText = null;

            if (m.Success)
            {
                jsonText = m.Groups[1].Value;
            }
            else
            {
                int start = errorMessage.IndexOf('{');
                int end = errorMessage.LastIndexOf('}');
                if (start >= 0 && end > start)
                    jsonText = errorMessage.Substring(start, end - start + 1);
            }

            if (string.IsNullOrWhiteSpace(jsonText))
                return null;

            try { return JObject.Parse(jsonText); }
            catch { return null; }
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