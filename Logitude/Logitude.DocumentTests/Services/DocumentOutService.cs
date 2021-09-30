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
    public class DocumentOutService: DocumentService
    {
        public string GetDocumentOutSecurityId()
        {
            var documentOut = GetDocumentOut();
            return documentOut.SecurityId;
        }
        public DocumentOutPM GetDocumentOut()
        {
            var documentOut = CreateDocumentOut();
            var DocumentOutCopyId = GetDocumentOutCopyId(DocumentData.DocumentTypeAirManifestId, documentOut.Id, UserTenant.Tenant);
            documentOut = UpdateOutDocument(documentOut, DocumentOutCopyId);
            return documentOut;
        }

        public string SendDocumentOut(DocumentOutPM documentOut)
        {
            var htmlFilter = new SendHtmlFilter()
            {
                Htmlstring = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body><p style=\"font-size: 16px; overflow-wrap: break-word;\">Integration Test</p></body></html>",
                InternalDocumentId = documentOut.Id,
                ToEmail = "ahmadb@logitudeworld.com",
                Tenant = UserTenant.Tenant,
                UserId = UserTenant.UserId,
                ObjectTableId = documentOut.ObjectTableId,
                EntityId = documentOut.EntityId,
                Subject= "Air Manifest",
                Attachments = documentOut.DocumentOutCopies[0].Id,
                ObjectTableName= "Shipment",
            };
            return APICaller.CallPost<string>(htmlFilter,Urls.PostSendHtmlDocument, UserTenant.Token)?.Data;

        }

        public DocumentOutPM CreateDocumentOut()
        {
            var arguments = GetCreateDocumentsFilingArgs(DirectionCodes.Out);
            return APICaller.CallGet<DocumentOutPM>(Urls.GetCreateDocumentsFiling(arguments), UserTenant.Token)?.Data;
        }
        
        public string GetDocumentOutCopyId(string documentTypeId, string documentId, int tenant)
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
            return APICaller.CallGet<string>(Urls.GetDocumentCopy(args), UserTenant.Token).Data;
        }

        public DocumentOutPM UpdateOutDocument(DocumentOutPM documentOut, string documentOutCopyId)
        {
            documentOut.IssuedByUserId = UserTenant.UserId;
            documentOut.Issued = true;
            documentOut.IsChangeIssuedDate = true;
            var documentOutCopy = new DocumentOutCopyPM()
            {
                Id = documentOutCopyId,
                Tenant = UserTenant.Tenant,
                DocumentId = documentOutCopyId,
                DocumentOutId = documentOut.Id,
                FileName = "Air Manifest",
                CalculatedFileName = "Air Manifest"
            };
            documentOut.DocumentOutCopies = new List<DocumentOutCopyPM>() { documentOutCopy };
            return APICaller.CallPut<DocumentOutPM>(documentOut, Urls.PutDocumentOut, UserTenant.Token).Data;
        }

        public DocumentTypePM GetDocumentTypeAirManifestTemplateCopyId(string documentTypeId, string documentId, int tenant)
        {
            return APICaller.CallGet<DocumentTypePM>(Urls.GetDocumentType(documentTypeId, documentId, tenant), UserTenant.Token).Data;

        }
    }
}
