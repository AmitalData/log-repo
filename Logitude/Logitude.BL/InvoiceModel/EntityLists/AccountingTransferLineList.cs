using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class AccountingTransferLineList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string EntityReference { get; set; }
        public string AccountingTransferHeaderId { get; set; }
        public string SearchFields { get; set; }
    }
}