using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentPickUpDeliveryValidator
    {
        public static void ValidatePickup(ShipmentPickUpPM pickUpPM, ShipmentPM shipmentPM)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("UNV", shipmentPM.Tenant)) return;

            ValidatePickupFromToFields(pickUpPM);
            ValidateFutureActualDates(pickUpPM.ATD, pickUpPM.ATA, pickUpPM.Tenant);
            ValidateEstimatedLegDates(pickUpPM.ETD, pickUpPM.ETA);
            ValidateActualLegDates(pickUpPM.ATD, pickUpPM.ATA);
            ValidatePickupWithNextLeg(pickUpPM, shipmentPM);
        }
        private static void ValidatePickupFromToFields(ShipmentPickUpPM pickUpPM)
        {
            FromToFields fromFields = BuildFromToFields_PickUp(pickUpPM, true);
            FromToFields toFields = BuildFromToFields_PickUp(pickUpPM, false);

            ValidateFromToFields(fromFields, "From");
            ValidateFromToFields(toFields, "To");
        }
        private static void ValidatePickupWithNextLeg(ShipmentPickUpPM pickUpPM, ShipmentPM shipmentPM)
        {
            if (shipmentPM.WarehouseLegWarehouseId != null && shipmentPM.DirectionId != "I")
            {
                ValidatePickupRegardingWarehouseLeg(pickUpPM, shipmentPM);
            }
            else if (shipmentPM.PreForwardingFromPortId != null && shipmentPM.PreForwardingToPortId != null)
            {
                ValidatePickupRegardingPreForwardingLeg(pickUpPM, shipmentPM);
            }
            else if (shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null)
            {
                ValidatePickupRegardingPreCarriageLeg(pickUpPM, shipmentPM);
            }
            else
            {
                ValidatePickupRegardingMainCarriageLeg(pickUpPM, shipmentPM);
            }
        }
        private static void ValidatePickupRegardingWarehouseLeg(ShipmentPickUpPM pickUpPM, ShipmentPM shipmentPM)
        {
            if (!IsFirstPickup(pickUpPM, shipmentPM)) return;

            if (RoutingDatesValidator.IsDateSeriesBiggerNotEqual(pickUpPM.ETA, shipmentPM.WarehouseLegExpectedEntryDate))
            {
                throw new ApplicationException("Pick up expected arrival must be equal or less than Warehouse expected entry");
            }

            if (RoutingDatesValidator.IsDateSeriesBiggerNotEqual(pickUpPM.ATA, shipmentPM.WarehouseLegActualEntryDate))
            {
                throw new ApplicationException("Pick up actual arrival must be equal or less than Warehouse actual entry");
            }
        }
        private static void ValidatePickupRegardingPreForwardingLeg(ShipmentPickUpPM pickUpPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateBigger(pickUpPM.ETA, shipmentPM.PreForwardingETD))
            {
                throw new ApplicationException("Pick up expected arrival must be less than pre forwarding expected departure");
            }

            if (RoutingDatesValidator.IsDateBigger(pickUpPM.ATA, shipmentPM.PreForwardingATD))
            {
                throw new ApplicationException("Pick up actual arrival must be less than pre forwarding actual departure");
            }
        }
        private static void ValidatePickupRegardingPreCarriageLeg(ShipmentPickUpPM pickUpPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateBigger(pickUpPM.ETA, shipmentPM.PreCarriageETD))
            {
                throw new ApplicationException("Pick up expected arrival must be less than pre carriage expected departure");
            }

            if (RoutingDatesValidator.IsDateBigger(pickUpPM.ATA, shipmentPM.PreCarriageATD))
            {
                throw new ApplicationException("Pick up actual arrival must be less than pre carriage actual departure");
            }
        }
        private static void ValidatePickupRegardingMainCarriageLeg(ShipmentPickUpPM pickUpPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateBigger(pickUpPM.ETA, shipmentPM.MainCarriageETD))
            {
                throw new ApplicationException("Pick up expected arrival must be less than main carriage expected departure");
            }

            if (RoutingDatesValidator.IsDateBigger(pickUpPM.ATA, shipmentPM.MainCarriageATD))
            {
                throw new ApplicationException("Pick up actual arrival must be less than main carriage actual departure");
            }
        }
        private static bool IsFirstPickup(ShipmentPickUpPM itemPM, ShipmentPM shipmentPM)
        {
            ShipmentPickUpPM firstPickup =
                            (from d in shipmentPM.ShipmentPickUps
                             where d.ChangeSetOp != ChangeSetOperation.Delete
                             select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (firstPickup == null)
            {
                return false;
            }
            if (itemPM.PickUpDeliveryNumber == firstPickup.PickUpDeliveryNumber)
            {
                return true;
            }

            return false;
        }

        public static void ValidateDelivery(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("UNV", shipmentPM.Tenant)) return;

            ValidateDeliveryFromFields(deliveryPM);
            ValidateFutureActualDates(deliveryPM.ATD, deliveryPM.ATA, deliveryPM.Tenant);
            ValidateEstimatedLegDates(deliveryPM.ETD, deliveryPM.ETA);
            ValidateActualLegDates(deliveryPM.ATD, deliveryPM.ATA);
            ValidateDeliveryWithPreviousLeg(deliveryPM, shipmentPM);
        }
        private static void ValidateDeliveryFromFields(ShipmentDeliveryPM deliveryPM)
        {
            FromToFields fromFields = BuildFromToFields_Delivery(deliveryPM, true);
            FromToFields toFields = BuildFromToFields_Delivery(deliveryPM, false);

            ValidateFromToFields(fromFields, "From");
            ValidateFromToFields(toFields, "To");
        }
        private static void ValidateDeliveryWithPreviousLeg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (shipmentPM.WarehouseLeg2WarehouseId != null)
            {
                ValidateDeliveryRegardingWarehouse2Leg(deliveryPM, shipmentPM);
            }
            else if (shipmentPM.OnForwardingFromPortId != null && shipmentPM.OnForwardingToPortId != null)
            {
                ValidateDeliveryRegardingOnForwardingLeg(deliveryPM, shipmentPM);
            }
            else if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null)
            {
                ValidateDeliveryRegardingOnCarriageLeg(deliveryPM, shipmentPM);
            }
            else if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null)
            {
                ValidateDeliveryRegardingTransshipment3Leg(deliveryPM, shipmentPM);
            }
            else if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null)
            {
                ValidateDeliveryRegardingTransshipment2Leg(deliveryPM, shipmentPM);
            }
            else if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null)
            {
                ValidateDeliveryRegardingTransshipment1Leg(deliveryPM, shipmentPM);
            }
            else
            {
                ValidateDeliveryRegardingMainCarriageLeg(deliveryPM, shipmentPM);
            }
        }
        private static void ValidateDeliveryRegardingWarehouse2Leg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.WarehouseLeg2ExpectedEntryDate))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than Destination Warehouse expected entry");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.WarehouseLeg2ActualEntryDate))
            {
                throw new ApplicationException("Delivery actual departure must be bigger than Destination Warehouse actual entry");
            }
        }
        private static void ValidateDeliveryRegardingOnForwardingLeg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.OnForwardingETA))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than On-Forwarding expected arrival");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.OnForwardingATA))
            {
                throw new ApplicationException("Delivery actual departure must be bigger than On-Forwarding actual arrival");
            }
        }
        private static void ValidateDeliveryRegardingOnCarriageLeg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.OnCarriageETA))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than On-Carriage expected arrival");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.OnCarriageATA))
            {
                throw new ApplicationException(" Delivery actual departure must be bigger than On-Carriage actual arrival");
            }
        }
        private static void ValidateDeliveryRegardingTransshipment3Leg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.Transshipment3ETA))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than Transshipment3 expected arrival");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.Transshipment3ATA))
            {
                throw new ApplicationException("Delivery actual departure must be bigger than Transshipment3 actual arrival");
            }
        }
        private static void ValidateDeliveryRegardingTransshipment2Leg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.Transshipment2ETA))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than Transshipment2 expected arrival");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.Transshipment2ATA))
            {
                throw new ApplicationException("Delivery actual departure must be bigger than Transshipment2 actual arrival");
            }
        }
        private static void ValidateDeliveryRegardingTransshipment1Leg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.Transshipment1ETA))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than Transshipment1 expected arrival");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.Transshipment1ATA))
            {
                throw new ApplicationException("Delivery actual departure must be bigger than Transshipment1 actual arrival");
            }
        }
        private static void ValidateDeliveryRegardingMainCarriageLeg(ShipmentDeliveryPM deliveryPM, ShipmentPM shipmentPM)
        {
            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ETD, shipmentPM.MainCarriageETA))
            {
                throw new ApplicationException("Delivery expected departure must be bigger than Main-Carriage expected arrival");
            }

            if (RoutingDatesValidator.IsDateSmaller(deliveryPM.ATD, shipmentPM.MainCarriageATA))
            {
                throw new ApplicationException("Delivery actual departure must be bigger than Main-Carriage actual arrival");
            }
        }

        private static FromToFields BuildFromToFields_PickUp(ShipmentPickUpPM pickUpPM, bool isFrom)
        {
            return new FromToFields()
            {
                TypeCode = isFrom ? pickUpPM.PickUpDeliveryFromTypeCode : pickUpPM.PickUpDeliveryToTypeCode,
                PartnerCardId = isFrom ? pickUpPM.FromPartnerCardId : pickUpPM.ToPartnerCardId,
                PortId = isFrom ? pickUpPM.FromPortId : pickUpPM.ToPortId,
                AddressCity = isFrom ? pickUpPM.FromAddressCity : pickUpPM.ToAddressCity,
                AddressZipCode = isFrom ? pickUpPM.FromAddressZipCode : pickUpPM.ToAddressZipCode,
                AddressCountryId = isFrom ? pickUpPM.FromAddressCountryId : pickUpPM.ToAddressCountryId,
            };
        }
        private static FromToFields BuildFromToFields_Delivery(ShipmentDeliveryPM deliveryPM, bool isFrom)
        {
            return new FromToFields()
            {
                TypeCode = isFrom ? deliveryPM.PickUpDeliveryFromTypeCode : deliveryPM.PickUpDeliveryToTypeCode,
                PartnerCardId = isFrom ? deliveryPM.FromPartnerCardId : deliveryPM.ToPartnerCardId,
                PortId = isFrom ? deliveryPM.FromPortId : deliveryPM.ToPortId,
                AddressCity = isFrom ? deliveryPM.FromAddressCity : deliveryPM.ToAddressCity,
                AddressZipCode = isFrom ? deliveryPM.FromAddressZipCode : deliveryPM.ToAddressZipCode,
                AddressCountryId = isFrom ? deliveryPM.FromAddressCountryId : deliveryPM.ToAddressCountryId,
            };
        }
        private static void ValidateFromToFields(FromToFields fromToFields, string indicator)
        {
            switch (fromToFields.TypeCode)
            {
                case "PART":
                    {
                        if (string.IsNullOrEmpty(fromToFields.PartnerCardId))
                        {
                            throw new ApplicationException(indicator + " Partner is required");
                        }
                        break;
                    }

                case "PORT":
                    {
                        if (string.IsNullOrEmpty(fromToFields.PortId))
                        {
                            throw new ApplicationException(indicator + " Port is required");
                        }
                        break;
                    }

                case "CASL":
                    {
                        if (string.IsNullOrEmpty(fromToFields.AddressCity) && string.IsNullOrEmpty(fromToFields.AddressZipCode))
                        {
                            throw new ApplicationException(indicator + " City or to Zip Code is required");
                        }

                        if (string.IsNullOrEmpty(fromToFields.AddressCountryId))
                        {
                            throw new ApplicationException(indicator + " Country is required");
                        }
                        break;
                    }
            }
        }
        private static void ValidateFutureActualDates(DateTime? ATD, DateTime? ATA, int tenant)
        {
            if (!RoutingDatesValidator.IsActualDateValid(ATD, tenant))
            {
                throw new ApplicationException("Can't set ATD to future date");
            }

            if (!RoutingDatesValidator.IsActualDateValid(ATA, tenant))
            {
                throw new ApplicationException("Can't set ATA to future date");
            }
        }
        private static void ValidateEstimatedLegDates(DateTime? ETD, DateTime? ETA)
        {
            if (!RoutingDatesValidator.IsRoutingLegDatesValid(ETD, ETA))
            {
                throw new ApplicationException("Expected departure must be less than Expected arrival");
            }
        }
        private static void ValidateActualLegDates(DateTime? ATD, DateTime? ATA)
        {
            if (!RoutingDatesValidator.IsRoutingLegDatesValid(ATD, ATA))
            {
                throw new ApplicationException("Actual departure must be less than Actual arrival");
            }
        }
    }

    public class FromToFields
    {
        public string TypeCode;
        public string PartnerCardId;
        public string PortId;
        public string AddressCity;
        public string AddressZipCode;
        public string AddressCountryId;
    }
}
