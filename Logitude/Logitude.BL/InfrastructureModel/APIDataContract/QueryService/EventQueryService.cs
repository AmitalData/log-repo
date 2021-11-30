using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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

        public void CreateShipmentTraceEvents(ShipmentPM shipmentPM, List<Event> addManualEvents, bool isNewEntity, string computingPartnerName)
        {
            shipmentPM.AddManualEvents = BuildTraceEventPMs(addManualEvents, shipmentPM.Tenant, computingPartnerName);
            this.CreateTraceEvent(shipmentPM);             
        }
        private List<TraceEventPM> BuildTraceEventPMs(List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                var MyList = new List<TraceEventPM>();
                UserRepository userRepository = new UserRepository(tenant);
                UserQueryService userQueryService = new UserQueryService(tenant);
                EventTypeQueryService eventTypeQueryService = new EventTypeQueryService(tenant);

                foreach (Event item in eventList)
                {
                    TraceEventPM temp = new TraceEventPM();
                    temp.Id = item.Id;
                    temp.EventDateTime = item.EventDateTime;
                    temp.Notes = item.Notes;

                    if (item.CreatedBy != null)
                    {
                        var myUserPM = userQueryService.UserDataMappingAndValidatin(item.CreatedBy, tenant, computingPartnerName, false);
                        if (myUserPM != null)
                        {
                            temp.UserId = myUserPM.Id;
                        }
                    }

                    else
                    {
                        Simplog.Data.CommonDataModel.EntityPOCOs.User systemUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, true);
                        if (systemUser != null)
                        {
                            temp.UserId = systemUser.Id;
                        }
                    }

                    if (item.EventType != null)
                    {
                        var myEventTypePM = eventTypeQueryService.EventTypeDataMappingAndValidatin(item.EventType, tenant, computingPartnerName, false);
                        if (myEventTypePM != null)
                        {
                            temp.EventTypeId = myEventTypePM.Id;
                            temp.EventTypeCode = myEventTypePM.Code;
                        }
                    }

                    MyList.Add(temp);
                }

                return MyList;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CreateTraceEvent(ShipmentPM shipmentPM)
        {
            AddManualTraceEventsHelper addManualTraceEventsHelper = new AddManualTraceEventsHelper(shipmentPM.Tenant);
            List<TraceEvent> shipmentEvents = this.GetShipmentEvents(shipmentPM);
            foreach (TraceEventPM traceEvent in shipmentPM.AddManualEvents)
            {
                this.ValidateEventType(traceEvent, shipmentPM.Tenant);
                bool isExists = this.IsTraceEventAdded(traceEvent, shipmentEvents);

                if (!isExists)
                {                    
                    NewTraceEventResult myResult = addManualTraceEventsHelper.Trace(new TraceEventsServiceArgs()
                    {
                        EntityId = shipmentPM.Id,
                        ObjectTableId = ObjectTableRepository.GetObjectTableByName("Shipment"),
                        EventTypeId = traceEvent.EventTypeId,
                        UserId = traceEvent.UserId,
                        EventDate = traceEvent.EventDateTime,
                        Notes = traceEvent.Notes,
                        IsFromAPI = true,
                    }
                    , "");
                }
            }
        }
        private List<TraceEvent> GetShipmentEvents(ShipmentPM shipmentPM)
        {
            TraceEventRepository traceEventRepository = new TraceEventRepository(shipmentPM.Tenant);
            return traceEventRepository.GetTraceEvents(shipmentPM.Tenant, shipmentPM.Id, ObjectTableRepository.GetObjectTableByName("Shipment")).ToList();
        }
        private bool IsTraceEventAdded(TraceEventPM traceEvent, List<TraceEvent> shipmentEvents)
        {
            return shipmentEvents.Where(e => e.EventTypeId == traceEvent.EventTypeId && e.EventDateTime == traceEvent.EventDateTime).Any();
        }
        private void ValidateEventType(TraceEventPM traceEvent, int tenant)
        {
            EventTypeRepository eventTypeRepository = new EventTypeRepository(tenant);
            var shipmentTableId = ObjectTableRepository.GetObjectTableByName("Shipment");
            var eventTypePOCO = eventTypeRepository.GetSingleEventTypeByCodeAndObjectTableId(traceEvent.EventTypeCode, shipmentTableId, tenant);

            if (eventTypePOCO == null)
            {
                throw new ApplicationException("Event Type is not allowed for shipment");
            }

            if (!eventTypePOCO.IsManualEntry)
            {
                throw new ApplicationException("Event Type is not allowed for manual entry");
            }
        }
    }
}
