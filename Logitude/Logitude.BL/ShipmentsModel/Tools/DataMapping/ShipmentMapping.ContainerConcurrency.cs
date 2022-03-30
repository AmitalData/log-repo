using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        private static bool isNew;
        public static void MapContainerConcurrencyFields(ContainerPM entityPM, Container entityPoco, bool isNewEntity)
        {
            isNew = isNewEntity;
            if (isNew)
            {
                MapContainerConcurrencyFields_Client(entityPM, entityPoco);
                return;
            }
            if (entityPM.IsUpdatedOceanInsightsAnalyzer)
            {
                MapConcurrencyFields_OceanInsightsFields(entityPM, entityPoco);
                return;
            }
            MapContainerConcurrencyFields_Client(entityPM, entityPoco);
        }
        private static void MapContainerConcurrencyFields_Client(ContainerPM entityPM, Container entityPoco)
        {
            entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            ShipmentMapping.MapContainerFields(entityPM, entityPoco, isNew);
            ShipmentMapping.MapContainerShipmentFields(entityPM, entityPoco);
        }
        private static void MapConcurrencyFields_OceanInsightsFields(ContainerPM entityPM, Container entityPoco)
        {
            ShipmentMapping.MapContainerFields(entityPM, entityPoco, isNew);
        }
    }
}
