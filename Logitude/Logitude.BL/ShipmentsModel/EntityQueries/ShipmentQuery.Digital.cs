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
using Newtonsoft.Json;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.Helpers;

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ShipperId,
                Reference1 = string.IsNullOrEmpty(shipment.ShipperReference1) ? "" : shipment.ShipperReference1,
                Reference2 = string.IsNullOrEmpty(shipment.ShipperReference2) ? "" : shipment.ShipperReference2,
                PartnerType = "Shipper",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ConsigneeId,
                Reference1 = string.IsNullOrEmpty(shipment.ConsigneeReference1) ? "" : shipment.ConsigneeReference1,
                Reference2 = string.IsNullOrEmpty(shipment.ConsigneeReference2) ? "" : shipment.ConsigneeReference2,
                PartnerType = "Consignee",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.AgentId,
                Reference1 = string.IsNullOrEmpty(shipment.AgentReference1) ? "" : shipment.AgentReference1,
                Reference2 = string.IsNullOrEmpty(shipment.AgentReference2) ? "" : shipment.AgentReference2,
                PartnerType = "Agent",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ColoaderId,
                Reference1 = string.IsNullOrEmpty(shipment.ColoaderReference1) ? "" : shipment.ColoaderReference1,
                PartnerType = "Coloader",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ConsigneeNotImporterId,
                Reference1 = string.IsNullOrEmpty(shipment.ConsigneeNotImporterReference) ? "" : shipment.ConsigneeNotImporterReference,
                PartnerType = "Consignee Not Importer",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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
            var item = new ShipmentPartnerPM
            {
                Id = shipment.FreightForwarderId,
                Reference1 = string.IsNullOrEmpty(shipment.FreightForwarderReference) ? "" : shipment.FreightForwarderReference,
                PartnerType = "Freight Forwarder",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.Notify1Id,
                Reference1 = string.IsNullOrEmpty(shipment.Notify1Reference) ? "" : shipment.Notify1Reference,
                Reference2 = string.IsNullOrEmpty(shipment.Notify1Reference2) ? "" : shipment.Notify1Reference2,
                PartnerType = "Notify 1",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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
            var item = new ShipmentPartnerPM
            {
                Id = shipment.Notify2Id,
                Reference1 = string.IsNullOrEmpty(shipment.Notify2Reference) ? "" : shipment.Notify2Reference,
                PartnerType = "Notify 2",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ShipperNotExporterId,
                Reference1 = string.IsNullOrEmpty(shipment.ShipperNotExporterReference1) ? "" : shipment.ShipperNotExporterReference1,
                Reference2 = string.IsNullOrEmpty(shipment.ShipperNotExporterReference2) ? "" : shipment.ShipperNotExporterReference2,
                PartnerType = "Shipper Not Exporter",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.CustomAgentExportId,
                Reference1 = string.IsNullOrEmpty(shipment.CustomAgentExportReference) ? "" : shipment.CustomAgentExportReference,
                PartnerType = "Custom Agent Export",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.CustomAgentImportId,
                Reference1 = string.IsNullOrEmpty(shipment.CustomAgentImportReference) ? "" : shipment.CustomAgentImportReference,
                PartnerType = "Custom Agent Import",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.CustomClearancePointId,
                Reference1 = string.IsNullOrEmpty(shipment.CustomClearancePointReference1) ? "" : shipment.CustomClearancePointReference1,
                PartnerType = "Custom Clearance Point",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ConsolidatorId,
                Reference1 = string.IsNullOrEmpty(shipment.ConsolidatorReference) ? "" : shipment.ConsolidatorReference,
                PartnerType = "Consolidator",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.ReleasingAgentId,
                Reference1 = string.IsNullOrEmpty(shipment.ReleasingAgentReference1) ? "" : shipment.ReleasingAgentReference1,
                Reference2 = string.IsNullOrEmpty(shipment.ReleasingAgentReference2) ? "" : shipment.ReleasingAgentReference2,
                PartnerType = "Releasing Agent",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

            var item = new ShipmentPartnerPM
            {
                Id = shipment.IssuingCarrierAgentId,
                Reference1 = string.IsNullOrEmpty(shipment.IssuingCarrierReference1) ? "" : shipment.IssuingCarrierReference1,
                PartnerType = "Issuing Carrier Agent",
                CountryCode = "",
                Email = "",
                ContactName = ""
            };

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

        #region Routing 

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
                Title = "Main route information",
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
            return new TransitTime
            {
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
                return shipment.MainCarriageFromPortCode + ", " + shipment.MainCarriageFromPortCountryCode;
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
                return shipment.MainCarriageToPortCode + ", " + shipment.MainCarriageToPortCountryCode;
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
            if (!string.IsNullOrEmpty(partnerAddress.Country?.Code))
            {
                address = address + ", " + partnerAddress.Country?.Code;
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
                    myResult = myResult + ", " + country.Code;
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
            if (!string.IsNullOrEmpty(shipment.PreCarriageFromPortId))
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

            if (!string.IsNullOrEmpty(shipment.OnCarriageFromPortId))
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
                Title = "Main route information",
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
                loadingPort = shipment.PreCarriageFromPortName + ", " + shipment.PreCarriageFromPortCountryCode;
            }
            else
            {
                loadingPort = shipment.MainCarriageFromPortName + ", " + shipment.MainCarriageFromPortCountryCode;
            }

            return loadingPort;
        }

        private string GetDischargePort(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.OnCarriageFromPortId))
            {
                return shipment.OnCarriageToPortName + ", " + shipment.OnCarriageToPortCountryCode;
            }
            else if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3ToPortName + ", " + shipment.Transshipment3ToPortCountryCode;
            }
            else if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment2ToPortName + ", " + shipment.Transshipment2ToPortCountryCode;
            }
            else if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment1ToPortName + ", " + shipment.Transshipment1ToPortCountryCode;
            }
            else if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.MainCarriageToPortName + ", " + shipment.MainCarriageToPortCountryCode;
            }

            return null;
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
                Title = "Pre carriage information",
                LegHeader = "PreCarriage",
                FromPort = shipment.PreCarriageFromPortName + ", " + shipment.PreCarriageFromPortCountryCode,
                ToPort = shipment.PreCarriageToPortName + ", " + shipment.PreCarriageToPortCountryCode,
                TransportMode = GetTransportModeName(shipment.PreCarriageTransportModeId),
                DepartureDate = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD,
                DateType = shipment.PreCarriageATD != null ? "ATD" : (shipment.PreCarriageETD != null ? "ETD" : null),
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
                Title = "Main carriage information",
                LegHeader = "MainCarriage",
                FromPort = shipment.MainCarriageFromPortName + ", " + shipment.MainCarriageFromPortCountryCode,
                ToPort = shipment.MainCarriageToPortName + ", " + shipment.MainCarriageToPortCountryCode,
                Carrier = shipment.MainCarriageCarrierName,
                CarrierNumber = shipment.MainCarriageCarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.MainCarriageVesselName : null,
                DepartureDate = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "ATD" : (shipment.MainCarriageETD != null ? "ETD" : null),
            };
            return mainCarriageLeg;
        }

        private ShipmentRoutingLeg AddTransshipment1Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment1 = new ShipmentRoutingLeg()
            {
                Title = "Transshipment 1 information",
                LegHeader = "Transshipment1",
                FromPort = shipment.Transshipment1FromPortName + ", " + shipment.Transshipment1FromPortCountryCode,
                ToPort = shipment.Transshipment1ToPortName + ", " + shipment.Transshipment1ToPortCountryCode,
                Carrier = shipment.Transshipment1CarrierName,
                CarrierNumber = shipment.Transshipment1CarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.Transshipment1VesselName : null,
                DepartureDate = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD,
                DateType = shipment.Transshipment1ATD != null ? "ATD" : (shipment.Transshipment1ETD != null ? "ETD" : null),
            };

            return transshipment1;
        }

        private ShipmentRoutingLeg AddTransshipment2Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment2 = new ShipmentRoutingLeg()
            {
                Title = "Transshipment 2 information",
                LegHeader = "Transshipment2",
                FromPort = shipment.Transshipment2FromPortName + ", " + shipment.Transshipment2FromPortCountryCode,
                ToPort = shipment.Transshipment2ToPortName + ", " + shipment.Transshipment2ToPortCountryCode,
                Carrier = shipment.Transshipment2CarrierName,
                CarrierNumber = shipment.Transshipment2CarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.Transshipment2VesselName : null,
                DepartureDate = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD,
                DateType = shipment.Transshipment2ATD != null ? "ATD" : (shipment.Transshipment2ETD != null ? "ETD" : null),
            };

            return transshipment2;
        }

        private ShipmentRoutingLeg AddTransshipment3Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment3 = new ShipmentRoutingLeg()
            {
                Title = "Transshipment 3 information",
                LegHeader = "Transshipment3",
                FromPort = shipment.Transshipment3FromPortName + ", " + shipment.Transshipment3FromPortCountryCode,
                ToPort = shipment.Transshipment3ToPortName + ", " + shipment.Transshipment3ToPortCountryCode,
                Carrier = shipment.Transshipment3CarrierName,
                CarrierNumber = shipment.Transshipment3CarrierNumber,
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ? shipment.Transshipment3VesselName : null,
                DepartureDate = shipment.Transshipment3ATD != null ? shipment.Transshipment3ATD : shipment.Transshipment3ETD,
                DateType = shipment.Transshipment3ATD != null ? "ATD" : (shipment.Transshipment3ETD != null ? "ETD" : null),
            };

            return transshipment3;
        }

        private ShipmentRoutingLeg AddOnCarriageLegLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg onCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "On carriage information",
                LegHeader = "OnCarriage",
                FromPort = shipment.OnCarriageFromPortName + ", " + shipment.OnCarriageFromPortCountryCode,
                ToPort = shipment.OnCarriageToPortName + ", " + shipment.OnCarriageToPortCountryCode,
                TransportMode = GetTransportModeName(shipment.OnCarriageTransportModeId),
                DepartureDate = shipment.OnCarriageATD != null ? shipment.OnCarriageATD : shipment.OnCarriageETD,
                DateType = shipment.OnCarriageATD != null ? "ATD" : (shipment.OnCarriageETD != null ? "ETD" : null),
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

        private string GetVerticalTimeLineMasterTextCode(ShipmentPM shipment)
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
                return "CMR/RWB/PRO #";
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

        private string GetVerticalTimelineCarrierNumberTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Flight no";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Voyage no";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Trucker no";
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

        #endregion Routing

        #region Shipment Horizontal TimeLine
        public void BuildShipmentListWithTimeLine(List<DigitalShipmentList> entityLists, int tenant)
        {
            InitializeServices(tenant);
            foreach (var item in entityLists)
            {
                this.FillShipmnetTimeLine(item, tenant);
            }
        }

        private void FillShipmnetTimeLine(DigitalShipmentList shipment, int tenant)
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

        private TimeLineData FillShipmnetTimeLineForInlandDomesticShipment(DigitalShipmentList shipment)
        {
            TimeLineData timeLineData = new TimeLineData();
            this.FillMainCarraigeFromTimeLineForInlandDomesticShipment(timeLineData, shipment);
            this.FillMainCarraigeToTimeLineForInlandDomesticShipment(timeLineData, shipment);
            return timeLineData;
        }

        private void FillMainCarraigeFromTimeLineForInlandDomesticShipment(TimeLineData timeLineData, DigitalShipmentList shipment)
        {
            timeLineData.MainCarriageFrom = new TimeLineStop()
            {
                City = GetCityForFromInlandDomestic(shipment),
                CountryCode = GetCountryForFromInlandDomestic(shipment),
                Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
            };
        }

        private void FillMainCarraigeToTimeLineForInlandDomesticShipment(TimeLineData timeLineData, DigitalShipmentList shipment)
        {
            timeLineData.MainCarriageTo = new TimeLineStop()
            {
                City = GetCityForToInlandDomestic(shipment),
                CountryCode = GetCountryForToInlandDomestic(shipment),
                Date = shipment.MainCarriageFinalDestinationATA != null ? shipment.MainCarriageFinalDestinationATA : shipment.MainCarriageFinalDestinationETA,
                DateType = shipment.MainCarriageFinalDestinationATA != null ? "Actual" : (shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null),
            };
        }

        private string GetCityForFromInlandDomestic(DigitalShipmentList shipment)
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

        private string GetCityForToInlandDomestic(DigitalShipmentList shipment)
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

        private string GetCountryForFromInlandDomestic(DigitalShipmentList shipment)
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

        private string GetCountryForToInlandDomestic(DigitalShipmentList shipment)
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
            if (string.IsNullOrEmpty(partnerAddress.Country?.Code))
            {
                country = partnerAddress.Country?.Code;
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

        private TimeLineData FillShipmnetTimeLineForShipment(DigitalShipmentList shipment)
        {
            var shipmentPickUpDeliveries = (from a in repository.context.ShipmentPickUpDeliveries.Include("FromAddressCountry").Include("ToAddressCountry") where a.ShipmentId == shipment.Id select a);
            TimeLineData timeLineData = new TimeLineData();
            this.FillMainCarraigeFromTimeLine(timeLineData, shipment);
            this.FillMainCarraigeToTimeLine(timeLineData, shipment);
            this.FillPickUpTimeLine(timeLineData, shipmentPickUpDeliveries);
            this.FillDeliveryTimeLine(timeLineData, shipmentPickUpDeliveries);
            return timeLineData;
        }

        private void FillMainCarraigeFromTimeLine(TimeLineData timeLineData, DigitalShipmentList shipment)
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

        private bool CheckIfViaPortsDatesFilled(DigitalShipmentList shipment)
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

        private void FillMainCarraigeToTimeLine(TimeLineData timeLineData, DigitalShipmentList shipment)
        {
            timeLineData.MainCarriageTo = new TimeLineStop()
            {
                City = !string.IsNullOrEmpty(shipment.MainCarriageToCity) ? shipment.MainCarriageToCity : shipment.ToPortName,
                CountryCode = shipment.ToCountryCode,
                Date = shipment.MainCarriageFinalDestinationATA != null ? shipment.MainCarriageFinalDestinationATA : shipment.MainCarriageFinalDestinationETA,
                DateType = shipment.MainCarriageFinalDestinationATA != null ? "Actual" : (shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null),
            };
        }


        private void FillPickUpTimeLine(TimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
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

        private void FillPickUpCityAndCountry(dynamic timeLineData, ShipmentPickUpDelivery firstPickup, int tenant)
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

        private void FillDeliveryTimeLine(TimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
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

        private void FillDeliveryCityAndCountry(dynamic timeLineData, ShipmentPickUpDelivery finalDelivery, int tenant)
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
        #endregion Shipment Horizontal TimeLine

        #region Shipment Vertical TimeLine

        public VerticalTimeLineData MapVerticalTimeLine(ShipmentPM shipment)
        {
            InitializeServices(tenant);

            // Build All Legs 
            var shipmentPickUpDeliveries = (from a in repository.context.ShipmentPickUpDeliveries.Include("FromAddressCountry").Include("ToAddressCountry") where a.ShipmentId == shipment.Id select a);
            VerticalTimeLineData VerticalTimeLineData = new VerticalTimeLineData();

            // Pick Up 
            this.FillPickUpVerticalTimeLine(VerticalTimeLineData, shipmentPickUpDeliveries);

            // WarehouseLeg,WarehouseLeg2 (Terminal hub)
            this.FillWarehouseLegsTimeLine(VerticalTimeLineData, shipment);

            // Pre Carraige
            this.FillPreCarraigeLegTimeLine(VerticalTimeLineData, shipment);

            // Main Carraige from
            this.FillMainCarraigeFromVerticalTimeLine(VerticalTimeLineData, shipment);

            // Transshipments  (1, 2, 3)
            this.FillTransshipmentTimeLine(VerticalTimeLineData, shipment);

            // Main Carraige to
            this.FillMainCarraigeToVerticalTimeLine(VerticalTimeLineData, shipment);

            // On Carraige
            this.FillOnCarraigeLegTimeLine(VerticalTimeLineData, shipment);

            // Delivery
            this.FillDeliveryVerticalTimeLine(VerticalTimeLineData, shipmentPickUpDeliveries);

            return VerticalTimeLineData;
        }

        private void FillPickUpVerticalTimeLine(VerticalTimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {

            var firstPickup = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "PICK").OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (firstPickup == null)
            {
                return;
            }

            int tenant = firstPickup.Tenant;
            timeLineData.Pickup = new VerticalTimeLineStop()
            {
                Title = "Pick up",
                City = "",
                CountryCode = "",
                ATDDate = firstPickup.ATD != null ? firstPickup.ATD : firstPickup.ETD,
                ATDDateType = firstPickup.ATD != null ? "Actual" : (firstPickup.ETD != null ? "Estimated" : null),
                ATADate = firstPickup.ATA != null ? firstPickup.ATA : firstPickup.ETA,
                ATADateType = firstPickup.ATA != null ? "Actual" : (firstPickup.ETA != null ? "Estimated" : null),
                Date = firstPickup.ATD != null ? firstPickup.ATD : firstPickup.ETD,
                DateType = firstPickup.ATD != null ? "Actual" : (firstPickup.ETD != null ? "Estimated" : null),
                LegDetails = FillPickUpDeliveryLegDetails(firstPickup),
                TransportModeId = firstPickup.TransportModeCode,
            };

            this.FillPickUpCityAndCountry(timeLineData, firstPickup, tenant);
        }

        private Dictionary<string, string> FillPickUpDeliveryLegDetails(ShipmentPickUpDelivery firstPickup)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            Card trucker = cardRepository.GetSingleCardWithoutInclude(firstPickup.CarrierId, tenant);
            var legDetails = new Dictionary<string, string>
            {
                { "Trucker Name", CheckEmptyValue(trucker?.EnglishName) },
                { "Trucker Number", CheckEmptyValue(firstPickup.CarrierNumber) }
            };

            return legDetails;
        }

        private void FillWarehouseLegsTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.WarehouseLegWarehouseId))
            {
                VerticalTimeLineData.Warehouse1 = new VerticalTimeLineStop()
                {
                    Title = "Terminal hub",
                    City = shipment.WarehouseLegAddressCity,
                    CountryCode = shipment.WarehouseLegAddressCountryCode,
                    Date = shipment.WarehouseLegActualEntryDate != null ? shipment.WarehouseLegActualEntryDate : shipment.WarehouseLegExpectedEntryDate,
                    DateType = shipment.WarehouseLegActualEntryDate != null ? "Actual" : (shipment.WarehouseLegExpectedEntryDate != null ? "Estimated" : null),
                    ATDDate = shipment.WarehouseLegActualEntryDate != null ? shipment.WarehouseLegActualEntryDate : shipment.WarehouseLegExpectedEntryDate,
                    ATDDateType = shipment.WarehouseLegActualEntryDate != null ? "Actual" : (shipment.WarehouseLegExpectedEntryDate != null ? "Estimated" : null),
                    ATADate = shipment.WarehouseLegActualReleaseDate != null ? shipment.WarehouseLegActualReleaseDate : shipment.WarehouseLegExpectedReleaseDate,
                    ATADateType = shipment.WarehouseLegActualReleaseDate != null ? "Actual" : (shipment.WarehouseLegExpectedReleaseDate != null ? "Estimated" : null),
                    LegDetails = FillTerminalLegDetails(shipment),
                    TransportModeId = shipment.TransportModeId,
                };
            }
        }

        private Dictionary<string, string> FillTerminalLegDetails(ShipmentPM shipment)
        {
            var storageDays = GetStorageDays(shipment);
            var storageDaysText = storageDays == 0 ? "–" : storageDays + " days";
            var warehouseLegCutOffDate = shipment.WarehouseLegCutOffDate == null ? "–" : shipment.WarehouseLegCutOffDate?.ToString("dd/MM/yyyy");
            var legDetails = new Dictionary<string, string>
            {
                { "Warehouse cut off date", warehouseLegCutOffDate},
                { "Storage days", storageDaysText }
            };

            return legDetails;
        }

        private int GetStorageDays(ShipmentPM shipment)
        {
            int storageDays = 0;
            if (shipment.WarehouseLegActualReleaseDate != null && shipment.WarehouseLegActualEntryDate != null)
            {
                if (shipment.WarehouseLegActualReleaseDate >= shipment.WarehouseLegActualEntryDate)
                {
                    DateTime warehouseLegActualReleaseDate = (DateTime)shipment.WarehouseLegActualReleaseDate;
                    DateTime warehouseLegActualEntryDate = (DateTime)shipment.WarehouseLegActualEntryDate;
                    TimeSpan span = warehouseLegActualReleaseDate.Subtract(warehouseLegActualEntryDate);
                    storageDays = (int)Math.Round(span.TotalDays);
                }
            }

            return storageDays;
        }

        private void FillPreCarraigeLegTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.PreCarriageFromPortId))
            {
                VerticalTimeLineData.PreCarriage = new VerticalTimeLineStop()
                {
                    Title = "Pre carriage",
                    City = shipment.PreCarriageFromPortName,
                    CountryCode = shipment.PreCarriageFromPortCountryCode,
                    Date = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD,
                    DateType = shipment.PreCarriageATD != null ? "Actual" : (shipment.PreCarriageETD != null ? "Estimated" : null),
                    ATDDate = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD,
                    ATDDateType = shipment.PreCarriageATD != null ? "Actual" : (shipment.PreCarriageETD != null ? "Estimated" : null),
                    ATADate = shipment.PreCarriageATA != null ? shipment.PreCarriageATA : shipment.PreCarriageETA,
                    ATADateType = shipment.PreCarriageATA != null ? "Actual" : (shipment.PreCarriageETA != null ? "Estimated" : null),
                    TransportModeId = shipment.TransportModeId,
                    LegDetails = FillPreCarriageLegDetails(shipment),
                    LegTransportModeId = shipment.PreCarriageTransportModeId,
                };
            }
        }

        private Dictionary<string, string> FillPreCarriageLegDetails(ShipmentPM shipment)
        {
            var legDetails = new Dictionary<string, string>
            {
                { "Carrier", CheckEmptyValue(shipment.PreCarriageCarrierName) },
                { "Carrier No", CheckEmptyValue(shipment.PreCarriageCarrierNumber) }
            };

            if (shipment.PreCarriageTransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(shipment.PreCarriageVesselName));
            }

            return legDetails;
        }

        private void FillMainCarraigeFromVerticalTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            VerticalTimeLineData.MainCarriageFrom = new VerticalTimeLineStop()
            {
                Title = shipment.TransportModeId == "A" ? "Gateway" : "Port of loading",
                City = shipment.MainCarriageFromPortName,
                CountryCode = shipment.MainCarriageFromPortCountryCode,
                Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
                ATDDate = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                ATDDateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
                ATADate = shipment.MainCarriageATA != null ? shipment.MainCarriageATA : shipment.MainCarriageETA,
                ATADateType = shipment.MainCarriageATA != null ? "Actual" : (shipment.MainCarriageETA != null ? "Estimated" : null),
                LegDetails = FillMainCarriageFromLegDetails(shipment),
            };
        }

        private Dictionary<string, string> FillMainCarriageFromLegDetails(ShipmentPM shipment)
        {
            var master = shipment.TransportModeId == "A" ? (shipment.AirlinePrefix != null && shipment.Master != null ? shipment.AirlinePrefix + "-" + shipment.Master : shipment.Master) : shipment.Master;
            var legDetails = new Dictionary<string, string>
            {
                { GetCarrierTextCode(shipment), CheckEmptyValue(shipment.MainCarriageCarrierName) },
                { GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.MainCarriageCarrierNumber) },
                { GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master) }
            };

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(shipment.MainCarriageVesselName));
            }

            return legDetails;
        }

        private void FillTransshipmentTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
            {
                VerticalTimeLineData.Transshipment1 = new VerticalTimeLineStop()
                {
                    Title = "Transshipment 1 port",
                    City = shipment.Transshipment1FromPortName,
                    CountryCode = shipment.Transshipment1FromPortCountryCode,
                    Date = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD,
                    DateType = shipment.Transshipment1ATD != null ? "Actual" : (shipment.Transshipment1ETD != null ? "Estimated" : null),
                    ATDDate = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD,
                    ATDDateType = shipment.Transshipment1ATD != null ? "Actual" : (shipment.Transshipment1ETD != null ? "Estimated" : null),
                    ATADate = shipment.Transshipment1ATA != null ? shipment.Transshipment1ATA : shipment.Transshipment1ETA,
                    ATADateType = shipment.Transshipment1ATA != null ? "Actual" : (shipment.Transshipment1ETA != null ? "Estimated" : null),
                    LegDetails = FillTransshipment1LegDetails(shipment),
                };
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
            {
                VerticalTimeLineData.Transshipment2 = new VerticalTimeLineStop()
                {
                    Title = "Transshipment 2 port",
                    City = shipment.Transshipment2FromPortName,
                    CountryCode = shipment.Transshipment2FromPortCountryCode,
                    Date = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD,
                    DateType = shipment.Transshipment2ATD != null ? "Actual" : (shipment.Transshipment2ETD != null ? "Estimated" : null),
                    ATDDate = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD,
                    ATDDateType = shipment.Transshipment2ATD != null ? "Actual" : (shipment.Transshipment2ETD != null ? "Estimated" : null),
                    ATADate = shipment.Transshipment2ATA != null ? shipment.Transshipment2ATA : shipment.Transshipment2ETA,
                    ATADateType = shipment.Transshipment2ATA != null ? "Actual" : (shipment.Transshipment2ETA != null ? "Estimated" : null),
                    LegDetails = FillTransshipment2LegDetails(shipment),
                };
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId))
            {
                VerticalTimeLineData.Transshipment3 = new VerticalTimeLineStop()
                {
                    Title = "Transshipment 3 port",
                    City = shipment.Transshipment3FromPortName,
                    CountryCode = shipment.Transshipment3FromPortCountryCode,
                    Date = shipment.Transshipment3ATD != null ? shipment.Transshipment3ATD : shipment.Transshipment3ETD,
                    DateType = shipment.Transshipment3ATD != null ? "Actual" : (shipment.Transshipment3ETD != null ? "Estimated" : null),
                    ATDDate = shipment.Transshipment3ATD != null ? shipment.Transshipment3ATD : shipment.Transshipment3ETD,
                    ATDDateType = shipment.Transshipment3ATD != null ? "Actual" : (shipment.Transshipment3ETD != null ? "Estimated" : null),
                    ATADate = shipment.Transshipment3ATA != null ? shipment.Transshipment3ATA : shipment.Transshipment3ETA,
                    ATADateType = shipment.Transshipment3ATA != null ? "Actual" : (shipment.Transshipment3ETA != null ? "Estimated" : null),
                    LegDetails = FillTransshipment3LegDetails(shipment),
                };
            }
        }

        private Dictionary<string, string> FillTransshipment1LegDetails(ShipmentPM shipment)
        {
            var master = shipment.Transshipment1AdditionalMAWBOBLBL;
            var legDetails = new Dictionary<string, string>
            {
                { GetCarrierTextCode(shipment), CheckEmptyValue(shipment.Transshipment1CarrierName) },
                { GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.Transshipment1CarrierNumber) },
                { GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master) }
            };

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(shipment.Transshipment1VesselName));
            }

            return legDetails;
        }

        private Dictionary<string, string> FillTransshipment2LegDetails(ShipmentPM shipment)
        {
            var master = shipment.Transshipment2AdditionalMAWBOBLBL;
            var legDetails = new Dictionary<string, string>
            {
                { GetCarrierTextCode(shipment), CheckEmptyValue(shipment.Transshipment2CarrierName) },
                { GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.Transshipment2CarrierNumber) },
                { GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master) }
            };

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(shipment.Transshipment2VesselName));
            }

            return legDetails;
        }

        private Dictionary<string, string> FillTransshipment3LegDetails(ShipmentPM shipment)
        {
            var master = shipment.Transshipment3AdditionalMAWBOBLBL;
            var legDetails = new Dictionary<string, string>
            {
                { GetCarrierTextCode(shipment), CheckEmptyValue(shipment.Transshipment3CarrierName) },
                { GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.Transshipment3CarrierNumber) },
                { GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master) }
            };

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(shipment.Transshipment3VesselName));
            }

            return legDetails;
        }

        private void FillMainCarraigeToVerticalTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            VerticalTimeLineData.MainCarriageTo = new VerticalTimeLineStop()
            {
                Title = shipment.TransportModeId == "A" ? "Destination" : "Discharge port",
                City = shipment.MainCarriageFinalDestinationPortName,
                CountryCode = shipment.MainCarriageFinalDestinationPortCountryCode,
                ATADate = shipment.MainCarriageFinalDestinationATA != null ? shipment.MainCarriageFinalDestinationATA : shipment.MainCarriageFinalDestinationETA,
                ATADateType = shipment.MainCarriageFinalDestinationATA != null ? "Actual" : (shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null),
                Date = shipment.MainCarriageFinalDestinationATA != null ? shipment.MainCarriageFinalDestinationATA : shipment.MainCarriageFinalDestinationETA,
                DateType = shipment.MainCarriageFinalDestinationATA != null ? "Actual" : (shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null),
                ATDDate = GetMainCarraigeToATDDate(shipment),
                ATDDateType = GetMainCarraigeToATDDateType(shipment),
                LegDetails = FillMainCarraigeToLegDetails(shipment)
            };
        }

        private DateTime? GetMainCarraigeToATDDate(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3ATD != null ? shipment.Transshipment3ATD : shipment.Transshipment3ETD;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD;
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD;
            }

            return null;
        }

        private string GetMainCarraigeToATDDateType(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3ATD != null ? "Actual" : (shipment.Transshipment3ETD != null ? "Estimated" : null);
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment1ATD != null ? "Actual" : (shipment.Transshipment1ETD != null ? "Estimated" : null);
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment2ATD != null ? "Actual" : (shipment.Transshipment2ETD != null ? "Estimated" : null);
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null);
            }

            return null;
        }

        private Dictionary<string, string> FillMainCarraigeToLegDetails(ShipmentPM shipment)
        {
            var master = GetMasterOfDischargeLeg(shipment);
            var legDetails = new Dictionary<string, string>
            {
                { GetCarrierTextCode(shipment), CheckEmptyValue(GetCarrierNameOfDischargeLeg(shipment)) },
                { GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(GetCarrierNumberOfDischargeLeg(shipment)) },
                { GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master) }
            };

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(GetVesselOfDischargeLeg(shipment)));
            }

            return legDetails;
        }

        private string GetMasterOfDischargeLeg(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3AdditionalMAWBOBLBL;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment2AdditionalMAWBOBLBL;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment1AdditionalMAWBOBLBL;
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.TransportModeId == "A" ? (shipment.AirlinePrefix != null && shipment.Master != null ? shipment.AirlinePrefix + "-" + shipment.Master : shipment.Master) : shipment.Master;
            }

            return null;
        }

        private string GetCarrierNameOfDischargeLeg(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3CarrierName;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment2CarrierName;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment1CarrierName;
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.MainCarriageCarrierName;
            }

            return null;
        }

        private string GetCarrierNumberOfDischargeLeg(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3CarrierNumber;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment2CarrierNumber;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment1CarrierNumber;
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.MainCarriageCarrierNumber;
            }

            return null;
        }

        private string GetVesselOfDischargeLeg(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return shipment.Transshipment3VesselName;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return shipment.Transshipment2VesselName;
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return shipment.Transshipment1VesselName;
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return shipment.MainCarriageVesselName;
            }

            return null;
        }

        private void FillOnCarraigeLegTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {

            if (!string.IsNullOrEmpty(shipment.OnCarriageFromPortId))
            {
                VerticalTimeLineData.OnCarriage = new VerticalTimeLineStop()
                {
                    Title = "On carriage",
                    City = shipment.OnCarriageFromPortName,
                    CountryCode = shipment.OnCarriageFromPortCountryCode,
                    Date = shipment.OnCarriageATA != null ? shipment.OnCarriageATA : shipment.OnCarriageETA,
                    DateType = shipment.OnCarriageATA != null ? "Actual" : (shipment.OnCarriageETA != null ? "Estimated" : null),
                    ATADate = shipment.OnCarriageATA != null ? shipment.OnCarriageATA : shipment.OnCarriageETA,
                    ATADateType = shipment.OnCarriageATA != null ? "Actual" : (shipment.OnCarriageETA != null ? "Estimated" : null),
                    ATDDate = shipment.OnCarriageATD != null ? shipment.OnCarriageATD : shipment.OnCarriageETD,
                    ATDDateType = shipment.OnCarriageATD != null ? "Actual" : (shipment.OnCarriageETD != null ? "Estimated" : null),
                    TransportModeId = shipment.TransportModeId,
                    LegDetails = FillOnCarriageLegDetails(shipment),
                    LegTransportModeId = shipment.OnCarriageTransportModeId,
                };
            }
        }

        private Dictionary<string, string> FillOnCarriageLegDetails(ShipmentPM shipment)
        {
            Dictionary<string, string> legDetails = new Dictionary<string, string>();
            legDetails.Add("Carrier", CheckEmptyValue(shipment.OnCarriageCarrierName));
            legDetails.Add("Carrier No", CheckEmptyValue(shipment.OnCarriageCarrierNumber));

            if (shipment.OnCarriageTransportModeId == "O")
            {
                legDetails.Add("Vessel", CheckEmptyValue(shipment.OnCarriageVesselName));
            }

            return legDetails;
        }

        private void FillDeliveryVerticalTimeLine(VerticalTimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var finalDelivery = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (finalDelivery == null)
            {
                return;
            }

            int tenant = finalDelivery.Tenant;
            timeLineData.Delivery = new VerticalTimeLineStop()
            {
                Title = "Delivery",
                City = "",
                CountryCode = "",
                Date = finalDelivery.ATA != null ? finalDelivery.ATA : finalDelivery.ETA,
                DateType = finalDelivery.ATA != null ? "Actual" : (finalDelivery.ETA != null ? "Estimated" : null),
                ATADate = finalDelivery.ATA != null ? finalDelivery.ATA : finalDelivery.ETA,
                ATADateType = finalDelivery.ATA != null ? "Actual" : (finalDelivery.ETA != null ? "Estimated" : null),
                ATDDate = finalDelivery.ATD != null ? finalDelivery.ATD : finalDelivery.ETD,
                ATDDateType = finalDelivery.ATD != null ? "Actual" : (finalDelivery.ETD != null ? "Estimated" : null),
                LegDetails = FillPickUpDeliveryLegDetails(finalDelivery),
                TransportModeId = finalDelivery.TransportModeCode,
            };

            this.FillDeliveryCityAndCountry(timeLineData, finalDelivery, tenant);
        }

        private string CheckEmptyValue(string value)
        {
            var newValue = value;
            if (string.IsNullOrWhiteSpace(value))
            {
                newValue = "–";
            }

            return newValue;
        }
        #endregion Shipment Vertical TimeLine

        #region Transport & Subtypes 

        public List<DigitalTransportModes> GetTransportModesWithSubTypes(int tenant)
        {
            var digitalTransportModes = new List<DigitalTransportModes>();
            var shipmentSubTypes = GetShipmentTypes(tenant);

            // Air 
            digitalTransportModes.Add(new DigitalTransportModes()
            {
                Name = "Air",
                Code = "A",
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "Air" }, "A")
            });

            // Ocean 
            digitalTransportModes.Add(new DigitalTransportModes()
            {
                Name = "Ocean",
                Code = "O",
                Children = GetOceanChildren(shipmentSubTypes),
            });

            // Ocean 
            digitalTransportModes.Add(new DigitalTransportModes()
            {
                Name = "Inland",
                Code = "I",
                Children = GetInlandChildren(shipmentSubTypes)
            });

            return digitalTransportModes;
        }

        private List<DigitalTransportModesChild> GetOceanChildren(List<ShipmentSubType> shipmentSubTypes)
        {
            var oceanChildren = new List<DigitalTransportModesChild>();
            oceanChildren.Add(new DigitalTransportModesChild()
            {
                ParentCode = "O",
                Name = "FCL",
                Code = "FCL,FCLD",
                DisaledOption = true,
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "FCL","FCLD"}, "O")
            });
            oceanChildren.Add(new DigitalTransportModesChild()
            {
                ParentCode = "O",
                Name = "LCL",
                Code = "LCL,LCLD",
                DisaledOption = true,
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "LCL", "LCLD" }, "O")
            });
            oceanChildren.Add(new DigitalTransportModesChild()
            {
                ParentCode = "O",
                Name = "Groupage Ocean",
                Code = "MyGo",
                DisaledOption = true,
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "MyGo" }, "O")
            });

            return oceanChildren;
        }

        private List<DigitalTransportModesChild> GetInlandChildren(List<ShipmentSubType> shipmentSubTypes)
        {
            var inlandChildren = new List<DigitalTransportModesChild>();
            inlandChildren.Add(new DigitalTransportModesChild()
            {
                ParentCode = "I",
                Name = "FTL",
                Code = "FTL",
                DisaledOption = true,
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "FTL" }, "I")
            });
            inlandChildren.Add(new DigitalTransportModesChild()
            {
                ParentCode = "I",
                Name = "LTL",
                Code = "LTL",
                DisaledOption = true,
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "LTL" }, "I")
            });
            inlandChildren.Add(new DigitalTransportModesChild()
            {
                ParentCode = "I",
                Name = "Groupage Inland",
                Code = "MyGI",
                DisaledOption = true,
                Children = GetSubTypes(shipmentSubTypes, new List<string> { "MyGI" }, "I")
            });

            return inlandChildren;
        }

        private List<DigitalTransportModesChild> GetSubTypes(List<ShipmentSubType> shipmentSubTypes, List<string> typeCodes, string parentCode)
        {
            var subTypes = shipmentSubTypes.Where(a => typeCodes.Contains(a.ShipmentTypeCode) || a.ShipmentTypeCode == null).ToList();
            if (subTypes == null)
                return null;

            var digitalSubTypes = new List<DigitalTransportModesChild>();
            foreach (var item in subTypes)
            {
                digitalSubTypes.Add(new DigitalTransportModesChild()
                {
                    ParentCode = parentCode,
                    Name = item.Name,
                    Code = item.Id,
                    DisaledOption = true,
                });
            }

            return digitalSubTypes;
        }

        private List<ShipmentSubType> GetShipmentTypes(int tenant)
        {
            ShipmentSubTypeRepository shipmentSubTypeRepository = new ShipmentSubTypeRepository(repository.context);
            IQueryable<ShipmentSubType> shipmentSubTypes = shipmentSubTypeRepository.GetShipmentSubTypes(tenant);
            return shipmentSubTypes.ToList();
        }

        #endregion Transport & Subtypes

        #region Global Search 
        public IQueryable<DigitalShipmentList> GetByFilters(GeneralFilters newFilters)
        {
            var tenant = newFilters.Tenant;

            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);

            var filters = new ApiQueryFilters()
            {
                Filter1Value = newFilters.CardId,
                Filter2Value = newFilters.CardType
            };

            var queryOperations = new QueryOperations()
            {
                ObjectTableName = "Shipment",
                PageIndex = newFilters.PageIndex,
                PageSize = newFilters.PageSize,
                QuerySection = "Shipments",
                SortByColumnName = newFilters.SortBy,
                SortDirectin = newFilters.SortDirection,
                QueryFilterItems = new List<QueryFilterItem>(),
            };

            string partnerTypeName = string.Empty;
            string shipmentLevelCodeValue = string.Empty;

            if (newFilters.CardType == "CS")
            {
                shipmentLevelCodeValue = "D,H,A";
                partnerTypeName = "CustomerId";
            }
            else if (newFilters.CardType == "AG")
            {
                shipmentLevelCodeValue = "D,C";
                partnerTypeName = "AgentId";
            }

            if (!string.IsNullOrEmpty(newFilters.CardId))
            {
                queryOperations.SetFilter(partnerTypeName, newFilters.CardId, false, "Equals", null, false);
            }

            if (!string.IsNullOrEmpty(shipmentLevelCodeValue))
            {
                queryOperations.SetFilter("ShipmentLevelCode", shipmentLevelCodeValue, false, "InListExact", null, false);
            }

            var ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableNameWithNoIncludes("Shipment", tenant);

            foreach (var filter in newFilters.AdditionalFilters)
            {
                var field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                if (field != null)
                {
                    string valuestring1 = filter.FieldValue?.ToString();
                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                    string valuestring2 = filter.FieldValue2?.ToString();
                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                    string valuestring3 = filter.FieldValue3?.ToString();
                    object value3 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring3);
                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                }
                else
                {
                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.FieldValue3, filter.DisplayInList);
                }
            }

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);

            var shipmentRepository = new ShipmentRepository(tenant);

            var customfilters = new ShipmentCustomFilter(tenant);

            IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(tenant);

            shipments = DigitalPortalCustomFilter.GetDigtalFilteredQuery(queryOperations, shipments, shipmentRepository, tenant);

            var nonListQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
            };

            var listQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
            };

            var genericFilter = new Simplog.Server.Infrastructure.Helpers.GenericFilter();

            shipments = genericFilter.GetFilteredQuery(nonListQueryOperation, shipments);

            var myShipmentQuery = new ShipmentQuery(shipmentRepository);
            var entityLists = myShipmentQuery.GetDigitalIQueryableShipmentList(shipments, tenant);
            entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                var propInfo = typeof(DigitalShipmentList).GetProperty(queryOperations.SortByColumnName);
                var shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

                var objectField = shipmentObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);

                if (objectField != null)
                {
                    var sortClass = new Simplog.Server.Infrastructure.Helpers.GenericSort();

                    if (!objectField.IsCustom)
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                                {
                                    entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "double":
                                {
                                    entityLists = sortClass.GetSorterQuery<DigitalShipmentList, double>(queryOperations, entityLists);
                                    break;
                                }
                            case "datetime":
                                {
                                    entityLists = sortClass.GetSorterQuery<DigitalShipmentList, DateTime>(queryOperations, entityLists);
                                    break;
                                }
                            case "integer":
                                {
                                    entityLists = sortClass.GetSorterQuery<DigitalShipmentList, int>(queryOperations, entityLists);
                                    break;
                                }
                            case "lookup":
                                {
                                    entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "boolean":
                                {
                                    entityLists = sortClass.GetSorterQuery<DigitalShipmentList, bool>(queryOperations, entityLists);
                                    break;
                                }
                            default:
                                {
                                    entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        entityLists = sortClass.GetSorterQuery<DigitalShipmentList, string>(queryOperations, entityLists);
                    }
                }
            }
            else
            {
                entityLists = entityLists.OrderByDescending(d => d.CreateDateTime);
            }

            return entityLists;
        }

        #endregion

        public Tuple<string, int> GetShipmentIdBySecurityKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            Shipment shipment = repository.context
                                          .Shipments
                                          .FirstOrDefault(a => a.SecurityKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (shipment == null)
            {
                return null;
            }

            return Tuple.Create(shipment.Id, shipment.Tenant);
        }
    }

    [JsonObject(IsReference = false, ItemIsReference = false)]
    public class DigitalTransportModes
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }
        [JsonProperty(PropertyName = "children")]
        public List<DigitalTransportModesChild> Children { get; set; }
    }

    [JsonObject(IsReference = false, ItemIsReference = false)]
    public class DigitalTransportModesChild
    {
        [JsonProperty(PropertyName = "parentCode")]
        public string ParentCode { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }
        [JsonProperty(PropertyName = "disaledOption")]
        public bool DisaledOption { get; set; }
        [JsonProperty(PropertyName = "children")]
        public List<DigitalTransportModesChild> Children { get; set; }
    }

}
