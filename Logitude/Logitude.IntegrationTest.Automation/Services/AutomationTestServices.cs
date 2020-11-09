using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Automation.Services
{
    class AutomationTestServices
    {
        public AutomationPM GetNewAutomationPM(AutomationParams automationParams)
        {
            AutomationPM automationPM = new AutomationPM
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                Name = automationParams.Name,
                ObjectTableId = automationParams.ObjectTableId,
                Type = automationParams.Type,
                ResultCode = automationParams.ResultCode,
                Description = automationParams.Name,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                DocumentTypeId = automationParams.DocumentTypeId,
                TemplateId = automationParams.TemplateId,
                Version = 1,
                CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                AutomatedDataBackup = GetAutomatedDataBackup(automationParams)
            };

            return automationPM;
        }

        private AutomatedBackup GetAutomatedDataBackup(AutomationParams automationParams)
        {
            AutomatedBackup automatedBackup = new AutomatedBackup
            {
                Name = automationParams.Name,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                ResultCode = automationParams.ResultCode,
                Description = automationParams.Name,
                DelaytimeIndicator = "OO",
                Version = 1,
                AautomationConditionLists = GetAutomationConditionLists(automationParams.AutomationConditionLists),
                AutomationSetValueLists = GetAutomationSetValueLists(automationParams.AutomationSetValueLists),
            };
            automatedBackup = GetAdditionalDataForAutomatedBackup(automationParams, automatedBackup);

            return automatedBackup;
        }

        private List<AutomationCondition> GetAutomationConditionLists(List<AutomationConditionParams> automationConditionLists)
        {
            List<AutomationCondition> automationConditions = new List<AutomationCondition>();
            if (automationConditionLists != null)
            {
                automationConditionLists.ForEach(automationCondition =>
                {
                    automationConditions.Add(new AutomationCondition
                    {
                        Tenant = IntegrationTestLoginParameters.Tenant,
                        Value = automationCondition.Value,
                        ConditionType = automationCondition.ConditionType,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        OperatorCode = automationCondition.OperatorCode,
                        CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                        UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                        ObjectFieldType = automationCondition.ObjectFieldType,
                        ObjectFieldCode = automationCondition.ObjectFieldCode,
                    });
                });
            }
            return automationConditions;
        }

        private List<AutomationSetValue> GetAutomationSetValueLists(List<AutomationSetValueParams> automationSetValueLists)
        {
            List<AutomationSetValue> automationSetValues = new List<AutomationSetValue>();
            if (automationSetValueLists != null)
            {
                automationSetValueLists.ForEach(automationSetValue =>
                {
                    automationSetValues.Add(new AutomationSetValue
                    {
                        OperatorCode = automationSetValue.OperatorCode,
                        Value = automationSetValue.Value,
                        FieldName = automationSetValue.FieldName,
                        DataTypeCode = automationSetValue.DataTypeCode,
                        ObjectFieldCode = automationSetValue.ObjectFieldCode,
                    });

                });
            }
            return automationSetValues;
        }
    
        private AutomatedBackup GetAdditionalDataForAutomatedBackup(AutomationParams automationParams, AutomatedBackup automatedBackup)
        {
            switch (automationParams.ResultCode)
            {
                case "Email":
                    if (automationParams.AutomationEmailDetails != null)
                    {
                        automatedBackup.Delaytime = automationParams.AutomationEmailDetails.Delaytime;
                        automatedBackup.DelaytimeOp = automationParams.AutomationEmailDetails.DelaytimeOp;
                        automatedBackup.SelectedDelaytimeFieldCode = automationParams.AutomationEmailDetails.SelectedDelaytimeFieldCode;
                        automatedBackup.DelaytimeIndicator = automationParams.AutomationEmailDetails.DelaytimeIndicator;
                        automatedBackup.Type = automationParams.AutomationEmailDetails.Type;
                    }
                    break;
                default:
                    break;
            }

            return automatedBackup;
        }
    }

    class AutomationParams
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ResultCode { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentTypeId { get; set; }
        public string TemplateId { get; set; }
        public List<AutomationConditionParams> AutomationConditionLists { get; set; }
        public List<AutomationSetValueParams> AutomationSetValueLists { get; set; }
        public AutomationEmailDetails AutomationEmailDetails { get; set; }

    }
    
    class AutomationConditionParams
    {
        public string Value { get; set; }
        public string ConditionType { get; set; }
        public string ObjectFieldType { get; set; }
        public string ObjectFieldCode { get; set; }
        public string OperatorCode { get; set; }
    }

    class AutomationSetValueParams
    {
        public string Value { get; set; }
        public string FieldName { get; set; }
        public string DataTypeCode { get; set; }
        public string ObjectFieldCode { get; set; }
        public string OperatorCode { get; set; }
    }

    class AutomationEmailDetails
    {
        public int Delaytime { get; set; }
        public string DelaytimeOp { get; set; }
        public string SelectedDelaytimeFieldCode { get; set; }
        public string DelaytimeIndicator { get; set; }
        public string Type { get; set; }
    }
}
