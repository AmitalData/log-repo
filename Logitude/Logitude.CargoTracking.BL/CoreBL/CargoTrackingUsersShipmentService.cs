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
        public CargoTrackingShipmentsResponse GetUserShipmentsResponse( CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            return new CargoTrackingShipmentsResponse
            {
                Shipments = GetUserShipments( shipmentSearchInput),
                ShipmentsCount = GetAllShipmentsCountForFirstPageOnly(shipmentSearchInput)
            };
        }

        private List<CargoTrackingShipmentList> GetUserShipments( CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentSearchInput);
            var shipments = shipmentSearchQuery.GetFilteredShipments( shipmentSearchInput);
            CargoTrackingShipmentQueryService cargoTrackingShipmentQueryService = new CargoTrackingShipmentQueryService(shipmentSearchInput.Tenant);
            cargoTrackingShipmentQueryService.SetFutureMilstone(shipments);
            return shipments;
        }
        private int GetAllShipmentsCountForFirstPageOnly( CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            var isNotFirstPage = shipmentSearchInput.PageIndex != 0;
            if (isNotFirstPage)
                return 0;

            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentSearchInput);
            return shipmentSearchQuery.GetShipmentsCount(shipmentSearchInput);
        }
        public CargoTrackingShipmentsCounter GetUserShipmentsCounter(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {

            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentSearchInput);
            IQueryable<CargoTrackingShipmentList> shipmentsIQuerable = shipmentSearchQuery.GetShipmentsByFilters(shipmentSearchInput);

            CargoTrackingShipmentsCounter counter = new CargoTrackingShipmentsCounter();
            counter.Air = shipmentsIQuerable.Count(d => d.TransportModeId == "A");
            counter.Land = shipmentsIQuerable.Count(d => d.TransportModeId == "I");
            counter.Sea = shipmentsIQuerable.Count(d => d.TransportModeId == "O");
            counter.Import = shipmentsIQuerable.Count(d => d.DirectionId == "I");
            counter.Export = shipmentsIQuerable.Count(d => d.DirectionId == "E");
            counter.Drop = shipmentsIQuerable.Count(d => d.DirectionId == "R");
            counter.Domestic = shipmentsIQuerable.Count(d => d.DirectionId == "D");
            counter.CustomsImport = shipmentsIQuerable.Count(d => d.DirectionId == "C");
            counter.HasException = shipmentsIQuerable.Count(d => d.CurrentMilestoneExceptions != null);
            counter.OrdersOnly = shipmentsIQuerable.Count(d => d.EntityType == "O");
            counter.EstimatedArrivalOnly = shipmentsIQuerable.Count(d => d.ArrivalEstimationDate != null && d.ArrivalDate == null);
            counter.OperationalClosedOnly = shipmentsIQuerable.Count(d => d.IsOperationalClosed == true);


            return counter;
        }

        private CargoTrackingShipmentSearchListQueryService GetCargoTrackingShipmentSearchQuery(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(shipmentSearchInput.Tenant);
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
        public int Domestic { get; set; }
        public int CustomsImport { get; set; }
        public int HasException { get; set; }
        public int OrdersOnly { get; set; }
        public int EstimatedArrivalOnly { get; set; }
        public int OperationalClosedOnly { get; set; }

    }
}