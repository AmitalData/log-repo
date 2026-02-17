using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class CommunicationLogExtendedController : ApiController
    {

        public HttpResponseMessage GetCommunicationLogPMsByEntityId(string entityId, int tenant)
        {
            Authentication();
            CommunicationLogQuery communicationLogQuery = new CommunicationLogQuery(tenant);
            IQueryable<CommunicationLogPM>  myResult = communicationLogQuery.GetCommunicationLogPMsByEntityId(entityId, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }


        public HttpResponseMessage GetCommunicationLogPMsByEntityIdAndDocumentOutId(string entityId, string documentOutId, int tenant)
        {
            Authentication();
            CommunicationLogQuery communicationLogQuery = new CommunicationLogQuery(tenant);
            IQueryable<CommunicationLogPM> myResult = communicationLogQuery.GetCommunicationLogPMsByEntityIdAndDocumentOutId(entityId,documentOutId, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }


        public HttpResponseMessage GetSendCommunicationLogToQueue(string communicationLogId, int tenant)
        {
            Authentication();

            SimulatorResponsesWebService simulatorResponsesWebService = new SimulatorResponsesWebService();
            simulatorResponsesWebService.SendCommunicationLogToQueue(communicationLogId, tenant);
     
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }



        

        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", authToken.Tenant);
        }
    }
}