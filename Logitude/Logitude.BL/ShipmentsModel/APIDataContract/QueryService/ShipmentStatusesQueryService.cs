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

            List<TraceEvent> shipmentEvents = GetShipmentStatusesEvents(shipment, latestStatusOnly);
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
        private List<TraceEvent> GetShipmentStatusesEvents(Shipment shipment, bool latestStatusOnly)
        {
            var objectTabelRepository = new ObjectTableRepository(0);
            var shipmentObjectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            var traceEventRepository = new TraceEventRepository(this.Tenant);

            if (latestStatusOnly)
            {
                return new List<TraceEvent> { traceEventRepository.GetLatestEntityStatusTraceEvent(this.Tenant, shipment.Id, shipmentObjectTable.Id) };
            }
            else
                return traceEventRepository.GetEntityStatusTraceEvents(this.Tenant, shipment.Id, shipmentObjectTable.Id).ToList();

        }
        private ShipmentStatuses BuildShipmentStatues(Shipment shipment, List<TraceEvent> shipmentEvents)
        {
            var shipmentStatuses = new ShipmentStatuses();
            shipmentStatuses.HouseNumber = shipment.House;
            shipmentStatuses.Statuses = GetTraceEventStatuses(shipmentEvents);
            return shipmentStatuses;
        }

        private List<StatusDetails> GetTraceEventStatuses(List<TraceEvent> shipmentEvents)
        {
            return shipmentEvents.Select(eventItem =>
             new StatusDetails()
             {
                 StatusCode = eventItem.EventType.EntityStatus.Code,
                 StatusName = eventItem.EventType.EntityStatus.Name,
                 StatusDateTime = eventItem.EventDateTime,
                 StatusRemarks = eventItem.Notes
             }).ToList();
        }


    }
}
