using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public class ShipmentStatusesQueryService
    {
        protected int Tenant { get; set; }
        public ShipmentStatusesQueryService(int tenant)
        {
            this.Tenant = tenant;
        }
        public ShipmentStatuses GetShipmentStatuses(string houseNumber, bool latestStatusOnly)
        {
            Shipment shipment = GetShipmetByHouseNumber(houseNumber);
            if (shipment == null)
                throw new EntityNotFoundException("Waybill Number not found!");

            SecurityUtility.CheckSharedContactAuthentication(this.Tenant, shipment.CustomerId);

            List<TraceEventPM> shipmentEvents = GetShipmentStatusesEvents(shipment, latestStatusOnly);
            ShipmentStatuses shipmentStatuses = BuildShipmentStatues(shipment, shipmentEvents);

            return shipmentStatuses;
        }
        private Shipment GetShipmetByHouseNumber(string houseNumber)
        {
            if (string.IsNullOrEmpty(houseNumber))
                return null;

            var shipmentRepository = new ShipmentRepository(this.Tenant);
            var shipment = shipmentRepository.GetAllShipmentsByHouseNumber(houseNumber, this.Tenant).FirstOrDefault();
            return shipment;
        }
        private List<TraceEventPM> GetShipmentStatusesEvents(Shipment shipment, bool latestStatusOnly)
        {
            var objectTabelRepository = new ObjectTableRepository(0);
            var shipmentObjectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            var traceEventRepository = new TraceEventRepository(this.Tenant);
            var traceEventQuery = new TraceEventQuery(this.Tenant);

            if (latestStatusOnly)
            {
                return new List<TraceEventPM> { traceEventQuery.GetLatestEntityStatusTraceEvent(this.Tenant, shipment.Id, shipmentObjectTable.Id) };
            }
            else
                return traceEventQuery.GetEntityStatusTraceEvents(this.Tenant, shipment.Id, shipmentObjectTable.Id).ToList();

        }
        private ShipmentStatuses BuildShipmentStatues(Shipment shipment, List<TraceEventPM> shipmentEvents)
        {
            var shipmentStatuses = new ShipmentStatuses();
            shipmentStatuses.HouseNumber = shipment.House;
            shipmentStatuses.Statuses = GetTraceEventStatuses(shipmentEvents);
            return shipmentStatuses;
        }

        private List<StatusDetails> GetTraceEventStatuses(List<TraceEventPM> shipmentEvents)
        {
            return shipmentEvents.Select(eventItem =>
             new StatusDetails()
             {
                 StatusCode = eventItem.EntityStatusCode,
                 StatusName = eventItem.EntityStatusName,
                 StatusDateTime = eventItem.EventDateTime,
                 StatusRemarks = eventItem.Notes
             }).ToList();
        }


    }
}
