using Logitude.ShipmentOrderModule.Def.EntityAMs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.EntityMapping
{
    public class ShipmentOrderAmMap
    {
        public ShipmentOrderAM GetForImporterShipmentOrders(int tenant, ShipmentOrderPM shipmentOrder)
        {
            ShipmentOrderAM shipmentOrderAM = new ShipmentOrderAM();
            shipmentOrderAM.Id = shipmentOrder.Id;
            shipmentOrderAM.CustomerShipmentNumber = shipmentOrder.CustomerShipmentNumber;
            shipmentOrderAM.CustomerTenantNumber = shipmentOrder.CustomerTenantNumber.Value;

            return shipmentOrderAM;
        }
    }
}
