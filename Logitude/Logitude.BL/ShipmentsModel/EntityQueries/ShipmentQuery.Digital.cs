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
        List<ShipmentPartnerPM> partners;
        AddressRepository addressRepository;
        ContactRepository contactRepository;
        CountryRepository countryRepository;
        ShipmentRepository shipmentRepository;
        Shipment shipment;
        int tenant;
        SharedLogisticsSetting sharedLogisticsSetting;

        public List<ShipmentPartnerPM> GetDigitalShipmentPartners(string shipmentId, int tenant)
        {
            Initialize(shipmentId, tenant);
            if (shipment == null)
            {
                return null;
            }

            GetSharedLogisticsSetting();
            AddShipperPartner();
            AddConsigeePartner();
            AddAgentPartner();
            AddColoaderPartner();
            AddConsigneeNotImporterPartner();
            AddFreightForwarderPartner();
            AddNotify1Partner();
            AddNotify2Partner();
            AddShipperNotExporterPartner();
            AddCustomsAgentExportPartner();
            AddCustomsAgentImportPartner();
            AddCustomClearancePartner();
            AddConsolidatorPartner();
            AddReleasingAgentPartner();
            AddIssuingCarrierAgentPartner();
            return partners;
        }

        private void Initialize(string shipmentId, int tenant)
        {
            this.tenant = tenant;
            partners = new List<ShipmentPartnerPM>();
            addressRepository = new AddressRepository(this.tenant);
            contactRepository = new ContactRepository(this.tenant);
            countryRepository = new CountryRepository(this.tenant);
            shipmentRepository = new ShipmentRepository(this.tenant);
            shipment = shipmentRepository.GetSingleShipment(shipmentId, this.tenant);
        }
        private void GetSharedLogisticsSetting()
        {
            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
        }
        private void AddShipperPartner()
        {
            bool isShipperShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperShared;
            if (string.IsNullOrEmpty(shipment.ShipperId) || !isShipperShared)
            {
                return;
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

            partners.Add(item);
        }

        private void AddConsigeePartner()
        {
            bool isConsigneeShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeShared;
            if (string.IsNullOrEmpty(shipment.ConsigneeId) || !isConsigneeShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddAgentPartner()
        {
            bool isAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsAgentShared;
            if (string.IsNullOrEmpty(shipment.AgentId) || !isAgentShared)
            {
                return;
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

            partners.Add(item);

        }
        private void AddColoaderPartner()
        {
            bool isColoaderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsColoaderShared;
            if (string.IsNullOrEmpty(shipment.ColoaderId) || !isColoaderShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddConsigneeNotImporterPartner()
        {
            bool isConsigneeNotImporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeNotImporterShared;
            if (string.IsNullOrEmpty(shipment.ConsigneeNotImporterId) || !isConsigneeNotImporterShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddFreightForwarderPartner()
        {
            bool isFreightForwarderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsFreightForwarderShared;
            if (string.IsNullOrEmpty(shipment.FreightForwarderId) || !isFreightForwarderShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddNotify1Partner()
        {
            bool isNotify1Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify1Shared;
            if (string.IsNullOrEmpty(shipment.Notify1Id) && !isNotify1Shared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddNotify2Partner()
        {
            bool isNotify2Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify2Shared;
            if (string.IsNullOrEmpty(shipment.Notify2Id) || !isNotify2Shared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddShipperNotExporterPartner()
        {
            bool isShipperNotExporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperNotExporterShared;
            if (string.IsNullOrEmpty(shipment.ShipperNotExporterId) || !isShipperNotExporterShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddCustomsAgentExportPartner()
        {
            bool isCustomsAgentExportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentExportShared;
            if (string.IsNullOrEmpty(shipment.CustomAgentExportId) || !isCustomsAgentExportShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddCustomsAgentImportPartner()
        {
            bool isCustomsAgentImportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentImportShared;
            if (string.IsNullOrEmpty(shipment.CustomAgentImportId) || !isCustomsAgentImportShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddCustomClearancePartner()
        {
            bool isCustomClearancePoinShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomClearancePoinShared;
            if (string.IsNullOrEmpty(shipment.CustomClearancePointId) || !isCustomClearancePoinShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddConsolidatorPartner()
        {
            bool isConsolidatorShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsolidatorShared;
            if (string.IsNullOrEmpty(shipment.ConsolidatorId) || !isConsolidatorShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddReleasingAgentPartner()
        {
            bool isReleasingAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsReleasingAgentShared;
            if (string.IsNullOrEmpty(shipment.ReleasingAgentId) || !isReleasingAgentShared)
            {
                return;
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

            partners.Add(item);
        }
        private void AddIssuingCarrierAgentPartner()
        {
            bool isIssuingCarrierAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsIssuingCarrierAgentShared;
            if (string.IsNullOrEmpty(shipment.IssuingCarrierAgentId) || !isIssuingCarrierAgentShared && shipment.TransportModeId == "A")
            {
                return;
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
            partners.Add(item);
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
    }
}
