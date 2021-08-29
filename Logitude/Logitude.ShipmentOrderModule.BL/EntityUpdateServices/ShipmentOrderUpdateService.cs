using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderModule.BL.EntityUpdateServices
{
    public partial class ShipmentOrderUpdateService
    {
        protected override void OnCreating(ShipmentOrderPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.SecurityKey = Guid.NewGuid().ToString("N");
            }

            GetShipmentId(entityPM);

        }

        protected override void OnUpdating(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            GetShipmentId(entityPM);
        }

        private static void GetShipmentId(ShipmentOrderPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.ShipmentNumber))
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(entityPM.Tenant);
                string shipmentId = shipmentQuery.GetShipmentIdByShipmentNumber(entityPM.ShipmentNumber, entityPM.Tenant);
                if (string.IsNullOrEmpty(shipmentId))
                    throw new ApplicationException("Shipment with Shipment Number " + entityPM.ShipmentNumber + " doesn't exist");
                entityPM.ShipmentId = shipmentId;
            }
        }
    }
}
