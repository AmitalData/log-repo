using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Linq;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        string pickedUpEventCode = "PICD";
        string deliveryArrivedEventCode = "DEAR";
        string partialDeliveredStatus = "PSDL";
        string partialPickupStatus = "PSHP";
        string  pickedUpArrivedCode = "RCS";

        private bool IsAllowingPartial(string entityStatusId)
        {
            if (string.IsNullOrEmpty(entityStatusId))
            {
                return false;
            }
            EntityStatus eventStatus = EntityStatusRepository.GetSingleEntityStatus(entityStatusId, tenant, true);
            if (eventStatus == null)
            {
                return false;
            }

            if (!eventStatus.AllowPartial && eventStatus.Code != partialDeliveredStatus && eventStatus.Code != partialPickupStatus)
            {
                return false;
            }
            return true;
        }


        private string ComputePartialStatusAmountShipmentPickUps()
        {
            string partialStatusAmount = null;
            var shipmentPickUpsCount = entityPM.ShipmentPickUps?.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete).Count();
            if (shipmentPickUpsCount == 0)
            {
                return null;
            }

            var pickUpsWithATACount = entityPM.ShipmentPickUps?.Where(d => d.ATA == null && d.ChangeSetOp != ChangeSetOperation.Delete).Count();
            if (pickUpsWithATACount == 0)
            {
                return null;
            }

            var shipmentPickUpsNotEmptyATDCount = entityPM.ShipmentPickUps.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete && a.ATD != null).Count();
            if (shipmentPickUpsNotEmptyATDCount == 0 || shipmentPickUpsNotEmptyATDCount == shipmentPickUpsCount)
            {
                return null;
            }

            partialStatusAmount = shipmentPickUpsNotEmptyATDCount + "/" + shipmentPickUpsCount;
            return partialStatusAmount;
        }
        private string ComputePartialStatusAmountShipmentDeliveries()
        {
            string partialStatusAmount = null;
            var shipmentDeliveriesCount = entityPM.ShipmentDeliveries?.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete).Count();
            if (shipmentDeliveriesCount == 0)
            {
                return null;
            }

            var shipmentDeliveriesNotEmptyATDCount = entityPM.ShipmentDeliveries.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete && a.ATA != null).Count();
            if (shipmentDeliveriesNotEmptyATDCount ==0 || shipmentDeliveriesNotEmptyATDCount == shipmentDeliveriesCount)
            {
                return null;
            }

            partialStatusAmount = shipmentDeliveriesNotEmptyATDCount + "/" + shipmentDeliveriesCount;
            return partialStatusAmount;
        }
        private bool IsCheckThePreviousEvent(ShipmentPM entityPM, EventType eventType)
        {
            if (eventType?.Code != deliveryArrivedEventCode && eventType?.Code != pickedUpEventCode)
            {
                return true;
            }
            if (eventType?.Code == pickedUpEventCode)
            {
                var isPickUpExist = entityPM.ShipmentPickUps.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete && a.ATD != null).Any();
                return isPickUpExist ? false : true;
            }
            if (eventType?.Code == deliveryArrivedEventCode)
            {
                var isDeliveryExist = entityPM.ShipmentDeliveries.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete && a.ATA != null).Any();
                return isDeliveryExist ? false : true;
            }
            return true;
        }

        private void HandlePickUpDeliveryPreviousEvent(EventType eventType, EntityStatus currentEventEntityStatus)
        {
            var isPickUpDeliveryPreviousEvent = false;
            if (eventType?.Code == pickedUpEventCode)
            {
                isPickUpDeliveryPreviousEvent = entityPM.ShipmentPickUps.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete && a.ATD != null).Any();
            }

            if (eventType?.Code == deliveryArrivedEventCode)
            {
                isPickUpDeliveryPreviousEvent = entityPM.ShipmentDeliveries.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete && a.ATA != null).Any();
            }
            
            if (isPickUpDeliveryPreviousEvent || currentEventEntityStatus.Code == partialPickupStatus || currentEventEntityStatus.Code == partialDeliveredStatus)
            {
                ComputePartialStatusAmount(eventType.Code);
            }
        }
        private void ComputePartialStatusAmount(string eventTypeCode)
        {
            string partialStatusAmount = null;
            if (eventTypeCode == pickedUpEventCode)
            {
                partialStatusAmount = ComputePartialStatusAmountShipmentPickUps();
            }

            if (eventTypeCode == deliveryArrivedEventCode)
            {
                partialStatusAmount = ComputePartialStatusAmountShipmentDeliveries();
            }

            entityPM.PartialStatusAmount = partialStatusAmount;
            entityPoco.PartialStatusAmount = entityPM.PartialStatusAmount;
            if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
            {
                entityMasterData.PartialStatusAmount = entityPM.PartialStatusAmount;
            }

            this.ComputePartialStatusId(partialStatusAmount, eventTypeCode);
        }
        private void ComputePartialStatusId(string partialStatusAmount, string eventTypeCode)
        {
            EntityStatus partiallyEntityStatus = null;
            var deliveredStatus = "SDLD";
            var pickupStatus = "SHPK";

            if (eventTypeCode == pickedUpEventCode)
            {
                partiallyEntityStatus = string.IsNullOrEmpty(partialStatusAmount) ? allEntityStatuses.Where(d => d.Code == pickupStatus).FirstOrDefault() : allEntityStatuses.Where(d => d.Code == partialPickupStatus).FirstOrDefault();
            }

            if (eventTypeCode == deliveryArrivedEventCode)
            {
                partiallyEntityStatus = string.IsNullOrEmpty(partialStatusAmount) ? allEntityStatuses.Where(d => d.Code == deliveredStatus).FirstOrDefault() : allEntityStatuses.Where(d => d.Code == partialDeliveredStatus).FirstOrDefault();
            }

            if (partiallyEntityStatus == null)
            {
                return;
            }
            entityPM.StatusId = partiallyEntityStatus?.Id;
            entityPoco.StatusId = entityPM.StatusId;
            if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
            {
                entityMasterData.StatusId = entityPM.StatusId;
            }
        }
    }
}
