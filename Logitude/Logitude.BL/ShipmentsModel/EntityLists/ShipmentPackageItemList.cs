using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentPackageItemList
    {
        [Key]
        public string PackageId { get; set; }
        [Key]
        public int LineNumber { get; set; }

        public int Tenant { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? GoodsValue { get; set; }
    }
}