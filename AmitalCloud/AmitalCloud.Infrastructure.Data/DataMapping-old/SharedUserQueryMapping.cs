using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityPMs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class SharedUserQueryMapping
    {
        public static void MapEntity(SharedUserQueryPM entityPM, SharedUserQuery entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.UserId = entityPM.UserId;
            entityPOCO.QueryId = entityPM.QueryId;
            entityPOCO.QueryCode = entityPM.QueryCode;

        }
    }
}
