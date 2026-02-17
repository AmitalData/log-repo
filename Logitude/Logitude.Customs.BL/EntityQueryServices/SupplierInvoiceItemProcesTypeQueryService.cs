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
    public partial class SupplierInvoiceItemProcesTypeQueryService
    {
        public List<SupplierInvoiceItemProcesTypePM> GetSupplierInvoiceItemProcesTypesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemProcesType> supplierInvoiceItemProcesTypes = repository.GetSupplierInvoiceItemProcesTypesForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemProcesTypePMs = (from a in supplierInvoiceItemProcesTypes
                                                                                      select new SupplierInvoiceItemProcesTypePM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          ProcessTypeCode = a.ProcessTypeCode,
                                                                                          ProcessTypeName = a.ProcessType != null ? a.ProcessType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,

                                                                                      }).ToList();
            return supplierInvoiceItemProcesTypePMs;

        }

        public List<SupplierInvoiceItemProcesTypePM> GetSupplierInvoiceItemProcesTypesForSupplierInvoiceItem(string declarationId, int invoiceCounterKey, int lineNumber,int tenant)
        {
            List<SupplierInvoiceItemProcesType> supplierInvoiceItemProcesTypes = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = invoiceCounterKey, LineNumber = lineNumber });
            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemProcesTypePMs = (from a in supplierInvoiceItemProcesTypes
                                                                                      select new SupplierInvoiceItemProcesTypePM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          ProcessTypeCode = a.ProcessTypeCode,
                                                                                          ProcessTypeName = a.ProcessType != null ? a.ProcessType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,

                                                                                      }).ToList();
            return supplierInvoiceItemProcesTypePMs;

        }

        public List<SupplierInvoiceItemProcesTypePM> GetSupplierInvoiceItemProcesTypesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemProcesType> supplierInvoiceItemProcesTypes = repository.GetSupplierInvoiceItemProcesTypesForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);
            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemProcesTypePMs = (from a in supplierInvoiceItemProcesTypes
                                                                                      select new SupplierInvoiceItemProcesTypePM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          ProcessTypeCode = a.ProcessTypeCode,
                                                                                          ProcessTypeName = a.ProcessType != null ? a.ProcessType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,

                                                                                      }).ToList();
            return supplierInvoiceItemProcesTypePMs;

        }
    }
}
