using Logitude.Customs.BL.CloseTables;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class DCASincroService
    {
        public string BuildDCAMessage(GenericRequestParams requestParamsData)
        {
            var mySincroTestCaseDetails = new SincroTestCaseDetails();
            var sincroTestCaseDetail = mySincroTestCaseDetails.GetAllSincroTestCaseDetails()
                .First(r => r.Code == requestParamsData.TestCase.Code);
            requestParamsData.InterfaceTypeCode = requestParamsData.TestCase.Code;
            requestParamsData.MainInterfaceCode = requestParamsData.TestCase.Code;
            var messagingService = MessagingServiceFactoryHelper.GetMessagingService(sincroTestCaseDetail.MainInterfaceCode, "FAKFAKE");
            string result = "";
            dynamic params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            if (Convert.ToString(params1.MasterLevel) == "true")
            {
                FAKE_CourierMasterDeclarations fAKE_CourierMasterDeclarations = new FAKE_CourierMasterDeclarations();
                var decList = fAKE_CourierMasterDeclarations.GetCourierMasterDeclarations(requestParamsData.AppicationId, requestParamsData.Tenant);
                foreach (string dec in decList)
                {
                    requestParamsData.AppicationId = dec;
                    result = messagingService.CreateFakeDCA(requestParamsData);
                }
            }
            if (Convert.ToString(params1.CourierLevel) == "true" || Convert.ToString(params1.CourierLevel)==null)
            {
                result = messagingService.CreateFakeDCA(requestParamsData);
            }
            return result;
        }
    }
}
