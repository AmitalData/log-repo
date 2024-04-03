using System;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.InfrastructureModel.EntityLists;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AgentList : CustomFieldList
    { 
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string VatNumber { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }
        public string PaymentTermEnglishName { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public string Notes { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string VatTypeId { get; set; }
        public string Website { get; set; }
        public string PaymentTermId { get; set; }
        public string SearchFields { get; set; }
        public string SharedLogisticsInvitationStatusName { get; set; }
        public string CargoTrackingInvitationStatusName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CASSCode { get; set; }
        public string IATACode { get; set; }
        public string RegulatedAgentCode { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string ExternalAccountingBusinessArea { get; set; }
        public string PaymentMethodCode { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
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
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string StateName { get; set; }
        public int? StorageFreeDays { get; set; }
        public string GLAccountNumber { get; set; }
        public string RegimenFiscalCode { get; set; }
        public string SATReceptorName { get; set; }
    }
}