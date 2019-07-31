using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class ARInvoiceStockLinePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARInvoiceStockId { get; set; }
        public string Number { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public bool IsUsed { get; set; }
        public string ARInvoiceId { get; set; }
        public string ShipmentNumber { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }        
    }
}
