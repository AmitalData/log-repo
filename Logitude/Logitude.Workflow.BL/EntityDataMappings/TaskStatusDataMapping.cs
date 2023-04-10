using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Workflow.BL.FieldsMapping;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskStatusDataMapping: IMapping<TaskStatusPM, TaskStatus>
   {
        public void CustomPMToPOCO(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
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
        
        private void ResetPMDummyFields(TaskStatusPM entityPM)
        {
            entityPM.CreatedByUserName = null;
            entityPM.UpdatedByUserName = null;
        }

        private void BuildSearchFields(TaskStatusPM entityPM)
        {
            string[] values = new string[] { entityPM.Code, entityPM.Name };
            string searchFields = EntityFieldsMapping.GetSearchFields(values);
            entityPM.SearchFields = searchFields;
        }

        public void CustomPOCOToPM(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            entityPM.CreatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
        }
    }
}