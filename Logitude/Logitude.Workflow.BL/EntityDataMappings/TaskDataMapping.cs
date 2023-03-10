using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Workflow.BL.FieldsMapping;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskDataMapping: IMapping<TaskPM, Task>
   {
        public void CustomPMToPOCO(TaskPM entityPM, Task entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
                entityPOCO.Id = entityPM.Id;
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                CustomMappedPOCOProperties.Add(POCOPropertyNames.CreateDate);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);

                entityPM.CreateDate = entityPOCO.CreateDate;
                entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(TaskPM entityPM, Task entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.OwnerName);
            CustomMappedPMProperties.Add(PMPropertyNames.PriorityName);
            CustomMappedPMProperties.Add(PMPropertyNames.StatusName);
            CustomMappedPMProperties.Add(PMPropertyNames.TaskTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.EntityObjectTableName);
            CustomMappedPMProperties.Add(PMPropertyNames.ClosedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.CheckWithName);
            CustomMappedPMProperties.Add(PMPropertyNames.Fields);
            CustomMappedPMProperties.Add(PMPropertyNames.ToDoConditions);
            CustomMappedPMProperties.Add(PMPropertyNames.DoneConditions);
            CustomMappedPMProperties.Add(PMPropertyNames.Description);

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

        private void BuildSearchFields(TaskPM entityPM, Task entityPOCO)
        {
            string taskTypeName = EntityFieldsMapping.GetTaskTypeName(entityPOCO.TaskTypeId, entityPOCO.Tenant);
            string[] values = new string[] { entityPM.Subject, taskTypeName };
            string searchFields = EntityFieldsMapping.GetSearchFields(values);
            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }
    }
}