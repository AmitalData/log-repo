using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.IntegrationTest.Automation.Services;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.IntegrationTest.Automation.Tests.Shipments
{
    [TestClass]
    public class EmailTest
    {
        AutomationTestServices automationTestServices;
        [TestMethod]
        public async Task OnCreateEmailAutomation_Post_Successful()
        {
            automationTestServices = new AutomationTestServices();
            AutomationParams automationParams = GetNewEmailAutomationParams("OnCreate");
            AutomationPM entityPM = automationTestServices.GetNewAutomationPM(automationParams);
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "AutomationExtended");
            AutomationPM automationPM = RestClientService.ParseResponse<AutomationPM>(response);
            Assert.IsNotNull(automationPM);
            Assert.IsNotNull(automationPM.Id);
        }

        [TestMethod]
        public async Task OnUpdateEmailAutomation_Post_Successful()
        {
            automationTestServices = new AutomationTestServices();
            AutomationParams automationParams = GetNewEmailAutomationParams("OnUpdate");
            AutomationPM entityPM = automationTestServices.GetNewAutomationPM(automationParams);
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "AutomationExtended");
            AutomationPM automationPM = RestClientService.ParseResponse<AutomationPM>(response);
            Assert.IsNotNull(automationPM);
            Assert.IsNotNull(automationPM.Id);
        }

        private AutomationParams GetNewEmailAutomationParams(string AutomationType)
        {
            List<AutomationConditionParams> automationConditionLists = GetEmailAutomationConditionParams();
            AutomationEmailDetails automationEmailDetails = GetEmailAutomationEmailDetails();
            AutomationParams automationParams = new AutomationParams
            {
                Name = AutomationType + "_Email",
                Type = AutomationType,
                ResultCode = "Email",
                ObjectTableId = "1-4",
                DocumentTypeId = "1-3163", //
                TemplateId = "1-336", //
                AutomationConditionLists = automationConditionLists,
                AutomationEmailDetails = automationEmailDetails,
            };

            return automationParams;
        }

        private List<AutomationConditionParams> GetEmailAutomationConditionParams()
        {
            List<AutomationConditionParams> automationConditionLists = new List<AutomationConditionParams>
            {
                new AutomationConditionParams
                {
                    Value = "I",
                    ConditionType = "And",
                    ObjectFieldType = "LookUp",
                    ObjectFieldCode = "Shipment.DirectionId",
                }
            };

            return automationConditionLists;
        }

        private AutomationEmailDetails GetEmailAutomationEmailDetails()
        {
            AutomationEmailDetails automationEmailDetails = new AutomationEmailDetails
            {
                Delaytime = 0,
                DelaytimeOp = null,
                SelectedDelaytimeFieldCode = null,
                DelaytimeIndicator = "OO",
                Type = "Immeduiatly",
            };

            return automationEmailDetails;
        }

    }
}
