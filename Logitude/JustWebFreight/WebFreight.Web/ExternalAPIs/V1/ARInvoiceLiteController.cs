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
    public class ARInvoiceLiteController : ApiController
    {
        public HttpResponseMessage GetSingleARInvoiceLite(string id, string number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("ARInvoice", authToken.Tenant);

                ARInvoiceQueryService Service = new ARInvoiceQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = new ARInvoiceLite();
                if (!string.IsNullOrEmpty(id))
                {
                    Result = Service.GetARInvoiceLiteById(id, tenant);
                }

                else if (!string.IsNullOrEmpty(number))
                {
                    Result = Service.GetARInvoiceLiteByInvoiceNumber(number, tenant);
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