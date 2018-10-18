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
    public partial class SupplierInvoiceItemsDescriptQueryService
    {
        public List<SupplierInvoiceItemsDescriptPM> GetSupplierInvoiceItemsDescriptsForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemsDescript> supplierInvoiceItemsDescripts = repository.GetSupplierInvoiceItemsDescriptsForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptPMs = (from a in supplierInvoiceItemsDescripts
                                                                                    select new SupplierInvoiceItemsDescriptPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          TypeCode = a.TypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          Description = a.Description,
                                                                                          TypeName = a.ProductNameType != null ? a.ProductNameType.LocalName : null,

                                                                                      }).ToList();
            return supplierInvoiceItemsDescriptPMs;

        }

        public List<SupplierInvoiceItemsDescriptPM> GetSupplierInvoiceItemsDescriptsForSupplierInvoiceItem(string declarationId, int invoiceCounterKey,int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemsDescript> supplierInvoiceItemsDescripts = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId=declarationId,CounterKey=invoiceCounterKey,LineNumber=lineNumber});
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptPMs = (from a in supplierInvoiceItemsDescripts
                                                                                    select new SupplierInvoiceItemsDescriptPM()
                                                                                    {
                                                                                        DeclarationId = a.DeclarationId,
                                                                                        InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                        TypeCode = a.TypeCode,
                                                                                        InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                        LineNumber = a.LineNumber,
                                                                                        Tenant = a.Tenant,
                                                                                        Description = a.Description,
                                                                                        TypeName = a.ProductNameType != null ? a.ProductNameType.LocalName : null,

                                                                                    }).ToList();
            return supplierInvoiceItemsDescriptPMs;

        }

        public List<SupplierInvoiceItemsDescriptPM> GetSupplierInvoiceItemsDescriptsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsDescript> supplierInvoiceItemsDescripts = repository.GetSupplierInvoiceItemsDescriptsForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey, itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptPMs = (from a in supplierInvoiceItemsDescripts
                                                                                    select new SupplierInvoiceItemsDescriptPM()
                                                                                    {
                                                                                        DeclarationId = a.DeclarationId,
                                                                                        InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                        TypeCode = a.TypeCode,
                                                                                        InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                        LineNumber = a.LineNumber,
                                                                                        Tenant = a.Tenant,
                                                                                        Description = a.Description,
                                                                                        TypeName = a.ProductNameType != null ? a.ProductNameType.LocalName : null,

                                                                                    }).ToList();
            return supplierInvoiceItemsDescriptPMs;

        }
    }
}
