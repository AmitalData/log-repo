using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class AdditionalCurrencyRateMapping
    {
        public static void MapEntity(AdditionalCurrencyRatePM entityPM, AdditionalCurrencyRate entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.RateCoefficient = entityPM.RateCoefficient;
            entityPOCO.SearchFields = entityPM.Name + "," + entityPM.RateCoefficient;
        }
    }
}
