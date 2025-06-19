using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CurrencyRatePM
   {
       [Key]
       [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
       [DataMember]
       public string Id { get; set; }
        
       [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public int Tenant { get; set; }

	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public string ExchangeRateId { get; set; }	  	  
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public string AdditionalCurrencyRateId { get; set; }
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public double Rate { get; set; }
	}   
}
	 