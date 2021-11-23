using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
    public partial class EventQueryService
    {
        public List<Event> EventCustomDataMapping(ShipmentPM shipmentPM, List<TraceEventPM> traceEventPMs, int tenant, string computingPartnerName = "")
        {
            try
            {
                var MyList = new List<Event>();

                foreach (TraceEventPM traceEventPM in traceEventPMs)
                {
                    MyList.Add(this.CreateAPIEvent(traceEventPM, computingPartnerName));
                }

                return MyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Event CreateAPIEvent(TraceEventPM traceEventPM, string computingPartnerName)
        {
            Event traceEvent = new Event()
            {
                Id = traceEventPM.Id,
                EventDateTime = traceEventPM.EventDateTime,
                LogDateTime = traceEventPM.LogDateTime,
                Notes = traceEventPM.Notes,
                IsAddedManually = traceEventPM.IsAddedManually,
            };

            if (traceEventPM.UserId != null)
            {
                UserQueryService Service = new UserQueryService(traceEventPM.Tenant);
                traceEvent.CreatedBy = Service.GetUserById(traceEventPM.UserId, traceEventPM.Tenant, computingPartnerName);
            }

            if (traceEventPM.EventTypeId != null)
            {
                EventTypeQueryService Service = new EventTypeQueryService(traceEventPM.Tenant);
                traceEvent.EventType = Service.GetEventTypeById(traceEventPM.EventTypeId, traceEventPM.Tenant, computingPartnerName);
            }

            return traceEvent;
        }
        public List<TraceEventPM> EventCustomDataMappingAndValidatin(Direct myEntity, List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                return null;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TraceEventPM> EventCustomDataMappingAndValidatin(House myEntity, List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                return null;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TraceEventPM> EventCustomDataMappingAndValidatin(Master myEntity, List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                return null;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
