using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.DocumentTests.Services.Preparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services
{
    public class DocumentInService: DocumentService
    {
        public DocumentsFilingPM CreateDocument()
        {
            var arguments = GetCreateDocumentsFilingArgs(DirectionCodes.In);
            return APICaller.CallGet<DocumentsFilingPM>(DocumentAPIUrls.GetCreateDocumentsFiling(arguments), UserTenant.Token)?.Data;
        }
        public void UpdateDocumentReceived(DocumentsFilingPM document)
        {
            document.Received = true;
            document.ReceivedByUserId = UserTenant.UserId;
            document.ReceivedDate = DateTime.Now;
            document.UpdateDate = DateTime.Now;
            document.DirectionCode = DirectionCodes.In;
            document.CreatedByUserId = UserTenant.UserId;
            document.OwnerId = UserTenant.UserId;
            document.CreateDate = DateTime.Now;
            document.UpdatedByUserId = UserTenant.UserId;

            

        }

    }
}
