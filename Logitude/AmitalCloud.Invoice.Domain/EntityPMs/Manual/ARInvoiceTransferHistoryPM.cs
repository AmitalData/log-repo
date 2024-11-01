using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmitalCloud.Invoice.Domain.EntityPMs
{
    public class ARInvoiceTransferHistoryPM
    {
        [Key]
        public string Id { get; set; }
        public string ARInvoiceId { get; set; }
        public string TransferNumber { get; set; }
        public DateTime? TransferDate { get; set; }
        public string FileName { get; set; }
    }
}