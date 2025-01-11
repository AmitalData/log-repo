using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Invoice.Domain.EntityPMs
{
    public class APInvoiceTransferHistoryPM
    {
        [Key]
        public string Id { get; set; }
        public string APInvoiceId { get; set; }
        public string TransferNumber { get; set; }
        public DateTime? TransferDate { get; set; }
        public string FileName { get; set; }
    }
}
