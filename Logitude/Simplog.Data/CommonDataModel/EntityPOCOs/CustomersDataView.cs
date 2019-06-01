using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomersDataView
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string Code { get; set; }
        public string LocalName { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public string SupportNotes { get; set; }
        public string BillToId { get; set; }
        public string Website { get; set; }
        public string SalesmanUserId { get; set; }
        public string PaymentTermId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string VatNumber { get; set; }
        public string SearchFields { get; set; }
        public string PaymentTermEnglishName { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public DateTime? LastShipmentDate { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public bool StartWorkingManuallySet { get; set; }
        public string AccountManagerUserEnglishName { get; set; }
        public string SalesmanUserEnglishName { get; set; }
        public string CollectorName { get; set; }
        public string ClassifierName { get; set; }
        public string VatTypeId { get; set; }
        public string BillToName { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string RankCode { get; set; }
        public string RankName { get; set; }
        public int? SharedLogisticsInvitationStatusCode { get; set; }
        public string SharedLogisticsInvitationStatusName { get; set; }
        public bool IsActiveForMobile { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? InvitationDate { get; set; }
        public string IndustryName { get; set; }
        public string LeadDescription { get; set; }
        public string ClassifierId { get; set; }
        public bool IsCustomer { get; set; }
        public string CollectorId { get; set; }
        public string FreelancerId { get; set; }
        public string FreelancerName { get; set; }
        public string ForwarderId { get; set; }
        public string ForwarderName { get; set; }
        public string CustomsAgentId { get; set; }
        public string CustomsAgentName { get; set; }
        public string MediatorId { get; set; }
        public string MediatorName { get; set; }
        public string BeforeDeactiveStatusCode { get; set; }
        public DateTime? ReadyForActivationDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactId { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string CustomerStatusCode { get; set; }
        public string CustomerStatusName { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string AccountManagerUserId { get; set; }
        public string SalesmanBusinessUnitId { get; set; }
        public DateTime? FirstShipmentDate { get; set; }
        public DateTime? FirstInvoiceDate { get; set; }
        public DateTime? LastOpportunityDate { get; set; }
        public DateTime? LastMeetingDate { get; set; }
        public DateTime? LastCallDate { get; set; }
        public DateTime? LastQuoteDate { get; set; }
        public DateTime? LastInteractionDate { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public bool ActivityWatch { get; set; }
        public string RankId { get; set; }
        public string IndustryId { get; set; }
        public string LeadSourceId { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string KnownConsignor { get; set; }
        public DateTime? KCExpirationDate { get; set; }
        public string PartnerTypeId { get; set; }
        public string CustomerSizeId { get; set; }
        public string CustomerSizeName { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
        public double? CreditLimitAmount { get; set; }
        public double? CreditLimitOpenBalance { get; set; }
        public int? CreditLimitWarningPercentage { get; set; }
        public string ExternalAccountingBusinessArea { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public bool BlockNewInvoiceCreation { get; set; }
        public bool BlockNewShipmentCreation { get; set; }
        public string ExternalId2 { get; set; }
        public string SATForeignRFC { get; set; }
        public string MetodoPagoCode { get; set; }
        public string UsoCFDICode { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string CompetitorFields { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? InactiveDate { get; set; }
        public DateTime? ActivationRequestDate { get; set; }
        public string ActivatedByUserId { get; set; }
        public string SetAsInactiveByUserId { get; set; }
        public string ActivationRequestedByUserId { get; set; }
        public string ActivatedByUserName { get; set; }
        public string SetAsInactiveByName { get; set; }
        public string ActivationRequestedByUserName { get; set; }
        public string LeadSourceName { get; set; }
    }
}
