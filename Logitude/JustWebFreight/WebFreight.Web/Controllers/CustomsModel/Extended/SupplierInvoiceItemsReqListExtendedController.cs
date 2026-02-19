
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
        [Route("LookupProductFileByModel")]
        public async Task<HttpResponseMessage> LookupProductFileByModel(
            string modelCode,
            string importerNumber,
            string originCountry,
            string declarationId)
        {
            try
            {               
                string token = HttpContext.Current?.Request?.Headers["Token"];
                if (string.IsNullOrEmpty(token))

                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new { error = "Missing authentication token." });

                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                const string interfaceName = CustomsPartnerFtpDetails.InterfaceName_SIIProductFileCheck;
                const string interfaceNameResponse = CustomsPartnerFtpDetails.InterfaceName_SIIProductFileCheck_Response;
                const string partnerCode = CustomsPartnerFtpDetails.PartnerCode_SII;

                var factory = new SIIRequestApiRequestFactory(authToken.Tenant);
                var credentials = factory.BuildCredentials(interfaceName, partnerCode);

                var commRequest = factory.BuildCommunicationsDto(interfaceName, partnerCode, declarationId);
                var commResponse = factory.BuildCommunicationsDto(interfaceNameResponse, partnerCode, declarationId);

                var dto = new ProductFileRequestDto
                {
                    credentials = credentials,
                    importerNumber = importerNumber,
                    modelCode = modelCode,
                    originCountry = originCountry
                };

                var apiRequest = factory.Create(interfaceName, partnerCode, dto, commRequest, commResponse);
                var executor = new RestRequestExecutor();


                var apiResp = await executor
                    .ExecuteAsync<ProductFileRequestDto, ProductFileCheckResponseDto>(apiRequest)
                    .ConfigureAwait(false);

                if (apiResp?.Success != true || apiResp.Result?.productFiles == null || !apiResp.Result.productFiles.Any())
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Product not found by model code." });

                var productFileId = apiResp?.Result?.productFiles?.FirstOrDefault()?.id;

                return Request.CreateResponse(HttpStatusCode.OK, productFileId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public class ProductFileDto
        {
            public string Id { get; set; }   
        }

    }
}