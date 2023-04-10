using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.FieldsMapping;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskDataMapping: IMapping<TaskPM, Task>
   {
        public void CustomPMToPOCO(TaskPM entityPM, Task entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(TaskPM entityPM, Task entityPOCO)
        {
            entityPM.CreatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
            entityPM.OwnerName = EntityFieldsMapping.GetUserName(entityPOCO.OwnerId, entityPOCO.Tenant);
            entityPM.PriorityName = EntityFieldsMapping.GetTaskPriorityName(entityPOCO.PriorityId, entityPOCO.Tenant);
            entityPM.StatusName = EntityFieldsMapping.GetTaskStatusName(entityPOCO.StatusId, entityPOCO.Tenant);
            entityPM.TaskTypeName = EntityFieldsMapping.GetTaskTypeName(entityPOCO.TaskTypeId, entityPOCO.Tenant);
            entityPM.EntityObjectTableName = EntityFieldsMapping.GetEntityObjectTableName(entityPOCO.EntityObjectTableId, entityPOCO.Tenant);
            entityPM.ClosedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.ClosedByUserId, entityPOCO.Tenant);
            entityPM.CheckWithName = EntityFieldsMapping.GetCardName(entityPOCO.CheckWithId, entityPOCO.Tenant);
            entityPM.Fields = EntityFieldsMapping.GetTaskFields(entityPOCO.Id, entityPOCO.Tenant);
            entityPM.ToDoConditions = EntityFieldsMapping.GetTaskToDoConditions(entityPOCO.Id, entityPOCO.Tenant);
            entityPM.DoneConditions = EntityFieldsMapping.GetTaskDoneConditions(entityPOCO.Id, entityPOCO.Tenant);
            entityPM.Description = EntityFieldsMapping.GetTaskDescription(entityPOCO.Id, entityPOCO.Tenant);
        }
    }
}