using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TransportModeMapping
    {
        public static void MapEntity(TransportModePM entityPM, TransportMode poco, bool isNewState)
        {
            poco.Id = entityPM.Id;
            poco.Name = entityPM.Name;
            poco.SearchFields = entityPM.SearchFields; 

        }
    }
}