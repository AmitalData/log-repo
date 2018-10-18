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
    /// Summary description for GuaranteeWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GuaranteeWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] GuaranteeReturnRequest(byte[] guaranteeReturnParams)
        {
            MemoryStream memorystream = new MemoryStream(guaranteeReturnParams);
            XmlSerializer serializer = new XmlSerializer(typeof(GuaranteeReturnRequesRequestParams));
            GuaranteeReturnRequesRequestParams newSearchClientParams = (GuaranteeReturnRequesRequestParams)serializer.Deserialize(memorystream);
            

            INF_MSG_GenericResponseData responseData = null;
            if (newSearchClientParams.TestCase != null && newSearchClientParams.TestCase.Type == "webservice" && newSearchClientParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (newSearchClientParams.TestCase.Code)
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
                
#if true
                var messageService = new GRNT_5004_GuaranteeReturnRequestMessagingService();
                responseData = messageService.Send(newSearchClientParams);
#else

                responseData = new INF_MSG_GenericResponseData() { HasException = true, ExceptionMessage = "merge20150121" };
#endif
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

        //<--- Yuval Chalup 08.09.2015 TASK-15038
        [WebMethod]
        public byte[] GuaranteeCertificateRequest(byte[] guaranteeCertificateParams)
        {
            MemoryStream memorystream = new MemoryStream(guaranteeCertificateParams);
            XmlSerializer serializer = new XmlSerializer(typeof(GuaranteeCertificateRequestParams));
            GuaranteeCertificateRequestParams newSearchClientParams = (GuaranteeCertificateRequestParams)serializer.Deserialize(memorystream);
            GuaranteeCertificateResponseData responseData = null;
            if (newSearchClientParams.TestCase != null && newSearchClientParams.TestCase.Type == "webservice" && newSearchClientParams.TestCase.Code != "Real Logic")
            {
                responseData = new GuaranteeCertificateResponseData();
                switch (newSearchClientParams.TestCase.Code)
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
                var messageService = new TPG_NG_8306_Web07_GuaranteeCertificateMessagingService();
                responseData = messageService.Send(newSearchClientParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(GuaranteeCertificateResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        //Yuval Chalup 08.09.2015 TASK-15038--->
    }
}
