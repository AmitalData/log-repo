using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
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
        public APITransshipmentHelper(ShipmentPM shipment, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            portRepository = new PortRepository(commonContext);
            cardRepository = new CardRepository(commonContext);

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
                                if(!myPort.IsAir)
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
            if(!string.IsNullOrEmpty(carrierId))
            {
                bool isValid = true;
                Card myCarrier = cardRepository.GetSingleCard(carrierId, tenant);
                if(myCarrier != null)
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
            
        }
        private void ValidatePortsSequence()
        {
            foreach (TransshipmentLeg item in shipmentPM.MainCarriageLegs.OrderBy(d => d.LegIndex))
            {
                TransshipmentLeg nextLeg = shipmentPM.MainCarriageLegs.Where(d => d.LegIndex == item.LegIndex + 1).FirstOrDefault();
                if(nextLeg != null)
                {
                    if(item.ToPortId != nextLeg.FromPortId)
                    {
                        throw new ApplicationException("Invalid ports between leg " + item.LegIndex + " and " + nextLeg.LegIndex);
                    }
                }
            }
        }

        public void MapTransshipments()
        {
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

            //this.SetPorts();
        }

        private void SetPorts()
        {
            this.Transshipment1FromPortChanged();
            this.Transshipment2FromPortChanged();
            this.Transshipment3FromPortChanged();
        }

        private void Transshipment1FromPortChanged()
        {
            if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
            {
                shipmentPM.MainCarriageToPortId = shipmentPM.Transshipment1FromPortId;

                if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
                {
                    shipmentPM.Transshipment1ToPortId = shipmentPM.Transshipment2FromPortId;
                }

                else if (!string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortId))
                {
                    shipmentPM.Transshipment1ToPortId = shipmentPM.Transshipment3FromPortId;
                }

                else
                {
                    shipmentPM.Transshipment1ToPortId = shipmentPM.MainCarriageFinalDestinationPortId;
                }
            }
            
            else
            {
                shipmentPM.Transshipment1ToPortId = null;
            }
        }
        private void Transshipment2FromPortChanged()
        {
            string myPortId = shipmentPM.Transshipment2FromPortId;

            // this.To
            if (!string.IsNullOrEmpty(myPortId))
            {
                shipmentPM.Transshipment2ToPortId = shipmentPM.MainCarriageFinalDestinationPortId;

                if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
                {
                    shipmentPM.Transshipment1ToPortId = myPortId;
                }

                else
                {
                    shipmentPM.MainCarriageToPortId = myPortId;
                }
            }

            else
            {
                shipmentPM.Transshipment2ToPortId = null;
                myPortId = shipmentPM.MainCarriageFinalDestinationPortId;
                
                if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
                {
                    shipmentPM.Transshipment1ToPortId = myPortId;
                }

                else
                {
                    shipmentPM.MainCarriageToPortId = myPortId;
                }
            }            
        }
        private void Transshipment3FromPortChanged()
        {
            string myPortId = shipmentPM.Transshipment3FromPortId;
            
            if (!string.IsNullOrEmpty(myPortId))
            {
                if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
                {
                    shipmentPM.Transshipment2ToPortId = myPortId;
                }

                else if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
                {
                    shipmentPM.Transshipment1ToPortId = myPortId;
                }

                else
                {
                    shipmentPM.MainCarriageToPortId = myPortId;
                }
                
                shipmentPM.Transshipment3ToPortId = shipmentPM.MainCarriageFinalDestinationPortId;
            }

            else
            {
                shipmentPM.Transshipment3ToPortId = null;
                
                if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
                {
                    shipmentPM.Transshipment2ToPortId = shipmentPM.MainCarriageFinalDestinationPortId;
                }

                else if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
                {
                    shipmentPM.Transshipment1ToPortId = shipmentPM.MainCarriageFinalDestinationPortId;
                }

                else
                {
                    shipmentPM.MainCarriageToPortId = shipmentPM.MainCarriageFinalDestinationPortId;
                }
            }
        }
    }
}