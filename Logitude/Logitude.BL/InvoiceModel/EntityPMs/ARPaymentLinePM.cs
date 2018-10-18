using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.Validators;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(ClassLevelValidator), "ValidateClass")]
    public class ARPaymentLinePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARPaymentId { get; set; }
        public string ChequeOrPaymentRef { get; set; }

        [CustomValidation(typeof(ValidationClass), "ValidateClass")]
        public double? Amount { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string Account { get; set; }        
    }
}