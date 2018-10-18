using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentCommodity
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public double? ChargeRate { get; set; }
        public double? ChargeAmount { get; set; }
        public string RateClassCode { get; set; }
        public string CommodityNumber { get; set; }
        public string DescriptionOfGoods { get; set; }

        public bool IsFirstLine { get; set; }

        public double? Volume { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public int? NumberOfPackages { get; set; }

        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; }

        [ForeignKey("RateClassCode")]
        public RateClass RateClass { get; set; }
    }
}
