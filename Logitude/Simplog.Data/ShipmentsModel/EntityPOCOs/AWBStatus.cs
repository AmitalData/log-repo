using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class AWBStatus
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        //public List<ShipmentCarrierStatus> ShipmentCarrierStatus { get; set; }
        //public List<Shipment> Shipments { get; set; }
    }
}
