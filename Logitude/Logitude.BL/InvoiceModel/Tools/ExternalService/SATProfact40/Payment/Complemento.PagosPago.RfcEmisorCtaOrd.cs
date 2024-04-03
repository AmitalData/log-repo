using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoRfcEmisorCtaOrd : ComplementoPagosPago
    {
        public static string Get(ARPaymentPM arPaymentPM)
        {
            string billToCountryCode = SATBaseProfact40Service.GetBillToCountryCode(arPaymentPM.BillToAddressId, arPaymentPM.Tenant, commonContext);
            List<string> paymentMethods = new List<string> { "02", "03", "04", "28", "29" };
            bool isMexicoCountry = billToCountryCode == SATData.TranslationMexicoCountryCode || billToCountryCode == SATData.MexicoCountryCode;

            if (!isMexicoCountry && paymentMethods.Contains(arPaymentPM.SATPaymentMethodCode))
            {
                return SATData.OutSideMexicoRfc;
            }

            return null;
        }
    }
}
