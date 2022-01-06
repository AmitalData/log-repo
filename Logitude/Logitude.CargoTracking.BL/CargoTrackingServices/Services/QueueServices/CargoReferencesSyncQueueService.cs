using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.QueueServices
{
    public class CargoReferencesSyncQueueService
    {
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const int ShipmentTable_GetShipmentOrders = 3;
        public void InsertToQueue(BulkDataPreperation bulkDataPreperation)
        {
            var shipmentsQueryType = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition;
            switch (shipmentsQueryType)
            {
                case ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments:

                    break;

            }
        }
    }
}
