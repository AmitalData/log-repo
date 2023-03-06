using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class RatesTableMapping
    {
        public static void MapEntity(RatesTablePM entityPM, RatesTable entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Tenant = entityPM.Tenant;
                entity.BaseCurrencyId = entityPM.BaseCurrencyId;
                entity.ForeignCurrencyId = entityPM.ForeignCurrencyId;
                entity.LogDateTime = TenantServerConfigration.GetCurrentDateTime(entity.Tenant);
            }

            entity.Rate = entityPM.Rate;
            entity.Unit = entityPM.Unit;
            entity.ValueDate = entityPM.ValueDate.Value.Date;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.UpdatedDate = entityPM.UpdatedDate;
        }
    }
}