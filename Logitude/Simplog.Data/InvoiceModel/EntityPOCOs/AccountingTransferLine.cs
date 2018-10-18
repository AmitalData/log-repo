using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class AccountingTransferLine
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string AccountingTransferHeaderId { get; set; }
        public string SearchFields { get; set; }
        public string EntityReference { get; set; }
        [ForeignKey("AccountingTransferHeaderId")]
        public virtual AccountingTransferHeader AccountingTransferHeader { get; set; }
    }
}
