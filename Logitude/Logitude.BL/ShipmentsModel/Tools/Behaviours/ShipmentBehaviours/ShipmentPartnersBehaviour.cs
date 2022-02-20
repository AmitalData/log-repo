using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentPartnersBehaviour : IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            HandleShipper();
            HandleConsignee();
            HandleAgent();
            HandleShipperNotExporter();
            HandleCustomAgentImport();
            HandleReleasingAgent();
            HandleFreightForwarder();
            HandleNotify1();
            HandleInlandDemosticPartners();            
            HandleConsigneeNotImporter();
        }

        private void HandleShipper()
        {
            string cardId = entityPM.ShipperId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ShipperName = IsShipmentFromToLogbox() || entityPM.IsExternalAPI ? entityPM.ShipperName : null;
                entityPM.ShipperNote = null;
                entityPM.ShipperContactId = null;
                entityPM.ShipperAddressId = null;
                entityPM.ShipperMainAddressId = null;
                entityPM.ShipperAddressText = null;
                entityPM.ShipperReference1 = null;
                entityPM.ShipperReference2 = null;
            }

            else if (entityPM.IsExternalAPI || entityPM.IsHybrid)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.ShipperName = card.EnglishName;

                    if (entityPM.IsExternalAPI)
                    {
                        if (string.IsNullOrEmpty(entityPM.ShipperContactId))
                        {
                            entityPM.ShipperContactId = card.PrimaryContactId;
                        }

                        if (string.IsNullOrEmpty(entityPM.ShipperAddressId))
                        {
                            entityPM.ShipperAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                        }
                    }

                    else
                    {
                        entityPM.ShipperContactId = card.PrimaryContactId;
                        entityPM.ShipperAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }
        private void HandleConsignee()
        {
            string cardId = entityPM.ConsigneeId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ConsigneeName = IsShipmentFromToLogbox() ? entityPM.ConsigneeName : null;
                entityPM.ConsigneeNote = null;
                entityPM.ConsigneeContactId = null;
                entityPM.ConsigneeAddressId = null;
                entityPM.ConsigneeMainAddressId = null;
                entityPM.ConsigneeAddressText = null;
                entityPM.ConsigneeReference1 = IsShipmentFromToLogbox() ? entityPM.ConsigneeReference1 : null;
                entityPM.ConsigneeReference2 = IsShipmentFromToLogbox() ? entityPM.ConsigneeReference2 : null;
            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.ConsigneeName = card.EnglishName;

                    if (string.IsNullOrEmpty(entityPM.ConsigneeContactId))
                    {
                        entityPM.ConsigneeContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrEmpty(entityPM.ConsigneeAddressId))
                    {
                        entityPM.ConsigneeAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }
        private void HandleAgent()
        {
            string cardId = entityPM.AgentId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.PrivateLabelAgentName = IsShipmentFromToLogbox() ? entityPM.PrivateLabelAgentName : null;
                entityPM.AgentName = null;
                entityPM.AgentNote = null;
                entityPM.AgentContactId = null;
                entityPM.AgentAddressId = null;
                entityPM.AgentAddressText = null;
                entityPM.AgentReference1 = null;
                entityPM.AgentReference2 = null;
            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.AgentName = card.EnglishName;
                    entityPM.PrivateLabelAgentName = card.EnglishName;

                    if (string.IsNullOrEmpty(entityPM.AgentContactId))
                    {
                        entityPM.AgentContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrEmpty(entityPM.AgentAddressId))
                    {
                        entityPM.AgentAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                entityPM.PrivateLabelAgentName = card?.EnglishName;
                // from new shipment screen: additional fields
                if (string.IsNullOrEmpty(entityPM.AgentAddressId))
                {        
                    if (card != null)
                    {
                        entityPM.AgentName = card.EnglishName;
                        entityPM.AgentContactId = card.PrimaryContactId;
                        entityPM.AgentAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }


            }            
        }
        private void HandleShipperNotExporter()
        {
            string cardId = entityPM.ShipperNotExporterId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ShipperNotExporterName = null;
                entityPM.ShipperNotExporterNote = null;
                entityPM.ShipperNotExporterContactId = null;
                entityPM.ShipperNotExporterAddressId = null;
                entityPM.ShipperNotExporterReference = null;
                entityPM.ShipperNotExporterReference1 = null;
                entityPM.ShipperNotExporterReference2 = null;

            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.ShipperNotExporterName = card.EnglishName;

                    if (string.IsNullOrWhiteSpace(entityPM.ShipperNotExporterContactId))
                    {
                        entityPM.ShipperNotExporterContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrWhiteSpace(entityPM.ShipperNotExporterAddressId))
                    {
                        entityPM.ShipperNotExporterAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }
        private void HandleCustomAgentImport()
        {
            string cardId = entityPM.CustomAgentImportId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.CustomAgentImportName = null;
                entityPM.CustomAgentImportNote = null;
                entityPM.CustomAgentImportContactId = null;
                entityPM.CustomAgentImportAddressId = null;
                entityPM.CustomAgentImportReference = null;
            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.CustomAgentImportName = card.EnglishName;

                    if (string.IsNullOrEmpty(entityPM.CustomAgentImportContactId))
                    {
                        entityPM.CustomAgentImportContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrEmpty(entityPM.CustomAgentImportAddressId))
                    {
                        entityPM.CustomAgentImportAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }
        private void HandleReleasingAgent()
        {
            string cardId = entityPM.ReleasingAgentId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ReleasingAgentName = null;
                entityPM.ReleasingAgentNote = null;
                entityPM.ReleasingAgentContactId = null;
                entityPM.ReleasingAgentAddressId = null;
                entityPM.ReleasingAgentReference1 = null;
                entityPM.ReleasingAgentReference2 = null;
            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.ReleasingAgentName = card.EnglishName;

                    if (string.IsNullOrEmpty(entityPM.ReleasingAgentContactId))
                    {
                        entityPM.ReleasingAgentContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrEmpty(entityPM.ReleasingAgentAddressId))
                    {
                        entityPM.ReleasingAgentAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }
        private void HandleFreightForwarder()
        {
            string cardId = entityPM.FreightForwarderId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.FreightForwarderName = null;
                entityPM.FreightForwarderNote = null;
                entityPM.FreightForwarderContactId = null;
                entityPM.FreightForwarderAddressId = null;
                entityPM.FreightForwarderReference = null;
            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.FreightForwarderName = card.EnglishName;

                    if (string.IsNullOrEmpty(entityPM.FreightForwarderContactId))
                    {
                        entityPM.FreightForwarderContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrEmpty(entityPM.FreightForwarderAddressId))
                    {
                        entityPM.FreightForwarderAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }
        private void HandleNotify1()
        {
            string cardId = entityPM.Notify1Id;

            if (string.IsNullOrEmpty(cardId))
            {
                ResetNotify1Properties();
            }

            else if (entityPM.IsExternalAPI || entityPM.IsHybrid)
            {
                MapNotify1FieldsFromNotify1Card(cardId);
            }
        }
        private void ResetNotify1Properties()
        {
            entityPM.Notify1Name = IsShipmentFromToLogbox() || entityPM.IsExternalAPI ? entityPM.Notify1Name : null;
            entityPM.Notify1Note = null;
            entityPM.Notify1ContactId = null;
            entityPM.Notify1AddressId = null;
            entityPM.Notify1Reference2 = null;
        }
        private void MapNotify1FieldsFromNotify1Card(string cardId)
        {
            Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
            if (card != null)
            {
                entityPM.Notify1Name = card.EnglishName;
                MapNotify1Address(card);
                MapNotify1Contact(card);
            }
        }
        private void MapNotify1Address(Card card)
        {
            if (entityPM.IsExternalAPI)
            {
                if (string.IsNullOrEmpty(entityPM.Notify1AddressId))
                {
                    entityPM.Notify1AddressId = initializer.AddressRepository.GetMainAddressId(card.Id, initializer.Tenant);
                }
            }

            else
            {
                entityPM.Notify1AddressId = initializer.AddressRepository.GetMainAddressId(card.Id, initializer.Tenant);
            }
        }
        private void MapNotify1Contact(Card card)
        {
            if (entityPM.IsExternalAPI)
            {
                if (string.IsNullOrEmpty(entityPM.Notify1ContactId))
                {
                    entityPM.Notify1ContactId = card.PrimaryContactId;
                }
            }

            else
            {
                entityPM.Notify1ContactId = card.PrimaryContactId;
            }
        }
        
        private void HandleConsigneeNotImporter()
        {
            string cardId = entityPM.ConsigneeNotImporterId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ConsigneeNotImporterName = null;
                entityPM.ConsigneeNotImporterNote = null;
                entityPM.ConsigneeNotImporterContactId = null;
                entityPM.ConsigneeNotImporterAddressId = null;
                entityPM.ConsigneeNotImporterReference = null;
            }

            else if (entityPM.IsExternalAPI)
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);
                if (card != null)
                {
                    entityPM.ConsigneeNotImporterName = card.EnglishName;

                    if (string.IsNullOrWhiteSpace(entityPM.ConsigneeNotImporterContactId))
                    {
                        entityPM.ConsigneeNotImporterContactId = card.PrimaryContactId;
                    }

                    if (string.IsNullOrWhiteSpace(entityPM.ConsigneeNotImporterAddressId))
                    {
                        entityPM.ConsigneeNotImporterAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                    }
                }
            }
        }

        private bool IsShipmentFromToLogbox()
        {
            bool isShipmentFromUNF = entityPM.IsHybrid;
            bool isShipmentFromOrToLogbox = isShipmentFromUNF || entityPM.IsImporterShipment || !string.IsNullOrEmpty(entityPM.ForwarderShipmentNumber);
            return isShipmentFromOrToLogbox;
        }
        private void HandleInlandDemosticPartners()
        {
            if (!IsInlandDomesticShipment())
            {
                return ;
            }

            if (entityPM.InlandDomesticFromTypeCode == "PART")
            {
                HandleMainCarrigeFromPartner();
            }
            if (entityPM.InlandDomesticToTypeCode == "PART")
            {
                HandleMainCarrigeToPartner();
            }

        }
        private bool IsInlandDomesticShipment()
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }
        private void HandleMainCarrigeFromPartner()
        {
            if (!entityPM.IsExternalAPI)
            {
                return ;
            }
            if (string.IsNullOrWhiteSpace(entityPM.MainCarriageFromAddressId))
            {
                entityPM.MainCarriageFromAddressId = initializer.AddressRepository.GetMainAddressId(entityPM.MainCarriageFromPartnerId, initializer.Tenant);
            }
        }
        private void HandleMainCarrigeToPartner()
        {
            if (!entityPM.IsExternalAPI)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(entityPM.MainCarriageToAddressId))
            {
                entityPM.MainCarriageToAddressId = initializer.AddressRepository.GetMainAddressId(entityPM.MainCarriageToPartnerId, initializer.Tenant);
            }
        }
    }
}
