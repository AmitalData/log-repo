using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class VatTypePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }
       
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EnglishName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalDescription { get; set; }

        public string ComputedLocalName { get; set; }
        public bool AddedManually { get; set; }
        public string SearchFields { get; set; }
        
        public double? NewEntityPercentage { get; set; }
        public DateTime? NewEntityPercentageDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalVATCard { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalTAXItemId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalVATCardExternalId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalTAXItemIdExternalId { get; set; }

        public bool IsMultiPercentage { get; set; }

        List<VatTypePercentagePM> vatTypePercentages;
        [Include]
        [Association("VatTypeVatTypePercentage","Id","VatTypeId")]
        [Composition]
        public virtual List<VatTypePercentagePM> VatTypePercentages 
        {
            get 
            {
                if (vatTypePercentages == null)
                {
                    vatTypePercentages = new List<VatTypePercentagePM>();
                }

                return vatTypePercentages;
            }

            set { vatTypePercentages = value; }
        }

        List<VATTypesGroupPM> vatTypeGroups;
        [Include]
        [Association("VatTypeVatTypesGroup", "Id", "GroupVATTypeId")]
        [Composition]
        public virtual List<VATTypesGroupPM> VatTypeGroups
        {
            get
            {
                if (vatTypeGroups == null)
                {
                    vatTypeGroups = new List<VATTypesGroupPM>();
                }

                return vatTypeGroups;
            }

            set { vatTypeGroups = value; }
        }
    }
}