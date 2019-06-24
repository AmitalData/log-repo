using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class AirlineAreasPortPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AirlineAreaId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PortId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AddedDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AddedByUserId { get; set; }



    }
}
