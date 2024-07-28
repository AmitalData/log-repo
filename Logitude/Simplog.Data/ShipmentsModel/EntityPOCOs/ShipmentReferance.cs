using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Collections.Generic;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{

    public class ShipmentReferance
    {
        [Key]
        public string ShipmentId { get; set; }
        [Key]
        public int Tenant { get; set; }
        [Key]
        public int LineNumber { get; set; }
        public string ReferenceType { get; set; }
        public string PartnerId { get; set; }
        public string ReferenceValue { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
        [ForeignKey("PartnerId")]
        public virtual Card Card { get; set; }
        [ForeignKey("ReferenceType")]
        public virtual ReferenceType ReferenceTypeCode { get; set; }
    }
}