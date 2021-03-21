using Logitude.BL.InfrastructureModel.APIDataContract;
using Logitude.BL.InfrastructureModel.APIDataContract.Messages;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs
{
    public class RatesUpdateController : ApiController
    {
        public HttpResponseMessage Post(RatesUpdate ratesUpdateEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticateAPICall(tenant);

                    RatesUpdateService ratesUpdateService = new RatesUpdateService(ratesUpdateEntity, tenant);
                    ratesUpdateService.CleanXMLText();
                    ratesUpdateService.ValidateRatesDataMapping();
                    ratesUpdateService.UpdateRatesData();

                    APIHelper.AddCommunicationLog("D", ratesUpdateEntity, ratesUpdateEntity, "RatesTable", null, "Rates Update API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, ratesUpdateEntity);
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", ratesUpdateEntity, apiExceptionResult.Exception, "RatesTable", null, "Rates Update API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}