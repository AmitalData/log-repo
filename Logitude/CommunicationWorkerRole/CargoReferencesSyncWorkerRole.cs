using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Data;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System.Data.Entity;
using Logitude.CargoTracking.BL.CargoTrackingServices;
using System.Data;
using System.Data.SqlClient;

namespace CommunicationWorkerRole
{
    public class CargoReferencesSyncWorkerRole : WorkerEntryPoint
    {
        const string ShipmentSearchesTableName = "CargoTrackingShipmentSearches";
        const string OrderPreFix = "Order ";
        const string ForwardingPreFix = "Forwarding ";
        private ICargoTrackingContext cargoContext;
        const int MaximumNumberOfConcurrentConnections = 12;
        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = MaximumNumberOfConcurrentConnections;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoReferencesSync";
            cargoContext = CargoTrackingContext.GetContext(0);
            return base.OnStart();
        }
        public override void Run()
        {
            while (IsRunning)
            {
                Do();
            }
        }

        private void Do()
        {
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
                return;
            }
            try
            {
                SyncConnectedShepments();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Thread.Sleep(10000);
            }

        }

        private void SyncConnectedShepments()
        {
            SyncOrderReferencesToForwarding();
            SyncForwardingReferencesToCustome();
        }

        private void SyncOrderReferencesToForwarding()
        {
            var orderShipmentQueue = GetTop_100_OrderReferencesQueue();
            if (orderShipmentQueue.Count <= 0)
                return;
            orderShipmentQueue.AddRange(GetSameShipmants(orderShipmentQueue));
            var orderSeatches = GetSearchesByShipmentIds(GetShipmentsIds(orderShipmentQueue));
            var newForwardingSearches = GetNewSearches(orderShipmentQueue, orderSeatches, Codes.OrderType);
            DeleteOldSearches(newForwardingSearches);
            AddSearchesByBulk(newForwardingSearches);
            SyncCustomSearches(orderShipmentQueue);
            RemoveQueueRecords(orderShipmentQueue);
        }

        private List<CargoReferencesSyncQueue> GetSameShipmants(List<CargoReferencesSyncQueue> shipmentQueue)
        {
            var list = new List<CargoReferencesSyncQueue>();
            foreach (var item in shipmentQueue)
            {
                list.Add(new CargoReferencesSyncQueue()
                {
                    ShipmentId = item.SyncTo,
                    SyncTo = item.SyncTo
                });
            }
            return list;
        }

        private List<CargoReferencesSyncQueue> GetTop_100_OrderReferencesQueue()
        {
            return cargoContext.CargoReferencesSyncQueues.Take(100).Where(e => e.ShipmentType == Codes.OrderType).ToList();
        }
        private List<CargoTrackingShipmentSearch> GetSearchesByShipmentIds(List<string> ShipmentIds)
        {
            return cargoContext.CargoTrackingShipmentSearches.AsNoTracking().Where(e => ShipmentIds.Contains(e.ShipmentId)).ToList();
        }
        private List<CargoTrackingShipmentSearch> GetNewSearches(List<CargoReferencesSyncQueue> shipmentQueue, List<CargoTrackingShipmentSearch> seatches, string sourceType)
        {
            var shipmentQueueDictionary = shipmentQueue.ToDictionary(e => e.ShipmentId, e => e);
            var searchesGroupByShipmentId = seatches.GroupBy(e => e.ShipmentId);
            var searchesGroupByShipmentIdDictionary = searchesGroupByShipmentId.ToDictionary(e => e.Key, e => e);
            var results = new List<CargoTrackingShipmentSearch>();
            foreach (var group in searchesGroupByShipmentId)
            {
                results.AddRange(CreateSearchesFromGroup(shipmentQueueDictionary, searchesGroupByShipmentIdDictionary, group, sourceType));
            }
            return results;
        }
        private void AddSearchesByBulk(List<CargoTrackingShipmentSearch> newForwardingSearches)
        {
            var cargoTrackingShipmentSearchDataTable = CreateCargoTrackingShipmentSearchDataTable();
            FillCargoTrackingShipmentSearchDataTable(cargoTrackingShipmentSearchDataTable, newForwardingSearches);
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cargoContext.GetConnection().ConnectionString))
            {
                bulkCopy.DestinationTableName = ShipmentSearchesTableName;
                bulkCopy.WriteToServer(cargoTrackingShipmentSearchDataTable);
            }
        }
        private void FillCargoTrackingShipmentSearchDataTable(DataTable cargoTrackingShipmentSearchDataTable, List<CargoTrackingShipmentSearch> newForwardingSearches)
        {
            foreach (var item in newForwardingSearches)
            {
                cargoTrackingShipmentSearchDataTable.Rows.Add(CreateDataRow(cargoTrackingShipmentSearchDataTable, item));
            }
        }
        private void SyncCustomSearches(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var customShipmentQueue = GetCustomIdsFromForwording(orderShipmentQueue);
            if (customShipmentQueue.Count <= 0)
                return;
            customShipmentQueue.AddRange(GetSameShipmants(customShipmentQueue));
            var customSeatches = GetSearchesByShipmentIds(GetShipmentsIds(customShipmentQueue));
            var newCustomSearches = GetNewSearches(customShipmentQueue, customSeatches, Codes.ForwardingType);
            DeleteOldSearches(newCustomSearches);
            AddSearchesByBulk(newCustomSearches);
        }

        private List<string> GetShipmentsIds(List<CargoReferencesSyncQueue> shipmentQueue)
        {
            var ids = shipmentQueue.Select(e => e.ShipmentId).ToList();
            return ids;
        }

        private List<CargoReferencesSyncQueue> GetCustomIdsFromForwording(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var ForwordingIds = orderShipmentQueue.Select(e => e.SyncTo).ToList();
            var CustomShipmentQueueQuery = cargoContext.CargoTrackingShipments.Where(e => ForwordingIds.Contains(e.EntityId) && e.CustomsShipmentHeaderId != null);
            var CustomShipmentQueue = (from e in CustomShipmentQueueQuery
                                       select
    new
    {
        ShipmentId = e.EntityId,
        SyncTo = e.CustomsShipmentHeaderId,
        Tenant = e.Tenant
    }).ToList();

            return CustomShipmentQueue.Select(e => new CargoReferencesSyncQueue()
            {
                ShipmentId = e.ShipmentId,
                SyncTo = e.SyncTo,
                Tenant = e.Tenant
            }).ToList();


        }

        private void SyncForwardingReferencesToCustome()
        {
            var forwardingShipmentQueue = GetTop_100_ForwardingReferencesQueue();
            if (forwardingShipmentQueue.Count <= 0)
                return;
            forwardingShipmentQueue.AddRange(GetSameShipmants(forwardingShipmentQueue));
            var forwardingSeatches = GetSearchesByShipmentIds(GetShipmentsIds(forwardingShipmentQueue));
            var newCustomeSearches = GetNewSearches(forwardingShipmentQueue, forwardingSeatches, Codes.ForwardingType);
            DeleteOldSearches(newCustomeSearches);
            AddSearchesByBulk(newCustomeSearches);
            RemoveQueueRecords(forwardingShipmentQueue);

        }



        private DataRow CreateDataRow(DataTable cargoTrackingShipmentSearchDataTable, CargoTrackingShipmentSearch item)
        {
            var row = cargoTrackingShipmentSearchDataTable.NewRow();
            row["Tenant"] = item.Tenant;
            row["SearchFields"] = item.SearchFields;
            row["ShipmentDate"] = item.ShipmentDate;
            row["Id"] = DBNull.Value;
            row["ShipmentId"] = item.ShipmentId;
            row["IsPublic"] = item.IsPublic;
            row["ReferenceType"] = item.ReferenceType;
            return row;
        }

        private DataTable CreateCargoTrackingShipmentSearchDataTable()
        {
            var datatable = new DataTable();
            datatable.Columns.Add("Tenant", typeof(int));
            datatable.Columns.Add("SearchFields", typeof(string));
            datatable.Columns.Add("ShipmentDate", typeof(DateTime));
            datatable.Columns.Add("Id", typeof(int));
            datatable.Columns.Add("ShipmentId", typeof(string));
            datatable.Columns.Add("IsPublic", typeof(bool));
            datatable.Columns.Add("ReferenceType", typeof(string));
            return datatable;
        }


        private List<CargoTrackingShipmentSearch> CreateSearchesFromGroup(Dictionary<string, CargoReferencesSyncQueue> shipmentQueueDictionary, Dictionary<string, IGrouping<string, CargoTrackingShipmentSearch>> searchesGroupByShipmentIdDictionary, IGrouping<string, CargoTrackingShipmentSearch> group, string sourceType)
        {
            var results = new List<CargoTrackingShipmentSearch>();
            var shipmentQueueItem = shipmentQueueDictionary[group.Key];
            results.AddRange(CreateSearches(shipmentQueueItem, group, sourceType));
            return results;
        }

        private List<CargoTrackingShipmentSearch> CreateSearches(CargoReferencesSyncQueue shipmentQueueItem, IGrouping<string, CargoTrackingShipmentSearch> group, string sourceType = null)
        {
            var results = new List<CargoTrackingShipmentSearch>();
            var searches = group.ToList();
            if (shipmentQueueItem.ShipmentId == shipmentQueueItem.SyncTo)
                searches = searches.Where(e => !e.ReferenceType.StartsWith(ForwardingPreFix) && !e.ReferenceType.StartsWith(OrderPreFix)).ToList();
            foreach (var item in searches)
            {
                results.Add(ConvertToNewSearches(item, shipmentQueueItem, sourceType));
            }
            return results;
        }

        private CargoTrackingShipmentSearch ConvertToNewSearches(CargoTrackingShipmentSearch item, CargoReferencesSyncQueue shipmentQueueItem, string sourceType)
        {
            item.Id = 0;
            item.ShipmentId = shipmentQueueItem.SyncTo;
            item.ReferenceType = GetNewReferenceType(shipmentQueueItem, item.ReferenceType, sourceType);
            return item;
        }

        private string GetNewReferenceType(CargoReferencesSyncQueue shipmentQueueItem, string referenceType, string sourceType)
        {
            if (sourceType == null || shipmentQueueItem.SyncTo == shipmentQueueItem.ShipmentId)
                return referenceType;

            if (referenceType.StartsWith(ForwardingPreFix))
                return referenceType;
            if (referenceType.StartsWith(OrderPreFix))
                return referenceType;

            switch (sourceType)
            {
                case Codes.OrderType:
                    return OrderPreFix + referenceType;
                case Codes.ForwardingType:
                    return ForwardingPreFix + referenceType;
            }
            return referenceType;
        }

        private List<CargoReferencesSyncQueue> GetTop_100_ForwardingReferencesQueue()
        {
            return cargoContext.CargoReferencesSyncQueues.Take(100).Where(e => e.ShipmentType == Codes.ForwardingType).ToList();
        }


        private void RemoveQueueRecords(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var ids = GetIdsAsString(orderShipmentQueue.Select(e => e.Id + "").ToList());
            var query = $"delete from CargoReferencesSyncQueues where id in ({ids}) ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        private void DeleteOldSearches(List<CargoTrackingShipmentSearch> searches)
        {
            var ids = GetIdsAsString(searches.Select(e => e.ShipmentId).ToList());
            var query = $"delete from CargoTrackingShipmentSearches where ShipmentId in ({ids}) ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }

        private string GetIdsAsString(List<string> Ids)
        {
            var ids = "";
            foreach (var item in Ids)
            {
                ids += $"'{item}',";
            }
            if (Ids.Count > 0)
                ids = ids.Substring(0, ids.Length - 1);
            return ids;

        }

    }
}
