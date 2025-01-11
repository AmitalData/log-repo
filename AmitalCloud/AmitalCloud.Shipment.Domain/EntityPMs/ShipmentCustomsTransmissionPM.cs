using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentCustomsTransmissionPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? LastSendDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string SentByUserId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CommunicationLogId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Error { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string MessageCode { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Status { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string StatusName { get; set; }


        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ByUserName { get; set; }
    }
}
