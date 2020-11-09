using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs
{
    public class ShipmentNumbersController : ApiController
    {
        public HttpResponseMessage Post(ShipmentNumbers entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticateAPICall(authToken.Tenant);

                    // Validate the tags 


                    // return the response 
                    var response = new Shipments
                    {
                        ShipmentList = new List<ShipmentResponseItem> {
        new ShipmentResponseItem {
            ShipmentNumber ="123",
            MasterShipmentNumber = "Event 1"
        },
        new ShipmentResponseItem {
            ShipmentNumber ="123",
            MasterShipmentNumber = "Event 2"
        },


    }
                    };
                    string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(response);
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
    }
    [XmlRoot("ShipmentNumbers")]
    public class Shipments
    {
        [XmlArray("Shipments")]
        [XmlArrayItem("Shipment")]
        public List<ShipmentResponseItem> ShipmentList { get; set; }

        public Shipments()
        {
            this.ShipmentList = new List<ShipmentResponseItem>();
        }
    }
    public class ShipmentResponseItem
    {
        public string ShipmentNumber { get; set; }
        public string MasterShipmentNumber { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}