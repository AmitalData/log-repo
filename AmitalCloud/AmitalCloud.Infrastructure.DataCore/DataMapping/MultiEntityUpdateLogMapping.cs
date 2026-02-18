using AmitalCloud.Infrastructure.Data.Counters;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class MultiEntityUpdateLogMapping
    {
        public static void MapEntity(MultiEntityUpdateLogPM entityPM, MultiEntityUpdateLog entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Tenant = entityPM.Tenant;
                entity.Id = IdCounter.GetNumber("MultiEntityUpdateLog", entity.Tenant).ToString(); ;
                entity.CreateDate = TenantServerConfigration.GetCurrentDateTime(0);
                entity.CreatedByUserId = entityPM.CreatedByUserId;
                entityPM.StatusCode = "W";
                entityPM.RetryNumber = 0;
            }

            entity.UpdatedEntitiesNumber = entityPM.UpdatedEntitiesNumber;
            entity.ExceptionMessage = entityPM.ExceptionMessage;
            entity.DoneDate = entityPM.DoneDate;
            entity.XMLData = entityPM.XMLData;
            entity.ObjectTableId = entityPM.ObjectTableId;
            entity.StartDate = entityPM.StartDate;
            entity.StatusCode = entityPM.StatusCode;
            entity.RetryNumber = entityPM.RetryNumber;
        }
    }
}
