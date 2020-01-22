using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class UserIdNumberRequestPM
    {
        public string ForwarderShipmentNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Master { get; set; }
        public string Hawb { get; set; }
        public string DeclarationNumber { get; set; }
        public string ShipmentValueInNIS { get; set; }
        public string SenderDetails { get; set; }
        public string GoodsDescritpion { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public int Tenant { get; set; }


    }
}
