using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoicePaymentQueryService : EntityQueryService<SupplierInvoicePayment, SupplierInvoicePaymentKeys, SupplierInvoicePaymentPM, SupplierInvoicePM, SupplierInvoiceKeys>
    {
 

        public List<SupplierInvoicePaymentPM> GetSupplierInvoicePaymentsForInvoice(string declarationId,int counterKey)
        {
            ICustomContext context = MainContext as CustomContext;
            List<SupplierInvoicePayment> mods = this.repository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = counterKey });
            List<SupplierInvoicePaymentPM> modPMs = new List<SupplierInvoicePaymentPM>();
            SupplierInvoicePaymentDataMapping mappings = new SupplierInvoicePaymentDataMapping();
            foreach (SupplierInvoicePayment mod in mods)
            {
                SupplierInvoicePaymentPM modPM = new SupplierInvoicePaymentPM();
                mappings.CustomPOCOToPM(modPM, mod);
                mappings.POCOToPM(modPM, mod);
                modPMs.Add(modPM);
            }
            return modPMs;

        }
    }
}
