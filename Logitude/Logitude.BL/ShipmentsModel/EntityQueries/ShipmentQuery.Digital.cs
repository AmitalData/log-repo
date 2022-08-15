using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Collections.Generic;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using System;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public partial class ShipmentQuery
    {
        AddressRepository addressRepository;
        ContactRepository contactRepository;
        CountryRepository countryRepository;
        ShipmentRepository shipmentRepository;
        SharedLogisticsSetting sharedLogisticsSetting;
        int tenant;

        public List<ShipmentPartnerPM> GetDigitalShipmentPartners(string shipmentId, int tenant)
        {
            InitializeServices(shipmentId, tenant);
            var partners = new List<ShipmentPartnerPM>();
            var shipment = shipmentRepository.GetSingleShipment(shipmentId, this.tenant);
            if (shipment == null)
            {
                return null;
            }

            GetSharedLogisticsSetting();
            var shipper = GetShipperPartner(shipment);
            if (shipper != null)
            {
                partners.Add(shipper);
            }

            var consigee = GetConsigeePartner(shipment);
            if (consigee != null)
            {
                partners.Add(consigee);
            }

            var agent = GetAgentPartner(shipment);
            if (agent != null)
            {
                partners.Add(agent);
            }

            var shipperNotExporter = GetShipperNotExporterPartner(shipment);
            if (shipperNotExporter != null)
            {
                partners.Add(shipperNotExporter);
            }

            var consigneeNotImporter = GetConsigneeNotImporterPartner(shipment);
            if (consigneeNotImporter != null)
            {
                partners.Add(consigneeNotImporter);
            }

            var notify1 = GetNotify1Partner(shipment);
            if (notify1 != null)
            {
                partners.Add(notify1);
            }

            var notify2 = GetNotify2Partner(shipment);
            if (notify2 != null)
            {
                partners.Add(notify2);
            }

            var freightForwarder = GetFreightForwarderPartner(shipment);
            if (freightForwarder != null)
            {
                partners.Add(freightForwarder);
            }

            var coloader = GetColoaderPartner(shipment);
            if (coloader != null)
            {
                partners.Add(coloader);
            }

            var customsAgent = GetCustomsAgentExportPartner(shipment);
            if (customsAgent != null)
            {
                partners.Add(customsAgent);
            }

            var customsAgentImport = GetCustomsAgentImportPartner(shipment);
            if (customsAgentImport != null)
            {
                partners.Add(customsAgentImport);
            }

            var customsClearanceImport = GetCustomClearancePartner(shipment);
            if (customsClearanceImport != null)
            {
                partners.Add(customsClearanceImport);
            }

            var releasingAgent = GetReleasingAgentPartner(shipment);
            if (releasingAgent != null)
            {
                partners.Add(releasingAgent);
            }

            var issuingCarrier = GetIssuingCarrierAgentPartner(shipment);
            if (issuingCarrier != null)
            {
                partners.Add(issuingCarrier);
            }

            var consolidator = GetConsolidatorPartner(shipment);
            if (consolidator != null)
            {
                partners.Add(consolidator);
            }

            return partners;
        }

        #region Private methods
        private void InitializeServices(string shipmentId, int tenant)
        {
            this.tenant = tenant;
            addressRepository = new AddressRepository(this.tenant);
            contactRepository = new ContactRepository(this.tenant);
            countryRepository = new CountryRepository(this.tenant);
            shipmentRepository = new ShipmentRepository(this.tenant);

        }
       
        private void GetSharedLogisticsSetting()
        {
            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
        }
        
        private ShipmentPartnerPM GetShipperPartner(Shipment shipment)
        {
            bool isShipperShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperShared;
            if (string.IsNullOrEmpty(shipment.ShipperId) || !isShipperShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ShipperId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ShipperReference1) ? "" : shipment.ShipperReference1;
            item.Reference2 = string.IsNullOrEmpty(shipment.ShipperReference2) ? "" : shipment.ShipperReference2;
            item.PartnerType = "Shipper";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
            item.PartnerName = string.IsNullOrEmpty(card?.EnglishName) ? "" : card?.EnglishName;

            AssignAddressToPartner(shipment.ShipperAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ShipperContactId, tenant);
            item.Email = string.IsNullOrEmpty(contact?.Email) ? "" : contact?.Email;
            item.ContactName = string.IsNullOrEmpty(contact?.EnglishName) ? "" : contact?.EnglishName;

            return item;
        }

        private ShipmentPartnerPM GetConsigeePartner(Shipment shipment)
        {
            bool isConsigneeShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeShared;
            if (string.IsNullOrEmpty(shipment.ConsigneeId) || !isConsigneeShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ConsigneeId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ConsigneeReference1) ? "" : shipment.ConsigneeReference1;
            item.Reference2 = string.IsNullOrEmpty(shipment.ConsigneeReference2) ? "" : shipment.ConsigneeReference2;
            item.PartnerType = "Consignee";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
            item.PartnerName = card?.EnglishName;

            AssignAddressToPartner(shipment.ConsigneeAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ConsigneeContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
       
        private ShipmentPartnerPM GetAgentPartner(Shipment shipment)
        {
            bool isAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsAgentShared;
            if (string.IsNullOrEmpty(shipment.AgentId) || !isAgentShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.AgentId;
            item.Reference1 = string.IsNullOrEmpty(shipment.AgentReference1) ? "" : shipment.AgentReference1;
            item.Reference2 = string.IsNullOrEmpty(shipment.AgentReference2) ? "" : shipment.AgentReference2;
            item.PartnerType = "Agent";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.AgentId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.AgentAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.AgentContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;

        }
        
        private ShipmentPartnerPM GetColoaderPartner(Shipment shipment)
        {
            bool isColoaderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsColoaderShared;
            if (string.IsNullOrEmpty(shipment.ColoaderId) || !isColoaderShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ColoaderId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ColoaderReference1) ? "" : shipment.ColoaderReference1;
            item.PartnerType = "Coloader";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ColoaderId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.ColoaderAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ColoaderContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
       
        private ShipmentPartnerPM GetConsigneeNotImporterPartner(Shipment shipment)
        {
            bool isConsigneeNotImporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeNotImporterShared;
            if (string.IsNullOrEmpty(shipment.ConsigneeNotImporterId) || !isConsigneeNotImporterShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ConsigneeNotImporterId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ConsigneeNotImporterReference) ? "" : shipment.ConsigneeNotImporterReference;
            item.PartnerType = "Consignee Not Importer";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ConsigneeNotImporterId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.ConsigneeNotImporterAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ConsigneeNotImporterContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetFreightForwarderPartner(Shipment shipment)
        {
            bool isFreightForwarderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsFreightForwarderShared;
            if (string.IsNullOrEmpty(shipment.FreightForwarderId) || !isFreightForwarderShared)
            {
                return null;
            }
            var item = new ShipmentPartnerPM();
            item.Id = shipment.FreightForwarderId;
            item.Reference1 = string.IsNullOrEmpty(shipment.FreightForwarderReference) ? "" : shipment.FreightForwarderReference;
            item.PartnerType = "Freight Forwarder";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.FreightForwarderId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.FreightForwarderAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.FreightForwarderContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetNotify1Partner(Shipment shipment)
        {
            bool isNotify1Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify1Shared;
            if (string.IsNullOrEmpty(shipment.Notify1Id) || !isNotify1Shared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.Notify1Id;
            item.Reference1 = string.IsNullOrEmpty(shipment.Notify1Reference) ? "" : shipment.Notify1Reference;
            item.Reference2 = string.IsNullOrEmpty(shipment.Notify1Reference2) ? "" : shipment.Notify1Reference2;
            item.PartnerType = "Notify 1";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.Notify1Id, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.Notify1AddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.Notify1ContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetNotify2Partner(Shipment shipment)
        {
            bool isNotify2Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify2Shared;
            if (string.IsNullOrEmpty(shipment.Notify2Id) || !isNotify2Shared)
            {
                return null;
            }
            var item = new ShipmentPartnerPM();
            item.Id = shipment.Notify2Id;
            item.Reference1 = string.IsNullOrEmpty(shipment.Notify2Reference) ? "" : shipment.Notify2Reference;
            item.PartnerType = "Notify 2";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.Notify2Id, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.Notify2AddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.Notify2ContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetShipperNotExporterPartner(Shipment shipment)
        {
            bool isShipperNotExporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperNotExporterShared;
            if (string.IsNullOrEmpty(shipment.ShipperNotExporterId) || !isShipperNotExporterShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ShipperNotExporterId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ShipperNotExporterReference1) ? "" : shipment.ShipperNotExporterReference1;
            item.Reference2 = string.IsNullOrEmpty(shipment.ShipperNotExporterReference2) ? "" : shipment.ShipperNotExporterReference2;
            item.PartnerType = "Shipper Not Exporter";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ShipperNotExporterId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.ShipperNotExporterAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ShipperNotExporterContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
       
        private ShipmentPartnerPM GetCustomsAgentExportPartner(Shipment shipment)
        {
            bool isCustomsAgentExportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentExportShared;
            if (string.IsNullOrEmpty(shipment.CustomAgentExportId) || !isCustomsAgentExportShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.CustomAgentExportId;
            item.Reference1 = string.IsNullOrEmpty(shipment.CustomAgentExportReference) ? "" : shipment.CustomAgentExportReference;
            item.PartnerType = "Custom Agent Export";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.CustomAgentExportId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.CustomAgentExportAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.CustomAgentExportContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetCustomsAgentImportPartner(Shipment shipment)
        {
            bool isCustomsAgentImportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentImportShared;
            if (string.IsNullOrEmpty(shipment.CustomAgentImportId) || !isCustomsAgentImportShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.CustomAgentImportId;
            item.Reference1 = string.IsNullOrEmpty(shipment.CustomAgentImportReference) ? "" : shipment.CustomAgentImportReference;
            item.PartnerType = "Custom Agent Import";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.CustomAgentImportId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.CustomAgentImportAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.CustomAgentImportContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetCustomClearancePartner(Shipment shipment)
        {
            bool isCustomClearancePoinShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomClearancePoinShared;
            if (string.IsNullOrEmpty(shipment.CustomClearancePointId) || !isCustomClearancePoinShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.CustomClearancePointId;
            item.Reference1 = string.IsNullOrEmpty(shipment.CustomClearancePointReference1) ? "" : shipment.CustomClearancePointReference1;
            item.PartnerType = "Custom Clearance Point";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.CustomClearancePointId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.CustomClearancePointAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.CustomClearancePointContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
       
        private ShipmentPartnerPM GetConsolidatorPartner(Shipment shipment)
        {
            bool isConsolidatorShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsolidatorShared;
            if (string.IsNullOrEmpty(shipment.ConsolidatorId) || !isConsolidatorShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ConsolidatorId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ConsolidatorReference) ? "" : shipment.ConsolidatorReference;
            item.PartnerType = "Consolidator";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ConsolidatorId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.ConsolidatorAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ConsolidatorContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetReleasingAgentPartner(Shipment shipment)
        {
            bool isReleasingAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsReleasingAgentShared;
            if (string.IsNullOrEmpty(shipment.ReleasingAgentId) || !isReleasingAgentShared)
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.ReleasingAgentId;
            item.Reference1 = string.IsNullOrEmpty(shipment.ReleasingAgentReference1) ? "" : shipment.ReleasingAgentReference1;
            item.Reference2 = string.IsNullOrEmpty(shipment.ReleasingAgentReference2) ? "" : shipment.ReleasingAgentReference2;
            item.PartnerType = "Releasing Agent";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.ReleasingAgentId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.ReleasingAgentAddressId, item);

            var contact = contactRepository.GetSingleContact(shipment.ReleasingAgentContactId, tenant);
            if (contact != null)
            {
                item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
            }

            return item;
        }
        
        private ShipmentPartnerPM GetIssuingCarrierAgentPartner(Shipment shipment)
        {
            bool isIssuingCarrierAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsIssuingCarrierAgentShared;
            if (string.IsNullOrEmpty(shipment.IssuingCarrierAgentId) || !isIssuingCarrierAgentShared && shipment.TransportModeId == "A")
            {
                return null;
            }

            var item = new ShipmentPartnerPM();
            item.Id = shipment.IssuingCarrierAgentId;
            item.Reference1 = string.IsNullOrEmpty(shipment.IssuingCarrierReference1) ? "" : shipment.IssuingCarrierReference1;
            item.PartnerType = "Issuing Carrier Agent";
            item.CountryCode = "";
            item.Email = "";
            item.ContactName = "";

            var card = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, tenant, true);
            if (card != null)
            {
                item.PartnerName = card.EnglishName;
            }

            AssignAddressToPartner(shipment.IssuingCarrierAddressId, item);
            return item;
        }
        
        private void AssignAddressToPartner(string addressId, ShipmentPartnerPM item)
        {
            var address = addressRepository.GetSingleAddress(addressId, tenant);
            item.Name = address?.Name;
            item.Address1 = string.IsNullOrEmpty(address?.Address1) ? "" : address?.Address1;
            item.Address2 = string.IsNullOrEmpty(address?.Address2) ? "" : address?.Address2;
            item.Phone = string.IsNullOrEmpty(address?.PhoneNumber) ? "" : address?.PhoneNumber;
            item.Fax = string.IsNullOrEmpty(address?.FaxNumber) ? "" : address?.FaxNumber;
            item.CityZipCode = address?.City + (string.IsNullOrEmpty(address?.ZipCode) ? "" : ", " + address?.ZipCode);
            var country = countryRepository.GetSingleCountry(address?.CountryId, tenant);
            item.CountryName = country?.EnglishName;
            item.CountryCode = country?.Code;
        }
        #endregion Private methods
    }
}
