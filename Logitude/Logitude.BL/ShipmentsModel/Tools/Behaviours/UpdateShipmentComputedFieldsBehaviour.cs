using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
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

        public static ShipmentComputedFields ShipmentComputedFields;
        private ShipmentPM shipmentPM;
        private IShipmentsContext context;
        private ShipmentComputedFieldsRepository shipmentComputedFieldsRepository;
        private ObjectTableRepository objectTableRepository;
        private DocumentsFilingQuery documentsFilingQuery;
        private ContactRepository contactRepository;
        private PortRepository portRepository;
        private AddressRepository addressRepository;
        private int tenant;
        private bool isNewEntity;

        public UpdateShipmentComputedFieldsBehaviour(ShipmentPM shipmentPM, IShipmentsContext context, bool isNewEntity)
        {
            this.shipmentPM = shipmentPM;
            this.context = context;
            this.isNewEntity = isNewEntity;
            this.tenant = shipmentPM.Tenant;
            IntializeNeededRepositories();
            GetShipmentComputedFields();
        }
        public void Handle()
        {
            UpdateShipmentComputedFields();
        }

        private void IntializeNeededRepositories()
        {
            shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(context);
            objectTableRepository = new ObjectTableRepository(tenant);
            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            contactRepository = new ContactRepository(tenant);
            portRepository = new PortRepository(tenant);
            addressRepository = new AddressRepository(tenant);
        }
        
        private void GetShipmentComputedFields()
        {
            if(isNewEntity)
            {
                CreateShipmentComputedFields();
            }
            else
            {
                GetExistShipmentComputedFields();
                UpdateExistShipmentComputedFieldsValues();
            }
        }

        private void GetExistShipmentComputedFields()
        {
            if (ShipmentComputedFields == null) {
                ShipmentComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentPM.Id, tenant);
            } 
        }

        private void UpdateExistShipmentComputedFieldsValues()
        {
            if (ShipmentComputedFields != null)
            {
                UpdateShipmentComputedFieldsWhenChanged();
                if (shipmentPM.IsOperationalClosed)
                {
                    UpdateShipmentComputedFieldsWhenShipmentClosed();
                }
                else
                {
                    UpdateShipmentComputedFieldsWhenShipmentNotClosed();
                }
                UpdateShipmentComputedFieldsWhenCustomsClearanceDateExist();
            }
        }

        private void UpdateShipmentComputedFieldsWhenCustomsClearanceDateExist()
        {
            if (shipmentPM.CustomsClearanceDate != null)
            {
                ShipmentComputedFields.IsMissingDocuments = false;
                ShipmentComputedFields.IsRequestedDocuments = false;
                ShipmentComputedFields.IsDigitalSignRequired = false;
                ShipmentComputedFields.IsDepositionRequired = false;
            }
        }

        private void UpdateShipmentComputedFieldsWhenShipmentNotClosed()
        { 
            var OTId = objectTableRepository.GetObjectTableIdByName("Shipment");
            ShipmentComputedFields.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(shipmentPM.Id, OTId, tenant, shipmentPM.IsOperationalClosed);
            ShipmentComputedFields.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(shipmentPM.Id, OTId, tenant, shipmentPM.IsOperationalClosed);
            if (ShipmentComputedFields.MissingDocumentsCount == 0)
            {
                ShipmentComputedFields.IsMissingDocuments = false;
            }
            else
            {
                ShipmentComputedFields.IsMissingDocuments = true;
            }
        }

        private void UpdateShipmentComputedFieldsWhenShipmentClosed()
        {
            ShipmentComputedFields.IsMissingDocuments = false;
            ShipmentComputedFields.IsRequestedDocuments = false;
            ShipmentComputedFields.IsDigitalSignRequired = false;
            ShipmentComputedFields.MissingDocumentsCount = 0;
            ShipmentComputedFields.MissingDocumentsNames = "";
        }

        private void UpdateShipmentComputedFieldsWhenChanged()
        {
            if (shipmentPM.IsShipmentComputedFieldChange)
            {
                ShipmentComputedFields.IsDepositionRequired = shipmentPM.IsDepositionRequired;
                ShipmentComputedFields.IsRequestedDocuments = shipmentPM.IsRequestedDocuments;
                ShipmentComputedFields.IsDigitalSignRequired = shipmentPM.IsDigitalSignRequired;
                ShipmentComputedFields.IsMissingDocuments = shipmentPM.IsMissingDocuments;
                ShipmentComputedFields.DocumentsSearchFields = shipmentPM.DocumentsSearchFields;
                ShipmentComputedFields.MissingDocumentsCount = shipmentPM.MissingDocumentsCount;
                ShipmentComputedFields.MissingDocumentsNames = shipmentPM.MissingDocumentsNames;
                ShipmentComputedFields.RequestedDocumentsCount = shipmentPM.RequestedDocumentsCount;
                ShipmentComputedFields.NumberOfHouses = shipmentPM.NumberOfHouses;
                ShipmentComputedFields.ImporterDepositionRequestDetails = shipmentPM.ImporterDepositionRequestDetails;
            }
        }
       
        private void CreateShipmentComputedFields()
        {
            ShipmentComputedFields = new ShipmentComputedFields();
            MapShipmentComputedFieldsBasicValues();
            shipmentComputedFieldsRepository.Add(ShipmentComputedFields);
        }

        private void MapShipmentComputedFieldsBasicValues()
        {
            ShipmentComputedFields.Id = shipmentPM.Id;
            ShipmentComputedFields.Tenant = shipmentPM.Tenant;

            if (shipmentPM.IsOperationalClosed)
            {
                ShipmentComputedFields.IsMissingDocuments = false;
                ShipmentComputedFields.IsRequestedDocuments = false;
            }
            else
            {
                ShipmentComputedFields.IsMissingDocuments = true;
            }

            if (shipmentPM.IsShipmentComputedFieldChange)
            {
                ShipmentComputedFields.IsDepositionRequired = shipmentPM.IsDepositionRequired;
            }

            if (shipmentPM.CustomsClearanceDate != null)
            {
                ShipmentComputedFields.IsDigitalSignRequired = false;
                ShipmentComputedFields.IsDepositionRequired = false;
            }
            ShipmentComputedFields.LastDocumentDateTime = null;// new DateTime(1900, 1, 1);
        }

        private void UpdateShipmentComputedFields()
        {
            if (ShipmentComputedFields != null)
            {
                MapComputedFields();
                shipmentComputedFieldsRepository.Update(ShipmentComputedFields);
            }

            shipmentPM.IsShipmentComputedFieldChange = false;
        }

        private void MapComputedFields()
        {
            ShipmentComputedFields.FirstPickupLocation = shipmentPM.FirstPickupLocation;
            ShipmentComputedFields.Commodity = shipmentPM.AWBCommodityItemNumber;
            ShipmentComputedFields.OperationallyClosedByUserId = shipmentPM.OperationalClosedByUserId;
            ShipmentComputedFields.NumberOfDeliveries = shipmentPM.ShipmentDeliveries != null ? shipmentPM.ShipmentDeliveries.Count() : 0;
            ShipmentComputedFields.ContainsDangerousGoods = shipmentPM.IsDangerous;
            ShipmentComputedFields.ImportDeclarationDate = shipmentPM.DeclarationDate;
            ShipmentComputedFields.ImportDeclarationNumber = shipmentPM.DeclarationNumber;
            GetContainersNumbers();
            GetOperationalClosedByUserName();
            GetShipmentLastPickUpFields();
            GetShipmentDeliveriesFields();
            GetShipmentPickUpFields();
        }
        private void GetContainersNumbers()
        {
            string myContainersNumbers = null;

            if (shipmentPM.ShipmentPackages != null)
            {
                foreach (ShipmentPackagePM packagePM in shipmentPM.ShipmentPackages.Where(p => p.ChangeSetOp != ChangeSetOperation.Delete))
                {
                    if (string.IsNullOrEmpty(myContainersNumbers))
                    {
                        myContainersNumbers = packagePM.ContainerNumber;
                    }
                    else
                    {
                        myContainersNumbers += ", " + packagePM.ContainerNumber;
                    }
                }

                if (!string.IsNullOrEmpty(myContainersNumbers) && myContainersNumbers.Length > 1000)
                {
                    myContainersNumbers = myContainersNumbers.Substring(0, 1000);
                }
            }
            ShipmentComputedFields.ContainersNumbers = myContainersNumbers;
        }

        private void GetShipmentLastPickUpFields()
        {
            ShipmentPickUpPM lastPickup = shipmentPM.ShipmentPickUps.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).
                OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (lastPickup != null)
            {
                ShipmentComputedFields.LastPickupATA = lastPickup.ATA;
                ShipmentComputedFields.LastPickupATD = lastPickup.ATD;
                ShipmentComputedFields.LastPickupETA = lastPickup.ETA;
                ShipmentComputedFields.LastPickupETD = lastPickup.ETD;
            }
        }

        private void GetShipmentPickUpFields()
        {
            if (shipmentPM.ShipmentPickUps.Count() > 0)
            {
                ShipmentPickUpPM firstPickUp = shipmentPM.ShipmentPickUps.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete)
                    .OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (firstPickUp.PickUpDeliveryToTypeCode == "PART")
                {
                    ShipmentComputedFields.PickupTo = GetPrtnerAddressCity(firstPickUp.ToAddressId);
                }
                else if (firstPickUp.PickUpDeliveryToTypeCode == "CASL")
                {
                    ShipmentComputedFields.PickupTo = firstPickUp.ToAddressCity;
                }
                else
                {
                    ShipmentComputedFields.PickupTo = GetPortName(firstPickUp.ToPortId);
                }

                if (firstPickUp.PickUpDeliveryFromTypeCode == "PART")
                {
                    ShipmentComputedFields.PickupFrom = GetPrtnerAddressCity(firstPickUp.FromAddressId);
                }
                else if (firstPickUp.PickUpDeliveryFromTypeCode == "CASL")
                {
                    ShipmentComputedFields.PickupFrom = firstPickUp.FromAddressCity;
                }
                else
                {
                    ShipmentComputedFields.PickupFrom = GetPortName(firstPickUp.FromPortId);
                }
            }
        }

        private void GetShipmentDeliveriesFields()
        {
            if (shipmentPM.ShipmentDeliveries.Count > 0)
            {
                ShipmentDeliveryPM finalDelivery = shipmentPM.ShipmentDeliveries.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete)
                    .OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (finalDelivery.PickUpDeliveryToTypeCode == "PART")
                {
                    ShipmentComputedFields.DeliveryTo = GetPrtnerAddressCity(finalDelivery.ToAddressId);
                    ShipmentComputedFields.DeliveryToCity = ShipmentComputedFields.DeliveryTo;
                }
                else if (finalDelivery.PickUpDeliveryToTypeCode == "CASL")
                {
                    ShipmentComputedFields.DeliveryTo = finalDelivery.ToAddressCity;
                    ShipmentComputedFields.DeliveryToCity = finalDelivery.ToAddressCity;
                }
                else
                {
                    ShipmentComputedFields.DeliveryTo = GetPortName(finalDelivery.ToPortId);
                    ShipmentComputedFields.DeliveryToCity = ShipmentComputedFields.DeliveryTo;
                    ShipmentComputedFields.DeliveryToPortId = finalDelivery.ToPortId;
                }

                if (finalDelivery.PickUpDeliveryFromTypeCode == "PART")
                {
                    ShipmentComputedFields.DeliveryFrom = GetPrtnerAddressCity(finalDelivery.FromAddressId);
                }
                else if (finalDelivery.PickUpDeliveryFromTypeCode == "CASL")
                {
                    ShipmentComputedFields.DeliveryFrom = finalDelivery.FromAddressCity;
                }
                else
                {
                    ShipmentComputedFields.DeliveryFrom = GetPortName(finalDelivery.FromPortId);
                }
            }
        }

        private string GetPortName(string portId)
        {
            Port port = null;
            if (!string.IsNullOrEmpty(portId))
            {
                port = portRepository.GetSinglePort(portId, tenant);
            }
            return port.EnglishName;
        }

        private string GetPrtnerAddressCity(string addressId)
        {
            Address partnerAddress = null;
            if (!string.IsNullOrEmpty(addressId))
            {
                partnerAddress = addressRepository.GetSingleAddress(addressId, tenant);
            }
            return partnerAddress.City;
        }
        private void GetOperationalClosedByUserName()
        {
            if (!string.IsNullOrEmpty(shipmentPM.OperationalClosedByUserId))
            {
                Contact contact = contactRepository.GetSingleContact(shipmentPM.OperationalClosedByUserId, tenant);
                ShipmentComputedFields.OperationallyClosedByUserName = contact != null ? contact.Name : "";
            }

        }
    }
}
