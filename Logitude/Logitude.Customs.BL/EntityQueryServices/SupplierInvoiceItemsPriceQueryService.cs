using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemsPriceQueryService  
    {

        public List<SupplierInvoiceItemsPricePM> GetSupplierInvoiceItemsPricesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsPrice> supplierInvoiceItemsPrices = repository.GetSupplierInvoiceItemsPricesForDeclarationId(declarationId, invoiceCounterKey, itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsPricePM> supplierInvoiceItemsPricePMs = (from a in supplierInvoiceItemsPrices
                                                                              select new SupplierInvoiceItemsPricePM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          AdditionalPrice = a.AdditionalPrice,
                                                                                          AdditionalPriceTypeCode = a.AdditionalPriceTypeCode,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          AdditionalPriceTypeName = a.AdditionalPriceType != null ? a.AdditionalPriceType.LocalName : null,

                                                                              }).ToList();
            return supplierInvoiceItemsPricePMs;

        }

    
    }
}
