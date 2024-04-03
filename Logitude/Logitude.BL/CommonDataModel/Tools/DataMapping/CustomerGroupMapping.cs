using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomerGroupMapping
    {
        public static void MapEntity(CustomerGroupPM entityPM, CustomerGroup entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                MapNewEntityFields(entityPM, entityPOCO);
            }

            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.InActive = entityPM.InActive;
            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void MapNewEntityFields(CustomerGroupPM entityPM, CustomerGroup entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
        }

        private static void BuildSearchFields(CustomerGroupPM entityPM, CustomerGroup entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
