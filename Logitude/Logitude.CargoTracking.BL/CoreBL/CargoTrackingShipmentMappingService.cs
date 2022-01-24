using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.CargoTracking.BL.CoreBL;
using Logitude.CargoTracking.Data.EntityLists;
using System;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public class CargoTrackingShipmentMappingService
    {
        public CargoTrackingShipmentPM cargoShipmentPM;
        ShipmentOrderPM shipmentOrderPM;
        ShipmentPM shipmentPM;


        public void MapCargoTrackingShipmentFields(CargoTrackingShipmentPM cargoShipmentPM, Dictionary<string,CargoTrackingMilestoneList> milestoneDictionary)
        {
            this.cargoShipmentPM = cargoShipmentPM;
            GetConnectedEntities();

            MapShipmentPMFields();
            BuildShipmentRoute();
            FillShipmentPackages();
            FillCustomsData();
            FillDocumentsFilings();
            BuildPartnerCards();
            BuildShipmentMilstones(milestoneDictionary);
            SetMilestonesStatus();
            SetRoutePortsCodes(cargoShipmentPM);
            SetTenantFields();
            SetShipmentCloudDataFields();
        }



        private void BuildShipmentMilstones(Dictionary<string, CargoTrackingMilestoneList> milestoneDictionary)
        {
            var cargoTrackingMilestoneBuilder = new CargoTrackingMilestoneBuilder();
            cargoShipmentPM.Milestones = cargoTrackingMilestoneBuilder.BuildShipmentMilstones(cargoShipmentPM, milestoneDictionary);
        }

        private void GetConnectedEntities()
        {
            GetShipmentOrder();
            GetShipmentPM();
        }
        private void MapShipmentPMFields()
        {

            cargoShipmentPM.CustomsBrokerReference = shipmentPM?.CustomFileNumber;
            cargoShipmentPM.ContainersNumbers = shipmentPM?.ContainersNumbers;
            cargoShipmentPM.IncotermName = shipmentOrderPM != null ? shipmentOrderPM.IncotermCode : shipmentPM?.IncotermCode;
            cargoShipmentPM.WarehouseLegName = !string.IsNullOrEmpty(shipmentPM?.WarehouseLegLocalName) ? shipmentPM?.WarehouseLegLocalName : shipmentPM?.WarehouseLegEnglishName;
            cargoShipmentPM.ImportManifest = shipmentPM?.ImportManifest;
            cargoShipmentPM.TotalTax = shipmentPM?.TotalTax;
            cargoShipmentPM.ShipmentTypeName = shipmentPM?.ShipmentTypeName;
            cargoShipmentPM.ShipmentOrderQuantity = shipmentOrderPM?.Quantity;
            cargoShipmentPM.ShipmentOrderPONumber = shipmentOrderPM?.PONumber;
            cargoShipmentPM.SHOBookingConfirmationNumber = shipmentOrderPM?.BookingConfirmationNumber;
            cargoShipmentPM.SHOPODate = shipmentOrderPM?.PODate;
            cargoShipmentPM.SHOCarrierName = shipmentOrderPM?.CarrierName;
            cargoShipmentPM.House = shipmentOrderPM?.House;



        }
        private void BuildShipmentRoute()
        {
            CargoTrackingShipmentRouteBuilder routeBuilder = new CargoTrackingShipmentRouteBuilder(shipmentOrderPM, shipmentPM);
            cargoShipmentPM.RoutingSteps = routeBuilder.BuildRoute();
        }
        private void FillShipmentPackages()
        {
            List<ShipmentPackagePM> shipmentPackages = GetShipmentPackages();

            cargoShipmentPM.Packages = new List<CargoShipmentPackage>();
            foreach (var package in shipmentPackages)
            {
                MapShipmentPackage(package);
            }
        }
        private void FillCustomsData()
        {
            CargoTrackingShipmentCustomsData customsData = GetShipmentCustomsData();
            BuildCargoShipmentCustomsData(customsData);
            BuildCustomsTaxDetails(customsData);
        }
        private void FillDocumentsFilings()
        {
            List<DocumentsFilingPM> documentsFilingPM = GetShipmentDocumentsFilings();

            cargoShipmentPM.DocumentsFilings = new List<CargoDocumentsFiling>();
            foreach (var documentFiling in documentsFilingPM)
            {
                MapCargoDocumentsFromDocumentsFilings(documentFiling);
            }
        }
        private void BuildPartnerCards()
        {
            CargoTrackingShipmenPartnersCardsBuilder partnerCardsBuilder = new CargoTrackingShipmenPartnersCardsBuilder(shipmentOrderPM, shipmentPM, cargoShipmentPM);
            cargoShipmentPM.PartnerCards = partnerCardsBuilder.BuildPartnerCards();
        }        

        private void SetRoutePortsCodes(CargoTrackingShipmentPM cargoShipmentPM)
        {
            CargoTrackingPortPM fromPort = GetCargoPort(cargoShipmentPM.FromPortId);
            CargoTrackingPortPM toPort = GetCargoPort(cargoShipmentPM.ToPortId);

            cargoShipmentPM.RouteFromPortCode = fromPort?.Code;
            cargoShipmentPM.RouteToPortCode = toPort?.Code;
        }

        private void SetTenantFields()
        {
            TenantManagementPM tenantManagment = GetTenantManagement(cargoShipmentPM.Tenant);
            cargoShipmentPM.ActivatedForDeclarationApprove = tenantManagment?.ActivatedforDeclarationApprove ?? false;
            cargoShipmentPM.TenantDeclarationMessage = tenantManagment?.DeclarationMessage;
        }

        private void SetShipmentCloudDataFields()
        {
            ShipmentAdditionalCloudData cloudData = GetShipmentCloud(cargoShipmentPM);
            cargoShipmentPM.IsImporterApprovalRequried = cloudData?.IsImporterApprovalRequried ?? false;
            cargoShipmentPM.ApprovedDate = cloudData?.ApproveDateTime;
            cargoShipmentPM.DenyDate = cloudData?.DenyDate;
            cargoShipmentPM.DenyReason = cloudData?.DenyReason;
        }

        private ShipmentAdditionalCloudData GetShipmentCloud(CargoTrackingShipmentPM cargoShipmentPM)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(cargoShipmentPM.Tenant);
            var cloudData = shipmentQuery.GetShipmentAdditionalCloudData(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);
            return cloudData;
        }

        private TenantManagementPM GetTenantManagement(int tenant)
        {
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
            var tenantManagment = tenantManagementQuery.GetTenantManagementPM(tenant);
            return tenantManagment;
        }
        private CargoTrackingPortPM GetCargoPort(string id)
        {
            CargoTrackingPortQueryService cargoTrackingPortQuery = new CargoTrackingPortQueryService(cargoShipmentPM.Tenant);
            cargoTrackingPortQuery.InitializeSettings();
            CargoTrackingPortPM cargoTrackingPortPM = cargoTrackingPortQuery.GetSingle(id, true, false);
            return cargoTrackingPortPM;
        }

        private ShipmentOrderPM GetShipmentOrder()
        {
            ShipmentOrderQueryService shipmentOrderQuery = new ShipmentOrderQueryService(cargoShipmentPM.Tenant);
            shipmentOrderPM = shipmentOrderQuery.GetSinglePMForCargo(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);
            return shipmentOrderPM;
        }
        private ShipmentPM GetShipmentPM()
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(cargoShipmentPM.Tenant);
            shipmentPM = shipmentQuery.GetShipmentPMForCargoTrackingByEntityId(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);
            return shipmentPM;
        }
       
        private List<ShipmentPackagePM> GetShipmentPackages()
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(cargoShipmentPM.Tenant);
            List<ShipmentPackagePM> shipmentPackages = shipmentQuery.GetPackagesOfShipment(cargoShipmentPM.Tenant, cargoShipmentPM.EntityId);
            return shipmentPackages;
        }
        private void MapShipmentPackage(ShipmentPackagePM package)
        {
            cargoShipmentPM.Packages.Add(new CargoShipmentPackage()
            {
                Id = package.Id,
                Tenant = package.Tenant,
                PackageTypeName = package.PackageTypeName,
                ContainerNumber = package.ContainerNumber,
                ShipperSeal = package.ShipperSeal,
                CarrierSeal = package.CarrierSeal,
                Quantity = package.Quantity,
                Weight = package.Weight,
                Volume = package.Volume,
                Height = package.Height,
                Length = package.Length,
                Width = package.Width
            });
        }
        private CargoTrackingShipmentCustomsData GetShipmentCustomsData()
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(cargoShipmentPM.Tenant);
            CargoTrackingShipmentCustomsData customsData = shipmentQuery.GetCargoTrackingShipmentCustomsData(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);
            return customsData;
        }
        private void BuildCargoShipmentCustomsData(CargoTrackingShipmentCustomsData customsData)
        {
            if (customsData == null)
                return;
            cargoShipmentPM.CustomsData = new ShipmentCustomsData()
            {
                DeclarationNumber = customsData.DeclarationNumber,
                DeclarationStatus = customsData.DeclarationStatus,
                CurrencySign = customsData.CurrencySign,
                CurrencyCode = customsData.CurrencyCode,
                CurrencyName = customsData.CurrencyName,
                GoodsDescription = customsData.GoodsDescription,
                ImporterVatAmount = customsData.ImporterVatAmount,
                TotalValueInNIS = customsData.TotalValueInNIS,
                TotalValueInForeignCurrency = customsData.TotalValueInForeignCurrency,
                TotalTax = customsData.TotalTax,
                ImporterId = customsData.ImporterId,
                CargoIdentifier1 = customsData.CargoIdentifier1,
                CargoIdentifier2 = customsData.CargoIdentifier2,
                CargoIdentifier3 = customsData.CargoIdentifier3
            };
        }
        private void BuildCustomsTaxDetails(CargoTrackingShipmentCustomsData customsData)
        {
            if (customsData == null)
                return;

            cargoShipmentPM.CustomsData.TaxDetails = new List<Def.EntityPMs.CargoTrackingShipmentCustomTaxDetails>();
            foreach (var details in customsData.TaxDetails)
            {
                cargoShipmentPM.CustomsData.TaxDetails.Add(new Def.EntityPMs.CargoTrackingShipmentCustomTaxDetails()
                {
                    TaxAmount = details.TaxAmount,
                    TaxBasis = details.TaxBasis,
                    TaxTypeName = details.TaxTypeName
                });
            }
        }
        private List<DocumentsFilingPM> GetShipmentDocumentsFilings()
        {
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(cargoShipmentPM.Tenant);
            
            List<DocumentsFilingPM> documentsFilingPM = documentsFilingQuery.GetInputDocumentsFilingPMsByEntityId(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);

            if (!string.IsNullOrWhiteSpace(cargoShipmentPM.ForwardingShipmentHeaderId))
            {
                List<DocumentsFilingPM> forwardingShipmentDocumentsFiling = documentsFilingQuery
                    .GetInputDocumentsFilingPMsByEntityId(cargoShipmentPM.ForwardingShipmentHeaderId, cargoShipmentPM.Tenant);
                AddForwardingShipmentsDocsToCustomsShipment(documentsFilingPM, forwardingShipmentDocumentsFiling);
            }
            return documentsFilingPM;
        }

        private void AddForwardingShipmentsDocsToCustomsShipment(List<DocumentsFilingPM> documentsFilingPM , List<DocumentsFilingPM> forwardingShipmentDocumentsFiling) {
            if (forwardingShipmentDocumentsFiling.Any())
            {
                documentsFilingPM.AddRange(forwardingShipmentDocumentsFiling);
            }
        }

        private void MapCargoDocumentsFromDocumentsFilings(DocumentsFilingPM document)
        {
            cargoShipmentPM.DocumentsFilings.Add(new CargoDocumentsFiling()
            {
                Id = document.Id,
                Tenant = document.Tenant,
                DocumentId = document.DocumentId,
                Code = document.Code,
                DocumentTypeId = document.DocumentTypeId,
                DocumentTypeName = document.DocumentTypeName,
                DocumentTypeCode = document.DocumentTypeCode,
                CreateDate = document.CreateDate,
                IsDigitallySigned = document.IsDigitallySigned,
                SecurityId = document.SecurityId,
                CustomReference = document.CustomReference
            });
        }
        public void SetMilestonesStatus()
        {
            SetCurrentMilestone();
            SetDoneMilstones();
            SetFutureMilstoneForShipment();
        }
        private void SetCurrentMilestone()
        {
            Milestone currentMilstone = GetMostRecentNotEstimatedMilestone(cargoShipmentPM.Milestones);
            if (currentMilstone != null)
                currentMilstone.IsCurrent = true;
        }
        private void SetDoneMilstones()
        {
            Milestone currentMilstone = cargoShipmentPM.Milestones.FirstOrDefault(d => d.IsCurrent == true);
            if (currentMilstone != null)
            {
                var doneMilstones = cargoShipmentPM.Milestones.Where(milstone => milstone.Weight < currentMilstone.Weight).ToList();
                doneMilstones.ForEach(doneMilstone =>
                {
                    doneMilstone.Done = true;
                    doneMilstone.IsEstimation = false;
                });
            }
        }
        private Milestone GetMostRecentNotEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation != true && s.Date != null).OrderByDescending(s => s.Weight).FirstOrDefault();
        }
        private void SetFutureMilstoneForShipment()
        {
            Milestone futureMilstone = GetMostRecentEstimatedMilestone(cargoShipmentPM.Milestones);

            if (futureMilstone != null)
            {
                cargoShipmentPM.FutureMilstoneCode = futureMilstone.Code;
                cargoShipmentPM.FutureMilstoneName = futureMilstone.Name;
                cargoShipmentPM.FutureMilstoneDate = futureMilstone.EstimationDate;
            }
        }

        private static Milestone GetMostRecentEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation == true && s.EstimationDate != null)
                                                        .OrderByDescending(s => s.Weight)
                                                        .FirstOrDefault();
        }
    }



}