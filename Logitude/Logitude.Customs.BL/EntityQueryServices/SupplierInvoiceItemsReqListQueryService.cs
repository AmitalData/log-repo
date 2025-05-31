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
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemsReqListQueryService : EntityQueryService<SupplierInvoiceItemsReqList, SupplierInvoiceItemsReqListKeys, SupplierInvoiceItemsReqListPM, object, SupplierInvoiceItemsReqListKeys>
    {
        public SupplierInvoiceItemsReqListPM GetOrCreate(string siiRequestId, string declarationid, int linenumber, int invoicecounterkey, int invoiceitemlinenumber,int tenant)
        {
            var pm = GetSingle(declarationid, linenumber, siiRequestId, invoicecounterkey, invoiceitemlinenumber, true, false);
            if(pm != null)
            {
                return pm;
            }

            var newPo = new SupplierInvoiceItemsReqList
            {
                Tenant = tenant,
                DeclarationId = declarationid,
                LineNumber = 1,  // not clear what linenumber shud be right now so we return 1 for now -- linenumber is key 
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
