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
        public CargoTrackingShipmentPM cargoShipmentPM;
        ShipmentOrderPM shipmentOrderPM;
        ShipmentPM shipmentPM;


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
            BuildShipmentMilstones();
            SetMilestonesStatus();
            SetRoutePortsCodes(cargoShipmentPM);
        }

       
        private void GetConnectedEntities()
        {
            GetShipmentOrder();
            GetShipmentPM();
        }
        private void MapShipmentPMFields()
        {
            cargoShipmentPM.CustomsBrokerReference = shipmentPM.CustomFileNumber;
            cargoShipmentPM.ContainersNumbers = shipmentPM.ContainersNumbers;
            cargoShipmentPM.IncotermName = shipmentOrderPM != null ? shipmentOrderPM.IncotermName : shipmentPM.IncotermName;
            cargoShipmentPM.WarehouseLegEnglishName = shipmentPM.WarehouseLegEnglishName;
            cargoShipmentPM.ImportManifest = shipmentPM.ImportManifest;
            cargoShipmentPM.TotalTax = shipmentPM.TotalTax;
            cargoShipmentPM.ShipmentTypeName = shipmentPM.ShipmentTypeName;
            cargoShipmentPM.ShipmentOrderQuantity = shipmentOrderPM?.Quantity;
            cargoShipmentPM.ShipmentOrderPONumber = shipmentOrderPM?.PONumber;
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

        public void BuildShipmentMilstones()
        {
            cargoShipmentPM.Milestones = new List<Milestone>
            {
                new Milestone()
                {
                    Id = 1,
                    Code = "Created",
                    Name = "Created",
                    Date = cargoShipmentPM.CreateDate,
                    EstimationDate = null,
                    Done = true,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = false
                },


                new Milestone()
                {
                    Id = 2,
                    Code = "Booking",
                    Name = "Booking",
                    Date = cargoShipmentPM.BookingDate,
                    EstimationDate = cargoShipmentPM.BookingEstimationDate,
                    Done = cargoShipmentPM.BookingDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.BookingDate == null && cargoShipmentPM.BookingEstimationDate != null
                },
                new Milestone()
                {
                    Id = 3,
                    Code = "Pickup",
                    Name = "Pickup",
                    Date = cargoShipmentPM.PickupDate,
                    EstimationDate = cargoShipmentPM.PickupEstimationDate,
                    Done = cargoShipmentPM.PickupDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.PickupDate == null && cargoShipmentPM.PickupEstimationDate != null
                },
                new Milestone()
                {
                    Id = 4,
                    Code = "OriginWarehouse",
                    Name = "Origin Warehouse",
                    Date = cargoShipmentPM.FromWarehouseDate,
                    EstimationDate = cargoShipmentPM.FromWarehouseEstimationDate,
                    Done = cargoShipmentPM.FromWarehouseDone,
                    Notes = cargoShipmentPM.FromWarehouseNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.FromWarehouseDate == null && cargoShipmentPM.FromWarehouseEstimationDate != null
                },
                new Milestone()
                {
                    Id = 5,
                    Code = "Departure",
                    Name = "Departure",
                    Date = cargoShipmentPM.DepartureDate,
                    EstimationDate = cargoShipmentPM.DepartureEstimationDate,
                    Done = cargoShipmentPM.DepartureDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DepartureDate == null && cargoShipmentPM.DepartureEstimationDate != null
                },
                new Milestone()
                {
                    Id = 6,
                    Code = "Arrival",
                    Name = "Arrival",
                    Date = cargoShipmentPM.ArrivalDate,
                    EstimationDate = cargoShipmentPM.ArrivalEstimationDate,
                    Done = cargoShipmentPM.ArrivalDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.ArrivalDate == null && cargoShipmentPM.ArrivalEstimationDate != null
                },
                new Milestone()
                {
                    Id = 7,
                    Code = "DestinationWarehouse",
                    Name = "Destination Warehouse",
                    Date = cargoShipmentPM.ToWarehouseDate,
                    EstimationDate = cargoShipmentPM.ToWarehouseEstimationDate,
                    Done = cargoShipmentPM.ToWarehouseDone,
                    Notes = cargoShipmentPM.ToWarehouseNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.ToWarehouseDate == null && cargoShipmentPM.ToWarehouseEstimationDate != null
                },
                new Milestone()
                {
                    Id = 8,
                    Code = "AssignedToCustomsBroker",
                    Name = "Assigned To Customs Broker",
                    Date = cargoShipmentPM.AssignedCustomsAgentDate,
                    EstimationDate = cargoShipmentPM.AssignedCustomsAgentEstDate,
                    Done = cargoShipmentPM.AssignedCustomsAgentDone,
                    Notes = cargoShipmentPM.AssignedCustomsAgentNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.AssignedCustomsAgentDate == null && cargoShipmentPM.AssignedCustomsAgentEstDate != null
                },
                new Milestone()
                {
                    Id = 9,
                    Code = "CustomsProcess",
                    Name = "Customs Process",
                    //Date = cargoShipmentPM.process,
                    EstimationDate = null,
                    //Done = cargoShipmentPM.CustomsPaymentDone,
                    Notes = null,
                    IsCurrent = false,
                    //IsEstimation = !cargoShipmentPM.CustomsPaymentDone
                },
                new Milestone()
                {
                    Id = 10,
                    Code = "GoodsClassification",
                    Name = "Goods Classification",
                    Date = cargoShipmentPM.GoodsClassificationDate,
                    EstimationDate = cargoShipmentPM.GoodsClassificationEstDate,
                    Done = cargoShipmentPM.GoodsClassificationDone,
                    Notes = cargoShipmentPM.GoodsClassificationNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.GoodsClassificationDate == null && cargoShipmentPM.GoodsClassificationEstDate != null
                },
                new Milestone()
                {
                    Id = 11,
                    Code = "DocumentInspection",
                    Name = "Document Inspection",
                    Date = cargoShipmentPM.DocumentInspectionDate,
                    EstimationDate = cargoShipmentPM.DocumentInspectionEstDate,
                    Done = cargoShipmentPM.DocumentInspectionDone,
                    Notes = cargoShipmentPM.DocumentInspectionNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DocumentInspectionDate == null && cargoShipmentPM.DocumentInspectionEstDate != null
                },
                new Milestone()
                {
                    Id = 12,
                    Code = "PaymentRequested",
                    Name = "Payment Requested",
                    Date = cargoShipmentPM.PaymentRequiredDate,
                    EstimationDate = cargoShipmentPM.PaymentRequiredEstimationDate,
                    Done = cargoShipmentPM.PaymentRequiredDone,
                    Notes = cargoShipmentPM.PaymentRequiredNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.PaymentRequiredDate == null && cargoShipmentPM.PaymentRequiredEstimationDate != null
                },
                new Milestone()
                {
                    Id = 13,
                    Code = "PaymentReceived",
                    Name = "Payment Received",
                    Date = cargoShipmentPM.PaymentReceivedDate,
                    EstimationDate = cargoShipmentPM.PaymentReceivedEstomationDate,
                    Done = cargoShipmentPM.PaymentReceivedDone,
                    Notes = cargoShipmentPM.PaymentReceivedNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.PaymentReceivedDate == null && cargoShipmentPM.PaymentReceivedEstomationDate != null
                },
                new Milestone()
                {
                    Id = 14,
                    Code = "CustomsPayment",
                    Name = "Customs Payment",
                    Date = cargoShipmentPM.CustomsPaymentDate,
                    EstimationDate = null,
                    Done = cargoShipmentPM.CustomsPaymentDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.CustomsPaymentDone != true
                },
                new Milestone()
                {
                    Id = 15,
                    Code = "Clearance",
                    Name = "Clearance",
                    Date = cargoShipmentPM.ClearanceDate,
                    EstimationDate = null,
                    Done = cargoShipmentPM.ClearanceDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.ClearanceDone != true
                },
                new Milestone()
                {
                    Id = 16,
                    Code = "GatepassArrived",
                    Name = "Gatepass Arrived",
                    Date = cargoShipmentPM.GatepassArrivedDate,
                    EstimationDate = cargoShipmentPM.GatepassArrivedEstDate,
                    Done = cargoShipmentPM.GatepassArrivedDone,
                    Notes = cargoShipmentPM.GatepassArrivedNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.GatepassArrivedDate == null && cargoShipmentPM.GatepassArrivedEstDate != null
                },
                new Milestone()
                {
                    Id = 17,
                    Code = "AssignedToTrucker",
                    Name = "Assigned To Trucker",
                    Date = cargoShipmentPM.AssignedTruckerDate,
                    EstimationDate = cargoShipmentPM.AssignedTruckerEstimationDate,
                    Done = cargoShipmentPM.AssignedTruckerDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.AssignedTruckerDate == null && cargoShipmentPM.AssignedTruckerEstimationDate != null
                },
                new Milestone()
                {
                    Id = 18,
                    Code = "DeliveryOnTheWay",
                    Name = "Delivery on the way",
                    Date = cargoShipmentPM.DeliveryDate,
                    EstimationDate = cargoShipmentPM.DeliveryEstimationDate,
                    Done = cargoShipmentPM.DeliveryDone,
                    Notes = cargoShipmentPM.DeliveryNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DeliveryDate == null && cargoShipmentPM.DeliveryEstimationDate != null
                },
                new Milestone()
                {
                    Id = 19,
                    Code = "Delivered",
                    Name = "Delivered",
                    Date = cargoShipmentPM.DeliveredDate,
                    EstimationDate = cargoShipmentPM.DeliveredEstimationDate,
                    Done = cargoShipmentPM.DeliveredDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DeliveredDate == null && cargoShipmentPM.DeliveredEstimationDate != null
                },
                new Milestone()
                {
                    Id = 20,
                    Code = "Invoiced",
                    Name = "Invoiced",
                    //Date = cargoShipmentPM.invoi,
                    //EstimationDate = cargoShipmentPM.DeliveredEstimationDate,
                    //Done = cargoShipmentPM.DeliveredDone,
                    Notes = null,
                    IsCurrent = false,
                    //IsEstimation = !cargoShipmentPM.DeliveredDone
                }
            };

            cargoShipmentPM.Milestones = cargoShipmentPM.Milestones.OrderByDescending(d => d.Id).ToList();
        }

        private void SetRoutePortsCodes(CargoTrackingShipmentPM cargoShipmentPM)
        {
            CargoTrackingPortPM fromPort = GetCargoPort(cargoShipmentPM.FromPortCode);
            CargoTrackingPortPM toPort = GetCargoPort(cargoShipmentPM.ToPortCode);

            cargoShipmentPM.RouteFromPortCode = fromPort?.Code;
            cargoShipmentPM.RouteToPortCode = toPort?.Code;
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
            shipmentOrderPM = shipmentOrderQuery.GetSingle(cargoShipmentPM.EntityId, true, false);
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
                var doneMilstones = cargoShipmentPM.Milestones.Where(milstone => milstone.Id < currentMilstone.Id).ToList();
                doneMilstones.ForEach(doneMilstone =>
                {
                    doneMilstone.Done = true;
                    doneMilstone.IsEstimation = false;
                });
            }
        }
        private Milestone GetMostRecentNotEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation != true && s.Date != null).OrderByDescending(s => s.Id).FirstOrDefault();
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
                                                        .OrderByDescending(s => s.Id)
                                                        .FirstOrDefault();
        }
    }



}