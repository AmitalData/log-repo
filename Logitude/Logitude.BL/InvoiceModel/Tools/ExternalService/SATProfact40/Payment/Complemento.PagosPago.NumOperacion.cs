using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoNumOperacion : ComplementoPagosPago
    {
        public static string Get(ARPaymentPM arPaymentPM)
        {
            const string cashPaymentMethodCode = "CC";
            const string bankTransferPaymentMethodCode = "BT";
            const string chequePaymentMethodCode = "CH";
            const int chequeOrPaymentRefMaxLength = 100;

            List<string> numOperacionPaymentMethodCodes = new List<string>
            {
                cashPaymentMethodCode,
                bankTransferPaymentMethodCode,
                chequePaymentMethodCode
            };

            if (!string.IsNullOrEmpty(arPaymentPM.ChequeOrPaymentRef) && numOperacionPaymentMethodCodes.Contains(arPaymentPM.AccountingPaymentMethodCode))
            {
                return StringHelper.TruncateLongString(arPaymentPM.ChequeOrPaymentRef, chequeOrPaymentRefMaxLength);
            }

            return null;
        }
    }
}
