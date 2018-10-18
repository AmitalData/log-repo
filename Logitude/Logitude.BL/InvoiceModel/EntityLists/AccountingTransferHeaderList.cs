using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class AccountingTransferHeaderList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TransferNumber { get; set; }
        public DateTime? TransferDate { get; set; }
        public string FileName { get; set; }
        public string AccountingTransferTypeCode { get; set; }
        public string AccountingTransferTypeName { get; set; }
        public string SearchFields { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
    }
}