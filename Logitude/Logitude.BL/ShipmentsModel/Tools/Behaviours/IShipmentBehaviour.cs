using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Infrastructure.Data.Models.AuditLog;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    interface IShipmentBehaviour
    {
        bool ReceivablePricingUpdated { get; set; }
        bool DatesFromCrossDocsUpdated { get; set; }
        void Handle(List<FieldChange> fieldChanges = null);

        void Save();
        void Trace(ShipmentTracing shipmentTracing);

    }
}
