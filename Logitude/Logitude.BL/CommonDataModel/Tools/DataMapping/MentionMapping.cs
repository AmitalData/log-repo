using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class MentionMapping
    {
        public static void MapEntity(MentionPM entityPM, Mention entityPOCO, bool isNewState)
        {

            if (isNewState) SetNewEntityDefaultValues(entityPM, entityPOCO);
            
            entityPOCO.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.InActive = entityPM.InActive;

            BuildSearchFields(entityPM, entityPOCO, isNewState);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private static void SetNewEntityDefaultValues(MentionPM entityPM, Mention entityPOCO)
        {
            entityPOCO.Id = IdCounter.GetNumber("Mention", 0);
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;
        }

        private static void BuildSearchFields(MentionPM entityPM, Mention entityPOCO, bool isNewState)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Description);
            entityPM.SearchFields = mySearchFields;
        }
    }
}
