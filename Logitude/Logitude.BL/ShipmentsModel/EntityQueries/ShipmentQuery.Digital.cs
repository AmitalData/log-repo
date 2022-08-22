using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Collections.Generic;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using System;
using Logitude.BL.ShipmentsModel.EntityLists;
using System.Linq;
using System.Data.Entity;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

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
            InitializeServices(tenant);
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
        private void InitializeServices(int tenant)
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

        public List<ShipmentRoutingLeg> GetDigitalShipmentRoutingLegs(string shipmentId, int tenant)
        {
            InitializeServices(tenant);

            var routingLegs = new List<ShipmentRoutingLeg>();
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM shipment = shipmentQuery.GetSinglePMWithoutComposition(shipmentId, tenant);

            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");

            if (isInlandDomesticShipment)
            {
                routingLegs = GetDigitalShipmentRoutingLegsForInlandDomesticShipment(shipment);
            }
            else
            {
                routingLegs = GetDigitalShipmentRoutingLegsForShipment(shipment);
            }

            return routingLegs;
        }

        private List<ShipmentRoutingLeg> GetDigitalShipmentRoutingLegsForInlandDomesticShipment(ShipmentPM shipment)
        {
            var routingLegs = new List<ShipmentRoutingLeg>();
            ShipmentRoutingLeg mainRouteInformation = AddMainRouteInformationForInlandDomesticShipment(shipment);
            if (mainRouteInformation != null)
            {
                routingLegs.Add(mainRouteInformation);
            }
            return routingLegs;
        }

        private ShipmentRoutingLeg AddMainRouteInformationForInlandDomesticShipment(ShipmentPM shipment)
        {
            // Main Info InlandDomestic
            ShipmentRoutingLeg mainRouteInformation = new MainRouteInformation()
            {
                Title = "Main route details",
                LegHeader = "MainRouteInlandDomestic",
                FromPort = GetFromAddressForInlandDomestic(shipment),
                ToPort = GetToAddressForInlandDomestic(shipment),
                TransportMode = "Inland",
                TransitTime = GetTransitTimeForInlandDomesticShipment(shipment),
            };

            return mainRouteInformation;
        }

        private TransitTime GetTransitTimeForInlandDomesticShipment(ShipmentPM shipment)
        {
            return new TransitTime { 
                DichargeDate = shipment.MainCarriageATA != null ? shipment.MainCarriageATA : shipment.MainCarriageETA,
                LoadingDate = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD
            };
        }

        private string GetFromAddressForInlandDomestic(ShipmentPM shipment)
        {
            if (shipment.InlandDomesticFromTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return shipment.MainCarriageFromPortCode + "," + shipment.MainCarriageFromPortCountryCode;
            }
            else if (shipment.InlandDomesticFromTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(shipment.InlandDomesticFromCountryId, shipment.InlandDomesticFromCity);
            }

            return null;
        }

        private string GetToAddressForInlandDomestic(ShipmentPM shipment)
        {
            if (shipment.InlandDomesticToTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (shipment.InlandDomesticToTypeCode == "PORT")
            {
                return shipment.MainCarriageToPortCode + "," + shipment.MainCarriageToPortCountryCode;
            }
            else if (shipment.InlandDomesticToTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(shipment.InlandDomesticToCountryId, shipment.InlandDomesticToCity);
            }

            return null;
        }

        private string GetFullAddressByPartnerId(string addressId)
        {
            if (string.IsNullOrEmpty(addressId))
            {
                return null;
            }

            Address partnerAddress = addressRepository.GetSingleAddress(addressId, tenant);
            if (partnerAddress == null)
            {
                return null;
            }

            var address = partnerAddress.City;
            if (!string.IsNullOrEmpty(partnerAddress.Country?.EnglishName))
            {
                address = address + "," + partnerAddress.Country?.Code;
            }

            return address;
        }

        private string GetFullAddressByCASLAddress(string countryId, string city)
        {
            string myResult = "";

            if (!string.IsNullOrEmpty(city))
            {
                myResult = city;
            }

            if (!string.IsNullOrEmpty(countryId))
            {
                Country country = CountryRepository.GetSingleCountry(countryId, tenant, false);
                if (country != null)
                {
                    myResult = myResult + "," + country.Code;
                }
            }

            return myResult;
        }

        private List<ShipmentRoutingLeg> GetDigitalShipmentRoutingLegsForShipment(ShipmentPM shipment)
        {
            var routingLegs = new List<ShipmentRoutingLeg>();

            // Main Info
            ShipmentRoutingLeg mainRouteInformation = AddMainRouteInformation(shipment);
            if (mainRouteInformation != null)
            {
                routingLegs.Add(mainRouteInformation);
            }

            // PreCarriage 
            if (!string.IsNullOrEmpty(shipment.PreCarriageCarrierId))
            {
                ShipmentRoutingLeg preCarriageLeg = AddPreCarriageLegLeg(shipment);
                if (preCarriageLeg != null)
                {
                    routingLegs.Add(preCarriageLeg);
                }
            }

            //MainCarriage
            ShipmentRoutingLeg mainCarriage = AddMainCarriageLeg(shipment);
            if (mainCarriage != null)
            {
                routingLegs.Add(mainCarriage);
            }

            //Transshipment shipments 
            if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
            {
                ShipmentRoutingLeg transshipment1Leg = AddTransshipment1Leg(shipment);
                if (transshipment1Leg != null)
                {
                    routingLegs.Add(transshipment1Leg);
                }
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
            {
                ShipmentRoutingLeg transshipment2Leg = AddTransshipment2Leg(shipment);
                if (transshipment2Leg != null)
                {
                    routingLegs.Add(transshipment2Leg);
                }
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId))
            {
                ShipmentRoutingLeg transshipment3Leg = AddTransshipment3Leg(shipment);
                if (transshipment3Leg != null)
                {
                    routingLegs.Add(transshipment3Leg);
                }
            }

            if (!string.IsNullOrEmpty(shipment.OnCarriageCarrierId))
            {
                //OnCarriage
                ShipmentRoutingLeg onCarriageLeg = AddOnCarriageLegLeg(shipment);
                if (onCarriageLeg != null)
                {
                    routingLegs.Add(onCarriageLeg);
                }
            }

            return routingLegs;
        }

        private ShipmentRoutingLeg AddMainRouteInformation(ShipmentPM shipment)
        {
            // Main Info
            ShipmentRoutingLeg mainRouteInformation = new MainRouteInformation()
            {
                Title = "Main route info",
                LegHeader = "MainRoute",
                Master = shipment.TransportModeId == "A" ? (shipment.AirlinePrefix != null && shipment.Master != null ? shipment.AirlinePrefix + "-" + shipment.Master : shipment.Master) : shipment.Master,
                MasterLabel = GetMasterTextCode(shipment),
                LoadingPortLabel = shipment.TransportModeId == "A" ? "Gateway" : "Port of loading",
                DischargePortLabel = shipment.TransportModeId == "A" ? "Destination" : "Port of discharge",
                LoadingPort = GetLoadingPort(shipment),
                DischargePort = GetDischargePort(shipment),
                TransitTime = GetTransitTime(shipment),
            };

            return mainRouteInformation;
        }

        private string GetLoadingPort(ShipmentPM shipment)
        {
            string loadingPort = null;
            if (!string.IsNullOrEmpty(shipment.PreCarriageCarrierId))
            {
                loadingPort = shipment.PreCarriageFromPortCode + "," + shipment.PreCarriageFromPortCountryCode;
            }
            else
            {
                loadingPort = shipment.MainCarriageFromPortCode + "," + shipment.MainCarriageFromPortCountryCode;
            }

            return loadingPort;
        }
        
        private string GetDischargePort(ShipmentPM shipment)
        {
            string dischargePort = null;
            if (!string.IsNullOrEmpty(shipment.OnCarriageCarrierId))
            {
                dischargePort = shipment.OnCarriageToPortCode + "," + shipment.OnCarriageToPortCountryCode;
            }
            else
            {
                dischargePort = shipment.MainCarriageToPortCode + "," + shipment.MainCarriageToPortCountryCode;
            }

            return dischargePort;
        }

        private TransitTime GetTransitTime(ShipmentPM shipment)
        {
            return new TransitTime
            {
                DichargeDate = GetPortOfDischargeDate(shipment),
                LoadingDate = GetPortOfLoadingDate(shipment)
            };
        }

        private DateTime? GetPortOfLoadingDate(ShipmentPM shipment)
        {
            if (shipment.PreCarriageATD != null)
            {
                return shipment.PreCarriageATD;
            }

            if (shipment.PreCarriageETD != null)
            {
                return shipment.PreCarriageETD;
            }

            if (shipment.MainCarriageATD != null)
            {
                return shipment.MainCarriageATD;
            }

            if (shipment.MainCarriageETD != null)
            {
                return shipment.MainCarriageETD;
            }

            return null;
        }
        
        private DateTime? GetPortOfDischargeDate(ShipmentPM shipment)
        {
            if (shipment.OnCarriageATA != null)
            {
                return shipment.OnCarriageATA;
            }

            if (shipment.OnCarriageETA != null)
            {
                return shipment.OnCarriageETA;
            }

            if (shipment.MainCarriageATA != null)
            {
                return shipment.MainCarriageATA;
            }

            if (shipment.MainCarriageETA != null)
            {
                return shipment.MainCarriageETA;
            }

            return null;
        }
        private ShipmentRoutingLeg AddPreCarriageLegLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg preCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "Pre carriage info.",
                LegHeader = "PreCarriage",
                FromPort = shipment.PreCarriageFromPortCode + "," + shipment.PreCarriageFromPortCountryCode,
                ToPort = shipment.PreCarriageToPortCode + "," + shipment.PreCarriageToPortCountryCode,
                TransportMode = GetTransportModeName(shipment.PreCarriageTransportModeId),
                DepartureDate = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD,
                Carrier = shipment.PreCarriageCarrierName,
                CarrierNumber = shipment.PreCarriageCarrierNumber,
                VesselName = shipment.PreCarriageTransportModeId == "O" ? shipment.PreCarriageVesselName : null,
            };

            return preCarriageLeg;
        }

        private ShipmentRoutingLeg AddMainCarriageLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg mainCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "Main carriage info.",
                LegHeader = "MainCarriage",
                FromPort = shipment.MainCarriageFromPortCode + "," + shipment.MainCarriageFromPortCountryCode,
                ToPort = shipment.MainCarriageToPortCode + "," + shipment.MainCarriageToPortCountryCode,
                Carrier = shipment.MainCarriageCarrierName,
                CarrierNumber = shipment.MainCarriageCarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.MainCarriageVesselName : null,
                DepartureDate = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
            };
            return mainCarriageLeg;
        }

        private ShipmentRoutingLeg AddTransshipment1Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment1 = new ShipmentRoutingLeg()
            {
                Title = "Transshipment 1 info.",
                LegHeader = "Transshipment1",
                FromPort = shipment.Transshipment1FromPortCode + "," + shipment.Transshipment1FromPortCountryCode,
                ToPort = shipment.Transshipment1ToPortCode + "," + shipment.Transshipment1ToPortCountryCode,
                Carrier = shipment.Transshipment1CarrierName,
                CarrierNumber = shipment.Transshipment1CarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.Transshipment1VesselName : null,
                DepartureDate = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD,
            };

            return transshipment1;
        }

        private ShipmentRoutingLeg AddTransshipment2Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment2 = new ShipmentRoutingLeg()
            {
                Title = "Transshipment 2 info.",
                LegHeader = "Transshipment2",
                FromPort = shipment.Transshipment2FromPortCode + "," + shipment.Transshipment2FromPortCountryCode,
                ToPort = shipment.Transshipment2ToPortCode + "," + shipment.Transshipment2ToPortCountryCode,
                Carrier = shipment.Transshipment2CarrierName,
                CarrierNumber = shipment.Transshipment2CarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.Transshipment2VesselName : null,
                DepartureDate = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD,
            };

            return transshipment2;
        }

        private ShipmentRoutingLeg AddTransshipment3Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment3 = new ShipmentRoutingLeg()
            {
                Title = "Transshipment 3 info.",
                LegHeader = "Transshipment3",
                FromPort = shipment.Transshipment3FromPortCode + "," + shipment.Transshipment3FromPortCountryCode,
                ToPort = shipment.Transshipment3ToPortCode + "," + shipment.Transshipment3ToPortCountryCode,
                Carrier = shipment.Transshipment3CarrierName,
                CarrierNumber = shipment.Transshipment3CarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.Transshipment3VesselName : null,
                DepartureDate = shipment.Transshipment3ATD != null ? shipment.Transshipment3ATD : shipment.Transshipment3ETD,
            };

            return transshipment3;
        }

        private ShipmentRoutingLeg AddOnCarriageLegLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg onCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "On carriage info.",
                LegHeader = "OnCarriage",
                FromPort = shipment.OnCarriageFromPortCode + "," + shipment.OnCarriageFromPortCountryCode,
                ToPort = shipment.OnCarriageToPortCode + "," + shipment.OnCarriageToPortCountryCode,
                TransportMode = GetTransportModeName(shipment.OnCarriageTransportModeId),
                DepartureDate = shipment.OnCarriageATD != null ? shipment.OnCarriageATD : shipment.OnCarriageETD,
                Carrier = shipment.OnCarriageCarrierName,
                CarrierNumber = shipment.OnCarriageCarrierNumber,
                VesselName = shipment.OnCarriageTransportModeId == "O" ? shipment.OnCarriageVesselName : null,
            };

            return onCarriageLeg;
        }
        
        private string GetMasterTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "MAWB";
            }

            if (shipment.TransportModeId == "O")
            {
                return "OBL";
            }

            if (shipment.TransportModeId == "I")
            {
                return "CMR/RWB#";
            }

            return null;
        }

        private string GetCarrierTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Airline";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Shipping line";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Trucker";
            }

            return null;
        }
        
        private string GetCarrierNumberTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Flight number";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Voyage number";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Trucker number";
            }

            return null;
        }

        private string GetTransportModeName(string transportModeId)
        {
            if (transportModeId == "A")
            {
                return "Air";
            }

            if (transportModeId == "O")
            {
                return "Ocean";
            }

            if (transportModeId == "I")
            {
                return "Inland";
            }

            return null;
        }

        #region Shipment TimeLine
        public void BuildShipmentListWithTimeLine(List<ShipmentList> entityLists, int tenant)
        {
            InitializeServices(tenant);
            foreach (var item in entityLists)
            {
                this.FillShipmnetTimeLine(item, tenant);
            }
        }

        private void FillShipmnetTimeLine(ShipmentList shipment, int tenant)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            if (isInlandDomesticShipment)
            {
                shipment.TimeLineData = FillShipmnetTimeLineForInlandDomesticShipment(shipment);
            }
            else
            {
                shipment.TimeLineData = FillShipmnetTimeLineForShipment(shipment);
            }
        }

        private TimeLineData FillShipmnetTimeLineForInlandDomesticShipment(ShipmentList shipment)
        {
            TimeLineData timeLineData = new TimeLineData();
            this.FillMainCarraigeFromTimeLineForInlandDomesticShipment(timeLineData, shipment);
            this.FillMainCarraigeToTimeLineForInlandDomesticShipment(timeLineData, shipment);
            return timeLineData;
        }

        private void FillMainCarraigeFromTimeLineForInlandDomesticShipment(TimeLineData timeLineData, ShipmentList shipment)
        {
            timeLineData.MainCarriageFrom = new TimeLineStop()
            {
                City = GetCityForFromInlandDomestic(shipment),
                CountryCode = GetCountryForFromInlandDomestic(shipment),
                Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
            };
        }

        private void FillMainCarraigeToTimeLineForInlandDomesticShipment(TimeLineData timeLineData, ShipmentList shipment)
        {
            timeLineData.MainCarriageTo = new TimeLineStop()
            {
                City = GetCityForToInlandDomestic(shipment),
                CountryCode = GetCountryForToInlandDomestic(shipment),
                Date = shipment.MainCarriageFinalDestinationATA != null ? shipment.MainCarriageFinalDestinationATA : shipment.MainCarriageFinalDestinationETA,
                DateType = shipment.MainCarriageFinalDestinationATA != null ? "Actual" : (shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null),
            };
        }

        private string GetCityForFromInlandDomestic(ShipmentList shipment)
        {
            if (shipment.InlandDomesticFromTypeCode == "PART")
            {
                return GetCityByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return shipment.MainCarriageFromPortCode;
            }
            else if (shipment.InlandDomesticFromTypeCode == "CASL")
            {
                return shipment.InlandDomesticFromCity;
            }

            return null;
        }
        
        private string GetCityForToInlandDomestic(ShipmentList shipment)
        {
            if (shipment.InlandDomesticToTypeCode == "PART")
            {
                return GetCityByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (shipment.InlandDomesticToTypeCode == "PORT")
            {
                return shipment.MainCarriageToPortCode;
            }
            else if (shipment.InlandDomesticToTypeCode == "CASL")
            {
                return shipment.InlandDomesticToCity;
            }

            return null;
        }

        private string GetCountryForFromInlandDomestic(ShipmentList shipment)
        {
            if (shipment.InlandDomesticFromTypeCode == "PART")
            {
                return GetCountryByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return shipment.MainCarriageFromPortCode;
            }
            else if (shipment.InlandDomesticFromTypeCode == "CASL")
            {
                return GetCountryByCASLAddress(shipment.InlandDomesticFromCountryId);
            }

            return null;
        }

        private string GetCountryForToInlandDomestic(ShipmentList shipment)
        {
            if (shipment.InlandDomesticToTypeCode == "PART")
            {
                return GetCountryByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (shipment.InlandDomesticToTypeCode == "PORT")
            {
                return shipment.MainCarriageToPortCode;
            }
            else if (shipment.InlandDomesticToTypeCode == "CASL")
            {
                return GetCountryByCASLAddress(shipment.InlandDomesticToCountryId);
            }

            return null;
        }
       
        private string GetCityByPartnerId(string addressId)
        {
            if (string.IsNullOrEmpty(addressId))
            {
                return null;
            }

            Address partnerAddress = addressRepository.GetSingleAddress(addressId, tenant);
            if (partnerAddress == null)
            {
                return null;
            }

            var city = partnerAddress.City;
            
            return city;
        }
      
        private string GetCountryByPartnerId(string addressId)
        {
            if (string.IsNullOrEmpty(addressId))
            {
                return null;
            }

            Address partnerAddress = addressRepository.GetSingleAddress(addressId, tenant);
            if (partnerAddress == null)
            {
                return null;
            }

            var country = partnerAddress.City;
            if (string.IsNullOrEmpty(partnerAddress.Country?.EnglishName))
            {
                country = partnerAddress.Country?.EnglishName;
            }

            return country;
        }
        
        private string GetCountryByCASLAddress(string countryId)
        {
            string myResult = "";

            if (!string.IsNullOrEmpty(countryId))
            {
                Country country = CountryRepository.GetSingleCountry(countryId, tenant, false);
                if (country != null)
                {
                    myResult = country.Code;
                }
            }

            return myResult;
        }

        private TimeLineData FillShipmnetTimeLineForShipment(ShipmentList shipment)
        {
            var shipmentPickUpDeliveries = (from a in repository.context.ShipmentPickUpDeliveries.Include("FromAddressCountry").Include("ToAddressCountry") where a.ShipmentId == shipment.Id select a);
            TimeLineData timeLineData = new TimeLineData();
            this.FillMainCarraigeFromTimeLine(timeLineData, shipment);
            this.FillMainCarraigeToTimeLine(timeLineData, shipment);
            this.FillPickUpTimeLine(timeLineData, shipment, shipmentPickUpDeliveries);
            this.FillDeliveryTimeLine(timeLineData, shipment, shipmentPickUpDeliveries);
            return timeLineData;
        }

        private void FillMainCarraigeFromTimeLine(TimeLineData timeLineData, ShipmentList shipment)
        {
            timeLineData.MainCarriageFrom = new TimeLineStop()
            {
                City = !string.IsNullOrEmpty(shipment.MainCarriageFromCity) ? shipment.MainCarriageFromCity : shipment.FromPortName,
                CountryCode = shipment.FromCountryCode,
                Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
                IsViaPortsDatesFilled = CheckIfViaPortsDatesFilled(shipment),
            };
        }

        private bool CheckIfViaPortsDatesFilled(ShipmentList shipment)
        {
            if (shipment.Transshipment1ETA != null || shipment.Transshipment1ATA != null || shipment.Transshipment1ETD != null || shipment.Transshipment1ATD != null)
            {
                return true;
            }
            if (shipment.Transshipment2ETA != null || shipment.Transshipment2ATA != null || shipment.Transshipment2ETD != null || shipment.Transshipment2ATD != null)
            {
                return true;
            }
            if (shipment.Transshipment3ETA != null || shipment.Transshipment3ATA != null || shipment.Transshipment3ETD != null || shipment.Transshipment3ATD != null)
            {
                return true;
            }

            return false;
        }

        private void FillMainCarraigeToTimeLine(TimeLineData timeLineData, ShipmentList shipment)
        {
            timeLineData.MainCarriageTo = new TimeLineStop()
            {
                City = !string.IsNullOrEmpty(shipment.MainCarriageToCity) ? shipment.MainCarriageToCity : shipment.ToPortName,
                CountryCode = shipment.ToCountryCode,
                Date = shipment.MainCarriageFinalDestinationATA != null ? shipment.MainCarriageFinalDestinationATA : shipment.MainCarriageFinalDestinationETA,
                DateType = shipment.MainCarriageFinalDestinationATA != null ? "Actual" : (shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null),
            };
        }
       
        private void FillPickUpTimeLine(TimeLineData timeLineData, ShipmentList item, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var firstPickup = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "PICK").OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (firstPickup == null)
            {
                return;
            }

            int tenant = firstPickup.Tenant;
            timeLineData.Pickup = new TimeLineStop()
            {
                City = "",
                CountryCode = "",
                Date = firstPickup.ATD != null ? firstPickup.ATD : firstPickup.ETD,
                DateType = firstPickup.ATD != null ? "Actual" : (firstPickup.ETD != null ? "Estimated" : null),
            };

            this.FillPickUpCityAndCountry(timeLineData, firstPickup, tenant);
        }
      
        private void FillPickUpCityAndCountry(TimeLineData timeLineData, ShipmentPickUpDelivery firstPickup, int tenant)
        {
            switch (firstPickup.PickUpDeliveryFromTypeCode)
            {
                case "PART":
                    {
                        if (!string.IsNullOrEmpty(firstPickup.FromPartnerCardId))
                        {
                            if (!string.IsNullOrEmpty(firstPickup.FromAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(firstPickup.FromAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.Pickup.City = myPartnerAddress.City;
                                    timeLineData.Pickup.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(firstPickup.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.Pickup.City = myPartnerAddress.City;
                                    timeLineData.Pickup.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                        }
                        break;
                    }

                case "PORT":
                    {
                        if (!string.IsNullOrEmpty(firstPickup.FromPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, firstPickup.FromPortId, true);
                            if (myPort != null)
                            {
                                timeLineData.Pickup.City = myPort.EnglishName;
                                timeLineData.Pickup.CountryCode = myPort.CountryCode;
                            }
                        }
                        break;
                    }

                case "CASL":
                    {
                        timeLineData.Pickup.City = firstPickup.FromAddressCity;
                        timeLineData.Pickup.CountryCode = firstPickup.FromAddressCountry?.Code;
                        break;
                    }
            }
        }
       
        private void FillDeliveryTimeLine(TimeLineData timeLineData, ShipmentList item, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var finalDelivery = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (finalDelivery == null)
            {
                return;
            }

            int tenant = finalDelivery.Tenant;
            timeLineData.Delivery = new TimeLineStop()
            {
                City = "",
                CountryCode = "",
                Date = finalDelivery.ATA != null ? finalDelivery.ATA : finalDelivery.ETA,
                DateType = finalDelivery.ATA != null ? "Actual" : (finalDelivery.ETA != null ? "Estimated" : null),
            };

            this.FillDeliveryCityAndCountry(timeLineData, finalDelivery, tenant);
        }
      
        private void FillDeliveryCityAndCountry(TimeLineData timeLineData, ShipmentPickUpDelivery finalDelivery, int tenant)
        {
            switch (finalDelivery.PickUpDeliveryToTypeCode)
            {
                case "PART":
                    {
                        if (!string.IsNullOrEmpty(finalDelivery.ToPartnerCardId))
                        {
                            if (!string.IsNullOrEmpty(finalDelivery.ToAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(finalDelivery.ToAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.Delivery.City = myPartnerAddress.City;
                                    timeLineData.Delivery.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(finalDelivery.ToPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.Delivery.City = myPartnerAddress.City;
                                    timeLineData.Delivery.CountryCode = myPartnerAddress.Country?.Code;
                                }
                            }
                        }

                        break;
                    }

                case "PORT":
                    {
                        if (!string.IsNullOrEmpty(finalDelivery.ToPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, finalDelivery.ToPortId, true);
                            if (myPort != null)
                            {
                                timeLineData.Delivery.City = myPort.EnglishName;
                                timeLineData.Delivery.CountryCode = myPort.CountryCode;
                            }
                        }

                        break;
                    }

                case "CASL":
                    {
                        timeLineData.Delivery.City = finalDelivery.ToAddressCity;
                        timeLineData.Delivery.CountryCode = finalDelivery.ToAddressCountry?.Code;
                        break;
                    }
            }
        }
        #endregion Shipment TimeLine

    }
}
