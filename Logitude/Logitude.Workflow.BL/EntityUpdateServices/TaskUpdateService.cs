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
        protected override void OnCreating(TaskPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CreateTaskExtended(entityPM);
            }
        }

        protected override void OnUpdating(TaskPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                UpdateTaskExtended(entityPM);
            }
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

        private bool IsAssignedTask(TaskPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.OwnerId))
            {
                return false;
            }
            return true;
        }
    }
}