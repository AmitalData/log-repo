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
                var preCarriageParams = new  { 
                    containerPM = containerPM,
                    shipmentPM = shipmentPM,
                    containerCode = code,
                    shipmentCode = code,
                    containerLocationId = containerPM.PreCarriageLocationPortId,
                    containerLocation = containerPM.PreCarriageLocation,
                    shipmentLocation = shipmentPM.PreCarriageFromPortId,
                    containerActualDate = containerPM.PreCarriageATD,
                    shipmentActualDate = shipmentPM.PreCarriageATD,
                    containerEstimatedDateName = "ETD",
                    containerActualDateName = "ATD",
                    shipmentDirection = "from",
                };
                CheckIsSameLocationUsingId(preCarriageParams);               
            }

            if (code == "OnCarriage")
            {
                var preCarriageParams = new
                {
                    containerPM = containerPM,
                    shipmentPM = shipmentPM,
                    containerCode = code,
                    shipmentCode = code,
                    containerLocationId = containerPM.OnCarriageLocationPortId,
                    containerLocation = containerPM.OnCarriageLocation,
                    shipmentLocation = shipmentPM.OnCarriageToPortId,
                    containerActualDate = containerPM.OnCarriageATA,
                    shipmentActualDate = shipmentPM.OnCarriageATA,
                    containerEstimatedDateName = "ETA",
                    containerActualDateName = "ATA",
                    shipmentDirection = "to",
                };
                CheckIsSameLocationUsingId(preCarriageParams);
            }

            if(code == "POLPreCarriage")
            {
                var discreapancyParams = new
                {
                    containerPM = containerPM,
                    shipmentPM = shipmentPM,
                    containerCode = code.Substring(0,3),
                    shipmentCode = code.Substring(3),
                    containerActualDate = containerPM.ActualPODVesselArrival,
                    shipmentActualDate = shipmentPM.OnCarriageATA,
                    containerEstimatedDateName = "ETD",
                    containerActualDateName = "ATD",
                    shipmentDirection = "from",
                };
                CheckIsActaulDateFilled(discreapancyParams);
            }

            if (code == "POLMainCarriage")
            {
                var preCarriageParams = new
                {
                    containerPM = containerPM,
                    shipmentPM = shipmentPM,
                    containerCode = code.Substring(0, 3),
                    shipmentCode = code.Substring(3),
                    containerLocation = containerPM.POLLocation,
                    shipmentLocation = shipmentPM.MainCarriageFromPortId,
                    containerActualDate = containerPM.ActualPOLVesselDeparture,
                    shipmentActualDate = shipmentPM.MainCarriageATD,
                    containerEstimatedDateName = "ETD",
                    containerActualDateName = "ATD",
                    shipmentDirection = "from",
                };
                CheckIsSameLocation(preCarriageParams);            
            }
            
            if(code == "PODMainCarriage")
            {
                var preCarriageParams = new
                {
                    containerPM = containerPM,
                    shipmentPM = shipmentPM,
                    containerCode = code.Substring(0, 3),
                    shipmentCode = code.Substring(3),
                    containerLocation = containerPM.PODLocation,
                    shipmentLocation = shipmentPM.MainCarriageFinalDestinationPortId,
                    containerActualDate = containerPM.ActualPODVesselArrival,
                    shipmentActualDate = shipmentPM.MainCarriageATA,
                    containerEstimatedDateName = "ETA",
                    containerActualDateName = "ATA",
                    shipmentDirection = "to",
                };
                CheckIsSameLocation(preCarriageParams);              
            }

            if (code == "PODOnCarriage") 
            {
                var discreapancyParams = new
                {
                    containerPM = containerPM,
                    shipmentPM = shipmentPM,
                    containerCode = code.Substring(0, 3),
                    shipmentCode = code.Substring(3),
                    containerActualDate = containerPM.ActualPODVesselArrival,
                    shipmentActualDate = shipmentPM.OnCarriageATA,
                    containerEstimatedDateName = "ETD",
                    containerActualDateName = "ATD",
                    shipmentDirection = "to",
                };
                CheckIsActaulDateFilled(discreapancyParams);
            }
        }
        private void CheckIsSameLocationUsingId(dynamic discrepancyParams)
        {
            if (string.IsNullOrEmpty(discrepancyParams.shipmentLocation) || !IsSameLocationUsingId(discrepancyParams.shipmentLocation, discrepancyParams.containerLocationId))
            {
                AddNotSameLocationDiscrepancy(discrepancyParams, discrepancyParams.containerEstimatedDateName);
                AddNotSameLocationDiscrepancy(discrepancyParams, discrepancyParams.containerActualDateName);
            }
            else
            {
                CheckIsActaulDateFilled(discrepancyParams);
            }
        }
        private void CheckIsSameLocation(dynamic discrepancyParams)
        {
            if (string.IsNullOrEmpty(discrepancyParams.shipmentLocation) || !IsSameLocation(discrepancyParams.shipmentLocation, discrepancyParams.containerLocation))
            {
                AddNotSameLocationDiscrepancy(discrepancyParams, discrepancyParams.containerEstimatedDateName);
                AddNotSameLocationDiscrepancy(discrepancyParams, discrepancyParams.containerActualDateName); 
            }
            else
            {
                CheckIsActaulDateFilled(discrepancyParams);
            }
        }
        private void CheckIsActaulDateFilled(dynamic discrepancyParams)
        {
            if (discrepancyParams.shipmentActualDate != null && discrepancyParams.containerActualDate && discrepancyParams.shipmentActualDate != discrepancyParams.containerActualDate)
            {
                AddActualDateDiscrepancy(discrepancyParams);
            }
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
            if (GetPropValue(shipmentPM, "Transshipment" + transshipmentLegIndex + "ToPortId") == null || !IsSameLocationUsingId((string)GetPropValue(shipmentPM, "Transshipment" + transshipmentLegIndex + "ToPortId"), portId))
            {
                AddNotSametransshipmentLegDiscrepancy(containerPM, shipmentPM, direction, transshipmentLegIndex, portId);
            }
            else 
            {
                var transshipmentATAInShipment = GetPropValue(shipmentPM, "Transshipment" + transshipmentLegIndex + "ATA");

                if (updatedFields.ActualDate != null && transshipmentATAInShipment != null && (DateTime)transshipmentATAInShipment != updatedFields.ActualDate)
                {
                    var discrepancyReason = $@"Transshipment{transshipmentLegIndex} ATA already has a value of {transshipmentATAInShipment} - did not update new container value {updatedFields.ActualDate}.";
                    AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);
                }
            }
        }
        private void AddNotSameLocationDiscrepancy (dynamic discrepancyParams, string time)
        {
            var shipmentUnloCode = GetUnloCodeFromPortId(discrepancyParams.shipmentLocation);
            var discrepancyReason = "Shipment " + discrepancyParams.shipmentCode + discrepancyParams.shipmentDirection + " port is empty - shipment " + time + " not updated.";
            if (shipmentUnloCode != null)
            {
                discrepancyReason = $@"Shipment {discrepancyParams.shipmentCode} {discrepancyParams.shipmentDirection} port {shipmentUnloCode} not equal to container {discrepancyParams.containerCode} port {discrepancyParams.containerLocation} - shipment {time} not updated.";
            }

            AddContainerDiscrepancyToService(discrepancyParams.containerPM, discrepancyParams.shipmentPM, discrepancyReason);
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
                discrepancyReason = $@"Transshipment{transshipmentLegIndex} {direction} port {shipmentUnloCode} not equal to container Transshipment{transshipmentLegIndex} {direction} port {containerUnloCode} - shipment ETA not updated.";
            }

            AddContainerDiscrepancyToService(containerPM, shipmentPM, discrepancyReason);
        }
        private string getTransshipmentUnloCode(ShipmentPM shipmentPM, int? transshipmentLegIndex)
        {
            if (transshipmentLegIndex == 1) return GetUnloCodeFromPortId(shipmentPM.Transshipment1FromPortId);
            if (transshipmentLegIndex == 2) return GetUnloCodeFromPortId(shipmentPM.Transshipment2FromPortId);

            return GetUnloCodeFromPortId(shipmentPM.Transshipment3FromPortId);
        }
        private void AddActualDateDiscrepancy(dynamic discrepancyParams)
        {
            var actualdateName = discrepancyParams.containerCode + " " + discrepancyParams.containerActualDateName;
            
            var discrepancyReason = $@"Shipment {actualdateName} already has a value of {discrepancyParams.shipmentActualDate} - did not update new container value {discrepancyParams.containerActualDate}.";

            AddContainerDiscrepancyToService(discrepancyParams.containerPM, discrepancyParams.shipmentPM, discrepancyReason);
        }
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        private void AddContainerDiscrepancyToService(ContainerPM containerPM, ShipmentPM shipmentPM, string reasonOfDiscrepancy)
        {
            this.containerDiscrepancyService = new ContainerDiscrepancyService(containerPM.Tenant);
            if (CheckIsDiscrepancyExist(containerPM.Id, reasonOfDiscrepancy)) return;
            this.containerDiscrepancyService.Create(containerPM, shipmentPM, reasonOfDiscrepancy);
        }
        private bool CheckIsDiscrepancyExist(string containerId, string containerDiscrepancy)
        {
            var discrepancy = this.containerDiscrepancyService.GetContainerDiscrepancyByContainerIdAndDiscrepancyReason(containerId, containerDiscrepancy);
            if (discrepancy != null) return true;
            return false;
        }
    }
}