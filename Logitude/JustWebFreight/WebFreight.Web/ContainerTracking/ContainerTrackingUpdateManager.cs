using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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
        public ContainerTrackingUpdateManager(ContainerUpdatedFields containerUpdatedFields)
        {
            this.containerUpdatedFields = containerUpdatedFields;
            this.tenant = containerUpdatedFields.Tenant;
            this.containerPM = containerUpdatedFields.ContainerPM;
            this.shipmentPM = containerUpdatedFields.ShipmentPM;
            this.portRepository = new PortRepository(tenant);
            this.portQuery = new PortQuery(portRepository);
            this.vesselRepository = new VesselRepository(tenant);
            this.tenantRepository = new TenantRepository(tenant);            
        }       
        public void Update()
        {
            this.UpdateContainer();
            this.UpdatePackage();
            this.UpdateEmptyReturnLeg();
            this.UpdateShipment();
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

        private void MapPreCarriage()
        {
            containerPM.PreCarriageLocation = containerUpdatedFields.OriginLocation;
            containerPM.PreCarriageLocationPortId = this.GetPortId(containerUpdatedFields.OriginLocation);
        }

        private void MapOnCarriage()
        {
            containerPM.OnCarriageLocation = containerUpdatedFields.DeliveryLocation;
            containerPM.OnCarriageLocationPortId = this.GetPortId(containerUpdatedFields.DeliveryLocation);
        }

        private void MapVizionPreCarriage()
        {
            if (containerUpdatedFields.VisionPreCarriage == null)
                return;
            var port = GetPortForVizion(containerUpdatedFields.VisionPreCarriage);
            if (port == null)
                return;
            containerPM.PreCarriageLocationPortId = port.Id;
            containerPM.PreCarriageLocation = port.CombinedCode;

        }
        private void MapVizionOnCarriage()
        {
            if (containerUpdatedFields.VisionOnCarriage == null)
                return;
            var port = GetPortForVizion(containerUpdatedFields.VisionOnCarriage);
            if (port == null)
                return;
            containerPM.OnCarriageLocationPortId = port.Id;
            containerPM.OnCarriageLocation = port.CombinedCode;
        }

        private Port GetPortForVizion(Location portLocation)
        {
            Port port = null;
            if (!string.IsNullOrEmpty(portLocation.unlocode))
                port = GetPort(portLocation.unlocode);
            if(port != null)
                return port;
            var name1 = portLocation.name;
            if (!string.IsNullOrEmpty(name1) && portLocation.name.Contains(','))
                name1 = portLocation.name.Split(',').First();
            var name2 = portLocation.city;
            port = GetPortByNames(name1, name2);
            return port;
        }

        private Port GetPortByNames(string name1, string name2)
        {
            var port = portRepository.GetOceanPortByNames(name1, name2,containerPM.Tenant);
            if (port != null)
                return port;
            port = portRepository.GetOceanPortByNames(name1, name2, 0);
            if(port == null)
                return null;
            var newPort = portQuery.GetPortCopyToCurrentTenantPoco(port.Id, tenant);
            return newPort;
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
            containerPM.PreCarriageETD = containerUpdatedFields.EstimatedOriginPickup;
            containerPM.PreCarriageATD = containerUpdatedFields.ActualOriginPickup;
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
            containerPM.EstimatedLIFArrival = containerUpdatedFields.EstimatedLIFArrival;
            containerPM.ActualLIFArrival = containerUpdatedFields.ActualLIFArrival;
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
        private void MapTransshipments()
        {
            if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)            
                this.MapVizionTransshipments();                
            else if (containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.OceanInsights)
                this.MapOITransshipments();
        }
        private void MapVizionTransshipments()
        {
            this.SetRelatedTransshipmentLeg(containerUpdatedFields.LoadedTransshipment, "From");
            this.SetRelatedTransshipmentLeg(containerUpdatedFields.VesselDeparted, "From");
            this.SetRelatedTransshipmentLeg(containerUpdatedFields.VesselArrived, "To");
            this.SetRelatedTransshipmentLeg(containerUpdatedFields.DischargedTransshipment, "To");
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

        private void SetRelatedTransshipmentLeg(ContainerTransshipment transshipmentObject, string direction)
        {
            foreach (ContainerTransshipmentUpdatedFields updatedFields in transshipmentObject.TransshipmentMilestones)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) return;
                string portId = this.GetPortId(updatedFields.Location);
                Vessel vessel = this.GetVessel(updatedFields.Vessel);

                int? transshipmentLegIndex = GetTransshipmentLegIndex(portId, direction);
                if (transshipmentLegIndex == null || string.IsNullOrEmpty(portId)) return;

                this.FillFieldsNewValues("Transshipment" + transshipmentLegIndex + "LocationPortId", portId, containerPM);
                this.FillFieldsNewValues("Transshipment" + transshipmentLegIndex + "Location", updatedFields.Location, containerPM);
                this.FillFieldsNewValues("Leg" + transshipmentLegIndex + "Vessel", updatedFields.Vessel, containerPM);
                this.FillFieldsNewValues("Leg" + transshipmentLegIndex + "VesselId", vessel?.Id, containerPM);
                this.FillFieldsNewValues("Leg" + transshipmentLegIndex + "Voyage", updatedFields.Voyage, containerPM);
                this.SetTransshipmentLegDates(transshipmentLegIndex, updatedFields, transshipmentObject.Key);
            }
        }
        private int? GetTransshipmentLegIndex(string portId, string direction)
        {
            if(direction == "From" && containerPM.ShipmentTransshipment1FromId == portId
                || direction == "To" && containerPM.ShipmentTransshipment1ToId == portId)
            {
                return 1;
            }

            else if (direction == "From" && containerPM.ShipmentTransshipment2FromId == portId
                || direction == "To" && containerPM.ShipmentTransshipment2ToId == portId)
            {
                return 2;
            }

            else if (direction == "From" && containerPM.ShipmentTransshipment3FromId == portId
                || direction == "To" && containerPM.ShipmentTransshipment3ToId == portId)
            {
                return 3;
            }

            return null;
        }
        private void SetTransshipmentLegDates(int? transshipmentLegIndex, ContainerTransshipmentUpdatedFields updatedFields, string key)
        {
            if(key == "LoadedTransshipment")
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
                delivery.ATA = containerPM.ActualEmptyReturn;
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
        private void UpdateShipment()
        {
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
                this.FillFieldsShipmentNewValues("PreCarriageETD", containerPM.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.PreCarriageATD == null)
                {
                    this.FillFieldsShipmentNewValues("PreCarriageATD", containerPM.ActualPOLVesselDeparture, shipmentPM);
                }
            }

            else if (POLShipmentUpdateIndicator == "Main Carriage")
            {
                if(containerUpdatedFields.TrackingSource == ContainerStatusSourceValues.Vizion)
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

            if(myTenant != null && myTenant.ShipmentATADateIndicator == "Container")
                myDate = containerPM.ActualPODDischarge;

            this.FillFieldsShipmentNewValues("MainCarriageATA", myDate, shipmentPM);
        }
        private void SaveShipment()
        {
            if (shipmentPM.IsUpdatedVizionAnalyzer || shipmentPM.IsUpdatedOceanInsightsAnalyzer || this.isUpdatingPackages || this.isUpdatingEmptyLeg)
            {
                string systemEmail = "system@tenant" + tenant + ".com";
                ShipmentService service = new ShipmentService(containerUpdatedFields.ShipmentContext, shipmentPM, systemEmail);
                service.Update(true);
            }
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
    }
}