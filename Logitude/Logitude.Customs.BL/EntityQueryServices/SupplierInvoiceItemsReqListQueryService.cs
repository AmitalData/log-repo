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
    public partial class SupplierInvoiceItemsReqListQueryService : EntityQueryService<SupplierInvoiceItemsReqList, SupplierInvoiceItemsReqListKeys, SupplierInvoiceItemsReqListPM, SupplierInvoiceItemPM, SupplierInvoiceItemKeys>
    {
        public SupplierInvoiceItemsReqListPM GetSinglePM(string siiRequestId, string declarationid, int linenumber, int invoicecounterkey, int invoiceitemlinenumber,int tenant)
        {
            
            var newPo = new SupplierInvoiceItemsReqList
            {
                Tenant = tenant,
                DeclarationId = declarationid,
                LineNumber = linenumber,
                InvoiceCounterKey = invoicecounterkey,
                InvoiceItemLineNumber = invoiceitemlinenumber,
            };
            
            var newPm = new SupplierInvoiceItemsReqListPM();
            SupplierInvoiceItemsReqListDataMapping mapping = new SupplierInvoiceItemsReqListDataMapping();
            mapping.CustomPOCOToPM(newPm, newPo);
            mapping.POCOToPM(newPm, newPo);
            return newPm;
        }
    }
}
