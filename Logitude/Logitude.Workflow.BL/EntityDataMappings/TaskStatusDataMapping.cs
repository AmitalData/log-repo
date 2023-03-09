using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskStatusDataMapping: IMapping<TaskStatusPM, TaskStatus>
   {
        public void CustomPMToPOCO(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            entityPM.CreatedByUserName = GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
        }

        private string GetUserName(string userId, int tenant)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var userRepository = new UserRepository(tenant);
                var user = userRepository.GetSingleUser(userId, tenant, false);
                return user?.Contact?.EnglishName;
            }
            return null;
        }

        private void BuildSearchFields(TaskStatusPM entityPM, TaskStatus entityPOCO)
        {
            string searchFields = "";

            searchFields = AppendToSearchFields(searchFields, entityPM.Code);
            searchFields = AppendToSearchFields(searchFields, entityPM.Name);

            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }

        private string AppendToSearchFields(string searchFields, string searchField)
        {
            if (string.IsNullOrEmpty(searchFields))
            {
                return searchField;
            }

            return searchFields + (string.IsNullOrEmpty(searchField) ? "" : ",") + searchField;
        }
    }
}