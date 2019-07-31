using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.Validators;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(PackageTypesValidator), "IsPackageTypeValid")]
    [DataContract]
    public class PackageTypePM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EnglishName { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalName { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsOcean { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAir { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsInland { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsContainer { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double TEU { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int ContainerSize { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal Volume { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrintAs { get; set; }
        [DataMember]
        public string ComputedLocalName { get; set; }
        [DataMember]
        public bool AddedManually { get; set; }
        [DataMember]
        public bool InActive { get; set; }
        [DataMember]
        public string MeasurementId { get; set; }
        [DataMember]
        public string MeasurementCode { get; set; }
        [DataMember]
        public string MeasurementShortName { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public bool IsHybrid { get; set; }

        [DataMember]
        public bool IsRefrigerated { get; set; }

        [DataMember]
        public bool IsVehicle { get; set; }

    }
}
