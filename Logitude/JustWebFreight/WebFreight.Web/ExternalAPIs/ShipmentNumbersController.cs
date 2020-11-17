using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.Messages;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs
{
    public class ShipmentNumbersController : ApiController
    {
        public HttpResponseMessage Post(GetShipmentNumbers entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticateAPICall(tenant);
                    entity = RemoveSpaces(entity);
                    ShipmentNumbersXML.ShipmentDataMappingValidating(entity, tenant);
                    var response = ShipmentNumbersXML.GetShipmentNumbersXMLMessage(entity, tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Shipment Numbers API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private GetShipmentNumbers RemoveSpaces(GetShipmentNumbers entity)
        {
            // we can use the reflection here to support the generalization (but there is a cost for for loop) - it needs discussion 
            GetShipmentNumbers newEntity = entity;
            newEntity.Direction = Regex.Replace(newEntity.Direction, @"\s+", "");
            newEntity.TransportMode = Regex.Replace(newEntity.TransportMode, @"\s+", "");
            newEntity.ShipmentLevel = Regex.Replace(newEntity.ShipmentLevel, @"\s+", "");
            newEntity.House = Regex.Replace(newEntity.House, @"\s+", "");
            newEntity.Master = Regex.Replace(newEntity.Master, @"\s+", "");
            newEntity.Carrier = Regex.Replace(newEntity.Carrier, @"\s+", "");
            newEntity.ContainerNumber = Regex.Replace(newEntity.ContainerNumber, @"\s+", "");
            return newEntity;
        }
    }
}