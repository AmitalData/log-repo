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

        }

        private void MapDepartureField()
        {
            if (!IsExist(VizionMmilestoneDescriptionEnums.VesselDepartureFromOriginPort))
                return;
            string pOLLocation = null;
            var plannedMilistone = MilestonesDictinoary[VizionMmilestoneDescriptionEnums.VesselDepartureFromOriginPort].FirstOrDefault(e => e.planned);
            if (plannedMilistone != null)
            {
                containerUpdatedFields.EstimatedPOLVesselDeparture = plannedMilistone.planned ? plannedMilistone.timestamp : containerUpdatedFields.MainCarriageETA;
                pOLLocation = plannedMilistone.location?.unlocode;
            }

            var milistone = MilestonesDictinoary[VizionMmilestoneDescriptionEnums.VesselDepartureFromOriginPort].FirstOrDefault(e => !e.planned);
            if (milistone != null)
            {
                containerUpdatedFields.ActualPOLVesselDeparture = milistone.planned ? containerUpdatedFields.MainCarriageATA : milistone.timestamp;
                pOLLocation = string.IsNullOrEmpty(milistone.location?.unlocode) ? pOLLocation : milistone.location?.unlocode;

            }
            if (!string.IsNullOrEmpty(pOLLocation))
            {
                containerUpdatedFields.POLLocation = milistone.location.unlocode;
            }
        }

        private void MapArrivedField()
        {
            if (!IsExist(VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort))
                return;
            string pODLocation = null;
            var plannedMilistone = MilestonesDictinoary[VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort].FirstOrDefault(e => e.planned);
            if(plannedMilistone != null)
            {
                containerUpdatedFields.EstimatedPODVesselArrival = plannedMilistone.planned ? plannedMilistone.timestamp : containerUpdatedFields.MainCarriageETA;
                pODLocation = plannedMilistone.location?.unlocode;
            }
            
            var milistone = MilestonesDictinoary[VizionMmilestoneDescriptionEnums.VesselArrivedAtDestinationPort].FirstOrDefault(e => !e.planned);
            if(milistone != null)
            {
                containerUpdatedFields.ActualPODVesselArrival = milistone.planned ? containerUpdatedFields.MainCarriageATA : milistone.timestamp;
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


    public static class VizionMmilestoneDescriptionEnums
    {
        public static string VesselDepartureFromOriginPort = "Vessel departure from origin port";
        public static string VesselArrivedAtDestinationPort = "Vessel arrived at destination port";

    }
}