using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class GeneralAutomationResultService
    {
        public ValidateAutomationResultClass ValidateAutomation(Automation automation, EntityChange entityChange, List<Field> automationConditionFields, string lastupdateautomation, string typeConditionValidate)
        {
            bool validconditionAnd = true;
            bool validconditionOr = false;
            bool IsConditionValid = false;

            ValidateAutomationResultClass validateResult = new ValidateAutomationResultClass();
            
            validateResult.ConditionsList = new List<AutomationCondition>();
            List<AutomationCondition> automationConditionList = null;

            AutomatedBackup automatedBackup = GetAutomatedBackupClass(automation, entityChange, lastupdateautomation);

            if (typeConditionValidate == "Delayed")  
                automationConditionList = automatedBackup.DelayAautomationConditionLists;
            else automationConditionList = automatedBackup.AautomationConditionLists;

            if (automationConditionList != null && automationConditionList.Count() > 0)
            {
                int conditionAndCount = automationConditionList.Where(d => d.ConditionType == "And").Count();
                int conditionOrCount = automationConditionList.Where(d => d.ConditionType == "Or").Count();


                 bool validAndList = true;
                 bool validOrList = false;


                if (conditionOrCount == 0) validOrList = true;
                foreach (AutomationCondition automationCondition in automationConditionList.Where(d => d.ConditionType == "And"))
                {
                     
                    validconditionAnd = ValidateCondition(automationConditionFields, automationCondition, entityChange);
                     
                    validateResult.ConditionsList.Add(automationCondition);
                     
                    if (!validconditionAnd)
                    {
                        validAndList = false;
                    }

                    //if (!validconditionAnd) break;
                }
                 
                // if (validconditionAnd)
                // {
                foreach (AutomationCondition automationCondition in automationConditionList.Where(d => d.ConditionType == "Or"))
                    { 

                       validconditionOr = ValidateCondition(automationConditionFields, automationCondition, entityChange);
                     
                        validateResult.ConditionsList.Add(automationCondition);

                    if (validconditionOr)
                    {
                        validOrList = true;
                    }

                    //if (validconditionOr) break;
                }
                // } 

                if (validAndList && validOrList) IsConditionValid = true;
            }

            else IsConditionValid = true; 
            validateResult.IsAutomationValid = IsConditionValid;
             

            if (automatedBackup != null)
            {
                validateResult.Type = automatedBackup.Type;
                validateResult.DelaytimeIndicator = automatedBackup.DelaytimeIndicator;
                validateResult.Delaytime = automatedBackup.Delaytime;
                validateResult.DelaytimeOp = automatedBackup.DelaytimeOp;
                validateResult.SelectedDelaytimeFieldCode = automatedBackup.SelectedDelaytimeFieldCode; 
            }

            //if(validateResult.ConditionsList.Count() == 0 && typeConditionValidate == "Delayed")
            //{
             //   validateResult.ConditionsList = automatedBackup.AautomationConditionLists;
             //   List<AutomationCondition> delayAutomationConditionsList = null;
             //   delayAutomationConditionsList = validateResult.ConditionsList;
             //   foreach (AutomationCondition automationCondition in delayAutomationConditionsList)
             //   {
              //       automationCondition.IsValid = true;  
              //    }
              //     validateResult.ConditionsList = delayAutomationConditionsList;
              //  }

            return validateResult;
        }

        private bool ValidateCondition(List<Field> automationConditionFieldLists, AutomationCondition automationCondition, EntityChange entityChange)
        {
            bool isValid = true;
            string automationConditionPartnerObjectFieldCode = !string.IsNullOrEmpty(automationCondition.PartnerObjectFieldCode) ? automationCondition.PartnerObjectFieldCode : null;
            Field automationConditionField = automationConditionFieldLists.Where(d => d.FieldCode == automationCondition.ObjectFieldCode && automationConditionPartnerObjectFieldCode == d.PartnerObjectFieldCode).FirstOrDefault();

            if (automationConditionField != null)
            {
                string automationConditionvalue = "";
                string automationConditionFieldValue = "";

                if (!string.IsNullOrEmpty(automationConditionField.Value)) automationConditionFieldValue = automationConditionField.Value.ToLower();
                if (!string.IsNullOrEmpty(automationCondition.Value)) automationConditionvalue = automationCondition.Value.ToLower();

                #region  CustomObjectFieldValue

                if (automationCondition.OperatorCode.Contains("F"))
                {
                    Field item = automationConditionFieldLists.Where(d => d.FieldCode.ToLower() == automationConditionvalue).FirstOrDefault();

                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value)) automationConditionvalue = item.Value.ToLower();
                    }
                }

                #endregion

                #region CustomDateValue

                else if (automationCondition.ObjectFieldType == "DateTime" || automationCondition.ObjectFieldType == "Date")
                {
                    if (!string.IsNullOrEmpty(automationConditionvalue))
                    {
                        string[] datearray = automationConditionvalue.Split('*');

                        if (automationConditionvalue.ToLower().Contains("date"))
                        {
                            if (datearray.Length > 1) automationConditionvalue = datearray[1];
                        }
                        else
                        {
                            if (datearray.Length > 2)
                            {
                                int days = 0;
                                if (!string.IsNullOrEmpty(datearray[1])) days = Int32.Parse(datearray[1]);
                                int dateEscalationTime = 0;
                                DateTime? date = datearray[0].Contains("old") ? FieldValueResolver.ConvertToDate(automationConditionField.OldValue) : TenantServerConfigration.GetCurrentDateTime(automationCondition.Tenant);
                                CustomFieldClass customFieldClass = new CustomFieldClass();

                                dateEscalationTime = (datearray[0] == "@today+" || datearray[0] == "@old value+") ? days : days * -1;
                                if (date != null)
                                {
                                    date = date.Value.AddDays(dateEscalationTime);
                                    automationConditionvalue = customFieldClass.ConvertToString(Convert.ToDateTime(date));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(automationConditionvalue) && automationConditionvalue.Length >= 9) automationConditionvalue = automationConditionvalue.Remove(8);
                        if (!string.IsNullOrEmpty(automationConditionFieldValue) && automationConditionFieldValue.Length >= 9) automationConditionFieldValue = automationConditionFieldValue.Remove(8);
                    }
                }

                else if (automationCondition.ObjectFieldType == "Decimal" || automationCondition.ObjectFieldType == "Integer" || automationCondition.ObjectFieldType == "Double")
                {
                    automationConditionvalue = FieldValueResolver.GetFieldStringValue(new ObjectField() { DataTypeCode = automationCondition.ObjectFieldType }, automationCondition.Value);
                }


                else if (!string.IsNullOrEmpty(automationCondition.Value) && automationCondition.Value.Contains("@StatusName:"))
                {
                    automationConditionvalue = automationCondition.Value.Split('@')[0];
                    if (!string.IsNullOrEmpty(automationConditionvalue)) automationConditionvalue = automationConditionvalue.ToLower();
                }


                if (automationCondition.ObjectFieldType == "Boolean")
                {
                    if (string.IsNullOrEmpty(automationConditionvalue)) automationConditionvalue = "false";
                    if (string.IsNullOrEmpty(automationConditionFieldValue)) automationConditionvalue = "false";
                }

                #endregion


                if (automationCondition.OperatorCode == "<=" || automationCondition.OperatorCode == "<=F" || automationCondition.OperatorCode == ">" || automationCondition.OperatorCode == ">F" || automationCondition.OperatorCode == ">=" || automationCondition.OperatorCode == ">=F" || automationCondition.OperatorCode == "<" || automationCondition.OperatorCode == "<F")
                {
                    if (string.IsNullOrEmpty(automationConditionvalue) || string.IsNullOrEmpty(automationConditionFieldValue))
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                if (automationCondition.OperatorCode == "=" || automationCondition.OperatorCode == "=F")
                {
                    if (automationConditionFieldValue != automationConditionvalue)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "<>" || automationCondition.OperatorCode == "<>F")
                {
                    if (automationConditionFieldValue == automationConditionvalue)
                    {
                        isValid = false; 
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "CONTAINS" || automationCondition.OperatorCode == "CONTAINSF")
                {
                    if (!automationConditionFieldValue.Contains(automationConditionvalue))
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }
                else if (automationCondition.OperatorCode == "!CONTAINS" || automationCondition.OperatorCode == "!CONTAINSF")
                {
                    if (!string.IsNullOrEmpty(automationConditionFieldValue) && automationConditionFieldValue.Contains(automationConditionvalue))
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == ">" || automationCondition.OperatorCode == ">F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare <= 0)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "<" || automationCondition.OperatorCode == "<F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare >= 0)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == ">=" || automationCondition.OperatorCode == ">=F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare == -1)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "<=" || automationCondition.OperatorCode == "<=F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare == 1)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "CHANGEDTO")
                {
                    if (automationConditionFieldValue != automationConditionvalue || !automationConditionField.IsChange)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "CHANGED")
                {
                    if (!automationConditionField.IsChange)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "ISEMPTY")
                {
                    if (!string.IsNullOrEmpty(automationConditionFieldValue))
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }
                else if (automationCondition.OperatorCode == "ISNOTEMPTY")
                {
                    if (string.IsNullOrEmpty(automationConditionFieldValue))
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "EqualSystemVariable" && automationCondition.Value == "SystemUser")
                {
                    string userId = GetSystemContactIdByTenant(entityChange.Tenant);
                    if (userId != automationConditionFieldValue)
                    {
                        isValid = false;
                    }
                    SetConditionValidate(automationCondition, isValid);

                    return isValid;
                }

            }
            else
            {
                isValid = false;  
            }
            SetConditionValidate(automationCondition, isValid);
            return isValid;
        }

        private static void SetConditionValidate(AutomationCondition automationCondition, bool isValid)
        {
            automationCondition.IsValid = isValid;
        }

        public string GetSystemContactIdByTenant(int tenant)
        {
            string loggedContactId = string.Empty;
            ContactRepository contactRepository = new ContactRepository(tenant);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            var contact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);
            if (contact != null) loggedContactId = contact.Id;

            return loggedContactId;
        }


        public EntityChangeAutomation CreateEntityChangeAutomation(Automation automation)
        {
            return new EntityChangeAutomation()
            {
                Id = IdCounter.GetNumber("EntityChangeAutomation", automation.Tenant),
                AutomationId = automation.Id,
                AutomationType = automation.Type,
                ResultCode = automation.ResultCode,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(automation.Tenant),
                AutomationDescription = automation.Description,
                AutomationName = automation.Name,
            };
        }


        public string GetLastAuomationUpdateDate(AutomationObjectTableClass automationObjectTableClass, AutomationObjectTableClass otherAutomationObjectTableClass, Automation automation)
        {
            string lastAuomationUpdateDate = automationObjectTableClass.AutomationLastUpdate;
            if (otherAutomationObjectTableClass != null && !string.IsNullOrEmpty(otherAutomationObjectTableClass.AutomationLastUpdate))
            {
                if (automation.ObjectTableId == otherAutomationObjectTableClass.Id)
                {
                    lastAuomationUpdateDate = otherAutomationObjectTableClass.AutomationLastUpdate;
                }
            }

            return lastAuomationUpdateDate;
        }


        public void AddAutomationQueue(AutomationQueueArgs automationQueueArgs)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("AutomationQueue", automationQueueArgs.Tenant);
            queueservice.Send(new Dictionary<string, string>() {{ "EntityChangeId", automationQueueArgs.EntityChangeId }, { "Tenant", automationQueueArgs.Tenant.ToString() }, { "Type", automationQueueArgs.AutomationType }, { "EntityId", automationQueueArgs.EntityId }, { "AutomationId", automationQueueArgs.AutomationId } , { "ExternalId", automationQueueArgs.ExternalId } , { "ExecutedImmediately", automationQueueArgs.ExecutedImmediately.ToString() } }, automationQueueArgs.Tenant, automationQueueArgs.AutomationDelayTime, null, null, null);

        }

        public TimeSpan? GetAutomationDelayTime(DelaytimeDetails delaytimeDetails, List<Field> automationFieldLists, int tenant)
        {
            TimeSpan? delayTime = null;
            int delay = delaytimeDetails.Delaytime;
            if (delaytimeDetails.DelaytimeIndicator == "OO" && delaytimeDetails.Delaytime != 0) delay = delaytimeDetails.Delaytime * 60;
            if (delaytimeDetails.DelaytimeIndicator == "DD" && delaytimeDetails.Delaytime != 0) delay = delaytimeDetails.Delaytime * 60 * 24;

            if (!string.IsNullOrEmpty(delaytimeDetails.SelectedDelaytimeFieldCode) && !string.IsNullOrEmpty(delaytimeDetails.DelaytimeOp) && delaytimeDetails.DelaytimeOp != "NL")
            {
                Field field = automationFieldLists.Where(d => d.FieldCode == delaytimeDetails.SelectedDelaytimeFieldCode).FirstOrDefault();
                delay = delaytimeDetails.DelaytimeOp == "BF" ? delay * -1 : delay;
                delayTime = GetFieldServerDelayTime(field, delay, tenant);
            }
            else
            {
                DateTime nextRunDate = DateTime.UtcNow.AddMinutes((double)delay);
                delayTime = nextRunDate - DateTime.UtcNow;
            }

            return delayTime;
        }

        private TimeSpan? GetFieldServerDelayTime(Field field, int delay, int tenant)
        {
            TimeSpan? delayTime = null;
            DateTime nextRunDateBeforeAddDelayed = DateTime.UtcNow;
            if (field != null && !string.IsNullOrEmpty(field.Value))
            {
                DateTime? fieldDateValue = ConvertToDate(field.Value);
                delay = GetServerDelay(fieldDateValue, delay, tenant);
                DateTime nextRunDate = nextRunDateBeforeAddDelayed.AddMinutes((double)delay);
                delayTime = nextRunDate - DateTime.UtcNow;
            }
            return delayTime;
        }

        private int GetServerDelay(DateTime? fieldDateValue, int delay, int tenant)
        {
            int serverDelay = 0;
            if (fieldDateValue != null)
            {
                DateTime tenantDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime fieldDateTime = (DateTime)fieldDateValue;
                DateTime fieldDateTimeWithTenantDelay = fieldDateTime.AddMinutes((double)delay);
                TimeSpan serverDelaySpanValue = fieldDateTimeWithTenantDelay - tenantDateTime;
                serverDelay = (fieldDateTimeWithTenantDelay > tenantDateTime) ? (int)serverDelaySpanValue.TotalMinutes : 0;
            }

            return serverDelay;
        }

        public DateTime? ConvertToDate(string value)
        {
            DateTime? date = null;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length < 12)
                {
                    int count = 12 - value.Length;
                    while (count != 0)
                    {
                        value += "0";
                        count -= 1;
                    }
                }

                if (value.Length >= 12)
                {
                    date = new DateTime(System.Convert.ToInt32(value.Substring(0, 4)), System.Convert.ToInt32(value.Substring(4, 2)), System.Convert.ToInt32(value.Substring(6, 2)), System.Convert.ToInt32(value.Substring(8, 2)), System.Convert.ToInt32(value.Substring(10, 2)), System.Convert.ToInt32(value.Substring(12, 2)));
                }
            }

            return date;
        }



        public AutomatedBackup GetAutomatedBackupClass(Automation automation, EntityChange entityChange, string lastUpdateDate)
        {
            AutomatedBackup automatedBackup = null;
            if (entityChange.CreateDate >= automation.UpdateDate)
            {
                string automatedBackupName = "AutomatedBackupName" + lastUpdateDate + automation.Id + automation.Tenant;

                if (CacheManager.CacheWrapper != null)
                {
                    if (CacheManager.CacheWrapper.Get(automatedBackupName) == null)
                    {
                        if (!string.IsNullOrEmpty(automation.AutomationXML))
                        {
                            automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                            CacheManager.CacheWrapper.Insert(automatedBackupName, automatedBackup, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        automatedBackup = (AutomatedBackup)CacheManager.CacheWrapper.Get(automatedBackupName);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    }
                }
            }
            else
            {
                AutomationHistoryRepository automationHistoryRepository = new AutomationHistoryRepository(entityChange.Tenant);
                string automationXML = automationHistoryRepository.GetAutomationXMLFromAutomationHistoryByDate(entityChange.CreateDate, automation.Id, entityChange.Tenant);
                if (!string.IsNullOrEmpty(automationXML))
                {
                    automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automationXML);
                }
            }

            return automatedBackup;
        }




    }

    public class DelaytimeDetails
    {
        public string Type { get; set; }
        public string DelaytimeIndicator { get; set; }
        public int Delaytime { get; set; }
        public string DelaytimeOp { get; set; }
        public string SelectedDelaytimeFieldCode { get; set; }
    }

    public class AutomationQueueArgs
    {
        public string EntityChangeId { get; set; }
        public string AutomationType { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string AutomationId { get; set; }
        public string ExternalId { get; set; }
        public TimeSpan? AutomationDelayTime { get; set; }
        public bool ExecutedImmediately { get; set; }



    }


}

