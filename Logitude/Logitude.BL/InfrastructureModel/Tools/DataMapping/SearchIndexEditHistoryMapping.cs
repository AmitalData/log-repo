using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class SearchIndexEditHistoryMapping
    {
        public static void MapEntity(SearchIndexEditHistoryPM entityPM, SearchIndexEditHistory entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.CreateDate = entityPM.CreateDate;
                entity.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entity.Screen = entityPM.Screen;
            entity.ScreenParam = entityPM.ScreenParam;
            entity.Entname = entityPM.Entname;
            entity.KeyVal = entityPM.KeyVal;
        }
    }
}
