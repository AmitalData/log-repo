using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentStoragePricing
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string WarehouseId { get; set; }
        public int StepFrom { get; set; }
        public int? StepTo { get; set; }
        public int? Days { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Amount { get; set; }
        public int LineNumber { get; set; }
        public int? ChargeableDays { get; set; }

        public virtual Shipment Shipment { get; set; }
        public virtual Card Warehouse { get; set; }
    }
}
