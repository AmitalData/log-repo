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
        }

        private void HandleShipper()
        {
            string cardId = entityPM.ShipperId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ShipperName = null;
                entityPM.ShipperNote = null;
                entityPM.ShipperContactId = null;
                entityPM.ShipperAddressId = null;
                entityPM.ShipperMainAddressId = null;
                entityPM.ShipperAddressText = null;
                entityPM.ShipperReference1 = null;
                entityPM.ShipperReference2 = null;
            }

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.ShipperName = card.EnglishName;

                if (entityPM.ShipperContactId == null)
                {
                    entityPM.ShipperContactId = card.PrimaryContactId;
                }

                if (entityPM.ShipperAddressId == null)
                {
                    entityPM.ShipperAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                }
            }
        }
        private void HandleConsignee()
        {
            string cardId = entityPM.ConsigneeId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.ConsigneeName = null;
                entityPM.ConsigneeNote = null;
                entityPM.ConsigneeContactId = null;
                entityPM.ConsigneeAddressId = null;
                entityPM.ConsigneeMainAddressId = null;
                entityPM.ConsigneeAddressText = null;
                entityPM.ConsigneeReference1 = null;
                entityPM.ConsigneeReference2 = null;
            }

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.ConsigneeName = card.EnglishName;

                if (entityPM.ConsigneeContactId == null)
                {
                    entityPM.ConsigneeContactId = card.PrimaryContactId;
                }

                if (entityPM.ConsigneeAddressId == null)
                {
                    entityPM.ConsigneeAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                }
            }
        }
        private void HandleAgent()
        {
            string cardId = entityPM.AgentId;

            if (string.IsNullOrEmpty(cardId))
            {
                entityPM.AgentName = null;
                entityPM.AgentNote = null;
                entityPM.AgentContactId = null;
                entityPM.AgentAddressId = null;
                entityPM.AgentAddressText = null;
                entityPM.AgentReference1 = null;
                entityPM.AgentReference2 = null;
            }

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.AgentName = card.EnglishName;

                if (entityPM.AgentContactId == null)
                {
                    entityPM.AgentContactId = card.PrimaryContactId;
                }

                if (entityPM.AgentAddressId == null)
                {
                    entityPM.AgentAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
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
            }

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.ShipperNotExporterName = card.EnglishName;

                if (entityPM.ShipperNotExporterContactId == null)
                {
                    entityPM.ShipperNotExporterContactId = card.PrimaryContactId;
                }

                if (entityPM.ShipperNotExporterAddressId == null)
                {
                    entityPM.ShipperNotExporterAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
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

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.CustomAgentImportName = card.EnglishName;

                if (entityPM.CustomAgentImportContactId == null)
                {
                    entityPM.CustomAgentImportContactId = card.PrimaryContactId;
                }

                if (entityPM.CustomAgentImportAddressId == null)
                {
                    entityPM.CustomAgentImportAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
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

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.ReleasingAgentName = card.EnglishName;

                if (entityPM.ReleasingAgentContactId == null)
                {
                    entityPM.ReleasingAgentContactId = card.PrimaryContactId;
                }

                if (entityPM.ReleasingAgentAddressId == null)
                {
                    entityPM.ReleasingAgentAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
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

            else
            {
                Card card = CardRepository.GetSingleCard(cardId, initializer.Tenant, true);

                entityPM.FreightForwarderName = card.EnglishName;

                if (entityPM.FreightForwarderContactId == null)
                {
                    entityPM.FreightForwarderContactId = card.PrimaryContactId;
                }

                if (entityPM.FreightForwarderAddressId == null)
                {
                    entityPM.FreightForwarderAddressId = initializer.AddressRepository.GetMainAddressId(cardId, initializer.Tenant);
                }
            }
        }
    }
}
