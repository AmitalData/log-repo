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
        public APITransshipmentHelper(ShipmentPM shipment, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            portRepository = new PortRepository(commonContext);

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

                if (string.IsNullOrEmpty(item.PortId))
                {
                    throw new ApplicationException("Missing Transshipment " + item.LegIndex + " port");
                }

                if (!string.IsNullOrEmpty(item.VesselId) && shipmentPM.TransportModeId != "O")
                {
                    throw new ApplicationException("Can't send vessel for non-ocean shipments");
                }
            }
        }

        public void MapTransshipments()
        {
            foreach (TransshipmentLeg item in shipmentPM.Transshipments.OrderBy(d => d.LegIndex))
            {
                switch (item.LegIndex)
                {
                    case 1:
                        {
                            shipmentPM.Transshipment1CarrierId = item.CarrierId;
                            shipmentPM.Transshipment1VesselId = item.VesselId;
                            shipmentPM.Transshipment1CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment1AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment1FromPortId = item.PortId;
                            break;
                        }

                    case 2:
                        {
                            shipmentPM.Transshipment2CarrierId = item.CarrierId;
                            shipmentPM.Transshipment2VesselId = item.VesselId;
                            shipmentPM.Transshipment2CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment2AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment2FromPortId = item.PortId;
                            break;
                        }

                    case 3:
                        {
                            shipmentPM.Transshipment3CarrierId = item.CarrierId;
                            shipmentPM.Transshipment3VesselId = item.VesselId;
                            shipmentPM.Transshipment3CarrierNumber = item.CarrierNumber;
                            shipmentPM.Transshipment3AdditionalMAWBOBLBL = item.MasterNumber;
                            shipmentPM.Transshipment3FromPortId = item.PortId;
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