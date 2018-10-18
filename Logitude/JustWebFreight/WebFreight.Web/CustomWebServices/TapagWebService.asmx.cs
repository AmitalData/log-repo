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
    /// Summary description for TapagWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TapagWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] SendDeficitFileFilterRequest(byte[] deficitFileFilterParams)
        {
            MemoryStream memorystream = new MemoryStream(deficitFileFilterParams);
            XmlSerializer serializer = new XmlSerializer(typeof(DeficitFileFilterRequestParams));
            DeficitFileFilterRequestParams newRequestParams = (DeficitFileFilterRequestParams)serializer.Deserialize(memorystream);


            DeficitFilesDetailResponseData responseData = null;
            if (newRequestParams.TestCase != null && newRequestParams.TestCase.Type == "webservice" && newRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new DeficitFilesDetailResponseData();
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
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new TPG_8304_DeficitFileFilterParamMessagingService();
                responseData = messageService.Send(newRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(DeficitFilesDetailResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
}
