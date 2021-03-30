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
        }
        private void FillMilstonesFieldsList()
        {
            FillForwardingMilstonesFieldsList();
            FillCustomsMilstonesFieldsList();
        }
        private string BuildScriptForUpdatingCustomsShipmentMilstones()
        {
            string buildCustomsUpdatedFields = this.BuildCustomsUpdatedFields();

            var sql = string.Concat(
                        $"update CustomShipment ",
                        $"set	{buildCustomsUpdatedFields} ",
                        $"from	 CargoTrackingShipments CustomShipment ",
                        $"join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId");
            return sql;
        }
        private string BuildScriptForUpdatingForwardingShipmentMilstones()
        {
			List<string> fieldsAssignments = new List<string>();

            foreach (var field in customsMilstonesFields)
				fieldsAssignments.Add(CreateSetForwardingFieldScript(field));

			var fieldsSetScript = string.Join(", ", fieldsAssignments);

            var sql = $"update ForwardingShipment " +
						$"set	{fieldsSetScript} " +
						$"from	 CargoTrackingShipments ForwardingShipment " +
                        $"join	CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId";
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

            var fieldsSetScript = string.Join(", ", fieldsAssignments);
            return fieldsSetScript;
		}

        private string CreateSetForwardingFieldScript(string fieldName)
        {
			string fromShipment = "CustomShipment";
			string toShipment = "ForwardingShipment";
			return $"{toShipment}.{fieldName} = iif({fromShipment}.{fieldName} is not null, {fromShipment}.{fieldName}, {toShipment}.{fieldName})";
        }
		private string CreateSetCustomsFieldScript(string fieldName)
		{
			string fromShipment = "ForwardingShipment";
			string toShipment = "CustomShipment";
			return $"{toShipment}.{fieldName} = iif({fromShipment}.{fieldName} is not null, {fromShipment}.{fieldName}, {toShipment}.{fieldName})";
		}

    }
}


