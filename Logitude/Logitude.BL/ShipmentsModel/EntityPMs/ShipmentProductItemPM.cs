using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentProductItemPM
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
        public string ASIN { get; set; }
        public string UPC { get; set; }
        public string OriginCountryId { get; set; }
        public string OriginCountryName { get; set; }
        public bool IsEmptyLine { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
