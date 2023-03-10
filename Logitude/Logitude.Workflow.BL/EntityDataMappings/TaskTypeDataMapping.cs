using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Workflow.BL.FieldsMapping;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskTypeDataMapping: IMapping<TaskTypePM, TaskType>
   {
        public void CustomPMToPOCO(TaskTypePM entityPM, TaskType entityPOCO)
        {
            if(entityPM.ChangeSetOp == ChangeSetOperation.Insert)
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

        public void CustomPOCOToPM(TaskTypePM entityPM, TaskType entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.EntityObjectTableName);
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            entityPM.EntityObjectTableName = EntityFieldsMapping.GetEntityObjectTableName(entityPOCO.EntityObjectTableId, entityPOCO.Tenant);
            entityPM.CreatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
        }

        private void BuildSearchFields(TaskTypePM entityPM, TaskType entityPOCO)
        {
            string[] values = new string[] { entityPM.Name };
            string searchFields = EntityFieldsMapping.GetSearchFields(values);
            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }
    }
}