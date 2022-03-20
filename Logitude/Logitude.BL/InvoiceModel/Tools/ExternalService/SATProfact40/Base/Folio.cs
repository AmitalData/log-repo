using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base
{
    internal class Folio
    {
        public static string Get(string entityNumber, string counterPrefix)
        {
            string folio = entityNumber;

            if (string.IsNullOrEmpty(counterPrefix)) return folio;

            if (string.IsNullOrEmpty(entityNumber)) return entityNumber;

            if (entityNumber.Contains(counterPrefix))
            {
                folio = entityNumber.Remove(0, counterPrefix.Length);
            }

            return folio;
        }
    }
}
