using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for PaymentOrderWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PaymentOrderWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] NewPaymentRequest(byte[] requestParams)
        {
            MemoryStream memorystream = new MemoryStream(requestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(NewPaymentRequestParams));
            NewPaymentRequestParams newRequestParams = (NewPaymentRequestParams)serializer.Deserialize(memorystream);
            ICustomContext customContext = CustomContext.GetContext(newRequestParams.Tenant);
            PaymentOrderUpdateService paymentOrderService = new PaymentOrderUpdateService(customContext, new Dictionary<string, IContext>(), newRequestParams.Tenant);


            //INF_MSG_GenericResponseData responseData;
            PaymentOrderReplyResponseData responseData;

            if (newRequestParams.TestCase != null && newRequestParams.TestCase.Type == "webservice" && newRequestParams.TestCase.Code != "Real Logic")
            {
                //responseData = new INF_MSG_GenericResponseData();
                responseData = new PaymentOrderReplyResponseData();
                switch (newRequestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            PaymentOrderPM peymentorder = new PaymentOrderPM() {ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, Id = IdCounter.GetNumber("Customs.PaymentOrder", newRequestParams.Tenant), Tenant = newRequestParams.Tenant, IsClosed = false, PaymentNumber = newRequestParams.PaymentNumber, ImporterId = newRequestParams.ExternalId};
                            paymentOrderService.Update(peymentorder, true);
                            responseData.ApplicationID = peymentorder.Id;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send payment order message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new TSH_NG_3053_MSG8_AgentPaymentRequestMessageService();
                responseData = messageService.Send(newRequestParams);
            }
            //


            MemoryStream memstream = new MemoryStream();
            //XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            XmlSerializer ser = new XmlSerializer(typeof(PaymentOrderReplyResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendPaymentOrderRequest(byte[] requestParams)
        {
            MemoryStream memorystream = new MemoryStream(requestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(GenericRequestParams));
            GenericRequestParams newRequestParams = (GenericRequestParams)serializer.Deserialize(memorystream);

            INF_MSG_GenericResponseData responseData;
            if (newRequestParams.TestCase != null && newRequestParams.TestCase.Type == "webservice" && newRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
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
                            responseData.UserMessage = "this is a test fail exception for send payment order message!";
                            break;
                        }

                }

            }
            else
            {
                var messageService = new TSH_MSG6_AgentPaymentMessageService();
                responseData = messageService.Send(newRequestParams);
            }
            //responseData 

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
        public byte[] SendMasavPaymentsToAgentRequest(byte[] requestParams)
        {
            MemoryStream memorystream = new MemoryStream(requestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(MasavPaymentsToAgentRequestParams));
            MasavPaymentsToAgentRequestParams newRequestParams = (MasavPaymentsToAgentRequestParams)serializer.Deserialize(memorystream);

            MasavPaymentsToAgentResponseData responseData;
            if (newRequestParams.TestCase != null && newRequestParams.TestCase.Type == "webservice" && newRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new MasavPaymentsToAgentResponseData();
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
                            responseData.UserMessage = "this is a test fail exception for send payment order message!";
                            break;
                        }
                }

            }
            else
            {
                var messageService = new TSH_8368_MasavPaymentsToAgentMessagingService();
                responseData = messageService.Send(newRequestParams);
            }
            //responseData 

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(MasavPaymentsToAgentResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
}
