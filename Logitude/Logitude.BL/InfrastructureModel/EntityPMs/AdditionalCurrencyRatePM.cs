using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class AdditionalCurrencyRatePM 
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
       public DateTime CreateDate { get; set; }	  	  
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId { get; set; }
	  	         
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate { get; set; }	  	  
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId { get; set; }
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public string SearchFields { get; set; }	  	  
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public string Name { get; set; }
       
	   [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	   [DataMember]
       public double? RateCoefficient { get; set; }
	}   
}
	 