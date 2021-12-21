using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class PayableProratedAmount
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string PayableId { get; set; }
        public string InvoiceId { get; set; }
        public double? ProratedAmountInLocalCurrency { get; set; }
        public double? ProratedAmountInProfitCurrency { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("PayableId")]
        public virtual ShipmentPayable ShipmentPayable { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual APInvoice APInvoice { get; set; }
    }
}
