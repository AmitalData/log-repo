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
    public class APILogsPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Direction { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Status { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime CreateDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime CreateDateUTC { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime LastUpdateDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime LastUpdateDateUTC { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int NumberOfRetries { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime ExpirationDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Subject { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EntityId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ObjectTableId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PartnerName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Refrence { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastExceptionMessage { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CorrelationId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DiagnosticLog { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RequestData { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ResponseData { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExceptionsMessage { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string StatusName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BatchNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ObjectTableName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QueueMessageMoreDetailsId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QueueType { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QueueMessage { get; set; }
        
    }
}
