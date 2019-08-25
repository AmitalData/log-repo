using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.DataContracts
{
    public class SurchargeSummary
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ChargeTypeId { get; set; }
        public string TariffId { get; set; }
        public string CurrencyId { get; set; }
        public string TariffNumber { get; set; }
        public decimal? Price { get; set; }
    }
}
