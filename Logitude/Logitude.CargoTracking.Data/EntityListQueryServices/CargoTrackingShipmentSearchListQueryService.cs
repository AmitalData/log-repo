using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.Model;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Def.DataContracts;
using Logitude.BL.GlobalModel.EntityQueries;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{

    public partial class CargoTrackingShipmentSearchListQueryService
    {
        public const string OrderType = "O";
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
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = GetShipmentsSearchEntities(searchText, tenant);

            List<string> shipmentsIds = shipmentsSearchEntities.Select(d => d.ShipmentId).ToList();

            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsIds, tenant);
            AddSearchsToShipments(shipments, tenant);
            return shipments;
        }

        public void AddSearchsToShipments(List<CargoTrackingShipmentList> shipments, int tenant)
        {
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
            List<CargoTrackingShipmentSearch> shipmentSearchs = repo.GetConnectedShipmentNumbersByShipmentIds(shipments.Where(e => e.EntityType != OrderType).Select(e => e.EntityId).ToList());
            var searchesGroupDictionary = shipmentSearchs.GroupBy(e => e.ShipmentId).ToDictionary(e => e.Key, e => e);
            foreach (var item in shipments)
            {
                item.ConnectedShipmentsNumbers = GetSearchesFromGroupsDictionary(item, searchesGroupDictionary);
            }

        }

        private string GetSearchesFromGroupsDictionary(CargoTrackingShipmentList item, Dictionary<string, IGrouping<string, CargoTrackingShipmentSearch>> searchesGroupDictionary)
        {

            if (!searchesGroupDictionary.ContainsKey(item.EntityId))
                return null;
            var connectedShipmentsNumbers = searchesGroupDictionary[item.EntityId].ToList();
            var maxConnectedShipmentsNumbers = connectedShipmentsNumbers.GroupBy(e => e.ReferenceType).Select(e => e.Max(f => f.SearchFields)).ToList();
            return string.Join("\n", maxConnectedShipmentsNumbers);
        }

        private static IQueryable<CargoTrackingShipmentSearch> GetShipmentsSearchEntities(string searchText, int tenant)
        {
			TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
			bool isSearchbyAbsoluteValue = tenantManagementQuery.GetSearchAbsoluteValuePublicByTenant(tenant);

			Boolean isContainsSlashORDash = searchText.Contains('-') || searchText.Contains('/');
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntitiesThatMatchWhole = repo.GetShipmentSearchEntities(searchText, tenant);

            if (isSearchbyAbsoluteValue || !isContainsSlashORDash) { return shipmentsSearchEntitiesThatMatchWhole; }
            else
            {
                var partOfSearchText = searchText.Contains('-') ? searchText.Substring(searchText.IndexOf('-') + 1) : searchText.Substring(searchText.IndexOf('/') + 1);
                IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntitiesThatMatchSecondPart = repo.GetShipmentSearchEntities(partOfSearchText, tenant);
                return shipmentsSearchEntitiesThatMatchWhole.Union(shipmentsSearchEntitiesThatMatchSecondPart);
            }
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
            CargoTrackingShipmentSearch shipmentsSearchEntiy = repo.GetFirstShipmentSearchesForWarmCargoTracking();
            return shipmentsSearchEntiy;
        }
        public List<CargoTrackingShipmentList> GetShipmentsByFilters(int pageIndex, int pageSize, CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            List<CargoTrackingShipmentList> shipments = GetFilteredShipments(shipmentSearchInput);

            return shipments;
        }

        public IQueryable<CargoTrackingShipmentList> GetShipmentsByFilters(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetFilteredShipmentsBySearchText(shipmentSearchInput);

            return shipments;
        }

        public List<CargoTrackingShipmentList> GetFilteredShipmentsByIds(int pageIndex, int pageSize, CargoTrackingShipmentSearchInput shipmentSearchInput, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetCargoTrackingShipments(pageIndex, pageSize, shipmentsIds.ToList(), shipmentSearchInput);
            return shipments;
        }
        public List<CargoTrackingShipmentList> GetFilteredShipments(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetFilteredSortedShipments(shipmentSearchInput);
            return shipments;
        }
        public List<Customer> GetShipmentsCustomers(int tenant)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            return shipmentsQuery.GetShipmentsCustomers(tenant);
        }

        public IQueryable<CargoTrackingShipmentList> GetFilteredShipmentsByIds(CargoTrackingShipmentSearchInput shipmentSearchInput, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            IQueryable<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsIds.ToList(), shipmentSearchInput);
            return shipments;
        }
        private IQueryable<CargoTrackingShipmentList> GetFilteredShipmentsBySearchText(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            IQueryable<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentSearchInput);
            return shipments;
        }

        public int GetShipmentsCount(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            //List<string> shipmentsIds = GetTenantShipmentsIdsBySearchKey(shipmentSearchInput.SearchText, shipmentSearchInput.Tenant);

            //bool hasSearchKeyWithNoResults = !string.IsNullOrWhiteSpace(shipmentSearchInput.SearchText) && shipmentsIds.Count == 0;
            //if (hasSearchKeyWithNoResults)
            //    return 0;

            return GetFilteredShipmentsCount(shipmentSearchInput);
        }

        private int GetFilteredShipmentsCount(CargoTrackingShipmentSearchInput shipmentSearchInput, List<string> shipmentsIds)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            int shipmentsCount = shipmentsQuery.GetShipmentsCount(shipmentsIds.ToList(), shipmentSearchInput);
            return shipmentsCount;
        }
        private int GetFilteredShipmentsCount(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            int shipmentsCount = shipmentsQuery.GetShipmentsCount(shipmentSearchInput);
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