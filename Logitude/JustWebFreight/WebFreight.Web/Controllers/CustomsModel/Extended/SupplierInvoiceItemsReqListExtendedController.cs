
using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.RestRequestExecutor;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class SupplierInvoiceItemsReqListExtendedController : ApiController
    {
        public HttpResponseMessage GetSingle(string declarationid, int linenumber, int invoicecounterkey, int invoiceitemlinenumber, string siirequestid)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext ctx = CustomContext.GetContext(authToken.Tenant);
                var qs = new SupplierInvoiceItemsReqListQueryService(ctx);

                SupplierInvoiceItemsReqListPM pm;

                if (!string.IsNullOrWhiteSpace(siirequestid) && siirequestid != "null")
                {
                    pm = qs.GetSingle(declarationid, linenumber, invoicecounterkey, invoiceitemlinenumber, true, false);
                }
                else
                {
                    pm = qs.GetSinglePM(null, declarationid, linenumber, invoicecounterkey, invoiceitemlinenumber, authToken.Tenant);
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

        public HttpResponseMessage GetProductFileExists(
            string modelCode,
            string importerNumber,
            string originCountry)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                string interfaceName = CustomsPartnerFtpDetails.InterfaceName_SIIProductFileCheck;
                string partnerCode = CustomsPartnerFtpDetails.PartnerCode_SII;

                var factory = new SIIRequestApiRequestFactory(authToken.Tenant);
                var credentials = factory.BuildCredentials(interfaceName, partnerCode);

                var dto = new ProductFileRequestDto
                {
                    credentials = credentials,
                    importerNumber = importerNumber,
                    modelCode = modelCode,
                    originCountry = originCountry,
                };

                var apiRequest = factory.Create(interfaceName, partnerCode, dto);

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


                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
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