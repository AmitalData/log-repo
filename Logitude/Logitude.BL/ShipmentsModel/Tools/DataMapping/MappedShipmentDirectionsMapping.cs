using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public class MappedShipmentDirectionsMapping
    {
        public static void MapEntity(MappedShipmentDirectionsPM entityPM, MappedShipmentDirections entityPOCO, bool isNewEntity)
        {
            //if (isNewEntity)
            //{
            //    entityPOCO.Id = entityPM.Id;
            //}

            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.ShipmentDirectionId = entityPM.ShipmentDirectionId;
            entityPOCO.UpdateDateTime = entityPM.UpdateDateTime; 
        }

        
    }
}
