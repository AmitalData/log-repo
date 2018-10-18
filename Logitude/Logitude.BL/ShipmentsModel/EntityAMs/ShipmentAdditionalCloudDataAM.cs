using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityAMs
{
    public class ShipmentAdditionalCloudDataAM
    {
        public string ShipmentNumber { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Remarks { get; set; }
        public string Direction { get; set; }
    }
}
