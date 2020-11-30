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

            CargoTrackingShipmentsResponse response = new CargoTrackingShipmentsResponse();
            response.Shipments = shipmentSearchQuery.GetShipmentsByFilters(pageIndex, pageSize, shipmentFilters).ToList();
            response.ShipmentsCount = GetAllShipmentsCount(pageIndex, shipmentFilters);

            return response;
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
}