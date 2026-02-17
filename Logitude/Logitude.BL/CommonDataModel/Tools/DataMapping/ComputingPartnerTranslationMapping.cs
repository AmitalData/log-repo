using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ComputingPartnerTranslationMapping
    {
        public static void MapEntity(ComputingPartnerTranslationPM entityPM, ComputingPartnerTranslation entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
                entityPOCO.ComputingPartnerId = entityPM.ComputingPartnerId;
                entityPOCO.ObjectTableId = entityPM.ObjectTableId;
                entityPOCO.OurCode = entityPM.OurCode;
            }

            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.PartnerCode = entityPM.PartnerCode;
            entityPOCO.SearchFields = entityPM.OurCode+","+entityPM.PartnerCode;

        }
    }
}