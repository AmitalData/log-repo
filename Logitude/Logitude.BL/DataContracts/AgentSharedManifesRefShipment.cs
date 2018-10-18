using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
   public class AgentSharedManifesRefShipment
    {
        public string ShipmentId { get; set; }
        public string AgentSharedManifestRef { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentNumber { get; set; }
        public bool IsShipmentShared { get; set; }
    }
}
