using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
         
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class RatesTablePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ForeignCurrencyId { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public string ForeignCurrencyName { get; set; }
        public string BaseCurrencyId { get; set; }
        public DateTime LogDateTime { get; set; }
                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Rate { get; set; }

                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ValueDate { get; set; }
    }
}