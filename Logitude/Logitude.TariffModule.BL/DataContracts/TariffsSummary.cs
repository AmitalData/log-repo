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
    }
}
