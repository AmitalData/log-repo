using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.ContainerTracking
{
    public class ContainerTrackingUpdateManager
    {
        private ContainerUpdatedFields containerUpdatedFields;
        private int tenant;
        private ContainerPM containerPM;
        private ShipmentPM shipmentPM;
        private PortRepository portRepository;
        private TenantRepository tenantRepository;
        private PortQuery portQuery;
        private VesselRepository vesselRepository;
        private string POLShipmentUpdateIndicator;
        private string PODShipmentUpdateIndicator;
        private bool isUpdatingPackages = false;
        private bool isUpdatingShipmentDateFields = false;
        private bool isUpdatingEmptyLeg = false;
        private Tenant myTenant;
        public List<dynamic> allShipmentTrasshipmentLegs;
        private ContainerTrackingHelper containerTrackingHelper;

        public ContainerTrackingUpdateManager(ContainerUpdatedFields containerUpdatedFields)
        {

            this.allShipmentTrasshipmentLegs = new List<dynamic>();
            this.containerUpdatedFields = containerUpdatedFields;
            this.containerPM = containerUpdatedFields.ContainerPM;
            this.shipmentPM = containerUpdatedFields.ShipmentPM;
            
        }

        public void Initialize(int tenant)
        {
            this.tenant = tenant;
            this.portRepository = new PortRepository(tenant);
            this.portQuery = new PortQuery(portRepository);
            this.vesselRepository = new VesselRepository(tenant);
            this.tenantRepository = new TenantRepository(tenant);
            containerTrackingHelper = new ContainerTrackingHelper(tenant);
        }
        public void Update(bool isUpatingContainer, bool isUpdatingShipment)
        {
            if (!isUpatingContainer) return;

            this.UpdateContainer();
            this.UpdatePackage();
            this.UpdateEmptyReturnLeg();
            this.UpdateShipment(isUpdatingShipment);
            this.SaveShipment();
        }
        private void UpdateContainer()
        {
            MapContainerFields();
            MapConcurrencyFields();
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
            {
                MapVizionPreCarriage();
                MapVizionOnCarriage();
            }
            else
            {
                MapOnCarriage();
                MapPreCarriage();
            }

            SaveContainer();
        }
        private void MapContainerFields()
        {
            this.FillFieldsNewValues("MainCarriageETD", containerUpdatedFields.MainCarriageETD, containerPM);
            this.FillFieldsNewValues("MainCarriageETA", containerUpdatedFields.MainCarriageETA, containerPM);
            this.FillFieldsNewValues("MainCarriageATD", containerUpdatedFields.MainCarriageATD, containerPM);
            this.FillFieldsNewValues("MainCarriageATA", containerUpdatedFields.MainCarriageATA, containerPM);
            this.FillFieldsNewValues("EmptyPickupLocation", containerUpdatedFields.EmptyPickupLocation, containerPM);
            this.FillFieldsNewValues("EstimatedEmptyPickupDate", containerUpdatedFields.EstimatedEmptyPickupDate, containerPM);
            this.FillFieldsNewValues("ActualEmptyPickupDate", containerUpdatedFields.ActualEmptyPickupDate, containerPM);
            this.FillFieldsNewValues("EstimatedPOLArrival", containerUpdatedFields.EstimatedPOLArrival, containerPM);
            this.FillFieldsNewValues("ActualPOLArrival", containerUpdatedFields.ActualPOLArrival, containerPM);
            this.FillFieldsNewValues("DepartureLocation", containerUpdatedFields.DepartureLocation, containerPM);
            this.FillFieldsNewValues("DestinationLocation", containerUpdatedFields.DestinationLocation, containerPM);

            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                containerPM.IsUpdatedVizionAnalyzer = true;
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                containerPM.IsUpdatedOceanInsightsAnalyzer = true;

            containerPM.CurrentStatus = containerUpdatedFields.CurrentStatus;
            containerPM.CurrentLocation = containerUpdatedFields.CurrentLocation;
            containerPM.CurrentStatusDate = containerUpdatedFields.CurrentStatusDate;
            containerPM.HasContainerException = containerUpdatedFields.HasContainerException;
            containerPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            containerPM.POLLocation = containerUpdatedFields.POLLocation;
            containerPM.EstimatedPOLLoaded = containerUpdatedFields.EstimatedPOLLoaded;
            containerPM.ActualPOLLoaded = containerUpdatedFields.ActualPOLLoaded;
            containerPM.EstimatedPOLVesselDeparture = containerUpdatedFields.EstimatedPOLVesselDeparture;
            containerPM.ActualPOLVesselDeparture = containerUpdatedFields.ActualPOLVesselDeparture;
            containerPM.TransshipmentCount = containerUpdatedFields.TransshipmentCount;
            containerPM.Leg5Vessel = containerUpdatedFields.Leg5Vessel;
            containerPM.Leg5VesselId = containerUpdatedFields.Leg5VesselId;
            containerPM.Leg5Voyage = containerUpdatedFields.Leg5Voyage;
            containerPM.PODLocation = containerUpdatedFields.PODLocation;
            containerPM.EstimatedPODVesselArrival = containerUpdatedFields.EstimatedPODVesselArrival;
            containerPM.ActualPODVesselArrival = containerUpdatedFields.ActualPODVesselArrival;
            containerPM.EstimatedPODDischarge = containerUpdatedFields.EstimatedPODDischarge;
            containerPM.ActualPODDischarge = containerUpdatedFields.ActualPODDischarge;
            containerPM.EstimatedPODDeparture = containerUpdatedFields.EstimatedPODDeparture;
            containerPM.ActualPODDeparture = containerUpdatedFields.ActualPODDeparture;
            containerPM.OnCarriageETD = containerUpdatedFields.EstimatedDelivery;
            containerPM.OnCarriageATD = containerUpdatedFields.ActualDelivery;
            containerPM.LIFLocation = containerUpdatedFields.LIFLocation;
            containerPM.EstimatedOnCarriageDeparture = containerUpdatedFields.EstimatedOnCarriageDeparture;
            containerPM.ActualOnCarriageDeparture = containerUpdatedFields.ActualOnCarriageDeparture;
            containerPM.EmptyReturnLocation = containerUpdatedFields.EmptyReturnLocation;
            containerPM.EstimatedEmptyReturn = containerUpdatedFields.EstimatedEmptyReturn;
            containerPM.ActualEmptyReturn = containerUpdatedFields.ActualEmptyReturn;
            containerPM.CustomsReleaseState = containerUpdatedFields.CustomsReleaseState;
            containerPM.CustomsReleaseDate = containerUpdatedFields.CustomsReleaseDate;
            containerPM.CarrierReleaseState = containerUpdatedFields.CarrierReleaseState;
            containerPM.CarrierReleaseDate = containerUpdatedFields.CarrierReleaseDate;
            containerPM.AvailablityDate = containerUpdatedFields.AvailablityDate;
            containerPM.AvailabilityLocation = containerUpdatedFields.AvailabilityLocation;
            containerPM.EmptyPickupLocationPortId = this.GetPortId(containerUpdatedFields.EmptyPickupLocation);
            containerPM.EmptyReturnLocationPortId = this.GetPortId(containerUpdatedFields.EmptyReturnLocation);
            containerPM.AvailabilityLocationPortId = this.GetPortId(containerUpdatedFields.AvailabilityLocation);
            containerPM.LIFLocationPortId = this.GetPortId(containerUpdatedFields.LIFLocation);
            containerPM.POLLocationPortId = this.GetPortId(containerUpdatedFields.POLLocation);
            containerPM.PODLocationPortId = this.GetPortId(containerUpdatedFields.PODLocation);
            containerPM.IsAutomaticUpdates = true;
            this.MapTransshipments();
        }
        private void MapPreCarriage()
        {
            containerPM.PreCarriageLocation = containerUpdatedFields.OriginLocation;
            containerPM.PreCarriageLocationPortId = this.GetPortId(containerUpdatedFields.OriginLocation);
            containerPM.PreCarriageETD = containerUpdatedFields.EstimatedOriginPickup;
            containerPM.PreCarriageATD = containerUpdatedFields.ActualOriginPickup;
        }
        private void MapOnCarriage()
        {
            containerPM.OnCarriageLocation = containerUpdatedFields.DeliveryLocation;
            containerPM.OnCarriageLocationPortId = this.GetPortId(containerUpdatedFields.DeliveryLocation);
            containerPM.EstimatedLIFArrival = containerUpdatedFields.EstimatedLIFArrival;
            containerPM.ActualLIFArrival = containerUpdatedFields.ActualLIFArrival;
        }
        private void MapVizionPreCarriage()
        {
            this.MapVizionPreCarriageLocation();
            this.MapVizionPreCarriageDates();
        }
        private void MapVizionPreCarriageLocation()
        {
            containerPM.PreCarriageLocationPortId = this.GetPortId(containerUpdatedFields.VisionPreCarriage);
            containerPM.PreCarriageLocation = containerUpdatedFields.VisionPreCarriage;
        }
        private void MapVizionPreCarriageDates()
        {

            if (containerUpdatedFields.OriginLocation == null) return;
            string portId = this.GetPortId(containerUpdatedFields.OriginLocation);

            if (!containerTrackingHelper.IsSameLocationUsingId(portId, containerPM.PreCarriageLocationPortId)) return;

            containerPM.PreCarriageETD = containerUpdatedFields.EstimatedOriginPickup;
            containerPM.PreCarriageATD = containerUpdatedFields.ActualOriginPickup;
            containerTrackingHelper.AddContainerDiscrepancy("PreCarriage", containerPM, shipmentPM);
            if (!containerTrackingHelper.IsSameLocationUsingId(containerPM.PreCarriageLocationPortId, shipmentPM.PreCarriageFromPortId)) return;

            shipmentPM.PreCarriageETD = containerPM.PreCarriageETD;
            shipmentPM.PreCarriageATD = shipmentPM.PreCarriageATD ?? containerPM.PreCarriageATD;

        }
        private void MapVizionOnCarriage()
        {
            this.MapVizionOnCarriageLocation();
            this.MapVizionOnCarriageDates();
        }
        private void MapVizionOnCarriageLocation()
        {
            containerPM.OnCarriageLocationPortId = this.GetPortId(containerUpdatedFields.VisionOnCarriage);
            containerPM.OnCarriageLocation = containerUpdatedFields.VisionOnCarriage;
        }
        private void MapVizionOnCarriageDates()
        {
            if (containerUpdatedFields.OnCarriageLocation == null) return;
            string portId = this.GetPortId(containerUpdatedFields.OnCarriageLocation);

            if (!containerTrackingHelper.IsSameLocationUsingId(portId, containerPM.OnCarriageLocationPortId)) return;

            containerPM.OnCarriageETA = containerUpdatedFields.OnCarriageETA;
            containerPM.OnCarriageATA = containerUpdatedFields.OnCarriageATA;

            containerTrackingHelper.AddContainerDiscrepancy("OnCarriage", containerPM, shipmentPM);

            if (!containerTrackingHelper.IsSameLocationUsingId(containerPM.OnCarriageLocationPortId, shipmentPM.OnCarriageToPortId)) return;

            shipmentPM.OnCarriageETA = containerPM.OnCarriageETA;

            shipmentPM.OnCarriageATA = shipmentPM.OnCarriageATA ?? containerPM.OnCarriageATA;
        }
        public void SetContainer(ContainerPM containerPM)
        {
            this.containerPM = containerPM;
            this.tenant = containerPM.Tenant;
        }
        public void SetShipment(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.tenant = shipmentPM.Tenant;
            this.GetTenant();
        }
        private void GetTenant()
        {
            myTenant = tenantRepository.GetSingleTenant(tenant);
        }
        private void MapTransshipments()
        {
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                this.MapVizionTransshipments();
            else if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                this.MapOITransshipments();
        }
        private void MapVizionTransshipments()
        {
            this.FillFirstContainerLegVesselVoyage();
            this.FillContainerTransshipmentLeg(containerUpdatedFields.LoadedTransshipment);
            this.FillContainerTransshipmentLeg(containerUpdatedFields.VesselDeparted);
            this.FillContainerTransshipmentLeg(containerUpdatedFields.VesselArrived);
            this.FillContainerTransshipmentLeg(containerUpdatedFields.DischargedTransshipment);
            this.FillLastContainerLegVesselVoyage();
            this.SetShipmentTransshipmentLegDates();
            this.SetShipmentVesselVoyageLegs();
        }

        private void FillFirstContainerLegVesselVoyage()
        {
            Vessel polLegVessel = this.GetVessel(containerUpdatedFields.POLLegVessel);
            this.FillContqainerVesselVoyage(polLegVessel?.EnglishName, containerUpdatedFields.POLLegVoyage, 1);
        }

        private void FillContainerTransshipmentLeg(MilestoneData transshipmentObject)
        {
            foreach (MilestoneDataUpdatedFields updatedFields in transshipmentObject.MilestoneFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = this.GetPortId(updatedFields.Location);

                int? transshipmentLegIndex = GetCorrespondingShipmentLegIndex(portId);
                if (transshipmentLegIndex == null || string.IsNullOrEmpty(portId)) continue;

                this.FillFieldsNewValues("Transshipment" + transshipmentLegIndex + "LocationPortId", portId, containerPM);
                this.FillFieldsNewValues("Transshipment" + transshipmentLegIndex + "Location", updatedFields.Location, containerPM);
                this.FillContqainerVesselVoyage(updatedFields.Vessel, updatedFields.Voyage, transshipmentLegIndex);
                this.SetTransshipmentLegDates(transshipmentLegIndex, updatedFields, transshipmentObject.Key);
            }
        }

        private void FillContqainerVesselVoyage(string vesselName, string voyage, int? transshipmentLegIndex)
        {
            this.FillFieldsNewValues("Leg" + transshipmentLegIndex + "Vessel", vesselName, containerPM);
            this.FillFieldsNewValues("Leg" + transshipmentLegIndex + "Voyage", voyage, containerPM);

            Vessel vessel = this.GetVessel(vesselName);
            if (vessel == null) return;
           
            this.FillFieldsNewValues("Leg" + transshipmentLegIndex + "VesselId", vessel?.Id, containerPM);
        }

        private int? GetCorrespondingShipmentLegIndex(string portId)
        {
            if (containerPM.ShipmentTransshipment1FromId == portId)
                return 1;

            else if (containerPM.ShipmentTransshipment2FromId == portId)
                return 2;

            else if (containerPM.ShipmentTransshipment3FromId == portId)
                return 3;

            return null;
        }
        private void FillLastContainerLegVesselVoyage()
        {
            var lastContainerLegIndex = GetLastContainerLegIndex();
            if (lastContainerLegIndex == 1 )
            {
                Vessel podLegVessel = this.GetVessel(containerUpdatedFields.PODLegVessel);
                this.FillContqainerVesselVoyage(podLegVessel?.EnglishName, containerUpdatedFields.PODLegVoyage, lastContainerLegIndex);
            }
            else if (lastContainerLegIndex != null && lastContainerLegIndex != 4)
            {
                Vessel podLegVessel = this.GetVessel(containerUpdatedFields.PODLegVessel);
                this.FillContqainerVesselVoyage(podLegVessel?.EnglishName, containerUpdatedFields.PODLegVoyage, lastContainerLegIndex + 1);
            }
        }
        private int? GetLastContainerLegIndex()
        {
            if (!string.IsNullOrEmpty((string)GetPropValue(containerPM, "Leg4VesselId")))
                return 4;

            if (!string.IsNullOrEmpty((string)GetPropValue(containerPM, "Leg3VesselId")))
                return 3;

            if (!string.IsNullOrEmpty((string)GetPropValue(containerPM, "Leg2VesselId")))
                return 2;

            if (!string.IsNullOrEmpty((string)GetPropValue(containerPM, "Leg1VesselId")))
                return 1;

            return null;
        }

        private void SetShipmentTransshipmentLegDates()
        {
            FillLegsMilestoneData();
            FillShipmentDates();
        }
        private void FillLegsMilestoneData()
        {
            FillShipmentVesselDepartedMilestoneFields();
            FillShipmentVesselArrivedMilestoneFields();
            FillShipmentLoadedTransshipmentMilestoneFields();
            FillShipmentDischargedTransshipmentMilestoneFields();
        }
        private void FillShipmentVesselDepartedMilestoneFields()
        {
            foreach (MilestoneDataUpdatedFields updatedFields in containerUpdatedFields.VesselDeparted.MilestoneFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = this.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(containerUpdatedFields.VesselDeparted.Key), Key = containerUpdatedFields.VesselDeparted.Key};
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentVesselArrivedMilestoneFields()
        {
            foreach (MilestoneDataUpdatedFields updatedFields in containerUpdatedFields.VesselArrived.MilestoneFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = this.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(containerUpdatedFields.VesselArrived.Key), Key = containerUpdatedFields.VesselArrived.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentLoadedTransshipmentMilestoneFields()
        {
            foreach (MilestoneDataUpdatedFields updatedFields in containerUpdatedFields.LoadedTransshipment.MilestoneFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = this.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(containerUpdatedFields.LoadedTransshipment.Key), Key = containerUpdatedFields.LoadedTransshipment.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentDischargedTransshipmentMilestoneFields()
        {
            foreach (MilestoneDataUpdatedFields updatedFields in containerUpdatedFields.DischargedTransshipment.MilestoneFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = this.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(containerUpdatedFields.DischargedTransshipment.Key), Key = containerUpdatedFields.DischargedTransshipment.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentDates()
        {
            allShipmentTrasshipmentLegs.Where(a=>a.Key != "LoadedTransshipment" && a.Key != "DischargedTransshipment").ToList().ForEach(leg =>
            {
                dynamic transshipmentLeg = GetShipmentLegDates(leg);

                if (transshipmentLeg == null) return;

                string portId = leg.PortId;
                var updatedFields = leg.MilestoneData;
                string dateType = "ETD";
                if (leg.Direction == "To") dateType = "ETA";

                if (transshipmentLeg.Index != 0)
                    containerTrackingHelper.AddTranshipmentDiscrepancyContainer(transshipmentLeg.Index, leg.Direction, containerPM, shipmentPM, portId, updatedFields, dateType);

                if (!containerTrackingHelper.IsSameLocationUsingId((string)GetPropValue(shipmentPM, transshipmentLeg.PortField), portId)) return;

                this.FillFieldsNewValues(transshipmentLeg.EstimatedDateField, updatedFields.EstimatedDate, shipmentPM);
                var transshipmentATDInShipment = GetPropValue(shipmentPM, transshipmentLeg.ActualDateField);
                if (transshipmentATDInShipment == null) this.FillFieldsNewValues(transshipmentLeg.ActualDateField, updatedFields.ActualDate, shipmentPM);
            });
        }
        private string GetPortLegDirection(string key)
        {
            if (key == "VesselDeparted" || key == "LoadedTransshipment")
            {
                return "From";
            }
            if (key == "VesselArrived" || key == "DischargedTransshipment")
            {
                return "To";
            }
            return null;
        }
        private dynamic GetShipmentLegDates(dynamic leg)
        {
            if (leg.Direction == "From")
            {
                return HandelFromShipmentLegsDates(leg);
            }
            else if (leg.Direction == "To")
            {
                return HandelToShipmentLegsDates(leg);
            }

            return null;
        }
        private dynamic HandelFromShipmentLegsDates(dynamic leg)
        {
            if (shipmentPM.Transshipment1FromPortId == leg.PortId)
                return new { Index = 1, PortField = "Transshipment1FromPortId", EstimatedDateField = "Transshipment1ETD", ActualDateField = "Transshipment1ATD" };

            if (shipmentPM.Transshipment2FromPortId == leg.PortId)
                return new { Index = 2, PortField = "Transshipment2FromPortId", EstimatedDateField = "Transshipment2ETD", ActualDateField = "Transshipment2ATD" };

            if (shipmentPM.Transshipment3FromPortId == leg.PortId)
                return new { Index = 3, PortField = "Transshipment3FromPortId", EstimatedDateField = "Transshipment3ETD", ActualDateField = "Transshipment3ATD" };

            return null;
        }
        private dynamic HandelToShipmentLegsDates(dynamic leg)
        {
            if (shipmentPM.MainCarriageToPortId == leg.PortId)
                return new { Index = 0, PortField = "MainCarriageToPortId", EstimatedDateField = "MainCarriageETA", ActualDateField = "MainCarriageATA" };

            if (shipmentPM.Transshipment1ToPortId == leg.PortId)
                return new { Index = 1, PortField = "Transshipment1ToPortId", EstimatedDateField = "Transshipment1ETA", ActualDateField = "Transshipment1ATA" };

            if (shipmentPM.Transshipment2ToPortId == leg.PortId)
                return new { Index = 2, PortField = "Transshipment2ToPortId", EstimatedDateField = "Transshipment2ETA", ActualDateField = "Transshipment2ATA" };

            if (shipmentPM.Transshipment3ToPortId == leg.PortId)
                return new { Index = 3, PortField = "Transshipment3ToPortId", EstimatedDateField = "Transshipment3ETA", ActualDateField = "Transshipment3ATA" };

            return null;
        }
        private void SetShipmentVesselVoyageLegs()
        {
            int? index = null;
            for (int i = 0; i < 4; i++)
            {
                if (i != 0) index = i;

                var fieldName = "Transshipment";
                if (index == null) fieldName = "MainCarriage";

                if (!IsTheSameFromToLocations(fieldName, index)) return;

                string legVesselId = (string)GetPropValue(containerPM, "Leg" + (i + 1) + "VesselId");
                string legVesselName = (string)GetPropValue(containerPM, "Leg" + (i + 1) + "Vessel");

                if (!string.IsNullOrEmpty(legVesselId)) FillShipmentVessel(legVesselName, legVesselId, index);
                string legVoyage = (string)GetPropValue(containerPM, "Leg" + (i + 1) + "Voyage");

                if (!string.IsNullOrEmpty(legVoyage)) FillShipmentVoyage(legVoyage, index);
            }
        }
        private bool IsTheSameFromToLocations(string fieldName, int? index)
        {
            string shipmentFromPortIdField = (string)GetPropValue(shipmentPM, fieldName + index + "FromPortId");
            string shipmentToPortIdField = (string)GetPropValue(shipmentPM, fieldName + index + "ToPortId");
            var istheSameFromLocation = false;
            var istheSameToLocation = false;

            if (allShipmentTrasshipmentLegs.Count() == 0)
            {
                istheSameFromLocation = containerTrackingHelper.IsSameLocation(shipmentFromPortIdField, containerUpdatedFields.POLLocation);
                istheSameToLocation = containerTrackingHelper.IsSameLocation(shipmentToPortIdField, containerUpdatedFields.PODLocation);
            }
            else
            {
                istheSameFromLocation = containerTrackingHelper.IsSameLocation(shipmentFromPortIdField, containerUpdatedFields.POLLocation);
                istheSameToLocation = allShipmentTrasshipmentLegs.Where(a => a.Key == "VesselArrived" || a.Key == "DischargedTransshipment" && a.PortId == shipmentToPortIdField).Any();

                if (index != null)
                {
                    istheSameFromLocation = allShipmentTrasshipmentLegs.Where(a => a.Key == "VesselDeparted" || a.Key == "LoadedTransshipment" && a.PortId == shipmentFromPortIdField).Any();
                    istheSameToLocation = allShipmentTrasshipmentLegs.Where(a => a.Key == "VesselArrived" || a.Key == "DischargedTransshipment" && a.PortId == shipmentToPortIdField).Any();
                }
            }

            if (istheSameFromLocation && istheSameToLocation) return true;

            return false;
        }
        private void FillShipmentVessel(string vessel, string vesselId, int? index)
        {
            var fieldName = "Transshipment";
            if (index == null) fieldName = "MainCarriage";
            
            if (string.IsNullOrEmpty((string)GetPropValue(shipmentPM, fieldName + index + "VesselName")))
                this.FillFieldsNewValues(fieldName + index + "VesselName", vessel, shipmentPM);

            if (string.IsNullOrEmpty((string)GetPropValue(shipmentPM, fieldName + index + "VesselId")))
                this.FillFieldsNewValues(fieldName + index + "VesselId", vesselId, shipmentPM);
        }
        private void FillShipmentVoyage(string voyage, int? index)
        {
            var fieldName = "Transshipment";
            if (index == null) fieldName = "MainCarriage";

            if (string.IsNullOrEmpty((string)GetPropValue(shipmentPM, fieldName + index + "CarrierNumber")))
                this.FillFieldsNewValues(fieldName + index + "CarrierNumber", voyage, shipmentPM);

            if (fieldName == "Transshipment")
            {
                if (string.IsNullOrEmpty((string)GetPropValue(shipmentPM, fieldName + index + "CarrierId")))
                {
                    var mainCarriageCarrierId = (string)GetPropValue(shipmentPM, "MainCarriageCarrierId");
                    this.FillFieldsNewValues(fieldName + index + "CarrierId", mainCarriageCarrierId, shipmentPM);
                }
            }
        }
        private void MapOITransshipments()
        {
            containerPM.Transshipment1Location = containerUpdatedFields.Transshipment1Location;
            containerPM.Transshipment1LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment1Location);
            containerPM.EstimatedTrans1VesselArrival = containerUpdatedFields.EstimatedTrans1VesselArrival;
            containerPM.ActualTransshipment1VesselArrival = containerUpdatedFields.ActualTransshipment1VesselArrival;
            containerPM.EstimatedTransshipment1Discharge = containerUpdatedFields.EstimatedTransshipment1Discharge;
            containerPM.ActualTransshipment1Discharge = containerUpdatedFields.ActualTransshipment1Discharge;
            containerPM.EstimatedTransshipment1Loaded = containerUpdatedFields.EstimatedTransshipment1Loaded;
            containerPM.ActualTransshipment1Loaded = containerUpdatedFields.ActualTransshipment1Loaded;
            containerPM.EstimatedTrans1VesselDeparture = containerUpdatedFields.EstimatedTrans1VesselDeparture;
            containerPM.ActualTrans1VesselDeparture = containerUpdatedFields.ActualTrans1VesselDeparture;
            containerPM.Leg1Vessel = containerUpdatedFields.Leg1Vessel;
            containerPM.Leg1VesselId = containerUpdatedFields.Leg1VesselId;
            containerPM.Leg1Voyage = containerUpdatedFields.Leg1Voyage;

            containerPM.Transshipment2Location = containerUpdatedFields.Transshipment2Location;
            containerPM.Transshipment2LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment2Location);
            containerPM.EstimatedTrans2VesselArrival = containerUpdatedFields.EstimatedTrans2VesselArrival;
            containerPM.ActualTransshipment2VesselArrival = containerUpdatedFields.ActualTransshipment2VesselArrival;
            containerPM.EstimatedTransshipment2Discharge = containerUpdatedFields.EstimatedTransshipment2Discharge;
            containerPM.ActualTransshipment2Discharge = containerUpdatedFields.ActualTransshipment2Discharge;
            containerPM.EstimatedTransshipment2Loaded = containerUpdatedFields.EstimatedTransshipment2Loaded;
            containerPM.ActualTransshipment2Loaded = containerUpdatedFields.ActualTransshipment2Loaded;
            containerPM.EstimatedTrans2VesselDeparture = containerUpdatedFields.EstimatedTrans2VesselDeparture;
            containerPM.ActualTrans2VesselDeparture = containerUpdatedFields.ActualTrans2VesselDeparture;
            containerPM.Leg2Vessel = containerUpdatedFields.Leg2Vessel;
            containerPM.Leg2VesselId = containerUpdatedFields.Leg2VesselId;
            containerPM.Leg2Voyage = containerUpdatedFields.Leg2Voyage;

            containerPM.Transshipment3Location = containerUpdatedFields.Transshipment3Location;
            containerPM.Transshipment3LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment3Location);
            containerPM.EstimatedTrans3VesselArrival = containerUpdatedFields.EstimatedTrans3VesselArrival;
            containerPM.ActualTransshipment3VesselArrival = containerUpdatedFields.ActualTransshipment3VesselArrival;
            containerPM.EstimatedTransshipment3Discharge = containerUpdatedFields.EstimatedTransshipment3Discharge;
            containerPM.ActualTransshipment3Discharge = containerUpdatedFields.ActualTransshipment3Discharge;
            containerPM.EstimatedTransshipment3Loaded = containerUpdatedFields.EstimatedTransshipment3Loaded;
            containerPM.ActualTransshipment3Loaded = containerUpdatedFields.ActualTransshipment3Loaded;
            containerPM.EstimatedTrans3VesselDeparture = containerUpdatedFields.EstimatedTrans3VesselDeparture;
            containerPM.ActualTrans3VesselDeparture = containerUpdatedFields.ActualTrans3VesselDeparture;
            containerPM.Leg3Vessel = containerUpdatedFields.Leg3Vessel;
            containerPM.Leg3VesselId = containerUpdatedFields.Leg3VesselId;
            containerPM.Leg3Voyage = containerUpdatedFields.Leg3Voyage;

            containerPM.Transshipment4Location = containerUpdatedFields.Transshipment4Location;
            containerPM.Transshipment4LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment4Location);
            containerPM.EstimatedTrans4VesselArrival = containerUpdatedFields.EstimatedTrans4VesselArrival;
            containerPM.ActualTransshipment4VesselArrival = containerUpdatedFields.ActualTransshipment4VesselArrival;
            containerPM.EstimatedTransshipment4Discharge = containerUpdatedFields.EstimatedTransshipment4Discharge;
            containerPM.ActualTransshipment4Discharge = containerUpdatedFields.ActualTransshipment4Discharge;
            containerPM.EstimatedTransshipment4Loaded = containerUpdatedFields.EstimatedTransshipment4Loaded;
            containerPM.ActualTransshipment4Loaded = containerUpdatedFields.ActualTransshipment4Loaded;
            containerPM.EstimatedTrans4VesselDeparture = containerUpdatedFields.EstimatedTrans4VesselDeparture;
            containerPM.ActualTrans4VesselDeparture = containerUpdatedFields.ActualTrans4VesselDeparture;
            containerPM.Leg4Vessel = containerUpdatedFields.Leg4Vessel;
            containerPM.Leg4VesselId = containerUpdatedFields.Leg4VesselId;
            containerPM.Leg4Voyage = containerUpdatedFields.Leg4Voyage;
        }
        private void SetTransshipmentLegDates(int? transshipmentLegIndex, MilestoneDataUpdatedFields updatedFields, string key)
        {
            if (key == "LoadedTransshipment")
            {
                this.FillFieldsNewValues("EstimatedTransshipment" + transshipmentLegIndex + "Loaded", updatedFields.EstimatedDate, containerPM);
                this.FillFieldsNewValues("ActualTransshipment" + transshipmentLegIndex + "Loaded", updatedFields.ActualDate, containerPM);
            }

            else if (key == "VesselArrived")
            {
                this.FillFieldsNewValues("EstimatedTrans" + transshipmentLegIndex + "VesselArrival", updatedFields.EstimatedDate, containerPM);
                this.FillFieldsNewValues("ActualTransshipment" + transshipmentLegIndex + "VesselArrival", updatedFields.ActualDate, containerPM);
            }

            else if (key == "VesselDeparted")
            {
                this.FillFieldsNewValues("EstimatedTrans" + transshipmentLegIndex + "VesselDeparture", updatedFields.EstimatedDate, containerPM);
                this.FillFieldsNewValues("ActualTrans" + transshipmentLegIndex + "VesselDeparture", updatedFields.ActualDate, containerPM);
            }

            else if (key == "DischargedTransshipment")
            {
                this.FillFieldsNewValues("EstimatedTransshipment" + transshipmentLegIndex + "Discharge", updatedFields.EstimatedDate, containerPM);
                this.FillFieldsNewValues("ActualTransshipment" + transshipmentLegIndex + "Discharge", updatedFields.ActualDate, containerPM);
            }
        }
        private Vessel GetVessel(string vesselName)
        {
            return vesselRepository.GetSingleVesselByName(vesselName, containerPM.Tenant);
        }
        private void MapConcurrencyFields()
        {
            containerPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(containerPM.ShipmentId))
            {
                return;
            }
            containerPM.ShipmentConcurrencyGUID = containerUpdatedFields.ContainerRepository.GetConcurrencyGUIDByShipmentId(containerPM.ShipmentId, containerPM.Tenant);
            containerPM.ShipmentNewConcurrencyGUID = Guid.NewGuid().ToString();
        }
        private void SaveContainer()
        {
            containerPM.RecentResponseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            ContainerService containerService = new ContainerService(containerUpdatedFields.ShipmentContext, tenant);
            containerService.Update(containerPM, containerUpdatedFields.ContainersExternal);
        }
        private void UpdatePackage()
        {
            this.isUpdatingPackages = false;
            DateTime todatDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? eventData = containerUpdatedFields.EventDate;
            ShipmentPackagePM package = shipmentPM.ShipmentPackages.Where(a => a.Id == containerUpdatedFields.ShipmentPackageId).FirstOrDefault();
            string oceanInsightsSource = "OIN";
            if (package != null)
            {
                if (eventData == null)
                {
                    eventData = todatDate;
                }

                if (package.LastStatusDate == null || eventData > package.LastStatusDate)
                {
                    package.LastStatusCode = containerUpdatedFields.ContainerStatus;
                    package.LastStatusDate = eventData;
                    package.ContainerStatusSourceCode = oceanInsightsSource;
                    package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    this.isUpdatingPackages = true;
                }
            }
        }
        private void UpdateEmptyReturnLeg()
        {
            
            this.isUpdatingEmptyLeg = false;
            ShipmentDeliveryPM delivery = this.GetEmptyReturnLeg();
            
            if (delivery != null)
            {
                this.isUpdatingEmptyLeg = true;
                delivery.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                delivery.ETA = containerPM.EstimatedEmptyReturn;
                containerTrackingHelper.AddDeliveryContainerDiscrepancy(delivery, shipmentPM, containerPM);
                delivery.ATA = delivery.ATA ?? containerPM.ActualEmptyReturn;
            }
        }
        private ShipmentDeliveryPM GetEmptyReturnLeg()
        {
            ShipmentDeliveryPM shipmentDelivery = null;

            ShipmentPickUpDeliveryPackageRepository pickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(containerUpdatedFields.ShipmentContext);
            List<ShipmentPickUpDeliveryPackage> packages = pickUpDeliveryPackageRepository.GetShipmentPickUpDeliveryPackagesByContainerIdAndTenant(containerPM.Id, tenant);
            if (packages != null && packages.Count > 0)
            {
                List<string> deliveryPackagesIds = packages.Select(s => s.ShipmentPickUpDeliveryId).ToList();
                shipmentDelivery = shipmentPM.ShipmentDeliveries.Where(a => deliveryPackagesIds.Contains(a.Id) && a.PickUpDeliveryTypeCode == "EMPT").FirstOrDefault();
            }

            return shipmentDelivery;
        }
        private void UpdateShipment(bool isUpdatingShipment)
        {
            if (!isUpdatingShipment)
            {
               string location = "POL MainCarriage";
                containerTrackingHelper.AddContainerDiscrepancy(location, containerPM, shipmentPM);
                location = "POD MainCarriage";
                containerTrackingHelper.AddContainerDiscrepancy(location, containerPM, shipmentPM);
                return;
            }

            if (FeatureToggleHelper.HasFeatureToggle("OIU", tenant))
            {
                if (shipmentPM == null)
                {
                    throw new Exception("Analyzing shipment faild, shipment not found");
                }

                else
                {

                    this.POLShipmentUpdateIndicator = this.GetPOLShipmentUpdateIndicator(containerPM.POLLocationPortId);
                    this.PODShipmentUpdateIndicator = this.GetPODShipmentUpdateIndicator(containerPM.PODLocationPortId);

                    if (!string.IsNullOrEmpty(POLShipmentUpdateIndicator) || !string.IsNullOrEmpty(PODShipmentUpdateIndicator))
                    {
                        this.StartProcessingUpdateShipment();
                    }
                }
            }
        }
        private string GetPOLShipmentUpdateIndicator(string portId)
        {
            string POLShipmentUpdateIndicator = null;

            if (!string.IsNullOrEmpty(portId))
            {
                if (shipmentPM.PreCarriageFromPortId == portId)
                {
                    POLShipmentUpdateIndicator = "Pre Carriage";
                }

                else if (shipmentPM.MainCarriageFromPortId == portId)
                {
                    POLShipmentUpdateIndicator = "Main Carriage";
                }
            }

            return POLShipmentUpdateIndicator;
        }
        private string GetPODShipmentUpdateIndicator(string portId)
        {
            string PODShipmentUpdateIndicator = null;

            if (!string.IsNullOrEmpty(portId))
            {
                if (shipmentPM.OnCarriageToPortId == portId)
                {
                    PODShipmentUpdateIndicator = "On Carriage";
                }

                else if (shipmentPM.MainCarriageToPortId == portId)
                {
                    PODShipmentUpdateIndicator = "Main Carriage";
                }
            }

            return PODShipmentUpdateIndicator;
        }
        private void StartProcessingUpdateShipment()
        {
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                shipmentPM.IsUpdatedVizionAnalyzer = true;

            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                shipmentPM.IsUpdatedOceanInsightsAnalyzer = true;

            this.UpdateShipmentDates();

            if (this.isUpdatingShipmentDateFields)
            {
                shipmentPM.OINewConcurrencyGUID = Guid.NewGuid().ToString();
            }
        }
        private void UpdateShipmentDates()
        {
            this.UpdatePOLDates();
            this.UpdatePODDates();
        }
        private void UpdatePOLDates()
        {
            if (POLShipmentUpdateIndicator == "Pre Carriage")
            {
                containerTrackingHelper.AddContainerDiscrepancy("POL PreCarriage", containerPM, shipmentPM);
                this.FillFieldsShipmentNewValues("PreCarriageETD", containerPM.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.PreCarriageATD == null)
                {
                    this.FillFieldsShipmentNewValues("PreCarriageATD", containerPM.ActualPOLVesselDeparture, shipmentPM);
                   
                }
            }


            else if (POLShipmentUpdateIndicator == "Main Carriage")
            {
                containerTrackingHelper.AddContainerDiscrepancy("POL MainCarriage", containerPM, shipmentPM);
                if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                    shipmentPM.IsUpdatedVizionMainCarriageDates = true;
                if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                    shipmentPM.IsUpdatedOceanInsightsMainCarriageDates = true;

                this.FillFieldsShipmentNewValues("MainCarriageETD", containerPM.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.MainCarriageATD == null)
                {
                    this.FillFieldsShipmentNewValues("MainCarriageATD", containerPM.ActualPOLVesselDeparture, shipmentPM);
                }

            }
        }
        private void UpdatePODDates()
        {
            if (PODShipmentUpdateIndicator == "On Carriage")
            {
                containerTrackingHelper.AddContainerDiscrepancy("POD OnCarriage", containerPM, shipmentPM);
                this.FillFieldsShipmentNewValues("OnCarriageETA", containerPM.EstimatedPODVesselArrival, shipmentPM);

                if (shipmentPM.OnCarriageATA == null)
                {
                    this.FillOnCarriageATA();
                }
            }

            else if (PODShipmentUpdateIndicator == "Main Carriage")
            {
                if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                    shipmentPM.IsUpdatedVizionMainCarriageDates = true;

                if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                    shipmentPM.IsUpdatedOceanInsightsMainCarriageDates = true;

                containerTrackingHelper.AddContainerDiscrepancy("POD MainCarriage", containerPM, shipmentPM);
                this.FillFieldsShipmentNewValues("MainCarriageETA", containerPM.EstimatedPODVesselArrival, shipmentPM);
                if (shipmentPM.MainCarriageATA == null)
                {
                    this.FillMainCarriageATA();
                   
                }
            }
        }
        private void FillOnCarriageATA()
        {
            DateTime? myDate = containerPM.ActualPODVesselArrival;

            if (myTenant != null && myTenant.ShipmentATADateIndicator == "Container")
                myDate = containerPM.ActualPODDischarge;

            this.FillFieldsShipmentNewValues("OnCarriageATA", myDate, shipmentPM);
        }
        private void FillMainCarriageATA()
        {
            DateTime? myDate = containerPM.ActualPODVesselArrival;

            if (myTenant != null && myTenant.ShipmentATADateIndicator == "Container")
                myDate = containerPM.ActualPODDischarge;

            this.FillFieldsShipmentNewValues("MainCarriageATA", myDate, shipmentPM);
        }
        private void SaveShipment()
        {
            if (shipmentPM.IsUpdatedVizionAnalyzer || shipmentPM.IsUpdatedOceanInsightsAnalyzer || this.isUpdatingPackages || this.isUpdatingEmptyLeg)
            {
                string systemEmail = "system@tenant" + tenant + ".com";
                ShipmentService service = new ShipmentService(containerUpdatedFields.ShipmentContext, shipmentPM, systemEmail);
                string activity = "(A) Update Shipment from Container";
                AddTotangoActivity(tenant, activity, systemEmail);
                service.Update(true);
            }
        }
        public void AddTotangoActivity(int tenant, string activity, string systemEmail)
        {
            string email = AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : "system@tenant" + tenant + ".com";
            string moduleName = "(A) Container";
            ActivityLogger.SendTotangoContactActivity(email,moduleName, activity, containerPM.Tenant,false,null);
        }

        private string GetPortId(string portCode)
        {
            Port port = portRepository.GetOceanPortByCombinedCode(portCode, tenant);
            string portId = null;
            if (port != null)
            {
                portId = port.Id;
            }
            else
            {
                portId = this.CopyPortCopyToCurrentTenant(portCode);
            }
            return portId;
        }
        private Port GetPort(string portCode)
        {
            Port port = portRepository.GetOceanPortByCombinedCode(portCode, tenant);
            if (port != null)
            {
                return port;
            }
            else
            {
                port = this.CopyPortCopyToCurrentTenantPoco(portCode);
            }
            return port;
        }
        private string CopyPortCopyToCurrentTenant(string portCode)
        {
            string portId = null;
            Port portZero = portRepository.GetOceanPortByCombinedCode(portCode, 0);
            if (portZero != null)
            {
                var newPort = portQuery.GetPortCopyToCurrentTenant(portZero.Id, tenant);
                portId = newPort.Id;
            }

            return portId;
        }
        private Port CopyPortCopyToCurrentTenantPoco(string portCode)
        {
            Port portZero = portRepository.GetOceanPortByCombinedCode(portCode, 0);
            if (portZero != null)
            {
                var newPort = portQuery.GetPortCopyToCurrentTenantPoco(portZero.Id, tenant);
                return newPort;
            }

            return portZero;
        }
        private void FillFieldsNewValues(string propertyName, object newValue, object entity)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);

            if (propertyInfo != null && newValue != null)
            {
                propertyInfo.SetValue(entity, newValue);
            }
        }
        private void FillFieldsShipmentNewValues(string propertyName, object newValue, object entity)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);
            var entityValue = propertyInfo.GetValue(entity);
            if (propertyInfo == null || newValue == null)
            {
                return;
            }

            if (entityValue != null && entityValue.Equals(newValue))
            {
                return;
            }

            this.isUpdatingShipmentDateFields = true;
            propertyInfo.SetValue(entity, newValue);
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}