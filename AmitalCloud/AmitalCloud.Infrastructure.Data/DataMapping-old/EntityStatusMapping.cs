using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityPMs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class EntityStatusMapping
    {
        public static void MapEntity(EntityStatusPM entityStatusPM, EntityStatus entityStatus, bool isNewState)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityStatusPM.Tenant);
            entityStatus.Name = entityStatusPM.Name;
            entityStatus.DisplayName = entityStatusPM.DisplayName;
            ObjectTable table = null;
            if (entityStatusPM.ObjectTableId != null)
            {
                table = objectTabelRepository.GetSingleObjectTable(entityStatusPM.ObjectTableId, entityStatus.Tenant, true);
            }

            if (string.IsNullOrEmpty(entityStatus.DisplayName))
            {
                entityStatus.DisplayName = entityStatusPM.DisplayName = entityStatusPM.Name;
            }

            entityStatus.ObjectTableId = entityStatusPM.ObjectTableId;
            entityStatus.StatusWeight = entityStatusPM.StatusWeight;
            entityStatus.Tenant = entityStatusPM.Tenant;
            entityStatus.Code = entityStatusPM.Code;
            entityStatus.InActive = entityStatusPM.InActive;
            entityStatus.SearchFields = entityStatusPM.Code + "," + entityStatusPM.Name + "," + (table != null ? table.Name : "");
            entityStatus.EntityStatusTypeCode = entityStatusPM.EntityStatusTypeCode;
            entityStatus.StatusLocalWeight = entityStatusPM.StatusLocalWeight;
            entityStatus.AllowPartial = entityStatusPM.AllowPartial;
            entityStatus.IsDigitalPortal = entityStatusPM.IsDigitalPortal;
        }
    }
}