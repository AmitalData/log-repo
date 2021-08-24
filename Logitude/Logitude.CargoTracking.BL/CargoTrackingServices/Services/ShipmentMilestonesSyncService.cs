using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class ShipmentMilestonesSyncService
    {
		List<string> forwardingMilstonesFields = new List<string>();
        List<string> customsMilstonesFields = new List<string>();

       
		public void SyncShipmentMilstones(CargoTrackingArgs cargoArgs)
        {
            FillMilstonesFieldsList();

            var sql = BuildScriptForUpdatingCustomsShipmentMilstones();
            ExcuteSqlScript(cargoArgs, sql);

            sql = BuildScriptForUpdatingForwardingShipmentMilstones();
            ExcuteSqlScript(cargoArgs, sql);

            sql = BuildScriptToSetCurrentMistones();
            ExcuteSqlScript(cargoArgs, sql);
        }
        private void FillMilstonesFieldsList()
        {
            FillForwardingMilstonesFieldsList();
            FillCustomsMilstonesFieldsList();
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
        private string BuildScriptForUpdatingForwardingShipmentMilstones()
        {
            string fieldsSetScript = BuildForwardingUpdatedFields();

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
                , "set		ForwardingShipment.CurrentMilestoneCode = iif(cast(ForwardingShipment.CurrentMilestoneCode as int) > cast(CustomShipment.CurrentMilestoneCode as int), ForwardingShipment.CurrentMilestoneCode ,CustomShipment.CurrentMilestoneCode),  ForwardingShipment.CurrentMilestoneDate = iif(cast(ForwardingShipment.CurrentMilestoneCode as int) > cast(CustomShipment.CurrentMilestoneCode as int), ForwardingShipment.CurrentMilestoneDate ,CustomShipment.CurrentMilestoneDate)    "
                , "from	CargoTrackingShipments ForwardingShipment join CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId  "
                , "where	ForwardingShipment.CurrentMilestoneCode is not null and CustomShipment.CurrentMilestoneCode is not null  "
                , Environment.NewLine
                , " update  CustomShipment   "
                , " set     CustomShipment.CurrentMilestoneCode = ForwardingShipment.CurrentMilestoneCode,  CustomShipment.CurrentMilestoneDate = ForwardingShipment.CurrentMilestoneDate    "
                , " from	CargoTrackingShipments CustomShipment  join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId  "
                , "where	ForwardingShipment.CurrentMilestoneCode is not null and CustomShipment.CurrentMilestoneCode is not null  "
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
        private string BuildForwardingUpdatedFields()
        {
            List<string> fieldsAssignments = new List<string>();

            AppendForwardingFieldAssignmentScript(fieldsAssignments);

            AppendForwardingAssignmentFromForwardingWhenCustomsIsEmpty(fieldsAssignments);

            var fieldsSetScript = ConvertListOfStringsToCommaSeperatedString(fieldsAssignments);
            return fieldsSetScript;
        }

        private void ExcuteSqlScript(CargoTrackingArgs cargoArgs, string sql)
        {
            ServiceHelper.ExecuteSql(sql, cargoArgs.DestinationConnectionString);
        }
     


        private void FillForwardingMilstonesFieldsList()
        {
            forwardingMilstonesFields.Add("PickupDate");
            forwardingMilstonesFields.Add("PickupDone");
            forwardingMilstonesFields.Add("PickupEstimationDate");
            forwardingMilstonesFields.Add("FromWarehouseDone");
            forwardingMilstonesFields.Add("FromWarehouseDate");
            forwardingMilstonesFields.Add("FromWarehouseEstimationDate");
            forwardingMilstonesFields.Add("FromWarehouseNotes");
            forwardingMilstonesFields.Add("DepartureDone");
            forwardingMilstonesFields.Add("DepartureDate");
            forwardingMilstonesFields.Add("DepartureEstimationDate");
            forwardingMilstonesFields.Add("ArrivalDone");
            forwardingMilstonesFields.Add("ArrivalDate");
            forwardingMilstonesFields.Add("ArrivalEstimationDate");

        }

        private void FillCustomsMilstonesFieldsList()
        {
            customsMilstonesFields.Add("ToWarehouseDone");
            customsMilstonesFields.Add("ToWarehouseDate");
            customsMilstonesFields.Add("ToWarehouseEstimationDate");
            customsMilstonesFields.Add("ToWarehouseNotes");
            customsMilstonesFields.Add("AssignedCustomsAgentDone");
            customsMilstonesFields.Add("AssignedCustomsAgentDate");
            customsMilstonesFields.Add("AssignedCustomsAgentEstDate");
            customsMilstonesFields.Add("AssignedCustomsAgentNotes");
            customsMilstonesFields.Add("CustomsPaymentDone");
            customsMilstonesFields.Add("CustomsPaymentDate");
            customsMilstonesFields.Add("ClearanceDone");
            customsMilstonesFields.Add("ClearanceDate");
            customsMilstonesFields.Add("AssignedTruckerDone");
            customsMilstonesFields.Add("AssignedTruckerDate");
            customsMilstonesFields.Add("AssignedTruckerEstimationDate");
            customsMilstonesFields.Add("AssignedTruckerNotes");
            customsMilstonesFields.Add("DeliveryDone");
            customsMilstonesFields.Add("DeliveryDate");
            customsMilstonesFields.Add("DeliveryEstimationDate");
            customsMilstonesFields.Add("DeliveryNotes");
            customsMilstonesFields.Add("DeliveryExceptionReason");
            customsMilstonesFields.Add("DeliveredDone");
            customsMilstonesFields.Add("DeliveredDate");
            customsMilstonesFields.Add("DeliveredEstimationDate");
            customsMilstonesFields.Add("GatepassArrivedDone");
            customsMilstonesFields.Add("GatepassArrivedDate");
            customsMilstonesFields.Add("GatepassArrivedEstDate");
            customsMilstonesFields.Add("GatepassArrivedNotes");
            customsMilstonesFields.Add("DocumentInspectionDate");
            customsMilstonesFields.Add("DocumentInspectionEstDate");
            customsMilstonesFields.Add("DocumentInspectionDone");
            customsMilstonesFields.Add("DocumentInspectionNotes");
            customsMilstonesFields.Add("GoodsClassificationDate");
            customsMilstonesFields.Add("GoodsClassificationDone");
            customsMilstonesFields.Add("GoodsClassificationNotes");
            customsMilstonesFields.Add("GoodsClassificationEstDate");

        }

        private string BuildCustomsUpdatedFields()
        {
            List<string> fieldsAssignments = new List<string>();

            AppendCustomsFieldAssignmentScript(fieldsAssignments);

            AppendCustomsAssignmentFromForwardingWhenCustomsIsEmpty(fieldsAssignments);

            return ConvertListOfStringsToCommaSeperatedString(fieldsAssignments);
        }

        private string ConvertListOfStringsToCommaSeperatedString(List<string> listOfStrings)
        {
            return string.Join(", ", listOfStrings);
        }

        private void AppendCustomsAssignmentFromForwardingWhenCustomsIsEmpty(List<string> fieldsAssignments)
        {
            foreach (var field in customsMilstonesFields)
                fieldsAssignments.Add(BuildCustomsAssignmentFromForwardingWhenCustomsIsEmpty(field));
        }

        private void AppendCustomsFieldAssignmentScript(List<string> fieldsAssignments)
        {
            foreach (var field in forwardingMilstonesFields)
                fieldsAssignments.Add(BuildCustomsFieldAssignmentScript(field));
        }
        private void AppendForwardingAssignmentFromForwardingWhenCustomsIsEmpty(List<string> fieldsAssignments)
        {
            foreach (var field in forwardingMilstonesFields)
                fieldsAssignments.Add(BuildForwardingAssignmentFromForwardingWhenCustomsIsEmpty(field));
        }

        private void AppendForwardingFieldAssignmentScript(List<string> fieldsAssignments)
        {
            foreach (var field in customsMilstonesFields)
                fieldsAssignments.Add(BuildForwardingFieldAssignmentScript(field));
        }


        private string BuildForwardingFieldAssignmentScript(string fieldName)
        {
            var compareOperator = "is not null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 1";

            return $"ForwardingShipment.{fieldName} = iif(CustomShipment.{fieldName} {compareOperator}, CustomShipment.{fieldName}, ForwardingShipment.{fieldName})" + Environment.NewLine;
        }
        private string BuildForwardingAssignmentFromForwardingWhenCustomsIsEmpty(string fieldName)
        {
            var compareOperator = "is null";

            if (fieldName.Contains("Done"))
                compareOperator = " = 0";

            return $"ForwardingShipment.{fieldName} = iif(ForwardingShipment.{fieldName} {compareOperator},CustomShipment.{fieldName},ForwardingShipment.{fieldName})" + Environment.NewLine;
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

        

    }
}