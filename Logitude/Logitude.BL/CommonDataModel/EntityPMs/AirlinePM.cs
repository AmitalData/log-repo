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
    [DataContract]
    public class AirlinePM : ObjectCustomFieldDataContractPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [DataMember]
        public bool IsSecured { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankAddress { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Swift { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankAccountNumber { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IBANNumber { get; set; }

        [DataMember]
        public bool IsHybrid { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public string CreatedByUserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string EnglishName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string LocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Website { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Notes { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string AWBAccount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string VatNumber { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivablesAccountingCard { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string InvoiceCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PaymentTermId { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool AddedManually { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Remark { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Prefix { get; set; }   

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string CarrierTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string VatTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool CheckDigit { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool LimitedLength { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedLocalName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportModeId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CityName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryName { get; set; }

        [DataMember]
        public string SearchFields { get; set; }    
   
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool FieldsChanged { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TTY { get; set; }

        [DataMember]
        public bool ChampNeedsRegistration { get; set; }

        [DataMember]
        public bool IsChampRegistered { get; set; }

        [DataMember]
        public bool IsExternal { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrimaryContactId { get; set; }

        [DataMember]
        public bool IsFirstContactToAdd { get; set; }

        [DataMember]
        public bool EnableConsolidationInvoices { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountNumber { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GLSHKPIMA { get; set; }

        [DataMember]
        public bool IsGLSHKRegistered { get; set; }

        [DataMember]
        public bool GLSHKNeedsRegistration { get; set; }

        [DataMember]
        public bool ChampFWB { get; set; }

        [DataMember]
        public bool ChampFHL { get; set; }

        [DataMember]
        public bool ChampFSU { get; set; }

        [DataMember]
        public bool ChampFSRFSA { get; set; }

        [DataMember]
        public bool ChampFVRFVA { get; set; }

        [DataMember]
        public bool ChampFFRFFA { get; set; }

        [DataMember]
        public bool GLSHKFWB { get; set; }

        [DataMember]
        public bool GLSHKFHL { get; set; }

        [DataMember]
        public bool GLSHKFSU { get; set; }

        [DataMember]
        public bool GLSHKFSRFSA { get; set; }

        [DataMember]
        public bool GLSHKFVRFVA { get; set; }

        [DataMember]
        public bool GLSHKFFRFFA { get; set; }

        [DataMember]
        public bool IsAllowedInAirlinesRestriction { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RegistrationNotes { get; set; }

        [DataMember]
        public bool ChampRegistrationRequested { get; set; }

        [DataMember]
        public bool GLSHKRegistrationRequested { get; set; }

        [DataMember]
        public bool HasAdaptations { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ICAO { get; set; }

        [DataMember]
        public string RegistrationUpdatedBy { get; set; }

        [DataMember]
        public bool IsManagingProduct { get; set; }

        [DataMember]
        public bool IsProductMandatory { get; set; }

        [DataMember]
        public bool IsDescriptionOfGoodsFromList { get; set; }

        [DataMember]
        public int? ScheduleDays { get; set; }

        [DataMember]
        public bool NoAvailabilityInFVAMessages { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IRSPlace { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IRSNumber { get; set; }

        [DataMember]
        public bool IsDeclined { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeclineNotes { get; set; }

        [DataMember]
        public string PrimaryContactName { get; set; }
        [DataMember]
        public string PrimaryContactEmail { get; set; }
        [DataMember]
        public string PrimaryContactPhone { get; set; }

        [Include]
        [Association("AirlineCarrier", "Id", "Id")]
        [DataMember]
        public CardPM Card { get; set; }        

        private List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencies;
        [Include]
        [Association("CardExternalCodeByCurrencyPM", "Id", "CardId")]
        [Composition]
        [DataMember]
        public virtual List<CardExternalCodeByCurrencyPM> CardExternalCodeByCurrencies
        {
            get
            {
                if (cardExternalCodeByCurrencies == null)
                {
                    cardExternalCodeByCurrencies = new List<CardExternalCodeByCurrencyPM>();
                }

                return cardExternalCodeByCurrencies;
            }

            set
            {
                cardExternalCodeByCurrencies = value;
            }
        }
                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ExternalAccountingBusinessArea { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PaymentMethodCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalId2 { get; set; }

        [DataMember]
        public string SATForeignRFC { get; set; }

        [DataMember]
        public string MetodoPagoCode { get; set; }

        [DataMember]
        public string UsoCFDICode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImageDetailId { get; set; }

        [DataMember]
        public string GLAccountId { get; set; }

        [DataMember]
        public bool AccountingVATSplit { get; set; }

        [DataMember]
        public string UploadingUniqueKey { get; set; }

        [DataMember]
        public string GLAccountNumber { get; set; }

        [DataMember]
        public string BillToId { get; set; }

        [DataMember]
        public string RegimenFiscalCode { get; set; }

        [DataMember]
        public string SATReceptorName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImportLocalCustomerGroupId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExportLocalCustomerGroupId { get; set; }
    }
}
