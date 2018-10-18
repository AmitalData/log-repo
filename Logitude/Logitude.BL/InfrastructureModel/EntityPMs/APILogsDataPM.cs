using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;
namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class APILogsDataPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DiagnosticLog { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RequestData { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ResponseData { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExceptionsMessage { get; set; }
    }
}
