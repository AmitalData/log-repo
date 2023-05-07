using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
	[CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	[DataContract]
	public partial class EventRemarkPM
		{
		[Key]
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public int Tenant { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public DateTime CreateDate { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public string CreatedByUserId { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public string SearchFields { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public string EventTypeId { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public string PartnerTypeId { get; set; }
		[DataMember]
		[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
		public bool IsChoose { get; set; }
	}

}
