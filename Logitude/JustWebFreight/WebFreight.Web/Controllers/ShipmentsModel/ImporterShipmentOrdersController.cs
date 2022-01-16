using Logitude.BL.Security;
using Logitude.ShipmentOrderModule.Def.EntityAMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ImporterShipmentOrdersController : ApiController
    {

        public HttpResponseMessage Post(ShipmentOrderAM shipmentOrder)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(shipmentOrder.CustomerTenantNumber);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(null));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}