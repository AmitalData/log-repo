using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
     [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CounterDefinitionPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string Parameter1 { get; set; }

        public string Parameter2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Prefix { get; set; }

        public bool UniquePerPrefix { get; set; }

        public string CounterId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int StartNumber { get; set; }
        public int StartNumber_Old { get; set; }

        public bool IsUsed { get; set; }
		public int? CounterSize { get; set; }
		public string Suffix { get; set; }
        public bool InActive { get; set; }
        public bool UsePerBranch { get; set; }
        public bool IsCustomized { get; set; }
        public bool IsAdded { get; set; }
    }
}
