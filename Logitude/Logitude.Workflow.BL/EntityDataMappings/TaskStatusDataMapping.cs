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

        public void CustomPOCOToPM(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            entityPM.CreatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = EntityFieldsMapping.GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
        }

        private void BuildSearchFields(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            string[] values = new string[] { entityPM.Code, entityPM.Name };
            string searchFields = EntityFieldsMapping.GetSearchFields(values);
            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }
    }
}