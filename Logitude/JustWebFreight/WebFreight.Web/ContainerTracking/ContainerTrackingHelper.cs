using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ContainerTracking
{
    public class ContainerTrackingHelper
    {
        private readonly PortRepository portRepository;
        private readonly int tenant;
        private ContainerDiscrepancyService containerDiscrepancyService;
        ContainerUpdatedFields updatedFields;
        public ContainerTrackingHelper(int tenant)
        {
            this.tenant = tenant;
            this.portRepository = new PortRepository(tenant);
            this.containerDiscrepancyService = new ContainerDiscrepancyService(tenant);
        }

        public bool IsSameLocationUsingId(string entityPortId, string responsePortId)
        {
            Port responsePort = GetPortById(responsePortId, tenant);
            Port responsePort_zero = responsePort == null ? null : GetPortByCode(responsePort.Code, 0);
            Port entityPort = GetPortById(entityPortId, tenant);
            Port entityPort_Zero = GetPortByCode(entityPort?.CombinedCode, 0);

            if (responsePort == null) return false;

            if (string.IsNullOrEmpty(entityPortId))
                return true;

            else if (entityPortId == responsePort.Id)
                return true;

            else if (responsePort_zero?.PortGroupId != null && entityPort_Zero?.PortGroupId != null && responsePort_zero?.PortGroupId == entityPort_Zero?.PortGroupId)
                return true;

            return false;
        }

        public bool IsSameLocation(string entityPortId, string responsePortCode)
        {
            Port responsePort = GetPortByCode(responsePortCode, tenant);
            Port responsePort_zero = GetPortByCode(responsePortCode, 0);
            Port entityPort = GetPortById(entityPortId, tenant);
            Port entityPort_Zero = GetPortByCode(entityPort?.CombinedCode, 0);

            if (responsePort == null) return false;

            if (string.IsNullOrEmpty(entityPortId))
                return true;

            else if (entityPortId == responsePort.Id)
                return true;

            else if (responsePort_zero?.PortGroupId != null && entityPort_Zero?.PortGroupId != null && responsePort_zero?.PortGroupId == entityPort_Zero?.PortGroupId)
                return true;

            return false;
        }

        private Port GetPortByCode(string portCode, int tenant)
        {
            return portRepository.GetOceanPortByCombinedCode(portCode, tenant);
        }
        private Port GetPortById(string portId, int tenant)
        {
            return portRepository.GetSinglePort(portId, tenant);
        }
        public void AddContainerDiscrepancy(string code, ContainerPM containerPM, ShipmentPM shipmentPM)
        {
            if(code == "PreCarriage")
            {
               // string containerLoc, string shipmentLoc, 
                if (!IsSameLocationUsingId(containerPM.PreCarriageLocationPortId, shipmentPM.PreCarriageFromPortId))
                {
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code, "ETD");
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code, "ATD");
                }
                else
                {
                    if (shipmentPM.PreCarriageATD != containerPM.PreCarriageATD)
                    {
                        AddActualDateDiscrepancy(containerPM, shipmentPM, code);
                    }
                }
            }

            if (code == "OnCarriage")
            {
                if (!IsSameLocationUsingId(containerPM.OnCarriageLocationPortId, shipmentPM.OnCarriageToPortId))
                {
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code, "ETA");
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code, "ATA");
                }
                else
                {
                    if (shipmentPM.OnCarriageATA != containerPM.OnCarriageATA)
                    {
                        AddActualDateDiscrepancy(containerPM, shipmentPM, code);
                    }
                }
            }

            if(code == "POLPreCarriage")
            {
                if (shipmentPM.PreCarriageATD != containerPM.ActualPOLVesselDeparture)
                {
                    AddActualDateDiscrepancy(containerPM, shipmentPM, code);
                }
            }

            if (code == "POLMainCarriage")
            {
                if (!IsSameLocation(shipmentPM.MainCarriageFromPortId, containerPM.POLLocation))
                {
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code.Substring(0, 3), "ETD");
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code.Substring(0, 3), "ATD");
                }
                else
                {
                    if (shipmentPM.MainCarriageATD != containerPM.ActualPOLVesselDeparture)
                    {
                        AddActualDateDiscrepancy(containerPM, shipmentPM, code);
                    }
                }
                 
            }
            
            if(code == "PODMainCarriage")
            {
                if (IsSameLocation(shipmentPM.MainCarriageFromPortId, containerPM.PODLocation))
                {
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code.Substring(0, 3), "ETA");
                    AddNotSameLocationDiscrepancy(containerPM, shipmentPM, code.Substring(0, 3), "ATA");
                }
                else
                {
                    if (shipmentPM.MainCarriageATA != containerPM.ActualPODVesselArrival)
                    {
                        AddActualDateDiscrepancy(containerPM, shipmentPM, code);
                    }
                }
                
            }

            if (code == "PODOnCarriage") 
            {
                if (shipmentPM.OnCarriageATA != containerPM.ActualPODVesselArrival)
                {
                    AddActualDateDiscrepancy(containerPM, shipmentPM, code);
                }
            }
        }
        private void CheckIsSameLocation(ContainerPM containerPM, ShipmentPM shipmentPM, string code)
        {

        }
        public void AddDeliveryContainerDiscrepancy(ShipmentDeliveryPM delivery, ShipmentPM shipmentPM, ContainerPM containerPM)
        {
            if (delivery.ATA != containerPM.ActualEmptyReturn)
            {
                var discrepancyReason = $@"Actual Empty Return already has a value of {delivery.ATA} - did not update new container value {containerPM.ActualEmptyReturn}.";
                AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);
            }
        }
        public void AddTranshipmentDiscrepancyContainer(int? transshipmentLegIndex, string direction, ContainerPM containerPM,
                                                        ShipmentPM shipmentPM, string portId, MilestoneDataUpdatedFields updatedFields)
        {
            if (!IsSameLocationUsingId((string)GetPropValue(shipmentPM, "Transshipment" + transshipmentLegIndex + "ToPortId"), portId))
            {
                AddNotSametransshipmentLegDiscrepancy(containerPM, shipmentPM, direction, transshipmentLegIndex, portId);
            }
            else 
            {
                var transshipmentATAInShipment = GetPropValue(shipmentPM, "Transshipment" + transshipmentLegIndex + "ATA");

                if ((DateTime)transshipmentATAInShipment != updatedFields.ActualDate)
                {
                    var discrepancyReason = $@"Transshipment{transshipmentLegIndex} ATA already has a value of {transshipmentATAInShipment} - did not update new container value {updatedFields.ActualDate}.";
                    AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);
                }
            }
        }
        private void AddNotSameLocationDiscrepancy(ContainerPM containerPM, ShipmentPM shipmentPM,/*,string containerLocation, string shipmentLocation, */string location, string time)
        {
            var shipmentLocation = "";
            var containerLocation = "";
            if (location == "PreCarriage")
            {
              shipmentLocation = shipmentPM.PreCarriageFromPortId;
              containerLocation = containerPM.PreCarriageLocation;
            }
            else if (location == "OnCarriage")
            {
                shipmentLocation = shipmentPM.OnCarriageToPortId;
                containerLocation = containerPM.OnCarriageLocation;
            }
            else if (location == "POL")
            {
                shipmentLocation = shipmentPM.MainCarriageToPortId;
                containerLocation = containerPM.POLLocation;
            }
            else
            {
                shipmentLocation = shipmentPM.MainCarriageFromPortId;
                containerLocation = containerPM.PODLocation;
            }

            var shipmentUnloCode = GetUnloCodeFromPortId(shipmentLocation);
            var discrepancyReason = "Shipment " + location + " from port is empty - shipment " + time + " not updated.";
            if (shipmentUnloCode != null)
            {
                discrepancyReason = $@"Shipment {location} from port {shipmentUnloCode} not equal to container {location} port {containerLocation} - shipment {time} not updated.";
            }

            AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);
        }
        private string GetUnloCodeFromPortId(string portId)
        {
            var newPort = GetPortById(portId, tenant);
            if (newPort == null) return null;
            var combineCode = newPort.CombinedCode;
            return combineCode;

        }
        private void AddNotSametransshipmentLegDiscrepancy(ContainerPM containerPM, ShipmentPM shipmentPM, string direction, int? transshipmentLegIndex, string portId)
        {
            var shipmentUnloCode = getTransshipmentUnloCode(shipmentPM, transshipmentLegIndex);

            var discrepancyReason = $@"Transshipment{transshipmentLegIndex} {direction} port is Empty - shipment ETD not updated.";

            var containerUnloCode = GetUnloCodeFromPortId(portId);

            if (shipmentUnloCode != null)
            {
                discrepancyReason = $@"Transshipment{transshipmentLegIndex} {direction} port {shipmentUnloCode} not equal to container Transshipment{transshipmentLegIndex} From port {containerUnloCode} - shipment ETA not updated.";
            }

            AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);
        }
        private string getTransshipmentUnloCode(ShipmentPM shipmentPM, int? transshipmentLegIndex)
        {
            if (transshipmentLegIndex == 1) return GetUnloCodeFromPortId(shipmentPM.Transshipment1FromPortId);
            if (transshipmentLegIndex == 2) return GetUnloCodeFromPortId(shipmentPM.Transshipment2FromPortId);

            return GetUnloCodeFromPortId(shipmentPM.Transshipment3FromPortId);
        }
        private void AddActualDateDiscrepancy(ContainerPM containerPM, ShipmentPM shipmentPM, string code)
        {
            var actualdateName = "";
            DateTime? shimpmentActualDate;
            DateTime? containerActualDate;
            if (code == "PreCarriage")
            {
                actualdateName = "Pre Carriage ATD";
                shimpmentActualDate = shipmentPM.PreCarriageATD;
                containerActualDate = containerPM.PreCarriageATD;
            }
            else if (code == "OnCarriage")
            {
                actualdateName = "On Carriage ATA";
                shimpmentActualDate = shipmentPM.OnCarriageATA;
                containerActualDate = containerPM.OnCarriageATA;

            }
            else if (code == "POLPreCarriage")
            {
                actualdateName = "Pre Carriage ATD";
                shimpmentActualDate = shipmentPM.PreCarriageATD;
                containerActualDate = containerPM.ActualPOLVesselDeparture;
            }
            else if (code == "POLMainCarriage")
            {
                actualdateName = "Main Carriage ATD";
                shimpmentActualDate = shipmentPM.PreCarriageATD;
                containerActualDate = containerPM.ActualPOLVesselDeparture;
            }
            else if (code == "PODMainCarriage")
            {
                actualdateName = "Main Carriage ATA";
                shimpmentActualDate = shipmentPM.MainCarriageATA;
                containerActualDate = containerPM.ActualPODVesselArrival;
            }
            else
            {
                actualdateName = "On Carriage ATA";
                shimpmentActualDate = shipmentPM.MainCarriageATA;
                containerActualDate = containerPM.ActualPODVesselArrival;
            }

            var discrepancyReason = $@"Shipment {actualdateName} already has a value of {shimpmentActualDate} - did not update new container value {containerActualDate}.";

            AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);

        }        
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        private void AddContainerDiscrepancyToService(ContainerPM containerPM, ShipmentPM shipmentPM, string reasonOfDiscrepancy)
        {
            this.containerDiscrepancyService = new ContainerDiscrepancyService(containerPM.Tenant);
            this.containerDiscrepancyService.Create(containerPM, shipmentPM, reasonOfDiscrepancy);
        }
    }
}