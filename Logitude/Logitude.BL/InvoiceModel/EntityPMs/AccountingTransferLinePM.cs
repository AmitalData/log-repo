using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class AccountingTransferLinePM
    {
        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EntityId { get; set; }
                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EntityReference { get; set; }

        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingTransferHeaderId { get; set; }

        public string SearchFields { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        public DateTime? InvoiceDate { get; set; }
        public string BillToName { get; set; }
        public string StatusName { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public string InvoiceCurrencyCode { get; set; }
    }
}