using Logitude.BL.DataContracts;
using Simplog.Data.ShipmentsModel;
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
        private Dictionary<string, VisionMilestone> MilestonesDictinoary;


        public VizionAnalyzer(VisionContainerStatus visionContainerStatus)
        {
            var shipmentsContext = ShipmentsContext.GetContext(0);
            containerUpdatedFields = new ContainerUpdatedFields();
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
            if (IsExist(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort, true))
                containerUpdatedFields.MainCarriageETA = GetValue(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort);
            if (IsExist(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort, false))
                containerUpdatedFields.MainCarriageATA = GetValue(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort);
            if (IsExist(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort, true))
                containerUpdatedFields.MainCarriageETD = GetValue(VizionMmilestoneDescriptionEnums.VesselDepartureFromOriginPort);
            if (IsExist(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort, false))
                containerUpdatedFields.MainCarriageATD = GetValue(VizionMmilestoneDescriptionEnums.VesselDepartureFromOriginPort);
        }

        private DateTime? GetValue(string destinationPort)
        {
            var milestone = MilestonesDictinoary[destinationPort];
            return milestone.timestamp;
        }

        private bool IsExist(string destinationPort, bool planned)
        {
            if (!MilestonesDictinoary.ContainsKey(destinationPort))
                return false;
            var milestone = MilestonesDictinoary[destinationPort];
            if (milestone.planned == planned)
                return true;
            return false;
        }

        private Dictionary<string, VisionMilestone> GetMilstonesAsDictinoary(List<VisionMilestone> milestones)
        {
            var milestonesDictinoary = new Dictionary<string, VisionMilestone>(milestones.Count);
            foreach (var item in milestones)
            {
                if (milestonesDictinoary.ContainsKey(item.description))
                    continue;
                milestonesDictinoary.Add(item.description, item);
            }
            return milestonesDictinoary;
        }
    }


    public static class VizionMmilestoneDescriptionEnums
    {
        public static string VesselDepartureFromOriginPort = "Vessel departure from origin port";
        public static string VesselArrivedAtDestinationPort = "Vessel arrived at destination port";

    }
}