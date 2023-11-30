using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderTests.Models
{
    public class Vessel
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string EnglishName { get; set; }

        public string PartnerCode { get; set; }

        public string ComputingPartnerCode { get; set; }
    }
}
