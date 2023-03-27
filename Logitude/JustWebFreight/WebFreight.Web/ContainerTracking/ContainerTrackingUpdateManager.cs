using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
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
        private PortRepository portRepository;
        private PortQuery portQuery;
        private VesselRepository vesselRepository;
        
        private ContainerTrackingHelper containerTrackingHelper;

        public ContainerTrackingUpdateManager(ContainerUpdatedFields containerUpdatedFields)
        {            this.containerUpdatedFields = containerUpdatedFields;
            this.containerPM = containerUpdatedFields.ContainerPM;          
        }

        public void Initialize(int tenant)
        {
            this.tenant = tenant;
            this.portRepository = new PortRepository(tenant);
            this.portQuery = new PortQuery(portRepository);
            this.vesselRepository = new VesselRepository(tenant);
            this.containerTrackingHelper = new ContainerTrackingHelper(tenant);
        }

        public void SetContainer(ContainerPM containerPM)
        {
            this.containerPM = containerPM;
            this.tenant = containerPM.Tenant;
        }      

        public void Update(bool isUpatingContainer)
        {
            if (!isUpatingContainer) return;

            this.UpdateContainer();
            this.SaveContainer();
        }
        private void UpdateContainer()
        {
            MapGeneralContainerFields();
            MapTransshipments();
            MapOnCarriage();
            MapPreCarriage();
            MapConcurrencyFields();            
        }
        private void MapGeneralContainerFields()
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

            containerPM.OIContainerStatus = containerUpdatedFields.ContainerStatus;
            containerPM.OIEventDate = containerUpdatedFields.EventDate;
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
        }
        private void MapPreCarriage()
        {
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                MapVizionPreCarriage();

            else
                MapOIPreCarriage();
        }
        private void MapOnCarriage()
        {
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                MapVizionOnCarriage();

            else
                MapOIOnCarriage();
        }
        private void MapOIPreCarriage()
        {
            containerPM.PreCarriageLocation = containerUpdatedFields.OriginLocation;
            containerPM.PreCarriageLocationPortId = this.GetPortId(containerUpdatedFields.OriginLocation);
            containerPM.PreCarriageETD = containerUpdatedFields.EstimatedOriginPickup;
            containerPM.PreCarriageATD = containerUpdatedFields.ActualOriginPickup;
        }
        private void MapOIOnCarriage()
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
        }        
        private void MapTransshipments()
        {
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
                this.MapVizionTransshipments();

            else if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                this.MapOITransshipments();
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
        private void MapVizionTransshipments()
        {
            this.FillFirstContainerLegVesselVoyage();
            this.FillContainerTransshipmentLeg(containerUpdatedFields.LoadedTransshipment);
            this.FillContainerTransshipmentLeg(containerUpdatedFields.VesselDeparted);
            this.FillContainerTransshipmentLeg(containerUpdatedFields.VesselArrived);
            this.FillContainerTransshipmentLeg(containerUpdatedFields.DischargedTransshipment);
            this.FillLastContainerLegVesselVoyage();
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
            containerPM.IsAutomaticUpdates = true;
            containerPM.RecentResponseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            ContainerService containerService = new ContainerService(containerUpdatedFields.ShipmentContext, tenant);
            containerService.Update(containerPM, containerUpdatedFields.ContainersExternal);
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
       
        private void FillFieldsNewValues(string propertyName, object newValue, object entity)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);

            if (propertyInfo != null && newValue != null)
            {
                propertyInfo.SetValue(entity, newValue);
            }
        }
        
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}