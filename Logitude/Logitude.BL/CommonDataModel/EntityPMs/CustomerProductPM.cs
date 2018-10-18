using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CustomerProductPM
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string ProductTypeCode { get; set; }

        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? PotentialChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? CommitmentChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? PotentialTEU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? CommitmentTEU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? PotentialNumberOfShipments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? CommitmentNumberOfShipments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProductTypeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? PotentialRevenue { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? CommitmentRevenue { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastShipmentDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrepaidCollectId { get; set; }

        public string PrepaidCollectName{ get; set; }

        private List<CustomerProductLocationPM> productLocations;
        [Include]
        [Association("CustomerProductCustomerProductLocation", "CustomerId,ProductTypeCode", "CustomerId,ProductTypeCode")]
        [Composition]
        public virtual List<CustomerProductLocationPM> ProductLocations
        {
            get
            {
                if (this.productLocations == null)
                {
                    productLocations = new List<CustomerProductLocationPM>();
                }

                return this.productLocations;
            }

            set
            {
                if (value != null)
                {
                    productLocations = value;
                }
            }
        }

        public ChangeSetOperation ChangeSetOp { get; set; }
        public List<CustomerProductLocationPM> LocationsChangeSet { get; set; }

        public bool NotesRightToLeft { get; set; }
    }
}