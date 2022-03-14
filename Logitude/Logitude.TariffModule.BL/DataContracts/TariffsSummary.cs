using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.DataContracts
{
   public class TariffsSummary
    {
        [Key]
        public int Id { get; set; }
        public int AirFreightCount { get; set; }
        public int AirSurchargeCount { get; set; }
        public int OceanSurchargeCount { get; set; }
        public int OceanLCLFreightCount { get; set; }
        public int OceanFCLFreightCount { get; set; }
        public int OceanFCLSurchargesCount { get; set; }
        public int ImportCustomsChargesCount { get; set; }
        public int ExportCustomsChargesCount { get; set; }
        public int InlandFTLTariffsCount { get; set; }
    }
}
