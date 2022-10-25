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
            //MapTransshipments();
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
            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort, ref containerUpdatedFields.LoadedTransshipmentLocation);
                MapMilestoneVesselField(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort, ref containerUpdatedFields.LoadedTransshipmentVessel);
                MapMilestoneVoyageField(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort, ref containerUpdatedFields.LoadedTransshipmentVoyage);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort, ref containerUpdatedFields.EstimatedLoadedTransshipment, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.LoadedOnVesselAtTransshipmentPort, ref containerUpdatedFields.ActualLoadedTransshipment);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.LoadedTransshipment))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.LoadedTransshipment, ref containerUpdatedFields.LoadedTransshipmentLocation);
                MapMilestoneVesselField(VizionMilestoneDescriptionCodes.LoadedTransshipment, ref containerUpdatedFields.LoadedTransshipmentVessel);
                MapMilestoneVoyageField(VizionMilestoneDescriptionCodes.LoadedTransshipment, ref containerUpdatedFields.LoadedTransshipmentVoyage);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.LoadedTransshipment, ref containerUpdatedFields.EstimatedLoadedTransshipment, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.LoadedTransshipment, ref containerUpdatedFields.ActualLoadedTransshipment);
            }
        }
        private void MapVesselArrived()
        {
            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort, ref containerUpdatedFields.VesselArrivedLocation);
                MapMilestoneVesselField(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort, ref containerUpdatedFields.VesselArrivedVessel);
                MapMilestoneVoyageField(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort, ref containerUpdatedFields.VesselArrivedVoyage);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort, ref containerUpdatedFields.EstimatedVesselArrived, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselArrivalAtTransshipmentPort, ref containerUpdatedFields.ActualVesselArrived);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselArrived))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.VesselArrived, ref containerUpdatedFields.VesselArrivedLocation);
                MapMilestoneVesselField(VizionMilestoneDescriptionCodes.VesselArrived, ref containerUpdatedFields.VesselArrivedVessel);
                MapMilestoneVoyageField(VizionMilestoneDescriptionCodes.VesselArrived, ref containerUpdatedFields.VesselArrivedVoyage);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselArrived, ref containerUpdatedFields.EstimatedVesselArrived, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselArrived, ref containerUpdatedFields.ActualVesselArrived);
            }
        }
        private void MapVesselDeparted()
        {
            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselDepartureFromTransshipmentPort))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.VesselDepartureFromTransshipmentPort, ref containerUpdatedFields.EmptyReturnLocation);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselDepartureFromTransshipmentPort, ref containerUpdatedFields.EstimatedEmptyReturn, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselDepartureFromTransshipmentPort, ref containerUpdatedFields.ActualEmptyReturn);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.VesselDeparted))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.VesselDeparted, ref containerUpdatedFields.EmptyReturnLocation);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselDeparted, ref containerUpdatedFields.EstimatedEmptyReturn, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.VesselDeparted, ref containerUpdatedFields.ActualEmptyReturn);
            }
        }
        private void MapDischargedTransshipment()
        {
            if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.DischargedFromVesselAtTransshipmentPort))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.DischargedFromVesselAtTransshipmentPort, ref containerUpdatedFields.EmptyReturnLocation);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.DischargedFromVesselAtTransshipmentPort, ref containerUpdatedFields.EstimatedEmptyReturn, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.DischargedFromVesselAtTransshipmentPort, ref containerUpdatedFields.ActualEmptyReturn);
            }

            else if (IsMilestoneSentWithinResponse(VizionMilestoneDescriptionCodes.DischargedTransshipment))
            {
                MapMilestoneLocationField(VizionMilestoneDescriptionCodes.DischargedTransshipment, ref containerUpdatedFields.EmptyReturnLocation);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.DischargedTransshipment, ref containerUpdatedFields.EstimatedEmptyReturn, true);
                MapMilestoneDateField(VizionMilestoneDescriptionCodes.DischargedTransshipment, ref containerUpdatedFields.ActualEmptyReturn);
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

        private void MapMilestoneVesselField(string descriptionCode, ref string vessel)
        {
            if (!IsMilestoneSentWithinResponse(descriptionCode))
                return;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => !string.IsNullOrEmpty(e.location?.unlocode));
            if (plannedMilistone != null)
            {
                //vessel = plannedMilistone.vessel;
            }
        }

        private void MapMilestoneVoyageField(string descriptionCode, ref string voyage)
        {
            if (!IsMilestoneSentWithinResponse(descriptionCode))
                return;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => !string.IsNullOrEmpty(e.location?.unlocode));
            if (plannedMilistone != null)
            {
                //voyage = plannedMilistone.voyage;
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

        //Loaded on vessel at transshipment port + Loaded transshipment // from port
        //Vessel arrival at transshipment port + Vessel arrived // to port
        //Vessel departure from transshipment port + Vessel departed // from
        //Discharged from vessel at transshipment port +  Discharged transshipment // to
    }
}