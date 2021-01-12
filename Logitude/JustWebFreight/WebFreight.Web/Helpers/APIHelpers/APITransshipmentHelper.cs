using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class APITransshipmentHelper
    {
        private int tenant;
        private ShipmentPM shipmentPM;
        private PortRepository portRepository;
        private CardRepository cardRepository;
        private AirlineRepository airlineRepository;
        public APITransshipmentHelper(ShipmentPM shipment, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            portRepository = new PortRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            airlineRepository = new AirlineRepository(commonContext);

            this.shipmentPM = shipment;
            this.tenant = tenant;
        }

        public void ValidateTransshipments()
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

                this.ValidatePort(item.FromPortId, item.LegIndex, "from");
                this.ValidatePort(item.ToPortId, item.LegIndex, "to");
                this.ValidateCarrier(item.CarrierId, item.LegIndex);

                if (item.LegIndex == 1)
                {
                    this.ValidateFirstLeg(item);
                }
            }

            this.ValidatePortsSequence();
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
                    if (myAirline != null)
                    {
                        if (!string.IsNullOrEmpty(myAirline.Prefix))
                        {
                            shipmentPM.AirlinePrefix = myAirline.Prefix.PadLeft(3, '0');
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
                        AirlinePrefix = shipmentPM.AirlinePrefix,
                        IsCancelled = shipmentPM.IsCancelled,
                        IsThrowingException = false,
                    });

                    if (validator.IsUsedInShipment)
                    {
                        throw new ApplicationException("Main Carriage Leg 1 Master field already used in another Shipment");
                    }

                    //bool isFieldExists = ShipmentValidating.IsMasterFieldUsedByAnotherShipment(shipmentPM.Id, item.MasterNumber, shipmentPM.AirlinePrefix, shipmentPM.DirectionId, shipmentPM.TransportModeId, shipmentPM.ShipmentLevelCode, shipmentPM.IsCancelled, tenant);
                    //if (isFieldExists)
                    //{
                    //    throw new ApplicationException("Main Carriage Leg 1 Master field already used in another Shipment");
                    //}
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
            bool isTransshipment1Exists = (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null) ? true : false;
            bool isTransshipment2Exists = (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) ? true : false;
            bool isTransshipment3Exists = (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) ? true : false;

            // Pickups
            DateTime? allPickupsETA = null;
            DateTime? allPickupsATA = null;
            if (shipmentPM.ShipmentPickUps.Count > 0)
            {
                foreach (ShipmentPickUpPM itemPickup in shipmentPM.ShipmentPickUps)
                {
                    if (itemPickup.ETA > allPickupsETA)
                    {
                        allPickupsETA = itemPickup.ETA;
                    }

                    if (itemPickup.ATA > allPickupsATA)
                    {
                        allPickupsATA = itemPickup.ATA;
                    }

                    // Self
                    if (!this.IsRoutingLegDatesValid(itemPickup.ETD, itemPickup.ETA))
                    {
                        throw new ApplicationException("Pick up expected departure must be less than pick up expected arrival");
                    }

                    if (!this.IsRoutingLegDatesValid(itemPickup.ATD, itemPickup.ATA))
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
                    if (!this.IsRoutingLegDatesValid(itemDelivery.ETD, itemDelivery.ETA))
                    {
                        throw new ApplicationException("Delivery expected departure must be less than Delivery expected arrival");
                    }

                    if (!this.IsRoutingLegDatesValid(itemDelivery.ATD, itemDelivery.ATA))
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
            if (!this.IsRoutingLegDatesValid(shipmentPM.MainCarriageETD, shipmentPM.MainCarriageETA))
            {
                throw new ApplicationException("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(shipmentPM.MainCarriageATD, shipmentPM.MainCarriageATA))
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
                if (!this.IsRoutingLegDatesValid(shipmentPM.Transshipment1ETD, shipmentPM.Transshipment1ETA))
                {
                    throw new ApplicationException("Via1 expected departure must be less than Via1 expected arrival");
                }

                if (!this.IsRoutingLegDatesValid(shipmentPM.Transshipment1ATD, shipmentPM.Transshipment1ATA))
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
                if (!this.IsRoutingLegDatesValid(shipmentPM.Transshipment2ETD, shipmentPM.Transshipment2ETA))
                {
                    throw new ApplicationException("Via2 expected departure must be less than Via2 expected arrival");
                }

                if (!this.IsRoutingLegDatesValid(shipmentPM.Transshipment2ATD, shipmentPM.Transshipment2ATA))
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
                if (!this.IsRoutingLegDatesValid(shipmentPM.Transshipment3ETD, shipmentPM.Transshipment3ETA))
                {
                    throw new ApplicationException("Via3 expected departure must be less than Via3 expected arrival");
                }

                if (!this.IsRoutingLegDatesValid(shipmentPM.Transshipment3ATD, shipmentPM.Transshipment3ATA))
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
        }
        private void ValidateActualDates()
        {
            if (!this.IsActualDateValid(shipmentPM.MainCarriageATD))
            {
                throw new ApplicationException("Can't set MainCarriageATD to future date");
            }

            if (!this.IsActualDateValid(shipmentPM.MainCarriageATA))
            {
                throw new ApplicationException("Can't set MainCarriageATA to future date");
            }

            //Transshipment1
            if (!this.IsActualDateValid(shipmentPM.Transshipment1ATD))
            {
                throw new ApplicationException("Can't set Transshipment1ATD to future date");
            }

            if (!this.IsActualDateValid(shipmentPM.Transshipment1ATA))
            {
                throw new ApplicationException("Can't set Transshipment1ATA to future date");
            }

            //Transshipment2
            if (!this.IsActualDateValid(shipmentPM.Transshipment2ATD))
            {
                throw new ApplicationException("Can't set Transshipment2ATD to future date");
            }

            if (!this.IsActualDateValid(shipmentPM.Transshipment2ATA))
            {
                throw new ApplicationException("Can't set Transshipment2ATA to future date");
            }

            //Transshipment3
            if (!this.IsActualDateValid(shipmentPM.Transshipment3ATD))
            {
                throw new ApplicationException("Can't set Transshipment3ATD to future date");
            }

            if (!this.IsActualDateValid(shipmentPM.Transshipment3ATA))
            {
                throw new ApplicationException("Can't set Transshipment3ATA to future date");
            }
        }
        private bool IsRoutingLegDatesValid(DateTime? date1, DateTime? date2)
        {
            bool myResult = true;

            if (date1 != null && date2 != null)
            {
                if (date1 > date2.Value.AddHours(24))
                {
                    myResult = false;
                }

                //if (date1 > date2)
                //{
                //    int ticks = (date1 - date2).Value.Milliseconds;
                //    int seconds = ticks / 1000;
                //    int minutes = seconds / 60;

                //    if (minutes > (24 * 60))
                //    {
                //        myResult = false;
                //    }
                //}
            }

            return myResult;
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

                    //int ticks = (date - date2).Value.Milliseconds;
                    //int seconds = ticks / 1000;
                    //int minutes = seconds / 60;

                    //if (minutes > (24 * 60))
                    //{
                    //    myResult = false;
                    //}
                }
            }

            return myResult;
        }

        public void MapTransshipments()
        {
            this.Reset();

            foreach (TransshipmentLeg item in shipmentPM.MainCarriageLegs.OrderBy(d => d.LegIndex))
            {
                Card myCarrier = null;
                if (!string.IsNullOrEmpty(item.CarrierId))
                {
                    myCarrier = cardRepository.GetSingleCard(item.CarrierId, tenant);
                }

                switch (item.LegIndex)
                {
                    case 1:
                        {
                            shipmentPM.MainCarriageCarrierId = item.CarrierId;
                            shipmentPM.MainCarriageVesselId = item.VesselId;
                            shipmentPM.MainCarriageCarrierNumber = item.CarrierNumber;
                            shipmentPM.Master = item.MasterNumber;
                            shipmentPM.MainCarriageFromPortId = item.FromPortId;
                            shipmentPM.MainCarriageToPortId = item.ToPortId;
                            shipmentPM.MainCarriageATA = item.ATA;
                            shipmentPM.MainCarriageATD = item.ATD;
                            shipmentPM.MainCarriageETA = item.ETA;
                            shipmentPM.MainCarriageETD = item.ETD;

                            if (myCarrier != null)
                            {
                                shipmentPM.MainCarriageCarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }

                    case 2:
                        {
                            shipmentPM.Transshipment1CarrierId = item.CarrierId;
                            shipmentPM.Transshipment1VesselId = item.VesselId;
                            shipmentPM.Transshipment1CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment1AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment1FromPortId = item.FromPortId;
                            shipmentPM.Transshipment1ToPortId = item.ToPortId;
                            shipmentPM.Transshipment1ATA = item.ATA;
                            shipmentPM.Transshipment1ATD = item.ATD;
                            shipmentPM.Transshipment1ETA = item.ETA;
                            shipmentPM.Transshipment1ETD = item.ETD;

                            if (myCarrier != null)
                            {
                                shipmentPM.Transshipment1CarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }

                    case 3:
                        {
                            shipmentPM.Transshipment2CarrierId = item.CarrierId;
                            shipmentPM.Transshipment2VesselId = item.VesselId;
                            shipmentPM.Transshipment2CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment2AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment2FromPortId = item.FromPortId;
                            shipmentPM.Transshipment2ToPortId = item.ToPortId;
                            shipmentPM.Transshipment2ATA = item.ATA;
                            shipmentPM.Transshipment2ATD = item.ATD;
                            shipmentPM.Transshipment2ETA = item.ETA;
                            shipmentPM.Transshipment2ETD = item.ETD;

                            if (myCarrier != null)
                            {
                                shipmentPM.Transshipment2CarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }

                    case 4:
                        {
                            shipmentPM.Transshipment3CarrierId = item.CarrierId;
                            shipmentPM.Transshipment3VesselId = item.VesselId;
                            shipmentPM.Transshipment3CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment3AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment3FromPortId = item.FromPortId;
                            shipmentPM.Transshipment3ToPortId = item.ToPortId;
                            shipmentPM.Transshipment3ATA = item.ATA;
                            shipmentPM.Transshipment3ATD = item.ATD;
                            shipmentPM.Transshipment3ETA = item.ETA;
                            shipmentPM.Transshipment3ETD = item.ETD;

                            if (myCarrier != null)
                            {
                                shipmentPM.Transshipment3CarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }
                }
            }

            this.ValidateRoutingsSeriesDates();
            this.ValidateActualDates();
        }

        private void Reset()
        {
            shipmentPM.Transshipment1CarrierId = null;
            shipmentPM.Transshipment1VesselId = null;
            shipmentPM.Transshipment1CarrierNumber = null;
            shipmentPM.Transshipment1AdditionalMAWBOBLBL = null;
            shipmentPM.Transshipment1FromPortId = null;
            shipmentPM.Transshipment1ToPortId = null;
            shipmentPM.Transshipment1ATA = null;
            shipmentPM.Transshipment1ATD = null;
            shipmentPM.Transshipment1ETA = null;
            shipmentPM.Transshipment1ETD = null;
            shipmentPM.Transshipment1CarrierPrefix = null;

            shipmentPM.Transshipment2CarrierId = null;
            shipmentPM.Transshipment2VesselId = null;
            shipmentPM.Transshipment2CarrierNumber = null;
            shipmentPM.Transshipment2AdditionalMAWBOBLBL = null;
            shipmentPM.Transshipment2FromPortId = null;
            shipmentPM.Transshipment2ToPortId = null;
            shipmentPM.Transshipment2ATA = null;
            shipmentPM.Transshipment2ATD = null;
            shipmentPM.Transshipment2ETA = null;
            shipmentPM.Transshipment2ETD = null;
            shipmentPM.Transshipment2CarrierPrefix = null;

            shipmentPM.Transshipment3CarrierId = null;
            shipmentPM.Transshipment3VesselId = null;
            shipmentPM.Transshipment3CarrierNumber = null;
            shipmentPM.Transshipment3AdditionalMAWBOBLBL = null;
            shipmentPM.Transshipment3FromPortId = null;
            shipmentPM.Transshipment3ToPortId = null;
            shipmentPM.Transshipment3ATA = null;
            shipmentPM.Transshipment3ATD = null;
            shipmentPM.Transshipment3ETA = null;
            shipmentPM.Transshipment3ETD = null;
            shipmentPM.Transshipment3CarrierPrefix = null;
        }

    }
}