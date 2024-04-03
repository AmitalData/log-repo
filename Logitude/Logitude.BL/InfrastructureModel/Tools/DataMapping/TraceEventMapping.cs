using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TraceEventMapping
    {
        public static void MapEntity(TraceEventPM traceEventPM, TraceEvent traceEvent, bool isNewState)
        {
            traceEvent.EntityId = traceEventPM.EntityId;
            traceEvent.EventDateTime = traceEventPM.EventDateTime;
            traceEvent.LogDateTime = traceEventPM.LogDateTime;
            traceEvent.EventTypeId = traceEventPM.EventTypeId;
            traceEvent.Notes = traceEventPM.Notes;
            traceEvent.ObjectTableId = traceEventPM.ObjectTableId;
            traceEvent.Tenant = traceEventPM.Tenant;
            traceEvent.UserId = traceEventPM.UserId;
            traceEvent.Deleted = traceEventPM.Deleted;
            traceEvent.ExternalId = traceEventPM.ExternalId;
            traceEvent.IsAddedManually = traceEventPM.IsAddedManually;
            traceEvent.Location = traceEventPM.Location;
            traceEvent.PartnerName = traceEventPM.PartnerName;
            traceEvent.ChildEntityId = traceEventPM.ChildEntityId;
            traceEvent.ChildObjectTableId = traceEventPM.ChildObjectTableId;
        }
    }
}