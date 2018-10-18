using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class BankAccountLitePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime CreateDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime UpdateDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EnglishName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IBAN { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SwiftCode { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankCode { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrencyId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrencyCode{ get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchAddress { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool Inactive { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatNumber { get; set; }
    }
}
