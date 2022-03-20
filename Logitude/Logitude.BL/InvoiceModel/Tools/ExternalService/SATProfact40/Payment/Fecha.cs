using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class Fecha : SATComprobante
    {
        public static DateTime Get()
        {
            int tenant = arPaymentPM.Tenant;
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            return new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
        }
    }
}
