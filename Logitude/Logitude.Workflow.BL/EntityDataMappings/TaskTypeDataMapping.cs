using Logitude.Server.Tools; 
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class TaskTypeDataMapping: IMapping<TaskTypePM, TaskType>
   {
        public void CustomPMToPOCO(TaskTypePM entityPM, TaskType entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(TaskTypePM entityPM, TaskType entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.EntityObjectTableName);
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            entityPM.EntityObjectTableName = GetEntityObjectTableName(entityPOCO.EntityObjectTableId, entityPOCO.Tenant);
            entityPM.CreatedByUserName = GetUserName(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
            entityPM.UpdatedByUserName = GetUserName(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
        }

        private string GetEntityObjectTableName(string entityObjectTableId, int tenant)
        {
            if (!string.IsNullOrEmpty(entityObjectTableId))
            {
                var objectTableRepository = new ObjectTableRepository(tenant);
                var objectTable = objectTableRepository.GetSingleObjectTable(entityObjectTableId, tenant, false);
                return objectTable != null ? (objectTable.FullNameTextCode != null ? objectTable.FullNameTextCode.DefaultText : objectTable.Name) : null;
            }
            return null;
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


        private void BuildSearchFields(TaskTypePM entityPM, TaskType entityPOCO)
        {
            string searchFields = "";

            searchFields = AppendToSearchFields(searchFields, entityPM.Name);
            //searchFields = AppendToSearchFields(searchFields, entityPM.Description);

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