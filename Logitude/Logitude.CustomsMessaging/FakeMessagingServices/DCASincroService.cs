using Logitude.Customs.BL.CloseTables;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
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
            var sincroTestCaseDetail =mySincroTestCaseDetails.GetAllSincroTestCaseDetails()
                .First(r => r.Code == requestParamsData.TestCase.Code);
            var messagingService =MessagingServiceFactoryHelper.GetMessagingService(sincroTestCaseDetail.MainInterfaceCode, "FAKFAKE");
            string result=messagingService.CreateFakeDCA(requestParamsData);
            return result;
        }
    }
}
