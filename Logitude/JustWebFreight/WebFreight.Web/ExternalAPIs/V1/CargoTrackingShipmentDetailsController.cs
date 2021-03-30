using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Logitude.CargoTracking.BL.APIDataContract;


namespace WebFreight.Web.ExternalAPIs.V1
{
    public class CargoTrackingShipmentDetailsController : ApiController
    {
        public HttpResponseMessage GetSingleCargoTrackingShipmentDetails(string number)
        {
            try
            {
                AuthenticationToken authToken = GetAuthenticationToken();
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                CargoTrackingShipmentDetailsInstanceCreator Creator = new CargoTrackingShipmentDetailsInstanceCreator(authToken.Tenant);
                CargoTrackingShipmentDetails cargoTrackingShipmentDetails = Creator.CreateCargoTrackingShipmentDetailsInstance(number);
                if(cargoTrackingShipmentDetails != null)
                    return Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentDetails);
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "More Than one Shipment Found");
            }
            catch (Exception ex)
            {
                return CreateResponse(ex, null);
            }
        }


        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateResponse(Exception exception, string message)
        {
            if (exception == null && message != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(exception);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

    }
}