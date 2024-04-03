using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ARInvoiceChargesConstraint
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InvoiceLineId { get; set; }
        public string ReceivableId { get; set; }        
        public DateTime CreateDate { get; set; }

        [ForeignKey("InvoiceLineId")]
        public virtual ARInvoiceLine ARInvoiceLine { get; set; }

        [ForeignKey("ReceivableId")]
        public virtual ShipmentReceivable ShipmentReceivable { get; set; }
    }
}
