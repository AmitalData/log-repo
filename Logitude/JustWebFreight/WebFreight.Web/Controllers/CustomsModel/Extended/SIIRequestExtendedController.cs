
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.Data.EntityKeys.Extended;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class SIIRequestExtendedController : ApiController
    {
        public HttpResponseMessage GetSingle(string declarationId, string id = null)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext ctx = CustomContext.GetContext(authToken.Tenant);
                var qs = new SIIRequestQueryService(ctx);
                qs.InitializeSettings();

                SIIRequestPM pm;

                if (!string.IsNullOrWhiteSpace(id) && id != "null")
                {
                    pm = qs.GetSingle(id, true, false);
                }
                else
                {
                    pm = qs.GetSinglePM(null, declarationId, authToken.Tenant);
                }

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, pm);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                       HttpStatusCode.BadRequest,
                       ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSupplierInvoiceItemsForSIIRequest(string declarationId,string siiRequestId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                var auth = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(auth.Tenant);

                var ctx = CustomContext.GetContext(auth.Tenant);
                var svc = new SIIRequestQueryService(ctx);
                svc.InitializeSettings();
                var list = svc.GetSupplierInvoiceItems(declarationId, siiRequestId,auth.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        ApiExceptionBuilder.BuildException(ex));
            }

        }

        public async Task<HttpResponseMessage> PostSendSIIRequest(string siiRequestId,string declarationId, int tenant, [FromBody] List<SiiSelectedRowDto> selectedRows)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                var auth = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                var sender = new SIIRequestApiSender(auth.Tenant);

                var (apiResp, dto) = await sender.SendAsync(siiRequestId, declarationId, selectedRows);

                if (apiResp == null)
                    throw new InvalidOperationException(
                        $"Did not receive a response from SII for request '{siiRequestId}'.");
                new SIIRequestApiResponseSaver(auth.Tenant)
                           .Save(apiResp, siiRequestId, dto);
                if (apiResp?.Success == true && apiResp.Result?.ResponseCode == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, apiResp.Result);
                }

                // error → 400 + error payload
                var errorPayload = new
                {
                    ResponseCode = apiResp?.ErrorCode ?? -1,
                    ValidationMessages = apiResp?.ErrorMessage ?? "Unknown error"
                };

                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    Error = true,
                    Details = errorPayload
                });
            }
            catch (Exception ex)
            {
                var errorPayload = new
                {
                    Error = true,
                    Details = new
                    {
                        ResponseCode = -1,
                        ValidationMessages = ex.Message
                    }
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorPayload);   // 400
            }
        }
    }
}