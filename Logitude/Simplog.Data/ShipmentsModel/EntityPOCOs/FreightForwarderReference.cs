using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Collections.Generic;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{

    public class FreightForwarderReference
    {
        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ShipmentId { get; set; }
		[Key]
		public int LineNumber { get; set; }
		public string ForwarderShipmentNumber { get; set; }
        public bool ForwarderFileConnect { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
    }
}