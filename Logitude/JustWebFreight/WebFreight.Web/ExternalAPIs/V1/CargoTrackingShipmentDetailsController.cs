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
                CargoTrackingShipmentDetailsResult cargoTrackingShipmentDetailsResult = CreateCargoTrackingShipmentDetailsResultInstance(number, authToken.Tenant);

                if (cargoTrackingShipmentDetailsResult.HasMoreThanOneShipmentWithSameHouse)
                    return CreateResponseWithStringMessage("More Than one Shipment Found");
                else
                    return CreateSuccessfulResponse(cargoTrackingShipmentDetailsResult.CargoTrackingShipmentDetails);
            }
            catch (Exception ex)
            {
                return CreateFailedResponse(ex);
            }
        }

        private static CargoTrackingShipmentDetailsResult CreateCargoTrackingShipmentDetailsResultInstance(string number, int tenant)
        {
            CargoTrackingShipmentDetailsInstanceCreator Creator = new CargoTrackingShipmentDetailsInstanceCreator(tenant);
            CargoTrackingShipmentDetailsResult cargoTrackingShipmentDetails = Creator.CreateCargoTrackingShipmentDetailsResultInstance(number);
            return cargoTrackingShipmentDetails;
        }

        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateSuccessfulResponse(CargoTrackingShipmentDetails cargoTrackingShipmentDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentDetails);
        }
        private HttpResponseMessage CreateFailedResponse(Exception exception)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(exception);
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);

        }
        private HttpResponseMessage CreateResponseWithStringMessage(string message)
        {
            if ( message == null) { return Request.CreateResponse(HttpStatusCode.OK, "Successful response");}
            return Request.CreateResponse(HttpStatusCode.OK, message);
        }

    }
}