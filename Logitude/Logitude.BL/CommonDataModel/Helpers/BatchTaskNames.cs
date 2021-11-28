using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Helpers
{
    public static class BatchTaskNames
    {
        public const string UpdateShipmentsForIncrementalService = "Update all shipments and orders for Cargo Incremental service";
        public const string BuildCargoTrackingShipments = "Build Cargo Tracking Shipments";
    }
}
