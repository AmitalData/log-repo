using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoNomBancoOrdExt : ComplementoPagosPago
    {
        public static string Get()
        {
            return IsPaymentMethodRequiredBankName() ? arPaymentPM.Bank : null;
        }

        public static bool IsPaymentMethodRequiredBankName(ARPaymentPM externalARPaymentPM = null)
        {
            if (arPaymentPM == null) arPaymentPM = externalARPaymentPM;
            if(commonContext == null) commonContext = CommonDataContext.GetContext(arPaymentPM.Tenant);

            string rfcEmisorCtaOrd = ComplementoPagosPagoRfcEmisorCtaOrd.Get(arPaymentPM);
            if (rfcEmisorCtaOrd != SATData.OutSideMexicoRfc) return false;
            
            List<string> paymentMethodCodesRequiredBankName = new List<string>
            {
                "02",
                "03",
                "04",
                "28",
                "29"
            };

            return paymentMethodCodesRequiredBankName.Any(paymentMethod => paymentMethod == arPaymentPM.SATPaymentMethodCode);
        }
    }
}
