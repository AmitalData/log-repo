using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.CRM.Data.Repsitories;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class DocumentsFilingQueryService
    {

        public DocumentsFilingPM DocumentsFilingCustomDataMappingAndValidating(DocumentsFiling documentsFiling, int Tenant, bool isNew, string ComputingPartnerCode = "")
        {

            DocumentsFilingValidating.Validate(documentsFiling);
            documentsFiling.DocumentType.Id = GetDocumentTypeId(documentsFiling, Tenant);

            DocumentsFilingPM temp = DocumentsFilingDataMappingAndValidatin(documentsFiling, Tenant);
            temp.Tenant = Tenant;
            temp.DirectionCode = "I";
            temp.HasFile = true;
            temp.ReceivedDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            temp.Received = true;
            temp.ReceivedByUserId = GetContact(Tenant).Id;
            temp.EntityId = GetEntityIdForType(documentsFiling, Tenant);
            temp.ReceivedByPartner = "External";
            return temp;

        }

        private static Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetContact(int Tenant)
        {
            ContactRepository contactRep = new ContactRepository(Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, Tenant);
            return loggedContact;
        }

        private string GetDocumentTypeId(DocumentsFiling documentsFiling, int tenant)
        {
            if (!string.IsNullOrEmpty(documentsFiling.DocumentType.Id) && string.IsNullOrEmpty(documentsFiling.DocumentType.Code))
                return documentsFiling.Id;

            var objectTableId = ObjectTableRepository.GetObjectTableByName(documentsFiling.EntityType.Name);
            if (objectTableId == null) throw new ApplicationException("Invalid entity type");

            var documentTypeId = new DocumentTypeRepository(tenant).GetDocumentTypeIdByCodeAndObjectTable(documentsFiling.DocumentType.Code, objectTableId, tenant);
            if (string.IsNullOrEmpty(documentTypeId))
                throw new ApplicationException("DocumentType with Code " + documentsFiling.DocumentType.Code + " doesn't exist");

            return documentTypeId;
        }

        public string GetEntityIdForType(DocumentsFiling documentsFiling, int tenant)
        {
            string entityId;
            switch (documentsFiling.EntityType.Name.ToLower())
            {
                case "shipment":
                    entityId = GetShipmentIdByShipmentNumber(documentsFiling.EntityNumber, tenant);
                    break;

                case "ticket":
                    TicketRepository ticketRepository = new TicketRepository(tenant);
                    entityId = ticketRepository.GetTicketId(documentsFiling.EntityNumber, tenant);
                    break;

                case "shipmentorder":
                    ShipmentOrderRepository shipmentRepository = new ShipmentOrderRepository(tenant);
                    entityId = shipmentRepository.GetIdByCode(documentsFiling.EntityNumber, tenant);
                    break;

                default:
                    throw new ApplicationException("Invalid entity type");

            }

            if (string.IsNullOrEmpty(entityId))
                throw new ApplicationException("Entity with Number " + documentsFiling.EntityNumber + " doesn't exist");

            return entityId;
        }

        public string GetShipmentIdByShipmentNumber(string shipmentNumber, int tenant)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            string shipmentId = shipmentRepository.GetShipmentIdByShipmentNumber(shipmentNumber, tenant);
            if (string.IsNullOrEmpty(shipmentId) && !string.IsNullOrEmpty(shipmentNumber) && shipmentNumber.ToUpper().StartsWith("A/") && FeatureToggleHelper.HasFeatureToggle("DFF ", tenant))
            {
                shipmentId = shipmentRepository.GetShipmentIdByShipmentNumber(("AF/" + shipmentNumber.Substring(2)), tenant);
            }
            return shipmentId;
        }
    }
}
