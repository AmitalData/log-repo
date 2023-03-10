using System.Linq;
using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class TaskUpdateService
    {
        private readonly string PendingStatusCode = "PEN";
        private readonly string CancelledStatusCode = "CAN";

        protected override void OnCreating(TaskPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                SetTaskStatus(entityPM, true);
                CreateTaskExtended(entityPM);
            }
        }

        protected override void OnUpdating(TaskPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                SetTaskStatus(entityPM, false);
                UpdateTaskExtended(entityPM);
            }
        }

        private void SetTaskStatus(TaskPM entityPM, bool isNew)
        {
            TaskStatusRepository taskStatusRepository = new TaskStatusRepository(entityPM.Tenant);
            TaskStatus taskStatus = null;

            if (isNew)
            {
                taskStatus = taskStatusRepository.GetAll(entityPM.Tenant).OrderBy(s => s.CreateDate).Where(s => s.Code == PendingStatusCode).FirstOrDefault();
            }
            else
            {
                taskStatus = taskStatusRepository.GetSingle(entityPM.StatusId, entityPM.Tenant);
            }

            entityPM.StatusId = taskStatus?.Id;
            entityPM.IsClosed = taskStatus != null && taskStatus.Closed;
            entityPM.IsCancelled = taskStatus != null && taskStatus.Code == CancelledStatusCode;
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
            taskExtended.Fields = entityPM.Fields;
            taskExtended.ToDoConditions = entityPM.ToDoConditions;
            taskExtended.DoneConditions = entityPM.DoneConditions;
            taskExtended.Description = entityPM.Description;

            taskExtendedRepository.Update(taskExtended);
        }
    }
}