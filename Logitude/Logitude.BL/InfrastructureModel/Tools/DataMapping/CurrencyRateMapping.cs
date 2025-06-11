using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class CurrencyRateMapping
        {
        public static void MapEntity(CurrencyRatePM entityPM, CurrencyRate entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.ExchangeRateId = entityPM.ExchangeRateId;
                entity.AdditionalCurrencyRateId = entityPM.AdditionalCurrencyRateId;
            }

            entity.Rate = entityPM.Rate;
        }
    }
}