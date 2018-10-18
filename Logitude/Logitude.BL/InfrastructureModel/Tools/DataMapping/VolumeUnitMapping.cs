using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class VolumeUnitMapping
    {
        public static void MapEntity(VolumeUnitPM entityPM, VolumeUnit poco, bool isNewState)
        {
            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name;
            poco.SearchFields = entityPM.SearchFields; 

        }
    }
}