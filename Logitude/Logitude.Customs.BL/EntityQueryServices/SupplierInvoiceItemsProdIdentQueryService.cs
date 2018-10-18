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
    public partial class SupplierInvoiceItemsProdIdentQueryService
    {
        public List<SupplierInvoiceItemsProdIdentPM> GetSupplierInvoiceItemsProdIdentsForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemsProdIdent> supplierInvoiceItemsProdIdents = repository.GetSupplierInvoiceItemsProdIdentsForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdentPMs = (from a in supplierInvoiceItemsProdIdents
                                                                                      select new SupplierInvoiceItemsProdIdentPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          Identification = a.Identification,
                                                                                          TypeName = a.ProductIdentificationType != null ? a.ProductIdentificationType.LocalName : null,
                                                                                      }).ToList();
            return supplierInvoiceItemsProdIdentPMs;

        }

        public List<SupplierInvoiceItemsProdIdentPM> GetSupplierInvoiceItemsProdIdentsForSupplierInvoiceItem(string declarationId, int invoiceCounterKey,int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemsProdIdent> supplierInvoiceItemsProdIdents = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = invoiceCounterKey, LineNumber = lineNumber });
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdentPMs = (from a in supplierInvoiceItemsProdIdents
                                                                                      select new SupplierInvoiceItemsProdIdentPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          Identification = a.Identification,
                                                                                          TypeName = a.ProductIdentificationType != null ? a.ProductIdentificationType.LocalName : null,
                                                                                      }).ToList();
            return supplierInvoiceItemsProdIdentPMs;

        }

        public List<SupplierInvoiceItemsProdIdentPM> GetSupplierInvoiceItemsProdIdentsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsProdIdent> supplierInvoiceItemsProdIdents = repository.GetSupplierInvoiceItemsProdIdentsForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdentPMs = (from a in supplierInvoiceItemsProdIdents
                                                                                      select new SupplierInvoiceItemsProdIdentPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          Identification = a.Identification,
                                                                                          TypeName = a.ProductIdentificationType != null ? a.ProductIdentificationType.LocalName : null,
                                                                                      }).ToList();
            return supplierInvoiceItemsProdIdentPMs;

        }
    }
}
