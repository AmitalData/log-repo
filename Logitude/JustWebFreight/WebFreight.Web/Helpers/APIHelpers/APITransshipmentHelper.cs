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
            foreach (TransshipmentLeg item in shipmentPM.Transshipments.OrderBy(d => d.LegIndex))
            {
                if (item.LegIndex != 1 && item.LegIndex != 2 && item.LegIndex != 3)
                {
                    throw new ApplicationException("Wrong Transshipment index");
                }

                else
                {
                    if (item.LegIndex == 2)
                    {
                        if (!shipmentPM.Transshipments.Where(d => d.LegIndex == 1).Any())
                        {
                            throw new ApplicationException("Wrong Transshipment index");
                        }
                    }

                    else
                    {
                        if (item.LegIndex == 3)
                        {
                            if (!shipmentPM.Transshipments.Where(d => d.LegIndex == 2).Any())
                            {
                                throw new ApplicationException("Wrong Transshipment index");
                            }
                        }
                    }
                }
                
                if (!string.IsNullOrEmpty(item.VesselId) && shipmentPM.TransportModeId != "O")
                {
                    throw new ApplicationException("Can't send vessel for non-ocean shipments");
                }
                
                this.ValidatePort(item.PortId, item.LegIndex);
                this.ValidateCarrier(item.CarrierId, item.LegIndex);
            }
        }
        private void ValidatePort(string portId, int index)
        {
            if (string.IsNullOrEmpty(portId))
            {
                throw new ApplicationException("Missing Transshipment " + index + " port");
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
                        throw new ApplicationException("Transshipment " + index + " port transport mode is different than shipment transport mode");
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

        public void MapTransshipments()
        {
            foreach (TransshipmentLeg item in shipmentPM.Transshipments.OrderBy(d => d.LegIndex))
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
                            shipmentPM.Transshipment1CarrierId = item.CarrierId;
                            shipmentPM.Transshipment1VesselId = item.VesselId;
                            shipmentPM.Transshipment1CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment1AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment1FromPortId = item.PortId;

                            if (myCarrier != null)
                            {
                                shipmentPM.Transshipment1CarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }

                    case 2:
                        {
                            shipmentPM.Transshipment2CarrierId = item.CarrierId;
                            shipmentPM.Transshipment2VesselId = item.VesselId;
                            shipmentPM.Transshipment2CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment2AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment2FromPortId = item.PortId;

                            if (myCarrier != null)
                            {
                                shipmentPM.Transshipment2CarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }

                    case 3:
                        {
                            shipmentPM.Transshipment3CarrierId = item.CarrierId;
                            shipmentPM.Transshipment3VesselId = item.VesselId;
                            shipmentPM.Transshipment3CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment3AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment3FromPortId = item.PortId;

                            if (myCarrier != null)
                            {
                                shipmentPM.Transshipment3CarrierPrefix = myCarrier.Code;
                            }

                            break;
                        }
                }
            }

            this.SetPorts();
        }

        private void SetPorts()
        {
            this.Transshipment1FromPortChanged();
            this.Transshipment2FromPortChanged();
            this.Transshipment3FromPortChanged();
            //this.FinalDestinationPortChanged();
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
        private void FinalDestinationPortChanged()
        {
            if (!string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortId))
            {
                shipmentPM.Transshipment3ToPortId = shipmentPM.MainCarriageToPortId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
            {
                shipmentPM.Transshipment2ToPortId = shipmentPM.MainCarriageToPortId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
            {
                shipmentPM.Transshipment1ToPortId = shipmentPM.MainCarriageToPortId;
            }

            else
            {
                shipmentPM.MainCarriageToPortId = shipmentPM.MainCarriageToPortId;
            }
        }
    }
}