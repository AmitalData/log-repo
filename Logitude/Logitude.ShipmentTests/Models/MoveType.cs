using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Models
{
    public class MoveType
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string MoveTypeEnglishName { get; set; }
        public string MoveTypeLocalName { get; set; }
        public string TransportModeId { get; set; }
        public string Code { get; set; }
        public bool IsAir { get; set; }
        public bool IsInland { get; set; }
        public bool IsOcean { get; set; }
    }
}
