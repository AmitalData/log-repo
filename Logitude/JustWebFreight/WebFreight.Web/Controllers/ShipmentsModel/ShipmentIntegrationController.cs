using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ShipmentIntegrationController: ApiController
    {
        public HttpResponseMessage GetShipmentVars()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentPreperationIntegrationVariables shipmentPreperationVariables = new ShipmentPreperationIntegrationVariables(tenant);
                ShipmentIntegrationVariables vars = shipmentPreperationVariables.GetShipmentVars();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                
                return Request.CreateResponse(HttpStatusCode.OK, vars); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}