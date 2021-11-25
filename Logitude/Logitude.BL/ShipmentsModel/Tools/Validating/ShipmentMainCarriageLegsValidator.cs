using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Helpers;
using System.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.QuoteModel;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentMainCarriageLegsValidator
    {
        private int tenant;
        private ShipmentPM shipmentPM;
        private PortRepository portRepository;
        private CardRepository cardRepository;
        private AirlineRepository airlineRepository;
        public ShipmentMainCarriageLegsValidator(ShipmentPM shipment)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            portRepository = new PortRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            airlineRepository = new AirlineRepository(commonContext);

            this.shipmentPM = shipment;
            this.tenant = shipment.Tenant;
        }
        public void ValidateMainCarriageLegs()
        {
            foreach (TransshipmentLeg item in shipmentPM.MainCarriageLegs.OrderBy(d => d.LegIndex))
            {
                if (item.LegIndex != 1 && item.LegIndex != 2 && item.LegIndex != 3 && item.LegIndex != 4)
                {
                    throw new ApplicationException("Wrong Transshipment index");
                }

                else
                {
                    if (item.LegIndex == 2)
                    {
                        if (!shipmentPM.MainCarriageLegs.Where(d => d.LegIndex == 1).Any())
                        {
                            throw new ApplicationException("Wrong Transshipment index");
                        }
                    }

                    else if (item.LegIndex == 3)
                    {
                        if (!shipmentPM.MainCarriageLegs.Where(d => d.LegIndex == 2).Any())
                        {
                            throw new ApplicationException("Wrong Transshipment index");
                        }
                    }

                    else if (item.LegIndex == 4)
                    {
                        if (!shipmentPM.MainCarriageLegs.Where(d => d.LegIndex == 3).Any())
                        {
                            throw new ApplicationException("Wrong Transshipment index");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(item.VesselId) && shipmentPM.TransportModeId != "O")
                {
                    throw new ApplicationException("Can't send vessel for non-ocean shipments");
                }

                ValidatePort(item.FromPortId, item.LegIndex, "from");
                ValidatePort(item.ToPortId, item.LegIndex, "to");
                ValidateCarrier(item.CarrierId, item.LegIndex);

                if (item.LegIndex == 1)
                {
                    ValidateFirstLeg(item);
                }
            }
            ValidatePortsSequence();
        } 
        private void ValidatePort(string portId, int index, string direction)
        {
            if (string.IsNullOrEmpty(portId))
            {
                throw new ApplicationException("Missing Transshipment " + index + " " + direction + " port");
            }

            else
            {
                bool isValid = true;
                Port myPort = portRepository.GetSinglePort(portId, tenant);
                if (myPort != null)
                {
                    switch (shipmentPM.TransportModeId)
                    {
                        case "A":
                            {
                                if (!myPort.IsAir)
                                {
                                    isValid = false;
                                }
                                break;
                            }

                        case "I":
                            {
                                if (!myPort.IsInland)
                                {
                                    isValid = false;
                                }
                                break;
                            }

                        case "O":
                            {
                                if (!myPort.IsOcean)
                                {
                                    isValid = false;
                                }
                                break;
                            }
                    }

                    if (!isValid)
                    {
                        throw new ApplicationException("Transshipment " + index + " " + direction + " port transport mode is different than shipment transport mode");
                    }
                }
            }
        }
        private void ValidateCarrier(string carrierId, int index)
        {
            if (!string.IsNullOrEmpty(carrierId))
            {
                bool isValid = true;
                Card myCarrier = cardRepository.GetSingleCard(carrierId, tenant);
                if (myCarrier != null)
                {
                    switch (shipmentPM.TransportModeId)
                    {
                        case "A":
                            {
                                if (myCarrier.PartnerTypeId != "AL")
                                {
                                    isValid = false;
                                }
                                break;
                            }

                        case "I":
                            {
                                if (myCarrier.PartnerTypeId != "TR")
                                {
                                    isValid = false;
                                }
                                break;
                            }

                        case "O":
                            {
                                if (myCarrier.PartnerTypeId != "SL")
                                {
                                    isValid = false;
                                }
                                break;
                            }
                    }

                    if (!isValid)
                    {
                        throw new ApplicationException("Transshipment " + index + " carrier is not allowed for shipment transport mode");
                    }
                }
            }
        }
        private void ValidateFirstLeg(TransshipmentLeg item)
        {
            if (shipmentPM.TransportModeId == "A")
            {
                if (!string.IsNullOrEmpty(item.MasterNumber))
                {
                    Airline myAirline = airlineRepository.GetSingleAirline(item.CarrierId, tenant);
                    string shipmentAirlinePrefix = shipmentPM.AirlinePrefix;
                    if (myAirline != null)
                    {
                        if (!string.IsNullOrEmpty(myAirline.Prefix))
                        {
                            shipmentAirlinePrefix = myAirline.Prefix.PadLeft(3, '0');
                        }
                    }

                    ShipmentMasterIsUsedValidator validator = new ShipmentMasterIsUsedValidator();

                    validator.Validate(new ShipmentMasterIsUsedValidatorArgs()
                    {
                        Tenant = tenant,
                        ShipmentId = shipmentPM.Id,
                        BookingId = shipmentPM.BookingId,
                        DirectionId = shipmentPM.DirectionId,
                        TransportModeId = shipmentPM.TransportModeId,
                        ShipmentLevelCode = shipmentPM.ShipmentLevelCode,
                        Master = item.MasterNumber,
                        AirlinePrefix = shipmentAirlinePrefix,
                        IsCancelled = shipmentPM.IsCancelled,
                        IsThrowingException = false,
                    });

                    if (validator.IsUsedInShipment)
                    {
                        throw new ApplicationException("Main Carriage Leg 1 Master field already used in another Shipment");
                    }
                }
            }
        }
        private void ValidatePortsSequence()
        {
            foreach (TransshipmentLeg item in shipmentPM.MainCarriageLegs.OrderBy(d => d.LegIndex))
            {
                TransshipmentLeg nextLeg = shipmentPM.MainCarriageLegs.Where(d => d.LegIndex == item.LegIndex + 1).FirstOrDefault();
                if (nextLeg != null)
                {
                    if (item.ToPortId != nextLeg.FromPortId)
                    {
                        throw new ApplicationException("Invalid ports between leg " + item.LegIndex + " and " + nextLeg.LegIndex);
                    }
                }
            }
        }
        public void ValidateRoutingsSeriesDates()
        {
            bool isTransshipment1Exists = (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null);
            bool isTransshipment2Exists = (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null);
            bool isTransshipment3Exists = (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null);

            // Pickups
            DateTime? allPickupsETA = null;
            DateTime? allPickupsATA = null;
            if (shipmentPM.ShipmentPickUps.Count > 0)
            {
                foreach (ShipmentPickUpPM itemPickup in shipmentPM.ShipmentPickUps)
                {
                    if (itemPickup.ETA > allPickupsETA) allPickupsETA = itemPickup.ETA;

                    if (itemPickup.ATA > allPickupsATA) allPickupsATA = itemPickup.ATA;

                    // Self
                    if (!IsRoutingLegDatesValid(itemPickup.ETD, itemPickup.ETA))
                    {
                        throw new ApplicationException("Pick up expected departure must be less than pick up expected arrival");
                    }

                    if (!IsRoutingLegDatesValid(itemPickup.ATD, itemPickup.ATA))
                    {
                        throw new ApplicationException("Pick up actual departure must be less than pick up actual arrival");
                    }

                    if (itemPickup.ETA >= shipmentPM.MainCarriageETD)
                    {
                        throw new ApplicationException("Pick up expected arrival must be less than main carriage expected departure");
                    }

                    if (itemPickup.ATA >= shipmentPM.MainCarriageATD)
                    {
                        throw new ApplicationException("Pick up actual arrival must be less than main carriage actual departure");
                    }
                }
            }

            // Deliveries
            DateTime? allDeliveriesETD = null;
            DateTime? allDeliveriesATD = null;
            if (shipmentPM.ShipmentDeliveries.Count > 0)
            {
                foreach (ShipmentDeliveryPM itemDelivery in shipmentPM.ShipmentDeliveries)
                {
                    if (itemDelivery.ETD != null)
                    {
                        if (allDeliveriesETD == null)
                        {
                            allDeliveriesETD = itemDelivery.ETD;
                        }

                        else if (allDeliveriesETD > itemDelivery.ETD)
                        {
                            allDeliveriesETD = itemDelivery.ETD;
                        }
                    }

                    if (itemDelivery.ATD != null)
                    {
                        if (allDeliveriesATD == null)
                        {
                            allDeliveriesATD = itemDelivery.ATD;
                        }

                        else if (allDeliveriesATD > itemDelivery.ATD)
                        {
                            allDeliveriesATD = itemDelivery.ATD;
                        }
                    }

                    // Self
                    if (!IsRoutingLegDatesValid(itemDelivery.ETD, itemDelivery.ETA))
                    {
                        throw new ApplicationException("Delivery expected departure must be less than Delivery expected arrival");
                    }

                    if (!IsRoutingLegDatesValid(itemDelivery.ATD, itemDelivery.ATA))
                    {
                        throw new ApplicationException("Delivery actual departure must be less than Delivery actual arrival");
                    }

                    // Previous
                    if (isTransshipment3Exists)
                    {
                        if (itemDelivery.ETD <= shipmentPM.Transshipment3ETA)
                        {
                            throw new ApplicationException("Delivery expected departure must be bigger than Transshipment3 expected arrival");
                        }

                        if (itemDelivery.ATD <= shipmentPM.Transshipment3ATA)
                        {
                            throw new ApplicationException("Delivery actual departure must be bigger than Transshipment3 actual arrival");
                        }
                    }

                    else if (isTransshipment2Exists)
                    {
                        if (itemDelivery.ETD <= shipmentPM.Transshipment2ETA)
                        {
                            throw new ApplicationException("Delivery expected departure must be bigger than Transshipment2 expected arrival");
                        }

                        if (itemDelivery.ATD <= shipmentPM.Transshipment2ATA)
                        {
                            throw new ApplicationException("Delivery actual departure must be bigger than Transshipment2 actual arrival");
                        }
                    }

                    else if (isTransshipment1Exists)
                    {
                        if (itemDelivery.ETD <= shipmentPM.Transshipment1ETA)
                        {
                            throw new ApplicationException("Delivery expected departure must be bigger than Transshipment1 expected arrival");
                        }

                        if (itemDelivery.ATD <= shipmentPM.Transshipment1ATA)
                        {
                            throw new ApplicationException("Delivery actual departure must be bigger than Transshipment1 actual arrival");
                        }
                    }

                    else
                    {
                        if (itemDelivery.ETD <= shipmentPM.MainCarriageETA)
                        {
                            throw new ApplicationException("Delivery expected departure must be bigger than Main-Carriage expected arrival");
                        }

                        if (itemDelivery.ATD <= shipmentPM.MainCarriageATA)
                        {
                            throw new ApplicationException("Delivery actual departure must be bigger than Main-Carriage actual arrival");
                        }
                    }
                }
            }

            // Self
            if (!IsRoutingLegDatesValid(shipmentPM.MainCarriageETD, shipmentPM.MainCarriageETA))
            {
                throw new ApplicationException("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
            }

            if (!IsRoutingLegDatesValid(shipmentPM.MainCarriageATD, shipmentPM.MainCarriageATA))
            {
                throw new ApplicationException("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
            }

            // Previous
            if (shipmentPM.ShipmentPickUps.Count > 0)
            {
                if (shipmentPM.MainCarriageETD <= allPickupsETA)
                {
                    throw new ApplicationException("Main-Carriage expected departure must be bigger than all pick ups expected arrival");
                }

                if (shipmentPM.MainCarriageATD <= allPickupsATA)
                {
                    throw new ApplicationException("Main-Carriage actual departure must be bigger than all pick ups actual arrival");
                }
            }

            // Next
            if (isTransshipment1Exists)
            {
                if (shipmentPM.MainCarriageETA >= shipmentPM.Transshipment1ETD)
                {
                    throw new ApplicationException("Main-Carriage expected arrival must be less than Via1 expected departure");
                }

                if (shipmentPM.MainCarriageATA >= shipmentPM.Transshipment1ATD)
                {
                    throw new ApplicationException("Main-Carriage actual arrival must be less than Via1 actual departure");
                }
            }

            else if (isTransshipment2Exists)
            {
                if (shipmentPM.MainCarriageETA >= shipmentPM.Transshipment2ETD)
                {
                    throw new ApplicationException("Main-Carriage expected arrival must be less than Via2 expected departure");
                }

                if (shipmentPM.MainCarriageATA >= shipmentPM.Transshipment2ATD)
                {
                    throw new ApplicationException("Main-Carriage actual arrival must be less than Via2 actual departure");
                }
            }

            else if (isTransshipment3Exists)
            {
                if (shipmentPM.MainCarriageETA >= shipmentPM.Transshipment3ETD)
                {
                    throw new ApplicationException("Main-Carriage expected arrival must be less than Via3 expected departure");
                }

                if (shipmentPM.MainCarriageATA >= shipmentPM.Transshipment3ATD)
                {
                    throw new ApplicationException("Main-Carriage actual arrival must be less than Via3 actual departure");
                }
            }

            else if (shipmentPM.ShipmentDeliveries.Count > 0)
            {
                if (shipmentPM.MainCarriageETA >= allDeliveriesETD)
                {
                    throw new ApplicationException("Main-Carriage expected arrival must be less than all deliveries expected departure");
                }

                if (shipmentPM.MainCarriageATA >= allDeliveriesATD)
                {
                    throw new ApplicationException("Main-Carriage actual arrival must be less than all deliveries actual departure");
                }
            }

            // Transshipment1
            if (isTransshipment1Exists)
            {
                // Self
                if (!IsRoutingLegDatesValid(shipmentPM.Transshipment1ETD, shipmentPM.Transshipment1ETA))
                {
                    throw new ApplicationException("Via1 expected departure must be less than Via1 expected arrival");
                }

                if (!IsRoutingLegDatesValid(shipmentPM.Transshipment1ATD, shipmentPM.Transshipment1ATA))
                {
                    throw new ApplicationException("Via1 actual departure must be less than Via1 actual arrival");
                }

                // Next
                if (isTransshipment2Exists)
                {
                    if (shipmentPM.Transshipment1ETA >= shipmentPM.Transshipment2ETD)
                    {
                        throw new ApplicationException("Via1 expected arrival must be less than Via2 expected departure");
                    }

                    if (shipmentPM.Transshipment1ATA >= shipmentPM.Transshipment2ATD)
                    {
                        throw new ApplicationException("Via1 actual arrival must be less than Via2 actual departure");
                    }
                }

                else if (isTransshipment3Exists)
                {
                    if (shipmentPM.Transshipment1ETA >= shipmentPM.Transshipment3ETD)
                    {
                        throw new ApplicationException("Via1 expected arrival must be less than Via3 expected departure");
                    }

                    if (shipmentPM.Transshipment1ATA >= shipmentPM.Transshipment3ATD)
                    {
                        throw new ApplicationException("Via1 actual arrival must be less than Via3 actual departure");
                    }
                }

                else if (shipmentPM.ShipmentDeliveries.Count > 0)
                {
                    if (shipmentPM.Transshipment1ETA >= allDeliveriesETD)
                    {
                        throw new ApplicationException("Via1 expected arrival must be less than all deliveries expected departure");
                    }

                    if (shipmentPM.Transshipment1ATA >= allDeliveriesATD)
                    {
                        throw new ApplicationException("Via1 actual arrival must be less than all deliveries actual departure");
                    }
                }
            }

            // Transshipment2
            if (isTransshipment2Exists)
            {
                // Self
                if (!IsRoutingLegDatesValid(shipmentPM.Transshipment2ETD, shipmentPM.Transshipment2ETA))
                {
                    throw new ApplicationException("Via2 expected departure must be less than Via2 expected arrival");
                }

                if (!IsRoutingLegDatesValid(shipmentPM.Transshipment2ATD, shipmentPM.Transshipment2ATA))
                {
                    throw new ApplicationException("Via2 actual departure must be less than Via2 actual arrival");
                }

                // Next
                if (isTransshipment3Exists)
                {
                    if (shipmentPM.Transshipment2ETA >= shipmentPM.Transshipment3ETD)
                    {
                        throw new ApplicationException("Via2 expected arrival must be less than Via3 expected departure");
                    }

                    if (shipmentPM.Transshipment2ATA >= shipmentPM.Transshipment3ATD)
                    {
                        throw new ApplicationException("Via2 actual arrival must be less than Via3 actual departure");
                    }
                }

                else if (shipmentPM.ShipmentDeliveries.Count > 0)
                {
                    if (shipmentPM.Transshipment2ETA >= allDeliveriesETD)
                    {
                        throw new ApplicationException("Via2 expected arrival must be less than all deliveries expected departure");
                    }

                    if (shipmentPM.Transshipment2ATA >= allDeliveriesATD)
                    {
                        throw new ApplicationException("Via2 actual arrival must be less than all deliveries actual departure");
                    }
                }
            }

            // Transshipment3
            if (isTransshipment3Exists)
            {

                // Self
                if (!IsRoutingLegDatesValid(shipmentPM.Transshipment3ETD, shipmentPM.Transshipment3ETA))
                {
                    throw new ApplicationException("Via3 expected departure must be less than Via3 expected arrival");
                }

                if (!IsRoutingLegDatesValid(shipmentPM.Transshipment3ATD, shipmentPM.Transshipment3ATA))
                {
                    throw new ApplicationException("Via3 actual departure must be less than Via3 actual arrival");
                }

                // Next
                if (shipmentPM.ShipmentDeliveries.Count > 0)
                {
                    if (shipmentPM.Transshipment3ETA >= allDeliveriesETD)
                    {
                        throw new ApplicationException("Via3 expected arrival must be less than all deliveries expected departure");
                    }

                    if (shipmentPM.Transshipment3ATA >= allDeliveriesATD)
                    {
                        throw new ApplicationException("Via3 actual arrival must be less than all deliveries actual departure");
                    }
                }
            }
            ShipmentWarehouseLegsValidator shipmentWarehouseLegsValidator = new ShipmentWarehouseLegsValidator(shipmentPM);
            shipmentWarehouseLegsValidator.ValidateWarehouseLeg();
        } 
        private bool IsRoutingLegDatesValid(DateTime? firstDate, DateTime? secondDate)
        {
            if (firstDate == null || secondDate == null) return true;
            if (firstDate > secondDate.Value.AddHours(24)) return false;

            return true;
        }
        public void ValidateActualDates()
        {
            if (!IsActualDateValid(shipmentPM.MainCarriageATD))
            {
                throw new ApplicationException("Can't set MainCarriageATD to future date");
            }

            if (!IsActualDateValid(shipmentPM.MainCarriageATA))
            {
                throw new ApplicationException("Can't set MainCarriageATA to future date");
            }

            //Transshipment1
            if (!IsActualDateValid(shipmentPM.Transshipment1ATD))
            {
                throw new ApplicationException("Can't set Transshipment1ATD to future date");
            }

            if (!IsActualDateValid(shipmentPM.Transshipment1ATA))
            {
                throw new ApplicationException("Can't set Transshipment1ATA to future date");
            }

            //Transshipment2
            if (!IsActualDateValid(shipmentPM.Transshipment2ATD))
            {
                throw new ApplicationException("Can't set Transshipment2ATD to future date");
            }

            if (!IsActualDateValid(shipmentPM.Transshipment2ATA))
            {
                throw new ApplicationException("Can't set Transshipment2ATA to future date");
            }

            //Transshipment3
            if (!IsActualDateValid(shipmentPM.Transshipment3ATD))
            {
                throw new ApplicationException("Can't set Transshipment3ATD to future date");
            }

            if (!IsActualDateValid(shipmentPM.Transshipment3ATA))
            {
                throw new ApplicationException("Can't set Transshipment3ATA to future date");
            }
        } 
        private bool IsActualDateValid(DateTime? date)
        {
            bool myResult = true;

            if (date != null)
            {
                DateTime date2 = TenantServerConfigration.GetCurrentDateTime(tenant);
                date2 = date2.AddHours(24);

                if (date > date2)
                {
                    myResult = false;
                }
            }

            return myResult;
        }
    }
}