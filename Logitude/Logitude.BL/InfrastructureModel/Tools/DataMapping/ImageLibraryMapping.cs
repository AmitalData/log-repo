using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ImageLibraryMapping
    {
        public static void MapEntity(ImageLibraryPM entityPM, ImageLibrary entityPOCO, bool isNewState)
        {

            if (isNewState)
            {
                entityPOCO.Id = IdCounter.GetNumber("ImageLibrary", 0);
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
                entityPOCO.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.SecurityId = entityPM.SecurityId;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.ImageDetailId = entityPM.ImageDetailId;

            BuildSearchFields(entityPM, entityPOCO, isNewState);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private static void BuildSearchFields(ImageLibraryPM entityPM, ImageLibrary entityPOCO, bool isNewState)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            entityPM.SearchFields = mySearchFields;
        }
    }
}
