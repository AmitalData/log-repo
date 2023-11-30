using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityOtherServices
{
    public class ContainerShipmentUpdateService
    {
        private int tenant;
        private ContainerPM containerPM;
        private ShipmentPM shipmentPM;
        private IShipmentsContext shipmentsContext;
        private PortRepository portRepository;
        private PortQuery portQuery;
        private ContainerTrackingHelper containerTrackingHelper;
        private Tenant myTenant;
        public List<dynamic> allShipmentTrasshipmentLegs;
        private List<TransshipmentData> transshipmentData;
        private bool isUpdatingShipmentDateFields = false;
        public ContainerShipmentUpdateService(ContainerPM container, IShipmentsContext context)
        {
            this.containerPM = container;
            this.tenant = container.Tenant;
            this.shipmentsContext = context;
            this.portRepository = new PortRepository(tenant);
            this.portQuery = new PortQuery(portRepository);
            this.containerTrackingHelper = new ContainerTrackingHelper(tenant);            
            this.allShipmentTrasshipmentLegs = new List<dynamic>();
            this.GetTenant();
        }
        private void GetTenant()
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            myTenant = tenantRepository.GetSingleTenant(tenant);
        }

        public void HandleUpdate()
        {
            this.shipmentPM = this.GetShipment();
            if (shipmentPM == null) return;

            bool isUpdatingEmptyReturn = HandleEmptyReturnLeg();
            bool isUpdatingShipmentPackage = UpdateShipmentPackage();

            bool isUpdatingShipment = false;
            if (IsUpdateShipmentAllowed())
                isUpdatingShipment = ProcessUpdatingShipment();

            else
            {
                string location = "POL MainCarriage";
                containerTrackingHelper.AddContainerDiscrepancy(location, containerPM, shipmentPM);
                location = "POD MainCarriage";
                containerTrackingHelper.AddContainerDiscrepancy(location, containerPM, shipmentPM);
            }

            if (isUpdatingEmptyReturn || isUpdatingShipment || isUpdatingShipmentPackage)
                SaveShipment();
        }

        private ShipmentPM GetShipment()
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            return shipmentQuery.GetSinglePM(containerPM.ShipmentId, tenant);
        }
        private bool IsUpdateShipmentAllowed()
        {
            if (shipmentPM.IsOperationalClosed)
                return false;

            if (!containerTrackingHelper.IsSameLocation(shipmentPM.MainCarriageFromPortId, containerPM.POLLocation))
                return false;

            if (!containerTrackingHelper.IsSameLocation(shipmentPM.MainCarriageFinalDestinationPortId, containerPM.PODLocation))
                return false;

            return true;
        }
        
        #region Empty Return
        private bool HandleEmptyReturnLeg()
        {
            if (shipmentPM.DirectionId != "I" && shipmentPM.DirectionId != "R") return false;
            if (string.IsNullOrEmpty(containerPM.EmptyReturnLocationPortId)) return false;
            if (!containerPM.IsEmptyReturnDatesChanged) return false;

            ShipmentDeliveryPM emptyReturn = this.GetEmptyReturnLeg(shipmentPM);
            if (emptyReturn != null)
            {
                UpdateEmptyReturnLeg(emptyReturn);
                return true;
            }

            else
            {
                ShipmentPackagePM shipmentPackage = shipmentPM.ShipmentPackages.Where(d => d.ContainerEntityId == containerPM.Id).FirstOrDefault();
                if (shipmentPackage == null) return false;

                emptyReturn = CreateEmptyReturnLeg(shipmentPackage, shipmentPM);
                ShipmentPickUpDeliveryPackagePM deliveryPackage = this.CreateEmptyReturnPackage(shipmentPackage);
                if (deliveryPackage != null)
                {
                    this.AddPackageHarmonizes(deliveryPackage, shipmentPackage);
                    emptyReturn.ShipmentPickUpDeliveryPackages.Add(deliveryPackage);
                }
                shipmentPM.ShipmentDeliveries.Add(emptyReturn);
                return true;
            }
        }
        private ShipmentDeliveryPM GetEmptyReturnLeg(ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM shipmentDelivery = null;

            ShipmentPickUpDeliveryPackageRepository pickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(shipmentsContext);
            List<ShipmentPickUpDeliveryPackage> packages = pickUpDeliveryPackageRepository.GetShipmentPickUpDeliveryPackagesByContainerIdAndTenant(containerPM.Id, tenant);
            if (packages != null && packages.Count > 0)
            {
                List<string> deliveryPackagesIds = packages.Select(s => s.ShipmentPickUpDeliveryId).ToList();
                shipmentDelivery = shipmentPM.ShipmentDeliveries.Where(a => deliveryPackagesIds.Contains(a.Id) && a.PickUpDeliveryTypeCode == "EMPT").FirstOrDefault();
            }

            return shipmentDelivery;
        }
        private void UpdateEmptyReturnLeg(ShipmentDeliveryPM emptyReturn)
        {
            emptyReturn.ETA = containerPM.EstimatedEmptyReturn;
            emptyReturn.ATA = emptyReturn.ATA ?? containerPM.ActualEmptyReturn;
            emptyReturn.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        }
        private ShipmentDeliveryPM CreateEmptyReturnLeg(ShipmentPackagePM shipmentPackage, ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM emptyReturn = this.CreateEmptyReturnInstance(shipmentPackage.Id);
            this.ComputeEmptyReturnFromProperties(emptyReturn, shipmentPM);

            PortRepository portRepository = new PortRepository(tenant);
            Port toPort = portRepository.GetSinglePort(containerPM.EmptyReturnLocationPortId, tenant);
            emptyReturn.ToAddress = "Port Of: " + toPort?.EnglishName;

            if (!string.IsNullOrEmpty(emptyReturn.FromPortId))
            {
                Port fromPort = portRepository.GetSinglePort(emptyReturn.FromPortId, tenant);
                emptyReturn.FromAddress = "Port Of: " + fromPort?.EnglishName;
            }

            return emptyReturn;
        }
        private ShipmentDeliveryPM CreateEmptyReturnInstance(string connectedPackageId)
        {
            return new ShipmentDeliveryPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                PickUpDeliveryTypeCode = "EMPT",
                ETA = containerPM.EstimatedEmptyReturn,
                ATA = containerPM.ActualEmptyReturn,
                PickUpDeliveryToTypeCode = "PORT",
                ToPortId = containerPM.EmptyReturnLocationPortId,
                FullResponsibility = true,
                ConnectedPackageId = connectedPackageId,
            };
        }
        private ShipmentPickUpDeliveryPackagePM CreateEmptyReturnPackage(ShipmentPackagePM shipmentPackage)
        {
            return new ShipmentPickUpDeliveryPackagePM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                ContainerEntityId = containerPM.Id,
                ContainerNumber = containerPM.ContainerNumber,
                PackageTypeId = shipmentPackage.PackageTypeId,
                Weight = shipmentPackage.Weight,
                Description = shipmentPackage.Description,
                PackageTypeName = shipmentPackage.PackageTypeName,
                Quantity = shipmentPackage.Quantity,
                Volume = shipmentPackage.Volume,
                ShipperSeal = shipmentPackage.ShipperSeal,
                Width = shipmentPackage.Width,
                Height = shipmentPackage.Height,
                Length = shipmentPackage.Length,
                Harmonize = shipmentPackage.Harmonize,
                OriginalShipmentPackageId = shipmentPackage.Id,
                IsMultiHarmonize = shipmentPackage.IsMultiHarmonize,
            };
        }
        private void AddPackageHarmonizes(ShipmentPickUpDeliveryPackagePM deliveryPackage, ShipmentPackagePM shipmentPackage)
        {
            foreach (ShipmentPackageHarmonizePM harmonizeItem in shipmentPackage.ShipmentPackageHarmonizes)
            {
                PickUpDeliveryPackageHarmonizePM harmonize = new PickUpDeliveryPackageHarmonizePM();
                harmonize.Harmonize = harmonizeItem.Harmonize;
                harmonize.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                deliveryPackage.PickUpDeliveryPackageHarmonizes.Add(harmonize);
            };
        }
        private void ComputeEmptyReturnFromProperties(ShipmentDeliveryPM emptyReturn, ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM lastDelivery = shipmentPM.ShipmentDeliveries.Where(d => d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(o => o.PickUpDeliveryNumber).FirstOrDefault();
            if (lastDelivery != null)
            {
                this.MapLocationFromLastDelivery(lastDelivery, emptyReturn);
                emptyReturn.PickUpDeliveryFromTypeCode = lastDelivery.PickUpDeliveryToTypeCode;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.WarehouseLegWarehouseId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PART";
                emptyReturn.FromPartnerCardId = shipmentPM.WarehouseLegWarehouseId;
                emptyReturn.FromAddressId = shipmentPM.WarehouseLegAddressId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.OnCarriageToPortId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.OnCarriageToPortId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.Transshipment3ToPortId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.Transshipment3ToPortId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.Transshipment2ToPortId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.Transshipment2ToPortId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.Transshipment1ToPortId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.Transshipment1ToPortId;
            }

            else
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.MainCarriageToPortId;
            }
        }
        private void MapLocationFromLastDelivery(ShipmentDeliveryPM lastDelivery, ShipmentDeliveryPM emptyReturn)
        {
            switch (lastDelivery.PickUpDeliveryToTypeCode)
            {
                case "PART":
                    {
                        emptyReturn.FromPartnerCardId = lastDelivery.ToPartnerCardId;
                        emptyReturn.FromAddressId = lastDelivery.ToAddressId;
                        break;
                    }

                case "PORT":
                    {
                        emptyReturn.FromPortId = lastDelivery.ToPortId;
                        emptyReturn.FromAddress = lastDelivery.ToAddress;
                        break;
                    }

                case "CASL":
                    {
                        emptyReturn.FromAddressCity = lastDelivery.ToAddressCity;
                        emptyReturn.FromAddressZipCode = lastDelivery.ToAddressZipCode;
                        emptyReturn.FromAddressCountryId = lastDelivery.ToAddressCountryId;
                        break;
                    }
            }
        }
        #endregion

        #region Shipment Package
        private bool UpdateShipmentPackage()
        {
            if (!containerPM.IsUpdatedOceanInsightsAnalyzer) return false;

            DateTime todatDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? eventData = containerPM.OIEventDate;
            ShipmentPackagePM package = shipmentPM.ShipmentPackages.Where(a => a.Id == containerPM.ShipmentPackagesId).FirstOrDefault();
            string oceanInsightsSource = "OIN";
            if (package != null)
            {
                if (eventData == null)
                    eventData = todatDate;

                if (package.LastStatusDate == null || eventData > package.LastStatusDate)
                {
                    package.LastStatusCode = containerPM.OIContainerStatus;
                    package.LastStatusDate = eventData;
                    package.ContainerStatusSourceCode = oceanInsightsSource;
                    package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Shipment
        private bool ProcessUpdatingShipment()
        {
            shipmentPM.IsUpdatedVizionAnalyzer = containerPM.IsUpdatedVizionAnalyzer;
            shipmentPM.IsUpdatedOceanInsightsAnalyzer = containerPM.IsUpdatedOceanInsightsAnalyzer;

            string POLShipmentUpdateIndicator = this.GetPOLShipmentUpdateIndicator(containerPM.POLLocationPortId);
            string PODShipmentUpdateIndicator = this.GetPODShipmentUpdateIndicator(containerPM.PODLocationPortId);

            if (!string.IsNullOrEmpty(POLShipmentUpdateIndicator) || !string.IsNullOrEmpty(PODShipmentUpdateIndicator))
            {
                this.UpdatePOLDates(POLShipmentUpdateIndicator);
                this.UpdatePODDates(PODShipmentUpdateIndicator);
            }

            bool isUpdatingPreCarriage = MapPreCarriageDates();
            bool isUpdatingOnCarriage =  MapOnCarriageDates();
            MapTransshipmentLegDates();
            MapVesselVoyageLegs();

            return isUpdatingShipmentDateFields || isUpdatingPreCarriage || isUpdatingOnCarriage;
        }

        private string GetPOLShipmentUpdateIndicator(string portId)
        {
            if (string.IsNullOrEmpty(portId)) return null;

            if (shipmentPM.PreCarriageFromPortId == portId)
                return "Pre Carriage";

            else if (shipmentPM.MainCarriageFromPortId == portId)
                return "Main Carriage";

            return null;
        }
        private string GetPODShipmentUpdateIndicator(string portId)
        {
            if (string.IsNullOrEmpty(portId)) return null;

            if (shipmentPM.OnCarriageToPortId == portId)
                return "On Carriage";

            else if (shipmentPM.Transshipment3ToPortId == portId)
                return "Transshipment3";

            else if (shipmentPM.Transshipment2ToPortId == portId)
                return "Transshipment2";

            else if (shipmentPM.Transshipment1ToPortId == portId)
                return "Transshipment1";

            else if (shipmentPM.MainCarriageToPortId == portId)
                return "Main Carriage";

            return null;
        }

        private void UpdatePOLDates(string POLShipmentUpdateIndicator)
        {
            if (POLShipmentUpdateIndicator == "Pre Carriage")
            {
                containerTrackingHelper.AddContainerDiscrepancy("POL PreCarriage", containerPM, shipmentPM);
                this.FillFieldsNewValues_Shipment("PreCarriageETD", containerPM.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.PreCarriageATD == null)                
                    this.FillFieldsNewValues_Shipment("PreCarriageATD", containerPM.ActualPOLVesselDeparture, shipmentPM);                
            }

            else if (POLShipmentUpdateIndicator == "Main Carriage")
            {
                containerTrackingHelper.AddContainerDiscrepancy("POL MainCarriage", containerPM, shipmentPM);
                shipmentPM.IsUpdatedVizionMainCarriageDates = containerPM.IsUpdatedVizionAnalyzer;
                shipmentPM.IsUpdatedOceanInsightsMainCarriageDates = containerPM.IsUpdatedOceanInsightsAnalyzer;

                this.FillFieldsNewValues_Shipment("MainCarriageETD", containerPM.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.MainCarriageATD == null)                
                    this.FillFieldsNewValues_Shipment("MainCarriageATD", containerPM.ActualPOLVesselDeparture, shipmentPM);                
            }
        }
        private void UpdatePODDates(string PODShipmentUpdateIndicator)
        {
            if (PODShipmentUpdateIndicator == "On Carriage")
            {
                containerTrackingHelper.AddContainerDiscrepancy("POD OnCarriage", containerPM, shipmentPM);
                this.FillFieldsNewValues_Shipment("OnCarriageETA", containerPM.EstimatedPODVesselArrival, shipmentPM);

                if (shipmentPM.OnCarriageATA == null)                
                    this.FillOnCarriageATA();                
            }

            else
            {
                var ETAfielName = PODShipmentUpdateIndicator == "Main Carriage" ? "MainCarriageETA" : PODShipmentUpdateIndicator + "ETA";
                shipmentPM.IsUpdatedVizionMainCarriageDates = containerPM.IsUpdatedVizionAnalyzer;
                shipmentPM.IsUpdatedOceanInsightsMainCarriageDates = containerPM.IsUpdatedOceanInsightsAnalyzer;

                containerTrackingHelper.AddContainerDiscrepancy("POD MainCarriage", containerPM, shipmentPM);
                this.FillFieldsNewValues_Shipment(ETAfielName, containerPM.EstimatedPODVesselArrival, shipmentPM);

                if (shipmentPM.MainCarriageATA == null)
                    this.FillMainCarriageATA();
            }
        }
        private void FillOnCarriageATA()
        {
            DateTime? myDate = containerPM.ActualPODVesselArrival;

            if (myTenant != null && myTenant.ShipmentATADateIndicator == "Container")
                myDate = containerPM.ActualPODDischarge;

            this.FillFieldsNewValues_Shipment("OnCarriageATA", myDate, shipmentPM);
        }
        private void FillMainCarriageATA()
        {
            DateTime? myDate = containerPM.ActualPODVesselArrival;

            if (myTenant != null && myTenant.ShipmentATADateIndicator == "Container")
                myDate = containerPM.ActualPODDischarge;

            this.FillFieldsNewValues_Shipment("MainCarriageATA", myDate, shipmentPM);
        }

        private bool MapPreCarriageDates()
        {
            containerTrackingHelper.AddContainerDiscrepancy("PreCarriage", containerPM, shipmentPM);
            if (!containerTrackingHelper.IsSameLocationUsingId(shipmentPM.PreCarriageFromPortId, containerPM.PreCarriageLocationPortId)) return false;
            shipmentPM.PreCarriageETD = containerPM.PreCarriageETD;
            shipmentPM.PreCarriageATD = shipmentPM.PreCarriageATD ?? containerPM.PreCarriageATD;
            return true;
        }
        private bool MapOnCarriageDates()
        {
            containerTrackingHelper.AddContainerDiscrepancy("OnCarriage", containerPM, shipmentPM);
            if (!containerTrackingHelper.IsSameLocationUsingId(shipmentPM.OnCarriageToPortId, containerPM.OnCarriageLocationPortId)) return false;
            shipmentPM.OnCarriageETA = containerPM.OnCarriageETA;
            shipmentPM.OnCarriageATA = shipmentPM.OnCarriageATA ?? containerPM.OnCarriageATA;
            return true;
        }

        private void MapTransshipmentLegDates()
        {
            FillTransshipmentDataList();
            FillTransshipmentLegsData();
            FillShipmentDates();
        }
        private void FillTransshipmentDataList()
        {
            this.transshipmentData = new List<TransshipmentData>();

            TransshipmentData vesselDeparted = new TransshipmentData("VesselDeparted");
            TransshipmentData vesselArrived = new TransshipmentData("VesselArrived");
            TransshipmentData loadedTransshipment = new TransshipmentData("LoadedTransshipment");
            TransshipmentData dischargedTransshipment = new TransshipmentData("DischargedTransshipment");

            if (!string.IsNullOrEmpty(containerPM.Transshipment1LocationPortId))
            {
                vesselDeparted.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment1Location,
                    Vessel = containerPM.Leg1VesselId,
                    Voyage = containerPM.Leg1Voyage,
                    EstimatedDate = containerPM.EstimatedTrans1VesselDeparture,
                    ActualDate = containerPM.ActualTrans1VesselDeparture
                });

                vesselArrived.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment1Location,
                    Vessel = containerPM.Leg1VesselId,
                    Voyage = containerPM.Leg1Voyage,
                    EstimatedDate = containerPM.EstimatedTrans1VesselArrival,
                    ActualDate = containerPM.ActualTransshipment1VesselArrival
                });

                loadedTransshipment.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment1Location,
                    Vessel = containerPM.Leg1VesselId,
                    Voyage = containerPM.Leg1Voyage,
                    EstimatedDate = containerPM.EstimatedTransshipment1Loaded,
                    ActualDate = containerPM.ActualTransshipment1Loaded
                });

                dischargedTransshipment.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment1Location,
                    Vessel = containerPM.Leg1VesselId,
                    Voyage = containerPM.Leg1Voyage,
                    EstimatedDate = containerPM.EstimatedTransshipment1Discharge,
                    ActualDate = containerPM.ActualTransshipment1Discharge
                });
            }

            if (!string.IsNullOrEmpty(containerPM.Transshipment2LocationPortId))
            {
                vesselDeparted.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment2Location,
                    Vessel = containerPM.Leg2VesselId,
                    Voyage = containerPM.Leg2Voyage,
                    EstimatedDate = containerPM.EstimatedTrans2VesselDeparture,
                    ActualDate = containerPM.ActualTrans2VesselDeparture
                });

                vesselArrived.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment2Location,
                    Vessel = containerPM.Leg2VesselId,
                    Voyage = containerPM.Leg2Voyage,
                    EstimatedDate = containerPM.EstimatedTrans2VesselArrival,
                    ActualDate = containerPM.ActualTransshipment2VesselArrival
                });

                loadedTransshipment.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment2Location,
                    Vessel = containerPM.Leg2VesselId,
                    Voyage = containerPM.Leg2Voyage,
                    EstimatedDate = containerPM.EstimatedTransshipment2Loaded,
                    ActualDate = containerPM.ActualTransshipment2Loaded
                });

                dischargedTransshipment.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment2Location,
                    Vessel = containerPM.Leg2VesselId,
                    Voyage = containerPM.Leg2Voyage,
                    EstimatedDate = containerPM.EstimatedTransshipment2Discharge,
                    ActualDate = containerPM.ActualTransshipment2Discharge
                });
            }

            if (!string.IsNullOrEmpty(containerPM.Transshipment3LocationPortId))
            {
                vesselDeparted.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment3Location,
                    Vessel = containerPM.Leg3VesselId,
                    Voyage = containerPM.Leg3Voyage,
                    EstimatedDate = containerPM.EstimatedTrans3VesselDeparture,
                    ActualDate = containerPM.ActualTrans3VesselDeparture
                });

                vesselArrived.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment3Location,
                    Vessel = containerPM.Leg3VesselId,
                    Voyage = containerPM.Leg3Voyage,
                    EstimatedDate = containerPM.EstimatedTrans3VesselArrival,
                    ActualDate = containerPM.ActualTransshipment3VesselArrival
                });

                loadedTransshipment.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment3Location,
                    Vessel = containerPM.Leg3VesselId,
                    Voyage = containerPM.Leg3Voyage,
                    EstimatedDate = containerPM.EstimatedTransshipment3Loaded,
                    ActualDate = containerPM.ActualTransshipment3Loaded
                });

                dischargedTransshipment.TransshipmentUpdatedFields.Add(new TransshipmentUpdatedFields()
                {
                    Location = containerPM.Transshipment3Location,
                    Vessel = containerPM.Leg3VesselId,
                    Voyage = containerPM.Leg3Voyage,
                    EstimatedDate = containerPM.EstimatedTransshipment3Discharge,
                    ActualDate = containerPM.ActualTransshipment3Discharge
                });
            }

            transshipmentData.Add(vesselDeparted);
            transshipmentData.Add(vesselArrived);
            transshipmentData.Add(loadedTransshipment);
            transshipmentData.Add(dischargedTransshipment);
        }
        private void FillTransshipmentLegsData()
        {
            FillShipmentVesselDepartedFields();
            FillShipmentVesselArrivedFields();
            FillShipmentLoadedTransshipmentFields();
            FillShipmentDischargedTransshipmentFields();
        }
        private void FillShipmentVesselDepartedFields()
        {
            TransshipmentData vesselDeparted = transshipmentData.Where(d => d.Key == "VesselDeparted").FirstOrDefault();
            if (vesselDeparted == null) return;

            foreach (TransshipmentUpdatedFields updatedFields in vesselDeparted.TransshipmentUpdatedFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = containerTrackingHelper.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(vesselDeparted.Key), Key = vesselDeparted.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentVesselArrivedFields()
        {
            TransshipmentData vesselArrived = transshipmentData.Where(d => d.Key == "VesselArrived").FirstOrDefault();
            if (vesselArrived == null) return; 

            foreach (TransshipmentUpdatedFields updatedFields in vesselArrived.TransshipmentUpdatedFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = containerTrackingHelper.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(vesselArrived.Key), Key = vesselArrived.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentLoadedTransshipmentFields()
        {
            TransshipmentData loadedTransshipment = transshipmentData.Where(d => d.Key == "LoadedTransshipment").FirstOrDefault();
            if (loadedTransshipment == null) return;

            foreach (TransshipmentUpdatedFields updatedFields in loadedTransshipment.TransshipmentUpdatedFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = containerTrackingHelper.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(loadedTransshipment.Key), Key = loadedTransshipment.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentDischargedTransshipmentFields()
        {
            TransshipmentData dischargedTransshipment = transshipmentData.Where(d => d.Key == "DischargedTransshipment").FirstOrDefault();
            if (dischargedTransshipment == null) return;

            foreach (TransshipmentUpdatedFields updatedFields in dischargedTransshipment.TransshipmentUpdatedFields)
            {
                if (string.IsNullOrEmpty(updatedFields.Location)) continue;
                string portId = containerTrackingHelper.GetPortId(updatedFields.Location);

                var leg = new { PortId = portId, MilestoneData = updatedFields, Direction = GetPortLegDirection(dischargedTransshipment.Key), Key = dischargedTransshipment.Key };
                allShipmentTrasshipmentLegs.Add(leg);
            }
        }
        private void FillShipmentDates()
        {
            allShipmentTrasshipmentLegs.Where(a => a.Key != "LoadedTransshipment" && a.Key != "DischargedTransshipment").ToList().ForEach(leg =>
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

                  this.FillFieldsNewValues_Shipment(transshipmentLeg.EstimatedDateField, updatedFields.EstimatedDate, shipmentPM);
                  var transshipmentATDInShipment = GetPropValue(shipmentPM, transshipmentLeg.ActualDateField);
                  if (transshipmentATDInShipment == null) this.FillFieldsNewValues_Shipment(transshipmentLeg.ActualDateField, updatedFields.ActualDate, shipmentPM);
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
        private void MapVesselVoyageLegs()
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
                istheSameFromLocation = containerTrackingHelper.IsSameLocation(shipmentFromPortIdField, containerPM.POLLocation);
                istheSameToLocation = containerTrackingHelper.IsSameLocation(shipmentToPortIdField, containerPM.PODLocation);
            }
            else
            {
                istheSameFromLocation = containerTrackingHelper.IsSameLocation(shipmentFromPortIdField, containerPM.POLLocation);
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

        #endregion

        public void AddTotangoActivity(int tenant, string activity, string systemEmail)
        {
            string email = AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : "system@tenant" + tenant + ".com";
            string moduleName = "(A) Container";
            ActivityLogger.SendTotangoContactActivity(email, moduleName, activity, containerPM.Tenant, false, null);
        }

        private void FillFieldsNewValues(string propertyName, object newValue, object entity)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);

            if (propertyInfo != null && newValue != null)
            {
                propertyInfo.SetValue(entity, newValue);
            }
        }
        private void FillFieldsNewValues_Shipment(string propertyName, object newValue, object entity)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);
            var entityValue = propertyInfo.GetValue(entity);
            if (propertyInfo == null || newValue == null)            
                return;            

            if (entityValue != null && entityValue.Equals(newValue))            
                return;            

            this.isUpdatingShipmentDateFields = true;
            propertyInfo.SetValue(entity, newValue);
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private void MapShipmentConcurrencyFields()
        {
            //if (string.IsNullOrEmpty(this.containerPM.ShipmentId))
            //{
            //    return;
            //}
            //this.containerPm.ShipmentConcurrencyGUID = entityRepository.GetConcurrencyGUIDByShipmentId(this.containerPm.ShipmentId, this.containerPm.Tenant);
            //this.containerPm.ShipmentNewConcurrencyGUID = Guid.NewGuid().ToString();
        }
        private void SaveShipment()
        {
            string oldStatusId = shipmentPM.StatusId;

            string updatedByEmail = null;
            if (shipmentPM.IsUpdatedVizionAnalyzer || shipmentPM.IsUpdatedOceanInsightsAnalyzer)
                updatedByEmail = "system@tenant" + tenant + ".com";

            else updatedByEmail = AuthenticationUtil.GetAuthenticatedUser();

            shipmentPM.ShipmentUpdatedFromContainer = true;
            ShipmentService service = new ShipmentService(shipmentsContext, shipmentPM, updatedByEmail);
            service.SetChangeSet(shipmentPM.ShipmentPackages, shipmentPM.ShipmentOrderPackages, shipmentPM.ShipmentPickUps, shipmentPM.ShipmentDeliveries, shipmentPM.ShipmentReceivables, shipmentPM.ShipmentPayables, shipmentPM.FollowUps, shipmentPM.ShipmentAWBPrintOnlies, shipmentPM.ShipmentConsoleShipments, shipmentPM.ShipmentCarrierStatuses, shipmentPM.AWBOCIPMs, shipmentPM.ShipmentCommodities, shipmentPM.ShipmentAssemblies, shipmentPM.ShipmentStoragePricings, shipmentPM.ShipmentProductItems, shipmentPM.ShipmentUnassignedFields);
            string activity = "(A) Update Shipment from Container";
            AddTotangoActivity(tenant, activity, updatedByEmail);
            service.Update();
            shipmentPM.ShipmentUpdatedFromContainer = false;

            ShipmentContainersEntityBehaviour.UpdateConatinarStatus(shipmentPM, oldStatusId != shipmentPM.StatusId, shipmentsContext);
        }
    }
}
