using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DirectionMapping
    {
        public static void MapEntity(DirectionPM entityPM, Direction poco, bool isNewState)
        {
            poco.Id = entityPM.Id;
            poco.Name = entityPM.Name;
            poco.SearchFields = entityPM.SearchFields; 

        }
    }
}