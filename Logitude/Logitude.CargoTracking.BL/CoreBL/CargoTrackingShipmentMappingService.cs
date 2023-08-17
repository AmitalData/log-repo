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
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityDataMappings;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public class CargoTrackingShipmentMappingService
    {
        public CargoTrackingShipmentPM cargoShipmentPM;
        ShipmentOrderPM shipmentOrderPM;
        ShipmentPM shipmentPM;
        ShipmentPM forwardingShipmentPM;


        public void MapCargoTrackingShipmentFields(CargoTrackingShipmentPM cargoShipmentPM, Dictionary<string, CargoTrackingMilestoneList> milestoneDictionary)
        {
            this.cargoShipmentPM = cargoShipmentPM;
            GetConnectedEntities();

            MapShipmentPMFields();
            BuildShipmentRoute();
            FillShipmentPackages();
            FillCustomsData();
            FillConnectedOrders();
            FillDocumentsFilings();
            BuildPartnerCards();
            BuildShipmentMilstones(milestoneDictionary);
            BuildShipmentEvents(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant, cargoShipmentPM.ForwardingShipmentHeaderId);
            SetMilestonesStatus();
            SetRoutePortsCodes(cargoShipmentPM);
            SetTenantFields();
            SetShipmentCloudDataFields();
            SetSharedLogisticsSettings();
            this.cargoShipmentPM.ChargeableWeightInKG = ShipmentMapping.GetWeightInKG(this.cargoShipmentPM.ChargeableWeightUnitCode, this.cargoShipmentPM.ChargeableWeight);
        }



        private void BuildShipmentMilstones(Dictionary<string, CargoTrackingMilestoneList> milestoneDictionary)
        {
            var cargoTrackingMilestoneBuilder = new CargoTrackingMilestoneBuilder();
            cargoShipmentPM.Milestones = cargoTrackingMilestoneBuilder.BuildShipmentMilstones(cargoShipmentPM, milestoneDictionary);
        }
        private void BuildShipmentEvents(string entityId,int tenant,string forwardingShipmentHeaderId)
        {

            var cargoTrackingEventsBuilder = new CargoTrackingEventsBuilder();
            cargoShipmentPM.Events = cargoTrackingEventsBuilder.BuildShipmentEvents(entityId,tenant, forwardingShipmentHeaderId);
        }
  
        private void GetConnectedEntities()
        {
            GetShipmentOrder();
            GetForwardingShipment();
            GetShipmentPM();
        }
        private string GetWarehouseLegName()
        {
            if (forwardingShipmentPM == null && cargoShipmentPM.EntityType == "C") {
                return null;
            }
            if (forwardingShipmentPM != null && cargoShipmentPM.EntityType == "C")
            {
                string warehouselegname = !string.IsNullOrEmpty(forwardingShipmentPM?.WarehouseLegLocalName) ? forwardingShipmentPM?.WarehouseLegLocalName : forwardingShipmentPM?.WarehouseLegEnglishName;
                return warehouselegname == "---" ? null : warehouselegname;
            }
            else
            {
                return !string.IsNullOrEmpty(shipmentPM?.WarehouseLegLocalName) ? shipmentPM?.WarehouseLegLocalName : shipmentPM?.WarehouseLegEnglishName;
            }
        }

        private void MapShipmentPMFields()
        {

            cargoShipmentPM.CustomsBrokerReference = shipmentPM?.CustomFileNumber;
            cargoShipmentPM.ContainersNumbers = shipmentPM?.ContainersNumbers;
            //cargoShipmentPM.IncotermName = shipmentOrderPM != null ? shipmentOrderPM.IncotermCode : shipmentPM?.IncotermCode;
            cargoShipmentPM.WarehouseLegName = GetWarehouseLegName();
            cargoShipmentPM.ImportManifest = shipmentPM?.ImportManifest;
            cargoShipmentPM.TotalTax = shipmentPM?.TotalTax;
            cargoShipmentPM.ShipmentTypeName = shipmentPM?.ShipmentTypeName;
            cargoShipmentPM.ShipmentOrderQuantity = shipmentOrderPM?.Quantity;
            cargoShipmentPM.ShipmentOrderPONumber = shipmentOrderPM?.PONumber;
            cargoShipmentPM.SHOBookingConfirmationNumber = shipmentOrderPM?.BookingConfirmationNumber;
            cargoShipmentPM.SHOPODate = shipmentOrderPM?.PODate;
            cargoShipmentPM.SHOCarrierName = shipmentOrderPM?.CarrierName;
            //cargoShipmentPM.House = shipmentOrderPM?.House;
            cargoShipmentPM.CustomerEnglishName = shipmentPM?.CustomerName;



        }
        private void BuildShipmentRoute()
        {
            CargoTrackingShipmentRouteBuilder routeBuilder = new CargoTrackingShipmentRouteBuilder(shipmentOrderPM, shipmentPM, forwardingShipmentPM);
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
            var distinctDocuments = GetDistinctDocuments(documentsFilingPM);

            cargoShipmentPM.DocumentsFilings = new List<CargoDocumentsFiling>();
            foreach (var documentFiling in distinctDocuments)
            {
                MapCargoDocumentsFromDocumentsFilings(documentFiling);
            }
        }

        private List<DocumentsFilingPM> GetDistinctDocuments(List<DocumentsFilingPM> documentsFilingPM)
        {
            List<DocumentsFilingPM> distinctDocuments = new List<DocumentsFilingPM>();

            foreach (var doc in documentsFilingPM)
            {
                doc.DocumentTypeCode = doc.DocumentTypeCode.StartsWith("SO") ? doc.DocumentTypeCode.Remove(0, 2) : doc.DocumentTypeCode;
            }
            foreach (var document in documentsFilingPM)
            {
                if (!distinctDocuments.Any(x => x.DocumentTypeCode == document.DocumentTypeCode && x.CalculatedFileName == document.CalculatedFileName
                     && x.FileSize == document.FileSize))
                {
                    distinctDocuments.Add(document);
                }
            }
            return distinctDocuments;
        }

        private void FillConnectedOrders()
        {
            ShipmentOrderQueryService shipmentOrderQuery = new ShipmentOrderQueryService(cargoShipmentPM.Tenant);

            var shipmentOrders = new List<ShipmentOrderPM>();
            // custom shipment
            if (!string.IsNullOrWhiteSpace(cargoShipmentPM.ForwardingShipmentHeaderId))
            {
                shipmentOrders = shipmentOrderQuery.GetConnectedShipmentOrdersByShipmentId(cargoShipmentPM.ForwardingShipmentHeaderId, cargoShipmentPM.Tenant);
            }
            else
            {
                shipmentOrders = shipmentOrderQuery.GetConnectedShipmentOrdersByShipmentNumber(cargoShipmentPM.ShipmentNumber, cargoShipmentPM.Tenant);
            }
            if (shipmentOrders.Any())
            {
                CardQuery cardQuery = new CardQuery(cargoShipmentPM.Tenant);
                var cardsList =  cardQuery.GetCardListsByListIds(shipmentOrders.Select(x=>x.ShipperId).ToList(), cargoShipmentPM.Tenant).ToList();
                cargoShipmentPM.ConnectedOrders = shipmentOrders.Select(x => new ConnectedOrder
                {
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                    PONumber = x.PONumber,
                    SupplyDateTime = x.SupplyDateTime,
                    PODate = x.PODate,
                    PickupActualDateTime = x.PickupActualDateTime,
                    BookingConfirmationNumber = x.BookingConfirmationNumber,
                    SupplierName = GetShipperName(x, cardsList)

                }).ToList();
            }
        }

        private string GetShipperName(ShipmentOrderPM order, List<CardList> cardsList)
        {
            if (cardsList.Any() && order.DirectionId == "I" && order.ShipperId != null)
            {
                return cardsList.FirstOrDefault(a => a.Id == order.ShipperId).EnglishName;
            }
            return null;
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

            cargoShipmentPM.RouteFromPortName = fromPort?.EnglishName;
            cargoShipmentPM.RouteToPortName = toPort?.EnglishName;
        }

        private void SetTenantFields()
        {
            TenantManagementPM tenantManagment = GetTenantManagement(cargoShipmentPM.Tenant);
            cargoShipmentPM.ActivatedForDeclarationApprove = tenantManagment?.ActivatedforDeclarationApprove ?? false;
            cargoShipmentPM.ShowMoneyOrder = tenantManagment?.ShowMoneyOrder ?? false;         
            cargoShipmentPM.TenantDeclarationMessage = tenantManagment?.DeclarationMessage;
            cargoShipmentPM.CargoTrackingPrivateShowEvents = tenantManagment?.CargoTrackingPrivateShowEvents ?? false;
        }

        private void SetSharedLogisticsSettings()
        {
            SharedLogisticsSettingQueryService query = new SharedLogisticsSettingQueryService(cargoShipmentPM.Tenant);
            SharedLogisticsSettingPM setting = query.GetSingle(cargoShipmentPM.Tenant.ToString(), false, false);
            cargoShipmentPM.SharedLogisticsSetting = setting;
        }

        private void SetShipmentCloudDataFields()
        {
            ShipmentAdditionalCloudData cloudData = GetShipmentCloud(cargoShipmentPM);
            cargoShipmentPM.IsImporterApprovalRequried = cloudData?.IsImporterApprovalRequried ?? false;
            cargoShipmentPM.ApprovedDate = cloudData?.ApproveDateTime;
            cargoShipmentPM.ApprovedByUserName = cloudData?.ApprovedByUserName;
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

        private ShipmentPM GetForwardingShipment()
        {
            if (!string.IsNullOrWhiteSpace(cargoShipmentPM.ForwardingShipmentHeaderId))
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(cargoShipmentPM.Tenant);
                forwardingShipmentPM = shipmentQuery.GetShipmentPMForCargoTrackingByEntityId(cargoShipmentPM.ForwardingShipmentHeaderId, cargoShipmentPM.Tenant);
                return forwardingShipmentPM;
            }
            return null;
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

            if (shipmentPackages.Any(x => x.PackageTypeName == null || x.PackageTypeName == "---" || x.PackageTypeName == "" 
            || x.ContainerNumber == null || x.ContainerNumber == "---" || x.ContainerNumber == "")
                && !string.IsNullOrWhiteSpace(cargoShipmentPM.ForwardingShipmentHeaderId) && cargoShipmentPM.EntityType == "C")
            {
                List<ShipmentPackagePM> forwardingShipmentPackages = shipmentQuery.GetPackagesOfShipment(cargoShipmentPM.Tenant, cargoShipmentPM.ForwardingShipmentHeaderId);

                foreach (var pkg in shipmentPackages)
                {
                    var forwardingPkg = forwardingShipmentPackages.FirstOrDefault(x => x.Quantity == pkg.Quantity && x.Weight == pkg.Weight && x.Volume == pkg.Volume);
                    if (pkg.PackageTypeName == null || pkg.PackageTypeName == "---" || pkg.PackageTypeName == "")
                    {
                        pkg.PackageTypeName = forwardingPkg?.PackageTypeName;
                    }
                    if (pkg.ContainerNumber == null || pkg.ContainerNumber == "---" || pkg.ContainerNumber == "")
                    {
                        pkg.ContainerNumber = forwardingPkg?.ContainerNumber;
                    }
                }
            }
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
                CargoIdentifier3 = customsData.CargoIdentifier3,
                SupplierName = customsData.SupplierName
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
            
            string[] allowedDocumentTypes = { "EINV", "CINV", "FINV" };

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(cargoShipmentPM.Tenant);
            List<DocumentsFilingPM> documentsFilingPM = documentsFilingQuery.GetInputDocumentsFilingPMsByEntityId(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);
            documentsFilingPM = documentsFilingPM.Where(document =>
           (allowedDocumentTypes.Contains(document.DocumentTypeCode.ToUpper()) && document.BillToId == cargoShipmentPM.CustomerId)
           || !allowedDocumentTypes.Contains(document.DocumentTypeCode.ToUpper())).ToList();

             if (!string.IsNullOrWhiteSpace(cargoShipmentPM.ForwardingShipmentHeaderId))
            {
                List<DocumentsFilingPM> forwardingShipmentDocumentsFiling = documentsFilingQuery
                    .GetInputDocumentsFilingPMsByEntityId(cargoShipmentPM.ForwardingShipmentHeaderId, cargoShipmentPM.Tenant);
                AddForwardingShipmentsDocsToCustomsShipment(documentsFilingPM, forwardingShipmentDocumentsFiling);
            }

            if (cargoShipmentPM.ConnectedOrders!= null && cargoShipmentPM.ConnectedOrders.Any()) {
                foreach (var order in cargoShipmentPM.ConnectedOrders)
                {
                    List<DocumentsFilingPM> orderShipmentDocumentsFiling = documentsFilingQuery
                                        .GetInputDocumentsFilingPMsByEntityId(order.Id, cargoShipmentPM.Tenant);

                    if (orderShipmentDocumentsFiling.Any())
                    {
                        documentsFilingPM.AddRange(orderShipmentDocumentsFiling);
                    }
                }
                
            }

            return documentsFilingPM;
        }

        private void AddForwardingShipmentsDocsToCustomsShipment(List<DocumentsFilingPM> documentsFilingPM, List<DocumentsFilingPM> forwardingShipmentDocumentsFiling)
        {
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
                CalculatedFileName = document.CalculatedFileName,
                DocumentId = document.DocumentId,
                Code = document.Code,
                DocumentTypeId = document.DocumentTypeId,
                DocumentTypeName = document.DocumentTypeName,
                DocumentTypeCode = document.DocumentTypeCode,
                CreateDate = document.CreateDate,
                IsDigitallySigned = document.IsDigitallySigned,
                SecurityId = document.SecurityId,
                CustomReference = document.CustomReference,
                FileExtension = document.FileExtension,
                Descreption = document.Description
            });
        }
        public void SetMilestonesStatus()
        {
            SetCurrentMilestone();
            SetDoneMilstones();
            SetFutureMilstoneForShipment();
            Milestone currentMilestone = cargoShipmentPM.Milestones.Where(x => x.IsCurrent == true).FirstOrDefault();
            SetCurrentMilstoneFields(currentMilestone);
        }
        private void SetCurrentMilstoneFields(Milestone currentMilestone)
        {
            if (currentMilestone != null)
            {
                cargoShipmentPM.CurrentMilestoneCode = currentMilestone.Code;
                cargoShipmentPM.CurrentMilestoneName = currentMilestone.Name;
                cargoShipmentPM.CurrentMilestoneDate = currentMilestone.Date;
            }
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
            var currentMilestone = milestones.Where(s => s.IsEstimation != true && s.Date != null && s.Date.Value.Date <= DateTime.Now.Date).OrderByDescending(s => s.Weight).FirstOrDefault();
            // get first milestone
            if (currentMilestone == null)
            {
                currentMilestone = milestones.Where(s => s.IsEstimation != true && s.Date != null).OrderByDescending(s => s.Weight).LastOrDefault();
            }
            return currentMilestone;
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