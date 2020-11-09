using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.Helpers;

namespace Logitude.IntegrationTest.Automation.Tests.Shipments
{
    [TestClass]
    public class OnCreate_SetFieldsValue
    {
        [TestMethod]
        public async Task CreateSetFieldsValueAutomation_Post_Successful()
        {
            AutomationPM entityPM = GetNewAutomationPM();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "AutomationExtended");
            AutomationPM automationPM = RestClientService.ParseResponse<AutomationPM>(response);
            Assert.IsNotNull(automationPM);
            Assert.IsNotNull(automationPM.Id);
        }

        private AutomationPM GetNewAutomationPM()
        {
            AutomationPM automationPM = new AutomationPM();
            automationPM.Tenant = IntegrationTestLoginParameters.Tenant;
            automationPM.Name = "OnCreate_SetFieldsValue";
            automationPM.ObjectTableId = "1-4";
            automationPM.Type = "OnCreate";
            automationPM.ResultCode = "FIELDSET";
            automationPM.Description = "OnCreate SetFieldsValue";
            automationPM.CreateDate = DateTime.UtcNow;
            automationPM.UpdateDate = DateTime.UtcNow;
            automationPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            automationPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            automationPM.AutomatedDataBackup = new AutomatedBackup
            {
                Name = "OnCreate_SetFieldsValue",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                ResultCode = "FIELDSET",
                Description = "OnCreate SetFieldsValue",
                Delaytime = 0,
                DelaytimeOp = null,
                SelectedDelaytimeFieldCode = null,
                DelaytimeIndicator = "OO",
                Type = "Immeduiatly",
                AautomationConditionLists = GetAutomationConditionLists(),
                AutomationSetValueLists = GetAutomationSetValueLists(),
            };
            return automationPM;
        }

        private List<AutomationCondition> GetAutomationConditionLists()
        {
            List<AutomationCondition> automationConditions = new List<AutomationCondition>();
            automationConditions.Append(new AutomationCondition
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                Value = "I",
                ConditionType = "And",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                ObjectFieldType = "LookUp",
                ObjectFieldCode = "Shipment.DirectionId"
            });

            return automationConditions;
        }

        private List<AutomationSetValue> GetAutomationSetValueLists()
        {
            List<AutomationSetValue> automationConditions = new List<AutomationSetValue>();
            automationConditions.Append(new AutomationSetValue
            {
                OperatorCode = "SV",
                Value = "10",
                FieldName = "Origin",
                DataTypeCode = "Text",
                ObjectFieldCode = "Shipment.Origin"
            });

            return automationConditions;
        }
    }
}
