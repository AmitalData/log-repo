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

        private string BuildForwardingUpdatedFields()
        {
            List<string> fieldsAssignments = new List<string>();

            foreach (var field in customsMilstonesFields)
                fieldsAssignments.Add(CreateSetForwardingFieldFromCustomScript(field));

            foreach (var field in forwardingMilstonesFields)
                fieldsAssignments.Add(GetFieldFromCustomsIfForwardingIsEmpty(field));

            var fieldsSetScript = string.Join(", ", fieldsAssignments);
            return fieldsSetScript;
        }

        private string BuildScriptToSetCurrentMistones()
        {
            var lastForwardingMilstoneOrderNumber = 5;
            var sql = string.Concat(

                "update  ForwardingShipment"
                , $"set     ForwardingShipment.CurrentMilestoneCode = iif(ForwardingShipment.CurrentMilestoneCode > {lastForwardingMilstoneOrderNumber}, CustomShipment.CurrentMilestoneCode, ForwardingShipment.CurrentMilestoneCode),"
                , $"        ForwardingShipment.CurrentMilestoneDate = iif(ForwardingShipment.CurrentMilestoneCode > {lastForwardingMilstoneOrderNumber}, CustomShipment.CurrentMilestoneDate, ForwardingShipment.CurrentMilestoneDate)"
                , "from CargoTrackingShipments ForwardingShipment"
                , "join CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId"
                , Environment.NewLine
                , "update  CustomShipment"
                , $"set     CustomShipment.CurrentMilestoneCode = iif(CustomShipment.CurrentMilestoneCode <= {lastForwardingMilstoneOrderNumber}, ForwardingShipment.CurrentMilestoneCode, CustomShipment.CurrentMilestoneCode),"
                , $"        CustomShipment.CurrentMilestoneDate = iif(CustomShipment.CurrentMilestoneCode <= {lastForwardingMilstoneOrderNumber}, ForwardingShipment.CurrentMilestoneDate, CustomShipment.CurrentMilestoneDate)"
                , "from CargoTrackingShipments CustomShipment"
                , "join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId");

            return sql;
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
        }

        private string BuildCustomsUpdatedFields()
		{
            List<string> fieldsAssignments = new List<string>();

            foreach (var field in forwardingMilstonesFields)
                fieldsAssignments.Add(CreateSetCustomsFieldScript(field));

            foreach (var field in customsMilstonesFields)
                fieldsAssignments.Add(GetFieldFromForwardingIfCustomsIsEmpty(field));

            var fieldsSetScript = string.Join(", ", fieldsAssignments);
            return fieldsSetScript;
		}

        private string CreateSetForwardingFieldFromCustomScript(string fieldName)
        {
			return $"ForwardingShipment.{fieldName} = iif(CustomShipment.{fieldName} is not null, CustomShipment.{fieldName}, ForwardingShipment.{fieldName})";
        }
        private string GetFieldFromCustomsIfForwardingIsEmpty(string fieldName)
        {
            return $"ForwardingShipment.{fieldName} = iif(ForwardingShipment.{fieldName} is null,CustomShipment.{fieldName},ForwardingShipment.{fieldName})";
        }
        private string GetFieldFromForwardingIfCustomsIsEmpty(string fieldName)
        {
            return $"CustomShipment.{fieldName} = iif(CustomShipment.{fieldName} is null,ForwardingShipment.{fieldName},CustomShipment.{fieldName})";
        }
        private string CreateSetCustomsFieldScript(string fieldName)
		{
			string fromShipment = "ForwardingShipment";
			string toShipment = "CustomShipment";
			return $"{toShipment}.{fieldName} = iif({fromShipment}.{fieldName} is not null, {fromShipment}.{fieldName}, {toShipment}.{fieldName})";
		}

    }
}


