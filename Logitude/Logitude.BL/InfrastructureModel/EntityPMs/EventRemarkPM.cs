using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
	[CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
	[DataContract]
	public partial class EventRemarkPM
		{
			public string Id { get; set; }

			private int Tenant { get; set; }

			private DateTime CreateDate { get; set; }

			private string CreatedByUserId { get; set; }

			private DateTime UpdateDate { get; set; }

			private string UpdatedByUserId { get; set; }

			private string SearchFields { get; set; }

			private string EventTypeId { get; set; }

			private string PartnerTypeId { get; set; }

			private bool IsChoose { get; set; }
	}

}
