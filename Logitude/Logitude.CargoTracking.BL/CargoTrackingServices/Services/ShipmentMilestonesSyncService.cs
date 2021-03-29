using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class ShipmentMilestonesSyncService
    {
		List<string> shipmentFieldsToCopyFromForwardingToCustoms = new List<string>();
        List<string> shipmentFieldsToCopyFromCustomsToForwarding = new List<string>();

        private void DeclareFields()
        {
            SetShipmentFieldsToCopyFromForwardingToCustoms();
            SetShipmentFieldsToCopyFromCustomsToForwarding();
        }
		public void SyncShipmentMilstones()
        {
            DeclareFields();

            CreateScriptForUpdatingCustomsShipmentMilstones();
            CreateScriptForUpdatingForwardingShipmentMilstones();
        }

		private void SetShipmentFieldsToCopyFromForwardingToCustoms()
        {
            shipmentFieldsToCopyFromForwardingToCustoms.Add("PickupDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("PickupDone");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("PickupEstimationDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("FromWarehouseDone");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("FromWarehouseDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("FromWarehouseEstimationDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("DepartureDone");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("DepartureDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("DepartureEstimationDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("ArrivalDone");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("ArrivalDate");
            shipmentFieldsToCopyFromForwardingToCustoms.Add("ArrivalEstimationDate");

        }

        private void SetShipmentFieldsToCopyFromCustomsToForwarding()
        {
            shipmentFieldsToCopyFromCustomsToForwarding.Add("ToWarehouseDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("ToWarehouseDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("ToWarehouseEstimationDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("ToWarehouseNotes");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedCustomsAgentDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedCustomsAgentDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedCustomsAgentEstDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedCustomsAgentNotes");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("CustomsPaymentDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("CustomsPaymentDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("ClearanceDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("ClearanceDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedTruckerDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedTruckerDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedTruckerEstimationDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("AssignedTruckerNotes");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveryDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveryDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveryEstimationDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveryNotes");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveryExceptionReason");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveredDone");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveredDate");
            shipmentFieldsToCopyFromCustomsToForwarding.Add("DeliveredEstimationDate");
        }

        private string CreateScriptForUpdatingForwardingShipmentMilstones()
        {
			List<string> fieldsAssignments = new List<string>();

            foreach (var field in shipmentFieldsToCopyFromCustomsToForwarding)
				fieldsAssignments.Add(CreateSetForwardingFieldScript(field));

			var fieldsSetScript = string.Join(", ", fieldsAssignments);

			var sql =	$"update ForwardingShipment " +
						$"set	{fieldsSetScript} " +
						$"from	 CargoTrackingShipments ForwardingShipment " +
                        $"join	CargoTrackingShipments CustomShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId";
			return sql;
        }

		private string CreateScriptForUpdatingCustomsShipmentMilstones()
		{
            List<string> fieldsAssignments = new List<string>();

            foreach (var field in shipmentFieldsToCopyFromForwardingToCustoms)
                fieldsAssignments.Add(CreateSetCustomsFieldScript(field));

            var fieldsSetScript = string.Join(", ", fieldsAssignments);

			var sql = $"update CustomShipment " +
						$"set	{fieldsSetScript} " +
						$"from	 CargoTrackingShipments CustomShipment " +
                        $"join CargoTrackingShipments ForwardingShipment on ForwardingShipment.CustomsShipmentHeaderId = CustomShipment.EntityId";
			return sql;

		}

		private static string CreateSetForwardingFieldScript(string fieldName)
        {
			string fromShipment = "CustomShipment";
			string toShipment = "ForwardingShipment";
			return $"{toShipment}.{fieldName} = iif({fromShipment}.{fieldName} is not null, {fromShipment}.{fieldName}, {toShipment}.{fieldName})";
        }
		private static string CreateSetCustomsFieldScript(string fieldName)
		{
			string fromShipment = "ForwardingShipment";
			string toShipment = "CustomShipment";
			return $"{toShipment}.{fieldName} = iif({fromShipment}.{fieldName} is not null, {fromShipment}.{fieldName}, {toShipment}.{fieldName})";
		}

    }
}


