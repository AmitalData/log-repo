using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.IntegrationTest.Automation.Services;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.Helpers;

namespace Logitude.IntegrationTest.Automation.Tests.Shipments
{
    [TestClass]
    public class SetFieldsValueTest
    {
        AutomationTestServices automationTestServices;
        [TestMethod]
        public async Task OnCreateSetFieldsValueAutomation_Post_Successful()
        {
            automationTestServices = new AutomationTestServices();
            AutomationParams automationParams = GetNewSetFieldsValueAutomationParams("OnCreate");
            AutomationPM entityPM = automationTestServices.GetNewAutomationPM(automationParams);
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "AutomationExtended");
            AutomationPM automationPM = RestClientService.ParseResponse<AutomationPM>(response);
            Assert.IsNotNull(automationPM);
            Assert.IsNotNull(automationPM.Id);
        }

        [TestMethod]
        public async Task OnUpdateSetFieldsValueAutomation_Post_Successful()
        {
            automationTestServices = new AutomationTestServices();
            AutomationParams automationParams = GetNewSetFieldsValueAutomationParams("OnUpdate"); 
            AutomationPM entityPM = automationTestServices.GetNewAutomationPM(automationParams);
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "AutomationExtended");
            AutomationPM automationPM = RestClientService.ParseResponse<AutomationPM>(response);
            Assert.IsNotNull(automationPM);
            Assert.IsNotNull(automationPM.Id);
        }

        private AutomationParams GetNewSetFieldsValueAutomationParams(string AutomationType)
        {
            List<AutomationConditionParams> automationConditionLists = GetSetFieldsValueAutomationConditionParams();
            List<AutomationSetValueParams> automationSetValueLists = GetSetFieldsValueAutomationSetValueParams();

            AutomationParams automationParams = new AutomationParams
            {
                Name = AutomationType + "_SetFieldsValue",
                Type = AutomationType,
                ResultCode = "FIELDSET",
                ObjectTableId = "1-4",
                AutomationConditionLists = automationConditionLists,
                AutomationSetValueLists = automationSetValueLists,
            };

            return automationParams;
        }

        private List<AutomationConditionParams> GetSetFieldsValueAutomationConditionParams()
        {
            List<AutomationConditionParams> automationConditionLists = new List<AutomationConditionParams>
            {
                new AutomationConditionParams
                {
                    Value = "I",
                    ConditionType = "And",
                    ObjectFieldType = "LookUp",
                    ObjectFieldCode = "Shipment.DirectionId",
                    OperatorCode = "=",
                }
            };

            return automationConditionLists;
        }

        private List<AutomationSetValueParams> GetSetFieldsValueAutomationSetValueParams()
        {
            List<AutomationSetValueParams> automationSetValueLists = new List<AutomationSetValueParams>
            {
                new AutomationSetValueParams{
                    OperatorCode = "SV",
                    Value = "10",
                    FieldName = "Origin",
                    DataTypeCode = "Text",
                    ObjectFieldCode = "Shipment.Origin" ,
                }
            };

            return automationSetValueLists;
        }
    }
}
