using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base
{
    internal class Serie
    {
        public static string Get(string counterPrefix)
        {
            string serie = "A";
            if (!string.IsNullOrEmpty(counterPrefix))
            {
                serie = counterPrefix;
            }

            return serie;
        }
    }
}
