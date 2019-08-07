using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityPMs
{
    public class TariffLineExpirationDatePM
    {
        public string TariffLineId { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
