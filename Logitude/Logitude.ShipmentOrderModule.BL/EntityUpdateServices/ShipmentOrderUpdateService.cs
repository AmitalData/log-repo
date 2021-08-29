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

            SetShipmentId(entityPM);

        }

        protected override void OnUpdating(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            SetShipmentId(entityPM);
        }

        private static void SetShipmentId(ShipmentOrderPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.ShipmentNumber))
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(entityPM.Tenant);
                entityPM.ShipmentId = shipmentQuery.GetShipmentIdByShipmentNumber(entityPM.ShipmentNumber, entityPM.Tenant);
            }
        }
    }
}
