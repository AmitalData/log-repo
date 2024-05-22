using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ChargesTypePM : ObjectCustomFieldPM
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
        public string ChargesGroupCode { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesGroupId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteChargesGroupCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteChargesGroupId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MeasurementId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerMeasurementId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsReceivable { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsPayable { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAir { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsOcean { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsInland { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAutoDisplayInShipment { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAutoDisplayInConsolidation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAutoDisplayInQuote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DueTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IATACodeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AWBPrintDescription { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int ViewOrder { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AccountingVATSplit { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayableDebitAccount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivableCreditAccount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivablesChargesTypeExtCode { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayablesChargesTypeExtCode { get; set; }        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayableDebitAccountExternalId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteGroupSectionID { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivableCreditAccountExtId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeExternalCodeExtId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAutoDisplayInCustoms { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCustoms { get; set; }

        public string ComputedLocalName { get; set; } 
        public bool AddedManually { get; set; }
        public bool InActive { get; set; } 
        public string MeasurementCode { get; set; }
        public string MeasurementShortName { get; set; }       
        public string ContainerMeasurementCode { get; set; }        
        public string SearchFields { get; set; }
        public string ReceivableAccountId { get; set; }
        public string PayableAccountId { get; set; }
        public string SATExternalId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayableDebitGLAcountId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivableCreditGLAccountId { get; set; }
        public string RecCreditGLAcountLocalName { get; set; }
        public string PayDebitGLAcountLocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsExpense { get; set; }

        private List<ChargeTypeAccountingPM> chargeTypeAccountings;
        [Include]
        [Association("ChargeTypeAccountingPMChargeType", "Id", "ChargeTypeId")]
        [Composition]
        public virtual List<ChargeTypeAccountingPM> ChargeTypeAccountings
        {
            get
            {

                if (this.chargeTypeAccountings == null)
                {
                    chargeTypeAccountings = new List<ChargeTypeAccountingPM>();
                }

                return this.chargeTypeAccountings;
            }

            set
            {
                if (value != null)
                {
                    chargeTypeAccountings = value;
                }
            }
        }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsBackToBack { get; set; }

        public bool IsImport { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsExport { get; set; }
        public bool IsDrop { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivablesDefaultCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayablesDefaultCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayableDebitGLAcountLocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayableDebitGLAcountNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivableCreditGLAcountLocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivableCreditGLAcountNumber { get; set; }
        public bool ApplyRegionalTax { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool HasPickup { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool HasDelivery { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsDirectionRestricted { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsActiveInExport { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsActiveInImport { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsActiveInDomestic { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsActiveInDrop { get; set; }
    }
}
