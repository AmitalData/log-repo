
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.Customs.BL.AzureSearch;
using System.Threading.Tasks;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.RestRequestExecutor;

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

        public HttpResponseMessage PostSendSIIRequest(string siiRequestId, int tenant)
        
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                var auth = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string interfaceName = CustomsPartnerFtpDetails.InterfaceName_SIISendRequest;
                string partnerCode = CustomsPartnerFtpDetails.PartnerCode_SII;

                var factory = new SIIRequestApiRequestFactory(auth.Tenant);
                var credentials = factory.BuildCredentials(interfaceName, partnerCode);

                var dto = new ProductFileRequestDto
                {
                    credentials = credentials,
                };

                var config = factory.GetEndpointConfig(interfaceName, partnerCode);
                var apiRequest = ApiRequestBuilder.Build(tenant, config, dto);

                var executor = new RestRequestExecutor();
                var apiResp = Task.Run(() =>
                    executor.ExecuteAsync<ProductFileRequestDto, ProductFileCheckResponseDto>(apiRequest))
                    .GetAwaiter()
                    .GetResult();

                bool fileExists = apiResp != null &&
                          apiResp.Success &&
                          apiResp.Result != null &&
                          apiResp.Result.productFiles != null &&
                          apiResp.Result.productFiles.Count > 0;


                SecurityUtility.AuthenticationOnTenant(auth.Tenant);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, fileExists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}