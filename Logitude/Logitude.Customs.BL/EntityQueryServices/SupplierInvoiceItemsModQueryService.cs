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
    public partial class SupplierInvoiceItemsModQueryService
    {
        public List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsModsForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemsMod> SupplierInvoiceItemsMods = repository.GetSupplierInvoiceItemsModsForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemsModPM> SupplierInvoiceItemsModPms = (from a in SupplierInvoiceItemsMods
                                                                          select new SupplierInvoiceItemsModPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          Amount = a.Amount,
                                                                                          CurrencyTypeCode = a.CurrencyTypeCode,
                                                                                          CurrencyTypeName = a.CurrencyType != null ? a.CurrencyType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          ModificationCounterKey = a.ModificationCounterKey,
                                                                                          TypeCode = a.TypeCode,
                                                                                          TypeName = a.ModificationAndDiscountType != null ? a.ModificationAndDiscountType.LocalName : null,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                           
                                                                                      }).ToList();
            return SupplierInvoiceItemsModPms;
        }

        public List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsModsForSupplierInvoiceItem(string declarationId, int invoiceCounterKey,int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemsMod> SupplierInvoiceItemsMods = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId=declarationId,CounterKey=invoiceCounterKey,LineNumber=lineNumber});
            List<SupplierInvoiceItemsModPM> SupplierInvoiceItemsModPms = (from a in SupplierInvoiceItemsMods
                                                                          select new SupplierInvoiceItemsModPM()
                                                                          {
                                                                              DeclarationId = a.DeclarationId,
                                                                              Amount = a.Amount,
                                                                              CurrencyTypeCode = a.CurrencyTypeCode,
                                                                              CurrencyTypeName = a.CurrencyType != null ? a.CurrencyType.LocalName : null,
                                                                              InvoiceCounterKey = a.InvoiceCounterKey,
                                                                              ModificationCounterKey = a.ModificationCounterKey,
                                                                              TypeCode = a.TypeCode,
                                                                              TypeName = a.ModificationAndDiscountType != null ? a.ModificationAndDiscountType.LocalName : null,
                                                                              LineNumber = a.LineNumber,
                                                                              Tenant = a.Tenant,

                                                                          }).ToList();
            return SupplierInvoiceItemsModPms;
        }

        public List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsModsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemsMod> SupplierInvoiceItemsMods = repository.GetSupplierInvoiceItemsModsForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey, itemsLineNumbers, tenant);
            List<SupplierInvoiceItemsModPM> SupplierInvoiceItemsModPms = (from a in SupplierInvoiceItemsMods
                                                                          select new SupplierInvoiceItemsModPM()
                                                                          {
                                                                              DeclarationId = a.DeclarationId,
                                                                              Amount = a.Amount,
                                                                              CurrencyTypeCode = a.CurrencyTypeCode,
                                                                              CurrencyTypeName = a.CurrencyType != null ? a.CurrencyType.LocalName : null,
                                                                              InvoiceCounterKey = a.InvoiceCounterKey,
                                                                              ModificationCounterKey = a.ModificationCounterKey,
                                                                              TypeCode = a.TypeCode,
                                                                              TypeName = a.ModificationAndDiscountType != null ? a.ModificationAndDiscountType.LocalName : null,
                                                                              LineNumber = a.LineNumber,
                                                                              Tenant = a.Tenant,

                                                                          }).ToList();
            return SupplierInvoiceItemsModPms;
        }
    }
}
