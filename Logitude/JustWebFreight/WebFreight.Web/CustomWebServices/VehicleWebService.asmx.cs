using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for VehicleWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VehicleWebService : System.Web.Services.WebService
    {

        [WebMethod]
    
        public byte[] UpdateVehicle(byte[] vehicleParamsData, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(vehicleParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(UpdateDeleteVehicleRequestParams));
            UpdateDeleteVehicleRequestParams vehicleParams = (UpdateDeleteVehicleRequestParams)serializer.Deserialize(memorystream);
            var messageService = new VP_NG_2690_VehicleInMessagingService();

            INF_MSG_GenericResponseData responseData;
            if (vehicleParams.TestCase != null && vehicleParams.TestCase.Type == "webservice" && vehicleParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (vehicleParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            ICustomContext customContext = CustomContext.GetContext(tenant);
                            VehicleQueryService vehicleQuery = new VehicleQueryService(tenant);
                            VehiclePM vehicle = vehicleQuery.GetSingle(vehicleParams.VehicleId, false, false);

                            if (vehicleParams.IsDelete)
                            {
                                vehicle.StatusCode = "4";
                            }


                            vehicle.ChangeSetOp = ChangeSetOperation.Update;
                            VehicleUpdateService updateService = new VehicleUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                            updateService.Update(vehicle, true);

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send vehicle message!";
                            break;
                        }
                   
                }

            }
            else
            {
                messageService = new VP_NG_2690_VehicleInMessagingService();
                responseData = messageService.Send(vehicleParams);
            }


            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(responseData.GetType());
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }


       [WebMethod]
        public byte[] NewVehicle(byte[] vehicleParamsData, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(vehicleParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(UpdateDeleteVehicleRequestParams));
            UpdateDeleteVehicleRequestParams vehicleParams = (UpdateDeleteVehicleRequestParams)serializer.Deserialize(memorystream);
            var messageService = new VP_NG_2690_VehicleInMessagingService();

            INF_MSG_GenericResponseData responseData;
            if (vehicleParams.TestCase != null && vehicleParams.TestCase.Type == "webservice" && vehicleParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (vehicleParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;


                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send vehicle message!";
                            break;
                        }

                }

            }
            else
            {
                messageService = new VP_NG_2690_VehicleInMessagingService();
                responseData = messageService.Send(vehicleParams);
            }


            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(responseData.GetType());
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

     
    }
}
