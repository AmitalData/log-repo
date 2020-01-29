using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.AutomationModel
{
    public class AutomationMetaDataUpdateService
    {
        public void UpdateAutomationMetaData()
        {
            try
            {
                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(0);
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(0);
                List<ObjectFieldList> objectFieldLists = objectFieldQuery.GetObjectFieldsForAutomations();
                Task updateAutomationDataTask = new Task(() => UpdateAutomationData(objectFieldLists)); updateAutomationDataTask.Start();
                Task updateAutomationHistorysTask = new Task(() => UpdateAutomationHistorysData(objectFieldLists)); updateAutomationHistorysTask.Start();
                Task UpdateAutomationResultEmailRecipientTask = new Task(() => UpdateAutomationResultEmailRecipient(objectFieldLists)); UpdateAutomationResultEmailRecipientTask.Start();
                Task updateEntityChangesTask = new Task(() => UpdateEntityChanges(objectFieldLists)); updateEntityChangesTask.Start();
                updateAutomationDataTask.Wait();
                updateAutomationHistorysTask.Wait();
                UpdateAutomationResultEmailRecipientTask.Wait();
                updateEntityChangesTask.Wait();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "UpdateAutomationMetaData", null, null);
            }

        }

        private void UpdateEntityChanges(List<ObjectFieldList> objectFieldLists)
        {
            TenantQuery tenantQuery = new TenantQuery(0);
            List<TenantList> tenantList = tenantQuery.GetTenantLists();
            foreach (TenantList tenant in tenantList)
            {
                UpdateEntityChangesData(objectFieldLists, tenant);
            }
        }

        private void UpdateAutomationResultEmailRecipient(List<ObjectFieldList> objectFieldLists)
        {
            AutomationResultEmailRecipientRepository automationResultEmailRecipientRepository = new AutomationResultEmailRecipientRepository(0);
            List<AutomationResultEmailRecipient> automationResultEmailRecipients = automationResultEmailRecipientRepository.GetAutomationResultEmailRecipient().Where(d => d.RecipientType != "Fixed").ToList();
            if (automationResultEmailRecipients.Count() > 0)
            {
                int loopCount = 0;
                foreach (AutomationResultEmailRecipient automationResultEmailRecipient in automationResultEmailRecipients)
                {
                    loopCount += 1;
                    automationResultEmailRecipient.RecipientValue = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationResultEmailRecipient.RecipientValue);
                    automationResultEmailRecipientRepository.Update(automationResultEmailRecipient);
                    if (loopCount == 1000)
                    {
                        automationResultEmailRecipientRepository.SubmitChanges();
                        loopCount = 0;
                    }
                }

                if (loopCount > 0) automationResultEmailRecipientRepository.SubmitChanges();

            }
        }

        private void UpdateAutomationHistorysData(List<ObjectFieldList> objectFieldLists)
        {
            AutomationHistoryRepository automationHistoryRepository = new AutomationHistoryRepository(0);
            List<AutomationHistory> automationHistorys = automationHistoryRepository.All();
            if (automationHistorys.Count() > 0)
            {
                int loopCount = 0;
                foreach (AutomationHistory automationHistory in automationHistorys)
                {
                    if (!string.IsNullOrEmpty(automationHistory.AutomationXML))
                    {
                        loopCount += 1;
                        AutomatedBackup automatedBackup = UpdateAutomatedBackup(objectFieldLists, automationHistory.AutomationXML);
                        automationHistory.AutomationXML = automatedBackup != null ? LogitudeXmlSerializer.SerializeObjectToXmlString(automatedBackup) : null;
                        automationHistoryRepository.Update(automationHistory);
                    }

                    if (loopCount == 1000)
                    {
                        automationHistoryRepository.SubmitChanges();
                        loopCount = 0;
                    }
                }
                if (loopCount > 0) automationHistoryRepository.SubmitChanges();

            }
        }

        private void UpdateAutomationData(List<ObjectFieldList> objectFieldLists)
        {
            AutomationRepository automationRepository = new AutomationRepository(0);
            List<Automation> automations = automationRepository.All();
            if (automations.Count() > 0)
            {
                int loopCount = 0;
                foreach (Automation automation in automations)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        loopCount += 1;
                        AutomatedBackup automatedBackup = UpdateAutomatedBackup(objectFieldLists, automation.AutomationXML);
                        automation.AutomationXML = automatedBackup != null ? LogitudeXmlSerializer.SerializeObjectToXmlString(automatedBackup) : null;
                        automationRepository.Update(automation);

                        if (loopCount == 1000)
                        {
                            automationRepository.SubmitChanges();
                            loopCount = 0;
                        }
                    }
                }

                if (loopCount > 0) automationRepository.SubmitChanges();

            }
        }

        private void UpdateEntityChangesData(List<ObjectFieldList> objectFieldLists, TenantList tenant)
        {
            EntityChangeRepository entityChangeRepository = new EntityChangeRepository(0);
            List<EntityChange> entityChanges = entityChangeRepository.GetEntityChanges(tenant.Id).ToList();
            if (entityChanges.Count() > 0)
            {
                int loopCount = 0;
                foreach (EntityChange entityChange in entityChanges)
                {
                    if (!string.IsNullOrEmpty(entityChange.ChangesAutomationFieldsXml) && !string.IsNullOrEmpty(entityChange.AutomationConditionFieldsXml))
                    {
                        loopCount += 1;
                        entityChange.ChangesAutomationFieldsXml = UpdateChangesAutomationFieldsXml(objectFieldLists, entityChange.ChangesAutomationFieldsXml);
                        entityChange.AutomationConditionFieldsXml = UpdateAutomationConditionFieldsXml(objectFieldLists, entityChange.AutomationConditionFieldsXml);
                        entityChangeRepository.Update(entityChange);
                    }

                    if (loopCount == 1000)
                    {
                        entityChangeRepository.SubmitChanges(); loopCount = 0;
                    }
                }
                if (loopCount > 0) entityChangeRepository.SubmitChanges();
            }
        }

        private string UpdateAutomationConditionFieldsXml(List<ObjectFieldList> objectFieldLists, string automationConditionFieldsXml)
        {
            string result = automationConditionFieldsXml;
            if (!string.IsNullOrEmpty(automationConditionFieldsXml))
            {
                AutomationConditionFields automationConditionFields = LogitudeXmlSerializer.DeserializeObject<AutomationConditionFields>(automationConditionFieldsXml);
                if (automationConditionFields.Fields != null && automationConditionFields.Fields.Count() > 0)
                {
                    automationConditionFields.Fields = FillObjectFieldCodeOnAutomationConditionFields(objectFieldLists, automationConditionFields.Fields);
                    result = LogitudeXmlSerializer.SerializeObjectToXmlString(automationConditionFields);

                }
            }
            return result;
        }

        private string UpdateChangesAutomationFieldsXml(List<ObjectFieldList> objectFieldLists, string changesAutomationFieldsXml)
        {
            string result = changesAutomationFieldsXml;
            if (!string.IsNullOrEmpty(changesAutomationFieldsXml))
            {
                r rFields = LogitudeXmlSerializer.DeserializeObject<r>(changesAutomationFieldsXml);
                if (rFields.cs != null && rFields.cs.Count() > 0)
                {
                    rFields.cs = FillObjectFieldCodeOnChangesAutomationFields(objectFieldLists, rFields.cs);
                    result = LogitudeXmlSerializer.SerializeObjectToXmlString(rFields);
                }
            }
            return result;
        }

        private List<Field> FillObjectFieldCodeOnAutomationConditionFields(List<ObjectFieldList> objectFieldLists, List<Field> fields)
        {
            List<Field> automationConditionFieldLists = new List<Field>();
            if (fields != null && fields.Count() > 0)
            {
                foreach (Field field in fields)
                {
                    field.FieldCode = GetObjectFieldCodeByObjecFieldId(objectFieldLists, field.Id);
                    field.PartnerObjectFieldCode = GetObjectFieldCodeByObjecFieldId(objectFieldLists, field.PartnerObjectFieldId);
                    automationConditionFieldLists.Add(field);
                }
            }

            return automationConditionFieldLists;
        }

        private AutomatedBackup UpdateAutomatedBackup(List<ObjectFieldList> objectFieldLists, string automationXML)
        {
            AutomatedBackup automatedBackup = null;
            if (!string.IsNullOrEmpty(automationXML))
            {
                automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automationXML);
                automatedBackup.AautomationConditionLists = automatedBackup.AautomationConditionLists != null ? FillObjectFieldCodeOnAutomationConditionLists(objectFieldLists, automatedBackup.AautomationConditionLists) : null;
                automatedBackup.DelayAautomationConditionLists = automatedBackup.DelayAautomationConditionLists != null ? FillObjectFieldCodeOnAutomationConditionLists(objectFieldLists, automatedBackup.DelayAautomationConditionLists) : null;
                automatedBackup.AutomationSetValueLists = automatedBackup.AutomationSetValueLists != null ? FillObjectFieldCodeOnAutomationSetValueLists(objectFieldLists, automatedBackup.AutomationSetValueLists) : null;
                automatedBackup.AutomationSetSLAValue = automatedBackup.AutomationSetSLAValue != null ? FillObjectFieldCodeOnAutomationSetSLAValue(objectFieldLists, automatedBackup.AutomationSetSLAValue) : null;
                automatedBackup.AutomationFollowUp = automatedBackup.AutomationFollowUp != null ? FillObjectFieldCodeOnAutomationFollowUpValue(objectFieldLists, automatedBackup.AutomationFollowUp) : null;
            }
            return automatedBackup;
        }

        private AutomationFollowUp FillObjectFieldCodeOnAutomationFollowUpValue(List<ObjectFieldList> objectFieldLists, AutomationFollowUp automationFollowUp)
        {
            AutomationFollowUp result = automationFollowUp;
            if (automationFollowUp != null)
            {
                if (automationFollowUp.OwnerFieldType == "Field")
                {
                    result.OwnerValue = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationFollowUp.OwnerValue);
                }
                if (!string.IsNullOrEmpty(automationFollowUp.DateValue))
                {
                    result.DateValue = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationFollowUp.DateValue);
                }
            }

            return result;
        }

        private List<c> FillObjectFieldCodeOnChangesAutomationFields(List<ObjectFieldList> objectFieldLists, List<c> changesAutomationFields)
        {
            List<c> changesAutomationFieldsLists = new List<c>();
            if (changesAutomationFields != null && changesAutomationFields.Count() > 0)
            {
                foreach (c changesAutomationField in changesAutomationFields)
                {
                    changesAutomationField.f = GetObjectFieldCodeByObjecFieldId(objectFieldLists, changesAutomationField.f);
                    changesAutomationFieldsLists.Add(changesAutomationField);
                }
            }

            return changesAutomationFieldsLists;
        }

        private AutomationSetSLAValue FillObjectFieldCodeOnAutomationSetSLAValue(List<ObjectFieldList> objectFieldLists, AutomationSetSLAValue automationSetSLAValue)
        {
            AutomationSetSLAValue result = automationSetSLAValue;
            if (automationSetSLAValue != null)
            {
                if (!string.IsNullOrEmpty(automationSetSLAValue.ObjectFieldId) && string.IsNullOrEmpty(automationSetSLAValue.ObjectFieldCode))
                {
                    result.ObjectFieldCode = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationSetSLAValue.ObjectFieldId);
                }
            }

            return result;
        }
        private List<AutomationCondition> FillObjectFieldCodeOnAutomationConditionLists(List<ObjectFieldList> objectFieldLists, List<AutomationCondition> automationConditions)
        {
            List<AutomationCondition> automationConditionLists = new List<AutomationCondition>();
            if (automationConditions != null && automationConditions.Count() > 0)
            {
                foreach (AutomationCondition automationCondition in automationConditions)
                {
                    automationCondition.ObjectFieldCode = string.IsNullOrEmpty(automationCondition.ObjectFieldCode) ? GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationCondition.ObjectFieldId) : automationCondition.ObjectFieldCode;
                    automationCondition.PartnerObjectFieldCode = string.IsNullOrEmpty(automationCondition.PartnerObjectFieldCode) ? GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationCondition.PartnerObjectFieldId) : automationCondition.PartnerObjectFieldCode;
                    if (automationCondition.OperatorCode.Contains("F")) automationCondition.Value = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationCondition.Value);
                    automationConditionLists.Add(automationCondition);
                }
            }

            return automationConditionLists;
        }
        private List<AutomationSetValue> FillObjectFieldCodeOnAutomationSetValueLists(List<ObjectFieldList> objectFieldLists, List<AutomationSetValue> automationSetValues)
        {
            List<AutomationSetValue> automationSetValueLists = new List<AutomationSetValue>();
            if (automationSetValues != null && automationSetValues.Count() > 0)
            {
                foreach (AutomationSetValue automationSetValue in automationSetValues)
                {
                    if (!string.IsNullOrEmpty(automationSetValue.ObjectFieldId) && string.IsNullOrEmpty(automationSetValue.ObjectFieldCode))
                    {
                        automationSetValue.ObjectFieldCode = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationSetValue.ObjectFieldId);
                        if (automationSetValue.OperatorCode.Contains("F")) automationSetValue.Value = GetObjectFieldCodeByObjecFieldId(objectFieldLists, automationSetValue.Value);

                        automationSetValueLists.Add(automationSetValue);
                    }
                }
            }

            return automationSetValueLists;
        }

        private string GetObjectFieldCodeByObjecFieldId(List<ObjectFieldList> objectFieldLists, string objectFieldId)
        {
            string result = objectFieldId;
            if (!string.IsNullOrEmpty(objectFieldId))
            {
                var objectField = objectFieldLists.Where(d => d.Id == objectFieldId).FirstOrDefault();
                if (objectField != null) result = objectField.FieldCode;
            }
            return result;
        }
    }
}
