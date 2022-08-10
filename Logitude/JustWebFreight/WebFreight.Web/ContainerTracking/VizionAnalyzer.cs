using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
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
            MapOriginLocation();
            MapPOL();

        }

        private void MapOriginLocation()
        {
            containerUpdatedFields.OriginLocation = visionContainerStatus?.payload?.origin_port?.unlocode ?? containerUpdatedFields.OriginLocation;
        }

        private void MapEmptyPickup()
        {
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.EmptyPickup, ref containerUpdatedFields.EstimatedEmptyPickupDate, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.EmptyPickup, ref containerUpdatedFields.ActualEmptyPickupDate);
            MapMilestoneLocationField(VizionMilestoneDescriptionCodes.EmptyPickup, ref containerUpdatedFields.EmptyPickupLocation);
        }

        

        private void MapPOL()
        {
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLArrival, ref containerUpdatedFields.EstimatedPOLLoaded, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLArrival, ref containerUpdatedFields.ActualPOLArrival);

            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLLoaded, ref containerUpdatedFields.EstimatedPOLLoaded, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLLoaded, ref containerUpdatedFields.ActualPOLLoaded);
            
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLVslDeparture, ref containerUpdatedFields.MainCarriageETD, true);
            MapMilestoneDateField(VizionMilestoneDescriptionCodes.POLVslDeparture, ref containerUpdatedFields.MainCarriageATD);


        }

        private void MapMilestoneDateField(string descriptionCode, ref DateTime? date,bool isPland = false)
        {
            if (!IsExist(descriptionCode))
                return;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => e.planned == isPland);
            if (plannedMilistone != null)
            {
                date =  plannedMilistone.timestamp;
            }
        }
        private void MapMilestoneLocationField(string descriptionCode, ref string location)
        {
            if (!IsExist(descriptionCode))
                return;
            var plannedMilistone = MilestonesDictinoary[descriptionCode].FirstOrDefault(e => !string.IsNullOrEmpty(e.location?.unlocode));
            if (plannedMilistone != null)
            {
                location = plannedMilistone.location?.unlocode;
            }
        }
        
        private void MapDepartureField()
        {
            if (!IsExist(VizionMilestoneDescriptionCodes.VesselDepartureFromOriginPort))
                return;
            string pOLLocation = null;
            var plannedMilistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselDepartureFromOriginPort].FirstOrDefault(e => e.planned);
            if (plannedMilistone != null)
            {
                containerUpdatedFields.EstimatedPOLVesselDeparture =  plannedMilistone.timestamp;
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

        private void MapArrivedField()
        {
            if (!IsExist(VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort))
                return;
            string pODLocation = null;
            var plannedMilistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort].FirstOrDefault(e => e.planned);
            if(plannedMilistone != null)
            {
                containerUpdatedFields.EstimatedPODVesselArrival =  plannedMilistone.timestamp ;
                pODLocation = plannedMilistone.location?.unlocode;
            }
            
            var milistone = MilestonesDictinoary[VizionMilestoneDescriptionCodes.VesselArrivedAtDestinationPort].FirstOrDefault(e => !e.planned);
            if(milistone != null)
            {
                containerUpdatedFields.ActualPODVesselArrival =  milistone.timestamp;
                pODLocation = string.IsNullOrEmpty(milistone.location?.unlocode) ? pODLocation : milistone.location?.unlocode;

            }
            if (!string.IsNullOrEmpty(pODLocation))
            {
                containerUpdatedFields.PODLocation = pODLocation;
            }

        }

       
        private DateTime? GetValue(string key)
        {
            var milestone = MilestonesDictinoary[key];
            return null;
        }

        private bool IsExist(string description)
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

        private string GetMilstonesKey(string description, string unlocode, bool planned)
        {
            return $"{description}-{unlocode}-{planned}";
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

    }
}