using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
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

        public void Handle()
        {
            GetEntity();
            MapEntity();
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
        private void MapEntity()
        {
            MapFields();
            MapContainersNumbersAndTypesArray();
            MapFirstPickUp();
            MapLastPickUp();
            MapLastDelivery();
            MapFirstDelivery();
            MapFieldsWhenChanged();
            MapOperationalClosed();
            MapCustomsClearance();
            MapMasterHouses();
            MapDocumentFields();
            MapAccountingClosed();
            MapMainCarriageDates();

            shipmentPM.IsShipmentComputedFieldChange = false;
            shipmentPM.IsDepositionRequired = entity.IsDepositionRequired;
            shipmentPM.IsRequestedDocuments = entity.IsRequestedDocuments;
            shipmentPM.IsDigitalSignRequired = entity.IsDigitalSignRequired;
            shipmentPM.CreatedFromDigital = entity.CreatedFromDigital;
        }

        private void MapMainCarriageDates()
        {
            entity.MainCarriageETA = shipmentPM.MainCarriageETA;
            entity.MainCarriageETD = shipmentPM.MainCarriageETD;
            entity.MainCarriageATA = shipmentPM.MainCarriageATA;
            entity.MainCarriageATD = shipmentPM.MainCarriageATD;
        }

        private void MapDocumentFields()
        {
            entity.BookingConfirmationSent = shipmentPM.BookingConfirmationSentDate;
            entity.PreAlertSent = shipmentPM.PreAlertSentDate;
            entity.DeliveryNoticeSent = shipmentPM.DeliveryNoticeSentDate;
            entity.ExpectedArrivalNoticeSent = shipmentPM.ExpectedArrivalNoticeSentDate;
            entity.ArrivalNoticeSent = shipmentPM.ArrivalNoticeSentDate;
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

        private void MapFields()
        {
            entity.FirstPickupLocation = shipmentPM.FirstPickupLocation;
            entity.Commodity = shipmentPM.AWBCommodityItemNumber;
            //entity.ContainsDangerousGoods = shipmentPM.IsDangerous;
            //entity.ImportDeclarationDate = shipmentPM.DeclarationDate;
            //entity.ImportDeclarationNumber = shipmentPM.DeclarationNumber;
            entity.NumberOfDeliveries = shipmentPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();
            entity.LastDocumentDateTime = null;// new DateTime(1900, 1, 1);
            entity.CreatedFromDigital = shipmentPM.CreatedFromDigital;
        }
        private void MapContainersNumbersAndTypesArray()
        {
            if (shipmentPM.ShipmentPackages != null)
            {
                MapContainersNumbersAndTypesArrayFromShipmentPackages();
            }
        }
        private void MapContainersNumbersAndTypesArrayFromShipmentPackages()
        {
            string shipmentContainersNumbers = string.Empty;
            string shipmentContainersNumbersAndTypesArray = string.Empty;
            foreach (ShipmentPackagePM packagePM in shipmentPM.ShipmentPackages.Where(p => p.ChangeSetOp != ChangeSetOperation.Delete))
            {
                shipmentContainersNumbers = AddPackageContainerNumberToShipmentContainersNumbers(shipmentContainersNumbers, packagePM.ContainerNumber);
                shipmentContainersNumbersAndTypesArray = AddPackageContainerNumberAndTypeToShipmentContainersNumbersAndTypesArray(shipmentContainersNumbersAndTypesArray, packagePM.ContainerNumber, packagePM.PackageTypeCode);
            }

            if (!string.IsNullOrEmpty(shipmentContainersNumbers) && shipmentContainersNumbers.Length > 1000)
            {
                shipmentContainersNumbers = shipmentContainersNumbers.Substring(0, 1000);
            }
            entity.ContainersNumbers = string.IsNullOrEmpty(shipmentContainersNumbers) ? null : shipmentContainersNumbers;
            entity.ContainersNumbersAndTypesArray = string.IsNullOrEmpty(shipmentContainersNumbersAndTypesArray) ? null : shipmentContainersNumbersAndTypesArray;
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
        private void MapFirstPickUp()
        {
            ShipmentPickUpPM firstPickUp = shipmentPM.ShipmentPickUps.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault(); 

            if (firstPickUp == null)
            {
                entity.PickupTo = null;
                entity.PickupFrom = null;
                entity.FirstPickupATA = null;
                entity.FirstPickupATD = null;

                entity.PickupTruckerId = null;
                entity.PickupTruckerNumber  = null;
                entity.PickupDriver = null;
                entity.PickupTrailerNumber = null;
                entity.PickupNotes = null;
                entity.OnHandDate = null;


            }

            else
            {
                entity.FirstPickupATA = firstPickUp.ATA;
                entity.FirstPickupATD = firstPickUp.ATD;

                entity.PickupTruckerId = firstPickUp.CarrierId;
                entity.PickupTruckerNumber = firstPickUp.CarrierNumber;
                entity.PickupDriver = firstPickUp.Driver;
                entity.PickupTrailerNumber = firstPickUp.TrailerNumber;
                entity.PickupNotes = firstPickUp.Notes;
                entity.OnHandDate = firstPickUp.ATA;

                if (firstPickUp.PickUpDeliveryToTypeCode == "PART")
                {
                    entity.PickupTo = GetPrtnerAddressCity(firstPickUp.ToAddressId);
                }
                else if (firstPickUp.PickUpDeliveryToTypeCode == "CASL")
                {
                    entity.PickupTo = firstPickUp.ToAddressCity;
                }
                else
                {
                    entity.PickupTo = GetPortName(firstPickUp.ToPortId);
                }

                if (firstPickUp.PickUpDeliveryFromTypeCode == "PART")
                {
                    entity.PickupFrom = GetPrtnerAddressCity(firstPickUp.FromAddressId);
                }
                else if (firstPickUp.PickUpDeliveryFromTypeCode == "CASL")
                {
                    entity.PickupFrom = firstPickUp.FromAddressCity;
                }
                else
                {
                    entity.PickupFrom = GetPortName(firstPickUp.FromPortId);
                }
            }
        }
        private void MapLastPickUp()
        {
            ShipmentPickUpPM lastPickup = shipmentPM.ShipmentPickUps.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (lastPickup == null)
            {
                entity.LastPickupATA = null;
                entity.LastPickupATD = null;
                entity.LastPickupETA = null;
                entity.LastPickupETD = null;
            }

            else
            {
                entity.LastPickupATA = lastPickup.ATA;
                entity.LastPickupATD = lastPickup.ATD;
                entity.LastPickupETA = lastPickup.ETA;
                entity.LastPickupETD = lastPickup.ETD;
            }
        }
        private void MapLastDelivery()
        {

            ShipmentDeliveryPM finalDelivery = shipmentPM.ShipmentDeliveries.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (finalDelivery == null)
            {
                entity.DeliveryTo = null;
                //entity.DeliveryToCity = null;
                entity.DeliveryToPortId = null;
                entity.DeliveryFrom = null;
                entity.FinalDeliveryATA = shipmentPM.FinalDeliveryATA = null;
                entity.FinalDeliveryATD = shipmentPM.FinalDeliveryATD = null;
                entity.FinalDeliveryETA = shipmentPM.FinalDeliveryETA = null;
                entity.FinalDeliveryETD = shipmentPM.FinalDeliveryETD = null;


                entity.DeliveryTruckerId = null;
                entity.DeliveryTruckerNumber = null;
                entity.DeliveryDriver = null;
                entity.DeliveryTrailerNumber = null;
                entity.DeliveryNotes = null;
                entity.DeliveryDate = null;
                entity.PODDate = null;
            }

            else
            {
                entity.FinalDeliveryATA = shipmentPM.FinalDeliveryATA= finalDelivery.ATA;
                entity.FinalDeliveryATD = shipmentPM.FinalDeliveryATD= finalDelivery.ATD;
                entity.FinalDeliveryETA = shipmentPM.FinalDeliveryETA =  finalDelivery.ETA;
                entity.FinalDeliveryETD = shipmentPM.FinalDeliveryETD = finalDelivery.ETD;


      

                entity.DeliveryTruckerId = finalDelivery.CarrierId;
                entity.DeliveryTruckerNumber = finalDelivery.CarrierNumber;
                entity.DeliveryDriver = finalDelivery.Driver;
                entity.DeliveryTrailerNumber = finalDelivery.TrailerNumber;
                entity.DeliveryNotes = finalDelivery.Notes;
                entity.PODDate = finalDelivery.ATA;







                if (finalDelivery.PickUpDeliveryToTypeCode == "PART")
                {
                    entity.DeliveryTo = GetPrtnerAddressCity(finalDelivery.ToAddressId);
                    //entity.DeliveryToCity = entity.DeliveryTo;
                }

                else if (finalDelivery.PickUpDeliveryToTypeCode == "CASL")
                {
                    entity.DeliveryTo = finalDelivery.ToAddressCity;
                    //entity.DeliveryToCity = finalDelivery.ToAddressCity;
                }

                else
                {
                    entity.DeliveryTo = GetPortName(finalDelivery.ToPortId);
                    //entity.DeliveryToCity = entity.DeliveryTo;
                    entity.DeliveryToPortId = finalDelivery.ToPortId;
                }

                if (finalDelivery.PickUpDeliveryFromTypeCode == "PART")
                {
                    entity.DeliveryFrom = GetPrtnerAddressCity(finalDelivery.FromAddressId);
                }

                else if (finalDelivery.PickUpDeliveryFromTypeCode == "CASL")
                {
                    entity.DeliveryFrom = finalDelivery.FromAddressCity;
                }

                else
                {
                    entity.DeliveryFrom = GetPortName(finalDelivery.FromPortId);
                }
            }
        }
        private void MapFirstDelivery()
        {
            ShipmentDeliveryPM firstDelivery = shipmentPM.ShipmentDeliveries.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (firstDelivery == null)
            {
                entity.DeliveryDate = null;
            }

            else
            {
                entity.DeliveryDate = firstDelivery.ATD;
            }
        }
        private void MapFieldsWhenChanged()
        {
            if (shipmentPM.IsShipmentComputedFieldChange)
            {
                entity.IsDepositionRequired = shipmentPM.IsDepositionRequired;
                entity.IsRequestedDocuments = shipmentPM.IsRequestedDocuments;
                entity.IsDigitalSignRequired = shipmentPM.IsDigitalSignRequired;
                entity.IsMissingDocuments = shipmentPM.IsMissingDocuments;
                entity.DocumentsSearchFields = shipmentPM.DocumentsSearchFields;
                entity.MissingDocumentsCount = shipmentPM.MissingDocumentsCount;
                entity.MissingDocumentsNames = shipmentPM.MissingDocumentsNames;
                entity.RequestedDocumentsCount = shipmentPM.RequestedDocumentsCount;
                entity.NumberOfHouses = shipmentPM.NumberOfHouses;
                entity.ImporterDepositionRequestDetails = shipmentPM.ImporterDepositionRequestDetails;
            }
        } 
          private void MapAccountingClosed()
        {
            entity.AccountingClosedByUserId = shipmentPM.AccountingClosedByUserId; 
        } 
        private void MapOperationalClosed()
        {
            entity.OperationallyClosedByUserId = shipmentPM.OperationalClosedByUserId;

            if (shipmentPM.IsOperationalClosed)
            {
                entity.IsMissingDocuments = false;
                entity.IsRequestedDocuments = false;
                entity.IsDigitalSignRequired = false;
                entity.MissingDocumentsCount = 0;
                entity.MissingDocumentsNames = "";
            }

            else
            {
                var OTId = objectTableRepository.GetObjectTableIdByName("Shipment");
                entity.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(shipmentPM.Id, OTId, tenant, shipmentPM.IsOperationalClosed);
                entity.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(shipmentPM.Id, OTId, tenant, shipmentPM.IsOperationalClosed);

                if (entity.MissingDocumentsCount == 0)
                {
                    entity.IsMissingDocuments = false;
                }

                else
                {
                    entity.IsMissingDocuments = true;
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.OperationalClosedByUserId))
            {
                Contact contact = contactRepository.GetSingleContact(shipmentPM.OperationalClosedByUserId, tenant);
                entity.OperationallyClosedByUserName = contact != null ? contact.Name : "";
            }
        }
        private void MapCustomsClearance()
        {
            if (shipmentPM.CustomsClearanceDate != null)
            {
                entity.IsMissingDocuments = false;
                entity.IsRequestedDocuments = false;
                entity.IsDigitalSignRequired = false;
                entity.IsDepositionRequired = false;
            }
        }
        private void MapMasterHouses()
        {
            if (this.isNewEntity)
            {
                if (shipmentPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipmentPM.MasterShipmentDataId))
                {
                    ShipmentComputedFields entityMasterComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentPM.MasterShipmentDataId, tenant);
                    if (entityMasterComputedFields != null)
                    {
                        entityMasterComputedFields.NumberOfHouses += 1;
                        shipmentComputedFieldsRepository.Update(entityMasterComputedFields);
                        shipmentComputedFieldsRepository.SubmitChanges();
                    }
                }
            }

            else if (shipmentPM.ShipmentLevelCode == "C")
            {
                entity.NumberOfHouses = shipmentPM.ShipmentConsoleShipments.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();
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
    }
}
