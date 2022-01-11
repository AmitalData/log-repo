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
        private ICargoTrackingContext cargoContext;
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
            var orderSeatches = GetSearchesByShipmentIds(orderShipmentQueue.Select(e => e.ShipmentId).ToList());
            var newForwardingSearches = GetNewSearches(orderShipmentQueue, orderSeatches);
            AddSearchesByBulk(newForwardingSearches);
            AddCustomSearches(orderShipmentQueue);
            RemoveQueueRecords(orderShipmentQueue);
        }

        private void AddCustomSearches(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var customShipmentQueue = GetCustomIdsFromForwording(orderShipmentQueue);
            if (customShipmentQueue.Count <= 0)
                return;
            var customSeatches = GetSearchesByShipmentIds(customShipmentQueue.Select(e => e.ShipmentId).ToList());
            var newCustomSearches = GetNewSearches(customShipmentQueue, customSeatches);
            AddSearchesByBulk(newCustomSearches);
        }

        private List<CargoReferencesSyncQueue> GetCustomIdsFromForwording(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var ForwordingIds = orderShipmentQueue.Select(e => e.SyncTo).ToList();
            var CustomShipmentQueueQuery = cargoContext.CargoTrackingShipments.Where(e => ForwordingIds.Contains(e.EntityId) && e.CustomsShipmentHeaderId != null);
            var CustomShipmentQueue = (from e in CustomShipmentQueueQuery select 
                                       new {
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
            var forwardingSeatches = GetSearchesByShipmentIds(forwardingShipmentQueue.Select(e => e.ShipmentId).ToList());
            var newCustomeSearches = GetNewSearches(forwardingShipmentQueue, forwardingSeatches);
            AddSearchesByBulk(newCustomeSearches);
            RemoveQueueRecords(forwardingShipmentQueue);

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

        private List<CargoTrackingShipmentSearch> GetNewSearches(List<CargoReferencesSyncQueue> shipmentQueue, List<CargoTrackingShipmentSearch> seatches)
        {
            var shipmentQueueDictionary = shipmentQueue.ToDictionary(e => e.ShipmentId, e => e);
            var searchesGroupByShipmentId = seatches.GroupBy(e => e.ShipmentId);
            var results = new List<CargoTrackingShipmentSearch>();
            foreach (var group in searchesGroupByShipmentId)
            {
                results.AddRange(CreateSearches(shipmentQueueDictionary, group));
            }
            return results;
        }

        private List<CargoTrackingShipmentSearch> CreateSearches(Dictionary<string, CargoReferencesSyncQueue> orderShipmentQueueDictionary, IGrouping<string, CargoTrackingShipmentSearch> group)
        {
            var shipmentQueueItem = orderShipmentQueueDictionary[group.Key];
            var results = new List<CargoTrackingShipmentSearch>();
            foreach (var item in group)
            {
                item.Id = 0;
                item.ShipmentId = shipmentQueueItem.SyncTo;
                results.Add(item);
            }
            return results;
        }

        private List<CargoReferencesSyncQueue> GetTop_100_OrderReferencesQueue()
        {
            return cargoContext.CargoReferencesSyncQueues.Take(100).Where(e => e.ShipmentType == Codes.OrderType).ToList();
        }
        private List<CargoReferencesSyncQueue> GetTop_100_ForwardingReferencesQueue()
        {
            return cargoContext.CargoReferencesSyncQueues.Take(100).Where(e => e.ShipmentType == Codes.ForwardingType).ToList();
        }
        private List<CargoTrackingShipmentSearch> GetSearchesByShipmentIds(List<string> ShipmentIds)
        {
            return cargoContext.CargoTrackingShipmentSearches.AsNoTracking().Where(e => ShipmentIds.Contains(e.ShipmentId)).ToList();
        }

        private void RemoveQueueRecords(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var ids = GetIdsAsString(orderShipmentQueue.Select(e => e.Id).ToList());
            var query = $"delete from CargoReferencesSyncQueues where id in ({ids}) ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        public override bool OnStart()
        {


            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoReferencesSync";
            cargoContext = CargoTrackingContext.GetContext(0);
            return base.OnStart();
        }
        private string GetIdsAsString(List<int> Ids)
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
