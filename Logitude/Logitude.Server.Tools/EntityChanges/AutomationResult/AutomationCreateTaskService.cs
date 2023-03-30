using Logitude.Server.Tools.EntityChanges.AutomationResultExternalServices;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationCreateTaskService : GeneralAutomationResultService, IAutomationResultService
    {

        public List<AutomationQueueArgs> AutomationQueues { get; set; }
        private AutomationResultArgs automationResultArgs { get; set; }
        private int tenant;
        private EntityChange entityChange;
        private object entityPM;
        private MainEntityChangeService mainEntityChangeService { get; set; }
        private List<Automation> createTaskAutomations = new List<Automation>();
        public string ResultCode { get { return "CREATETASK"; } }


        public  bool DependencyOnLastEntityUpdate { get { return (processType == "OnCreate") ? true : false; } }

        private string processType = string.Empty;
        public AutomationCreateTaskService(string processType)
        {
            this.processType = processType;
        }



        public void Run(AutomationResultArgs automationResultArgs)
        {
            AutomationQueues = new List<AutomationQueueArgs>();
            this.automationResultArgs = automationResultArgs;
            this.entityChange = automationResultArgs.EntityChange;
            this.entityPM = automationResultArgs.EntityPM;
            this.tenant = this.entityChange.Tenant;
            mainEntityChangeService = automationResultArgs.MainEntityChangeService;
            createTaskAutomations = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode).ToList();
            if (createTaskAutomations.Count > 0)
            {
                ApplyCreateTaskAutomations();
            }
        }

        public void ApplyCreateTaskAutomations()
        {
            foreach (Automation automation in createTaskAutomations)
            {
                DateTime automationStartProcessingDate = DateTime.Now;
                ValidateAutomationResultClass validateAutomationResult = GetValidateAutomationResult(automation);
                if (validateAutomationResult.IsAutomationValid)
                {
                    CreateTaskCollaborationTool(validateAutomationResult.AutomatedBackup.AutomationCreateTask);
                    entityChange.HasExecutedRecord = true;
                }

                EntityChangeAutomation entityChangesAutomation = GetEntityChangeAutomation(automation, validateAutomationResult, automationStartProcessingDate);
                mainEntityChangeService.AddEntityChangesAutomation(entityChangesAutomation);
            }
        }

   
        private void CreateTaskCollaborationTool(AutomationCreateTask automationCreateTask)
        {
            IQueueService queueservice = new DbQueueService();
            string entityNumber = GetPropertyValueFromObject("ShipmentNumber", entityPM);
            DateTime? taskEndDate = new TaskAutomationEndDateService(tenant).GetDate(automationCreateTask.EndDateValue, automationCreateTask.EndDateTypeValue, entityPM);
            string OnwerId = GetOwnerId(automationCreateTask);
            string AssigneeId = GetAssigneeId(automationCreateTask);
            queueservice.InitializeQueue("CreateTaskCollaborationTool", tenant);
            queueservice.Send(new Dictionary<string, string>() { { "AssigneeId", AssigneeId }, { "Tenant", tenant.ToString() }, { "TaskType", automationCreateTask.TaskType }, { "OwnerId", OnwerId }, { "EndDate", taskEndDate != null ? taskEndDate.ToString() : null }, { "EntityNumber", entityNumber } }, tenant, null, null, null, null);
        }

        private string GetAssigneeId(AutomationCreateTask automationCreateTask)
        {

            string assigneeId = automationCreateTask.AssigneeValue;
            if (automationCreateTask != null)
            {
                #region Fill Data  
                //AssigneeId
                if (automationCreateTask.AssigneeFieldType == "Field")
                {

                    assigneeId = GetPropertyValueFromObject(assigneeId, entityPM); 
 
                }

                if (string.IsNullOrEmpty(assigneeId)) assigneeId = entityChange.CreateByUserId;

                #endregion

            }
             
            return assigneeId;
        }

        private string GetOwnerId(AutomationCreateTask automationCreateTask)
        {

            string ownerId = automationCreateTask.OwnerValue;
            if (automationCreateTask != null)
            {
                #region Fill Data  
                //OwnerId
                if (automationCreateTask.OwnerFieldType == "Field")
                {

                    ownerId = GetPropertyValueFromObject(ownerId, entityPM);

                }

                if (string.IsNullOrEmpty(ownerId)) ownerId = entityChange.CreateByUserId;

                #endregion

            }

            return ownerId;
        }

        private EntityChangeAutomation GetEntityChangeAutomation(Automation automation, ValidateAutomationResultClass validateResult, DateTime automationStartProcessingDate)
        {
            EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);
            entityChangesAutomation.ResultCode = "Create Task";
            entityChangesAutomation.ConditionsList = validateResult.ConditionsList;
            entityChangesAutomation.type = validateResult.IsAutomationValid ? "CreateTaskSsucceed" : "CreateTaskFailed";
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - automationStartProcessingDate.Ticks) / TimeSpan.TicksPerMillisecond);
            entityChangesAutomation.IsConditionTrue = validateResult.IsAutomationValid ? true : false;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            return entityChangesAutomation;
        }


        private ValidateAutomationResultClass GetValidateAutomationResult(Automation automation)
        {
            string lastAuomationUpdateDate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);
            ValidateAutomationResultClass validateAutomationResult = ValidateAutomation(automation, entityChange, automationResultArgs.AutomationFieldLists, lastAuomationUpdateDate, "");
            return validateAutomationResult;
        }


    }


}
