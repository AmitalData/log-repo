using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.CargoTracking.BL.CoreBL
{
    public class CargoTrackingEventsBuilder
    {
        public List<Event> BuildShipmentEvents(string entityId, int tenant, string forwardingShipmentHeaderId)
        {
            var eventTypeQuery = new EventTypeQuery();

            var events = eventTypeQuery.GetEventByShipment(entityId, tenant, forwardingShipmentHeaderId);
            List<Event> Events = new List<Event>();
            foreach (var e in events)
            {
                Event Event = new Event();
                Event.LocalName = e.LocalName;
                Event.EventDatetime = e.EventDatetime;
                Event.Notes = e.Notes;
                Event.IsChoose = e.IsChoose;
                Event.PartnerTypeId = e.PartnerTypeId;
				Event.EntityType = e.EntityType;

				Events.Add(Event);
            }
            return Events;
        }

    }
}
