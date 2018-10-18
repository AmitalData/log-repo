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
    public partial class SupplierInvoiceFreightAmountQueryService
    {
        public List<SupplierInvoiceFreightAmountPM> GetSupplierInvoiceFreightAmountsByInvoice(string declarationId, int counterKey)
        {
            List<SupplierInvoiceFreightAmount> pocos = repository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = counterKey });
            List<SupplierInvoiceFreightAmountPM> pms = new List<SupplierInvoiceFreightAmountPM>();
            foreach (SupplierInvoiceFreightAmount item in pocos)
            {
                SupplierInvoiceFreightAmountPM pm = new SupplierInvoiceFreightAmountPM()
                {
                    Amount = item.Amount,
                    CurrencyTypeCode = item.CurrencyTypeCode,
                    DeclarationId = item.DeclarationId,
                    InvoiceCounterKey = item.InvoiceCounterKey,
                    Tenant = item.Tenant,

                };
                pms.Add(pm);
            }
            return pms;
        }

    }
}
