using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
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
    /// Summary description for SendCustomsMessagesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SendCustomsMessagesWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void SendMessageToQueue(byte[] data)
        {
            try
            {

                string messagedata = System.Text.Encoding.UTF8.GetString(data);
                BrokeredMessage currentMessage = new BrokeredMessage(new MemoryStream(data), true);
                //message.Properties["MessageXML"] = messagedata;

                string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("CustomsQueue");

                QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);

                client.Send(currentMessage);
            }
            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
            }
        }

        [WebMethod]
        public byte[] SendBlockListInWarehouseFilterRequest(byte[] BlockListInWarehouseRequestParams)
        {
            MemoryStream memorystream = new MemoryStream(BlockListInWarehouseRequestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(BlockListInWarehouseRequestParams));
            BlockListInWarehouseRequestParams newBlockListInWarehouseRequestParams = (BlockListInWarehouseRequestParams)serializer.Deserialize(memorystream);

            BlockListInWarehouseResponseData responseData = null;
            if (newBlockListInWarehouseRequestParams.TestCase != null && newBlockListInWarehouseRequestParams.TestCase.Type == "webservice" && newBlockListInWarehouseRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new BlockListInWarehouseResponseData();
                switch (newBlockListInWarehouseRequestParams.TestCase.Code)
                {
                    case "Search Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            //responseData.ResponseStatusXML = "Test Status String to be xml";
                            break;
                        }
                    case "Search Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search Declaration Status message!";
                            break;
                        }
                }
            }
            else
            {
                // use messageing service
                var service = new ST_8330_BlockListInWarehouseFilterMessagingService();
                responseData = service.Send(newBlockListInWarehouseRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(BlockListInWarehouseResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendCustomsMessageRestoreRequest(byte[] MessageRestoreRequestParams)
        {
            MemoryStream memorystream = new MemoryStream(MessageRestoreRequestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(MessageRestoreRequestParams));
            MessageRestoreRequestParams newMessageRestoreRequestParams = (MessageRestoreRequestParams)serializer.Deserialize(memorystream);

            MessageRestoreResponseData responseData = null;
            if (newMessageRestoreRequestParams.TestCase != null && newMessageRestoreRequestParams.TestCase.Type == "webservice" && newMessageRestoreRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new MessageRestoreResponseData();
                switch (newMessageRestoreRequestParams.TestCase.Code)
                {
                    case "Search Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            //responseData.ResponseStatusXML = "Test Status String to be xml";
                            break;
                        }
                    case "Search Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search Declaration Status message!";
                            break;
                        }
                }
            }
            else
            {
                // use messageing service
                //MessageRestoreRequestParams, MessageRestoreResponseData,
#if true
                var service = new SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService();
                responseData = service.Send(newMessageRestoreRequestParams);
#else

                responseData = new MessageRestoreResponseData() { HasException = true, ExceptionMessage = "merge20150121" };
#endif

            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(MessageRestoreResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
}
