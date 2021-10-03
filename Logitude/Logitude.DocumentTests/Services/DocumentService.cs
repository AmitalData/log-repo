using Logitude.DocumentTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services
{
    public class DocumentService
    {
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
    }
}
