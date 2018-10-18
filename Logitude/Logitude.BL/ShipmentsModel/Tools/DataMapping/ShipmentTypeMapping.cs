using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public class ShipmentTypeMapping
    {
        public static void MapEntity(ShipmentTypePM entityPM, ShipmentType poco, bool isNewState)
        {
            poco.Id = entityPM.Id;
            poco.Name = entityPM.Name;
            poco.TransportModeId = entityPM.TransportModeId;
            poco.SearchFields = entityPM.SearchFields; 

        }
    }
}