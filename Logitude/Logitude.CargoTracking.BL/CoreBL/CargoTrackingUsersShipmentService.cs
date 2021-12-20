using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Def.DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CargoTracking.BL.CoreBL
{
    public class CargoTrackingUsersShipmentService
    {
        public CargoTrackingShipmentsResponse GetUserShipmentsResponse(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            return new CargoTrackingShipmentsResponse
            {
                Shipments = GetUserShipments(pageIndex, pageSize, shipmentFilters),
                ShipmentsCount = GetAllShipmentsCountForFirstPageOnly(pageIndex, shipmentFilters)
            };
        }

        private List<CargoTrackingShipmentList> GetUserShipments(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentFilters);
            var shipments = shipmentSearchQuery.GetFilteredShipments(pageIndex, pageSize, shipmentFilters);
            CargoTrackingShipmentQueryService cargoTrackingShipmentQueryService = new CargoTrackingShipmentQueryService(shipmentFilters.Tenant);
            cargoTrackingShipmentQueryService.SetFutureMilstone(shipments);
            return shipments;
        }
        private int GetAllShipmentsCountForFirstPageOnly(int pageIndex, CargoTrackingShipmentFilters shipmentFilters)
        {
            var isNotFirstPage = pageIndex != 0;
            if (isNotFirstPage)
                return 0;

            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentFilters);
            return shipmentSearchQuery.GetShipmentsCount(shipmentFilters);
        }
        public CargoTrackingShipmentsCounter GetUserShipmentsCounter(CargoTrackingShipmentFilters shipmentFilters)
        {

            shipmentFilters.TransportModeCodes = "";
            shipmentFilters.DirectionCodes = "";
            //var customers = shipmentFilters.CustomersIdsString;
            //shipmentFilters.CustomersIdsString = "";

            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentFilters);
            IQueryable<CargoTrackingShipmentList> shipmentsIQuerable = shipmentSearchQuery.GetShipmentsByFilters(shipmentFilters);

            CargoTrackingShipmentsCounter counter = new CargoTrackingShipmentsCounter();
            counter.Air = shipmentsIQuerable.Count(d => d.TransportModeId == "A");
            counter.Land = shipmentsIQuerable.Count(d => d.TransportModeId == "I");
            counter.Sea = shipmentsIQuerable.Count(d => d.TransportModeId == "O");
            counter.Import = shipmentsIQuerable.Count(d => d.DirectionId == "I" || d.DirectionId == "C");
            counter.Export = shipmentsIQuerable.Count(d => d.DirectionId == "E");
            counter.Drop = shipmentsIQuerable.Count(d => d.DirectionId == "R");
            counter.HasException = shipmentsIQuerable.Count(d => d.CurrentMilestoneExceptions != null);
            counter.OrdersOnly = shipmentsIQuerable.Count(d => d.EntityType == "O");
            counter.EstimatedArrivalOnly = shipmentsIQuerable.Count(d => d.ArrivalEstimationDate != null && d.ArrivalDate == null);
            counter.OperationalClosedOnly = shipmentsIQuerable.Count(d => d.IsOperationalClosed == true);


            return counter;
        }

        private CargoTrackingShipmentSearchListQueryService GetCargoTrackingShipmentSearchQuery(CargoTrackingShipmentFilters shipmentFilters)
        {
            ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(shipmentFilters.Tenant);
            CargoTrackingShipmentSearchListQueryService cargoTrackingShipmentSearchQuery = new CargoTrackingShipmentSearchListQueryService(MyContext);
            return cargoTrackingShipmentSearchQuery;
        }
    }
    public class CargoTrackingShipmentsResponse
    {
        public List<CargoTrackingShipmentList> Shipments { get; set; }
        public int ShipmentsCount { get; set; }
    }
    public class CargoTrackingShipmentsCounter
    {
        public int Import { get; set; }
        public int Export { get; set; }
        public int Air { get; set; }
        public int Land { get; set; }
        public int Sea { get; set; }
        public int Drop { get; set; }
        public int HasException { get; set; }
        public int OrdersOnly { get; set; }
        public int EstimatedArrivalOnly { get; set; }
        public int OperationalClosedOnly { get; set; }

    }
}