	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Def.DataContracts;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{

    public partial class CargoTrackingShipmentSearchListQueryService
    {
        private IQueryable<CargoTrackingShipmentSearchList> GetIqueryableList(IQueryable<CargoTrackingShipmentSearch> iQueryable)
        {
            IQueryable<CargoTrackingShipmentSearchList> query = (from a in iQueryable
                                                                 select new CargoTrackingShipmentSearchList()
                                                                 {

                                                                     Id = a.Id,

                                                                     Tenant = a.Tenant,

                                                                     SearchFields = a.SearchFields,

                                                                     ShipmentId = a.ShipmentId,

                                                                     ShipmentDate = a.ShipmentDate,

                                                                 });
            return query;
        }

        private IQueryable<CargoTrackingShipmentSearch> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CargoTrackingShipmentSearch> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<CargoTrackingShipmentSearch> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CargoTrackingShipmentSearch> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<CargoTrackingShipmentList> GetShipments(string searchText, int tenant)
        {
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = repo.GetShipmentSearchEntities(searchText, tenant);

            List<string> shipmentsIds = shipmentsSearchEntities.Select(d => d.ShipmentId).ToList();

            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsIds, tenant);

          
            return shipments;
        }
        public List<CargoTrackingShipmentList> GetTop500Shipments(int pageIndex, int pageSize, int tenant)
        {
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = repo.GetShipmentSearchs(tenant);

            List<string> shipmentsIds = shipmentsSearchEntities.Select(d => d.ShipmentId).Distinct().ToList();

            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsIds.Skip(pageIndex * pageSize).Take(pageSize).ToList(), tenant);


            return shipments;
        }
        public List<CargoTrackingShipmentList> GetShipments(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            List<string> shipmentsIds = GetTenantShipmentsIdsBySearchKey(shipmentFilters.SearchText, shipmentFilters.Tenant);

            List<CargoTrackingShipmentList> shipments = GetFilteredShipments(shipmentFilters, shipmentsIds);

            return shipments.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        private List<CargoTrackingShipmentList> GetFilteredShipments(CargoTrackingShipmentFilters shipmentFilters, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsIds.ToList(), shipmentFilters);
            return shipments;
        }

        public int GetShipmentsCount(CargoTrackingShipmentFilters shipmentFilters)
        {
            List<string> shipmentsIds = GetTenantShipmentsIdsBySearchKey(shipmentFilters.SearchText, shipmentFilters.Tenant);
            int shipmentsCount = GetFilteredShipmentsCount(shipmentFilters, shipmentsIds);

            return shipmentsCount;
        }

        private int GetFilteredShipmentsCount(CargoTrackingShipmentFilters shipmentFilters, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            int shipmentsCount = shipmentsQuery.GetShipmentsCount(shipmentsIds.ToList(), shipmentFilters);
            return shipmentsCount;
        }

        private static List<string> GetTenantShipmentsIdsBySearchKey(string searchKey, int tenant)
        {
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = repo.GetShipmentSearchs(searchKey, tenant);

            List<string> shipmentsIds = shipmentsSearchEntities.Select(d => d.ShipmentId).Distinct().ToList();
            return shipmentsIds;
        }
    }


}
	