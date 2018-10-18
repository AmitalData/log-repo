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
    /// Summary description for DepositWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DepositWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] BankAccountToRefund(byte[] bankAccountToRefundParams)
        {
            MemoryStream memorystream = new MemoryStream(bankAccountToRefundParams);
            XmlSerializer serializer = new XmlSerializer(typeof(BankAccountToRefundRequestParams));
            BankAccountToRefundRequestParams newReqParams = (BankAccountToRefundRequestParams)serializer.Deserialize(memorystream);
            
            var messageService = new TPG_NG_2018_BankAccountToRefundUpdateReplayMessagingService();
            
            INF_MSG_GenericResponseData responseData = null;
            if (newReqParams.TestCase != null && newReqParams.TestCase.Type == "webservice" && newReqParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (newReqParams.TestCase.Code)
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
                
                messageService = new TPG_NG_2018_BankAccountToRefundUpdateReplayMessagingService();
                responseData = messageService.Send(newReqParams);
                
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
    }
}
