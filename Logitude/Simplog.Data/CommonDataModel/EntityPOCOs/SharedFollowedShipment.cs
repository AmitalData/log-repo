using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class SharedFollowedShipment
    {
    
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContactId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime TrackDate { get; set; }


        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }



        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

      
    }
}
