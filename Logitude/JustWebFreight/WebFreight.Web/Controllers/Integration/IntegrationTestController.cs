using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
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
using Logitude.BL.IntegrationTestModel.EntityQueries;
using Logitude.BL.IntegrationTestModel.EntityPMs;

namespace WebFreight.Web.Controllers.Integration
{
    public class IntegrationTestController : ApiController
    {
        public HttpResponseMessage GetBaseShipment()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                ShipmentPreparationVariables shipmentPreperationVariables = new ShipmentPreparationVariables(tenant);
                ShipmentVariables Variables = shipmentPreperationVariables.GetBaseShipmentVaribles();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, Variables); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetBaseLocations() {

            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                LocationsPreparationVariables locationsPreperationVariables = new LocationsPreparationVariables(tenant);
                LocationsVariables Variables = locationsPreperationVariables.GetBaseLocationsVaribles();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, Variables); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBasePartners()
        {

            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                PartnersPreparationVariables partnersPreperationVariables = new PartnersPreparationVariables(tenant);
                PartnersVariables Variables = partnersPreperationVariables.GetBasePartnersVaribles();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, Variables); ;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}