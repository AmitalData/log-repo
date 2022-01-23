using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges
{
  public  class AutomationObjectFieldService
    {
        EntityChangeArgs entityChangeArgs { get; set; }
        public AutomationObjectFieldService(EntityChangeArgs entityChangeArgs)
        {
            this.entityChangeArgs = entityChangeArgs;
        }

     
        public AutomationConditionFields GetAutomationConditionFields(List<Field> automationFieldLists, AutomationObjectTableClass automationObjectTable, AutomationObjectTableClass otherAutomationObjectTable)
        {
            AutomationConditionFields automationConditionFields = new AutomationConditionFields();
            automationConditionFields.Fields = automationFieldLists;
            automationConditionFields.LastUpdateDate = automationObjectTable.AutomationLastUpdateDate;
            automationConditionFields.ObjectTableId = automationObjectTable.Id;
            automationConditionFields.OtherObjectTableIdWithLastUpdate = (otherAutomationObjectTable != null && !string.IsNullOrEmpty(otherAutomationObjectTable.AutomationLastUpdate)) ? (otherAutomationObjectTable.Id + "@" + otherAutomationObjectTable.AutomationLastUpdate) : "";
            automationConditionFields.IsRunMasterHouseAutomation = otherAutomationObjectTable != null ? true : false;
            return automationConditionFields;
        }

        public List<ObjectField> GetAutomationObjectFieldLists(List<Automation> automationLists , AutomationObjectTableClass automationObjectTableClass)
        {
            List<Automation> automations = automationLists.Where(D => D.ObjectTableId == automationObjectTableClass.Id).ToList();
            List<ObjectField> customObjectFieldLists = new List<ObjectField>();
            string customObjectFieldListsName = "CustomObjectFieldLists" + automationObjectTableClass.Id + entityChangeArgs.Tenant + automationObjectTableClass.AutomationLastUpdate + entityChangeArgs.ProcessType;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(customObjectFieldListsName) == null)
                {
                    customObjectFieldLists = BuildCusomObjectFieldLists(automations, automationObjectTableClass.AutomationLastUpdate, automationObjectTableClass.OriginalId);

                    if (customObjectFieldLists != null)
                    {
                        CacheManager.CacheWrapper.Insert(customObjectFieldListsName, customObjectFieldLists, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    customObjectFieldLists = (List<ObjectField>)CacheManager.CacheWrapper.Get(customObjectFieldListsName);
                }
            }

            else
            {
                customObjectFieldLists = BuildCusomObjectFieldLists(automations, automationObjectTableClass.AutomationLastUpdate, automationObjectTableClass.OriginalId);
            }
            return customObjectFieldLists;
        }

        private List<ObjectField> BuildCusomObjectFieldLists(List<Automation> Automations, string lastupdateautomation, string objectTableId)
        {
            AutomationResultEmailRecipientRepository automationResultEmailRecipientRepository = new AutomationResultEmailRecipientRepository(entityChangeArgs.Tenant);
            ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(entityChangeArgs.Tenant);
            int LargeDelayTime = 0;
            AutomatedBackup automatedBackup = null;
            List<ObjectField> customObjectFieldLists = new List<ObjectField>();
            List<AutomationCondition> automationConditionList = new List<AutomationCondition>();
            List<AutomationSetValue> automationSetValueLists = new List<AutomationSetValue>();
            string automationDelayTimeFieldCode = "";
            AutomationFollowUp automationFollowUp = new AutomationFollowUp();
            AutomationQueuedTask automationQueuedTask = new AutomationQueuedTask();

            List<AutomationCondition> delayedAutomationConditionList = new List<AutomationCondition>();
            List<ObjectField> objectFieldLists = objectFieldsRepository.GetAutomationObjectFieldsByObjectTableId(objectTableId, entityChangeArgs.Tenant);

            foreach (Automation automation in Automations)
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    string automatedBackupName = "AutomatedBackupName" + lastupdateautomation + automation.Id + automation.Tenant;
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(automatedBackupName) == null || CacheManager.CacheWrapper.Get(automatedBackupName) == null)
                        {
                            automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                            automationConditionList = automatedBackup.AautomationConditionLists;
                            automationSetValueLists = automatedBackup.AutomationSetValueLists;
                            automationFollowUp = automatedBackup.AutomationFollowUp;
                            automationQueuedTask = automatedBackup.AutomationQueuedTask;
                            delayedAutomationConditionList = automatedBackup.DelayAautomationConditionLists;
                            automationDelayTimeFieldCode = automatedBackup.SelectedDelaytimeFieldCode;

                            CacheManager.CacheWrapper.Insert(automatedBackupName, automatedBackup, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                        }
                        else
                        {
                            automatedBackup = (AutomatedBackup)CacheManager.CacheWrapper.Get(automatedBackupName);
                            automationConditionList = automatedBackup.AautomationConditionLists;
                            automationSetValueLists = automatedBackup.AutomationSetValueLists;
                            automationFollowUp = automatedBackup.AutomationFollowUp;
                            automationQueuedTask = automatedBackup.AutomationQueuedTask;
                            delayedAutomationConditionList = automatedBackup.DelayAautomationConditionLists;
                            automationDelayTimeFieldCode = automatedBackup.SelectedDelaytimeFieldCode;
                        }
                    }
                    else
                    {
                        automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        automationConditionList = automatedBackup.AautomationConditionLists;
                        automationSetValueLists = automatedBackup.AutomationSetValueLists;
                        automationFollowUp = automatedBackup.AutomationFollowUp;
                        automationQueuedTask = automatedBackup.AutomationQueuedTask;
                        delayedAutomationConditionList = automatedBackup.DelayAautomationConditionLists;
                        automationDelayTimeFieldCode = automatedBackup.SelectedDelaytimeFieldCode;
                    }

                    if (automatedBackup != null && automatedBackup.Delaytime > LargeDelayTime)
                    {
                        LargeDelayTime = automatedBackup.Delaytime;
                    }
                }

                #region Add Automation Condition Object Field

                if (automationConditionList != null && automationConditionList.Count > 0)
                {
                    foreach (AutomationCondition automationCondition in automationConditionList)
                    {
                        if (automationCondition.OperatorCode.Contains("F"))
                        {
                            ObjectField customobjectField = objectFieldLists.Where(d => d.FieldCode == automationCondition.Value).FirstOrDefault();
                            if (customobjectField != null && !customObjectFieldLists.Contains(customobjectField))
                            {
                                customObjectFieldLists.Add(customobjectField);
                            }
                        }

                        ObjectField objectField = objectFieldLists.Where(d => d.FieldCode == automationCondition.ObjectFieldCode).FirstOrDefault();
                        if (objectField != null && !customObjectFieldLists.Contains(objectField))
                        {
                            customObjectFieldLists.Add(objectField);
                        }

                        ObjectField partnerObjectField = objectFieldLists.Where(d => d.FieldCode == automationCondition.PartnerObjectFieldCode).FirstOrDefault();
                        if (partnerObjectField != null && !customObjectFieldLists.Contains(partnerObjectField))
                        {
                            customObjectFieldLists.Add(partnerObjectField);
                        }

                    }
                }
                #endregion

                #region Add Automation Set Value Object Field

                if (automationSetValueLists != null && automationSetValueLists.Count > 0)
                {
                    foreach (AutomationSetValue automationSetValue in automationSetValueLists)
                    {
                        if (automationSetValue.OperatorCode.Contains("F"))
                        {
                            ObjectField objectFieldValue = objectFieldLists.Where(d => d.FieldCode == automationSetValue.Value).FirstOrDefault();
                            if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                            {
                                customObjectFieldLists.Add(objectFieldValue);
                            }
                        }


                        ObjectField partnerObjectField = objectFieldLists.Where(d => d.FieldCode == automationSetValue.PartnerObjectFieldCode).FirstOrDefault();
                        if (partnerObjectField != null && !customObjectFieldLists.Contains(partnerObjectField))
                        {
                            customObjectFieldLists.Add(partnerObjectField);
                        }

                    }
                }
                #endregion

                #region Add Automation Delay Time Value Object Field

                if (!string.IsNullOrEmpty(automationDelayTimeFieldCode))
                {
                    ObjectField objectFieldValue = objectFieldLists.Where(d => d.FieldCode == automationDelayTimeFieldCode).FirstOrDefault();
                    if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                    {
                        customObjectFieldLists.Add(objectFieldValue);
                    }
                }
                #endregion

                #region F/U Creation ObjectField

                if (automationFollowUp != null)
                {
                    if (automationFollowUp.OwnerFieldType == "Field")
                    {
                        ObjectField objectFieldValue = objectFieldLists.Where(d => d.FieldCode == automationFollowUp.OwnerValue).FirstOrDefault();
                        if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                        {
                            customObjectFieldLists.Add(objectFieldValue);
                        }
                    }


                    if (automationFollowUp.DateValue != null)
                    {
                        ObjectField objectFieldValue = objectFieldLists.Where(d => d.FieldCode == automationFollowUp.DateValue.ToString()).FirstOrDefault();
                        if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                        {
                            customObjectFieldLists.Add(objectFieldValue);
                        }
                    }
                }
                #endregion

                #region Add Delayed Automation Condition Object Field

                if (delayedAutomationConditionList != null && delayedAutomationConditionList.Count > 0)
                {
                    foreach (AutomationCondition delayedautomationCondition in delayedAutomationConditionList)
                    {
                        if (delayedautomationCondition.OperatorCode.Contains("F"))
                        {
                            ObjectField customobjectField = objectFieldLists.Where(d => d.FieldCode == delayedautomationCondition.Value).FirstOrDefault();
                            if (customobjectField != null && !customObjectFieldLists.Contains(customobjectField))
                            {
                                customObjectFieldLists.Add(customobjectField);
                            }
                        }

                        ObjectField objectField = objectFieldLists.Where(d => d.FieldCode == delayedautomationCondition.ObjectFieldCode).FirstOrDefault();
                        if (objectField != null && !customObjectFieldLists.Contains(objectField))
                        {
                            customObjectFieldLists.Add(objectField);
                        }
                    }
                }
                #endregion

                #region Add Email Recipient Object Field

                List<AutomationResultEmailRecipient> resultEmailRecipient = automationResultEmailRecipientRepository.GetAutomationResultEmailRecipientByAutomationId(automation.Id, automation.Tenant);

                foreach (AutomationResultEmailRecipient automationResultEmailRecipient in resultEmailRecipient)
                {
                    ObjectField objectField = objectFieldLists.Where(d => d.FieldCode == automationResultEmailRecipient.RecipientValue).FirstOrDefault();

                    if (objectField != null && !customObjectFieldLists.Contains(objectField))
                    {
                        customObjectFieldLists.Add(objectField);
                    }


                    ObjectField partnerObjectField = objectFieldLists.Where(d => d.FieldCode == automationResultEmailRecipient.PartnerObjectFieldCode).FirstOrDefault();
                    if (partnerObjectField != null && !customObjectFieldLists.Contains(partnerObjectField))
                    {
                        customObjectFieldLists.Add(partnerObjectField);
                    }




                }

                #endregion
            }

            return customObjectFieldLists;
        }
       
        
    }
}
