
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.BL.EntityPMs; 
using Logitude.ShipmentOrderModule.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.ShipmentOrderModule.BL.EntityDataMappings
{
   
   public partial class ShipmentOrderDataMapping: IMapping<ShipmentOrderPM, ShipmentOrder>
   {

        public void CustomPMToPOCO(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;

        }

        public void CustomPOCOToPM(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            if (!string.IsNullOrEmpty(entityPOCO.TransportModeId))
            {
                TransportModeQuery transportModeQuery = new TransportModeQuery(entityPM.Tenant);
                TransportModePM transportModePM = transportModeQuery.GetSinglePM(entityPM.TransportModeId, entityPM.Tenant);
                entityPM.TransportModeName = transportModePM.Name;
            }
            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
            {
                ShipmentTypeQuery shipmentTypeQuery = new ShipmentTypeQuery(entityPM.Tenant);
                ShipmentTypePM shipmentTypePM = shipmentTypeQuery.GetSinglePM(entityPM.ShipmentTypeId, entityPM.Tenant);
                entityPM.ShipmentTypeName = shipmentTypePM.Name;
            }
        }
   }


}
   