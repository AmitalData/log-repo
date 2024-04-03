using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviour
{
    public class UpdateShipmentComputedFieldsBehaviour : IShipmentBehaviour
    {
        private ShipmentComputedFields entity;
        private ShipmentPM shipmentPM;
        private IShipmentsContext context;
        private ICommonDataContext commonContext;
        private ShipmentComputedFieldsRepository shipmentComputedFieldsRepository;
        private ObjectTableRepository objectTableRepository;
        private DocumentsFilingQuery documentsFilingQuery;
        private ContactRepository contactRepository;
        private PortRepository portRepository;
        private AddressRepository addressRepository;
        private int tenant;
        private bool isNewEntity;

        public bool ReceivablePricingUpdated { get; set; }
        public bool DatesFromCrossDocsUpdated { get; set; }

        public UpdateShipmentComputedFieldsBehaviour(ShipmentPM shipmentPM, IShipmentsContext context, ShipmentComputedFields shipmentComputedFields, bool isNewEntity)
        {
            this.shipmentPM = shipmentPM;
            this.context = context;
            this.isNewEntity = isNewEntity;
            this.tenant = shipmentPM.Tenant;
            this.entity = shipmentComputedFields;

            commonContext = CommonDataContext.GetContext(tenant);
            shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(context);
            objectTableRepository = new ObjectTableRepository(tenant);
            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            contactRepository = new ContactRepository(commonContext);
            portRepository = new PortRepository(commonContext);
            addressRepository = new AddressRepository(commonContext);
        }

        public void Handle(List<FieldChange> fieldChanges = null)
        {
            GetEntity();
            MapEntity(fieldChanges);
        }

        private void GetEntity()
        {
            if (isNewEntity)
            {
                entity = new ShipmentComputedFields()
                {
                    Id = shipmentPM.Id,
                    Tenant = tenant,
                };
            }

            else if (entity == null)
            {
                entity = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentPM.Id, tenant);
            }
        }

        private void MapEntity(List<FieldChange> fieldChanges)
        {
            MapFields(fieldChanges);
            MapContainersNumbersAndTypesArray(fieldChanges);
            MapFirstPickUp(fieldChanges);
            MapLastPickUp(fieldChanges);
            MapLastDelivery(fieldChanges);
            MapFirstDelivery(fieldChanges);
            MapFieldsWhenChanged(fieldChanges);
            MapOperationalClosed(fieldChanges);
            MapCustomsClearance(fieldChanges);
            MapMasterHouses(fieldChanges);
            MapDocumentFields(fieldChanges);
            MapAccountingClosed(fieldChanges);
            MapMainCarriageDates(fieldChanges);
            MapTransshipments(fieldChanges);

            shipmentPM.IsShipmentComputedFieldChange = false;
            shipmentPM.IsDepositionRequired = entity.IsDepositionRequired;
            shipmentPM.IsRequestedDocuments = entity.IsRequestedDocuments;
            shipmentPM.IsDigitalSignRequired = entity.IsDigitalSignRequired;
            shipmentPM.CreatedFromDigital = entity.CreatedFromDigital;
        }

        private void MapMainCarriageDates(List<FieldChange> fieldChanges)
        {
            if (shipmentPM.ShipmentLevelCode == "H" && shipmentPM.IsConnectToMasterShipment) return;

            FieldChange.Add(entity.MainCarriageETA, shipmentPM.MainCarriageETA, nameof(shipmentPM.MainCarriageETA), fieldChanges);
            entity.MainCarriageETA = shipmentPM.MainCarriageETA;
            
            FieldChange.Add(entity.MainCarriageETD, shipmentPM.MainCarriageETD, nameof(shipmentPM.MainCarriageETD), fieldChanges);
            entity.MainCarriageETD = shipmentPM.MainCarriageETD;
            
            FieldChange.Add(entity.MainCarriageATA, shipmentPM.MainCarriageATA, nameof(shipmentPM.MainCarriageATA), fieldChanges);
            entity.MainCarriageATA = shipmentPM.MainCarriageATA;
            
            FieldChange.Add(entity.MainCarriageATD, shipmentPM.MainCarriageATD, nameof(shipmentPM.MainCarriageATD), fieldChanges);
            entity.MainCarriageATD = shipmentPM.MainCarriageATD;
        }

        private void MapDocumentFields(List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entity.BookingConfirmationSent, shipmentPM.BookingConfirmationSentDate, nameof(entity.BookingConfirmationSent), fieldChanges);
            entity.BookingConfirmationSent = shipmentPM.BookingConfirmationSentDate;
            
            FieldChange.Add(entity.PreAlertSent, shipmentPM.PreAlertSentDate, nameof(entity.PreAlertSent), fieldChanges);
            entity.PreAlertSent = shipmentPM.PreAlertSentDate;
            
            FieldChange.Add(entity.DeliveryNoticeSent, shipmentPM.DeliveryNoticeSentDate, nameof(entity.DeliveryNoticeSent), fieldChanges);
            entity.DeliveryNoticeSent = shipmentPM.DeliveryNoticeSentDate;
            
            FieldChange.Add(entity.ExpectedArrivalNoticeSent, shipmentPM.ExpectedArrivalNoticeSentDate, nameof(entity.ExpectedArrivalNoticeSent), fieldChanges);
            entity.ExpectedArrivalNoticeSent = shipmentPM.ExpectedArrivalNoticeSentDate;
            
            FieldChange.Add(entity.ArrivalNoticeSent, shipmentPM.ArrivalNoticeSentDate, nameof(entity.ArrivalNoticeSent), fieldChanges);
            entity.ArrivalNoticeSent = shipmentPM.ArrivalNoticeSentDate;
            
            FieldChange.Add(entity.T1Received, shipmentPM.T1ReceivedDate, nameof(entity.T1Received), fieldChanges);
            entity.T1Received = shipmentPM.T1ReceivedDate;
        }

        public void Trace(ShipmentTracing shipmentTracing)
        {

        }

        public void Save()
        {
            if (isNewEntity)
            {
                shipmentComputedFieldsRepository.Add(entity);
            }

            else
            {
                shipmentComputedFieldsRepository.Update(entity);
            }


            shipmentComputedFieldsRepository.SubmitChanges();
        }

        private void MapFields(List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entity.FirstPickupLocation, shipmentPM.FirstPickupLocation, nameof(shipmentPM.FirstPickupLocation), fieldChanges);
            entity.FirstPickupLocation = shipmentPM.FirstPickupLocation;

            FieldChange.Add(entity.Commodity, shipmentPM.AWBCommodityItemNumber, nameof(shipmentPM.AWBCommodityItemNumber), fieldChanges);
            entity.Commodity = shipmentPM.AWBCommodityItemNumber;

            var numberOfDeliveries = shipmentPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();
            FieldChange.Add(entity.NumberOfDeliveries, numberOfDeliveries, nameof(entity.NumberOfDeliveries), fieldChanges);
            entity.NumberOfDeliveries = numberOfDeliveries;
            
            FieldChange.Add(entity.LastDocumentDateTime, null, nameof(entity.LastDocumentDateTime), fieldChanges);
            entity.LastDocumentDateTime = null;
            
            FieldChange.Add(entity.CreatedFromDigital, shipmentPM.CreatedFromDigital, nameof(shipmentPM.CreatedFromDigital), fieldChanges);
            entity.CreatedFromDigital = shipmentPM.CreatedFromDigital;

            FieldChange.Add(entity.IsDocumentsNeedApprove, shipmentPM.IsDocumentsNeedApprove, nameof(entity.IsDocumentsNeedApprove), fieldChanges);
            entity.IsDocumentsNeedApprove = shipmentPM.IsDocumentsNeedApprove;
        }

        private void MapContainersNumbersAndTypesArray(List<FieldChange> fieldChanges)
        {
            if (shipmentPM.ShipmentPackages != null)
            {
                MapDetailsFromShipmentPackages(fieldChanges);
            }
        }
        private void MapDetailsFromShipmentPackages(List<FieldChange> fieldChanges)
        {
            string shipmentContainersNumbers = string.Empty;
            string shipmentContainersNumbersAndTypesArray = string.Empty;
            string packagesQuantityAndType = null;
            foreach (ShipmentPackagePM packagePM in shipmentPM.ShipmentPackages.Where(p => p.ChangeSetOp != ChangeSetOperation.Delete))
            {
                shipmentContainersNumbers = AddPackageContainerNumberToShipmentContainersNumbers(shipmentContainersNumbers, packagePM.ContainerNumber);
                shipmentContainersNumbersAndTypesArray = AddPackageContainerNumberAndTypeToShipmentContainersNumbersAndTypesArray(shipmentContainersNumbersAndTypesArray, packagePM.ContainerNumber, packagePM.PackageTypeCode);
                packagesQuantityAndType = AddPackageDetailsToPackagesQuantityAndType(packagesQuantityAndType, packagePM.Quantity, packagePM.PackageTypeCode);
            }

            if (!string.IsNullOrEmpty(shipmentContainersNumbers) && shipmentContainersNumbers.Length > 1000)
            {
                shipmentContainersNumbers = shipmentContainersNumbers.Substring(0, 1000);
            }

            if (!string.IsNullOrEmpty(packagesQuantityAndType) && packagesQuantityAndType.Length > 2000)
            {
                packagesQuantityAndType = packagesQuantityAndType.Substring(0, 2000);
            }

            var containersNumbers = string.IsNullOrEmpty(shipmentContainersNumbers) ? null : shipmentContainersNumbers;
            FieldChange.Add(entity.ContainersNumbers, containersNumbers, nameof(entity.ContainersNumbers), fieldChanges);
            entity.ContainersNumbers = containersNumbers;

            var containersNumbersAndTypesArray = string.IsNullOrEmpty(shipmentContainersNumbersAndTypesArray) ? null : shipmentContainersNumbersAndTypesArray;
            FieldChange.Add(entity.ContainersNumbersAndTypesArray, containersNumbersAndTypesArray, nameof(entity.ContainersNumbersAndTypesArray), fieldChanges);
            entity.ContainersNumbersAndTypesArray = containersNumbersAndTypesArray;

            FieldChange.Add(entity.PackagesQuantityAndType, packagesQuantityAndType, nameof(entity.PackagesQuantityAndType), fieldChanges);
            entity.PackagesQuantityAndType = packagesQuantityAndType;
        }

        private string AddPackageContainerNumberToShipmentContainersNumbers(string shipmentContainersNumbers, string packageContainerNumber)
        {
            string allShipmentContainersNumbers = shipmentContainersNumbers;
            if (!string.IsNullOrEmpty(packageContainerNumber))
            {
                allShipmentContainersNumbers += (!string.IsNullOrEmpty(allShipmentContainersNumbers) ? ", " : "") + packageContainerNumber;
            }
            return allShipmentContainersNumbers;
        }
        private string AddPackageContainerNumberAndTypeToShipmentContainersNumbersAndTypesArray(string shipmentContainersNumbersAndTypesArray, string packageContainerNumber, string packageTypeCode)
        {
            string allShipmentContainersNumbersAndTypesArray = shipmentContainersNumbersAndTypesArray;
            if (!string.IsNullOrEmpty(packageContainerNumber))
            {
                allShipmentContainersNumbersAndTypesArray += (!string.IsNullOrEmpty(allShipmentContainersNumbersAndTypesArray) ? ", " : "") + packageContainerNumber + "[" + packageTypeCode + "]";
            }
            return allShipmentContainersNumbersAndTypesArray;
        }
        private string AddPackageDetailsToPackagesQuantityAndType(string packagesQuantityAndType, int? quantity, string packageTypeCode)
        {
            string allPackagesQuantityAndType = packagesQuantityAndType;
            if (quantity == null || quantity <= 0)
            {
                return allPackagesQuantityAndType;
            }

            if (!string.IsNullOrEmpty(packageTypeCode))
            {
                allPackagesQuantityAndType += (!string.IsNullOrEmpty(allPackagesQuantityAndType) ? ", " : "") + (quantity + "x" + packageTypeCode);
            }

            else
            {
                allPackagesQuantityAndType += (!string.IsNullOrEmpty(allPackagesQuantityAndType) ? ", " : "") + quantity;
            }

            return allPackagesQuantityAndType;
        }

        private void MapFirstPickUp(List<FieldChange> fieldChanges)
        {
            ShipmentPickUpPM firstPickUp = shipmentPM.ShipmentPickUps.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (firstPickUp == null)
            {
                FieldChange.Add(entity.PickupTo, null, nameof(entity.PickupTo), fieldChanges);
                entity.PickupTo = null;
                
                FieldChange.Add(entity.PickupFrom, null, nameof(entity.PickupFrom), fieldChanges);
                entity.PickupFrom = null;
                
                FieldChange.Add(entity.FirstPickupATA, null, nameof(entity.FirstPickupATA), fieldChanges);
                entity.FirstPickupATA = null;
                
                FieldChange.Add(entity.FirstPickupATD, null, nameof(entity.FirstPickupATD), fieldChanges);
                entity.FirstPickupATD = null;

                FieldChange.Add(entity.PickupTruckerId, null, nameof(entity.PickupTruckerId), fieldChanges);
                entity.PickupTruckerId = null;
                
                FieldChange.Add(entity.PickupTruckerNumber, null, nameof(entity.PickupTruckerNumber), fieldChanges);
                entity.PickupTruckerNumber = null;
                
                FieldChange.Add(entity.PickupDriver, null, nameof(entity.PickupDriver), fieldChanges);
                entity.PickupDriver = null;
                
                FieldChange.Add(entity.PickupTrailerNumber, null, nameof(entity.PickupTrailerNumber), fieldChanges);
                entity.PickupTrailerNumber = null;
                
                FieldChange.Add(entity.PickupNotes, null, nameof(entity.PickupNotes), fieldChanges);
                entity.PickupNotes = null;
                
                FieldChange.Add(entity.OnHandDate, null, nameof(entity.OnHandDate), fieldChanges);
                entity.OnHandDate = null;
            }
            else
            {
                FieldChange.Add(entity.FirstPickupATA, firstPickUp.ATA, nameof(entity.FirstPickupATA), fieldChanges);
                entity.FirstPickupATA = firstPickUp.ATA;

                FieldChange.Add(entity.FirstPickupATD, firstPickUp.ATD, nameof(entity.FirstPickupATD), fieldChanges);
                entity.FirstPickupATD = firstPickUp.ATD;

                FieldChange.Add(entity.PickupTruckerId, firstPickUp.CarrierId, nameof(entity.PickupTruckerId), fieldChanges);
                entity.PickupTruckerId = firstPickUp.CarrierId;
                
                FieldChange.Add(entity.PickupTruckerNumber, firstPickUp.CarrierNumber, nameof(entity.PickupTruckerNumber), fieldChanges);
                entity.PickupTruckerNumber = firstPickUp.CarrierNumber;
                
                FieldChange.Add(entity.PickupDriver, firstPickUp.Driver, nameof(entity.PickupDriver), fieldChanges);
                entity.PickupDriver = firstPickUp.Driver;
                
                FieldChange.Add(entity.PickupTrailerNumber, firstPickUp.TrailerNumber, nameof(entity.PickupTrailerNumber), fieldChanges);
                entity.PickupTrailerNumber = firstPickUp.TrailerNumber;
                
                FieldChange.Add(entity.PickupNotes, firstPickUp.Notes, nameof(entity.PickupNotes), fieldChanges);
                entity.PickupNotes = firstPickUp.Notes;
                
                FieldChange.Add(entity.OnHandDate, firstPickUp.ATA, nameof(entity.OnHandDate), fieldChanges);
                entity.OnHandDate = firstPickUp.ATA;

                if (firstPickUp.PickUpDeliveryToTypeCode == "PART")
                {
                    var pickupTo = GetPrtnerAddressCity(firstPickUp.ToAddressId);
                    FieldChange.Add(entity.PickupTo, pickupTo, nameof(entity.PickupTo), fieldChanges);
                    entity.PickupTo = pickupTo;
                }
                else if (firstPickUp.PickUpDeliveryToTypeCode == "CASL")
                {
                    FieldChange.Add(entity.PickupTo, firstPickUp.ToAddressCity, nameof(entity.PickupTo), fieldChanges);
                    entity.PickupTo = firstPickUp.ToAddressCity;
                }
                else
                {
                    var pickupTo = GetPortName(firstPickUp.ToPortId);
                    FieldChange.Add(entity.PickupTo, pickupTo, nameof(entity.PickupTo), fieldChanges);
                    entity.PickupTo = pickupTo;
                }

                if (firstPickUp.PickUpDeliveryFromTypeCode == "PART")
                {
                    var pickupFrom = GetPrtnerAddressCity(firstPickUp.FromAddressId);
                    FieldChange.Add(entity.PickupFrom, pickupFrom, nameof(entity.PickupFrom), fieldChanges);
                    entity.PickupFrom = pickupFrom;
                }
                else if (firstPickUp.PickUpDeliveryFromTypeCode == "CASL")
                {
                    FieldChange.Add(entity.PickupFrom, firstPickUp.FromAddressCity, nameof(entity.PickupFrom), fieldChanges);
                    entity.PickupFrom = firstPickUp.FromAddressCity;
                }
                else
                {
                    var pickupFrom = GetPortName(firstPickUp.FromPortId);
                    FieldChange.Add(entity.PickupFrom, pickupFrom, nameof(entity.PickupFrom), fieldChanges);
                    entity.PickupFrom = pickupFrom;
                }
            }
        }

        private void MapLastPickUp(List<FieldChange> fieldChanges)
        {
            ShipmentPickUpPM lastPickup = shipmentPM.ShipmentPickUps.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (lastPickup == null)
            {
                FieldChange.Add(entity.LastPickupATA, null, nameof(entity.LastPickupATA), fieldChanges);
                entity.LastPickupATA = null;
                
                FieldChange.Add(entity.LastPickupATD, null, nameof(entity.LastPickupATD), fieldChanges);
                entity.LastPickupATD = null;
                
                FieldChange.Add(entity.LastPickupETA, null, nameof(entity.LastPickupETA), fieldChanges);
                entity.LastPickupETA = null;
                
                FieldChange.Add(entity.LastPickupETD, null, nameof(entity.LastPickupETD), fieldChanges);
                entity.LastPickupETD = null;
            }

            else
            {
                FieldChange.Add(entity.LastPickupATA, lastPickup.ATA, nameof(entity.LastPickupATA), fieldChanges);
                entity.LastPickupATA = lastPickup.ATA;
                
                FieldChange.Add(entity.LastPickupATD, lastPickup.ATD, nameof(entity.LastPickupATD), fieldChanges);
                entity.LastPickupATD = lastPickup.ATD;
                
                FieldChange.Add(entity.LastPickupETA, lastPickup.ETA, nameof(entity.LastPickupETA), fieldChanges);
                entity.LastPickupETA = lastPickup.ETA;
                
                FieldChange.Add(entity.LastPickupETD, lastPickup.ETD, nameof(entity.LastPickupETD), fieldChanges);
                entity.LastPickupETD = lastPickup.ETD;
            }
        }

        private void MapLastDelivery(List<FieldChange> fieldChanges)
        {

            ShipmentDeliveryPM finalDelivery = shipmentPM.ShipmentDeliveries.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (finalDelivery == null)
            {
                FieldChange.Add(entity.DeliveryTo, null, nameof(entity.DeliveryTo), fieldChanges);
                entity.DeliveryTo = null;
                //entity.DeliveryToCity = null;
                
                FieldChange.Add(entity.DeliveryToPortId, null, nameof(entity.DeliveryToPortId), fieldChanges);
                entity.DeliveryToPortId = null;
                
                FieldChange.Add(entity.DeliveryFrom, null, nameof(entity.DeliveryFrom), fieldChanges);
                entity.DeliveryFrom = null;
                
                FieldChange.Add(entity.FinalDeliveryATA, null, nameof(entity.FinalDeliveryATA), fieldChanges);
                entity.FinalDeliveryATA = shipmentPM.FinalDeliveryATA = null;
                
                FieldChange.Add(entity.FinalDeliveryATD, null, nameof(entity.FinalDeliveryATD), fieldChanges);
                entity.FinalDeliveryATD = shipmentPM.FinalDeliveryATD = null;
                
                FieldChange.Add(entity.FinalDeliveryETA, null, nameof(entity.FinalDeliveryETA), fieldChanges);
                entity.FinalDeliveryETA = shipmentPM.FinalDeliveryETA = null;
                
                FieldChange.Add(entity.FinalDeliveryETD, null, nameof(entity.FinalDeliveryETD), fieldChanges);
                entity.FinalDeliveryETD = shipmentPM.FinalDeliveryETD = null;

                FieldChange.Add(entity.DeliveryTruckerId, null, nameof(entity.DeliveryTruckerId), fieldChanges);
                entity.DeliveryTruckerId = null;
                
                FieldChange.Add(entity.DeliveryTruckerNumber, null, nameof(entity.DeliveryTruckerNumber), fieldChanges);
                entity.DeliveryTruckerNumber = null;
                
                FieldChange.Add(entity.DeliveryDriver, null, nameof(entity.DeliveryDriver), fieldChanges);
                entity.DeliveryDriver = null;
                
                FieldChange.Add(entity.DeliveryTrailerNumber, null, nameof(entity.DeliveryTrailerNumber), fieldChanges);
                entity.DeliveryTrailerNumber = null;
                
                FieldChange.Add(entity.DeliveryNotes, null, nameof(entity.DeliveryNotes), fieldChanges);
                entity.DeliveryNotes = null;
                
                FieldChange.Add(entity.DeliveryDate, null, nameof(entity.DeliveryDate), fieldChanges);
                entity.DeliveryDate = null;
                
                FieldChange.Add(entity.PODDate, null, nameof(entity.PODDate), fieldChanges);
                entity.PODDate = null;
            }
            else
            {
                FieldChange.Add(entity.FinalDeliveryATA, finalDelivery.ATA, nameof(entity.FinalDeliveryATA), fieldChanges);
                entity.FinalDeliveryATA = shipmentPM.FinalDeliveryATA = finalDelivery.ATA;
                
                FieldChange.Add(entity.FinalDeliveryATD, finalDelivery.ATD, nameof(entity.FinalDeliveryATD), fieldChanges);
                entity.FinalDeliveryATD = shipmentPM.FinalDeliveryATD = finalDelivery.ATD;
                
                FieldChange.Add(entity.FinalDeliveryETA, finalDelivery.ETA, nameof(entity.FinalDeliveryETA), fieldChanges);
                entity.FinalDeliveryETA = shipmentPM.FinalDeliveryETA = finalDelivery.ETA;
                
                FieldChange.Add(entity.FinalDeliveryETD, finalDelivery.ETD, nameof(entity.FinalDeliveryETD), fieldChanges);
                entity.FinalDeliveryETD = shipmentPM.FinalDeliveryETD = finalDelivery.ETD;

                FieldChange.Add(entity.DeliveryTruckerId, finalDelivery.CarrierId, nameof(entity.DeliveryTruckerId), fieldChanges);
                entity.DeliveryTruckerId = finalDelivery.CarrierId;

                FieldChange.Add(entity.DeliveryTruckerNumber, finalDelivery.CarrierNumber, nameof(entity.DeliveryTruckerNumber), fieldChanges);
                entity.DeliveryTruckerNumber = finalDelivery.CarrierNumber;

                FieldChange.Add(entity.DeliveryDriver, finalDelivery.Driver, nameof(entity.DeliveryDriver), fieldChanges);
                entity.DeliveryDriver = finalDelivery.Driver;

                FieldChange.Add(entity.DeliveryTrailerNumber, finalDelivery.TrailerNumber, nameof(entity.DeliveryTrailerNumber), fieldChanges);
                entity.DeliveryTrailerNumber = finalDelivery.TrailerNumber;

                FieldChange.Add(entity.DeliveryNotes, finalDelivery.Notes, nameof(entity.DeliveryNotes), fieldChanges);
                entity.DeliveryNotes = finalDelivery.Notes;

                FieldChange.Add(entity.PODDate, finalDelivery.ATA, nameof(entity.PODDate), fieldChanges);
                entity.PODDate = finalDelivery.ATA;

                if (finalDelivery.PickUpDeliveryToTypeCode == "PART")
                {
                    var deliveryTo = GetPrtnerAddressCity(finalDelivery.ToAddressId);
                    FieldChange.Add(entity.DeliveryTo, deliveryTo, nameof(entity.DeliveryTo), fieldChanges);
                    entity.DeliveryTo = deliveryTo;
                    //entity.DeliveryToCity = entity.DeliveryTo;
                }
                else if (finalDelivery.PickUpDeliveryToTypeCode == "CASL")
                {
                    FieldChange.Add(entity.DeliveryTo, finalDelivery.ToAddressCity, nameof(entity.DeliveryTo), fieldChanges);
                    entity.DeliveryTo = finalDelivery.ToAddressCity;
                    //entity.DeliveryToCity = finalDelivery.ToAddressCity;
                }
                else
                {
                    var deliveryTo = GetPortName(finalDelivery.ToPortId);
                    FieldChange.Add(entity.DeliveryTo, deliveryTo, nameof(entity.DeliveryTo), fieldChanges);
                    entity.DeliveryTo = deliveryTo;
                    //entity.DeliveryToCity = entity.DeliveryTo;
                    FieldChange.Add(entity.DeliveryToPortId, finalDelivery.ToPortId, nameof(entity.DeliveryToPortId), fieldChanges);
                    entity.DeliveryToPortId = finalDelivery.ToPortId;
                }

                if (finalDelivery.PickUpDeliveryFromTypeCode == "PART")
                {
                    var deliveryFrom = GetPrtnerAddressCity(finalDelivery.FromAddressId);
                    FieldChange.Add(entity.DeliveryFrom, deliveryFrom, nameof(entity.DeliveryFrom), fieldChanges);
                    entity.DeliveryFrom = deliveryFrom;
                }
                else if (finalDelivery.PickUpDeliveryFromTypeCode == "CASL")
                {
                    FieldChange.Add(entity.DeliveryFrom, finalDelivery.FromAddressCity, nameof(entity.DeliveryFrom), fieldChanges);
                    entity.DeliveryFrom = finalDelivery.FromAddressCity;
                }
                else
                {
                    var deliveryFrom = GetPortName(finalDelivery.FromPortId);
                    FieldChange.Add(entity.DeliveryFrom, deliveryFrom, nameof(entity.DeliveryFrom), fieldChanges);
                    entity.DeliveryFrom = deliveryFrom;
                }
            }
        }

        private void MapFirstDelivery(List<FieldChange> fieldChanges)
        {
            ShipmentDeliveryPM firstDelivery = shipmentPM.ShipmentDeliveries.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (firstDelivery == null)
            {
                FieldChange.Add(entity.DeliveryDate, null, nameof(entity.DeliveryDate), fieldChanges);
                entity.DeliveryDate = null;
            }
            else
            {
                FieldChange.Add(entity.DeliveryDate, firstDelivery.ATD, nameof(entity.DeliveryDate), fieldChanges);
                entity.DeliveryDate = firstDelivery.ATD;
            }
        }

        private void MapFieldsWhenChanged(List<FieldChange> fieldChanges)
        {
            if (shipmentPM.IsShipmentComputedFieldChange)
            {
                FieldChange.Add(entity.IsDepositionRequired, shipmentPM.IsDepositionRequired, nameof(entity.IsDepositionRequired), fieldChanges);
                entity.IsDepositionRequired = shipmentPM.IsDepositionRequired;
                
                FieldChange.Add(entity.IsRequestedDocuments, shipmentPM.IsRequestedDocuments, nameof(entity.IsRequestedDocuments), fieldChanges);
                entity.IsRequestedDocuments = shipmentPM.IsRequestedDocuments;
                
                FieldChange.Add(entity.IsDigitalSignRequired, shipmentPM.IsDigitalSignRequired, nameof(entity.IsDigitalSignRequired), fieldChanges);
                entity.IsDigitalSignRequired = shipmentPM.IsDigitalSignRequired;
                
                FieldChange.Add(entity.IsMissingDocuments, shipmentPM.IsMissingDocuments, nameof(entity.IsMissingDocuments), fieldChanges);
                entity.IsMissingDocuments = shipmentPM.IsMissingDocuments;
                
                FieldChange.Add(entity.DocumentsSearchFields, shipmentPM.DocumentsSearchFields, nameof(entity.DocumentsSearchFields), fieldChanges);
                entity.DocumentsSearchFields = shipmentPM.DocumentsSearchFields;
                
                FieldChange.Add(entity.MissingDocumentsCount, shipmentPM.MissingDocumentsCount, nameof(entity.MissingDocumentsCount), fieldChanges);
                entity.MissingDocumentsCount = shipmentPM.MissingDocumentsCount;
                
                FieldChange.Add(entity.MissingDocumentsNames, shipmentPM.MissingDocumentsNames, nameof(entity.MissingDocumentsNames), fieldChanges);
                entity.MissingDocumentsNames = shipmentPM.MissingDocumentsNames;
                
                FieldChange.Add(entity.RequestedDocumentsCount, shipmentPM.RequestedDocumentsCount, nameof(entity.RequestedDocumentsCount), fieldChanges);
                entity.RequestedDocumentsCount = shipmentPM.RequestedDocumentsCount;
                
                FieldChange.Add(entity.NumberOfHouses, shipmentPM.NumberOfHouses, nameof(entity.NumberOfHouses), fieldChanges);
                entity.NumberOfHouses = shipmentPM.NumberOfHouses;
                
                FieldChange.Add(entity.ImporterDepositionRequestDetails, shipmentPM.ImporterDepositionRequestDetails, nameof(entity.ImporterDepositionRequestDetails), fieldChanges);
                entity.ImporterDepositionRequestDetails = shipmentPM.ImporterDepositionRequestDetails;

            }
        }

        private void MapAccountingClosed(List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entity.AccountingClosedByUserId, shipmentPM.AccountingClosedByUserId, nameof(entity.AccountingClosedByUserId), fieldChanges);
            entity.AccountingClosedByUserId = shipmentPM.AccountingClosedByUserId;
        }

        private void MapOperationalClosed(List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entity.OperationallyClosedByUserId, shipmentPM.OperationalClosedByUserId, nameof(entity.OperationallyClosedByUserId), fieldChanges);
            entity.OperationallyClosedByUserId = shipmentPM.OperationalClosedByUserId;

            if (shipmentPM.IsOperationalClosed)
            {
                FieldChange.Add(entity.IsMissingDocuments, false, nameof(entity.IsMissingDocuments), fieldChanges);
                entity.IsMissingDocuments = false;
                
                FieldChange.Add(entity.IsRequestedDocuments, false, nameof(entity.IsRequestedDocuments), fieldChanges);
                entity.IsRequestedDocuments = false;
                
                FieldChange.Add(entity.IsDigitalSignRequired, false, nameof(entity.IsDigitalSignRequired), fieldChanges);
                entity.IsDigitalSignRequired = false;
                
                FieldChange.Add(entity.MissingDocumentsCount, 0, nameof(entity.MissingDocumentsCount), fieldChanges);
                entity.MissingDocumentsCount = 0;
                
                FieldChange.Add(entity.MissingDocumentsNames, "", nameof(entity.MissingDocumentsNames), fieldChanges);
                entity.MissingDocumentsNames = "";
            }
            else
            {
                var OTId = objectTableRepository.GetObjectTableIdByName("Shipment");
                var missingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(shipmentPM.Id, OTId, tenant, shipmentPM.IsOperationalClosed);
                FieldChange.Add(entity.MissingDocumentsCount, missingDocumentsCount, nameof(entity.MissingDocumentsCount), fieldChanges);
                entity.MissingDocumentsCount = missingDocumentsCount;

                var missingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(shipmentPM.Id, OTId, tenant, shipmentPM.IsOperationalClosed);
                FieldChange.Add(entity.MissingDocumentsNames, missingDocumentsNames, nameof(entity.MissingDocumentsNames), fieldChanges);
                entity.MissingDocumentsNames = missingDocumentsNames;

                if (entity.MissingDocumentsCount == 0)
                {
                    FieldChange.Add(entity.IsMissingDocuments, false, nameof(entity.IsMissingDocuments), fieldChanges);
                    entity.IsMissingDocuments = false;
                }
                else
                {
                    FieldChange.Add(entity.IsMissingDocuments, true, nameof(entity.IsMissingDocuments), fieldChanges);
                    entity.IsMissingDocuments = true;
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.OperationalClosedByUserId))
            {
                Contact contact = contactRepository.GetSingleContact(shipmentPM.OperationalClosedByUserId, tenant);
                var operationallyClosedByUserName = contact != null ? contact.Name : "";
                FieldChange.Add(entity.OperationallyClosedByUserName, operationallyClosedByUserName, nameof(entity.OperationallyClosedByUserName), fieldChanges);
                entity.OperationallyClosedByUserName = operationallyClosedByUserName;
            }
        }

        private void MapCustomsClearance(List<FieldChange> fieldChanges)
        {
            if (shipmentPM.CustomsClearanceDate != null)
            {
                FieldChange.Add(entity.IsMissingDocuments, false, nameof(entity.IsMissingDocuments), fieldChanges);
                entity.IsMissingDocuments = false;
                
                FieldChange.Add(entity.IsRequestedDocuments, false, nameof(entity.IsRequestedDocuments), fieldChanges);
                entity.IsRequestedDocuments = false;
                
                FieldChange.Add(entity.IsDigitalSignRequired, false, nameof(entity.IsDigitalSignRequired), fieldChanges);
                entity.IsDigitalSignRequired = false;
                
                FieldChange.Add(entity.IsDepositionRequired, false, nameof(entity.IsDepositionRequired), fieldChanges);
                entity.IsDepositionRequired = false;
            }
        }

        private void MapMasterHouses(List<FieldChange> fieldChanges)
        {
            if (this.isNewEntity)
            {
                if (shipmentPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipmentPM.MasterShipmentDataId))
                {
                    ShipmentComputedFields entityMasterComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentPM.MasterShipmentDataId, tenant);
                    if (entityMasterComputedFields != null)
                    {
                        var numberOfHouses = entityMasterComputedFields.NumberOfHouses + 1;
                        FieldChange.Add(entity.NumberOfHouses, numberOfHouses, nameof(entity.NumberOfHouses), fieldChanges);
                        entityMasterComputedFields.NumberOfHouses = numberOfHouses;
                        shipmentComputedFieldsRepository.Update(entityMasterComputedFields);
                        shipmentComputedFieldsRepository.SubmitChanges();
                    }
                }
            }
            else if (shipmentPM.ShipmentLevelCode == "C")
            {
                var numberOfHouses = shipmentPM.ShipmentConsoleShipments.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();
                FieldChange.Add(entity.NumberOfHouses, numberOfHouses, nameof(entity.NumberOfHouses), fieldChanges);
                entity.NumberOfHouses = numberOfHouses;
            }
        }
        private string GetPortName(string portId)
        {
            string portName = "";
            if (!string.IsNullOrEmpty(portId))
            {
                Port port = portRepository.GetSinglePort(portId, tenant);
                portName = port != null ? port.EnglishName : "";
            }
            return portName;
        }
        private string GetPrtnerAddressCity(string addressId)
        {
            string partnerAddressCity = "";
            if (!string.IsNullOrEmpty(addressId))
            {
                Address partnerAddress = addressRepository.GetSingleAddress(addressId, tenant);
                partnerAddressCity = partnerAddress != null ? partnerAddress.City : "";
            }
            return partnerAddressCity;
        }
        private void MapTransshipments(List<FieldChange> fieldChanges)
        {
            string myResult = null;

            string TS1PortCode = null;
            string TS2PortCode = null;
            string TS3PortCode = null;

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
            {
                PortPM port = PortQuery.GetSinglePort(tenant, shipmentPM.Transshipment1FromPortId, true);
                TS1PortCode = port.Code;                
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
            {
                PortPM port = PortQuery.GetSinglePort(tenant, shipmentPM.Transshipment2FromPortId, true);
                TS2PortCode = port.Code;
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortId))
            {
                PortPM port = PortQuery.GetSinglePort(tenant, shipmentPM.Transshipment3FromPortId, true);
                TS3PortCode = port.Code;
            }

            if (TS1PortCode != null)
            {
                myResult = TS1PortCode;
            }

            if (TS2PortCode != null)
            {
                myResult = myResult + " , " +TS2PortCode;
            }

            if (TS3PortCode != null)
            {
                myResult = myResult + " , " + TS3PortCode;
            }

            shipmentPM.Transshipments = myResult;
            FieldChange.Add(entity.Transshipments, myResult, nameof(entity.Transshipments), fieldChanges);
            entity.Transshipments = myResult;
        }
    }
}
