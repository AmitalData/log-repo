using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations;

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
        public string SKU { get; set; }
        public bool ApprovedByCustomer { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public virtual ProductItem ProductItem { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}
