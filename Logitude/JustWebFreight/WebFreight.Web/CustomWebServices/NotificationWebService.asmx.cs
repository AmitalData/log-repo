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
    /// Summary description for NotificationWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotificationWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] SendNotificationReplyRequest(byte[] requestParams)
        {
            MemoryStream memorystream = new MemoryStream(requestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(MessageToAgentRequestParams));
            MessageToAgentRequestParams newRequestParams = (MessageToAgentRequestParams)serializer.Deserialize(memorystream);

            //INF_MSG_GenericResponseData responseData;
            MessageToAgentResponseData responseData;  //Yuval Chalup 18.06.2015 TASK-13858
            if (newRequestParams.TestCase != null && newRequestParams.TestCase.Type == "webservice" && newRequestParams.TestCase.Code != "Real Logic")
            {
                //responseData = new INF_MSG_GenericResponseData();
                responseData = new MessageToAgentResponseData();  //Yuval Chalup 18.06.2015 TASK-13858
                switch (newRequestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for send notification reply message!";
                            break;
                        }

                }

            }
            else
            {
                //responseData = new INF_MSG_GenericResponseData() { HasException = true, UserMessage = "merge20150121" };
                //<--- Yuval Chalup 18.06.2015 TASK-13858
                var messagingService = new DOC_NG_5101_GNMessageToAgentMessagingService();
                responseData = messagingService.Send(newRequestParams);
                //Yuval Chalup 18.06.2015 TASK-13858 --->
            }
            //


            MemoryStream memstream = new MemoryStream();
            //XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            XmlSerializer ser = new XmlSerializer(typeof(MessageToAgentResponseData));  //Yuval Chalup 18.06.2015 TASK-13858
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

    }
}
