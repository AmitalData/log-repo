using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CRMModel.Extended
{
    public class TicketExtendedController : ApiController
    {

        public HttpResponseMessage GetTicketsCountByShipmentId(string shipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

                ICRMContext crmContext = CRMContext.GetContext(tenant);
                TicketListQueryService listService = new TicketListQueryService(crmContext);
                int myResult = listService.GetTicketsCountByShipmentId(shipmentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}