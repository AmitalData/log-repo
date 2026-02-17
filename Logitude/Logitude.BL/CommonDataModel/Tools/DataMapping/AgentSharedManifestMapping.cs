using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
   public class AgentSharedManifestMapping
    {
        public static void MapEntity(AgentSharedManifestPM entityPM, AgentSharedManifest entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.ShipmentTypeId = entityPM.ShipmentTypeId;
            entityPOCO.AgentReference = entityPM.AgentReference;
            entityPOCO.Master = entityPM.Master;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.ManifestXML = entityPM.ManifestXML;

            entityPOCO.SearchFields = entityPM.AgentReference + "," + entityPM.Master;
            entityPOCO.TransportModeId = entityPM.TransportModeId;
            entityPOCO.GrossWeight = entityPM.GrossWeight;
            entityPOCO.ChargeableWeight = entityPM.ChargeableWeight;
            entityPOCO.TEU = entityPM.TEU;
            entityPOCO.PackagesQuantity = entityPM.PackagesQuantity;
            entityPOCO.AgentId = entityPM.AgentId;
            entityPOCO.DirectionId = entityPM.DirectionId;
            entityPOCO.FromPortId = entityPM.FromPortId;
            entityPOCO.ToPortId = entityPM.ToPortId;
            entityPOCO.StatusCode = entityPM.StatusCode;
            entityPOCO.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            entityPOCO.CancelledBySenderAgent = entityPM.CancelledBySenderAgent;

        }
    }
}
