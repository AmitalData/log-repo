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
    public class DocumentInService: DocumentService
    {
        public DocumentsFilingPM CreateDocument()
        {
            var arguments = GetCreateDocumentsFilingArgs(DirectionCodes.In);
            return APICaller.CallGet<DocumentsFilingPM>(Urls.GetCreateDocumentsFiling(arguments), UserTenant.Token)?.Data;
        }
        public void UpdateDocumentReceived(DocumentsFilingPM document)
        {
            document.Received = true;
            document.ReceivedByUserId = UserTenant.UserId;
            document.ReceivedDate = DateTime.Now;
            document.UpdateDate = DateTime.Now;

        }

    }
}
