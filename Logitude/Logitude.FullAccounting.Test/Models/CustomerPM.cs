using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{

    public class CustomerPM
    {

        public string Id { get; set; }

        public int Tenant { get; set; }

        public bool IsSecured { get; set; }


        public string BankName { get; set; }


        public string CompetitorFields { get; set; }


        public string BankAddress { get; set; }


        public string Swift { get; set; }


        public string AccountNumber { get; set; }


        public string IBANNumber { get; set; }


        public DateTime? StartWorkingDate { get; set; }


        public string Notes { get; set; }//card


        public string SupportNotes { get; set; }//card


        public string Code { get; set; }//card


        public string EnglishName { get; set; }//card


        public string LocalName { get; set; }//card


        public string AccountManagerUserId { get; set; }


        public string SalesmanUserId { get; set; }


        public string SalesmanUserEnglishName { get; set; }


        public string SalesmanBusinessUnitId { get; set; }


        public string Website { get; set; }


        public bool InActive { get; set; }//card


        public string BillToId { get; set; }


        public string BillToName { get; set; }


        public string VatNumber { get; set; }//card


        public string ReceivablesAccountingCard { get; set; }//card


        public string EORInumber { get; set; }


        public string PayablesAccountingCard { get; set; }//card


        public string InvoiceCurrencyId { get; set; }


        public string PaymentTermId { get; set; }//card


        public string RankId { get; set; }


        public string VatTypeId { get; set; }


        public string ComputedLocalName { get; set; }


        public bool FieldsChanged { get; set; }


        public string PartnerTypeId { get; set; }//card


        public DateTime? CreateDate { get; set; }


        public DateTime? UpdateDate { get; set; }


        public string CreatedByUserId { get; set; }


        public string UpdatedByUserId { get; set; }


        public string SearchFields { get; set; }


        public string CardPMId { get; set; }


        public string ExistedContactId { get; set; }


        public bool StartWorkingManuallySet { get; set; }


        public string CityName { get; set; }

        public string CityCode { get; set; }


        public string CountryId { get; set; }


        public string CountryCode { get; set; }


        public string CountryName { get; set; }


        public string ATTN { get; set; }


        public string AccountManagerUserEnglishName { get; set; }


        public string RankName { get; set; }


        public string RankCode { get; set; }


        public string PhoneNumber { get; set; }// main address phone


        public string FaxNumber { get; set; }//main address fax


        public string CityWithCountry { get; set; }


        public string ImageDetailId { get; set; }




        public string SharedLogisticsInvitationStatusName { get; set; }


        public bool IsActiveForMobile { get; set; }


        public DateTime? LastLoginDate { get; set; }


        public DateTime? InvitationDate { get; set; }


        public bool IsHybrid { get; set; }


        public string FreelancerId { get; set; }


        public string ForwarderId { get; set; }


        public string CustomsAgentId { get; set; }


        public string MediatorId { get; set; }


        public string FreelancerName { get; set; }


        public string ForwarderName { get; set; }


        public string CustomsAgentName { get; set; }


        public string MediatorName { get; set; }


        public string GoogleAddressString { get; set; }


        public bool IsExternal { get; set; }


        public bool IsCustomer { get; set; }


        public string CustomerStatusCode { get; set; }


        public string BeforeDeactiveStatusCode { get; set; }


        public DateTime? ActivationDate { get; set; }


        public DateTime? InactiveDate { get; set; }


        public DateTime? ActivationRequestDate { get; set; }


        public string ActivatedByUserId { get; set; }


        public string SetAsInactiveByUserId { get; set; }


        public string ActivationRequestedByUserId { get; set; }


        public bool SetReActivated { get; set; }


        public bool SetInActive { get; set; }


        public bool SetReady { get; set; }


        public bool SetActivated { get; set; }


        public string EventNote { get; set; }

        //------------Address Fields------------


        public string Address1_Potential { get; set; }


        public string Address2_Potential { get; set; }


        public string City_Potential { get; set; }


        public string CountryId_Potential { get; set; }


        public string ZipCode_Potential { get; set; }


        public string StateId_Potential { get; set; }


        public string PhoneNumber_Potential { get; set; }


        public string FaxNumber_Potential { get; set; }


        public string ATTN_Potential { get; set; }


        public bool IsLocalLanguage { get; set; }


        public string CodeMyCustomer { get; set; }


        public string PrimaryContactName { get; set; }


        public string PrimaryContactEmail { get; set; }


        public string PrimaryContactPhone { get; set; }

        public string EmailForSendingSingArinvoice { get; set; }


        public string CustomerStatusName { get; set; }


        public string PrimaryContactId { get; set; }


        public string Phone { get; set; }


        public DateTime? ReadyForActivationDate { get; set; }


        public bool IsFirstContactToAdd { get; set; }


        public bool SavedForActivation { get; set; }


        public Guid QueueMessageLockToken { get; set; }

        public string QuestionnaireAnsewrsHtmlString { get; set; }


        public bool SetAsPotential { get; set; }


        public string UpdatedByUserCode { get; set; }


        public string IRSPlace { get; set; }


        public string IRSNumber { get; set; }


        public string RequestedAirlines { get; set; }

        public string RegisteredAirlines { get; set; }

        public string PendingAirlines { get; set; }


        public bool IsCustomerAllowed { get; set; }



        public CardPM Card { get; set; }




        public string IndustryId { get; set; }


        public string LeadSourceId { get; set; }


        public string CollectorId { get; set; }


        public string ClassifierId { get; set; }


        public string IndustryName { get; set; }


        public string LeadSourceName { get; set; }


        public string CollectorName { get; set; }


        public string ClassifierName { get; set; }


        public double? CreditLimit { get; set; }


        public string LeadDescription { get; set; }


        public string RegionId { get; set; }


        public string RegionName { get; set; }


        public string CustomerSizeId { get; set; }


        public DateTime? LastCallDate { get; set; }


        public DateTime? LastMeetingDate { get; set; }


        public DateTime? LastOpportunityDate { get; set; }


        public string LastOpportunitySubject { get; set; }


        public string LastOpportunityStatus { get; set; }



        public DateTime? FirstInvoiceDate { get; set; }


        public DateTime? FirstShipmentDate { get; set; }


        public DateTime? LastShipmentDate { get; set; }


        public DateTime? LastQuoteDate { get; set; }


        public DateTime? LastInteractionDate { get; set; }


        public bool EnableConsolidationInvoices { get; set; }


        public bool ActivityWatch { get; set; }


        public string KnownConsignor { get; set; }


        public DateTime? KCExpirationDate { get; set; }


        public bool LogBoxActivated { get; set; }








        public bool IsLogBox { get; set; }


        public bool IsPrivateLabelCustomer { get; set; }


        public bool IsCreditLimitEnabled { get; set; }


        public double? CreditLimitAmount { get; set; }


        public double? InsuredcreditLimit { get; set; }


        public double? CreditLimitOpenBalance { get; set; }


        public int? CreditLimitWarningPercentage { get; set; }


        public string ExternalAccountingBusinessArea { get; set; }


        public string PaymentMethodCode { get; set; }


        public bool BlockNewInvoiceCreation { get; set; }


        public bool BlockNewShipmentCreation { get; set; }


        public string ExternalId2 { get; set; }




        public string SATForeignRFC { get; set; }


        public string MetodoPagoCode { get; set; }


        public string UsoCFDICode { get; set; }


        public int CustomerTenant { get; set; }

        public string MainAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string GLAccountId { get; set; }
        public string CreatedByPartner { get; set; }


        public int? StorageFreeDays { get; set; }



        public bool AccountingVATSplit { get; set; }


        public string UploadingUniqueKey { get; set; }


        public string GLAccountNumber { get; set; }


        public bool IsAutonomy { get; set; }

    }
}
