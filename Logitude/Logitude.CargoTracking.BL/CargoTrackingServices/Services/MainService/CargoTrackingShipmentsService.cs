using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CustomMapping;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.SearchService;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.BulkOperations;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.MainService
{
    public class CargoTrackingShipmentsService
    {
        CargoTrackingShipmentsMappingService map { get; set; }
        public CargoTrackingShipmentsService()
        {
            map = new CargoTrackingShipmentsMappingService();
        }
        public CargoTrackingShipmentsServiceResults Update(UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            var milestones = GetMilestones(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
            using (SqlConnection sourceConnection =
                    new SqlConnection(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.SourceConnectionString)
            )
            {
                sourceConnection.Open();
                var reader = GetReaderForCargoTrackingShipments(sourceConnection, updateCargoTrackingRecords);
                var results = FillData(reader, updateCargoTrackingRecords, milestones);
                return results;
            }
        }

        private CargoTrackingShipmentsServiceResults FillData(SqlDataReader reader, UpdateCargoTrackingRecords updateCargoTrackingRecords, List<CargoTrackingMilestoneList> milestones)
        {
            var addedRowsDictionary = new Dictionary<string, string>();
            var maxBulkNumber = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.NumberOfBulkPerTime;
            var runBulkArgs = new RunBulkArgs()
            {
                ConnectionString = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString,
                TableName = "dbo." + updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName,
                InnerTableName = "dbo." + updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName,
                IsUpdateFromBuild = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.IsUpdateFromBuild,
                CargoTrackingShipmentContexts = new List<CargoTrackingShipmentResources>()
            };

            var results = new CargoTrackingShipmentsServiceResults();

            while (reader.Read())
            {
                var row = SqlDataReaderConverter.ConvertToObject<CargoTrackingShipmentQueryResult>(reader);
                var cargoTracingShipments = GetShipmentsFromRow(row, addedRowsDictionary, milestones);
                FillLastUpdateDate(results, row);
                runBulkArgs.CargoTrackingShipmentContexts.AddRange(cargoTracingShipments);
                if (runBulkArgs.CargoTrackingShipmentContexts.Count == maxBulkNumber)
                {
                    RunBulk(runBulkArgs);
                    results.RecordsNumber += runBulkArgs.CargoTrackingShipmentContexts.Count;
                    runBulkArgs.CargoTrackingShipmentContexts.Clear();
                }
            }
            if (runBulkArgs.CargoTrackingShipmentContexts.Count > 0)
            {
                RunBulk(runBulkArgs);
                results.RecordsNumber += runBulkArgs.CargoTrackingShipmentContexts.Count;
            }
            return results;
        }
        public List<CargoTrackingMilestoneList> GetMilestones(string connectionString)
        {
            var milestons = new List<CargoTrackingMilestoneList>();

            var query = "select * from CargoTrackingMilestones";
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                SqlCommand commandSourceData = new SqlCommand(query, sourceConnection);
                commandSourceData.CommandTimeout = int.MaxValue;
                sourceConnection.Open();
                SqlDataReader reader = commandSourceData.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    var row = SqlDataReaderConverter.ConvertToObject<CargoTrackingMilestoneList>(reader);
                    milestons.Add(row);
                }
                return milestons;
            }

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

        private List<CargoTrackingShipmentResources> GetShipmentsFromRow(CargoTrackingShipmentQueryResult row, Dictionary<string, string> addRowsDictionary, List<CargoTrackingMilestoneList> milestones)
        {
            var cargoTrackingShipmentContext = new List<CargoTrackingShipmentResources>();
            CargoTrackingShipmentResources order = null, forwarding = null, custom = null;
            if (CheckIsAddedOrExist(Codes.OrderType, row.OrderId, addRowsDictionary))
            {
                order = GetOrderFromRow(row, milestones);
                cargoTrackingShipmentContext.Add(order);
            }
            if (row.ForwardingShipmentLevelCode != Codes.CustomShipmentLevelCode && CheckIsAddedOrExist(Codes.ForwardingType, row.ForwardingId, addRowsDictionary))
            {
                forwarding = GetForwardingFromRow(row, milestones, order);
                cargoTrackingShipmentContext.Add(forwarding);
            }
            if (row.CustomShipmentLevelCode == Codes.CustomShipmentLevelCode && CheckIsAddedOrExist(Codes.CustomType, row.CustomId, addRowsDictionary))
            {
                custom = GetCustomFromRow(row, milestones, forwarding);
                cargoTrackingShipmentContext.Add(custom);
            }

            return cargoTrackingShipmentContext;
        }

        private CargoTrackingShipmentResources GetCustomFromRow(CargoTrackingShipmentQueryResult row, List<CargoTrackingMilestoneList> milestones, CargoTrackingShipmentResources forwarding)
        {
            var custom = map.GetCustom(row);
            if (forwarding != null)
            {
                SyncCustomeMilestones(forwarding.CargoTrackingShipment, custom.CargoTrackingShipment);
                custom.Child = forwarding;
            }
            SetMilestonesDoneFields(custom.CargoTrackingShipment);
            SetCurrentMilestone(custom.CargoTrackingShipment, milestones);
            return custom;
        }

        private CargoTrackingShipmentResources GetForwardingFromRow(CargoTrackingShipmentQueryResult row, List<CargoTrackingMilestoneList> milestones, CargoTrackingShipmentResources order)
        {
            var forwarding = map.GetForwarding(row);
            if (order != null)
            {
                SyncForwardingMilestones(order.CargoTrackingShipment, forwarding.CargoTrackingShipment);
                forwarding.Child = order;
            }
            SetMilestonesDoneFields(forwarding.CargoTrackingShipment);
            SetCurrentMilestone(forwarding.CargoTrackingShipment, milestones);
            return forwarding;
        }

        private CargoTrackingShipmentResources GetOrderFromRow(CargoTrackingShipmentQueryResult row, List<CargoTrackingMilestoneList> milestones)
        {
            var map = new CargoTrackingShipmentsMappingService();
            var order = map.GetOrder(row);
            SetMilestonesDoneFields(order.CargoTrackingShipment);
            SetCurrentMilestone(order.CargoTrackingShipment, milestones);
            return order;
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
                    var list = runBulkArgs.CargoTrackingShipmentContexts.Select(e => e.CargoTrackingShipment).ToList();
                    bulk.DestinationTableName = runBulkArgs.TableName;
                    bulk.BulkInsert(list);
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
            var cargoTrackingShipmentSearches = GetCargoTrackingShipmentSearches(runBulkArgs.CargoTrackingShipmentContexts);

            if (!runBulkArgs.IsUpdateFromBuild)
            {
                DeleteOldSearches(runBulkArgs.CargoTrackingShipmentContexts, connection, runBulkArgs.InnerTableName);
            }
            using (var bulk = new BulkOperation<CargoTrackingShipmentSearch>(connection))
            {
                bulk.DestinationTableName = runBulkArgs.InnerTableName;
                bulk.BulkInsert(cargoTrackingShipmentSearches);
            }

        }

        private void DeleteOldSearches(List<CargoTrackingShipmentResources> cargoTrackingShipmentContexts, SqlConnection sqlConnection, string tableName)
        {
            var query = CargoTrackingQueriesService.GetDeleteSearchesQuery(cargoTrackingShipmentContexts, tableName);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandTimeout = int.MaxValue;
            sqlCommand.ExecuteNonQuery();
        }

        private List<CargoTrackingShipmentSearch> GetCargoTrackingShipmentSearches(List<CargoTrackingShipmentResources> cargoTrackingShipmentContexts)
        {
            var cargoTrackingShipmentSearchs = new List<CargoTrackingShipmentSearch>();
            foreach (var shipment in cargoTrackingShipmentContexts)
            {
                cargoTrackingShipmentSearchs.AddRange(AddShipmentSearchesToList(shipment, shipment.CargoTrackingShipment.EntityId));
            }
            return cargoTrackingShipmentSearchs;
        }

        private List<CargoTrackingShipmentSearch> AddShipmentSearchesToList(CargoTrackingShipmentResources shipment, string entityId)
        {
            var cargoTrackingShipmentSearchs = new List<CargoTrackingShipmentSearch>();

            if (CargoTrackingSearchService.IsShipmentValidToCreateRefrences(shipment.CargoTrackingShipment))
            {
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetShipmentNumberReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetCustomsDeclarationNumberReferences(shipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetForwarderShipmentNumberReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetShipperNameReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetConsigneeNameReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetCustomerReferenceReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetContainerNumbersReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetHouseReferences(shipment.CargoTrackingShipment, entityId));
                cargoTrackingShipmentSearchs.AddRange(CargoTrackingSearchService.GetMasterReferences(shipment.CargoTrackingShipment, entityId));
            }
            if(shipment.Child != null)
            {
                cargoTrackingShipmentSearchs.AddRange(AddShipmentSearchesToList(shipment.Child, entityId));
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
                    var list = runBulkArgs.CargoTrackingShipmentContexts.Select(e => e.CargoTrackingShipment).ToList();
                    bulk.DestinationTableName = runBulkArgs.TableName;
                    bulk.AutoMapKeyExpression = c => new { c.EntityType, c.EntityId, c.Tenant };
                    bulk.BulkMerge(list);
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
        private static void SetMilestonesDoneFields(CargoTrackingShipment item)
        {
            item.PickupDone = item.PickupDate.HasValue;
            item.BookingDone = item.BookingDate.HasValue;
            item.CreatedDone = true;/*item.CreateDate.HasValue;*/
            item.DepartureDone = item.DepartureDate.HasValue;
            item.ArrivalDone = item.ArrivalDate.HasValue;
            item.FromWarehouseDone = item.FromWarehouseDate.HasValue && item.DirectionId == Codes.ExportDirection;
            item.ToWarehouseDone = item.ToWarehouseDate.HasValue && item.DirectionId == Codes.ImportDirection;
            item.CustomsPaymentDone = item.CustomsPaymentDate.HasValue;
            item.ClearanceDone = item.ClearanceDate.HasValue;
            item.DeliveredDone = item.DeliveredDate.HasValue;
            item.DeliveryDone = item.DeliveryDate.HasValue;
            item.AssignedTruckerDone = item.AssignedTruckerDate.HasValue;
            item.AssignedCustomsAgentDone = item.AssignedCustomsAgentDate.HasValue;
            item.GoodsClassificationDone = item.GoodsClassificationDate.HasValue;
            item.DocumentInspectionDone = item.DocumentInspectionDate.HasValue;
            item.GatepassArrivedDone = item.GatepassArrivedDate.HasValue;
            item.PaymentReceivedDone = item.PaymentReceivedDate.HasValue;
            item.PaymentRequiredDone = item.PaymentRequiredDate.HasValue;


        }
        private static void SetCurrentMilestone(CargoTrackingShipment item, List<CargoTrackingMilestoneList> milestones)
        {
            var current = new CurrentMilestone();
            foreach (var milestone in milestones)
            {
                switch (milestone.Code)
                {
                    case CargoTrackingMilestoneValues.Created:
                        if (item.CreatedDone.HasValue && item.CreatedDone.Value)
                            CheckMilestone(current, milestone, item.CreateDate);
                        break;
                    case CargoTrackingMilestoneValues.Booking:
                        if (item.BookingDone.HasValue && item.BookingDone.Value)
                            CheckMilestone(current, milestone, item.BookingDate);
                        break;
                    case CargoTrackingMilestoneValues.Pickup:
                        if (item.PickupDone.HasValue && item.PickupDone.Value)
                            CheckMilestone(current, milestone, item.PickupDate);
                        break;
                    case CargoTrackingMilestoneValues.FromWarehouse:
                        if (item.FromWarehouseDone.HasValue && item.FromWarehouseDone.Value)
                            CheckMilestone(current, milestone, item.FromWarehouseDate);
                        break;
                    case CargoTrackingMilestoneValues.Departure:
                        if (item.DepartureDone.HasValue && item.DepartureDone.Value)
                            CheckMilestone(current, milestone, item.DepartureDate);
                        break;
                    case CargoTrackingMilestoneValues.Arrival:
                        if (item.ArrivalDone.HasValue && item.ArrivalDone.Value)
                            CheckMilestone(current, milestone, item.ArrivalDate);
                        break;
                    case CargoTrackingMilestoneValues.ToWarehouse:
                        if (item.ToWarehouseDone.HasValue && item.ToWarehouseDone.Value)
                            CheckMilestone(current, milestone, item.ToWarehouseDate);
                        break;
                    case CargoTrackingMilestoneValues.AssignedToCustomsBroker:
                        if (item.AssignedCustomsAgentDone.HasValue && item.AssignedCustomsAgentDone.Value)
                            CheckMilestone(current, milestone, item.AssignedCustomsAgentDate);
                        break;
                    //case CargoTrackingMilestoneValues.CustomsProcess:
                    //  if(item.CreatedDone.HasValue && item.Done.Value)
                    //    CheckMilestone(current, milestone, item.CustomsProcess);break;
                    case CargoTrackingMilestoneValues.GoodsClassification:
                        if (item.GoodsClassificationDone.HasValue && item.GoodsClassificationDone.Value)
                            CheckMilestone(current, milestone, item.GoodsClassificationDate);
                        break;
                    case CargoTrackingMilestoneValues.DocumentInspection:
                        if (item.DocumentInspectionDone.HasValue && item.DocumentInspectionDone.Value)
                            CheckMilestone(current, milestone, item.DocumentInspectionDate);
                        break;
                    case CargoTrackingMilestoneValues.PaymentRequested:
                        if (item.PaymentRequiredDone.HasValue && item.PaymentRequiredDone.Value)
                            CheckMilestone(current, milestone, item.PaymentRequiredDate);
                        break;
                    case CargoTrackingMilestoneValues.PaymentReceived:
                        if (item.PaymentReceivedDone.HasValue && item.PaymentReceivedDone.Value)
                            CheckMilestone(current, milestone, item.PaymentReceivedDate);
                        break;
                    case CargoTrackingMilestoneValues.CustomsPayment:
                        if (item.CustomsPaymentDone.HasValue && item.CustomsPaymentDone.Value)
                            CheckMilestone(current, milestone, item.CustomsPaymentDate);
                        break;
                    case CargoTrackingMilestoneValues.Clearance:
                        if (item.ClearanceDone.HasValue && item.ClearanceDone.Value)
                            CheckMilestone(current, milestone, item.ClearanceDate);
                        break;
                    case CargoTrackingMilestoneValues.GatepassArrived:
                        if (item.GatepassArrivedDone.HasValue && item.GatepassArrivedDone.Value)
                            CheckMilestone(current, milestone, item.GatepassArrivedDate);
                        break;
                    case CargoTrackingMilestoneValues.AssignedToTrucker:
                        if (item.AssignedTruckerDone)
                            CheckMilestone(current, milestone, item.AssignedTruckerDate);
                        break;
                    case CargoTrackingMilestoneValues.DeliveryOut:
                        if (item.DeliveryDone)
                            CheckMilestone(current, milestone, item.DeliveryDate);
                        break;
                    case CargoTrackingMilestoneValues.Delivered:
                        if (item.DeliveredDone.HasValue && item.DeliveredDone.Value)
                            CheckMilestone(current, milestone, item.DeliveredDate);
                        break;
                    //case CargoTrackingMilestoneValues.Invoiced:
                    //if (item.CreatedDone.HasValue && item.Done.Value)
                    //    CheckMilestone(current, milestone, item); break;
                    default:
                        break;
                }
            }
            item.CurrentMilestoneCode = current.CurrentMilestoneCode;
            item.CurrentMilestoneDate = current.CurrentMilestoneDate;

        }

        private static void CheckMilestone(CurrentMilestone current, CargoTrackingMilestoneList milestone, DateTime? date)
        {
            if (milestone.Weight > current.Wheight)
            {
                current.Wheight = milestone.Weight.HasValue ? milestone.Weight.Value : 0;
                current.CurrentMilestoneDate = date;
                current.CurrentMilestoneCode = milestone.Code;
            }
        }

        public Dictionary<int, Dictionary<string, string>> GetAllMilestonesNotPermitted(string connectionString)
        {
            var MilestonesNotAllowedToBeViewed = new List<CargoTenantMilestoneDefinitions>();

            var query = "select * from CargoTenantMilestoneDefinitions where IsCustomerView = 0";
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                SqlCommand commandSourceData = new SqlCommand(query, sourceConnection);
                commandSourceData.CommandTimeout = int.MaxValue;
                sourceConnection.Open();
                SqlDataReader reader = commandSourceData.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    var row = SqlDataReaderConverter.ConvertToObject<CargoTenantMilestoneDefinitions>(reader);
                    MilestonesNotAllowedToBeViewed.Add(row);
                }
                var DictionaryMilestonesNotPermitted = GetDictionaryMilestonesNotPermittedByTenentAndCode(MilestonesNotAllowedToBeViewed);
                return DictionaryMilestonesNotPermitted;
            }
        }

        private Dictionary<int, Dictionary<string, string>> GetDictionaryMilestonesNotPermittedByTenentAndCode(List<CargoTenantMilestoneDefinitions> milestonesNotAllowedToBeViewed)
        {
            // first key = tenant , second key = milestone code
            var dictionaryMilestonesNotPermitted = new Dictionary<int, Dictionary<string, string>>();
            foreach (var item in milestonesNotAllowedToBeViewed)
            {
                //Check tenant
                if (!dictionaryMilestonesNotPermitted.ContainsKey(item.Tenant))
                {
                    dictionaryMilestonesNotPermitted.Add(item.Tenant, new Dictionary<string, string>());
                }

                var tenantMilestonesNotPermitted = dictionaryMilestonesNotPermitted[item.Tenant];
                if (!tenantMilestonesNotPermitted.ContainsKey(item.Code))
                {
                    tenantMilestonesNotPermitted.Add(item.Code, item.Code);
                }
            }
            return dictionaryMilestonesNotPermitted;
        }
    }
    public class CurrentMilestone
    {
        public int Wheight = 0;
        public string CurrentMilestoneCode = null;
        public DateTime? CurrentMilestoneDate = null;
    }
}
