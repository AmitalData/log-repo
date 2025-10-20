using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ExpenseAllocationFlowList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime RunDate { get; set; }
        public string SettingId { get; set; }
        public string Status { get; set; }
        public string JournalId { get; set; }
        public string JournalNumber { get; set; }



    }
}