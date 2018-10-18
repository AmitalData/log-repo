using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class CommunicationAttachmentExtendedController : ApiController
    {


        public HttpResponseMessage GetCommunicationAttachmentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

             CommunicationAttachmentQuery communicationAttachmentQuery = new CommunicationAttachmentQuery(tenant);
             IQueryable<CommunicationAttachmentPM> communicationAttachmentLists =  communicationAttachmentQuery.GetCommunicationAttachmentPMsByTenant(tenant);
             return Request.CreateResponse(HttpStatusCode.OK, communicationAttachmentLists);

        }


        public HttpResponseMessage GetCommunicationAttachmentsByCommunicationLogId(string communicationLogId , int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CommunicationAttachmentQuery communicationAttachmentQuery = new CommunicationAttachmentQuery(tenant);
            IQueryable<CommunicationAttachmentPM> communicationAttachmentLists = communicationAttachmentQuery.GetCommunicationAttachmentsByCommunicationLogId(communicationLogId, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, communicationAttachmentLists);

        }


    }
}