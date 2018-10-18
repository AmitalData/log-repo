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
    /// Summary description for ClaimWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ClaimWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] SendClaimFileFilter(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(TPG_NG_8244_ClaimFileFilterRequestParams));
            TPG_NG_8244_ClaimFileFilterRequestParams requestParams = (TPG_NG_8244_ClaimFileFilterRequestParams)serializer.Deserialize(memorystream);
            TPG_NG_8245_ClaimFilesDetailResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new TPG_NG_8245_ClaimFilesDetailResponseData();
                switch (requestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                            break;
                        }
                }

            }
            else
            {
                // use messageing service
                var messagingService = new TPG_NG_8244_ClaimFileFilterParamMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(TPG_NG_8245_ClaimFilesDetailResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendClaim(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CLAIM_2340_ClaimRequestRequestParams));
            CLAIM_2340_ClaimRequestRequestParams requestParams = (CLAIM_2340_ClaimRequestRequestParams)serializer.Deserialize(memorystream);
            ClaimAnswerResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new ClaimAnswerResponseData();
                switch (requestParams.TestCase.Code)
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
                            responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                            break;
                        }
                }
            }
            else
            {
                // use messageing service
                var messagingService = new CLAIM_2340_ClaimRequestMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ClaimAnswerResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
}
