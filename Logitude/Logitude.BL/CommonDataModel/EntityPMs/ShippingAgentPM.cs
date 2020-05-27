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
    public class ShippingAgentPM
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
        public string AccountNumber { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IBANNumber { get; set; }
          
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Code { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string EnglishName { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string LocalName { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Website { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InActive { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Notes { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ForwarderAccountNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ForwarderCreditNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string VatNumber { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ReceivablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PayablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string InvoiceCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PaymentTermId { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string VatTypeId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedLocalName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool FieldsChanged { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PartnerTypeId { get; set; }//card

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }
    
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CardPMId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExistedContactId { get; set; }

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
        public bool IsHybrid { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsExternal { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrimaryContactId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsFirstContactToAdd { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string LocalCustomsCode { get; set; }

        [DataMember]
        public bool EnableConsolidationInvoices { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IRSPlace { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IRSNumber { get; set; }

        [DataMember]
        public string PrimaryContactName { get; set; }
        [DataMember]
        public string PrimaryContactEmail { get; set; }
        [DataMember]
        public string PrimaryContactPhone { get; set; }

        [Include]
        [Association("ShippingAgentCardPM", "Id", "Id", IsForeignKey = true)]
        public CardPM Card { get; set; }

        private List<AddressPM> addresses;
        [Include]
        [Association("ShippingAgentPMAddressPM", "Id", "CardId")]
        public virtual List<AddressPM> Addresses
        {
            get
            {
                if (addresses == null)
                {
                    addresses = new List<AddressPM>();
                }
                return addresses;
            }
            set { addresses = value; }
        }

        private List<ContactPM> contacts;
        [Include]
        [Association("ShippingAgentPMContactPM", "Id", "CardId")]
        public virtual List<ContactPM> Contacts
        {
            get
            {
                if (contacts == null)
                {
                    contacts = new List<ContactPM>();
                }
                return contacts;
            }
            set { contacts = value; }
        }


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
        public string GLAccountId { get; set; }
    }
}