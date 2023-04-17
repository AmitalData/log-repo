using Logitude.Accounting.BL.APIDataContract.ApiV1;
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

    public class JournalLiteController : ApiController
    {
        public HttpResponseMessage GetSingleJournalLite(string externalNo, string externalSystem)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Journal", authToken.Tenant);

                JournalQueryService Service = new JournalQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                JournalLite Result = new JournalLite();


                if ((externalNo != null && externalSystem == null) || externalNo == null && externalSystem != null)
                {
                    throw new Exception("Both ExternalEntityCode and ExternalEntityReference are required");
                }
                if (externalNo != null && externalSystem != null)
                {
                    Result = Service.GetSingleJournalLiteByExternalNoAndExternalSystem(externalNo, externalSystem, tenant);
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