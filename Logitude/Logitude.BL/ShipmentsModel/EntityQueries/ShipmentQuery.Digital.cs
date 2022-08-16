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

        public List<ShipmentRoutingLeg> GetDigitalShipmentRoutingLegs(string shipmentId, int tenant)
        {
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
           
            return routingLegs;
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
            ShipmentRoutingLeg preCarriageLeg = AddPreCarriageLegLeg(shipment);
            if (preCarriageLeg != null)
            {
                routingLegs.Add(preCarriageLeg);
            }

            //MainCarriage
            ShipmentRoutingLeg mainCarriage = AddMainCarriageLeg(shipment);
            if (mainCarriage != null)
            {
                routingLegs.Add(mainCarriage);
            }

            //Transshipment shipments 
            ShipmentRoutingLeg transshipment1Leg = AddTransshipment1Leg(shipment);
            if (transshipment1Leg != null)
            {
                routingLegs.Add(transshipment1Leg);
            }

            ShipmentRoutingLeg transshipment2Leg = AddTransshipment2Leg(shipment);
            if (transshipment2Leg != null)
            {
                routingLegs.Add(transshipment2Leg);
            }

            ShipmentRoutingLeg transshipment3Leg = AddTransshipment3Leg(shipment);
            if (transshipment3Leg != null)
            {
                routingLegs.Add(transshipment3Leg);
            }

            //OnCarriage
            ShipmentRoutingLeg onCarriageLeg = AddOnCarriageLegLeg(shipment);
            if (onCarriageLeg != null)
            {
                routingLegs.Add(onCarriageLeg);
            }

            return routingLegs;
        }

        private ShipmentRoutingLeg AddMainRouteInformation(ShipmentPM shipment)
        {
            // Main Info
            ShipmentRoutingLeg mainRouteInformation = new MainRouteInformation()
            {
                Title = "Main Route Info",
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

        private string GetTransitTime(ShipmentPM shipment)
        {
            string transitTime = null;
            var portOfLoadingDate = GetPortOfLoadingDate(shipment);
            var portOfDischargeDate = GetPortOfDischargeDate(shipment);


            return transitTime;
        }

        private DateTime? GetPortOfLoadingDate(ShipmentPM shipment)
        {
            DateTime? portOfLoadingDate = null;
            return portOfLoadingDate;
        }

        private DateTime? GetPortOfDischargeDate(ShipmentPM shipment)
        {
            DateTime? portOfDischargeDate = null;
            return portOfDischargeDate;
        }

        private ShipmentRoutingLeg AddPreCarriageLegLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg preCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "Pre Carriage Info.",
                LegHeader = "PreCarriage",
                FromPortCode = shipment.PreCarriageFromPortCode + "," + shipment.PreCarriageFromPortCountryCode,
                ToPortCode = shipment.PreCarriageToPortCode + "," + shipment.PreCarriageToPortCountryCode,
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
                Title = "Main Carriage Info.",
                LegHeader = "MainCarriage",
                FromPortCode = shipment.MainCarriageFromPortCode + "," + shipment.MainCarriageFromPortCountryCode,
                ToPortCode = shipment.MainCarriageToPortCode + "," + shipment.MainCarriageToPortCountryCode,
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
                Title = "Transshipment 1 Info.",
                LegHeader = "Transshipment1",
                FromPortCode = shipment.Transshipment1FromPortCode + "," + shipment.Transshipment1FromPortCountryCode,
                ToPortCode = shipment.Transshipment1ToPortCode + "," + shipment.Transshipment1ToPortCountryCode,
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
                Title = "Transshipment 2 Info.",
                LegHeader = "Transshipment2",
                FromPortCode = shipment.Transshipment2FromPortCode + "," + shipment.Transshipment2FromPortCountryCode,
                ToPortCode = shipment.Transshipment2ToPortCode + "," + shipment.Transshipment2ToPortCountryCode,
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
                Title = "Transshipment 3 Info.",
                LegHeader = "Transshipment3",
                FromPortCode = shipment.Transshipment3FromPortCode + "," + shipment.Transshipment3FromPortCountryCode,
                ToPortCode = shipment.Transshipment3ToPortCode + "," + shipment.Transshipment3ToPortCountryCode,
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
                Title = "On Carriage Info.",
                LegHeader = "OnCarriage",
                FromPortCode = shipment.OnCarriageFromPortCode + "," + shipment.OnCarriageFromPortCountryCode,
                ToPortCode = shipment.OnCarriageToPortCode + "," + shipment.OnCarriageToPortCountryCode,
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
                return "Shipping Line";
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
                return "Flight Number";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Voyage Number";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Trucker Number";
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
    }
}
