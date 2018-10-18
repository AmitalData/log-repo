using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using UnifreightIIG.Common.ChangingTimeServiceReference;
using UnifreightIIG.Common.ClientSdk;
using Logitude.Customs.BL;
using WebFreight.Web.CustomModel;
using System.Configuration;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for CustomCheckWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomCheckWebService : System.Web.Services.WebService
    {
      

        [WebMethod(true)]
        public byte[] SendCheckRequest(byte[] checkParamsBytes)
        {
           
            MemoryStream memorystream = new MemoryStream(checkParamsBytes);
            XmlSerializer serializer = new XmlSerializer(typeof(CH_NG_191_MSG2_ChangingTimeRequestParams));
            CH_NG_191_MSG2_ChangingTimeRequestParams checkParams = (CH_NG_191_MSG2_ChangingTimeRequestParams)serializer.Deserialize(memorystream);

            CH_NG_192_MSG3_ApproveChangeTimeResponseData responseData;

            PhysicalCheckQueryService query = new PhysicalCheckQueryService(checkParams.Tenant);
            PhysicalCheckPM physicalCheck = query.GetSingle(checkParams.PhysicalCheckId, false, false);
            ICustomContext customContext = CustomContext.GetContext(checkParams.Tenant);
            PhysicalCheckUpdateService updateService = new PhysicalCheckUpdateService(customContext, new Dictionary<string, IContext>(), checkParams.Tenant);

            if (checkParams.TestCase != null && checkParams.TestCase.Type == "webservice" && checkParams.TestCase.Code != "Real Logic")
            {
                responseData = new CH_NG_192_MSG3_ApproveChangeTimeResponseData();
                switch (checkParams.TestCase.Code)
                {
                    case "Available Time List Succeeded":
                        {
                            int minutes = 0;
                            Random hoursRand = new Random();
                            Random minRand = new Random();
                            var dates = new List<DateTime?>();
                            if (checkParams.DateSearchFromSpecified && checkParams.DateSearchToSpecified)
                            {
                                for (var dt = checkParams.DateSearchFrom.Value; dt <= checkParams.DateSearchTo.Value; dt = dt.AddHours(3))
                                {

                                    minutes = minRand.Next(60);
                                    dt = dt.AddMinutes(minutes);
                                    dates.Add(dt);
                                }
                            }

                            responseData.XrayItems = dates.ToList();
                            responseData.HasException = false;
                            responseData.Succeeded = true;

                            break;
                        }
                    case "Available Time List Fail":
                        {
                           
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "This is a test exception to show the failure of getting available time list!";

                            break;
                        }
                    case "Choose Specific Time":
                        {

                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            physicalCheck.LimitDate = checkParams.QueueDate;
                            physicalCheck.ChangeSetOp = ChangeSetOperation.Update;
                            updateService.Update(physicalCheck, true);
                            
                            break;
                        }
                }
                
            }
            else
            {
                CH_NG_191_MSG2_ChangingTimeRequestMessagingService messagingService = new CH_NG_191_MSG2_ChangingTimeRequestMessagingService();
                responseData = messagingService.Send(checkParams);
            }
            
            
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CH_NG_192_MSG3_ApproveChangeTimeResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
           
        }

       
        
      
    }
}
