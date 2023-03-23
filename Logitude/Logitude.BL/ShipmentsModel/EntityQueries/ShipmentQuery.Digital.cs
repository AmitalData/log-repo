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
using System.Dynamic;
using Logitude.BL.GlobalModel.EntityPMs;

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
        Dictionary<string, List<string>> blockedFields;

        public ShipmentPM GetSingleDigitalPM(string id, int tenant, string cardId = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var shipment = repository.context
                                         .Shipments
                                         .Include("EntityStatus")
                                         .Include("ShipmentLevel")
                                         .Include("ShipmentType")
                                         .FirstOrDefault(a => a.Id == id
                                                              && a.Tenant == tenant);
                if (shipment != null)
                {
                    CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                    customFieldResolver.SetCustomFieldsValues("Shipment", tenant, new List<Shipment> { shipment }.Cast<object>().ToList());

                    var masterData = repository.context
                                               .ShipmentMasterDatas
                                               .FirstOrDefault(a => a.Id == shipment.MasterShipmentDataId);

                    ShipmentPM shipmentPM = new ShipmentPM();
                    shipmentPM.Tenant = shipment.Tenant;
                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true, false, cardId);
                    ShipmentPM securedPM = new ShipmentPM();

                    securedPM = Simplog.Server.Infrastructure.Helpers.SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);
                    MapShipmentComputedFields(securedPM, masterData);
                    return securedPM;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

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
                PartnerType = "Shipment.F.ShipperName",
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
                PartnerType = "Shipment.F.ConsigneeName",
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
                PartnerType = "Shipment.F.AgentName",
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
                PartnerType = "Shipment.F.ColoaderId",
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
                PartnerType = "Shipment.G.ConsigneeNotImporter",
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
                PartnerType = "Shipment.F.FreightForwarderId",
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
                PartnerType = "Shipment.F.Notify1Id",
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
                PartnerType = "Shipment.F.Notify2Id",
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
                PartnerType = "Shipment.G.ShipperNotExporter",
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
                PartnerType = "Shipment.G.CustomAgentExport",
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
                PartnerType = "Shipment.G.CustomAgentImport",
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
                PartnerType = "Shipment.F.CustomClearancePointId",
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
                PartnerType = "Shipment.F.ConsolidatorName",
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
                PartnerType = "Shipment.F.ReleasingAgentId",
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
                PartnerType = "Shipment.G.IssuingCarrierAgent",
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

        private string GetCountryNameForFromInlandDomestic(dynamic shipment)
        {
            if (shipment.InlandDomesticFromTypeCode == "PART")
            {
                return this.GetCountryNameByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return shipment.MainCarriageFromPortCountryName;
            }
            else if (shipment.InlandDomesticFromTypeCode == "CASL")
            {
                var res = this.GetCountryNameByCASLAddress(shipment.InlandDomesticFromCountryId, tenant);

                if (res != null)
                {
                    return res.EnglishName;
                }

                return "";
            }

            return null;
        }

        private string GetCountryNameForToInlandDomestic(dynamic shipment)
        {
            if (shipment.InlandDomesticToTypeCode == "PART")
            {
                return this.GetCountryNameByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (shipment.InlandDomesticToTypeCode == "PORT")
            {
                return shipment.MainCarriageToPortCountryName;
            }
            else if (shipment.InlandDomesticToTypeCode == "CASL")
            {
                var res = this.GetCountryNameByCASLAddress(shipment.InlandDomesticToCountryId, tenant);

                if (res != null)
                {
                    return res.EnglishName;
                }

                return "";

            }

            return null;
        }

        private string GetCountryNameByPartnerId(string addressId)
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
            if (!string.IsNullOrEmpty(partnerAddress.Country?.EnglishName))
            {
                country = partnerAddress.Country?.EnglishName;
            }

            return country;
        }

        private Country GetCountryNameByCASLAddress(string countryId, int tenant)
        {
            Country myResult = null;

            if (!string.IsNullOrEmpty(countryId))
            {
                myResult = CountryRepository.GetSingleCountry(countryId, tenant, false);
            }

            return myResult;
        }

        private dynamic SetPermissonFieldValue(string objectTbaleName, string fieldCode, dynamic filedValue)
        {
            if (blockedFields.ContainsKey(objectTbaleName) && blockedFields[objectTbaleName].Contains($"{fieldCode}"))
            {
                return filedValue = null;
            }

            return filedValue;
        }

        #endregion Private methods

        #region Routing 

        public List<ShipmentRoutingLeg> GetDigitalShipmentRoutingLegs(string shipmentId, int tenant, Dictionary<string, List<string>> blockedFields)
        {
            InitializeServices(tenant);
            this.blockedFields = blockedFields;
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
                Title = "Shipment.G.MainRouteInformation",
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
                DichargeDate = shipment.MainCarriageATA != null ?
                               SetPermissonFieldValue("Shipment", "Shipment.MainCarriageATA", shipment.MainCarriageATA)
                               :
                               SetPermissonFieldValue("Shipment", "Shipment.MainCarriageETA", shipment.MainCarriageETA),
                LoadingDate = shipment.MainCarriageATD != null ?
                               SetPermissonFieldValue("Shipment", "Shipment.MainCarriageATD", shipment.MainCarriageATD)
                               :
                               SetPermissonFieldValue("Shipment", "Shipment.MainCarriageETD", shipment.MainCarriageETD),
            };
        }

        private string GetFromAddressForInlandDomestic(ShipmentPM shipment)
        {
            if (CheckIsPermissonField("Shipment", "Shipment.InlandDomesticFromTypeCode")
                 && CheckIsPermissonField("Shipment", "Shipment.MainCarriageFromAddressId")
                 && shipment.InlandDomesticFromTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (CheckIsPermissonField("Shipment", "Shipment.InlandDomesticFromTypeCode")
                     && shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return (CheckIsPermissonField("Shipment", "Shipment.MainCarriageFromPortCode") ? $"{shipment.MainCarriageFromPortCode}, " : "") 
                        + (CheckIsPermissonField("Shipment", "Shipment.MainCarriageFromPortCountryCode") ? shipment.MainCarriageFromPortCountryCode : "");
            }
            else if (CheckIsPermissonField("Shipment", "Shipment.InlandDomesticFromTypeCode")
                     && CheckIsPermissonField("Shipment", "Shipment.InlandDomesticFromCountryId")
                     && CheckIsPermissonField("Shipment", "Shipment.InlandDomesticFromCity")
                     && shipment.InlandDomesticFromTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(shipment.InlandDomesticFromCountryId, shipment.InlandDomesticFromCity);
            }

            return null;
        }

        private string GetToAddressForInlandDomestic(ShipmentPM shipment)
        {
            if (CheckIsPermissonField("Shipment", "Shipment.InlandDomesticToTypeCode")
                && CheckIsPermissonField("Shipment", "Shipment.MainCarriageToAddressId")
                && shipment.InlandDomesticToTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (CheckIsPermissonField("Shipment", "Shipment.InlandDomesticToTypeCode")
                     && shipment.InlandDomesticToTypeCode == "PORT")
            {
                return (CheckIsPermissonField("Shipment", "Shipment.MainCarriageToPortCode") ? $"{shipment.MainCarriageToPortCode}, " : "")
                        + (CheckIsPermissonField("Shipment", "Shipment.MainCarriageToPortCountryCode") ? shipment.MainCarriageToPortCountryCode : "");
            }
            else if (CheckIsPermissonField("Shipment", "Shipment.InlandDomesticToTypeCode")
                     && CheckIsPermissonField("Shipment", "Shipment.InlandDomesticToCountryId")
                     && CheckIsPermissonField("Shipment", "Shipment.InlandDomesticToCity")
                     && shipment.InlandDomesticToTypeCode == "CASL")
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

            // Pick Up 
            ShipmentRoutingLeg pickUp = AddPickUpLeg(shipment);
            if (pickUp != null)
            {
                routingLegs.Add(pickUp);
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

            // Delivery
            ShipmentRoutingLeg delivery = AddDeliveryLeg(shipment);
            if (delivery != null)
            {
                routingLegs.Add(delivery);
            }

            return routingLegs;
        }

        private ShipmentRoutingLeg AddMainRouteInformation(ShipmentPM shipment)
        {
            var master = shipment.TransportModeId == "A" ?
                        (shipment.AirlinePrefix != null && shipment.Master != null
                                 ? (CheckIsPermissonField("Shipment", "Shipment.AirlinePrefix") ? $"{shipment.AirlinePrefix}-" : "") + SetPermissonFieldValue("Shipment", "Shipment.Master", shipment.Master)
                                 : SetPermissonFieldValue("Shipment", "Shipment.Master", shipment.Master))
                        : SetPermissonFieldValue("Shipment", "Shipment.Master", shipment.Master);

            // Main Info
            ShipmentRoutingLeg mainRouteInformation = new MainRouteInformation()
            {
                Title = "Shipment.G.MainRouteInformation",
                LegHeader = "MainRoute",
                Master =  master,
                MasterLabel = GetMasterTextCode(shipment),
                LoadingPortLabel = shipment.TransportModeId == "A" ? "Shipment.G.GatewayLable" : "Shipment.G.PortOfLoadingLable",
                DischargePortLabel = shipment.TransportModeId == "A" ? "Shipment.G.DestinationLable" : "Shipment.G.PortOfDischargeLable",
                LoadingPort = GetLoadingPort(shipment),
                DischargePort = GetDischargePort(shipment),
                TransitTime = GetTransitTime(shipment),
            };

            return mainRouteInformation;
        }

        private string GetLoadingPort(ShipmentPM shipment)
        {
            string loadingPort;
            if (!string.IsNullOrEmpty(shipment.PreCarriageCarrierId))
            {
                loadingPort = (CheckIsPermissonField("Shipment", "Shipment.PreCarriageFromPortName") ? $"{ shipment.PreCarriageFromPortName }, ": "")
                             + SetPermissonFieldValue("Shipment", "Shipment.PreCarriageFromPortCountryCode", shipment.PreCarriageFromPortCountryCode);
            }
            else
            {
                loadingPort = (CheckIsPermissonField("Shipment", "Shipment.MainCarriageFromPortName") ? $"{ shipment.MainCarriageFromPortName }, " : "")
                              + SetPermissonFieldValue("Shipment", "Shipment.MainCarriageFromPortCountryCode", shipment.MainCarriageFromPortCountryCode);
            }

            return loadingPort;
        }

        private string GetDischargePort(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.OnCarriageFromPortId))
            {
                return (CheckIsPermissonField("Shipment", "Shipment.OnCarriageToPortName") ? $"{ shipment.OnCarriageToPortName }, " : "")
                        + SetPermissonFieldValue("Shipment", "Shipment.OnCarriageToPortCountryCode", shipment.OnCarriageToPortCountryCode);
            }
            else if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return (CheckIsPermissonField("Shipment", "Shipment.Transshipment3ToPortName") ? $"{ shipment.Transshipment3ToPortName }, " : "")
                        + SetPermissonFieldValue("Shipment", "Shipment.Transshipment3ToPortCountryCode", shipment.Transshipment3ToPortCountryCode);
            }
            else if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return (CheckIsPermissonField("Shipment", "Shipment.Transshipment2ToPortName") ? $"{ shipment.Transshipment2ToPortName }, " : "")
                       + SetPermissonFieldValue("Shipment", "Shipment.Transshipment2ToPortCountryCode", shipment.Transshipment2ToPortCountryCode);
            }
            else if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return (CheckIsPermissonField("Shipment", "Shipment.Transshipment1ToPortName") ? $"{ shipment.Transshipment1ToPortName }, " : "")
                       + SetPermissonFieldValue("Shipment", "Shipment.Transshipment1ToPortCountryCode", shipment.Transshipment1ToPortCountryCode);

            }
            else if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return (CheckIsPermissonField("Shipment", "Shipment.MainCarriageToPortName") ? $"{ shipment.MainCarriageToPortName }, " : "")
                       + SetPermissonFieldValue("Shipment", "Shipment.MainCarriageToPortCountryCode", shipment.MainCarriageToPortCountryCode);
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
                return SetPermissonFieldValue("Shipment", "Shipment.PreCarriageATD", shipment.PreCarriageATD);
            }

            if (shipment.PreCarriageETD != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.PreCarriageETD", shipment.PreCarriageETD);
            }

            if (shipment.MainCarriageATD != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.MainCarriageATD", shipment.MainCarriageATD);
            }

            if (shipment.MainCarriageETD != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.MainCarriageETD", shipment.MainCarriageETD);
            }

            return null;
        }

        private DateTime? GetPortOfDischargeDate(ShipmentPM shipment)
        {
            if (shipment.OnCarriageATA != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.OnCarriageATA", shipment.OnCarriageATA);
            }

            if (shipment.OnCarriageETA != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.OnCarriageETA", shipment.OnCarriageETA);
            }

            if (shipment.MainCarriageATA != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.MainCarriageATA", shipment.MainCarriageATA);
            }

            if (shipment.MainCarriageETA != null)
            {
                return SetPermissonFieldValue("Shipment", "Shipment.MainCarriageETA", shipment.MainCarriageETA);
            }

            return null;
        }

        private ShipmentRoutingLeg AddPickUpLeg(ShipmentPM shipment)
        {
            var shipmentPickUpDeliveries = repository.context
                                                    .ShipmentPickUpDeliveries
                                                    .Include("FromAddressCountry")
                                                    .Include("ToAddressCountry")
                                                    .Where(a => a.ShipmentId == shipment.Id);

            var firstPickup = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "PICK")
                                                       .OrderBy(s => s.PickUpDeliveryNumber)
                                                       .FirstOrDefault();
           
            if (firstPickup == null)
            {
                return null;
            }

            CardRepository cardRepository = new CardRepository(tenant);
            Card trucker = cardRepository.GetSingleCardWithoutInclude(firstPickup.CarrierId, tenant);
            ShipmentRoutingLeg pickUp = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.PickupInformation",
                LegHeader = "Pickup",
                FromPort = "",
                ToPort = "",
                TransportMode = GetTransportModeName("I"),
                DepartureDate = firstPickup.ATD != null ? 
                                SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD", firstPickup.ATD)  
                                :
                                SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD", firstPickup.ETD),
                ArrivalDate = firstPickup.ATA != null ?
                              SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA", firstPickup.ATA)
                              :
                              SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA", firstPickup.ETA),
                ArrivalDateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && firstPickup.ATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && firstPickup.ETA != null 
                                     ? "ETA" 
                                     : null),
                DepartureDateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && firstPickup.ATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && firstPickup.ETD != null 
                                        ? "ETD" 
                                        : null),
                Carrier = SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.Id", trucker?.EnglishName),
                CarrierNumber = SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.PickUpDeliveryNumber", firstPickup.CarrierNumber),
                CarrierNumberLabel = "ShipmentPickUpDelivery.PickUpDeliveryNumber",
                CarrierLabel = "ShipmentPickUpDelivery.Id",
            };
            FillRountingPickUpDeliveryFromPortAndCountry(pickUp, firstPickup, shipment.Tenant);
            FillRountingPickUpDeliveryToPortAndCountry(pickUp, firstPickup, shipment.Tenant);
            return pickUp;
        }

        private ShipmentRoutingLeg AddPreCarriageLegLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg preCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.PreCarrigeInformation",
                LegHeader = "PreCarriage",
                FromPort = (CheckIsPermissonField("Shipment", "Shipment.PreCarriageFromPortName") ? $"{ shipment.PreCarriageFromPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.PreCarriageFromPortCountryCode", shipment.PreCarriageFromPortCountryCode),
                ToPort = (CheckIsPermissonField("Shipment", "Shipment.PreCarriageToPortName") ? $"{ shipment.PreCarriageToPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.PreCarriageToPortCountryCode", shipment.PreCarriageToPortCountryCode),
                TransportMode = GetTransportModeName(shipment.PreCarriageTransportModeId),
                DepartureDate = shipment.PreCarriageATD != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.PreCarriageATD", shipment.PreCarriageATD)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.PreCarriageETD", shipment.PreCarriageETD),
                DepartureDateType = CheckIsPermissonField("Shipment", "Shipment.PreCarriageATD") && shipment.PreCarriageATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("Shipment", "Shipment.PreCarriageETD") && shipment.PreCarriageETD != null 
                                        ? "ETD" 
                                        : null),
                ArrivalDate = shipment.PreCarriageATA != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.PreCarriageATA", shipment.PreCarriageATA)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.PreCarriageETA", shipment.PreCarriageETA),
                ArrivalDateType = CheckIsPermissonField("Shipment", "Shipment.PreCarriageATA") && shipment.PreCarriageATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("Shipment", "Shipment.PreCarriageETA") && shipment.PreCarriageETA != null 
                                      ? "ETA" 
                                      : null),
                Carrier = SetPermissonFieldValue("Shipment", "Shipment.PreCarriageCarrierName", shipment.PreCarriageCarrierName),
                CarrierLabel = "Shipment.PreCarriageCarrierName",
                CarrierNumberLabel = "Shipment.PreCarriageCarrierNumber",
                CarrierNumber = SetPermissonFieldValue("Shipment", "Shipment.PreCarriageCarrierNumber", shipment.PreCarriageCarrierNumber),
                VesselName = shipment.PreCarriageTransportModeId == "O" ? 
                                SetPermissonFieldValue("Shipment", "Shipment.PreCarriageVesselName", shipment.PreCarriageVesselName)  
                                : null,
            };

            return preCarriageLeg;
        }

        private ShipmentRoutingLeg AddMainCarriageLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg mainCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.MainCarrigeInformation",
                LegHeader = "MainCarriage",
                FromPort = (CheckIsPermissonField("Shipment", "Shipment.MainCarriageFromPortName") ? $"{ shipment.MainCarriageFromPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.MainCarriageFromPortCountryCode", shipment.MainCarriageFromPortCountryCode),
                ToPort = (CheckIsPermissonField("Shipment", "Shipment.MainCarriageToPortName") ? $"{ shipment.MainCarriageToPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.MainCarriageToPortCountryCode", shipment.MainCarriageToPortCountryCode),
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" 
                             ? SetPermissonFieldValue("Shipment", "Shipment.MainCarriageVesselName", shipment.MainCarriageVesselName)
                             : null,
                DepartureDate = shipment.MainCarriageATD != null 
                                ? SetPermissonFieldValue("Shipment", "Shipment.MainCarriageATD", shipment.MainCarriageATD) 
                                : SetPermissonFieldValue("Shipment", "Shipment.MainCarriageETD", shipment.MainCarriageETD),
                DepartureDateType = CheckIsPermissonField("Shipment", "Shipment.MainCarriageATD") && shipment.MainCarriageATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("Shipment", "Shipment.MainCarriageETD") && shipment.MainCarriageETD != null 
                                       ? "ETD" 
                                       : null),
                ArrivalDate = CheckIsPermissonField("Shipment", "Shipment.MainCarriageATA") && shipment.MainCarriageATA != null 
                             ? shipment.MainCarriageATA 
                             : (CheckIsPermissonField("Shipment", "Shipment.MainCarriageETA")
                                && shipment.MainCarriageETA != null 
                                ? shipment.MainCarriageETA : null),
                ArrivalDateType = CheckIsPermissonField("Shipment", "Shipment.MainCarriageATA") &&  shipment.MainCarriageATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("Shipment", "Shipment.MainCarriageETA") && shipment.MainCarriageETA != null 
                                    ? "ETA" 
                                    : null),
                TransportMode = GetTransportModeName(shipment.TransportModeId),
            };

            if (CheckIsPermissonField("Shipment", "Shipment.MainCarriageCarrierName"))
            {
                mainCarriageLeg.Carrier = shipment.MainCarriageCarrierName;
            }

            if (CheckIsPermissonField("Shipment", "Shipment.MainCarriageCarrierNumber"))
            {
                mainCarriageLeg.CarrierNumber = shipment.MainCarriageCarrierNumber;
            }

            return mainCarriageLeg;
        }

        private ShipmentRoutingLeg AddTransshipment1Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment1 = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.Transshipment1Information",
                LegHeader = "Transshipment1",
                FromPort = (CheckIsPermissonField("Shipment", "Shipment.Transshipment1FromPortName") ? $"{ shipment.Transshipment1FromPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.Transshipment1FromPortCountryCode", shipment.Transshipment1FromPortCountryCode),
                ToPort = (CheckIsPermissonField("Shipment", "Shipment.Transshipment1ToPortName") ? $"{ shipment.Transshipment1ToPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.Transshipment1ToPortCountryCode", shipment.Transshipment1ToPortCountryCode),
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment1VesselName", shipment.Transshipment1VesselName)
                                : null,
                DepartureDate = shipment.Transshipment1ATD != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment1ATD", shipment.Transshipment1ATD)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment1ETD", shipment.Transshipment1ETD),
                ArrivalDate = shipment.Transshipment1ATA != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment1ATA", shipment.Transshipment1ATA)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment1ETA", shipment.Transshipment1ETA),

                DepartureDateType = CheckIsPermissonField("Shipment", "Shipment.Transshipment1ATD") && shipment.Transshipment1ATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("Shipment", "Shipment.Transshipment1ETD") && shipment.Transshipment1ETD != null 
                                       ? "ETD" 
                                       : null),
                ArrivalDateType = CheckIsPermissonField("Shipment", "Shipment.Transshipment1ATA") && shipment.Transshipment1ATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("Shipment", "Shipment.Transshipment1ETA") && shipment.Transshipment1ETA != null 
                                     ? "ETA" 
                                     : null),
                TransportMode = GetTransportModeName(shipment.TransportModeId),
            };

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment1CarrierName"))
            {
                transshipment1.Carrier = shipment.Transshipment1CarrierName;
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment1CarrierNumber"))
            {
                transshipment1.CarrierNumber = shipment.Transshipment1CarrierNumber;
            }

            return transshipment1;
        }

        private ShipmentRoutingLeg AddTransshipment2Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment2 = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.Transshipment2Information",
                LegHeader = "Transshipment2",
                FromPort = (CheckIsPermissonField("Shipment", "Shipment.Transshipment2FromPortName") ? $"{ shipment.Transshipment2FromPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.Transshipment2FromPortCountryCode", shipment.Transshipment2FromPortCountryCode),
                ToPort = (CheckIsPermissonField("Shipment", "Shipment.Transshipment2ToPortName") ? $"{ shipment.Transshipment2ToPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.Transshipment2ToPortCountryCode", shipment.Transshipment2ToPortCountryCode),
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment2VesselName", shipment.Transshipment2VesselName)
                                : null,
                DepartureDate = shipment.Transshipment2ATD != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment2ATD", shipment.Transshipment2ATD)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment2ETD", shipment.Transshipment2ETD),
                ArrivalDate = shipment.Transshipment2ATA != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment2ATA", shipment.Transshipment2ATA)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment2ETA", shipment.Transshipment2ETA),
                DepartureDateType = CheckIsPermissonField("Shipment", "Shipment.Transshipment2ATD") && shipment.Transshipment2ATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("Shipment", "Shipment.Transshipment2ETD") && shipment.Transshipment2ETD != null 
                                       ? "ETD" 
                                       : null),
                ArrivalDateType = CheckIsPermissonField("Shipment", "Shipment.Transshipment2ATA") && shipment.Transshipment2ATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("Shipment", "Shipment.Transshipment2ETA") && shipment.Transshipment2ETA != null 
                                     ? "ETA" 
                                     : null),
                TransportMode = GetTransportModeName(shipment.TransportModeId),
            };

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment2CarrierName"))
            {
                transshipment2.Carrier = shipment.Transshipment2CarrierName;
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment2CarrierNumber"))
            {
                transshipment2.CarrierNumber = shipment.Transshipment2CarrierNumber;
            }

            return transshipment2;
        }

        private ShipmentRoutingLeg AddTransshipment3Leg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg transshipment3 = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.Transshipment3Information",
                LegHeader = "Transshipment3",
                FromPort = (CheckIsPermissonField("Shipment", "Shipment.Transshipment3FromPortName") ? $"{ shipment.Transshipment3FromPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.Transshipment3FromPortCountryCode", shipment.Transshipment3FromPortCountryCode),
                ToPort = (CheckIsPermissonField("Shipment", "Shipment.Transshipment3ToPortName") ? $"{ shipment.Transshipment3ToPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.Transshipment3ToPortCountryCode", shipment.Transshipment3ToPortCountryCode),
                CarrierLabel = GetCarrierTextCode(shipment),
                CarrierNumberLabel = GetCarrierNumberTextCode(shipment),
                VesselName = shipment.TransportModeId == "O" ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment3VesselName", shipment.Transshipment3VesselName)
                                : null,
                DepartureDate = shipment.Transshipment3ATD != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment3ATD", shipment.Transshipment3ATD)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment3ETD", shipment.Transshipment3ETD),
                ArrivalDate = shipment.Transshipment3ATA != null ?
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment3ATA", shipment.Transshipment3ATA)
                                :
                                SetPermissonFieldValue("Shipment", "Shipment.Transshipment3ETA", shipment.Transshipment3ETA),
                DepartureDateType = CheckIsPermissonField("Shipment", "Shipment.Transshipment3ATD") && shipment.Transshipment3ATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("Shipment", "Shipment.Transshipment3ETD") && shipment.Transshipment3ETD != null 
                                      ? "ETD" 
                                      : null),
                ArrivalDateType = CheckIsPermissonField("Shipment", "Shipment.Transshipment3ATA") && shipment.Transshipment3ATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("Shipment", "Shipment.Transshipment3ETA") && shipment.Transshipment3ETA != null 
                                  ? "ETA" 
                                  : null),
                TransportMode = GetTransportModeName(shipment.TransportModeId),
            };

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment3CarrierName"))
            {
                transshipment3.Carrier = shipment.Transshipment3CarrierName;
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment3CarrierNumber"))
            {
                transshipment3.CarrierNumber = shipment.Transshipment3CarrierNumber;
            }

            return transshipment3;
        }

        private ShipmentRoutingLeg AddOnCarriageLegLeg(ShipmentPM shipment)
        {
            ShipmentRoutingLeg onCarriageLeg = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.OnCarrigeInformation",
                LegHeader = "OnCarriage",
                FromPort = (CheckIsPermissonField("Shipment", "Shipment.OnCarriageFromPortName") ? $"{ shipment.OnCarriageFromPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.OnCarriageFromPortCountryCode", shipment.OnCarriageFromPortCountryCode),
                ToPort = (CheckIsPermissonField("Shipment", "Shipment.OnCarriageToPortName") ? $"{ shipment.OnCarriageToPortName }, " : "")
                           + SetPermissonFieldValue("Shipment", "Shipment.OnCarriageToPortCountryCode", shipment.OnCarriageToPortCountryCode),
                TransportMode = GetTransportModeName(shipment.OnCarriageTransportModeId),
                DepartureDate = CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageATD") && shipment.OnCarriageATD != null 
                                ? shipment.OnCarriageATD
                                : CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageETD") ? shipment.OnCarriageETD : null,
                ArrivalDate = CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageATA") && shipment.OnCarriageATA != null 
                                ? shipment.OnCarriageATA
                                : CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageETA") ? shipment.OnCarriageETA : null,
                DepartureDateType = CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageATD") && shipment.OnCarriageATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageETD") && shipment.OnCarriageETD != null 
                                       ? "ETD" 
                                       : null),
                ArrivalDateType = CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageATA") && shipment.OnCarriageATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("ShipmentPackage", "ShipmentPackage.OnCarriageETA") && shipment.OnCarriageETA != null 
                                     ? "ETA" 
                                     : null),
                Carrier = SetPermissonFieldValue("Shipment", "Shipment.OnCarriageCarrierName", shipment.OnCarriageCarrierName),
                CarrierNumber = SetPermissonFieldValue("Shipment", "Shipment.OnCarriageCarrierNumber", shipment.OnCarriageCarrierNumber),
                CarrierLabel = "Shipment.OnCarriageCarrierName",
                CarrierNumberLabel = "Shipment.OnCarriageCarrierNumber",
                VesselName = shipment.OnCarriageTransportModeId == "O" 
                             ? SetPermissonFieldValue("Shipment", "Shipment.OnCarriageVesselName", shipment.OnCarriageVesselName) 
                             : null,
            };

            return onCarriageLeg;
        }

        private ShipmentRoutingLeg AddDeliveryLeg(ShipmentPM shipment)
        {
            var shipmentPickUpDeliveries = repository.context
                                                     .ShipmentPickUpDeliveries
                                                     .Include("FromAddressCountry")
                                                     .Include("ToAddressCountry")
                                                     .Where(a => a.ShipmentId == shipment.Id);

            var finalDelivery = shipmentPickUpDeliveries?
                                .Where(d => d.PickUpDeliveryTypeCode == "DELV")
                                .OrderByDescending(s => s.PickUpDeliveryNumber)
                                .FirstOrDefault();
       
            if (finalDelivery == null)
            {
                return null;
            }

            CardRepository cardRepository = new CardRepository(tenant);
            Card trucker = cardRepository.GetSingleCardWithoutInclude(finalDelivery.CarrierId, tenant);
            ShipmentRoutingLeg delivery = new ShipmentRoutingLeg()
            {
                Title = "Shipment.G.DeliveryInformation",
                LegHeader = "Delivery",
                FromPort = "",
                ToPort = "",
                TransportMode = GetTransportModeName("I"),
                DepartureDate = finalDelivery.ATD != null ?
                                SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD", finalDelivery.ATD)
                                :
                                SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD", finalDelivery.ETD),
                ArrivalDate = finalDelivery.ATA != null ?
                              SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA", finalDelivery.ATA)
                              :
                              SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA", finalDelivery.ETA),
                DepartureDateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && finalDelivery.ATD != null 
                                    ? "ATD" 
                                    : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") &&  finalDelivery.ETD != null 
                                        ? "ETD" 
                                        : null),
                ArrivalDateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                                  ? "ATA" 
                                  : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && finalDelivery.ETA != null 
                                     ? "ETA" 
                                     : null),
                Carrier = SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.Id", trucker?.EnglishName),
                CarrierNumber = SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.PickUpDeliveryNumber", finalDelivery.CarrierNumber),
                CarrierNumberLabel = "ShipmentPickUpDelivery.PickUpDeliveryNumber",
                CarrierLabel = "ShipmentPickUpDelivery.Id",
            };

            FillRountingPickUpDeliveryFromPortAndCountry(delivery, finalDelivery, shipment.Tenant);
            FillRountingPickUpDeliveryToPortAndCountry(delivery, finalDelivery, shipment.Tenant);
            return delivery;
        }

        private void FillRountingPickUpDeliveryToPortAndCountry(ShipmentRoutingLeg leg, ShipmentPickUpDelivery pickUpDelivery, int tenant)
        {
            switch (pickUpDelivery.PickUpDeliveryToTypeCode)
            {
                case "PART":
                    {
                        if (SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToPartnerCardId", pickUpDelivery?.ToPartnerCardId) != null 
                            && !string.IsNullOrEmpty(pickUpDelivery.ToPartnerCardId))
                        {
                            if (SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressId", pickUpDelivery?.ToAddressId) != null 
                                && !string.IsNullOrEmpty(pickUpDelivery.ToAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(pickUpDelivery.ToAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    leg.ToPort = myPartnerAddress.City + ", " + myPartnerAddress.Country?.Code;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(pickUpDelivery.ToPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    leg.ToPort = myPartnerAddress.City + ", " + myPartnerAddress.Country?.Code;
                                }
                            }
                        }

                        break;
                    }

                case "PORT":
                    {
                        if (SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToPortId", pickUpDelivery?.ToPortId) != null 
                            && !string.IsNullOrEmpty(pickUpDelivery.ToPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, pickUpDelivery.ToPortId, true);
                            if (myPort != null)
                            {
                                leg.ToPort = myPort.EnglishName + ", " + myPort.CountryCode;
                            }
                        }

                        break;
                    }

                case "CASL":
                    {
                        leg.ToPort = SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCity", pickUpDelivery?.ToAddressCity) != null 
                                      ? $"{pickUpDelivery.ToAddressCity}, " 
                                          + SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCountry", pickUpDelivery?.ToAddressCountry) != null 
                                            ? pickUpDelivery.ToAddressCountry?.Code
                                            : ""
                                      : "";
                        break;
                    }
            }
        }

        private void FillRountingPickUpDeliveryFromPortAndCountry(ShipmentRoutingLeg leg, ShipmentPickUpDelivery pickUpDelivery, int tenant)
        {
            switch (pickUpDelivery.PickUpDeliveryFromTypeCode)
            {
                case "PART":
                    {
                        if (SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromPartnerCardId", pickUpDelivery?.FromPartnerCardId) != null 
                             && !string.IsNullOrEmpty(pickUpDelivery.FromPartnerCardId))
                        {
                            if (SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromAddressId", pickUpDelivery?.FromAddressId) != null 
                                && !string.IsNullOrEmpty(pickUpDelivery.FromAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(pickUpDelivery.FromAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    leg.FromPort = myPartnerAddress.City + ", " + myPartnerAddress.Country?.Code;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(pickUpDelivery.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    leg.FromPort = myPartnerAddress.City + ", " + myPartnerAddress.Country?.Code;
                                }
                            }
                        }
                        break;
                    }

                case "PORT":
                    {
                        if (SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromPortId", pickUpDelivery?.FromPortId) != null
                                && !string.IsNullOrEmpty(pickUpDelivery.FromPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, pickUpDelivery.FromPortId, true);
                            if (myPort != null)
                            {
                                leg.FromPort = myPort.EnglishName + ", " + myPort.CountryCode;
                            }
                        }
                        break;
                    }

                case "CASL":
                    {
                        leg.FromPort = SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCity", pickUpDelivery?.ToAddressCity) != null
                                      ? $"{pickUpDelivery.ToAddressCity}, "
                                          + SetPermissonFieldValue("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCountry", pickUpDelivery?.ToAddressCountry) != null
                                            ? pickUpDelivery.ToAddressCountry?.Code
                                            : ""
                                      : "";
                        break;
                    }
            }
        }

        private string GetMasterTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Shipment.O.Overview.MAWB";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Shipment.G.OBL";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Shipment.G.CMR/RWB#";
            }

            return null;
        }

        private string GetVerticalTimeLineMasterTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Shipment.MAWB";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Shipment.OBL";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Shipment.CMRRWB";
            }

            return null;
        }

        private string GetCarrierTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Shipment.G.Airline";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Shipment.G.Shippingline";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Shipment.TruckerId";
            }

            return null;
        }

        private string GetCarrierNumberTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Shipment.G.FlightNumber";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Shipment.G.VoyageNumber";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Shipment.F.TruckNumber";
            }

            return null;
        }

        private string GetVerticalTimelineCarrierNumberTextCode(ShipmentPM shipment)
        {
            if (shipment.TransportModeId == "A")
            {
                return "Shipment.O.FilghtNo";
            }

            if (shipment.TransportModeId == "O")
            {
                return "Shipment.G.VoyageNo";
            }

            if (shipment.TransportModeId == "I")
            {
                return "Shipment.F.TruckNumber";
            }

            return null;
        }

        private string GetTransportModeName(string transportModeId)
        {
            if (transportModeId == "A")
            {
                return "Dashboard.G.AirLabel";
            }

            if (transportModeId == "O")
            {
                return "Dashboard.G.OceanLabel";
            }

            if (transportModeId == "I")
            {
                return "Dashboard.G.InlandLabel";
            }

            return null;
        }

        #endregion Routing

        #region Shipment Horizontal TimeLine
        public List<dynamic> BuildShipmentListWithTimeLine(List<dynamic> entityLists, int tenant, Dictionary<string, List<string>> dictBlockedFeilds)
        {
            this.blockedFields = dictBlockedFeilds;

            InitializeServices(tenant);

            var res = new List<dynamic>();

            foreach (var item in entityLists)
            {
                res.Add(this.FillShipmnetTimeLine(item));
            }

            return res;
        }

        public bool DoesPropertyExistInDynamic(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }

        private dynamic FillShipmnetTimeLine(dynamic shipment)
        {
            bool isInlandDomesticShipment = DoesPropertyExistInDynamic(shipment, "DirectionId")
                                            && DoesPropertyExistInDynamic(shipment, "TransportModeId") 
                                            ? (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                                            : false;

            dynamic expando = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(shipment));

            if (isInlandDomesticShipment)
            {
                expando.TimeLineData = this.FillShipmnetTimeLineForInlandDomesticShipment(shipment);
            }
            else
            {
                expando.TimeLineData = this.FillShipmnetTimeLineForShipment(shipment);
            }

            return expando;
        }

        private TimeLineData FillShipmnetTimeLineForInlandDomesticShipment(dynamic shipment)
        {
            TimeLineData timeLineData = new TimeLineData();
            this.FillMainCarraigeFromTimeLineForInlandDomesticShipment(timeLineData, shipment);
            this.FillMainCarraigeToTimeLineForInlandDomesticShipment(timeLineData, shipment);
            return timeLineData;
        }

        private void FillMainCarraigeFromTimeLineForInlandDomesticShipment(TimeLineData timeLineData, dynamic shipment)
        {
            timeLineData.MainCarriageFrom = new TimeLineStop()
            {
                City = this.GetCityForFromInlandDomestic(shipment),
                CountryCode = this.GetCountryCodeForFromInlandDomestic(shipment),
                Date = DoesPropertyExistInDynamic(shipment, "MainCarriageATD") && shipment.MainCarriageATD != null 
                       ? shipment.MainCarriageATD 
                       : DoesPropertyExistInDynamic(shipment, "MainCarriageETD") 
                         ? shipment.MainCarriageETD 
                         : null,
                DateType = DoesPropertyExistInDynamic(shipment, "MainCarriageATD") && shipment.MainCarriageATD != null 
                           ? "Actual" 
                           : (DoesPropertyExistInDynamic(shipment, "MainCarriageETD") && shipment.MainCarriageETD != null ? "Estimated" : null),
            };
        }

        private void FillMainCarraigeToTimeLineForInlandDomesticShipment(TimeLineData timeLineData, dynamic shipment)
        {
            timeLineData.MainCarriageTo = new TimeLineStop()
            {
                City = this.GetCityForToInlandDomestic(shipment),
                CountryCode = this.GetCountryCodeForToInlandDomestic(shipment),
                Date = DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationATA") && shipment.MainCarriageFinalDestinationATA != null 
                       ? shipment.MainCarriageFinalDestinationATA 
                       : DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationETA") 
                         ?  shipment.MainCarriageFinalDestinationETA 
                         : null,
                DateType = DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationATA") && shipment.MainCarriageFinalDestinationATA != null 
                           ? "Actual" 
                           : (DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationETA") 
                              ? shipment.MainCarriageFinalDestinationETA != null ? "Estimated" : null
                              : null)
            };
        }

        private string GetCityForFromInlandDomestic(dynamic shipment)
        {
            if (DoesPropertyExistInDynamic(shipment, "InlandDomesticFromTypeCode") 
                && DoesPropertyExistInDynamic(shipment, "MainCarriageFromAddressId") 
                && shipment.InlandDomesticFromTypeCode == "PART")
            {
                return this.GetCityByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticFromTypeCode") 
                     && DoesPropertyExistInDynamic(shipment, "MainCarriageFromPortCode") 
                     && shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return shipment.MainCarriageFromPortCode;
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticFromTypeCode") 
                      && DoesPropertyExistInDynamic(shipment, "InlandDomesticFromCity") 
                      &&  shipment.InlandDomesticFromTypeCode == "CASL")
            {
                return shipment.InlandDomesticFromCity;
            }

            return null;
        }

        private string GetCityForToInlandDomestic(dynamic shipment)
        {
            if (DoesPropertyExistInDynamic(shipment, "InlandDomesticToTypeCode") 
                 && DoesPropertyExistInDynamic(shipment, "MainCarriageToAddressId") 
                 &&  shipment.InlandDomesticToTypeCode == "PART")
            {
                return this.GetCityByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticToTypeCode") 
                     && DoesPropertyExistInDynamic(shipment, "MainCarriageToPortCode") 
                     &&  shipment.InlandDomesticToTypeCode == "PORT")
            {
                return shipment.MainCarriageToPortCode;
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticToTypeCode")
                     && DoesPropertyExistInDynamic(shipment, "InlandDomesticToCity")
                     && shipment.InlandDomesticToTypeCode == "CASL")
            {
                return shipment.InlandDomesticToCity;
            }

            return null;
        }

        private string GetCountryCodeForFromInlandDomestic(dynamic shipment)
        {
            if (DoesPropertyExistInDynamic(shipment, "InlandDomesticFromTypeCode") && DoesPropertyExistInDynamic(shipment, "MainCarriageFromAddressId") &&  shipment.InlandDomesticFromTypeCode == "PART")
            {
                return this.GetCountryCodeByPartnerId(shipment.MainCarriageFromAddressId);
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticFromTypeCode") && DoesPropertyExistInDynamic(shipment, "MainCarriageFromPortCountryCode") &&  shipment.InlandDomesticFromTypeCode == "PORT")
            {
                return shipment.MainCarriageFromPortCountryCode;
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticFromTypeCode") && DoesPropertyExistInDynamic(shipment, "InlandDomesticFromCountryId") &&  shipment.InlandDomesticFromTypeCode == "CASL")
            {
                var res = this.GetCountryCodeByCASLAddress(shipment.InlandDomesticFromCountryId, tenant);
                
                if (res != null)
                {
                    return res.Code;
                }

                return "";
            }

            return null;
        }

        private string GetCountryCodeForToInlandDomestic(dynamic shipment)
        {
            if (DoesPropertyExistInDynamic(shipment, "InlandDomesticToTypeCode") 
                && DoesPropertyExistInDynamic(shipment, "MainCarriageToAddressId")
                &&  shipment.InlandDomesticToTypeCode == "PART")
            {
                return this.GetCountryCodeByPartnerId(shipment.MainCarriageToAddressId);
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticToTypeCode") 
                     && DoesPropertyExistInDynamic(shipment, "MainCarriageToPortCountryCode")
                     && shipment.InlandDomesticToTypeCode == "PORT")
            {
                return shipment.MainCarriageToPortCountryCode;
            }
            else if (DoesPropertyExistInDynamic(shipment, "InlandDomesticToTypeCode") 
                     && DoesPropertyExistInDynamic(shipment, "InlandDomesticToCountryId") 
                     &&  shipment.InlandDomesticToTypeCode == "CASL")
            {
                var res = this.GetCountryCodeByCASLAddress(shipment.InlandDomesticToCountryId, tenant);

                if (res != null)
                {
                    return res.Code;
                }

                return "";
            }

            return null;
        }

        private string GetCountryCodeByPartnerId(string addressId)
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
            if (!string.IsNullOrEmpty(partnerAddress.Country?.Code))
            {
                country = partnerAddress.Country?.Code;
            }

            return country;
        }

        private Country GetCountryCodeByCASLAddress(string countryId, int tenant)
        {
            Country myResult = null;

            if (!string.IsNullOrEmpty(countryId))
            {
                myResult = CountryRepository.GetSingleCountry(countryId, tenant, false);
            }

            return myResult;
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

        private TimeLineData FillShipmnetTimeLineForShipment(dynamic shipment)
        {
            var timeLineData = new TimeLineData();

            if (!DoesPropertyExistInDynamic(shipment, "Id"))
            {
                return timeLineData;
            }

            string id = shipment.Id;
            var shipmentPickUpDeliveries = repository.context
                                                     .ShipmentPickUpDeliveries
                                                     .Include("FromAddressCountry")
                                                     .Include("ToAddressCountry")
                                                     .Where(a => a.ShipmentId == id);

            this.FillMainCarraigeFromTimeLine(timeLineData, shipment);
            this.FillMainCarraigeToTimeLine(timeLineData, shipment);
            this.FillPickUpTimeLine(timeLineData, shipmentPickUpDeliveries);
            this.FillDeliveryTimeLine(timeLineData, shipmentPickUpDeliveries);
            return timeLineData;
        }

        private void FillMainCarraigeFromTimeLine(TimeLineData timeLineData, dynamic shipment)
        {
            timeLineData.MainCarriageFrom = new TimeLineStop()
            {
                City = DoesPropertyExistInDynamic(shipment, "MainCarriageFromCity") 
                        && !string.IsNullOrEmpty(shipment.MainCarriageFromCity) 
                       ? shipment.MainCarriageFromCity 
                       : (DoesPropertyExistInDynamic(shipment, "FromPortName") 
                          ? shipment.FromPortName 
                          : ""),
                CountryCode = DoesPropertyExistInDynamic(shipment, "FromCountryCode") ? shipment.FromCountryCode : "",
                Date = DoesPropertyExistInDynamic(shipment, "MainCarriageATD") 
                        && shipment.MainCarriageATD != null 
                       ? shipment.MainCarriageATD 
                       : DoesPropertyExistInDynamic(shipment, "MainCarriageETD") ? shipment.MainCarriageETD : null,
                DateType = DoesPropertyExistInDynamic(shipment, "MainCarriageATD") 
                            && shipment.MainCarriageATD != null 
                            ? "Actual" 
                            : (DoesPropertyExistInDynamic(shipment, "MainCarriageETD") && shipment.MainCarriageETD != null 
                                ? "Estimated" 
                                : null),
                IsViaPortsDatesFilled = CheckIfViaPortsDatesFilled(shipment),
            };
        }

        private bool CheckIfViaPortsDatesFilled(dynamic shipment)
        {
            if ((DoesPropertyExistInDynamic(shipment, "Transshipment1ETA") && shipment.Transshipment1ETA != null) 
                || (DoesPropertyExistInDynamic(shipment, "Transshipment1ATA") && shipment.Transshipment1ATA != null )
                || (DoesPropertyExistInDynamic(shipment, "Transshipment1ETD") && shipment.Transshipment1ETD != null )
                || (DoesPropertyExistInDynamic(shipment, "Transshipment1ATD") && shipment.Transshipment1ATD != null))
            {
                return true;
            }
            else if ((DoesPropertyExistInDynamic(shipment, "Transshipment2ETA") && shipment.Transshipment2ETA != null)
                || (DoesPropertyExistInDynamic(shipment, "Transshipment2ATA") && shipment.Transshipment2ATA != null)
                || (DoesPropertyExistInDynamic(shipment, "Transshipment2ETD") && shipment.Transshipment2ETD != null)
                || (DoesPropertyExistInDynamic(shipment, "Transshipment2ATD") && shipment.Transshipment2ATD != null))
            {
                return true;
            }
            else if ((DoesPropertyExistInDynamic(shipment, "Transshipment3ETA") && shipment.Transshipment3ETA != null)
                || (DoesPropertyExistInDynamic(shipment, "Transshipment3ATA") && shipment.Transshipment3ATA != null)
                || (DoesPropertyExistInDynamic(shipment, "Transshipment3ETD") && shipment.Transshipment3ETD != null)
                || (DoesPropertyExistInDynamic(shipment, "Transshipment3ATD") && shipment.Transshipment3ATD != null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FillMainCarraigeToTimeLine(TimeLineData timeLineData, dynamic shipment)
        {
            timeLineData.MainCarriageTo = new TimeLineStop()
            {
                City = DoesPropertyExistInDynamic(shipment, "MainCarriageToCity") && !string.IsNullOrEmpty(shipment.MainCarriageToCity) 
                       ? shipment.MainCarriageToCity 
                       : DoesPropertyExistInDynamic(shipment, "ToPortName") ? shipment.ToPortName : "",
                CountryCode = DoesPropertyExistInDynamic(shipment, "ToCountryCode") ? shipment.ToCountryCode : "",
                Date = DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationATA") 
                        && shipment.MainCarriageFinalDestinationATA != null 
                       ? shipment.MainCarriageFinalDestinationATA 
                       : (DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationETA") 
                          ? shipment.MainCarriageFinalDestinationETA 
                          : null),
                DateType = DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationATA") 
                            && shipment.MainCarriageFinalDestinationATA != null 
                           ? "Actual" 
                           : (DoesPropertyExistInDynamic(shipment, "MainCarriageFinalDestinationATA") 
                              && shipment.MainCarriageFinalDestinationETA != null 
                              ? "Estimated" 
                              : null),
            };
        }

        private void FillPickUpTimeLine(TimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var firstPickup = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "PICK")
                                                       .OrderBy(s => s.PickUpDeliveryNumber)
                                                       .FirstOrDefault();
            if (firstPickup == null)
            {
                return;
            }

            int tenant = firstPickup.Tenant;
            timeLineData.Pickup = new TimeLineStop()
            {
                City = "",
                CountryCode = "",
                Date = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && firstPickup.ATD != null 
                      ? firstPickup.ATD 
                      : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && firstPickup.ETD != null 
                        ? firstPickup.ETD 
                        : null,
                DateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && firstPickup.ATD != null 
                           ? "Actual"
                           : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && firstPickup.ETD != null 
                               ? "Estimated" 
                               : null)
            };

            this.FillPickUpCityAndCountry(timeLineData, firstPickup, tenant);
        }

        private void FillPickUpCityAndCountry(dynamic timeLineData, ShipmentPickUpDelivery firstPickup, int tenant)
        {
            switch (firstPickup.PickUpDeliveryFromTypeCode)
            {
                case "PART":
                    {
                        if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromPartnerCardId") 
                             && !string.IsNullOrEmpty(firstPickup.FromPartnerCardId))
                        {
                            if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromAddressId")
                                && !string.IsNullOrEmpty(firstPickup.FromAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(firstPickup.FromAddressId, tenant);
                                if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromAddressId")
                                    &&  myPartnerAddress != null)
                                {
                                    timeLineData.Pickup.City = myPartnerAddress.City;
                                    timeLineData.Pickup.CountryCode = myPartnerAddress.Country?.Code;
                                    timeLineData.Pickup.CountryName = myPartnerAddress.Country?.EnglishName;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(firstPickup.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.Pickup.City = myPartnerAddress.City;
                                    timeLineData.Pickup.CountryCode = myPartnerAddress.Country?.Code;
                                    timeLineData.Pickup.CountryName = myPartnerAddress.Country?.EnglishName;
                                }
                            }
                        }
                        break;
                    }

                case "PORT":
                    {
                        if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromPortId") 
                            && !string.IsNullOrEmpty(firstPickup.FromPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, firstPickup.FromPortId, true);
                            if (myPort != null)
                            {
                                timeLineData.Pickup.City = myPort.EnglishName;
                                timeLineData.Pickup.CountryCode = myPort.CountryCode;
                                timeLineData.Pickup.CountryName = myPort.CountryName;
                            }
                        }
                        break;
                    }

                case "CASL":
                    {
                        timeLineData.Pickup.City = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromAddressCity") 
                                                   ? firstPickup.FromAddressCity 
                                                   : "";
                        timeLineData.Pickup.CountryCode = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromAddressCountry") 
                                                          ? firstPickup.FromAddressCountry?.Code 
                                                          : "";
                        timeLineData.Pickup.CountryName = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.FromAddressCountry") 
                                                          ? firstPickup.FromAddressCountry?.EnglishName
                                                          : "";
                        break;
                    }
            }
        }

        private void FillDeliveryTimeLine(TimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var finalDelivery = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "DELV")
                                                         .OrderByDescending(s => s.PickUpDeliveryNumber)
                                                         .FirstOrDefault();
            if (finalDelivery == null)
            {
                return;
            }

            int tenant = finalDelivery.Tenant;
            timeLineData.Delivery = new TimeLineStop()
            {
                City = "",
                CountryCode = "",
                Date = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                       ? finalDelivery.ATA 
                       : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && finalDelivery.ETA != null ? finalDelivery.ETA : null,
                DateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                           ? "Actual" 
                           : (finalDelivery.ETA != null ? "Estimated" : null),
            };

            this.FillDeliveryCityAndCountry(timeLineData, finalDelivery, tenant);
        }

        private void FillDeliveryCityAndCountry(dynamic timeLineData, ShipmentPickUpDelivery finalDelivery, int tenant)
        {
            switch (finalDelivery.PickUpDeliveryToTypeCode)
            {
                case "PART":
                    {
                        if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToPartnerCardId") 
                             && !string.IsNullOrEmpty(finalDelivery.ToPartnerCardId))
                        {
                            if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressId")
                                 && !string.IsNullOrEmpty(finalDelivery.ToAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(finalDelivery.ToAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    timeLineData.Delivery.City = myPartnerAddress.City;
                                    timeLineData.Delivery.CountryCode = myPartnerAddress.Country?.Code;
                                    timeLineData.Delivery.CountryName = myPartnerAddress.Country?.EnglishName;
                                }
                            }
                            else
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(finalDelivery.ToPartnerCardId, tenant);
                                if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToPartnerCardId")
                                    && myPartnerAddress != null)
                                {
                                    timeLineData.Delivery.City = myPartnerAddress.City;
                                    timeLineData.Delivery.CountryCode = myPartnerAddress.Country?.Code;
                                    timeLineData.Delivery.CountryName = myPartnerAddress.Country?.EnglishName;
                                }
                            }
                        }

                        break;
                    }

                case "PORT":
                    {
                        if (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToPortId")
                             && !string.IsNullOrEmpty(finalDelivery.ToPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, finalDelivery.ToPortId, true);
                            if (myPort != null)
                            {
                                timeLineData.Delivery.City = myPort.EnglishName;
                                timeLineData.Delivery.CountryCode = myPort.CountryCode;
                                timeLineData.Delivery.CountryName = myPort.CountryName;
                            }
                        }

                        break;
                    }

                case "CASL":
                    {
                        timeLineData.Delivery.City = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCity")
                                                     ? finalDelivery.ToAddressCity
                                                     : "";
                        timeLineData.Delivery.CountryCode = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCountry")
                                                             ? finalDelivery.ToAddressCountry?.Code
                                                             : "";
                        timeLineData.Delivery.CountryName = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ToAddressCountry")
                                                             ? finalDelivery.ToAddressCountry ?.EnglishName
                                                             : "";
                        break;
                    }
            }
        }
        #endregion Shipment Horizontal TimeLine

        #region Shipment Vertical TimeLine

        public VerticalTimeLineData MapVerticalTimeLine(ShipmentPM shipment, Dictionary<string, List<string>> blockedFields, string profileCode = "CS")
        {
            int tenant = shipment.Tenant;
            InitializeServices(tenant);
            this.blockedFields = blockedFields;

            // Build All Legs 
            var shipmentPickUpDeliveries = repository.context
                                                       .ShipmentPickUpDeliveries
                                                       .Include("FromAddressCountry")
                                                       .Include("ToAddressCountry")
                                                       .Where(a => a.ShipmentId == shipment.Id);

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

        private bool CheckIsPermissonField(string objectTbaleName, string fieldCode)
        {
            bool isPermissonField = true;
            if (blockedFields.ContainsKey(objectTbaleName) && blockedFields[objectTbaleName].Contains($"{fieldCode}"))
            {
                isPermissonField =  false;
            }
            
            return isPermissonField;
        }

        private void FillPickUpVerticalTimeLine(VerticalTimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {

            var firstPickup = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "PICK")
                                                       .OrderBy(s => s.PickUpDeliveryNumber)
                                                       .FirstOrDefault();
            if (firstPickup == null)
            {
                return;
            }

            int tenant = firstPickup.Tenant;
            timeLineData.Pickup = new VerticalTimeLineStop()
            {
                Title = "Shipment.G.PickUp",
                City = "",
                CountryCode = "",
                ATDDate = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") 
                            && firstPickup.ATD != null 
                          ? firstPickup.ATD 
                          : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && firstPickup.ETD != null ? firstPickup.ETD : null),
                ATDDateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") 
                                && firstPickup.ATD != null 
                              ? "Actual" 
                              : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && firstPickup.ETD != null 
                                 ? "Estimated" 
                                 : null),
                ATADate = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && firstPickup.ATA != null 
                          ? firstPickup.ATA 
                          : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && firstPickup.ETA != null 
                            ? firstPickup.ETA 
                            : null,
                ATADateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") 
                                && firstPickup.ATA != null 
                              ? "Actual" 
                              : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && firstPickup.ETA != null 
                                 ? "Estimated" 
                                 : null),
                Date = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && firstPickup.ATD != null 
                       ? firstPickup.ATD 
                       : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") 
                         ? firstPickup.ETD 
                         : null,
                DateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") 
                            && firstPickup.ATD != null 
                           ? "Actual" 
                           : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && firstPickup.ETD != null 
                              ? "Estimated" 
                              : null),
                LegDetails = FillPickUpDeliveryLegDetails(firstPickup),
                TransportModeId = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.TransportModeCode") 
                                  ? firstPickup.TransportModeCode 
                                  : "",
            };

            this.FillPickUpCityAndCountry(timeLineData, firstPickup, tenant);
        }

        private Dictionary<string, string> FillPickUpDeliveryLegDetails(ShipmentPickUpDelivery firstPickup)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            Card trucker = cardRepository.GetSingleCardWithoutInclude(firstPickup.CarrierId, tenant);
            var legDetails = new Dictionary<string, string>
            {
                { "ShipmentPickUpDelivery.Id", CheckEmptyValue(trucker?.EnglishName) },
                { "ShipmentPickUpDelivery.PickUpDeliveryNumber", CheckEmptyValue(firstPickup.CarrierNumber) }
            };

            return legDetails;
        }

        private void FillWarehouseLegsTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.WarehouseLegWarehouseId))
            {
                VerticalTimeLineData.Warehouse1 = new VerticalTimeLineStop()
                {
                    Title = "Shipment.G.TerminalHub",
                    City = shipment.WarehouseLegAddressCity,
                    CountryCode = shipment.WarehouseLegAddressCountryCode,
                    CountryName = shipment.WarehouseLegAddressCountryName,
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
            var warehouseLegCutOffDate = CheckIsPermissonField("Shipment", "Shipment.WarehouseLegCutOffDate") 
                                         ? (shipment.WarehouseLegCutOffDate == null ? "–" : shipment.WarehouseLegCutOffDate?.ToString("dd/MM/yyyy"))
                                         : "";

            var legDetails = new Dictionary<string, string>
            {
                { "Shipment.WarehouseLegCutOffDate", warehouseLegCutOffDate},
            };

            if (CheckIsPermissonField("Shipment", "Shipment.WarehouseLegActualReleaseDate") 
                 && CheckIsPermissonField("Shipment", "Shipment.WarehouseLegActualEntryDate"))
            {
                var storageDays = GetStorageDays(shipment);
                var storageDaysText = storageDays == 0 ? "–" : storageDays + " days";
                legDetails.Add("Shipment.G.StorageDays", storageDaysText );
            }

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
                    Title = "Shipment.G.PreCarriage",
                    City = shipment.PreCarriageFromPortName,
                    CountryCode = shipment.PreCarriageFromPortCountryCode,
                    CountryName = shipment.PreCarriageFromPortCountryName,
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
                { "Shipment.PreCarriageCarrierName", CheckEmptyValue(shipment.PreCarriageCarrierName) },
                { "Shipment.PreCarriageCarrierNumber", CheckEmptyValue(shipment.PreCarriageCarrierNumber) }
            };

            if (shipment.PreCarriageTransportModeId == "O")
            {
                legDetails.Add("Shipment.PreCarriageVesselName", CheckEmptyValue(shipment.PreCarriageVesselName));
            }

            return legDetails;
        }

        private void FillMainCarraigeFromVerticalTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            VerticalTimeLineData.MainCarriageFrom = new VerticalTimeLineStop()
            {
                Title = shipment.TransportModeId == "A" ? "Shipment.G.GatewayLable" : "Shipment.G.PortOfLoadingLable",
                City = isInlandDomesticShipment ? GetCityForFromInlandDomestic(shipment) : shipment.MainCarriageFromPortName,
                CountryCode = isInlandDomesticShipment ? GetCountryCodeForFromInlandDomestic(shipment) : shipment.MainCarriageFromPortCountryCode,
                CountryName = isInlandDomesticShipment ? GetCountryNameForFromInlandDomestic(shipment) : shipment.MainCarriageFromPortCountryName,
                Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                DateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
                ATDDate = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD,
                ATDDateType = shipment.MainCarriageATD != null ? "Actual" : (shipment.MainCarriageETD != null ? "Estimated" : null),
                ATADate = shipment.MainCarriageATA != null ? shipment.MainCarriageATA : shipment.MainCarriageETA,
                ATADateType = shipment.MainCarriageATA != null ? "Actual" : (shipment.MainCarriageETA != null ? "Estimated" : null),
                LegDetails = FillMainCarriageFromLegDetails(shipment)
            };
        }

        private Dictionary<string, string> FillMainCarriageFromLegDetails(ShipmentPM shipment)
        {
            var master = shipment.TransportModeId == "A" 
                         ? (shipment.AirlinePrefix != null && shipment.Master != null 
                           ? shipment.AirlinePrefix + "-" + shipment.Master 
                           : shipment.Master) 
                         : shipment.Master;

            var legDetails = new Dictionary<string, string>();
           
            if (CheckIsPermissonField("Shipment", "Shipment.MainCarriageCarrierName"))
            {
                legDetails.Add(GetCarrierTextCode(shipment), CheckEmptyValue(shipment.MainCarriageCarrierName));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.MainCarriageCarrierNumber"))
            {
                legDetails.Add(GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.MainCarriageCarrierNumber));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Master"))
            {
                legDetails.Add(GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master));
            }

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Shipment.MainCarriageVesselName", CheckEmptyValue(shipment.MainCarriageVesselName));
            }

            return legDetails;
        }

        private void FillTransshipmentTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
            {
                VerticalTimeLineData.Transshipment1 = new VerticalTimeLineStop()
                {
                    Title = "Shipment.G.Transshipment1Port",
                    City = shipment.Transshipment1FromPortName,
                    CountryCode = shipment.Transshipment1FromPortCountryCode,
                    CountryName = shipment.Transshipment1FromPortCountryName,
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
                    Title = "Shipment.G.Transshipment2Port",
                    City = shipment.Transshipment2FromPortName,
                    CountryCode = shipment.Transshipment2FromPortCountryCode,
                    CountryName = shipment.Transshipment2FromPortCountryName,
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
                    Title = "Shipment.G.Transshipment3Port",
                    City = shipment.Transshipment3FromPortName,
                    CountryCode = shipment.Transshipment3FromPortCountryCode,
                    CountryName = shipment.Transshipment3FromPortCountryName,
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

            var legDetails = new Dictionary<string, string>();
            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment1CarrierName"))
            {
                legDetails.Add(GetCarrierTextCode(shipment), CheckEmptyValue(shipment.Transshipment1CarrierName));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment1CarrierNumber"))
            {
                legDetails.Add(GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.Transshipment1CarrierNumber));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment1AdditionalMAWBOBLBL"))
            {
                legDetails.Add(GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master));
            }
            
            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Shipment.Transshipment1VesselName", CheckEmptyValue(shipment.Transshipment1VesselName));
            }

            return legDetails;
        }

        private Dictionary<string, string> FillTransshipment2LegDetails(ShipmentPM shipment)
        {
            var master = shipment.Transshipment2AdditionalMAWBOBLBL;
            var legDetails = new Dictionary<string, string>();
            
            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment2CarrierName"))
            {
                legDetails.Add(GetCarrierTextCode(shipment), CheckEmptyValue(shipment.Transshipment2CarrierName));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment2CarrierNumber"))
            {
                legDetails.Add(GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.Transshipment2CarrierNumber));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment2AdditionalMAWBOBLBL"))
            {
                legDetails.Add(GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master));
            }

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Shipment.Transshipment2VesselName", CheckEmptyValue(shipment.Transshipment2VesselName));
            }

            return legDetails;
        }

        private Dictionary<string, string> FillTransshipment3LegDetails(ShipmentPM shipment)
        {
            var master = shipment.Transshipment3AdditionalMAWBOBLBL;
            var legDetails = new Dictionary<string, string>();
            
            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment3CarrierName"))
            {
                legDetails.Add(GetCarrierTextCode(shipment), CheckEmptyValue(shipment.Transshipment3CarrierName));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment3CarrierNumber"))
            {
                legDetails.Add(GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(shipment.Transshipment3CarrierNumber));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Transshipment3AdditionalMAWBOBLBL"))
            {
                legDetails.Add(GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master));
            }

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add("Shipment.Transshipment3VesselName", CheckEmptyValue(shipment.Transshipment3VesselName));
            }

            return legDetails;
        }

        private void FillMainCarraigeToVerticalTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            VerticalTimeLineData.MainCarriageTo = new VerticalTimeLineStop()
            {
                Title = shipment.TransportModeId == "A" ? "Shipment.G.DestinationLable" : "Shipment.G.DischargePort",
                City = isInlandDomesticShipment ? GetCityForToInlandDomestic(shipment) : shipment.MainCarriageFinalDestinationPortName,
                CountryCode = isInlandDomesticShipment ? GetCountryCodeForToInlandDomestic(shipment) : shipment.MainCarriageFinalDestinationPortCountryCode,
                CountryName = isInlandDomesticShipment ? GetCountryNameForToInlandDomestic(shipment) : shipment.MainCarriageFinalDestinationPortCountryName,
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
            var legDetails = new Dictionary<string, string>();

            if (CheckIsPermissonField("Shipment", GetCarrierNameOfDischargeLegTitle(shipment)))
            {
                legDetails.Add(GetCarrierTextCode(shipment), CheckEmptyValue(GetCarrierNameOfDischargeLeg(shipment)));
            }

            if (CheckIsPermissonField("Shipment", GetCarrierNumberOfDischargeLegTitle(shipment)))
            {
                legDetails.Add(GetVerticalTimelineCarrierNumberTextCode(shipment), CheckEmptyValue(GetCarrierNumberOfDischargeLeg(shipment)));
            }

            if (CheckIsPermissonField("Shipment", "Shipment.Master"))
            {
                legDetails.Add(GetVerticalTimeLineMasterTextCode(shipment), CheckEmptyValue(master));
            }

            if (shipment.TransportModeId == "O")
            {
                legDetails.Add(GetVesselOfDischargeLegTitle(shipment), CheckEmptyValue(GetVesselOfDischargeLeg(shipment)));
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
                return shipment.TransportModeId == "A"
                               ? (shipment.AirlinePrefix != null && shipment.Master != null 
                                   ? shipment.AirlinePrefix + "-" + shipment.Master 
                                   : shipment.Master) 
                               : shipment.Master;
            }

            return null;
        }

        private string GetCarrierNameOfDischargeLeg(ShipmentPM shipment)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            if (isInlandDomesticShipment)
            {
                return shipment.MainCarriageCarrierName;
            }

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

        private string GetCarrierNameOfDischargeLegTitle(ShipmentPM shipment)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            if (isInlandDomesticShipment)
            {
                return "Shipment.MainCarriageCarrierName";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return "Shipment.Transshipment3CarrierName";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return "Shipment.Transshipment2CarrierName";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return "Shipment.Transshipment1CarrierName";
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return "Shipment.MainCarriageCarrierName";
            }

            return null;
        }

        private string GetCarrierNumberOfDischargeLeg(ShipmentPM shipment)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            if (isInlandDomesticShipment)
            {
                return shipment.MainCarriageCarrierNumber;
            }

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

        private string GetCarrierNumberOfDischargeLegTitle(ShipmentPM shipment)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            if (isInlandDomesticShipment)
            {
                return "Shipment.MainCarriageCarrierNumber";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return "Shipment.Transshipment3CarrierNumber";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return "Shipment.Transshipment2CarrierNumber";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return "Shipment.Transshipment1CarrierNumber";
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return "Shipment.MainCarriageCarrierNumber";
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
        private string GetVesselOfDischargeLegTitle(ShipmentPM shipment)
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                return "Shipment.Transshipment3VesselName";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                return "Shipment.Transshipment2VesselName";
            }

            if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                return "Shipment.Transshipment1VesselName";
            }

            if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                return "Shipment.MainCarriageVesselName";
            }

            return null;
        }

        private void FillOnCarraigeLegTimeLine(VerticalTimeLineData VerticalTimeLineData, ShipmentPM shipment)
        {

            if (!string.IsNullOrEmpty(shipment.OnCarriageFromPortId))
            {
                VerticalTimeLineData.OnCarriage = new VerticalTimeLineStop()
                {
                    Title = "Shipment.G.OnCarriage",
                    City = shipment.OnCarriageFromPortName,
                    CountryCode = shipment.OnCarriageFromPortCountryCode,
                    CountryName = shipment.OnCarriageFromPortCountryName,
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
            legDetails.Add("Shipment.OnCarriageCarrierName", CheckEmptyValue(shipment.OnCarriageCarrierName));
            legDetails.Add("Shipment.OnCarriageCarrierNumber", CheckEmptyValue(shipment.OnCarriageCarrierNumber));

            if (shipment.OnCarriageTransportModeId == "O")
            {
                legDetails.Add("Shipment.OnCarriageVesselName", CheckEmptyValue(shipment.OnCarriageVesselName));
            }

            return legDetails;
        }

        private void FillDeliveryVerticalTimeLine(VerticalTimeLineData timeLineData, IQueryable<ShipmentPickUpDelivery> shipmentPickUpDeliveries)
        {
            var finalDelivery = shipmentPickUpDeliveries?.Where(d => d.PickUpDeliveryTypeCode == "DELV")
                                                         .OrderByDescending(s => s.PickUpDeliveryNumber)
                                                         .FirstOrDefault();
            if (finalDelivery == null)
            {
                return;
            }

            int tenant = finalDelivery.Tenant;
            timeLineData.Delivery = new VerticalTimeLineStop()
            {
                Title = "Shipment.G.Delivery",
                City = "",
                CountryCode = "",
                Date = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                       ? finalDelivery.ATA 
                       : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") 
                         ? finalDelivery.ETA 
                         : null,
                DateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                           ? "Actual" 
                           : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && finalDelivery.ETA != null ? "Estimated" : null),
                ATADate = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                          ? finalDelivery.ATA 
                          : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") ? finalDelivery.ETA : null,
                ATADateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATA") && finalDelivery.ATA != null 
                              ? "Actual" 
                              : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETA") && finalDelivery.ETA != null ? "Estimated" : null),
                ATDDate = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && finalDelivery.ATD != null 
                          ? finalDelivery.ATD 
                          : CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") 
                            ? finalDelivery.ETD 
                            : null,
                ATDDateType = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ATD") && finalDelivery.ATD != null 
                              ? "Actual" 
                              : (CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.ETD") && finalDelivery.ETD != null ? "Estimated" : null),
                LegDetails = FillPickUpDeliveryLegDetails(finalDelivery),
                TransportModeId = CheckIsPermissonField("ShipmentPickUpDelivery", "ShipmentPickUpDelivery.TransportModeCode") ? finalDelivery.TransportModeCode : "",
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
            var shipmentSubTypes = GetShipmentTypes(tenant);

            var digitalTransportModes = new List<DigitalTransportModes>
            {
                // Air 
                new DigitalTransportModes()
                {
                    Name = "General.G.AirLabel",
                    Code = "A",
                    Children = GetSubTypes(shipmentSubTypes, new List<string> { "General.G.AirLabel" }, "A")
                },

                // Ocean 
                new DigitalTransportModes()
                {
                    Name = "General.G.OceanLabel",
                    Code = "O",
                    Children = GetOceanChildren(shipmentSubTypes)
                },

                // Ocean 
                new DigitalTransportModes()
                {
                    Name = "General.G.InlandLabel",
                    Code = "I",
                    Children = GetInlandChildren(shipmentSubTypes)
                }
            };

            return digitalTransportModes;
        }

        private List<DigitalTransportModesChild> GetOceanChildren(List<ShipmentSubType> shipmentSubTypes)
        {
            var oceanChildren = new List<DigitalTransportModesChild>
            {
                new DigitalTransportModesChild()
                {
                    ParentCode = "O",
                    Name = "General.G.FCL",
                    Code = "FCL,FCLD",
                    DisaledOption = true,
                    Children = GetSubTypes(shipmentSubTypes, new List<string> { "General.G.FCL", "General.G.FCLD" }, "O")
                },
                new DigitalTransportModesChild()
                {
                    ParentCode = "O",
                    Name = "General.G.LCL",
                    Code = "LCL,LCLD",
                    DisaledOption = true,
                    Children = GetSubTypes(shipmentSubTypes, new List<string> { "General.G.LCL", "General.G.LCLD" }, "O")
                }
            };

            return oceanChildren;
        }

        private List<DigitalTransportModesChild> GetInlandChildren(List<ShipmentSubType> shipmentSubTypes)
        {
            var inlandChildren = new List<DigitalTransportModesChild>
            {
                new DigitalTransportModesChild()
                {
                    ParentCode = "I",
                    Name = "General.G.FTL",
                    Code = "FTL",
                    DisaledOption = true,
                    Children = GetSubTypes(shipmentSubTypes, new List<string> { "General.G.FTL" }, "I")
                },
                new DigitalTransportModesChild()
                {
                    ParentCode = "I",
                    Name = "General.G.LTL",
                    Code = "LTL",
                    DisaledOption = true,
                    Children = GetSubTypes(shipmentSubTypes, new List<string> { "General.G.LTL" }, "I")
                }
            };

            return inlandChildren;
        }

        private List<DigitalTransportModesChild> GetSubTypes(List<ShipmentSubType> shipmentSubTypes, List<string> typeCodes, string parentCode)
        {
            var subTypes = shipmentSubTypes.Where(a => typeCodes.Contains(a.ShipmentTypeCode) 
                                                       || a.ShipmentTypeCode == null)
                                           .ToList();
            if (subTypes == null)
            {
                return null;
            }

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
            IQueryable<ShipmentSubType> shipmentSubTypes = shipmentSubTypeRepository.GetShipmentSubTypesWithoutIncludes(tenant);
            return shipmentSubTypes.Where(a => !a.Inactive).ToList();
        }

        #endregion Transport & Subtypes

        #region Global Search 
        public Tuple<QueryOperations, IQueryable<DigitalShipmentList>> GetByFiltersTuple(GeneralFilters newFilters)
        {
            var tenant = newFilters.Tenant;

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
                queryOperations.SetFilter(partnerTypeName, newFilters.CardId, false, "InList", null, false);
            }

            if (!string.IsNullOrEmpty(shipmentLevelCodeValue))
            {
                queryOperations.SetFilter("ShipmentLevelCode", shipmentLevelCodeValue, false, "InListExact", null, false);
            }

            var ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant);

            foreach (var filter in newFilters.AdditionalFilters)
            {
                var field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                if (field != null)
                {
                    string valuestring1 = filter.FieldValue?.ToString();
                    object value1 = Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                    string valuestring2 = filter.FieldValue2?.ToString();
                    object value2 = Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                    string valuestring3 = filter.FieldValue3?.ToString();
                    object value3 = Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring3);
                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                }
                else
                {
                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.FieldValue3, filter.DisplayInList);
                }
            }

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

            return Tuple.Create(queryOperations, entityLists);
        }

        public IQueryable<DigitalShipmentList> GetByFilters(GeneralFilters newFilters)
        {
            var response = GetByFiltersTuple(newFilters);

            return response.Item2;
        }

        public IQueryable<DigitalShipmentList>  GetByFilterWithSortingFilter(GeneralFilters newFilters)
        {
            var response = GetByFiltersTuple(newFilters); 

            var queryOperations = response.Item1;
            var entityLists  = response.Item2;

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
