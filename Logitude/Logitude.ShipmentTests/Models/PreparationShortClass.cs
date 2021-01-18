using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Models
{
    public class PreparationShortClass
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public double? Rate { get; set; }
        public string ForeignCurrencyId { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public string BaseCurrencyId { get; set; }
        public DateTime? ValueDate { get; set; }
    }
}
