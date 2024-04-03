using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class APInvoiceLiteController : ApiController
    {
        public HttpResponseMessage GetSingleAPInvoiceLite(string number, string externalId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("APInvoice", authToken.Tenant);

                APInvoiceQueryService Service = new APInvoiceQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = new APInvoiceLite();

                if (!string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(externalId))
                {
                    Result = Service.GetAPInvoiceLiteByInvoiceNumberAndExternalId(number, externalId, tenant);
                }

                else if (!string.IsNullOrEmpty(number))
                {
                    Result = Service.GetAPInvoiceLiteByInvoiceNumber(number, tenant);
                }

                else if (!string.IsNullOrEmpty(externalId))
                {
                    Result = Service.GetAPInvoiceLiteByExternalId(externalId, tenant);
                }



                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}