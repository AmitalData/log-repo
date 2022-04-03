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
            entityPM.ConcurrencyGUID = Guid.NewGuid().ToString();
            MapShipmentFieldsFromOceanInsights(entityPM, entityPoco);
            ShipmentMapping.MapContainerFields(entityPM, entityPoco, isNew);
        }

        private static void MapShipmentFieldsFromOceanInsights(ContainerPM entityPM, Container entityPoco)
        {
            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPoco.MainCarriageATA = entityPM.MainCarriageATA;
            entityPoco.MainCarriageATD = entityPM.MainCarriageATD;
            entityPoco.MainCarriageETA = entityPM.MainCarriageETA;
            entityPoco.MainCarriageETD = entityPM.MainCarriageETD;
            entityPoco.ShipmentOnCarriageETA = entityPM.ShipmentOnCarriageETA;
            entityPoco.ShipmentOnCarriageETD = entityPM.ShipmentOnCarriageETD;
            entityPoco.ShipmentOnCarriageATA = entityPM.ShipmentOnCarriageATA;
            entityPoco.ShipmentOnCarriageATD = entityPM.ShipmentOnCarriageATD;
            entityPoco.ShipmentPreCarriageETA = entityPM.ShipmentPreCarriageETA;
            entityPoco.ShipmentPreCarriageETD = entityPM.ShipmentPreCarriageETD;
            entityPoco.ShipmentPreCarriageATA = entityPM.ShipmentPreCarriageATA;
            entityPoco.ShipmentPreCarriageATD = entityPM.ShipmentPreCarriageATD;
            entityPoco.ShipmentMainCarriageETA = entityPM.ShipmentMainCarriageETA;
            entityPoco.ShipmentMainCarriageETD = entityPM.ShipmentMainCarriageETD;
            entityPoco.ShipmentMainCarriageATA = entityPM.ShipmentMainCarriageATA;
            entityPoco.ShipmentMainCarriageATD = entityPM.ShipmentMainCarriageATD;
        }
    }
}
