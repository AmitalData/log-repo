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
    public class ComputingPartnerTableMapping
    {
        public static void MapEntity(ComputingPartnerTablePM entityPM, ComputingPartnerTable entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ObjectTableId = entityPM.ObjectTableId;
                entityPOCO.ComputingPartnerId = entityPM.ComputingPartnerId;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.HasPartnerList = entityPM.HasPartnerList;
            entityPOCO.MustUsePartnerList = entityPM.MustUsePartnerList;
            entityPOCO.TransalationRequired = entityPM.TransalationRequired;
            entityPOCO.TenantLevelTranslationBlocked = entityPM.TenantLevelTranslationBlocked; 
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
        }
    }
}