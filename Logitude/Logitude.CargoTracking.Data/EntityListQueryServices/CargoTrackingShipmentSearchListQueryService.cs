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


        public CargoTrackingShipmentSearch GetFirstShipmentSearchesForWarmCargoTracking()
        {
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(0);
            CargoTrackingShipmentSearch  shipmentsSearchEntiy= repo.GetFirstShipmentSearchesForWarmCargoTracking();
            return shipmentsSearchEntiy;
        }
        public List<CargoTrackingShipmentList> GetShipmentsByFilters(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            List<CargoTrackingShipmentList> shipments = GetFilteredShipments(pageIndex, pageSize, shipmentFilters);

            return shipments;
        }

        public IQueryable<CargoTrackingShipmentList> GetShipmentsByFilters(CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetFilteredShipmentsBySearchText(shipmentFilters);

            return shipments;
        }

        private void SortShipments(CargoTrackingShipmentFilters shipmentFilters, List<CargoTrackingShipmentList> shipments)
        {
            if (shipmentFilters.SortDescending)
                shipments.OrderByDescending(d => d.CreateDate);
            else
                shipments.OrderBy(d => d.CreateDate);

        }

        public List<CargoTrackingShipmentList> GetFilteredShipmentsByIds(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetCargoTrackingShipments(pageIndex, pageSize,shipmentsIds.ToList(), shipmentFilters);
            return shipments;
        }
        public List<CargoTrackingShipmentList> GetFilteredShipments(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetFilteredShipmentsWithMilstones(pageIndex, pageSize, shipmentFilters);
            return shipments;
        }
        public IQueryable<CargoTrackingShipmentList> GetFilteredShipmentsByIds(CargoTrackingShipmentFilters shipmentFilters, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            IQueryable<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsIds.ToList(), shipmentFilters);
            return shipments;
        }
        private IQueryable<CargoTrackingShipmentList> GetFilteredShipmentsBySearchText(CargoTrackingShipmentFilters shipmentFilters)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            IQueryable<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentFilters);
            return shipments;
        }

        public int GetShipmentsCount(CargoTrackingShipmentFilters shipmentFilters)
        {
            //List<string> shipmentsIds = GetTenantShipmentsIdsBySearchKey(shipmentFilters.SearchText, shipmentFilters.Tenant);

            //bool hasSearchKeyWithNoResults = !string.IsNullOrWhiteSpace(shipmentFilters.SearchText) && shipmentsIds.Count == 0;
            //if (hasSearchKeyWithNoResults)
            //    return 0;

            return GetFilteredShipmentsCount(shipmentFilters);
        }

        private int GetFilteredShipmentsCount(CargoTrackingShipmentFilters shipmentFilters, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            int shipmentsCount = shipmentsQuery.GetShipmentsCount(shipmentsIds.ToList(), shipmentFilters);
            return shipmentsCount;
        }
        private int GetFilteredShipmentsCount(CargoTrackingShipmentFilters shipmentFilters)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            int shipmentsCount = shipmentsQuery.GetShipmentsCount(shipmentFilters);
            return shipmentsCount;
        }
        private static List<string> GetTenantShipmentsIdsBySearchKey(string searchKey, int tenant)
        {
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
                IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = repo.GetShipmentSearchs(searchKey, tenant);
                IQueryable<string> lastQuery = shipmentsSearchEntities.Select(d => d.ShipmentId).Distinct();
                return lastQuery.ToList();
            }

            return new List<string>();
        }
    }


}