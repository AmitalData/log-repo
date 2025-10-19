using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ExpenseAllocationSettingList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal NumberOfPayments { get; set; }
        public int MonthInterval { get; set; }
        public string PaymentDateType { get; set; }

    }
}