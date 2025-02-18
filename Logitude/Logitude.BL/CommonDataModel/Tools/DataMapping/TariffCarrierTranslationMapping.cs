using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class TariffCarrierTranslationMapping
    {
        public static void MapEntity(TariffCarrierTranslationPM entityPM, TariffCarrierTranslation entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;                
                entityPOCO.CarrierId = entityPM.CarrierId;
            }

            entityPOCO.PortId = entityPM.PortId;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.PartnerCode = entityPM.PartnerCode;
            entityPOCO.SearchFields = entityPM.PortCode + "," + entityPM.PartnerCode;
        }
    }
}
