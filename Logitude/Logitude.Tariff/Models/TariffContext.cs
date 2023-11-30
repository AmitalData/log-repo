using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Models
{
    public class TariffContext
    {
        public TariffPM AirFreightCost { get; set; }
        public TariffPM OceanLCLFreightCost { get; set; }
        public TariffPM OceanFCLFreightCost { get; set; }
        public TariffPM AirSurchargeCost { get; set; }
        public TariffPM OceanLCLSurchargeCost { get; set; }
        public TariffPM OceanFCLSurchargeCost { get; set; }
    }
}
