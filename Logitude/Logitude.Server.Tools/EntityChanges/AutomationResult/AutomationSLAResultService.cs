using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
  public  class AutomationSLAResultService : GeneralAutomationResultService, IAutomationResultService
    {

        AutomationResultArgs automationResultArgs { get; set; }
        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            List<Automation> setSLAAutomationsList = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "SETSLA").ToList();
            if (setSLAAutomationsList.Count > 0)
            {

                var otherObjectTableIdWithLastUpdate = (automationResultArgs.OtherAutomationObjectTable != null && !string.IsNullOrEmpty(automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate)) ? (automationResultArgs.OtherAutomationObjectTable.Id + "@" + automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate) : "";


                ApplySetSLAValueAutomation(automationResultArgs.EntityChangeArgs.EntityPM, setSLAAutomationsList, automationResultArgs.EntityChange, automationResultArgs.AutomationFieldLists, automationResultArgs.AutomationObjectTable.AutomationLastUpdate, automationResultArgs.EntityChangeArgs.OldEntityPM, automationResultArgs.EntityChangeArgs.ProcessType, automationResultArgs.EntityChangeArgs.EntityId, otherObjectTableIdWithLastUpdate);
            }
        }


        public void ApplySetSLAValueAutomation(Object entityPM, List<Automation> automationsList, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            int tenant = entityChange.Tenant;

            foreach (Automation automation in automationsList)
            {
                DateTime dateBefore = DateTime.Now;
                EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);

                entityChangesAutomation.ResultCode = "Set SLA Value";
                string lastUpdate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);
                ValidateAutomationResultClass validateResult = ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");
                entityChangesAutomation.ConditionsList = validateResult.ConditionsList;
                entityChangesAutomation.type = validateResult.IsAutomationValid ? "SetSsucceed" : "SetFailed";

                if (validateResult.IsAutomationValid)
                {
                    SetSLAValue(entityPM, entityChange, automationFieldLists, lastUpdate, automationResultArgs.MainEntityChangeService.EntityChangesAutomationsSsucceedList, automationResultArgs.MainEntityChangeService.Changefields, automation, entityChangesAutomation, dateBefore);
                }

                else
                {
                    entityChangesAutomation.ConditionsList = validateResult.ConditionsList;
                    entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                    automationResultArgs.MainEntityChangeService.EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                }
            }
        }

        public void SetSLAValue(Object entityPM, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList, List<c> fields, Automation automation, EntityChangeAutomation entityChangesAutomation, DateTime dateBefore)
        {
            AutomationSetSLAValue AutomationSetSLAValue = null;
            string automationSetSLAValueName = "AutomationSetSLAValue" + lastupdateautomation + automation.Id + automation.Tenant;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(automationSetSLAValueName) == null)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        AutomationSetSLAValue = AutomatedBackup.AutomationSetSLAValue;
                        CacheManager.CacheWrapper.Insert(automationSetSLAValueName, AutomationSetSLAValue, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    AutomationSetSLAValue = (AutomationSetSLAValue)CacheManager.CacheWrapper.Get(automationSetSLAValueName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    AutomationSetSLAValue = AutomatedBackup.AutomationSetSLAValue;
                }
            }

            #region Set SLA Value

            object oldValue;
            object newValue = "";

            PropertyInfo propInfo = entityPM.GetType().GetProperty("SLAId");

            if (propInfo != null)
            {
                oldValue = propInfo.GetValue(entityPM);
                newValue = AutomationSetSLAValue.SLAId;

                if (oldValue == null) oldValue = "";
                if (newValue == null) newValue = "";

                if (oldValue.ToString().ToLower() != newValue.ToString().ToLower())
                {
                    automationResultArgs.MainEntityChangeService.IsChangeSLA = true;
                    propInfo.SetValue(entityPM, newValue, null);
                    c fieldc = new c()
                    {
                        f = AutomationSetSLAValue.ObjectFieldCode,
                        o = oldValue.ToString(),
                        n = newValue.ToString(),
                    };

                    fields.Add(fieldc);
                }
            }

            #endregion

            entityChange.HasExecutedRecord = true;
            entityChangesAutomation.IsConditionTrue = true;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
            EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
        }

    }
}
