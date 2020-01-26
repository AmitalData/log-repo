using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.DataContracts
{
    public class WarehouseEntryPackageArgs
    {
        public string CustomerId { get; set; }
        public string WarehouseId { get; set; }
        public string ShipperConsigneesId { get; set; }
        public string ShipmentId { get; set; }
        public int Tenant { get; set; }

        public int DaysInWarehouseValue { get; set; }
        public string DaysInWarehouseOperatorFilterValue { get; set; }
    }
}
