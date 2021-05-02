using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentProductItem
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ProductItemId { get; set; }
        public string Description { get; set; }
        public string HTSCode { get; set; }
        public virtual ProductItem ProductItem { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}
