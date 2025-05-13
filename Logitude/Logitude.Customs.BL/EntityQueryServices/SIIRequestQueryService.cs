using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityDataMappings;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Logitude.Customs.Data.DataContracts;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SIIRequestQueryService : EntityQueryService<SIIRequest, SIIRequestKeys, SIIRequestPM, object, SIIRequestKeys>
    {
        public SIIRequestPM GetSinglePM(string siiRequestId,
                                   string declarationId,
                                   int tenant)
        {
            
            var newPo = new SIIRequest
            {
                Tenant = tenant,
                DeclarationId = declarationId,
            };
            
            var newPm = new SIIRequestPM();
            SIIRequestDataMapping mapping = new SIIRequestDataMapping();
            mapping.CustomPOCOToPM(newPm, newPo);
            mapping.POCOToPM(newPm, newPo);
            return newPm;
        }


        public List<SupplieInvoiceItemsForSIIRequest> GetSupplierInvoiceItems(string declarationId, int tenant)
        {
            List<SupplieInvoiceItemsForSIIRequest> items= repository.GetSupplierInvoiceItems(declarationId, tenant);
            return items;

        }

    }
}
