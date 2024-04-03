using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        private static bool isNew;
        public static void MapContainerConcurrencyFields(ContainerPM entityPM, Container entityPoco, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            isNew = isNewEntity;
            if (isNew)
            {
                MapContainerConcurrencyFields_Client(entityPM, entityPoco, fieldChanges);
                return;
            }
            if (entityPM.IsUpdatedOceanInsightsAnalyzer)
            {
                MapConcurrencyFields_OceanInsightsFields(entityPM, entityPoco, fieldChanges);
                return;
            }
            MapContainerConcurrencyFields_Client(entityPM, entityPoco, fieldChanges);
        }
        private static void MapContainerConcurrencyFields_Client(ContainerPM entityPM, Container entityPoco, List<FieldChange> fieldChanges)
        {
            if (!entityPM.IsShipmentBatchUpdate)
            {
                FieldChange.Add(entityPoco.ConcurrencyGUID, entityPM.NewConcurrencyGUID, nameof(entityPM.ConcurrencyGUID), fieldChanges);
                entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;

                FieldChange.Add(entityPoco.ConcurrencyGUID, entityPM.NewConcurrencyGUID, nameof(entityPM.ConcurrencyGUID), fieldChanges);
                entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            }

            ShipmentMapping.MapContainerFields(entityPM, entityPoco, isNew, fieldChanges);
            ShipmentMapping.MapContainerShipmentFields(entityPM, entityPoco, fieldChanges);
        }
        private static void MapConcurrencyFields_OceanInsightsFields(ContainerPM entityPM, Container entityPoco, List<FieldChange> fieldChanges)
        {
            var newGuid = Guid.NewGuid().ToString();
            FieldChange.Add(entityPoco.ConcurrencyGUID, newGuid, nameof(entityPM.ConcurrencyGUID), fieldChanges);
            entityPM.ConcurrencyGUID = newGuid;
            
            MapShipmentFieldsFromOceanInsights(entityPM, entityPoco, fieldChanges);
            ShipmentMapping.MapContainerFields(entityPM, entityPoco, isNew, fieldChanges);
        }

        private static void MapShipmentFieldsFromOceanInsights(ContainerPM entityPM, Container entityPoco, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.ConcurrencyGUID, entityPM.ConcurrencyGUID, nameof(entityPM.ConcurrencyGUID), fieldChanges);
            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            
            FieldChange.Add(entityPoco.MainCarriageATA, entityPM.MainCarriageATA, nameof(entityPM.MainCarriageATA), fieldChanges);
            entityPoco.MainCarriageATA = entityPM.MainCarriageATA;
            
            FieldChange.Add(entityPoco.MainCarriageATD, entityPM.MainCarriageATD, nameof(entityPM.MainCarriageATD), fieldChanges);
            entityPoco.MainCarriageATD = entityPM.MainCarriageATD;
            
            FieldChange.Add(entityPoco.MainCarriageETA, entityPM.MainCarriageETA, nameof(entityPM.MainCarriageETA), fieldChanges);
            entityPoco.MainCarriageETA = entityPM.MainCarriageETA;
            
            FieldChange.Add(entityPoco.MainCarriageETD, entityPM.MainCarriageETD, nameof(entityPM.MainCarriageETD), fieldChanges);
            entityPoco.MainCarriageETD = entityPM.MainCarriageETD;
            
            FieldChange.Add(entityPoco.ShipmentOnCarriageETA, entityPM.ShipmentOnCarriageETA, nameof(entityPM.ShipmentOnCarriageETA), fieldChanges);
            entityPoco.ShipmentOnCarriageETA = entityPM.ShipmentOnCarriageETA;
            
            FieldChange.Add(entityPoco.ShipmentOnCarriageETD, entityPM.ShipmentOnCarriageETD, nameof(entityPM.ShipmentOnCarriageETD), fieldChanges);
            entityPoco.ShipmentOnCarriageETD = entityPM.ShipmentOnCarriageETD;
            
            FieldChange.Add(entityPoco.ShipmentOnCarriageATA, entityPM.ShipmentOnCarriageATA, nameof(entityPM.ShipmentOnCarriageATA), fieldChanges);
            entityPoco.ShipmentOnCarriageATA = entityPM.ShipmentOnCarriageATA;
            
            FieldChange.Add(entityPoco.ShipmentOnCarriageATD, entityPM.ShipmentOnCarriageATD, nameof(entityPM.ShipmentOnCarriageATD), fieldChanges);
            entityPoco.ShipmentOnCarriageATD = entityPM.ShipmentOnCarriageATD;
            
            FieldChange.Add(entityPoco.ShipmentPreCarriageETA, entityPM.ShipmentPreCarriageETA, nameof(entityPM.ShipmentPreCarriageETA), fieldChanges);
            entityPoco.ShipmentPreCarriageETA = entityPM.ShipmentPreCarriageETA;
            
            FieldChange.Add(entityPoco.ShipmentPreCarriageETD, entityPM.ShipmentPreCarriageETD, nameof(entityPM.ShipmentPreCarriageETD), fieldChanges);
            entityPoco.ShipmentPreCarriageETD = entityPM.ShipmentPreCarriageETD;
            
            FieldChange.Add(entityPoco.ShipmentPreCarriageATA, entityPM.ShipmentPreCarriageATA, nameof(entityPM.ShipmentPreCarriageATA), fieldChanges);
            entityPoco.ShipmentPreCarriageATA = entityPM.ShipmentPreCarriageATA;
            
            FieldChange.Add(entityPoco.ShipmentPreCarriageATD, entityPM.ShipmentPreCarriageATD, nameof(entityPM.ShipmentPreCarriageATD), fieldChanges);
            entityPoco.ShipmentPreCarriageATD = entityPM.ShipmentPreCarriageATD;
            
            FieldChange.Add(entityPoco.ShipmentMainCarriageETA, entityPM.ShipmentMainCarriageETA, nameof(entityPM.ShipmentMainCarriageETA), fieldChanges);
            entityPoco.ShipmentMainCarriageETA = entityPM.ShipmentMainCarriageETA;
            
            FieldChange.Add(entityPoco.ShipmentMainCarriageETD, entityPM.ShipmentMainCarriageETD, nameof(entityPM.ShipmentMainCarriageETD), fieldChanges);
            entityPoco.ShipmentMainCarriageETD = entityPM.ShipmentMainCarriageETD;
            
            FieldChange.Add(entityPoco.ShipmentMainCarriageATA, entityPM.ShipmentMainCarriageATA, nameof(entityPM.ShipmentMainCarriageATA), fieldChanges);
            entityPoco.ShipmentMainCarriageATA = entityPM.ShipmentMainCarriageATA;
            
            FieldChange.Add(entityPoco.ShipmentMainCarriageATD, entityPM.ShipmentMainCarriageATD, nameof(entityPM.ShipmentMainCarriageATD), fieldChanges);
            entityPoco.ShipmentMainCarriageATD = entityPM.ShipmentMainCarriageATD;

            FieldChange.Add(entityPoco.ShipmentLastLegATA, entityPM.ShipmentLastLegATA, nameof(entityPM.ShipmentLastLegATA), fieldChanges);
            entityPoco.ShipmentLastLegATA = entityPM.ShipmentLastLegATA;

            FieldChange.Add(entityPoco.ShipmentLastLegETA, entityPM.ShipmentLastLegETA, nameof(entityPM.ShipmentLastLegETA), fieldChanges);
            entityPoco.ShipmentLastLegETA = entityPM.ShipmentLastLegETA;
        }
    }
}
