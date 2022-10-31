using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ContainerTracking
{
    public class VizionAnalyzer
    {
        private ContainerUpdatedFields containerUpdatedFields;
        private VisionContainerStatus visionContainerStatus;
        private Dictionary<string, List<VisionMilestone>> MilestonesDictinoary;

        public VizionAnalyzer(VisionContainerStatus visionContainerStatus)
        {
            this.visionContainerStatus = visionContainerStatus;
            containerUpdatedFields = new ContainerUpdatedFields();
            containerUpdatedFields.ShipmentContext = ShipmentsContext.GetContext(0);
            containerUpdatedFields.ContainerRepository = new ContainerRepository(containerUpdatedFields.ShipmentContext);

        }

        public ContainerUpdatedFields Run()
        {
            if (visionContainerStatus == null || !(visionContainerStatus.payload?.milestones?.Count >= 0))
                return containerUpdatedFields;

            MilestonesDictinoary = GetMilstonesAsDictinoary(visionContainerStatus.payload.milestones);
            MapFields();
            return containerUpdatedFields;
        }

        private void MapFields()
        {
            MapArrivedField();
            MapDepartureField();
            MapEmptyPickup();
            MapPOL();
            MapPOD();
            MapPreCarriage();
            MapOnCarriage();
            MapGate();
            MapEmptyReturn();
            MapCarrierRelease();
            MapCustomsRelease();
            MapTransshipments();
        }
        private void MapArrivedField()
        {
            if (!IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort))
                return;
            string pODLocation = null;
            var plannedMilistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort].FirstOrDefault(e => e.planned);
            if (plannedMilistone != null)
            {
                containerUpdatedFields.EstimatedPODVesselArrival = plannedMilistone.timestamp;
                pODLocation = plannedMilistone.location?.unlocode;
            }

            var milistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort].FirstOrDefault(e => !e.planned);
            if (milistone != null)
            {
                containerUpdatedFields.ActualPODVesselArrival = milistone.timestamp;
                pODLocation = string.IsNullOrEmpty(milistone.location?.unlocode) ? pODLocation : milistone.location?.unlocode;

            }
            if (!string.IsNullOrEmpty(pODLocation))
            {
                containerUpdatedFields.PODLocation = pODLocation;
            }
        }
        private void MapDepartureField()
        {
            if (!IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselDepartureFromOriginPort))
                return;
            string pOLLocation = null;
            var plannedMilistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselDepartureFromOriginPort].FirstOrDefault(e => e.planned);
            if (plannedMilistone != null)
            {
                containerUpdatedFields.EstimatedPOLVesselDeparture = plannedMilistone.timestamp;
                pOLLocation = plannedMilistone.location?.unlocode;
            }

            var milistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselDepartureFromOriginPort].FirstOrDefault(e => !e.planned);
            if (milistone != null)
            {
                containerUpdatedFields.ActualPOLVesselDeparture = milistone.timestamp;
                pOLLocation = string.IsNullOrEmpty(milistone.location?.unlocode) ? pOLLocation : milistone.location?.unlocode;

            }
            if (!string.IsNullOrEmpty(pOLLocation))
            {
                containerUpdatedFields.POLLocation = milistone.location.unlocode;
            }
        }
        private void MapEmptyPickup()
        {
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.EmptyPickup, ref containerUpdatedFields.EstimatedEmptyPickupDate, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.EmptyPickup, ref containerUpdatedFields.ActualEmptyPickupDate);
            MapMilestoneLocationField(VizionMilestoneDescriptionCodes.EmptyPickup, ref containerUpdatedFields.EmptyPickupLocation);
        }
        private void MapPOL()
        {
            containerUpdatedFields.POLLocation = visionContainerStatus?.payload?.origin_port?.unlocode ?? containerUpdatedFields.POLLocation;

            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLArrival, ref containerUpdatedFields.EstimatedPOLArrival, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLArrival, ref containerUpdatedFields.ActualPOLArrival);

            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLLoaded, ref containerUpdatedFields.EstimatedPOLLoaded, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLLoaded, ref containerUpdatedFields.ActualPOLLoaded);

            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLVslDeparture, ref containerUpdatedFields.MainCarriageETD, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLVslDeparture, ref containerUpdatedFields.MainCarriageATD);
        }
        private void MapPOD()
        {
            containerUpdatedFields.PODLocation = visionContainerStatus?.payload?.destination_port?.unlocode ?? containerUpdatedFields.PODLocation;

            MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort, ref containerUpdatedFields.MainCarriageETA, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort, ref containerUpdatedFields.MainCarriageATA);

            MapMilestoneDateField(VizionMilestoneDescriptionCodes.DischargedFromVesselAtDestinationPort, ref containerUpdatedFields.EstimatedPODDischarge, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.DischargedFromVesselAtDestinationPort, ref containerUpdatedFields.ActualPODDischarge);
        }
        private void MapPreCarriage()
        {
            if (!IsDifferentPort(visionContainerStatus?.payload?.inland_origin, visionContainerStatus?.payload?.origin_port))
                return;
            containerUpdatedFields.VisionPreCarriage = visionContainerStatus?.payload?.inland_origin;
        }
        private void MapOnCarriage()
        {
            if (!IsDifferentPort(visionContainerStatus?.payload?.inland_destination, visionContainerStatus?.payload?.destination_port))
                return;
            containerUpdatedFields.VisionOnCarriage = visionContainerStatus?.payload?.inland_destination;
        }
        private void MapGate()
        {
            var containersExternal = new ContainersExternal();
            containersExternal.IsFromOceanInsights = true;
            var containersExternalData = new ContainersExternalData();
            containersExternalData.GateIn = GetMilestoneDateField(VizionMilestoneDescriptionCodes.GateInOriginPort);
            containersExternalData.GateOut = GetMilestoneDateField(VizionMilestoneDescriptionCodes.GateOutFromDestinationPort);

            containersExternal.ContainersExternalData_New = containersExternalData;
            containerUpdatedFields.ContainersExternal = containersExternal;
        }
        private void MapEmptyReturn()
        {
            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.GateInEmptyReturn))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.GateInEmptyReturn, ref containerUpdatedFields.EmptyReturnLocation);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.GateInEmptyReturn, ref containerUpdatedFields.EstimatedEmptyReturn, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.GateInEmptyReturn, ref containerUpdatedFields.ActualEmptyReturn);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.GateInEmpty))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.GateInEmpty, ref containerUpdatedFields.EmptyReturnLocation);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.GateInEmpty, ref containerUpdatedFields.EstimatedEmptyReturn, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.GateInEmpty, ref containerUpdatedFields.ActualEmptyReturn);
            }
        }
        private void MapCarrierRelease()
        {
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.CarrierRelease, ref containerUpdatedFields.CarrierReleaseDate);
            containerUpdatedFields.CarrierReleaseState = GetReleaseState(VizionMilestoneDescriptionCodes.CarrierRelease);
        }
        private void MapCustomsRelease()
        {
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.CustomsRelease, ref containerUpdatedFields.CustomsReleaseDate);
            containerUpdatedFields.CustomsReleaseState = GetReleaseState(VizionMilestoneDescriptionCodes.CustomsRelease);
        }
        private void MapTransshipments()
        {
            MapLoadedTransshipment();
            MapVesselArrived();
            MapVesselDeparted();
            MapDischargedTransshipment();
        }
        private void MapLoadedTransshipment()
        {
            containerUpdatedFields.LoadedTransshipment = new ContainerTransshipment("LoadedTransshipment");

            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort, ref containerUpdatedFields.LoadedTransshipment.TransshipmentMilestones);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.LoadedTransshipment))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.LoadedTransshipment, ref containerUpdatedFields.LoadedTransshipment.TransshipmentMilestones);
            }
        }
        private void MapVesselArrived()
        {
            containerUpdatedFields.VesselArrived = new ContainerTransshipment("VesselArrived");

            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort, ref containerUpdatedFields.VesselArrived.TransshipmentMilestones);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselArrived))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.VesselArrived, ref containerUpdatedFields.VesselArrived.TransshipmentMilestones);
            }
        }
        private void MapVesselDeparted()
        {
            containerUpdatedFields.VesselDeparted = new ContainerTransshipment("VesselDeparted");

            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselDepartureFromTransshipmentPort))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.VesselDepartureFromTransshipmentPort, ref containerUpdatedFields.VesselDeparted.TransshipmentMilestones);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselDeparted))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.VesselDeparted, ref containerUpdatedFields.VesselDeparted.TransshipmentMilestones);
            }
        }
        private void MapDischargedTransshipment()
        {
            containerUpdatedFields.DischargedTransshipment = new ContainerTransshipment("DischargedTransshipment");

            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.DischargedFromVesselAtTransshipmentPort))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.DischargedFromVesselAtTransshipmentPort, ref containerUpdatedFields.DischargedTransshipment.TransshipmentMilestones);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.DischargedTransshipment))
            {
                MapMilestoneLocationList(VizionMilestoneDescriptionCodes.DischargedTransshipment, ref containerUpdatedFields.DischargedTransshipment.TransshipmentMilestones);
            }
        }

        private bool IsDifferentPort(Location location, Location mainLocation)
        {
            if (mainLocation == null)
                return false;
            if (location == null)
                return false;

            if (!string.IsNullOrEmpty(location?.unlocode) &&
                !string.IsNullOrEmpty(mainLocation?.unlocode) &&
                location?.unlocode == mainLocation?.unlocode)
                return false;

            if (location.name == mainLocation.name &&
            location.country == mainLocation.country &&
            location.city == mainLocation.city &&
            location.state == mainLocation.state)
                return false;

            return true;
        }       
        private string GetReleaseState(string descriptionCode)
        {
            if (!IsMilestoneSentWithinResponse(descriptionCode))
                return null;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => !e.planned);
            if (plannedMilistone != null)
            {
                return "true";
            }
            plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => e.planned);
            if (plannedMilistone != null)
            {
                return "false";
            }
            return null;
        }
        private void MapMilestoneDateField(string descriptionCode, ref DateTime? date, bool isPland = false)
        {
            if (!IsMilestoneSentWithinResponse(descriptionCode))
                return;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => e.planned == isPland);
            if (plannedMilistone != null)
            {
                date = plannedMilistone.timestamp;
            }
        }
        private DateTime? GetMilestoneDateField(string descriptionCode, bool isPland = false)
        {
            if (!IsMilestoneSentWithinResponse(descriptionCode))
                return null;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => e.planned == isPland);
            if (plannedMilistone != null)
            {
                return plannedMilistone.timestamp;
            }
            return null;
        }
        private void MapMilestoneLocationField(string descriptionCode, ref string location)
        {
            if (!IsMilestoneSentWithinResponse(descriptionCode))
                return;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => !string.IsNullOrEmpty(e.location?.unlocode));
            if (plannedMilistone != null)
            {
                location = plannedMilistone.location?.unlocode;
            }
        }
        private bool IsMilestoneSentWithinResponse(string description)
        {
            if (!MilestonesDictinoary.ContainsKey(description))
                return false;

            return true;
        }
        private Dictionary<string, List<VisionMilestone>> GetMilstonesAsDictinoary(List<VisionMilestone> milestones)
        {
            var milestonesDictinoary = new Dictionary<string, List<VisionMilestone>>(milestones.Count);
            foreach (var item in milestones)
            {
                if (!milestonesDictinoary.ContainsKey(item.description))
                    milestonesDictinoary.Add(item.description, new List<VisionMilestone>() { item });
                milestonesDictinoary[item.description].Add(item);

            }
            return milestonesDictinoary;
        }
        private void MapMilestoneLocationList(string descriptionCode, ref List<ContainerTransshipmentUpdatedFields> transshipmentsmilestones)
        {
            List<VisionMilestone> visionMilestones = MilestonesDictinoary[descriptionCode].Where(d => d.source == "carrier").ToList();

            foreach (VisionMilestone visionMilestone in visionMilestones)
            {
                ContainerTransshipmentUpdatedFields updatedFields = new ContainerTransshipmentUpdatedFields();
                updatedFields.Location = visionMilestone.location?.unlocode;
                updatedFields.Vessel = visionMilestone.vessel;
                updatedFields.Voyage = visionMilestone.voyage;

                if (visionMilestone.planned)
                    updatedFields.EstimatedDate = visionMilestone.timestamp;

                else
                    updatedFields.ActualDate = visionMilestone.timestamp;

                transshipmentsmilestones.Add(updatedFields);
            }
        }
    }

    public static class VizionMilestoneDescriptionCodes
    {
        public static string VesselDepartureFromOriginPort = "Vessel departure from origin port";
        public static string VesselArrivedAtDestinationPort = "Vessel arrived at destination port";
        public static string EmptyPickup = "Gate out from origin port";
        public static string POLArrival = "Gate in at origin port";
        public static string POLLoaded = "Loaded on vessel at origin port";
        public static string POLVslDeparture = "Vessel departure from origin port";
        public static string GateInOriginPort = "Gate in at origin port";
        public static string GateOutFromDestinationPort = "Gate out from destination port";
        public static string DischargedFromVesselAtDestinationPort = "Discharged from vessel at destination port";
        public static string GateInEmptyReturn = "Gate in empty return";
        public static string GateInEmpty = "Gate in empty"; 
        public static string CarrierRelease = "Carrier release";
        public static string CustomsRelease = "Customs release";
        public static string LoadedOnVesselAtTransshipmentPort = "Loaded on vessel at transshipment port";
        public static string LoadedTransshipment = "Loaded transshipment";
        public static string VesselArrivalAtTransshipmentPort = "Vessel arrival at transshipment port";
        public static string VesselArrived = "Vessel arrived";
        public static string VesselDepartureFromTransshipmentPort = "Vessel departure from transshipment port";
        public static string VesselDeparted = "Vessel departed";
        public static string DischargedFromVesselAtTransshipmentPort = "Discharged from vessel at transshipment port";
        public static string DischargedTransshipment = "Discharged transshipment";       
    }
}