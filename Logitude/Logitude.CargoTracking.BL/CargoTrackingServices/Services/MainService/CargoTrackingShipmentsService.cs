using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CustomMapping;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.SearchService;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.BulkOperations;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.MainService
{
    public class CargoTrackingShipmentsService
    {
        public CargoTrackingShipmentsServiceResults Update(UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            using (SqlConnection sourceConnection =
                    new SqlConnection(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.SourceConnectionString)
            )
            {
                sourceConnection.Open();
                var reader = GetReaderForCargoTrackingShipments(sourceConnection, updateCargoTrackingRecords);
                var results = FillData(reader, updateCargoTrackingRecords);
                return results;
            }
        }

        private CargoTrackingShipmentsServiceResults FillData(SqlDataReader reader, UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            var addedRowsDictionary = new Dictionary<string, string>();
            var maxBulkNumber = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.NumberOfBulkPerTime;
            var runBulkArgs = new RunBulkArgs()
            {
                ConnectionString = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString,
                TableName = "dbo." + updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName,
                InnerTableName = "dbo." + updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName,
                IsUpdateFromBuild = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.IsUpdateFromBuild,
                CargoTrackingShipments = new List<CargoTrackingShipment>()
            };

            var results = new CargoTrackingShipmentsServiceResults();

            while (reader.Read())
            {
                var row = SqlDataReaderConverter.ConvertToObject<CargoTrackingShipmentQueryResult>(reader);
                var cargoTracingShipments = GetShipmentsFromRow(row, addedRowsDictionary);
                FillLastUpdateDate(results, row);
                runBulkArgs.CargoTrackingShipments.AddRange(cargoTracingShipments);
                if (runBulkArgs.CargoTrackingShipments.Count == maxBulkNumber)
                {
                    RunBulk(runBulkArgs);
                    results.RecordsNumber += runBulkArgs.CargoTrackingShipments.Count;
                    runBulkArgs.CargoTrackingShipments.Clear();
                }
            }
            if (runBulkArgs.CargoTrackingShipments.Count > 0)
            {
                RunBulk(runBulkArgs);
                results.RecordsNumber += runBulkArgs.CargoTrackingShipments.Count;
            }
            return results;
        }

        private void FillLastUpdateDate(CargoTrackingShipmentsServiceResults results, CargoTrackingShipmentQueryResult row)
        {
            if (results.LastUpdateDate == null)
            {
                results.LastUpdateDate = GetMaxDate(row);
                return;
            }
            if (row.OrderAutomaticLastUpdateDate.HasValue &&
                     row.OrderAutomaticLastUpdateDate > results.LastUpdateDate)
            {
                results.LastUpdateDate = row.OrderAutomaticLastUpdateDate;
            }
            if (row.ForwardingAutomaticLastUpdateDate.HasValue &&
                     row.ForwardingAutomaticLastUpdateDate > results.LastUpdateDate)
            {
                results.LastUpdateDate = row.ForwardingAutomaticLastUpdateDate;
            }
            if (row.CustomAutomaticLastUpdateDate.HasValue &&
                     row.CustomAutomaticLastUpdateDate > results.LastUpdateDate)
            {
                results.LastUpdateDate = row.CustomAutomaticLastUpdateDate;
            }
        }

        private DateTime? GetMaxDate(CargoTrackingShipmentQueryResult row)
        {
            DateTime? maxDate = null;
            if (row.OrderAutomaticLastUpdateDate.HasValue)
            {
                maxDate = row.OrderAutomaticLastUpdateDate;
            }

            if (row.ForwardingAutomaticLastUpdateDate.HasValue &&
                (maxDate == null || row.ForwardingAutomaticLastUpdateDate > maxDate))
            {
                maxDate = row.ForwardingAutomaticLastUpdateDate;
            }

            if (row.CustomAutomaticLastUpdateDate.HasValue &&
            (maxDate == null || row.CustomAutomaticLastUpdateDate > maxDate))
            {
                maxDate = row.CustomAutomaticLastUpdateDate;
            }
            return maxDate;
        }

        private void RunBulk(RunBulkArgs runBulkArgs)
        {
            if (runBulkArgs.IsUpdateFromBuild)
            {
                BulkInsert(runBulkArgs);
            }
            else
            {
                BulkMarge(runBulkArgs);
            }
        }

        private List<CargoTrackingShipment> GetShipmentsFromRow(CargoTrackingShipmentQueryResult row, Dictionary<string, string> addRowsDictionary)
        {
            var CargoTrackingShipments = new List<CargoTrackingShipment>();
            var map = new CargoTrackingShipmentsMappingService();
            CargoTrackingShipment order = null, forwarding = null, custom = null;
            if (CheckIsAddedOrExist(Codes.OrderType, row.OrderId, addRowsDictionary))
            {
                order = map.GetOrder(row);
                CargoTrackingShipments.Add(order);
            }
            if (row.ForwardingShipmentLevelCode != Codes.CustomShipmentLevelCode && CheckIsAddedOrExist(Codes.ForwardingType, row.ForwardingId, addRowsDictionary))
            {
                forwarding = map.GetForwarding(row);
                CargoTrackingShipments.Add(forwarding);
                if (order != null)
                {
                    SyncForwardingMilestones(order, forwarding);
                    forwarding.Chiled = order;
                }
            }
            if (row.CustomShipmentLevelCode == Codes.CustomShipmentLevelCode && CheckIsAddedOrExist(Codes.CustomType, row.CustomId, addRowsDictionary))
            {
                custom = map.GetCustom(row);
                CargoTrackingShipments.Add(custom);
                if (forwarding != null)
                {
                    SyncCustomeMilestones(forwarding, custom);
                    custom.Chiled = forwarding;
                }
            }
            return CargoTrackingShipments;
        }
        private void SyncCustomeMilestones(CargoTrackingShipment from, CargoTrackingShipment to)
        {
            //from forwarding
            to.CreateDate = from.CreateDate;
            to.BookingDate = from.BookingDate ?? to.BookingDate;
            to.PickupEstimationDate = from.PickupEstimationDate;
            to.FirstPickupETD = from.PickupDate ?? to.PickupDate;
            to.PickupDate = from.PickupDate ?? to.PickupDate;

            //from forwarding
            to.FromWarehouseDate = from.FromWarehouseDate ?? to.FromWarehouseDate;
            to.FromWarehouseEstimationDate = from.FromWarehouseEstimationDate ?? to.FromWarehouseEstimationDate;
            to.FromWarehouseNotes = from.FromWarehouseNotes ?? to.FromWarehouseNotes;

            to.DepartureDate = from.DepartureDate ?? to.DepartureDate;
            to.DepartureEstimationDate = from.DepartureEstimationDate ?? to.DepartureEstimationDate;
            to.ArrivalDate = from.ArrivalDate ?? to.ArrivalDate;
            to.ArrivalEstimationDate = from.ArrivalEstimationDate ?? to.ArrivalEstimationDate;
            //from Customs
            to.WarehouseLegActualEntryDate = to.WarehouseLegActualEntryDate ?? from.WarehouseLegActualEntryDate;
            to.WarehouseLegExpectedEntryDate = to.WarehouseLegExpectedEntryDate ?? from.WarehouseLegExpectedEntryDate;
            to.WarehouseLegRemarks = to.WarehouseLegRemarks ?? from.WarehouseLegRemarks;

            to.ToWarehouseDate = to.ToWarehouseDate ?? from.ToWarehouseDate;
            to.ToWarehouseEstimationDate = to.ToWarehouseEstimationDate ?? from.ToWarehouseEstimationDate;
            to.ToWarehouseNotes = to.ToWarehouseNotes ?? from.ToWarehouseNotes;


            to.AssignedCustomsAgentDate = to.AssignedCustomsAgentDate ?? from.AssignedCustomsAgentDate;
            to.AssignedCustomsAgentEstDate = to.AssignedCustomsAgentEstDate ?? from.AssignedCustomsAgentEstDate;
            to.AssignedCustomsAgentExcReason = to.AssignedCustomsAgentExcReason ?? from.AssignedCustomsAgentExcReason;
            to.AssignedCustomsAgentNotes = to.AssignedCustomsAgentNotes ?? from.AssignedCustomsAgentNotes;
            to.GoodsClassificationDate = to.GoodsClassificationDate ?? from.GoodsClassificationDate;
            to.GoodsClassificationEstDate = to.GoodsClassificationEstDate ?? from.GoodsClassificationEstDate;
            to.PaymentReceivedDate = to.PaymentReceivedDate ?? from.PaymentReceivedDate;
            to.PaymentReceivedEstomationDate = to.PaymentReceivedEstomationDate ?? from.PaymentReceivedEstomationDate;
            to.PaymentReceivedNotes = to.PaymentReceivedNotes ?? from.PaymentReceivedNotes;
            to.PaymentRequiredDate = to.PaymentRequiredDate ?? from.PaymentRequiredDate;
            to.PaymentRequiredEstimationDate = to.PaymentRequiredEstimationDate ?? from.PaymentRequiredEstimationDate;
            to.PaymentRequiredNotes = to.PaymentRequiredNotes ?? from.PaymentRequiredNotes;
            to.ClearanceDate = to.ClearanceDate ?? from.ClearanceDate;
            to.CustomsClearanceDate = to.CustomsClearanceDate ?? from.CustomsClearanceDate;

        }
        private void SyncForwardingMilestones(CargoTrackingShipment from, CargoTrackingShipment to)
        {
            //from order
            to.CreateDate = from.CreateDate;
            to.BookingDate = from.BookingDate ?? to.BookingDate;
            to.PickupEstimationDate = from.PickupEstimationDate;
            to.FirstPickupETD = from.PickupDate ?? to.PickupDate;
            to.PickupDate = from.PickupDate ?? to.PickupDate;

            //from forwarding
            to.FromWarehouseDate = to.FromWarehouseDate ?? from.FromWarehouseDate;
            to.FromWarehouseEstimationDate = to.FromWarehouseEstimationDate ?? from.FromWarehouseEstimationDate;
            to.FromWarehouseNotes = to.FromWarehouseNotes ?? from.FromWarehouseNotes;

            to.DepartureDate = to.DepartureDate ?? from.DepartureDate;
            to.DepartureEstimationDate = from.DepartureEstimationDate ?? to.DepartureEstimationDate;
            to.ArrivalDate = to.ArrivalDate ?? from.ArrivalDate;
            to.ArrivalEstimationDate = to.ArrivalEstimationDate ?? from.ArrivalEstimationDate;

            

        }
        private bool CheckIsAddedOrExist(string key, string Id, Dictionary<string, string> addRowsDictionary)
        {
            if (!string.IsNullOrEmpty(Id) && !addRowsDictionary.ContainsKey(key + Id.ToString()))
            {
                addRowsDictionary.Add(key + Id, Id);
                return true;
            }
            return false;

        }

        private void BulkInsert(RunBulkArgs runBulkArgs)
        {
            using (SqlConnection connection = new SqlConnection(runBulkArgs.ConnectionString))
            {
                connection.Open();
                using (var bulk = new BulkOperation<CargoTrackingShipment>(connection))
                {
                    bulk.DestinationTableName = runBulkArgs.TableName;
                    bulk.BulkInsert(runBulkArgs.CargoTrackingShipments);
                }
                AddShipmentSearches(runBulkArgs, connection);

            }
        }
        private void AddShipmentSearches(RunBulkArgs runBulkArgs, SqlConnection connection)
        {

            if (string.IsNullOrEmpty(runBulkArgs.InnerTableName))
            {
                return;
            }
            var cargoTrackingShipmentSearches = GetCargoTrackingShipmentSearches(runBulkArgs.CargoTrackingShipments);

            if (!runBulkArgs.IsUpdateFromBuild)
            {
                DeleteOldSearches(runBulkArgs.CargoTrackingShipments, connection, runBulkArgs.InnerTableName);
            }
            using (var bulk = new BulkOperation<CargoTrackingShipmentSearch>(connection))
            {
                bulk.DestinationTableName = runBulkArgs.InnerTableName;
                bulk.BulkInsert(cargoTrackingShipmentSearches);
            }

        }

        private void DeleteOldSearches(List<CargoTrackingShipment> cargoTrackingShipments, SqlConnection sqlConnection, string tableName)
        {
            var query = CargoTrackingQueriesService.GetDeleteSearchesQuery(cargoTrackingShipments, tableName);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandTimeout = int.MaxValue;
            sqlCommand.ExecuteNonQuery();
        }

        private List<CargoTrackingShipmentSearch> GetCargoTrackingShipmentSearches(List<CargoTrackingShipment> cargoTrackingShipments)
        {
            var cargoTrackingShipmentSearchs = new List<CargoTrackingShipmentSearch>();
            foreach (var shipment in cargoTrackingShipments)
            {
                cargoTrackingShipmentSearchs.AddRange(AddShipmentSearchesToList(shipment, shipment.EntityId));
            }
            return cargoTrackingShipmentSearchs;
        }

        private List<CargoTrackingShipmentSearch> AddShipmentSearchesToList(CargoTrackingShipment shipment, string entityId)
        {
            var cargoTrackingShipmentSearchs = new List<CargoTrackingShipmentSearch>();

            if (CargoTrackingSearchService.IsShipmentValidToCreateRefrences(shipment))
            {
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetShipmentNumberReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetCustomsDeclarationNumberReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetForwarderShipmentNumberReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetShipperNameReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetConsigneeNameReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetCustomerReferenceReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetContainerNumbersReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetHouseReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetMasterReferences(shipment, entityId));
            }
            if(shipment.Chiled != null)
            {
                cargoTrackingShipmentSearchs.AddRange(AddShipmentSearchesToList(shipment.Chiled, entityId));
            }
            return cargoTrackingShipmentSearchs;
        }

        private void BulkMarge(RunBulkArgs runBulkArgs)
        {
            using (SqlConnection connection = new SqlConnection(runBulkArgs.ConnectionString))
            {
                connection.Open();
                using (var bulk = new BulkOperation<CargoTrackingShipment>(connection))
                {
                    bulk.DestinationTableName = runBulkArgs.TableName;
                    bulk.AutoMapKeyExpression = c => new { c.EntityType, c.EntityId, c.Tenant };
                    bulk.BulkMerge(runBulkArgs.CargoTrackingShipments);
                }
                AddShipmentSearches(runBulkArgs, connection);
            }
        }

        private SqlDataReader GetReaderForCargoTrackingShipments(SqlConnection sourceConnection, UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            var query = CargoTrackingQueriesService.GetQuery(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs);

            SqlCommand commandSourceData = new SqlCommand(query, sourceConnection);
            commandSourceData.Transaction = sourceConnection.BeginTransaction(IsolationLevel.Snapshot);
            commandSourceData.CommandTimeout = int.MaxValue;
            SqlDataReader reader = commandSourceData.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }




    }
}
