using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ComputingPartnerPM
    {
        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Remarks { get; set; }

        public string SearchFields { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string Code { get; set; }
        public int Tenant { get; set; }
        public int LoggedTenantId { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }
        private List<ComputingPartnerTablePM> partnerTables;
        [Include]
        [Association("ComputingPartnerTableComputingPartner", "Id", "ComputingPartnerId")]
        [Composition]
        public virtual List<ComputingPartnerTablePM> PartnerTables
        {
            get
            {

                if (this.partnerTables == null)
                {
                    partnerTables = new List<ComputingPartnerTablePM>();
                }

                return this.partnerTables;
            }

            set
            {
                if (value != null)
                {
                    partnerTables = value;
                }
            }
        }
    }
}