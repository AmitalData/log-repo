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
using Simplog.Data.ShipmentsModel;

namespace CommunicationWorkerRole
{
    /// <summary>
    /// this worker role to sync the CargoTrackingShipmentSearches and customerReferences when connect the shipments
    /// In incremental process > any shipment that contains the CustomfileId or any order contains the shipmentId will add to queue 
    /// this worker role get all records from Queue and get all searches from cargo database for all shipments in queue
    /// then update customerReferences and add the shipment searches to the connected shipment in cargo 
    /// Note > we remove the old searches and add it again with connected searches
    /// </summary>
    public class CargoReferencesSyncWorkerRole : WorkerEntryPoint
    {
        const string ShipmentSearchesTableName = "CargoTrackingShipmentSearches";
        const string OrderPreFix = "Order ";
        const string ForwardingPreFix = "Forwarding ";
        const int MaxShepmentsNumberPerTime = 100;
        private ICargoTrackingContext cargoContext;
        const int MaximumNumberOfConcurrentConnections = 12;
        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = MaximumNumberOfConcurrentConnections;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoReferencesSync";
            cargoContext = CargoTrackingContext.GetContext((int)Tenant);
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
            var orderShipmentQueue = GetOrderReferencesQueue();
            if (orderShipmentQueue.Count <= 0)
                return;
            var forwardingIds = GetForwardingShipmentsIds(orderShipmentQueue);
            orderShipmentQueue.AddRange(GetSameShipmants(orderShipmentQueue));
            var shipmentsIds = GetShipmentsIds(orderShipmentQueue);
            UpdateForwardingCustomerReference(forwardingIds);
            var orderSeatches = GetSearchesByShipmentIds(shipmentsIds);
            var newForwardingSearches = GetNewSearches(orderShipmentQueue, orderSeatches, Codes.OrderType);
            UpdateSearches(newForwardingSearches);
            SyncCustomSearches(orderShipmentQueue);
            RemoveQueueRecords(orderShipmentQueue);
        }

        private List<string> GetForwardingShipmentsIds(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var ids = orderShipmentQueue.Select(e => e.SyncTo).ToList();
            return ids;
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

        private List<CargoReferencesSyncQueue> GetOrderReferencesQueue()
        {
            return cargoContext.CargoReferencesSyncQueues.Take(MaxShepmentsNumberPerTime).Where(e => e.ShipmentType == Codes.OrderType).ToList();
        }
        private List<CargoTrackingShipmentSearch> GetSearchesByShipmentIds(List<string> ShipmentIds)
        {
            return cargoContext.CargoTrackingShipmentSearches.AsNoTracking().Where(e => ShipmentIds.Contains(e.ShipmentId)).ToList();
        }
        private List<CargoTrackingShipmentSearch> GetNewSearches(List<CargoReferencesSyncQueue> shipmentQueue, List<CargoTrackingShipmentSearch> seatches, string sourceType)
        {
            var shipmentQueueDictionary = GetShipmentQueueDictionary(shipmentQueue);
            var searchesGroupByShipmentId = seatches.GroupBy(e => e.ShipmentId);
            var searchesGroupByShipmentIdDictionary = searchesGroupByShipmentId.ToDictionary(e => e.Key, e => e);
            var results = new List<CargoTrackingShipmentSearch>();
            foreach (var group in searchesGroupByShipmentId)
            {
                results.AddRange(CreateSearchesFromGroup(shipmentQueueDictionary, searchesGroupByShipmentIdDictionary, group, sourceType));
            }
            return results;
        }
        private void AddSearchesByBulk(SqlConnection sqlConnection, SqlTransaction transaction, List<CargoTrackingShipmentSearch> newForwardingSearches)
        {
            var cargoTrackingShipmentSearchDataTable = CreateCargoTrackingShipmentSearchDataTable();
            FillCargoTrackingShipmentSearchDataTable(cargoTrackingShipmentSearchDataTable, newForwardingSearches);
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, transaction))
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
            var shipmentsIds = GetShipmentsIds(customShipmentQueue.Where(e=>e.ShipmentId == e.SyncTo).ToList());
            UpdateCustomCustomerReference(shipmentsIds);
            var customSeatches = GetSearchesByShipmentIds(GetShipmentsIds(customShipmentQueue));
            var newCustomSearches = GetNewSearches(customShipmentQueue, customSeatches, Codes.ForwardingType);
            UpdateSearches(newCustomSearches);

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
            var forwardingShipmentQueue = GetForwardingReferencesQueue();
            if (forwardingShipmentQueue.Count <= 0)
                return;
            var forwardingIds = GetForwardingShipmentsIds(forwardingShipmentQueue);
            UpdateCustomCustomerReference(forwardingIds);
            forwardingShipmentQueue.AddRange(GetSameShipmants(forwardingShipmentQueue));
            var forwardingSeatches = GetSearchesByShipmentIds(GetShipmentsIds(forwardingShipmentQueue));
            var newCustomeSearches = GetNewSearches(forwardingShipmentQueue, forwardingSeatches, Codes.ForwardingType);
            UpdateSearches(newCustomeSearches);
            RemoveQueueRecords(forwardingShipmentQueue);

        }

        private void UpdateSearches(List<CargoTrackingShipmentSearch> newCustomeSearches)
        {

            using (SqlConnection sqlConnection = new SqlConnection(cargoContext.GetConnection().ConnectionString))
            {
                sqlConnection.Open();
                var transaction = sqlConnection.BeginTransaction();
                try
                {
                    DeleteOldSearches(sqlConnection, transaction, newCustomeSearches);
                    AddSearchesByBulk(sqlConnection, transaction, newCustomeSearches);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

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
            row["ReferenceFromShipmentId"] = item.ReferenceFromShipmentId;
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
            datatable.Columns.Add("ReferenceFromShipmentId", typeof(string));
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
            item.ReferenceFromShipmentId = GetReferenceFromShipmentId(item.ReferenceFromShipmentId, shipmentQueueItem);
            return item;
        }

        private string GetReferenceFromShipmentId(string referenceFromShipmentId, CargoReferencesSyncQueue shipmentQueueItem)
        {
            if (shipmentQueueItem.SyncTo == shipmentQueueItem.ShipmentId)
                return null;
            if (string.IsNullOrEmpty(referenceFromShipmentId))
                return shipmentQueueItem.ShipmentId;
            return referenceFromShipmentId;
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

        private List<CargoReferencesSyncQueue> GetForwardingReferencesQueue()
        {
            return cargoContext.CargoReferencesSyncQueues.Take(MaxShepmentsNumberPerTime).Where(e => e.ShipmentType == Codes.ForwardingType).ToList();
        }
        private void UpdateForwardingCustomerReference(List<string> shipmentsIds)
        {
            var ids = GetIdsAsString(shipmentsIds);
            var customerReferenceSyncModels = GetForwardingCustomerReferenceSyncModels(ids);
            var customerReferences = CreateCustomerReferences(customerReferenceSyncModels);
            SyncCustomerReference(customerReferences, ids);
        }
        private void UpdateCustomCustomerReference(List<string> shipmentsIds)
        {
            var ids = GetIdsAsString(shipmentsIds);
            var customerReferenceSyncModels = GetCustomCustomerReferenceSyncModels(ids);
            var customerReferences = CreateCustomerReferences(customerReferenceSyncModels);
            SyncCustomerReference(customerReferences, ids);
        }

        private void SyncCustomerReference(List<CustomerReferenceModel> customerReferences, string ids)
        {
            var updateCustomerReferenceCases = CreateCustomerReferenceUpdateCases(customerReferences);
            var updateOrderHouseCases = CreateHouseUpdateCases(customerReferences.Where(e=>!string.IsNullOrEmpty(e.OrderHouse)).ToDictionary(e=>e.Id,e=>e.OrderHouse));
            var updateForwardingHouseCases = CreateHouseUpdateCases(customerReferences.Where(e => !string.IsNullOrEmpty(e.ForwardingHouse)).ToDictionary(e => e.Id, e => e.ForwardingHouse));
            var query = $@"
                            UPDATE CargoTrackingShipments 
                            SET CustomerReference = 
                                {updateCustomerReferenceCases}
                                        ,
                                SHOHouse = 
                                {updateOrderHouseCases}
                                         ,
                                ForwardingHouse = 
                                {updateForwardingHouseCases}
                                        
                            WHERE EntityId IN({ids});
                            ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);

        }

        private string CreateCustomerReferenceUpdateCases(List<CustomerReferenceModel> customerReferences)
        {
            var updateCases = "(CASE EntityId ";
            foreach (var item in customerReferences)
            {
                updateCases += $" WHEN '{item.Id}' THEN '{item.CustomerReference}' ";
            }
            updateCases += " END)";
            return updateCases ;
        }
        private string CreateHouseUpdateCases(Dictionary<string,string> houses)
        {
            if(houses.Count <= 0)
            {
                return "NULL";
            }
            var updateCases = "(CASE EntityId ";
            foreach (var item in houses)
            {
                updateCases += $" WHEN '{item.Key}' THEN '{item.Value}' ";
            }
            updateCases += " END)";
            return updateCases;
        }
       

        private List<CustomerReferenceModel> CreateCustomerReferences(List<CustomerReferenceSyncModel> customerReferenceSyncModels)
        {
            var customerReferences = new List<CustomerReferenceModel>();
            foreach (var item in customerReferenceSyncModels)
            {
                customerReferences.Add(CreateCustomerReference(item));
            }
            return customerReferences;
        }

        private CustomerReferenceModel CreateCustomerReference(CustomerReferenceSyncModel item)
        {
            var customerReferenceModel = new CustomerReferenceModel();

            customerReferenceModel.CustomerReference = CraeteCustomerReferenceAsString(item);
            customerReferenceModel.Id = item.Id;
            customerReferenceModel.OrderHouse = item.OrderHouse;
            customerReferenceModel.ForwardingHouse = item.ForwardingHouse;
            return customerReferenceModel;
        }

        private string CraeteCustomerReferenceAsString(CustomerReferenceSyncModel item)
        {
            var customerReferences = new List<string>();
            if (!string.IsNullOrEmpty(item.CustomerReference1))
                customerReferences.AddRange(item.CustomerReference1.Split(','));
            if (!string.IsNullOrEmpty(item.CustomerReference2))
                customerReferences.AddRange(item.CustomerReference2.Split(','));
            if (!string.IsNullOrEmpty(item.ForwardingCustomerReferences1))
                customerReferences.AddRange(item.ForwardingCustomerReferences1.Split(','));
            if (!string.IsNullOrEmpty(item.ForwardingCustomerReferences2))
                customerReferences.AddRange(item.ForwardingCustomerReferences2.Split(','));

            if (!string.IsNullOrEmpty(item.OrderCustomerReferences))
                customerReferences.AddRange(item.OrderCustomerReferences.Split(','));
            if (!string.IsNullOrEmpty(item.OrderPONumber))
                customerReferences.Add(item.OrderPONumber);
            if (!string.IsNullOrEmpty(item.OrderBookingConfirmationNumber))
                customerReferences.Add(item.OrderBookingConfirmationNumber);
            customerReferences = customerReferences.Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            return string.Join(",", customerReferences);
        }

        private List<CustomerReferenceSyncModel> GetCustomCustomerReferenceSyncModels(string ids)
        {
            var query = $@"  Select 
                             C.Id,
                             C.CustomerReference1,
                             C.CustomerReference2,
                             C.CustomerReference3,
                             SHO.CustomerReferences as OrderCustomerReferences,
                             F.CustomerReference1 as ForwardingCustomerReferences1,
                             F.CustomerReference2 as ForwardingCustomerReferences2,
                             F.CustomerReference3 as ForwardingCustomerReferences3,
                             F.House as ForwardingHouse,
							 SHO.House as OrderHouse,
							 SHO.PONumber as OrderPONumber,
							 SHO.BookingConfirmationNumber as OrderBookingConfirmationNumber
                             from Shipments C 
                             LEFT OUTER JOIN dbo.Shipments F   
                             ON C.Id = F.CustomFileId 
                             LEFT OUTER JOIN dbo.ShipmentOrders SHO    
                             ON SHO.ShipmentId = F.Id 
                             where C.Id in ({ids}) 
                        ";
            var shipmentsContext = ShipmentsContext.GetContext((int)Tenant);
            var data = shipmentsContext.GetActiveDbContext().Database.SqlQuery<CustomerReferenceSyncModel>( query, new object[0]).ToListAsync().Result;
            return data;
        }
        private List<CustomerReferenceSyncModel> GetForwardingCustomerReferenceSyncModels(string ids)
        {
            var query = $@"  Select 
                             F.Id,
                             F.CustomerReference1,
                             F.CustomerReference2,
                             F.CustomerReference3,
                             SHO.CustomerReferences as OrderCustomerReferences,
                             NULL as ForwardingCustomerReferences,
                             SHO.House as OrderHouse,
							 SHO.PONumber as OrderPONumber,
							 SHO.BookingConfirmationNumber as OrderBookingConfirmationNumber
                             from Shipments F 
                             LEFT OUTER JOIN dbo.ShipmentOrders SHO    
                             ON SHO.ShipmentId = F.Id 
                             where F.Id in ({ids}) 
                        ";
            var shipmentsContext = ShipmentsContext.GetContext((int)Tenant);
            var data = shipmentsContext.GetActiveDbContext().Database.SqlQuery<CustomerReferenceSyncModel>(query, new object[0]).ToListAsync().Result;
            return data;
        }

        private void RemoveQueueRecords(List<CargoReferencesSyncQueue> orderShipmentQueue)
        {
            var ids = GetIdsAsString(orderShipmentQueue.Select(e => e.Id + "").ToList());
            var query = $"delete from CargoReferencesSyncQueues where id in ({ids}) ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        private void DeleteOldSearches(SqlConnection sqlConnection, SqlTransaction transaction, List<CargoTrackingShipmentSearch> searches)
        {
            var ids = GetIdsAsString(searches.Select(e => e.ShipmentId).ToList());
            var query = $"delete from CargoTrackingShipmentSearches where ShipmentId in ({ids}) ";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Transaction = transaction;
            command.ExecuteNonQuery();
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
        private Dictionary<string, CargoReferencesSyncQueue> GetShipmentQueueDictionary(List<CargoReferencesSyncQueue> shipmentQueue)
        {
            var dictionary = new Dictionary<string, CargoReferencesSyncQueue>(shipmentQueue.Count);
            foreach (var item in shipmentQueue)
            {
                if (!dictionary.ContainsKey(item.ShipmentId))
                {
                    dictionary.Add(item.ShipmentId, item);
                }
            }
            return dictionary;
        }

    }
    public class CustomerReferenceSyncModel
    {
        public String Id { get; set; }
        public String CustomerReference1 { get; set; }
        public String CustomerReference2 { get; set; }
        public String CustomerReference3 { get; set; }
        public String ForwardingCustomerReferences1 { get; set; }
        public String ForwardingCustomerReferences2 { get; set; }
        public String ForwardingCustomerReferences3 { get; set; }
        public String OrderCustomerReferences { get; set; }
        public String OrderBookingConfirmationNumber { get; set; }
        public String OrderPONumber { get; set; }
        public String ForwardingHouse { get; set; }
        public String OrderHouse { get; set; }
        
    }
    public class CustomerReferenceModel
    {
        public String Id { get; set; }
        public String CustomerReference { get; set; }
        public String OrderHouse { get; set; }
        public String ForwardingHouse { get; set; }


    }
}
