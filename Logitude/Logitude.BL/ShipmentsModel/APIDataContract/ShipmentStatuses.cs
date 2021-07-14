using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public class ShipmentStatuses
    {
        public string HouseNumber { get; set; }
        public List<StatusDetails> Statuses { get; set; }
    }
}
