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
            SaveEntity();

            MapShipmentFields();
            MapMasterHouses();
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
            MapContainersNumbers();
            MapFirstPickUp();
            MapLastPickUp();
            MapLastDelivery();
            MapFieldsWhenChanged();
            MapOperationalClosed();
            MapCustomsClearance();
            MapMasterHouses();

            shipmentPM.IsShipmentComputedFieldChange = false;
        }
        private void SaveEntity()
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
            entity.NumberOfDeliveries = shipmentPM.ShipmentDeliveries != null ? shipmentPM.ShipmentDeliveries.Count() : 0;
            entity.ContainsDangerousGoods = shipmentPM.IsDangerous;
            entity.ImportDeclarationDate = shipmentPM.DeclarationDate;
            entity.ImportDeclarationNumber = shipmentPM.DeclarationNumber;

            if (shipmentPM.IsShipmentComputedFieldChange)
            {
                entity.IsDepositionRequired = shipmentPM.IsDepositionRequired;
            }

            if (shipmentPM.CustomsClearanceDate != null)
            {
                entity.IsDigitalSignRequired = false;
                entity.IsDepositionRequired = false;
            }

            entity.LastDocumentDateTime = null;// new DateTime(1900, 1, 1);
        }
        private void MapContainersNumbers()
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

            entity.ContainersNumbers = myContainersNumbers;
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
            }

            else
            {
                entity.FirstPickupATA = firstPickUp.ATA;
                entity.FirstPickupATD = firstPickUp.ATD;

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
                entity.DeliveryToCity = null;
                entity.DeliveryToPortId = null;
                entity.DeliveryFrom = null;
                entity.FinalDeliveryATA = null;
                entity.FinalDeliveryATD = null;
                entity.FinalDeliveryETA = null;
                entity.FinalDeliveryETD = null;
            }

            else
            {
                entity.FinalDeliveryATA = finalDelivery.ATA;
                entity.FinalDeliveryATD = finalDelivery.ATD;
                entity.FinalDeliveryETA = finalDelivery.ETA;
                entity.FinalDeliveryETD = finalDelivery.ETD;

                if (finalDelivery.PickUpDeliveryToTypeCode == "PART")
                {
                    entity.DeliveryTo = GetPrtnerAddressCity(finalDelivery.ToAddressId);
                    entity.DeliveryToCity = entity.DeliveryTo;
                }

                else if (finalDelivery.PickUpDeliveryToTypeCode == "CASL")
                {
                    entity.DeliveryTo = finalDelivery.ToAddressCity;
                    entity.DeliveryToCity = finalDelivery.ToAddressCity;
                }

                else
                {
                    entity.DeliveryTo = GetPortName(finalDelivery.ToPortId);
                    entity.DeliveryToCity = entity.DeliveryTo;
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
                entity.NumberOfHouses = (from d in context.Shipments where d.ShipmentLevelCode == "H" && d.MasterShipmentDataId == shipmentPM.Id select d).Count();
            }
        }
        private void MapShipmentFields()
        {
            shipmentPM.IsDepositionRequired = entity.IsDepositionRequired;
            shipmentPM.IsRequestedDocuments = entity.IsRequestedDocuments;
            shipmentPM.IsDigitalSignRequired = entity.IsDigitalSignRequired;
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
