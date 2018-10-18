using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemsLevyQueryService
    {
        public List<SupplierInvoiceItemsLevyPM> GetSupplierInvoiceItemsLeviesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemsLevy> supplierInvoiceItemsLevies = repository.GetSupplierInvoiceItemsLeviesForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyPMs = (from a in supplierInvoiceItemsLevies
                                                                            select new SupplierInvoiceItemsLevyPM()
                                                                                 {
                                                                                     DeclarationId = a.DeclarationId,
                                                                                     InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                     TradeLevyExamptCode = a.TradeLevyExamptCode,
                                                                                     TradeLevyExamptName = a.TradeLevyExamptType != null ? a.TradeLevyExamptType.LocalName : null,
                                                                                     TradeLevyNumber = a.TradeLevyNumber,
                                                                                     InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                     LineNumber = a.LineNumber,
                                                                                     Tenant = a.Tenant,

                                                                                 }).ToList();
            return supplierInvoiceItemsLevyPMs;

        }

        public List<SupplierInvoiceItemsLevyPM> GetSupplierInvoiceItemsLeviesForSupplierInvoiceItem(string declarationId, int invoiceCounterKey,int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemsLevy> supplierInvoiceItemsLevies = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = invoiceCounterKey, LineNumber = lineNumber });
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyPMs = (from a in supplierInvoiceItemsLevies
                                                                            select new SupplierInvoiceItemsLevyPM()
                                                                            {
                                                                                DeclarationId = a.DeclarationId,
                                                                                InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                TradeLevyExamptCode = a.TradeLevyExamptCode,
                                                                                TradeLevyExamptName = a.TradeLevyExamptType != null ? a.TradeLevyExamptType.LocalName : null,
                                                                                TradeLevyNumber = a.TradeLevyNumber,
                                                                                InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                LineNumber = a.LineNumber,
                                                                                Tenant = a.Tenant,

                                                                            }).ToList();
            return supplierInvoiceItemsLevyPMs;

        }

        public List<SupplierInvoiceItemsLevyPM> GetSupplierInvoiceItemsLeviesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsLevy> supplierInvoiceItemsLevies = repository.GetSupplierInvoiceItemsLeviesForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyPMs = (from a in supplierInvoiceItemsLevies
                                                                            select new SupplierInvoiceItemsLevyPM()
                                                                            {
                                                                                DeclarationId = a.DeclarationId,
                                                                                InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                TradeLevyExamptCode = a.TradeLevyExamptCode,
                                                                                TradeLevyExamptName = a.TradeLevyExamptType != null ? a.TradeLevyExamptType.LocalName : null,
                                                                                TradeLevyNumber = a.TradeLevyNumber,
                                                                                InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                LineNumber = a.LineNumber,
                                                                                Tenant = a.Tenant,

                                                                            }).ToList();
            return supplierInvoiceItemsLevyPMs;

        }
    }
}
