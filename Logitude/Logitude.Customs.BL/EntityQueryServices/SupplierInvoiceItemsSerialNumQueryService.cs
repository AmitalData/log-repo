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
    public partial class SupplierInvoiceItemsSerialNumQueryService
    {
        public List<SupplierInvoiceItemsSerialNumPM> GetSupplierInvoiceItemsSerialNumsForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemsSerialNum> supplierInvoiceItemsSerialNums = repository.GetSupplierInvoiceItemsSerialNumsForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumPMs = (from a in supplierInvoiceItemsSerialNums
                                                                                      select new SupplierInvoiceItemsSerialNumPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          SerialNumber = a.SerialNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          TypeName = a.CargoIdentityQualifier != null ? a.CargoIdentityQualifier.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                      }).ToList();
            return supplierInvoiceItemsSerialNumPMs;

        }

        public List<SupplierInvoiceItemsSerialNumPM> GetSupplierInvoiceItemsSerialNumsForSupplierInvoiceItem(string declarationId, int invoiceCounterKey, int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemsSerialNum> supplierInvoiceItemsSerialNums = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId=declarationId,CounterKey=invoiceCounterKey,LineNumber=lineNumber});
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumPMs = (from a in supplierInvoiceItemsSerialNums
                                                                                      select new SupplierInvoiceItemsSerialNumPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          SerialNumber = a.SerialNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          TypeName = a.CargoIdentityQualifier != null ? a.CargoIdentityQualifier.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                      }).ToList();
            return supplierInvoiceItemsSerialNumPMs;

        }

        public List<SupplierInvoiceItemsSerialNumPM> GetSupplierInvoiceItemsSerialNumsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsSerialNum> supplierInvoiceItemsSerialNums = repository.GetSupplierInvoiceItemsSerialNumsForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumPMs = (from a in supplierInvoiceItemsSerialNums
                                                                                      select new SupplierInvoiceItemsSerialNumPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          SerialNumber = a.SerialNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          TypeName = a.CargoIdentityQualifier != null ? a.CargoIdentityQualifier.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                      }).ToList();
            return supplierInvoiceItemsSerialNumPMs;

        }
    }
}
