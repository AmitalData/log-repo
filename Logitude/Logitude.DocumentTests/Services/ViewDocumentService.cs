using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services
{
    public class ViewDocumentService
    {
        public string GetDocumentWithfileSecurityId()
        {
            var documentService = new DocumentService();
            var documentFileService = new DocumentFileService();
            var document = documentService.CreateDocument(DirectionCodes.In);
            documentFileService.Uploadfile(document);
            return document.SecurityId;

        }
    }
}
