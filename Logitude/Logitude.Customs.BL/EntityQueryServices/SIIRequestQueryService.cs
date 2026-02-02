using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var newPm = new SIIRequestPM
            {
                Tenant = tenant,
                DeclarationId = declarationId,
            };
            SIIRequestDataMapping mapping = new SIIRequestDataMapping();
            mapping.CustomPOCOToPM(newPm, newPo);
            return newPm;
        }


        public List<SupplieInvoiceItemsForSIIRequest> GetSupplierInvoiceItems(string declarationId, string siiRequestId, int tenant)
        {
            List<SupplieInvoiceItemsForSIIRequest> items = repository.GetSupplierInvoiceItems(declarationId, siiRequestId, tenant);
            return items;

        }
        public int GetSIIFormApplicationMaxNumber(int tenant)
        {
            return repository.GetSIIFormApplicationMaxNumber(tenant);
        }
        public override void GetComposition(EntityKeyFields entityKeys, SIIRequestPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            SIIRequestKeys sIIRequestKeys = entityKeys as SIIRequestKeys;
            SupplierInvoiceItemsReqListQueryService supplierInvoiceItemsReqListQueryService = new SupplierInvoiceItemsReqListQueryService(context);
            entityPM.SupplierInvoiceItemsReqLists = supplierInvoiceItemsReqListQueryService.GetMulti(sIIRequestKeys, false,true);
        }


    }
}
