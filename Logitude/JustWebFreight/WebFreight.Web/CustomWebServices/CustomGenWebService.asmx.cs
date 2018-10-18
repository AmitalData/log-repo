using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
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
    /// Summary description for GenWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomGenWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] SendCreditQuery(byte[] creditQueryParams)
        {
            MemoryStream memorystream = new MemoryStream(creditQueryParams);
            XmlSerializer serializer = new XmlSerializer(typeof(CreditQueryRequestParams));
            CreditQueryRequestParams creditQueryRequestParams = (CreditQueryRequestParams)serializer.Deserialize(memorystream);


            CreditQueryResponseData responseData = null;
            if (creditQueryRequestParams.TestCase != null && creditQueryRequestParams.TestCase.Type == "webservice" && creditQueryRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new CreditQueryResponseData();
                switch (creditQueryRequestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {              
                var messageService = new TSH_NG_8289_Web05_CreditQueryMessagingService();
                responseData = messageService.Send(creditQueryRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CreditQueryResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendSpecialActivityQuery(byte[] specialActivityParams)
        {
            MemoryStream memorystream = new MemoryStream(specialActivityParams);
            XmlSerializer serializer = new XmlSerializer(typeof(SpecialActivityRequestParams));
            SpecialActivityRequestParams specialActivityRequestParams = (SpecialActivityRequestParams)serializer.Deserialize(memorystream);


            INF_MSG_GenericResponseData responseData = null;
            if (specialActivityRequestParams.TestCase != null && specialActivityRequestParams.TestCase.Type == "webservice" && specialActivityRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (specialActivityRequestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new ST_NG_40_MSG7_SpecialActivityRequestMessagingService();
                responseData = messageService.Send(specialActivityRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }


        [WebMethod]
        public byte[] SendCourierBOLQuery(byte[] courierBOLParams)
        {
            MemoryStream memorystream = new MemoryStream(courierBOLParams);
            XmlSerializer serializer = new XmlSerializer(typeof(CourierBOLQueryRequestParams));
            CourierBOLQueryRequestParams courierBOLQueryRequestParams = (CourierBOLQueryRequestParams)serializer.Deserialize(memorystream);


            CourierBOLQueryResponseData responseData = null;
            if (courierBOLQueryRequestParams.TestCase != null && courierBOLQueryRequestParams.TestCase.Type == "webservice" && courierBOLQueryRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new CourierBOLQueryResponseData();
                switch (courierBOLQueryRequestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new MN_NG_9022_CourierBOLQueryMessagingService();
                responseData = messageService.Send(courierBOLQueryRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CourierBOLQueryResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendCustomsBookInQuery(byte[] customsBookIn)
        {
            MemoryStream memorystream = new MemoryStream(customsBookIn);
            XmlSerializer serializer = new XmlSerializer(typeof(CustomsBookInRequestParams));
            CustomsBookInRequestParams customsBookInRequestParams = (CustomsBookInRequestParams)serializer.Deserialize(memorystream);


            CustomsBookInResponseData responseData = null;
            if (customsBookInRequestParams.TestCase != null && customsBookInRequestParams.TestCase.Type == "webservice" && customsBookInRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new CustomsBookInResponseData();
                switch (customsBookInRequestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new CBC_NG_8361_MSG01_CustomsBookInMessagingService();
                responseData = messageService.Send(customsBookInRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomsBookInResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendCustomItemLegalDemandsQuery(byte[] customsBookIn)
        {
            MemoryStream memorystream = new MemoryStream(customsBookIn);
            XmlSerializer serializer = new XmlSerializer(typeof(CustomItemLegalDemandsRequestParams));
            CustomItemLegalDemandsRequestParams customItemLegalDemandsRequestParams = (CustomItemLegalDemandsRequestParams)serializer.Deserialize(memorystream);

            CustomItemLegalDemandsResponseData responseData = null;
            if (customItemLegalDemandsRequestParams.TestCase != null && customItemLegalDemandsRequestParams.TestCase.Type == "webservice" && customItemLegalDemandsRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new CustomItemLegalDemandsResponseData();
                switch (customItemLegalDemandsRequestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new CB_NG_8316_CustomItemLegalDemandsMessagingService();
                responseData = messageService.Send(customItemLegalDemandsRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomItemLegalDemandsResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
}
