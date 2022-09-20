using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentDigitalFieldList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsCustomerArchived { get; set; }
    }
}
