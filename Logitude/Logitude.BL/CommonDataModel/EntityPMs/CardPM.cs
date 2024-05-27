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
    public class CardPM : ObjectCustomFieldPM
    {
        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EnglishName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }

        public string PartnerCode { get; set; }

        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string Swift { get; set; }
        public string AccountNumber { get; set; }
        public string IBANNumber { get; set; }
        public int Tenant { get; set; }
        public string GLAccountDisplayNumber { get; set; }
        public double? CreditLimitAmount { get; set; }
        public string Phone { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalName { get; set; }
        public string ComputedLocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentTermId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PartnerTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanUserId { get; set; }
        public string AccountManagerUserId { get; set; }
        public string TeamId { get; set; }
        public string SalesmanBusinessUnitId { get; set; }

        public string PartnerTypeName { get; set; }
        public string MainAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string PickupDeliveryAddressId { get; set; }
        public string Website { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string VatTypeId { get; set; }

        public string Prefix { get; set; }
        public string CityName { get; set; }
        public string ImageDetailId { get; set; }
        public bool DisconectFromContact { get; set; }
        public bool InternetAccess { get; set; }
        public string PrimaryContactId { get; set; }
        public bool IsCustomer { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string StateName { get; set; }
        public string RankId { get; set; }
        public string IndustryId { get; set; }
        public string LeadDescription { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public string LeadSourceId { get; set; }
        public string CustomerSizeId { get; set; }
        [Include]
        [Association("CardCustomAgent", "Id", "Id", IsForeignKey = true)]
        public virtual CustomAgentPM CustomAgent { get; set; }
        [Include]
        [Association("CardShippingAgent", "Id", "Id", IsForeignKey = true)]
        public virtual ShippingAgentPM ShippingAgent { get; set; }
        [Include]
        [Association("CardCustomer", "Id", "Id", IsForeignKey = true)]
        public virtual CustomerPM Customer { get; set; }

        [Include]
        [Association("CardAgent", "Id", "Id", IsForeignKey = true)]
        public virtual AgentPM Agent { get; set; }

        [Include]
        [Association("CardVendor", "Id", "Id", IsForeignKey = true)]
        public virtual VendorPM Vendor { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string SearchFields { get; set; }
        public bool FieldsChanged { get; set; }
        public string ContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SATForeignRFC { get; set; }

        // used for customer API
        private List<ContactPM> contacts;
        [Include]
        [Association("ContactsCards", "Id", "CardId")]
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
        //////////////////////////////////////

        private List<AddressPM> addresses;
        [Include]
        [Association("CardPMAddressPM", "Id", "CardId")]
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

        public DateTime? InvitationDate { get; set; }
        public DateTime? CargoTrackingInvitationDate { get; set; }
        public int? SharedLogisticsInvitationStatusCode { get; set; }
        public string SharedLogisticsInvitationStatusName { get; set; }
        public int? CargoTrackingInvitationStatusCode { get; set; }
        public string CargoTrackingInvitationStatusName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string CollectorId { get; set; }
        public string ClassifierId { get; set; }
        public string CollectorName { get; set; }
        public string ClassifierName { get; set; }
        public bool IsActiveForMobile { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SupportNotes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GLAccountId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalAccountingBusinessArea { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SATPaymentMethodCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalId2 { get; set; }

        public string MetodoPagoCode { get; set; }


        public string UsoCFDICode { get; set; }
        public string RegimenFiscalCode { get; set; }
        public bool IsInternationalPartner { get; set; }
        public bool IsAutonomy { get; set; }

        public string CustomerStatusCode { get; set; }


        [DataMember]
        public string CalculatedLocalName { get; set; }
        [DataMember]
        public string CalculatedEnglishName { get; set; }
        [DataMember]
        public bool IsDisconnectedFromGLAccount { get; set; }
        public string CreatedByPartner { get; set; }
        public int? StorageFreeDays { get; set; }
        [DataMember]
        public bool AccountingVATSplit { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [DataMember]
        public string BillToId { get; set; }
        public string ICAO { get; set; }
        public bool AllowUnassignedEntry { get; set; }
        public string SATCustomerName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImportLocalCustomerGroupId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExportLocalCustomerGroupId { get; set; }
        public bool IsPotential { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EORInumber { get; set; }
        public string SingleInvoiceTemplateId { get; set; }
        public string CustomsInvoiceTemplateId { get; set; }
        public string ConsolidationInvoiceTemplateId { get; set; }
        public string ManifestInvoiceTemplateId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string EmailForSendingSingArinvoice { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool SendingInterestReport { get; set; }
    }
}
