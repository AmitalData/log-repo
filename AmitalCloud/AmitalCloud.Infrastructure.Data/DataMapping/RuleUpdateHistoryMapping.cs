using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class RuleUpdateHistoryMapping
    {
        public static void MapEntity(RuleUpdateHistoryPM entityPM, RuleUpdateHistory entityPOCO, bool isNewState)
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
            entityPOCO.RuleCode = entityPM.RuleCode;
            entityPOCO.EventName = entityPM.EventName;

        }
    }
}
