using Logitude.ShipmentOrderModule.Def.EntityAMs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.ImporterShipmentOrders;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ImporterShipmentOrdersController : ApiController
    {

        public HttpResponseMessage Post(ShipmentOrderAM shipmentOrder)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(shipmentOrder.CustomerTenantNumber);
                string correlationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterShipmentOrdersService importerShipmentOrdersService = new ImporterShipmentOrdersService(shipmentOrder.CustomerTenantNumber, correlationId);
                var shipment = importerShipmentOrdersService.UpdateShipment(shipmentOrder);
                return Request.CreateResponse(HttpStatusCode.OK, new List<string>() { shipment.Id, shipment.ShipmentNumber });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}