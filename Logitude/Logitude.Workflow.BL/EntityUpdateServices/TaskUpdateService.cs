using Logitude.BL.Workfkow;
using Logitude.BL.Workfkow.Constants;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.Repositories;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class TaskUpdateService
    {
        protected override void OnCreating(TaskPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CreateTaskExtended(entityPM);

                AddWorkflowEntityQueueMessage(entityPM, QueueMessagesTypes.Create, null);
            }
        }

        protected override void OnUpdating(TaskPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                UpdateTaskExtended(entityPM);
            }
        }

        protected override void AfterUpdating(TaskPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                CalculateFieldChanges(entityPM);

                AuditLog auditLog = AddAuditLog(entityPM);
                AddWorkflowEntityQueueMessage(entityPM, QueueMessagesTypes.Update, auditLog?.Id);
            }
        }

        private void CalculateFieldChanges(TaskPM entityPM)
        {
            Mapping.PMToOldPM(entityPM, OldEntityPM);

            OldEntityPM.ChangedProperties.ForEach(change =>
            {
                FieldChanges.Add(new FieldChange()
                {
                    Field = change.PropertyName,
                    OldValue = change.OldValue,
                    NewValue = change.NewValue
                });
            });
        }

        private void CreateTaskExtended(TaskPM entityPM)
        {
            IWorkflowContext workflowContext = MainContext as WorkflowContext;
            TaskExtendedRepository taskExtendedRepository = new TaskExtendedRepository(workflowContext);

            TaskExtended taskExtended = new TaskExtended()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant,
                Fields = entityPM.Fields,
                ToDoConditions = entityPM.ToDoConditions,
                DoneConditions = entityPM.DoneConditions,
                Description = entityPM.Description
            };

            taskExtendedRepository.Add(taskExtended);
        }

        private void UpdateTaskExtended(TaskPM entityPM)
        {
            IWorkflowContext workflowContext = MainContext as WorkflowContext;
            TaskExtendedRepository taskExtendedRepository = new TaskExtendedRepository(workflowContext);

            TaskExtended taskExtended = taskExtendedRepository.GetSingle(entityPM.Id, entityPM.Tenant);

            FieldChange.Add(taskExtended.Fields, entityPM.Fields, nameof(entityPM.Fields), FieldChanges);
            taskExtended.Fields = entityPM.Fields;

            FieldChange.Add(taskExtended.ToDoConditions, entityPM.ToDoConditions, nameof(entityPM.ToDoConditions), FieldChanges);
            taskExtended.ToDoConditions = entityPM.ToDoConditions;
            
            FieldChange.Add(taskExtended.DoneConditions, entityPM.DoneConditions, nameof(entityPM.DoneConditions), FieldChanges);
            taskExtended.DoneConditions = entityPM.DoneConditions;
            
            FieldChange.Add(taskExtended.Description, entityPM.Description, nameof(entityPM.Description), FieldChanges);
            taskExtended.Description = entityPM.Description;

            taskExtendedRepository.Update(taskExtended);
        }

        private AuditLog AddAuditLog(TaskPM entityPM)
        {
            AuditLog auditLog = null;
            if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
            {
                auditLog = BuildTaskAuditLog(entityPM);
                AuditLogRepository.Add(auditLog);
                AuditLogRepository.SubmitChanges();
            }

            return auditLog;
        }

        private AuditLog BuildTaskAuditLog(TaskPM entityPM)
        {
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Task", 0, true);

            return new AuditLog()
            {
                Id = IdCounter.GetNumber("AuditLog", entityPM.Tenant).ToString(),
                Tenant = entityPM.Tenant,
                UpdateDate = entityPM.UpdateDate,
                UpdatedByUserId = entityPM.UpdatedByUserId,
                EntityId = entityPM.Id,
                ObjectTableId = objecttable.Id,
                ChangesJson = JsonConvert.SerializeObject(FieldChanges)
            };
        }

        private void AddWorkflowEntityQueueMessage(TaskPM entityPM, string type, string auditLogId)
        {
            new WorkflowEntityQueueMessage()
            {
                Entity = WorkflowEntities.Task,
                EntityId = entityPM.Id,
                AuditLogId = auditLogId,
                Tenant = entityPM.Tenant,
                Type = type
            }.Produce();
        }
    }
}