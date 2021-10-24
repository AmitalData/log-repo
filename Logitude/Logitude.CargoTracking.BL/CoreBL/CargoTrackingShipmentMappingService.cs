using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public class CargoTrackingShipmentMappingService
    {
        const string OceanTransportMode = "O";
        const string AirTransportMode = "A";
        const string ShipmentOrderEntityType = "O";
        const string InlandTransportMode = "I";
        const string WarehouseTransportMode = "W";
        CargoTrackingShipmentPM cargoShipmentPM;
        ShipmentOrderPM shipmentOrderPM;
        ShipmentPM shipmentPM;
        bool isShipmentOrderEntity { get { return cargoShipmentPM.EntityType == ShipmentOrderEntityType; } }


        public void MapCargoTrackingShipmentFields(CargoTrackingShipmentPM cargoShipmentPM)
        {
            this.cargoShipmentPM = cargoShipmentPM;
            GetConnectedEntities();


            MapShipmentPMFields();
            BuildShipmentRoute();
            FillShipmentPackages();
            FillCustomsData();
            FillDocumentsFilings();
            BuildPartnerCards();

        }
        private void GetConnectedEntities()
        {
            GetShipmentOrder();
            GetShipmentPM();
            GetPartnerAddresses();
        }
        private void MapShipmentPMFields()
        {
            cargoShipmentPM.CustomsBrokerReference = shipmentPM.CustomFileNumber;
            cargoShipmentPM.ContainersNumbers = shipmentPM.ContainersNumbers;
        }
        private void BuildShipmentRoute()
        {
            CargoTrackingShipmentRouteBuilder routeBuilder = new CargoTrackingShipmentRouteBuilder(shipmentOrderPM, shipmentPM);
            routeBuilder.BuildRoute();
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
            CargoTrackingShipmenPartnersCardsBuilder partnerCardsBuilder = new CargoTrackingShipmenPartnersCardsBuilder(shipmentOrderPM, shipmentPM);
            partnerCardsBuilder.BuildPartnerCards();
        }
        private ShipmentOrderPM GetShipmentOrder()
        {
            ShipmentOrderQueryService shipmentOrderQuery = new ShipmentOrderQueryService(cargoShipmentPM.Tenant);
            shipmentOrderPM = shipmentOrderQuery.GetSingle(cargoShipmentPM.EntityId, true, false);
            return shipmentOrderPM;
        }
        private ShipmentPM GetShipmentPM()
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(cargoShipmentPM.Tenant);
            shipmentPM = shipmentQuery.GetShipmentPMForCargoTrackingByEntityId(cargoShipmentPM.EntityId, cargoShipmentPM.Tenant);
            return shipmentPM;
        }
        private void GetPartnerAddresses()
        {
            var partnersIds = GetShipmentPartnersIds(shipmentPM, cargoShipmentPM, shipmentOrderPM);
            AddressQuery addressQuery = new AddressQuery(cargoShipmentPM.Tenant);
            PartnersAddresses = addressQuery.GetAddressesByCardIds(partnersIds, cargoShipmentPM.Tenant);
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
            return documentsFilingPM;
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
        private List<string> GetShipmentPartnersIds(ShipmentPM shipmentPM, CargoTrackingShipmentPM cargoShipmentPM, ShipmentOrderPM shipmentOrderPM)
        {
            if (cargoShipmentPM.EntityType == ShipmentOrderEntityType)
            {
                return new List<string>() {
                    shipmentOrderPM.ShipperId,
                    shipmentOrderPM.ConsigneeId,
                    shipmentOrderPM.AgentId,
                    shipmentOrderPM.CarrierId
                };
            }
            else
            {

                List<string> partnersIds = new List<string>() {
                    shipmentPM.ShipperId,
                    shipmentPM.ConsigneeId,
                    shipmentPM.FreightForwarderId,
                    shipmentPM.CustomerId,
                    shipmentPM.AgentId,
                    shipmentPM.IssuingCarrierAgentId,
                    shipmentPM.CustomAgentExportId,
                    shipmentPM.CustomAgentImportId,
                    shipmentPM.Notify1Id,
                    shipmentPM.Notify2Id,
                    shipmentPM.ShipperNotExporterId,
                    shipmentPM.ConsigneeNotImporterId,
                    shipmentPM.CustomClearancePointId,
                    shipmentPM.ColoaderId,
                    shipmentPM.FreelancerId,
                    shipmentPM.ConsolidatorId,
                    shipmentPM.ReleasingAgentId,
                    shipmentPM.MainCarriageCarrierId,
                    shipmentPM.WarehouseLegWarehouseId,
                    cargoShipmentPM.ShipperId
                };

                partnersIds.AddRange(shipmentPM.ShipmentDeliveries.Select(d => d.CarrierId).ToList());
                partnersIds.AddRange(shipmentPM.ShipmentPickUps.Select(d => d.CarrierId).ToList());
                return partnersIds;
            }

        }
    }



}