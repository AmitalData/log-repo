using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityPMs
{
    public class TariffLineExpirationDatePM
    {
        public string OriginPortId { get; set; }
        public string DestinationPortId { get; set; }
        public string ViaPortId { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
