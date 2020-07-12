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
    public partial class SupplierInvoiceUCRQueryService : EntityQueryService<SupplierInvoiceUCR, SupplierInvoiceUCRKeys, SupplierInvoiceUCRPM, SupplierInvoicePM, SupplierInvoiceKeys>
    {
        //public List<SupplierInvoicePaymentPM> GetSupplierInvoicePaymentsForDeclaration(string declarationId)
        //{
        //    ICustomContext context = MainContext as CustomContext;
        //    List<SupplierInvoicePayment> mods = this.repository.GeSupplierInvoicePaymentsForDeclaration(declarationId);
        //    List<SupplierInvoicePaymentPM> modPMs = new List<SupplierInvoicePaymentPM>();
        //    SupplierInvoicePaymentDataMapping mappings = new SupplierInvoicePaymentDataMapping();
        //    foreach (SupplierInvoicePayment mod in mods)
        //    {
        //        SupplierInvoicePaymentPM modPM = new SupplierInvoicePaymentPM();
        //        mappings.CustomPOCOToPM(modPM, mod);
        //        mappings.POCOToPM(modPM, mod);
        //        modPMs.Add(modPM);
        //    }
        //    return modPMs;

        //}

        public List<SupplierInvoiceUCRPM> GetSupplierInvoiceUCRsForInvoice(string declarationId,int counterKey)
        {
            ICustomContext context = MainContext as CustomContext;
            List<SupplierInvoiceUCR> ucrs = this.repository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = counterKey });
            List<SupplierInvoiceUCRPM> ucrPMs = new List<SupplierInvoiceUCRPM>();
            SupplierInvoiceUCRDataMapping mappings = new SupplierInvoiceUCRDataMapping();
            foreach (SupplierInvoiceUCR ucr in ucrs)
            {
                SupplierInvoiceUCRPM ucrPM = new SupplierInvoiceUCRPM();
                mappings.CustomPOCOToPM(ucrPM, ucr);
                mappings.POCOToPM(ucrPM, ucr);
                ucrPMs.Add(ucrPM);
            }
            return ucrPMs;

        }
    }
}
