using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ShipmentDigitalArchivedArgs
    {
        public string CardId { get; set; }
        public bool IsCustomerArchived { get; set; }
        public string ShipmentId { get; set; }
    }
}