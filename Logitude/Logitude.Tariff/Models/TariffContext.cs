using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Models
{
    public class TariffContext
    {
        public TariffPM TariffAirFreightCost { get; set; }
        public TariffPM TariffOceanLCLFreightCost { get; set; }
    }
}
