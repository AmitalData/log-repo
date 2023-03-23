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
                entityPOCO.Id = entityPM.Id;
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                entityPM.CreateDate = entityPOCO.CreateDate;
                entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

            ResetPMDummyFields(entityPM);
            BuildSearchFields(entityPM);
        }
        
        private void ResetPMDummyFields(TaskTypePM entityPM)
        {
            entityPM.EntityObjectTableName = null;
            entityPM.CreatedByUserName = null;
            entityPM.UpdatedByUserName = null;
        }

        private void BuildSearchFields(TaskTypePM entityPM)
        {
            string[] values = new string[] { entityPM.Name };
            string searchFields = EntityFieldsMapping.GetSearchFields(values);
            entityPM.SearchFields = searchFields;
        }

        public void CustomPOCOToPM(TaskTypePM entityPM, TaskType entityPOCO)
        {
            entityPM.EntityObjectTableName = EntityFieldsMapping.GetEntityObjectTableName(entityPOCO.EntityObjectTableId, entityPOCO.Tenant);
            entityPM.CreatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
        }
    }
}