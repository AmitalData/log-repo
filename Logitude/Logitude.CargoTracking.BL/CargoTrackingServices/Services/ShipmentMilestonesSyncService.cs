using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class ShipmentMilestonesSyncService
    {
        List<string> customsMilstonesFields = new List<string>
            {
                "ToWarehouseDone",
                "ToWarehouseDate",
                "ToWarehouseEstimationDate",
                "ToWarehouseNotes",
                "AssignedCustomsAgentDone",
                "AssignedCustomsAgentDate",
                "AssignedCustomsAgentEstDate",
                "AssignedCustomsAgentNotes",
                "CustomsPaymentDone",
                "CustomsPaymentDate",
                "ClearanceDone",
                "ClearanceDate",
                "AssignedTruckerDone",
                "AssignedTruckerDate",
                "AssignedTruckerEstimationDate",
                "AssignedTruckerNotes",
                "DeliveryDone",
                "DeliveryDate",
                "DeliveryEstimationDate",
                "DeliveryNotes",
                "DeliveryExceptionReason",
                "DeliveredDone",
                "DeliveredDate",
                "DeliveredEstimationDate",
                "GatepassArrivedDone",
                "GatepassArrivedDate",
                "GatepassArrivedEstDate",
                "GatepassArrivedNotes",
                "DocumentInspectionDate",
                "DocumentInspectionEstDate",
                "DocumentInspectionDone",
                "DocumentInspectionNotes",
                "GoodsClassificationDate",
                "GoodsClassificationDone",
                "GoodsClassificationNotes",
                "GoodsClassificationEstDate"
            };
        List<string> forwardingMilstonesFields = new List<string>
            {
                "PickupDate",
                "PickupDone",
                "PickupEstimationDate",
                "FromWarehouseDone",
                "FromWarehouseDate",
                "FromWarehouseEstimationDate",
                "FromWarehouseNotes",
                "DepartureDone",
                "DepartureDate",
                "DepartureEstimationDate",
                "ArrivalDone",
                "ArrivalDate",
                "ArrivalEstimationDate",
                "CreatedDone",
                "CreateDate"
            };
        List<string> orderMilstonesFields = new List<string>
            {
                "PickupDate",
                "PickupDone",
                "PickupEstimationDate",
                "BookingDate",
                "CreateDate",

            };



        public void SyncShipmentMilstones(BulkDataPreperation bulkDataPreperation)
        {
            var buildCargoArgs = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs;
            // move fields from order to forwarding
            // & if fields of forwarding is empty, fill their values from order
            var sql = BuildScriptForUpdatingForwardingShipmentMilstonesFromShipmentOrder();
            ExcuteSqlScript(buildCargoArgs, sql);

            sql = BuildScriptForUpdatingOrderShipmentMilstones();
            ExcuteSqlScript(buildCargoArgs, sql);


            // customs
            sql = BuildScriptForUpdatingCustomsShipmentMilstones();
            ExcuteSqlScript(buildCargoArgs, sql);

            sql = BuildScriptForUpdatingForwardingShipmentMilstonesFromCustoms();
            ExcuteSqlScript(buildCargoArgs, sql);

            // current
            //sql = BuildScriptToSetCurrentMistones();
            sql = BuildScriptToSetCurrentMistonesByWeight(bulkDataPreperation);
            ExcuteSqlScript(buildCargoArgs, sql);


            sql = DisconnectShipments(buildCargoArgs);
            //ExcuteSqlScriptForSourceDatabase(cargoArgs, sql);
        }

        private string BuildScriptToSetCurrentMistonesByWeight(BulkDataPreperation bulkDataPreperation)
        {
            var Milestones = bulkDataPreperation.Milestones.Where(e => e.Weight != null).OrderByDescending(e => e.Weight).ToList();
            var doneCases = GetCurrentMistonesQueryDoneCases(Milestones);
            var dateCases = GetCurrentMistonesQueryDateCases(Milestones);
            var query = $@" update CargoTrackingShipments set 
                            CurrentMilestoneCode = 
                                CASE
		                            {doneCases}
		                            ELSE '{CargoTrackingMilestoneValues.Created}'
	                            END,
                            CurrentMilestoneDate = 
                                CASE
		                            {dateCases}
		                            ELSE CurrentMilestoneDate
	                            END
            ";
            return query;
        }

        private string GetCurrentMistonesQueryDoneCases(List<CargoTrackingMilestoneList> milestones)
        {
            var cases = "";
            foreach (var milestone in milestones)
            {
                cases += GetMistonesDoneCase(milestone);
            }
            return cases;
        }

        private string GetMistonesDoneCase(CargoTrackingMilestoneList milestone)
        {
            switch (milestone.Code)
            {
                case CargoTrackingMilestoneValues.Created: return $"WHEN [CreatedDone] = 1 THEN {CargoTrackingMilestoneValues.Created}  ";
                case CargoTrackingMilestoneValues.Booking: return $"WHEN [BookingDone] = 1 THEN {CargoTrackingMilestoneValues.Booking}  ";
                case CargoTrackingMilestoneValues.Pickup: return $"WHEN [PickupDone] = 1 THEN {CargoTrackingMilestoneValues.Pickup}  ";
                case CargoTrackingMilestoneValues.FromWarehouse: return $"WHEN [FromWarehouseDone] = 1 THEN {CargoTrackingMilestoneValues.FromWarehouse}  ";
                case CargoTrackingMilestoneValues.Departure: return $"WHEN [DepartureDone] = 1 THEN {CargoTrackingMilestoneValues.Departure}  ";
                case CargoTrackingMilestoneValues.Arrival: return $"WHEN [ArrivalDone] = 1 THEN {CargoTrackingMilestoneValues.Arrival}  ";
                case CargoTrackingMilestoneValues.ToWarehouse: return $"WHEN [ToWarehouseDone] = 1 THEN {CargoTrackingMilestoneValues.ToWarehouse}  ";
                case CargoTrackingMilestoneValues.AssignedToCustomsBroker: return $"WHEN [AssignedCustomsAgentDone] = 1 THEN {CargoTrackingMilestoneValues.AssignedToCustomsBroker}  ";
                //case CargoTrackingMilestoneValues.CustomsProcess: return $"WHEN [CreatedDone] = 1 THEN {CargoTrackingMilestoneValues.CustomsProcess}  ";
                case CargoTrackingMilestoneValues.GoodsClassification: return $"WHEN [GoodsClassificationDone] = 1 THEN {CargoTrackingMilestoneValues.GoodsClassification}  ";
                case CargoTrackingMilestoneValues.DocumentInspection: return $"WHEN [DocumentInspectionDone] = 1 THEN {CargoTrackingMilestoneValues.DocumentInspection}  ";
                case CargoTrackingMilestoneValues.PaymentRequested: return $"WHEN [PaymentRequiredDone] = 1 THEN {CargoTrackingMilestoneValues.PaymentRequested}  ";
                case CargoTrackingMilestoneValues.PaymentReceived: return $"WHEN [PaymentReceivedDone] = 1 THEN {CargoTrackingMilestoneValues.PaymentReceived}  ";
                case CargoTrackingMilestoneValues.CustomsPayment: return $"WHEN [CustomsPaymentDone] = 1 THEN {CargoTrackingMilestoneValues.CustomsPayment}  ";
                case CargoTrackingMilestoneValues.Clearance: return $"WHEN [ClearanceDone] = 1 THEN {CargoTrackingMilestoneValues.Clearance}  ";
                case CargoTrackingMilestoneValues.GatepassArrived: return $"WHEN [GatepassArrivedDone] = 1 THEN {CargoTrackingMilestoneValues.GatepassArrived}  ";
                case CargoTrackingMilestoneValues.AssignedToTrucker: return $"WHEN [AssignedTruckerDone] = 1 THEN {CargoTrackingMilestoneValues.AssignedToTrucker}  ";
                case CargoTrackingMilestoneValues.DeliveryOut: return $"WHEN [DeliveryDone] = 1 THEN {CargoTrackingMilestoneValues.DeliveryOut}  ";
                case CargoTrackingMilestoneValues.Delivered: return $"WHEN [DeliveredDone] = 1 THEN {CargoTrackingMilestoneValues.Delivered}  ";
                //case CargoTrackingMilestoneValues.Invoiced: return $"WHEN [CreatedDone] = 1 THEN {CargoTrackingMilestoneValues.Invoiced}  ";
            }
            return "";
        }
        private string GetCurrentMistonesQueryDateCases(List<CargoTrackingMilestoneList> milestones)
        {
            var cases = "";
            foreach (var milestone in milestones)
            {
                cases += GetMistonesDateCase(milestone);
            }
            return cases;
        }
        private string GetMistonesDateCase(CargoTrackingMilestoneList milestone)
        {
            switch (milestone.Code)
            {
                case CargoTrackingMilestoneValues.Created: return $"WHEN [CreatedDone] = 1 THEN [CreateDate] ";
                case CargoTrackingMilestoneValues.Booking: return $"WHEN [BookingDone] = 1 THEN [BookingDate]  ";
                case CargoTrackingMilestoneValues.Pickup: return $"WHEN [PickupDone] = 1 THEN [PickupDate]  ";
                case CargoTrackingMilestoneValues.FromWarehouse: return $"WHEN [FromWarehouseDone] = 1 THEN [FromWarehouseDate]  ";
                case CargoTrackingMilestoneValues.Departure: return $"WHEN [DepartureDone] = 1 THEN [DepartureDate]  ";
                case CargoTrackingMilestoneValues.Arrival: return $"WHEN [ArrivalDone] = 1 THEN [ArrivalDate]  ";
                case CargoTrackingMilestoneValues.ToWarehouse: return $"WHEN [ToWarehouseDone] = 1 THEN [ToWarehouseDate]  ";
                case CargoTrackingMilestoneValues.AssignedToCustomsBroker: return $"WHEN [AssignedCustomsAgentDone] = 1 THEN [AssignedCustomsAgentDate]  ";
                //case CargoTrackingMilestoneValues.CustomsProcess: return $"WHEN [] = 1 THEN  [] ";
                case CargoTrackingMilestoneValues.GoodsClassification: return $"WHEN [GoodsClassificationDone] = 1 THEN [GoodsClassificationDate]  ";
                case CargoTrackingMilestoneValues.DocumentInspection: return $"WHEN [DocumentInspectionDone] = 1 THEN [DocumentInspectionDate]  ";
                case CargoTrackingMilestoneValues.PaymentRequested: return $"WHEN [PaymentRequiredDone] = 1 THEN [PaymentReceivedDate]  ";
                case CargoTrackingMilestoneValues.PaymentReceived: return $"WHEN [PaymentReceivedDone] = 1 THEN [PaymentRequiredDate]  ";
                case CargoTrackingMilestoneValues.CustomsPayment: return $"WHEN [CustomsPaymentDone] = 1 THEN [CustomsPaymentDate]  ";
                case CargoTrackingMilestoneValues.Clearance: return $"WHEN [ClearanceDone] = 1 THEN [ClearanceDate]  ";
                case CargoTrackingMilestoneValues.GatepassArrived: return $"WHEN [GatepassArrivedDone] = 1 THEN [GatepassArrivedDate]  ";
                case CargoTrackingMilestoneValues.AssignedToTrucker: return $"WHEN [AssignedTruckerDone] = 1 THEN [AssignedTruckerDate]  ";
                case CargoTrackingMilestoneValues.DeliveryOut: return $"WHEN [DeliveryDone] = 1 THEN [DeliveryDate]  ";
                case CargoTrackingMilestoneValues.Delivered: return $"WHEN [DeliveredDone] = 1 THEN [DeliveredDate]  ";
                    //case CargoTrackingMilestoneValues.Invoiced: return $"WHEN [] = 1 THEN  [] ";
            }
            return "";
        }

        private string BuildScriptForUpdatingForwardingShipmentMilstonesFromShipmentOrder()
        { // order
            string forwardingFields = BuildForwardingUpdatedFieldsFromOrder();

            var sql = string.Concat(
                        $"update ForwardingShipment ",
                        $"set	{forwardingFields} ",
                        $"from	 CargoTrackingShipments ForwardingShipment ",
                        $"join CargoTrackingShipments OrderShipment on OrderShipment.ForwardingShipmentHeaderId = ForwardingShipment.EntityId",
                        $" where  OrderShipment.EntityType = 'O'");
            return sql;
        }
        private string BuildScriptForUpdatingOrderShipmentMilstones()
        {
            string shipmentOrderFields = BuildOrderUpdatedFields();

            var sql = string.Concat(
                        $"update OrderShipment ",
                        $"set	{shipmentOrderFields} ",
                        $"from	 CargoTrackingShipments ForwardingShipment ",
                        $"join CargoTrackingShipments OrderShipment on OrderShipment.ForwardingShipmentHeaderId = ForwardingShipment.EntityId",
                        $" where  OrderShipment.EntityType = 'O'");
            return sql;
        }
        private string BuildScriptForUpdatingCustomsShipmentMilstones()
        {
            string buildCustomsUpdatedFields = BuildCustomsUpdatedFields();

            var sql = string.Concat(
                        $"update CustomShipment ",
                        $"set	{buildCustomsUpdatedFields} ",
                        $"from	 CargoTrackingShipments CustomShipment ",
                        $"join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId");
            return sql;
        }
        private string BuildScriptForUpdatingForwardingShipmentMilstonesFromCustoms()
        {
            string fieldsSetScript = BuildForwardingUpdatedFieldsFromCustom();

            var sql = $"update ForwardingShipment " +
                        $"set	{fieldsSetScript} " +
                        $"from	 CargoTrackingShipments ForwardingShipment " +
                        $"join	CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId";
            return sql;
        }

        private string BuildScriptToSetCurrentMistones()
        {
            var lastForwardingMilstoneOrderNumber = 5;
            var sql = string.Concat(
                "update  ForwardingShipment    "
                , "set		ForwardingShipment.CurrentMilestoneCode = iif(cast(ForwardingMilestones.Weight as int) > cast(OrderMilestones.Weight as int), ForwardingShipment.CurrentMilestoneCode ,OrderShipment.CurrentMilestoneCode),  ForwardingShipment.CurrentMilestoneDate = iif(cast(ForwardingMilestones.Weight as int) > cast(OrderMilestones.Weight as int), ForwardingShipment.CurrentMilestoneDate ,OrderShipment.CurrentMilestoneDate)    "
                , "from	CargoTrackingShipments ForwardingShipment join CargoTrackingShipments OrderShipment on ForwardingShipment.EntityId = OrderShipment.ForwardingShipmentHeaderId  "
                , "	left join CargoTrackingMilestones ForwardingMilestones on ForwardingShipment.CurrentMilestoneCode = ForwardingMilestones.Code "
                , "left join CargoTrackingMilestones OrderMilestones on OrderShipment.CurrentMilestoneCode = OrderMilestones.Code  "
                , "where	ForwardingShipment.CurrentMilestoneCode is not null and OrderShipment.CurrentMilestoneCode is not null and OrderShipment.EntityType='O'  "
                , Environment.NewLine
                , " update  OrderShipment   "
                , " set OrderShipment.CurrentMilestoneCode = iif(cast(ForwardingMilestones.Weight as int) > cast(OrderMilestones.Weight as int), ForwardingShipment.CurrentMilestoneCode, OrderShipment.CurrentMilestoneCode), OrderShipment.CurrentMilestoneDate = iif(cast(ForwardingMilestones.Weight as int) > cast(OrderMilestones.Weight as int), ForwardingShipment.CurrentMilestoneDate, OrderShipment.CurrentMilestoneDate)    "
                , " from	CargoTrackingShipments OrderShipment  join CargoTrackingShipments ForwardingShipment on OrderShipment.ForwardingShipmentHeaderId = ForwardingShipment.EntityId "
                , "	left join CargoTrackingMilestones ForwardingMilestones on ForwardingShipment.CurrentMilestoneCode = ForwardingMilestones.Code "
                , "left join CargoTrackingMilestones OrderMilestones on OrderShipment.CurrentMilestoneCode = OrderMilestones.Code  "
                , " where	OrderShipment.EntityType='O' and (ForwardingShipment.CurrentMilestoneCode is not null and OrderShipment.CurrentMilestoneCode is not null)  "
                , Environment.NewLine
                , "update  ForwardingShipment    "
                , "set		ForwardingShipment.CurrentMilestoneCode = iif(cast(ForwardingMilestones.Weight as int) > cast(CustomMilestones.Weight as int), ForwardingShipment.CurrentMilestoneCode ,CustomShipment.CurrentMilestoneCode),  ForwardingShipment.CurrentMilestoneDate = iif(cast(ForwardingMilestones.Weight as int) > cast(CustomMilestones.Weight as int), ForwardingShipment.CurrentMilestoneDate ,CustomShipment.CurrentMilestoneDate)    "
                , "from	CargoTrackingShipments ForwardingShipment join CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId  "
                , "left join CargoTrackingMilestones ForwardingMilestones on ForwardingShipment.CurrentMilestoneCode = ForwardingMilestones.Code "
                , "left join CargoTrackingMilestones CustomMilestones on CustomShipment.CurrentMilestoneCode = CustomMilestones.Code  "
                , "where	ForwardingShipment.CurrentMilestoneCode is not null and CustomShipment.CurrentMilestoneCode is not null  "
                , Environment.NewLine
                , " update  CustomShipment   "
                , " set     CustomShipment.CurrentMilestoneCode = ForwardingShipment.CurrentMilestoneCode,  CustomShipment.CurrentMilestoneDate = ForwardingShipment.CurrentMilestoneDate    "
                , " from	CargoTrackingShipments CustomShipment  join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId  "
                , "where	ForwardingShipment.CurrentMilestoneCode is not null and CustomShipment.CurrentMilestoneCode is not null  "
                , Environment.NewLine

                // order
                , "update  ForwardingShipment    "
                , "set		ForwardingShipment.CurrentMilestoneCode = iif(ForwardingShipment.CurrentMilestoneCode is not null, ForwardingShipment.CurrentMilestoneCode ,OrderShipment.CurrentMilestoneCode),    "
                , "		ForwardingShipment.CurrentMilestoneDate = iif(ForwardingShipment.CurrentMilestoneCode is not null, ForwardingShipment.CurrentMilestoneDate ,OrderShipment.CurrentMilestoneDate)    "
                , "from	CargoTrackingShipments ForwardingShipment join CargoTrackingShipments OrderShipment on OrderShipment.ForwardingShipmentHeaderId = ForwardingShipment.EntityId  "
                , " where	OrderShipment.EntityType='O' and (ForwardingShipment.CurrentMilestoneCode is null and OrderShipment.CurrentMilestoneCode is not null)  or (ForwardingShipment.CurrentMilestoneCode is not null and OrderShipment.CurrentMilestoneCode is null)   "
                , Environment.NewLine
                , " update  OrderShipment   "
                , " set     OrderShipment.CurrentMilestoneCode = ForwardingShipment.CurrentMilestoneCode,  OrderShipment.CurrentMilestoneDate = ForwardingShipment.CurrentMilestoneDate    "
                , " from	CargoTrackingShipments OrderShipment  join CargoTrackingShipments ForwardingShipment on OrderShipment.ForwardingShipmentHeaderId = ForwardingShipment.EntityId  "
                , " where	OrderShipment.EntityType='O' and (ForwardingShipment.CurrentMilestoneCode is null and OrderShipment.CurrentMilestoneCode is not null)  or (ForwardingShipment.CurrentMilestoneCode is not null and OrderShipment.CurrentMilestoneCode is null)  "
                , Environment.NewLine

                , "update  ForwardingShipment    "
                , "set		ForwardingShipment.CurrentMilestoneCode = iif(ForwardingShipment.CurrentMilestoneCode is not null, ForwardingShipment.CurrentMilestoneCode ,CustomShipment.CurrentMilestoneCode),    "
                , "		ForwardingShipment.CurrentMilestoneDate = iif(ForwardingShipment.CurrentMilestoneCode is not null, ForwardingShipment.CurrentMilestoneDate ,CustomShipment.CurrentMilestoneDate)    "
                , "from	CargoTrackingShipments ForwardingShipment join CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId  "
                , "where	(ForwardingShipment.CurrentMilestoneCode is null and CustomShipment.CurrentMilestoneCode is not null)  or (ForwardingShipment.CurrentMilestoneCode is not null and CustomShipment.CurrentMilestoneCode is null)   "
                , Environment.NewLine
                , " update  CustomShipment   "
                , " set     CustomShipment.CurrentMilestoneCode = ForwardingShipment.CurrentMilestoneCode,  CustomShipment.CurrentMilestoneDate = ForwardingShipment.CurrentMilestoneDate    "
                , " from	CargoTrackingShipments CustomShipment  join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId  "
                , "where	(ForwardingShipment.CurrentMilestoneCode is null and CustomShipment.CurrentMilestoneCode is not null)  or (ForwardingShipment.CurrentMilestoneCode is not null and CustomShipment.CurrentMilestoneCode is null)  "

                );

            return sql;
        }
        private string DisconnectShipments(CargoTrackingArgs cargoArgs)
        {
            string disconnectedShipmentsIds = GetDisconnectedShipmentsIds(cargoArgs);

            if (disconnectedShipmentsIds.Length == 0)
                return null;

            string script =
                $"update shipments  " +
                $"set LastUpdateDate = GETDATE() " +
                $"where id in ({disconnectedShipmentsIds}) ";

            return script;
        }

        private static string GetDisconnectedShipmentsIds(CargoTrackingArgs cargoArgs)
        {
            var dataTable = new DataTable();
            string query = "IF OBJECT_ID(N'dbo.OldCargoShipments', N'U') IS NOT NULL " + Environment.NewLine +
                "select	old.ForwardingShipmentHeaderId " +
                            "from CargoTrackingShipments shipment join OldCargoShipments old on shipment.EntityId = old.EntityId " +
                            "where shipment.ForwardingShipmentHeaderId is null and old.ForwardingShipmentHeaderId is not null " +
                            "and shipment.EntityType = 'O'";
            SqlConnection conn = new SqlConnection(cargoArgs.DestinationConnectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();


            var disconnectedShipments = dataTable.AsEnumerable().Select(d => "'" + d.ItemArray[0] + "'").ToList();

            var disconnectedShipmentsIds = string.Join(",", disconnectedShipments);
            return disconnectedShipmentsIds;
        }

        private string BuildForwardingUpdatedFieldsFromCustom()
        {
            List<string> fieldsAssignments = new List<string>();

            AppendForwardingFieldAssignmentScriptFromCustom(fieldsAssignments);

            AppendForwardingAssignmentFromForwardingWhenCustomsIsEmpty(fieldsAssignments);

            var fieldsSetScript = ConvertListOfStringsToCommaSeperatedString(fieldsAssignments);
            return fieldsSetScript;
        }

        // order
        private string BuildForwardingUpdatedFieldsFromOrder()
        {
            List<string> fieldsAssignments = new List<string>();

            AppendForwardingFieldAssignmentScriptFromOrder(fieldsAssignments);

            AppendForwardingAssignmentFromForwardingWhenOrderIsEmpty(fieldsAssignments);

            var fieldsSetScript = ConvertListOfStringsToCommaSeperatedString(fieldsAssignments);
            return fieldsSetScript;
        }

        private void ExcuteSqlScript(CargoTrackingArgs cargoArgs, string sql)
        {
            ServiceHelper.ExecuteSql(sql, cargoArgs.DestinationConnectionString);
        }
        private void ExcuteSqlScriptForSourceDatabase(CargoTrackingArgs cargoArgs, string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return;

            ServiceHelper.ExecuteSql(sql, cargoArgs.SourceConnectionString);
        }


        private string BuildCustomsUpdatedFields()
        {
            List<string> fieldsAssignments = new List<string>();

            AppendCustomsFieldAssignmentScript(fieldsAssignments);

            AppendCustomsAssignmentFromForwardingWhenCustomsIsEmpty(fieldsAssignments);

            return ConvertListOfStringsToCommaSeperatedString(fieldsAssignments);
        }

        private string BuildOrderUpdatedFields()
        {
            List<string> fieldsAssignments = new List<string>();

            AppendOrderFieldsAssignmentScript(fieldsAssignments);

            AppendOrderAssignmentFromForwardingWhenOrderIsEmpty(fieldsAssignments);

            return ConvertListOfStringsToCommaSeperatedString(fieldsAssignments);
        }

        private string ConvertListOfStringsToCommaSeperatedString(List<string> listOfStrings)
        {
            return string.Join(", ", listOfStrings);
        }
        private void AppendOrderAssignmentFromForwardingWhenOrderIsEmpty(List<string> fieldsAssignments)
        {
            foreach (var field in orderMilstonesFields)
                fieldsAssignments.Add(BuildOrderAssignmentFromForwardingWhenCustomsIsEmpty(field));
        }

        private void AppendCustomsAssignmentFromForwardingWhenCustomsIsEmpty(List<string> fieldsAssignments)
        {
            foreach (var field in customsMilstonesFields)
                fieldsAssignments.Add(BuildCustomsAssignmentFromForwardingWhenCustomsIsEmpty(field));
        }

        private void AppendOrderFieldsAssignmentScript(List<string> fieldsAssignments)
        {
            foreach (var field in forwardingMilstonesFields.Where(d => !orderMilstonesFields.Contains(d)))
                fieldsAssignments.Add(BuildOrderFieldAssignmentScript(field));
        }
        private void AppendCustomsFieldAssignmentScript(List<string> fieldsAssignments)
        {
            foreach (var field in forwardingMilstonesFields)
                fieldsAssignments.Add(BuildCustomsFieldAssignmentScript(field));
            var list = new List<string>()
            {
                "BookingDate",
                "BookingDone",
                "BookingNotes",
                "BookingEstimationDate"
            };
            foreach (var field in list)
                fieldsAssignments.Add(BuildCustomsFieldAssignmentScript(field));
        }
        private void AppendForwardingAssignmentFromForwardingWhenCustomsIsEmpty(List<string> fieldsAssignments)
        {
            foreach (var field in forwardingMilstonesFields)
                fieldsAssignments.Add(BuildForwardingAssignmentFromForwardingWhenCustomsIsEmpty(field));
        }
        private void AppendForwardingAssignmentFromForwardingWhenOrderIsEmpty(List<string> fieldsAssignments)
        {
            foreach (var field in forwardingMilstonesFields.Where(d => !orderMilstonesFields.Contains(d)))
                fieldsAssignments.Add(BuildForwardingAssignmentFromForwardingWhenOrderIsEmpty(field));
        }
        private void AppendForwardingFieldAssignmentScriptFromCustom(List<string> fieldsAssignments)
        {
            foreach (var field in customsMilstonesFields)
                fieldsAssignments.Add(BuildForwardingFieldAssignmentScript(field));
        }
        private void AppendForwardingFieldAssignmentScriptFromOrder(List<string> fieldsAssignments)
        {
            foreach (var field in orderMilstonesFields)
                fieldsAssignments.Add(BuildForwardingFieldAssignmentScriptFromOrder(field));
        }

        private string BuildForwardingFieldAssignmentScript(string fieldName)
        {
            var compareOperator = "is not null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 1";

            return $"ForwardingShipment.{fieldName} = iif(CustomShipment.{fieldName} {compareOperator}, CustomShipment.{fieldName}, ForwardingShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildForwardingFieldAssignmentScriptFromOrder(string fieldName)
        {
            var compareOperator = "is not null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 1";

            return $"ForwardingShipment.{fieldName} = iif(OrderShipment.{fieldName} {compareOperator}, OrderShipment.{fieldName}, ForwardingShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildForwardingAssignmentFromForwardingWhenCustomsIsEmpty(string fieldName)
        {
            var compareOperator = "is null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 0";

            return $"ForwardingShipment.{fieldName} = iif(ForwardingShipment.{fieldName} {compareOperator},CustomShipment.{fieldName},ForwardingShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildForwardingAssignmentFromForwardingWhenOrderIsEmpty(string fieldName)
        {
            var compareOperator = "is null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 0";

            return $"ForwardingShipment.{fieldName} = iif(ForwardingShipment.{fieldName} {compareOperator},OrderShipment.{fieldName},ForwardingShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildOrderAssignmentFromForwardingWhenCustomsIsEmpty(string fieldName)
        {
            var compareOperator = "is null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 0";

            return $"OrderShipment.{fieldName} = iif(OrderShipment.{fieldName} {compareOperator},ForwardingShipment.{fieldName},OrderShipment.{fieldName})" + Environment.NewLine;
        }

        private string BuildCustomsAssignmentFromForwardingWhenCustomsIsEmpty(string fieldName)
        {
            var compareOperator = "is null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 0";

            return $"CustomShipment.{fieldName} = iif(CustomShipment.{fieldName} {compareOperator},ForwardingShipment.{fieldName},CustomShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildCustomsFieldAssignmentScript(string fieldName)
        {
            var compareOperator = "is not null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 1";

            return $"CustomShipment.{fieldName} = iif(ForwardingShipment.{fieldName} {compareOperator}, ForwardingShipment.{fieldName}, CustomShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildOrderFieldAssignmentScript(string fieldName)
        {
            var compareOperator = "is not null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 1";

            return $"OrderShipment.{fieldName} = iif(ForwardingShipment.{fieldName} {compareOperator}, ForwardingShipment.{fieldName}, OrderShipment.{fieldName})" + Environment.NewLine;
        }


    }
}