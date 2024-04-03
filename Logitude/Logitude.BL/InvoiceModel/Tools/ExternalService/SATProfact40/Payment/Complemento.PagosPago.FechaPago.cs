using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoFechaPago : ComplementoPagosPago
    {
        public static DateTime Get(ARPaymentPM arPaymentPM)
        {
            DateTime fechaPago = arPaymentPM.RegisterDate.Value;

            TimeSpan time = new TimeSpan(12, 00, 00);
            DateTime resultdate = fechaPago.Date + time;
            fechaPago = resultdate;
            if (arPaymentPM.FechaPago != null)
            {
                fechaPago = arPaymentPM.FechaPago.Value;
            }

            return fechaPago;
        }
    }
}
