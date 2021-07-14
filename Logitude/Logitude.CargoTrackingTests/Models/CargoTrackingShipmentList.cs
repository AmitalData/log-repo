using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTrackingTests.Models
{
    public class CargoTrackingShipmentList
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public decimal? GrossWeight { get; set; }
    }
}
