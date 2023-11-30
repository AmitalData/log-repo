using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;

namespace WebFreight.Web.Helpers.ImporterShipmentOrderDocuments
{
    public class ShipmentOrderDocumentAmMapping
    {
        private readonly int tenant;
        private readonly ObjectTableRepository objectTabelRepository;
        private DocumentTypeRepository documentTypeRepository;
        private ContactQuery contactQuery;

        public ShipmentOrderDocumentAmMapping(int tenant)
        {
            this.tenant = tenant;
            objectTabelRepository = new ObjectTableRepository(tenant);
            var commoncontext = CommonDataContext.GetContext(tenant);
            documentTypeRepository = new DocumentTypeRepository(commoncontext);
            contactQuery = new ContactQuery(tenant);
        }

        public DocumentsFilingPM Map(DocumentsFilingAM documentsFilingAM, ShipmentPM shipment)
        {
            DocumentsFilingPM documentsFilingPM = new DocumentsFilingPM();
            documentsFilingPM.Tenant = tenant;
            documentsFilingPM.ObjectTableId = GetShipmentObjectTableId();
            documentsFilingPM.IsDeleted = documentsFilingAM.IsDeleted;
            documentsFilingPM.DocumentId = documentsFilingAM.DocumentId;
            documentsFilingPM.Notes = documentsFilingAM.Notes;
            documentsFilingPM.EntityId = shipment.Id;
            documentsFilingPM.Description = documentsFilingAM.Description;
            documentsFilingPM.FileData = documentsFilingAM.FileData;
            documentsFilingPM.FileExtension = documentsFilingAM.Extension;
            documentsFilingPM.SignersList = documentsFilingAM.SignersList;
            documentsFilingPM.IsRequested = documentsFilingAM.IsRequested;
            documentsFilingPM.IsDigitallySigned = documentsFilingAM.IsDigitallySigned;
            documentsFilingPM.DirectionCode = "I";
            documentsFilingPM.FileSize = documentsFilingAM.FileSize;
            documentsFilingPM.FileName = documentsFilingAM.FileName;
            documentsFilingPM.DontAddToQueue = true;
            documentsFilingPM.IsSharedWithCustomer = documentsFilingAM.IsSharedWithCustomer;
            documentsFilingPM.DocumentTypeId = GetDocumentTypeId(documentsFilingAM.DocumentType.Code);

            var loggedContact = GetLoggedContact();
            documentsFilingPM.CreatedByUserId = loggedContact.Id;
            documentsFilingPM.OwnerId = loggedContact.Id;
            documentsFilingPM.UpdatedByUserId = loggedContact.Id;
            documentsFilingPM.EntityReference = shipment.ShipmentNumber;

            return documentsFilingPM;
        }

        private ContactPM GetLoggedContact()
        {
            string systemEmail = "system@tenant" + tenant + ".com";
            var loggedContact = contactQuery.GetContactByNameAndTenant(systemEmail, tenant, true);
            if (loggedContact == null) loggedContact = contactQuery.GetContactByEmailOnly(systemEmail, tenant);
            return loggedContact;
        }

        private string GetDocumentTypeId(string documentTypeCode)
        {
            var documentTypeId = documentTypeRepository.GetSingleDocumentTypeByCode(documentTypeCode, tenant)?.Id;
            if (documentTypeId == null) throw new Exception("DocumentTypeId field doesn't exist in the database, insert this entity before using it.");
            return documentTypeId;
        }

        private string GetShipmentObjectTableId()
        {
            return objectTabelRepository.GetObjectTableByName("Shipment", tenant, true).Id;
        }
    }
}