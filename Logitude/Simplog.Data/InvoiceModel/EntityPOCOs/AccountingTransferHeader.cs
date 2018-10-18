using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class AccountingTransferHeader
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TransferNumber { get; set; }
        public DateTime? TransferDate { get; set; }
        public string FileName { get; set; }
        public string AccountingTransferTypeCode { get; set; }
        public string SearchFields { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("AccountingTransferTypeCode")]
        public virtual AccountingTransferType TransferType { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }        
    }
}
