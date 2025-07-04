
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
using System;
using System.Linq;
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
        public HttpResponseMessage GetSingle(string declarationId, int lineNumber, int invoiceCounterKey, int invoiceItemLineNumber, string siiRequestId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current?.Request?.Headers["Token"];
                if(string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("Missing authentication token.");
                }
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext ctx = CustomContext.GetContext(authToken.Tenant);

                var qs = new SupplierInvoiceItemsReqListQueryService(ctx);
                SupplierInvoiceItemsReqListPM pm = qs.GetOrCreate(siiRequestId, declarationId, lineNumber, invoiceCounterKey, invoiceItemLineNumber, authToken.Tenant);

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

        [HttpGet]                                    
        public async Task<HttpResponseMessage> GetProductFileExists(
            string modelCode,
            string importerNumber,
            string originCountry,
            string declarationId)
        {
            try
            {
                string token = HttpContext.Current?.Request?.Headers["Token"];
                if (string.IsNullOrEmpty(token))
                {
                    return Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        "Missing authentication token.");
                }

                var auth = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                const string interfaceName = CustomsPartnerFtpDetails.InterfaceName_SIIProductFileCheck;
                const string interfaceName_Response = CustomsPartnerFtpDetails.InterfaceName_SIIProductFileCheck_Response;
                const string partnerCode = CustomsPartnerFtpDetails.PartnerCode_SII;

                var factory = new SIIRequestApiRequestFactory(auth.Tenant);
                var credentials = factory.BuildCredentials(interfaceName, partnerCode);

                var commRequest = factory.BuildCommunicationsDto(interfaceName, partnerCode, declarationId);
                var commResponse = factory.BuildCommunicationsDto(interfaceName_Response, partnerCode, declarationId);


                var dto = new ProductFileRequestDto
                {
                    credentials = credentials,
                    importerNumber = importerNumber,
                    modelCode = modelCode,
                    originCountry = originCountry,
                };

                var apiRequest = factory.Create(interfaceName, partnerCode, dto, commRequest, commResponse);

                var executor = new RestRequestExecutor();
                var apiResp =
                    await executor
                          .ExecuteAsync<ProductFileRequestDto, ProductFileCheckResponseDto>(apiRequest)
                          .ConfigureAwait(false);

                bool fileExists = apiResp?.Success == true &&
                                  apiResp.Result?.productFiles?.Any() == true;


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