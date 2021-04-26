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
            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentFilters);

            CargoTrackingShipmentsResponse response = new CargoTrackingShipmentsResponse
            {
                Shipments = shipmentSearchQuery.GetShipmentsByFilters(pageIndex, pageSize, shipmentFilters).ToList(),
                ShipmentsCount = GetAllShipmentsCount(pageIndex, shipmentFilters)
            };

            return response;
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
            counter.Import = shipmentsIQuerable.Count(d => d.DirectionId == "I");
            counter.Export = shipmentsIQuerable.Count(d => d.DirectionId == "E");

            return counter;
        }
        private int GetAllShipmentsCount(int pageIndex, CargoTrackingShipmentFilters shipmentFilters)
        {
            CargoTrackingShipmentSearchListQueryService shipmentSearchQuery = GetCargoTrackingShipmentSearchQuery(shipmentFilters);
            var count = 0;
            if (pageIndex == 0)
                count = shipmentSearchQuery.GetShipmentsCount(shipmentFilters);

            return count;
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
    }
}