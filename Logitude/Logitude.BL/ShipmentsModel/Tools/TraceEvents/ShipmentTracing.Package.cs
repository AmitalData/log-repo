using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        public void TracePackage(ShipmentPackagePM shipmentPackagePM, ShipmentPackage shipmentPackage, ShipmentPM shipmentPM)
        {
            string containerStrippedEventCode = "COST";
            if (RoutingDate.IsDateAddedOrModified(shipmentPackagePM.ContainerStrippedDate, shipmentPackage.ContainerStrippedDate))
            {
                this.CreateTraceEvent(containerStrippedEventCode, shipmentPackagePM.ContainerStrippedDate, shipmentPackagePM);
            }

            else if (RoutingDate.IsDateRemoved(shipmentPackagePM.ContainerStrippedDate, shipmentPackage.ContainerStrippedDate))
            {
                this.DeleteTraceEvent(containerStrippedEventCode, shipmentPackagePM.Id, shipmentPackage.ContainerStrippedDate);
            }
        }

        public void TraceDeletedPackage(ShipmentPackagePM shipmentPackagePM, ShipmentPackage shipmentPackage, ShipmentPM shipmentPM)
        {
            string containerStrippedEventCode = "COST";
            if (shipmentPackage.ContainerStrippedDate != null)
            {
                this.DeleteTraceEvent(containerStrippedEventCode, shipmentPackagePM.Id, shipmentPackage.ContainerStrippedDate);
            }
        }
    }
}
