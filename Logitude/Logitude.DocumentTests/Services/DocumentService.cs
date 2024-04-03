using Logitude.DocumentTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services
{
    public class DocumentService
    {
        public DocumentsFilingArgs GetCreateDocumentsFilingArgs(string directionCode)
        {
            return new DocumentsFilingArgs()
            {
                DocumentTypeId = DocumentData.DocumentTypeAirManifestId,
                EntityId = DocumentData.ShipmentId,
                ObjectTableId = DocumentData.ShipmentObjectTableId,
                DirectionCode = directionCode,
                Tenant = UserTenant.Tenant
            };
        }
    }
}
