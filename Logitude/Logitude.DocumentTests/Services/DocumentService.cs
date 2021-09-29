using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.DocumentTests.Services.Preparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services
{
    public class DocumentService
    {
        public DocumentsFilingPM CreateDocument(string directionCode)
        {
            var arguments = GetCreateDocumentsFilingArgs(directionCode);
            return APICaller.CallGet<DocumentsFilingPM>(Urls.GetCreateDocumentsFiling(arguments), UserTenant.Token)?.Data;
        }
        public GetCreateDocumentsFilingArgs GetCreateDocumentsFilingArgs(string directionCode)
        {
            return new GetCreateDocumentsFilingArgs()
            {
                DocumentTypeId = DocumentData.DocumentTypeAirManifestId,
                EntityId = DocumentData.ShipmentId,
                ObjectTableId = DocumentData.ShipmentObjectTableId,
                DirectionCode = directionCode,
                Tenant = UserTenant.Tenant
            };
        }
        public void UpdateDocumentReceived(DocumentsFilingPM document)
        {
            document.Received = true;
            document.ReceivedByUserId = UserTenant.UserId;
            document.ReceivedDate = DateTime.Now;
            document.UpdateDate = DateTime.Now;

        }

        public DocumentsFilingPM GetDocumentCopyId(string documentTypeId, string documentId, int tenant)
        {
            var typeCopy = GetDocumentTypeAirManifestTemplateCopyId(documentTypeId, documentId, tenant);
            typeCopy.DocumentTypeCopies.Should().NotBeEmpty();
            var args = new GetDocumentCopyArgs()
            {
                DocumentTypeId = documentTypeId,
                EntityId = DocumentData.ShipmentId,
                EntityObjectTableId = DocumentData.ShipmentObjectTableId,
                DocumentOutId = documentId,
                Tenant = tenant,
                DocumentTypeCopyId = typeCopy.DocumentTypeCopies[0].Id,
                UserId = UserTenant.UserId
            };
            return APICaller.CallGet<DocumentsFilingPM>(Urls.GetDocumentCopy(args), UserTenant.Token).Data;
        }

        public DocumentTypePM GetDocumentTypeAirManifestTemplateCopyId(string documentTypeId, string documentId, int tenant)
        {
            return APICaller.CallGet<DocumentTypePM>(Urls.GetDocumentType(documentTypeId, documentId, tenant), UserTenant.Token).Data;

        }
    }
}
