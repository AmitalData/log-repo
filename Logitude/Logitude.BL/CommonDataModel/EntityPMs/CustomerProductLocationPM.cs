using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CustomerProductLocationPM
    {
        [Key]
        public string CustomerId { get; set; }
        
        [Key]
        public string ProductTypeCode { get; set; }
        
        [Key]
        public string CountryId { get; set; }

        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? PotentialTEU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? PotentialNumberOfShipments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? PotentialChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? CommitmentTEU { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? CommitmentNumberOfShipments { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? CommitmentChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? PotentialRevenue { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? CommitmentRevenue { get; set; }

        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public Simplog.Server.Infrastructure.ChangeSetOperation ChangeSetOp { get; set; }
    }
}